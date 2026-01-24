// File: Mod.cs
// Entrypoint: registers settings, locales, ECS system

namespace MagicHearse
{
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Colossal.Localization;
    using Colossal.Logging;
    using CS2HonuShared;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using System;
    using System.Reflection;

    public sealed class Mod : IMod
    {
        public const string ModName = "Magic Hearse";
        public const string ModId = "MagicHearse";
        public const string ModTag = "[MH]";

        public static readonly string ModVersion =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";

        private static bool s_BannerLogged;

#if DEBUG
        private const string buildTag = "[DEBUG]";
#else
        private const string buildTag = "[RELEASE]";
#endif

        public static readonly ILog s_Log =
            LogManager.GetLogger(ModId).SetShowsErrorsInUI(
#if DEBUG
                true
#else
                false
#endif
            );

        public static Setting? Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            if (!s_BannerLogged)      
            {
                s_BannerLogged = true;  // makes it a one-time log banner
                LogSafe(() => $"{ModName} v{ModVersion} OnLoad {buildTag}");
            }

            if (GameManager.instance == null)   // Is game running (not a city check)
            {
                WarnSafe(() => "GameManager.instance is null in Mod.OnLoad.");
                return;
            }

            var setting = new Setting(this);
            Settings = setting;

            // Locales should be best-effort; never crash mod load.
            AddLocaleSource("en-US", new LocaleEN(setting));
            AddLocaleSource("fr-FR", new LocaleFR(setting));
            AddLocaleSource("es-ES", new LocaleES(setting));
            AddLocaleSource("de-DE", new LocaleDE(setting));
            AddLocaleSource("it-IT", new LocaleIT(setting));
            AddLocaleSource("ja-JP", new LocaleJA(setting));
            AddLocaleSource("ko-KR", new LocaleKO(setting));
            AddLocaleSource("zh-HANS", new LocaleZH_CN(setting));
            AddLocaleSource("pl-PL", new LocalePL(setting));
            AddLocaleSource("pt-BR", new LocalePT_BR(setting));
            AddLocaleSource("zh-HANT", new LocaleZH_HANT(setting));

            // Settings + Options UI (best-effort; defaults are fine).
            try
            {
                AssetDatabase.global.LoadSettings(ModId, setting, new Setting(this));
                setting.RegisterInOptionsUI();
            }
            catch (Exception ex)
            {
                WarnSafe(() => $"Settings/UI init failed: {ex.GetType().Name}: {ex.Message}");
            }

            // System scheduling/init (best-effort).
            try
            {
                updateSystem.UpdateAfter<FuneralDirectorSystem>(SystemUpdatePhase.PrefabUpdate);
                updateSystem.UpdateAt<MagicHearseSystem>(SystemUpdatePhase.GameSimulation);

                updateSystem.World.GetOrCreateSystemManaged<MagicHearseSystem>().Enabled = setting.EnableMagicHearse;

                if (setting.FuneralDirector)
                {
                    updateSystem.World.GetOrCreateSystemManaged<FuneralDirectorSystem>()
                        .RequestReapplyFromSettings();
                }
            }
            catch (Exception ex)
            {
                WarnSafe(() => $"System scheduling/init failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        public void OnDispose()
        {
            LogSafe(() => "OnDispose");

            if (Settings != null)
            {
                try { Settings.UnregisterInOptionsUI(); }
                catch (Exception ex) { WarnSafe(() => $"UnregisterInOptionsUI failed: {ex.GetType().Name}: {ex.Message}"); }

                Settings = null;
            }
        }

        // LogUtil Helpers
        public static void LogSafe(Func<string> messageFactory) => LogUtil.TryLog(s_Log, Level.Info, messageFactory);
        public static void WarnSafe(Func<string> messageFactory) => LogUtil.TryLog(s_Log, Level.Warn, messageFactory);
        public static void WarnOnce(string key, Func<string> messageFactory) => LogUtil.WarnOnce(s_Log, key, messageFactory);

        private static void AddLocaleSource(string localeId, IDictionarySource source)
        {
            if (string.IsNullOrEmpty(localeId))
            {
                return;
            }

            LocalizationManager? lm = GameManager.instance?.localizationManager;
            if (lm == null)
            {
                WarnSafe(() => $"AddLocaleSource: No LocalizationManager; cannot add source for '{localeId}'.");
                return;
            }

            try { lm.AddSource(localeId, source); }
            catch (Exception ex)
            {
                WarnSafe(() => $"AddLocaleSource: AddSource for '{localeId}' failed: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
