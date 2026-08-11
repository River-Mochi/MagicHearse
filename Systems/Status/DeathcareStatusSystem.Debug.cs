// <copyright file="DeathcareStatusSystem.Debug.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Status/DeathcareStatusSystem.Debug.cs
// Purpose: On-demand Scene Explorer-style samples written by Log Report in all builds.

namespace MagicHearse
{
    using System;              // Math
    using System.Text;         // StringBuilder
    using Game.Areas;          // ServiceDistrict
    using Game.Buildings;      // DeathcareFacility, DeathcareFacilityFlags, Efficiency, Patient
    using Game.Citizens;      // HealthProblem, CurrentBuilding/Transport, TravelPurpose
    using Game.Companies;      // ServiceDispatch
    using Game.Creatures;     // CurrentVehicle
    using Game.Pathfind;      // PathInformation
    using Game.Simulation;    // HealthcareRequest, ServiceRequest, Dispatched, UpdateFrame
    using Game.Vehicles;      // Hearse, HearseFlags, OwnedVehicle, ParkedCar
    using Unity.Collections;  // NativeArray, Allocator
    using Unity.Entities;     // Entity, ComponentLookup
    using DeathcareFacilityData = Game.Prefabs.DeathcareFacilityData;
    using InstalledUpgrade = Game.Buildings.InstalledUpgrade;
    using PrefabRef = Game.Prefabs.PrefabRef;
    using UpgradeUtils = Game.Prefabs.UpgradeUtils;

    public sealed partial class DeathcareStatusSystem
    {
        public string BuildRequestSamples()
        {
            CompleteDependency();

            const int SamplesPerStage = 5;
            const int FailedRequestSamples = 10;
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
                    int sampleLimit =
                        stage == CorpseStage.RetryCooldown
                            ? FailedRequestSamples
                            : SamplesPerStage;
                    if (sampleCounts[stageIndex] >= sampleLimit)
                    {
                        continue;
                    }

                    sampleCounts[stageIndex]++;
                    StringBuilder sample =
                        samples[stageIndex] ??= new StringBuilder();

                    AppendRequestSample(
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
            report.AppendLine("CORPSE / REQUEST SAMPLES");
            report.AppendLine(
                $"  Up to {SamplesPerStage} samples per category and " +
                $"{FailedRequestSamples} failed/retry samples; " +
                "IDs use Scene Explorer Index:Version.");

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

            AppendFacilityReverseDispatchSamples(report);
            return report.ToString();
        }

        private void AppendFacilityReverseDispatchSamples(StringBuilder report)
        {
            ComponentLookup<DeathcareFacility> facilityLookup =
                GetComponentLookup<DeathcareFacility>(true);
            ComponentLookup<HealthcareRequest> healthcareRequestLookup =
                GetComponentLookup<HealthcareRequest>(true);
            ComponentLookup<ServiceRequest> serviceRequestLookup =
                GetComponentLookup<ServiceRequest>(true);
            ComponentLookup<Dispatched> dispatchedLookup =
                GetComponentLookup<Dispatched>(true);
            ComponentLookup<PathInformation> pathInformationLookup =
                GetComponentLookup<PathInformation>(true);
            ComponentLookup<Hearse> hearseLookup =
                GetComponentLookup<Hearse>(true);
            ComponentLookup<ParkedCar> parkedCarLookup =
                GetComponentLookup<ParkedCar>(true);
            ComponentLookup<PrefabRef> prefabRefLookup =
                GetComponentLookup<PrefabRef>(true);
            ComponentLookup<DeathcareFacilityData> facilityDataLookup =
                GetComponentLookup<DeathcareFacilityData>(true);

            BufferLookup<ServiceDistrict> serviceDistrictLookup =
                GetBufferLookup<ServiceDistrict>(true);
            BufferLookup<OwnedVehicle> ownedVehicleLookup =
                GetBufferLookup<OwnedVehicle>(true);
            BufferLookup<ServiceDispatch> serviceDispatchLookup =
                GetBufferLookup<ServiceDispatch>(true);
            BufferLookup<InstalledUpgrade> installedUpgradeLookup =
                GetBufferLookup<InstalledUpgrade>(true);
            BufferLookup<Efficiency> efficiencyLookup =
                GetBufferLookup<Efficiency>(true);
            BufferLookup<Patient> patientLookup =
                GetBufferLookup<Patient>(true);

            report.AppendLine();
            report.AppendLine("FACILITY REVERSE-DISPATCH SAMPLES");
            report.AppendLine(
                "  On-demand state for every placed deathcare facility. " +
                "Zero service districts means whole-city service.");

            using NativeArray<Entity> facilities =
                m_DeathcarePlacedQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < facilities.Length; i++)
            {
                Entity facilityEntity = facilities[i];
                if (!facilityLookup.TryGetComponent(
                        facilityEntity,
                        out DeathcareFacility facility))
                {
                    continue;
                }

                int districtCount =
                    serviceDistrictLookup.TryGetBuffer(
                        facilityEntity,
                        out DynamicBuffer<ServiceDistrict> serviceDistricts)
                        ? serviceDistricts.Length
                        : 0;
                int pendingDispatches =
                    serviceDispatchLookup.TryGetBuffer(
                        facilityEntity,
                        out DynamicBuffer<ServiceDispatch> dispatches)
                        ? dispatches.Length
                        : 0;

                DeathcareFacilityData facilityData = default;
                if (prefabRefLookup.TryGetComponent(
                        facilityEntity,
                        out PrefabRef prefabRef) &&
                    facilityDataLookup.TryGetComponent(
                        prefabRef.m_Prefab,
                        out DeathcareFacilityData prefabFacilityData))
                {
                    facilityData = prefabFacilityData;
                    if (installedUpgradeLookup.TryGetBuffer(
                            facilityEntity,
                            out DynamicBuffer<InstalledUpgrade> installedUpgrades) &&
                        installedUpgrades.Length != 0)
                    {
                        UpgradeUtils.CombineStats(
                            ref facilityData,
                            installedUpgrades,
                            ref prefabRefLookup,
                            ref facilityDataLookup);
                    }
                }

                float efficiency = 1f;
                float immediateEfficiency = 1f;
                if (efficiencyLookup.TryGetBuffer(
                        facilityEntity,
                        out DynamicBuffer<Efficiency> efficiencies))
                {
                    efficiency = BuildingUtils.GetEfficiency(efficiencies);
                    immediateEfficiency =
                        BuildingUtils.GetImmediateEfficiency(efficiencies);
                }

                int currentDispatchCapacity = BuildingUtils.GetVehicleCapacity(
                    Math.Min(efficiency, immediateEfficiency),
                    facilityData.m_HearseCapacity);
                int patientsInside =
                    patientLookup.TryGetBuffer(
                        facilityEntity,
                        out DynamicBuffer<Patient> patients)
                        ? patients.Length
                        : 0;

                int spawnedHearses = 0;
                int onRoadHearses = 0;
                int parkedHearses = 0;
                int disabledHearses = 0;
                int parkedNonDisabledHearses = 0;
                if (ownedVehicleLookup.TryGetBuffer(
                        facilityEntity,
                        out DynamicBuffer<OwnedVehicle> ownedVehicles))
                {
                    for (int j = 0; j < ownedVehicles.Length; j++)
                    {
                        Entity vehicle = ownedVehicles[j].m_Vehicle;
                        if (!hearseLookup.TryGetComponent(vehicle, out Hearse hearse))
                        {
                            continue;
                        }

                        spawnedHearses++;
                        bool isDisabled =
                            (hearse.m_State & HearseFlags.Disabled) != 0;
                        if (isDisabled)
                        {
                            disabledHearses++;
                        }

                        if (parkedCarLookup.HasComponent(vehicle))
                        {
                            parkedHearses++;
                            if (!isDisabled)
                            {
                                parkedNonDisabledHearses++;
                            }
                        }
                        else
                        {
                            onRoadHearses++;
                        }
                    }
                }

                report.AppendLine();
                report.AppendLine($"  Facility: {FormatEntity(facilityEntity)}");
                report.AppendLine(
                    $"    flags={facility.m_Flags}, serviceDistricts={districtCount}, " +
                    $"pendingDispatches={pendingDispatches}");
                report.AppendLine(
                    $"    efficiency={efficiency * 100f:0.#}%, " +
                    $"immediateEfficiency={immediateEfficiency * 100f:0.#}%, " +
                    $"hearseCapacity={currentDispatchCapacity}/" +
                    $"{facilityData.m_HearseCapacity}");
                report.AppendLine(
                    $"    spawnedHearses={spawnedHearses}, onRoad={onRoadHearses}, " +
                    $"parked={parkedHearses}, disabled={disabledHearses}, " +
                    $"parkedNonDisabled={parkedNonDisabledHearses}");
                report.AppendLine(
                    $"    patientsInside={patientsInside}, " +
                    $"longTermStored={facility.m_LongTermStoredCount}/" +
                    $"{facilityData.m_StorageCapacity}");
                report.AppendLine(
                    "    Note: parkedNonDisabled is observed state, not by itself " +
                    "proof that dispatch/pathfinding considers the hearse eligible.");

                Entity targetRequest = facility.m_TargetRequest;
                report.AppendLine(
                    $"    reverseTargetRequest={FormatEntity(targetRequest)}");
                if (targetRequest == Entity.Null ||
                    !EntityManager.Exists(targetRequest))
                {
                    report.AppendLine("    reverse request state: missing");
                    continue;
                }

                if (healthcareRequestLookup.TryGetComponent(
                        targetRequest,
                        out HealthcareRequest healthcareRequest))
                {
                    report.AppendLine(
                        $"    HealthcareRequest: citizen/source=" +
                        $"{FormatEntity(healthcareRequest.m_Citizen)}, " +
                        $"type={healthcareRequest.m_Type}");
                }

                if (serviceRequestLookup.TryGetComponent(
                        targetRequest,
                        out ServiceRequest serviceRequest))
                {
                    double approximateRetryMinutes =
                        serviceRequest.m_Cooldown * 256d /
                        kSimulationFramesPerMinute;
                    report.AppendLine(
                        $"    ServiceRequest: failCount={serviceRequest.m_FailCount}, " +
                        $"cooldown={serviceRequest.m_Cooldown}, " +
                        $"approxRetryInIfNotWoken={approximateRetryMinutes:0.0} sim min, " +
                        $"flags={serviceRequest.m_Flags}");
                }
                else
                {
                    report.AppendLine("    ServiceRequest: missing");
                }

                if (EntityManager.HasComponent<UpdateFrame>(targetRequest))
                {
                    UpdateFrame updateFrame =
                        EntityManager.GetSharedComponent<UpdateFrame>(targetRequest);
                    report.AppendLine($"    UpdateFrame: {updateFrame.m_Index}");
                }

                if (pathInformationLookup.TryGetComponent(
                        targetRequest,
                        out PathInformation pathInformation))
                {
                    report.AppendLine(
                        $"    PathInformation: origin=" +
                        $"{FormatEntity(pathInformation.m_Origin)}, " +
                        $"destination={FormatEntity(pathInformation.m_Destination)}, " +
                        $"state={pathInformation.m_State}");
                }
                else
                {
                    report.AppendLine("    PathInformation: none");
                }

                if (dispatchedLookup.TryGetComponent(
                        targetRequest,
                        out Dispatched dispatched))
                {
                    report.AppendLine(
                        $"    Dispatched: handler={FormatEntity(dispatched.m_Handler)}");
                }
                else
                {
                    report.AppendLine("    Dispatched: none");
                }
            }
        }

        private void AppendRequestSample(
            StringBuilder report,
            Entity citizen,
            HealthProblem healthProblem,
            CorpseStage stage,
            bool outsideService,
            in CorpseLookups lookups,
            ComponentLookup<TravelPurpose> travelPurposeLookup)
        {
            report.AppendLine($"    Citizen: {FormatEntity(citizen)}");
            report.AppendLine(
                $"      HealthProblem: flags={healthProblem.m_Flags}, " +
                $"timer={healthProblem.m_Timer}, " +
                $"request={FormatEntity(healthProblem.m_HealthcareRequest)}");

            if (EntityManager.HasComponent<MHWarningDelay>(citizen))
            {
                MHWarningDelay warningDelay =
                    EntityManager.GetComponentData<MHWarningDelay>(citizen);
                uint estimatedStartFrame =
                    warningDelay.WaitEstimateInitialized
                        ? warningDelay.EstimatedWaitStartFrame
                        : unchecked(
                            m_SimulationSystem.frameIndex -
                            ((uint)healthProblem.m_Timer *
                             kSimulationFramesPerHealthTimerTick));
                double estimatedMinutes = unchecked(
                    m_SimulationSystem.frameIndex - estimatedStartFrame) /
                    kSimulationFramesPerMinute;

                report.AppendLine(
                    $"      MHWarningDelay: vanillaLimit={warningDelay.VanillaTimerLimit}, " +
                    $"extraTicks={warningDelay.ExtraTicksElapsed}, " +
                    $"vanillaReached={warningDelay.VanillaReached}, " +
                    $"completed={warningDelay.Completed}, " +
                    $"estimatedWait={estimatedMinutes:0.0} sim min");
            }

            if (lookups.CurrentBuilding.TryGetComponent(
                    citizen,
                    out CurrentBuilding currentBuilding))
            {
                report.AppendLine(
                    $"      CurrentBuilding: {FormatEntity(currentBuilding.m_CurrentBuilding)}");
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
                    $"      CurrentTransport: {FormatEntity(currentTransport.m_CurrentTransport)}");

                if (lookups.CurrentVehicle.TryGetComponent(
                        currentTransport.m_CurrentTransport,
                        out CurrentVehicle currentVehicle))
                {
                    report.AppendLine(
                        $"      CurrentVehicle: {FormatEntity(currentVehicle.m_Vehicle)} " +
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
                        $"      HealthcareRequest: citizen={FormatEntity(healthcareRequest.m_Citizen)}, " +
                        $"type={healthcareRequest.m_Type}");
                }

                if (lookups.ServiceRequest.TryGetComponent(
                        request,
                        out ServiceRequest serviceRequest))
                {
                    double approximateRetryMinutes =
                        serviceRequest.m_Cooldown * 256d /
                        kSimulationFramesPerMinute;
                    report.AppendLine(
                        $"      ServiceRequest: failCount={serviceRequest.m_FailCount}, " +
                        $"cooldown={serviceRequest.m_Cooldown}, " +
                        $"approxRetryInIfNotWoken={approximateRetryMinutes:0.0} sim min, " +
                        $"flags={serviceRequest.m_Flags}");
                }

                if (lookups.Dispatched.TryGetComponent(
                        request,
                        out Dispatched dispatched))
                {
                    report.AppendLine(
                        $"      Dispatched: handler={FormatEntity(dispatched.m_Handler)}");
                }

                if (lookups.PathInformation.TryGetComponent(
                        request,
                        out PathInformation pathInformation))
                {
                    report.AppendLine(
                        $"      PathInformation: origin={FormatEntity(pathInformation.m_Origin)}, " +
                        $"destination={FormatEntity(pathInformation.m_Destination)}");
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

        private static string FormatEntity(Entity entity)
        {
            return entity == Entity.Null
                ? "none"
                : $"{entity.Index}:{entity.Version}";
        }
    }
}
