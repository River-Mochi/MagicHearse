// File: Systems/DeathcareStatus.cs
// Purpose: UI-facing cached Status snapshot strings for Options UI.
// Notes:
// - Refresh is driven by OptionsUI getters (no performance cost, only while tab is open).
// - Uses explicit state (bool/ticks), NOT string comparisons.
// - Cache is invalidated on main-menu <-> in-game transitions.
// - Fixed UI strings are pulled from LocalizationManager.activeDictionary.

namespace MagicHearse
{
    using Game;                   // GameMode extension: IsGame()
    using Game.SceneFlow;         // GameManager
    using System;                 // DateTime, TimeSpan
    using Unity.Entities;         // World

    public static class DeathcareStatus
    {
        // Custom keys (add to all Locales)
        private const string kKeyStatusNotLoaded = "MH_STATUS_NOT_LOADED";
        private const string kKeyNoCityLoaded = "MH_STATUS_NO_CITY_LOADED";


        // Public UI strings used by Setting.cs getters.
        public static string SummaryLine1 { get; set; } = string.Empty;
        public static string SummaryLine2 { get; set; } = string.Empty;
        public static string SummaryLine3 { get; set; } = string.Empty;

        // Throttle refresh while the Status tab is open. Time update frequency.
        public static int RefreshIntervalSeconds { get; set; } = 15;

        private static bool s_WasInGame;
        private static bool s_HasSnapshotThisCity;
        private static bool s_ShowNoCityLoaded;
        private static long s_LastRefreshTicksUtc;

        /// <summary>
        /// Clear cached snapshot so the next getter refreshes (no stale data from city switches).
        /// Safe to call from anywhere.
        /// </summary>
        public static void InvalidateCache()
        {
            s_HasSnapshotThisCity = false;
            s_ShowNoCityLoaded = false;
            s_LastRefreshTicksUtc = 0;

            SummaryLine1 = L(kKeyStatusNotLoaded);
            SummaryLine2 = string.Empty;
            SummaryLine3 = string.Empty;
        }

        /// <summary>
        /// Marks the cached snapshot as stale so the next getter refreshes instantly.
        /// Does NOT overwrite current UI strings (UX: prevents text flicker: keep last snapshot until next refresh).
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
            // Initialize placeholder text (localized) before the first city loads.
            if (string.IsNullOrEmpty(SummaryLine1))
            {
                SummaryLine1 = L(kKeyStatusNotLoaded);
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
                // In menu: show the message once; do not keep ticking.
                if (!s_ShowNoCityLoaded)
                {
                    s_ShowNoCityLoaded = true;
                    SummaryLine1 = L(kKeyNoCityLoaded);
                    SummaryLine2 = string.Empty;
                    SummaryLine3 = string.Empty;
                }
                return;
            }

            // City loaded: refresh instantly when there is no snapshot yet.
            if (!s_HasSnapshotThisCity)
            {
                world.GetOrCreateSystemManaged<DeathcareStatusSystem>().RefreshNow();
                s_HasSnapshotThisCity = true;
                s_LastRefreshTicksUtc = DateTime.UtcNow.Ticks;
                return;
            }

            // Throttle refresh while tab is open.
            long nowTicks = DateTime.UtcNow.Ticks;
            long minNext = s_LastRefreshTicksUtc + TimeSpan.FromSeconds(RefreshIntervalSeconds).Ticks;
            if (nowTicks < minNext)
            {
                return;
            }

            world.GetOrCreateSystemManaged<DeathcareStatusSystem>().RefreshNow();
            s_LastRefreshTicksUtc = nowTicks;
        }

        // No-op: optional manual refresh hook (possible future button).
        public static void ForceRefreshNow()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            world.GetOrCreateSystemManaged<DeathcareStatusSystem>().RefreshNow();
            s_HasSnapshotThisCity = true;
            s_LastRefreshTicksUtc = DateTime.UtcNow.Ticks;
        }

        private static string L(string entryId)
        {
            // Localize via active dictionary. Fallback = key (debug-visible, avoids hardcoded English here).
            var lm = GameManager.instance?.localizationManager;
            var dict = lm?.activeDictionary;

            if (dict != null && dict.TryGetValue(entryId, out string value) && !string.IsNullOrEmpty(value))
            {
                return value;
            }

            return entryId;
        }
    }
}
