// Localization/LocaleIT.cs
// Italian it-IT for Magic Hearse Redux.

namespace MagicHearse
{
    using System.Collections.Generic;
    using Colossal;

    public sealed class LocaleIT : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleIT(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                // panel name in Options -> Modding
                { m_Setting.GetSettingsLocaleID(), "Magic Hearse Redux" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nome mostrato per questa mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versione" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Versione attuale della mod Magic Hearse." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Abilita Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Rimuove automaticamente i cittadini morti in attesa del carro funebre." },

                // Links group header + button
                { "OPTIONS.GROUP[MagicHearse.MagicHearse.Mod.Links]", "Link" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Apre il sito Paradox con le mod dell’autore." },
            };
        }

        public void Unload()
        {
        }
    }
}
