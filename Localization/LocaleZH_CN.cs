// File: Localization/LocaleZH_CN.cs
// Purpose: Simplified Chinese zh-HANS locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

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
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), Mod.ModName + " " + Mod.ModTag },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "操作" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "关于" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "自动清理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "手动管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "模组信息" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "链接" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "启用魔法" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "自动移除正在等待灵车的死亡市民。\n" +
                    "想在不移除模组的情况下禁用，请取消勾选两个选项。"
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "葬仪主管" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "缩放殡葬设施数值（速度、车队、存储）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "处理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "设施**处理速度**倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "车队规模" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "每个设施的**最大灵车数量**倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地存储" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "提升**墓地最大存储量**。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "重置滑块" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "将所有滑块重置为 **100%**（原版默认）。" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "模组" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "版本" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "打开作者的 Paradox Mods 页面。" },
            };
        }

        public void Unload()
        { }
    }
}
