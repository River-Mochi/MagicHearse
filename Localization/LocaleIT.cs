// File: Localization/LocaleIT.cs
// Purpose: Italian it-IT locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocaleIT : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleIT(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Azioni" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Info" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Pulizia automatica" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Gestione manuale" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Link" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Abilita magia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "Rimuove automaticamente i cittadini morti in attesa di un carro funebre.\n" +
                    "Disattiva entrambe le caselle per spegnere il mod senza rimuoverlo."
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Direttore funebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Scala i valori delle strutture funerarie (velocità, flotta, deposito)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Velocità di elaborazione" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "Moltiplicatore della **velocità di elaborazione**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Dimensione flotta" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "Moltiplicatore del **numero massimo di carri funebri**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Deposito cimitero" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "Aumenta il **deposito massimo del cimitero**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Ripristina cursori" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Reimposta tutti i cursori al **100%** (valori predefiniti)." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Versione" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Apre la pagina Paradox Mods dell’autore." },
            };
        }

        public void Unload()
        { }
    }
}
