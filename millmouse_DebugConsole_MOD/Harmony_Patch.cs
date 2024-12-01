using System;
using UnityEngine;
using Harmony;
using System.Collections.Generic;
using System.Collections;
using GlobalBullet;
using UnityEngine.UI;
using WorkerSpine;
using WorkerSprite;
using MyMod.Patches;

namespace MyMod
{
    public class Harmony_Patch : MonoBehaviour
    { 

        public Harmony_Patch()
        {
            Harmony_Patch.Invoke(delegate
            {
                HarmonyInstance mod = HarmonyInstance.Create("Lobotomy.sage.mod");
                new StartGamePatch(mod);
                GameObject updatePatchObject = new GameObject("UpdatePatchObject");

                PatchCaller patchCaller = new PatchCaller();
                patchCaller.CallPatches(mod);   

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

        public const string ModName = "Lobotomy.xyz.debugGUI";

        private static Dictionary<string, int> _limiter = new Dictionary<string, int>();
    }

    public class MonoBehaviourWithCoroutine : MonoBehaviour { }
}