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
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Działania" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Informacje" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Automatyczne czyszczenie" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Samodzielne zarządzanie" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Zaawansowane" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Status" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Informacje o modzie" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Linki" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Debugowanie" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Włącz Magiczne czyszczenie" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Automatycznie usuwa zmarłych wymagających transportu karawanem.\n" +
                    "Magiczne czyszczenie i samodzielne zarządzanie wzajemnie się wykluczają; wybierz jedno z nich.\n" +
                    "Wyłącz wszystkie pola wyboru, aby wyłączyć mod bez jego usuwania.\n" +
                    "Uwaga techniczna: wymagane są IsDead = true i WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Zresetuj pełny cmentarz" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Opróżnia pełny cmentarz**, aby nie był zablokowany ikoną PEŁNY.\n" +
                    "Magiczne czyszczenie usuwa większość ciał przed pochówkiem — ta opcja nadal opróżni każdy cmentarz, który **jest już pełny**.\n" +
                    "<[ ] Domyślnie WYŁ.>.\n" +
                    "Włącz tę opcję tylko wtedy, gdy tryb Magicznego czyszczenia ma również opróżniać już pełne cmentarze.\n" +
                    "Po opróżnieniu zwykle nie trzeba pozostawiać tej opcji włączonej, jeśli Magiczne czyszczenie pozostaje aktywne."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Dyrektor zakładu pogrzebowego" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Samodzielnie zarządzaj i optymalizuj normalne systemy usług pogrzebowych gry.\n" +
                    "**Wartości skali:** tempo, flota, magazynowanie.\n" +
                    "Opcjonalnie: **zwiększ także liczbę pracowników**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Przetwarzanie krematorium" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Szybkość przetwarzania krematorium.**\n" +
                    "Wyższe wartości szybciej kremują ciała i wcześniej zwalniają miejsce w obiekcie.\n" +
                    "**100%** = domyślna wartość gry."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Łączna liczba karawanów" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Maksymalna liczba karawanów** na obiekt.\n" +
                    "**100%** = domyślna wartość gry.\n" +
                    "**[Uwaga]** Zbyt wiele karawanów może wpływać na ruch w zależności od liczby zgonów."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Prędkość karawanu" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Zwiększa maksymalną dozwoloną prędkość jazdy karawanu**.\n" +
                    "**100%** = domyślna wartość gry.\n" +
                    "<Ograniczenia prędkości na drogach nadal obowiązują>.\n" +
                    "\n" +
                    "Skaluje też przyspieszanie/hamowanie (łagodnie), aby nowa prędkość maksymalna nie powodowała gwałtownych startów i zatrzymań.\n" +
                    "Uwaga: nawet po zwiększeniu prędkości maksymalnej karawanu jego rzeczywista prędkość zależy od:\n" +
                    "maksymalnej prędkości pojazdu, ograniczenia drogi, bezpiecznej prędkości AI gry (zakręty, uszkodzenia drogi) i ruchu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Opóźnienie powiadomienia o zgonie (min)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "To całkowity czas, jaki karawan ma na dotarcie do budynku, zanim pojawią się ikony problemu **oczekiwania na karawan**.\n" +
                    "**3 minuty** są zbliżone do domyślnej wartości gry wynoszącej około 2,5 minuty symulacji.\n" +
                    "Możesz zwiększyć tę wartość, aby karawany miały rozsądniejszy czas na ukończenie przejazdu przed pojawieniem się ikony zgonu.\n" +
                    "Uwaga:\n" +
                    "- <Sugerowane: 10 minut>. W bardzo zakorkowanych miastach wypróbuj więcej.\n" +
                    "- Sprawdź raport Status na dole, aby zobaczyć liczbę spóźnionych przypadków.\n" +
                    "- Już widoczne ikony nie zostaną ukryte po pierwszym zwiększeniu tej wartości; pozostaną, aż usunie je karawan lub budynek zostanie wyburzony.\n" +
                    "- Pozwól obecnym zleceniom zakończyć się naturalnie albo użyj jednorazowo pola <Magiczne czyszczenie [x]>, aby szybko zacząć od nowa z nowymi harmonogramami."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Pojemność cmentarza" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Pojemność cmentarza** dla głównego budynku.\n" +
                    "Większa pojemność pozwala pełnemu cmentarzowi ponownie przyjmować odbiory.\n" +
                    "Nie wysyła więcej karawanów, chyba że brak miejsca blokował obiekt.\n" +
                    "**100%** = domyślna wartość gry."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Automatycznie resetuj cmentarz" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Opróżnia pełny cmentarz**, aby nie był zablokowany ikoną PEŁNY nad budynkiem.\n" +
                    "Nie trzeba już usuwać i odbudowywać pełnych cmentarzy.\n" +
                    "Wyłącz tę opcję, aby zamiast tego używać stopniowego **tempa rotacji cmentarza**.\n" +
                    "<[ ✓ ] Domyślnie WŁ.>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Tempo rotacji cmentarza" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Stopniowo zwalnia zajęte miejsca na cmentarzu.**\n" +
                    "Wyższe wartości sprawiają, że miejsca stają się ponownie dostępne szybciej niż w podstawowej grze.\n" +
                    "Jeśli cmentarze nadal zapełniają się zbyt często przy 500%,\n" +
                    "włącz zamiast tego **[Automatycznie resetuj cmentarz]**.\n" +
                    "**100%** = domyślne tempo ponownego wykorzystania grobów w grze."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Dostosuj pracowników" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Przełącznik zgodności:\n" +
                    "**Włącz [✓]**, aby zwiększyć liczbę pracowników.\n" +
                    "**[o_o]** Pozostaw WYŁ., jeśli **ConfigXML** lub inny mod ma kontrolować pracowników usług pogrzebowych."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Maksymalna liczba pracowników" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Zwiększa maksymalną liczbę pracowników**.\n" +
                    "**100%** = domyślna wartość gry."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Resetuj suwaki" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Ustawia suwaki procentowe na **100%**, a opóźnienie powiadomienia o zgonie na **3 minuty**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Potrzebny karawan" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Oczekuje** = wszyscy zmarli obywatele nadal znajdujący się na zewnątrz i czekający na odbiór.\n" +
                    "**Po terminie** = oczekujący obywatele, dla których minął wybrany czas opóźnienia powiadomienia.\n" +
                    " - Jeśli wiele przypadków jest po terminie, zwiększ czas w opcji Opóźnienie powiadomienia o zgonie."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Wolumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Miesięczne sumy** ze statystyk gry.\n" +
                    "**Maks./mies.** = przetwarzanie krematoriów plus rotacja cmentarzy przy bieżącej wydajności.\n" +
                    "To maksymalna liczba ciał, jaką wszystkie aktywne obiekty pogrzebowe mogłyby obsłużyć w ciągu miesiąca."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Zasoby" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Pojemności aktywnych budynków:** łączna liczba karawanów, budynki, maks. pracowników.\n" +
                    "\n" +
                    "**Uwagi:**\n" +
                    "▪ Karawan: Aktywny-niezaparkowany / (Łącznie* karawanów)\n" +
                    "▪ *Łączna liczba karawanów:\n" +
                    "== obejmuje karawany w konserwacji (np. przy niskim budżecie usług), \n" +
                    "== nie obejmuje karawanów wyłączonych budynków.\n" +
                    "▪ Skan statusu działa tylko przy otwartych Opcjach (lub podczas używania suwaka); nie działa co klatkę w mieście, więc praktycznie nie wpływa na wydajność :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status nie został wczytany." },
                { "MH_STATUS_NO_CITY_LOADED", "Nie wczytano miasta." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Brak miasta... ¯\\_(ツ)_/¯ ...Brak statystyk" },

                { "MH_STATUS_LINE1_V2", "{0} oczekuje | {1} po terminie | {2} zgonów/mies." },
                { "MH_STATUS_LINE2_V2", "{0} maks./mies." },
                { "MH_STATUS_LINE3", "{0} / {1} karawanów | {2} / {3} budynków | {4} maks. pracowników" },
                { "MH_STATUS_UPDATED", "zaktualizowano {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "teraz sugerowane: ~{0}% przetwarzania krematorium" },
                { "MH_STATUS_PROCESSING_MORE", "teraz sugerowane: 500% przetwarzania krematorium + więcej aktywnych obiektów" },
                { "MH_STATUS_PROCESSING_NONE", "sugerowane: włącz/dodaj krematoria" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cmentarz" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Pokazuje **użyte groby**, aktywne obiekty cmentarne i resety pełnych cmentarzy w tej sesji.\n" +
                    "Status jest czyszczony po ponownym uruchomieniu lub zmianie miasta."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} użytych grobów | {2} obiekty | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} reset" },
                { "MH_STATUS_RESET_PLURAL", "{0} resetów" },
                { "MH_STATUS_CEMETERY_NONE", "brak w tej sesji" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} więcej" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Wyświetlana nazwa tego moda." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Wersja" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Bieżąca wersja." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Otwiera stronę autora w Paradox Mods." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Raport dziennika" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "Zapisuje szczegółowy raport usług pogrzebowych i prawdopodobne obszary problemów do MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Otwórz dziennik" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Otwiera **Logs/MagicHearse.log**, jeśli istnieje.\n" +
                    "Jeśli plik jeszcze nie istnieje, otwiera zamiast tego folder Logs."
                },
            };
        }

        public void Unload()
        { }
    }
}
