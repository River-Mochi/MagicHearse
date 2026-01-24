// File: Systems/FuneralDirectorSystem.cs
// Purpose: “Self Manage” Funeral Director [FD] that applies deathcare multipliers to PREFABS.
// Notes:
// - Runs only on-demand (when settings change or on game load), then disables itself.
// - Reads TRUE vanilla baselines from PrefabSystem -> PrefabBase authoring components (NOT PrefabRef data).
// - Writes changes to Game.Prefabs.DeathcareFacilityData and WorkplaceData on prefab entities.
// - Workers control is optional (Setting.ControlWorkers).
// - When workers control turns OFF (or FD turns OFF), restore workers ONLY if current values still match MH’s last-applied values.
//   If values differ, assume another mod owns workers now -> leave it alone.

namespace MagicHearse
{
    using Colossal.Serialization.Entities;  // Purpose
    using Game;                             // GameSystemBase, GameMode
    using Game.Prefabs;                     // DeathcareFacility, Workplace, PrefabSystem, PrefabBase
    using Unity.Collections;                // Allocator
    using Unity.Entities;                   // Entity, PrefabData, IComponentData, EntityCommandBuffer, SystemAPI
    using Unity.Mathematics;                // math.*

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;
        private PrefabSystem m_PrefabSystem = null!; // assigned in OnCreate

        // Marker: MH’s last applied worker values (survives restarts/saves).
        private struct MHWorkplaceMarker : IComponentData
        {
            public int MaxWorkers;
            public int MinWorkers;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            // One-shot system: only enabled for a single apply/restore pass.
            Enabled = false;

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

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
                // FD must never crash the game; worst case, it fails silently + warns once.
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

            bool controlWorkers = setting.ControlWorkers;
            float workersScalar = setting.WorkersScalar * 0.01f;

            // ----------------------------------------------------------------
            // 1) DeathcareFacilityData on prefabs (always when FD is ON)
            // ----------------------------------------------------------------
            foreach ((RefRW<DeathcareFacilityData> dc, Entity entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareAuthoring(entity, out DeathcareFacility authoring))
                {
                    continue;
                }

                float baseRate = authoring.m_ProcessingRate;
                int baseHearses = authoring.m_HearseCapacity;
                int baseStorage = authoring.m_StorageCapacity;
                bool baseLongTerm = authoring.m_LongTermStorage;

                DeathcareFacilityData newData = new DeathcareFacilityData
                {
                    m_HearseCapacity = baseHearses,
                    m_StorageCapacity = baseStorage,
                    m_LongTermStorage = baseLongTerm,
                    m_ProcessingRate = baseRate,
                };

                // Processing rate: scale if vanilla > 0, otherwise keep 0.
                newData.m_ProcessingRate = baseRate <= 0f ? 0f : math.max(0.01f, baseRate * procScalar);

                // Fleet: scale if vanilla > 0, otherwise keep 0.
                if (baseHearses <= 0)
                {
                    newData.m_HearseCapacity = 0;
                }
                else
                {
                    int scaledHearses = (int)math.round(baseHearses * fleetScalar);
                    newData.m_HearseCapacity = math.max(1, scaledHearses);
                }

                // Storage: only scale long-term storage facilities.
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

            // ----------------------------------------------------------------
            // 2) WorkplaceData on deathcare prefabs (optional, compatibility toggle)
            //    Use ECB for marker add/remove so enumeration stays safe.
            // ----------------------------------------------------------------
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            if (controlWorkers)
            {
                foreach ((RefRW<WorkplaceData> wp, Entity entity) in SystemAPI
                             .Query<RefRW<WorkplaceData>>()
                             .WithAll<PrefabData, DeathcareFacilityData>()
                             .WithEntityAccess())
                {
                    if (!TryGetWorkplaceAuthoring(entity, out Workplace workplace))
                    {
                        continue;
                    }

                    int baseMax = workplace.m_Workplaces;
                    int baseMin = workplace.m_MinimumWorkersLimit;

                    WorkplaceData newWp = wp.ValueRO;

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
            }
            else
            {
                foreach ((RefRW<WorkplaceData> wp, RefRO<MHWorkplaceMarker> marker, Entity entity) in SystemAPI
                             .Query<RefRW<WorkplaceData>, RefRO<MHWorkplaceMarker>>()
                             .WithAll<PrefabData, DeathcareFacilityData>()
                             .WithEntityAccess())
                {
                    WorkplaceData current = wp.ValueRO;

                    bool stillMatchesMh =
                        current.m_MaxWorkers == marker.ValueRO.MaxWorkers &&
                        current.m_MinimumWorkersLimit == marker.ValueRO.MinWorkers;

                    if (stillMatchesMh && TryGetWorkplaceAuthoring(entity, out Workplace workplace))
                    {
                        current.m_MaxWorkers = workplace.m_Workplaces;
                        current.m_MinimumWorkersLimit = workplace.m_MinimumWorkersLimit;
                        wp.ValueRW = current;
                    }

                    ecb.RemoveComponent<MHWorkplaceMarker>(entity);
                }
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void RestoreVanillaFromAuthoring()
        {
            foreach ((RefRW<DeathcareFacilityData> dc, Entity entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareAuthoring(entity, out DeathcareFacility authoring))
                {
                    continue;
                }

                dc.ValueRW = new DeathcareFacilityData
                {
                    m_HearseCapacity = authoring.m_HearseCapacity,
                    m_StorageCapacity = authoring.m_StorageCapacity,
                    m_LongTermStorage = authoring.m_LongTermStorage,
                    m_ProcessingRate = authoring.m_ProcessingRate,
                };
            }

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach ((RefRW<WorkplaceData> wp, RefRO<MHWorkplaceMarker> marker, Entity entity) in SystemAPI
                         .Query<RefRW<WorkplaceData>, RefRO<MHWorkplaceMarker>>()
                         .WithAll<PrefabData, DeathcareFacilityData>()
                         .WithEntityAccess())
            {
                WorkplaceData current = wp.ValueRO;

                bool stillMatchesMh =
                    current.m_MaxWorkers == marker.ValueRO.MaxWorkers &&
                    current.m_MinimumWorkersLimit == marker.ValueRO.MinWorkers;

                if (stillMatchesMh && TryGetWorkplaceAuthoring(entity, out Workplace workplace))
                {
                    current.m_MaxWorkers = workplace.m_Workplaces;
                    current.m_MinimumWorkersLimit = workplace.m_MinimumWorkersLimit;
                    wp.ValueRW = current;
                }

                ecb.RemoveComponent<MHWorkplaceMarker>(entity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private bool TryGetDeathcareAuthoring(Entity prefabEntity, out DeathcareFacility authoring)
        {
            authoring = null!;

            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
            {
                return false;
            }

            return prefabBase.TryGet(out authoring);
        }

        private bool TryGetWorkplaceAuthoring(Entity prefabEntity, out Workplace workplace)
        {
            workplace = null!;

            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
            {
                return false;
            }

            return prefabBase.TryGetExactly(out workplace);
        }
    }
}
