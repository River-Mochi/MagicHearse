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
    using System;                     // Exception, StringComparison, Type
    using System.IO;                  // Directory, File, Path
    using System.Reflection;          // Assembly (version)

    using Colossal;                   // Hash128
    using Colossal.IO.AssetDatabase;  // AssetDatabase, AssetDataPath, SettingAsset
    using Colossal.Localization;      // LocalizationManager
    using Colossal.Logging;           // ILog, LogManager
    using Colossal.PSI.Environment;   // EnvPath

    using CS2Shared.RiverMochi;       // LogUtils

    using Game;                       // UpdateSystem, SystemUpdatePhase
    using Game.Modding;               // IMod
    using Game.SceneFlow;             // GameManager
    using static Game.UI.Menu.AssetUploadPanelUISystem;

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

        public static MHSetting? Settings;

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

            MigrateLegacySettingsFile();

            MHSetting setting = new MHSetting(this);
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
                    localizationManager.AddSource("pl-PL", new LocalePL(setting));
                    localizationManager.AddSource("pt-BR", new LocalePT_BR(setting));
                    localizationManager.AddSource("zh-HANS", new LocaleZH_CN(setting));
                    localizationManager.AddSource("zh-HANT", new LocaleZH_HANT(setting));
                    // next are locales not officially supported, languages mods needed to use these.
                    localizationManager.AddSource("pt-PT", new LocalePT_PT(setting));
                    localizationManager.AddSource("th-TH", new LocaleTH(setting));
                    localizationManager.AddSource("uk-UA", new LocaleUK(setting));
                    localizationManager.AddSource("vi-VN", new LocaleVI(setting));
                }
                catch (Exception ex)
                {
                    LogUtils.Warn(() => $"Localization registration failed: {ex.GetType().Name}: {ex.Message}");
                }
            }

            AssetDatabase.global.LoadSettings(ModId, setting, new MHSetting(this));
            setting.RegisterInOptionsUI();

            updateSystem.UpdateAfter<FuneralDirectorSystem>(SystemUpdatePhase.PrefabUpdate);
            updateSystem.UpdateAt<MagicHearseSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<CemeteryResetSystem>(SystemUpdatePhase.GameSimulation);

            updateSystem.World.GetOrCreateSystemManaged<MagicHearseSystem>().Enabled = setting.EnableMagicHearse;

            // Auto-reset scanner runs for whichever active mode wants it (FD default ON, Magic default OFF).
            updateSystem.World.GetOrCreateSystemManaged<CemeteryResetSystem>().Enabled =
                (setting.FuneralDirector && setting.AutoResetCemetery) || (setting.EnableMagicHearse && setting.MagicResetCemetery);

            if (setting.FuneralDirector)
            {
                updateSystem.World.GetOrCreateSystemManaged<FuneralDirectorSystem>().ScheduleReapply();
            }
        }

        private static void MigrateLegacySettingsFile()
        {
            try
            {
                string oldLocation = Path.Combine(
                    EnvPath.kUserDataPath,
                    "ModsSettings",
                    $"{ModId}.coc");

                if (!File.Exists(oldLocation))
                {
                    return;
                }

                string directory = Path.Combine(
                    EnvPath.kUserDataPath,
                    "ModsSettings",
                    ModId);

                string correctLocation = Path.Combine(
                    directory,
                    $"{ModId}.coc");

                // Two files are ambiguous. Do not overwrite either one automatically.
                if (File.Exists(correctLocation))
                {
                    LogUtils.Warn(() =>
                        "Both legacy and standard MagicHearse settings files exist; migration was skipped.");
                    return;
                }

                IDataSourceProvider dataSource = AssetDatabase.user.dataSource;
                string normalizedOldLocation = Path.GetFullPath(oldLocation);

                bool oldEntryFound = false;
                Hash128 oldGuid = default;

                foreach ((Type type, Hash128 hash) entry in dataSource.Enumerate())
                {
                    if (entry.type != typeof(SettingAsset))
                    {
                        continue;
                    }

                    SourceMeta meta = dataSource.GetMeta(entry.hash);
                    if (string.IsNullOrEmpty(meta.path))
                    {
                        continue;
                    }

                    string candidateLocation = Path.GetFullPath(meta.path);
                    if (!string.Equals(
                            candidateLocation,
                            normalizedOldLocation,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    oldGuid = entry.hash;
                    oldEntryFound = true;
                    break;
                }

                if (!oldEntryFound)
                {
                    // Moving only the physical file would leave the game's in-memory
                    // settings entry pointed at the old location.
                    LogUtils.Warn(() =>
                        "Legacy MagicHearse settings file was found, but its Asset Database entry was not found. Migration was skipped.");
                    return;
                }

                Directory.CreateDirectory(directory);

                // Copy first so the settings data is safe at the new location.
                File.Copy(oldLocation, correctLocation, overwrite: false);

                // Keep the same Asset Database GUID, but remap it from the old
                // physical path to the new physical path for this running session.
                dataSource.DeleteEntryFromDatabase(oldGuid);
                dataSource.AddEntryFromDatabase(
                    AssetDataPath.Create(
                        $"ModsSettings/{ModId}/{ModId}.coc",
                        EscapeStrategy.None),
                    typeof(SettingAsset),
                    oldGuid);

                LogUtils.Info(() =>
                    "Migrated settings to ModsSettings/MagicHearse/MagicHearse.coc.");
            }
            catch (Exception ex)
            {
                // Migration failure must not prevent the mod from loading.
                LogUtils.Warn(() =>
                    $"Settings migration failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        public void OnDispose()
        {
            Settings?.UnregisterInOptionsUI();
            Settings = null;
        }
    }
}
