// File: Systems/FuneralDirectorSystem.cs
// Purpose: One-pass “Self Manage” (Funeral Director) that applies deathcare/hearse multipliers to PREFABS.
// Notes:
// - Runs only on demand (when settings change or on game load), then disables itself.
// - Prefab entities are reliably identified by Game.Prefabs.PrefabData (same pattern as DispatchBoss/ASC).
// - Caches vanilla values once so 100% restores defaults.

namespace MagicHearse
{
    using Colossal.Serialization.Entities; // Purpose
    using Game;
    using Game.Prefabs;
    using System;
    using System.Collections.Generic;
    using Unity.Entities;

    public sealed partial class FuneralDirectorSystem : GameSystemBase
    {
        private bool m_Dirty;

        // Cache vanilla baseline so we can re-apply cleanly.
        private readonly Dictionary<Entity, DeathcareFacilityData> m_DeathcareBase =
            new Dictionary<Entity, DeathcareFacilityData>();

        private readonly Dictionary<Entity, HearseData> m_HearseBase =
            new Dictionary<Entity, HearseData>();

        protected override void OnCreate()
        {
            base.OnCreate();

            // Like DispatchBoss: require PrefabData + component.
            EntityQuery dcQuery = SystemAPI.QueryBuilder()
                .WithAll<PrefabData, DeathcareFacilityData>()
                .Build();

            EntityQuery hearseQuery = SystemAPI.QueryBuilder()
                .WithAll<PrefabData, HearseData>()
                .Build();

            RequireForUpdate(dcQuery);
            RequireForUpdate(hearseQuery);

            Enabled = false;
            Mod.Log.Info("[FD] System created.");
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            Setting? setting = Mod.Settings;
            if (setting != null && setting.FuneralDirector)
            {
                RequestReapplyFromSettings();
            }
        }

        /// <summary>Called by Setting setters (and Mod.OnLoad) to do one apply pass.</summary>
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
                RestoreVanilla();
                Enabled = false;
                return;
            }

            ApplyMultipliers(setting);
            Enabled = false;
        }

        private void ApplyMultipliers(Setting setting)
        {
            float procScalar = PercentToScalar(setting.ProcScalar);
            float fleetScalar = PercentToScalar(setting.FleetScalar);
            float storageScalar = PercentToScalar(setting.StorageScalar);
            float hearseScalar = PercentToScalar(setting.HearseCapacityScalar);

            int editedFacilities = 0;
            int editedHearses = 0;
            int storageEdited = 0;

            // ---- Deathcare facility prefabs ----
            foreach ((RefRW<DeathcareFacilityData> dcRef, Entity entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                DeathcareFacilityData current = dcRef.ValueRO;
                CacheDeathcareBaseIfNeeded(entity, current);

                DeathcareFacilityData baseData = m_DeathcareBase[entity];
                DeathcareFacilityData newData = baseData;

                newData.m_ProcessingRate = Max01(baseData.m_ProcessingRate * procScalar);
                newData.m_HearseCapacity = Max1((int)Math.Round(baseData.m_HearseCapacity * fleetScalar));

                // Apply storage only to long-term storage facilities (cemetery-like).
                if (baseData.m_LongTermStorage)
                {
                    newData.m_StorageCapacity = Max1((int)Math.Round(baseData.m_StorageCapacity * storageScalar));
                    storageEdited++;
                }

                dcRef.ValueRW = newData;
                editedFacilities++;
            }

            // ---- Hearse vehicle prefabs ----
            foreach ((RefRW<HearseData> hdRef, Entity entity) in SystemAPI
                         .Query<RefRW<HearseData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                HearseData current = hdRef.ValueRO;
                CacheHearseBaseIfNeeded(entity, current);

                HearseData baseData = m_HearseBase[entity];
                int newCap = Max1((int)Math.Round(baseData.m_CorpseCapacity * hearseScalar));

                hdRef.ValueRW = new HearseData(newCap);
                editedHearses++;
            }

            Mod.Log.Info(
                $"[FD] Applied: proc={setting.ProcScalar}% fleet={setting.FleetScalar}% storage={setting.StorageScalar}% hearse={setting.HearseCapacityScalar}% | " +
                $"deathcareEdited={editedFacilities} (storageEdited={storageEdited}) hearseEdited={editedHearses}");
        }

        private void RestoreVanilla()
        {
            int restoredFacilities = 0;
            int restoredHearses = 0;

            foreach ((RefRW<DeathcareFacilityData> dcRef, Entity entity) in SystemAPI
                         .Query<RefRW<DeathcareFacilityData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (m_DeathcareBase.TryGetValue(entity, out DeathcareFacilityData baseData))
                {
                    dcRef.ValueRW = baseData;
                    restoredFacilities++;
                }
            }

            foreach ((RefRW<HearseData> hdRef, Entity entity) in SystemAPI
                         .Query<RefRW<HearseData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                if (m_HearseBase.TryGetValue(entity, out HearseData baseData))
                {
                    hdRef.ValueRW = baseData;
                    restoredHearses++;
                }
            }

            Mod.Log.Info($"[FD] Restored vanilla: deathcare={restoredFacilities} hearse={restoredHearses}");
        }

        private void CacheDeathcareBaseIfNeeded(Entity e, DeathcareFacilityData current)
        {
            if (!m_DeathcareBase.ContainsKey(e))
            {
                m_DeathcareBase.Add(e, current);
            }
        }

        private void CacheHearseBaseIfNeeded(Entity e, HearseData current)
        {
            if (!m_HearseBase.ContainsKey(e))
            {
                m_HearseBase.Add(e, current);
            }
        }

        private static float PercentToScalar(int percent) => percent <= 0 ? 0f : (percent / 100f);
        private static int Max1(int v) => v < 1 ? 1 : v;
        private static float Max01(float v) => v < 0.01f ? 0.01f : v;
    }
}
