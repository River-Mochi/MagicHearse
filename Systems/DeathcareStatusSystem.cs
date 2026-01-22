// File: Systems/DeathcareStatusSystem.cs
// Purpose: Computes the OptionsUI Status snapshot for Deathcare (no healthcare/patient data).
// Notes:
// - On-demand: called directly from DeathcareStatus.RefreshIfNeeded().
// - Uses placed buildings + upgrades + efficiency (CO logic style), but DOES NOT require Patient in queries.
// - Cemetery use/capacity counts ONLY long-term storage facilities.

namespace MagicHearse
{
    using Game;                                 // GameSystemBase
    using Game.Buildings;                       // BuildingUtils, Building, DeathcareFacility, ServiceDispatch, Efficiency
    using Game.City;                            // StatisticType
    using Game.Common;                          // Deleted, Temp
    using Game.Prefabs;                         // DeathcareFacilityData, PrefabRef, InstalledUpgrade, UpgradeUtils, PrefabData
    using Game.SceneFlow;                       // GameManager
    using Game.Simulation;                      // CityStatisticsSystem
    using Game.Tools;
    using System;
    using Unity.Collections;                    // Allocator
    using Unity.Entities;                       // Entity, EntityQuery, ComponentType
    using Unity.Mathematics;                    // math

    public sealed partial class DeathcareStatusSystem : GameSystemBase
    {
        private CityStatisticsSystem m_CityStats = null!;

        private EntityQuery m_DeathcarePlacedQuery;
        private EntityQuery m_DeathcarePrefabQuery;

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

            // Prefab query used only for "total hearses capacity" (prefab-level, optional).
            m_DeathcarePrefabQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<PrefabData>(),
                    ComponentType.ReadOnly<DeathcareFacilityData>(),
                },
            });
        }

        protected override void OnUpdate()
        {
            // No continuous work. This system is invoked on-demand from OptionsUI getters.
        }

        public void RefreshIfNeeded()
        {
            var gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                DeathcareStatus.LastRefreshUtc = FormatUtc(DateTime.UtcNow);
                DeathcareStatus.SummaryLine1 = "No city loaded yet.";
                DeathcareStatus.SummaryLine2 = string.Empty;
                return;
            }

            RefreshNow();
        }

        public void RefreshNow()
        {
            // Lookups/buffer lookups fetched from the System (not EntityManager).
            var prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            var dcLookup = GetComponentLookup<DeathcareFacilityData>(true);

            var upgradesLookup = GetBufferLookup<InstalledUpgrade>(true);
            var effLookup = GetBufferLookup<Efficiency>(true);

            // Deaths/mo.
            float deathsPerMonth = 0f;
            if (m_CityStats != null)
            {
                deathsPerMonth = m_CityStats.GetStatisticValue(StatisticType.DeathRate);
            }

            float processingRate = 0f;   // efficiency * processingRate
            float cemeteryUse = 0f;      // stored bodies in long-term storage
            float cemeteryCapacity = 0f; // long-term storage capacity (with upgrades)

            int facilities = 0;

            using (var entities = m_DeathcarePlacedQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity e = entities[i];

                    // Efficiency
                    float efficiency = 1f;
                    if (effLookup.TryGetBuffer(e, out var effBuf))
                    {
                        efficiency = BuildingUtils.GetEfficiency(effBuf);
                    }

                    if (efficiency == 0f)
                    {
                        continue;
                    }

                    // Prefab data
                    if (!prefabRefLookup.HasComponent(e))
                    {
                        continue;
                    }

                    Entity prefab = prefabRefLookup[e].m_Prefab;

                    DeathcareFacilityData data = default;
                    if (dcLookup.HasComponent(prefab))
                    {
                        data = dcLookup[prefab];
                    }

                    // Combine upgrades
                    if (upgradesLookup.TryGetBuffer(e, out var upgrades) && upgrades.Length != 0)
                    {
                        UpgradeUtils.CombineStats(ref data, upgrades, ref prefabRefLookup, ref dcLookup);
                    }

                    // Count as a facility if it has any deathcare stats.
                    if (data.m_ProcessingRate > 0f || data.m_HearseCapacity > 0 || data.m_StorageCapacity > 0)
                    {
                        facilities++;
                    }

                    // Cemetery use/capacity only for long-term storage facilities
                    if (data.m_LongTermStorage)
                    {
                        var b = EntityManager.GetComponentData<Game.Buildings.DeathcareFacility>(e);
                        cemeteryUse += b.m_LongTermStoredCount;
                        cemeteryCapacity += data.m_StorageCapacity;
                    }

                    // Total processing
                    processingRate += efficiency * data.m_ProcessingRate;
                }
            }

            long hearses = GetPrefabTotalHearses();

            DeathcareStatus.LastRefreshUtc = FormatUtc(DateTime.UtcNow);

            DeathcareStatus.SummaryLine1 =
                $"deaths: {Format0(deathsPerMonth)} | can handle: {Format0(processingRate)}";

            DeathcareStatus.SummaryLine2 =
                $"hearses: {Format0(hearses)} | facilities: {facilities} | cemetery: {Format0(cemeteryUse)} / {Format0(cemeteryCapacity)}";
        }

        private long GetPrefabTotalHearses()
        {
            long total = 0;

            using (var dcData = m_DeathcarePrefabQuery.ToComponentDataArray<DeathcareFacilityData>(Allocator.Temp))
            {
                for (int i = 0; i < dcData.Length; i++)
                {
                    total += dcData[i].m_HearseCapacity;
                }
            }

            return total;
        }

        private static string FormatUtc(DateTime utc)
        {
            // Time only; stable looking while you stare at it.
            return utc.ToString("HH:mm:s") + " UTC";
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
