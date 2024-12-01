using System;
using System.Diagnostics;
using System.Reflection;
using Harmony;
using MyMod;
using UnityEngine;

namespace MyMod.Patches
{
    public class AgentModelPatch
    {

        private static readonly Type targetType = typeof(AgentModel);

        public AgentModelPatch(HarmonyInstance mod, string targetMethodName)
        {
            string patchMethodName = "Postfix_LoggerPatch";
            Patch(mod, targetMethodName, patchMethodName);
        }

        private void Patch(HarmonyInstance mod, string targetMethodName, string patchMethodName)
        {

            var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var myPatchMethod = typeof(AgentModelPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

            if (originalMethod != null)
            {

                mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
            }
            else
            {

                Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
            }
        }

        private static string GetAgentStatDetails(AgentModel agent)
        {
            if (agent == null)
            {
                return "Agent is null. No details available.";
            }

            var sb = new System.Text.StringBuilder();

            sb.AppendLine("Primary Stat Experience:");
            sb.AppendLine($"- Battle: {agent.primaryStatExp?.battle ?? 0}");
            sb.AppendLine($"- Work: {agent.primaryStatExp?.work ?? 0}");
            sb.AppendLine($"- Mental: {agent.primaryStatExp?.mental ?? 0}");
            sb.AppendLine($"- HP: {agent.primaryStatExp?.hp ?? 0}");

            sb.AppendLine("Current Primary Stat Values:");
            sb.AppendLine($"- Battle: {agent.primaryStat?.battle ?? 0}");
            sb.AppendLine($"- Work: {agent.primaryStat?.work ?? 0}");
            sb.AppendLine($"- Mental: {agent.primaryStat?.mental ?? 0}");
            sb.AppendLine($"- HP: {agent.primaryStat?.hp ?? 0}");

            return sb.ToString();
        }

        public static void Postfix_LoggerPatch(AgentModel __instance)
        {
            if (__instance == null) return;

            var agentStatDetails = GetAgentStatDetails(__instance);
            Log.LogAndDebug($"Agent Details:\n{agentStatDetails}", ColorUtils.HexToColor("#f7e160"));
        }
    }
}
