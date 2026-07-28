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
    using System;               // DateTime
    using Game;                 // GameSystemBase
    using Game.Buildings;       // Building, DeathcareFacility, BuildingUtils
    using Game.Citizens;        // Citizen, HealthProblem, HealthProblemFlags
    using Game.City;            // StatisticType (DeathRate)
    using Game.Common;          // Deleted, Owner
    using Game.Companies;       // WorkProvider, ServiceDispatch
    using Game.Prefabs;         // PrefabRef, DeathcareFacilityData, InstalledUpgrade, UpgradeUtils
    using Game.Simulation;      // CityStatisticsSystem, Dispatched, SimulationSystem
    using Game.Tools;           // Temp
    using Game.Vehicles;        // Hearse, HearseFlags, ParkedCar
    using Unity.Collections;    // NativeArray, Allocator
    using Unity.Entities;       // Entity, EntityQuery, lookups, buffers, chunks

    public sealed partial class DeathcareStatusSystem : GameSystemBase
    {
        public readonly struct Snapshot
        {
            public readonly float DeathsPerMonth;
            public readonly float ProcessingRate;

            public readonly long Hearses;          // active fleet capacity slots
            public readonly long SpawnedHearses;   // entities owned by active in-city facilities
            public readonly long ParkedHearses;    // spawned entities with ParkedCar
            public readonly long WorkingHearses;   // spawned entities without ParkedCar

            public readonly long CemeteryUse;
            public readonly long CemeteryCapacity;
            public readonly long MaxWorkers;

            public readonly int ActiveFacilities;
            public readonly int TotalFacilities;

            public readonly long DeadWaiting;
            public readonly long DeadAssigned;
            public readonly long DeadUnassigned;
            public readonly long DeadAssignedOutside;

            public readonly long HearseIdle;
            public readonly long HearseDispatched;
            public readonly long HearseTransporting;
            public readonly long HearseReturning;
            public readonly long HearseDisabled;

            public readonly int FullFacilities;
            public readonly int FacilitiesWithoutAvailableHearse;
            public readonly int FacilitiesWithProcessingQueue;

            public readonly DateTime SnapshotTimeLocal;

            public Snapshot(
                float deathsPerMonth,
                float processingRate,
                long hearses,
                long spawnedHearses,
                long parkedHearses,
                long workingHearses,
                long cemeteryUse,
                long cemeteryCapacity,
                long maxWorkers,
                int activeFacilities,
                int totalFacilities,
                long deadWaiting,
                long deadAssigned,
                long deadUnassigned,
                long deadAssignedOutside,
                long hearseIdle,
                long hearseDispatched,
                long hearseTransporting,
                long hearseReturning,
                long hearseDisabled,
                int fullFacilities,
                int facilitiesWithoutAvailableHearse,
                int facilitiesWithProcessingQueue,
                DateTime snapshotTimeLocal)
            {
                DeathsPerMonth = deathsPerMonth;
                ProcessingRate = processingRate;

                Hearses = hearses;
                SpawnedHearses = spawnedHearses;
                ParkedHearses = parkedHearses;
                WorkingHearses = workingHearses;

                CemeteryUse = cemeteryUse;
                CemeteryCapacity = cemeteryCapacity;
                MaxWorkers = maxWorkers;

                ActiveFacilities = activeFacilities;
                TotalFacilities = totalFacilities;

                DeadWaiting = deadWaiting;
                DeadAssigned = deadAssigned;
                DeadUnassigned = deadUnassigned;
                DeadAssignedOutside = deadAssignedOutside;

                HearseIdle = hearseIdle;
                HearseDispatched = hearseDispatched;
                HearseTransporting = hearseTransporting;
                HearseReturning = hearseReturning;
                HearseDisabled = hearseDisabled;

                FullFacilities = fullFacilities;
                FacilitiesWithoutAvailableHearse = facilitiesWithoutAvailableHearse;
                FacilitiesWithProcessingQueue = facilitiesWithProcessingQueue;

                SnapshotTimeLocal = snapshotTimeLocal;
            }
        }

        private CityStatisticsSystem m_CityStats = null!;
        private SimulationSystem m_SimulationSystem = null!;
        private EntityQuery m_DeathcarePlacedQuery;
        private EntityQuery m_DeadWaitingQuery;
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

            m_DeadWaitingQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Citizens.Citizen, Game.Citizens.HealthProblem>()
                .WithNone<Temp, Deleted>()
                .Build();

            // All hearse vehicles currently in the world.
            m_HearseQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Vehicles.Hearse, Owner>()
                .WithNone<Temp, Deleted>()
                .Build();

            // Options calls BuildSnapshot() directly; no live simulation update is needed.
            Enabled = false;
        }

        protected override void OnUpdate()
        {
            // GameSystemBase requires this override; BuildSnapshot() does the work on demand.
        }

        public Snapshot BuildSnapshot()
        {
            // Finish jobs this system depends on before reading live data on the main thread.
            CompleteDependency();

            // These read-only lookups reach from placed buildings to prefab and upgrade data.
            ComponentLookup<PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<DeathcareFacilityData> dcLookup = GetComponentLookup<DeathcareFacilityData>(true);
            ComponentLookup<Game.Buildings.DeathcareFacility> buildingDcLookup =
                GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
            ComponentLookup<WorkProvider> workProviderLookup = GetComponentLookup<WorkProvider>(true);

            BufferLookup<InstalledUpgrade> upgradesLookup = GetBufferLookup<InstalledUpgrade>(true);
            BufferLookup<Efficiency> effLookup = GetBufferLookup<Efficiency>(true);
            BufferLookup<Patient> patientLookup = GetBufferLookup<Patient>(true);

            float deathsPerMonth = m_CityStats.GetStatisticValue(StatisticType.DeathRate);

            float processingRate = 0f;
            long hearses = 0;
            long spawnedHearses = 0;
            long parkedHearses = 0;
            long workingHearses = 0;
            long cemeteryUse = 0;
            long cemeteryCapacity = 0;
            long maxWorkers = 0;

            int totalFacilities = 0;
            int activeFacilities = 0;
            int fullFacilities = 0;
            int facilitiesWithoutAvailableHearse = 0;
            int facilitiesWithProcessingQueue = 0;

            // Add capacity and worker totals from every placed deathcare facility.
            using (NativeArray<Entity> entities = m_DeathcarePlacedQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity facilityEntity = entities[i];

                    if (!prefabRefLookup.HasComponent(facilityEntity))
                    {
                        continue;
                    }

                    Entity prefab = prefabRefLookup[facilityEntity].m_Prefab;

                    DeathcareFacilityData data = dcLookup.HasComponent(prefab) ? dcLookup[prefab] : default;

                    if (upgradesLookup.TryGetBuffer(
                            facilityEntity,
                            out DynamicBuffer<InstalledUpgrade> upgrades) &&
                        upgrades.Length != 0)
                    {
                        UpgradeUtils.CombineStats(ref data, upgrades, ref prefabRefLookup, ref dcLookup);
                    }

                    if (data.m_ProcessingRate <= 0f &&
                        data.m_HearseCapacity <= 0 &&
                        data.m_StorageCapacity <= 0)
                    {
                        continue;
                    }

                    totalFacilities++;

                    float efficiency = 1f;
                    if (effLookup.TryGetBuffer(facilityEntity, out DynamicBuffer<Efficiency> effBuf))
                    {
                        efficiency = BuildingUtils.GetEfficiency(effBuf);
                    }

                    // Disabled facilities count toward total buildings, but not active capacity.
                    if (efficiency <= 0f)
                    {
                        continue;
                    }

                    activeFacilities++;
                    processingRate += efficiency * data.m_ProcessingRate;
                    hearses += data.m_HearseCapacity;

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

            // A working hearse is not parked and belongs to an active deathcare building.
            ComponentLookup<Owner> ownerLookup = GetComponentLookup<Owner>(true);
            ComponentLookup<ParkedCar> parkedLookup = GetComponentLookup<ParkedCar>(true);
            ComponentLookup<Game.Buildings.DeathcareFacility> deathcareBuildingLookup =
                GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
            ComponentLookup<Game.Vehicles.Hearse> hearseLookup =
                GetComponentLookup<Game.Vehicles.Hearse>(true);

            long hearseIdle = 0;
            long hearseDispatched = 0;
            long hearseTransporting = 0;
            long hearseReturning = 0;
            long hearseDisabled = 0;

            using (NativeArray<Entity> hearseEntities = m_HearseQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < hearseEntities.Length; i++)
                {
                    Entity hearseEntity = hearseEntities[i];

                    if (!ownerLookup.HasComponent(hearseEntity))
                    {
                        continue;
                    }

                    Entity owner = ownerLookup[hearseEntity].m_Owner;

                    if (!deathcareBuildingLookup.HasComponent(owner))
                    {
                        continue;
                    }

                    float ownerEfficiency = 1f;
                    if (effLookup.TryGetBuffer(owner, out DynamicBuffer<Efficiency> ownerEfficiencies))
                    {
                        ownerEfficiency = BuildingUtils.GetEfficiency(ownerEfficiencies);
                    }

                    if (ownerEfficiency <= 0f)
                    {
                        continue;
                    }

                    spawnedHearses++;

                    HearseFlags state = hearseLookup[hearseEntity].m_State;

                    if ((state & HearseFlags.Disabled) != 0)
                    {
                        hearseDisabled++;
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
                        hearseIdle++;
                    }

                    if (parkedLookup.HasComponent(hearseEntity))
                    {
                        parkedHearses++;
                    }
                    else
                    {
                        workingHearses++;
                    }
                }
            }

            // Count dead citizens who still require transport.
            long deadWaiting = 0;
            long deadAssigned = 0;
            long deadUnassigned = 0;
            long deadAssignedOutside = 0;
            const HealthProblemFlags Want =
                HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;
            ComponentTypeHandle<HealthProblem> hpType =
                GetComponentTypeHandle<HealthProblem>(isReadOnly: true);
            ComponentLookup<Dispatched> dispatchedLookup = GetComponentLookup<Dispatched>(true);
            ComponentLookup<Game.Objects.OutsideConnection> outsideConnectionLookup =
                GetComponentLookup<Game.Objects.OutsideConnection>(true);
            EntityStorageInfoLookup entityLookup = GetEntityStorageInfoLookup();

            using (NativeArray<ArchetypeChunk> chunks =
                m_DeadWaitingQuery.ToArchetypeChunkArray(Allocator.Temp))
            {
                for (int c = 0; c < chunks.Length; c++)
                {
                    ArchetypeChunk chunk = chunks[c];
                    NativeArray<HealthProblem> hp = chunk.GetNativeArray(ref hpType);

                    for (int j = 0; j < hp.Length; j++)
                    {
                        if ((hp[j].m_Flags & Want) == Want)
                        {
                            deadWaiting++;

                            Entity request = hp[j].m_HealthcareRequest;
                            if (request == Entity.Null ||
                                !entityLookup.Exists(request) ||
                                !dispatchedLookup.TryGetComponent(request, out Dispatched dispatched) ||
                                dispatched.m_Handler == Entity.Null ||
                                !entityLookup.Exists(dispatched.m_Handler))
                            {
                                deadUnassigned++;
                                continue;
                            }

                            deadAssigned++;
                            if (IsOutsideHandler(
                                    dispatched.m_Handler,
                                    entityLookup,
                                    outsideConnectionLookup,
                                    ownerLookup))
                            {
                                deadAssignedOutside++;
                            }
                        }
                    }
                }
            }

            return new Snapshot(
                deathsPerMonth: deathsPerMonth,
                processingRate: processingRate,
                hearses: hearses,
                spawnedHearses: spawnedHearses,
                parkedHearses: parkedHearses,
                workingHearses: workingHearses,
                cemeteryUse: cemeteryUse,
                cemeteryCapacity: cemeteryCapacity,
                maxWorkers: maxWorkers,
                activeFacilities: activeFacilities,
                totalFacilities: totalFacilities,
                deadWaiting: deadWaiting,
                deadAssigned: deadAssigned,
                deadUnassigned: deadUnassigned,
                deadAssignedOutside: deadAssignedOutside,
                hearseIdle: hearseIdle,
                hearseDispatched: hearseDispatched,
                hearseTransporting: hearseTransporting,
                hearseReturning: hearseReturning,
                hearseDisabled: hearseDisabled,
                fullFacilities: fullFacilities,
                facilitiesWithoutAvailableHearse: facilitiesWithoutAvailableHearse,
                facilitiesWithProcessingQueue: facilitiesWithProcessingQueue,
                snapshotTimeLocal: DateTime.Now);
        }

        // Outside-service dispatch can point to the connection itself or to one of its hearses.
        private static bool IsOutsideHandler(
            Entity handler,
            EntityStorageInfoLookup entityLookup,
            ComponentLookup<Game.Objects.OutsideConnection> outsideConnectionLookup,
            ComponentLookup<Owner> ownerLookup)
        {
            if (outsideConnectionLookup.HasComponent(handler))
            {
                return true;
            }

            if (!ownerLookup.TryGetComponent(handler, out Owner owner) ||
                !entityLookup.Exists(owner.m_Owner))
            {
                return false;
            }

            return outsideConnectionLookup.HasComponent(owner.m_Owner);
        }
    }
}
