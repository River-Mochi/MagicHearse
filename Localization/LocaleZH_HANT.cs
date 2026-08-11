// <copyright file="LocaleZH_HANT.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleZH_HANT.cs
// Traditional Chinese zh-HANT locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Traditional Chinese localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleZH_HANT : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Traditional Chinese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleZH_HANT(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Traditional Chinese localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "關於" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "自動清理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "自行管理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "進階" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "狀態" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "模組資訊" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "連結" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "偵錯" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "啟用魔法清理" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "自動移除需要靈車運送的遺體。\n" +
                    "魔法清理與自行管理互斥，請二選一。\n" +
                    "取消所有核取方塊即可停用模組，無需移除。\n" +
                    "技術說明：必須符合 IsDead = true 且 WaitingForHearse = true。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "重設已滿墓園" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**清空已滿墓園**，使其不會被「已滿」圖示阻擋。\n" +
                    "魔法清理會在下葬前移除大多數遺體——此選項仍會清空任何**已經滿了**的墓園。\n" +
                    "<[ ] 預設關閉>。\n" +
                    "只有在你希望魔法清理模式也清空已經滿了的墓園時才啟用此選項。\n" +
                    "清空後，只要魔法清理保持啟用，通常不需要繼續開啟此選項。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "殯葬主管" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "自行管理並最佳化遊戲正常的殯葬系統。\n" +
                    "**縮放數值：** 處理率、車隊、儲存量。\n" +
                    "選用：**同時增加員工數量**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "火葬場處理" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**火葬場處理速度。**\n" +
                    "數值越高，遺體火化越快，也能更早釋放設施儲存空間。\n" +
                    "**100%** = 遊戲原版預設值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "靈車總數" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "每個設施的**最大靈車數量**。\n" +
                    "**100%** = 遊戲原版預設值。\n" +
                    "**[注意]** 靈車過多可能會依死亡率影響交通。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "靈車速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**提高靈車允許的最高行駛速度**。\n" +
                    "**100%** = 遊戲原版預設值。\n" +
                    "<道路限速仍然生效>。\n" +
                    "\n" +
                    "同時溫和調整加速/煞車，避免新的最高速度造成過激的起步或停車行為。\n" +
                    "注意：即使提高靈車最高速度，實際行駛速度仍會受到以下因素影響：\n" +
                    "車輛允許的最高速度、道路限速、遊戲 AI 的安全速度（彎道、道路損壞）以及交通狀況。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "死亡通知延遲（分鐘）" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "這是靈車抵達建築前可用的總時間；逾時後會出現**等待靈車**問題圖示。\n" +
                    "**3 分鐘**接近遊戲預設的約 2.5 個模擬分鐘。\n" +
                    "可以提高此數值，讓靈車有更合理的時間完成行程，再顯示死亡圖示。\n" +
                    "注意：\n" +
                    "- <建議：10 分鐘>。嚴重壅塞的城市可嘗試更高值。\n" +
                    "- 查看底部的狀態報告，了解有多少案例已經逾時。\n" +
                    "- 第一次提高此數值時，已經顯示的圖示不會被隱藏；它們會一直保留，直到靈車處理完畢或建築被拆除。\n" +
                    "- 可以讓目前派車自然完成，或一次性使用 <魔法清理 [x]> 核取方塊，以新時間安排快速重新開始。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "墓園儲存" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "主建築的**墓園儲存容量**。\n" +
                    "更大容量可以讓已滿墓園重新接受遺體接收。\n" +
                    "除非空間不足正在阻擋設施，否則不會因此派出更多靈車。\n" +
                    "**100%** = 遊戲原版預設值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "自動重設墓園" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**清空已滿墓園**，使其不會被建築上方的「已滿」圖示阻擋。\n" +
                    "之後不必再刪除並重建已滿墓園。\n" +
                    "關閉此選項可改用逐步進行的**墓園周轉速度**。\n" +
                    "<[ ✓ ] 預設開啟>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "墓園周轉速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**逐步釋放已占用的墓位。**\n" +
                    "數值越高，墓位重新可用的速度就比原版更快。\n" +
                    "如果設為 500% 後墓園仍經常滿，\n" +
                    "請改為啟用 **[自動重設墓園]**。\n" +
                    "**100%** = 遊戲預設的墓位循環再利用速度。"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "調整員工" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "相容性開關：\n" +
                    "**啟用 [✓]** 可增加員工數量。\n" +
                    "**[o_o]** 如果希望由 **ConfigXML** 或其他模組控制殯葬員工，請保持關閉。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "最大員工數" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**提高允許的最大員工數量**。\n" +
                    "**100%** = 遊戲原版預設值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "重設滑桿" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "將百分比滑桿設為 **100%**，並將死亡通知延遲設為 **3 分鐘**。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "需要靈車" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**等待中** = 所有仍在室外等待接走的死亡市民。\n" +
                    "**已逾時** = 所選通知延遲已經到期的等待市民。\n" +
                    " - 如果逾時數量很多，請考慮提高「死亡通知延遲」的時間。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "處理量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "來自遊戲統計的**每月總量**。\n" +
                    "**最大/月** = 目前效率下的火葬場處理量 + 墓園周轉量。\n" +
                    "這是所有正在運作的殯葬設施每月最多能夠處理的遺體數量。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "資產" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**正在運作建築的容量：** 靈車總數、建築數、最大員工數。\n" +
                    "\n" +
                    "**說明：**\n" +
                    "▪ 靈車：正在使用-未停放 /（靈車總數*）\n" +
                    "▪ *靈車總數：\n" +
                    "== 包括維護中的靈車（例如服務預算較低）， \n" +
                    "== 不包括已停用建築的靈車。\n" +
                    "▪ 狀態掃描只會在「選項」開啟時（或使用滑桿時）執行；不會在城市中逐幀執行，因此基本沒有效能影響 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "狀態尚未載入。" },
                { "MH_STATUS_NO_CITY_LOADED", "未載入城市。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "沒有城市... ¯\\_(ツ)_/¯ ...沒有統計" },

                { "MH_STATUS_LINE1_V2", "{0} 等待中 | {1} 已逾時 | {2} 死亡/月" },
                { "MH_STATUS_LINE2_V2", "{0} 最大/月" },
                { "MH_STATUS_LINE3", "{0} / {1} 靈車 | {2} / {3} 建築 | {4} 最大員工" },
                { "MH_STATUS_UPDATED", "更新於 {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "目前建議：火葬場處理約 {0}%" },
                { "MH_STATUS_PROCESSING_MORE", "目前建議：火葬場處理 500% + 增加正在運作的設施" },
                { "MH_STATUS_PROCESSING_NONE", "建議：啟用/增加火葬場" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "墓園" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "顯示**已使用墓位**、正在運作的墓園設施，以及本次工作階段中已滿墓園的重設次數。\n" +
                    "重新啟動或切換城市時會清除狀態。"
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} 墓位已用 | {2} 設施 | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "重設 {0} 次" },
                { "MH_STATUS_RESET_PLURAL", "重設 {0} 次" },
                { "MH_STATUS_CEMETERY_NONE", "本次工作階段無" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} 個" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "模組" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "此模組的顯示名稱。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "目前版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "開啟作者的 Paradox Mods 頁面。" },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "日誌報告" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "將詳細的殯葬報告及可能的問題區域寫入 MagicHearse.log。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "開啟日誌" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "如果存在，則開啟 **Logs/MagicHearse.log**。\n" +
                    "如果尚未找到該檔案，則改為開啟 Logs 資料夾。"
                },
            };
        }

        public void Unload()
        { }
    }
}
