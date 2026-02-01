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
        private void ApplyInstantWorkersToPlacedDeathcare(ref EntityCommandBuffer ecb)
        {
            // Lookups are read-only; writes happen via ECB to avoid direct structural writes during iteration.
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

            ComponentLookup<MHWorkProviderMarker> markerLookup =
                GetComponentLookup<MHWorkProviderMarker>(isReadOnly: true);

            int touched = 0;

            // This query can include building entities (and sometimes upgrade entities).
            // The worker cache lives on the *owner building entity* that has WorkProvider.
            using (NativeArray<Entity> entities = m_PlacedDeathcareQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity e = entities[i];

                    // Resolve to the entity that actually owns the building state (where WorkProvider lives).
                    Entity ownerEntity = e;
                    if (ownerLookup.HasComponent(e))
                    {
                        ownerEntity = ownerLookup[e].m_Owner;
                    }

                    // Skip dead entities.
                    if (deletedLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    // Requirement: never add WorkProvider; only mutate if it already exists.
                    if (!workProviderLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    // CityUtils indexes PrefabRef on the owner entity directly.
                    if (!prefabRefLookup.HasComponent(ownerEntity))
                    {
                        continue;
                    }

                    // Compute using the game's public helper so behavior stays aligned with vanilla.
                    int maxWorkers = CityUtils.GetCityServiceWorkplaceMaxWorkers(
                        ownerEntity,
                        ref prefabRefLookup,
                        ref upgradesLookup,
                        ref deletedLookup,
                        ref workplaceDataLookup,
                        ref schoolDataLookup,
                        ref studentLookup);

                    // If the game thinks this building has no workplaces, do not touch WorkProvider.
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

                    // Update the runtime cache.
                    if (providerNeedsUpdate)
                    {
                        WorkProvider updated = existing;
                        updated.m_MaxWorkers = maxWorkers;
                        ecb.SetComponent(ownerEntity, updated);
                    }

                    // Track what MH last wrote on the same entity that was mutated.
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

            ComponentLookup<MHWorkProviderMarker> markerLookup =
                GetComponentLookup<MHWorkProviderMarker>(isReadOnly: true);

            // Marked query targets the owner building entities that MH previously touched.
            using (NativeArray<Entity> entities = m_PlacedDeathcareMarkedQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity ownerEntity = entities[i];

                    // Always remove marker from invalid entities.
                    if (deletedLookup.HasComponent(ownerEntity) || !workProviderLookup.HasComponent(ownerEntity))
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

                    // Safety: if another system/mod changed the value since MH wrote it, do not overwrite.
                    if (current.m_MaxWorkers != marker.MaxWorkers)
                    {
                        ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                        continue;
                    }

                    // CityUtils requires PrefabRef on the owner entity.
                    if (!prefabRefLookup.HasComponent(ownerEntity))
                    {
                        ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
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

                    // Only apply if the computed value is meaningful and actually differs.
                    if (maxWorkers > 0 && maxWorkers != current.m_MaxWorkers)
                    {
                        WorkProvider updated = current;
                        updated.m_MaxWorkers = maxWorkers;
                        ecb.SetComponent(ownerEntity, updated);
                    }

                    // Marker is always removed on restore path.
                    ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                }
            }
        }
    }
}
