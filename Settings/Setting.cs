// File: Settings/Setting.cs
// Options UI + settings for Magic Hearse (Actions/About tabs).

namespace MagicHearse
{
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using System;
    using Unity.Entities;
    using UnityEngine;

    [FileLocation("ModsSettings/MagicHearse/MagicHearse")]
    [SettingsUITabOrder(ActionsTab, AboutTab)]
    [SettingsUIGroupOrder(
        AutoCleanGrp,
        SelfManageGrp,
        AboutInfoGrp,
        AboutLinksGrp)]
    [SettingsUIShowGroupName(
        AutoCleanGrp,
        SelfManageGrp,
        // AboutInfoGrp intentionally omitted so it has no header
        AboutLinksGrp)]
    public sealed class Setting : ModSetting
    {
        // ---- TABS ----
        public const string ActionsTab = "Actions";
        public const string AboutTab = "About";

        // ---- GROUPS ----
        public const string AutoCleanGrp = "AutoClean";
        public const string SelfManageGrp = "SelfManage";
        public const string AboutInfoGrp = "AboutInfo";
        public const string AboutLinksGrp = "AboutLinks";

        private const string kUrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        // Backing fields (so we can enforce mutual exclusivity but still allow both OFF).
        private bool m_EnableMagicHearse = true;
        private bool m_FuneralDirector;

        public Setting(IMod mod)
            : base(mod)
        {
        }

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
            m_EnableMagicHearse = value;

            // If turning Magic ON, force FD OFF.
            if (value && m_FuneralDirector)
            {
                m_FuneralDirector = false;
            }

            ApplySystemsLive();
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
            m_FuneralDirector = value;

            // If turning FD ON, force Magic OFF.
            if (value && m_EnableMagicHearse)
            {
                m_EnableMagicHearse = false;
            }

            ApplySystemsLive();
        }

        // Sliders (shown only when FD is ON)
        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetProcScalar))]
        public int ProcScalar { get; set; } = 100;

        private void SetProcScalar(int value)
        {
            ProcScalar = value;
            RequestFdApply();
        }

        [SettingsUISlider(min = 100, max = 400, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetFleetScalar))]
        public int FleetScalar { get; set; } = 100;

        private void SetFleetScalar(int value)
        {
            FleetScalar = value;
            RequestFdApply();
        }

        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetStorageScalar))]
        public int StorageScalar { get; set; } = 100;

        private void SetStorageScalar(int value)
        {
            StorageScalar = value;
            RequestFdApply();
        }

        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, SelfManageGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(SetHearseCapacityScalar))]
        public int HearseCapacityScalar { get; set; } = 100;

        private void SetHearseCapacityScalar(int value)
        {
            HearseCapacityScalar = value;
            RequestFdApply();
        }

        // Reset button (only when FD is ON)
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
                HearseCapacityScalar = 100;

                RequestFdApply();
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

            ProcScalar = 100;
            FleetScalar = 100;
            StorageScalar = 100;
            HearseCapacityScalar = 100;
        }

        // --------------------------------------------------------------------
        // Live apply helpers
        // --------------------------------------------------------------------

        private void ApplySystemsLive()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            // Magic system follows toggle.
            world.GetOrCreateSystemManaged<MagicHearseSystem>().Enabled = m_EnableMagicHearse;

            // FD: if ON, apply once; if OFF, restore once.
            FuneralDirectorSystem fd = world.GetOrCreateSystemManaged<FuneralDirectorSystem>();
            if (m_FuneralDirector)
            {
                fd.RequestReapplyFromSettings();
            }
            else
            {
                // Turning FD off should restore vanilla (one pass).
                fd.RequestReapplyFromSettings();
            }
        }

        private void RequestFdApply()
        {
            if (!m_FuneralDirector)
            {
                return;
            }

            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            world.GetOrCreateSystemManaged<FuneralDirectorSystem>()
                .RequestReapplyFromSettings();
        }
    }
}
