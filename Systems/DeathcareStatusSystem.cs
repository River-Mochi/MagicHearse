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
    using Game.City;                            // StatisticType
    using Game.Common;                          // Deleted
    using Game.Companies;                       // WorkProvider
    using Game.Prefabs;                         // DeathcareFacilityData, PrefabRef, InstalledUpgrade, UpgradeUtils
    using Game.Simulation;                      // CityStatisticsSystem
    using Game.Tools;                           // Temp
    using System;
    using Unity.Collections;                    // Allocator
    using Unity.Entities;                       // Entity, EntityQuery, ComponentType
    using Unity.Mathematics;                    // math

    public sealed partial class DeathcareStatusSystem : GameSystemBase
    {
        private CityStatisticsSystem m_CityStats = null!;
        private EntityQuery m_DeathcarePlacedQuery;

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

            var refreshedUtc = FormatUtc(DateTime.UtcNow);
            DeathcareStatus.LastRefreshUtc = refreshedUtc;

            DeathcareStatus.SummaryLine1 =
                $"Deaths: {Format0(deathsPerMonth)} | Can handle: {Format0(processingRate)} | Updated: {refreshedUtc}";

            DeathcareStatus.SummaryLine2 =
                $"Hearses: {Format0(hearses)} | Buildings: {activeFacilities} / {totalFacilities} | Cemetery: {Format0(cemeteryUse)} / {Format0(cemeteryCapacity)} | Max workers: {Format0(maxWorkers)}";
        }

        private static string FormatUtc(DateTime utc)
        {
            return utc.ToString("HH:mm:ss") + " UTC";
        }

        private static string Format0(float v)
        {
            return ((long)math.round(v)).ToString("N0");
        }

        private static string Format0(long v)
        {
            return v.ToString("N0");
        }
    }
}
