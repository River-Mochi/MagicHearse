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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "自動クリーン" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "自己管理" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "詳細設定" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "ステータス" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Mod情報" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "リンク" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "デバッグ" },

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
                    "**満杯の墓地を空にし**、「満杯」アイコンで機能停止しないようにします。\n" +
                    "魔法クリーンは埋葬前にほとんどの遺体を削除しますが、この設定は**すでに満杯**の墓地も空にします。\n" +
                    "<[ ] 既定でOFF>。\n" +
                    "魔法クリーンモードですでに満杯の墓地も空にしたい場合にのみ有効にしてください。\n" +
                    "いったん空にすれば、魔法クリーンを有効にしたままである限り、通常はこの設定を有効にしておく必要はありません。"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "葬儀ディレクター" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "通常のゲームの葬祭システムを自分で管理・最適化します。\n" +
                    "**スケール値：** 処理速度、車両数、保管量。\n" +
                    "任意：**労働者数も増やします**。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "火葬場の処理速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**火葬場の処理速度。**\n" +
                    "値を上げると遺体をより早く火葬し、施設の保管容量を早く空けます。\n" +
                    "**100%** = バニラのゲーム既定値。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "霊柩車の総数" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "施設ごとの**霊柩車の最大数**。\n" +
                    "**100%** = バニラのゲーム既定値。\n" +
                    "**[注意]** 霊柩車が多すぎると、死亡率によっては交通に影響する場合があります。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "霊柩車の速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**霊柩車に許可される最高走行速度を上げます**。\n" +
                    "**100%** = バニラのゲーム既定値。\n" +
                    "<道路の制限速度は引き続き適用されます>。\n" +
                    "\n" +
                    "加速/減速（穏やか）もスケールし、新しい最高速度で極端な発進/停止が起きないようにします。\n" +
                    "注：霊柩車の最高速度を上げても、実際の走行速度は次の影響を受けます：\n" +
                    "車両に許可された最高速度、道路の制限速度、ゲームAIの安全速度（カーブ、道路損傷）、交通状況。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "死亡通知の遅延（分）" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "霊柩車が建物に到着するまでに使える合計時間です。この時間を過ぎると、**霊柩車待ち**の問題アイコンが表示されます。\n" +
                    "**3分** は、ゲーム既定の約2.5シミュレーション分に近い値です。\n" +
                    "この値を増やすと、死亡アイコンが表示される前に霊柩車が移動を完了するための、より現実的な時間を確保できます。\n" +
                    "注：\n" +
                    "- <推奨：10分>。交通渋滞がひどい都市ではさらに高くしてください。\n" +
                    "- 下部のステータスレポートで期限超過の件数を確認できます。\n" +
                    "- この値を初めて増やしたとき、すでに表示中のアイコンは非表示になりません。霊柩車が処理するか建物を解体するまで残ります。\n" +
                    "- 現在の出動を自然に完了させるか、<魔法クリーン [x]> を一度だけ使って、新しい時間設定で素早くリスタートしてください。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "墓地の収容" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "主建物の**墓地収容容量**。\n" +
                    "容量を増やすと、満杯の墓地が再び遺体の回収を受け入れられます。\n" +
                    "空き不足が施設を止めていた場合を除き、霊柩車の出動数は増えません。\n" +
                    "**100%** = バニラのゲーム既定値。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "墓地を自動リセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**満杯の墓地を空にし**、建物上の「満杯」アイコンで機能停止しないようにします。\n" +
                    "満杯の墓地を削除して建て直す必要はもうありません。\n" +
                    "これをOFFにすると、代わりに緩やかな**墓地区画の再利用速度**を使用できます。\n" +
                    "<[ ✓ ] 既定でON>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "墓地区画の再利用速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**使用中の墓地区画を徐々に再利用可能にします。**\n" +
                    "値を上げると、バニラより早く墓地区画が再び空きます。\n" +
                    "500%でも墓地が頻繁に満杯になる場合は、\n" +
                    "代わりに **[墓地を自動リセット]** を有効にしてください。\n" +
                    "**100%** = ゲーム既定の墓区画再利用速度。"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "労働者数を調整" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "互換性トグル：\n" +
                    "**[✓] をON** にすると労働者数を増やします。\n" +
                    "**[o_o]** **ConfigXML** など別のModに葬祭施設の労働者数を任せたい場合はOFFのままにしてください。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "最大労働者数" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "許可される**最大労働者数を増やします**。\n" +
                    "**100%** = バニラのゲーム既定値。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "スライダーをリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "割合スライダーを **100%** にし、死亡通知の遅延を **3分** に設定します。" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "霊柩車が必要" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**待機中** = まだ屋外にいて回収を待っている死亡市民すべて。\n" +
                    "**期限超過** = 選択した通知遅延時間を過ぎた待機中の市民。\n" +
                    " - 期限超過が多い場合は、「死亡通知の遅延」の時間を増やしてください。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "処理量" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "ゲーム統計の**月間合計**。\n" +
                    "**最大/月** = 現在の効率での火葬場処理と墓地区画再利用の合計。\n" +
                    "稼働中のすべての葬祭施設が1か月に処理できる最大遺体数です。"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "資産" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**稼働中建物の容量：** 霊柩車合計、建物数、最大労働者数。\n" +
                    "\n" +
                    "**注記：**\n" +
                    "▪ 霊柩車：稼働中（駐車中ではない） /（合計* 霊柩車）\n" +
                    "▪ *霊柩車の合計：\n" +
                    "== メンテナンス中の霊柩車も含みます（例：サービス予算が低い場合）、 \n" +
                    "== 無効化された建物の霊柩車は含みません。\n" +
                    "▪ ステータススキャンはオプションが開いている間（またはスライダー操作時）だけ動作します。都市内で毎フレーム動くわけではないため、基本的にパフォーマンスへの影響はありません :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "ステータス未読み込み。" },
                { "MH_STATUS_NO_CITY_LOADED", "都市が読み込まれていません。" },
                { "MH_STATUS_STATS_NOT_AVAIL", "都市なし... ¯\\_(ツ)_/¯ ...統計なし" },

                { "MH_STATUS_LINE1_V2", "{0} 待機中 | {1} 期限超過 | {2} 死亡/月" },
                { "MH_STATUS_LINE2_V2", "{0} 最大/月" },
                { "MH_STATUS_LINE3", "{0} / {1} 霊柩車 | {2} / {3} 建物 | {4} 最大労働者" },
                { "MH_STATUS_UPDATED", "更新 {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "現在の推奨：火葬場処理 約{0}%" },
                { "MH_STATUS_PROCESSING_MORE", "現在の推奨：火葬場処理 500% + 稼働施設を追加" },
                { "MH_STATUS_PROCESSING_NONE", "推奨：火葬場を稼働/追加" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "墓地" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**使用中の墓数**、稼働中の墓地施設、このセッション中の満杯墓地リセット数を表示します。\n" +
                    "ステータスは再起動または都市切り替えで消去されます。"
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} 墓使用 | {2} 施設 | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} 回リセット" },
                { "MH_STATUS_RESET_PLURAL", "{0} 回リセット" },
                { "MH_STATUS_CEMETERY_NONE", "このセッションではなし" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} 件" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "このModの表示名。" },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "バージョン" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "現在のバージョン。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "作者のParadox Modsページを開きます。" },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "ログレポート" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "詳細な葬祭レポートと考えられる問題箇所をMagicHearse.logに書き込みます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "ログを開く" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "**Logs/MagicHearse.log** が存在する場合は開きます。\n" +
                    "まだファイルがない場合は、代わりにLogsフォルダーを開きます。"
                },
            };
        }

        public void Unload()
        { }
    }
}
