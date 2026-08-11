// <copyright file="HearseWarningDelaySystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/HearseWarningDelaySystem.cs
// Purpose: Applies a hearse-only warning delay without changing the shared ambulance timer.

namespace MagicHearse
{
    using CS2Shared.RiverMochi; // LogUtils
    using Game;                  // GameSystemBase
    using Game.Citizens;         // HealthProblem, HealthProblemFlags
    using Game.Common;           // Deleted
    using Game.Notifications;    // IconCommandBuffer, IconCommandSystem, IconPriority
    using Game.Prefabs;          // HealthcareParameterData
    using Game.Simulation;       // SimulationSystem
    using Game.Tools;            // Temp
    using Unity.Collections;     // Allocator, NativeArray
    using Unity.Entities;        // Entity, EntityCommandBuffer, IComponentData, SystemAPI

    /// <summary>
    /// Per-corpse runtime progress used only while Funeral Director extends the vanilla warning.
    /// </summary>
    internal struct MHWarningDelay : IComponentData
    {
        public int VanillaTimerLimit;
        public int ExtraTicksElapsed;
        public uint EstimatedWaitStartFrame;
        public bool WaitEstimateInitialized;
        public bool VanillaReached;
        public bool Completed;
    }

    /// <summary>
    /// Runs immediately after HealthProblemSystem. It lets the vanilla timer advance normally,
    /// then suppresses only the hearse icon until the configured target is reached.
    /// </summary>
    public sealed partial class HearseWarningDelaySystem : GameSystemBase
    {
        private const float kTimerTicksPerSimulationSecond = 15f / 64f;
        private const int kSecondsPerMinute = 60;
        private const int kSimulationFramesPerTimerTick = 256;

        private EntityQuery m_HealthcareSettingsQuery;
        private EntityQuery m_TrackedQuery;
        private EntityQuery m_TrackedWithoutHealthProblemQuery;
        private IconCommandSystem m_IconCommandSystem = null!;
        private SimulationSystem m_SimulationSystem = null!;

#if DEBUG
        private bool m_DebugLiveConfigLogPending;
        private Colossal.Serialization.Entities.Purpose m_DebugLoadPurpose;
        private GameMode m_DebugLoadMode;
#endif

        protected override void OnCreate()
        {
            base.OnCreate();

            m_IconCommandSystem = World.GetOrCreateSystemManaged<IconCommandSystem>();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();

            m_HealthcareSettingsQuery = SystemAPI.QueryBuilder()
                .WithAll<HealthcareParameterData>()
                .Build();

            m_TrackedQuery = SystemAPI.QueryBuilder()
                .WithAll<MHWarningDelay>()
                .Build();

            m_TrackedWithoutHealthProblemQuery = SystemAPI.QueryBuilder()
                .WithAll<MHWarningDelay>()
                .WithNone<HealthProblem>()
                .Build();
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 16;
        }

#if DEBUG
        protected override void OnGameLoadingComplete(
            Colossal.Serialization.Entities.Purpose purpose,
            GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            if ((mode & GameMode.Game) == 0)
            {
                return;
            }

            m_DebugLoadPurpose = purpose;
            m_DebugLoadMode = mode;
            m_DebugLiveConfigLogPending = true;
            TryLogLiveConfiguration();
        }

        private void TryLogLiveConfiguration()
        {
            if (!m_DebugLiveConfigLogPending || m_HealthcareSettingsQuery.IsEmptyIgnoreFilter)
            {
                return;
            }

            HealthcareParameterData healthcareParameters =
                m_HealthcareSettingsQuery.GetSingleton<HealthcareParameterData>();
            float warningSeconds = healthcareParameters.m_TransportWarningTime;
            int timerLimit = (int)(warningSeconds * kTimerTicksPerSimulationSecond);
            float quantizedSeconds = timerLimit / kTimerTicksPerSimulationSecond;

            LogUtils.Info(() =>
                $"[HearseWarning] Live city configuration: " +
                $"m_TransportWarningTime={warningSeconds:0.###} simulation seconds, " +
                $"HealthProblem.m_Timer limit={timerLimit}, " +
                $"quantized icon delay={quantizedSeconds:0.###} simulation seconds, " +
                $"purpose={m_DebugLoadPurpose}, mode={m_DebugLoadMode}.");

            m_DebugLiveConfigLogPending = false;
        }
#endif

        protected override void OnUpdate()
        {
            CompleteDependency();

#if DEBUG
            TryLogLiveConfiguration();
#endif

            MHSetting? setting = Mod.Settings;
            if (setting == null || !setting.FuneralDirector || m_HealthcareSettingsQuery.IsEmptyIgnoreFilter)
            {
                ClearTracking();
                return;
            }

            HealthcareParameterData healthcareParameters =
                m_HealthcareSettingsQuery.GetSingleton<HealthcareParameterData>();
            Entity hearseNotificationPrefab =
                healthcareParameters.m_HearseNotificationPrefab;

            // CWD may disable this prefab's display component. Do not inspect or change that
            // enableable state; only require the game-owned prefab entity itself to be valid.
            if (hearseNotificationPrefab == Entity.Null ||
                !EntityManager.Exists(hearseNotificationPrefab))
            {
                ClearTracking();
                return;
            }

            int vanillaTimerLimit =
                (int)(healthcareParameters.m_TransportWarningTime * kTimerTicksPerSimulationSecond);
            int targetTimerLimit =
                (int)(setting.HearseWarningMinutes * kSecondsPerMinute * kTimerTicksPerSimulationSecond);

            // A non-positive or byte-unreachable vanilla threshold cannot be extended safely here.
            if (vanillaTimerLimit <= 0 || vanillaTimerLimit > byte.MaxValue)
            {
                ClearTracking();
                return;
            }

            EntityCommandBuffer ecb = new(Allocator.Temp);
            IconCommandBuffer iconCommands = m_IconCommandSystem.CreateCommandBuffer();

            RemoveInvalidTracking(ref ecb);
            UpdateTrackedCorpses(
                ref ecb,
                ref iconCommands,
                hearseNotificationPrefab,
                vanillaTimerLimit,
                targetTimerLimit);
            TrackNewCorpses(
                ref ecb,
                ref iconCommands,
                hearseNotificationPrefab,
                vanillaTimerLimit,
                targetTimerLimit);

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void UpdateTrackedCorpses(
            ref EntityCommandBuffer ecb,
            ref IconCommandBuffer iconCommands,
            Entity hearseNotificationPrefab,
            int vanillaTimerLimit,
            int targetTimerLimit)
        {
            foreach ((RefRW<HealthProblem> health, RefRW<MHWarningDelay> delay, Entity entity) in SystemAPI
                         .Query<RefRW<HealthProblem>, RefRW<MHWarningDelay>>()
                         .WithAll<Citizen>()
                         .WithNone<Deleted, Temp>()
                         .WithEntityAccess())
            {
                HealthProblemFlags required =
                    HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;
                if ((health.ValueRO.m_Flags & required) != required)
                {
                    ecb.RemoveComponent<MHWarningDelay>(entity);
                    continue;
                }

                int timer = health.ValueRO.m_Timer;
                if (!delay.ValueRO.WaitEstimateInitialized)
                {
                    // Handles an in-memory component left by an older Debug build that did not
                    // yet contain the wait-estimate fields.
                    delay.ValueRW.EstimatedWaitStartFrame = unchecked(
                        m_SimulationSystem.frameIndex -
                        (uint)(timer * kSimulationFramesPerTimerTick));
                    delay.ValueRW.WaitEstimateInitialized = true;
                }

                if (delay.ValueRO.Completed)
                {
                    continue;
                }

                if (delay.ValueRO.VanillaTimerLimit != vanillaTimerLimit)
                {
                    delay.ValueRW.VanillaTimerLimit = vanillaTimerLimit;
                    delay.ValueRW.ExtraTicksElapsed = 0;
                    delay.ValueRW.VanillaReached = false;
                }

                if (targetTimerLimit < vanillaTimerLimit)
                {
                    if (timer >= targetTimerLimit)
                    {
                        iconCommands.Add(
                            entity,
                            hearseNotificationPrefab,
                            IconPriority.MajorProblem);
                        delay.ValueRW.Completed = true;
                    }

                    continue;
                }

                if (targetTimerLimit == vanillaTimerLimit)
                {
                    if (timer >= vanillaTimerLimit)
                    {
                        delay.ValueRW.Completed = true;
                    }

                    continue;
                }

                if (timer < vanillaTimerLimit)
                {
                    continue;
                }

                int extraTicksRequired = targetTimerLimit - vanillaTimerLimit;
                if (!delay.ValueRO.VanillaReached)
                {
                    delay.ValueRW.VanillaReached = true;
                }
                else
                {
                    delay.ValueRW.ExtraTicksElapsed++;
                }

                if (delay.ValueRO.ExtraTicksElapsed >= extraTicksRequired)
                {
                    delay.ValueRW.Completed = true;
                    continue;
                }

                // HealthProblemSystem just queued the vanilla icon at this threshold. Because this
                // system's icon buffer is created later, its remove command wins in playback order.
                iconCommands.Remove(entity, hearseNotificationPrefab);
                health.ValueRW.m_Timer = (byte)(vanillaTimerLimit - 1);
            }
        }

        private void TrackNewCorpses(
            ref EntityCommandBuffer ecb,
            ref IconCommandBuffer iconCommands,
            Entity hearseNotificationPrefab,
            int vanillaTimerLimit,
            int targetTimerLimit)
        {
            foreach ((RefRO<HealthProblem> health, Entity entity) in SystemAPI
                         .Query<RefRO<HealthProblem>>()
                         .WithAll<Citizen>()
                         .WithNone<MHWarningDelay, Deleted, Temp>()
                         .WithEntityAccess())
            {
                HealthProblemFlags required =
                    HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;
                if ((health.ValueRO.m_Flags & required) != required)
                {
                    continue;
                }

                int timer = health.ValueRO.m_Timer;
                MHWarningDelay delay = new()
                {
                    VanillaTimerLimit = vanillaTimerLimit,
                    // The timer supplies a useful pre-load seed. From this point forward the
                    // simulation frame provides a 32-bit secondary counter without touching
                    // the game's byte-sized HealthProblem timer.
                    EstimatedWaitStartFrame = unchecked(
                        m_SimulationSystem.frameIndex -
                        (uint)(timer * kSimulationFramesPerTimerTick)),
                    WaitEstimateInitialized = true,
                };

                if (targetTimerLimit > vanillaTimerLimit && timer >= vanillaTimerLimit)
                {
                    // Do not hide a warning that was already visible when tracking began.
                    delay.Completed = true;
                }
                else if (targetTimerLimit < vanillaTimerLimit && timer >= targetTimerLimit)
                {
                    // A shorter selected delay applies immediately to an existing wait.
                    iconCommands.Add(
                        entity,
                        hearseNotificationPrefab,
                        IconPriority.MajorProblem);
                    delay.Completed = true;
                }
                else if (targetTimerLimit == vanillaTimerLimit &&
                         timer >= vanillaTimerLimit)
                {
                    delay.Completed = true;
                }

                ecb.AddComponent(entity, delay);
            }
        }

        private void RemoveInvalidTracking(ref EntityCommandBuffer ecb)
        {
            using NativeArray<Entity> entities =
                m_TrackedWithoutHealthProblemQuery.ToEntityArray(Allocator.Temp);
            foreach (Entity entity in entities)
            {
                ecb.RemoveComponent<MHWarningDelay>(entity);
            }
        }

        private void ClearTracking()
        {
            if (m_TrackedQuery.IsEmptyIgnoreFilter)
            {
                return;
            }

            using NativeArray<Entity> entities = m_TrackedQuery.ToEntityArray(Allocator.Temp);
            EntityCommandBuffer ecb = new(Allocator.Temp);
            foreach (Entity entity in entities)
            {
                ecb.RemoveComponent<MHWarningDelay>(entity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}
