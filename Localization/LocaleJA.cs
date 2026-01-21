// File: Localization/LocaleJA.cs
// Purpose: Japanese ja-JP locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocaleJA : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleJA(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "情報" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "自動クリーン" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "手動管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod 情報" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "リンク" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "マジックを有効化" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "霊柩車を待っている死亡市民を自動的に削除します。\n" +
                    "Mod を無効化するには両方のチェックを外してください。"
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "葬儀ディレクター" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "葬儀施設の値を調整します（速度・台数・保管量）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "処理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "施設の**処理速度**倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "車両数" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "施設ごとの**最大霊柩車数**倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地の保管容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**墓地の最大保管容量**を増やします。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "スライダーをリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "すべてのスライダーを **100%**（デフォルト）に戻します。" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "バージョン" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "作者の Paradox Mods ページを開きます。" },
            };
        }

        public void Unload()
        { }
    }
}
