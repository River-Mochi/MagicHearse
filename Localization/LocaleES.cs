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
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the Spanish locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleES(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Spanish localization entries for this mod.</summary>
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), Mod.ModName + " " + Mod.ModTag },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Acciones" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Acerca de" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Limpieza automática" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Gestión manual" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Estado de la ciudad" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info del mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Enlaces" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activar magia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Elimina automáticamente** ciudadanos muertos que esperan una carroza.\n" +
                    "Apaga ambas casillas para desactivar el mod sin quitarlo."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Director funerario" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Escala **valores de instalaciones** (ritmo, flota, almacenamiento, trabajadores)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Ritmo de procesado" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Velocidad de procesado** de la instalación.\n" +
                    "**100%** = valor original del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Tamaño de flota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Máximo de carrozas** por instalación.\n" +
                    "Consejo: demasiadas pueden aumentar el tráfico.\n" +
                    "**100%** = valor original del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Almacenamiento del cementerio" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacidad de almacenamiento** del cementerio en el edificio principal.\n" +
                    "**100%** = valor original del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Trabajadores máx. (ver notas)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Escala **Trabajadores máximos** para instalaciones funerarias.\n" +
                     "**100%** = valor original del juego.\n" +
                    "**Consejos:**\n" +
                    "  - se aplica a edificios nuevos (creados después del cambio).\n" +
                    "  - truco: borrar/poner extensiones también actualiza al instante.\n\n" +
                    "Nota dev: flota/almacenamiento se actualizan al instante (stats del prefab). " +
                    "Trabajadores máx. es distinto (lo calcula el juego).\n" +
                    "Es más seguro reemplazar el edificio o la extensión para forzar refresco " +
                    "en vez de mutar un componente en tiempo de ejecución."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Restablecer sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Pone todos los sliders en **100%** (valores originales)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "Ciudadanos muertos esperando una carroza." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                    "Totales mensuales de las estadísticas del juego.\n" +
                    "Intenta que **puede manejarse** sea mayor que **muertes/mes**.\n" +
                    "...o activa la magia :)"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "Capacidades **activas** (no desactivadas): carrozas, edificios, trabajadores máx.\n\n" +
                    "**Notas:**\n" +
                    "  - incluye carrozas en mantenimiento\n" +
                    "  - no incluye carrozas de edificios desactivados.\n" +
                    "  - el slider de trabajadores máx. aplica a <edificios nuevos>\n" +
                    "  - el escaneo solo ocurre en Opciones y no corre por frame en la simulación;\n" +
                    "el mod está pensado para cuidar el rendimiento.\n"
                },

                // Status text templates 
                { "MH_STATUS_NOT_LOADED", "Estado no cargado." },
                { "MH_STATUS_NO_CITY_LOADED", "Aún no hay una ciudad cargada." },

                { "MH_STATUS_LINE1", "{0} muertos esperando | {1} actualizado" },
                { "MH_STATUS_LINE2", "{0} muertes/mes | {1} se puede manejar" },
                { "MH_STATUS_LINE3", "{0} carrozas | {1} / {2} edificios | {3} / {4} uso de cementerio | {5} trabajadores máx." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                  { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nombre visible del mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Versión" },
                  { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Versión actual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Abre la página del autor en Paradox Mods." },
            };
        }

        public void Unload()
        { }
    }
}
