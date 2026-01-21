// File: Systems/FuneralDirectorSystem.cs
// Purpose: “Self Manage” Funeral Director that applies deathcare multipliers to PREFABS.
// Notes:
// - Runs only on-demand (when settings change or on game load), then disables itself.
// - Reads TRUE vanilla baselines from PrefabSystem -> PrefabBase authoring components (NOT PrefabRef data).
// - Writes changes to Game.Prefabs.DeathcareFacilityData on prefab entities.
// - FD OFF restores vanilla (authoring) values.

namespace MagicHearse
{
    using Colossal.Serialization.Entities;  // Purpose
    using Game;                             // GameSystemBase, GameMode
    using Game.Prefabs;                     // DeathcareFacility (authoring), PrefabSystem, PrefabBase
    using Unity.Entities;                   // Entity, PrefabData, SystemAPI
    using Unity.Mathematics;                // math.*

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;
        private PrefabSystem m_PrefabSystem = null!; // assigned in OnCreate

        protected override void OnCreate()
        {
            base.OnCreate();

            // Start disabled; enable only for a one-pass apply/restore.
            Enabled = false;

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            Mod.Log.Info("[FD] System created.");
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            // Apply once on load if FD is enabled.
            Setting? setting = Mod.Settings;
            if (setting != null && setting.FuneralDirector)
            {
                Mod.Log.Info("[FD] OnGameLoadingComplete: requesting apply.");
                RequestReapplyFromSettings();
            }
        }

        /// <summary>Called by settings setters to schedule one apply/restore pass.</summary>
        public void RequestReapplyFromSettings()
        {
            m_Dirty = true;
            Enabled = true;
            Mod.Log.Info("[FD] RequestReapplyFromSettings()");
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
                // FD off => restore TRUE vanilla values from PrefabBase authoring.
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

            int edited = 0;
            int skipped = 0;

            // Prefab ECS entities are tagged with PrefabData and carry DeathcareFacilityData.
            foreach (var (dc, entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareAuthoring(entity, out DeathcareFacility authoring))
                {
                    skipped++;
                    continue;
                }

                // Start from TRUE vanilla authoring values every time (prevents stacking/drift).
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

                // Processing:
                // - if vanilla is 0, keep 0 (cemeteries).
                // - otherwise scale; clamp tiny min so it can't collapse to 0.
                newData.m_ProcessingRate =
                    baseRate <= 0f ? 0f : math.max(0.01f, baseRate * procScalar);

                // Fleet:
                // - if vanilla is 0, keep 0.
                // - otherwise scale and clamp >= 1.
                if (baseHearses <= 0)
                {
                    newData.m_HearseCapacity = 0;
                }
                else
                {
                    int scaledHearses = (int)math.round(baseHearses * fleetScalar);
                    newData.m_HearseCapacity = math.max(1, scaledHearses);
                }

                // Storage:
                // - only scale long-term storage facilities.
                // - keep vanilla storage for non-long-term facilities.
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

            Mod.Log.Info(
                $"[FD] Applied from authoring: proc={setting.ProcScalar}% fleet={setting.FleetScalar}% storage={setting.StorageScalar}% | " +
                $"deathcarePrefabs={edited} skipped={skipped}");
        }

        private void RestoreVanillaFromAuthoring()
        {
            int restored = 0;
            int skipped = 0;

            foreach (var (dc, entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (!TryGetDeathcareAuthoring(entity, out DeathcareFacility authoring))
                {
                    skipped++;
                    continue;
                }

                // Restore TRUE vanilla authoring values.
                dc.ValueRW = new DeathcareFacilityData
                {
                    m_HearseCapacity = authoring.m_HearseCapacity,
                    m_StorageCapacity = authoring.m_StorageCapacity,
                    m_LongTermStorage = authoring.m_LongTermStorage,
                    m_ProcessingRate = authoring.m_ProcessingRate,
                };

                restored++;
            }

            Mod.Log.Info($"[FD] Restored vanilla from authoring: deathcarePrefabs={restored} skipped={skipped}");
        }

        private bool TryGetDeathcareAuthoring(Entity prefabEntity, out DeathcareFacility authoring)
        {
            authoring = null!;

            // prefabEntity here is the prefab ECS entity (has PrefabData).
            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
            {
                return false;
            }

            // Read TRUE vanilla authoring component values.
            return prefabBase.TryGet(out authoring);
        }
    }
}
