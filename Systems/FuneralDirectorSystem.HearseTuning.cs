// File: Systems/FuneralDirectorSystem.HearseTuning.cs
// Purpose: Hearse vehicle tuning on prefab entities (speed + accel/brake scaling).

namespace MagicHearse
{
    using Game.Prefabs;      // PrefabBase, CarPrefab
    using Unity.Entities;    // Entity, RefRW, SystemAPI
    using Unity.Mathematics; // math.*

    public sealed partial class FuneralDirectorSystem
    {
        private void ApplyHearseCarTuningFromAuthoring(float speedScalar)
        {
            // Top speed uses scalar directly.
            // Accel/brake use sqrt(scalar) to reduce extreme launch/stop behavior at high speeds.
            float clampedSpeedScalar = math.max(0f, speedScalar);
            float accelBrakeScalar = math.sqrt(math.max(0.01f, clampedSpeedScalar));

            foreach ((RefRW<Game.Prefabs.CarData> car, Entity prefabEntity) in SystemAPI
                         .Query<RefRW<Game.Prefabs.CarData>>()
                         .WithAll<PrefabData, HearseData>() // prefab hearses only
                         .WithEntityAccess())
            {
                if (!TryGetCarPrefab(prefabEntity, out CarPrefab carPrefab))
                {
                    continue;
                }

                // Authoring is km/h; runtime CarData uses m/s.
                float baseMaxSpeedMs = carPrefab.m_MaxSpeed * (1f / 3.6f);

                CarData tuned = car.ValueRO;

                tuned.m_MaxSpeed = baseMaxSpeedMs <= 0f
                    ? 0f
                    : math.max(0.01f, baseMaxSpeedMs * clampedSpeedScalar);

                tuned.m_Acceleration = carPrefab.m_Acceleration <= 0f
                    ? 0f
                    : carPrefab.m_Acceleration * accelBrakeScalar;

                tuned.m_Braking = carPrefab.m_Braking <= 0f
                    ? 0f
                    : carPrefab.m_Braking * accelBrakeScalar;

                car.ValueRW = tuned;
            }
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
