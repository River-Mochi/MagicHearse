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
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Italian localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleIT : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Italian locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleIT(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Italian localization entries for this mod.</summary>
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            string title = Mod.kModName;

            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title = title + " (" + Mod.ModVersion + ")";
            }
            return new Dictionary<string, string>

            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), title },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Azioni" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Informazioni" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Pulizia automatica" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Gestione manuale" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Avanzate" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Stato" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Info mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Link" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Debug" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Abilita pulizia magica" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Rimuove automaticamente i corpi che richiedono il trasporto (carro funebre).\n" +
                    "Pulizia magica e gestione manuale si escludono a vicenda; scegline una.\n" +
                    "Disattiva tutte le caselle per disabilitare la mod senza rimuoverla.\n" +
                    "Nota tecnica: sono necessari IsDead = true e WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Reimposta cimitero pieno" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Svuota un cimitero pieno** affinché non resti bloccato con l’icona PIENO.\n" +
                    "Pulizia magica rimuove la maggior parte dei corpi prima della sepoltura — questa opzione svuota comunque qualsiasi cimitero **già pieno**.\n" +
                    "<[ ] Disattivato per impostazione predefinita>.\n" +
                    "Attiva questa opzione solo se la modalità Pulizia magica deve svuotare anche i cimiteri già pieni.\n" +
                    "Una volta svuotato, normalmente non è necessario lasciare attiva questa opzione finché Pulizia magica resta attiva."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Direttore funebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Gestisci e ottimizza manualmente i normali sistemi funerari del gioco.\n" +
                    "**Valori di scala:** ritmo, flotta, stoccaggio.\n" +
                    "Opzionale: **aumenta anche i lavoratori**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Trattamento del crematorio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Velocità di trattamento del crematorio.**\n" +
                    "Valori più alti cremano i corpi e liberano prima lo spazio della struttura.\n" +
                    "**100%** = valore vanilla del gioco."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Totale carri funebri" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Numero massimo di carri funebri** per struttura.\n" +
                    "**100%** = valore vanilla del gioco.\n" +
                    "**[Nota]** Troppi carri funebri possono influire sul traffico a seconda del tasso di mortalità."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Velocità del carro funebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Aumenta la velocità di guida massima consentita del carro funebre**.\n" +
                    "**100%** = valore vanilla del gioco.\n" +
                    "<I limiti di velocità della strada si applicano ancora>.\n" +
                    "\n" +
                    "Scala anche accelerazione/frenata (dolce) così la nuova velocità massima non crea partenze o arresti estremi.\n" +
                    "Nota: anche se aumenta la velocità massima del carro funebre, la velocità effettiva è influenzata da:\n" +
                    "massimo consentito del veicolo, limite stradale, velocità sicura dell’IA del gioco (curve, danni stradali) e traffico."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Ritardo avviso decesso (min)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "È il tempo totale che un carro funebre ha per raggiungere un edificio prima che compaiano le icone di problema **in attesa del carro funebre**.\n" +
                    "**3 minuti** è vicino al valore predefinito del gioco di circa 2,5 minuti di simulazione.\n" +
                    "Puoi aumentarlo per dare ai carri funebri un tempo più ragionevole per completare il viaggio prima che compaia l’icona del decesso.\n" +
                    "Nota:\n" +
                    "- <Suggerito: 10 minuti>. Prova un valore più alto nelle città molto congestionate.\n" +
                    "- Controlla il rapporto Stato in basso per vedere quanti casi sono scaduti.\n" +
                    "- Le icone già visibili non vengono nascoste quando aumenti questo valore per la prima volta; restano finché non vengono risolte da un carro funebre o dalla demolizione dell’edificio.\n" +
                    "- Lascia che le spedizioni attuali finiscano normalmente oppure usa una volta la casella <Pulizia magica [x]> per ripartire rapidamente con i nuovi tempi."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Stoccaggio del cimitero" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacità di stoccaggio del cimitero** per l’edificio principale.\n" +
                    "Una capacità maggiore permette a un cimitero pieno di accettare di nuovo i ritiri.\n" +
                    "Non invia più carri funebri, a meno che la mancanza di spazio bloccasse la struttura.\n" +
                    "**100%** = valore vanilla del gioco."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Ripristino automatico cimitero" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Svuota un cimitero pieno** così non resta bloccato dall’icona PIENO sopra l’edificio.\n" +
                    "Non serve più eliminare e ricostruire i cimiteri pieni.\n" +
                    "Disattiva questa opzione per usare invece il **Ricambio del cimitero** graduale.\n" +
                    "<[ ✓ ] Attivo per impostazione predefinita>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Ricambio del cimitero" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Libera gradualmente le tombe occupate del cimitero.**\n" +
                    "Valori più alti rendono di nuovo disponibili i posti più rapidamente rispetto al vanilla.\n" +
                    "Se i cimiteri si riempiono ancora troppo spesso al 500%,\n" +
                    "attiva invece **[Ripristino automatico cimitero]**.\n" +
                    "**100%** = velocità predefinita del gioco per il riutilizzo delle tombe."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Regola lavoratori" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Interruttore di compatibilità:\n" +
                    "**Abilita [✓]** per aumentare il numero di lavoratori.\n" +
                    "**[o_o]** Lascia DISATTIVATO se vuoi che **ConfigXML** o un’altra mod controlli i lavoratori dei servizi funebri."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Lavoratori massimi" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Aumenta il massimo di lavoratori** consentiti.\n" +
                    "**100%** = valore vanilla del gioco."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Reset slider" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Imposta gli slider percentuali su **100%** e il ritardo dell’avviso di decesso su **3 minuti**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Carro funebre necessario" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**In attesa** = tutti i cittadini deceduti ancora all’esterno e in attesa di ritiro.\n" +
                    "**Scaduti** = cittadini in attesa per i quali è scaduto il ritardo di notifica selezionato.\n" +
                    " - Se ci sono molti casi scaduti, valuta di aumentare il Ritardo avviso decesso."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Totali mensili** dalle statistiche di gioco.\n" +
                    "**Max/mese** = trattamento dei crematori più ricambio dei cimiteri all’efficienza attuale.\n" +
                    "È il massimo di corpi che tutte le strutture funebri attive possono gestire al mese."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Risorse" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacità attive degli edifici:** carri funebri totali, edifici, lavoratori max.\n" +
                    "\n" +
                    "**Note:**\n" +
                    "▪ Carro funebre: Attivo-non parcheggiato / (Totale* carri funebri)\n" +
                    "▪ *Totale carri funebri:\n" +
                    "== include i carri funebri in manutenzione (ad es. per un budget di servizio basso), \n" +
                    "== non include i carri funebri degli edifici disabilitati.\n" +
                    "▪ La scansione dello stato viene eseguita solo mentre le Opzioni sono aperte (o quando si usa uno slider); non viene eseguita a ogni fotogramma in città, quindi non ha praticamente alcun impatto sulle prestazioni :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Stato non caricato." },
                { "MH_STATUS_NO_CITY_LOADED", "Nessuna città caricata." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Nessuna città... ¯\\_(ツ)_/¯ ...Nessuna statistica" },

                { "MH_STATUS_LINE1_V2", "{0} in attesa | {1} scaduti | {2} morti/mese" },
                { "MH_STATUS_LINE2_V2", "{0} max/mese" },
                { "MH_STATUS_LINE3", "{0} / {1} carri funebri | {2} / {3} edifici | {4} lavoratori max" },
                { "MH_STATUS_UPDATED", "aggiornato {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "suggerito ora: trattamento dei crematori ~{0}%" },
                { "MH_STATUS_PROCESSING_MORE", "suggerito ora: trattamento dei crematori al 500% + più strutture attive" },
                { "MH_STATUS_PROCESSING_NONE", "suggerito: attiva/aggiungi crematori" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cimitero" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Mostra **tombe usate**, strutture cimiteriali attive e ripristini di cimiteri pieni in questa sessione.\n" +
                    "Lo stato si azzera al riavvio o quando cambi città."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} tombe usate | {2} strutture | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} ripristino" },
                { "MH_STATUS_RESET_PLURAL", "{0} ripristini" },
                { "MH_STATUS_CEMETERY_NONE", "nessuno in questa sessione" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+altri {0}" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nome visualizzato di questa mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Versione" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Versione attuale." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Apre la pagina Paradox Mods dell’autore." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Rapporto di log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "Scrive un rapporto dettagliato sui servizi funebri e sui probabili problemi in MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Apri log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Apre **Logs/MagicHearse.log** se esiste.\n" +
                    "Se il file non esiste ancora, apre invece la cartella Logs."
                },
            };
        }

        public void Unload()
        { }
    }
}
