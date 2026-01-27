// File: Settings/Setting.Helpers.cs
// Purpose: Runtime helpers + Settings UI setter handlers for Setting.cs.

namespace MagicHearse
{
    using Unity.Entities; // World

    public sealed partial class Setting
    {
        // --------------------------------------------------------------------
        // Helpers
        // --------------------------------------------------------------------

        private static World? GetWorld()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return null;
            }

            return world;
        }

        // Enables/disables systems immediately based on current toggles; also triggers FD reapply when needed.
        private void ApplySystemsLive(bool magicChanged, bool fdChanged)
        {
            World? world = GetWorld();
            if (world == null)
            {
                return;
            }

            if (magicChanged)  // Enable immediately without needing a restart.
            {
                world.GetOrCreateSystemManaged<MagicHearseSystem>().Enabled = m_EnableMagicHearse;
            }

            if (fdChanged)  // Trigger FD to apply/restore prefab scaling on next update pass.
            {
                world.GetOrCreateSystemManaged<FuneralDirectorSystem>()
                    .RequestReapplyFromSettings();
            }
        }

        // Schedules a one-shot FD apply/restore pass when FD is ON; forces Status to refresh next UI poll.
        private void RequestFdApplyIfEnabled()
        {
            if (!m_FuneralDirector)
            {
                return;
            }

            World? world = GetWorld();
            if (world == null)
            {
                return;
            }
            // Queue a one-shot FD apply pass (system enables itself, runs once, disables itself).
            world.GetOrCreateSystemManaged<FuneralDirectorSystem>()
                .RequestReapplyFromSettings();

            DeathcareStatus.MarkDirty();   // Ensure OptionsUI status refreshes immediately after a slider/toggle change.
        }

        // --------------------------------------------------------------------
        // UI Setter Handlers
        // --------------------------------------------------------------------

        private void SetEnableMagicHearse(bool value)
        {
            bool wasFdOn = m_FuneralDirector;

            m_EnableMagicHearse = value;

            // Mutual exclusivity: Magic ON forces FD OFF.
            if (value && m_FuneralDirector)
            {
                m_FuneralDirector = false;
            }

            ApplySystemsLive(magicChanged: true, fdChanged: wasFdOn != m_FuneralDirector);
        }

        private void SetFuneralDirector(bool value)
        {
            bool wasMagicOn = m_EnableMagicHearse;

            m_FuneralDirector = value;

            // Mutual exclusivity: FD ON forces Magic OFF.
            if (value && m_EnableMagicHearse)
            {
                m_EnableMagicHearse = false;
            }

            ApplySystemsLive(magicChanged: wasMagicOn != m_EnableMagicHearse, fdChanged: true);
        }

        private void SetProcScalar(int value)
        {
            ProcScalar = value;
            RequestFdApplyIfEnabled();
        }

        private void SetFleetScalar(int value)
        {
            FleetScalar = value;
            RequestFdApplyIfEnabled();
        }

        private void SetStorageScalar(int value)
        {
            StorageScalar = value;
            RequestFdApplyIfEnabled();
        }

        private void SetHearseSpeedScalar(int value)
        {
            HearseSpeedScalar = value;
            RequestFdApplyIfEnabled();
        }

        private void SetControlWorkers(bool value)
        {
            m_ControlWorkers = value;
            RequestFdApplyIfEnabled();
        }

        private void SetWorkersScalar(int value)
        {
            WorkersScalar = value;
            RequestFdApplyIfEnabled();
        }
    }
}
