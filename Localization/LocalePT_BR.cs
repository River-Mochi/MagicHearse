// File: Localization/LocalePT_BR.cs
// Portuguese (Brazil) pt-BR locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Portuguese (Brazil) localization source for Magic Hearse [MH].</summary>
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
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "Limpeza automática" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "Gerenciar manualmente" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "Avançado" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "Info do mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "Links" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Ativar limpeza mágica" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Remove automaticamente cidadãos mortos** que estão esperando um carro funerário.\n" +
                    "Desligue as duas caixas para desativar o mod sem removê-lo."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Diretor funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Gerencie tudo manualmente.\n" +
                    "**Valores de escala:** taxa, frota, armazenamento.\n" +
                    "Opcional: **aumentar trabalhadores** também."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Taxa de processamento" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Velocidade de processamento da instalação** (cremações)\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Tamanho da frota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Máximo de carros funerários** por instalação.\n" +
                    "**100%** = padrão vanilla do jogo.\n" +
                    "**[o_o]** Carros funerários demais podem afetar o trânsito dependendo da taxa de mortes."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Armazenamento do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacidade de armazenamento do cemitério** do prédio principal.\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "Velocidade do carro funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**Aumenta a velocidade máxima do carro funerário**.\n" +
                    "**100%** = padrão vanilla do jogo.\n" +
                    "<Os limites de velocidade das vias ainda valem>.\n\n" +
                    "Também ajusta aceleração/frenagem (suave) para que a nova velocidade máxima não crie arrancadas/paradas extremas.\n" +
                    "Obs.: mesmo aumentando a velocidade máxima, a velocidade real é basicamente:\n" +
                    "(máx. do veículo, limite da via, velocidade segura da IA, trânsito)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Controlar trabalhadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Alternância de compatibilidade:\n" +
                    "**Ativar [✓]** para aumentar o número de trabalhadores.\n" +
                    "**[o_o]** Deixe em OFF se quiser que o **ConfigXML** ou outro mod controle os trabalhadores do serviço funerário."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Trabalhadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**Aumenta o máximo de trabalhadores** permitido.\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Resetar sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Define todos os sliders de volta para **100%** (padrões vanilla)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Carro funerário necessário" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Cidadãos mortos esperando** a coleta do carro funerário."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Totais mensais** das estatísticas do jogo.\n" +
                     "**Cremação máx./mês** = painel de info Handling/mês do jogo.\n" +
                     "Este é o máximo de corpos que poderiam ser processados por crematórios por mês."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Ativos" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Capacidades ativas dos prédios:** total de carros funerários, prédios, trabalhadores máx.\n\n" +
                    "**Notas:**\n" +
                    "▪ Carro funerário: Ativo-não estacionado / (Total* carros funerários)\n" +
                    "▪ *Total carro funerário:" +
                    "=== inclui carro funerário em manutenção (ex.: orçamento de serviço baixo), \n" +
                    "=== não inclui carros funerários de prédios desativados.\n" +
                    "▪ A varredura de status só roda enquanto Options estiver aberto (ou ao mexer em um slider); " +
                    "não roda por frame na cidade, então praticamente sem impacto de performance :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status não carregado." },
                { "MH_STATUS_NO_CITY_LOADED", "Nenhuma cidade carregada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Sem cidade... ¯\\_(ツ)_/¯ ...Sem stats" },

                { "MH_STATUS_LINE1", "{0} mortos esperando | atualizado {1}" },
                { "MH_STATUS_LINE2", "{0} mortes/mês | {1} cremação máx./mês | {2} / {3} uso do cemitério" },
                { "MH_STATUS_LINE3", "{0} / {1} carros funerários | {2} / {3} prédios | {4} túmulos vazios | {5} trabalhadores máx." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nome exibido deste mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Versão atual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Abre a página Paradox Mods do autor." },
            };
        }

        public void Unload()
        { }
    }
}
