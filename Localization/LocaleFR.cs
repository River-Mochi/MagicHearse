// <copyright file="LocaleFR.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleFR.cs
// French fr-FR locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// French localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleFR : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the French locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleFR(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all French localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Actions" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "À propos" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "Nettoyage auto" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "Gestion autonome" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "Avancé" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "Statut" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "Infos du mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "Liens" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "Débogage" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Activer le nettoyage magique" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Supprime automatiquement les corps qui nécessitent un transport (corbillard).\n" +
                    "Le nettoyage magique et la gestion autonome sont incompatibles ; choisissez l’un ou l’autre.\n" +
                    "Décochez toutes les cases pour désactiver le mod sans le supprimer.\n" +
                    "Note technique : IsDead = true et WaitingForHearse = true sont requis."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Réinitialiser le cimetière plein" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Vide tout cimetière plein** afin qu’il ne reste pas bloqué avec l’icône PLEIN.\n" +
                    "Le nettoyage magique retire la plupart des corps avant l’inhumation — cette option vide tout de même les cimetières **déjà pleins**.\n" +
                    "<[ ] DÉSACTIVÉ par défaut>.\n" +
                    "N’activez cette option que si le mode Nettoyage magique doit également vider les cimetières déjà pleins.\n" +
                    "Une fois vidés, il n’est normalement pas nécessaire de laisser cette option activée tant que le nettoyage magique reste activé."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Directeur funéraire" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Tout gérer soi-même.\n" +
                    "**Valeurs d'échelle :** rythme, flotte, stockage.\n" +
                    "Optionnel : **augmenter les employés** aussi."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Traitement du crématorium" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Vitesse de traitement du crématorium.**\n" +
                    "Des valeurs plus élevées incinèrent les corps et libèrent plus vite le stockage de l’établissement.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Taille de la flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Corbillards maximum** par établissement.\n" +
                    "**100%** = valeur vanilla du jeu.\n" +
                    "**[Remarque]** Trop de corbillards peut affecter le trafic selon le taux de décès."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Stockage du cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacité de stockage du cimetière** pour le bâtiment principal.\n" +
                    "Une capacité supérieure permet à un cimetière plein d’accepter de nouveau des enlèvements.\n" +
                    "Elle n’envoie pas plus de corbillards, sauf si le manque de place bloquait l’établissement.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Réinitialiser le cimetière plein" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Vide un cimetière** lorsqu’il est plein afin qu’il ne reste pas bloqué par l’icône PLEIN au-dessus du bâtiment.\n" +
                    "Il n’est plus nécessaire de supprimer et reconstruire les cimetières pleins.\n" +
                    "Désactivez cette option pour utiliser à la place la **Libération progressive des tombes**.\n" +
                    "<[ ✓ ] Activé par défaut>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Libération progressive des tombes" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Libère progressivement les tombes occupées du cimetière.**\n" +
                    "Si les cimetières affichent encore trop souvent l’icône PLEIN, augmentez ce curseur.\n" +
                    "Des valeurs plus élevées rendent les tombes disponibles plus vite que dans le jeu vanilla.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Vitesse du corbillard" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Augmente la vitesse max du corbillard**.\n" +
                    "**100%** = valeur vanilla du jeu.\n" +
                    "<Les limites de vitesse routières s'appliquent toujours>.\n\n" +
                    "Met aussi à l'échelle l'accélération/le freinage (doux) pour éviter des départs/arrêts extrêmes.\n" +
                    "Note : même si la vitesse maximale du corbillard est augmentée, sa vitesse réelle dépend de :\n" +
                    "la vitesse maximale autorisée du véhicule, la limite de la route, la vitesse sûre de l’IA du jeu (virages, routes endommagées) et le trafic."

                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Contrôler les employés max" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                "Interrupteur de compatibilité :\n" +
                "**Activer [✓]** pour augmenter le nombre d'employés.\n" +
                "**[o_o]** Laisser OFF si **ConfigXML** ou un autre mod doit gérer les employés des services funéraires."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Employés max" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Augmente le maximum d'employés** autorisés.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Réinitialiser les curseurs" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "Remet tous les curseurs à **100%** (valeurs vanilla)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Corbillard requis" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Citoyens morts en attente** d'un ramassage par corbillard."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "**Totaux mensuels** depuis les stats du jeu.\n" +
                     "**Traitement max./mois** = traitement des crématoriums plus renouvellement des cimetières à l’efficacité actuelle.\n" +
                     "C’est le nombre maximal de corps que tous les établissements funéraires actifs peuvent gérer par mois."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Ressources" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacités des bâtiments actifs :** total corbillards, bâtiments, employés max.\n\n" +
                    "**Remarques :**\n" +
                    "▪ Corbillard : Actif-non garé / (Total* corbillards)\n" +
                    "▪ *Total des corbillards :\n" +
                    "== inclut les corbillards en maintenance (p. ex. budget de service faible), \n" +
                    "== n’inclut pas les corbillards des bâtiments désactivés.\n" +
                    "▪ L’analyse du statut ne s’exécute que lorsque les Options sont ouvertes (ou qu’un curseur est utilisé) ; " +
                    "elle ne s’exécute pas à chaque image en ville et n’a donc pratiquement aucun impact sur les performances :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Statut non chargé." },
                { "MH_STATUS_NO_CITY_LOADED", "Aucune ville chargée." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Pas de ville... ¯\\_(ツ)_/¯ ...Pas de stats" },

                { "MH_STATUS_LINE1", "{0} en attente | {1} décès/mo | mis à jour {2}" },
                { "MH_STATUS_LINE2", "{0} traitement max./mois | {1}/{2} tombes utilisées" },
                { "MH_STATUS_LINE3", "{0} / {1} corbillards | {2} / {3} bâtiments | {4} employés max" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "Suggestion actuelle : traitement à ~{0} %" },
                { "MH_STATUS_PROCESSING_MORE", "Suggestion actuelle : traitement à 500 % + plus d'établissements actifs" },
                { "MH_STATUS_PROCESSING_NONE", "Suggestion : activez/ajoutez des crématoriums" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Cimetières vidés automatiquement pendant cette session** par Réinitialiser le cimetière plein.\n" +
                    "Affiche le nombre total de réinitialisations et le nombre de cimetières distincts.\n" +
                    "S’efface au redémarrage ou lors d’un changement de ville."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "Quels cimetières ont été vidés et combien de fois chacun (nom × nombre)." },

                { "MH_STATUS_LINE4", "réinitialisations : {0} · cimetières : {1}" },
                { "MH_STATUS_CEMETERY_NONE", "aucun pendant cette session" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} de plus" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nom affiché de ce mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Version actuelle." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Ouvre la page Paradox Mods de l'auteur." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Rapport du journal" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "Écrit un rapport détaillé des services funéraires et des problèmes probables dans MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Ouvrir le journal" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Ouvre **Logs/MagicHearse.log** s’il existe.\n" +
                    "Si le fichier n’existe pas encore, ouvre le dossier Logs à la place." },
            };
        }

        public void Unload()
        { }
    }
}
