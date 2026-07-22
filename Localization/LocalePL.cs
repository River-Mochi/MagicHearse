// <copyright file="LocalePL.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

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
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Polish locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocalePL(MHSetting setting)
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.ActionsTab), "Akcje" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.AboutTab), "O modzie" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AutoCleanGrp),   "Auto czyszczenie" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.SelfManageGrp),  "Ręczne zarządzanie" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AdvancedGrp),    "Zaawansowane" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.StatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutInfoGrp),   "Informacje o modzie" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutLinksGrp),  "Linki" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Włącz magiczne czyszczenie" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Automatycznie usuwa ciała wymagające transportu (karawanem).\n" +
                    "Magiczne czyszczenie i samodzielne zarządzanie wzajemnie się wykluczają; wybierz jedną z tych opcji.\n" +
                    "Wyłącz wszystkie pola wyboru, aby wyłączyć mod bez jego usuwania.\n" +
                    "Uwaga techniczna: wymagane są IsDead = true oraz WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Resetuj pełny cmentarz" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Opróżnia każdy pełny cmentarz**, aby nie był zablokowany ikoną PEŁNY.\n" +
                    "Magiczne czyszczenie usuwa większość ciał przed pochówkiem — ta opcja opróżnia także każdy cmentarz, który **jest już pełny**.\n" +
                    "[ ✓ ] Domyślnie WŁ.\n" +
                    "Jeśli nie ma pełnych cmentarzy, nie stanowi to problemu i magiczne czyszczenie jest zawsze włączone,\n" +
                    " tę opcję można wyłączyć, ponieważ nie jest potrzebna."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Dyrektor pogrzebowy" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Zarządzaj wszystkim ręcznie.\n" +
                    "**Wartości skali:** tempo, flota, magazyn.\n" +
                    "Opcjonalnie: **zwiększ pracowników** też."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Szybkość przetwarzania" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Szybkość przetwarzania obiektu** (kremacje)\n" +
                    "**100%** = domyślne ustawienie gry (vanilla)."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Wielkość floty" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Maksymalna liczba karawanów** na obiekt.\n" +
                    "**100%** = domyślne ustawienie gry (vanilla).\n" +
                    "**[Uwaga]** Zbyt wiele karawanów może wpływać na ruch w zależności od liczby zgonów."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Pojemność cmentarza" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Pojemność magazynu cmentarza** dla głównego budynku.\n" +
                    "**100%** = domyślne ustawienie gry (vanilla)."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Resetuj pełny cmentarz" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Opróżnia cmentarz**, gdy jest pełny, aby nie blokowała go ikona PEŁNY nad budynkiem.\n" +
                    "Nie trzeba już usuwać i odbudowywać pełnych cmentarzy.\n" +
                    "Współpracuje z suwakiem **Pojemność cmentarza**: ustaw wielkość cmentarzy i pozwól na ich ponowne wykorzystanie, aby nigdy więcej nie burzyć pełnego cmentarza.\n" +
                    "<[ ✓ ] Domyślnie WŁ.>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Prędkość karawanu" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Zwiększa prędkość maksymalną karawanu**.\n" +
                    "**100%** = domyślne ustawienie gry (vanilla).\n" +
                    "<Limity prędkości dróg nadal obowiązują>.\n\n" +
                    "Skaluje też przyspieszenie/hamowanie (łagodnie), aby nowa prędkość maksymalna nie powodowała ekstremalnych startów/stopów.\n" +
                    "Uwaga: nawet po zwiększeniu prędkości maksymalnej karawanu na jego rzeczywistą prędkość wpływają:\n" +
                    "dozwolona prędkość maksymalna pojazdu, ograniczenie drogi, bezpieczna prędkość AI gry (zakręty, uszkodzenia drogi) oraz ruch."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Kontroluj maks. pracowników" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Przełącznik zgodności:\n" +
                    "**Włącz [✓]**, aby zwiększyć liczbę pracowników.\n" +
                    "**[o_o]** Zostaw OFF, jeśli **ConfigXML** lub inny mod ma kontrolować pracowników usług pogrzebowych."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Maks. pracownicy" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Zwiększa maksymalną liczbę pracowników**.\n" +
                    "**100%** = domyślne ustawienie gry (vanilla)."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Reset suwaków" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "Ustawia wszystkie suwaki z powrotem na **100%** (domyślne wartości)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Potrzebny karawan" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Martwi obywatele czekają** na odbiór przez karawan."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Wolumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "**Miesięczne sumy** ze statystyk gry.\n" +
                     "**Kremacje max/mies.** = panel info Handling/mies. w grze.\n" +
                     "To maksymalna liczba ciał, które mogłyby zostać przetworzone przez krematoria w miesiącu."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Zasoby" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Aktywne pojemności budynków:** karawany łącznie, budynki, maks. pracownicy.\n\n" +
                    "**Uwagi:**\n" +
                    "▪ Karawan: aktywny-nie zaparkowany / (Razem* karawany)\n" +
                    "▪ *Razem karawan:\n" +
                    "== obejmuje karawany w serwisie (np. niski budżet usług), \n" +
                    "== nie obejmuje karawanów z wyłączonych budynków.\n" +
                    "▪ Skan statusu działa tylko, gdy Options jest otwarte (albo gdy używasz suwaka); " +
                    "nie działa co klatkę w mieście, więc praktycznie brak wpływu na wydajność :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status nie został załadowany." },
                { "MH_STATUS_NO_CITY_LOADED", "Brak wczytanego miasta." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Brak miasta... ¯\\_(ツ)_/¯ ...Brak statystyk" },

                { "MH_STATUS_LINE1", "{0} czeka | {1} zgony/mies. | zaktualizowano {2}" },
                { "MH_STATUS_LINE2", "{0} kremacje max/mies. | {1}/{2} groby użyte" },
                { "MH_STATUS_LINE3", "{0} / {1} karawany | {2} / {3} budynki | {4} maks. pracownicy" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cmentarz" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Cmentarze automatycznie opróżnione w tej sesji** przez opcję Resetuj pełny cmentarz.\n" +
                    "Pokazuje łączną liczbę resetów i liczbę różnych cmentarzy.\n" +
                    "Dane są czyszczone po ponownym uruchomieniu lub zmianie miasta."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "Które cmentarze opróżniono i ile razy każdy z nich (nazwa × liczba)." },

                { "MH_STATUS_LINE4", "resety: {0} · cmentarze: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "brak w tej sesji" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} więcej" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nazwa wyświetlana w menedżerze modów." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Wersja" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Aktualna wersja moda." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Otwiera stronę autora w serwisie Paradox Mods." },
            };
        }

        public void Unload()
        { }
    }
}
