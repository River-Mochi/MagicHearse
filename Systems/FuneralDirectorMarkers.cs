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
