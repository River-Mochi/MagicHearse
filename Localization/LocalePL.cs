// File: Localization/LocalePL.cs
// Purpose: Polish pl-PL locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocalePL : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocalePL(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), Mod.ModName + " " + Mod.ModTag },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Akcje" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "O modzie" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Auto czyszczenie" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Ręczne zarządzanie" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info o modzie" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Linki" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Włącz magię" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "Automatycznie usuwa martwych obywateli czekających na karawan.\n" +
                    "Aby wyłączyć mod bez usuwania, odznacz oba checkboxy."
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Dyrektor pogrzebowy" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Skaluje wartości placówek pogrzebowych (tempo, flota, magazyn)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Tempo przetwarzania" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "Mnożnik **szybkości przetwarzania** placówki." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Wielkość floty" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "Mnożnik **maksymalnej liczby karawanów** na placówkę." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Magazyn cmentarza" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "Zwiększa **maksymalny magazyn cmentarza**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset suwaków" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Ustawia wszystkie suwaki z powrotem na **100%** (domyślne)." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Wersja" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Otwiera stronę autora na Paradox Mods." },
            };
        }

        public void Unload()
        { }
    }
}
