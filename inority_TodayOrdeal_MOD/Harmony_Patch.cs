using System;
using UnityEngine;
using Harmony;
using System.Collections.Generic;
using SageMod.Patches;
using System.Collections;
using GlobalBullet;

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

                CallPatches(mod);

                GameObject guiObject = new GameObject("HelloWorldGUI");
                guiInstance = guiObject.AddComponent<HelloWorldGUI>();

                guiObject.AddComponent<MonoBehaviourWithCoroutine>().StartCoroutine(WaitForDebugTab(guiInstance));

            });
        }

        private void CallPatches(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(BgmManager), "PlayBgm");
            new AnyPatch(mod, typeof(OrdealManager), "OnGameInit");
            new AnyPatch(mod, typeof(GlobalAudioManager), "OnSceneLoad");
            new AnyPatch(mod, typeof(OrdealManager), "OnStageStart");
            //new AnyPatch(mod, typeof(CursorManager), "Start");
            //new AnyPatch(mod, typeof(CursorManager), "HideCursor");
            //new AnyPatch(mod, typeof(CursorManager), "ForcelyCurserSet");
            //new AnyPatch(mod, typeof(CursorManager), "OnEnteredTarget");
            //new AnyPatch(mod, typeof(CursorManager), "OnExitTarget");
            //new AnyPatch(mod, typeof(CursorManager), "CursorSet");
            //new AnyPatch(mod, typeof(RandomEventManager), "OnStageStart");
            //new AnyPatch(mod, typeof(AgentManager), "OnStageStart");
            //new AnyPatch(mod, typeof(AgentManager), "GetAgentList");
            //new AnyPatch(mod, typeof(AgentManager), "GetAgentList");
            //new AnyPatch(mod, typeof(AnimatorManager), "Init");
            //new AnyPatch(mod, typeof(MissionManager), "Init");
            //new AnyPatch(mod, typeof(ObserveInfoManager), "get_instance");
            //new AnyPatch(mod, typeof(OfficerManager), "Init");
            //new AnyPatch(mod, typeof(HierarchicalDataManager), "Init");

            //new AnyPatch(mod, typeof(GlobalGameManager), "InitHidden");
            //new AnyPatch(mod, typeof(GlobalGameManager), "InitStoryMode");
            //new AnyPatch(mod, typeof(GlobalGameManager), "LoadSaveFile");
            //new AnyPatch(mod, typeof(GlobalGameManager), "ChangeLanguage");
            //new AnyPatch(mod, typeof(GlobalGameManager), "Start");
            //new AnyPatch(mod, typeof(GlobalGameManager), "ExistSaveData");
            //new AnyPatch(mod, typeof(GlobalGameManager), "ExistSaveData");
            //new AnyPatch(mod, typeof(GlobalGameManager), "IsPlaying");
            //new AnyPatch(mod, typeof(GlobalGameManager), "OnDisable");
            //new AnyPatch(mod, typeof(GlobalGameManager), "OnEnable");
            //new AnyPatch(mod, typeof(GlobalGameManager), "OnLevelWasLoaded");
            //new AnyPatch(mod, typeof(GlobalGameManager), "LoadGlobalData");
            //new AnyPatch(mod, typeof(GlobalGameManager), "LoadData");
            //new AnyPatch(mod, typeof(GlobalGameManager), "SaveData");
            //new AnyPatch(mod, typeof(GlobalGameManager), "SaveDataWithCheckPoint");

            //new AnyPatch(mod, typeof(GlobalBulletManager), "UpdateUI");
            //new AnyPatch(mod, typeof(GlobalBulletManager), "ActivateBullet");
            //new AnyPatch(mod, typeof(GlobalBulletManager), "ExcuteBullet");
            //new AnyPatch(mod, typeof(GlobalBulletManager), "OnStageStart");
            //new AnyPatch(mod, typeof(GlobalBulletManager), "SetMaxBullet");

            //new AnyPatch(mod, typeof(ExplodeGutManager), "MakeEffects");
            //new AnyPatch(mod, typeof(ExplodeGutManager), "Start");

            //new AnyPatch(mod, typeof(CreatureManager), "Init");
            //new AnyPatch(mod, typeof(CreatureManager), "RegisterCreature");
            //new AnyPatch(mod, typeof(CreatureManager), "OnGameInit");
            //new AnyPatch(mod, typeof(CreatureManager), "AddCreature");
            //new AnyPatch(mod, typeof(CreatureManager), "FindCreature");
            //new AnyPatch(mod, typeof(CreatureManager), "GetCreature");
            //new AnyPatch(mod, typeof(CreatureManager), "GetCreatureList");
            //new AnyPatch(mod, typeof(CreatureManager), "GetObserveInfo");
            //new AnyPatch(mod, typeof(CreatureManager), "GetSaveData");
            //new AnyPatch(mod, typeof(CreatureManager), "LoadData");
            //new AnyPatch(mod, typeof(CreatureManager), "ResetProbReductionCounterAll");
            //new AnyPatch(mod, typeof(CreatureManager), "TryGetValue");
            //new AnyPatch(mod, typeof(CreatureManager), "LoadSpecialSkillTable");
            //new AnyPatch(mod, typeof(CreatureManager), "OnStageStart");


            new AgentModelPatch(mod, "OnClick");
            new AgentModelPatch(mod, "OnEnterRoom");
            new AgentModelPatch(mod, "OnStageStart");

            //TODO: Onclick agent model print name, rwbp, current sefira, promotion points, which monsters worked on and how many times.

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