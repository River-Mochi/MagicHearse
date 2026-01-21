// File: Localization/LocaleFR.cs
// Purpose: French fr-FR locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    public sealed class LocaleFR : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleFR(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Actions" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "À propos" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Nettoyage auto" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Gestion manuelle" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Infos du mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Liens" },

                // Auto Clean
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activer la magie" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "Supprime automatiquement les citoyens morts en attente d’un corbillard.\n" +
                    "Désactivez les deux cases pour désactiver le mod sans le supprimer."
                    },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Directeur funéraire" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Ajuste les valeurs des bâtiments funéraires (vitesse, flotte, stockage)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Vitesse de traitement" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "Multiplicateur de **vitesse de traitement**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Taille de la flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "Multiplicateur du **nombre maximal de corbillards**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Stockage du cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "Augmente le **stockage maximal du cimetière**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Réinitialiser les curseurs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Rétablit tous les curseurs à **100 %** (valeurs par défaut)." },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Version" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Ouvre la page Paradox Mods de l’auteur." },
            };
        }

        public void Unload()
        { }
    }
}
