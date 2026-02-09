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
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "Auto czyszczenie" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "Ręczne zarządzanie" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "Zaawansowane" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "Informacje o modzie" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "Linki" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Włącz magiczne czyszczenie" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Automatycznie usuwa martwych obywateli**, którzy czekają na karawan.\n" +
                    "Wyłącz oba pola wyboru, aby wyłączyć mod bez usuwania go."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Dyrektor pogrzebowy" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Zarządzaj wszystkim ręcznie.\n" +
                    "**Wartości skali:** tempo, flota, magazyn.\n" +
                    "Opcjonalnie: **zwiększ pracowników** też."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Szybkość przetwarzania" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Szybkość przetwarzania obiektu** (kremacje)\n" +
                    "**100%** = domyślne ustawienie gry (vanilla)."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Wielkość floty" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Maksymalna liczba karawanów** na obiekt.\n" +
                    "**100%** = domyślne ustawienie gry (vanilla).\n" +
                    "**[o_o]** Zbyt wiele karawanów może wpływać na ruch w zależności od liczby zgonów."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Pojemność cmentarza" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Pojemność magazynu cmentarza** dla głównego budynku.\n" +
                    "**100%** = domyślne ustawienie gry (vanilla)."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "Prędkość karawanu" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**Zwiększa prędkość maksymalną karawanu**.\n" +
                    "**100%** = domyślne ustawienie gry (vanilla).\n" +
                    "<Limity prędkości dróg nadal obowiązują>.\n\n" +
                    "Skaluje też przyspieszenie/hamowanie (łagodnie), aby nowa prędkość maksymalna nie powodowała ekstremalnych startów/stopów.\n" +
                    "Uwaga: nawet jeśli prędkość maksymalna karawanu jest zwiększona, jego rzeczywista prędkość to w praktyce:\n" +
                    "(maks. pojazdu, limit drogi, bezpieczna prędkość AI, ruch)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Kontroluj maks. pracowników" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Przełącznik zgodności:\n" +
                    "**Włącz [✓]**, aby zwiększyć liczbę pracowników.\n" +
                    "**[o_o]** Zostaw OFF, jeśli **ConfigXML** lub inny mod ma kontrolować pracowników usług pogrzebowych."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Maks. pracownicy" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**Zwiększa maksymalną liczbę pracowników**.\n" +
                    "**100%** = domyślne ustawienie gry (vanilla)."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset suwaków" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Ustawia wszystkie suwaki z powrotem na **100%** (domyślne wartości)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Potrzebny karawan" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Martwi obywatele czekają** na odbiór przez karawan."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Wolumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Miesięczne sumy** ze statystyk gry.\n" +
                     "**Kremacje max/mies.** = panel info Handling/mies. w grze.\n" +
                     "To maksymalna liczba ciał, które mogłyby zostać przetworzone przez krematoria w miesiącu."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Zasoby" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Aktywne pojemności budynków:** karawany łącznie, budynki, maks. pracownicy.\n\n" +
                    "**Uwagi:**\n" +
                    "▪ Karawan: aktywny-nie zaparkowany / (Razem* karawany)\n" +
                    "▪ *Razem karawan:" +
                    "=== obejmuje karawany w serwisie (np. niski budżet usług), \n" +
                    "=== nie obejmuje karawanów z wyłączonych budynków.\n" +
                    "▪ Skan statusu działa tylko, gdy Options jest otwarte (albo gdy używasz suwaka); " +
                    "nie działa co klatkę w mieście, więc praktycznie brak wpływu na wydajność :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status nie został załadowany." },
                { "MH_STATUS_NO_CITY_LOADED", "Nie wczytano miasta." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Brak miasta... ¯\\_(ツ)_/¯ ...Brak statystyk" },

                { "MH_STATUS_LINE1", "{0} martwi czekają | zaktualizowano {1}" },
                { "MH_STATUS_LINE2", "{0} zgony/mies. | {1} kremacje max/mies. | {2} / {3} użycie cmentarza" },
                { "MH_STATUS_LINE3", "{0} / {1} karawany | {2} / {3} budynki | {4} puste groby | {5} maks. pracownicy" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Wyświetlana nazwa tego moda." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Wersja" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Aktualna wersja." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Otwiera stronę Paradox Mods autora." },
            };
        }

        public void Unload()
        { }
    }
}
