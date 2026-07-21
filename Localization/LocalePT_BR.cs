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
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AutoCleanGrp),   "Limpeza automática" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.SelfManageGrp),  "Gerenciar manualmente" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AdvancedGrp),    "Avançado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.StatusGrp),      "Status" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutInfoGrp),   "Info do mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutLinksGrp),  "Links" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Ativar limpeza mágica" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "**Remove automaticamente cidadãos mortos** que estão esperando um carro funerário.\n" +
                    "Desligue as duas caixas para desativar o mod sem removê-lo."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Diretor funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Gerencie tudo manualmente.\n" +
                    "**Valores de escala:** taxa, frota, armazenamento.\n" +
                    "Opcional: **aumentar trabalhadores** também."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Taxa de processamento" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Velocidade de processamento da instalação** (cremações)\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Tamanho da frota" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Máximo de carros funerários** por instalação.\n" +
                    "**100%** = padrão vanilla do jogo.\n" +
                    "**[o_o]** Carros funerários demais podem afetar o trânsito dependendo da taxa de mortes."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Armazenamento do cemitério" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacidade de armazenamento do cemitério** do prédio principal.\n" +
                    "**100%** = padrão vanilla do jogo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Esvaziar auto. quando cheio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Esvazia um cemitério automaticamente** assim que ele enche.\n" +
                    "As sepulturas ocupadas voltam a 0 — como demolir e reconstruir, mas instantâneo e automático.\n" +
                    "Combina com o controle deslizante **Armazenamento do cemitério**: defina o tamanho dos cemitérios e deixe-os se reciclar para nunca demolir um cheio.\n" +
                    "Ativado por padrão enquanto o **Diretor funerário** estiver ativo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Velocidade do carro funerário" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Aumenta a velocidade máxima do carro funerário**.\n" +
                    "**100%** = padrão vanilla do jogo.\n" +
                    "<Os limites de velocidade das vias ainda valem>.\n\n" +
                    "Também ajusta aceleração/frenagem (suave) para que a nova velocidade máxima não crie arrancadas/paradas extremas.\n" +
                    "Obs.: mesmo aumentando a velocidade máxima, a velocidade real é basicamente:\n" +
                    "(máx. do veículo, limite da via, velocidade segura da IA, trânsito)"

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
                     "**Cremação máx./mês** = painel de info Handling/mês do jogo.\n" +
                     "Este é o máximo de corpos que poderiam ser processados por crematórios por mês."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Ativos" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
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
             
                { "MH_STATUS_LINE1", "{0} esperando | {1} mortes/mês | atualizado {2}" },
                { "MH_STATUS_LINE2", "{0} cremação máx./mês | {1}/{2} túmulos usados" },
                { "MH_STATUS_LINE3", "{0} / {1} carros funerários | {2} / {3} prédios | {4} trabalhadores máx." },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cemetery" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Cemeteries auto-emptied this session** by Auto-empty when full.\n" +
                    "Shows total resets and how many distinct cemeteries.\n" +
                    "Clears on reboot or when you switch city."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "Which cemeteries were emptied, and how many times each (name × count)." },

                { "MH_STATUS_LINE4", "resets: {0} · cemeteries: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "none this session" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} more" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nome exibido deste mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Versão atual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Abre a página Paradox Mods do autor." },
            };
        }

        public void Unload()
        { }
    }
}
