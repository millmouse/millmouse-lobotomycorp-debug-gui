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
            PatchAgentManager(mod);
        }

        private void PatchAgentManager(HarmonyInstance mod)
        {
            new AgentManagerPatch(mod);
        }

        private void PatchAgentModel(HarmonyInstance mod)
        {
            new AgentModelPatch(mod, "OnClick");
        }

    }
}
