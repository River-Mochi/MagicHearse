// <copyright file="LocalePT_PT.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocalePT_PT.cs
// European Portuguese pt-PT locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// European Portuguese localization source for Magic Hearse [MH].</summary>
    public sealed class LocalePT_PT : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the European Portuguese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocalePT_PT(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all European Portuguese localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Ações" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Sobre" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Limpeza automática" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Gestão manual" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Avançado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Estado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Info do mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Ligações" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Depuração" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Ativar Limpeza Mágica" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Remove automaticamente os cadáveres que necessitam de transporte por carro funerário.\n" +
                    "A Limpeza Mágica e a gestão manual são mutuamente exclusivas; escolha uma ou outra.\n" +
                    "Desmarque todas as caixas para desativar o mod sem o remover.\n" +
                    "Nota técnica: são necessários IsDead = true e WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Repor cemitério cheio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Esvazia um cemitério cheio** para que não fique bloqueado com o ícone CHEIO.\n" +
                    "A Limpeza Mágica remove a maioria dos cadáveres antes do enterro — esta opção continua a esvaziar qualquer cemitério que **já esteja cheio**.\n" +
                    "<[ ] DESATIVADO por predefinição>.\n" +
                    "Ative esta opção apenas se o modo Limpeza Mágica também deve esvaziar cemitérios que já estejam cheios.\n" +
                    "Depois de esvaziado, normalmente não é necessário manter esta opção ativa enquanto a Limpeza Mágica permanecer ativa."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Diretor funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Faça a gestão e otimize manualmente os sistemas funerários normais do jogo.\n" +
                    "**Valores de escala:** taxa, frota, armazenamento.\n" +
                    "Opcional: **aumentar também os trabalhadores**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Processamento do crematório" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Velocidade de processamento do crematório.**\n" +
                    "Valores mais altos cremam os corpos e libertam o armazenamento da instalação mais cedo.\n" +
                    "**100%** = valor predefinido do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Total de carros funerários" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Máximo de carros funerários** por instalação.\n" +
                    "**100%** = valor predefinido do jogo.\n" +
                    "**[Nota]** Demasiados carros funerários podem afetar o trânsito consoante a taxa de mortes."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Velocidade do carro funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Aumenta a velocidade máxima de condução permitida do carro funerário**.\n" +
                    "**100%** = valor predefinido do jogo.\n" +
                    "<Os limites de velocidade das estradas continuam a aplicar-se>.\n" +
                    "\n" +
                    "Também ajusta a aceleração/travagem (suave) para que a nova velocidade máxima não cause arranques ou paragens extremos.\n" +
                    "Nota: mesmo que a velocidade máxima do carro funerário seja aumentada, a velocidade real é influenciada por:\n" +
                    "limite máximo do veículo, limite da estrada, velocidade segura da IA do jogo (curvas, danos na estrada) e trânsito."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Atraso do aviso de morte (min)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "Este é o tempo total que um carro funerário tem para chegar a um edifício antes de aparecerem os ícones de problema de **espera por carro funerário**.\n" +
                    "**3 minutos** é próximo do valor predefinido do jogo de cerca de 2,5 minutos de simulação.\n" +
                    "Pode aumentar este valor para dar aos carros funerários um tempo mais razoável para concluir a viagem antes de aparecer o ícone de morte.\n" +
                    "Nota:\n" +
                    "- <Sugerido: 10 minutos>. Experimente um valor superior em cidades muito congestionadas.\n" +
                    "- Consulte o relatório de Estado no fundo para ver quantos casos estão atrasados.\n" +
                    "- Os ícones já visíveis não são ocultados quando este valor é aumentado pela primeira vez; permanecem até serem resolvidos por um carro funerário ou pela demolição do edifício.\n" +
                    "- Deixe os despachos atuais terminar normalmente ou use uma vez a caixa <Limpeza Mágica [x]> para recomeçar rapidamente com os novos horários."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Armazenamento do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacidade de armazenamento do cemitério** para o edifício principal.\n" +
                    "Mais capacidade permite que um cemitério cheio volte a aceitar recolhas.\n" +
                    "Não envia mais carros funerários, exceto se a falta de espaço estivesse a bloquear a instalação.\n" +
                    "**100%** = valor predefinido do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Repor cemitério automaticamente" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Esvazia um cemitério cheio** para que não fique bloqueado pelo ícone CHEIO acima do edifício.\n" +
                    "Já não é necessário eliminar e reconstruir cemitérios cheios.\n" +
                    "Desative esta opção para usar a **Taxa de renovação do cemitério** gradual.\n" +
                    "<[ ✓ ] ATIVADO por predefinição>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Taxa de renovação do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Liberta gradualmente os túmulos ocupados do cemitério.**\n" +
                    "Valores mais altos tornam os espaços novamente disponíveis mais depressa do que no jogo base.\n" +
                    "Se os cemitérios continuarem a encher-se com demasiada frequência a 500%,\n" +
                    "ative **[Repor cemitério automaticamente]** em alternativa.\n" +
                    "**100%** = taxa predefinida do jogo para reutilização dos túmulos."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Ajustar trabalhadores" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Comutador de compatibilidade:\n" +
                    "**Ative [✓]** para aumentar o número de trabalhadores.\n" +
                    "**[o_o]** Deixe DESATIVADO se quiser que o **ConfigXML** ou outro mod controle os trabalhadores dos serviços funerários."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Máximo de trabalhadores" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Aumenta o máximo de trabalhadores** permitido.\n" +
                    "**100%** = valor predefinido do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Repor controlos" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Define os controlos percentuais para **100%** e o atraso do aviso de morte para **3 minutos**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Carro funerário necessário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**À espera** = todos os cidadãos mortos ainda no exterior e à espera de recolha.\n" +
                    "**Atrasados** = cidadãos à espera cujo atraso de notificação selecionado já terminou.\n" +
                    " - Se houver muitos atrasados, considere aumentar o tempo em Atraso do aviso de morte."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Totais mensais** das estatísticas do jogo.\n" +
                    "**Máx./mês** = processamento dos crematórios mais renovação dos cemitérios com a eficiência atual.\n" +
                    "Este é o máximo de corpos que todas as instalações funerárias ativas poderiam processar por mês."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Recursos" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacidades dos edifícios ativos:** total de carros funerários, edifícios, máximo de trabalhadores.\n" +
                    "\n" +
                    "**Notas:**\n" +
                    "▪ Carro funerário: Ativo-não estacionado / (Total* de carros funerários)\n" +
                    "▪ *Total de carros funerários:\n" +
                    "== inclui carros funerários em manutenção (ex.: orçamento de serviço baixo), \n" +
                    "== não inclui carros funerários de edifícios desativados.\n" +
                    "▪ A verificação de estado só é executada enquanto as Opções estão abertas (ou ao usar um controlo); não é executada a cada frame na cidade, por isso praticamente não tem impacto no desempenho :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Estado não carregado." },
                { "MH_STATUS_NO_CITY_LOADED", "Nenhuma cidade carregada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Sem cidade... ¯\\_(ツ)_/¯ ...Sem estatísticas" },

                { "MH_STATUS_LINE1_V2", "{0} à espera | {1} atrasados | {2} mortes/mês" },
                { "MH_STATUS_LINE2_V2", "{0} máx./mês" },
                { "MH_STATUS_LINE3", "{0} / {1} carros funerários | {2} / {3} edifícios | {4} máx. trabalhadores" },
                { "MH_STATUS_UPDATED", "atualizado {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "sugerido agora: ~{0}% de processamento dos crematórios" },
                { "MH_STATUS_PROCESSING_MORE", "sugerido agora: 500% de processamento dos crematórios + mais instalações ativas" },
                { "MH_STATUS_PROCESSING_NONE", "sugerido: ative/adicione crematórios" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Mostra **túmulos usados**, instalações de cemitério ativas e reposições de cemitérios cheios nesta sessão.\n" +
                    "O estado é limpo ao reiniciar ou ao mudar de cidade."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} túmulos usados | {2} instalações | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} reposição" },
                { "MH_STATUS_RESET_PLURAL", "{0} reposições" },
                { "MH_STATUS_CEMETERY_NONE", "nenhuma nesta sessão" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} mais" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nome apresentado deste mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Versão atual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Abre a página de mods do autor no Paradox Mods." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Relatório de registo" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "Escreve um relatório detalhado dos serviços funerários e das áreas problemáticas prováveis em MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Abrir registo" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Abre **Logs/MagicHearse.log** se existir.\n" +
                    "Se o ficheiro ainda não existir, abre a pasta Logs."
                },
            };
        }

        public void Unload()
        { }
    }
}
