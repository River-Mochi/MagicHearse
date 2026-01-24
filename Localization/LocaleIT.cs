// File: Localization/LocaleIT.cs
// Italian it-IT locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Italian localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleIT : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Italian locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleIT(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Italian localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Azioni" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Info" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Pulizia auto" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Gestione" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Stato" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Link" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Attiva magia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Rimuove automaticamente i morti**\n" +
                    "che aspettano un carro funebre.\n" +
                    "Spegni entrambe le caselle per disattivare il mod senza rimuoverlo."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Direttore funebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Scala i valori delle **strutture** (tasso, flotta, deposito).\n" +
                    "Opzionale: **aumenta i lavoratori**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Tasso di lavorazione" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Velocità di lavorazione** (cremazioni)\n" +
                    "**100%** = valore vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Dimensione flotta" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Carri funebri max** per struttura.\n" +
                    "**100%** = valore vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Deposito cimitero" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacità cimitero** (edificio principale).\n" +
                    "**100%** = valore vanilla."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Controlla lavoratori max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Attiva per far aumentare i lavoratori al **Direttore funebre**.\n" +
                    "Lascia OFF se vuoi che **ConfigXML** (o un altro mod) gestisca i lavoratori."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Lavoratori max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Scala i **lavoratori massimi** nelle strutture di decessi.\n" +
                    "**100%** = valore vanilla.\n\n" +
                    "**[o_o] Consigli**\n" +
                    "  - Vale per **nuovi edifici**.\n" +
                    "  - Aggiungere/rimuovere un'estensione spesso forza l'aggiornamento."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset slider" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Riporta tutti gli slider a **100%** (valori vanilla)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Carro funebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Morti in attesa** di ritiro."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Totali mensili** dalle statistiche di gioco.\n" +
                     "**Cremazione max/mese** = voce «Handling/mese» nel pannello info del gioco.\n" +
                     "È il massimo di corpi che tutti i crematori potrebbero processare al mese."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Risorse" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "Capacità dei **edifici attivi** (carri funebri, edifici, lavoratori max).\n\n" +
                    "**Note:**\n" +
                    "  - include anche i carri funebri ancora in manutenzione (budget basso).\n" +
                    "  - non include i carri funebri di edifici disattivati.\n" +
                    "  - la scansione stato gira solo nel menu Opzioni o usando uno slider; non per-frame in città, quindi impatto prestazioni praticamente zero :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Stato non caricato." },
                { "MH_STATUS_NO_CITY_LOADED", "Nessuna città caricata." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Statistiche non disponibili. Apri una città e lascia girare la simulazione." },

                { "MH_STATUS_LINE1", "{0} morti in attesa | agg. {1}" },
                { "MH_STATUS_LINE2", "{0} morti/mese | {1} cremazione max/mese | {2} / {3} uso cimitero" },
                { "MH_STATUS_LINE3", "{0} carri funebri | {1} / {2} edifici | {3} tombe libere | {4} lavoratori max" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nome visualizzato di questo mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Versione" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Versione attuale." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Apre la pagina Paradox Mods dell'autore." },
            };
        }

        public void Unload()
        { }
    }
}
