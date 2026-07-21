// <copyright file="LocaleKO.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleKO.cs
// Korean ko-KR locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Korean localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleKO : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Korean locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleKO(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Korean localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "작업" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "정보" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "자동 정리" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "수동 관리" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "고급" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "상태" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "모드 정보" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "링크" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "마법 정리 활성화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "영구차를 기다리는 **사망 시민을 자동으로 제거**합니다.\n" +
                    "두 체크박스를 모두 OFF로 하면, 모드를 제거하지 않고 비활성화할 수 있습니다."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "장의사" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "모든 것을 수동으로 관리합니다.\n" +
                    "**스케일 값:** 처리, 차량, 저장.\n" +
                    "선택: **근로자 수**도 증가."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "처리 속도" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**시설 처리 속도** (화장)\n" +
                    "**100%** = 바닐라 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "차량 수" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "시설당 **영구차 최대 수**.\n" +
                    "**100%** = 바닐라 기본값.\n" +
                    "**[o_o]** 영구차가 너무 많으면 사망률에 따라 교통에 영향을 줄 수 있습니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "묘지 저장" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "메인 건물의 **묘지 저장 용량**.\n" +
                    "**100%** = 바닐라 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AutoResetCemetery)), "가득 차면 자동 비움" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AutoResetCemetery)),
                    "**묘지가 가득 차는 즉시 자동으로 비웁니다**.\n" +
                    "사용 중인 구획이 0으로 초기화됩니다 — 철거 후 재건축과 같지만 즉시 자동으로 처리됩니다.\n" +
                    "**묘지 저장** 슬라이더와 함께 사용: 묘지 용량을 정한 뒤 재활용되도록 두면 가득 찬 묘지를 철거할 필요가 없습니다.\n" +
                    "**장의사**가 켜져 있는 동안 기본값은 켜짐입니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "영구차 속도" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**영구차 최고 속도를 증가**합니다.\n" +
                    "**100%** = 바닐라 기본값.\n" +
                    "<도로 제한 속도는 그대로 적용>。\n\n" +
                    "가속/제동(부드럽게)도 함께 스케일되어, 최고속 증가가 과격한 출발/정지로 이어지지 않게 합니다.\n" +
                    "참고: 최고 속도를 올려도 실제 주행 속도는 대략 다음에 의해 결정됩니다:\n" +
                    "(차량 최대, 도로 제한, AI 안전 속도, 교통)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "최대 근로자 제어" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "호환성 토글:\n" +
                    "**[✓] ON** 하면 근로자 수를 증가.\n" +
                    "**[o_o]** **ConfigXML** 또는 다른 모드가 장례 서비스 근로자 수를 제어하게 하려면 OFF로 두기."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "최대 근로자" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**최대 근로자 수** 를 증가.\n" +
                    "**100%** = 바닐라 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "슬라이더 리셋" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "모든 슬라이더를 **100%**(바닐라 기본값)로 되돌립니다." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "영구차 필요" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "영구차 픽업을 기다리는 **사망 시민**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "물량" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "게임 통계의 **월간 합계**.\n" +
                     "**화장 최대/월** = 게임 Handling/월 정보 패널.\n" +
                     "이는 화장장이 한 달에 처리할 수 있는 최대 시신 수입니다."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "자산" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**활성 건물 용량:** 영구차 합계, 건물 수, 최대 근로자.\n\n" +
                    "**참고:**\n" +
                    "▪ 영구차: 활성(주차 아님) / (총합* 영구차)\n" +
                    "▪ *총합 영구차:" +
                    "=== 정비 중 영구차 포함(예: 서비스 예산 낮음), \n" +
                    "=== 비활성화된 건물의 영구차는 포함하지 않음.\n" +
                    "▪ 상태 스캔은 Options가 열려 있을 때(또는 슬라이더를 사용할 때)만 실행됩니다; " +
                    "도시에서 매 프레임 실행되지 않으므로 성능 영향은 사실상 거의 없습니다 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "상태를 불러오지 못했습니다." },
                { "MH_STATUS_NO_CITY_LOADED", "도시가 로드되지 않았습니다." },
                { "MH_STATUS_STATS_NOT_AVAIL", "도시 없음... ¯\\_(ツ)_/¯ ...통계 없음" },

                { "MH_STATUS_LINE1", "{0} 대기 | {1} 사망/월 | 업데이트 {2}" },
                { "MH_STATUS_LINE2", "{0} 화장 최대/월 | {1}/{2} 무덤 사용" },
                { "MH_STATUS_LINE3", "{0} / {1} 영구차 | {2} / {3} 건물 | {4} 최대 근로자" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary4)), "Cemetery" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary4)),
                    "**Cemeteries auto-emptied this session** by Auto-empty when full.\n" +
                    "Shows total resets and how many distinct cemeteries.\n" +
                    "Clears on reboot or when you switch city."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusCemetery1)),
                    "Which cemeteries were emptied, and how many times each (name × count)." },

                { "MH_STATUS_LINE4", "resets: {0} · cemeteries: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "none this session" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} more" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "모드" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "이 모드의 표시 이름." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "버전" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "현재 버전." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "작성자의 Paradox Mods 페이지를 엽니다." },
            };
        }

        public void Unload()
        { }
    }
}
