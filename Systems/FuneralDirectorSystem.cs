// <copyright file="FuneralDirectorSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/FuneralDirectorSystem.cs
// Purpose: “Self Manage” Funeral Director [FD] that applies multipliers to PREFABS.
// Notes:
// - runs only on-demand (settings change), then disables itself.
// - Reads TRUE vanilla baselines from PrefabSystem -> PrefabBase authoring components (NOT PrefabRef).
// - Workers control optional; placed-building worker cache can be recomputed one-shot via WorkProvider.m_MaxWorkers.

namespace MagicHearse
{
    using Colossal.Serialization.Entities; // Purpose
    using CS2Shared.RiverMochi;           // LogUtils
    using Game;                           // GameSystemBase, GameMode
    using Game.Prefabs;                   // PrefabSystem, PrefabBase
    using Unity.Collections;              // Allocator
    using Unity.Entities;                 // Entity, EntityCommandBuffer, SystemAPI
    using Unity.Mathematics;              // math.*

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;
        private PrefabSystem m_PrefabSystem = null!;

        private EntityQuery m_PlacedDeathcareQuery;
        private EntityQuery m_PlacedDeathcareMarkedQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            Enabled = false;

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            // Built-once and reused by Workers partial for one-shot instance cache refresh/restore.
            m_PlacedDeathcareQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Buildings.DeathcareFacility>()
                .WithAll<Game.Prefabs.PrefabRef>()
                .WithNone<Game.Tools.Temp>()
                .WithNone<Game.Common.Deleted>()
                .Build();

            // Query targets owner building entities previously touched.
            m_PlacedDeathcareMarkedQuery = SystemAPI.QueryBuilder()
                .WithAll<WorkProviderMax>()                     
                .WithAll<Game.Buildings.DeathcareFacility>()    // marker lives on owner
                .WithAll<Game.Prefabs.PrefabRef>()
                .WithNone<Game.Tools.Temp>()
                .WithNone<Game.Common.Deleted>()
                .Build();

#if DEBUG
            LogUtils.Info(() => "[FD] System created.");
#endif
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            MHSetting? setting = Mod.Settings;
            if (setting != null && setting.FuneralDirector)
            {
#if DEBUG
                LogUtils.Info(() => $"[FD] OnGameLoadingComplete: purpose={purpose}, mode={mode}");
#endif
                ScheduleReapply();
            }
        }

        /// <summary>Marks system dirty and enables one apply/restore pass.</summary>
        public void ScheduleReapply()
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

            MHSetting? setting = Mod.Settings;
            if (setting == null)
            {
                LogUtils.Warn(() => "[FD] No settings instance; skipping.");
                Enabled = false;
                return;
            }

            try
            {
                if (!setting.FuneralDirector)
                {
                    RestoreBaseline();
                }
                else
                {
                    ApplyScalars(setting);
                }
            }
            catch (System.Exception ex)
            {
                LogUtils.WarnOnce("MH_FD_EXCEPTION", () =>
                    $"[FD] Apply/restore failed: {ex.GetType().Name}: {ex.Message}");
            }
            finally
            {
                Enabled = false;
            }
        }

        private void ApplyScalars(MHSetting setting)
        {
            float procScalar = setting.ProcScalar * 0.01f;
            float cemeteryTurnoverScalar =
                setting.AutoResetCemetery
                    ? 1f
                    : setting.CemeteryTurnoverScalar * 0.01f;
            float fleetScalar = setting.FleetScalar * 0.01f;
            float storageScalar = setting.StorageScalar * 0.01f;

            float hearseSpeedScalar = math.clamp(setting.HearseSpeedScalar * 0.01f, 1f, 10f);

            bool controlWorkers = setting.ControlWorkers;
            float workersScalar = setting.WorkersScalar * 0.01f;

            // Scale from authoring values so repeated setting changes never stack.
            // DeathcareFacilityData on prefab entities
            foreach ((RefRW<Game.Prefabs.DeathcareFacilityData> dc, Entity entity) in SystemAPI
                         .Query<RefRW<Game.Prefabs.DeathcareFacilityData>>()
                         .WithAll<Game.Prefabs.PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareBase(entity, out Game.Prefabs.DeathcareFacility authoring))
                {
                    continue;
                }

                float baseRate = authoring.m_ProcessingRate;
                int baseHearses = authoring.m_HearseCapacity;
                int baseStorage = authoring.m_StorageCapacity;
                bool baseLongTerm = authoring.m_LongTermStorage;

                DeathcareFacilityData newData = new()
                {
                    m_HearseCapacity = baseHearses,
                    m_StorageCapacity = baseStorage,
                    m_LongTermStorage = baseLongTerm,
                    m_ProcessingRate = baseRate,
                };

                float rateScalar = baseLongTerm
                    ? cemeteryTurnoverScalar
                    : procScalar;

                newData.m_ProcessingRate =
                    baseRate <= 0f
                        ? 0f
                        : math.max(0.01f, baseRate * rateScalar);

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

            // 1b) Hearse vehicle tuning on prefab entities
            ApplyHearseCarTuning(hearseSpeedScalar);

            // 2) WorkplaceData on deathcare prefab entities (optional)
            EntityCommandBuffer ecb = new(Allocator.Temp);

            if (controlWorkers)
            {
                foreach ((RefRW<Game.Prefabs.WorkplaceData> wp, Entity entity) in SystemAPI
                             .Query<RefRW<Game.Prefabs.WorkplaceData>>()
                             .WithAll<Game.Prefabs.PrefabData, Game.Prefabs.DeathcareFacilityData>()    // fully qualify prevents errors.
                             .WithEntityAccess())
                {
                    if (!TryGetWorkplaceBase(entity, out Game.Prefabs.Workplace workplace))
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

                    MHWorkplaceMarker marker = new()
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

                // One-shot refresh for placed deathcare buildings (WorkProvider.m_MaxWorkers).
                RefreshPlacedDeathcareWorkers(ref ecb);
            }
            else
            {
                foreach ((RefRW<Game.Prefabs.WorkplaceData> wp, RefRO<MHWorkplaceMarker> marker, Entity entity) in SystemAPI
                             .Query<RefRW<Game.Prefabs.WorkplaceData>, RefRO<MHWorkplaceMarker>>()
                             .WithAll<Game.Prefabs.PrefabData, Game.Prefabs.DeathcareFacilityData>()
                             .WithEntityAccess())
                {
                    Game.Prefabs.WorkplaceData current = wp.ValueRO;

                    bool stillMatches =
                        current.m_MaxWorkers == marker.ValueRO.MaxWorkers &&
                        current.m_MinimumWorkersLimit == marker.ValueRO.MinWorkers;

                    if (stillMatches && TryGetWorkplaceBase(entity, out Game.Prefabs.Workplace workplace))
                    {
                        current.m_MaxWorkers = workplace.m_Workplaces;
                        current.m_MinimumWorkersLimit = workplace.m_MinimumWorkersLimit;
                        wp.ValueRW = current;
                    }

                    ecb.RemoveComponent<MHWorkplaceMarker>(entity);
                }

                // Restore instance-side workers only for buildings previously touched by MH.
                RestorePlacedDeathcareWorkers(ref ecb);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void RestoreBaseline()
        {
            foreach ((RefRW<Game.Prefabs.DeathcareFacilityData> dc, Entity entity) in SystemAPI
                         .Query<RefRW<Game.Prefabs.DeathcareFacilityData>>()
                         .WithAll<Game.Prefabs.PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareBase(entity, out Game.Prefabs.DeathcareFacility authoring))
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

            ApplyHearseCarTuning(1f);

            EntityCommandBuffer ecb = new(Allocator.Temp);

            foreach ((RefRW<Game.Prefabs.WorkplaceData> wp, RefRO<MHWorkplaceMarker> marker, Entity entity) in SystemAPI
                         .Query<RefRW<Game.Prefabs.WorkplaceData>, RefRO<MHWorkplaceMarker>>()
                         .WithAll<Game.Prefabs.PrefabData, Game.Prefabs.DeathcareFacilityData>()
                         .WithEntityAccess())
            {
                Game.Prefabs.WorkplaceData current = wp.ValueRO;

                bool stillMatches =
                    current.m_MaxWorkers == marker.ValueRO.MaxWorkers &&
                    current.m_MinimumWorkersLimit == marker.ValueRO.MinWorkers;

                if (stillMatches && TryGetWorkplaceBase(entity, out Game.Prefabs.Workplace workplace))
                {
                    current.m_MaxWorkers = workplace.m_Workplaces;
                    current.m_MinimumWorkersLimit = workplace.m_MinimumWorkersLimit;
                    wp.ValueRW = current;
                }

                ecb.RemoveComponent<MHWorkplaceMarker>(entity);
            }

            RestorePlacedDeathcareWorkers(ref ecb);

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private bool TryGetDeathcareBase(Entity prefabEntity, out Game.Prefabs.DeathcareFacility authoring)
        {
            authoring = default!;

            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out Game.Prefabs.PrefabBase prefabBase))
            {
                return false;
            }

            return prefabBase.TryGet(out authoring);
        }

        private bool TryGetWorkplaceBase(Entity prefabEntity, out Workplace workplace)
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
