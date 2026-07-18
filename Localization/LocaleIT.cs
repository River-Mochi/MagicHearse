// <copyright file="LocaleIT.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

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
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Informazioni" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "Pulizia automatica" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "Gestione manuale" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "Avanzate" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "Stato" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "Info mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "Link" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Abilita pulizia magica" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Rimuove automaticamente i cittadini morti** che stanno aspettando un carro funebre.\n" +
                    "Disattiva entrambe le caselle per disabilitare la mod senza rimuoverla."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Direttore funebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Gestisci tutto manualmente.\n" +
                    "**Valori di scala:** ritmo, flotta, stoccaggio.\n" +
                    "Opzionale: **aumenta i lavoratori** anche."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Velocità di lavorazione" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Velocità di lavorazione della struttura** (cremazioni)\n" +
                    "**100%** = valore vanilla del gioco."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Dimensione flotta" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Numero massimo di carri funebri** per struttura.\n" +
                    "**100%** = valore vanilla del gioco.\n" +
                    "**[o_o]** Troppi carri funebri possono influire sul traffico a seconda del tasso di mortalità."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Stoccaggio del cimitero" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacità di stoccaggio del cimitero** per l'edificio principale.\n" +
                    "**100%** = valore vanilla del gioco."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AutoResetCemetery)), "Svuota auto. se pieno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AutoResetCemetery)),
                    "**Svuota automaticamente un cimitero** non appena si riempie.\n" +
                    "Le tombe occupate tornano a 0 — come demolire e ricostruire, ma istantaneo e automatico.\n" +
                    "Si combina con il cursore **Stoccaggio del cimitero**: dimensiona i tuoi cimiteri e lasciali riciclare per non demolire mai un cimitero pieno.\n" +
                    "Attivo di default quando il **Direttore funebre** è attivo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "Velocità del carro funebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**Aumenta la velocità massima del carro funebre**.\n" +
                    "**100%** = valore vanilla del gioco.\n" +
                    "<I limiti di velocità della strada si applicano ancora>.\n\n" +
                    "Scala anche accelerazione/frenata (dolce) così la nuova velocità massima non crea partenze/stop estremi.\n" +
                    "Nota: anche se la velocità massima del carro funebre aumenta, la sua velocità reale è praticamente:\n" +
                    "(massimo del veicolo, limite strada, velocità sicura IA, traffico)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Controlla lavoratori max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Interruttore di compatibilità:\n" +
                    "**Abilita [✓]** per aumentare il numero di lavoratori.\n" +
                    "**[o_o]** Lasciare su OFF se si preferisce che **ConfigXML** o un altro mod controlli i lavoratori dei servizi funebri."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Lavoratori max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**Aumenta il massimo di lavoratori** consentiti.\n" +
                    "**100%** = valore vanilla del gioco."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset slider" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Riporta tutti gli slider a **100%** (valori vanilla)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Carro funebre necessario" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Cittadini morti in attesa** del ritiro da un carro funebre."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Totali mensili** dalle statistiche di gioco.\n" +
                     "**Cremazione max/mese** = pannello info Handling/mese del gioco.\n" +
                     "Questo è il massimo di corpi che potrebbero essere processati dai crematori al mese."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Risorse" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Capacità attive degli edifici:** carri funebri totali, edifici, lavoratori max.\n\n" +
                    "**Notes:**\n" +
                    "▪ Carro funebre: Attivo-non parcheggiato / (Total* carri funebri)\n" +
                    "▪ *Total carro funebre:" +
                    "=== include carri funebri in manutenzione (es. budget servizio basso), \n" +
                    "=== non include carri funebri di edifici disabilitati.\n" +
                    "▪ La scansione stato gira solo mentre Options è aperto (o usi uno slider); " +
                    "non gira per-frame in città, quindi praticamente nessun impatto sulle prestazioni :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Stato non caricato." },
                { "MH_STATUS_NO_CITY_LOADED", "Nessuna città caricata." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Nessuna città... ¯\\_(ツ)_/¯ ...Nessuna statistica" },

                { "MH_STATUS_LINE1", "{0} in attesa | {1} morti/mese | aggiornato {2}" },
                { "MH_STATUS_LINE2", "{0} cremazione max/mese | {1}/{2} tombe usate" },
                { "MH_STATUS_LINE3", "{0} / {1} carri funebri | {2} / {3} edifici | {4} lavoratori max" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary4)), "Cemetery" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary4)),
                    "**Cemeteries auto-emptied this session** by Auto-empty when full.\n" +
                    "Shows total resets and how many distinct cemeteries.\n" +
                    "Clears on reboot or when you switch city."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusCemetery1)),
                    "Cemetery name × how many times it was emptied this session." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusCemetery2)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusCemetery2)),
                    "Cemetery name × how many times it was emptied this session." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusCemetery3)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusCemetery3)),
                    "Cemetery name × how many times it was emptied this session." },

                { "MH_STATUS_LINE4", "{0} resets · {1} cemeteries" },
                { "MH_STATUS_CEMETERY_NONE", "none this session" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nome visualizzato di questa mod." },
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
