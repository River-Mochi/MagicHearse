// File: Localization/LocaleKO.cs
// Purpose: Korean ko-KR locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocaleKO : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleKO(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "동작" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "정보" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "자동 정리" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "수동 관리" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "모드 정보" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "링크" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "마법 활성화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "영구차를 기다리는 사망 시민을 자동으로 제거합니다.\n" +
                    "모드를 끄려면 두 체크박스를 모두 해제하세요."
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "장의 관리자" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "장의 시설 수치를 조정합니다 (속도, 차량 수, 저장량)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "처리 속도" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "시설 **처리 속도** 배율." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "차량 수" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "시설당 **최대 영구차 수** 배율." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "묘지 저장 공간" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**묘지 최대 저장 용량**을 증가시킵니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "슬라이더 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "모든 슬라이더를 **100%**(기본값)로 되돌립니다." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "버전" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "작성자의 Paradox Mods 페이지를 엽니다." },
            };
        }

        public void Unload()
        { }
    }
}
