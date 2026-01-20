// File: Localization/LocaleEN.cs
// English en-US for Magic Hearse Redux.

namespace MagicHearse
{
    using Colossal;
    using System.Collections.Generic;

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
                // Options mod name (Options -> Modding)
                { m_Setting.GetSettingsLocaleID(), Mod.ModName + " " + Mod.ModTag },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Actions" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "About"   },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.MagicGrp),      "Auto Clean" },
                { m_Setting.GetOptionGroupLocaleID(Setting.FdGrp),         "Self Manage" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),  "Mod info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Actions -> Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Enable Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "Keeps things clean automatically.\nAuto removes dead citizens waiting for a hearse." },

                // Actions -> Self Manage
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Funeral Director" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Enable sliders to boost deathcare capacity (no always-on sim cost)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcessingScalar)), "Processing rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcessingScalar)), "How fast facilities process bodies." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FacilityHearseScalar)), "Facility hearse count" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FacilityHearseScalar)), "Max hearses per cemetery/crematorium." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FacilityStorageScalar)), "Cemetery storage" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FacilityStorageScalar)), "Long-term storage capacity (cemeteries)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseCapacityScalar)), "Hearse capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseCapacityScalar)), "How many bodies each hearse can carry." },

                // About -> Info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Display name of this mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Current version." },

                // About -> Links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Opens the author's Paradox Mods page." },
            };
        }

        public void Unload()
        {
        }
    }
}
