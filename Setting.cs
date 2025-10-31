// Setting.cs
// Options UI + all tiny locales for Magic Hearse Redux.

namespace MagicHearse
{
    using System.Collections.Generic;
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Unity.Entities;

    // Saves to:
    // ...\ModsSettings\MagicHearseRedux.coc
    [FileLocation("ModsSettings/MagicHearseRedux")]
    public sealed class Setting : ModSetting
    {
        // set in ctor
        public static Setting? Instance;

        public Setting(IMod mod)
            : base(mod)
        {
            Instance = this;
        }

        // main toggle
        public bool EnableMagicHearse { get; set; } = true;

        // info-only, shown under the checkbox
        public string ModName { get; set; } = "Magic Hearse Redux";

        public string ModVersion { get; set; } = "1.3.2";

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
            ModName = "Magic Hearse Redux";
            ModVersion = "1.3.2";
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
                { m_Setting.GetSettingsLocaleID(), "Magic Hearse Redux" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Enable Magic Hearse Redux" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Auto removes dead citizens that are waiting for a hearse." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModName)), "Display name of this mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersion)), "Current build of Magic Hearse Redux." },
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

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activer Magic Hearse Redux" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Supprime automatiquement les citoyens décédés en attente d’un corbillard." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModName)), "Nom du mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersion)), "Version actuelle de Magic Hearse Redux." },
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

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Activar Magic Hearse Redux" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Elimina automáticamente a los ciudadanos muertos que esperan un coche fúnebre." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModName)), "Nombre de este mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersion)), "Versión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersion)), "Versión actual de Magic Hearse Redux." },
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

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "Magic Hearse aktivieren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "Entfernt automatisch verstorbene Bürger, die auf einen Leichenwagen warten." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModName)), "Name dieses Mods." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersion)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersion)), "Aktuelle Version von Magic Hearse Redux." },
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

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableMagicHearse)), "启用魔法灵车" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableMagicHearse)), "自动清理等待灵车的死亡市民。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModName)), "模组" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModName)), "此模组的名称。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersion)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersion)), "Magic Hearse Redux 当前版本。" },
            };
        }

        public void Unload()
        {
        }
    }
}
