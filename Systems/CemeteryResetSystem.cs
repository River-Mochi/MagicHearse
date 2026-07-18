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
//   Independent of the Cemetery-storage slider (which scales PREFAB capacity), so the two compose.
// - Why this instance write is safe (InstanceEntities.md "Strategy 3"): 0 is the game's own valid
//   "empty cemetery" state (every cemetery starts there and the AI system handles count==0 natively),
//   only DeathcareFacilityAISystem touches this field so we are not fighting a recompute, there is no
//   baseline to preserve/restore (so no marker needed), and SetComponentData is NOT a structural change.
//   Net effect == vanilla "bulldoze + rebuild" minus the rebuild. No Harmony, no reflection.
// - Leaves the IsFull flag alone on purpose: DeathcareFacilityAISystem clears the flag AND removes the
//   "facility full" notification on its next tick once it sees room again. Clearing it here would orphan
//   that icon (the game only removes it while the flag is still set).
// - No Burst / job: cities have only a handful of cemeteries (well under 50), so a throttled main-thread
//   scan costs microseconds and is far easier to diagnose. ToComponentDataArray + SetComponentData
//   complete the deathcare job dependency for us (safe reads/writes, no torn reads).

namespace MagicHearse
{
    using Game;                     // GameSystemBase, SystemUpdatePhase
    using Game.Buildings;           // DeathcareFacility, DeathcareFacilityFlags
    using Game.Common;              // Deleted
    using Game.Tools;               // Temp
    using Unity.Collections;        // NativeArray, Allocator
    using Unity.Entities;           // EntityQuery, SystemAPI, EntityManager

    public sealed partial class CemeteryResetSystem : GameSystemBase
    {
        private EntityQuery m_CemeteryQuery;

        // Cemeteries fill over in-game weeks and cities have very few of them, so a light cadence
        // is plenty (matches MagicHearseSystem's constant for consistency).
        public const int UpdatesPerDay = 256;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            // Game ticksPerDay constant = 262144.
            return 262144 / UpdatesPerDay;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            // Mod.OnLoad + the FD/AutoReset setters flip this on when both toggles are ON.
            Enabled = false;

            // Game.Buildings.DeathcareFacility (runtime instance component) lives only on placed
            // deathcare buildings; exclude preview/removed. Fully qualified to avoid the
            // Game.Prefabs.DeathcareFacility name clash.
            m_CemeteryQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Buildings.DeathcareFacility>()
                .WithNone<Deleted, Temp>()
                .Build();

#if DEBUG
            Mod.LogSafe(() => "[MH] CemeteryReset system created.");
#endif

            RequireForUpdate(m_CemeteryQuery);
        }

        protected override void OnUpdate()
        {
            // Both arrays iterate the query in the same chunk order, so index i lines up.
            // ToComponentDataArray completes the read dependency; SetComponentData completes the write.
            NativeArray<Entity> entities = m_CemeteryQuery.ToEntityArray(Allocator.Temp);
            NativeArray<DeathcareFacility> facilities =
                m_CemeteryQuery.ToComponentDataArray<DeathcareFacility>(Allocator.Temp);

            try
            {
                for (int i = 0; i < facilities.Length; i++)
                {
                    DeathcareFacility facility = facilities[i];

                    // Only long-term cemeteries ever carry IsFull. Guard on count so we write once
                    // per fill cycle (count is already 0 while we wait for the game to clear the flag).
                    bool full = (facility.m_Flags & DeathcareFacilityFlags.IsFull) != 0;

                    if (full && facility.m_LongTermStoredCount > 0)
                    {
                        facility.m_LongTermStoredCount = 0;
                        EntityManager.SetComponentData(entities[i], facility);
                    }
                }
            }
            finally
            {
                entities.Dispose();
                facilities.Dispose();
            }
        }
    }
}
