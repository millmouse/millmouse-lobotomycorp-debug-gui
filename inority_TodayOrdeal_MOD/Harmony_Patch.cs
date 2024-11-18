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
                // Apply the BGM patch
                new AnyPatch(mod, typeof(BgmManager), "PlayBgm");
                new AnyPatch(mod, typeof(OrdealManager), "OnGameInit");
                new AnyPatch(mod, typeof(GlobalAudioManager), "OnSceneLoad");
                new AnyPatch(mod, typeof(OrdealManager), "OnStageStart");
                new AnyPatch(mod, typeof(CursorManager), "Start");
                new AnyPatch(mod, typeof(RandomEventManager), "OnStageStart");
                new AnyPatch(mod, typeof(AgentManager), "OnStageStart");
                new AnyPatch(mod, typeof(AgentManager), "GetAgentList");
                //new AnyPatch(mod, typeof(AgentModel), "OnClick");
                //new AnyPatch(mod, typeof(AgentModel), "OnEnterRoom");
                //new AnyPatch(mod, typeof(AgentModel), "OnStageStart");
                new AgentModelPatch(mod, "OnClick");
                new AgentModelPatch(mod, "OnEnterRoom");
                new AgentModelPatch(mod, "OnStageStart");
                new AnyPatch(mod, typeof(AnimatorManager), "Init");
                //GameManagerPatch updatePatch = updatePatchObject.AddComponent<GameManagerPatch>();
                //updatePatch.Init(mod);

                //new AnyPatch(mod, typeof(AgentManager), "Init");
                // cursormanager
                // bgm manager

                // Create the GUI object
                GameObject guiObject = new GameObject("HelloWorldGUI");
                guiInstance = guiObject.AddComponent<HelloWorldGUI>();

                // Start a coroutine to wait until debugTab is initialized
                guiObject.AddComponent<MonoBehaviourWithCoroutine>().StartCoroutine(WaitForDebugTab(guiInstance));
                Log.LogAndDebug("HelloWorldGUI initialized with DebugTab ready.");


            });
        }

        // Coroutine to wait for debugTab to be initialized
        private IEnumerator WaitForDebugTab(HelloWorldGUI guiInstance)
        {
            // Wait until guiInstance.debugTab is not null
            while (guiInstance?.debugTab == null)
            {
                yield return null; // Wait until the next frame and check again
            }

            // Once debugTab is initialized, add the debug message
            //make ethod to log and debug at same time w same message.
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

    // Helper MonoBehaviour class to run coroutines
    public class MonoBehaviourWithCoroutine : MonoBehaviour { }
}
