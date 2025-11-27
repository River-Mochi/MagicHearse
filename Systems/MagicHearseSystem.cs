// Systems/MagicHearseSystem.cs
// ECS system that finds dead + waiting citizens and deletes them.

namespace MagicHearse
{
    using Game;
    using Game.Citizens;
    using Game.Common;
    using Game.Tools;
    using Unity.Burst;
    using Unity.Burst.Intrinsics;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Jobs;

    public sealed partial class MagicHearseSystem : GameSystemBase
    {
        private EntityQuery m_DeadCitizenQuery;
        private EndFrameBarrier m_EndFrameBarrier = null!; // set in OnCreate

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 64;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

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
                m_HealthProblemType = SystemAPI.GetComponentTypeHandle<HealthProblem>(true),
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

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                NativeArray<Entity> citizens = chunk.GetNativeArray(m_EntityTypeHandle);
                NativeArray<HealthProblem> health = chunk.GetNativeArray(ref m_HealthProblemType);

                for (var i = 0; i < citizens.Length; i++)
                {
                    HealthProblemFlags flags = health[i].m_Flags;

                    var isDeadAndWaiting =
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
