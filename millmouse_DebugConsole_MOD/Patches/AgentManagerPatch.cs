using System;
using System.Collections.Generic;
using System.Reflection;

using Harmony;
using MyMod;

namespace MyMod.Patches
{
    public class AgentManagerPatch
    {
        private static readonly Type targetType = typeof(AgentManager);
        private const string TargetMethodName = "GetAgentList"; 

        public AgentManagerPatch(HarmonyInstance mod)
        {
            string patchMethodName = "Postfix_LoggerPatch";
            Patch(mod, patchMethodName);
        }

        private void Patch(HarmonyInstance mod, string patchMethodName)
        {
            var originalMethod = targetType.GetMethod(TargetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var myPatchMethod = typeof(AgentManagerPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

            if (originalMethod != null)
            {
                mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
            }
            else
            {
                Log.Error($"Failed to find method: {TargetMethodName} in class {targetType.Name}");
            }
        }

        public static void Postfix_LoggerPatch(IList<AgentModel> __result)
        {
            if (Harmony_Patch.guiInstance?.debugTab != null)
            {
                Log.LogAndDebug($"AgentManagerPatch: GetAgentList returned {__result?.Count ?? 0} agents:");

                if (__result != null)
                {
                    foreach (var agent in __result)
                    {
                        string agentName = agent?.name ?? "Unknown Agent";
                        Log.LogAndDebug($"- Agent Name: {agentName}");
                        //Log.LogAndDebug($"- Agent Name: {agentName}");
                    }
                }
            }
        }
    }
}
