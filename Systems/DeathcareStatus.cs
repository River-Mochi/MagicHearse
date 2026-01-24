// File: Systems/DeathcareStatus.cs
// Purpose: UI-facing cached Status snapshot strings for Options UI.
// Notes:
// - Refresh driven by OptionsUI getters (only when Options is open, not city).
// - Cache invalidates on main-menu <-> city transitions (prevents stale on city switching).
// - Localization + formatting safety is handled here; DeathcareStatusSystem returns raw numbers only.

namespace MagicHearse
{
    using Game;                   // GameMode extension: IsGame()
    using Game.SceneFlow;         // GameManager
    using System;                 // DateTime, TimeSpan, Math, Exception, FormatException
    using Unity.Entities;         // World
    using UnityEngine;            // Time.frameCount

    public static class DeathcareStatus
    {
        // Throttle refresh while Options UI is open.
        // NOTE: Do not set to 0 (would refresh every UI poll).
        public static int RefreshIntervalSeconds { get; set; } = 15;

        // Locale keys (add to all Locale*.cs)
        internal const string KeyStatusNotLoaded = "MH_STATUS_NOT_LOADED";
        internal const string KeyNoCityLoaded = "MH_STATUS_NO_CITY_LOADED";
        internal const string KeyStatsNotAvail = "MH_STATUS_STATS_NOT_AVAIL";
        internal const string KeyLine1 = "MH_STATUS_LINE1";
        internal const string KeyLine2 = "MH_STATUS_LINE2";
        internal const string KeyLine3 = "MH_STATUS_LINE3";

        // English fallbacks (placeholders must match BuildAndApplySnapshot arg lists)
        private const string FallbackStatusNotLoaded = "Status not loaded.";
        private const string FallbackNoCityLoaded   = "No city loaded.";
        private const string FallbackStatsNotAvail  = "No city... ¯\\_(ツ)_/¯ ...No stats ";
        private const string FallbackLine1 = "{0} dead waiting | updated {1}";
        private const string FallbackLine2 = "{0} deaths/mo | {1} cremation max/mo | {2} / {3} cemetery use";
        private const string FallbackLine3 = "{0} hearses | {1} / {2} buildings | {3} empty graves | {4} max workers";

        // Public UI strings read by Setting.cs getters
        public static string SummaryLine1 { get; private set; } = string.Empty;
        public static string SummaryLine2 { get; private set; } = string.Empty;
        public static string SummaryLine3 { get; private set; } = string.Empty;

        // Cache state
        private static bool s_WasInGame;
        private static bool s_HasSnapshotThisCity;
        private static long s_LastRefreshTicksUtc;
        private static int s_LastUiFrame = -1;

        /// <summary>Clears snapshot so next getter refreshes (prevents stale data after city switches).</summary>
        public static void InvalidateCache()
        {
            s_HasSnapshotThisCity = false;
            s_LastRefreshTicksUtc = 0;
            s_LastUiFrame = -1;

            SummaryLine1 = Localize(KeyStatusNotLoaded, FallbackStatusNotLoaded);
            SummaryLine2 = string.Empty;
            SummaryLine3 = string.Empty;
        }

        /// <summary>Marks snapshot stale so next getter refreshes. Current text stays until refresh.</summary>
        public static void MarkDirty()
        {
            s_HasSnapshotThisCity = false;
            s_LastRefreshTicksUtc = 0;
        }

        // Called by Setting.cs getters.
        public static void RefreshIfNeeded()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            // Frame guard: Setting.cs calls this 3 times per UI draw (3 getters).
            int frame = Time.frameCount;
            if (frame == s_LastUiFrame)
            {
                return;
            }

            s_LastUiFrame = frame;

            if (string.IsNullOrEmpty(SummaryLine1))
            {
                SummaryLine1 = Localize(KeyStatusNotLoaded, FallbackStatusNotLoaded);
            }

            GameManager gm = GameManager.instance;
            bool isGame = (gm != null && gm.gameMode.IsGame());

            // Detect transitions (menu <-> city, city switches).
            if (isGame != s_WasInGame)
            {
                s_WasInGame = isGame;
                InvalidateCache();
            }

            // If no city loaded, re-localize these two lines when UI refreshes
            if (!isGame)
            {
                SummaryLine1 = Localize(KeyNoCityLoaded, FallbackNoCityLoaded);
                SummaryLine2 = Localize(KeyStatsNotAvail, FallbackStatsNotAvail);
                SummaryLine3 = string.Empty;
                return;
            }

            long nowUtc = DateTime.UtcNow.Ticks;

            // First snapshot after load OR dirty => refresh immediately.
            if (!s_HasSnapshotThisCity)
            {
                BuildSnapshotSafe(world);
                s_HasSnapshotThisCity = true;
                s_LastRefreshTicksUtc = nowUtc;
                return;
            }

            // Throttle refresh while Options UI is open.
            int interval = RefreshIntervalSeconds;
            if (interval < 1)
            {
                interval = 15;
            }

            long nextAllowed = s_LastRefreshTicksUtc + TimeSpan.FromSeconds(interval).Ticks;
            if (nowUtc < nextAllowed)
            {
                return;
            }

            BuildSnapshotSafe(world);
            s_LastRefreshTicksUtc = nowUtc;
        }

        private static void BuildSnapshotSafe(World world)
        {
            try
            {
                BuildAndApplySnapshot(world);
            }
            catch (Exception ex)
            {
                SummaryLine1 = Localize(KeyStatusNotLoaded, FallbackStatusNotLoaded);
                SummaryLine2 = string.Empty;
                SummaryLine3 = string.Empty;

                Mod.WarnOnce("MH_STATUS_SNAPSHOT_EXCEPTION", () =>
                    $"[MH] Status snapshot failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void BuildAndApplySnapshot(World world)
        {
            DeathcareStatusSystem sys = world.GetOrCreateSystemManaged<DeathcareStatusSystem>();
            DeathcareStatusSystem.Snapshot snap = sys.BuildSnapshot();

            string refreshedTime = snap.SnapshotTimeLocal.ToString("HH:mm:ss");

            long emptyGraves = snap.CemeteryCapacity - snap.CemeteryUse;
            if (emptyGraves < 0) emptyGraves = 0;

            SummaryLine1 = SafeFormat(
                KeyLine1,
                fallbackFormat: FallbackLine1,
                Format0(snap.DeadWaiting),  // {0}
                refreshedTime);             // {1}

            SummaryLine2 = SafeFormat(
                KeyLine2,
                fallbackFormat: FallbackLine2,
                Format0(snap.DeathsPerMonth),       // {0}
                Format0(snap.ProcessingRate),       // {1} <- game’s “handling capacity/mo”
                Format0(snap.CemeteryUse),          // {2}
                Format0(snap.CemeteryCapacity));    // {3}       

            SummaryLine3 = SafeFormat(
                KeyLine3,
                fallbackFormat: FallbackLine3,
                Format0(snap.Hearses),      // {0}
                snap.ActiveFacilities,      // {1}
                snap.TotalFacilities,       // {2}
                Format0(emptyGraves),       // {3}
                Format0(snap.MaxWorkers));  // {4}
        }

        // ---- helpers (kept minimal, but safe) ----

        private static string Localize(string entryId, string fallback)
        {
            var dict = GameManager.instance?.localizationManager?.activeDictionary;
            if (dict != null && dict.TryGetValue(entryId, out string value) && !string.IsNullOrEmpty(value))
            {
                return value;
            }

            return fallback;
        }

        private static string SafeFormat(string key, string fallbackFormat, params object[] args)
        {
            string format = Localize(key, fallbackFormat);

            try
            {
                return string.Format(format, args);
            }
            catch (FormatException)
            {
                Mod.WarnOnce("MH_STATUS_BAD_FORMAT_" + key, () =>
                    $"[MH] Status format error. Key={key} Args={args.Length}");

                // Try English fallback format.
                try { return string.Format(fallbackFormat, args); }
                catch { return fallbackFormat; }
            }
            catch
            {
                return fallbackFormat;
            }
        }

        private static string Format0(float v) => ((long)Math.Round(v)).ToString("N0");
        private static string Format0(long v) => v.ToString("N0");
    }
}
