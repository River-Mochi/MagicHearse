// File: Systems/MagicHearseSystem.cs
// Purpose: Removes dead citizens that are waiting for transport.

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

        public static readonly int UpdatesPerDay = 128;   // 128 = low frequency; increase to clean faster.

        // Lower reduces spike risk, but makes initial cleanup slower.
        private const int MaxChunksPerUpdate = 32;

        // Rolling window start for chunk slicing.
        private int m_ChunkStart;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 262144 / UpdatesPerDay; // Game ticksPerDay constant is 262144.
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_DeadCitizenQuery = SystemAPI.QueryBuilder()
                .WithAll<Citizen, HealthProblem>()
                .WithNone<Deleted, Temp>()
                .Build();

            m_EndFrameBarrier = World.GetOrCreateSystemManaged<EndFrameBarrier>();

            m_ChunkStart = 0;

            Mod.Log.Info("MagicHearseSystem created.");
            RequireForUpdate(m_DeadCitizenQuery);
        }

        protected override void OnUpdate()
        {
            int chunkCount = m_DeadCitizenQuery.CalculateChunkCountWithoutFiltering();
            if (chunkCount <= 0)
            {
                m_ChunkStart = 0;
                return;
            }

            int sliceStart = m_ChunkStart;
            int sliceCount = MaxChunksPerUpdate;

            // Wrap the window so repeated updates eventually cover all chunks.
            m_ChunkStart += sliceCount;
            if (m_ChunkStart >= chunkCount)
            {
                m_ChunkStart = 0;
            }

            JobHandle handle = new MagicHearseJob
            {
                m_EntityTypeHandle = SystemAPI.GetEntityTypeHandle(),
                m_HealthProblemType = SystemAPI.GetComponentTypeHandle<HealthProblem>(isReadOnly: true),
                m_CommandBuffer = m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),

                m_SliceStart = sliceStart,
                m_SliceEndExclusive = sliceStart + sliceCount,
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

            // Chunk slicing window.
            [ReadOnly] public int m_SliceStart;
            [ReadOnly] public int m_SliceEndExclusive;

            public void Execute(
                in ArchetypeChunk chunk,
                int unfilteredChunkIndex,
                bool useEnabledMask,
                in v128 chunkEnabledMask)
            {
                // Time-slice: skip chunks outside the current window.
                // Note: unfilteredChunkIndex ordering is stable enough for workload spreading,
                // but it is not a guarantee of perfect fairness across archetypes.
                if (unfilteredChunkIndex < m_SliceStart || unfilteredChunkIndex >= m_SliceEndExclusive)
                {
                    return;
                }

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
