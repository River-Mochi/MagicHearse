// <copyright file="MHSetting.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Settings/MHSetting.cs
// Purpose: Options UI + settings for Magic Hearse (Actions/Status/About tabs).

namespace MagicHearse
{
    using System;                    // Exception
    using Colossal.IO.AssetDatabase; // FileLocation
    using CS2Shared.RiverMochi;      // ShellOpen
    using Game.Modding;              // IMod, ModSetting
    using Game.Settings;             // Settings UI attributes
    using Game.UI;                   // Unit
    using UnityEngine;               // Application.OpenURL

    [FileLocation("ModsSettings/MagicHearse/MagicHearse")]
    [SettingsUITabOrder(kActionsTab, kAboutTab)]
    [SettingsUIGroupOrder(
        kAutoCleanGrp, kSelfManageGrp, kAdvancedGrp, kStatusGrp,
        kAboutInfoGrp, kAboutLinksGrp, kDebugGrp)]
    [SettingsUIShowGroupName(
        kAutoCleanGrp, kSelfManageGrp, kAdvancedGrp,
        kStatusGrp, kAboutLinksGrp, kDebugGrp)]
    public sealed partial class MHSetting : ModSetting
    {
        // ---- TABS ----
        public const string kActionsTab = "Actions";
        public const string kAboutTab = "About";

        // ---- GROUPS (ACTIONS) ----
        public const string kAutoCleanGrp = "AutoClean";
        public const string kSelfManageGrp = "SelfManage";
        public const string kAdvancedGrp = "Advanced";
        public const string kStatusGrp = "Status";

        // ---- GROUPS (ABOUT) ----
        public const string kAboutInfoGrp = "AboutInfo";
        public const string kAboutLinksGrp = "AboutLinks";
        public const string kDebugGrp = "Debug";
        public const string kDebugButtonsRow = "DebugButtonsRow";

        // ---- TUNABLE CONSTANTS ----
        private const int kDefaultPercent = 100;

        private const int kProcMin = 100;
        private const int kProcMax = 500;
        private const int kProcStep = 10;

        private const int kFleetMin = 100;
        private const int kFleetMax = 400;
        private const int kFleetStep = 10;

        private const int kStorageMin = 100;
        private const int kStorageMax = 500;
        private const int kStorageStep = 10;

        private const int kHearseSpeedMin = 100;
        private const int kHearseSpeedMax = 1000;
        private const int kHearseSpeedStep = 10;

        private const int kDefaultHearseWarningMinutes = 3;
        private const int kHearseWarningMinutesMin = 2;
        private const int kHearseWarningMinutesMax = 30;
        private const int kHearseWarningMinutesStep = 1;

        private const int kWorkersMin = 100;
        private const int kWorkersMax = 500;
        private const int kWorkersStep = 10;

        private const string kUrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        // ---- BACKING FIELDS ----
        private bool m_EnableMagicHearse = true;
        private bool m_FuneralDirector;

        // Default OFF so ConfigXML or another building mod can own worker counts.
        private bool m_ControlWorkers;

        // FD defaults to instant reset.
        private bool m_AutoResetCemetery = true;

        // Magic removes most corpses before burial, so its separate reset stays opt-in.
        private bool m_MagicResetCemetery;

        public MHSetting(IMod mod)
            : base(mod)
        {
        }

        // Options shows Maximum workers only when FD and worker control are both on.
        public bool WorkersControlEnabled => m_FuneralDirector && m_ControlWorkers;

        // Options shows gradual turnover only when FD is on and instant reset is off.
        public bool CemeteryTurnoverEnabled => m_FuneralDirector && !m_AutoResetCemetery;

        // --------------------------------------------------------------------
        // ACTIONS – AUTO CLEAN
        // --------------------------------------------------------------------

        [SettingsUISection(kActionsTab, kAutoCleanGrp)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetEnableMagicHearse))]
        public bool EnableMagicHearse
        {
            get => m_EnableMagicHearse;
            set => m_EnableMagicHearse = value;
        }

        [SettingsUISection(kActionsTab, kAutoCleanGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(EnableMagicHearse), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetMagicResetCemetery))]
        public bool MagicResetCemetery
        {
            get => m_MagicResetCemetery;
            set => m_MagicResetCemetery = value;
        }

        // --------------------------------------------------------------------
        // ACTIONS – SELF MANAGE (FD)
        // --------------------------------------------------------------------

        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetFuneralDirector))]
        public bool FuneralDirector
        {
            get => m_FuneralDirector;
            set => m_FuneralDirector = value;
        }

        // Instant reset and gradual turnover are exclusive in the UI; storage works with either.
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetAutoResetCemetery))]
        public bool AutoResetCemetery
        {
            get => m_AutoResetCemetery;
            set => m_AutoResetCemetery = value;
        }

        [SettingsUISlider(min = kHearseWarningMinutesMin, max = kHearseWarningMinutesMax, step = kHearseWarningMinutesStep, scalarMultiplier = 1, unit = Unit.kInteger)]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetHearseWarningMinutes))]
        public int HearseWarningMinutes { get; set; } = kDefaultHearseWarningMinutes;

        [SettingsUISlider(min = kProcMin, max = kProcMax, step = kProcStep, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetProcScalar))]
        public int ProcScalar { get; set; } = kDefaultPercent;

        [SettingsUISlider(min = kFleetMin, max = kFleetMax, step = kFleetStep, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetFleetScalar))]
        public int FleetScalar { get; set; } = kDefaultPercent;

        [SettingsUISlider(min = kHearseSpeedMin, max = kHearseSpeedMax, step = kHearseSpeedStep, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetHearseSpeedScalar))]
        public int HearseSpeedScalar { get; set; } = kDefaultPercent;

        [SettingsUISlider(min = kStorageMin, max = kStorageMax, step = kStorageStep, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetStorageScalar))]
        public int StorageScalar { get; set; } = kDefaultPercent;

        [SettingsUISlider(min = kProcMin, max = kProcMax, step = kProcStep, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(CemeteryTurnoverEnabled), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetCemeteryTurnoverScalar))]
        public int CemeteryTurnoverScalar { get; set; } = kDefaultPercent;

        [SettingsUIButton]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        public bool ResetGameDefaults
        {
            set
            {
                if (!value)
                {
                    return;
                }

                ProcScalar = kDefaultPercent;
                CemeteryTurnoverScalar = kDefaultPercent;
                FleetScalar = kDefaultPercent;
                StorageScalar = kDefaultPercent;
                HearseSpeedScalar = kDefaultPercent;
                HearseWarningMinutes = kDefaultHearseWarningMinutes;
                WorkersScalar = kDefaultPercent;

                ApplyAndSave();
                RequestFdApplyIfEnabled();
            }
        }

        // --------------------------------------------------------------------
        // ACTIONS – ADVANCED (Workers compatibility)
        // --------------------------------------------------------------------

        [SettingsUISection(kActionsTab, kAdvancedGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetControlWorkers))]
        public bool ControlWorkers
        {
            get => m_ControlWorkers;
            set => m_ControlWorkers = value;
        }

        [SettingsUISlider(min = kWorkersMin, max = kWorkersMax, step = kWorkersStep, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActionsTab, kAdvancedGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(WorkersControlEnabled), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetWorkersScalar))]
        public int WorkersScalar { get; set; } = kDefaultPercent;

        // --------------------------------------------------------------------
        // ACTIONS – STATUS (getters must never throw)
        // --------------------------------------------------------------------

        [SettingsUISection(kActionsTab, kStatusGrp)]
        public string StatusSummary1
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryLine1 ?? string.Empty;
            }
        }

        [SettingsUISection(kActionsTab, kStatusGrp)]
        public string StatusSummary2
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryLine2 ?? string.Empty;
            }
        }

        [SettingsUISection(kActionsTab, kStatusGrp)]
        public string StatusSummary3
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryLine3 ?? string.Empty;
            }
        }

        // Cemetery capacity and reset tally use one compact status row.
        [SettingsUISection(kActionsTab, kStatusGrp)]
        public string StatusSummary4
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryLine4 ?? string.Empty;
            }
        }

        [SettingsUISection(kActionsTab, kStatusGrp)]
        [SettingsUIDisplayName(overrideValue: "\u00A0")]
        [SettingsUIDescription(overrideValue: "Time the status snapshot was last refreshed.")]
        public string StatusUpdated
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryUpdated ?? string.Empty;
            }
        }

        // --------------------------------------------------------------------
        // ABOUT – INFO (no header)
        // --------------------------------------------------------------------

        [SettingsUISection(kAboutTab, kAboutInfoGrp)]
        public string AboutName => Mod.kModName;

        [SettingsUISection(kAboutTab, kAboutInfoGrp)]
        public string AboutVersion => Mod.ModVersion + "  [" + Mod.kBuildType + "]";

        // --------------------------------------------------------------------
        // ABOUT – LINKS
        // --------------------------------------------------------------------

        [SettingsUIButton]
        [SettingsUIButtonGroup(kAboutLinksGrp)]
        [SettingsUISection(kAboutTab, kAboutLinksGrp)]
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
                    // A failed browser launch should not break Options.
                }
            }
        }

        // --------------------------------------------------------------------
        // ABOUT – DEBUG
        // --------------------------------------------------------------------

        [SettingsUIButton]
        [SettingsUIButtonGroup(kDebugButtonsRow)]
        [SettingsUISection(kAboutTab, kDebugGrp)]
        public bool LogReport
        {
            set
            {
                if (!value)
                {
                    return;
                }

                DeathcareLogReport.Write();
            }
        }

        [SettingsUIButton]
        [SettingsUIButtonGroup(kDebugButtonsRow)]
        [SettingsUISection(kAboutTab, kDebugGrp)]
        public bool OpenLog
        {
            set
            {
                if (!value)
                {
                    return;
                }

                ShellOpen.OpenModLogOrLogsFolder();
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
            m_AutoResetCemetery = true;
            m_MagicResetCemetery = false;

            ProcScalar = kDefaultPercent;
            CemeteryTurnoverScalar = kDefaultPercent;
            FleetScalar = kDefaultPercent;
            StorageScalar = kDefaultPercent;
            HearseSpeedScalar = kDefaultPercent;
            HearseWarningMinutes = kDefaultHearseWarningMinutes;
            WorkersScalar = kDefaultPercent;
        }
    }
}
