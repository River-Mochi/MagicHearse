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
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Info" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Auto-Clean" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Selbst verwalten" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod-Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Magie aktivieren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Entfernt automatisch tote Bürger**,\n" +
                    "die auf einen Leichenwagen warten.\n" +
                    "Beide Häkchen AUS = Mod deaktiviert, ohne sie zu entfernen."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Bestatter" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Skaliert **Gebäude**-Werte (Rate, Fahrzeuge, Lager).\n" +
                    "Optional: **mehr Arbeiter**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Verarbeitungsrate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Verarbeitungstempo** (Kremierungen)\n" +
                    "**100%** = Vanilla-Standard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Fuhrparkgröße" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Maximale Leichenwagen** pro Gebäude.\n" +
                    "**100%** = Vanilla-Standard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Friedhofs-Speicher" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Friedhofs-Kapazität** (Hauptgebäude).\n" +
                    "**100%** = Vanilla-Standard."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Max. Arbeiter steuern" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Aktivieren, damit der **Bestatter** mehr Arbeiter zulässt.\n" +
                    "AUS lassen, wenn **ConfigXML** (oder ein anderer Mod) Arbeiter steuern soll."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Max. Arbeiter" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Skaliert **maximale Arbeiter** für Todesfürsorge.\n" +
                    "**100%** = Vanilla-Standard.\n\n" +
                    "**[o_o] Tipps**\n" +
                    "  - Wirkt bei **neuen Gebäuden**.\n" +
                    "  - Erweiterung hinzufügen/löschen erzwingt oft ein Update."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Regler zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Setzt alle Regler auf **100%** (Vanilla-Standard)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Leichenwagen nötig" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Tote Bürger warten** auf Abholung."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Menge" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Monatssummen** aus Spiel-Statistiken.\n" +
                     "**Kremation max/Monat** = Anzeige „Handling/Monat“ im Spiel.\n" +
                     "Das ist die maximale Anzahl an Körpern, die alle Krematorien pro Monat verarbeiten könnten."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Assets" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Kapazitäten aktiver Gebäude:** Gesamt-Leichenwagen, Gebäude, max. Mitarbeiter.\n\n" +
                    "**Hinweise:**\n" +
                    "▪ Leichenwagen: aktiv (nicht geparkt) / Gesamtkapazität*\n" +
                    "▪ *Gesamtkapazität = Summe der Leichenwagen-Slots aktiver Gebäude (Effizienz > 0).\n" +
                    "  Kann auch geparkte/nicht verfügbare Leichenwagen enthalten.\n" +
                    "▪ Status-Scan läuft nur, wenn das Optionsmenü offen ist (oder nach einer Änderung).\n" +
                    "  Läuft nicht pro Frame in der Stadt – Performance-Impact ist minimal."
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status nicht geladen." },
                { "MH_STATUS_NO_CITY_LOADED", "Noch keine Stadt geladen." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Keine Stadt... ¯\\_(ツ)_/¯ ...Keine Stats" },


                { "MH_STATUS_LINE1", "{0} Tote warten | aktualisiert {1}" },
                { "MH_STATUS_LINE2", "{0} Tode/Monat | {1} Kremation max/Monat | {2} / {3} Friedhof belegt" },
                { "MH_STATUS_LINE3", "{0} / {1} Leichenwagen | {2} / {3} Gebäude | {4} freie Gräber | {5} max. Arbeiter" },

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
