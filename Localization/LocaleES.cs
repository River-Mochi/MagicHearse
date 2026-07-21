// <copyright file="LocaleES.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleES.cs
// Spanish es-ES locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// Spanish localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleES : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Spanish locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleES(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Spanish localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.ActionsTab), "Acciones" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.AboutTab), "Acerca de" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AutoCleanGrp),   "Limpieza automática" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.SelfManageGrp),  "Gestión manual" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AdvancedGrp),    "Avanzado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.StatusGrp),      "Estado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutInfoGrp),   "Info del mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutLinksGrp),  "Enlaces" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Activar limpieza mágica" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "**Elimina automáticamente a los ciudadanos muertos** que están esperando un coche fúnebre.\n" +
                    "Desactiva ambas casillas para deshabilitar el mod sin quitarlo."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Director funerario" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Gestiona todo manualmente.\n" +
                    "**Valores de escala:** ritmo, flota, almacenamiento.\n" +
                    "Opcional: **aumentar trabajadores** también."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Tasa de procesamiento" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Velocidad de procesamiento de la instalación** (cremaciones)\n" +
                    "**100%** = valor vanilla del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Tamaño de flota" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Máximo de coches fúnebres** por instalación.\n" +
                    "**100%** = valor vanilla del juego.\n" +
                    "**[o_o]** Demasiados coches fúnebres pueden afectar el tráfico según la tasa de muertes."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Almacenamiento del cementerio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacidad de almacenamiento del cementerio** para el edificio principal.\n" +
                    "**100%** = valor vanilla del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Vaciar auto. al llenarse" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Vacía un cementerio automáticamente** en cuanto se llena.\n" +
                    "Las tumbas ocupadas vuelven a 0 — como demoler y reconstruir, pero instantáneo y automático.\n" +
                    "Se combina con el control deslizante **Almacenamiento del cementerio**: dimensiona tus cementerios y deja que se reciclen para no demoler nunca uno lleno.\n" +
                    "Activado por defecto mientras el **Director funerario** está activo."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Velocidad del coche fúnebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Aumenta la velocidad máxima del coche fúnebre**.\n" +
                    "**100%** = valor vanilla del juego.\n" +
                    "<Los límites de velocidad de la carretera siguen aplicando>.\n\n" +
                    "También ajusta la aceleración/frenado (suave) para que la nueva velocidad tope no cree salidas/paradas extremas.\n" +
                    "Nota: incluso si se aumenta la velocidad máxima del coche fúnebre, su velocidad real es básicamente:\n" +
                    "(máximo del vehículo, límite de la carretera, velocidad segura de la IA, tráfico)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Controlar trabajadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Interruptor de compatibilidad:\n" +
                    "**Activar [✓]** para aumentar el número de trabajadores.\n" +
                    "**[o_o]** Dejar en OFF si se prefiere que **ConfigXML** u otro mod controle los trabajadores del servicio funerario."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Trabajadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Aumenta el máximo de trabajadores** permitido.\n" +
                    "**100%** = valor vanilla del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Restablecer deslizadores" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "Devuelve todos los deslizadores a **100%** (valores vanilla)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Fúnebre necesario" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Ciudadanos muertos esperando** la recogida del coche fúnebre."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "**Totales mensuales** de las estadísticas del juego.\n" +
                     "**Cremación máx./mes** = panel de info Handling/mes del juego.\n" +
                     "Este es el máximo de cuerpos que podrían procesar los crematorios por mes."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Activos" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacidades activas de edificios:** total de coches fúnebres, edificios, trabajadores máx.\n\n" +
                    "**Notes:**\n" +
                    "▪ Fúnebre: Activo-no aparcado / (Total* fúnebres)\n" +
                    "▪ *Total fúnebre:" +
                    "=== incluye fúnebres en mantenimiento (p. ej. presupuesto de servicio bajo), \n" +
                    "=== no incluye fúnebres de edificios deshabilitados.\n" +
                    "▪ El escaneo de estado solo corre mientras Options está abierto (o usas un deslizador); " +
                    "no corre por frame en la ciudad, así que básicamente sin impacto de rendimiento :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Estado no cargado." },
                { "MH_STATUS_NO_CITY_LOADED", "No hay ciudad cargada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Sin ciudad... ¯\\_(ツ)_/¯ ...Sin estadísticas" },

                { "MH_STATUS_LINE1", "{0} esperando | {1} muertes/mes | actualizado {2}" },
                { "MH_STATUS_LINE2", "{0} cremación máx./mes | {1}/{2} tumbas usadas" },
                { "MH_STATUS_LINE3", "{0} / {1} fúnebres | {2} / {3} edificios | {4} trabajadores máx." },

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
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nombre mostrado de este mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Versión" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Versión actual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Abre la página de mods de Paradox del autor." },
            };
        }

        public void Unload()
        { }
    }
}
