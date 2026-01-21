// Mod.cs
// Entrypoint: registers settings, locales, ECS system

namespace MagicHearse
{
    using Colossal;                       // IDictionarySource
    using Colossal.IO.AssetDatabase;      // AssetDatabase
    using Colossal.Localization;          // LocalizationManager
    using Colossal.Logging;               // ILog, LogManager
    using Game;                           // UpdateSystem, SystemUpdatePhase
    using Game.Modding;                   // IMod
    using Game.SceneFlow;                 // GameManager
    using System;                         // Exception
    using System.Reflection;              // Assembly version number

    public sealed class Mod : IMod
    {
        // single source of truth for name + version
        public const string ModName = "Magic Hearse";
        public const string ModId = "MagicHearse";
        public const string ModTag = "[MH]";

        /// <summary>
        /// Read Version from .csproj (3-part).
        /// </summary>
        public static readonly string ModVersion =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";

        private static bool s_BannerLogged;

        // ----- Logger & public properties ----

        public static readonly ILog Log =
            LogManager.GetLogger("MagicHearse").SetShowsErrorsInUI(
#if DEBUG
                true
#else
                false
#endif
            );

        public static Setting? Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            // One-time banner.
            if (!s_BannerLogged)
            {
                s_BannerLogged = true;
                Log.Info($"{ModName} v{ModVersion} OnLoad");
            }

            GameManager? gameManager = GameManager.instance;
            if (gameManager == null)
            {
                Log.Error("GameManager.instance is null in Mod.OnLoad.");
                return;
            }

            // settings first
            var setting = new Setting(this);
            Settings = setting;

            // Register locales via helper (safer wrapper around LocalizationManager.AddSource)
            AddLocaleSource("en-US", new LocaleEN(setting));
            AddLocaleSource("fr-FR", new LocaleFR(setting));
            AddLocaleSource("es-ES", new LocaleES(setting));
            AddLocaleSource("de-DE", new LocaleDE(setting));
            AddLocaleSource("it-IT", new LocaleIT(setting));
            AddLocaleSource("ja-JP", new LocaleJA(setting));
            AddLocaleSource("ko-KR", new LocaleKO(setting));
            AddLocaleSource("zh-HANS", new LocaleZH(setting));
            AddLocaleSource("pl-PL", new LocalePL(setting));
            AddLocaleSource("pt-BR", new LocalePT_BR(setting));
            AddLocaleSource("zh-HANT", new LocaleZH_HANT(setting));

            // load saved settings (file name is in Setting.cs [FileLocation])
            AssetDatabase.global.LoadSettings("MagicHearse", setting, new Setting(this));

            // Show in OptionsUI
            setting.RegisterInOptionsUI();

            // Prefab edits must run after PrefabUpdate init work.
            updateSystem.UpdateAfter<FuneralDirectorSystem>(SystemUpdatePhase.PrefabUpdate);

            // Magic cleanup stays in simulation.
            updateSystem.UpdateAt<MagicHearseSystem>(SystemUpdatePhase.GameSimulation);

            updateSystem.World.GetOrCreateSystemManaged<MagicHearseSystem>().Enabled = setting.EnableMagicHearse;

            if (setting.FuneralDirector)
            {
                updateSystem.World.GetOrCreateSystemManaged<FuneralDirectorSystem>()
                    .RequestReapplyFromSettings();
            }


        }

        public void OnDispose()
        {
            Log.Info("OnDispose");

            if (Settings != null)
            {
                Settings.UnregisterInOptionsUI();
                Settings = null;
            }
        }

        // --------------------------------------------------------------------
        // Localization helper
        // --------------------------------------------------------------------

        /// <summary>
        /// Wrapper around LocalizationManager.AddSource that catches exceptions
        /// so localization issues can't break mod loading.
        /// </summary>
        private static void AddLocaleSource(string localeId, IDictionarySource source)
        {
            if (string.IsNullOrEmpty(localeId))
            {
                return;
            }

            LocalizationManager? lm = GameManager.instance?.localizationManager;
            if (lm == null)
            {
                Log.Warn($"AddLocaleSource: No LocalizationManager; cannot add source for '{localeId}'.");
                return;
            }

            try
            {
                lm.AddSource(localeId, source);
            }
            catch (Exception ex)
            {
                Log.Warn(
                    $"AddLocaleSource: AddSource for '{localeId}' failed: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
