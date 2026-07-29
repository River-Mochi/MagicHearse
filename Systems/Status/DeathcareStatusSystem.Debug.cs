// <copyright file="DeathcareStatusSystem.Debug.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Status/DeathcareStatusSystem.Debug.cs
// Purpose: DEBUG-only Scene Explorer-style samples written by the Log Report button.

#if DEBUG
namespace MagicHearse
{
    using System.Text;         // StringBuilder
    using Game.Citizens;      // HealthProblem, CurrentBuilding/Transport, TravelPurpose
    using Game.Creatures;     // CurrentVehicle
    using Game.Pathfind;      // PathInformation
    using Game.Simulation;    // HealthcareRequest, ServiceRequest, Dispatched, UpdateFrame
    using Unity.Collections;  // NativeArray, Allocator
    using Unity.Entities;     // Entity, ComponentLookup

    public sealed partial class DeathcareStatusSystem
    {
        public string BuildDebugRequestSamples()
        {
            CompleteDependency();

            const int SamplesPerStage = 3;
            const HealthProblemFlags Want =
                HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;

            CorpseLookups lookups = CreateCorpseLookups();
            ComponentLookup<TravelPurpose> travelPurposeLookup =
                GetComponentLookup<TravelPurpose>(true);

            int[] sampleCounts = new int[(int)CorpseStage.Count];
            StringBuilder?[] samples = new StringBuilder?[(int)CorpseStage.Count];

            using (NativeArray<Entity> citizens =
                m_DeadTransportQuery.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < citizens.Length; i++)
                {
                    Entity citizen = citizens[i];
                    if (!lookups.HealthProblem.TryGetComponent(
                            citizen,
                            out HealthProblem healthProblem) ||
                        (healthProblem.m_Flags & Want) != Want)
                    {
                        continue;
                    }

                    CorpseStage stage = ClassifyCorpse(
                        citizen,
                        healthProblem,
                        in lookups,
                        out bool outsideService);

                    int stageIndex = (int)stage;
                    if (sampleCounts[stageIndex] >= SamplesPerStage)
                    {
                        continue;
                    }

                    sampleCounts[stageIndex]++;
                    StringBuilder sample =
                        samples[stageIndex] ??= new StringBuilder();

                    AppendDebugSample(
                        sample,
                        citizen,
                        healthProblem,
                        stage,
                        outsideService,
                        in lookups,
                        travelPurposeLookup);
                }
            }

            StringBuilder report = new();
            report.AppendLine();
            report.AppendLine("DEBUG CORPSE / REQUEST SAMPLES");
            report.AppendLine(
                $"  Up to {SamplesPerStage} samples per category; Entity IDs work with Scene Explorer.");

            for (int i = 0; i < (int)CorpseStage.Count; i++)
            {
                StringBuilder? sample = samples[i];
                if (sample == null)
                {
                    continue;
                }

                report.AppendLine();
                report.AppendLine(
                    $"  {GetStageLabel((CorpseStage)i).ToUpperInvariant()}");
                report.Append(sample);
            }

            return report.ToString();
        }

        private void AppendDebugSample(
            StringBuilder report,
            Entity citizen,
            HealthProblem healthProblem,
            CorpseStage stage,
            bool outsideService,
            in CorpseLookups lookups,
            ComponentLookup<TravelPurpose> travelPurposeLookup)
        {
            report.AppendLine($"    Citizen: {citizen}");
            report.AppendLine(
                $"      HealthProblem: flags={healthProblem.m_Flags}, " +
                $"timer={healthProblem.m_Timer}, request={healthProblem.m_HealthcareRequest}");

            if (lookups.CurrentBuilding.TryGetComponent(
                    citizen,
                    out CurrentBuilding currentBuilding))
            {
                report.AppendLine(
                    $"      CurrentBuilding: {currentBuilding.m_CurrentBuilding}");
            }
            else
            {
                report.AppendLine("      CurrentBuilding: none");
            }

            if (lookups.CurrentTransport.TryGetComponent(
                    citizen,
                    out CurrentTransport currentTransport))
            {
                report.AppendLine(
                    $"      CurrentTransport: {currentTransport.m_CurrentTransport}");

                if (lookups.CurrentVehicle.TryGetComponent(
                        currentTransport.m_CurrentTransport,
                        out CurrentVehicle currentVehicle))
                {
                    report.AppendLine(
                        $"      CurrentVehicle: {currentVehicle.m_Vehicle} " +
                        $"flags={currentVehicle.m_Flags}");
                }
            }
            else
            {
                report.AppendLine("      CurrentTransport: none");
            }

            if (travelPurposeLookup.TryGetComponent(
                    citizen,
                    out TravelPurpose travelPurpose))
            {
                report.AppendLine(
                    $"      TravelPurpose: {travelPurpose.m_Purpose}");
            }

            Entity request = healthProblem.m_HealthcareRequest;
            if (request != Entity.Null && lookups.Entity.Exists(request))
            {
                if (lookups.HealthcareRequest.TryGetComponent(
                        request,
                        out HealthcareRequest healthcareRequest))
                {
                    report.AppendLine(
                        $"      HealthcareRequest: citizen={healthcareRequest.m_Citizen}, " +
                        $"type={healthcareRequest.m_Type}");
                }

                if (lookups.ServiceRequest.TryGetComponent(
                        request,
                        out ServiceRequest serviceRequest))
                {
                    report.AppendLine(
                        $"      ServiceRequest: failCount={serviceRequest.m_FailCount}, " +
                        $"cooldown={serviceRequest.m_Cooldown}, flags={serviceRequest.m_Flags}");
                }

                if (lookups.Dispatched.TryGetComponent(
                        request,
                        out Dispatched dispatched))
                {
                    report.AppendLine(
                        $"      Dispatched: handler={dispatched.m_Handler}");
                }

                if (lookups.PathInformation.TryGetComponent(
                        request,
                        out PathInformation pathInformation))
                {
                    report.AppendLine(
                        $"      PathInformation: origin={pathInformation.m_Origin}, " +
                        $"destination={pathInformation.m_Destination}");
                    report.AppendLine(
                        $"        distance={pathInformation.m_Distance:0.###}, " +
                        $"duration={pathInformation.m_Duration:0.###}, " +
                        $"cost={pathInformation.m_TotalCost:0.###}, " +
                        $"methods={pathInformation.m_Methods}, state={pathInformation.m_State}");
                }

                if (EntityManager.HasComponent<UpdateFrame>(request))
                {
                    UpdateFrame updateFrame =
                        EntityManager.GetSharedComponent<UpdateFrame>(request);
                    report.AppendLine($"      UpdateFrame: {updateFrame.m_Index}");
                }
            }
            else
            {
                report.AppendLine("      Request entity: missing");
            }

            report.AppendLine(
                $"      MH category: {GetStageLabel(stage)}, outsideService={outsideService}");
        }
    }
}
#endif
