// File: Systems/DeathcareStatus.cs
// Purpose: UI-facing cached Status snapshot strings for Options UI.
// Notes:
// - Refresh is driven by OptionsUI getters (i.e., only while tab is open).
// - Handles main-menu -> city-load transition automatically (no stuck "No city loaded yet.").
// - Throttled to avoid expensive refresh spam.

namespace MagicHearse
{
    using Game;
    using Game.SceneFlow;          // GameManager
    using System;
    using Unity.Entities;          // World

    public static class DeathcareStatus
    {
        public static string LastRefreshUtc { get; set; } = "Idle";
        public static string SummaryLine1 { get; set; } = "Status not loaded.";
        public static string SummaryLine2 { get; set; } = string.Empty;

        // Throttle refresh while the Status tab is open.
        private const int RefreshIntervalSeconds = 15;
        private static long s_LastRefreshTicksUtc;

        // Called by Setting.cs getters.
        public static void RefreshIfNeeded()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            var gm = GameManager.instance;
            bool isGame = (gm != null && gm.gameMode.IsGame());

            if (!isGame)
            {
                // Update once when in main menu / no city loaded.
                if (LastRefreshUtc == "Idle" || SummaryLine1 == "Status not loaded.")
                {
                    s_LastRefreshTicksUtc = DateTime.UtcNow.Ticks;
                    LastRefreshUtc = FormatUtc(new DateTime(s_LastRefreshTicksUtc, DateTimeKind.Utc));
                    SummaryLine1 = "No city loaded yet.";
                    SummaryLine2 = string.Empty;
                }
                return;
            }

            // We are in-game now. If we previously wrote the main-menu message,
            // treat that as "invalid" so we don't get stuck forever.
            if (SummaryLine1 == "No city loaded yet.")
            {
                InvalidateCache();
            }

            // If there are Idle refreshed for this city/session, do it now.
            if (LastRefreshUtc == "Idle" || SummaryLine1 == "Status not loaded.")
            {
                world.GetOrCreateSystemManaged<DeathcareStatusSystem>().RefreshNow();
                s_LastRefreshTicksUtc = DateTime.UtcNow.Ticks;
                return;
            }

            // Otherwise refresh only if the throttle window has passed.
            long nowTicks = DateTime.UtcNow.Ticks;
            long minNext = s_LastRefreshTicksUtc + TimeSpan.FromSeconds(RefreshIntervalSeconds).Ticks;
            if (nowTicks < minNext)
            {
                return;
            }

            world.GetOrCreateSystemManaged<DeathcareStatusSystem>().RefreshNow();
            s_LastRefreshTicksUtc = nowTicks;
        }

        // Optional manual refresh hook (you can wire to a button later if you want).
        public static void ForceRefreshNow()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            world.GetOrCreateSystemManaged<DeathcareStatusSystem>().RefreshNow();
            s_LastRefreshTicksUtc = DateTime.UtcNow.Ticks;
        }

        public static void InvalidateCache()
        {
            LastRefreshUtc = "Idle";
            SummaryLine1 = "Status not loaded.";
            SummaryLine2 = string.Empty;
            s_LastRefreshTicksUtc = 0;
        }

        private static string FormatUtc(DateTime utc)
        {
            // Seconds only (no milliseconds).
            return utc.ToString("HH:mm:ss") + " UTC";
        }
    }
}
