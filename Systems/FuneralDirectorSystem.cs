// File: Systems/FuneralDirectorSystem.cs
// Purpose: “Self Manage” Funeral Director [FD] that applies deathcare multipliers to PREFABS.
// Notes:
// - Runs only on-demand (when settings change or on game load), then disables itself.
// - Reads TRUE vanilla baselines from PrefabSystem -> PrefabBase authoring components (NOT PrefabRef data).
// - Writes changes to Game.Prefabs.DeathcareFacilityData and WorkplaceData on prefab entities.
// - FD OFF auto restores vanilla (authoring) values.

namespace MagicHearse
{
    using Colossal.Serialization.Entities;  // Purpose
    using Game;                             // GameSystemBase, GameMode
    using Game.Prefabs;                     // DeathcareFacility, Workplace, PrefabSystem, PrefabBase
    using Unity.Entities;                   // Entity, PrefabData, SystemAPI
    using Unity.Mathematics;                // math.*

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;
        private PrefabSystem m_PrefabSystem = null!; // assigned in OnCreate

        protected override void OnCreate()
        {
            base.OnCreate();

            // One-shot system: only enabled for a single apply/restore pass.
            Enabled = false;

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            Mod.Log.Info("[FD] System created.");
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            Setting? setting = Mod.Settings;
            if (setting != null && setting.FuneralDirector)
            {
                Mod.Log.Info($"[FD] OnGameLoadingComplete: purpose={purpose}, mode={mode}");
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
                Mod.Log.Warn("[FD] No settings instance; skipping.");
                Enabled = false;
                return;
            }

            if (!setting.FuneralDirector)
            {
                RestoreVanillaFromAuthoring();
                Enabled = false;
                return;
            }

            ApplyMultipliersFromAuthoring(setting);
            Enabled = false;
        }

        private void ApplyMultipliersFromAuthoring(Setting setting)
        {
            float procScalar = setting.ProcScalar * 0.01f;
            float fleetScalar = setting.FleetScalar * 0.01f;
            float storageScalar = setting.StorageScalar * 0.01f;
            float workersScalar = setting.WorkersScalar * 0.01f;

            int edited = 0;
            int skipped = 0;

            // ----------------------------------------------------------------
            // 1) DeathcareFacilityData on prefabs
            // ----------------------------------------------------------------
            foreach ((RefRW<DeathcareFacilityData> dc, Entity entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareAuthoring(entity, out DeathcareFacility authoring))
                {
                    skipped++;
                    continue;
                }

                // TRUE vanilla authoring values.
                float baseRate = authoring.m_ProcessingRate;
                int baseHearses = authoring.m_HearseCapacity;
                int baseStorage = authoring.m_StorageCapacity;
                bool baseLongTerm = authoring.m_LongTermStorage;

                // Start from vanilla authoring (prevents stacking/drift).
                DeathcareFacilityData newData = new DeathcareFacilityData
                {
                    m_HearseCapacity = baseHearses,
                    m_StorageCapacity = baseStorage,
                    m_LongTermStorage = baseLongTerm,
                    m_ProcessingRate = baseRate,
                };

                // Process rate check: scale if vanilla > 0, otherwise keep 0.
                newData.m_ProcessingRate =
                    baseRate <= 0f ? 0f : math.max(0.01f, baseRate * procScalar);

                // Fleet check: scale if vanilla > 0, otherwise keep 0.
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
                edited++;
            }

            // ----------------------------------------------------------------
            // 2) WorkplaceData on deathcare prefabs (filter by DeathcareFacilityData)
            // ----------------------------------------------------------------
            foreach ((RefRW<WorkplaceData> wp, Entity entity) in SystemAPI
                         .Query<RefRW<WorkplaceData>>()
                         .WithAll<PrefabData, DeathcareFacilityData>()
                         .WithEntityAccess())
            {
                if (!TryGetWorkplaceAuthoring(entity, out Workplace workplace))
                {
                    // Some deathcare prefabs may not have Workplace authoring; ignore.
                    continue;
                }

                int baseMax = workplace.m_Workplaces;
                int baseMin = workplace.m_MinimumWorkersLimit;

                WorkplaceData newWp = wp.ValueRO;

                // Baseline from TRUE vanilla authoring.
                newWp.m_MaxWorkers = baseMax;
                newWp.m_MinimumWorkersLimit = baseMin;

                // Scale max workers (100–500%).
                if (baseMax > 0)
                {
                    int scaledMax = (int)math.round(baseMax * workersScalar);
                    newWp.m_MaxWorkers = math.max(1, scaledMax);

                    // Keep min <= max; scale min similarly.
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
                    // If vanilla max is 0, keep 0.
                    newWp.m_MaxWorkers = 0;
                    newWp.m_MinimumWorkersLimit = 0;
                }

                wp.ValueRW = newWp;
            }

        }

        private void RestoreVanillaFromAuthoring()
        {
            int restored = 0;
            int skipped = 0;

            // ----------------------------------------------------------------
            // 1) Restore DeathcareFacilityData
            // ----------------------------------------------------------------
            foreach ((RefRW<DeathcareFacilityData> dc, Entity entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareAuthoring(entity, out DeathcareFacility authoring))
                {
                    skipped++;
                    continue;
                }

                dc.ValueRW = new DeathcareFacilityData
                {
                    m_HearseCapacity = authoring.m_HearseCapacity,
                    m_StorageCapacity = authoring.m_StorageCapacity,
                    m_LongTermStorage = authoring.m_LongTermStorage,
                    m_ProcessingRate = authoring.m_ProcessingRate,
                };

                restored++;
            }

            // ----------------------------------------------------------------
            // 2) Restore WorkplaceData for deathcare prefabs
            // ----------------------------------------------------------------
            foreach ((RefRW<WorkplaceData> wp, Entity entity) in SystemAPI
                         .Query<RefRW<WorkplaceData>>()
                         .WithAll<PrefabData, DeathcareFacilityData>()
                         .WithEntityAccess())
            {
                if (!TryGetWorkplaceAuthoring(entity, out Workplace workplace))
                {
                    continue;
                }

                WorkplaceData newWp = wp.ValueRO;
                newWp.m_MaxWorkers = workplace.m_Workplaces;
                newWp.m_MinimumWorkersLimit = workplace.m_MinimumWorkersLimit;
                wp.ValueRW = newWp;
            }

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

            // done exactly like the game’s WorkPlaceGlobalMode does it.
            return prefabBase.TryGetExactly<Workplace>(out workplace);
        }
    }
}
