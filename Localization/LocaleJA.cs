// Localization/LocaleJA.cs
// Japanese ja-JP for Magic Hearse Redux.

namespace MagicHearse
{
    using System.Collections.Generic;
    using Colossal;

    public sealed class LocaleJA : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleJA(Setting setting)
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

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "MOD" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "このMODの表示名です。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "バージョン" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Magic Hearse MOD の現在のバージョンです。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "マジックを有効にする" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "霊柩車を待っている死亡した市民を自動で削除します。" },

                // Links group header + button
                { "OPTIONS.GROUP[MagicHearse.MagicHearse.Mod.Links]", "リンク" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "作者のMOD一覧ページを Paradox のサイトで開きます。" },
            };
        }

        public void Unload()
        {
        }
    }
}
