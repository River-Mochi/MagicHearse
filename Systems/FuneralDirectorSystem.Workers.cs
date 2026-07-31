// <copyright file="FuneralDirectorSystem.Workers.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/FuneralDirectorSystem.Workers.cs
// Purpose: Recomputes placed-building WorkProvider.m_MaxWorkers once using current
//          prefab/upgrade data and the game's public CityUtils calculation.

namespace MagicHearse
{
    using Game.Buildings;       // InstalledUpgrade, Student
    using Game.City;            // CityUtils
    using Game.Common;          // Deleted, Owner
    using Game.Companies;       // WorkProvider
    using Game.Prefabs;         // PrefabRef, WorkplaceData
    using Unity.Collections;    // Allocator, NativeArray
    using Unity.Entities;       // EntityCommandBuffer, ComponentLookup, BufferLookup

    public sealed partial class FuneralDirectorSystem
    {
        private void RefreshPlacedDeathcareWorkers(ref EntityCommandBuffer ecb)
        {
            ComponentLookup<Game.Common.Owner> ownerLookup =
                GetComponentLookup<Owner>(isReadOnly: true);

            ComponentLookup<Game.Common.Deleted> deletedLookup =
                GetComponentLookup<Deleted>(isReadOnly: true);

            ComponentLookup<Game.Companies.WorkProvider> workProviderLookup =
                GetComponentLookup<WorkProvider>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup =
                GetComponentLookup<PrefabRef>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.WorkplaceData> workplaceDataLookup =
                GetComponentLookup<WorkplaceData>(isReadOnly: true);      

            // CityUtils requires SchoolData and Student lookups even though deathcare does not use those branches.
            ComponentLookup<Game.Prefabs.SchoolData> schoolDataLookup =
                GetComponentLookup<SchoolData>(isReadOnly: true);

            BufferLookup<Game.Buildings.Student> studentLookup =
                GetBufferLookup<Student>(isReadOnly: true);

            BufferLookup<Game.Buildings.InstalledUpgrade> upgradesLookup =
                GetBufferLookup<InstalledUpgrade>(isReadOnly: true);

            ComponentLookup<WorkProviderMax> markerLookup =
                GetComponentLookup<WorkProviderMax>(isReadOnly: true);

            int touched = 0;

            using NativeArray<Entity> entities = m_PlacedDeathcareQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity e = entities[i];

                // WorkProvider may live on the owner rather than a queried deathcare entity.
                Entity ownerEntity = e;
                if (ownerLookup.HasComponent(e))
                {
                    ownerEntity = ownerLookup[e].m_Owner;
                }

                if (deletedLookup.HasComponent(ownerEntity))
                {
                    continue;
                }

                // Don't add WorkProvider; only mutate if it already exists.
                if (!workProviderLookup.HasComponent(ownerEntity))
                {
                    continue;
                }

                // CityUtils indexes PrefabRef on the owner entity.
                if (!prefabRefLookup.HasComponent(ownerEntity))
                {
                    continue;
                }

                // Use game's CityUtils calculation so upgrades and service rules stay vanilla-aligned.
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
                    WorkProviderMax marker = new()
                    {
                        MaxWorkers = maxWorkers
                    };

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

#if DEBUG
            if (touched > 0)
            {
                CS2Shared.RiverMochi.LogUtils.Info(() => $"[FD] Placed workers refreshed {touched} deathcare buildings.");
            }
#endif
        }

        private void RestorePlacedDeathcareWorkers(ref EntityCommandBuffer ecb)
        {
            ComponentLookup<Game.Common.Deleted> deletedLookup =
                GetComponentLookup<Deleted>(isReadOnly: true);

            ComponentLookup<Game.Companies.WorkProvider> workProviderLookup =
                GetComponentLookup<WorkProvider>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup =
                GetComponentLookup<PrefabRef>(isReadOnly: true);

            ComponentLookup<Game.Prefabs.WorkplaceData> workplaceDataLookup =
                GetComponentLookup<WorkplaceData>(isReadOnly: true);

            // CityUtils still requires its unused school lookups here.
            ComponentLookup<Game.Prefabs.SchoolData> schoolDataLookup =
                GetComponentLookup<SchoolData>(isReadOnly: true);

            BufferLookup<Game.Buildings.Student> studentLookup =
                GetBufferLookup<Student>(isReadOnly: true);

            BufferLookup<Game.Buildings.InstalledUpgrade> upgradesLookup =
                GetBufferLookup<InstalledUpgrade>(isReadOnly: true);

            ComponentLookup<WorkProviderMax> markerLookup =
                GetComponentLookup<WorkProviderMax>(isReadOnly: true);

            using NativeArray<Entity> entities = m_PlacedDeathcareMarkedQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity ownerEntity = entities[i];

                if (deletedLookup.HasComponent(ownerEntity) || !workProviderLookup.HasComponent(ownerEntity))
                {
                    ecb.RemoveComponent<WorkProviderMax>(ownerEntity);
                    continue;
                }

                if (!markerLookup.HasComponent(ownerEntity))
                {
                    continue;
                }

                WorkProvider current = workProviderLookup[ownerEntity];
                WorkProviderMax marker = markerLookup[ownerEntity];

                // Restore only when value still matches last MH write (don't stomp other mods).
                if (current.m_MaxWorkers != marker.MaxWorkers)
                {
                    ecb.RemoveComponent<WorkProviderMax>(ownerEntity);
                    continue;
                }

                if (!prefabRefLookup.HasComponent(ownerEntity))
                {
                    ecb.RemoveComponent<WorkProviderMax>(ownerEntity);
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

                ecb.RemoveComponent<WorkProviderMax>(ownerEntity);
            }
        }

    }
}
