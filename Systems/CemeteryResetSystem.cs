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
    using Game;                            // GameSystemBase, SystemUpdatePhase, GameMode
    using Game.Buildings;                  // DeathcareFacility, DeathcareFacilityFlags
    using Game.Common;                     // Deleted
    using Game.Tools;                      // Temp
    using Unity.Entities;                  // SystemAPI, RefRW, Entity

    public sealed partial class CemeteryResetSystem : GameSystemBase
    {
        // 128 simulation frames catches full cemeteries quickly; cities normally have few to scan.
        public const int kUpdateIntervalFrames = 128;

        /// <summary>One cemetery's session tally: how many times it was emptied, plus its display name.</summary>
        public struct Tally
        {
            public int Count;
            public string Name;
        }

        private Game.UI.NameSystem m_NameSystem = null!;

        // Tallies are session-scoped because Entity keys are only valid in the current city.
        private int m_SessionResetTotal;
        private readonly Dictionary<Entity, Tally> m_Tallies = new();

        // Collect reset entities so name lookup stays outside the RefRW scan.
        private readonly List<Entity> m_JustReset = new();

        public int SessionResetTotal => m_SessionResetTotal;

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

            // Mod.OnLoad and the setting setters enable this when either active mode wants cemetery reset.
            Enabled = false;

            m_NameSystem = World.GetOrCreateSystemManaged<Game.UI.NameSystem>();

            RequireForUpdate<DeathcareFacility>();

#if DEBUG
            CS2Shared.RiverMochi.LogUtils.Info(() => "[MH] CemeteryReset system created.");
#endif
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

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
            foreach ((RefRW<DeathcareFacility> facilityRef, Entity entity) in SystemAPI
                         .Query<RefRW<DeathcareFacility>>()
                         .WithNone<Deleted, Temp>()
                         .WithEntityAccess())
            {
                DeathcareFacility facility = facilityRef.ValueRO;

                // Count must still be positive so this runs only once per fill cycle.
                if ((facility.m_Flags & DeathcareFacilityFlags.IsFull) != 0 &&
                    facility.m_LongTermStoredCount > 0)
                {
                    // Clear the placed cemetery only; the storage slider changes prefab capacity.
                    // Leave IsFull alone so vanilla clears it and its notification on the next tick.
                    facilityRef.ValueRW.m_LongTermStoredCount = 0;
                    m_JustReset.Add(entity);
                }
            }

            for (int i = 0; i < m_JustReset.Count; i++)
            {
                RecordReset(m_JustReset[i]);
            }

            m_JustReset.Clear();
        }

        private void RecordReset(Entity cemetery)
        {
            m_SessionResetTotal++;

            // Name lookup failure must never stop cemetery resetting.
            string name;
            try
            {
                name = m_NameSystem.GetRenderedLabelName(cemetery);
            }
            catch
            {
                name = string.Empty;
            }

            if (m_Tallies.TryGetValue(cemetery, out Tally tally))
            {
                tally.Count++;
                tally.Name = name;
                m_Tallies[cemetery] = tally;
            }
            else
            {
                m_Tallies[cemetery] = new Tally
                {
                    Count = 1,
                    Name = name,
                };
            }
        }
    }
}
