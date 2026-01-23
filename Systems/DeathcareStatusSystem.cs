// File: Systems/DeathcareStatusSystem.cs
// Purpose: ECS scanner for OptionsUI Status report.
// Notes:
// - On-demand: called from DeathcareStatus.RefreshIfNeeded().
// - “Active” means efficiency > 0 (exclude disabled/out of service).
// - Max workers uses WorkProvider.m_MaxWorkers (game-maintained runtime value).
// - Returns raw numbers (no localization / formatting here).

namespace MagicHearse
{
    using Game;                                 // GameSystemBase
    using Game.Buildings;                       // BuildingUtils, DeathcareFacility, ServiceDispatch, Efficiency
    using Game.Citizens;                        // Citizen, HealthProblem, HealthProblemFlags
    using Game.City;                            // StatisticType
    using Game.Common;                          // Deleted
    using Game.Companies;                       // WorkProvider
    using Game.Prefabs;                         // DeathcareFacilityData, PrefabRef, InstalledUpgrade, UpgradeUtils
    using Game.Simulation;                      // CityStatisticsSystem
    using Game.Tools;                           // Temp
    using System;                               // DateTime
    using Unity.Collections;                    // Allocator, NativeArray
    using Unity.Entities;                       // Entity, EntityQuery, ComponentType, ArchetypeChunk, ComponentTypeHandle

    public sealed partial class DeathcareStatusSystem : GameSystemBase
    {
        public readonly struct Snapshot
        {
            public readonly float DeathsPerMonth;
            public readonly float ProcessingRate;

            public readonly long Hearses;
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

        protected override void OnCreate()
        {
            base.OnCreate();

            m_CityStats = World.GetOrCreateSystemManaged<CityStatisticsSystem>();

            // Placed deathcare buildings only. No Patient requirement.
            m_DeathcarePlacedQuery = GetEntityQuery(
                ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(),
                ComponentType.ReadOnly<Building>(),
                ComponentType.ReadOnly<ServiceDispatch>(),
                ComponentType.ReadOnly<PrefabRef>(),
                ComponentType.Exclude<Temp>(),
                ComponentType.Exclude<Deleted>());

            // Citizens that have HealthProblem (not all citizens). Flags filtered in code.
            m_DeadWaitingQuery = GetEntityQuery(
                ComponentType.ReadOnly<Citizen>(),
                ComponentType.ReadOnly<HealthProblem>(),
                ComponentType.Exclude<Temp>(),
                ComponentType.Exclude<Deleted>());
        }

        protected override void OnUpdate()
        {
            // No continuous work. Invoked on-demand from OptionsUI getters.
        }

        public Snapshot BuildSnapshot()
        {
            // Lookups from the system.
            ComponentLookup<PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<DeathcareFacilityData> dcLookup = GetComponentLookup<DeathcareFacilityData>(true);
            ComponentLookup<Game.Buildings.DeathcareFacility> buildingDcLookup = GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
            ComponentLookup<WorkProvider> workProviderLookup = GetComponentLookup<WorkProvider>(true);

            BufferLookup<InstalledUpgrade> upgradesLookup = GetBufferLookup<InstalledUpgrade>(true);
            BufferLookup<Efficiency> effLookup = GetBufferLookup<Efficiency>(true);

            // Deaths/month from game stats (CityStatisticsSystem exists from OnCreate).
            float deathsPerMonth = m_CityStats.GetStatisticValue(StatisticType.DeathRate);

            float processingRate = 0f;      // ACTIVE: efficiency * processingRate
            long hearses = 0;               // ACTIVE: sum hearse capacity
            long cemeteryUse = 0;           // ACTIVE: stored bodies in long-term storage
            long cemeteryCapacity = 0;      // ACTIVE: long-term storage capacity
            long maxWorkers = 0;            // ACTIVE: sum WorkProvider.m_MaxWorkers

            int totalFacilities = 0;        // facilities regardless of disabled
            int activeFacilities = 0;       // efficiency > 0

            using (NativeArray<Entity> entities = m_DeathcarePlacedQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity e = entities[i];

                    if (!prefabRefLookup.HasComponent(e))
                    {
                        continue;
                    }

                    Entity prefab = prefabRefLookup[e].m_Prefab;

                    // Effective values (prefab stats + upgrades).
                    DeathcareFacilityData data = default;
                    if (dcLookup.HasComponent(prefab))
                    {
                        data = dcLookup[prefab];
                    }

                    if (upgradesLookup.TryGetBuffer(e, out DynamicBuffer<InstalledUpgrade> upgrades) && upgrades.Length != 0)
                    {
                        UpgradeUtils.CombineStats(ref data, upgrades, ref prefabRefLookup, ref dcLookup);
                    }

                    bool isFacility =
                        data.m_ProcessingRate > 0f || data.m_HearseCapacity > 0 || data.m_StorageCapacity > 0;

                    if (!isFacility)
                    {
                        continue;
                    }

                    totalFacilities++;

                    // Efficiency (disabled/out-of-service tends to be 0).
                    float efficiency = 1f;
                    if (effLookup.TryGetBuffer(e, out DynamicBuffer<Efficiency> effBuf))
                    {
                        efficiency = BuildingUtils.GetEfficiency(effBuf);
                    }

                    if (efficiency == 0f)
                    {
                        continue;
                    }

                    activeFacilities++;

                    processingRate += efficiency * data.m_ProcessingRate;
                    hearses += data.m_HearseCapacity;

                    if (data.m_LongTermStorage)
                    {
                        if (buildingDcLookup.HasComponent(e))
                        {
                            Game.Buildings.DeathcareFacility b = buildingDcLookup[e];
                            cemeteryUse += b.m_LongTermStoredCount;
                        }

                        cemeteryCapacity += data.m_StorageCapacity;
                    }

                    if (workProviderLookup.HasComponent(e))
                    {
                        maxWorkers += workProviderLookup[e].m_MaxWorkers;
                    }
                }
            }

            // Count dead citizens waiting for transport (Dead + RequireTransport).
            long deadWaiting = 0;
            ComponentTypeHandle<HealthProblem> hpType = GetComponentTypeHandle<HealthProblem>(isReadOnly: true);

            using (NativeArray<ArchetypeChunk> chunks = m_DeadWaitingQuery.ToArchetypeChunkArray(Allocator.Temp))
            {
                for (int c = 0; c < chunks.Length; c++)
                {
                    ArchetypeChunk chunk = chunks[c];
                    NativeArray<HealthProblem> hp = chunk.GetNativeArray(ref hpType);

                    for (int i = 0; i < hp.Length; i++)
                    {
                        HealthProblemFlags flags = hp[i].m_Flags;
                        bool isDeadAndWaiting =
                            (flags & (HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport)) ==
                            (HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport);

                        if (isDeadAndWaiting)
                        {
                            deadWaiting++;
                        }
                    }
                }
            }

            return new Snapshot(
                deathsPerMonth: deathsPerMonth, processingRate: processingRate,
                hearses: hearses, cemeteryUse: cemeteryUse, cemeteryCapacity: cemeteryCapacity, maxWorkers: maxWorkers,
                activeFacilities: activeFacilities, totalFacilities: totalFacilities,
                deadWaiting: deadWaiting,
                snapshotTimeLocal: DateTime.Now);
        }
    }
}
