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
            PatchAgentModel(mod);
            PatchCalculateLevelExp(mod);
            PatchFinishWorkSuccessfully(mod);
            PatchAgentManagerGetAgentList(mod);
            Log.LogAndDebug("Patched!");
        }
        private void PatchAgentManagerGetAgentList(HarmonyInstance mod)
        {
            new AgentManagerPatch(mod);
        }
        private void PatchEXPStats(HarmonyInstance mod)
        {
            new AnyPatch(mod, typeof(WorkerPrimaryStatExp), "Init");
            new AnyPatch(mod, typeof(AgentModel), "UpdatePrimaryStat");
            new AnyPatch(mod, typeof(AgentModel), "UpdateBestRwbp");
            new AnyPatch(mod, typeof(AgentModel), "CalculateStatLevel");
            new AnyPatch(mod, typeof(WorkerPrimaryStat), "UpdateStat");
            new AnyPatch(mod, typeof(WorkerPrimaryStat), "GetAddedStat");
            new AnyPatch(mod, typeof(WorkerPrimaryStat), "MaxStatR");
            new AnyPatch(mod, typeof(WorkerPrimaryStat), "MaxStatW");
            new AnyPatch(mod, typeof(WorkerPrimaryStat), "MaxStatB");
            new AnyPatch(mod, typeof(WorkerPrimaryStat), "MaxStatP");

            new AnyPatch(mod, typeof(UseSkill), "CalculateLevelExp");
            new AnyPatch(mod, typeof(UseSkill), "FinishWorkSuccessfully");
            new AnyPatch(mod, typeof(UseSkill), "GetFeelingState");
            new AnyPatch(mod, typeof(UseSkill), "GetSuccessCubeCount");
        }

        private void PatchCalculateLevelExp(HarmonyInstance mod)
        {
            new CalculateLevelExpPatch(mod);
        }

        private void PatchFinishWorkSuccessfully(HarmonyInstance mod)
        {
            new FinishWorkSuccessfullyPatch(mod);
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
            new AnyPatch(mod, typeof(AgentHistory), "WorkResult");
            new AnyPatch(mod, typeof(AgentHistory), "Disposition");
            new AnyPatch(mod, typeof(AgentHistory), "PromotionValAdd");
            new AnyPatch(mod, typeof(AgentHistory), "PromotionValReset");
            new AnyPatch(mod, typeof(AgentHistory), "GetWorkCount");

            new AnyPatch(mod, typeof(PromotionNeedVal), "GetNeedVal");

            new AnyPatch(mod, typeof(History), "Disposition");
            new AnyPatch(mod, typeof(History), "AddPanic");
            new AnyPatch(mod, typeof(History), "AddWorkCubeCount");
            new AnyPatch(mod, typeof(History), "AddWorkDay");
            new AnyPatch(mod, typeof(History), "CreatureAttack");
            new AnyPatch(mod, typeof(History), "LoadData");
            new AnyPatch(mod, typeof(History), "GetSaveData");
            new AnyPatch(mod, typeof(History), "Suppress");
            new AnyPatch(mod, typeof(History), "AddWorkSuccess");
            new AnyPatch(mod, typeof(History), "PhysicalDamage");
            new AnyPatch(mod, typeof(History), "MentalDamage");
            new AnyPatch(mod, typeof(History), "WorkResult");
            new AnyPatch(mod, typeof(History), "Disposition");
            new AnyPatch(mod, typeof(History), "PromotionValAdd");
            new AnyPatch(mod, typeof(History), "PromotionValReset");
            new AnyPatch(mod, typeof(History), "GetWorkCount");
        }

        private void PatchAgentModel(HarmonyInstance mod)
        {
            new AgentModelPatch(mod, "OnClick");
        }

    }
}
