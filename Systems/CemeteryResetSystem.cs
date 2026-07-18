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
//   baseline to preserve/restore (so no marker needed), and the write is NOT a structural change.
//   Net effect == vanilla "bulldoze + rebuild" minus the rebuild. No Harmony, no reflection.
// - Leaves the IsFull flag alone on purpose: DeathcareFacilityAISystem clears the flag AND removes the
//   "facility full" notification on its next tick once it sees room again. Clearing it here would orphan
//   that icon (the game only removes it while the flag is still set).
// - Continuous assist-style scan (same shape/idiom as MagicGarbage's GarbagePriorityAssistSystem and
//   MagicHearse's own FuneralDirectorSystem), NOT a one-shot: it must keep watching as buildings fill.
//   No Burst/job -- cities have very few cemeteries, so a stateless main-thread scan costs microseconds
//   and is far easier to diagnose. SystemAPI.Query completes the deathcare job dependency for us.

namespace MagicHearse
{
    using Game;                     // GameSystemBase, SystemUpdatePhase
    using Game.Buildings;           // DeathcareFacility, DeathcareFacilityFlags
    using Game.Common;              // Deleted
    using Game.Tools;               // Temp
    using Unity.Entities;           // SystemAPI, RefRW

    public sealed partial class CemeteryResetSystem : GameSystemBase
    {
        // Watchdog cadence in sim ticks (named-const idiom shared with GarbagePriorityAssistSystem).
        // 256 is the floor that loses nothing: the game's DeathcareFacilityAISystem sets AND clears the
        // IsFull flag + its notification only on its own 256-tick beat, so a faster scan cannot shorten
        // the "full" flash (the game will not clear the notification until its next 256-tick pass). Work
        // is also filtered to a tiny subset (full cemeteries only), so even this cadence is nearly free.
        public const int UpdateIntervalFrames = 256;

        protected override void OnCreate()
        {
            base.OnCreate();

            // Mod.OnLoad + the FD/AutoReset setters flip this on when both toggles are ON.
            Enabled = false;

            // Gate: only run in a city that actually has a deathcare building.
            RequireForUpdate<Game.Buildings.DeathcareFacility>();

#if DEBUG
            Mod.LogSafe(() => "[MH] CemeteryReset system created.");
#endif
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return UpdateIntervalFrames;
        }

        protected override void OnUpdate()
        {
            // Main-thread RefRW scan. SystemAPI.Query completes the deathcare job dependency for us, and
            // WithNone matches the game's own deathcare query so we see exactly the buildings whose IsFull
            // flag it maintains. Fully qualified to avoid the Game.Prefabs.DeathcareFacility name clash.
            foreach (RefRW<Game.Buildings.DeathcareFacility> facilityRef in SystemAPI
                         .Query<RefRW<Game.Buildings.DeathcareFacility>>()
                         .WithNone<Deleted, Temp>())
            {
                DeathcareFacility facility = facilityRef.ValueRO;

                // Only long-term cemeteries ever carry IsFull. Guard on count so we write once per fill
                // cycle (count is already 0 while we wait for the game to clear the flag).
                if ((facility.m_Flags & DeathcareFacilityFlags.IsFull) != 0 && facility.m_LongTermStoredCount > 0)
                {
                    facilityRef.ValueRW.m_LongTermStoredCount = 0;
                }
            }
        }
    }
}
