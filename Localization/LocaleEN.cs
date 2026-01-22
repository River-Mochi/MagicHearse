// File: Localization/LocaleEN.cs
// English en-US locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Unity.IO.LowLevel.Unsafe;

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
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "City Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Auto Clean (magic)
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
                     "**100%** = vanilla game default.\n" +
                    "**Tips:**\n" +
                    "  - max workers applies to new buildings (made after the slider change).\n" +
                    "  - trick: just deleting/adding extension buildings also instant updates workers.\n\n" +
                    "Dev Note: fleet/storage update instantly (prefab stats). Max workers is different " +
                    "(the game computes it).\n" +
                    "It's safer to just replace the building or extension to nudge a refresh " +
                    "rather than the mod trying to mutate a runtime component."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Sets all sliders back to **100%** (vanilla defaults)." },


                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "Dead citizens currently waiting for a hearse pickup." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                    "Monthly totals from game stats.\n" +
                    "Aim to keep the **can be handled** higher than **deaths/mo.**\n" +
                    "...or just enable magic :)"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Active** (not disabled) building capacities (hearses, buildings, max workers).\n\n" +
                    "**Notes:**\n" +
                    "  - includes hearses in maintenance\n" +
                    "  - does not include any disabled building hearses.\n" +
                    "  - max workers slider applies to <new buildings>\n" +
                    "  - status scan only happens when you are in the Options menu and does not run per-frame in the city sim;\n" +
                    "the mod was designed with the best city performance in mind.\n"
                },
         
                // Status text templates 
                { "MH_STATUS_NOT_LOADED", "Status not loaded." },
                { "MH_STATUS_NO_CITY_LOADED", "No city loaded yet." },

                { "MH_STATUS_LINE1", "{0} dead waiting | {1} updated" },
                { "MH_STATUS_LINE2", "{0} deaths/month | {1} can be handled" },
                { "MH_STATUS_LINE3", "{0} hearses | {1} / {2} buildings | {3} / {4} cemetery use | {5} max workers" },


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
