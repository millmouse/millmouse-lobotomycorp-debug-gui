using System;
using UnityEngine;
using Harmony;
using System.Collections.Generic;

namespace TodayOrdeal
{
    public class Harmony_Patch : MonoBehaviour
    {
        public static HelloWorldGUI guiInstance;

        public Harmony_Patch()
        {
            Harmony_Patch.Invoke(delegate
            {
                HarmonyInstance mod = HarmonyInstance.Create("Lobotomy.sage.mod");
                new StartGamePatch(mod);
                GameObject updatePatchObject = new GameObject("UpdatePatchObject");
                UpdatePatch updatePatch = updatePatchObject.AddComponent<UpdatePatch>();
                updatePatch.Init(mod);
                GameObject guiObject = new GameObject("HelloWorldGUI");
                guiInstance = guiObject.AddComponent<HelloWorldGUI>();
            });
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
