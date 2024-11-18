using System;
using UnityEngine;
using Harmony;
using System.Collections.Generic;
using System.Collections;
using GlobalBullet;
using UnityEngine.UI;
using WorkerSpine;
using WorkerSprite;

namespace MyMod
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

                PatchCaller patchCaller = new PatchCaller();
                patchCaller.CallPatches(mod);

                GameObject guiObject = new GameObject("HelloWorldGUI");
                guiInstance = guiObject.AddComponent<HelloWorldGUI>();

                guiObject.AddComponent<MonoBehaviourWithCoroutine>().StartCoroutine(WaitForDebugTab(guiInstance));

            });
        }


        private IEnumerator WaitForDebugTab(HelloWorldGUI guiInstance)
        {

            while (guiInstance?.debugTab == null)
            {
                yield return null;
            }
            Log.LogAndDebug("HelloWorldGUI initialized with DebugTab ready.", ColorUtils.HexToColor("#f57e42"));

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

        public const string ModName = "Lobotomy.xyz.debugGUI";

        private static Dictionary<string, int> _limiter = new Dictionary<string, int>();
    }

    public class MonoBehaviourWithCoroutine : MonoBehaviour { }
}