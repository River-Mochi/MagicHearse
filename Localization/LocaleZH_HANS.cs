// <copyright file="LocaleZH_HANS.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleZH_HANS.cs
// Simplified Chinese zh-HANS locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Simplified Chinese localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleZH_HANS : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Simplified Chinese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleZH_HANS(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Simplified Chinese localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "操作" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "关于" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "自动清理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "手动管理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "高级" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "状态" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "模组信息" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "链接" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "调试" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "启用魔法清理" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "自动移除需要运输（灵车）的遗体。\n" +
                    "魔法清理与自行管理互斥，请二选一。\n" +
                    "关闭所有复选框即可禁用模组，而无需移除它。\n" +
                    "技术说明：必须满足 IsDead = true 且 WaitingForHearse = true。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "重置已满墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**清空所有已满墓地**，使其不会被“已满”图标阻塞。\n" +
                    "魔法清理会在下葬前移除大多数遗体；此选项仍会清空任何**已经满员**的墓地。\n" +
                    "<[ ] 默认关闭>。\n" +
                    "仅当需要魔法清理模式同时清空已经满员的墓地时，才启用此选项。\n" +
                    "清空后，只要魔法清理保持启用，通常就无需继续启用此选项。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "葬礼管理员" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "全部自行管理。\n" +
                    "**缩放数值：** 速度、车队、存储。\n" +
                    "可选：也可**增加工人**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "火葬场处理" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**火葬场处理速度。**\n" +
                    "数值越高，遗体火化和设施存储空间释放得越快。\n" +
                    "**100%** = 原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "灵车总数" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "每个设施的**灵车最大数量**。\n" +
                    "**100%** = 原版默认值。\n" +
                    "**[注意]** 灵车过多可能会根据死亡率影响交通。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "灵车速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**提高灵车最高速度**。\n" +
                    "**100%** = 原版默认值。\n" +
                    "<仍受道路限速影响>。\n\n" +
                    "同时缩放加速/刹车（温和），避免新最高速度带来夸张的起步/急停。\n" +
                    "注意：即使提高了灵车最高速度，其实际行驶速度仍受以下因素影响：\n" +
                    "车辆允许的最高速度、道路限速、游戏 AI 的安全速度（弯道、道路损坏）以及交通状况。"

                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "灵车警告延迟（分钟）" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "**显示“等待灵车”问题图标前的模拟分钟数。**\n" +
                    "**3 分钟**接近原版约 2.5 分钟的设置。\n" +
                    "仅更改灵车警告；救护车警告仍使用游戏设置。\n" +
                    "增大此值不会隐藏已经显示的图标。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "墓地容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "主建筑的**墓地存储容量**。\n" +
                    "容量越大，已满墓地越能重新接收遗体。\n" +
                    "除非设施因空间不足而停止服务，否则不会直接派出更多灵车。\n" +
                    "**100%** = 原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "自动重置墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "墓地已满时将其**清空**，避免建筑上方的“已满”图标阻塞服务。\n" +
                    "无需再删除并重建已满的墓地。\n" +
                    "关闭此选项可改用逐步的**墓位周转速度**。\n" +
                    "<[ ✓ ] 默认开启>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "墓位周转速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**逐步释放已占用的墓位。**\n" +
                    "数值越高，墓位就会比原版更快恢复可用。\n" +
                    "如果设为 500% 后墓地仍然经常满载，请改用 **[自动重置墓地]**。\n" +
                    "**100%** = 原版默认值。"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "调整工人数" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "兼容性开关：\n" +
                    "**启用 [✓]** 以增加工作人员数量。\n" +
                    "**[o_o]** 如果希望由 **ConfigXML** 或其他模组控制殡葬服务的工作人员数量，请保持关闭。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "最大工人数" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**提高允许的最大工作人员数**。\n" +
                    "**100%** = 原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "重置滑条" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "将百分比滑条重置为 **100%**，灵车警告延迟重置为 **3 分钟**。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "需要灵车" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**等待灵车接走的死亡市民**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "数量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "来自游戏统计的**月度总计**。\n" +
                     "**最大处理量/月** = 当前效率下的火葬场处理量与墓位周转量之和。\n" +
                     "这是所有运行中的殡葬设施每月最多可处理的遗体数量。"
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "资源" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**活跃建筑容量：** 灵车总数、建筑数、最大工人。\n\n" +
                    "**备注：**\n" +
                    "▪ 灵车：活跃-未停放 /（总计* 灵车）\n" +
                    "▪ *灵车总数：\n" +
                    "== 包含维护中的灵车（例如服务预算较低时），\n" +
                    "== 不包含已禁用建筑的灵车。\n" +
                    "▪ 状态扫描仅在选项菜单打开时（或使用滑条时）运行；" +
                    "不会在城市中每帧运行，因此基本没有性能影响 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "状态未加载。" },
                { "MH_STATUS_NO_CITY_LOADED", "未加载城市。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "没有城市... ¯\\_(ツ)_/¯ ...没有统计" },

                { "MH_STATUS_LINE1", "{0} 等待 | {1} 死亡/月 | 更新于 {2}" },
                { "MH_STATUS_LINE2", "{0} 最大处理量/月 | {1}/{2} 墓位已用" },
                { "MH_STATUS_LINE3", "{0} / {1} 灵车 | {2} / {3} 建筑 | {4} 最大工人" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "当前建议：火葬场处理速度约 {0}%" },
                { "MH_STATUS_PROCESSING_MORE", "当前建议：火葬场处理速度 500% + 更多运营中的设施" },
                { "MH_STATUS_PROCESSING_NONE", "建议：启用/增建火葬场" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "通过“重置已满墓地”在**本次游戏中自动清空的墓地**。\n" +
                    "显示重置总次数及不同墓地的数量。\n" +
                    "重新启动或切换城市时清除。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "已清空的墓地，以及各自的清空次数（名称 × 次数）。" },

                { "MH_STATUS_LINE4", "重置：{0} · 墓地：{1}" },
                { "MH_STATUS_CEMETERY_NONE", "本次游戏中无" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "另外 {0} 个" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "模组" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "此模组的显示名称。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "当前版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "打开作者的 Paradox Mods 页面。" },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "日志报告" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "将详细的殡葬服务报告和可能的问题写入 MagicHearse.log。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "打开日志" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "如果存在，则打开 **Logs/MagicHearse.log**。\n" +
                    "如果尚未找到文件，则改为打开 Logs 文件夹。" },
            };
        }
        public void Unload()
        { }
    }
}
