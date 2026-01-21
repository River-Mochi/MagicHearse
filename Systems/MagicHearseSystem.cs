// File: Systems/MagicHearseSystem.cs
// Purpose: Removes dead citizens that are waiting for transport.
// Notes:
// - Uses a simple ECS chunk job (no slicing or rolling windows).
// - Update interval throttles execution to avoid spikes.
// - Guaranteed to eventually clean all eligible citizens.

namespace MagicHearse
{
    using Game;                     // GameSystemBase, SystemUpdatePhase
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
            // Game ticksPerDay constant is 262144.
            return 262144 / UpdatesPerDay;
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

            Mod.Log.Info("MagicHearseSystem created.");
            RequireForUpdate(m_DeadCitizenQuery);
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
        private struct MagicHearseJob : IJobChunk
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
