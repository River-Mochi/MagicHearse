// <copyright file="LocaleJA.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleJA.cs
// Japanese ja-JP locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Japanese localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleJA : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Japanese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleJA(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Japanese localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "アクション" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "情報" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "自動クリーン" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "手動管理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "詳細設定" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "ステータス" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "Mod情報" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "リンク" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "デバッグ" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "魔法クリーンを有効化" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "搬送（霊柩車）が必要な遺体を自動的に削除します。\n" +
                    "魔法クリーンと自己管理は同時に使用できません。どちらか一方を選んでください。\n" +
                    "すべてのチェックをOFFにすると、Modを削除せずに無効化できます。\n" +
                    "技術メモ：IsDead = true かつ WaitingForHearse = true が必要です。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "満杯の墓地をリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**満杯の墓地をすべて空にし**、「満杯」アイコンで機能停止しないようにします。\n" +
                    "魔法クリーンは埋葬前にほとんどの遺体を削除しますが、この設定は**すでに満杯**の墓地も空にします。\n" +
                    "<[ ] 既定でOFF>。\n" +
                    "魔法クリーンモードですでに満杯の墓地も空にしたい場合にのみ有効にしてください。\n" +
                    "いったん空にすれば、魔法クリーンを有効にしたままである限り、通常はこの設定を有効にしておく必要はありません。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "葬儀ディレクター" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "すべてを手動で管理します。\n" +
                    "**スケール値：** 速度、車両数、保管。\n" +
                    "任意：**労働者数**も増やします。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "火葬場の処理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**火葬場の処理速度。**\n" +
                    "値を上げると遺体をより早く火葬し、施設の保管容量を早く空けます。\n" +
                    "**100%** = バニラ既定。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "車両台数" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "施設ごとの**霊柩車の最大数**。\n" +
                    "**100%** = バニラ既定。\n" +
                    "**[注意]** 霊柩車が多すぎると、死亡率次第で交通に影響する場合があります。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "墓地の収容" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "主建物の**墓地収容容量**。\n" +
                    "容量を増やすと、満杯の墓地が再び遺体の回収を受け入れられます。\n" +
                    "空き不足が施設を止めていた場合を除き、霊柩車の出動数は直接増えません。\n" +
                    "**100%** = バニラ既定。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "満杯の墓地をリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "墓地が満杯になると**墓地を空にし**、建物上の「満杯」アイコンで機能停止しないようにします。\n" +
                    "満杯の墓地を削除して建て直す必要はもうありません。\n" +
                    "これをOFFにすると、代わりに緩やかな**墓地区画の再利用速度**を使用できます。\n" +
                    "<[ ✓ ] 既定でON>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "墓地区画の再利用速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**使用中の墓地区画を徐々に再利用可能にします。**\n" +
                    "墓地に「満杯」アイコンが頻繁に表示される場合は、このスライダーを上げてください。\n" +
                    "値を上げると、バニラより早く墓地区画が再び空きます。\n" +
                    "**100%** = バニラ既定。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "霊柩車の速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**霊柩車の最高速度を上げます**。\n" +
                    "**100%** = バニラ既定。\n" +
                    "<道路の制限速度は適用されます>。\n\n" +
                    "加速/減速（穏やか）もスケールして、新しい最高速度でも極端な発進/停止にならないようにします。\n" +
                    "注：霊柩車の最高速度を上げても、実際の走行速度は次の影響を受けます：\n" +
                    "車両に許可された最高速度、道路の制限速度、ゲームAIの安全速度（カーブ、道路損傷）、交通状況。"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "最大労働者を制御" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "互換性トグル:\n" +
                    "**[✓] をON** にすると従業員数を増やす。\n" +
                    "**[o_o]** **ConfigXML** など別のMODに従業員数を任せたい場合は OFF のまま。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "最大労働者" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**最大従業員数** を増やす。\n" +
                    "**100%** = バニラ既定値。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "スライダーをリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "すべてのスライダーを **100%**（バニラ既定）に戻します。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "霊柩車が必要" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "霊柩車の回収待ちの**死亡市民**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "ゲーム統計の**月間合計**。\n" +
                     "**最大処理数/月** = 現在の効率での火葬場処理と墓地区画の再利用の合計。\n" +
                     "稼働中のすべての葬祭施設が1か月に処理できる最大数です。"
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "資産" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**稼働中建物の容量：** 霊柩車合計、建物数、最大労働者。\n\n" +
                    "**注記：**\n" +
                    "▪ 霊柩車：稼働中（駐車中ではない） /（合計* 霊柩車）\n" +
                    "▪ *霊柩車の合計：\n" +
                    "== メンテナンス中の霊柩車も含みます（例：サービス予算が低い場合）、\n" +
                    "== 無効化された建物の霊柩車は含みません。\n" +
                    "▪ ステータススキャンはオプションが開いている間（またはスライダー操作時）だけ動作します。" +
                    "都市内で毎フレーム動くわけではないので、基本的にパフォーマンスへの影響はほぼありません :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "ステータス未読み込み。" },
                { "MH_STATUS_NO_CITY_LOADED", "都市が読み込まれていません。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "都市なし... ¯\\_(ツ)_/¯ ...統計なし" },

                { "MH_STATUS_LINE1", "{0} 待機 | {1} 死亡/月 | 更新 {2}" },
                { "MH_STATUS_LINE2", "{0} 最大処理数/月 | {1}/{2} 墓使用" },
                { "MH_STATUS_LINE3", "{0} / {1} 霊柩車 | {2} / {3} 建物 | {4} 最大労働者" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "現在の提案: 処理速度約{0}%" },
                { "MH_STATUS_PROCESSING_MORE", "現在の提案: 処理速度500% + 稼働施設を追加" },
                { "MH_STATUS_PROCESSING_NONE", "提案: 火葬場を稼働/追加" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "「満杯の墓地をリセット」により、**このセッションで自動的に空になった墓地**。\n" +
                    "リセットの合計回数と、対象となった墓地の数を表示します。\n" +
                    "再起動または都市の切り替え時に消去されます。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "空になった墓地と、それぞれが空になった回数（名前 × 回数）。" },

                { "MH_STATUS_LINE4", "リセット: {0} · 墓地: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "このセッションではなし" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "ほか{0}件" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "このModの表示名。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "バージョン" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "現在のバージョン。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "作者の Paradox Mods ページを開きます。" },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "ログレポート" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "詳細なデスケア情報と考えられる問題箇所を MagicHearse.log に書き込みます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "ログを開く" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "**Logs/MagicHearse.log** があれば開きます。\n" +
                    "ファイルがまだ見つからない場合は、代わりに Logs フォルダーを開きます。" },
            };
        }

        public void Unload()
        { }
    }
}
