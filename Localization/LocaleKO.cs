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
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "자동 정리" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "자체 관리" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "상태" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "모드 정보" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "링크" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "마법 활성화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "영구차를 기다리는 **사망 시민을 자동 제거**합니다.\n" +
                    "두 체크를 모두 끄면 제거 없이 모드를 비활성화할 수 있어요."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "장의사" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "**시설** 값(처리, 차량, 보관)을 조절합니다.\n" +
                    "선택: **근로자도 증가**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "처리 속도" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**시설 처리 속도** (화장)\n" +
                    "**100%** = 바닐라 기본."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "차량 수" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "시설당 **영구차 최대 수**.\n" +
                    "**100%** = 바닐라 기본."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "묘지 보관" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "메인 건물의 **묘지 보관 용량**.\n" +
                    "**100%** = 바닐라 기본."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "최대 근로자 제어" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "**장의사**가 근로자 수까지 늘리게 하려면 ON.\n" +
                    "**ConfigXML**(또는 다른 모드)이 근로자를 관리하게 하려면 OFF."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "최대 근로자" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "사망 관련 시설의 **최대 근로자**를 조절합니다.\n" +
                    "**100%** = 바닐라 기본.\n\n" +
                    "**[o_o] 팁**\n" +
                    "  - **새 건물**에 적용됩니다.\n" +
                    "  - 확장 추가/삭제로 갱신되는 경우가 많아요."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "슬라이더 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "모든 슬라이더를 **100%**(바닐라 기본)로 되돌립니다." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "영구차 필요" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "영구차 픽업을 기다리는 **사망 시민**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "물량" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "게임 통계의 **월간 합계**입니다.\n" +
                     "**화장 최대/월** = 게임 정보 패널의 Handling/월.\n" +
                     "모든 화장장이 한 달에 처리할 수 있는 시신 수의 최대치입니다."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "자산" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**가동 중인 건물** 용량(영구차, 건물 수, 최대 근로자).\n\n" +
                    "**메모:**\n" +
                    "  - 예산이 낮아 정비 중인 영구차도 포함됩니다.\n" +
                    "  - 비활성화된 건물의 영구차는 포함하지 않습니다.\n" +
                    "  - 상태 스캔은 옵션 메뉴 또는 슬라이더 사용 시에만 실행되며, 도시에서 매 프레임 돌지 않아 성능 영향은 사실상 거의 없어요 :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "상태가 로드되지 않았습니다." },
                { "MH_STATUS_NO_CITY_LOADED", "아직 도시가 로드되지 않았습니다." },
                { "MH_STATUS_STATS_NOT_AVAIL", "도시 없음... ¯\\_(ツ)_/¯ ...통계 없음" },


                { "MH_STATUS_LINE1", "{0} 사망 대기 | 업데이트 {1}" },
                { "MH_STATUS_LINE2", "{0} 사망/월 | {1} 화장 최대/월 | {2} / {3} 묘지 사용" },
                { "MH_STATUS_LINE3", "{0} 영구차 | {1} / {2} 건물 | {3} 빈 무덤 | {4} 최대 근로자" },

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
