// Settings/Setting.cs
// Options UI for Magic Hearse Redux (name + version + toggle + links).

namespace MagicHearse
{
    using System;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Unity.Entities;
    using UnityEngine;

    // Saves to: ModsSettings/MagicHearseRedux.coc
    [FileLocation("ModsSettings/MagicHearseRedux")]
    [SettingsUIGroupOrder(MainTabInfo, LinksGroup)]
    // Info group header hidden on purpose; only Links group shows a header via locale.
    public sealed class Setting : ModSetting
    {
        public const string MainTab = "Main";
        public const string MainTabInfo = "Info";
        public const string LinksGroup = "Links";

        private const string UrlParadox =
             "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

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

        // --- main toggle ---

        [SettingsUISection(MainTab, MainTabInfo)]
        public bool EnableMagicHearse { get; set; } = true;

        // --- Links group: Paradox button ---

        [SettingsUIButtonGroup(LinksGroup)]
        [SettingsUIButton]
        [SettingsUISection(MainTab, LinksGroup)]
        public bool OpenParadoxMods
        {
            set
            {
                if (!value)
                {
                    return;
                }

                try
                {
                    Application.OpenURL(UrlParadox);
                }
                catch (Exception)
                {
                    // Silent catch; worst case, the link just does nothing.
                }
            }
        }

        public override void Apply()
        {
            base.Apply();

            // Guard
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }

            world
                .GetOrCreateSystemManaged<MagicHearseSystem>()
                .Enabled = EnableMagicHearse;
        }

        public override void SetDefaults()
        {
            // Default: Magic Hearse enabled when the mod is first installed.
            EnableMagicHearse = true;
        }
    }
}
