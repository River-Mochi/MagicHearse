// File: Localization/LocaleJA.cs
// Japanese ja-JP locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Japanese localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleJA : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Japanese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleJA(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Japanese localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "情報" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "自動クリーン" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "自己管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "ステータス" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Mod情報" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "リンク" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "魔法を有効化" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "霊柩車を待っている**死亡市民を自動で削除**します。\n" +
                    "両方OFFで、削除せずにModを無効化できます。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "葬儀ディレクター" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "**施設**の値（処理速度・車両数・保管）を調整。\n" +
                    "任意：**労働者も増やす**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "処理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**施設の処理速度**（火葬）\n" +
                    "**100%** = バニラ標準。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "車両数" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "施設ごとの**霊柩車の最大数**。\n" +
                    "**100%** = バニラ標準。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地の保管" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "メイン建物の**墓地保管容量**。\n" +
                    "**100%** = バニラ標準。"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "最大労働者数を制御" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "**葬儀ディレクター**で労働者数も増やしたい場合にON。\n" +
                    "**ConfigXML**（または他Mod）に任せるならOFF。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "最大労働者数" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "死亡関連施設の**最大労働者数**を調整。\n" +
                    "**100%** = バニラ標準。\n\n" +
                    "**[o_o] ヒント**\n" +
                    "  - **新しい建物**に反映。\n" +
                    "  - 拡張の追加/削除で更新されることが多いです。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "スライダーをリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "すべてのスライダーを**100%**（バニラ標準）に戻します。" },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "霊柩車が必要" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "霊柩車を待つ**死亡市民**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "ボリューム" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**月間合計**（ゲーム統計）\n" +
                     "**火葬 最大/月** = ゲームの「Handling/月」情報パネル。\n" +
                     "全ての火葬場が1か月に処理できる遺体数の上限です。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "資産" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**稼働中の建物の容量:** 霊柩車の合計、建物数、最大従業員数。\n\n" +
                    "**メモ:**\n" +
                    "▪ 霊柩車: 稼働中（駐車中は除外）/ 総容量*\n" +
                    "▪ *総容量 = 稼働中の建物（効率 > 0）の霊柩車スロット合計。\n" +
                    "  駐車中/利用不可の霊柩車も含まれる場合があります。\n" +
                    "▪ ステータススキャンはオプションを開いている間（または設定変更後）のみ実行。\n" +
                    "  街中で毎フレームは動かないので負荷はほぼありません。"
                },


                // Status text templates
                { "MH_STATUS_NOT_LOADED", "ステータス未読込。" },
                { "MH_STATUS_NO_CITY_LOADED", "まだ都市が読み込まれていません。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "都市なし... ¯\\_(ツ)_/¯ ...統計なし" },

                { "MH_STATUS_LINE1", "{0} 死亡待ち | 更新 {1}" },
                { "MH_STATUS_LINE2", "{0} 死亡/月 | {1} 火葬 最大/月 | {2} / {3} 墓地使用" },
                { "MH_STATUS_LINE3", "{0} / {1} 霊柩車 | {2} / {3} 建物 | {4} 空き墓 | {5} 最大労働者" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "このModの表示名。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "バージョン" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "現在のバージョン。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "作者のParadox Modsページを開きます。" },
            };
        }

        public void Unload()
        { }
    }
}
