// File: Utils/LogUtil.cs
// Shared version 0.3.2
// Purpose:
// - WarnOnce: prevents repeated WARN spam in hot paths
// - TryLog: lazy message construction inside try/catch
// - Popup-safe: if messageFactory throws, log WITHOUT passing an Exception object (avoid UI popups)

namespace CS2HonuShared
{
    using Colossal.Logging;
    using System;
    using System.Collections.Generic;

    public static class LogUtil
    {
        // Each mod is a separate assembly; one static set per mod.
        private static readonly HashSet<string> s_WarnOnceKeys =
            new HashSet<string>(StringComparer.Ordinal);

        private const int MaxWarnOnceKeys = 2048;

        public static bool WarnOnce(ILog log, string key, Func<string> messageFactory, Exception? exception = null)
        {
            if (log == null || string.IsNullOrEmpty(key) || messageFactory == null)
            {
                return false;
            }

            if (!log.isLevelEnabled(Level.Warn))
            {
                return false;
            }

            string fullKey = log.name + "|" + key;

            lock (s_WarnOnceKeys)
            {
                if (s_WarnOnceKeys.Count >= MaxWarnOnceKeys)
                {
                    s_WarnOnceKeys.Clear();
                }

                if (!s_WarnOnceKeys.Add(fullKey))
                {
                    return false;
                }
            }

            TryLog(log, Level.Warn, messageFactory, exception);
            return true;
        }

        /// <summary>
        /// Safe logging wrapper:
        /// - Only evaluates messageFactory if the level is enabled
        /// - Never throws outward (even if messageFactory or the logger throws)
        /// </summary>
        public static void TryLog(ILog log, Level level, Func<string> messageFactory, Exception? exception = null)
        {
            if (log == null || messageFactory == null)
            {
                return;
            }

            if (!log.isLevelEnabled(level))
            {
                return;
            }

            string message;

            try
            {
                message = messageFactory() ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Message factory failed; best effort log, never throw.
                // IMPORTANT: do NOT pass the Exception object into log.Log here,
                // because that can surface as an in-game error popup.
                try
                {
                    string safe =
                        "Log message factory threw: " + ex.GetType().Name + ": " + ex.Message;
                    log.Log(Level.Warn, safe, null!);
                }
                catch
                {
                    // Logging must never throw back into gameplay/mod loading.
                }

                return;
            }

            try
            {
                // Attach an Exception only when it's a real one.
                // Attaching Exceptions can trigger the in-game error popup.
                log.Log(level, message, exception ?? null!);
            }
            catch
            {
                // Logging must never throw back into gameplay/mod loading.
            }
        }
    }
}
