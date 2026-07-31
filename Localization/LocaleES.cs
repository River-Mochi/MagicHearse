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
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Acciones" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Acerca de" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "Limpieza automática" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "Gestión manual" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "Avanzado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "Estado" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "Info del mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "Enlaces" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "Depuración" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Activar limpieza mágica" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Elimina automáticamente los cadáveres que requieren transporte (coche fúnebre).\n" +
                    "La limpieza mágica y la gestión manual son mutuamente excluyentes; elige una u otra.\n" +
                    "Desactiva todas las casillas para deshabilitar el mod sin quitarlo.\n" +
                    "Nota técnica: IsDead = true y WaitingForHearse = true son obligatorios."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Restablecer cementerio lleno" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Vacía cualquier cementerio lleno** para que no quede bloqueado con el icono LLENO.\n" +
                    "La limpieza mágica elimina la mayoría de los cadáveres antes del entierro — esta opción también vacía cualquier cementerio que **ya esté lleno**.\n" +
                    "<[ ] Desactivado por defecto>.\n" +
                    "Activa esta opción solo si el modo de limpieza mágica también debe vaciar los cementerios que ya estén llenos.\n" +
                    "Una vez vaciados, normalmente no es necesario mantener esta opción activada mientras la limpieza mágica siga activada."
                },               

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Director funerario" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Gestiona todo manualmente.\n" +
                    "**Valores de escala:** ritmo, flota, almacenamiento.\n" +
                    "Opcional: **aumentar trabajadores** también."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Procesamiento del crematorio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Velocidad de procesamiento del crematorio.**\n" +
                    "Los valores más altos incineran los cuerpos y liberan antes el almacenamiento de la instalación.\n" +
                    "**100%** = valor vanilla del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Tamaño de flota" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Máximo de coches fúnebres** por instalación.\n" +
                    "**100%** = valor vanilla del juego.\n" +
                    "**[Nota]** Demasiados coches fúnebres pueden afectar el tráfico según la tasa de muertes."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Almacenamiento del cementerio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacidad de almacenamiento del cementerio** para el edificio principal.\n" +
                    "Una mayor capacidad permite que un cementerio lleno vuelva a aceptar recogidas.\n" +
                    "No envía más coches fúnebres salvo que la falta de espacio estuviera bloqueando la instalación.\n" +
                    "**100%** = valor vanilla del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Restablecer cementerio lleno" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Vacía un cementerio** cuando se llena para que no quede bloqueado con el icono LLENO sobre el edificio.\n" +
                    "Ya no hace falta eliminar y reconstruir los cementerios llenos.\n" +
                    "Desactiva esta opción para usar en su lugar la **Renovación del cementerio** gradual.\n" +
                    "<[ ✓ ] Activado por defecto>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Renovación del cementerio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Libera gradualmente las tumbas ocupadas del cementerio.**\n" +
                    "Si los cementerios siguen mostrando el icono LLENO con demasiada frecuencia, aumenta este control.\n" +
                    "Los valores más altos vuelven a dejar tumbas disponibles antes que el juego vanilla.\n" +
                    "**100%** = valor vanilla del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Velocidad del coche fúnebre" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Aumenta la velocidad máxima del coche fúnebre**.\n" +
                    "**100%** = valor vanilla del juego.\n" +
                    "<Los límites de velocidad de la carretera siguen aplicando>.\n\n" +
                    "También ajusta la aceleración/frenado (suave) para que la nueva velocidad tope no cree salidas/paradas extremas.\n" +
                    "Nota: aunque se aumente la velocidad máxima del coche fúnebre, su velocidad real está influida por:\n" +
                    "máximo permitido del vehículo, límite de la carretera, velocidad segura de la IA del juego (curvas, daños en la carretera) y tráfico."

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
                     "**Capacidad máx./mes** = procesamiento de crematorios más renovación de cementerios con la eficiencia actual.\n" +
                     "Es el máximo de cuerpos que todas las instalaciones funerarias activas podrían gestionar al mes."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Activos" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacidades activas de edificios:** total de coches fúnebres, edificios, trabajadores máx.\n\n" +
                    "**Notas:**\n" +
                    "▪ Fúnebre: Activo-no aparcado / (Total* fúnebres)\n" +
                    "▪ *Total de coches fúnebres:\n" +
                    "== incluye los que están en mantenimiento (p. ej., por un presupuesto de servicio bajo), \n" +
                    "== no incluye los de edificios deshabilitados.\n" +
                    "▪ El escaneo de estado solo se ejecuta mientras las Opciones están abiertas (o al usar un deslizador); " +
                    "no se ejecuta en cada fotograma de la ciudad, por lo que prácticamente no afecta al rendimiento :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Estado no cargado." },
                { "MH_STATUS_NO_CITY_LOADED", "No hay ciudad cargada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Sin ciudad... ¯\\_(ツ)_/¯ ...Sin estadísticas" },

                { "MH_STATUS_LINE1", "{0} esperando | {1} muertes/mes | actualizado {2}" },
                { "MH_STATUS_LINE2", "{0} capacidad máx./mes | {1}/{2} tumbas usadas" },
                { "MH_STATUS_LINE3", "{0} / {1} fúnebres | {2} / {3} edificios | {4} trabajadores máx." },
                { "MH_STATUS_PROCESSING_SUGGESTED", "Sugerencia actual: ~{0}% de procesamiento de crematorios" },
                { "MH_STATUS_PROCESSING_MORE", "Sugerencia actual: 500% de procesamiento de crematorios + más instalaciones activas" },
                { "MH_STATUS_PROCESSING_NONE", "Sugerencia: activa/añade crematorios" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cementerio" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Cementerios vaciados automáticamente en esta sesión** por Restablecer cementerio lleno.\n" +
                    "Muestra el total de restablecimientos y cuántos cementerios distintos hay.\n" +
                    "Se borra al reiniciar o al cambiar de ciudad."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "Qué cementerios se vaciaron y cuántas veces cada uno (nombre × cantidad)." },

                { "MH_STATUS_LINE4", "restablecimientos: {0} · cementerios: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "ninguno en esta sesión" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} más" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nombre mostrado de este mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Versión" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Versión actual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Abre la página de mods de Paradox del autor." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Informe de registro" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "Escribe un informe detallado de servicios funerarios y posibles problemas en MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Abrir log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Abre **Logs/MagicHearse.log** si existe.\n" +
                    "Si el archivo aún no existe, abre la carpeta Logs." },
            };
        }

        public void Unload()
        { }
    }
}
