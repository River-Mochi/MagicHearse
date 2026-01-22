// File: Systems/MagicHearseSystem.cs
// Purpose: Removes dead citizens that are waiting for transport.
// Notes:
// - Uses a simple ECS chunk job.
// - Update interval throttles execution to avoid spikes.
// - Guaranteed to eventually clean all eligible citizens.

namespace MagicHearse
{
   // using Colossal.Serialization.Entities; // Purpose (inline this to avoid ambiguity with Citizens).
    using Game;                     // GameSystemBase, SystemUpdatePhase, GameMode
    using Game.Citizens;            // Citizen, HealthProblem
    using Game.Common;              // Deleted, Temp
    using Game.Tools;               // EndFrameBarrier
    using Unity.Burst;              // BurstCompile
    using Unity.Burst.Intrinsics;   // v128
    using Unity.Collections;        // NativeArray, ReadOnly
    using Unity.Entities;           // EntityQuery, SystemAPI, EntityCommandBuffer
    using Unity.Jobs;               // JobHandle

    public sealed partial class MagicHearseSystem : GameSystemBase
    {
        private EntityQuery m_DeadCitizenQuery;
        private EndFrameBarrier m_EndFrameBarrier = null!; // assigned in OnCreate

        // Lower frequency reduces spike risk; increase to clean faster.
        public static readonly int UpdatesPerDay = 128;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {        
            return 262144 / UpdatesPerDay;   // Game ticksPerDay constant = 262144.
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            // Query: dead citizens waiting for transport, not already deleted.
            m_DeadCitizenQuery = SystemAPI.QueryBuilder()
                .WithAll<Citizen, HealthProblem>()
                .WithNone<Deleted, Temp>()
                .Build();

            m_EndFrameBarrier = World.GetOrCreateSystemManaged<EndFrameBarrier>();

            Mod.Log.Info("MH System created.");
            RequireForUpdate(m_DeadCitizenQuery);
        }

        protected override void OnGameLoadingComplete(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            // City load complete or city switched:
            // reset cached Status so OptionsUI will refresh for THIS city.
            DeathcareStatus.InvalidateCache();
        }

        protected override void OnUpdate()
        {
            JobHandle handle = new MagicHearseJob
            {
                m_EntityTypeHandle = SystemAPI.GetEntityTypeHandle(),
                m_HealthProblemType = SystemAPI.GetComponentTypeHandle<HealthProblem>(isReadOnly: true),
                m_CommandBuffer = m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
            }.ScheduleParallel(m_DeadCitizenQuery, Dependency);

            m_EndFrameBarrier.AddJobHandleForProducer(handle);
            Dependency = handle;
        }

        [BurstCompile]
        private struct MagicHearseJob : Unity.Entities.IJobChunk
        {
            [ReadOnly] public EntityTypeHandle m_EntityTypeHandle;
            [ReadOnly] public ComponentTypeHandle<HealthProblem> m_HealthProblemType;
            public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

            public void Execute(
                in ArchetypeChunk chunk,
                int unfilteredChunkIndex,
                bool useEnabledMask,
                in v128 chunkEnabledMask)
            {
                NativeArray<Entity> citizens = chunk.GetNativeArray(m_EntityTypeHandle);
                NativeArray<HealthProblem> health = chunk.GetNativeArray(ref m_HealthProblemType);

                for (int i = 0; i < citizens.Length; i++)
                {
                    HealthProblemFlags flags = health[i].m_Flags;

                    bool isDeadAndWaiting =
                        (flags & (HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport)) ==
                        (HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport);

                    if (isDeadAndWaiting)
                    {
                        m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, citizens[i]);
                    }
                }
            }
        }
    }
}
