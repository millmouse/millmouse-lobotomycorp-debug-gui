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
        private static DateTime? lastLogTime = null; 
        private static readonly TimeSpan logCooldown = TimeSpan.FromSeconds(20); 


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
            if (__result == null || __result.Count == 0)
            {
                Log.LogAndDebug("AgentManagerPatch: No agents found in GetAgentList.");
                return;
            }

            if (!ShouldLogMessage())
                return;

            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine("AgentManagerPatch: EGO gift counts ordered by most to least:");

            var agentGiftCounts = new List<KeyValuePair<string, int>>();

            foreach (var agent in __result)
            {
                string agentName = agent?.name ?? "Unknown Agent";
                int giftCount = agent?.GetAllGifts()?.Count ?? 0; 
                agentGiftCounts.Add(new KeyValuePair<string, int>(agentName, giftCount));
            }

            agentGiftCounts.Sort((x, y) => y.Value.CompareTo(x.Value));

            foreach (var agentGiftCount in agentGiftCounts)
            {
                logMessage.AppendLine($"- {agentGiftCount.Key}: {agentGiftCount.Value} EGO gifts");
            }

            Log.LogAndDebug(logMessage.ToString());
        }

        private static bool ShouldLogMessage()
        {
            DateTime now = DateTime.Now;

            if (lastLogTime.HasValue && (now - lastLogTime.Value) < logCooldown)
            {
                return false;
            }

            lastLogTime = now; 
            return true;
        }
    }

}
