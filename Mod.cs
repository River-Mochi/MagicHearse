// <copyright file="Mod.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Mod.cs
// Entrypoint: registers settings, locales, ECS systems.

namespace MagicHearse
{
    using System;                    // Exception
    using System.Reflection;         // Assembly (version)

    using Colossal.IO.AssetDatabase; // AssetDatabase.LoadSettings
    using Colossal.Localization;     // LocalizationManager
    using Colossal.Logging;          // ILog, LogManager

    using CS2Shared.RiverMochi;      // LogUtils

    using Game;                      // UpdateSystem, SystemUpdatePhase
    using Game.Modding;              // IMod
    using Game.SceneFlow;            // GameManager

    public sealed class Mod : IMod
    {
        public const string ModName = "Magic Hearse";
        public const string ModId = "MagicHearse";
        public const string ModTag = "[MH]";

        // Which build is loaded. Shown in the log banner AND the About tab so it is obvious at a glance.
#if DEBUG
        public const string BuildType = "DEBUG";
#else
        public const string BuildType = "RELEASE";
#endif

        public static readonly string ModVersion =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";

        public static readonly ILog s_Log =
            LogManager.GetLogger(ModId).SetShowsErrorsInUI(
#if DEBUG
                true
#else
                false
#endif
            );

        public static Setting? Settings;

        private static bool s_BannerLogged;

        public void OnLoad(UpdateSystem updateSystem)
        {
            LogUtils.Configure(ModId, s_Log);

            if (!s_BannerLogged)
            {
                s_BannerLogged = true;  // one-time banner
                LogUtils.Info(() => $"{ModName} {ModTag} v{ModVersion} [{BuildType}] OnLoad");
            }

            GameManager? gameManager = GameManager.instance;
            if (gameManager == null)
            {
                LogUtils.Warn(() => "GameManager.instance is null in Mod.OnLoad.");
                return;
            }

            Setting setting = new Setting(this);
            Settings = setting;

            // Locales are best-effort: one guard + one try/catch so a bad source never crashes load.
            LocalizationManager? localizationManager = gameManager.localizationManager;
            if (localizationManager == null)
            {
                LogUtils.Warn(() => "LocalizationManager is null; locale sources were not registered.");
            }
            else
            {
                try
                {
                    localizationManager.AddSource("en-US", new LocaleEN(setting));
                    localizationManager.AddSource("fr-FR", new LocaleFR(setting));
                    localizationManager.AddSource("es-ES", new LocaleES(setting));
                    localizationManager.AddSource("de-DE", new LocaleDE(setting));
                    localizationManager.AddSource("it-IT", new LocaleIT(setting));
                    localizationManager.AddSource("ja-JP", new LocaleJA(setting));
                    localizationManager.AddSource("ko-KR", new LocaleKO(setting));
                    localizationManager.AddSource("zh-HANS", new LocaleZH_CN(setting));
                    localizationManager.AddSource("pl-PL", new LocalePL(setting));
                    localizationManager.AddSource("pt-BR", new LocalePT_BR(setting));
                    localizationManager.AddSource("zh-HANT", new LocaleZH_HANT(setting));
                }
                catch (Exception ex)
                {
                    LogUtils.Warn(() => $"Localization registration failed: {ex.GetType().Name}: {ex.Message}");
                }
            }

            AssetDatabase.global.LoadSettings(ModId, setting, new Setting(this));
            setting.RegisterInOptionsUI();

            updateSystem.UpdateAfter<FuneralDirectorSystem>(SystemUpdatePhase.PrefabUpdate);
            updateSystem.UpdateAt<MagicHearseSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<CemeteryResetSystem>(SystemUpdatePhase.GameSimulation);

            updateSystem.World.GetOrCreateSystemManaged<MagicHearseSystem>().Enabled = setting.EnableMagicHearse;

            // Auto-reset scanner runs only when Funeral Director + AutoReset are both ON.
            updateSystem.World.GetOrCreateSystemManaged<CemeteryResetSystem>().Enabled =
                setting.FuneralDirector && setting.AutoResetCemetery;

            if (setting.FuneralDirector)
            {
                updateSystem.World.GetOrCreateSystemManaged<FuneralDirectorSystem>().ScheduleReapply();
            }
        }

        public void OnDispose()
        {
            Settings?.UnregisterInOptionsUI();
            Settings = null;
        }
    }
}
