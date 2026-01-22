// File: Systems/DeathcareStatus.cs
// Purpose: UI-facing cached Status snapshot strings for Options UI.
// Notes:
// - Refresh is driven by OptionsUI getters (no performance cost, only while tab is open).
// - Uses explicit state (bool/ticks), NOT string comparisons, so "Idle" etc. are safe.
// - Cache is invalidated on main-menu <-> in-game transitions.

namespace MagicHearse
{
    using Game;
    using Game.SceneFlow;          // GameManager
    using System;                  // DateTime, TimeSpan
    using Unity.Entities;          // World

    public static class DeathcareStatus
    {
        // Public UI strings consumed by Setting.cs getters.
        public static string SummaryLine1 { get; set; } = "Status not loaded.";
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

            SummaryLine1 = "Status not loaded.";
            SummaryLine2 = string.Empty;
            SummaryLine3 = string.Empty;
        }

        /// <summary>
        /// Marks the cached snapshot as stale so the next getter refreshes immediately.
        /// Does NOT overwrite current UI strings (No text flicker: keep last snapshot until next refresh).
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
                    SummaryLine1 = "No city loaded yet.";
                    SummaryLine2 = string.Empty;
                    SummaryLine3 = string.Empty;
                }
                return;
            }

            // In-game: refresh immediately when there is no snapshot yet.
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

        // No-op: optional manual refresh hook (wire to a future button if desired).
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
    }
}
