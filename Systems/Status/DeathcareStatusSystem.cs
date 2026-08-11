// <copyright file="DeathcareStatusSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Status/DeathcareStatusSystem.cs
// Purpose: Builds raw deathcare stats on demand for Status and Log Report.

namespace MagicHearse
{
    using System;                    // DateTime, Math
    using Game;                      // GameSystemBase
    using Game.Buildings;            // DeathcareFacility, BuildingUtils
    using Game.Citizens;             // HealthProblem
    using Game.City;                 // StatisticType (DeathRate)
    using Game.Common;               // Deleted, Owner
    using Game.Companies;            // WorkProvider, ServiceDispatch
    using Game.Notifications;        // IconElement
    using Game.Prefabs;              // PrefabRef, InstalledUpgrade
    using Game.Simulation;           // CityStatisticsSystem, ServiceRequest, Dispatched
    using Game.Tools;                // Temp
    using Game.Vehicles;             // Hearse, HearseFlags, ParkedCar
    using Unity.Collections;         // NativeArray, Allocator
    using Unity.Entities;            // EntityQuery, lookups, buffers, chunks

    public sealed partial class DeathcareStatusSystem : GameSystemBase
    {
        // Report-only threshold; vanilla dispatch behavior is unchanged.
        internal const int kRepeatedDispatchFailureThreshold = 4;
        private const double kSimulationFramesPerMinute = 3600d;
        private const uint kSimulationFramesPerHealthTimerTick = 256u;

        private CityStatisticsSystem m_CityStats = null!;
        private SimulationSystem m_SimulationSystem = null!;
        private EntityQuery m_DeathcarePlacedQuery;
        private EntityQuery m_DeadTransportQuery;
        private EntityQuery m_HealthcareSettingsQuery;
        private EntityQuery m_HearseQuery;

        public uint CurrentSimulationFrame => m_SimulationSystem.frameIndex;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_CityStats = World.GetOrCreateSystemManaged<CityStatisticsSystem>();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();

            m_DeathcarePlacedQuery = SystemAPI.QueryBuilder()
                .WithAll<
                    Game.Buildings.DeathcareFacility,
                    Game.Buildings.Building, ServiceDispatch,
                    Game.Prefabs.PrefabRef>()
                .WithNone<Temp, Deleted>()
                .Build();

            m_DeadTransportQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Citizens.Citizen, Game.Citizens.HealthProblem>()
                .WithNone<Temp, Deleted>()
                .Build();

            m_HealthcareSettingsQuery = SystemAPI.QueryBuilder()
                .WithAll<HealthcareParameterData>()
                .Build();

            m_HearseQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Vehicles.Hearse, Owner>()
                .WithNone<Temp, Deleted>()
                .Build();

            // Built only on demand from Options or Log Report; never on each simulation update.
            Enabled = false;
        }

        protected override void OnUpdate()
        {
            // Required by GameSystemBase; snapshots are built on demand.
        }

        public Snapshot BuildSnapshot()
        {
            // Finish jobs this system depends on before reading live data on the main thread.
            CompleteDependency();

            ComponentLookup<PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<DeathcareFacilityData> dcLookup =
                GetComponentLookup<DeathcareFacilityData>(true);
            ComponentLookup<Game.Buildings.DeathcareFacility> buildingDcLookup =
                GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
            ComponentLookup<WorkProvider> workProviderLookup =
                GetComponentLookup<WorkProvider>(true);
            ComponentLookup<MHWarningDelay> warningDelayLookup =
                GetComponentLookup<MHWarningDelay>(true);
            ComponentLookup<Deleted> deletedLookup =
                GetComponentLookup<Deleted>(true);

            BufferLookup<InstalledUpgrade> upgradesLookup =
                GetBufferLookup<InstalledUpgrade>(true);
            BufferLookup<Efficiency> efficiencyLookup = GetBufferLookup<Efficiency>(true);
            BufferLookup<Patient> patientLookup = GetBufferLookup<Patient>(true);
            BufferLookup<IconElement> iconElementsLookup =
                GetBufferLookup<IconElement>(true);

            float deathsPerMonth = m_CityStats.GetStatisticValue(StatisticType.DeathRate);
            HealthcareParameterData healthcareParameters =
                m_HealthcareSettingsQuery.GetSingleton<HealthcareParameterData>();
            float transportWarningTime = healthcareParameters.m_TransportWarningTime;
            Entity hearseNotificationPrefab =
                healthcareParameters.m_HearseNotificationPrefab;

            // HealthProblemSystem uses this exact conversion (60 frames/sec, each citizen every 256 frames).
            int transportWarningTimerLimit = (int)(transportWarningTime * (15f / 64f));

            float processingRate = 0f;
            float crematoriumProcessingRate = 0f;
            float cemeteryTurnoverRate = 0f;
            long hearses = 0;
            long budgetHearseCapacity = 0;
            long cemeteryUse = 0;
            long cemeteryCapacity = 0;
            long maxWorkers = 0;

            int totalFacilities = 0;
            int activeFacilities = 0;
            int activeCemeteryFacilities = 0;
            int fullFacilities = 0;
            int facilitiesWithoutAvailableHearse = 0;
            int facilitiesWithoutRoomForBodies = 0;
            int facilitiesWithProcessingQueue = 0;
            int facilitiesWithZeroDispatchCapacity = 0;

            using (NativeArray<Entity> entities =
                m_DeathcarePlacedQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity facilityEntity = entities[i];

                    if (!prefabRefLookup.TryGetComponent(
                            facilityEntity,
                            out PrefabRef prefabRef))
                    {
                        continue;
                    }

                    Entity prefab = prefabRef.m_Prefab;
                    DeathcareFacilityData data =
                        dcLookup.HasComponent(prefab) ? dcLookup[prefab] : default;

                    if (upgradesLookup.TryGetBuffer(
                            facilityEntity,
                            out DynamicBuffer<InstalledUpgrade> upgrades) &&
                        upgrades.Length != 0)
                    {
                        UpgradeUtils.CombineStats(
                            ref data,
                            upgrades,
                            ref prefabRefLookup,
                            ref dcLookup);
                    }

                    if (data.m_ProcessingRate <= 0f &&
                        data.m_HearseCapacity <= 0 &&
                        data.m_StorageCapacity <= 0)
                    {
                        continue;
                    }

                    totalFacilities++;

                    float efficiency = 1f;
                    float immediateEfficiency = 1f;
                    if (efficiencyLookup.TryGetBuffer(
                            facilityEntity,
                            out DynamicBuffer<Efficiency> efficiencies))
                    {
                        efficiency = BuildingUtils.GetEfficiency(efficiencies);
                        immediateEfficiency =
                            BuildingUtils.GetImmediateEfficiency(efficiencies);
                    }

                    // Disabled facilities count toward placed buildings, but not active capacity.
                    if (efficiency <= 0f)
                    {
                        continue;
                    }

                    activeFacilities++;
                    float activeProcessingRate =
                        efficiency * data.m_ProcessingRate;
                    processingRate += activeProcessingRate;

                    // Long-term storage marks cemeteries; the rest add crematorium capacity.
                    if (data.m_LongTermStorage)
                    {
                        activeCemeteryFacilities++;
                        cemeteryTurnoverRate += activeProcessingRate;
                    }
                    else
                    {
                        crematoriumProcessingRate += activeProcessingRate;
                    }

                    hearses += data.m_HearseCapacity;

                    int currentDispatchCapacity = BuildingUtils.GetVehicleCapacity(
                        Math.Min(efficiency, immediateEfficiency),
                        data.m_HearseCapacity);
                    budgetHearseCapacity += currentDispatchCapacity;

                    if (data.m_HearseCapacity > 0 && currentDispatchCapacity <= 0)
                    {
                        facilitiesWithZeroDispatchCapacity++;
                    }

                    Game.Buildings.DeathcareFacility facility =
                        buildingDcLookup.HasComponent(facilityEntity)
                            ? buildingDcLookup[facilityEntity]
                            : default;

                    if ((facility.m_Flags & DeathcareFacilityFlags.IsFull) != 0)
                    {
                        fullFacilities++;
                    }

                    if (data.m_HearseCapacity > 0 &&
                        (facility.m_Flags & DeathcareFacilityFlags.HasAvailableHearses) == 0)
                    {
                        facilitiesWithoutAvailableHearse++;
                    }

                    if ((facility.m_Flags & DeathcareFacilityFlags.HasRoomForBodies) == 0)
                    {
                        facilitiesWithoutRoomForBodies++;
                    }

                    bool hasProcessingQueue =
                        data.m_ProcessingRate > 0f &&
                        ((patientLookup.TryGetBuffer(
                                facilityEntity,
                                out DynamicBuffer<Patient> patients) &&
                            patients.Length > 0) ||
                         facility.m_LongTermStoredCount > 0);

                    if (hasProcessingQueue)
                    {
                        facilitiesWithProcessingQueue++;
                    }

                    if (data.m_LongTermStorage)
                    {
                        cemeteryUse += facility.m_LongTermStoredCount;
                        cemeteryCapacity += data.m_StorageCapacity;
                    }

                    if (workProviderLookup.HasComponent(facilityEntity))
                    {
                        maxWorkers += workProviderLookup[facilityEntity].m_MaxWorkers;
                    }
                }
            }

            ComponentLookup<Owner> ownerLookup = GetComponentLookup<Owner>(true);
            ComponentLookup<ParkedCar> parkedLookup = GetComponentLookup<ParkedCar>(true);
            ComponentLookup<Game.Buildings.DeathcareFacility> deathcareBuildingLookup =
                GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
            ComponentLookup<Game.Vehicles.Hearse> hearseLookup =
                GetComponentLookup<Game.Vehicles.Hearse>(true);

            long spawnedHearses = 0;
            long parkedHearses = 0;
            long workingHearses = 0;
            long parkedAvailableHearses = 0;
            long parkedDisabledHearses = 0;
            long hearseDispatched = 0;
            long hearseTransporting = 0;
            long hearseReturning = 0;
            long hearseOtherOnRoad = 0;
            long hearseDisabledOnRoad = 0;

            // Parked and on-road buckets are exclusive and must add up to SpawnedHearses.
            using (NativeArray<Entity> hearseEntities =
                m_HearseQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < hearseEntities.Length; i++)
                {
                    Entity hearseEntity = hearseEntities[i];

                    if (!ownerLookup.TryGetComponent(hearseEntity, out Owner owner) ||
                        !deathcareBuildingLookup.HasComponent(owner.m_Owner))
                    {
                        continue;
                    }

                    float ownerEfficiency = 1f;
                    if (efficiencyLookup.TryGetBuffer(
                            owner.m_Owner,
                            out DynamicBuffer<Efficiency> ownerEfficiencies))
                    {
                        ownerEfficiency = BuildingUtils.GetEfficiency(ownerEfficiencies);
                    }

                    if (ownerEfficiency <= 0f)
                    {
                        continue;
                    }

                    spawnedHearses++;
                    HearseFlags state = hearseLookup[hearseEntity].m_State;
                    bool isDisabled = (state & HearseFlags.Disabled) != 0;

                    if (parkedLookup.HasComponent(hearseEntity))
                    {
                        parkedHearses++;
                        if (isDisabled)
                        {
                            parkedDisabledHearses++;
                        }
                        else
                        {
                            parkedAvailableHearses++;
                        }

                        continue;
                    }

                    workingHearses++;

                    if (isDisabled)
                    {
                        hearseDisabledOnRoad++;
                    }
                    else if ((state & HearseFlags.Transporting) != 0)
                    {
                        hearseTransporting++;
                    }
                    else if ((state & HearseFlags.Dispatched) != 0)
                    {
                        hearseDispatched++;
                    }
                    else if ((state & HearseFlags.Returning) != 0)
                    {
                        hearseReturning++;
                    }
                    else
                    {
                        hearseOtherOnRoad++;
                    }
                }
            }

            CorpseLookups corpseLookups = CreateCorpseLookups();

            long deadRequiringTransport = 0;
            long deadWaiting = 0;
            long deadNoRequest = 0;
            long deadWaitingForDispatch = 0;
            long deadPathfinding = 0;
            long deadRetryCooldown = 0;
            long deadAssignedFacility = 0;
            long deadAssignedHearse = 0;
            long deadInsideHearse = 0;
            long deadAtFacility = 0;
            long deadOther = 0;
            long deadAssignedOutside = 0;

            int maxWaitingTimer = 0;
            long waitingBelowHalfWarning = 0;
            long waitingHalfwayToWarning = 0;
            long waitingAtWarning = 0;
            int maxDispatchFailCount = 0;
            long waitingWithFailedDispatches = 0;
            long waitingWithRepeatedDispatchFailures = 0;
            long repeatedFailuresHalfwayToWarning = 0;
            long waitingPastDue = 0;
            long warningTrackedWaiting = 0;
            long warningSuppressedWaiting = 0;
            double estimatedWaitMinutesTotal = 0d;
            double estimatedWaitMinutesMax = 0d;

            const HealthProblemFlags Want =
                HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;

            using (NativeArray<Entity> citizens =
                m_DeadTransportQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < citizens.Length; i++)
                {
                    Entity citizen = citizens[i];
                    if (!corpseLookups.HealthProblem.TryGetComponent(
                            citizen,
                            out HealthProblem healthProblem) ||
                        (healthProblem.m_Flags & Want) != Want)
                    {
                        continue;
                    }

                    deadRequiringTransport++;

                    CorpseStage stage = ClassifyCorpse(
                        citizen,
                        healthProblem,
                        in corpseLookups,
                        out bool outsideService);

                    switch (stage)
                    {
                        case CorpseStage.NoRequest:
                            deadNoRequest++;
                            break;
                        case CorpseStage.WaitingForDispatch:
                            deadWaitingForDispatch++;
                            break;
                        case CorpseStage.Pathfinding:
                            deadPathfinding++;
                            break;
                        case CorpseStage.RetryCooldown:
                            deadRetryCooldown++;
                            break;
                        case CorpseStage.AssignedFacility:
                            deadAssignedFacility++;
                            break;
                        case CorpseStage.AssignedHearse:
                            deadAssignedHearse++;
                            break;
                        case CorpseStage.InsideHearse:
                            deadInsideHearse++;
                            break;
                        case CorpseStage.AtFacility:
                            deadAtFacility++;
                            break;
                        default:
                            deadOther++;
                            break;
                    }

                    if (outsideService)
                    {
                        deadAssignedOutside++;
                    }

                    if (stage == CorpseStage.InsideHearse ||
                        stage == CorpseStage.AtFacility)
                    {
                        continue;
                    }

                    deadWaiting++;

                    if (iconElementsLookup.TryGetBuffer(
                            citizen,
                            out DynamicBuffer<IconElement> iconElements))
                    {
                        for (int iconIndex = 0; iconIndex < iconElements.Length; iconIndex++)
                        {
                            Entity iconEntity = iconElements[iconIndex].m_Icon;
                            if (iconEntity == Entity.Null ||
                                iconEntity.Index < 0 ||
                                deletedLookup.HasComponent(iconEntity) ||
                                !prefabRefLookup.TryGetComponent(
                                    iconEntity,
                                    out PrefabRef iconPrefabRef) ||
                                iconPrefabRef.m_Prefab != hearseNotificationPrefab)
                            {
                                continue;
                            }

                            waitingPastDue++;
                            break;
                        }
                    }

                    if (warningDelayLookup.TryGetComponent(
                            citizen,
                            out MHWarningDelay warningDelay))
                    {
                        warningTrackedWaiting++;
                        if (warningDelay.VanillaReached && !warningDelay.Completed)
                        {
                            warningSuppressedWaiting++;
                        }

                        uint estimatedStartFrame =
                            warningDelay.WaitEstimateInitialized
                                ? warningDelay.EstimatedWaitStartFrame
                                : unchecked(
                                    m_SimulationSystem.frameIndex -
                                    ((uint)healthProblem.m_Timer *
                                     kSimulationFramesPerHealthTimerTick));
                        uint elapsedFrames = unchecked(
                            m_SimulationSystem.frameIndex - estimatedStartFrame);
                        double estimatedMinutes =
                            elapsedFrames / kSimulationFramesPerMinute;
                        estimatedWaitMinutesTotal += estimatedMinutes;
                        estimatedWaitMinutesMax = Math.Max(
                            estimatedWaitMinutesMax,
                            estimatedMinutes);
                    }

                    int timer = healthProblem.m_Timer;
                    maxWaitingTimer = Math.Max(maxWaitingTimer, timer);
                    bool isHalfwayToWarning =
                        transportWarningTimerLimit > 0 &&
                        timer >= (transportWarningTimerLimit + 1) / 2;

                    Entity request = healthProblem.m_HealthcareRequest;
                    if (request != Entity.Null &&
                        corpseLookups.ServiceRequest.TryGetComponent(
                            request,
                            out ServiceRequest serviceRequest) &&
                        serviceRequest.m_FailCount > 0)
                    {
                        int failCount = serviceRequest.m_FailCount;
                        maxDispatchFailCount =
                            Math.Max(maxDispatchFailCount, failCount);
                        waitingWithFailedDispatches++;

                        if (failCount >= kRepeatedDispatchFailureThreshold)
                        {
                            waitingWithRepeatedDispatchFailures++;
                            if (isHalfwayToWarning)
                            {
                                repeatedFailuresHalfwayToWarning++;
                            }
                        }
                    }

                    if (transportWarningTimerLimit <= 0 ||
                        timer < (transportWarningTimerLimit + 1) / 2)
                    {
                        waitingBelowHalfWarning++;
                    }
                    else if (timer < transportWarningTimerLimit)
                    {
                        waitingHalfwayToWarning++;
                    }
                    else
                    {
                        waitingAtWarning++;
                    }
                }
            }

            return new Snapshot(
                deathsPerMonth: deathsPerMonth,
                processingRate: processingRate,
                crematoriumProcessingRate: crematoriumProcessingRate,
                cemeteryTurnoverRate: cemeteryTurnoverRate,
                hearses: hearses,
                budgetHearseCapacity: budgetHearseCapacity,
                spawnedHearses: spawnedHearses,
                parkedHearses: parkedHearses,
                workingHearses: workingHearses,
                parkedAvailableHearses: parkedAvailableHearses,
                parkedDisabledHearses: parkedDisabledHearses,
                hearseDispatched: hearseDispatched,
                hearseTransporting: hearseTransporting,
                hearseReturning: hearseReturning,
                hearseOtherOnRoad: hearseOtherOnRoad,
                hearseDisabledOnRoad: hearseDisabledOnRoad,
                cemeteryUse: cemeteryUse,
                cemeteryCapacity: cemeteryCapacity,
                maxWorkers: maxWorkers,
                activeFacilities: activeFacilities,
                activeCemeteryFacilities: activeCemeteryFacilities,
                totalFacilities: totalFacilities,
                fullFacilities: fullFacilities,
                facilitiesWithoutAvailableHearse: facilitiesWithoutAvailableHearse,
                facilitiesWithoutRoomForBodies: facilitiesWithoutRoomForBodies,
                facilitiesWithProcessingQueue: facilitiesWithProcessingQueue,
                facilitiesWithZeroDispatchCapacity: facilitiesWithZeroDispatchCapacity,
                deadRequiringTransport: deadRequiringTransport,
                deadWaiting: deadWaiting,
                deadNoRequest: deadNoRequest,
                deadWaitingForDispatch: deadWaitingForDispatch,
                deadPathfinding: deadPathfinding,
                deadRetryCooldown: deadRetryCooldown,
                deadAssignedFacility: deadAssignedFacility,
                deadAssignedHearse: deadAssignedHearse,
                deadInsideHearse: deadInsideHearse,
                deadAtFacility: deadAtFacility,
                deadOther: deadOther,
                deadAssignedOutside: deadAssignedOutside,
                transportWarningTime: transportWarningTime,
                transportWarningTimerLimit: transportWarningTimerLimit,
                maxWaitingTimer: maxWaitingTimer,
                waitingBelowHalfWarning: waitingBelowHalfWarning,
                waitingHalfwayToWarning: waitingHalfwayToWarning,
                waitingAtWarning: waitingAtWarning,
                maxDispatchFailCount: maxDispatchFailCount,
                waitingWithFailedDispatches: waitingWithFailedDispatches,
                waitingWithRepeatedDispatchFailures:
                    waitingWithRepeatedDispatchFailures,
                repeatedFailuresHalfwayToWarning:
                    repeatedFailuresHalfwayToWarning,
                waitingPastDue: waitingPastDue,
                warningTrackedWaiting: warningTrackedWaiting,
                warningSuppressedWaiting: warningSuppressedWaiting,
                estimatedAverageWaitMinutes:
                    warningTrackedWaiting > 0
                        ? estimatedWaitMinutesTotal / warningTrackedWaiting
                        : 0d,
                estimatedMaximumWaitMinutes: estimatedWaitMinutesMax,
                snapshotTimeLocal: DateTime.Now);
        }
    }
}
