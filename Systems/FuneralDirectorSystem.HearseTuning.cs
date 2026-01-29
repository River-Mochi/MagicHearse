// File: Systems/FuneralDirectorSystem.HearseTuning.cs
// Purpose: Hearse vehicle tuning on prefab entities (speed + accel/brake scaling).

namespace MagicHearse
{
    using Game.Prefabs;              // CarData, CarPrefab
    using Unity.Collections;         // Allocator, NativeArray
    using Unity.Entities;            // Entity
    using Unity.Mathematics;         // math.*

    public sealed partial class FuneralDirectorSystem
    {
        private void ApplyHearseCarTuningFromAuthoring(float speedScalar)
        {
            float accelBrakeScalar = math.sqrt(math.max(0.01f, speedScalar));

            using (NativeArray<Entity> entities = m_HearseCarPrefabQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity entity = entities[i];

                    if (!TryGetCarPrefab(entity, out Game.Prefabs.CarPrefab carPrefab))
                    {
                        continue;
                    }

                    Game.Prefabs.CarData newCar = EntityManager.GetComponentData<Game.Prefabs.CarData>(entity);

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

                    EntityManager.SetComponentData(entity, newCar);
                }
            }
        }

        private bool TryGetCarPrefab(Entity prefabEntity, out Game.Prefabs.CarPrefab carPrefab)
        {
            carPrefab = default!;

            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out Game.Prefabs.PrefabBase prefabBase))
            {
                return false;
            }

            if (prefabBase is Game.Prefabs.CarPrefab car)
            {
                carPrefab = car;
                return true;
            }

            return false;
        }
    }
}
