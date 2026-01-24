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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Acciones" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "Acerca de" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Limpieza auto" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Autogestión" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Estado" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info del mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Enlaces" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activar magia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Elimina automáticamente a los muertos**\n" +
                    "que están esperando un coche fúnebre.\n" +
                    "Apaga ambas casillas para desactivar el mod sin quitarlo."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Director funerario" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Escala valores de **edificios** (tasa, flota, almacén).\n" +
                    "Opcional: **subir trabajadores**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Tasa de procesamiento" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Velocidad de procesamiento** (cremaciones)\n" +
                    "**100%** = por defecto del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Tamaño de flota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Máx. coches fúnebres** por edificio.\n" +
                    "**100%** = por defecto del juego."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Almacenamiento del cementerio" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacidad del cementerio** (edificio principal).\n" +
                    "**100%** = por defecto del juego."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Controlar trabajadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Activa para que el **Director funerario** aumente los trabajadores.\n" +
                    "Déjalo OFF si quieres que **ConfigXML** (u otro mod) controle los trabajadores."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Trabajadores máx." },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Escala el **máximo de trabajadores** en edificios de defunción.\n" +
                    "**100%** = por defecto del juego.\n\n" +
                    "**[o_o] Tips**\n" +
                    "  - Se aplica a **edificios nuevos**.\n" +
                    "  - Añadir/quitar una extensión suele forzar refresco."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Reiniciar sliders" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Pone todos los sliders en **100%** (por defecto del juego)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Fúnebre necesario" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Muertos esperando** recogida."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volumen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Totales mensuales** de las stats del juego.\n" +
                     "**Cremación máx/mes** = el panel del juego «Handling/mes».\n" +
                     "Este es el máximo de cuerpos que podrían procesar todos los crematorios por mes."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Recursos" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "Capacidades de **edificios activos** (coches fúnebres, edificios, trabajadores máx).\n\n" +
                    "**Notas:**\n" +
                    "  - incluye coches fúnebres que siguen en mantenimiento (por presupuesto bajo).\n" +
                    "  - no incluye coches fúnebres de edificios desactivados.\n" +
                    "  - el escaneo de estado solo corre en Opciones o al usar un deslizador; no va por frame en la ciudad, así que casi sin impacto en rendimiento :)"
                },


                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Estado no cargado." },
                { "MH_STATUS_NO_CITY_LOADED", "Aún no hay ciudad cargada." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Sin ciudad... ¯\\_(ツ)_/¯ ...Sin stats" },


                { "MH_STATUS_LINE1", "{0} muertos esperando | act. {1}" },
                { "MH_STATUS_LINE2", "{0} muertes/mes | {1} cremación máx/mes | {2} / {3} uso cementerio" },
                { "MH_STATUS_LINE3", "{0} coches fúnebres | {1} / {2} edificios | {3} tumbas libres | {4} trabajadores máx." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nombre mostrado de este mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Versión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Versión actual." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Abre la página de Paradox Mods del autor." },
            };
        }

        public void Unload()
        { }
    }
}
