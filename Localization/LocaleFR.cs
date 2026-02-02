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
                { m_Setting.GetOptionGroupLocaleID(Setting.AutoCleanGrp),   "Nettoyage auto" },
                { m_Setting.GetOptionGroupLocaleID(Setting.SelfManageGrp),  "Gestion autonome" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AdvancedGrp),    "Avancé" },
                { m_Setting.GetOptionGroupLocaleID(Setting.StatusGrp),      "Statut" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGrp),   "Infos du mod" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGrp),  "Liens" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activer le nettoyage magique" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)),
                    "**Supprime automatiquement les citoyens morts** qui attendent un corbillard.\n" +
                    "Désactivez les deux cases pour couper le mod sans le retirer."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FuneralDirector)), "Directeur funéraire" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FuneralDirector)),
                    "Tout gérer soi-même.\n" +
                    "**Valeurs d'échelle :** rythme, flotte, stockage.\n" +
                    "Optionnel : **augmenter les employés** aussi."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProcScalar)), "Vitesse de traitement" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ProcScalar)),
                    "**Vitesse de traitement des établissements** (crémations)\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FleetScalar)), "Taille de la flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FleetScalar)),
                    "**Corbillards maximum** par établissement.\n" +
                    "**100%** = valeur vanilla du jeu.\n" +
                    "**[o_o]** Trop de corbillards peut affecter le trafic selon le taux de décès."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageScalar)), "Stockage du cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageScalar)),
                    "**Capacité de stockage du cimetière** pour le bâtiment principal.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HearseSpeedScalar)), "Vitesse du corbillard" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HearseSpeedScalar)),
                    "**Augmente la vitesse max du corbillard**.\n" +
                    "**100%** = valeur vanilla du jeu.\n" +
                    "<Les limites de vitesse routières s'appliquent toujours>.\n\n" +
                    "Met aussi à l'échelle l'accélération/le freinage (doux) pour éviter des départs/arrêts extrêmes.\n" +
                    "Note : même si la vitesse max du corbillard est augmentée, sa vitesse réelle est grosso modo :\n" +
                    "(max du véhicule, limite de vitesse, vitesse sûre de l'IA, trafic)"

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ControlWorkers)), "Contrôler les employés max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ControlWorkers)),
                "Interrupteur de compatibilité :\n" +
                "**Activer [✓]** pour augmenter le nombre d'employés.\n" +
                "**[o_o]** Laisser OFF si **ConfigXML** ou un autre mod doit gérer les employés des services funéraires."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WorkersScalar)), "Employés max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WorkersScalar)),
                    "**Augmente le maximum d'employés** autorisés.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetGameDefaults)), "Réinitialiser les curseurs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetGameDefaults)),
                    "Remet tous les curseurs à **100%** (valeurs vanilla)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary1)), "Corbillard requis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary1)),
                    "**Citoyens morts en attente** d'un ramassage par corbillard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary2)),
                     "**Totaux mensuels** depuis les stats du jeu.\n" +
                     "**Crémation max/mo** = panneau d'info « Handling/mo » du jeu.\n" +
                     "C'est le nombre maximum de corps pouvant être traités par les crématoriums par mois."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StatusSummary3)), "Ressources" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StatusSummary3)),
                    "**Capacités des bâtiments actifs :** total corbillards, bâtiments, employés max.\n\n" +
                    "**Notes:**\n" +
                    "▪ Corbillard : Actif-non garé / (Total* corbillards)\n" +
                    "▪ *Total corbillard:" +
                    "=== inclut les corbillards en maintenance (ex : budget de service faible), \n" +
                    "=== n'inclut pas les corbillards des bâtiments désactivés.\n" +
                    "▪ Le scan de statut tourne seulement quand Options est ouvert (ou quand un curseur est utilisé); " +
                    "ne tourne pas frame par frame en ville, donc impact perf quasi nul :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Statut non chargé." },
                { "MH_STATUS_NO_CITY_LOADED", "Aucune ville chargée." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Pas de ville... ¯\\_(ツ)_/¯ ...Pas de stats" },

                { "MH_STATUS_LINE1", "{0} morts en attente | mis à jour {1}" },
                { "MH_STATUS_LINE2", "{0} décès/mo | {1} crémation max/mo | {2} / {3} usage cimetière" },
                { "MH_STATUS_LINE3", "{0} / {1} corbillards | {2} / {3} bâtiments | {4} tombes libres | {5} employés max" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutName)), "Nom affiché de ce mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AboutVersion)), "Version actuelle." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Ouvre la page Paradox Mods de l'auteur." },
            };
        }

        public void Unload()
        { }
    }
}
