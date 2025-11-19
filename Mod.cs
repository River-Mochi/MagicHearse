// Mod.cs
// Entrypoint: registers settings, locales, and the ECS system.

namespace MagicHearse
{
    using System.Reflection;
    using Colossal.IO.AssetDatabase;
    using Colossal.Localization;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using Game.UI;

    public sealed class Mod : IMod
    {
        // single source of truth for name + version
        public const string ModName = "Magic Hearse Redux";
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
            LogManager.GetLogger("MagicHearseRedux").SetShowsErrorsInUI(
#if DEBUG
                true
#else
                false
#endif
            );

        public static Setting? Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            // metadata banner (once)
            if (!s_BannerLogged)
            {
                s_BannerLogged = true;
                Log.Info($"{ModName} v{ModVersion} OnLoad");
            }

            // settings first
            var setting = new Setting(this);
            Settings = setting;

            // Register locales here
            LocalizationManager? lm = GameManager.instance?.localizationManager;
            if (lm != null)
            {
                lm.AddSource("en-US", new LocaleEN(setting));
                lm.AddSource("fr-FR", new LocaleFR(setting));
                lm.AddSource("es-ES", new LocaleES(setting));
                lm.AddSource("de-DE", new LocaleDE(setting));
                lm.AddSource("it-IT", new LocaleIT(setting));
                lm.AddSource("ja-JP", new LocaleJA(setting));
                lm.AddSource("ko-KR", new LocaleKO(setting));
                lm.AddSource("zh-HANS", new LocaleZH(setting));

            }
            else
            {
                Log.Warn("LocalizationManager not found; settings UI texts may be missing.");
            }

            // load saved settings (file name is in Setting.cs [FileLocation])
            AssetDatabase.global.LoadSettings("MagicHearseRedux", setting, new Setting(this));

            // show in Options -> Mods
            setting.RegisterInOptionsUI();

            // run the cleanup system in simulation
            updateSystem.UpdateAt<MagicHearseSystem>(SystemUpdatePhase.GameSimulation);

            // apply current toggle value
            updateSystem.World
                .GetOrCreateSystemManaged<MagicHearseSystem>()
                .Enabled = setting.EnableMagicHearse;
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
    }
}
