using System;
using System.IO;
using UnityEngine;

namespace TodayOrdeal
{
    internal class Log
    {
        public static void Error(Exception e)
        {
            //Log.Error(e.GetType().Name + ": " + e.Message);
            //Log.Error(e.StackTrace);
        }

        public static void Error(string message)
        {
            //File.AppendAllText(Application.dataPath + "/BaseMods/Error_Sage.txt", message + "\n");
        }
    }
}
