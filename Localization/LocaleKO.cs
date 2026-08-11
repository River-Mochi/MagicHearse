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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "자동 정리" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "직접 관리" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "고급" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "상태" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "모드 정보" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "링크" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "디버그" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "매직 클린 활성화" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "운구차 운송이 필요한 사망자를 자동으로 제거합니다.\n" +
                    "매직 클린과 직접 관리는 동시에 사용할 수 없습니다. 둘 중 하나를 선택하세요.\n" +
                    "모드를 삭제하지 않고 비활성화하려면 모든 체크박스를 끄세요.\n" +
                    "기술 참고: IsDead = true 및 WaitingForHearse = true여야 합니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "가득 찬 묘지 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**가득 찬 묘지를 비워** 가득 참 아이콘 때문에 막히지 않게 합니다.\n" +
                    "매직 클린은 매장 전에 대부분의 시신을 제거하지만, 이 옵션은 **이미 가득 찬** 묘지도 비웁니다.\n" +
                    "<[ ] 기본값 꺼짐>.\n" +
                    "매직 클린 모드가 이미 가득 찬 묘지도 비우게 하려는 경우에만 활성화하세요.\n" +
                    "한 번 비운 뒤에는 매직 클린을 계속 켜 두는 한 보통 이 옵션을 계속 활성화할 필요가 없습니다."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "장례 관리자" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "게임의 일반 장례 시스템을 직접 관리하고 최적화합니다.\n" +
                    "**배율 값:** 처리율, 차량 수, 저장 공간.\n" +
                    "선택 사항: **작업자 수도 증가**합니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "화장장 처리" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**화장장 처리 속도입니다.**\n" +
                    "값이 높을수록 시신을 더 빨리 화장하고 시설 저장 공간을 더 빨리 확보합니다.\n" +
                    "**100%** = 게임 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "총 운구차" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "시설당 **최대 운구차 수**입니다.\n" +
                    "**100%** = 게임 기본값.\n" +
                    "**[참고]** 운구차가 너무 많으면 사망률에 따라 교통에 영향을 줄 수 있습니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "운구차 속도" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**운구차의 허용 최대 주행 속도를 높입니다**.\n" +
                    "**100%** = 게임 기본값.\n" +
                    "<도로 제한 속도는 계속 적용됩니다>.\n" +
                    "\n" +
                    "새 최고 속도에서 극단적인 출발/정지가 생기지 않도록 가속/제동도 부드럽게 조정합니다.\n" +
                    "참고: 운구차의 최고 속도를 높여도 실제 주행 속도는 다음의 영향을 받습니다:\n" +
                    "차량 허용 최고 속도, 도로 제한 속도, 게임 AI의 안전 속도(커브, 도로 손상), 교통 상황."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "사망 알림 지연 (분)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "운구차가 건물에 도착하기까지 허용되는 총 시간입니다. 이 시간이 지나면 **운구차 대기** 문제 아이콘이 나타납니다.\n" +
                    "**3분**은 게임 기본값인 약 2.5 시뮬레이션 분과 비슷합니다.\n" +
                    "값을 늘리면 사망 아이콘이 나타나기 전에 운구차가 이동을 완료할 합리적인 시간을 더 확보할 수 있습니다.\n" +
                    "참고:\n" +
                    "- <권장: 10분>. 교통이 심하게 막히는 도시는 더 높여 보세요.\n" +
                    "- 아래 상태 보고서에서 기한 초과 건수를 확인하세요.\n" +
                    "- 이 값을 처음 늘려도 이미 표시된 아이콘은 숨겨지지 않으며, 운구차가 처리하거나 건물을 철거할 때까지 남아 있습니다.\n" +
                    "- 현재 출동을 자연스럽게 끝내거나 <매직 클린 [x]> 체크박스를 한 번 사용해 새 시간 설정으로 빠르게 다시 시작하세요."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "묘지 저장 공간" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "본관의 **묘지 저장 용량**입니다.\n" +
                    "용량을 늘리면 가득 찬 묘지가 다시 수거를 받을 수 있습니다.\n" +
                    "공간 부족 때문에 시설이 막혀 있던 경우가 아니면 더 많은 운구차를 보내지는 않습니다.\n" +
                    "**100%** = 게임 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "묘지 자동 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**가득 찬 묘지를 비워** 건물 위의 가득 참 아이콘 때문에 막히지 않게 합니다.\n" +
                    "이제 가득 찬 묘지를 철거하고 다시 지을 필요가 없습니다.\n" +
                    "이 옵션을 끄면 대신 점진적인 **묘지 순환율**을 사용합니다.\n" +
                    "<[ ✓ ] 기본값 켜짐>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "묘지 순환율" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**사용 중인 묘지의 무덤을 점진적으로 비웁니다.**\n" +
                    "값이 높을수록 기본 게임보다 무덤 공간을 더 빨리 다시 사용할 수 있습니다.\n" +
                    "500%에서도 묘지가 너무 자주 가득 찬다면,\n" +
                    "대신 **[묘지 자동 초기화]**를 활성화하세요.\n" +
                    "**100%** = 게임의 기본 무덤 재사용 속도입니다."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "작업자 조정" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "호환성 토글:\n" +
                    "**활성화 [✓]** 하면 작업자 수를 늘립니다.\n" +
                    "**[o_o]** **ConfigXML** 또는 다른 모드가 장례 시설 작업자 수를 제어하게 하려면 꺼짐으로 두세요."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "최대 작업자" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "허용되는 **최대 작업자 수를 늘립니다**.\n" +
                    "**100%** = 게임 기본값."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "슬라이더 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "백분율 슬라이더를 **100%**로, 사망 알림 지연을 **3분**으로 설정합니다." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "운구차 필요" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**대기 중** = 아직 외부에 있으며 수거를 기다리는 모든 사망자.\n" +
                    "**기한 초과** = 선택한 알림 지연 시간이 지난 대기 중 시민.\n" +
                    " - 기한 초과가 많다면 사망 알림 지연 시간을 늘려 보세요."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "처리량" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "게임 통계의 **월간 합계**입니다.\n" +
                    "**최대/월** = 현재 효율의 화장장 처리량 + 묘지 순환량입니다.\n" +
                    "모든 활성 장례 시설이 한 달에 처리할 수 있는 최대 시신 수입니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "자산" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**활성 건물 용량:** 총 운구차, 건물, 최대 작업자.\n" +
                    "\n" +
                    "**참고:**\n" +
                    "▪ 운구차: 운행 중-주차 아님 / (총* 운구차)\n" +
                    "▪ *총 운구차:\n" +
                    "== 정비 중인 운구차 포함(예: 낮은 서비스 예산), \n" +
                    "== 비활성화된 건물의 운구차는 제외.\n" +
                    "▪ 상태 스캔은 옵션이 열려 있거나 슬라이더를 사용할 때만 실행됩니다. 도시에서 매 프레임 실행되지 않으므로 사실상 성능 영향이 없습니다 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "상태가 로드되지 않았습니다." },
                { "MH_STATUS_NO_CITY_LOADED", "로드된 도시가 없습니다." },
                { "MH_STATUS_STATS_NOT_AVAIL", "도시 없음... ¯\\_(ツ)_/¯ ...통계 없음" },

                { "MH_STATUS_LINE1_V2", "{0} 대기 | {1} 기한 초과 | {2} 사망/월" },
                { "MH_STATUS_LINE2_V2", "{0} 최대/월" },
                { "MH_STATUS_LINE3", "{0} / {1} 운구차 | {2} / {3} 건물 | {4} 최대 작업자" },
                { "MH_STATUS_UPDATED", "업데이트 {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "현재 권장: 화장장 처리 약 {0}%" },
                { "MH_STATUS_PROCESSING_MORE", "현재 권장: 화장장 처리 500% + 활성 시설 추가" },
                { "MH_STATUS_PROCESSING_NONE", "권장: 화장장 활성화/추가" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "묘지" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**사용 중인 무덤 수**, 활성 묘지 시설, 이번 세션의 가득 찬 묘지 초기화 횟수를 표시합니다.\n" +
                    "재부팅하거나 도시를 바꾸면 상태가 초기화됩니다."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} 무덤 사용 | {2} 시설 | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0}회 초기화" },
                { "MH_STATUS_RESET_PLURAL", "{0}회 초기화" },
                { "MH_STATUS_CEMETERY_NONE", "이번 세션 없음" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0}개 더" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "모드" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "이 모드의 표시 이름입니다." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "버전" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "현재 버전입니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "제작자의 Paradox Mods 페이지를 엽니다." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "로그 보고서" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "자세한 장례 서비스 보고서와 예상 문제 영역을 MagicHearse.log에 기록합니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "로그 열기" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "**Logs/MagicHearse.log**가 있으면 엽니다.\n" +
                    "아직 파일이 없으면 대신 Logs 폴더를 엽니다."
                },
            };
        }

        public void Unload()
        { }
    }
}
