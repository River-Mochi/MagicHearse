// File: Settings/Setting.cs
// Purpose: Options UI + settings for Magic Hearse (Actions/Status/About tabs).

namespace MagicHearse
{
    using Colossal.IO.AssetDatabase;    // FileLocation
    using Game.Modding;                 // IMod, ModSetting
    using Game.Settings;                // Settings UI attributes
    using Game.UI;                      // Unit
    using System;                       // Exception
    using Unity.Entities;               // World
    using UnityEngine;                  // Application.OpenURL

    [FileLocation("ModsSettings/MagicHearse")]
    [SettingsUITabOrder(ActionsTab, AboutTab)]
    [SettingsUIGroupOrder(
        AutoCleanGrp, SelfManageGrp,
        StatusGrp,
        AboutInfoGrp, AboutLinksGrp)]
    [SettingsUIShowGroupName(
        AutoCleanGrp, SelfManageGrp,
        StatusGrp,
        AboutLinksGrp)]
    public sealed class Setting : ModSetting
    {
        // ---- TABS ----
        public const string ActionsTab = "Actions";
        public const string AboutTab = "About";

        // ---- GROUPS ----
        public const string AutoCleanGrp = "AutoClean";
        public const string SelfManageGrp = "SelfManage";
        public const string StatusGrp = "Status";
        public const string AboutInfoGrp = "AboutInfo";
        public const string AboutLinksGrp = "AboutLinks";

        private const string kUrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        // Backing fields allow mutual exclusivity while still supporting both OFF.
        private bool m_EnableMagicHearse = true;
        private bool m_FuneralDirector;

        // Workers control is a sub-toggle INSIDE FD.
        // Default OFF to avoid surprise conflicts with ConfigXML or other building mods.
        private bool m_ControlWorkers = false;

        public Setting(IMod mod)
            : base(mod)
        {
        }

        // Helper for UI conditions (keeps WorkersScalar hidden unless FD + toggle are ON).
        public bool WorkersControlEnabled => m_FuneralDirector && m_ControlWorkers;

        // --------------------------------------------------------------------
        // ACTIONS – AUTO CLEAN
        // --------------------------------------------------------------------

        [SettingsUISection(ActionsTab, AutoCleanGrp)]
        [SettingsUISetter(typeof(Setting), nameof(SetEnableMagicHearse))]
        public bool EnableMagicHearse
        {
            get => m_EnableMagicHearse;
            set => m_EnableMagicHearse = value;
        }

        private void SetEnableMagicHearse(bool value)
        {
            bool wasFdOn = m_FuneralDirector;

            m_EnableMagicHearse = value;

            // Mutually exclusive: Magic ON forces FD OFF.
            if (value && m_FuneralDirector)
            {
                m_FuneralDirector = false;
            }

            ApplySystemsLive(magicChanged: true, fdChanged: wasFdOn != m_FuneralDirector);
        }

        // --------------------------------------------------------------------
        // ACTIONS – SELF MANAGE (FD)
        // --------------------------------------------------------------------

        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUISetter(typeof(Setting), nameof(SetFuneralDirector))]
        public bool FuneralDirector
        {
            get => m_FuneralDirector;
            set => m_FuneralDirector = value;
        }

        private void SetFuneralDirector(bool value)
        {
            bool wasMagicOn = m_EnableMagicHearse;

            m_FuneralDirector = value;

            // Mutually exclusive: FD ON forces Magic OFF.
            if (value && m_EnableMagicHearse)
            {
                m_EnableMagicHearse = false;
            }

            ApplySystemsLive(magicChanged: wasMagicOn != m_EnableMagicHearse, fdChanged: true);
        }

        // Sliders + toggles (shown only when FD is ON)

        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetProcScalar))]
        public int ProcScalar { get; set; } = 100;

        private void SetProcScalar(int value)
        {
            ProcScalar = value;
            RequestFdApplyIfEnabled();
        }

        [SettingsUISlider(min = 100, max = 400, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetFleetScalar))]
        public int FleetScalar { get; set; } = 100;

        private void SetFleetScalar(int value)
        {
            FleetScalar = value;
            RequestFdApplyIfEnabled();
        }

        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetStorageScalar))]
        public int StorageScalar { get; set; } = 100;

        private void SetStorageScalar(int value)
        {
            StorageScalar = value;
            RequestFdApplyIfEnabled();
        }

        // Workers compatibility plan:
        // - FD is the master switch for prefab scaling.
        // - ControlWorkers decides whether MH touches WorkplaceData at all.
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetControlWorkers))]
        public bool ControlWorkers
        {
            get => m_ControlWorkers;
            set => m_ControlWorkers = value;
        }

        private void SetControlWorkers(bool value)
        {
            m_ControlWorkers = value;
            RequestFdApplyIfEnabled(); // schedules apply/restore pass (workers ownership logic lives in FD system)
        }

        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(WorkersControlEnabled), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetWorkersScalar))]
        public int WorkersScalar { get; set; } = 100;

        private void SetWorkersScalar(int value)
        {
            WorkersScalar = value;
            RequestFdApplyIfEnabled();
        }

        [SettingsUIButton]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        public bool ResetGameDefaults
        {
            set
            {
                if (!value)
                {
                    return;
                }

                ProcScalar = 100;
                FleetScalar = 100;
                StorageScalar = 100;
                WorkersScalar = 100;

                RequestFdApplyIfEnabled();
            }
        }

        // --------------------------------------------------------------------
        // ACTIONS – STATUS (getters must never throw)
        // --------------------------------------------------------------------

        [SettingsUISection(ActionsTab, StatusGrp)]
        public string StatusSummary1
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryLine1 ?? string.Empty;
            }
        }

        [SettingsUISection(ActionsTab, StatusGrp)]
        public string StatusSummary2
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryLine2 ?? string.Empty;
            }
        }

        [SettingsUISection(ActionsTab, StatusGrp)]
        public string StatusSummary3
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryLine3 ?? string.Empty;
            }
        }

        // --------------------------------------------------------------------
        // ABOUT – INFO (no header)
        // --------------------------------------------------------------------

        [SettingsUISection(AboutTab, AboutInfoGrp)]
        public string AboutName => Mod.ModName;

        [SettingsUISection(AboutTab, AboutInfoGrp)]
        public string AboutVersion => Mod.ModVersion;

        // --------------------------------------------------------------------
        // ABOUT – LINKS
        // --------------------------------------------------------------------

        [SettingsUIButton]
        [SettingsUIButtonGroup(AboutLinksGrp)]
        [SettingsUISection(AboutTab, AboutLinksGrp)]
        public bool OpenParadoxMods
        {
            set
            {
                if (!value)
                {
                    return;
                }

                try
                {
                    Application.OpenURL(kUrlParadox);
                }
                catch (Exception)
                {
                    // Silent catch; worst case the link does nothing.
                }
            }
        }

        // --------------------------------------------------------------------
        // DEFAULTS
        // --------------------------------------------------------------------

        public override void SetDefaults()
        {
            m_EnableMagicHearse = true;
            m_FuneralDirector = false;

            m_ControlWorkers = false;

            ProcScalar = 100;
            FleetScalar = 100;
            StorageScalar = 100;
            WorkersScalar = 100;
        }

        // --------------------------------------------------------------------
        // Live apply helpers
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

        private void ApplySystemsLive(bool magicChanged, bool fdChanged)
        {
            World? world = GetWorld();
            if (world == null)
            {
                return;
            }

            if (magicChanged)
            {
                world.GetOrCreateSystemManaged<MagicHearseSystem>().Enabled = m_EnableMagicHearse;
            }

            if (fdChanged)
            {
                world.GetOrCreateSystemManaged<FuneralDirectorSystem>()
                    .RequestReapplyFromSettings();
            }
        }

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

            world.GetOrCreateSystemManaged<FuneralDirectorSystem>()
                .RequestReapplyFromSettings();

            // When sliders/toggles change, refresh status immediately on next UI poll (don't wait for throttle).
            DeathcareStatus.MarkDirty();
        }
    }
}
