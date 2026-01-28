// File: Systems/FuneralDirectorSystem.cs
// Purpose: “Self Manage” Funeral Director [FD] that applies deathcare multipliers to PREFABS.
// Notes:
// - Runs only on-demand (when settings change or on game load), then disables itself.
// - Reads TRUE vanilla baselines from PrefabSystem -> PrefabBase authoring components (NOT PrefabRef data).
// - Writes changes to Game.Prefabs.DeathcareFacilityData and WorkplaceData on prefab entities.
// - Workers control is extra optional.
// - Instance-side worker limits are derived/cached by the game; this system optionally performs a one-shot recompute
//   for placed deathcare buildings (WorkProvider.m_MaxWorkers) using the same WorkplaceData + InstalledUpgrade logic.

namespace MagicHearse
{
    using Colossal.Serialization.Entities;   // Purpose
    using Game;                              // GameSystemBase, GameMode
    using Game.Common;                       // Deleted, Owner
    using Game.Companies;                    // WorkProvider
    using Game.Prefabs;                      // DeathcareFacility, Workplace, PrefabSystem, PrefabBase, CarData, DeathcareFacilityData, HearseData, CarPrefab, PrefabRef, WorkplaceData
    using Game.Tools;                        // Temp
    using Unity.Collections;                 // Allocator
    using Unity.Entities;                    // Entity, PrefabData, IComponentData, EntityCommandBuffer, SystemAPI
    using Unity.Mathematics;                 // math.*

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;
        private PrefabSystem m_PrefabSystem = null!; // assigned in OnCreate

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

            // 1) DeathcareFacilityData on prefabs
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

            // 1b) Hearse vehicle tuning on prefabs
            ApplyHearseCarTuningFromAuthoring(hearseSpeedScalar);

            // 2) WorkplaceData on deathcare prefabs (optional)
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

                // Instant one-shot recompute for placed deathcare buildings.
                ApplyInstantWorkersToPlacedDeathcare(ref ecb);
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

                // Restore instance-side workers only for buildings previously touched by MH.
                RestoreInstantWorkersOnPlacedDeathcare(ref ecb);
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

            ApplyHearseCarTuningFromAuthoring(1f);

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

            // Restore instance-side workers only for buildings previously touched by MH.
            RestoreInstantWorkersOnPlacedDeathcare(ref ecb);

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void ApplyInstantWorkersToPlacedDeathcare(ref EntityCommandBuffer ecb)
        {
            ComponentLookup<Owner> ownerLookup = GetComponentLookup<Owner>(true);

            ComponentLookup<WorkProvider> workProviderRO = GetComponentLookup<WorkProvider>(true);
            ComponentLookup<PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<Deleted> deletedLookup = GetComponentLookup<Deleted>(true);
            ComponentLookup<WorkplaceData> workplaceDataLookup = GetComponentLookup<WorkplaceData>(true);
            BufferLookup<Game.Buildings.InstalledUpgrade> upgradesLookup = GetBufferLookup<Game.Buildings.InstalledUpgrade>(true);

            int touched = 0;

            foreach ((RefRO<PrefabRef> prefabRef, Entity e) in SystemAPI
                         .Query<RefRO<PrefabRef>>()
                         .WithAll<Game.Buildings.DeathcareFacility>()
                         .WithNone<Temp, Deleted>()
                         .WithEntityAccess())
            {
                Entity ownerEntity = e;
                if (ownerLookup.HasComponent(e))
                {
                    ownerEntity = ownerLookup[e].m_Owner;
                }

                if (deletedLookup.HasComponent(ownerEntity))
                {
                    continue;
                }

                Entity prefabEntity = prefabRef.ValueRO.m_Prefab;

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

                if (workProviderRO.TryGetComponent(ownerEntity, out WorkProvider existing))
                {
                    WorkProvider updated = existing;
                    updated.m_MaxWorkers = maxWorkers;
                    ecb.SetComponent(ownerEntity, updated);

                    MHWorkProviderMarker marker = new MHWorkProviderMarker { MaxWorkers = maxWorkers };
                    if (SystemAPI.HasComponent<MHWorkProviderMarker>(ownerEntity))
                    {
                        ecb.SetComponent(ownerEntity, marker);
                    }
                    else
                    {
                        ecb.AddComponent(ownerEntity, marker);
                    }

                    touched++;
                }
                else
                {
                    // Adding WorkProvider is uncommon for city service buildings; keep minimal but safe.
                    ecb.AddComponent(ownerEntity, new WorkProvider
                    {
                        m_MaxWorkers = maxWorkers,
                        m_EfficiencyCooldown = 0
                    });

                    MHWorkProviderMarker marker = new MHWorkProviderMarker { MaxWorkers = maxWorkers };
                    ecb.AddComponent(ownerEntity, marker);

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
            ComponentLookup<Owner> ownerLookup = GetComponentLookup<Owner>(true);

            ComponentLookup<WorkProvider> workProviderRO = GetComponentLookup<WorkProvider>(true);
            ComponentLookup<PrefabRef> prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            ComponentLookup<Deleted> deletedLookup = GetComponentLookup<Deleted>(true);
            ComponentLookup<WorkplaceData> workplaceDataLookup = GetComponentLookup<WorkplaceData>(true);
            BufferLookup<Game.Buildings.InstalledUpgrade> upgradesLookup = GetBufferLookup<Game.Buildings.InstalledUpgrade>(true);

            foreach ((RefRO<MHWorkProviderMarker> marker, RefRO<PrefabRef> prefabRef, Entity e) in SystemAPI
                         .Query<RefRO<MHWorkProviderMarker>, RefRO<PrefabRef>>()
                         .WithAll<Game.Buildings.DeathcareFacility>()
                         .WithNone<Temp, Deleted>()
                         .WithEntityAccess())
            {
                Entity ownerEntity = e;
                if (ownerLookup.HasComponent(e))
                {
                    ownerEntity = ownerLookup[e].m_Owner;
                }

                if (!workProviderRO.TryGetComponent(ownerEntity, out WorkProvider current))
                {
                    ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                    continue;
                }

                if (current.m_MaxWorkers != marker.ValueRO.MaxWorkers)
                {
                    ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
                    continue;
                }

                Entity prefabEntity = prefabRef.ValueRO.m_Prefab;

                int maxWorkers = ComputeCityServiceWorkplaceMaxWorkers(
                    ownerEntity,
                    prefabEntity,
                    ref prefabRefLookup,
                    ref upgradesLookup,
                    ref deletedLookup,
                    ref workplaceDataLookup);

                WorkProvider updated = current;
                updated.m_MaxWorkers = maxWorkers;
                ecb.SetComponent(ownerEntity, updated);

                ecb.RemoveComponent<MHWorkProviderMarker>(ownerEntity);
            }
        }

        private static int ComputeCityServiceWorkplaceMaxWorkers(
            Entity ownerEntity,
            Entity prefabEntity,
            ref ComponentLookup<PrefabRef> prefabRefs,
            ref BufferLookup<Game.Buildings.InstalledUpgrade> installedUpgrades,
            ref ComponentLookup<Deleted> deleteds,
            ref ComponentLookup<WorkplaceData> workplaceDatas)
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

            DynamicBuffer<Game.Buildings.InstalledUpgrade> upgrades = installedUpgrades[ownerEntity];

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

        private void ApplyHearseCarTuningFromAuthoring(float speedScalar)
        {
            float accelBrakeScalar = math.sqrt(math.max(0.01f, speedScalar));

            foreach ((RefRW<CarData> car, Entity entity) in SystemAPI
                         .Query<RefRW<CarData>>()
                         .WithAll<PrefabData, HearseData>()
                         .WithEntityAccess())
            {
                if (!TryGetCarPrefab(entity, out CarPrefab carPrefab))
                {
                    continue;
                }

                CarData newCar = car.ValueRO;

                float baseMaxSpeedMs = carPrefab.m_MaxSpeed * (1f / 3.6f);

                newCar.m_MaxSpeed = baseMaxSpeedMs <= 0f
                    ? 0f
                    : math.max(0.01f, baseMaxSpeedMs * speedScalar);

                newCar.m_Acceleration = carPrefab.m_Acceleration <= 0f
                    ? 0f
                    : carPrefab.m_Acceleration * accelBrakeScalar;

                newCar.m_Braking = carPrefab.m_Braking <= 0f
                    ? 0f
                    : carPrefab.m_Braking * accelBrakeScalar;

                car.ValueRW = newCar;
            }
        }

        private bool TryGetDeathcareAuthoring(Entity prefabEntity, out DeathcareFacility authoring)
        {
            authoring = default!;

            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
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

        private bool TryGetCarPrefab(Entity prefabEntity, out CarPrefab carPrefab)
        {
            carPrefab = default!;

            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
            {
                return false;
            }

            if (prefabBase is CarPrefab car)
            {
                carPrefab = car;
                return true;
            }

            return false;
        }
    }
}
