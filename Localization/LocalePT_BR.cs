// <copyright file="LocalePT_BR.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocalePT_BR.cs
// Portuguese (Brazil) pt-BR locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Portuguese (Brazil) localization source for Magic Hearse [MH].</summary>
    public sealed class LocalePT_BR : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Portuguese (Brazil) locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocalePT_BR(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Portuguese (Brazil) localization entries for this mod.</summary>
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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "Limpeza automática" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "Gerenciar manualmente" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "Avançado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "Info do mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "Links" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "Depuração" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Ativar limpeza mágica" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Remove automaticamente corpos que precisam de transporte (carro funerário).\n" +
                    "A limpeza mágica e a gestão manual são mutuamente exclusivas; escolha uma ou outra.\n" +
                    "Desligue todas as caixas para desativar o mod sem removê-lo.\n" +
                    "Nota técnica: IsDead = true e WaitingForHearse = true são obrigatórios."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Redefinir cemitério cheio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Esvazia qualquer cemitério cheio** para que ele não fique bloqueado com o ícone CHEIO.\n" +
                    "A limpeza mágica remove a maioria dos corpos antes do enterro — esta opção ainda esvazia qualquer cemitério que **já esteja cheio**.\n" +
                    "<[ ] DESLIGADO por padrão>.\n" +
                    "Ative esta opção somente se quiser que o modo de limpeza mágica também esvazie cemitérios que já estejam cheios.\n" +
                    "Depois de esvaziados, normalmente não é necessário manter esta opção ativada enquanto a limpeza mágica permanecer ligada."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Diretor funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Gerencie tudo manualmente.\n" +
                    "**Valores de escala:** taxa, frota, armazenamento.\n" +
                    "Opcional: **aumentar trabalhadores** também."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Processamento do crematório" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Velocidade de processamento do crematório.**\n" +
                    "Valores maiores cremam os corpos e liberam o armazenamento da instalação mais cedo.\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Tamanho da frota" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Máximo de carros funerários** por instalação.\n" +
                    "**100%** = padrão vanilla do jogo.\n" +
                    "**[Nota]** Carros funerários demais podem afetar o trânsito dependendo da taxa de mortes."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Armazenamento do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacidade de armazenamento do cemitério** do prédio principal.\n" +
                    "Mais capacidade permite que um cemitério cheio volte a aceitar coletas.\n" +
                    "Isso não envia mais carros funerários, a menos que a falta de espaço estivesse bloqueando a instalação.\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Redefinir cemitério cheio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Esvazia um cemitério** quando ele está cheio para não ficar bloqueado pelo ícone CHEIO acima do prédio.\n" +
                    "Não é mais preciso excluir e reconstruir cemitérios cheios.\n" +
                    "Desative esta opção para usar a **Taxa de renovação do cemitério** gradual.\n" +
                    "<[ ✓ ] Ativado por padrão>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Taxa de renovação do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Libera gradualmente os espaços de sepultura ocupados.**\n" +
                    "Se os cemitérios ainda mostrarem o ícone CHEIO com muita frequência, aumente este controle.\n" +
                    "Valores maiores tornam os espaços disponíveis novamente mais rápido que no jogo vanilla.\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Velocidade do carro funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Aumenta a velocidade máxima do carro funerário**.\n" +
                    "**100%** = padrão vanilla do jogo.\n" +
                    "<Os limites de velocidade das vias ainda valem>.\n\n" +
                    "Também ajusta aceleração/frenagem (suave) para que a nova velocidade máxima não crie arrancadas/paradas extremas.\n" +
                    "Obs.: mesmo aumentando a velocidade máxima do carro funerário, a velocidade real é influenciada por:\n" +
                    "máximo permitido do veículo, limite da via, velocidade segura da IA do jogo (curvas, danos na via) e trânsito."

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Controlar trabalhadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Alternância de compatibilidade:\n" +
                    "**Ativar [✓]** para aumentar o número de trabalhadores.\n" +
                    "**[o_o]** Deixe em OFF se quiser que o **ConfigXML** ou outro mod controle os trabalhadores do serviço funerário."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Trabalhadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Aumenta o máximo de trabalhadores** permitido.\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Resetar sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "Define todos os sliders de volta para **100%** (padrões vanilla)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Carro funerário necessário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Cidadãos mortos esperando** a coleta do carro funerário."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "**Totais mensais** das estatísticas do jogo.\n" +
                     "**Capacidade máx./mês** = processamento dos crematórios mais renovação dos cemitérios na eficiência atual.\n" +
                     "É o máximo de corpos que todas as instalações funerárias ativas podem atender por mês."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Ativos" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacidades ativas dos prédios:** total de carros funerários, prédios, trabalhadores máx.\n\n" +
                    "**Notas:**\n" +
                    "▪ Carro funerário: Ativo-não estacionado / (Total* carros funerários)\n" +
                    "▪ *Total de carros funerários:\n" +
                    "== inclui carros funerários em manutenção (ex.: orçamento de serviço baixo), \n" +
                    "== não inclui carros funerários de prédios desativados.\n" +
                    "▪ A verificação de status só roda enquanto as Opções estiverem abertas (ou ao usar um controle); " +
                    "não roda a cada quadro na cidade, então praticamente não afeta o desempenho :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Status não carregado." },
                { "MH_STATUS_NO_CITY_LOADED", "Nenhuma cidade carregada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Sem cidade... ¯\\_(ツ)_/¯ ...Sem stats" },

                { "MH_STATUS_LINE1", "{0} esperando | {1} mortes/mês | atualizado {2}" },
                { "MH_STATUS_LINE2", "{0} capacidade máx./mês | {1}/{2} túmulos usados" },
                { "MH_STATUS_LINE3", "{0} / {1} carros funerários | {2} / {3} prédios | {4} trabalhadores máx." },
                { "MH_STATUS_PROCESSING_SUGGESTED", "Sugestão atual: processamento em ~{0}%" },
                { "MH_STATUS_PROCESSING_MORE", "Sugestão atual: processamento em 500% + mais instalações ativas" },
                { "MH_STATUS_PROCESSING_NONE", "Sugestão: ative/adicione crematórios" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Cemitérios esvaziados automaticamente nesta sessão** por Redefinir cemitério cheio.\n" +
                    "Mostra o total de redefinições e quantos cemitérios diferentes foram afetados.\n" +
                    "É apagado ao reiniciar ou ao trocar de cidade."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "Quais cemitérios foram esvaziados e quantas vezes cada um (nome × quantidade)." },

                { "MH_STATUS_LINE4", "redefinições: {0} · cemitérios: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "nenhum nesta sessão" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} a mais" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nome exibido deste mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Versão atual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Abre a página Paradox Mods do autor." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Relatório de log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "Grava um relatório detalhado dos serviços funerários e possíveis problemas em MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Abrir log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Abre **Logs/MagicHearse.log** se existir.\n" +
                    "Se o arquivo ainda não existir, abre a pasta Logs." },
            };
        }

        public void Unload()
        { }
    }
}
