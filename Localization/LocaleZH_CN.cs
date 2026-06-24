// <copyright file="LocaleZH_CN.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleZH_CN.cs
// Simplified Chinese zh-HANS locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Simplified Chinese localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleZH_CN : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Simplified Chinese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleZH_CN(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Simplified Chinese localization entries for this mod.</summary>
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
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "自动清理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "手动管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "高级" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "状态" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "模组信息" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "链接" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "启用魔法清理" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**自动移除等待灵车的死亡市民**。\n" +
                    "关闭两个复选框即可禁用模组，而无需移除它。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "葬礼管理员" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "全部自行管理。\n" +
                    "**缩放数值：** 速度、车队、存储。\n" +
                    "可选：也可**增加工人**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "处理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**设施处理速度**（火化）\n" +
                    "**100%** = 原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "车队数量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "每个设施的**灵车最大数量**。\n" +
                    "**100%** = 原版默认值。\n" +
                    "**[o_o]** 灵车过多可能会根据死亡率影响交通。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "主建筑的**墓地存储容量**。\n" +
                    "**100%** = 原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "灵车速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**提高灵车最高速度**。\n" +
                    "**100%** = 原版默认值。\n" +
                    "<仍受道路限速影响>。\n\n" +
                    "同时缩放加速/刹车（温和），避免新最高速度带来夸张的起步/急停。\n" +
                    "注意：即使提高了灵车最高速度，实际行驶速度基本由以下决定：\n" +
                    "(车辆上限、道路限速、AI安全速度、交通)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "控制最大工人数" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "兼容性开关：\n" +
                    "**启用 [✓]** 以增加工作人员数量。\n" +
                    "**[o_o]** 如果希望由 **ConfigXML** 或其他模组控制殡葬服务的工作人员数量，请保持关闭。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "最大工人数" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**提高允许的最大工作人员数**。\n" +
                    "**100%** = 原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "重置滑条" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "将所有滑条重置为 **100%**（原版默认值）。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "需要灵车" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**等待灵车接走的死亡市民**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "数量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "来自游戏统计的**月度总计**。\n" +
                     "**火化上限/月** = 游戏 Handling/月 信息面板。\n" +
                     "这是火葬场每月最多可处理的遗体数量。"
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "资源" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**活跃建筑容量：** 灵车总数、建筑数、最大工人。\n\n" +
                    "**备注：**\n" +
                    "▪ 灵车：活跃-未停放 /（总计* 灵车）\n" +
                    "▪ *总计 灵车:" +
                    "=== 包含维护中的灵车（例如服务预算较低时）, \n" +
                    "=== 不包含已禁用建筑的灵车。\n" +
                    "▪ 状态扫描仅在 Options 打开时（或使用滑条时）运行；" +
                    "不会在城市中每帧运行，因此基本没有性能影响 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "状态未加载。" },
                { "MH_STATUS_NO_CITY_LOADED", "未加载城市。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "没有城市... ¯\\_(ツ)_/¯ ...没有统计" },

                { "MH_STATUS_LINE1", "{0} 等待 | {1} 死亡/月 | 更新于 {2}" },
                { "MH_STATUS_LINE2", "{0} 火化上限/月 | {1}/{2} 墓位已用" },
                { "MH_STATUS_LINE3", "{0} / {1} 灵车 | {2} / {3} 建筑 | {4} 最大工人" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "模组" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "此模组的显示名称。" },
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
