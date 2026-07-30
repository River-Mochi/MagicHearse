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
    using System;                      // Math, Exception
    using System.Collections.Generic;  // List
    using System.Text;                 // StringBuilder
    using Colossal.Localization;
    using CS2Shared.RiverMochi;        // LogUtils
    using Game;                        // IsGame()
    using Game.SceneFlow;              // GameManager
    using Unity.Entities;              // World

    public static class DeathcareStatus
    {
        // Locale keys
        internal const string kKeyStatusNotLoaded = "MH_STATUS_NOT_LOADED";
        internal const string kKeyNoCityLoaded = "MH_STATUS_NO_CITY_LOADED";
        internal const string kKeyStatsNotAvail = "MH_STATUS_STATS_NOT_AVAIL";
        internal const string kKeyLine1 = "MH_STATUS_LINE1";
        internal const string kKeyLine2 = "MH_STATUS_LINE2";
        internal const string kKeyLine3 = "MH_STATUS_LINE3";
        internal const string kKeyProcessingSuggested =
            "MH_STATUS_PROCESSING_SUGGESTED";
        internal const string kKeyProcessingMore =
            "MH_STATUS_PROCESSING_MORE";
        internal const string kKeyProcessingNone =
            "MH_STATUS_PROCESSING_NONE";
        internal const string kKeyLine4 = "MH_STATUS_LINE4";
        internal const string kKeyCemeteryNone = "MH_STATUS_CEMETERY_NONE";
        internal const string kKeyCemeteryRow = "MH_STATUS_CEMETERY_ROW";
        internal const string kKeyCemeteryMore = "MH_STATUS_CEMETERY_MORE";

        // Rough character limit for packed cemetery-names row before it spills to "+N more".
        private const int kCemeteryNameBudget = 46;

        // English fallbacks (placeholders must match BuildAndApplySnapshot arg lists)
        private const string kFallbackStatusNotLoaded = "Status not loaded.";
        private const string kFallbackNoCityLoaded   = "No city loaded.";
        private const string kFallbackStatsNotAvail  = "No city... ¯\\_(ツ)_/¯ ...No stats";
        private const string kFallbackLine1 = "{0} waiting | {1} deaths/mo | updated {2}";
        private const string kFallbackLine2 = "{0} cremate max/mo | {1}/{2} graves used";
        private const string kFallbackLine3 = "{0} / {1} hearses | {2} / {3} buildings | {4} max workers";
        private const string kFallbackProcessingSuggested =
            "Suggested now: ~{0}% processing";
        private const string kFallbackProcessingMore =
            "Suggested now: 500% processing + more active facilities";
        private const string kFallbackProcessingNone =
            "Suggested: turn on/add crematoriums";
        private const string kFallbackLine4 = "resets: {0} · cemeteries: {1}";
        private const string kFallbackCemeteryNone = "none this session";
        private const string kFallbackCemeteryRow = "{0} ×{1}";
        private const string kFallbackCemeteryMore = "+{0} more";

        // Public UI strings read by MHSetting.cs getters
        public static string SummaryLine1 { get; private set; } = string.Empty;
        public static string SummaryLine2 { get; private set; } = string.Empty;
        public static string SummaryLine3 { get; private set; } = string.Empty;
        public static string SummaryLine4 { get; private set; } = string.Empty;
        public static string SummaryCemetery1 { get; private set; } = string.Empty;

        // Reused buffer for the top-N cemetery tallies.
        private static readonly List<CemeteryResetSystem.Tally> s_TopBuffer = new();

        // Cache state
        private static bool s_WasInGame;
        private static bool s_HasSnapshotThisCity;
        private static uint s_LastSnapshotSimulationFrame = uint.MaxValue;

        /// <summary>Clears snapshot so next getter refreshes (prevents stale data after city switches).</summary>
        public static void InvalidateCache()
        {
            s_HasSnapshotThisCity = false;
            s_LastSnapshotSimulationFrame = uint.MaxValue;

            SummaryLine1 = Localize(kKeyStatusNotLoaded, kFallbackStatusNotLoaded);
            SummaryLine2 = string.Empty;
            SummaryLine3 = string.Empty;
            ClearCemeteryLines();
        }

        /// <summary>Marks snapshot stale so next getter refreshes. Current text stays until refresh.</summary>
        public static void MarkDirty()
        {
            s_HasSnapshotThisCity = false;
            s_LastSnapshotSimulationFrame = uint.MaxValue;
        }

        // Called by MHSetting.cs getters.
        public static void RefreshIfNeeded()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            if (string.IsNullOrEmpty(SummaryLine1))
            {
                SummaryLine1 = Localize(kKeyStatusNotLoaded, kFallbackStatusNotLoaded);
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
                SummaryLine1 = Localize(kKeyNoCityLoaded, kFallbackNoCityLoaded);
                SummaryLine2 = Localize(kKeyStatsNotAvail, kFallbackStatsNotAvail);
                SummaryLine3 = string.Empty;
                    ClearCemeteryLines();
                return;
            }

            DeathcareStatusSystem statusSystem =
                world.GetOrCreateSystemManaged<DeathcareStatusSystem>();

            uint simulationFrame = statusSystem.CurrentSimulationFrame;

            // Options pauses the city. Same simulation frame means the snapshot is still current.
            if (s_HasSnapshotThisCity &&
                s_LastSnapshotSimulationFrame == simulationFrame)
            {
                return;
            }

            BuildSnapshotSafe(world, statusSystem);
            s_HasSnapshotThisCity = true;
            s_LastSnapshotSimulationFrame = simulationFrame;
        }

        private static void BuildSnapshotSafe(
            World world,
            DeathcareStatusSystem statusSystem)
        {
            try
            {
                BuildAndApplySnapshot(world, statusSystem);
            }
            catch (Exception ex)
            {
                SummaryLine1 = Localize(kKeyStatusNotLoaded, kFallbackStatusNotLoaded);
                SummaryLine2 = string.Empty;
                SummaryLine3 = string.Empty;
                    ClearCemeteryLines();

                LogUtils.WarnOnce("MH_STATUS_SNAPSHOT_EXCEPTION", () =>
                    $"[MH] Status snapshot failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void BuildAndApplySnapshot(
            World world,
            DeathcareStatusSystem statusSystem)
        {
            DeathcareStatusSystem.Snapshot snap = statusSystem.BuildSnapshot();

            string refreshedTime = snap.SnapshotTimeLocal.ToString("HH:mm:ss");

            SummaryLine1 = SafeFormat(
                kKeyLine1,
                fallbackFormat: kFallbackLine1,
                Format0(snap.DeadWaiting),      // {0}
                Format0(snap.DeathsPerMonth),   // {1}
                refreshedTime);                 // {2}

            SummaryLine2 = SafeFormat(
                kKeyLine2,
                fallbackFormat: kFallbackLine2,
                Format0(snap.ProcessingRate),   // {0} <- game’s “handling capacity/mo”
                Format0(snap.CemeteryUse),      // {1}
                Format0(snap.CemeteryCapacity)  // {2}
            );
            AppendProcessingSuggestion(snap);

            SummaryLine3 = SafeFormat(
                kKeyLine3,
                fallbackFormat: kFallbackLine3,
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

        private static void AppendProcessingSuggestion(
            DeathcareStatusSystem.Snapshot snap)
        {
            if (snap.DeathsPerMonth <= 0f ||
                snap.DeathsPerMonth <= snap.ProcessingRate)
            {
                return;
            }

            string suggestion;
            if (snap.CrematoriumProcessingRate <= 0f)
            {
                suggestion = Localize(
                    kKeyProcessingNone,
                    kFallbackProcessingNone);
            }
            else
            {
                int suggestedPercent =
                    GetSuggestedProcessingPercent(snap);

                suggestion = suggestedPercent <= 500
                    ? SafeFormat(
                        kKeyProcessingSuggested,
                        kFallbackProcessingSuggested,
                        suggestedPercent)
                    : Localize(
                        kKeyProcessingMore,
                        kFallbackProcessingMore);
            }

            SummaryLine2 += " · " + suggestion;
        }

        internal static int GetSuggestedProcessingPercent(
            DeathcareStatusSystem.Snapshot snap)
        {
            if (snap.ProcessingRate <= 0f || snap.DeathsPerMonth <= 0f)
            {
                return 0;
            }

            MHSetting? settings = Mod.Settings;
            int currentPercent =
                settings != null && settings.FuneralDirector
                    ? Math.Max(100, settings.ProcScalar)
                    : 100;

            if (snap.CrematoriumProcessingRate <= 0f)
            {
                return 0;
            }

            // Cemetery turnover is controlled separately, so only calculate the
            // crematorium increase needed after its current contribution.
            double crematoriumRateNeeded =
                Math.Max(0d, snap.DeathsPerMonth - snap.CemeteryTurnoverRate);
            double needed =
                currentPercent * crematoriumRateNeeded /
                snap.CrematoriumProcessingRate;

            // Match the Funeral Director slider's 10% steps.
            return Math.Max(100, (int)(Math.Ceiling(needed / 10d) * 10d));
        }

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
                SummaryLine4 = Localize(kKeyCemeteryNone, kFallbackCemeteryNone);
                SummaryCemetery1 = string.Empty;
                return;
            }

            // Summary row shows totals; packed row names the cemeteries (with "+N more" spill)
            SummaryLine4 = SafeFormat(kKeyLine4, kFallbackLine4, total, resetSys.DistinctCemeteryCount);
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
                string entry = SafeFormat(kKeyCemeteryRow, kFallbackCemeteryRow, tally.Name ?? string.Empty, tally.Count);

                int sep = shown == 0 ? 0 : 3; // " · "
                if (shown > 0 && sb.Length + sep + entry.Length > kCemeteryNameBudget)
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

                sb.Append(SafeFormat(kKeyCemeteryMore, kFallbackCemeteryMore, remaining));
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
