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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Automatische Reinigung" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Selbst verwalten" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Erweitert" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Status" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Mod-Info" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Links" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Debug" },

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
                    "**Leert einen vollen Friedhof**, damit er nicht durch ein VOLL-Symbol blockiert wird.\n" +
                    "Magische Reinigung entfernt die meisten Verstorbenen vor der Beerdigung — diese Option leert dennoch jeden Friedhof, der **bereits voll** ist.\n" +
                    "<[ ] Standardmäßig AUS>.\n" +
                    "Aktiviere diese Option nur, wenn die magische Reinigung auch bereits volle Friedhöfe leeren soll.\n" +
                    "Nach dem Leeren muss diese Option normalerweise nicht aktiviert bleiben, solange die magische Reinigung aktiviert bleibt."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Bestattungsleiter" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Verwalte und optimiere die normalen Bestattungssysteme des Spiels selbst.\n" +
                    "**Skalierungswerte:** Rate, Flotte, Lager.\n" +
                    "Optional: **Mitarbeiter ebenfalls erhöhen**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Krematoriumsverarbeitung" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Verarbeitungsgeschwindigkeit des Krematoriums.**\n" +
                    "Höhere Werte kremieren Leichen schneller und geben die Lagerkapazität der Anlage früher frei.\n" +
                    "**100%** = Vanilla-Standard des Spiels."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Leichenwagen gesamt" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Maximale Leichenwagen** pro Anlage.\n" +
                    "**100%** = Vanilla-Standard des Spiels.\n" +
                    "**[Hinweis]** Zu viele Leichenwagen können je nach Sterberate den Verkehr beeinflussen."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Leichenwagen-Geschwindigkeit" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Erhöht die maximal zulässige Fahrgeschwindigkeit des Leichenwagens**.\n" +
                    "**100%** = Vanilla-Standard des Spiels.\n" +
                    "<Straßengeschwindigkeitsbegrenzungen gelten weiterhin>.\n" +
                    "\n" +
                    "Skaliert außerdem Beschleunigung/Bremsen (sanft), damit die neue Höchstgeschwindigkeit keine extremen Start-/Stopp-Effekte erzeugt.\n" +
                    "Hinweis: Auch wenn die Höchstgeschwindigkeit des Leichenwagens erhöht wird, wird seine tatsächliche Fahrgeschwindigkeit beeinflusst durch:\n" +
                    "zulässige Fahrzeughöchstgeschwindigkeit, Straßenlimit, die sichere Geschwindigkeit der Spiel-KI (Kurven, Straßenschäden) und den Verkehr."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Todesmeldung verzögern (Min.)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "Dies ist die Gesamtzeit, die ein Leichenwagen hat, um ein Gebäude zu erreichen, bevor **Warten-auf-Leichenwagen**-Problemsymbole erscheinen.\n" +
                    "**3 Minuten** liegen nahe am Spielstandard von etwa 2,5 Simulationsminuten.\n" +
                    "Der Wert kann erhöht werden, damit Leichenwagen mehr realistische Zeit für die Fahrt haben, bevor das Todessymbol erscheint.\n" +
                    "Hinweis:\n" +
                    "- <Empfohlen: 10 Minuten>. Bei starkem Verkehr höher versuchen.\n" +
                    "- Unten im Statusbericht siehst du, wie viele Fälle überfällig sind.\n" +
                    "- Bereits sichtbare Symbole werden beim ersten Erhöhen dieses Werts nicht ausgeblendet; sie bleiben sichtbar, bis ein Leichenwagen sie erledigt oder das Gebäude abgerissen wird.\n" +
                    "- Lass aktuelle Einsätze normal enden oder aktiviere einmal <Magische Reinigung [x]>, um mit neuen Zeitplänen schnell frisch zu starten."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Friedhofslager" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Friedhofs-Lagerkapazität** für das Hauptgebäude.\n" +
                    "Mehr Kapazität lässt einen vollen Friedhof wieder Abholungen annehmen.\n" +
                    "Es werden nicht mehr Leichenwagen ausgesendet, außer Platzmangel hat die Anlage blockiert.\n" +
                    "**100%** = Vanilla-Standard des Spiels."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Friedhof automatisch zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Leert einen vollen Friedhof**, damit er nicht durch ein VOLL-Symbol über dem Gebäude blockiert wird.\n" +
                    "Volle Friedhöfe müssen nicht mehr abgerissen und neu gebaut werden.\n" +
                    "Schalte dies AUS, um stattdessen die allmähliche **Friedhofs-Freigaberate** zu verwenden.\n" +
                    "<[ ✓ ] Standardmäßig EIN>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Friedhofs-Freigaberate" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Gibt belegte Grabplätze nach und nach wieder frei.**\n" +
                    "Höhere Werte machen Grabplätze schneller als im Vanilla-Spiel wieder verfügbar.\n" +
                    "Wenn Friedhöfe sich selbst bei 500% noch zu oft füllen,\n" +
                    "aktiviere stattdessen **[Friedhof automatisch zurücksetzen]**.\n" +
                    "**100%** = Standardrate des Spiels für die Wiederverwendung von Gräbern."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Mitarbeiter anpassen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Kompatibilitäts-Schalter:\n" +
                    "**Aktivieren [✓]**, um die Anzahl der Mitarbeiter zu erhöhen.\n" +
                    "**[o_o]** AUS lassen, wenn **ConfigXML** oder ein anderer Mod die Mitarbeiter der Bestattungsdienste steuern soll."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Maximale Mitarbeiter" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Erhöht die maximal zulässige Mitarbeiterzahl**.\n" +
                    "**100%** = Vanilla-Standard des Spiels."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Regler zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Setzt die Prozent-Regler auf **100%** und die Verzögerung der Todesmeldung auf **3 Minuten**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Leichenwagen nötig" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Wartend** = alle toten Bürger, die noch außerhalb liegen und auf Abholung warten.\n" +
                    "**Überfällig** = wartende Bürger, deren gewählte Benachrichtigungsverzögerung abgelaufen ist.\n" +
                    " - Wenn viele Fälle überfällig sind, erhöhe die Zeit bei „Todesmeldung verzögern“."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Monatliche Summen** aus den Spielstatistiken.\n" +
                    "**Max./Mon.** = Krematoriumsverarbeitung plus Friedhofs-Freigabe bei aktueller Effizienz.\n" +
                    "Dies ist die maximale Anzahl an Körpern, die alle aktiven Bestattungseinrichtungen pro Monat bewältigen könnten."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Bestand" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Kapazitäten aktiver Gebäude:** Leichenwagen gesamt, Gebäude, max. Mitarbeiter.\n" +
                    "\n" +
                    "**Hinweise:**\n" +
                    "▪ Leichenwagen: Aktiv-nicht geparkt / (Gesamt* Leichenwagen)\n" +
                    "▪ *Leichenwagen gesamt:\n" +
                    "== enthält Leichenwagen in Wartung (z. B. bei niedrigem Service-Budget), \n" +
                    "== enthält keine Leichenwagen deaktivierter Gebäude.\n" +
                    "▪ Der Status-Scan läuft nur, solange Optionen geöffnet sind (oder du einen Regler benutzt); er läuft in der Stadt nicht pro Frame und hat daher praktisch keine Leistungsauswirkungen :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status nicht geladen." },
                { "MH_STATUS_NO_CITY_LOADED", "Keine Stadt geladen." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Keine Stadt... ¯\\_(ツ)_/¯ ...Keine Stats" },

                { "MH_STATUS_LINE1_V2", "{0} wartend | {1} überfällig | {2} Tote/Mon." },
                { "MH_STATUS_LINE2_V2", "{0} max./Mon." },
                { "MH_STATUS_LINE3", "{0} / {1} Leichenwagen | {2} / {3} Gebäude | {4} max. Mitarbeiter" },
                { "MH_STATUS_UPDATED", "aktualisiert {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "jetzt empfohlen: ~{0}% Krematoriumsverarbeitung" },
                { "MH_STATUS_PROCESSING_MORE", "jetzt empfohlen: 500% Krematoriumsverarbeitung + mehr aktive Einrichtungen" },
                { "MH_STATUS_PROCESSING_NONE", "empfohlen: Krematorien aktivieren/hinzufügen" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Friedhof" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Zeigt **belegte Gräber**, aktive Friedhofsanlagen und Zurücksetzungen voller Friedhöfe in dieser Sitzung.\n" +
                    "Der Status wird beim Neustart oder beim Wechsel der Stadt gelöscht."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} Gräber belegt | {2} Anlagen | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} Zurücksetzung" },
                { "MH_STATUS_RESET_PLURAL", "{0} Zurücksetzungen" },
                { "MH_STATUS_CEMETERY_NONE", "keine in dieser Sitzung" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} weitere" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Anzeigename dieses Mods." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Aktuelle Version." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Öffnet die Paradox-Mods-Seite des Autors." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Log-Bericht" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "Schreibt einen detaillierten Bestattungsbericht und wahrscheinliche Problembereiche in MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Log öffnen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Öffnet **Logs/MagicHearse.log**, falls vorhanden.\n" +
                    "Wenn die Datei noch nicht vorhanden ist, wird stattdessen der Logs-Ordner geöffnet."
                },
            };
        }

        public void Unload()
        { }
    }
}
