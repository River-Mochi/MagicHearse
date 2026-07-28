// <copyright file="DeathcareStatusSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Status/DeathcareStatusSystem.cs
// Purpose: Builds raw deathcare stats on-demand for the Options Status.

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
    using Game.Simulation;      // CityStatisticsSystem
    using Game.Tools;           // Temp
    using Game.Vehicles;        // Hearse, ParkedCar
    using Unity.Collections;    // NativeArray, Allocator
    using Unity.Entities;       // Entity, EntityQuery, lookups, buffers, chunks

    public sealed partial class DeathcareStatusSystem : GameSystemBase
    {
        public readonly struct Snapshot
        {
            public readonly float DeathsPerMonth;
            public readonly float ProcessingRate;

            public readonly long Hearses;          // current total (slots)
            public readonly long WorkingHearses;   // non-parked vehicles

            public readonly long CemeteryUse;
            public readonly long CemeteryCapacity;
            public readonly long MaxWorkers;

            public readonly int ActiveFacilities;
            public readonly int TotalFacilities;

            public readonly long DeadWaiting;

            public readonly DateTime SnapshotTimeLocal;

            public Snapshot(
                float deathsPerMonth,
                float processingRate,
                long hearses,
                long workingHearses,
                long cemeteryUse,
                long cemeteryCapacity,
                long maxWorkers,
                int activeFacilities,
                int totalFacilities,
                long deadWaiting,
                DateTime snapshotTimeLocal)
            {
                DeathsPerMonth = deathsPerMonth;
                ProcessingRate = processingRate;

                Hearses = hearses;
                WorkingHearses = workingHearses;

                CemeteryUse = cemeteryUse;
                CemeteryCapacity = cemeteryCapacity;
                MaxWorkers = maxWorkers;

                ActiveFacilities = activeFacilities;
                TotalFacilities = totalFacilities;

                DeadWaiting = deadWaiting;
                SnapshotTimeLocal = snapshotTimeLocal;
            }
        }

        private CityStatisticsSystem m_CityStats = null!;
        private EntityQuery m_DeathcarePlacedQuery;
        private EntityQuery m_DeadWaitingQuery;
        private EntityQuery m_HearseQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_CityStats = World.GetOrCreateSystemManaged<CityStatisticsSystem>();

            m_DeathcarePlacedQuery = SystemAPI.QueryBuilder()
                .WithAll<
                    Game.Buildings.DeathcareFacility,
                    Game.Buildings.Building, ServiceDispatch,
                    Game.Prefabs.PrefabRef>()
                .WithNone<Game.Tools.Temp, Game.Common.Deleted>()
                .Build();

            m_DeadWaitingQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Citizens.Citizen, Game.Citizens.HealthProblem>()
                .WithNone<Game.Tools.Temp, Game.Common.Deleted>()
                .Build();

            // All hearse vehicles currently in the world.
            m_HearseQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Vehicles.Hearse, Game.Common.Owner>()
                .WithNone<Temp, Deleted>()
                .Build();
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

            float deathsPerMonth = m_CityStats.GetStatisticValue(StatisticType.DeathRate);

            float processingRate = 0f;
            long hearses = 0;
            long workingHearses = 0;
            long cemeteryUse = 0;
            long cemeteryCapacity = 0;
            long maxWorkers = 0;

            int totalFacilities = 0;
            int activeFacilities = 0;

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

                    if (data.m_LongTermStorage)
                    {
                        if (buildingDcLookup.HasComponent(facilityEntity))
                        {
                            cemeteryUse += buildingDcLookup[facilityEntity].m_LongTermStoredCount;
                        }

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

            using (NativeArray<Entity> hearseEntities = m_HearseQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < hearseEntities.Length; i++)
                {
                    Entity hearseEntity = hearseEntities[i];

                    if (parkedLookup.HasComponent(hearseEntity))
                    {
                        continue;
                    }

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

                    workingHearses++;
                }
            }

            // Count dead citizens who still require transport.
            long deadWaiting = 0;
            const HealthProblemFlags Want =
                HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;
            ComponentTypeHandle<HealthProblem> hpType =
                GetComponentTypeHandle<HealthProblem>(isReadOnly: true);

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
                        }
                    }
                }
            }

            return new Snapshot(
                deathsPerMonth: deathsPerMonth,
                processingRate: processingRate,
                hearses: hearses,
                workingHearses: workingHearses,
                cemeteryUse: cemeteryUse,
                cemeteryCapacity: cemeteryCapacity,
                maxWorkers: maxWorkers,
                activeFacilities: activeFacilities,
                totalFacilities: totalFacilities,
                deadWaiting: deadWaiting,
                snapshotTimeLocal: DateTime.Now);
        }
    }
}
