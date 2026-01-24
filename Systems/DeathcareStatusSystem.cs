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
    using Game.Buildings;                       // BuildingUtils, Building, ServiceDispatch, Efficiency
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

            m_DeathcarePlacedQuery = GetEntityQuery(
                ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(),
                ComponentType.ReadOnly<Building>(),
                ComponentType.ReadOnly<ServiceDispatch>(),
                ComponentType.ReadOnly<PrefabRef>(),
                ComponentType.Exclude<Temp>(),
                ComponentType.Exclude<Deleted>());

            m_DeadWaitingQuery = GetEntityQuery(
                ComponentType.ReadOnly<Citizen>(),
                ComponentType.ReadOnly<HealthProblem>(),
                ComponentType.Exclude<Temp>(),
                ComponentType.Exclude<Deleted>());
        }

        protected override void OnUpdate()
        {
            // On-demand only.
        }

        public Snapshot BuildSnapshot()
        {
            // Lookups
            ComponentLookup<PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<DeathcareFacilityData> dcLookup = GetComponentLookup<DeathcareFacilityData>(true);
            ComponentLookup<Game.Buildings.DeathcareFacility> buildingDcLookup = GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
            ComponentLookup<WorkProvider> workProviderLookup = GetComponentLookup<WorkProvider>(true);

            BufferLookup<InstalledUpgrade> upgradesLookup = GetBufferLookup<InstalledUpgrade>(true);
            BufferLookup<Efficiency> effLookup = GetBufferLookup<Efficiency>(true);

            // Monthly stats from game
            float deathsPerMonth = m_CityStats.GetStatisticValue(StatisticType.DeathRate);

            float processingRate = 0f;
            long hearses = 0;
            long cemeteryUse = 0;
            long cemeteryCapacity = 0;
            long maxWorkers = 0;

            int totalFacilities = 0;
            int activeFacilities = 0;

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

                    DeathcareFacilityData data = dcLookup.HasComponent(prefab) ? dcLookup[prefab] : default;

                    if (upgradesLookup.TryGetBuffer(e, out DynamicBuffer<InstalledUpgrade> upgrades) && upgrades.Length != 0)
                    {
                        UpgradeUtils.CombineStats(ref data, upgrades, ref prefabRefLookup, ref dcLookup);
                    }

                    // Skip non-facility entries (defensive)
                    if (data.m_ProcessingRate <= 0f && data.m_HearseCapacity <= 0 && data.m_StorageCapacity <= 0)
                    {
                        continue;
                    }

                    totalFacilities++;

                    float efficiency = 1f;
                    if (effLookup.TryGetBuffer(e, out DynamicBuffer<Efficiency> effBuf))
                    {
                        efficiency = BuildingUtils.GetEfficiency(effBuf);
                    }

                    if (efficiency <= 0f)
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
                            cemeteryUse += buildingDcLookup[e].m_LongTermStoredCount;
                        }

                        cemeteryCapacity += data.m_StorageCapacity;
                    }

                    if (workProviderLookup.HasComponent(e))
                    {
                        maxWorkers += workProviderLookup[e].m_MaxWorkers;
                    }
                }
            }

            // Dead waiting (Dead + RequireTransport)
            long deadWaiting = 0;
            const HealthProblemFlags Want = HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;

            ComponentTypeHandle<HealthProblem> hpType = GetComponentTypeHandle<HealthProblem>(isReadOnly: true);

            using (NativeArray<ArchetypeChunk> chunks = m_DeadWaitingQuery.ToArchetypeChunkArray(Allocator.Temp))
            {
                for (int c = 0; c < chunks.Length; c++)
                {
                    ArchetypeChunk chunk = chunks[c];
                    NativeArray<HealthProblem> hp = chunk.GetNativeArray(ref hpType);

                    for (int i = 0; i < hp.Length; i++)
                    {
                        if ((hp[i].m_Flags & Want) == Want)
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
