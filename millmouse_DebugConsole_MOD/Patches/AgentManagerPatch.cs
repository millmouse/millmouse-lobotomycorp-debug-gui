using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
                // Check if the result list is null or empty
                if (__result == null || __result.Count == 0)
                {
                    Log.LogAndDebug("AgentManagerPatch: No agents found in GetAgentList.");
                    return;
                }

                // Initialize StringBuilder to accumulate the message
                StringBuilder logMessage = new StringBuilder();
                logMessage.AppendLine("AgentManagerPatch: EGO gift counts ordered by most to least:");

                // Prepare a list to store agent names and their EGO gift counts
                var agentGiftCounts = new List<KeyValuePair<string, int>>();

                // Iterate through the agent list and collect their EGO gift counts
                foreach (var agent in __result)
                {
                    string agentName = agent?.name ?? "Unknown Agent";
                    int giftCount = agent?.GetAllGifts()?.Count ?? 0; // Safely get the gift count
                    agentGiftCounts.Add(new KeyValuePair<string, int>(agentName, giftCount));
                }

                // Sort the list by gift count in descending order
                agentGiftCounts.Sort((x, y) => y.Value.CompareTo(x.Value));

                // Append the sorted results to the logMessage
                foreach (var agentGiftCount in agentGiftCounts)
                {
                    logMessage.AppendLine($"- {agentGiftCount.Key}: {agentGiftCount.Value} EGO gifts");
                }

                // Log the accumulated message
                Log.LogAndDebug(logMessage.ToString());
            }
        }


    }
}
