using System;
using System.IO;
using UnityEngine;

namespace TodayOrdeal
{
    internal class Log
    {
        private static readonly string logFilePath = Application.dataPath + "/BaseMods/Error_Sage.txt";

        public static void Error(Exception e)
        {
            Error(e.GetType().Name + ": " + e.Message);
            Error(e.StackTrace);
        }

        public static void Error(string message)
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    File.Delete(logFilePath);
                }

                File.AppendAllText(logFilePath, message + "\n");
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to write to log file: " + ex.Message);
            }
        }
    }
}
