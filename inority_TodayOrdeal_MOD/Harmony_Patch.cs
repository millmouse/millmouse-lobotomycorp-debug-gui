using System;
using UnityEngine;
using Harmony;
using System.Collections.Generic;
using SageMod.Patches;
using System.Collections;

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

                new AnyPatch(mod, typeof(BgmManager), "PlayBgm");
                new AnyPatch(mod, typeof(OrdealManager), "OnGameInit");
                new AnyPatch(mod, typeof(GlobalAudioManager), "OnSceneLoad");
                new AnyPatch(mod, typeof(OrdealManager), "OnStageStart");
                new AnyPatch(mod, typeof(CursorManager), "Start");
                new AnyPatch(mod, typeof(RandomEventManager), "OnStageStart");
                new AnyPatch(mod, typeof(AgentManager), "OnStageStart");
                new AnyPatch(mod, typeof(AgentManager), "GetAgentList");

                new AgentModelPatch(mod, "OnClick");
                new AgentModelPatch(mod, "OnEnterRoom");
                new AgentModelPatch(mod, "OnStageStart");
                new AnyPatch(mod, typeof(AnimatorManager), "Init");

                GameObject guiObject = new GameObject("HelloWorldGUI");
                guiInstance = guiObject.AddComponent<HelloWorldGUI>();

                guiObject.AddComponent<MonoBehaviourWithCoroutine>().StartCoroutine(WaitForDebugTab(guiInstance));
                Log.LogAndDebug("HelloWorldGUI initialized with DebugTab ready.");

            });
        }

        private IEnumerator WaitForDebugTab(HelloWorldGUI guiInstance)
        {

            while (guiInstance?.debugTab == null)
            {
                yield return null;
            }

            Log.LogAndDebug("HelloWorldGUI initialized with DebugTab ready.");
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