// File: Localization/LocaleZH_HANT.cs
// Chinese (Traditional) zh-HANT locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Chinese (Traditional) localization source for Magic Hearse.</summary>
    public sealed class LocaleZH_HANT : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Chinese (Traditional) locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleZH_HANT(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Chinese (Traditional) localization entries for this mod.</summary>
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
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "自動清理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "自行管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "狀態" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod 資訊" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "連結" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "啟用魔法" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**自動移除死亡市民**（正在等靈車）。\n" +
                    "兩個都關 = 不移除也能停用本 Mod。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "殯葬主管" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "縮放**設施**數值（處理、車隊、儲存）。\n" +
                    "可選：**增加員工**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "處理速率" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**設施處理速度**（火化）\n" +
                    "**100%** = 原版預設。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "車隊規模" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "每個設施的**靈車上限**。\n" +
                    "**100%** = 原版預設。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地儲存" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "墓地主建物的**儲存容量**。\n" +
                    "**100%** = 原版預設。"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "控制最大員工" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "開啟後讓**殯葬主管**也增加員工數量。\n" +
                    "若想讓 **ConfigXML**（或其他 Mod）管理員工，請保持 OFF。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "最大員工" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "縮放死亡相關設施的**最大員工數**。\n" +
                    "**100%** = 原版預設。\n\n" +
                    "**[o_o] 小提示**\n" +
                    "  - 只對<新建築>生效。\n" +
                    "  - 加/刪擴展通常會強制刷新。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "重置滑桿" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "把所有滑桿重置為 **100%**（原版預設）。" },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "需要靈車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**等待靈車**的死亡市民。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "流量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "來自遊戲統計的**每月總計**。\n" +
                     "**火葬 最大/月** = 遊戲資訊面板的 Handling/月。\n" +
                     "這是所有火葬場每月理論上最多可處理的遺體數量。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "資產" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**運作中的建築**容量（靈車、建築數量、最大工人）。\n\n" +
                    "**說明：**\n" +
                    "  - 包含仍在維護中的靈車（可能因為預算太低）。\n" +
                    "  - 不包含已停用建築的靈車。\n" +
                    "  - 狀態掃描只會在選項選單或你調整滑桿時執行；不會在城市裡逐幀跑，所以幾乎沒有效能影響 :)"
                },


                // Status text templates
                { "MH_STATUS_NOT_LOADED", "狀態未載入。" },
                { "MH_STATUS_NO_CITY_LOADED", "尚未載入城市。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "沒有城市... ¯\\_(ツ)_/¯ ...沒有統計" },


                { "MH_STATUS_LINE1", "{0} 死亡等待 | 更新 {1}" },
                { "MH_STATUS_LINE2", "{0} 死亡/月 | {1} 火葬 最大/月 | {2} / {3} 墓地占用" },
                { "MH_STATUS_LINE3", "{0} 靈車 | {1} / {2} 建築 | {3} 空墓位 | {4} 最大員工" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "此 Mod 的顯示名稱。" },
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
