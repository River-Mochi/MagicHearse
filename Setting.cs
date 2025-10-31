// Setting.cs
// Options UI for Magic Hearse Redux (name + version + toggle) with hidden group header.

namespace MagicHearse
{
    using System.Collections.Generic;
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Unity.Entities;

    // Saves to: ModsSettings/MagicHearseRedux.coc
    [FileLocation("ModsSettings/MagicHearseRedux")]
    [SettingsUIGroupOrder(MainTabInfo)]
    // NOTE: intentionally NO [SettingsUIShowGroupName(...)] so the header bar is hidden
    public sealed class Setting : ModSetting
    {
        public const string MainTab = "Main";
        public const string MainTabInfo = "Info";

        public static Setting? Instance;

        public Setting(IMod mod)
            : base(mod)
        {
            Instance = this;
        }

        // --- read-only display from Mod.cs ---

        [SettingsUISection(MainTab, MainTabInfo)]
        public string ModNameDisplay => Mod.ModName;

        [SettingsUISection(MainTab, MainTabInfo)]
        public string ModVersionDisplay => Mod.ModVersion;

        // --- actual toggle ---

        [SettingsUISection(MainTab, MainTabInfo)]
        public bool EnableMagicHearse { get; set; } = true;

        public override void Apply()
        {
            base.Apply();

            World.DefaultGameObjectInjectionWorld
                .GetOrCreateSystemManaged<MagicHearseSystem>()
                .Enabled = EnableMagicHearse;
        }

        public override void SetDefaults()
        {
            EnableMagicHearse = true;
        }
    }

    // ============================================================
    // en-US
    // ============================================================
    public sealed class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                // panel name in Options -> Modding
                { m_Setting.GetSettingsLocaleID(), "Magic Hearse Redux" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Display name of this mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Current build of Magic Hearse Redux." },

                // you wanted this short:
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Enable Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Auto removes dead citizens that are waiting for a hearse." },
            };
        }

        public void Unload()
        {
        }
    }

    // ============================================================
    // fr-FR
    // ============================================================
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
                { m_Setting.GetSettingsLocaleID(), "Magic Hearse Redux" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nom de ce mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Version actuelle de Magic Hearse Redux." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activer Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Supprime automatiquement les citoyens décédés en attente d’un corbillard." },
            };
        }

        public void Unload()
        {
        }
    }

    // ============================================================
    // es-ES
    // ============================================================
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
                { m_Setting.GetSettingsLocaleID(), "Magic Hearse Redux" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nombre de este mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Versión actual de Magic Hearse Redux." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activar Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Elimina automáticamente a los ciudadanos muertos que esperan un coche fúnebre." },
            };
        }

        public void Unload()
        {
        }
    }

    // ============================================================
    // de-DE
    // ============================================================
    public sealed class LocaleDE : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleDE(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Magic Hearse Redux" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Name dieses Mods." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Aktuelle Version von Magic Hearse Redux." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Magic aktivieren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Entfernt automatisch verstorbene Bürger, die auf einen Leichenwagen warten." },
            };
        }

        public void Unload()
        {
        }
    }

    // ============================================================
    // zh-HANS
    // ============================================================
    public sealed class LocaleZH : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleZH(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Magic Hearse Redux 魔法灵车" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "模组" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "此模组的名称。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Magic Hearse Redux 当前版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "启用 Magic" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "自动清理等待灵车的死亡市民。" },
            };
        }

        public void Unload()
        {
        }
    }
}
