// <copyright file="LocaleDE.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleDE.cs
// German de-DE locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// German localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleDE : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the German locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleDE(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all German localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Aktionen" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Über" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "Automatische Reinigung" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "Selbst verwalten" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "Erweitert" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "Mod-Info" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "Links" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "Debug" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Magische Reinigung aktivieren" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Entfernt automatisch tote Bürger, die transportiert werden müssen (Leichenwagen).\n" +
                    "Magische Reinigung und Selbstverwaltung schließen sich gegenseitig aus; wähle eine der beiden Optionen.\n" +
                    "Deaktiviere alle Kontrollkästchen, um den Mod auszuschalten, ohne ihn zu entfernen.\n" +
                    "Technischer Hinweis: IsDead = true und WaitingForHearse = true sind erforderlich."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Vollen Friedhof zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Leert jeden vollen Friedhof**, damit er nicht durch ein VOLL-Symbol blockiert wird.\n" +
                    "Die magische Reinigung entfernt die meisten Verstorbenen vor der Beerdigung — diese Option leert dennoch jeden Friedhof, der **bereits voll** ist.\n" +
                    "<[ ] Standardmäßig AUS>.\n" +
                    "Aktiviere diese Option nur, wenn die magische Reinigung auch bereits volle Friedhöfe leeren soll.\n" +
                    "Nach dem Leeren muss diese Option normalerweise nicht aktiviert bleiben, solange die magische Reinigung aktiviert bleibt."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Bestattungsleiter" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Alles selbst verwalten.\n" +
                    "**Skalierungswerte:** Rate, Flotte, Lager.\n" +
                    "Optional: **Mitarbeiter erhöhen** auch."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Verarbeitungsrate" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Verarbeitungsgeschwindigkeit der Anlage** (Kremationen)\n" +
                    "**100%** = Vanilla-Standard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Flottengröße" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Maximale Leichenwagen** pro Anlage.\n" +
                    "**100%** = Vanilla-Standard.\n" +
                    "**[Hinweis]** Zu viele Leichenwagen können je nach Sterberate den Verkehr beeinflussen."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Friedhofslager" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Friedhofs-Lagerkapazität** für das Hauptgebäude.\n" +
                    "**100%** = Vanilla-Standard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Vollen Friedhof zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Leert einen Friedhof**, sobald er voll ist, damit kein VOLL-Symbol über dem Gebäude den Betrieb blockiert.\n" +
                    "Volle Friedhöfe müssen nicht mehr abgerissen und neu gebaut werden.\n" +
                    "Zusammen mit dem Regler **Friedhofslager**: Lege die Größe deiner Friedhöfe fest und lass sie sich wiederverwenden, damit nie wieder ein voller Friedhof abgerissen werden muss.\n" +
                    "<[ ✓ ] Standardmäßig EIN>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Leichenwagen-Geschwindigkeit" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Erhöht die Höchstgeschwindigkeit des Leichenwagens**.\n" +
                    "**100%** = Vanilla-Standard.\n" +
                    "<Straßengeschwindigkeitsbegrenzungen gelten weiterhin>.\n\n" +
                    "Skaliert außerdem Beschleunigung/Bremsen (sanft), damit die neue Top-Speed keine extremen Start/Stopp-Effekte erzeugt.\n" +
                    "Hinweis: Auch wenn die Höchstgeschwindigkeit des Leichenwagens erhöht wird, wird seine tatsächliche Fahrgeschwindigkeit beeinflusst durch:\n" +
                    "zulässige Fahrzeughöchstgeschwindigkeit, Straßenlimit, die sichere Geschwindigkeit der Spiel-KI (Kurven, Straßenschäden) und den Verkehr."

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Max. Mitarbeiter steuern" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Kompatibilitäts-Schalter:\n" +
                    "**Aktivieren [✓]**, um die Anzahl der Arbeiter zu erhöhen.\n" +
                    "**[o_o]** OFF lassen, wenn **ConfigXML** oder ein anderer Mod die Arbeiterzahl steuern soll."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Max. Mitarbeiter" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Erhöht die maximale Arbeiterzahl**.\n" +
                    "**100%** = Vanilla-Standardwert."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Regler zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "Setzt alle Regler zurück auf **100%** (Vanilla-Standard)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Leichenwagen nötig" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Tote Bürger warten** auf eine Abholung durch den Leichenwagen."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "**Monatliche Summen** aus den Spiel-Statistiken.\n" +
                     "**Kremation max/Monat** = Handling/Monat-Info im Spiel.\n" +
                     "Das ist die maximale Anzahl an Körpern, die Krematorien pro Monat verarbeiten könnten."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Bestand" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Aktive Gebäudekapazitäten:** Leichenwagen gesamt, Gebäude, max. Mitarbeiter.\n\n" +
                    "**Hinweise:**\n" +
                    "▪ Leichenwagen: Aktiv-nicht geparkt / (Total* Leichenwagen)\n" +
                    "▪ *Total Leichenwagen:\n" +
                    "== enthält Leichenwagen in Wartung (z. B. bei niedrigem Service-Budget), \n" +
                    "== enthält keine Leichenwagen von deaktivierten Gebäuden.\n" +
                    "▪ Der Status-Scan läuft nur, während die Optionen geöffnet sind (oder ein Regler benutzt wird); " +
                    "er läuft nicht in jedem Frame der Stadt und hat daher praktisch keine Leistungsauswirkungen :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status nicht geladen." },
                { "MH_STATUS_NO_CITY_LOADED", "Keine Stadt geladen." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Keine Stadt... ¯\\_(ツ)_/¯ ...Keine Stats" },

                { "MH_STATUS_LINE1", "{0} warten | {1} Tote/Monat | aktualisiert {2}" },
                { "MH_STATUS_LINE2", "{0} Kremation max/Monat | {1}/{2} Gräber belegt" },
                { "MH_STATUS_LINE3", "{0} / {1} Leichenwagen | {2} / {3} Gebäude | {4} max. Mitarbeiter" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "Aktueller Vorschlag: ~{0}% Verarbeitungsrate" },
                { "MH_STATUS_PROCESSING_MORE", "Aktueller Vorschlag: 500% Verarbeitungsrate + mehr aktive Einrichtungen" },
                { "MH_STATUS_PROCESSING_NONE", "Vorschlag: Krematorien aktivieren/hinzufügen" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Friedhof" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**In dieser Sitzung automatisch geleerte Friedhöfe** durch ‚Vollen Friedhof zurücksetzen‘.\n" +
                    "Zeigt die Gesamtzahl der Zurücksetzungen und die Anzahl verschiedener Friedhöfe.\n" +
                    "Wird beim Neustart oder beim Wechsel der Stadt gelöscht."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "Welche Friedhöfe geleert wurden und wie oft jeweils (Name × Anzahl)." },

                { "MH_STATUS_LINE4", "Zurücksetzungen: {0} · Friedhöfe: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "keine in dieser Sitzung" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} weitere" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Anzeigename dieses Mods." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Aktuelle Version." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Öffnet die Paradox-Mods-Seite des Autors." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Log-Bericht" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "Schreibt einen detaillierten Todespflegebericht und wahrscheinliche Problembereiche in MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Log öffnen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Öffnet **Logs/MagicHearse.log**, falls vorhanden.\n" +
                    "Wenn die Datei noch nicht gefunden wird, wird stattdessen der Logs-Ordner geöffnet." },
            };
        }

        public void Unload()
        { }
    }
}
