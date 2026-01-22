// File: Systems/DeathcareStatusSystem.cs
// Purpose: Computes the OptionsUI Status snapshot for Deathcare.
// Notes:
// - On-demand: called from DeathcareStatus.RefreshIfNeeded().
// - “Active” means efficiency > 0 (disabled/out of service excluded).
// - Shows buildings active/total.
// - Max workers uses WorkProvider.m_MaxWorkers (game-maintained runtime value).

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
    using System;
    using Unity.Collections;                    // Allocator, NativeArray
    using Unity.Entities;                       // Entity, EntityQuery, ComponentType, ArchetypeChunk, ComponentTypeHandle
    using Unity.IO.LowLevel.Unsafe;
    using Unity.Mathematics;                    // math

    public sealed partial class DeathcareStatusSystem : GameSystemBase
    {
        private CityStatisticsSystem m_CityStats = null!;
        private EntityQuery m_DeathcarePlacedQuery;
        private EntityQuery m_DeadWaitingQuery;

        // Custom keys (add to all Locales)
        private const string kLine2Key = "MH_STATUS_LINE2";
        private const string kLine3Key = "MH_STATUS_LINE3";

        protected override void OnCreate()
        {
            base.OnCreate();

            m_CityStats = World.GetOrCreateSystemManaged<CityStatisticsSystem>();

            // Placed deathcare buildings only. NO Patient requirement.
            m_DeathcarePlacedQuery = GetEntityQuery(
                ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(),
                ComponentType.ReadOnly<Building>(),
                ComponentType.ReadOnly<ServiceDispatch>(),
                ComponentType.ReadOnly<PrefabRef>(),
                ComponentType.Exclude<Temp>(),
                ComponentType.Exclude<Deleted>());

            // Citizens that *have* HealthProblem (not all citizens). We'll filter flags in code.
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

        public void RefreshNow()
        {
            // Lookups from the System.
            ComponentLookup<PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<DeathcareFacilityData> dcLookup = GetComponentLookup<DeathcareFacilityData>(true);
            ComponentLookup<Game.Buildings.DeathcareFacility> buildingDcLookup = GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
            ComponentLookup<WorkProvider> workProviderLookup = GetComponentLookup<WorkProvider>(true);

            BufferLookup<InstalledUpgrade> upgradesLookup = GetBufferLookup<InstalledUpgrade>(true);
            BufferLookup<Efficiency> effLookup = GetBufferLookup<Efficiency>(true);

            // Deaths/mo. (from game stats)
            float deathsPerMonth = 0f;
            if (m_CityStats != null)
            {
                deathsPerMonth = m_CityStats.GetStatisticValue(StatisticType.DeathRate);
            }

            float processingRate = 0f;      // ACTIVE: efficiency * processingRate
            long hearses = 0;               // ACTIVE: sum of hearse capacity (with upgrades / FD edits)
            long cemeteryUse = 0;           // ACTIVE: stored bodies in long-term storage
            long cemeteryCapacity = 0;      // ACTIVE: long-term storage capacity (with upgrades / FD edits)
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

                    // Current effective values (not vanilla baseline)
                    DeathcareFacilityData data = default;
                    if (dcLookup.HasComponent(prefab))
                    {
                        data = dcLookup[prefab];
                    }

                    // Combine upgrades on the placed building
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

                    // Efficiency (disabled/out-of-service tends to be 0)
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

                    // Status totals
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

            var refreshedUtc = FormatUtc(DateTime.UtcNow);


            DeathcareStatus.SummaryLine1 =
                $"{Format0(hearses)} hearses | {activeFacilities} / {totalFacilities} buildings | {Format0(cemeteryUse)} / {Format0(cemeteryCapacity)} cemetery use | {Format0(maxWorkers)} max workers";

            DeathcareStatus.SummaryLine2 = string.Format(
                T(kLine2Key, "{0} deaths | {1} can be handled"),
                Format0(deathsPerMonth), Format0(processingRate));

            DeathcareStatus.SummaryLine3 = string.Format(
                T(kLine3Key, "{0} dead | {1} updated"),
                Format0(deadWaiting), refreshedUtc);

        }


        // HELPERS
        private static string FormatUtc(DateTime utc)
        {
            return utc.ToString("HH:mm:ss");
        }

        private static string Format0(float v)
        {
            return ((long)math.round(v)).ToString("N0");
        }

        private static string Format0(long v)
        {
            return v.ToString("N0");
        }

 
        private static string T(string entryId, string englishFallback)
        {
            var lm = Game.SceneFlow.GameManager.instance?.localizationManager;
            var dict = lm?.activeDictionary;

            if (dict != null && dict.TryGetValue(entryId, out string value) && !string.IsNullOrEmpty(value))
            {
                return value;
            }

            return englishFallback;

        }

    }
}
