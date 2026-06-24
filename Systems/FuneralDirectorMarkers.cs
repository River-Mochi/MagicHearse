// <copyright file="FuneralDirectorMarkers.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/FuneralDirectorSystem.Markers.cs
// Purpose: Tracking components used by FuneralDirectorSystem (safe restore + non-stomp behavior).

namespace MagicHearse
{
    using Unity.Entities;

    public sealed partial class FuneralDirectorSystem
    {
        /// <summary>
        /// Legacy marker shipped previously.
        /// Nested type name remains stable for save compatibility:
        /// MagicHearse.FuneralDirectorSystem/MHWorkplaceMarker
        /// </summary>
        private struct MHWorkplaceMarker : IComponentData
        {
            public int MaxWorkers;
            public int MinWorkers;
        }
    }

    /// <summary>
    /// Tracks last WorkProvider.m_MaxWorkers written on placed building owner entities.
    /// MagicHearse.WorkProviderMax
    /// </summary>
    internal struct WorkProviderMax : IComponentData
    {
        public int MaxWorkers;
    }

}
