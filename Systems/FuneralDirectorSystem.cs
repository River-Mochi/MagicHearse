// File: Systems/FuneralDirectorSystem.cs
// Purpose: “Self Manage” Funeral Director [FD] that applies deathcare multipliers to PREFABS.
// Notes:
// - Runs only on-demand (when settings change or on game load), then disables itself.
// - Reads TRUE vanilla baselines from PrefabSystem -> PrefabBase authoring components (NOT PrefabRef data).
// - Writes changes to Game.Prefabs.DeathcareFacilityData and WorkplaceData on prefab entities.
// - Workers control is optional; placed-building worker cache can be recomputed one-shot via WorkProvider.m_MaxWorkers.

namespace MagicHearse
{
    using Colossal.Serialization.Entities;   // Purpose
    using Game;                              // GameSystemBase, GameMode
    using Game.Prefabs;                      // PrefabSystem, PrefabBase, authoring components, prefab components
    using Unity.Collections;                 // Allocator
    using Unity.Entities;                    // Entity, EntityQuery, ComponentType, EntityCommandBuffer, SystemAPI, RefRW/RefRO, IComponentData
    using Unity.Mathematics;                 // math.*

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;
        private PrefabSystem m_PrefabSystem = null!; // assigned in OnCreate

        private EntityQuery m_PlacedDeathcareQuery;
        private EntityQuery m_PlacedDeathcareMarkedQuery;
        private EntityQuery m_HearseCarPrefabQuery;

        // Marker: MH’s last applied worker values on prefab entities.
        private struct MHWorkplaceMarker : IComponentData
        {
            public int MaxWorkers;
            public int MinWorkers;
        }

        // Marker: MH’s last applied derived WorkProvider max on placed building entities.
        private struct MHWorkProviderMarker : IComponentData
        {
            public int MaxWorkers;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            Enabled = false;

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            // Placed deathcare (buildings + upgrades) used for one-shot recompute.
            m_PlacedDeathcareQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(),
                    ComponentType.ReadOnly<Game.Prefabs.PrefabRef>(),
                },
                None = new[]
                {
                    ComponentType.ReadOnly<Game.Tools.Temp>(),
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                },
            });

            // Placed deathcare owners touched by MH (marker lives on owner entity).
            m_PlacedDeathcareMarkedQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<MHWorkProviderMarker>(),
                    ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(),
                    ComponentType.ReadOnly<Game.Prefabs.PrefabRef>(),
                },
                None = new[]
                {
                    ComponentType.ReadOnly<Game.Tools.Temp>(),
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                },
            });

            // Hearse car prefab entities to tune.
            m_HearseCarPrefabQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<Game.Prefabs.PrefabData>(),
                    ComponentType.ReadOnly<Game.Prefabs.HearseData>(),
                    ComponentType.ReadWrite<Game.Prefabs.CarData>(),
                },
            });

#if DEBUG
            Mod.LogSafe(() => "[FD] System created.");
#endif
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            Setting? setting = Mod.Settings;
            if (setting != null && setting.FuneralDirector)
            {
#if DEBUG
                Mod.LogSafe(() => $"[FD] OnGameLoadingComplete: purpose={purpose}, mode={mode}");
#endif
                RequestReapplyFromSettings();
            }
        }

        /// <summary>Called by settings setters to schedule one apply/restore pass.</summary>
        public void RequestReapplyFromSettings()
        {
            m_Dirty = true;
            Enabled = true;
        }

        protected override void OnUpdate()
        {
            if (!m_Dirty)
            {
                Enabled = false;
                return;
            }

            m_Dirty = false;

            Setting? setting = Mod.Settings;
            if (setting == null)
            {
                Mod.WarnSafe(() => "[FD] No settings instance; skipping.");
                Enabled = false;
                return;
            }

            try
            {
                if (!setting.FuneralDirector)
                {
                    RestoreVanillaFromAuthoring();
                }
                else
                {
                    ApplyMultipliersFromAuthoring(setting);
                }
            }
            catch (System.Exception ex)
            {
                Mod.WarnOnce("MH_FD_EXCEPTION", () =>
                    $"[FD] Apply/restore failed: {ex.GetType().Name}: {ex.Message}");
            }
            finally
            {
                Enabled = false;
            }
        }

        private void ApplyMultipliersFromAuthoring(Setting setting)
        {
            float procScalar = setting.ProcScalar * 0.01f;
            float fleetScalar = setting.FleetScalar * 0.01f;
            float storageScalar = setting.StorageScalar * 0.01f;

            float hearseSpeedScalar = math.clamp(setting.HearseSpeedScalar * 0.01f, 1f, 10f);

            bool controlWorkers = setting.ControlWorkers;
            float workersScalar = setting.WorkersScalar * 0.01f;

            // 1) DeathcareFacilityData on PREFABS
            foreach ((RefRW<Game.Prefabs.DeathcareFacilityData> dc, Entity entity) in SystemAPI
                         .Query<RefRW<Game.Prefabs.DeathcareFacilityData>>()
                         .WithAll<Game.Prefabs.PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareAuthoring(entity, out Game.Prefabs.DeathcareFacility authoring))
                {
                    continue;
                }

                float baseRate = authoring.m_ProcessingRate;
                int baseHearses = authoring.m_HearseCapacity;
                int baseStorage = authoring.m_StorageCapacity;
                bool baseLongTerm = authoring.m_LongTermStorage;

                Game.Prefabs.DeathcareFacilityData newData = new Game.Prefabs.DeathcareFacilityData
                {
                    m_HearseCapacity = baseHearses,
                    m_StorageCapacity = baseStorage,
                    m_LongTermStorage = baseLongTerm,
                    m_ProcessingRate = baseRate,
                };

                newData.m_ProcessingRate = baseRate <= 0f ? 0f : math.max(0.01f, baseRate * procScalar);

                if (baseHearses <= 0)
                {
                    newData.m_HearseCapacity = 0;
                }
                else
                {
                    int scaledHearses = (int)math.round(baseHearses * fleetScalar);
                    newData.m_HearseCapacity = math.max(1, scaledHearses);
                }

                if (baseLongTerm)
                {
                    if (baseStorage <= 0)
                    {
                        newData.m_StorageCapacity = 0;
                    }
                    else
                    {
                        int scaledStorage = (int)math.round(baseStorage * storageScalar);
                        newData.m_StorageCapacity = math.max(1, scaledStorage);
                    }
                }

                dc.ValueRW = newData;
            }

            // 1b) Hearse vehicle tuning on PREFABS (query via EntityQuery to keep SystemAPI in one file)
            ApplyHearseCarTuningFromAuthoring(hearseSpeedScalar);

            // 2) WorkplaceData on deathcare PREFABS (optional)
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            if (controlWorkers)
            {
                foreach ((RefRW<Game.Prefabs.WorkplaceData> wp, Entity entity) in SystemAPI
                             .Query<RefRW<Game.Prefabs.WorkplaceData>>()
                             .WithAll<Game.Prefabs.PrefabData, Game.Prefabs.DeathcareFacilityData>()
                             .WithEntityAccess())
                {
                    if (!TryGetWorkplaceAuthoring(entity, out Game.Prefabs.Workplace workplace))
                    {
                        continue;
                    }

                    int baseMax = workplace.m_Workplaces;
                    int baseMin = workplace.m_MinimumWorkersLimit;

                    Game.Prefabs.WorkplaceData newWp = wp.ValueRO;

                    newWp.m_MaxWorkers = baseMax;
                    newWp.m_MinimumWorkersLimit = baseMin;

                    if (baseMax > 0)
                    {
                        int scaledMax = (int)math.round(baseMax * workersScalar);
                        newWp.m_MaxWorkers = math.max(1, scaledMax);

                        if (baseMin > 0)
                        {
                            int scaledMin = (int)math.round(baseMin * workersScalar);
                            newWp.m_MinimumWorkersLimit = math.clamp(scaledMin, 0, newWp.m_MaxWorkers);
                        }
                        else
                        {
                            newWp.m_MinimumWorkersLimit = 0;
                        }
                    }
                    else
                    {
                        newWp.m_MaxWorkers = 0;
                        newWp.m_MinimumWorkersLimit = 0;
                    }

                    wp.ValueRW = newWp;

                    MHWorkplaceMarker marker = new MHWorkplaceMarker
                    {
                        MaxWorkers = newWp.m_MaxWorkers,
                        MinWorkers = newWp.m_MinimumWorkersLimit,
                    };

                    if (SystemAPI.HasComponent<MHWorkplaceMarker>(entity))
                    {
                        ecb.SetComponent(entity, marker);
                    }
                    else
                    {
                        ecb.AddComponent(entity, marker);
                    }
                }

                // One-shot recompute for placed deathcare buildings (WorkProvider.m_MaxWorkers).
                ApplyInstantWorkersToPlacedDeathcare(ref ecb);
            }
            else
            {
                foreach ((RefRW<Game.Prefabs.WorkplaceData> wp, RefRO<MHWorkplaceMarker> marker, Entity entity) in SystemAPI
                             .Query<RefRW<Game.Prefabs.WorkplaceData>, RefRO<MHWorkplaceMarker>>()
                             .WithAll<Game.Prefabs.PrefabData, Game.Prefabs.DeathcareFacilityData>()
                             .WithEntityAccess())
                {
                    Game.Prefabs.WorkplaceData current = wp.ValueRO;

                    bool stillMatchesMh =
                        current.m_MaxWorkers == marker.ValueRO.MaxWorkers &&
                        current.m_MinimumWorkersLimit == marker.ValueRO.MinWorkers;

                    if (stillMatchesMh && TryGetWorkplaceAuthoring(entity, out Game.Prefabs.Workplace workplace))
                    {
                        current.m_MaxWorkers = workplace.m_Workplaces;
                        current.m_MinimumWorkersLimit = workplace.m_MinimumWorkersLimit;
                        wp.ValueRW = current;
                    }

                    ecb.RemoveComponent<MHWorkplaceMarker>(entity);
                }

                // Restore instance-side workers only for buildings previously touched by MH.
                RestoreInstantWorkersOnPlacedDeathcare(ref ecb);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void RestoreVanillaFromAuthoring()
        {
            foreach ((RefRW<Game.Prefabs.DeathcareFacilityData> dc, Entity entity) in SystemAPI
                         .Query<RefRW<Game.Prefabs.DeathcareFacilityData>>()
                         .WithAll<Game.Prefabs.PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareAuthoring(entity, out Game.Prefabs.DeathcareFacility authoring))
                {
                    continue;
                }

                dc.ValueRW = new Game.Prefabs.DeathcareFacilityData
                {
                    m_HearseCapacity = authoring.m_HearseCapacity,
                    m_StorageCapacity = authoring.m_StorageCapacity,
                    m_LongTermStorage = authoring.m_LongTermStorage,
                    m_ProcessingRate = authoring.m_ProcessingRate,
                };
            }

            ApplyHearseCarTuningFromAuthoring(1f);

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach ((RefRW<Game.Prefabs.WorkplaceData> wp, RefRO<MHWorkplaceMarker> marker, Entity entity) in SystemAPI
                         .Query<RefRW<Game.Prefabs.WorkplaceData>, RefRO<MHWorkplaceMarker>>()
                         .WithAll<Game.Prefabs.PrefabData, Game.Prefabs.DeathcareFacilityData>()
                         .WithEntityAccess())
            {
                Game.Prefabs.WorkplaceData current = wp.ValueRO;

                bool stillMatchesMh =
                    current.m_MaxWorkers == marker.ValueRO.MaxWorkers &&
                    current.m_MinimumWorkersLimit == marker.ValueRO.MinWorkers;

                if (stillMatchesMh && TryGetWorkplaceAuthoring(entity, out Game.Prefabs.Workplace workplace))
                {
                    current.m_MaxWorkers = workplace.m_Workplaces;
                    current.m_MinimumWorkersLimit = workplace.m_MinimumWorkersLimit;
                    wp.ValueRW = current;
                }

                ecb.RemoveComponent<MHWorkplaceMarker>(entity);
            }

            RestoreInstantWorkersOnPlacedDeathcare(ref ecb);

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private bool TryGetDeathcareAuthoring(Entity prefabEntity, out Game.Prefabs.DeathcareFacility authoring)
        {
            authoring = default!;

            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out Game.Prefabs.PrefabBase prefabBase))
            {
                return false;
            }

            return prefabBase.TryGet(out authoring);
        }

        private bool TryGetWorkplaceAuthoring(Entity prefabEntity, out Workplace workplace)
        {
            workplace = default!;

            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
            {
                return false;
            }

            return prefabBase.TryGetExactly(out workplace);
        }
    }
}
