// Localization/LocalePT_BR.cs
// Portuguese pt-BR for Magic Hearse Redux.

namespace MagicHearse
{
    using System.Collections.Generic;
    using Colossal;

    public sealed class LocalePT_BR : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocalePT_BR(Setting setting)
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
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nome exibido para este mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Versão atual do mod Magic Hearse." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Ativar Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Remove automaticamente cidadãos mortos que estão esperando o carro funerário." },

                // Links group header + button
                { "OPTIONS.GROUP[MagicHearse.MagicHearse.Mod.Links]", "Links" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Abre o site da Paradox com os mods do autor." },
            };
        }

        public void Unload()
        {
        }
    }
}
