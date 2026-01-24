// File: Localization/LocalePT_BR.cs
// Portuguese (Brazil) pt-BR locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Portuguese (Brazil) localization source for Magic Hearse.</summary>
    public sealed class LocalePT_BR : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Portuguese (Brazil) locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocalePT_BR(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Portuguese (Brazil) localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Ações" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Sobre" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Limpeza automática" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Autogerenciar" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info do mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Links" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Ativar magia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Remove automaticamente cidadãos mortos**\n" +
                    "que estão esperando um carro funerário.\n" +
                    "Desligue as duas caixas para desativar o mod sem removê-lo."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Diretor funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Ajusta valores de **instalações** (taxa, frota, armazenamento).\n" +
                    "Opcional: **aumentar trabalhadores**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Taxa de processamento" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Velocidade de processamento** (cremações)\n" +
                    "**100%** = padrão vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Tamanho da frota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Máx. carros funerários** por instalação.\n" +
                    "**100%** = padrão vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Armazenamento do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacidade de armazenamento** do cemitério (prédio principal).\n" +
                    "**100%** = padrão vanilla."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Controlar trabalhadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Ative para o **Diretor funerário** aumentar o número de trabalhadores.\n" +
                    "Deixe OFF se você quer que **ConfigXML** (ou outro mod) controle os trabalhadores."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Trabalhadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Ajusta **trabalhadores máximos** para instalações de óbito.\n" +
                    "**100%** = padrão vanilla.\n\n" +
                    "**[o_o] Dicas**\n" +
                    "  - Aplica em **novos prédios**.\n" +
                    "  - Adicionar/remover uma extensão geralmente força atualização."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reset sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Define todos os sliders para **100%** (vanilla)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Carro funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Cidadãos mortos** esperando coleta."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Totais mensais** das estatísticas do jogo.\n" +
                     "**Cremação máx/mês** = painel info do jogo (Handling/mês).\n" +
                     "É o máximo de corpos que todos os crematórios poderiam processar por mês."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Recursos" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "Capacidades dos **prédios ativos** (carros funerários, prédios, trabalhadores máx).\n\n" +
                    "**Notas:**\n" +
                    "  - inclui carros funerários ainda em manutenção (por orçamento baixo).\n" +
                    "  - não inclui carros funerários de prédios desativados.\n" +
                    "  - a varredura de status só roda no menu Opções ou ao usar um slider; não roda por frame na cidade, então impacto de desempenho é praticamente zero :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status não carregado." },
                { "MH_STATUS_NO_CITY_LOADED", "Nenhuma cidade carregada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Stats ainda indisponíveis. Abra uma cidade e deixe a simulação rodar um pouco." },

                { "MH_STATUS_LINE1", "{0} mortos esperando | att {1}" },
                { "MH_STATUS_LINE2", "{0} mortes/mês | {1} cremação máx/mês | {2} / {3} uso do cemitério" },
                { "MH_STATUS_LINE3", "{0} carros funerários | {1} / {2} prédios | {3} covas livres | {4} trabalhadores máx." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nome exibido deste mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Versão atual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Abre a página do autor no Paradox Mods." },
            };
        }

        public void Unload()
        { }
    }
}
