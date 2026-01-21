// File: Localization/LocaleZH_HANT.cs
// Purpose: Traditional Chinese zh-HANT locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocaleZH_HANT : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleZH_HANT(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "關於" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "自動清理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "自行管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "模組資訊" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "連結" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "啟用魔法" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "自動移除正在等待靈車的死亡市民。\n" +
                    "想在不移除模組的情況下停用，請取消勾選兩個核取方塊。"
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "殯葬主管" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "縮放殯葬設施數值（速度、車隊、儲存）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "處理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "設施**處理速度**倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "車隊規模" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**每座設施最大靈車數量**倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地儲存" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "提高**墓地最大儲存量**。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "重置滑桿" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "將所有滑桿重置為 **100%**（原版預設）。" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "模組" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "版本" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "開啟作者的 Paradox Mods 頁面。" },
            };
        }

        public void Unload()
        { }
    }
}
