// <copyright file="LocaleEN.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleEN.cs
// English en-US locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// English localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleEN : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the English locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleEN(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all English localization entries for this mod.</summary>
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            string title = Mod.kModName;

            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title = title + " (" + Mod.ModVersion + ")";
            }
            return new Dictionary<string, string>

            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), title },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Actions" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "About" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "Auto Clean" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "Self Manage" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "Advanced" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "Mod info" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "Links" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "Debug" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Enable Magic Clean" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Auto removes dead corpses that require transport (hearse).\n" +
                    "Magic clean is mutually exclusive with self-manage, pick one or the other.\n"+
                    "Turn off all checkboxes to disable the mod without removing it.\n"+
                    "Tech note: must be IsDead = true and WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Reset full cemetery" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Empties a full cemetery** so it's not blocked with a FULL icon.\n" +
                    "Magic Clean removes most corpses before burial — this still clears any cemetery that's **already full**.\n" +
                    "<[ ] Default OFF>.\n" +
                    "Enable this only if Magic clean mode should also empty cemeteries that are already full.\n" +
                    "Once empty, there is normally no need to keep this enabled as long as magic clean is left enabled."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Funeral Director" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Self manage and optimize normal game death systems.\n" +
                    "**Scale values:** rate, fleet, storage.\n" +
                    "Optional: **increase workers** too."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Crematorium processing" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Crematorium processing speed.**\n" +
                    "Higher values cremate bodies and free facility storage sooner.\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Fleet size" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Maximum hearses** per facility.\n" +
                    "**100%** = vanilla game default.\n" +
                    "**[Note]** Too many hearses may affect traffic depending on death rate."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Cemetery storage" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Cemetery storage capacity** for the main building.\n" +
                    "More capacity lets a full cemetery accept pickups again.\n" +
                    "It does not send more hearses unless lack of room was blocking the facility.\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Reset full cemetery" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Empties a full cemetery** so it's not blocked with a FULL icon above the building.\n" +
                    "No need to delete and rebuild full cemeteries anymore.\n" +
                    "Turn this OFF to use gradual **Cemetery turnover rate** instead.\n" +
                    "<[ ✓ ] Default ON>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Cemetery turnover rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Gradually frees occupied cemetery graves.**\n" +
                    "If cemeteries still show the FULL icon too often, increase this slider.\n" +
                    "Higher values make grave spaces available again faster than vanilla.\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Hearse speed" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Increases hearse maximum allowed driving speed**.\n" +
                    "**100%** = vanilla game default.\n" +
                    "<Road speed limits still apply>.\n\n" +
                    "Also scales acceleration/braking (gentle) so the new top speed does not create extreme launch/stop behavior.\n" +
                    "Note: even if the hearse’s max speed is increased, its actual driving speed is influenced by:\n" +
                    "vehicle max allowed, road speed limit, Game's own AI safe speed (curves, road damage), and traffic."

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Control max workers" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Compatibility toggle:\n" +
                    "**Enable [✓]** to increase the number of workers.\n" +
                    "**[o_o]** Leave OFF if you want **ConfigXML** or another mod to control deathcare workers."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Max workers" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Increases maximum workers** allowed.\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Reset sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "Sets all sliders back to **100%** (vanilla defaults)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Hearse needed" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Dead citizens waiting** for a hearse pickup."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "**Monthly totals** from game stats.\n" +
                     "**Cremation max/mo** = game's Handling/mo. info panel.\n" +
                     "This is the maximum bodies that could be processed by crematoriums per month."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Assets" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Active building capacities:** total hearses, buildings, max workers.\n\n" +
                    "**Notes:**\n" +
                    "▪ Hearse: Active-not parked / (Total* hearses)\n" +
                    "▪ *Total hearse:\n" +
                    "== includes hearse in maintenance (e.g. low service budget), \n" +
                    "== does not include any disabled building hearses.\n" +
                    "▪ Status scan runs only while Options is open (or you use a slider); " +
                    "does not run per-frame in the city, so basically no performance impact :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status not loaded." },
                { "MH_STATUS_NO_CITY_LOADED", "No city loaded." },
                { "MH_STATUS_STATS_NOT_AVAIL", "No city... ¯\\_(ツ)_/¯ ...No stats" },

                { "MH_STATUS_LINE1", "{0} waiting | {1} deaths/mo | updated {2}" },
                { "MH_STATUS_LINE2", "{0} cremate max/mo | {1}/{2} graves used" },
                { "MH_STATUS_LINE3", "{0} / {1} hearses | {2} / {3} buildings | {4} max workers" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "Suggested now: ~{0}% crematorium processing" },
                { "MH_STATUS_PROCESSING_MORE", "Suggested now: 500% crematorium processing + more active facilities" },
                { "MH_STATUS_PROCESSING_NONE", "Suggested: turn on/add crematoriums" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cemetery" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Full Cemeteries emptied this session**.\n" +
                    "Shows total resets and how many distinct cemeteries.\n" +
                    "Status clears on reboot or when you switch city."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "Which cemeteries were emptied, and how many times each (name × count)." },

                { "MH_STATUS_LINE4", "resets: {0} · cemeteries: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "none this session" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} more" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Display name of this mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Current version." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Opens the author’s Paradox mods page." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Log Report" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "Writes a detailed deathcare report and likely problem areas to MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Open Log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Opens **Logs/MagicHearse.log** if it exists.\n" +
                    "If the file is not found yet, opens the Logs folder instead." },
            };
        }

        public void Unload()
        { }
    }
}
