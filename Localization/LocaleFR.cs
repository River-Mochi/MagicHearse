// File: Localization/LocaleFR.cs
// French fr-FR locale for Magic Hearse.

namespace MagicHearse
{
    using Colossal; // IDictionarySource, IDictionaryEntryError
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    /// <summary>
    /// French localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleFR : IDictionarySource
    {
        private readonly Setting m_Setting;

        /// <summary>
        /// Constructs the French locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleFR(Setting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all French localization entries for this mod.</summary>
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
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Statut de la ville" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Infos du mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Liens" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activer la magie" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Supprime automatiquement** les citoyens morts en attente d’un corbillard.\n" +
                    "Désactive les deux cases pour couper le mod sans le retirer."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Directeur funéraire" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Ajuste les **valeurs des bâtiments** (taux, flotte, stockage, employés)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Taux de traitement" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Vitesse de traitement** des bâtiments.\n" +
                    "**100%** = valeur vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Taille de flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Nombre max de corbillards** par bâtiment.\n" +
                    "Astuce : trop peut aussi augmenter le trafic.\n" +
                    "**100%** = valeur vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Stockage du cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacité de stockage** du cimetière (bâtiment principal).\n" +
                    "**100%** = valeur vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Employés max (voir notes)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Ajuste les **employés maximum** des services funéraires.\n" +
                     "**100%** = valeur vanilla.\n" +
                    "**Astuces :**\n" +
                    "  - s’applique aux nouveaux bâtiments (après le changement).\n" +
                    "  - astuce : supprimer/ajouter une extension met aussi à jour tout de suite.\n\n" +
                    "Note dev : flotte/stockage se mettent à jour instantanément (stats du prefab). " +
                    "Employés max est différent (calculé par le jeu).\n" +
                    "Le plus sûr est de remplacer le bâtiment ou l’extension pour forcer un refresh, " +
                    "plutôt que de modifier un composant runtime."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Réinitialiser" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Remet tous les sliders à **100%** (valeurs vanilla)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "Citoyens morts en attente d’un corbillard." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                    "Totaux mensuels des stats du jeu.\n" +
                    "Garde **peut gérer** au-dessus de **décès/mois**.\n" +
                    "...ou active la magie :)"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), string.Empty },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "Capacités **actives** (non désactivées) : corbillards, bâtiments, employés max.\n\n" +
                    "**Notes :**\n" +
                    "  - inclut les corbillards en maintenance\n" +
                    "  - n’inclut pas les corbillards des bâtiments désactivés.\n" +
                    "  - le slider employés max s’applique aux <nouveaux bâtiments>\n" +
                    "  - le scan ne se fait que dans le menu Options et ne tourne pas par frame en simulation;\n" +
                    "le mod est pensé pour préserver les performances.\n"
                },

                // Status text templates 
                { "MH_STATUS_NOT_LOADED", "Statut non chargé." },
                { "MH_STATUS_NO_CITY_LOADED", "Aucune ville chargée pour l’instant." },

                { "MH_STATUS_LINE1", "{0} morts en attente | {1} mis à jour" },
                { "MH_STATUS_LINE2", "{0} décès/mois | {1} peut gérer" },
                { "MH_STATUS_LINE3", "{0} corbillards | {1} / {2} bâtiments | {3} / {4} utilisation cimetière | {5} employés max" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                  { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nom affiché du mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Version" },
                  { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Version actuelle." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Ouvre la page Paradox Mods de l’auteur." },
            };
        }

        public void Unload()
        { }
    }
}
