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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Nettoyage auto" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Gestion autonome" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Avancé" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Statut" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Infos du mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Liens" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Débogage" },

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
                    "**Vide un cimetière plein** afin qu’il ne reste pas bloqué avec l’icône PLEIN.\n" +
                    "Le nettoyage magique retire la plupart des corps avant l’inhumation — cette option vide tout de même les cimetières **déjà pleins**.\n" +
                    "<[ ] DÉSACTIVÉ par défaut>.\n" +
                    "N’activez cette option que si le mode Nettoyage magique doit également vider les cimetières déjà pleins.\n" +
                    "Une fois vidé, il n’est normalement pas nécessaire de laisser cette option activée tant que le nettoyage magique reste activé."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Directeur funéraire" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Gérez vous-même et optimisez les systèmes funéraires normaux du jeu.\n" +
                    "**Valeurs d’échelle :** rythme, flotte, stockage.\n" +
                    "Optionnel : **augmenter aussi les employés**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Traitement du crématorium" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Vitesse de traitement du crématorium.**\n" +
                    "Des valeurs plus élevées incinèrent les corps et libèrent plus vite le stockage de l’établissement.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Nombre total de corbillards" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Corbillards maximum** par établissement.\n" +
                    "**100%** = valeur vanilla du jeu.\n" +
                    "**[Remarque]** Trop de corbillards peuvent affecter le trafic selon le taux de décès."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Vitesse du corbillard" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Augmente la vitesse de conduite maximale autorisée du corbillard**.\n" +
                    "**100%** = valeur vanilla du jeu.\n" +
                    "<Les limites de vitesse routières s’appliquent toujours>.\n" +
                    "\n" +
                    "Met aussi à l’échelle l’accélération/le freinage (doux) pour éviter que la nouvelle vitesse maximale ne crée des départs/arrêts extrêmes.\n" +
                    "Note : même si la vitesse maximale du corbillard est augmentée, sa vitesse réelle dépend de :\n" +
                    "la vitesse maximale autorisée du véhicule, la limite de la route, la vitesse sûre de l’IA du jeu (virages, routes endommagées) et le trafic."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Délai de l’alerte décès (min)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "Il s’agit du temps total dont dispose un corbillard pour atteindre un bâtiment avant l’apparition des icônes de problème **en attente d’un corbillard**.\n" +
                    "**3 minutes** est proche de la valeur par défaut du jeu d’environ 2,5 minutes de simulation.\n" +
                    "Vous pouvez augmenter cette valeur pour laisser aux corbillards un délai plus raisonnable pour terminer le trajet avant l’apparition de l’icône de décès.\n" +
                    "Note :\n" +
                    "- <Suggestion : 10 minutes>. Essayez davantage dans les villes très congestionnées.\n" +
                    "- Consultez le rapport Statut en bas pour voir combien de cas sont en retard.\n" +
                    "- Les icônes déjà visibles ne sont pas masquées lorsque ce délai est augmenté pour la première fois ; elles restent jusqu’à ce qu’un corbillard intervienne ou que le bâtiment soit démoli.\n" +
                    "- Laissez les interventions actuelles se terminer naturellement ou utilisez une fois la case <Nettoyage magique [x]> pour repartir rapidement avec les nouveaux horaires."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Stockage du cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Capacité de stockage du cimetière** pour le bâtiment principal.\n" +
                    "Une capacité supérieure permet à un cimetière plein d’accepter de nouveau des enlèvements.\n" +
                    "Elle n’envoie pas plus de corbillards, sauf si le manque de place bloquait l’établissement.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Réinitialisation auto. du cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Vide un cimetière plein** afin qu’il ne reste pas bloqué par l’icône PLEIN au-dessus du bâtiment.\n" +
                    "Il n’est plus nécessaire de supprimer et reconstruire les cimetières pleins.\n" +
                    "Désactivez cette option pour utiliser à la place la **Libération progressive des tombes**.\n" +
                    "<[ ✓ ] ACTIVÉ par défaut>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Libération progressive des tombes" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Libère progressivement les tombes occupées du cimetière.**\n" +
                    "Des valeurs plus élevées rendent les emplacements disponibles plus vite que dans le jeu vanilla.\n" +
                    "Si les cimetières restent trop souvent pleins à 500%,\n" +
                    "activez plutôt **[Réinitialisation auto. du cimetière]**.\n" +
                    "**100%** = taux par défaut du jeu pour la réutilisation des tombes."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Ajuster les employés" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Interrupteur de compatibilité :\n" +
                    "**Activer [✓]** pour augmenter le nombre d’employés.\n" +
                    "**[o_o]** Laissez DÉSACTIVÉ si vous souhaitez que **ConfigXML** ou un autre mod gère les employés des services funéraires."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Employés maximum" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Augmente le nombre maximal d’employés** autorisé.\n" +
                    "**100%** = valeur vanilla du jeu."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Réinitialiser les curseurs" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Règle les curseurs de pourcentage sur **100%** et le délai de l’alerte décès sur **3 minutes**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Corbillard requis" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**En attente** = tous les citoyens décédés encore à l’extérieur et en attente d’enlèvement.\n" +
                    "**En retard** = citoyens en attente dont le délai de notification sélectionné a expiré.\n" +
                    " - S’il y a beaucoup de cas en retard, augmentez le délai de l’alerte décès."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Volume" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Totaux mensuels** des statistiques du jeu.\n" +
                    "**Max./mois** = traitement des crématoriums plus rotation des cimetières à l’efficacité actuelle.\n" +
                    "C’est le nombre maximal de corps que tous les établissements funéraires actifs pourraient traiter par mois."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Ressources" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Capacités des bâtiments actifs :** total de corbillards, bâtiments, employés max.\n" +
                    "\n" +
                    "**Remarques :**\n" +
                    "▪ Corbillard : Actif-non garé / (Total* corbillards)\n" +
                    "▪ *Total des corbillards :\n" +
                    "== inclut les corbillards en maintenance (p. ex. budget de service faible), \n" +
                    "== n’inclut pas les corbillards des bâtiments désactivés.\n" +
                    "▪ L’analyse du statut ne s’exécute que lorsque les Options sont ouvertes (ou qu’un curseur est utilisé) ; elle ne s’exécute pas à chaque image en ville et n’a donc pratiquement aucun impact sur les performances :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Statut non chargé." },
                { "MH_STATUS_NO_CITY_LOADED", "Aucune ville chargée." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Pas de ville... ¯\\_(ツ)_/¯ ...Pas de stats" },

                { "MH_STATUS_LINE1_V2", "{0} en attente | {1} en retard | {2} décès/mois" },
                { "MH_STATUS_LINE2_V2", "{0} max./mois" },
                { "MH_STATUS_LINE3", "{0} / {1} corbillards | {2} / {3} bâtiments | {4} employés max" },
                { "MH_STATUS_UPDATED", "mis à jour {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "suggestion actuelle : traitement des crématoriums à ~{0}%" },
                { "MH_STATUS_PROCESSING_MORE", "suggestion actuelle : traitement des crématoriums à 500% + plus d’établissements actifs" },
                { "MH_STATUS_PROCESSING_NONE", "suggestion : activez/ajoutez des crématoriums" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Cimetière" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Affiche les **tombes utilisées**, les cimetières actifs et les réinitialisations de cimetières pleins pendant cette session.\n" +
                    "Le statut s’efface au redémarrage ou lors d’un changement de ville."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} tombes utilisées | {2} établissements | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} réinitialisation" },
                { "MH_STATUS_RESET_PLURAL", "{0} réinitialisations" },
                { "MH_STATUS_CEMETERY_NONE", "aucune pendant cette session" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} de plus" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Nom affiché de ce mod." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Version actuelle." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Ouvre la page Paradox Mods de l’auteur." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Rapport du journal" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "Écrit un rapport détaillé des services funéraires et des problèmes probables dans MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Ouvrir le journal" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Ouvre **Logs/MagicHearse.log** s’il existe.\n" +
                    "Si le fichier n’existe pas encore, ouvre le dossier Logs à la place."
                },
            };
        }

        public void Unload()
        { }
    }
}
