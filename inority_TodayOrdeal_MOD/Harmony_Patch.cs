using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace TodayOrdeal
{
    public class Harmony_Patch
    {
        public Harmony_Patch()
        {
            Harmony_Patch.Invoke(delegate
            {
                HarmonyInstance mod = HarmonyInstance.Create("Lobotomy.sage.mod");
                this.Patch(mod);
            });
        }

        public void Patch(HarmonyInstance mod)
        {
            MethodInfo original = typeof(GameManager).GetMethod("StartGame");
            MethodInfo postfix = typeof(Harmony_Patch).GetMethod("Postfix_StartGame");

            mod.Patch(original, null, new HarmonyMethod(postfix), null);

            Log.Error("patch success!");

            GameObject guiObject = new GameObject("HelloWorldGUI");
            guiObject.AddComponent<HelloWorldGUI>();
        }

        private static void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public const string ModName = "Lobotomy.inority.TodayOrdeal";

        private static Dictionary<string, int> _limiter = new Dictionary<string, int>();
    }
}
