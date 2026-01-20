// File: Systems/FuneralDirectorSystem.cs
// Purpose: One-pass “Self Manage” (Funeral Director) that applies deathcare multipliers to PREFABS.
// Notes:
// - Runs only on demand (when settings change or on game load), then disables itself.
// - Writes to Game.Prefabs.DeathcareFacilityData on prefab entities.
// - Caches vanilla values once so 100% restores defaults.

namespace MagicHearse
{
    using Colossal.Serialization.Entities; // Purpose
    using Game;
    using Game.Prefabs;
    using System;
    using System.Collections.Generic;
    using Unity.Collections;
    using Unity.Entities;

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;

        private EntityQuery m_DeathcarePrefabQuery = default;

        // Cache vanilla baseline so we can re-apply cleanly.
        private readonly Dictionary<Entity, DeathcareFacilityData> m_DeathcareBase =
            new Dictionary<Entity, DeathcareFacilityData>();

        protected override void OnCreate()
        {
            base.OnCreate();

            // IMPORTANT: In CS2, prefab entities are tagged with Unity.Entities.Prefab (NOT Game.Prefabs.PrefabData).
            m_DeathcarePrefabQuery = SystemAPI.QueryBuilder()
                .WithAll<Unity.Entities.Prefab, DeathcareFacilityData>()
                .Build();

            // Start disabled; we enable only for a one-pass apply.
            Enabled = false;

            Mod.Log.Info("[FD] System created.");
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            // Apply once on load if FD is enabled.
            var setting = Mod.Settings;
            if (setting != null && setting.FuneralDirector)
            {
                Mod.Log.Info("[FD] OnGameLoadingComplete: requesting apply.");
                RequestReapplyFromSettings();
            }
        }

        /// <summary>Called by Setting setters (and optionally Mod.OnLoad) to do one apply pass.</summary>
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

            var setting = Mod.Settings;
            if (setting == null)
            {
                Mod.Log.Warn("[FD] No settings instance; skipping.");
                Enabled = false;
                return;
            }

            if (!setting.FuneralDirector)
            {
                // FD off => restore vanilla values (if we have them cached).
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
            float procScalar = PercentToScalar(setting.ProcScalar);
            float fleetScalar = PercentToScalar(setting.FleetScalar);
            float storageScalar = PercentToScalar(setting.StorageScalar);

            int editedFacilities = 0;

            // ---- Deathcare facility prefabs ----
            using (NativeArray<Entity> prefabs = m_DeathcarePrefabQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < prefabs.Length; i++)
                {
                    Entity e = prefabs[i];

                    DeathcareFacilityData current = EntityManager.GetComponentData<DeathcareFacilityData>(e);
                    CacheDeathcareBaseIfNeeded(e, current);

                    DeathcareFacilityData baseData = m_DeathcareBase[e];
                    DeathcareFacilityData newData = baseData;

                    // Facility processing rate
                    newData.m_ProcessingRate = Max01(baseData.m_ProcessingRate * procScalar);

                    // Facility fleet size (max hearses per facility)
                    newData.m_HearseCapacity = Max1((int)Math.Round(baseData.m_HearseCapacity * fleetScalar));

                    // Storage multiplier: apply only to long-term storage (cemetery-like) prefabs.
                    if (baseData.m_LongTermStorage)
                    {
                        newData.m_StorageCapacity = Max1((int)Math.Round(baseData.m_StorageCapacity * storageScalar));
                    }

                    EntityManager.SetComponentData(e, newData);
                    editedFacilities++;
                }
            }

            Mod.Log.Info(
                $"[FD] Applied: proc={setting.ProcScalar}% fleet={setting.FleetScalar}% storage={setting.StorageScalar}% | " +
                $"deathcarePrefabs={editedFacilities}");
        }

        private void RestoreVanilla()
        {
            int restoredFacilities = 0;

            // Restore deathcare facility prefabs.
            using (NativeArray<Entity> prefabs = m_DeathcarePrefabQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < prefabs.Length; i++)
                {
                    Entity e = prefabs[i];
                    if (m_DeathcareBase.TryGetValue(e, out var baseData))
                    {
                        EntityManager.SetComponentData(e, baseData);
                        restoredFacilities++;
                    }
                }
            }

            Mod.Log.Info($"[FD] Restored vanilla: deathcarePrefabs={restoredFacilities}");
        }

        private void CacheDeathcareBaseIfNeeded(Entity e, DeathcareFacilityData current)
        {
            if (!m_DeathcareBase.ContainsKey(e))
            {
                m_DeathcareBase.Add(e, current);
            }
        }

        private static float PercentToScalar(int percent)
        {
            if (percent <= 0)
            {
                return 0f;
            }

            return percent / 100f;
        }

        private static int Max1(int v) => v < 1 ? 1 : v;
        private static float Max01(float v) => v < 0.01f ? 0.01f : v;
    }
}
