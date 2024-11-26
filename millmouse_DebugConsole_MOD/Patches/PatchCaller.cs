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
            //PatchAgentInfoWindow(mod);
            PatchAgentModel(mod);
            PatchAgentHistory(mod);
            //make a patch specifically for detecting hp up and down of agent. 
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

        //private void PatchAgentInfoWindow(HarmonyInstance mod)
        //{
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "Init");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "Start");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "OnEnable");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "CloseWindow");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "CreateWindow");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "OnDisable");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "GetCurrentColor");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "OnClickPortrait");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "EnforcementWindow");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "OnClearOverlay");
        //    new AnyPatch(mod, typeof(AgentInfoWindow), "UnPinCurrentAgent");
        //}

        private void PatchAgentModel(HarmonyInstance mod)
        {
            new AgentModelPatch(mod, "OnClick");
            new AgentModelPatch(mod, "OnEnterRoom");
            new AgentModelPatch(mod, "OnStageStart");
        }

    }
}
