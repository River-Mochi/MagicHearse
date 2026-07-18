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
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// English localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the English locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all English localization entries for this mod.</summary>
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            string title = Mod.ModName;

            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title = title + " (" + Mod.ModVersion + ")";
            }
            return new Dictionary<string, string>

            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), title },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Actions" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "About" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "Auto Clean" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "Self Manage" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "Advanced" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "Mod info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "Links" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Enable Magic Clean" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Auto removes dead citizens** that are waiting for a hearse.\n" +
                    "Turn off both checkboxes to disable the mod without removing it."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Funeral Director" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Self manage everything.\n" +
                    "**Scale values:** rate, fleet, storage.\n" +
                    "Optional: **increase workers** too."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Processing rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Facility processing speed** (cremations)\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Fleet size" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Maximum hearses** per facility.\n" +
                    "**100%** = vanilla game default.\n" +
                    "**[o_o]** Too many hearses may affect traffic depending on death rate."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Cemetery storage" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Cemetery storage capacity** for the main building.\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AutoResetCemetery)), "Auto-empty when full" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AutoResetCemetery)),
                    "**Automatically empties a cemetery** the instant it fills up.\n" +
                    "Occupied plots reset to 0 — like bulldozing and rebuilding, but instant and automatic.\n" +
                    "Pairs with the **Cemetery storage** slider: size your cemeteries, then let them recycle so you never have to demolish a full one.\n" +
                    "Default ON while **Funeral Director** is active."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "Hearse speed" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**Increases hearse max speed**.\n" +
                    "**100%** = vanilla game default.\n" +
                    "<Road speed limits still apply>.\n\n" +
                    "Also scales acceleration/braking (gentle) so the new top speed does not create extreme launch/stop behavior.\n" +
                    "Note: even if the hearse’s max speed is increased, its actual driving speed is basically:\n" +
                    "(vehicle max, road speed limit, AI safe speed, traffic)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Control max workers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Compatibility toggle:\n" +
                    "**Enable [✓]** to increase the number of workers.\n" +
                    "**[o_o]** Leave OFF if you want **ConfigXML** or another mod to control deathcare workers."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Max workers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**Increases maximum workers** allowed.\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Sets all sliders back to **100%** (vanilla defaults)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Hearse needed" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Dead citizens waiting** for a hearse pickup."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Monthly totals** from game stats.\n" +
                     "**Cremation max/mo** = game's Handling/mo. info panel.\n" +
                     "This is the maximum bodies that could be processed by crematoriums per month."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Assets" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
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

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Display name of this mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Current version." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Opens the author’s Paradox mods page." },
            };
        }

        public void Unload()
        { }
    }
}
