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
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Korean localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleKO : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Korean locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleKO(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Korean localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "작업" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "정보" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "자동 정리" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "수동 관리" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "고급" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "상태" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "모드 정보" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "링크" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "마법 정리 활성화" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "운송(영구차)이 필요한 시신을 자동으로 제거합니다.\n" +
                    "마법 정리와 직접 관리는 동시에 사용할 수 없습니다. 둘 중 하나를 선택하세요.\n" +
                    "모든 체크박스를 OFF로 하면 모드를 제거하지 않고 비활성화할 수 있습니다.\n" +
                    "기술 참고: IsDead = true 및 WaitingForHearse = true여야 합니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "가득 찬 묘지 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**가득 찬 모든 묘지를 비워** 가득 참 아이콘으로 운영이 중단되지 않게 합니다.\n" +
                    "마법 정리는 매장 전에 대부분의 시신을 제거하지만, 이 설정은 **이미 가득 찬** 묘지도 비웁니다.\n" +
                    "<[ ] 기본값 OFF>.\n" +
                    "마법 정리 모드에서도 이미 가득 찬 묘지를 비우려는 경우에만 이 설정을 켜세요.\n" +
                    "한번 비운 뒤에는 마법 정리를 계속 켜 두는 한, 일반적으로 이 설정을 계속 켜 둘 필요가 없습니다."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "장의사" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "모든 것을 수동으로 관리합니다.\n" +
                    "**스케일 값:** 처리, 차량, 저장.\n" +
                    "선택: **근로자 수**도 증가."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "처리 속도" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**시설 처리 속도** (화장)\n" +
                    "**100%** = 바닐라 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "차량 수" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "시설당 **영구차 최대 수**.\n" +
                    "**100%** = 바닐라 기본값.\n" +
                    "**[참고]** 영구차가 너무 많으면 사망률에 따라 교통에 영향을 줄 수 있습니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "묘지 저장" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "메인 건물의 **묘지 저장 용량**.\n" +
                    "**100%** = 바닐라 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "가득 찬 묘지 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "묘지가 가득 차면 **묘지를 비워** 건물 위의 가득 참 아이콘으로 운영이 중단되지 않게 합니다.\n" +
                    "더 이상 가득 찬 묘지를 철거하고 다시 지을 필요가 없습니다.\n" +
                    "**묘지 저장** 슬라이더와 함께 사용하세요. 묘지 크기를 정한 뒤 재사용되도록 두면 가득 찬 묘지를 다시 철거할 필요가 없습니다.\n" +
                    "<[ ✓ ] 기본값 ON>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "영구차 속도" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**영구차 최고 속도를 증가**합니다.\n" +
                    "**100%** = 바닐라 기본값.\n" +
                    "<도로 제한 속도는 그대로 적용>.\n\n" +
                    "가속/제동(부드럽게)도 함께 스케일되어, 최고속 증가가 과격한 출발/정지로 이어지지 않게 합니다.\n" +
                    "참고: 영구차의 최고 속도를 올려도 실제 주행 속도는 다음의 영향을 받습니다:\n" +
                    "차량에 허용된 최고 속도, 도로 제한 속도, 게임 AI의 안전 속도(커브, 도로 손상), 교통."

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "최대 근로자 제어" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "호환성 토글:\n" +
                    "**[✓] ON** 하면 근로자 수를 증가.\n" +
                    "**[o_o]** **ConfigXML** 또는 다른 모드가 장례 서비스 근로자 수를 제어하게 하려면 OFF로 두기."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "최대 근로자" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**최대 근로자 수** 를 증가.\n" +
                    "**100%** = 바닐라 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "슬라이더 리셋" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "모든 슬라이더를 **100%**(바닐라 기본값)로 되돌립니다." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "영구차 필요" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "영구차 픽업을 기다리는 **사망 시민**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "물량" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "게임 통계의 **월간 합계**.\n" +
                     "**화장 최대/월** = 게임 Handling/월 정보 패널.\n" +
                     "이는 화장장이 한 달에 처리할 수 있는 최대 시신 수입니다."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "자산" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**활성 건물 용량:** 영구차 합계, 건물 수, 최대 근로자.\n\n" +
                    "**참고:**\n" +
                    "▪ 영구차: 활성(주차 아님) / (총합* 영구차)\n" +
                    "▪ *영구차 총합:\n" +
                    "== 정비 중인 영구차 포함(예: 낮은 서비스 예산), \n" +
                    "== 비활성화된 건물의 영구차는 포함하지 않음.\n" +
                    "▪ 상태 스캔은 옵션이 열려 있을 때(또는 슬라이더를 사용할 때)만 실행됩니다. " +
                    "도시에서 매 프레임 실행되지 않으므로 성능 영향은 사실상 거의 없습니다 :)"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusDispatch)), "배차" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusDispatch)),
                    "수거 요청의 배정, 미배정, 외부 서비스. 외부 서비스는 배정에 포함됩니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusHearses)), "영구차" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusHearses)),
                    "출동 = 수거하러 이동 중, 운구 중 = 시신을 싣고 있음."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusFacilities)), "시설" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusFacilities)),
                    "가득 찼거나 사용 가능한 영구차가 없거나 시신 처리 대기 중인 활성 시설입니다."
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "상태를 불러오지 못했습니다." },
                { "MH_STATUS_NO_CITY_LOADED", "도시가 로드되지 않았습니다." },
                { "MH_STATUS_STATS_NOT_AVAIL", "도시 없음... ¯\\_(ツ)_/¯ ...통계 없음" },

                { "MH_STATUS_LINE1", "{0} 대기 | {1} 사망/월 | 업데이트 {2}" },
                { "MH_STATUS_LINE2", "{0} 화장 최대/월 | {1}/{2} 무덤 사용" },
                { "MH_STATUS_LINE3", "{0} / {1} 영구차 | {2} / {3} 건물 | {4} 최대 근로자" },
                { "MH_STATUS_DISPATCH", "{0} 배정 | {1} 미배정 | {2} 외부 서비스" },
                { "MH_STATUS_HEARSES", "{0} 대기 | {1} 출동 | {2} 운구 중 | {3} 복귀 | {4} 비활성" },
                { "MH_STATUS_FACILITIES", "{0} 가득 참 | {1} 사용 가능한 영구차 없음 | {2} 처리 대기" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "묘지" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "가득 찬 묘지 초기화로 **이번 세션에서 자동으로 비워진 묘지**입니다.\n" +
                    "총 초기화 횟수와 서로 다른 묘지 수를 표시합니다.\n" +
                    "재시작하거나 도시를 변경하면 지워집니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "비워진 묘지와 각 묘지가 비워진 횟수(이름 × 횟수)." },

                { "MH_STATUS_LINE4", "초기화: {0} · 묘지: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "이번 세션에는 없음" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0}개 더" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "모드" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "이 모드의 표시 이름." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "버전" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "현재 버전." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "작성자의 Paradox Mods 페이지를 엽니다." },
            };
        }

        public void Unload()
        { }
    }
}
