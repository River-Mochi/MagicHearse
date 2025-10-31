// Mod.cs
// Entrypoint: registers settings, locales, and the ECS system.

namespace MagicHearse
{
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;

    public sealed class Mod : IMod
    {
        // single source of truth for name + version
        public const string ModName = "Magic Hearse Redux";
        public const string ModVersion = "1.3.2"; // bump when publishing

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
            Log.Info($"{ModName} v{ModVersion} OnLoad");

            // settings first
            var setting = new Setting(this);
            Settings = setting;

            // register locales
            var lm = GameManager.instance?.localizationManager;
            if (lm != null)
            {
                lm.AddSource("en-US", new LocaleEN(setting));
                lm.AddSource("fr-FR", new LocaleFR(setting));
                lm.AddSource("es-ES", new LocaleES(setting));
                lm.AddSource("de-DE", new LocaleDE(setting));
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
