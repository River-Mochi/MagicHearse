// File: Utils/LogUtil.cs
// Shared version 0.3.2
// Purpose:
// - WarnOnce: prevents repeated WARN spam in hot paths
// - TryLog: lazy message construction inside try/catch
// Safety goals: never throw back into gameplay/mod loading

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
                try
                {
                    // Use WARN so severity is not escalated.
                    log.Log(Level.Warn, "Log message factory threw: " + ex.GetType().Name + ": " + ex.Message, ex);
                }
                catch
                {
                    // Logging must never throw back into gameplay/mod loading.
                }

                return;
            }

            try
            {
                // Colossal ILog.Log takes an Exception parameter; pass null safely.
                Exception exToLog = exception ?? null!;
                log.Log(level, message, exToLog);
            }
            catch
            {
                // Logging must never throw back into gameplay/mod loading.
            }
        }
    }
}
