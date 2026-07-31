// <copyright file="MagicHearseSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/MagicHearseSystem.cs
// Purpose: Removes dead corpses that are waiting for transport.

namespace MagicHearse
{
    using Game;                     // GameSystemBase, SystemUpdatePhase, GameMode
    using Game.Citizens;            // Citizen, HealthProblem
    using Game.Common;              // Deleted, Temp
    using Game.Tools;               // EndFrameBarrier
    using Unity.Burst;              // BurstCompile
    using Unity.Burst.Intrinsics;   // v128
    using Unity.Collections;        // NativeArray, ReadOnly
    using Unity.Entities;           // EntityQuery, SystemAPI, EntityCommandBuffer, IJobChunk
    using Unity.Jobs;               // JobHandle

    public sealed partial class MagicHearseSystem : GameSystemBase
    {
        private EntityQuery m_DeadCitizenQuery;
        private EndFrameBarrier m_EndFrameBarrier = null!; // assigned in OnCreate

        // Lower frequency reduces spike risk; increase cleans faster.
        public const int kUpdatesPerDay = 256;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            // Game ticksPerDay constant = 262144.
            return 262144 / kUpdatesPerDay;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_DeadCitizenQuery = SystemAPI.QueryBuilder()
                .WithAll<Citizen, HealthProblem>()
                .WithNone<Deleted, Temp>()
                .Build();

            m_EndFrameBarrier = World.GetOrCreateSystemManaged<EndFrameBarrier>();

#if DEBUG
            CS2Shared.RiverMochi.LogUtils.Info(() => "MH System created.");
#endif

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
                _ = useEnabledMask;
                _ = chunkEnabledMask;

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
