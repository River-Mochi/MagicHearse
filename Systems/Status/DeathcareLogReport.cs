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
            AppendWarningProgress(report, snap);
            AppendCemeteries(report, world, snap);
            AppendFocus(report, snap);
            report.Append(statusSystem.BuildRequestSamples());

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
            long pipelineTotal =
                snap.DeadNoRequest +
                snap.DeadWaitingForDispatch +
                snap.DeadPathfinding +
                snap.DeadRetryCooldown +
                snap.DeadAssignedFacility +
                snap.DeadAssignedHearse +
                snap.DeadOther;

            report.AppendLine();
            report.AppendLine("CORPSES AND PICKUP PIPELINE");
            report.AppendLine(
                $"  Dead + RequireTransport total: {Format0(snap.DeadRequiringTransport)}");
            report.AppendLine(
                $"  Waiting outside for pickup: {Format0(snap.DeadWaiting)}");
            report.AppendLine(
                $"  Already inside a hearse (returning): {Format0(snap.DeadInsideHearse)}");
            report.AppendLine(
                $"  Already delivered to a facility: {Format0(snap.DeadAtFacility)}");
            report.AppendLine(
                $"  Location total check: {Format0(snap.DeadWaiting + snap.DeadInsideHearse + snap.DeadAtFacility)}");
            report.AppendLine($"  Deaths per month: {Format0(snap.DeathsPerMonth)}");

            report.AppendLine();
            report.AppendLine("  Waiting outside for pickup, by current stage:");
            report.AppendLine($"    No request yet: {Format0(snap.DeadNoRequest)}");
            report.AppendLine(
                $"    Waiting for dispatch group: {Format0(snap.DeadWaitingForDispatch)}");
            report.AppendLine($"    Pathfinding: {Format0(snap.DeadPathfinding)}");
            report.AppendLine(
                $"    Failed / retry cooldown: {Format0(snap.DeadRetryCooldown)}");
            report.AppendLine(
                $"    Assigned to facility: {Format0(snap.DeadAssignedFacility)}");
            report.AppendLine(
                $"    Assigned to hearse: {Format0(snap.DeadAssignedHearse)}");
            report.AppendLine(
                $"    Other / needs investigation: {Format0(snap.DeadOther)}");
            report.AppendLine(
                $"    Waiting-stage total check: {Format0(pipelineTotal)}");

            report.AppendLine(
                $"  Incoming outside-service cases in these categories: {Format0(snap.DeadAssignedOutside)}");
            report.AppendLine();
            report.AppendLine("  Failed dispatch attempts among corpses still waiting outside:");
            report.AppendLine(
                $"    With at least one failed attempt: {Format0(snap.WaitingWithFailedDispatches)}");
            report.AppendLine(
                $"    With {DeathcareStatusSystem.kRepeatedDispatchFailureThreshold}+ failed attempts: " +
                $"{Format0(snap.WaitingWithRepeatedDispatchFailures)}");
            report.AppendLine(
                $"    Highest failed-attempt count: {snap.MaxDispatchFailCount}");
            report.AppendLine("  Meaning:");
            report.AppendLine(
                "    Assigned to facility = a deathcare facility was selected, but its hearse");
            report.AppendLine(
                "    has not taken ownership of the request yet.");
            report.AppendLine(
                "    Assigned to hearse = a hearse is going to the corpse.");
            report.AppendLine(
                "    Already inside hearse = pickup is complete and the corpse is returning.");
            report.AppendLine(
                "    Outside service = a hearse can enter from an outside connection;");
            report.AppendLine(
                "    it does not mean a city-owned hearse was sent outside.");
        }

        private static void AppendHearses(
            StringBuilder report,
            DeathcareStatusSystem.Snapshot snap)
        {
            long unusedEntitySlots = Math.Max(0L, snap.Hearses - snap.SpawnedHearses);
            long parkedTotal =
                snap.ParkedAvailableHearses + snap.ParkedDisabledHearses;
            long onRoadTotal =
                snap.HearseDispatched +
                snap.HearseTransporting +
                snap.HearseReturning +
                snap.HearseOtherOnRoad +
                snap.HearseDisabledOnRoad;

            report.AppendLine();
            report.AppendLine("HEARSES OWNED BY ACTIVE IN-CITY FACILITIES");
            report.AppendLine(
                $"  Configured fleet capacity slots: {Format0(snap.Hearses)}");
            report.AppendLine(
                $"  Usable on-road slots at current budget/efficiency: {Format0(snap.BudgetHearseCapacity)}");
            report.AppendLine(
                $"  Spawned hearse entities: {Format0(snap.SpawnedHearses)}");
            report.AppendLine(
                $"    Parked (exact ParkedCar component): {Format0(snap.ParkedHearses)}");
            report.AppendLine(
                $"    On-road: {Format0(snap.WorkingHearses)}");
            report.AppendLine(
                $"    Spawned total check: {Format0(snap.ParkedHearses + snap.WorkingHearses)}");
            report.AppendLine(
                $"  Configured slots with no hearse entity: {Format0(unusedEntitySlots)}");

            report.AppendLine();
            report.AppendLine("  Parked breakdown:");
            report.AppendLine(
                $"    Available parked hearses: {Format0(snap.ParkedAvailableHearses)}");
            report.AppendLine(
                $"    Disabled parked hearses: {Format0(snap.ParkedDisabledHearses)}");
            report.AppendLine($"    Parked total check: {Format0(parkedTotal)}");

            report.AppendLine();
            report.AppendLine("  On-road breakdown (each on-road hearse counted once):");
            report.AppendLine(
                $"    Going to pickup: {Format0(snap.HearseDispatched)}");
            report.AppendLine(
                $"    Returning with corpse: {Format0(snap.HearseTransporting)}");
            report.AppendLine(
                $"    Returning without corpse: {Format0(snap.HearseReturning)}");
            report.AppendLine(
                $"    Other on-road state: {Format0(snap.HearseOtherOnRoad)}");
            report.AppendLine(
                $"    Disabled on-road: {Format0(snap.HearseDisabledOnRoad)}");
            report.AppendLine($"    On-road total check: {Format0(onRoadTotal)}");

            report.AppendLine();
            report.AppendLine(
                "  Parked means the vehicle has ParkedCar. It never means stopped at a");
            report.AppendLine(
                "  traffic light. A configured slot with no entity is normal: the facility");
            report.AppendLine(
                "  can spawn a hearse when a valid dispatch reaches it.");
        }

        private static void AppendFacilities(
            StringBuilder report,
            DeathcareStatusSystem.Snapshot snap)
        {
            report.AppendLine();
            report.AppendLine("FACILITIES AND PROCESSING");
            report.AppendLine(
                $"  Active facilities: {snap.ActiveFacilities} of {snap.TotalFacilities} placed");
            report.AppendLine(
                $"  Cremation processing max per month: {Format0(snap.ProcessingRate)}");
            report.AppendLine(
                $"  Max workers at active facilities: {Format0(snap.MaxWorkers)}");
            report.AppendLine(
                $"  Active facilities flagged full: {snap.FullFacilities}");
            report.AppendLine(
                $"  Active facilities with no room for another body: {snap.FacilitiesWithoutRoomForBodies}");
            report.AppendLine(
                $"  Active facilities reporting no available hearse: {snap.FacilitiesWithoutAvailableHearse}");
            report.AppendLine(
                $"  Active facilities with zero usable dispatch slots: {snap.FacilitiesWithZeroDispatchCapacity}");
            report.AppendLine(
                $"  Active facilities with bodies waiting to process: {snap.FacilitiesWithProcessingQueue}");
            report.AppendLine(
                "  Processing rate does not directly send more hearses. It helps dispatch");
            report.AppendLine(
                "  indirectly only when a facility has no room for another body.");
        }

        private static void AppendWarningProgress(
            StringBuilder report,
            DeathcareStatusSystem.Snapshot snap)
        {
            report.AppendLine();
            report.AppendLine("HEARSE WARNING ICON PROGRESS");
            report.AppendLine(
                $"  Live game warning setting: {snap.TransportWarningTime:0.###} simulation seconds");
            report.AppendLine(
                $"  Internal HealthProblem.m_Timer limit: {snap.TransportWarningTimerLimit}");
            report.AppendLine(
                $"  Highest timer among corpses waiting outside: {snap.MaxWaitingTimer}");
            report.AppendLine(
                $"  Below halfway to warning: {Format0(snap.WaitingBelowHalfWarning)}");
            report.AppendLine(
                $"  Halfway to warning: {Format0(snap.WaitingHalfwayToWarning)}");
            report.AppendLine(
                $"  At warning limit: {Format0(snap.WaitingAtWarning)}");
            report.AppendLine(
                $"  Warning-progress total check: {Format0(snap.WaitingBelowHalfWarning + snap.WaitingHalfwayToWarning + snap.WaitingAtWarning)}");
            report.AppendLine(
                $"  Critical overlap ({DeathcareStatusSystem.kRepeatedDispatchFailureThreshold}+ failed attempts and at least halfway): " +
                $"{Format0(snap.RepeatedFailuresHalfwayToWarning)}");
            report.AppendLine(
                "  m_Timer is a small progress counter, not elapsed time. It stops at the");
            report.AppendLine(
                "  warning limit, so existing game data cannot provide an exact wait duration.");
            report.AppendLine(
                "  Faster game speed reaches the same simulation-time limit sooner in real time.");
        }

        private static void AppendCemeteries(
            StringBuilder report,
            World world,
            DeathcareStatusSystem.Snapshot snap)
        {
            report.AppendLine();
            report.AppendLine("CEMETERIES");
            report.AppendLine(
                $"  Graves used: {Format0(snap.CemeteryUse)} of {Format0(snap.CemeteryCapacity)}");

            CemeteryResetSystem resetSystem =
                world.GetOrCreateSystemManaged<CemeteryResetSystem>();
            report.AppendLine(
                $"  Resets this session: {resetSystem.SessionResetTotal}");
            report.AppendLine(
                $"  Cemeteries reset this session: {resetSystem.DistinctCemeteryCount}");

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
                report.AppendLine(
                    "  No corpse currently waiting outside for pickup.");
                if (snap.DeadInsideHearse > 0 || snap.DeadAtFacility > 0)
                {
                    report.AppendLine(
                        $"  {Format0(snap.DeadInsideHearse)} are already returning in hearses and " +
                        $"{Format0(snap.DeadAtFacility)} are already at a facility.");
                }

                return;
            }

            bool found = false;

            if (snap.FacilitiesWithoutRoomForBodies > 0)
            {
                found = true;
                report.AppendLine(
                    $"  - Processing/storage: {snap.FacilitiesWithoutRoomForBodies} active facilities");
                report.AppendLine(
                    "    have no room for another body, which blocks those facilities from");
                report.AppendLine(
                    "    accepting more hearse work. Raise processing/storage or add facilities.");
            }

            if (snap.DeadRetryCooldown > 0)
            {
                found = true;
                report.AppendLine(
                    $"  - Pickup trouble: {Format0(snap.DeadRetryCooldown)} requests are in retry cooldown.");
                report.AppendLine(
                    "    This can mean no eligible facility/hearse, no body storage room,");
                report.AppendLine(
                    "    a service-district restriction, or a road/path problem.");
                report.AppendLine(
                    "    More fleet capacity alone will not fix a failed pickup match.");
            }

            if (snap.RepeatedFailuresHalfwayToWarning > 0)
            {
                found = true;
                report.AppendLine(
                    $"  - Repeated dispatch trouble: {Format0(snap.RepeatedFailuresHalfwayToWarning)} corpses");
                report.AppendLine(
                    $"    have {DeathcareStatusSystem.kRepeatedDispatchFailureThreshold}+ failed attempts and are at least");
                report.AppendLine(
                    "    halfway to the hearse warning. Samples below list Scene Explorer IDs.");
            }

            if (snap.DeadNoRequest > 0)
            {
                found = true;
                report.AppendLine(
                    $"  - Request creation: {Format0(snap.DeadNoRequest)} waiting corpses have no valid");
                report.AppendLine(
                    "    hearse request yet. Let the city run briefly and repeat the report.");
                report.AppendLine(
                    "    Samples below list citizen Entity IDs for investigation.");
            }

            long beingMatched = snap.DeadWaitingForDispatch + snap.DeadPathfinding;
            if (beingMatched > 0)
            {
                found = true;
                report.AppendLine(
                    $"  - Matching now: {Format0(beingMatched)} requests are waiting for their dispatch");
                report.AppendLine(
                    "    update group or pathfinding. This is pipeline work, not proven failure.");
            }

            long assignedPickup = snap.DeadAssignedFacility + snap.DeadAssignedHearse;
            if (assignedPickup > 0)
            {
                found = true;
                report.AppendLine(
                    $"  - Pickup underway: {Format0(assignedPickup)} waiting corpses already have a");
                report.AppendLine(
                    "    facility or hearse assignment. Traffic and route distance affect these.");
            }

            if (snap.BudgetHearseCapacity < snap.Hearses)
            {
                found = true;
                report.AppendLine(
                    $"  - Budget/efficiency currently allows {Format0(snap.BudgetHearseCapacity)} of");
                report.AppendLine(
                    $"    {Format0(snap.Hearses)} configured on-road hearse slots.");
            }

            if (snap.BudgetHearseCapacity > 0 &&
                snap.WorkingHearses >= snap.BudgetHearseCapacity)
            {
                found = true;
                report.AppendLine(
                    "  - Fleet: all currently usable on-road hearse slots are occupied.");
                report.AppendLine(
                    "    More service budget/fleet capacity can help if valid requests remain.");
            }

            if (snap.ProcessingRate > 0f &&
                snap.DeathsPerMonth > snap.ProcessingRate)
            {
                found = true;
                report.AppendLine(
                    $"  - Long-term processing: deaths/month ({Format0(snap.DeathsPerMonth)})");
                report.AppendLine(
                    $"    exceed cremation max/month ({Format0(snap.ProcessingRate)}).");

                int suggestedPercent =
                    DeathcareStatus.GetSuggestedProcessingPercent(snap);
                if (suggestedPercent <= 500)
                {
                    report.AppendLine(
                        $"    Suggested now: about {suggestedPercent}% processing with the currently");
                    report.AppendLine(
                        $"    active facilities ({snap.ActiveFacilities} of {snap.TotalFacilities}).");
                }
                else
                {
                    report.AppendLine(
                        "    Suggested now: 500% processing plus more active crematorium capacity.");
                }
            }

            if (snap.FullFacilities > 0)
            {
                found = true;
                report.AppendLine(
                    $"  - Storage: {snap.FullFacilities} active deathcare facilities are flagged full.");
            }

            if (snap.CemeteryCapacity > 0 &&
                snap.CemeteryUse * 10 >= snap.CemeteryCapacity * 9)
            {
                found = true;
                report.AppendLine(
                    "  - Cemetery storage is at least 90% full.");
            }

            if (!found)
            {
                report.AppendLine(
                    "  No single capacity, request, or pathfinding problem is proven here.");
            }
        }

        private static string OnOff(bool value) => value ? "ON" : "OFF";

        private static string Format0(float value) =>
            ((long)Math.Round(value)).ToString("N0");
        private static string Format0(long value) => value.ToString("N0");
    }
}
