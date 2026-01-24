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
                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Actions" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), "À propos" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp), "Nettoyage auto" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp), "Gestion" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp), "Statut" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp), "Infos du mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp), "Liens" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activer la magie" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Supprime automatiquement les morts**\n" +
                    "qui attendent un corbillard.\n" +
                    "Désactivez les deux cases pour couper le mod sans le retirer."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Directeur funéraire" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Ajuste les valeurs des **bâtiments** (vitesse, flotte, stockage).\n" +
                    "Optionnel : **augmenter les employés**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Taux de traitement" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Vitesse de traitement** (crémations)\n" +
                    "**100%** = valeur vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Taille de la flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Nombre max de corbillards** par bâtiment.\n" +
                    "**100%** = valeur vanilla."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Stockage du cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacité du cimetière** (bâtiment principal).\n" +
                    "**100%** = valeur vanilla."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Contrôler les employés max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                    "Activez pour que le **Directeur funéraire** augmente le nombre d’employés.\n" +
                    "Laissez OFF si vous voulez que **ConfigXML** (ou un autre mod) gère les employés."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Employés max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "Ajuste le **maximum d’employés** des bâtiments de décès.\n" +
                    "**100%** = valeur vanilla.\n\n" +
                    "**[o_o] Astuces**\n" +
                    "  - S’applique aux **nouveaux bâtiments**.\n" +
                    "  - Ajouter/supprimer une extension force souvent une mise à jour."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Réinitialiser" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Remet tous les curseurs à **100%** (valeurs vanilla)." },

                // STATUS fields (keep labels SHORT; left column is narrow!

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Corbillard requis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Morts en attente** d’un corbillard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Totaux mensuels** des stats du jeu.\n" +
                     "**Crémation max/mois** = l’info « Handling/mois » du jeu.\n" +
                     "C’est le maximum de corps que tous les crématoriums pourraient traiter par mois."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Ressources" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "Capacités des **bâtiments actifs** (corbillards, bâtiments, ouvriers max).\n\n" +
                    "**Notes :**\n" +
                    "  - inclut aussi les corbillards encore en maintenance (budget trop bas).\n" +
                    "  - n’inclut pas les corbillards des bâtiments désactivés.\n" +
                    "  - le scan de statut tourne seulement dans le menu Options ou via un slider ; pas par frame en ville, donc impact perf quasi nul :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Statut non chargé." },
                { "MH_STATUS_NO_CITY_LOADED", "Aucune ville chargée." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Pas de ville... ¯\\_(ツ)_/¯...Pas de stats" },

                { "MH_STATUS_LINE1", "{0} morts en attente | maj {1}" },
                { "MH_STATUS_LINE2", "{0} décès/mois | {1} crémation max/mois | {2} / {3} usage cimetière" },
                { "MH_STATUS_LINE3", "{0} corbillards | {1} / {2} bâtiments | {3} tombes libres | {4} employés max" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nom affiché de ce mod." },
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
