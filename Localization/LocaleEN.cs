// File: Localization/LocaleEN.cs
// Purpose: English en-US locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using Colossal.IO.AssetDatabase.Internal;
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
                { m_Setting.GetOptionTabLocaleID(Setting.StatusTab), "Status" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "About" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Auto Clean" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Self Manage" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Enable Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "Auto removes dead citizens that are waiting for a hearse.\n" +
                    "Turn off both checkboxes to disable the mod without removing it."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Funeral Director" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Scales deathcare facility values (rate, fleet, storage, workers)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Processing rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "Facility **processing speed** multiplier." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Fleet size" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Maximum hearses** per facility multiplier." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Cemetery storage" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "Increases **Cemetery maximum storage** of the main building." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Max workers (new buildings)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Increases **max workplaces** for deathcare facilities.\n" +
                    "<Applies to **New** buildings.\n>" +
                    "Existing buildings must be rebuilt to update the workers component."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Sets all sliders back to **100%** (vanilla defaults)." },

                // Status fields (keep labels SHORT; left column is narrow)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusLastRefreshUtc)), "Last refresh" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusLastRefreshUtc)),
                    "Updates only while this tab is open." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Totals/month" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "Monthly totals (from game stats / infoview)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Active" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),   
                    "Active (not disabled) deathcare hearses and buildings." },


                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Version" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Opens the author’s Paradox mods page." },
            };
        }

        public void Unload()
        { }
    }
}
