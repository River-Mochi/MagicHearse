// File: Localization/LocaleZH_CN.cs
// Chinese (Simplified) zh-HANS locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Chinese (Simplified) localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleZH_CN : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Chinese (Simplified) locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleZH_CN(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Chinese (Simplified) localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "操作" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "关于" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "自动清理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "自我管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "状态" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod 信息" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "链接" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "启用魔法" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**自动移除死亡市民**（正在等灵车）。\n" +
                    "两项都关 = 不卸载也能停用本 Mod。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "殡葬主管" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "缩放**设施**数值（处理、车队、存储）。\n" +
                    "可选：**增加员工**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "处理速率" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**设施处理速度**（火化）\n" +
                    "**100%** = 原版默认。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "车队规模" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "每个设施的**灵车上限**。\n" +
                    "**100%** = 原版默认。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地存储" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "墓地主建筑的**存储容量**。\n" +
                    "**100%** = 原版默认。"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "控制最大员工" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "开启后让**殡葬主管**也增加员工数量。\n" +
                    "若想让 **ConfigXML**（或其他 Mod）管员工，请保持 OFF。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "最大员工" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "缩放死亡相关设施的**最大员工数**。\n" +
                    "**100%** = 原版默认。\n\n" +
                    "**[o_o] 小提示**\n" +
                    "  - 只对<新建筑>生效。\n" +
                    "  - 加/删扩展通常会强制刷新。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "重置滑条" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "把所有滑条重置到 **100%**（原版默认）。" },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "需要灵车" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**等待灵车**的死亡市民。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "流量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "来自游戏统计的**每月总计**。\n" +
                     "**火葬 最大/月** = 游戏信息面板的 Handling/月。\n" +
                     "这是所有火葬场每月理论上最多可处理的遗体数量。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "资产" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**运行中的建筑**容量（灵车、建筑数量、最大工人）。\n\n" +
                    "**说明：**\n" +
                    "  - 包括仍在维护中的灵车（可能因为预算太低）。\n" +
                    "  - 不包括已禁用建筑的灵车。\n" +
                    "  - 状态扫描只会在选项菜单里或你调滑杆时运行；不会在城市里逐帧运行，所以几乎没有性能影响 :)"
                },


                // Status text templates
                { "MH_STATUS_NOT_LOADED", "状态未加载。" },
                { "MH_STATUS_NO_CITY_LOADED", "尚未加载城市。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "没有城市... ¯\\_(ツ)_/¯ ...没有统计" },


                { "MH_STATUS_LINE1", "{0} 死亡等待 | 更新 {1}" },
                { "MH_STATUS_LINE2", "{0} 死亡/月 | {1} 火葬 最大/月 | {2} / {3} 墓地占用" },
                { "MH_STATUS_LINE3", "{0} 灵车 | {1} / {2} 建筑 | {3} 空墓位 | {4} 最大员工" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "此 Mod 的显示名称。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "当前版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "打开作者的 Paradox Mods 页面。" },
            };
        }

        public void Unload()
        { }
    }
}
