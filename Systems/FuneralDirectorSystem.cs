// File: Systems/FuneralDirectorSystem.cs
// Funeral Director: one-shot (on-demand) deathcare tuning from Settings sliders.
// Notes:
// - Runs once on city load + once when sliders change.
// - Caches vanilla values per-entity to avoid compounding multipliers.
// - Applies FacilityStorageScalar ONLY when DeathcareFacilityData.m_LongTermStorage is true (cemeteries).

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
        private EntityQuery m_DeathcareFacilityDataQuery;
        private EntityQuery m_HearseDataQuery;

        private readonly Dictionary<Entity, DeathcareFacilityData> m_BaseDeathcare =
            new Dictionary<Entity, DeathcareFacilityData>();

        private readonly Dictionary<Entity, HearseData> m_BaseHearse =
            new Dictionary<Entity, HearseData>();

        private bool m_Dirty;

        protected override void OnCreate()
        {
            base.OnCreate();

            // These queries intentionally target prefab-ish data components.
            // If CS2 changes where these live, we can tighten with additional tags.
            m_DeathcareFacilityDataQuery = SystemAPI.QueryBuilder()
                .WithAllRW<DeathcareFacilityData>()
                .Build();

            m_HearseDataQuery = SystemAPI.QueryBuilder()
                .WithAllRW<HearseData>()
                .Build();

            Enabled = false; // on-demand only
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            Setting? s = Setting.Instance;
            if (s != null && s.FuneralDirector)
            {
                RequestReapplyFromSettings();
            }
        }

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

            Setting? s = Setting.Instance;
            if (s == null || !s.FuneralDirector)
            {
                Enabled = false;
                return;
            }

            float processingMul = PercentToScalar(s.ProcessingScalar);
            float facilityHearseMul = PercentToScalar(s.FacilityHearseScalar);
            float facilityStorageMul = PercentToScalar(s.FacilityStorageScalar);
            float hearseCapMul = PercentToScalar(s.HearseCapacityScalar);

            ApplyDeathcareFacilityData(processingMul, facilityHearseMul, facilityStorageMul);
            ApplyHearseData(hearseCapMul);

            Enabled = false;
        }

        private void ApplyDeathcareFacilityData(float processingMul, float hearseMul, float storageMul)
        {
            NativeArray<Entity> entities = m_DeathcareFacilityDataQuery.ToEntityArray(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                Entity e = entities[i];

                DeathcareFacilityData current = EntityManager.GetComponentData<DeathcareFacilityData>(e);

                if (!m_BaseDeathcare.TryGetValue(e, out DeathcareFacilityData baseline))
                {
                    baseline = current;
                    m_BaseDeathcare.Add(e, baseline);
                }

                // Start from baseline every time (no compounding).
                DeathcareFacilityData updated = baseline;

                updated.m_ProcessingRate = Math.Max(0.01f, baseline.m_ProcessingRate * processingMul);

                updated.m_HearseCapacity = Math.Max(1, (int)Math.Round(baseline.m_HearseCapacity * hearseMul));

                // Only apply storage multiplier to cemetery-like facilities.
                if (baseline.m_LongTermStorage)
                {
                    updated.m_StorageCapacity = Math.Max(1, (int)Math.Round(baseline.m_StorageCapacity * storageMul));
                }

                if (!DeathcareEquals(current, updated))
                {
                    EntityManager.SetComponentData(e, updated);
                }
            }

            entities.Dispose();
        }

        private void ApplyHearseData(float capacityMul)
        {
            NativeArray<Entity> entities = m_HearseDataQuery.ToEntityArray(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                Entity e = entities[i];

                HearseData current = EntityManager.GetComponentData<HearseData>(e);

                if (!m_BaseHearse.TryGetValue(e, out HearseData baseline))
                {
                    baseline = current;
                    m_BaseHearse.Add(e, baseline);
                }

                HearseData updated = baseline;
                updated.m_CorpseCapacity = Math.Max(1, (int)Math.Round(baseline.m_CorpseCapacity * capacityMul));

                if (current.m_CorpseCapacity != updated.m_CorpseCapacity)
                {
                    EntityManager.SetComponentData(e, updated);
                }
            }

            entities.Dispose();
        }

        private static float PercentToScalar(int percent)
        {
            // 100 => 1.0f
            // Clamp just to be defensive.
            int p = Math.Max(1, percent);
            return p / 100f;
        }

        private static bool DeathcareEquals(DeathcareFacilityData a, DeathcareFacilityData b)
        {
            return
                a.m_HearseCapacity == b.m_HearseCapacity
                && a.m_StorageCapacity == b.m_StorageCapacity
                && Math.Abs(a.m_ProcessingRate - b.m_ProcessingRate) < 0.0001f
                && a.m_LongTermStorage == b.m_LongTermStorage;
        }
    }
}
