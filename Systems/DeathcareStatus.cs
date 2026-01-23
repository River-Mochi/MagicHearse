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
    using System;                 // DateTime, TimeSpan, Math, FormatException
    using Unity.Entities;         // World
    using UnityEngine;            // Time.frameCount, Debug

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

        // --------------------------------------------------------------------------
        // English fallbacks (player-facing)
        // IMPORTANT: placeholders must match the arg lists in BuildAndApplySnapshot().
        // --------------------------------------------------------------------------

        private const string FallbackStatusNotLoaded = "Status not loaded.";
        private const string FallbackNoCityLoaded = "No city loaded yet.";

        // Line1 expects: {0}=deadWaiting, {1}=deaths/mo, {2}=canHandled, {3}=time
        private const string FallbackLine1 =
            "{0} dead waiting • {1} deaths/month • {2} can be handled • updated {3}";

        // Line2 expects: {0}=deaths/mo, {1}=canHandled
        private const string FallbackLine2 =
            "{0} deaths/month | {1} can be handled";

        // Line3 expects: {0}=hearses, {1}=activeFacilities, {2}=totalFacilities,
        //                {3}=cemUse, {4}=cemCap, {5}=maxWorkers
        private const string FallbackLine3 =
            "{0} hearses | {1} / {2} buildings | {3} / {4} cemetery use | {5} max workers";

        // -----------------------------------------------------------------
        // Public UI strings used by Setting.cs getters
        // -----------------------------------------------------------------

        // Throttle refresh while the Status group is visible in Options UI.
        public static int RefreshIntervalSeconds { get; set; } = 15;

        public static string SummaryLine1 { get; private set; } = string.Empty;
        public static string SummaryLine2 { get; private set; } = string.Empty;
        public static string SummaryLine3 { get; private set; } = string.Empty;

        // -----------------------------------------------------------------
        // Cache state
        // -----------------------------------------------------------------

        private static bool s_WasInGame;
        private static bool s_HasSnapshotThisCity;
        private static bool s_ShowNoCityLoadedOnce;
        private static long s_LastRefreshTicksUtc;
        private static int s_LastUiFrame = -1;

        // One-time diagnostics: don’t spam logs every frame if a locale string is broken.
        private static bool s_LoggedBadFormat;

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

            // Frame guard: prevents Setting.cs from calling this 3x per UI draw.
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

            // Throttle refresh while Options UI is open (UTC = no DST weirdness).
            long nowTicksUtc = DateTime.UtcNow.Ticks;
            long minNext = s_LastRefreshTicksUtc + TimeSpan.FromSeconds(RefreshIntervalSeconds).Ticks;
            if (nowTicksUtc < minNext)
            {
                return;
            }

            BuildAndApplySnapshot(world);
            s_LastRefreshTicksUtc = nowTicksUtc;
        }

        // No-op: manual refresh hook (possible future button).
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

            // UX Local time string for players (do not use for throttling, it has DST).
            string refreshedTime = FormatTime(snap.SnapshotTimeLocal);

            // Safe formatting so a bad locale string cannot crash Options UI.
            SummaryLine1 = FormatOrFallback(
                key: KeyLine1,
                localizedFormat: L(KeyLine1, FallbackLine1),
                fallbackFormat: FallbackLine1,
                Format0(snap.DeadWaiting),       // {0}
                Format0(snap.DeathsPerMonth),    // {1}
                Format0(snap.ProcessingRate),    // {2}
                refreshedTime);                  // {3}

            SummaryLine2 = FormatOrFallback(
                key: KeyLine2,
                localizedFormat: L(KeyLine2, FallbackLine2),
                fallbackFormat: FallbackLine2,
                Format0(snap.DeathsPerMonth),    // {0}
                Format0(snap.ProcessingRate));   // {1}

            SummaryLine3 = FormatOrFallback(
                key: KeyLine3,
                localizedFormat: L(KeyLine3, FallbackLine3),
                fallbackFormat: FallbackLine3,
                Format0(snap.Hearses),           // {0}
                snap.ActiveFacilities,           // {1}
                snap.TotalFacilities,            // {2}
                Format0(snap.CemeteryUse),       // {3}
                Format0(snap.CemeteryCapacity),  // {4}
                Format0(snap.MaxWorkers));       // {5}
        }

        private static string L(string entryId, string englishFallback)
        {
            // Localize via active dictionary with English fallback.
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

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private static string FormatOrFallback(string key, string localizedFormat, string fallbackFormat, params object[] args)
        {
            try
            {
                return string.Format(localizedFormat, args);
            }
            catch (FormatException ex)
            {
                // If a locale string has wrong {n} placeholders, never crash the UI.
                if (!s_LoggedBadFormat)
                {
                    s_LoggedBadFormat = true;

                    Debug.LogError(
                        "[MH] Status format error. " +
                        "A locale string has wrong {n} placeholders. " +
                        $"Key={key} Args={args.Length} Format='{localizedFormat}'");
                    Debug.LogException(ex);
                }

                // Try the English fallback (should be correct).
                try
                {
                    return string.Format(fallbackFormat, args);
                }
                catch
                {
                    // Worst case: return something readable and safe.
                    return fallbackFormat;
                }
            }
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
