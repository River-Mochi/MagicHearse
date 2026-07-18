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
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// German localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleDE : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the German locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleDE(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all German localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Aktionen" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Über" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "Automatische Reinigung" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "Selbst verwalten" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "Erweitert" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "Mod-Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "Links" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Magische Reinigung aktivieren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Entfernt automatisch tote Bürger** die auf einen Leichenwagen warten.\n" +
                    "Schalte beide Kontrollkästchen aus, um den Mod zu deaktivieren, ohne ihn zu entfernen."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Bestattungsleiter" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Alles selbst verwalten.\n" +
                    "**Skalierungswerte:** Rate, Flotte, Lager.\n" +
                    "Optional: **Mitarbeiter erhöhen** auch."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Verarbeitungsrate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Verarbeitungsgeschwindigkeit der Anlage** (Kremationen)\n" +
                    "**100%** = Vanilla-Standard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Flottengröße" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Maximale Leichenwagen** pro Anlage.\n" +
                    "**100%** = Vanilla-Standard.\n" +
                    "**[o_o]** Zu viele Leichenwagen können je nach Sterberate den Verkehr beeinflussen."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Friedhofslager" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Friedhofs-Lagerkapazität** für das Hauptgebäude.\n" +
                    "**100%** = Vanilla-Standard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AutoResetCemetery)), "Auto-Leeren bei Voll" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AutoResetCemetery)),
                    "**Leert einen Friedhof automatisch**, sobald er voll ist.\n" +
                    "Belegte Gräber werden auf 0 zurückgesetzt — wie Abreißen und Neubau, aber sofort und automatisch.\n" +
                    "Passt zum Regler **Friedhofslager**: Dimensioniere deine Friedhöfe und lass sie sich recyceln, damit du nie einen vollen Friedhof abreißen musst.\n" +
                    "Standardmäßig AN, solange der **Bestattungsleiter** aktiv ist."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "Leichenwagen-Geschwindigkeit" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**Erhöht die Höchstgeschwindigkeit des Leichenwagens**.\n" +
                    "**100%** = Vanilla-Standard.\n" +
                    "<Straßengeschwindigkeitsbegrenzungen gelten weiterhin>.\n\n" +
                    "Skaliert außerdem Beschleunigung/Bremsen (sanft), damit die neue Top-Speed keine extremen Start/Stopp-Effekte erzeugt.\n" +
                    "Hinweis: auch wenn die Höchstgeschwindigkeit erhöht wird, ist die reale Fahrgeschwindigkeit im Grunde:\n" +
                    "(Fahrzeugmaximum, Straßenlimit, sichere KI-Geschwindigkeit, Verkehr)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Max. Mitarbeiter steuern" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Kompatibilitäts-Schalter:\n" +
                    "**Aktivieren [✓]**, um die Anzahl der Arbeiter zu erhöhen.\n" +
                    "**[o_o]** OFF lassen, wenn **ConfigXML** oder ein anderer Mod die Arbeiterzahl steuern soll."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Max. Mitarbeiter" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**Erhöht die maximale Arbeiterzahl**.\n" +
                    "**100%** = Vanilla-Standardwert."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Regler zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Setzt alle Regler zurück auf **100%** (Vanilla-Standard)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Leichenwagen nötig" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Tote Bürger warten** auf eine Abholung durch den Leichenwagen."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Monatliche Summen** aus den Spiel-Statistiken.\n" +
                     "**Kremation max/Monat** = Handling/Monat-Info im Spiel.\n" +
                     "Das ist die maximale Anzahl an Körpern, die Krematorien pro Monat verarbeiten könnten."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Bestand" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Aktive Gebäudekapazitäten:** Leichenwagen gesamt, Gebäude, max. Mitarbeiter.\n\n" +
                    "**Notes:**\n" +
                    "▪ Leichenwagen: Aktiv-nicht geparkt / (Total* Leichenwagen)\n" +
                    "▪ *Total Leichenwagen:" +
                    "=== enthält Leichenwagen in Wartung (z.B. niedriges Service-Budget), \n" +
                    "=== enthält keine Leichenwagen von deaktivierten Gebäuden.\n" +
                    "▪ Status-Scan läuft nur, während Options offen ist (oder ein Regler benutzt wird); " +
                    "läuft nicht pro Frame in der Stadt, also praktisch kein Performance-Impact :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status nicht geladen." },
                { "MH_STATUS_NO_CITY_LOADED", "Keine Stadt geladen." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Keine Stadt... ¯\\_(ツ)_/¯ ...Keine Stats" },
             
                { "MH_STATUS_LINE1", "{0} warten | {1} Tote/Monat | aktualisiert {2}" },
                { "MH_STATUS_LINE2", "{0} Kremation max/Monat | {1}/{2} Gräber belegt" },
                { "MH_STATUS_LINE3", "{0} / {1} Leichenwagen | {2} / {3} Gebäude | {4} max. Mitarbeiter" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Anzeigename dieses Mods." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Aktuelle Version." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Öffnet die Paradox-Mods-Seite des Autors." },
            };
        }

        public void Unload()
        { }
    }
}
