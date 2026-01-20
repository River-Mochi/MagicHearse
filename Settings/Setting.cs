// File: Settings/Setting.cs
// Options UI + settings for Magic Hearse Redux (Actions/About tabs).
// Notes:
// - Uses SettingsUISetter to react to UI changes.
// - Does NOT call ApplyAndSave() from sliders; Options UI handles saving normally.

namespace MagicHearse
{
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using System;
    using Unity.Entities;
    using UnityEngine;

    [FileLocation("ModsSettings/MagicHearseRedux")]
    [SettingsUITabOrder(ActionsTab, AboutTab)]
    [SettingsUIGroupOrder(
        MagicGrp,
        FdGrp,
        AboutInfoGrp,
        AboutLinksGrp)]
    [SettingsUIShowGroupName(
        MagicGrp,
        FdGrp,
        // AboutInfoGrp intentionally omitted so it has no header
        AboutLinksGrp)]
    public sealed class Setting : ModSetting
    {
        // ---- Tabs ----
        public const string ActionsTab = "Actions";
        public const string AboutTab = "About";

        // ---- Groups ----
        public const string MagicGrp = "Magic";
        public const string FdGrp = "FD";
        public const string AboutInfoGrp = "AboutInfo";
        public const string AboutLinksGrp = "AboutLinks";

        private const string kUrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        public static Setting? Instance;

        public Setting(IMod mod)
            : base(mod)
        {
            Instance = this;
        }

        // --------------------------------------------------------------------
        // Actions -> Magic
        // --------------------------------------------------------------------

        [SettingsUISection(ActionsTab, MagicGrp)]
        [SettingsUISetter(typeof(Setting), nameof(OnEnableMagicChanged))]
        public bool EnableMagicHearse { get; set; } = true;

        // --------------------------------------------------------------------
        // Actions -> Funeral Director (Self Manage)
        // --------------------------------------------------------------------

        [SettingsUISection(ActionsTab, FdGrp)]
        [SettingsUISetter(typeof(Setting), nameof(OnFuneralDirectorChanged))]
        public bool FuneralDirector { get; set; } = false;

        // Facility: processing rate (100–500%)
        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, FdGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(OnFdScalarChanged))]
        public int ProcessingScalar { get; set; } = 100;

        // Facility: max hearses (fleet size) (100–400%)
        [SettingsUISlider(min = 100, max = 400, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, FdGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(OnFdScalarChanged))]
        public int FacilityHearseScalar { get; set; } = 100;

        // Facility: storage capacity (100–500%)
        // We apply this to DeathcareFacilityData.m_StorageCapacity, but ONLY when m_LongTermStorage is true (cemeteries).
        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, FdGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(OnFdScalarChanged))]
        public int FacilityStorageScalar { get; set; } = 100;

        // Hearse vehicle: body capacity (100–500%)
        [SettingsUISlider(min = 100, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, FdGrp)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(Setting), nameof(OnFdScalarChanged))]
        public int HearseCapacityScalar { get; set; } = 100;

        // --------------------------------------------------------------------
        // About -> Info (no header)
        // --------------------------------------------------------------------

        [SettingsUISection(AboutTab, AboutInfoGrp)]
        public string AboutName => Mod.ModName;

        [SettingsUISection(AboutTab, AboutInfoGrp)]
        public string AboutVersion => Mod.ModVersion;

        // --------------------------------------------------------------------
        // About -> Links
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
                }
            }
        }

        // --------------------------------------------------------------------
        // Defaults
        // --------------------------------------------------------------------

        public override void SetDefaults()
        {
            EnableMagicHearse = true;

            FuneralDirector = false;
            ProcessingScalar = 100;
            FacilityHearseScalar = 100;
            FacilityStorageScalar = 100;
            HearseCapacityScalar = 100;
        }

        // --------------------------------------------------------------------
        // SettingsUISetter callbacks (CO Options UI)
        // --------------------------------------------------------------------

        private static void OnEnableMagicChanged(bool value)
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            world
                .GetOrCreateSystemManaged<MagicHearseSystem>()
                .Enabled = value;
        }

        private static void OnFuneralDirectorChanged(bool value)
        {
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            FuneralDirectorSystem fd = world.GetOrCreateSystemManaged<FuneralDirectorSystem>();

            if (!value)
            {
                // Turn off and do nothing else.
                fd.Enabled = false;
                return;
            }

            // Turn on + apply once.
            fd.RequestReapplyFromSettings();
        }

        private static void OnFdScalarChanged(int _)
        {
            // Any FD slider change should reapply once (if FD is enabled).
            Setting? s = Instance;
            if (s == null || !s.FuneralDirector)
            {
                return;
            }

            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null || !world.IsCreated)
            {
                return;
            }

            world
                .GetOrCreateSystemManaged<FuneralDirectorSystem>()
                .RequestReapplyFromSettings();
        }
    }
}
