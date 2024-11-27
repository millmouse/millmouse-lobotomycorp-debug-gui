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
            //PatchAgentModel(mod);
            PatchAgentClick(mod);

        }

        private void PatchAgentClick(HarmonyInstance mod)
        {
            new AgentOnClickPatch(mod);
        }

        //private void PatchAgentModel(HarmonyInstance mod)
        //{
        //    new AgentModelPatch(mod, "OnClick");
        //    new AgentModelPatch(mod, "OnEnterRoom");
        //    new AgentModelPatch(mod, "OnStageStart");
        //}

    }
}
