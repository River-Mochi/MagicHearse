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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "アクション" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "情報" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "自動クリーン" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "手動管理" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "詳細設定" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "ステータス" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "Mod情報" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "リンク" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "魔法クリーンを有効化" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "霊柩車を待っている**死亡市民を自動で削除**します。\n" +
                    "両方のチェックをOFFにすると、削除せずにModを無効化できます。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "葬儀ディレクター" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "すべてを手動で管理します。\n" +
                    "**スケール値：** 速度、車両数、保管。\n" +
                    "任意：**労働者数**も増やします。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "処理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**施設の処理速度**（火葬）\n" +
                    "**100%** = バニラ既定。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "車両台数" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "施設ごとの**霊柩車の最大数**。\n" +
                    "**100%** = バニラ既定。\n" +
                    "**[o_o]** 霊柩車が多すぎると、死亡率次第で交通に影響する場合があります。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "墓地の収容" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "主建物の**墓地収容容量**。\n" +
                    "**100%** = バニラ既定。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "霊柩車の速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**霊柩車の最高速度を上げます**。\n" +
                    "**100%** = バニラ既定。\n" +
                    "<道路の制限速度は適用されます>。\n\n" +
                    "加速/減速（穏やか）もスケールして、新しい最高速度でも極端な発進/停止にならないようにします。\n" +
                    "注：最高速度を上げても、実際の走行速度はだいたい次で決まります：\n" +
                    "(車両の上限、道路の制限速度、AI安全速度、交通)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "最大労働者を制御" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "互換性トグル:\n" +
                    "**[✓] をON** にすると従業員数を増やす。\n" +
                    "**[o_o]** **ConfigXML** など別のMODに従業員数を任せたい場合は OFF のまま。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "最大労働者" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**最大従業員数** を増やす。\n" +
                    "**100%** = バニラ既定値。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "スライダーをリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "すべてのスライダーを **100%**（バニラ既定）に戻します。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "霊柩車が必要" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "霊柩車の回収待ちの**死亡市民**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "ゲーム統計の**月間合計**。\n" +
                     "**火葬上限/月** = ゲームの Handling/月 情報パネル。\n" +
                     "これは、火葬場が1か月に処理できる最大数です。"
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "資産" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**稼働中建物の容量：** 霊柩車合計、建物数、最大労働者。\n\n" +
                    "**注記：**\n" +
                    "▪ 霊柩車：稼働中（駐車中ではない） /（合計* 霊柩車）\n" +
                    "▪ *合計 霊柩車:" +
                    "=== メンテ中の霊柩車も含みます（例：サービス予算が低い場合）, \n" +
                    "=== 無効化された建物の霊柩車は含みません。\n" +
                    "▪ ステータススキャンは Options が開いている間（またはスライダー操作時）だけ動作します；" +
                    "都市内で毎フレーム動くわけではないので、基本的にパフォーマンスへの影響はほぼありません :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "ステータス未読み込み。" },
                { "MH_STATUS_NO_CITY_LOADED", "都市が読み込まれていません。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "都市なし... ¯\\_(ツ)_/¯ ...統計なし" },

                { "MH_STATUS_LINE1", "{0} 待機 | {1} 死亡/月 | 更新 {2}" },
                { "MH_STATUS_LINE2", "{0} 火葬上限/月 | {1}/{2} 墓使用" },
                { "MH_STATUS_LINE3", "{0} / {1} 霊柩車 | {2} / {3} 建物 | {4} 最大労働者" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "このModの表示名。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "バージョン" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "現在のバージョン。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "作者の Paradox Mods ページを開きます。" },
            };
        }

        public void Unload()
        { }
    }
}
