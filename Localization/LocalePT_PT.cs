// <copyright file="LocalePT_PT.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocalePT_PT.cs
// Portuguese (Portugal) pt-PT locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Portuguese (Portugal) localization source for Magic Hearse [MH].</summary>
    public sealed class LocalePT_PT : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Portuguese (Portugal) locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocalePT_PT(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Portuguese (Portugal) localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.ActionsTab), "Ações" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.AboutTab), "Sobre" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AutoCleanGrp), "Limpeza automática" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.SelfManageGrp), "Gestão manual" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AdvancedGrp), "Avançado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.StatusGrp), "Estado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutInfoGrp), "Informação do mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutLinksGrp), "Ligações" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Ativar limpeza mágica" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Remove automaticamente corpos que precisam de transporte (carro funerário).\n" +
                    "A limpeza mágica e a gestão manual são mutuamente exclusivas; escolha uma ou outra.\n" +
                    "Desative todas as caixas para desligar o mod sem o remover.\n" +
                    "Nota técnica: IsDead = true e WaitingForHearse = true são obrigatórios."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Repor cemitério cheio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Esvazia qualquer cemitério cheio** para que não fique bloqueado com o ícone CHEIO.\n" +
                    "A limpeza mágica remove a maioria dos corpos antes do enterro — esta opção continua a esvaziar qualquer cemitério que **já esteja cheio**.\n" +
                    "[ ✓ ] ATIVADO por predefinição.\n" +
                    "Se não houver cemitérios cheios, isto não for uma preocupação e a limpeza mágica ficar sempre ativa,\n" +
                    " pode desativar esta opção, pois não é necessária."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Diretor funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Faça a gestão manual de tudo.\n" +
                    "**Ajuste os valores:** processamento, frota, capacidade.\n" +
                    "Opcional: também pode **aumentar os trabalhadores**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Taxa de processamento" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Velocidade de processamento da instalação** (cremações)\n" +
                    "**100%** = valor predefinido do jogo base."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Tamanho da frota" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Máximo de carros funerários** por instalação.\n" +
                    "**100%** = valor predefinido do jogo base.\n" +
                    "**[Nota]** Demasiados carros funerários podem afetar o trânsito, consoante a taxa de mortalidade."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Capacidade do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacidade do edifício principal do cemitério**.\n" +
                    "**100%** = valor predefinido do jogo base."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Repor cemitério cheio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Esvazia um cemitério** quando fica cheio, para que não seja bloqueado pelo ícone CHEIO sobre o edifício.\n" +
                    "Já não é necessário eliminar e reconstruir cemitérios cheios.\n" +
                    "Funciona com o controlo **Capacidade do cemitério**: defina o tamanho dos cemitérios e deixe-os ser reutilizados, para nunca mais ter de demolir um cemitério cheio.\n" +
                    "<[ ✓ ] ATIVADO por predefinição>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Velocidade do carro funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Aumenta a velocidade máxima do carro funerário**.\n" +
                    "**100%** = valor predefinido do jogo base.\n" +
                    "<Os limites de velocidade das estradas continuam a aplicar-se>.\n\n" +
                    "Também ajusta suavemente a aceleração e a travagem, para que a nova velocidade máxima não cause arranques ou paragens bruscos.\n" +
                    "Nota: mesmo com a velocidade máxima do carro funerário aumentada, a velocidade real é influenciada por:\n" +
                    "máximo permitido do veículo, limite da estrada, velocidade segura da IA do jogo (curvas, danos na estrada) e trânsito."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Controlar máx. de trabalhadores" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Opção de compatibilidade:\n" +
                    "**Ative [✓]** para aumentar o número de trabalhadores.\n" +
                    "**[o_o]** Deixe DESATIVADO se quiser que o **ConfigXML** ou outro mod controle os trabalhadores dos serviços funerários."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Máx. de trabalhadores" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Aumenta o máximo de trabalhadores** permitido.\n" +
                    "**100%** = valor predefinido do jogo base."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Repor valores" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Repõe todos os controlos para **100%** (valores predefinidos do jogo base)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "À espera de carro" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Cidadãos falecidos à espera** de recolha por um carro funerário."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Totais mensais** das estatísticas do jogo.\n" +
                    "**Cremações máx./mês** = valor Tratamento/mês no painel de informações do jogo.\n" +
                    "É o número máximo de corpos que os crematórios conseguem processar por mês."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Recursos" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacidades ativas dos edifícios:** total de carros funerários, edifícios e máx. de trabalhadores.\n\n" +
                    "**Notas:**\n" +
                    "▪ Carros funerários: ativos, não estacionados / (Total* de carros funerários)\n" +
                    "▪ *Total de carros funerários:\n" +
                    "== inclui carros em manutenção (por ex., devido a orçamento baixo), \n" +
                    "== não inclui carros de edifícios desativados.\n" +
                    "▪ A análise de estado só é executada enquanto as Opções estão abertas (ou ao usar um controlo); " +
                    "não é executada a cada frame na cidade, por isso praticamente não afeta o desempenho :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Estado não carregado." },
                { "MH_STATUS_NO_CITY_LOADED", "Nenhuma cidade carregada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Sem cidade... ¯\\_(ツ)_/¯ ...Sem estatísticas" },

                { "MH_STATUS_LINE1", "{0} à espera | {1} mortes/mês | atualizado {2}" },
                { "MH_STATUS_LINE2", "{0} cremações máx./mês | {1}/{2} sepulturas ocupadas" },
                { "MH_STATUS_LINE3", "{0} / {1} carros funerários | {2} / {3} edifícios | {4} trabalhadores máx." },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Cemitérios esvaziados automaticamente nesta sessão** pela opção Repor cemitério cheio.\n" +
                    "Mostra o total de reposições e o número de cemitérios diferentes.\n" +
                    "É limpo ao reiniciar o jogo ou ao mudar de cidade."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)), "Que cemitérios foram esvaziados e quantas vezes cada um (nome × quantidade)." },

                { "MH_STATUS_LINE4", "reposições: {0} · cemitérios: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "nenhum nesta sessão" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+mais {0}" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nome apresentado deste mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Versão atual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Abre a página de mods Paradox do autor." },
            };
        }

        public void Unload()
        { }
    }
}
