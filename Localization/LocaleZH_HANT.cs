// File: Localization/LocaleZH_HANT.cs
// Traditional Chinese zh-HANT locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Traditional Chinese localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleZH_HANT : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Traditional Chinese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleZH_HANT(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Traditional Chinese localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "關於" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "自動清理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "手動管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "進階" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "狀態" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "模組資訊" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "連結" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "啟用魔法清理" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**自動移除正在等待靈車的死亡市民**。\n" +
                    "把兩個勾選框都關掉即可停用模組，而不需要移除它。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "葬禮管理員" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "全部自行管理。\n" +
                    "**縮放數值：** 速度、車隊、儲存。\n" +
                    "可選：也可**增加工人**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "處理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**設施處理速度**（火化）\n" +
                    "**100%** = 原版預設值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "車隊數量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "每個設施的**靈車最大數量**。\n" +
                    "**100%** = 原版預設值。\n" +
                    "**[o_o]** 靈車太多可能會依死亡率影響交通。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "主建築的**墓地儲存容量**。\n" +
                    "**100%** = 原版預設值。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "靈車速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**提高靈車最高速度**。\n" +
                    "**100%** = 原版預設值。\n" +
                    "<道路限速仍然適用>。\n\n" +
                    "同時縮放加速/煞車（溫和），避免新最高速造成誇張的起步/急停。\n" +
                    "注意：即使提高了靈車最高速度，實際行駛速度基本由以下決定：\n" +
                    "(車輛上限、道路限速、AI 安全速度、交通)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "控制最大工人數" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "相容性開關：\n" +
                    "**啟用 [✓]** 以增加工人數。\n" +
                    "**[o_o]** 如果希望由 **ConfigXML** 模組控制殯葬工人，請保持 OFF。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "最大工人數" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**提高允許的最大工人數**。\n" +
                    "**100%** = 原版預設值。\n\n" +
                    "僅對**新建建築**生效。\n" +
                    "**提示**\n" +
                    "▪ 新增/移除擴建通常也會刷新。\n\n" +
                    "**[o_o]** 技術說明：工作名額是由遊戲計算的元件\n" +
                    "<==不像其他滑條那樣==>；比起在執行時由模組直接修改數值（危險），新建建築是更安全/更簡單的刷新方式。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "重置滑條" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "把所有滑條重置回 **100%**（原版預設值）。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "需要靈車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**等待靈車接走的死亡市民**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "數量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "來自遊戲統計的**每月總計**。\n" +
                     "**火化上限/月** = 遊戲的 Handling/月 資訊面板。\n" +
                     "這是火葬場每月最多可處理的遺體數量。"
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "資源" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**啟用中建築容量：** 靈車總數、建築數、最大工人。\n\n" +
                    "**備註：**\n" +
                    "▪ 靈車：啟用-未停放 /（總計* 靈車）\n" +
                    "▪ *總計 靈車:" +
                    "=== 包含維修中的靈車（例如服務預算偏低時）, \n" +
                    "=== 不包含已停用建築的靈車。\n" +
                    "▪ 狀態掃描只會在 Options 開啟時（或使用滑條時）執行；" +
                    "不會在城市中每幀執行，所以基本上沒有性能影響 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "狀態未載入。" },
                { "MH_STATUS_NO_CITY_LOADED", "未載入城市。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "沒有城市... ¯\\_(ツ)_/¯ ...沒有統計" },

                { "MH_STATUS_LINE1", "{0} 死亡等待 | 更新於 {1}" },
                { "MH_STATUS_LINE2", "{0} 死亡/月 | {1} 火化上限/月 | {2} / {3} 墓地使用" },
                { "MH_STATUS_LINE3", "{0} / {1} 靈車 | {2} / {3} 建築 | {4} 空墓位 | {5} 最大工人" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "模組" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "此模組的顯示名稱。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "目前版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "開啟作者的 Paradox Mods 頁面。" },
            };
        }

        public void Unload()
        { }
    }
}
