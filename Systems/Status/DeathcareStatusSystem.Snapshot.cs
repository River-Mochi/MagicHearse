// <copyright file="DeathcareStatusSystem.Snapshot.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Status/DeathcareStatusSystem.Snapshot.cs
// Purpose: Raw values returned by the on-demand deathcare scan.

namespace MagicHearse
{
    using System; // DateTime

    public sealed partial class DeathcareStatusSystem
    {
        public readonly struct Snapshot
        {
            public readonly float DeathsPerMonth;

            // Combined processing is the current crematorium plus cemetery contribution.
            public readonly float ProcessingRate;
            public readonly float CrematoriumProcessingRate;
            public readonly float CemeteryTurnoverRate;

            public readonly long Hearses;                // configured fleet slots
            public readonly long BudgetHearseCapacity;   // slots usable at current efficiency/budget

            // Spawned hearses split into exclusive parked and on-road buckets.
            public readonly long SpawnedHearses;         // parked + on-road entities
            public readonly long ParkedHearses;          // exact ParkedCar count
            public readonly long WorkingHearses;         // on-road entities
            public readonly long ParkedAvailableHearses;
            public readonly long ParkedDisabledHearses;
            public readonly long HearseDispatched;
            public readonly long HearseTransporting;
            public readonly long HearseReturning;
            public readonly long HearseOtherOnRoad;
            public readonly long HearseDisabledOnRoad;

            public readonly long CemeteryUse;
            public readonly long CemeteryCapacity;
            public readonly long MaxWorkers;

            public readonly int ActiveFacilities;
            public readonly int TotalFacilities;
            public readonly int FullFacilities;
            public readonly int FacilitiesWithoutAvailableHearse;
            public readonly int FacilitiesWithoutRoomForBodies;
            public readonly int FacilitiesWithProcessingQueue;
            public readonly int FacilitiesWithZeroDispatchCapacity;

            // Stage buckets total DeadRequiringTransport; DeadAssignedOutside overlaps them.
            public readonly long DeadRequiringTransport;
            public readonly long DeadWaiting;
            public readonly long DeadNoRequest;
            public readonly long DeadWaitingForDispatch;
            public readonly long DeadPathfinding;
            public readonly long DeadRetryCooldown;
            public readonly long DeadAssignedFacility;
            public readonly long DeadAssignedHearse;
            public readonly long DeadInsideHearse;
            public readonly long DeadAtFacility;
            public readonly long DeadOther;
            public readonly long DeadAssignedOutside;

            public readonly float TransportWarningTime;
            public readonly int TransportWarningTimerLimit;
            public readonly int MaxWaitingTimer;
            public readonly long WaitingBelowHalfWarning;
            public readonly long WaitingHalfwayToWarning;
            public readonly long WaitingAtWarning;
            public readonly int MaxDispatchFailCount;
            public readonly long WaitingWithFailedDispatches;
            public readonly long WaitingWithRepeatedDispatchFailures;
            public readonly long RepeatedFailuresHalfwayToWarning;

            public readonly DateTime SnapshotTimeLocal;

            public Snapshot(
                float deathsPerMonth,
                float processingRate,
                float crematoriumProcessingRate,
                float cemeteryTurnoverRate,
                long hearses,
                long budgetHearseCapacity,
                long spawnedHearses,
                long parkedHearses,
                long workingHearses,
                long parkedAvailableHearses,
                long parkedDisabledHearses,
                long hearseDispatched,
                long hearseTransporting,
                long hearseReturning,
                long hearseOtherOnRoad,
                long hearseDisabledOnRoad,
                long cemeteryUse,
                long cemeteryCapacity,
                long maxWorkers,
                int activeFacilities,
                int totalFacilities,
                int fullFacilities,
                int facilitiesWithoutAvailableHearse,
                int facilitiesWithoutRoomForBodies,
                int facilitiesWithProcessingQueue,
                int facilitiesWithZeroDispatchCapacity,
                long deadRequiringTransport,
                long deadWaiting,
                long deadNoRequest,
                long deadWaitingForDispatch,
                long deadPathfinding,
                long deadRetryCooldown,
                long deadAssignedFacility,
                long deadAssignedHearse,
                long deadInsideHearse,
                long deadAtFacility,
                long deadOther,
                long deadAssignedOutside,
                float transportWarningTime,
                int transportWarningTimerLimit,
                int maxWaitingTimer,
                long waitingBelowHalfWarning,
                long waitingHalfwayToWarning,
                long waitingAtWarning,
                int maxDispatchFailCount,
                long waitingWithFailedDispatches,
                long waitingWithRepeatedDispatchFailures,
                long repeatedFailuresHalfwayToWarning,
                DateTime snapshotTimeLocal)
            {
                DeathsPerMonth = deathsPerMonth;
                ProcessingRate = processingRate;
                CrematoriumProcessingRate = crematoriumProcessingRate;
                CemeteryTurnoverRate = cemeteryTurnoverRate;

                Hearses = hearses;
                BudgetHearseCapacity = budgetHearseCapacity;
                SpawnedHearses = spawnedHearses;
                ParkedHearses = parkedHearses;
                WorkingHearses = workingHearses;
                ParkedAvailableHearses = parkedAvailableHearses;
                ParkedDisabledHearses = parkedDisabledHearses;
                HearseDispatched = hearseDispatched;
                HearseTransporting = hearseTransporting;
                HearseReturning = hearseReturning;
                HearseOtherOnRoad = hearseOtherOnRoad;
                HearseDisabledOnRoad = hearseDisabledOnRoad;

                CemeteryUse = cemeteryUse;
                CemeteryCapacity = cemeteryCapacity;
                MaxWorkers = maxWorkers;

                ActiveFacilities = activeFacilities;
                TotalFacilities = totalFacilities;
                FullFacilities = fullFacilities;
                FacilitiesWithoutAvailableHearse = facilitiesWithoutAvailableHearse;
                FacilitiesWithoutRoomForBodies = facilitiesWithoutRoomForBodies;
                FacilitiesWithProcessingQueue = facilitiesWithProcessingQueue;
                FacilitiesWithZeroDispatchCapacity = facilitiesWithZeroDispatchCapacity;

                DeadRequiringTransport = deadRequiringTransport;
                DeadWaiting = deadWaiting;
                DeadNoRequest = deadNoRequest;
                DeadWaitingForDispatch = deadWaitingForDispatch;
                DeadPathfinding = deadPathfinding;
                DeadRetryCooldown = deadRetryCooldown;
                DeadAssignedFacility = deadAssignedFacility;
                DeadAssignedHearse = deadAssignedHearse;
                DeadInsideHearse = deadInsideHearse;
                DeadAtFacility = deadAtFacility;
                DeadOther = deadOther;
                DeadAssignedOutside = deadAssignedOutside;

                TransportWarningTime = transportWarningTime;
                TransportWarningTimerLimit = transportWarningTimerLimit;
                MaxWaitingTimer = maxWaitingTimer;
                WaitingBelowHalfWarning = waitingBelowHalfWarning;
                WaitingHalfwayToWarning = waitingHalfwayToWarning;
                WaitingAtWarning = waitingAtWarning;
                MaxDispatchFailCount = maxDispatchFailCount;
                WaitingWithFailedDispatches = waitingWithFailedDispatches;
                WaitingWithRepeatedDispatchFailures =
                    waitingWithRepeatedDispatchFailures;
                RepeatedFailuresHalfwayToWarning =
                    repeatedFailuresHalfwayToWarning;

                SnapshotTimeLocal = snapshotTimeLocal;
            }
        }
    }
}
