// File: Localization/LocalePT_BR.cs
// Purpose: Portuguese (Brazil) pt-BR locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocalePT_BR : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocalePT_BR(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Ações" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Sobre" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Limpeza automática" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Gerenciar" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info do mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Ativar magia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "Remove automaticamente cidadãos mortos que estão esperando um carro funerário.\n" +
                    "Desmarque as duas caixas para desativar o mod sem removê-lo."
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Diretor funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Ajusta os valores das instalações funerárias (taxa, frota, armazenamento)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Taxa de processamento" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "Multiplicador da **velocidade de processamento** da instalação." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Tamanho da frota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "Multiplicador do **máximo de carros funerários** por instalação." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Armazenamento do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "Aumenta o **armazenamento máximo do cemitério**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Resetar sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Volta todos os sliders para **100%** (padrão do jogo)." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Versão" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Abre a página do autor no Paradox Mods." },
            };
        }

        public void Unload()
        { }
    }
}
