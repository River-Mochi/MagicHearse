// <copyright file="DeathcareStatusSystem.Corpses.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Status/DeathcareStatusSystem.Corpses.cs
// Purpose: Classifies each Dead + RequireTransport citizen into one clear pipeline stage.

namespace MagicHearse
{
    using Game.Buildings;   // DeathcareFacilityFlags, HospitalFlags
    using Game.Citizens;    // HealthProblem, CurrentBuilding/Transport
    using Game.Common;      // Owner
    using Game.Creatures;   // CurrentVehicle
    using Game.Pathfind;    // PathInformation
    using Game.Simulation;  // HealthcareRequest, ServiceRequest, Dispatched
    using Unity.Entities;   // Entity, ComponentLookup, EntityStorageInfoLookup

    public sealed partial class DeathcareStatusSystem
    {
        private enum CorpseStage
        {
            NoRequest,
            WaitingForDispatch,
            Pathfinding,
            RetryCooldown,
            AssignedFacility,
            AssignedHearse,
            InsideHearse,
            AtFacility,
            Other,
            Count,
        }

        private struct CorpseLookups
        {
            public ComponentLookup<HealthProblem> HealthProblem;
            public ComponentLookup<CurrentBuilding> CurrentBuilding;
            public ComponentLookup<CurrentTransport> CurrentTransport;
            public ComponentLookup<CurrentVehicle> CurrentVehicle;
            public ComponentLookup<Game.Buildings.DeathcareFacility> DeathcareFacility;
            public ComponentLookup<Game.Buildings.Hospital> Hospital;
            public ComponentLookup<Game.Vehicles.Hearse> Hearse;
            public ComponentLookup<HealthcareRequest> HealthcareRequest;
            public ComponentLookup<ServiceRequest> ServiceRequest;
            public ComponentLookup<Dispatched> Dispatched;
            public ComponentLookup<PathInformation> PathInformation;
            public ComponentLookup<Owner> Owner;
            public ComponentLookup<Game.Objects.OutsideConnection> OutsideConnection;
            public EntityStorageInfoLookup Entity;
        }

        private CorpseLookups CreateCorpseLookups()
        {
            return new CorpseLookups
            {
                HealthProblem = GetComponentLookup<HealthProblem>(true),
                CurrentBuilding = GetComponentLookup<CurrentBuilding>(true),
                CurrentTransport = GetComponentLookup<CurrentTransport>(true),
                CurrentVehicle = GetComponentLookup<CurrentVehicle>(true),
                DeathcareFacility =
                    GetComponentLookup<Game.Buildings.DeathcareFacility>(true),
                Hospital = GetComponentLookup<Game.Buildings.Hospital>(true),
                Hearse = GetComponentLookup<Game.Vehicles.Hearse>(true),
                HealthcareRequest = GetComponentLookup<HealthcareRequest>(true),
                ServiceRequest = GetComponentLookup<ServiceRequest>(true),
                Dispatched = GetComponentLookup<Dispatched>(true),
                PathInformation = GetComponentLookup<PathInformation>(true),
                Owner = GetComponentLookup<Owner>(true),
                OutsideConnection =
                    GetComponentLookup<Game.Objects.OutsideConnection>(true),
                Entity = GetEntityStorageInfoLookup(),
            };
        }

        private static CorpseStage ClassifyCorpse(
            Entity citizen,
            HealthProblem healthProblem,
            in CorpseLookups lookups,
            out bool outsideService)
        {
            outsideService = false;

            // Physical location wins so picked-up or delivered corpses are not counted as waiting.
            if (TryGetCurrentHearse(citizen, in lookups, out Entity currentHearse))
            {
                outsideService = IsOutsideHandler(currentHearse, in lookups);
                return CorpseStage.InsideHearse;
            }

            if (IsAtProcessingFacility(citizen, in lookups))
            {
                return CorpseStage.AtFacility;
            }

            Entity request = healthProblem.m_HealthcareRequest;
            if (request == Entity.Null ||
                !lookups.Entity.Exists(request) ||
                !lookups.HealthcareRequest.TryGetComponent(
                    request,
                    out HealthcareRequest healthcareRequest) ||
                healthcareRequest.m_Type != HealthcareRequestType.Hearse)
            {
                return CorpseStage.NoRequest;
            }

            if (lookups.Dispatched.TryGetComponent(
                    request,
                    out Dispatched dispatched) &&
                dispatched.m_Handler != Entity.Null &&
                lookups.Entity.Exists(dispatched.m_Handler))
            {
                outsideService = IsOutsideHandler(dispatched.m_Handler, in lookups);

                if (lookups.Hearse.HasComponent(dispatched.m_Handler))
                {
                    return CorpseStage.AssignedHearse;
                }

                if (lookups.DeathcareFacility.HasComponent(dispatched.m_Handler) ||
                    lookups.OutsideConnection.HasComponent(dispatched.m_Handler))
                {
                    return CorpseStage.AssignedFacility;
                }

                return CorpseStage.Other;
            }

            if (lookups.PathInformation.HasComponent(request))
            {
                return CorpseStage.Pathfinding;
            }

            if (lookups.ServiceRequest.TryGetComponent(
                    request,
                    out ServiceRequest serviceRequest))
            {
                return serviceRequest.m_Cooldown > 0
                    ? CorpseStage.RetryCooldown
                    : CorpseStage.WaitingForDispatch;
            }

            return CorpseStage.Other;
        }

        private static bool TryGetCurrentHearse(
            Entity citizen,
            in CorpseLookups lookups,
            out Entity hearse)
        {
            hearse = Entity.Null;

            if (!lookups.CurrentTransport.TryGetComponent(
                    citizen,
                    out CurrentTransport currentTransport) ||
                !lookups.CurrentVehicle.TryGetComponent(
                    currentTransport.m_CurrentTransport,
                    out CurrentVehicle currentVehicle) ||
                !lookups.Hearse.HasComponent(currentVehicle.m_Vehicle))
            {
                return false;
            }

            hearse = currentVehicle.m_Vehicle;
            return true;
        }

        private static bool IsAtProcessingFacility(
            Entity citizen,
            in CorpseLookups lookups)
        {
            if (!lookups.CurrentBuilding.TryGetComponent(
                    citizen,
                    out CurrentBuilding currentBuilding))
            {
                return false;
            }

            Entity building = currentBuilding.m_CurrentBuilding;
            if (lookups.DeathcareFacility.TryGetComponent(
                    building,
                    out Game.Buildings.DeathcareFacility deathcareFacility) &&
                (deathcareFacility.m_Flags &
                 (DeathcareFacilityFlags.CanProcessCorpses |
                  DeathcareFacilityFlags.CanStoreCorpses)) != 0)
            {
                return true;
            }

            // Hospitals with CanProcessCorpses count as delivered too.
            return lookups.Hospital.TryGetComponent(
                       building,
                       out Game.Buildings.Hospital hospital) &&
                   (hospital.m_Flags & HospitalFlags.CanProcessCorpses) != 0;
        }

        private static bool IsOutsideHandler(
            Entity handler,
            in CorpseLookups lookups)
        {
            // Outside service may be the handler itself or the handler's owner.
            if (lookups.OutsideConnection.HasComponent(handler))
            {
                return true;
            }

            return lookups.Owner.TryGetComponent(handler, out Owner owner) &&
                   lookups.Entity.Exists(owner.m_Owner) &&
                   lookups.OutsideConnection.HasComponent(owner.m_Owner);
        }

        private static string GetStageLabel(CorpseStage stage)
        {
            return stage switch
            {
                CorpseStage.NoRequest => "No request yet",
                CorpseStage.WaitingForDispatch => "Waiting for dispatch group",
                CorpseStage.Pathfinding => "Pathfinding",
                CorpseStage.RetryCooldown => "Failed / retry cooldown",
                CorpseStage.AssignedFacility => "Assigned to facility",
                CorpseStage.AssignedHearse => "Assigned to hearse",
                CorpseStage.InsideHearse => "Already inside hearse",
                CorpseStage.AtFacility => "Already at facility",
                _ => "Other / needs investigation",
            };
        }
    }
}
