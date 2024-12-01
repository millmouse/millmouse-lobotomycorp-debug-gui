using System;
using System.Collections.Generic;
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

        public static string GetAgentHistoryDetails(AgentHistory agentHistory)
        {
            if (agentHistory == null)
            {
                return "AgentHistory is null.";
            }

            var details = new System.Text.StringBuilder();
            details.AppendLine("===== Agent History Details =====");
            details.AppendLine("\nOne Day History:");
            details.Append(GetHistoryDetails(agentHistory.Oneday));
            details.AppendLine("\nTotal History:");
            details.Append(GetHistoryDetails(agentHistory.Total));
            details.AppendLine("=================================");
            return details.ToString();
        }

        private static string GetHistoryDetails(AgentHistory.History history)
        {
            if (history == null)
            {
                return "History is null.";
            }

            return $@"
Work Cube Counts:
{GetWorkCubeCountsString(history.workCubeCounts)}

Promotion Value: {history.promotionVal}
";
        }


        private static string GetWorkCubeCountsString(Dictionary<RwbpType, int> workCubeCounts)
        {
            if (workCubeCounts == null || workCubeCounts.Count == 0)
            {
                return "None";
            }

            var order = new List<RwbpType> { RwbpType.R, RwbpType.W, RwbpType.B, RwbpType.P };

            var result = new System.Text.StringBuilder();
            bool first = true;

            foreach (var key in order)
            {
                if (workCubeCounts.ContainsKey(key))
                {
                    if (!first)
                    {
                        result.AppendLine();
                    }
                    result.Append($"{key}: {workCubeCounts[key]}");
                    first = false;
                }
            }

            return result.Length > 0 ? result.ToString() : "None";
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

            var originalMethod = new StackTrace().GetFrame(1).GetMethod();
            string targetClassName = originalMethod.DeclaringType?.Name ?? "Unknown Class";
            string targetMethodName = originalMethod.Name;

            string agentName = __instance?.name ?? "Unknown Agent Name";
            //if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            //{
            //    Log.LogAndDebug($"Logged from class: {targetClassName}, method: {targetMethodName}, Agent Name: {agentName}", ColorUtils.HexToColor("#f7e160"));
            //    if (__instance?.history != null)
            //    {
            //        string agentStatDetails = GetAgentStatDetails(__instance);
            //        Log.LogAndDebug($"Agent Details:\n{agentStatDetails}", ColorUtils.HexToColor("#f7e160"));
            //    }

            //}
        }

    }
}