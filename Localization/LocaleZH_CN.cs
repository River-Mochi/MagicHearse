// Localization/LocaleZH_CN.cs
// Simplified Chinese zh-HANS for Magic Hearse Redux.

namespace MagicHearse
{
    using System.Collections.Generic;
    using Colossal;

    public sealed class LocaleZH : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleZH(Setting setting)
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
                { m_Setting.GetSettingsLocaleID(), "Magic Hearse Redux 魔法灵车" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "模组" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "在这里显示的模组名称。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Magic Hearse 模组的当前版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "启用 Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "自动清理正在等待灵车的死亡市民。" },

                // Links group header + button
                { "OPTIONS.GROUP[MagicHearse.MagicHearse.Mod.Links]", "链接" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "打开 Paradox 网站上的作者模组页面。" },
            };
        }

        public void Unload()
        {
        }
    }
}
