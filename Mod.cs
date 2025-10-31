// Mod.cs
// Entrypoint for Magic Hearse Redux; registers settings, locales, and the ECS cleanup system.

namespace MagicHearse
{
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;

    public sealed class Mod : IMod
    {
        public const string ModName = "Magic Hearse Redux";
        public const string ModVersion = "1.3.1";

        public static readonly ILog Log =
            LogManager.GetLogger("MagicHearseRedux.Mod").SetShowsErrorsInUI(
#if DEBUG
                true
#else
                false
#endif
            );

        // set in OnLoad, cleared in OnDispose
        public static Setting? Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            Log.Info($"{ModName} v{ModVersion} OnLoad");

            // create settings first
            var setting = new Setting(this);
            Settings = setting;

            // register locales (all in Setting.cs)
            var lm = GameManager.instance != null ? GameManager.instance.localizationManager : null;
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

            // load from: ModsSettings/MagicHearseRedux.coc
            AssetDatabase.global.LoadSettings("MagicHearseRedux", setting, new Setting(this));

            // show in Options → Mods
            setting.RegisterInOptionsUI();

            // schedule ECS system
            updateSystem.UpdateAt<MagicHearseSystem>(SystemUpdatePhase.GameSimulation);

            // honor current toggle
            var sys = updateSystem.World.GetOrCreateSystemManaged<MagicHearseSystem>();
            sys.Enabled = setting.EnableMagicHearse;
        }

        public void OnDispose()
        {
            Log.Info("OnDispose");

            if (Settings != null)
            {
                Settings.UnregisterInOptionsUI();
            }

            Settings = null;
        }
    }
}
