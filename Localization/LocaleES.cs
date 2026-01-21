// File: Localization/LocaleES.cs
// Purpose: Spanish es-ES locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocaleES : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleES(Setting setting)
        {
            m_Setting = setting;
        }

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
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Info del mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Enlaces" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activar magia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "Elimina automáticamente ciudadanos muertos que esperan un coche fúnebre.\n" +
                    "Desactiva ambas casillas para apagar el mod sin eliminarlo."
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Director funerario" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Escala valores de edificios funerarios (velocidad, flota, almacenamiento)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Velocidad de proceso" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "Multiplicador de **velocidad de procesamiento**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Tamaño de flota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "Multiplicador de **máximo de coches fúnebres**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Almacenamiento del cementerio" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "Aumenta el **almacenamiento máximo del cementerio**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Restablecer controles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Restablece todos los controles a **100 %** (valores originales)." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Versión" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Abre la página del autor en Paradox Mods." },
            };
        }

        public void Unload()
        { }
    }
}
