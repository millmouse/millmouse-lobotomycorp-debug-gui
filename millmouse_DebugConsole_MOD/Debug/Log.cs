using System;
using System.IO;
using UnityEngine;

namespace MyMod
{
    internal class Log
    {
        private static readonly string logFilePath = Application.dataPath + "/BaseMods/Error_Sage.txt";
        private static bool logFileCreated = false;
        private static DateTime? lastLogTime = null; // Track the last log time
        private static readonly TimeSpan logCooldown = TimeSpan.FromSeconds(20); // Cooldown duration

        public static void Error(Exception e)
        {
            Error(e.GetType().Name + ": " + e.Message);
            Error(e.StackTrace);
        }

        public static void Error(string message)
        {
            try
            {
                if (!logFileCreated && File.Exists(logFilePath))
                {
                    File.Delete(logFilePath);
                }

                string timestampedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(logFilePath, timestampedMessage + "\n");
                logFileCreated = true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to write to log file: " + ex.Message);
            }
        }

        public static void LogAndDebug(string message)
        {
            if (!ShouldLogMessage())
                return;

            Log.Error(message);
            DebugTab.AddMessage(message, ColorUtils.HexToColor("#edf4ff"));
        }

        public static void LogAndDebug(string message, Color color)
        {
            if (!ShouldLogMessage())
                return;

            Log.Error(message);
            DebugTab.AddMessage(message, color);
        }

        private static bool ShouldLogMessage()
        {
            DateTime now = DateTime.Now;

            if (lastLogTime.HasValue && (now - lastLogTime.Value) < logCooldown)
            {
                // Skip logging if within cooldown period
                return false;
            }

            lastLogTime = now; // Update the last log time
            return true;
        }
    }
}
