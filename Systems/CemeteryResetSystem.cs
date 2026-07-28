// <copyright file="CemeteryResetSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/CemeteryResetSystem.cs
// Purpose: Empties full cemeteries for either active mode and tracks session reset counts.

namespace MagicHearse
{
    using System.Collections.Generic;      // Dictionary, List
    using Colossal.Serialization.Entities; // Purpose
    using CS2Shared.RiverMochi;            // LogUtils, DEBUG build
    using Game;                            // GameSystemBase, SystemUpdatePhase, GameMode
    using Game.Buildings;                  // DeathcareFacility, DeathcareFacilityFlags
    using Game.Common;                     // Deleted
    using Game.Tools;                      // Temp
    using Unity.Entities;                  // SystemAPI, RefRW, Entity

    public sealed partial class CemeteryResetSystem : GameSystemBase
    {
        // Frequent enough to catch full cemeteries quickly; query is tiny.
        public const int kUpdateIntervalFrames = 128;

        /// <summary>One cemetery's session tally: how many times it was emptied, plus its display name.</summary>
        public struct Tally
        {
            public int Count;
            public string Name;
        }

        private Game.UI.NameSystem m_NameSystem = null!;

        // Session tally, cleared on city load/switch. Entity-keyed -> must be cleared there (stale otherwise).
        private int m_SessionResetTotal;
        private readonly Dictionary<Entity, Tally> m_Tallies = new Dictionary<Entity, Tally>();

        // Reused per-update buffer so name resolution happens AFTER the RefRW scan, not during it.
        private readonly List<Entity> m_JustReset = new List<Entity>();

        /// <summary>Total cemetery resets this session (all cemeteries).</summary>
        public int SessionResetTotal => m_SessionResetTotal;

        /// <summary>How many distinct cemeteries were emptied this session.</summary>
        public int DistinctCemeteryCount => m_Tallies.Count;

        /// <summary>Fills <paramref name="buffer"/> with up to <paramref name="max"/> most-emptied cemeteries
        /// (count descending). Called only while the Options Status report is open.</summary>
        public void CopyTopEmptied(List<Tally> buffer, int max)
        {
            buffer.Clear();

            foreach (KeyValuePair<Entity, Tally> kv in m_Tallies)
            {
                buffer.Add(kv.Value);
            }

            buffer.Sort((a, b) => b.Count.CompareTo(a.Count));

            if (buffer.Count > max)
            {
                buffer.RemoveRange(max, buffer.Count - max);
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            // Mod.OnLoad + the FD/AutoReset setters flip this on when both toggles are ON.
            Enabled = false;

            m_NameSystem = World.GetOrCreateSystemManaged<Game.UI.NameSystem>();

            // Gate: only run in a city that actually has a deathcare building.
            RequireForUpdate<Game.Buildings.DeathcareFacility>();

#if DEBUG
            LogUtils.Info(() => "[MH] CemeteryReset system created.");
#endif
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            // Session-scoped tally: clear on every city load/switch so it never carries across cities
            // (and so the Entity keys never go stale). Matches how the rest of the Status report resets.
            m_SessionResetTotal = 0;
            m_Tallies.Clear();
            m_JustReset.Clear();
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return kUpdateIntervalFrames;
        }

        protected override void OnUpdate()
        {
            // Few cemeteries, this main-thread scan is cheap.
            // SystemAPI completes the component dependency before the RefRW scan.
            foreach ((RefRW<Game.Buildings.DeathcareFacility> facilityRef, Entity entity) in SystemAPI
                         .Query<RefRW<Game.Buildings.DeathcareFacility>>()
                         .WithNone<Deleted, Temp>()
                         .WithEntityAccess())
            {
                DeathcareFacility facility = facilityRef.ValueRO;

                // Only long-term cemeteries ever carry IsFull. Guard on count so we write once per fill
                // cycle (count is already 0 while we wait for the game to clear the flag).
                if ((facility.m_Flags & DeathcareFacilityFlags.IsFull) != 0 && facility.m_LongTermStoredCount > 0)
                {
                    // Clears a placed cemetery only; storage slider changes prefab capacity.
                    // Leave IsFull alone so vanilla also clears the full notification next tick.
                    facilityRef.ValueRW.m_LongTermStoredCount = 0;
                    m_JustReset.Add(entity);
                }
            }

            // Tally after the write-scan so NameSystem reads happen outside the RefRW query iteration.
            // This only runs on the rare pass where a cemetery actually filled up.
            for (int i = 0; i < m_JustReset.Count; i++)
            {
                RecordReset(m_JustReset[i]);
            }

            m_JustReset.Clear();
        }

        private void RecordReset(Entity cemetery)
        {
            m_SessionResetTotal++;

            // Name resolution touches localization; keep it from ever breaking the scan.
            string name;
            try { name = m_NameSystem.GetRenderedLabelName(cemetery); }
            catch { name = string.Empty; }

            if (m_Tallies.TryGetValue(cemetery, out Tally tally))
            {
                tally.Count++;
                tally.Name = name;
                m_Tallies[cemetery] = tally;
            }
            else
            {
                m_Tallies[cemetery] = new Tally { Count = 1, Name = name };
            }
        }
    }
}
