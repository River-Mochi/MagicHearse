// File: Localization/LocaleDE.cs
// Purpose: German de-DE locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocaleDE : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleDE(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Aktionen" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Info" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Auto-Reinigung" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Manuelle Steuerung" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod-Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Magie aktivieren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "Entfernt automatisch tote Bürger, die auf einen Leichenwagen warten.\n" +
                    "Beide Kontrollkästchen deaktivieren, um den Mod auszuschalten."
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Bestattungsleiter" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Skaliert Werte von Bestattungsgebäuden (Rate, Flotte, Lager)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Verarbeitungsgeschwindigkeit" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "Multiplikator für **Verarbeitungsgeschwindigkeit**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Flottengröße" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "Multiplikator für **maximale Leichenwagenanzahl**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Friedhofsspeicher" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "Erhöht den **maximalen Friedhofsspeicher**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Regler zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Setzt alle Regler auf **100 %** (Standardwerte)." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Version" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Öffnet die Paradox-Mods-Seite des Autors." },
            };
        }

        public void Unload()
        { }
    }
}
