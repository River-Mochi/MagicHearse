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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "自動清理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "手動管理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "進階" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "狀態" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "模組資訊" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "連結" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "偵錯" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "啟用魔法清理" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "自動移除需要運送（靈車）的遺體。\n" +
                    "魔法清理與自行管理互斥，請選擇其中一項。\n" +
                    "關閉所有勾選框即可停用模組，而不需要移除它。\n" +
                    "技術說明：必須符合 IsDead = true 且 WaitingForHearse = true。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "重設已滿墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**清空所有已滿墓地**，使其不會被「已滿」圖示阻塞。\n" +
                    "魔法清理會在下葬前移除大多數遺體；此選項仍會清空任何**已經滿員**的墓地。\n" +
                    "<[ ] 預設關閉>。\n" +
                    "僅在需要魔法清理模式同時清空已經滿員的墓地時，才啟用此選項。\n" +
                    "清空後，只要魔法清理保持啟用，通常就不需要繼續啟用此選項。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "葬禮管理員" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "全部自行管理。\n" +
                    "**縮放數值：** 速度、車隊、儲存。\n" +
                    "可選：也可**增加工人**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "處理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**設施處理速度**（火化）\n" +
                    "**100%** = 原版預設值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "車隊數量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "每個設施的**靈車最大數量**。\n" +
                    "**100%** = 原版預設值。\n" +
                    "**[注意]** 靈車太多可能會依死亡率影響交通。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "墓地容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "主建築的**墓地儲存容量**。\n" +
                    "**100%** = 原版預設值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "重設已滿墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "墓地已滿時將其**清空**，避免建築上方的「已滿」圖示阻塞服務。\n" +
                    "不再需要刪除並重建已滿墓地。\n" +
                    "與**墓地容量**滑桿搭配使用：設定墓地大小後讓其循環使用，以後不必再拆除已滿墓地。\n" +
                    "<[ ✓ ] 預設開啟>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "靈車速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**提高靈車最高速度**。\n" +
                    "**100%** = 原版預設值。\n" +
                    "<道路限速仍然適用>。\n\n" +
                    "同時縮放加速/煞車（溫和），避免新最高速造成誇張的起步/急停。\n" +
                    "注意：即使提高了靈車最高速度，其實際行駛速度仍受以下因素影響：\n" +
                    "車輛允許的最高速度、道路限速、遊戲 AI 的安全速度（彎道、道路損壞）以及交通狀況。"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "控制最大工人數" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "相容性開關：\n" +
                    "**啟用 [✓]** 以增加工作人員數量。\n" +
                    "**[o_o]** 若希望由 **ConfigXML** 或其他模組控制殯葬服務的工作人員數量，請保持 OFF。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "最大工人數" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**提高允許的最大工作人員數**。\n" +
                    "**100%** = 原版預設值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "重置滑條" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "把所有滑條重置回 **100%**（原版預設值）。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "需要靈車" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**等待靈車接走的死亡市民**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "數量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "來自遊戲統計的**每月總計**。\n" +
                     "**火化上限/月** = 遊戲的 Handling/月 資訊面板。\n" +
                     "這是火葬場每月最多可處理的遺體數量。"
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "資源" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**啟用中建築容量：** 靈車總數、建築數、最大工人。\n\n" +
                    "**備註：**\n" +
                    "▪ 靈車：啟用-未停放 /（總計* 靈車）\n" +
                    "▪ *靈車總數：\n" +
                    "== 包含維修中的靈車（例如服務預算偏低時），\n" +
                    "== 不包含已停用建築的靈車。\n" +
                    "▪ 狀態掃描只會在選項選單開啟時（或使用滑條時）執行；" +
                    "不會在城市中每幀執行，所以基本上沒有性能影響 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "狀態未載入。" },
                { "MH_STATUS_NO_CITY_LOADED", "未載入城市。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "沒有城市... ¯\\_(ツ)_/¯ ...沒有統計" },

                { "MH_STATUS_LINE1", "{0} 等待 | {1} 死亡/月 | 更新於 {2}" },
                { "MH_STATUS_LINE2", "{0} 火化上限/月 | {1}/{2} 墓位已用" },
                { "MH_STATUS_LINE3", "{0} / {1} 靈車 | {2} / {3} 建築 | {4} 最大工人" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "透過「重設已滿墓地」在**本次遊戲中自動清空的墓地**。\n" +
                    "顯示重設總次數及不同墓地的數量。\n" +
                    "重新啟動或切換城市時清除。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "已清空的墓地，以及各自清空的次數（名稱 × 次數）。" },

                { "MH_STATUS_LINE4", "重設：{0} · 墓地：{1}" },
                { "MH_STATUS_CEMETERY_NONE", "本次遊戲中無" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "另外 {0} 個" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "模組" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "此模組的顯示名稱。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "目前版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "開啟作者的 Paradox Mods 頁面。" },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "日誌報告" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "將詳細的殯葬服務報告和可能的問題寫入 MagicHearse.log。" },
            };
        }

        public void Unload()
        { }
    }
}

