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
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Auto Clean" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Self Manage" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Enable Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Auto removes dead citizens** that are waiting for a hearse.\n" +
                    "Turn off both checkboxes to disable the mod without removing it."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Funeral Director" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Scales **facility** values (rate, fleet, storage).\n" +
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
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Cemetery storage" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Cemetery storage capacity** for the main building.\n" +
                    "**100%** = vanilla game default."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Control max workers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Enable for **Funeral Director** to increase the number of workers.\n" +
                    "Leave OFF if you want **ConfigXML** (or another mod) to control deathcare workers."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Max workers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**Scales the total maximum workers**.\n" +
                    "**100%** = vanilla game default.\n\n" +
                    "**[o_o] Tips**\n" +
                    "  - new values only appears on **new buildings**.\n" +
                    "  - adding or deleting an extension can also refresh the values.\n\n" +
                    "  - (work places is a game computed component (not like the other sliders) which is why a new building is needed to safely reinitialize values.)"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Sets all sliders back to **100%** (vanilla defaults)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Hearse Needed" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Dead citizens waiting** for a hearse pickup."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Monthly totals** from game stats.\n" +
                     "**Cremation max/mo** = game's Handling/mo. info panel.\n" +
                     "This is the maximum bodies that could be processed by all crematoriums per month."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Assets" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Active building** capacities (hearses, buildings, max workers).\n\n" +
                    "**Notes:**\n" +
                    "  - includes hearses that are still in maintenance (due to low budget).\n" +
                    "  - does not include any disabled building hearses.\n" +
                    "  - status scan only runs when you are in the Options menu or use a slider; does not run per-frame in the city so essentially no performance impact:)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status not loaded." },
                { "MH_STATUS_NO_CITY_LOADED", "No city loaded." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Open a city and let the simulation run." },

                { "MH_STATUS_LINE1", "{0} dead waiting | updated {1}" },
                { "MH_STATUS_LINE2", "{0} deaths/mo | {1} cremation max/mo | {2} / {3} cemetery use" },
                { "MH_STATUS_LINE3", "{0} hearses | {1} / {2} buildings | {3} empty graves | {4} max workers" },

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
