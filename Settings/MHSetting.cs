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
    using Game.Modding;              // IMod, ModSetting
    using Game.Settings;             // Settings UI attributes
    using Game.UI;                   // Unit
    using UnityEngine;               // Application.OpenURL

    [FileLocation("ModsSettings/MagicHearse/MagicHearse")]
    [SettingsUITabOrder(kActionsTab, kAboutTab)]
    [SettingsUIGroupOrder(
        kAutoCleanGrp, kSelfManageGrp, kAdvancedGrp, kStatusGrp,
        kAboutInfoGrp, kAboutLinksGrp)]
    [SettingsUIShowGroupName(
        kAutoCleanGrp, kSelfManageGrp, kAdvancedGrp,
        kStatusGrp, kAboutLinksGrp)]
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

        private const int kWorkersMin = 100;
        private const int kWorkersMax = 500;
        private const int kWorkersStep = 10;

        private const string kUrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        // ---- BACKING FIELDS ----
        private bool m_EnableMagicHearse = true;
        private bool m_FuneralDirector;

        // Workers control is a sub-toggle inside FD.
        // Default OFF to avoid surprise conflicts with ConfigXML or other building mods.
        private bool m_ControlWorkers;

        // Cemetery auto-reset under Funeral Director. Default ON.
        private bool m_AutoResetCemetery = true;

        // Cemetery auto-reset under Magic Clean. Its OWN field with a different default (OFF): Magic Clean
        // removes corpses before burial so cemeteries rarely fill -- opt in there only if you need it.
        private bool m_MagicResetCemetery;

        public MHSetting(IMod mod)
            : base(mod)
        {
        }

        // UI condition helper: WorkersScalar only visible when FD + ControlWorkers are ON.
        public bool WorkersControlEnabled => m_FuneralDirector && m_ControlWorkers;

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

        // Magic Clean's own cemetery-reset toggle -- independent of the FD one, and OFF by default.
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

        [SettingsUISlider(min = kStorageMin, max = kStorageMax, step = kStorageStep, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetStorageScalar))]
        public int StorageScalar { get; set; } = kDefaultPercent;

        // Instance-level companion to Cemetery storage slider: empties a placed
        // cemetery back to 0 the moment the game flags it full. Independent of capacity,
        // so both can be used together.
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetAutoResetCemetery))]
        public bool AutoResetCemetery
        {
            get => m_AutoResetCemetery;
            set => m_AutoResetCemetery = value;
        }

        [SettingsUISlider(min = kHearseSpeedMin, max = kHearseSpeedMax, step = kHearseSpeedStep, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActionsTab, kSelfManageGrp)]
        [SettingsUIHideByCondition(typeof(MHSetting), nameof(FuneralDirector), true)]
        [SettingsUISetter(typeof(MHSetting), nameof(SetHearseSpeedScalar))]
        public int HearseSpeedScalar { get; set; } = kDefaultPercent;

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
                FleetScalar = kDefaultPercent;
                StorageScalar = kDefaultPercent;
                HearseSpeedScalar = kDefaultPercent;
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

        [SettingsUISection(kActionsTab, kStatusGrp)]
        public string StatusDispatch
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryDispatch ?? string.Empty;
            }
        }

        [SettingsUISection(kActionsTab, kStatusGrp)]
        public string StatusHearses
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryHearses ?? string.Empty;
            }
        }

        [SettingsUISection(kActionsTab, kStatusGrp)]
        public string StatusFacilities
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryFacilities ?? string.Empty;
            }
        }

        // Cemetery auto-reset tally (session): a summary row + one packed row naming the cemeteries.
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
        public string StatusCemetery1
        {
            get
            {
                try { DeathcareStatus.RefreshIfNeeded(); } catch { }
                return DeathcareStatus.SummaryCemetery1 ?? string.Empty;
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
            m_AutoResetCemetery = true;    // FD cemetery reset: ON
            m_MagicResetCemetery = false;  // Magic cemetery reset: OFF (opt-in)

            ProcScalar = kDefaultPercent;
            FleetScalar = kDefaultPercent;
            StorageScalar = kDefaultPercent;
            HearseSpeedScalar = kDefaultPercent;
            WorkersScalar = kDefaultPercent;
        }
    }
}
