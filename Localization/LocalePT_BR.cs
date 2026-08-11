// <copyright file="LocalePT_BR.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocalePT_BR.cs
// Brazilian Portuguese pt-BR locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Brazilian Portuguese localization source for Magic Hearse [MH].</summary>
    public sealed class LocalePT_BR : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Brazilian Portuguese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocalePT_BR(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Brazilian Portuguese localization entries for this mod.</summary>
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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Gerenciar manualmente" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Avançado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Status" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Info do mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Links" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Depuração" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Ativar Limpeza Mágica" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Remove automaticamente os corpos que precisam de transporte por carro funerário.\n" +
                    "A Limpeza Mágica e o gerenciamento manual são mutuamente exclusivos; escolha um ou outro.\n" +
                    "Desmarque todas as caixas para desativar o mod sem removê-lo.\n" +
                    "Nota técnica: é necessário IsDead = true e WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Redefinir cemitério lotado" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Esvazia um cemitério lotado** para que ele não fique bloqueado com o ícone LOTADO.\n" +
                    "A Limpeza Mágica remove a maioria dos corpos antes do enterro — esta opção ainda esvazia qualquer cemitério que **já esteja lotado**.\n" +
                    "<[ ] DESATIVADO por padrão>.\n" +
                    "Ative esta opção somente se o modo Limpeza Mágica também deve esvaziar cemitérios que já estejam lotados.\n" +
                    "Depois de esvaziado, normalmente não é preciso manter esta opção ativada enquanto a Limpeza Mágica continuar ativa."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Diretor funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Gerencie e otimize manualmente os sistemas funerários normais do jogo.\n" +
                    "**Valores de escala:** taxa, frota, armazenamento.\n" +
                    "Opcional: **aumentar também os trabalhadores**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Processamento do crematório" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Velocidade de processamento do crematório.**\n" +
                    "Valores maiores cremam os corpos e liberam o armazenamento da instalação mais cedo.\n" +
                    "**100%** = padrão do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Total de carros funerários" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Máximo de carros funerários** por instalação.\n" +
                    "**100%** = padrão do jogo.\n" +
                    "**[Nota]** Carros funerários demais podem afetar o trânsito dependendo da taxa de mortes."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Velocidade do carro funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Aumenta a velocidade máxima de condução permitida do carro funerário**.\n" +
                    "**100%** = padrão do jogo.\n" +
                    "<Os limites de velocidade das vias continuam valendo>.\n" +
                    "\n" +
                    "Também ajusta aceleração/frenagem (suave) para que a nova velocidade máxima não cause arrancadas ou paradas extremas.\n" +
                    "Nota: mesmo que a velocidade máxima do carro funerário seja aumentada, a velocidade real é influenciada por:\n" +
                    "limite máximo do veículo, limite da via, velocidade segura da IA do jogo (curvas, danos na via) e trânsito."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Atraso do aviso de morte (min)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "Este é o tempo total que um carro funerário tem para chegar a um prédio antes de aparecerem os ícones de problema de **espera por carro funerário**.\n" +
                    "**3 minutos** é próximo do padrão do jogo, de cerca de 2,5 minutos de simulação.\n" +
                    "Você pode aumentar esse valor para dar aos carros funerários um tempo mais razoável para concluir o trajeto antes que o ícone de morte apareça.\n" +
                    "Nota:\n" +
                    "- <Sugerido: 10 minutos>. Tente um valor maior em cidades muito congestionadas.\n" +
                    "- Confira o relatório de Status na parte inferior para ver quantos casos estão atrasados.\n" +
                    "- Ícones que já estão visíveis não são ocultados quando esse valor é aumentado pela primeira vez; eles permanecem até serem resolvidos por um carro funerário ou pela demolição do prédio.\n" +
                    "- Deixe os despachos atuais terminarem normalmente ou use uma vez a caixa <Limpeza Mágica [x]> para recomeçar rapidamente com os novos horários."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Armazenamento do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacidade de armazenamento do cemitério** para o prédio principal.\n" +
                    "Mais capacidade permite que um cemitério lotado volte a aceitar coletas.\n" +
                    "Isso não envia mais carros funerários, a menos que a falta de espaço estivesse bloqueando a instalação.\n" +
                    "**100%** = padrão do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Redefinir cemitério automaticamente" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Esvazia um cemitério lotado** para que ele não fique bloqueado pelo ícone LOTADO acima do prédio.\n" +
                    "Não é mais preciso apagar e reconstruir cemitérios lotados.\n" +
                    "Desative esta opção para usar a **Taxa de renovação do cemitério** gradual.\n" +
                    "<[ ✓ ] ATIVADO por padrão>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Taxa de renovação do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Libera gradualmente os túmulos ocupados do cemitério.**\n" +
                    "Valores maiores tornam os espaços disponíveis novamente mais rápido que no jogo padrão.\n" +
                    "Se os cemitérios ainda lotarem com muita frequência em 500%,\n" +
                    "ative **[Redefinir cemitério automaticamente]** em vez disso.\n" +
                    "**100%** = taxa padrão do jogo para reutilização dos túmulos."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Ajustar trabalhadores" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Alternância de compatibilidade:\n" +
                    "**Ative [✓]** para aumentar o número de trabalhadores.\n" +
                    "**[o_o]** Deixe DESATIVADO se quiser que o **ConfigXML** ou outro mod controle os trabalhadores do serviço funerário."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Máximo de trabalhadores" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Aumenta o máximo de trabalhadores** permitido.\n" +
                    "**100%** = padrão do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Redefinir controles" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Define os controles de porcentagem em **100%** e o atraso do aviso de morte em **3 minutos**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Carro funerário necessário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Aguardando** = todos os cidadãos mortos que ainda estão do lado de fora aguardando coleta.\n" +
                    "**Atrasados** = cidadãos aguardando cujo atraso de notificação selecionado expirou.\n" +
                    " - Se houver muitos atrasados, considere aumentar o tempo em Atraso do aviso de morte."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Totais mensais** das estatísticas do jogo.\n" +
                    "**Máx./mês** = processamento dos crematórios mais renovação dos cemitérios na eficiência atual.\n" +
                    "Este é o máximo de corpos que todas as instalações funerárias ativas poderiam processar por mês."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Recursos" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacidades dos prédios ativos:** total de carros funerários, prédios, máximo de trabalhadores.\n" +
                    "\n" +
                    "**Notas:**\n" +
                    "▪ Carro funerário: Ativo-não estacionado / (Total* de carros funerários)\n" +
                    "▪ *Total de carros funerários:\n" +
                    "== inclui carros funerários em manutenção (ex.: orçamento de serviço baixo), \n" +
                    "== não inclui carros funerários de prédios desativados.\n" +
                    "▪ A verificação de status só é executada enquanto as Opções estão abertas (ou ao usar um controle); não é executada a cada quadro na cidade, portanto praticamente não afeta o desempenho :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status não carregado." },
                { "MH_STATUS_NO_CITY_LOADED", "Nenhuma cidade carregada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Sem cidade... ¯\\_(ツ)_/¯ ...Sem estatísticas" },

                { "MH_STATUS_LINE1_V2", "{0} aguardando | {1} atrasados | {2} mortes/mês" },
                { "MH_STATUS_LINE2_V2", "{0} máx./mês" },
                { "MH_STATUS_LINE3", "{0} / {1} carros funerários | {2} / {3} prédios | {4} máx. trabalhadores" },
                { "MH_STATUS_UPDATED", "atualizado {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "sugerido agora: ~{0}% de processamento dos crematórios" },
                { "MH_STATUS_PROCESSING_MORE", "sugerido agora: 500% de processamento dos crematórios + mais instalações ativas" },
                { "MH_STATUS_PROCESSING_NONE", "sugerido: ative/adicione crematórios" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Mostra **túmulos usados**, instalações de cemitério ativas e redefinições de cemitérios lotados nesta sessão.\n" +
                    "O status é apagado ao reiniciar ou trocar de cidade."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} túmulos usados | {2} instalações | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} redefinição" },
                { "MH_STATUS_RESET_PLURAL", "{0} redefinições" },
                { "MH_STATUS_CEMETERY_NONE", "nenhuma nesta sessão" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} a mais" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nome exibido deste mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Versão atual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Abre a página de mods do autor no Paradox Mods." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Relatório de log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "Grava um relatório detalhado do serviço funerário e áreas prováveis de problema em MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Abrir log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Abre **Logs/MagicHearse.log** se existir.\n" +
                    "Se o arquivo ainda não existir, abre a pasta Logs."
                },
            };
        }

        public void Unload()
        { }
    }
}
