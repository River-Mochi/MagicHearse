// File: Systems/FuneralDirectorSystem.Workers.cs
// Purpose: One-shot recompute of placed-building worker cache (WorkProvider.m_MaxWorkers)
//          based on current prefab WorkplaceData + InstalledUpgrade, using the game's own CityUtils logic.

namespace MagicHearse
{
    using Game.Buildings;                // InstalledUpgrade, Student
    using Game.City;                     // CityUtils
    using Game.Common;                   // Deleted, Owner
    using Game.Companies;                // WorkProvider
    using Game.Prefabs;                  // PrefabRef, WorkplaceData, SchoolData
    using Unity.Collections;             // Allocator, NativeArray
    using Unity.Entities;                // Entity, EntityCommandBuffer, ComponentLookup, BufferLookup

    public sealed partial class FuneralDirectorSystem
    {
        private void RefreshPlacedDeathcareWorkers(ref EntityCommandBuffer ecb)
        {
            // Lookups are read-only; writes happen via ECB.
            ComponentLookup<Game.Common.Owner> ownerLookup =
                GetComponentLookup<Game.Common.Owner>(isReadOnly: true);

            ComponentLookup<Game.Common.Deleted> deletedLookup =
                GetComponentLookup<Game.Common.Deleted>(isReadOnly: true);

            ComponentLookup<Game.Companies.WorkProvider> workProviderLookup =
                GetComponentLookup<Game.Companies.WorkProvider>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup =
                GetComponentLookup<Game.Prefabs.PrefabRef>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.WorkplaceData> workplaceDataLookup =
                GetComponentLookup<Game.Prefabs.WorkplaceData>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.SchoolData> schoolDataLookup =
                GetComponentLookup<Game.Prefabs.SchoolData>(isReadOnly: true);

            BufferLookup<Game.Buildings.Student> studentLookup =
                GetBufferLookup<Game.Buildings.Student>(isReadOnly: true);

            BufferLookup<Game.Buildings.InstalledUpgrade> upgradesLookup =
                GetBufferLookup<Game.Buildings.InstalledUpgrade>(isReadOnly: true);

            ComponentLookup<WorkProviderMaxMark> markerLookup =
                GetComponentLookup<WorkProviderMaxMark>(isReadOnly: true);

            int touched = 0;

            // Query includes building entities (and sometimes upgrade entities).
            // Worker cache lives on the owner building entity that has WorkProvider.
            using (NativeArray<Entity> entities = m_PlacedDeathcareQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity e = entities[i];

                    // Resolve to the entity that owns building state (where WorkProvider lives).
                    Entity ownerEntity = e;
                    if (ownerLookup.HasComponent(e))
                    {
                        ownerEntity = ownerLookup[e].m_Owner;
                    }

                    if (deletedLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    // Requirement: never add WorkProvider; only mutate if it already exists.
                    if (!workProviderLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    // CityUtils indexes PrefabRef on the owner entity.
                    if (!prefabRefLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    int maxWorkers = CityUtils.GetCityServiceWorkplaceMaxWorkers(
                        ownerEntity,
                        ref prefabRefLookup,
                        ref upgradesLookup,
                        ref deletedLookup,
                        ref workplaceDataLookup,
                        ref schoolDataLookup,
                        ref studentLookup);

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
                        WorkProviderMaxMark marker = new WorkProviderMaxMark { MaxWorkers = maxWorkers };

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
                Mod.LogSafe(() => $"[FD] Placed workers refreshed {touched} deathcare buildings.");
            }
#endif
        }

        private void RestorePlacedDeathcareWorkers(ref EntityCommandBuffer ecb)
        {
            ComponentLookup<Game.Common.Deleted> deletedLookup =
                GetComponentLookup<Game.Common.Deleted>(isReadOnly: true);

            ComponentLookup<Game.Companies.WorkProvider> workProviderLookup =
                GetComponentLookup<Game.Companies.WorkProvider>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup =
                GetComponentLookup<Game.Prefabs.PrefabRef>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.WorkplaceData> workplaceDataLookup =
                GetComponentLookup<Game.Prefabs.WorkplaceData>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.SchoolData> schoolDataLookup =
                GetComponentLookup<Game.Prefabs.SchoolData>(isReadOnly: true);

            BufferLookup<Game.Buildings.Student> studentLookup =
                GetBufferLookup<Game.Buildings.Student>(isReadOnly: true);

            BufferLookup<Game.Buildings.InstalledUpgrade> upgradesLookup =
                GetBufferLookup<Game.Buildings.InstalledUpgrade>(isReadOnly: true);

            ComponentLookup<WorkProviderMaxMark> markerLookup =
                GetComponentLookup<WorkProviderMaxMark>(isReadOnly: true);

            // Marked query targets owner building entities previously touched.
            using (NativeArray<Entity> entities = m_PlacedDeathcareMarkedQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity ownerEntity = entities[i];

                    if (deletedLookup.HasComponent(ownerEntity) || !workProviderLookup.HasComponent(ownerEntity))
                    {
                        ecb.RemoveComponent<WorkProviderMaxMark>(ownerEntity);
                        continue;
                    }

                    if (!markerLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    WorkProvider current = workProviderLookup[ownerEntity];
                    WorkProviderMaxMark marker = markerLookup[ownerEntity];

                    // Safety: if another system/mod changed the value since last write, do not overwrite.
                    if (current.m_MaxWorkers != marker.MaxWorkers)
                    {
                        ecb.RemoveComponent<WorkProviderMaxMark>(ownerEntity);
                        continue;
                    }

                    if (!prefabRefLookup.HasComponent(ownerEntity))
                    {
                        ecb.RemoveComponent<WorkProviderMaxMark>(ownerEntity);
                        continue;
                    }

                    int maxWorkers = CityUtils.GetCityServiceWorkplaceMaxWorkers(
                        ownerEntity,
                        ref prefabRefLookup,
                        ref upgradesLookup,
                        ref deletedLookup,
                        ref workplaceDataLookup,
                        ref schoolDataLookup,
                        ref studentLookup);

                    if (maxWorkers > 0 && maxWorkers != current.m_MaxWorkers)
                    {
                        WorkProvider updated = current;
                        updated.m_MaxWorkers = maxWorkers;
                        ecb.SetComponent(ownerEntity, updated);
                    }

                    ecb.RemoveComponent<WorkProviderMaxMark>(ownerEntity);
                }
            }
        }
    }
}
