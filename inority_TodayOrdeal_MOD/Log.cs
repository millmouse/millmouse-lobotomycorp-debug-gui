using SageMod.Util;
using System;
using System.IO;
using UnityEngine;

namespace MyMod
{
    internal class Log
    {
        private static readonly string logFilePath = Application.dataPath + "/BaseMods/Error_Sage.txt";
        private static bool logFileCreated = false;

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

                string timestampedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}"; // Prefix with timestamp
                File.AppendAllText(logFilePath, timestampedMessage + "\n");
                logFileCreated = true; // Set the flag to true after the first write
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to write to log file: " + ex.Message);
            }
        }

        public static void LogAndDebug(string message)
        {
            // Log to the console
            Log.Error(message);

            // Add the message to the DebugTab
            DebugTab.AddMessage(message, ColorUtils.HexToColor("#7d9ed1"));
            DebugTab.AddMessage("AAA", ColorUtils.HexToColor("#7d9ed1"));
        }

        public static void LogAndDebug(string message,Color color)
        {
            // Log to the console
            Log.Error(message);

            // Add the message to the DebugTab
            DebugTab.AddMessage(message, color);
        }
    }
}
