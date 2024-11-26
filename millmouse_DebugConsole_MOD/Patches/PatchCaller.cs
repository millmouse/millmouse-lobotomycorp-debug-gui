using GlobalBullet;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkerSpine;
using static AgentHistory;

namespace MyMod.Patches
{
    public class PatchCaller
    {
        public void CallPatches(HarmonyInstance mod)
        {
            PatchAgentManager(mod);
            PatchResultScreen(mod);
            PatchCreatureManager(mod);
            //PatchAgentInfoWindow(mod);
            PatchAgentSpriteChanger(mod);
            PatchAgentModel(mod);
            PatchAgentHistory(mod);
        }

        private void PatchAgentHistory(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(AgentHistory), "Disposition");
            new AnyPatch(mod, typeof(AgentHistory), "AddPanic");
            new AnyPatch(mod, typeof(AgentHistory), "AddWorkCubeCount");
            new AnyPatch(mod, typeof(AgentHistory), "AddWorkDay");
            new AnyPatch(mod, typeof(AgentHistory), "CreatureAttack");
            new AnyPatch(mod, typeof(AgentHistory), "LoadData");
            new AnyPatch(mod, typeof(AgentHistory), "GetSaveData");
            new AnyPatch(mod, typeof(AgentHistory), "Suppress");
            new AnyPatch(mod, typeof(AgentHistory), "AddWorkSuccess");
            new AnyPatch(mod, typeof(AgentHistory), "PhysicalDamage");
            new AnyPatch(mod, typeof(AgentHistory), "MentalDamage");

            new AnyPatch(mod, typeof(PromotionNeedVal), "GetNeedVal");
        }

        private void PatchAgentManager(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(AgentManager), "OnStageStart");
        }

        private void PatchResultScreen(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(ResultScreen), "SoundMake");
            new AnyPatch(mod, typeof(ResultScreen), "OnEnable");
            new AnyPatch(mod, typeof(ResultScreen), "OnSuccessManagement");
            new AnyPatch(mod, typeof(ResultScreen), "CreatureAnimCall");
            new AnyPatch(mod, typeof(ResultScreen), "OnClickContinueGame");
            new AnyPatch(mod, typeof(ResultScreen), "AgentReset");
        }

        private void PatchGlobalGameManager(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(GlobalGameManager), "InitHidden");
            new AnyPatch(mod, typeof(GlobalGameManager), "InitStoryMode");
            new AnyPatch(mod, typeof(GlobalGameManager), "LoadSaveFile");
            new AnyPatch(mod, typeof(GlobalGameManager), "ChangeLanguage");
            new AnyPatch(mod, typeof(GlobalGameManager), "Start");
            new AnyPatch(mod, typeof(GlobalGameManager), "ExistSaveData");
            new AnyPatch(mod, typeof(GlobalGameManager), "IsPlaying");
            new AnyPatch(mod, typeof(GlobalGameManager), "OnDisable");
            new AnyPatch(mod, typeof(GlobalGameManager), "OnEnable");
            new AnyPatch(mod, typeof(GlobalGameManager), "OnLevelWasLoaded");
            new AnyPatch(mod, typeof(GlobalGameManager), "LoadGlobalData");
            new AnyPatch(mod, typeof(GlobalGameManager), "LoadData");
            new AnyPatch(mod, typeof(GlobalGameManager), "SaveData");
            new AnyPatch(mod, typeof(GlobalGameManager), "SaveDataWithCheckPoint");
        }

        private void PatchCreatureManager(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(CreatureManager), "Init");
            new CreatureManagerPatch(mod);
            new AnyPatch(mod, typeof(CreatureManager), "OnGameInit");
            new AnyPatch(mod, typeof(CreatureManager), "AddCreature");
            new AnyPatch(mod, typeof(CreatureManager), "FindCreature");
            new AnyPatch(mod, typeof(CreatureManager), "GetCreature");
            new AnyPatch(mod, typeof(CreatureManager), "GetCreatureList");
            new AnyPatch(mod, typeof(CreatureManager), "GetObserveInfo");
            new AnyPatch(mod, typeof(CreatureManager), "GetSaveData");
            new AnyPatch(mod, typeof(CreatureManager), "LoadData");
            new AnyPatch(mod, typeof(CreatureManager), "ResetProbReductionCounterAll");
            new AnyPatch(mod, typeof(CreatureManager), "TryGetValue");
            new AnyPatch(mod, typeof(CreatureManager), "LoadSpecialSkillTable");
            new AnyPatch(mod, typeof(CreatureManager), "OnStageStart");
        }

        private void PatchAgentInfoWindow(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(AgentInfoWindow), "Init");
            new AnyPatch(mod, typeof(AgentInfoWindow), "Start");
            new AnyPatch(mod, typeof(AgentInfoWindow), "OnEnable");
            new AnyPatch(mod, typeof(AgentInfoWindow), "CloseWindow");
            new AnyPatch(mod, typeof(AgentInfoWindow), "CreateWindow");
            new AnyPatch(mod, typeof(AgentInfoWindow), "OnDisable");
            new AnyPatch(mod, typeof(AgentInfoWindow), "GetCurrentColor");
            new AnyPatch(mod, typeof(AgentInfoWindow), "OnClickPortrait");
            new AnyPatch(mod, typeof(AgentInfoWindow), "EnforcementWindow");
            new AnyPatch(mod, typeof(AgentInfoWindow), "OnClearOverlay");
            new AnyPatch(mod, typeof(AgentInfoWindow), "UnPinCurrentAgent");
        }

        private void PatchAgentSpriteChanger(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(AgentSpriteChanger), "ClothesSetting");
            new AnyPatch(mod, typeof(AgentSpriteChanger), "WeaponSetting");
            new AnyPatch(mod, typeof(AgentSpriteChanger), "Start");
        }

        private void PatchAgentModel(HarmonyInstance mod)
        {
            new AgentModelPatch(mod, "OnClick");
            new AgentModelPatch(mod, "OnEnterRoom");
            new AgentModelPatch(mod, "OnStageStart");
        }

    }
}
