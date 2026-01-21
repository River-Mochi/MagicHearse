// File: Systems/FuneralDirectorSystem.cs
// Purpose: One-pass “Self Manage” (Funeral Director) that applies deathcare multipliers to PREFABS.
// Notes:
// - Runs only on demand (when settings change or on game load), then disables itself.
// - Writes to Game.Prefabs.DeathcareFacilityData on prefab entities.
// - Caches vanilla values once so 100% restores defaults.

namespace MagicHearse
{
    using Colossal.Serialization.Entities; // Purpose
    using Game; // GameSystemBase, GameMode
    using Game.Prefabs; // DeathcareFacilityData, PrefabData
    using System.Collections.Generic; // Dictionary
    using Unity.Entities; // Entity, SystemAPI
    using Unity.Mathematics; // math.*

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;

        // Cache vanilla baseline so 100% restores defaults.
        private readonly Dictionary<Entity, DeathcareFacilityData> m_DeathcareBase =
            new Dictionary<Entity, DeathcareFacilityData>();

        protected override void OnCreate()
        {
            base.OnCreate();

            // Start disabled; enable only for a one-pass apply/restore.
            Enabled = false;

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
                // FD off => restore vanilla values (if cached).
                RestoreVanilla();
                Enabled = false;
                return;
            }

            ApplyMultipliers(setting);

            // One-pass; no per-frame cost.
            Enabled = false;
        }

        private void ApplyMultipliers(Setting setting)
        {
            float procScalar = setting.ProcScalar * 0.01f;
            float fleetScalar = setting.FleetScalar * 0.01f;
            float storageScalar = setting.StorageScalar * 0.01f;

            int editedFacilities = 0;

            // Deathcare prefab entities are tagged with Game.Prefabs.PrefabData.
            foreach (var (dc, entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                CacheDeathcareBaseIfNeeded(entity, dc.ValueRO);

                DeathcareFacilityData baseData = m_DeathcareBase[entity];
                DeathcareFacilityData newData = baseData;

                // Facility processing rate (avoid writing 0).
                newData.m_ProcessingRate = math.max(0.01f, baseData.m_ProcessingRate * procScalar);

                // Fleet size (max hearses per facility).
                int newFleet = (int)math.round(baseData.m_HearseCapacity * fleetScalar);
                newData.m_HearseCapacity = math.max(1, newFleet);

                // Cemetery storage only (long-term storage).
                if (baseData.m_LongTermStorage)
                {
                    int newStorage = (int)math.round(baseData.m_StorageCapacity * storageScalar);
                    newData.m_StorageCapacity = math.max(1, newStorage);
                }

                dc.ValueRW = newData;
                editedFacilities++;
            }

            Mod.Log.Info(
                $"[FD] Applied: proc={setting.ProcScalar}% fleet={setting.FleetScalar}% storage={setting.StorageScalar}% | " +
                $"deathcarePrefabs={editedFacilities}");
        }

        private void RestoreVanilla()
        {
            int restoredFacilities = 0;

            foreach (var (dc, entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (m_DeathcareBase.TryGetValue(entity, out DeathcareFacilityData baseData))
                {
                    dc.ValueRW = baseData;
                    restoredFacilities++;
                }
            }

            Mod.Log.Info($"[FD] Restored vanilla: deathcarePrefabs={restoredFacilities}");
        }

        // --------------------------------------------------------------------
        // Helpers
        // --------------------------------------------------------------------

        private void CacheDeathcareBaseIfNeeded(Entity e, DeathcareFacilityData current)
        {
            if (!m_DeathcareBase.ContainsKey(e))
            {
                m_DeathcareBase.Add(e, current);
            }
        }
    }
}
