// <copyright file="FuneralDirectorMarkers.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later,
// with the Cities: Skylines II Linking Exception.
// See LICENSE and LICENSE-EXCEPTION in the project root.
// This notice MUST be kept with copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/FuneralDirectorMarkers.cs
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
