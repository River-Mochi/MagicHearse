// <copyright file="CemeteryResetSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/CemeteryResetSystem.cs
// Purpose: Auto-empties cemeteries the moment the game flags them full (Funeral Director option).
// Notes:
// - Instance-level: zeroes Game.Buildings.DeathcareFacility.m_LongTermStoredCount on placed buildings.
//   This is independent of the Cemetery-storage slider (which scales PREFAB capacity), so the two compose.
// - Reset semantics == vanilla "bulldoze + rebuild" minus the rebuild; only UI reads the stored count.
// - Leaves the IsFull flag alone on purpose: the game's DeathcareFacilityAISystem clears the flag AND
//   removes the "facility full" notification on its next tick once it sees room again. Clearing the flag
//   here would orphan that notification icon.
// - Continuously throttled scanner (like MagicHearseSystem). Enabled only while FD + AutoReset are ON.

namespace MagicHearse
{
    using Game;                     // GameSystemBase, SystemUpdatePhase
    using Game.Buildings;           // DeathcareFacility, DeathcareFacilityFlags
    using Game.Common;              // Deleted
    using Game.Tools;               // Temp
    using Unity.Burst;              // BurstCompile
    using Unity.Burst.Intrinsics;   // v128
    using Unity.Collections;        // NativeArray
    using Unity.Entities;           // EntityQuery, SystemAPI, IJobChunk
    using Unity.Jobs;               // JobHandle

    public sealed partial class CemeteryResetSystem : GameSystemBase
    {
        private EntityQuery m_CemeteryQuery;

        // Cemeteries fill slowly, so a modest cadence is plenty (matches MagicHearseSystem).
        public const int UpdatesPerDay = 256;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            // Game ticksPerDay constant = 262144.
            return 262144 / UpdatesPerDay;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            // Mod.OnLoad + the AutoReset/FD setters flip this on when both toggles are ON.
            Enabled = false;

            // DeathcareFacility only lives on placed deathcare buildings; exclude preview/removed.
            m_CemeteryQuery = SystemAPI.QueryBuilder()
                .WithAll<DeathcareFacility>()
                .WithNone<Deleted, Temp>()
                .Build();

#if DEBUG
            Mod.LogSafe(() => "[MH] CemeteryReset system created.");
#endif

            RequireForUpdate(m_CemeteryQuery);
        }

        protected override void OnUpdate()
        {
            JobHandle handle = new CemeteryResetJob
            {
                m_DeathcareFacilityType = SystemAPI.GetComponentTypeHandle<DeathcareFacility>(isReadOnly: false),
            }.ScheduleParallel(m_CemeteryQuery, Dependency);

            Dependency = handle;
        }

        [BurstCompile]
        private struct CemeteryResetJob : IJobChunk
        {
            public ComponentTypeHandle<DeathcareFacility> m_DeathcareFacilityType;

            public void Execute(
                in ArchetypeChunk chunk,
                int unfilteredChunkIndex,
                bool useEnabledMask,
                in v128 chunkEnabledMask)
            {
                _ = unfilteredChunkIndex;
                _ = useEnabledMask;
                _ = chunkEnabledMask;

                NativeArray<DeathcareFacility> facilities = chunk.GetNativeArray(ref m_DeathcareFacilityType);

                for (int i = 0; i < facilities.Length; i++)
                {
                    DeathcareFacility facility = facilities[i];

                    // Only long-term storage buildings ever get IsFull. Guard on count so we write
                    // once per fill cycle (count is already 0 while we wait for the game to clear
                    // the flag) and leave the flag for the game to reset + drop the notification.
                    bool full = (facility.m_Flags & DeathcareFacilityFlags.IsFull) != 0;

                    if (full && facility.m_LongTermStoredCount > 0)
                    {
                        facility.m_LongTermStoredCount = 0;
                        facilities[i] = facility;
                    }
                }
            }
        }
    }
}
