// File: Localization/LocalePL.cs
// Polish pl-PL locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Polish localization source for Magic Hearse [MH].</summary>
    public sealed class LocalePL : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Polish locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocalePL(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Polish localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Akcje" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "O modzie" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Auto czyszczenie" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Zarządzanie" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info moda" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Linki" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Włącz magię" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Automatycznie usuwa zmarłych**\n" +
                    "czekających na karawan.\n" +
                    "Wyłącz oba pola, aby wyłączyć mod bez usuwania go."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Zarządca pogrzebów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Skaluje wartości **obiektów** (tempo, flota, magazyn).\n" +
                    "Opcjonalnie: **zwiększ pracowników**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Tempo przetwarzania" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Szybkość przetwarzania** (kremacje)\n" +
                    "**100%** = domyślne (vanilla)."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Wielkość floty" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Maks. karawanów** na obiekt.\n" +
                    "**100%** = domyślne (vanilla)."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Magazyn cmentarza" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Pojemność magazynu** cmentarza (główny budynek).\n" +
                    "**100%** = domyślne (vanilla)."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Kontroluj max pracowników" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Włącz, aby **Zarządca pogrzebów** zwiększał liczbę pracowników.\n" +
                    "Zostaw OFF, jeśli **ConfigXML** (lub inny mod) ma kontrolować pracowników."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Max pracowników" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Skaluje **maksymalną liczbę pracowników** dla obiektów pogrzebowych.\n" +
                    "**100%** = domyślne (vanilla).\n\n" +
                    "**[o_o] Wskazówki**\n" +
                    "  - Zmiany dotyczą **nowych budynków**.\n" +
                    "  - Dodanie/usunięcie ulepszenia często wymusza odświeżenie."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset suwaków" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Ustawia wszystkie suwaki na **100%** (vanilla)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Potrzebny karawan" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Zmarli** czekający na odbiór."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Wolumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "Ze statystyk gry: **miesięczne sumy**.\n" +
                     "**Kremacje max/mies.** = panel info gry „Handling/mies.”.\n" +
                     "To maksymalna liczba ciał, które wszystkie krematoria mogłyby przerobić w miesiącu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Zasoby" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Pojemności aktywnych budynków:** łączna liczba karawanów, budynki, maks. pracownicy.\n\n" +
                    "**Uwagi:**\n" +
                    "▪ Karawany: aktywne (niezaparkowane) / całkowita pojemność*\n" +
                    "▪ *Całkowita pojemność = suma slotów karawanów w aktywnych budynkach (wydajność > 0).\n" +
                    "  Może obejmować karawany zaparkowane/niedostępne.\n" +
                    "▪ Skan statusu działa tylko przy otwartych Opcjach (albo po zmianie ustawień).\n" +
                    "  Nie działa co klatkę w mieście – wpływ na wydajność jest minimalny."
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status niezaładowany." },
                { "MH_STATUS_NO_CITY_LOADED", "Brak wczytanego miasta." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Brak miasta... ¯\\_(ツ)_/¯ ...Brak statystyk" },


                { "MH_STATUS_LINE1", "{0} zmarłych czeka | akt. {1}" },
                { "MH_STATUS_LINE2", "{0} zgony/mies. | {1} kremacje max/mies. | {2} / {3} użycie cmentarza" },
                { "MH_STATUS_LINE3", "{0} / {1} karawanów | {2} / {3} budynków | {4} wolnych grobów | {5} max pracowników" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nazwa wyświetlana modu." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Wersja" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Aktualna wersja." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Otwiera stronę autora na Paradox Mods." },
            };
        }

        public void Unload()
        { }
    }
}
