// File: Systems/DeathcareStatus.cs
// Purpose: UI-facing cached Status snapshot strings for Options UI.
// Notes:
// - Performance: refresh driven by OptionsUI getters (only when Options is open, not city).
// - Uses explicit state (bool/ticks), not string comparisons.
// - Cache is invalidated on main-menu <-> city transitions (prevent stale when city switching).
// - Localization applied here; DeathcareStatusSystem returns raw numbers only.

namespace MagicHearse
{
    using Game;                   // GameMode extension: IsGame()
    using Game.SceneFlow;         // GameManager
    using System;                 // DateTime, TimeSpan, Math
    using Unity.Entities;         // World
    using UnityEngine;            // Time.frameCount

    public static class DeathcareStatus
    {
        // -----------------------------------------------------------------
        // Locale keys (add to all Locale*.cs)
        // -----------------------------------------------------------------

        internal const string KeyStatusNotLoaded = "MH_STATUS_NOT_LOADED";
        internal const string KeyNoCityLoaded = "MH_STATUS_NO_CITY_LOADED";
        internal const string KeyLine1 = "MH_STATUS_LINE1";
        internal const string KeyLine2 = "MH_STATUS_LINE2";
        internal const string KeyLine3 = "MH_STATUS_LINE3";

        // -----------------------------------------------------------------
        // English fallbacks (player-facing)
        // -----------------------------------------------------------------

        private const string FallbackStatusNotLoaded = "Status not loaded.";
        private const string FallbackNoCityLoaded = "No city loaded yet.";

        private const string FallbackLine1 = "{0} dead waiting | {1} updated";
        private const string FallbackLine2 = "{0} deaths/month | {1} can be handled";
        private const string FallbackLine3 = "{0} hearses | {1} / {2} buildings | {3} / {4} cemetery use | {5} max workers";

        // -----------------------------------------------------------------
        // Public UI strings used by Setting.cs getters
        // -----------------------------------------------------------------

        public static string SummaryLine1 { get; private set; } = string.Empty;
        public static string SummaryLine2 { get; private set; } = string.Empty;
        public static string SummaryLine3 { get; private set; } = string.Empty;

        // Throttle refresh while the Status group is visible in Options UI.
        public static int RefreshIntervalSeconds { get; set; } = 15;

        // -----------------------------------------------------------------
        // Cache state
        // -----------------------------------------------------------------

        private static bool s_WasInGame;
        private static bool s_HasSnapshotThisCity;
        private static bool s_ShowNoCityLoadedOnce;
        private static long s_LastRefreshTicksUtc;
        private static int s_LastUiFrame = -1;

        /// <summary>
        /// Clears cached snapshot so the next getter refreshes (prevents stale data after city switches).
        /// Safe to call from anywhere.
        /// </summary>
        public static void InvalidateCache()
        {
            s_HasSnapshotThisCity = false;
            s_ShowNoCityLoadedOnce = false;
            s_LastRefreshTicksUtc = 0;
            s_LastUiFrame = -1;

            SummaryLine1 = L(KeyStatusNotLoaded, FallbackStatusNotLoaded);
            SummaryLine2 = string.Empty;
            SummaryLine3 = string.Empty;
        }

        /// <summary>
        /// Marks the snapshot stale so the next getter refreshes.
        /// Current text stays until refresh (prevents text flicker).
        /// </summary>
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

            // Frame guard: Setting.cs may call this 3x per UI draw.
            int frame = Time.frameCount;
            if (frame == s_LastUiFrame)
            {
                return;
            }

            s_LastUiFrame = frame;

            // Placeholder for early UI reads (keeps the field non-empty).
            if (string.IsNullOrEmpty(SummaryLine1))
            {
                SummaryLine1 = L(KeyStatusNotLoaded, FallbackStatusNotLoaded);
            }

            GameManager gm = GameManager.instance;
            bool isGame = (gm != null && gm.gameMode.IsGame());

            // Detect transitions (main menu -> city, city -> main menu, switching saves, etc.).
            if (isGame != s_WasInGame)
            {
                s_WasInGame = isGame;
                InvalidateCache();
            }

            if (!isGame)
            {
                // Menu: print "no city loaded" once; do not tick.
                if (!s_ShowNoCityLoadedOnce)
                {
                    s_ShowNoCityLoadedOnce = true;
                    SummaryLine1 = L(KeyNoCityLoaded, FallbackNoCityLoaded);
                    SummaryLine2 = string.Empty;
                    SummaryLine3 = string.Empty;
                }

                return;
            }

            // First snapshot after a city loads: refresh immediately.
            if (!s_HasSnapshotThisCity)
            {
                BuildAndApplySnapshot(world);
                s_HasSnapshotThisCity = true;
                s_LastRefreshTicksUtc = DateTime.UtcNow.Ticks;
                return;
            }

            // Throttle refresh while Options UI is open.
            long nowTicksUtc = DateTime.UtcNow.Ticks;
            long minNext = s_LastRefreshTicksUtc + TimeSpan.FromSeconds(RefreshIntervalSeconds).Ticks;
            if (nowTicksUtc < minNext)
            {
                return;
            }

            BuildAndApplySnapshot(world);
            s_LastRefreshTicksUtc = nowTicksUtc;
        }

        // Optional manual refresh hook (future button).
        public static void ForceRefreshNow()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            BuildAndApplySnapshot(world);
            s_HasSnapshotThisCity = true;
            s_LastRefreshTicksUtc = DateTime.UtcNow.Ticks;
        }

        // -----------------------------------------------------------------
        // Snapshot formatting + localization (player-facing)
        // -----------------------------------------------------------------

        private static void BuildAndApplySnapshot(World world)
        {
            DeathcareStatusSystem sys = world.GetOrCreateSystemManaged<DeathcareStatusSystem>();
            DeathcareStatusSystem.Snapshot snap = sys.BuildSnapshot();

            // Local time label only (no UTC/Z semantics shown to players).
            string refreshedTime = FormatTime(snap.SnapshotTimeLocal);

            SummaryLine1 = string.Format(
                L(KeyLine1, FallbackLine1),
                Format0(snap.DeadWaiting),
                refreshedTime);

            SummaryLine2 = string.Format(
                L(KeyLine2, FallbackLine2),
                Format0(snap.DeathsPerMonth),
                Format0(snap.ProcessingRate));

            SummaryLine3 = string.Format(
                L(KeyLine3, FallbackLine3),
                Format0(snap.Hearses),
                snap.ActiveFacilities,
                snap.TotalFacilities,
                Format0(snap.CemeteryUse),
                Format0(snap.CemeteryCapacity),
                Format0(snap.MaxWorkers));
        }

        private static string L(string entryId, string englishFallback)
        {
            // Localize via active dictionary. Fallback is English for resilience.
            var lm = GameManager.instance?.localizationManager;
            var dict = lm?.activeDictionary;

            if (dict != null &&
                dict.TryGetValue(entryId, out string value) &&
                !string.IsNullOrEmpty(value))
            {
                return value;
            }

            return englishFallback;
        }

        private static string FormatTime(DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }

        private static string Format0(float v)
        {
            return ((long)Math.Round(v)).ToString("N0");
        }

        private static string Format0(long v)
        {
            return v.ToString("N0");
        }
    }
}
