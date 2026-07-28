// <copyright file="DeathcareLogReport.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Status/DeathcareLogReport.cs
// Purpose: Writes a detailed, explained deathcare snapshot to MagicHearse.log on request.

namespace MagicHearse
{
    using System;                      // Math, Exception
    using System.Collections.Generic;  // List
    using System.Text;                 // StringBuilder
    using CS2Shared.RiverMochi;        // LogUtils
    using Game;                        // IsGame()
    using Game.SceneFlow;              // GameManager
    using Unity.Entities;              // World

    internal static class DeathcareLogReport
    {
        public static void Write()
        {
            try
            {
                LogUtils.Info(BuildReport());
            }
            catch (Exception ex)
            {
                LogUtils.Warn(() =>
                    $"[MH] Log Report failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static string BuildReport()
        {
            StringBuilder report = new();
            report.AppendLine();
            report.AppendLine("================ MAGIC HEARSE LOG REPORT ================");
            report.AppendLine($"Mod: {Mod.kModName} {Mod.ModVersion} [{Mod.kBuildType}]");
            report.AppendLine($"Report time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            World world = World.DefaultGameObjectInjectionWorld;
            GameManager? gameManager = GameManager.instance;
            if (world == null || !world.IsCreated ||
                gameManager == null || !gameManager.gameMode.IsGame())
            {
                report.AppendLine("No city is loaded, so no deathcare data is available.");
                report.AppendLine("===========================================================");
                return report.ToString();
            }

            DeathcareStatusSystem statusSystem =
                world.GetOrCreateSystemManaged<DeathcareStatusSystem>();
            DeathcareStatusSystem.Snapshot snap = statusSystem.BuildSnapshot();

            AppendSettings(report);
            AppendDispatch(report, snap);
            AppendHearses(report, snap);
            AppendFacilities(report, snap);
            AppendCemeteries(report, world, snap);
            AppendFocus(report, snap);

            report.AppendLine("===========================================================");
            return report.ToString();
        }

        private static void AppendSettings(StringBuilder report)
        {
            MHSetting? settings = Mod.Settings;
            report.AppendLine();
            report.AppendLine("MODE AND SETTINGS");

            if (settings == null)
            {
                report.AppendLine("  Settings are not available.");
                return;
            }

            report.AppendLine($"  Magic Clean: {OnOff(settings.EnableMagicHearse)}");
            report.AppendLine($"  Magic cemetery reset: {OnOff(settings.MagicResetCemetery)}");
            report.AppendLine($"  Funeral Director: {OnOff(settings.FuneralDirector)}");
            report.AppendLine($"  Processing rate: {settings.ProcScalar}%");
            report.AppendLine($"  Fleet size: {settings.FleetScalar}%");
            report.AppendLine($"  Cemetery storage: {settings.StorageScalar}%");
            report.AppendLine($"  Hearse speed: {settings.HearseSpeedScalar}%");
            report.AppendLine($"  Funeral Director cemetery reset: {OnOff(settings.AutoResetCemetery)}");
            report.AppendLine($"  Control workers: {OnOff(settings.ControlWorkers)}");
            report.AppendLine($"  Max workers: {settings.WorkersScalar}%");
        }

        private static void AppendDispatch(
            StringBuilder report,
            DeathcareStatusSystem.Snapshot snap)
        {
            report.AppendLine();
            report.AppendLine("DEATHS AND DISPATCH");
            report.AppendLine($"  Dead waiting for pickup: {Format0(snap.DeadWaiting)}");
            report.AppendLine($"  Assigned to a service: {Format0(snap.DeadAssigned)}");
            report.AppendLine($"  Not currently assigned: {Format0(snap.DeadUnassigned)}");
            report.AppendLine($"  Incoming outside-service assignments: {Format0(snap.DeadAssignedOutside)}");
            report.AppendLine($"  Assignment total check: {Format0(snap.DeadAssigned + snap.DeadUnassigned)}");
            report.AppendLine($"  Deaths per month: {Format0(snap.DeathsPerMonth)}");
            report.AppendLine("  Note: outside service is included in assigned.");
            report.AppendLine("  Outside service means a hearse can come from an outside connection into the city;");
            report.AppendLine("  it does not mean a city-owned hearse was sent outside to provide service.");
        }

        private static void AppendHearses(
            StringBuilder report,
            DeathcareStatusSystem.Snapshot snap)
        {
            long unusedSlots = Math.Max(0L, snap.Hearses - snap.SpawnedHearses);
            long stateTotal =
                snap.HearseIdle + snap.HearseDispatched + snap.HearseTransporting +
                snap.HearseReturning + snap.HearseDisabled;

            report.AppendLine();
            report.AppendLine("HEARSES OWNED BY ACTIVE IN-CITY FACILITIES");
            report.AppendLine($"  Fleet capacity slots: {Format0(snap.Hearses)}");
            report.AppendLine($"  Spawned hearse entities: {Format0(snap.SpawnedHearses)}");
            report.AppendLine($"  Parked hearse entities: {Format0(snap.ParkedHearses)}");
            report.AppendLine($"  On-road / active hearse entities: {Format0(snap.WorkingHearses)}");
            report.AppendLine($"  Capacity slots without a spawned entity: {Format0(unusedSlots)}");
            report.AppendLine("  State breakdown (each spawned hearse is counted once):");
            report.AppendLine($"    No active state / idle: {Format0(snap.HearseIdle)}");
            report.AppendLine($"    Sent to pickup: {Format0(snap.HearseDispatched)}");
            report.AppendLine($"    Carrying a corpse: {Format0(snap.HearseTransporting)}");
            report.AppendLine($"    Returning: {Format0(snap.HearseReturning)}");
            report.AppendLine($"    Disabled flag: {Format0(snap.HearseDisabled)}");
            report.AppendLine($"    State total check: {Format0(stateTotal)}");
            report.AppendLine("  Explanation: fleet capacity is the maximum number of vehicle slots, not the");
            report.AppendLine("  number of hearse entities that currently exist. Parked is a separate position");
            report.AppendLine("  count and normally overlaps the no-active-state / idle state.");
        }

        private static void AppendFacilities(
            StringBuilder report,
            DeathcareStatusSystem.Snapshot snap)
        {
            report.AppendLine();
            report.AppendLine("FACILITIES AND PROCESSING");
            report.AppendLine($"  Active facilities: {snap.ActiveFacilities} of {snap.TotalFacilities} placed");
            report.AppendLine($"  Cremation processing max per month: {Format0(snap.ProcessingRate)}");
            report.AppendLine($"  Max workers at active facilities: {Format0(snap.MaxWorkers)}");
            report.AppendLine($"  Active facilities flagged full: {snap.FullFacilities}");
            report.AppendLine($"  Active facilities reporting no available hearse: {snap.FacilitiesWithoutAvailableHearse}");
            report.AppendLine($"  Active facilities with bodies waiting to process: {snap.FacilitiesWithProcessingQueue}");
            report.AppendLine("  A zero means the game is not reporting that condition at this snapshot;");
            report.AppendLine("  it is not a recommendation by itself.");
        }

        private static void AppendCemeteries(
            StringBuilder report,
            World world,
            DeathcareStatusSystem.Snapshot snap)
        {
            report.AppendLine();
            report.AppendLine("CEMETERIES");
            report.AppendLine($"  Graves used: {Format0(snap.CemeteryUse)} of {Format0(snap.CemeteryCapacity)}");

            CemeteryResetSystem resetSystem =
                world.GetOrCreateSystemManaged<CemeteryResetSystem>();
            report.AppendLine($"  Resets this session: {resetSystem.SessionResetTotal}");
            report.AppendLine($"  Cemeteries reset this session: {resetSystem.DistinctCemeteryCount}");

            if (resetSystem.SessionResetTotal <= 0)
            {
                return;
            }

            List<CemeteryResetSystem.Tally> tallies = new();
            resetSystem.CopyTopEmptied(tallies, int.MaxValue);
            for (int i = 0; i < tallies.Count; i++)
            {
                CemeteryResetSystem.Tally tally = tallies[i];
                string name = string.IsNullOrWhiteSpace(tally.Name)
                    ? "Unnamed cemetery"
                    : tally.Name;
                report.AppendLine($"    {name}: {tally.Count}");
            }
        }

        private static void AppendFocus(
            StringBuilder report,
            DeathcareStatusSystem.Snapshot snap)
        {
            report.AppendLine();
            report.AppendLine("WHAT TO FOCUS ON");

            if (snap.DeadWaiting <= 0)
            {
                report.AppendLine("  No current corpse-pickup backlog was found.");
                return;
            }

            bool found = false;

            if (snap.DeadUnassigned > 0)
            {
                found = true;
                report.AppendLine($"  - Dispatch: {Format0(snap.DeadUnassigned)} waiting corpses have no current service assignment.");
                if (snap.FacilitiesWithoutAvailableHearse > 0)
                {
                    report.AppendLine($"    {snap.FacilitiesWithoutAvailableHearse} active facilities report no available hearse.");
                    report.AppendLine("    Check service budget/fleet size and whether hearses are tied up in traffic.");
                }
                else
                {
                    report.AppendLine("    Hearse availability is not the obvious limit in this snapshot.");
                    report.AppendLine("    Check road access and service-district restrictions, then let the city run");
                    report.AppendLine("    and repeat the report to see whether assignments are merely still being matched.");
                }
            }

            if (snap.DeadAssigned > 0)
            {
                found = true;
                report.AppendLine($"  - Travel: {Format0(snap.DeadAssigned)} waiting corpses already have a service assignment.");
                report.AppendLine("    If they remain waiting, route distance and traffic are the likely things to inspect.");
            }

            if (snap.DeadAssignedOutside > 0)
            {
                found = true;
                report.AppendLine($"  - Outside service: {Format0(snap.DeadAssignedOutside)} assignments are incoming from outside connections.");
                report.AppendLine("    They can take longer because the hearse starts outside the city.");
            }

            if (snap.FullFacilities > 0)
            {
                found = true;
                report.AppendLine($"  - Storage: {snap.FullFacilities} active deathcare facilities are flagged full.");
                report.AppendLine("    Empty/increase cemetery storage or add processing capacity.");
            }

            if (snap.ProcessingRate > 0f && snap.DeathsPerMonth > snap.ProcessingRate)
            {
                found = true;
                report.AppendLine($"  - Processing: deaths/month ({Format0(snap.DeathsPerMonth)}) exceed cremation max/month ({Format0(snap.ProcessingRate)}).");
                report.AppendLine("    Raise processing rate or add crematorium capacity if processing queues keep growing.");
            }

            if (snap.CemeteryCapacity > 0 &&
                snap.CemeteryUse * 10 >= snap.CemeteryCapacity * 9)
            {
                found = true;
                report.AppendLine("  - Cemetery storage is at least 90% full.");
            }

            if (snap.FacilitiesWithProcessingQueue > 0)
            {
                found = true;
                report.AppendLine($"  - Processing queue: {snap.FacilitiesWithProcessingQueue} active facilities currently contain bodies awaiting processing.");
                report.AppendLine("    A short queue is normal; focus on it only if the count persists or grows.");
            }

            if (!found)
            {
                report.AppendLine("  No single capacity or dispatch problem is proven by this snapshot.");
            }
        }

        private static string OnOff(bool value) => value ? "ON" : "OFF";
        private static string Format0(float value) => ((long)Math.Round(value)).ToString("N0");
        private static string Format0(long value) => value.ToString("N0");
    }
}
