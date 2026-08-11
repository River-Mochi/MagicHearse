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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "自动清理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "自行管理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "高级" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "状态" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "模组信息" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "链接" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "调试" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "启用魔法清理" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "自动移除需要灵车运输的遗体。\n" +
                    "魔法清理与自行管理互斥，请二选一。\n" +
                    "取消所有复选框即可停用模组，无需卸载。\n" +
                    "技术说明：必须满足 IsDead = true 且 WaitingForHearse = true。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "重置已满墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**清空已满墓地**，使其不会被“已满”图标阻塞。\n" +
                    "魔法清理会在下葬前移除大多数遗体——此选项仍会清空任何**已经满了**的墓地。\n" +
                    "<[ ] 默认关闭>。\n" +
                    "仅当你希望魔法清理模式也清空已经满了的墓地时启用此选项。\n" +
                    "清空后，只要魔法清理保持启用，通常无需继续开启此选项。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "殡葬主管" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "自行管理并优化游戏正常的殡葬系统。\n" +
                    "**缩放数值：** 处理率、车辆数、存储量。\n" +
                    "可选：**同时增加员工数量**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "火葬场处理" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**火葬场处理速度。**\n" +
                    "数值越高，遗体火化越快，也能更早释放设施存储空间。\n" +
                    "**100%** = 游戏原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "灵车总数" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "每个设施的**最大灵车数量**。\n" +
                    "**100%** = 游戏原版默认值。\n" +
                    "**[注意]** 灵车过多可能会根据死亡率影响交通。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "灵车速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**提高灵车允许的最高行驶速度**。\n" +
                    "**100%** = 游戏原版默认值。\n" +
                    "<道路限速仍然生效>。\n" +
                    "\n" +
                    "同时温和调整加速/制动，避免新的最高速度造成过激的起步或停车行为。\n" +
                    "注意：即使提高灵车最高速度，实际行驶速度仍会受到以下因素影响：\n" +
                    "车辆允许的最高速度、道路限速、游戏 AI 的安全速度（弯道、道路损坏）以及交通状况。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "死亡通知延迟（分钟）" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "这是灵车到达建筑前可用的总时间；超时后会出现**等待灵车**问题图标。\n" +
                    "**3 分钟**接近游戏默认的约 2.5 个模拟分钟。\n" +
                    "可以提高此数值，让灵车有更合理的时间完成行程，再显示死亡图标。\n" +
                    "注意：\n" +
                    "- <建议：10 分钟>。严重拥堵的城市可尝试更高值。\n" +
                    "- 查看底部的状态报告，了解有多少案例已经超时。\n" +
                    "- 第一次提高此数值时，已经显示的图标不会被隐藏；它们会一直保留，直到灵车处理完毕或建筑被拆除。\n" +
                    "- 可以让当前派车自然完成，或一次性使用 <魔法清理 [x]> 复选框，以新时间安排快速重新开始。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "墓地存储" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "主建筑的**墓地存储容量**。\n" +
                    "更大容量可以让已满墓地重新接受遗体接收。\n" +
                    "除非空间不足正在阻塞设施，否则不会因此派出更多灵车。\n" +
                    "**100%** = 游戏原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "自动重置墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**清空已满墓地**，使其不会被建筑上方的“已满”图标阻塞。\n" +
                    "以后无需再删除并重建已满墓地。\n" +
                    "关闭此选项可改用逐渐进行的**墓地周转速度**。\n" +
                    "<[ ✓ ] 默认开启>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "墓地周转速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**逐步释放已占用的墓位。**\n" +
                    "数值越高，墓位重新可用的速度就越快于原版。\n" +
                    "如果设置为 500% 后墓地仍经常满，\n" +
                    "请改为启用 **[自动重置墓地]**。\n" +
                    "**100%** = 游戏默认的墓位循环再利用速度。"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "调整员工" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "兼容性开关：\n" +
                    "**启用 [✓]** 可增加员工数量。\n" +
                    "**[o_o]** 如果希望由 **ConfigXML** 或其他模组控制殡葬员工，请保持关闭。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "最大员工数" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**提高允许的最大员工数量**。\n" +
                    "**100%** = 游戏原版默认值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "重置滑块" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "将百分比滑块设为 **100%**，并将死亡通知延迟设为 **3 分钟**。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "需要灵车" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**等待中** = 所有仍在室外等待接走的死亡市民。\n" +
                    "**已超时** = 所选通知延迟已经到期的等待市民。\n" +
                    " - 如果超时数量很多，请考虑提高“死亡通知延迟”的时间。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "处理量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "来自游戏统计的**每月总量**。\n" +
                    "**最大/月** = 当前效率下的火葬场处理量 + 墓地周转量。\n" +
                    "这是所有正在运行的殡葬设施每月最多能够处理的遗体数量。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "资产" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**正在运行建筑的容量：** 灵车总数、建筑数、最大员工数。\n" +
                    "\n" +
                    "**说明：**\n" +
                    "▪ 灵车：正在使用-未停放 /（灵车总数*）\n" +
                    "▪ *灵车总数：\n" +
                    "== 包括维护中的灵车（例如服务预算较低）， \n" +
                    "== 不包括已停用建筑的灵车。\n" +
                    "▪ 状态扫描只会在“选项”打开时（或使用滑块时）运行；不会在城市中逐帧运行，因此基本没有性能影响 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "状态尚未加载。" },
                { "MH_STATUS_NO_CITY_LOADED", "未加载城市。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "没有城市... ¯\\_(ツ)_/¯ ...没有统计" },

                { "MH_STATUS_LINE1_V2", "{0} 等待中 | {1} 已超时 | {2} 死亡/月" },
                { "MH_STATUS_LINE2_V2", "{0} 最大/月" },
                { "MH_STATUS_LINE3", "{0} / {1} 灵车 | {2} / {3} 建筑 | {4} 最大员工" },
                { "MH_STATUS_UPDATED", "更新于 {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "当前建议：火葬场处理约 {0}%" },
                { "MH_STATUS_PROCESSING_MORE", "当前建议：火葬场处理 500% + 增加正在运行的设施" },
                { "MH_STATUS_PROCESSING_NONE", "建议：启用/增加火葬场" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "显示**已使用墓位**、正在运行的墓地设施，以及本次会话中已满墓地的重置次数。\n" +
                    "重新启动或切换城市时会清除状态。"
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} 墓位已用 | {2} 设施 | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "重置 {0} 次" },
                { "MH_STATUS_RESET_PLURAL", "重置 {0} 次" },
                { "MH_STATUS_CEMETERY_NONE", "本次会话无" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} 个" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "模组" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "此模组的显示名称。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "当前版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "打开作者的 Paradox Mods 页面。" },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "日志报告" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "将详细的殡葬报告及可能的问题区域写入 MagicHearse.log。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "打开日志" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "如果存在，则打开 **Logs/MagicHearse.log**。\n" +
                    "如果尚未找到该文件，则改为打开 Logs 文件夹。"
                },
            };
        }

        public void Unload()
        { }
    }
}
