// File: Systems/FuneralDirectorSystem.Workers.cs
// Purpose: One-shot recompute of placed-building worker cache (WorkProvider.m_MaxWorkers) based on current prefab WorkplaceData + InstalledUpgrade.

namespace MagicHearse
{
    using Game.Buildings;       // InstalledUpgrade
    using Game.Common;            // Deleted, Owner
    using Game.Companies;        // WorkProvider
    using Game.Prefabs;          // PrefabRef, WorkplaceData
    using Unity.Collections;    // Allocator, NativeArray
    using Unity.Entities;       // Entity, EntityCommandBuffer, ComponentLookup, BufferLookup, DynamicBuffer

    public sealed partial class FuneralDirectorSystem
    {
        private void ApplyInstantWorkersToPlacedDeathcare(ref EntityCommandBuffer ecb)
        {
            ComponentLookup<Game.Common.Owner> ownerLookup = GetComponentLookup<Owner>(true);
            ComponentLookup<Game.Common.Deleted> deletedLookup = GetComponentLookup<Deleted>(true);

            ComponentLookup<Game.Companies.WorkProvider> workProviderLookup = GetComponentLookup<WorkProvider>(true);
            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<Game.Prefabs.WorkplaceData> workplaceDataLookup = GetComponentLookup<WorkplaceData>(true);
            BufferLookup<Game.Buildings.InstalledUpgrade> upgradesLookup = GetBufferLookup<InstalledUpgrade>(true);

            ComponentLookup<MHWorkProviderMarker> markerLookup = GetComponentLookup<MHWorkProviderMarker>(true);

            int touched = 0;

            using (NativeArray<Entity> entities = m_PlacedDeathcareQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity e = entities[i];

                    Entity ownerEntity = e;
                    if (ownerLookup.HasComponent(e))
                    {
                        ownerEntity = ownerLookup[e].m_Owner;
                    }

                    if (deletedLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    // Per requirement: never add WorkProvider, only mutate if it already exists.
                    if (!workProviderLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    if (!prefabRefLookup.HasComponent(e))
                    {
                        continue;
                    }

                    Entity prefabEntity = prefabRefLookup[e].m_Prefab;

                    int maxWorkers = ComputeCityServiceWorkplaceMaxWorkers(
                        ownerEntity,
                        prefabEntity,
                        ref prefabRefLookup,
                        ref upgradesLookup,
                        ref deletedLookup,
                        ref workplaceDataLookup);

                    if (maxWorkers <= 0)
                    {
                        continue;
                    }

                    WorkProvider existing = workProviderLookup[ownerEntity];

                    bool providerNeedsUpdate = existing.m_MaxWorkers != maxWorkers;

                    bool markerNeedsUpdate =
                        !markerLookup.HasComponent(ownerEntity) ||
                        markerLookup[ownerEntity].MaxWorkers != maxWorkers;

                    if (!providerNeedsUpdate && !markerNeedsUpdate)
                    {
                        continue;
                    }

                    if (providerNeedsUpdate)
                    {
                        WorkProvider updated = existing;
                        updated.m_MaxWorkers = maxWorkers;
                        ecb.SetComponent(ownerEntity, updated);
                    }

                    if (markerNeedsUpdate)
                    {
                        MHWorkProviderMarker marker = new MHWorkProviderMarker { MaxWorkers = maxWorkers };
                        if (markerLookup.HasComponent(ownerEntity))
                        {
                            ecb.SetComponent(ownerEntity, marker);
                        }
                        else
                        {
                            ecb.AddComponent(ownerEntity, marker);
                        }
                    }

                    touched++;
                }
            }

#if DEBUG
            if (touched > 0)
            {
                Mod.LogSafe(() => $"[FD] Instant workers updated {touched} placed deathcare buildings.");
            }
#endif
        }

        private void RestoreInstantWorkersOnPlacedDeathcare(ref EntityCommandBuffer ecb)
        {
            ComponentLookup<Game.Common.Deleted> deletedLookup = GetComponentLookup<Game.Common.Deleted>(true);

            ComponentLookup<Game.Companies.WorkProvider> workProviderLookup = GetComponentLookup<WorkProvider>(true);
            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<Game.Prefabs.WorkplaceData> workplaceDataLookup = GetComponentLookup<WorkplaceData>(true);
            BufferLookup<Game.Buildings.InstalledUpgrade> upgradesLookup = GetBufferLookup<InstalledUpgrade>(true);

            ComponentLookup<MHWorkProviderMarker> markerLookup = GetComponentLookup<MHWorkProviderMarker>(true);

            using (NativeArray<Entity> entities = m_PlacedDeathcareMarkedQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity ownerEntity = entities[i];

                    if (deletedLookup.HasComponent(ownerEntity))
                    {
                        ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                        continue;
                    }

                    if (!workProviderLookup.HasComponent(ownerEntity))
                    {
                        ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                        continue;
                    }

                    if (!markerLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    WorkProvider current = workProviderLookup[ownerEntity];
                    MHWorkProviderMarker marker = markerLookup[ownerEntity];

                    // If current value differs from what MH last wrote, another mod/system changed it.
                    // In that case, only remove MH marker and do not overwrite.
                    if (current.m_MaxWorkers != marker.MaxWorkers)
                    {
                        ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                        continue;
                    }

                    if (!prefabRefLookup.HasComponent(ownerEntity))
                    {
                        ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                        continue;
                    }

                    Entity prefabEntity = prefabRefLookup[ownerEntity].m_Prefab;

                    int maxWorkers = ComputeCityServiceWorkplaceMaxWorkers(
                        ownerEntity,
                        prefabEntity,
                        ref prefabRefLookup,
                        ref upgradesLookup,
                        ref deletedLookup,
                        ref workplaceDataLookup);

                    if (maxWorkers > 0 && maxWorkers != current.m_MaxWorkers)
                    {
                        WorkProvider updated = current;
                        updated.m_MaxWorkers = maxWorkers;
                        ecb.SetComponent(ownerEntity, updated);
                    }

                    ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                }
            }
        }

        private static int ComputeCityServiceWorkplaceMaxWorkers(
            Entity ownerEntity,
            Entity prefabEntity,
            ref ComponentLookup<Game.Prefabs.PrefabRef> prefabRefs,
            ref BufferLookup<Game.Buildings.InstalledUpgrade> installedUpgrades,
            ref ComponentLookup<Game.Common.Deleted> deleteds,
            ref ComponentLookup<Game.Prefabs.WorkplaceData> workplaceDatas)
        {
            if (deleteds.HasComponent(ownerEntity))
            {
                return 0;
            }

            if (!workplaceDatas.HasComponent(prefabEntity))
            {
                return 0;
            }

            int result = workplaceDatas[prefabEntity].m_MaxWorkers;

            if (!installedUpgrades.HasBuffer(ownerEntity))
            {
                return result;
            }

            int minWorkers = workplaceDatas[prefabEntity].m_MinimumWorkersLimit == 0
                ? result
                : workplaceDatas[prefabEntity].m_MinimumWorkersLimit;

            DynamicBuffer<InstalledUpgrade> upgrades = installedUpgrades[ownerEntity];

            for (int i = 0; i < upgrades.Length; i++)
            {
                Entity upgradeEntity = upgrades[i].m_Upgrade;

                if (!prefabRefs.HasComponent(upgradeEntity))
                {
                    continue;
                }

                if (deleteds.HasComponent(upgradeEntity))
                {
                    continue;
                }

                Entity upgradePrefab = prefabRefs[upgradeEntity].m_Prefab;

                if (!workplaceDatas.HasComponent(upgradePrefab))
                {
                    continue;
                }

                minWorkers += workplaceDatas[upgradePrefab].m_MinimumWorkersLimit;
                result += workplaceDatas[upgradePrefab].m_MaxWorkers;
            }

            _ = minWorkers;
            return result;
        }
    }
}
