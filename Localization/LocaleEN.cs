// File: Localization/LocaleEN.cs
// Purpose: English en-US locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), Mod.ModName + " " + Mod.ModTag },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Actions" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "About" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Auto Clean" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Self Manage" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Status totals" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Enable Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Auto removes** dead citizens that are waiting for a hearse.\n" +
                    "Turn off both checkboxes to disable the mod without removing it."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Funeral Director" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Scales **facility values** (rate, fleet, storage, workers)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Processing rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Facility processing speed**.\n" +
                    "**100%** = vanilla game default."
                },


                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Fleet size" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Maximum hearses** per facility.\n" +
                    "Tip: balance and test as too many could also add to traffic.\n" +
                    "**100%** = vanilla game default."
                },


                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Cemetery storage" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Cemetery storage** capacity for the main building.\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Max workers (see notes)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Scales **Maximum workers** for deathcare facilities.\n" +
                    "**Notes:**\n" +
                    "  - max workers only affects new buildings (made after the slider change)\n" +
                    "**Tip:** just deleting/adding extension buildings also instant updates max workers.\n" +
                    "**100%** = vanilla game default."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Sets all sliders back to **100%** (vanilla defaults)." },

                // Status fields (keep labels SHORT; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Monthly" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "Monthly totals from game stats.\n" +
                    "Good balance to have the **can handle > deaths**\n" +
                    "...or just enable Magic ;)"
                    },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), " " },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),   
                    "**Active** (not disabled) deathcare capacity + max workers.\n\n" +
                    "**Notes:**\n" +
                    "    - includes hearses in maintenance due to low healthcare budget,\n" +
                    "    - does not inlcude any disabled buildings or hearses.\n" +
                    "    - max workers only affects <new buildings> (made after the slider change)\n" +
                    "**Tip:** just deleting/adding an extension building also instant updates max workers.\n"
                },

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
