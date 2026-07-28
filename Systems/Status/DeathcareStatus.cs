// <copyright file="DeathcareStatus.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Status/DeathcareStatus.cs
// Purpose: Builds cached, localized Status text for Options UI requests.
// - Localization + formatting safety handled here; DeathcareStatusSystem returns raw numbers only.

namespace MagicHearse
{
    using System;                      // DateTime, TimeSpan, Math, Exception
    using System.Collections.Generic;  // List
    using System.Text;                 // StringBuilder
    using Colossal.Localization;
    using CS2Shared.RiverMochi;        // LogUtils
    using Game;                        // IsGame()
    using Game.SceneFlow;              // GameManager
    using Unity.Entities;              // World
    using UnityEngine;                 // Time.frameCount

    public static class DeathcareStatus
    {
        // Refresh driven by OptionsUI getters (only when Options is open, not city).
        // NOTE: Do not set to 0 (would refresh every UI poll).
        public static int RefreshIntervalSeconds { get; set; } = 15;    // Throttle refresh while Options UI open.

        // Locale keys (add to all Locale*.cs)
        internal const string KeyStatusNotLoaded = "MH_STATUS_NOT_LOADED";
        internal const string KeyNoCityLoaded = "MH_STATUS_NO_CITY_LOADED";
        internal const string KeyStatsNotAvail = "MH_STATUS_STATS_NOT_AVAIL";
        internal const string KeyLine1 = "MH_STATUS_LINE1";
        internal const string KeyLine2 = "MH_STATUS_LINE2";
        internal const string KeyLine3 = "MH_STATUS_LINE3";
        internal const string KeyLine4 = "MH_STATUS_LINE4";
        internal const string KeyCemeteryNone = "MH_STATUS_CEMETERY_NONE";
        internal const string KeyCemeteryRow = "MH_STATUS_CEMETERY_ROW";
        internal const string KeyCemeteryMore = "MH_STATUS_CEMETERY_MORE";

        // Rough character budget for the packed cemetery-names row before it spills to "+N more".
        private const int CemeteryNameBudget = 46;

        // English fallbacks (placeholders must match BuildAndApplySnapshot arg lists)
        private const string FallbackStatusNotLoaded = "Status not loaded.";
        private const string FallbackNoCityLoaded   = "No city loaded.";
        private const string FallbackStatsNotAvail  = "No city... ¯\\_(ツ)_/¯ ...No stats";
        private const string FallbackLine1 = "{0} waiting | {1} deaths/mo | updated {2}";
        private const string FallbackLine2 = "{0} cremate max/mo | {1}/{2} graves used";
        private const string FallbackLine3 = "{0} / {1} hearses | {2} / {3} buildings | {4} max workers";
        private const string FallbackLine4 = "resets: {0} · cemeteries: {1}";
        private const string FallbackCemeteryNone = "none this session";
        private const string FallbackCemeteryRow = "{0} ×{1}";
        private const string FallbackCemeteryMore = "+{0} more";

        // Public UI strings read by MHSetting.cs getters
        public static string SummaryLine1 { get; private set; } = string.Empty;
        public static string SummaryLine2 { get; private set; } = string.Empty;
        public static string SummaryLine3 { get; private set; } = string.Empty;
        public static string SummaryLine4 { get; private set; } = string.Empty;
        public static string SummaryCemetery1 { get; private set; } = string.Empty;

        // Reused buffer for the top-N cemetery tallies (UI thread only).
        private static readonly List<CemeteryResetSystem.Tally> s_TopBuffer = new();

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
            ClearCemeteryLines();
        }

        /// <summary>Marks snapshot stale so next getter refreshes. Current text stays until refresh.</summary>
        public static void MarkDirty()
        {
            s_HasSnapshotThisCity = false;
            s_LastRefreshTicksUtc = 0;
        }

        // Called by MHSetting.cs getters.
        public static void RefreshIfNeeded()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            // Frame guard: MHSetting.cs calls this once per Status getter per UI draw (several getters).
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
                ClearCemeteryLines();
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
                ClearCemeteryLines();

                LogUtils.WarnOnce("MH_STATUS_SNAPSHOT_EXCEPTION", () =>
                    $"[MH] Status snapshot failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void BuildAndApplySnapshot(World world)
        {
            DeathcareStatusSystem sys = world.GetOrCreateSystemManaged<DeathcareStatusSystem>();
            DeathcareStatusSystem.Snapshot snap = sys.BuildSnapshot();

            string refreshedTime = snap.SnapshotTimeLocal.ToString("HH:mm:ss");

            SummaryLine1 = SafeFormat(
                KeyLine1,
                fallbackFormat: FallbackLine1,
                Format0(snap.DeadWaiting),      // {0}
                Format0(snap.DeathsPerMonth),   // {1}
                refreshedTime);                 // {2}

            SummaryLine2 = SafeFormat(
                KeyLine2,
                fallbackFormat: FallbackLine2,
                Format0(snap.ProcessingRate),   // {0} <- game’s “handling capacity/mo”
                Format0(snap.CemeteryUse),      // {1}
                Format0(snap.CemeteryCapacity)  // {2}
            );

            SummaryLine3 = SafeFormat(
                KeyLine3,
                fallbackFormat: FallbackLine3,
                Format0(snap.WorkingHearses),   // {0}
                Format0(snap.Hearses),          // {1}
                snap.ActiveFacilities,          // {2}
                snap.TotalFacilities,           // {3}
                Format0(snap.MaxWorkers)        // {4}
            );

            // Cemetery auto-reset tally (session-scoped; populated by CemeteryResetSystem).
            ApplyCemeterySection(world.GetOrCreateSystemManaged<CemeteryResetSystem>());
        }

        // ---- Helpers -------

        private static void ClearCemeteryLines()
        {
            SummaryLine4 = string.Empty;
            SummaryCemetery1 = string.Empty;
        }

        private static void ApplyCemeterySection(CemeteryResetSystem resetSys)
        {
            int total = resetSys.SessionResetTotal;

            if (total <= 0)
            {
                SummaryLine4 = Localize(KeyCemeteryNone, FallbackCemeteryNone);
                SummaryCemetery1 = string.Empty;
                return;
            }

            // Summary row shows totals; packed row names the cemeteries (with "+N more" spill),
            // so nothing is hidden even when a city has more cemeteries than fit on one line.
            SummaryLine4 = SafeFormat(KeyLine4, FallbackLine4, total, resetSys.DistinctCemeteryCount);
            SummaryCemetery1 = BuildPackedCemeteries(resetSys);
        }

        // Packs the most-emptied cemeteries onto one row ("name ×count · name ×count · +N more"),
        // stopping at a rough character budget so the row never runs off the panel.
        private static string BuildPackedCemeteries(CemeteryResetSystem resetSys)
        {
            resetSys.CopyTopEmptied(s_TopBuffer, 32);

            StringBuilder sb = new();
            int shown = 0;

            for (int i = 0; i < s_TopBuffer.Count; i++)
            {
                CemeteryResetSystem.Tally tally = s_TopBuffer[i];
                string entry = SafeFormat(KeyCemeteryRow, FallbackCemeteryRow, tally.Name ?? string.Empty, tally.Count);

                int sep = shown == 0 ? 0 : 3; // " · "
                if (shown > 0 && sb.Length + sep + entry.Length > CemeteryNameBudget)
                {
                    break;
                }

                if (shown > 0)
                {
                    sb.Append(" · ");
                }

                sb.Append(entry);
                shown++;
            }

            int remaining = resetSys.DistinctCemeteryCount - shown;
            if (remaining > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" · ");
                }

                sb.Append(SafeFormat(KeyCemeteryMore, FallbackCemeteryMore, remaining));
            }

            return sb.ToString();
        }

        private static string Localize(string entryId, string fallback)
        {
            LocalizationDictionary? dict = GameManager.instance?.localizationManager?.activeDictionary;
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
                LogUtils.WarnOnce("MH_STATUS_BAD_FORMAT_" + key, () =>
                    $"[MH] Status format error. Key={key} Args={args.Length}");

                // Try English fallback.
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
