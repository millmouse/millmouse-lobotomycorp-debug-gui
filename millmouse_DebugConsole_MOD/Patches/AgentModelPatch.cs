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

        private static string GetStatProgressDetails(AgentModel agent)
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

            // For each stat (Battle, Work, Mental, HP), calculate and log progress
            sb.AppendLine("Primary Stat Progress:");
            sb.AppendLine(FormatStatProgress("HP", agent, RwbpType.R));
            sb.AppendLine(FormatStatProgress("Mental", agent, RwbpType.W));
            sb.AppendLine(FormatStatProgress("Work", agent, RwbpType.B));
            sb.AppendLine(FormatStatProgress("Battle", agent, RwbpType.P));
            sb.AppendLine("\n```\n");

            return sb.ToString();
        }

        private static string FormatStatProgress(string statName, AgentModel agent, RwbpType rwbpType)
        {
            // Retrieve the relevant stat data directly
            string statDisplayName = StatUtils.GetStatName(rwbpType);
            float statValue = StatUtils.GetStatEXPValue(agent, rwbpType);
            int primaryValue = StatUtils.GetStatPrimaryValue(agent, rwbpType);
            int primaryWithExpModifier = Convert.ToInt32(Math.Round(statValue)) + primaryValue;

            int nextLevel = StatUtils.GetNextLevel(primaryWithExpModifier);
            int minExpForNextLevel = StatUtils.GetMinStatForLevel(nextLevel);

            // Calculate progress percentage
            float progressPercentage = 0;
            if (minExpForNextLevel > 0)
            {
                progressPercentage = (primaryWithExpModifier / (float)minExpForNextLevel) * 100;
            }

            // Format the progress string
            string progress = progressPercentage == 0
                ? $"({primaryWithExpModifier} / {minExpForNextLevel})"
                : $"({primaryWithExpModifier} / {minExpForNextLevel}) = ({progressPercentage:F2}%)";

            return $"{statDisplayName} Progress: {progress}";
        }

        public static void Postfix_LoggerPatch(AgentModel __instance)
        {
            if (__instance == null) return;

            var statProgressDetails = GetStatProgressDetails(__instance);
            Log.LogAndDebug($"```\nAgent {__instance.name}'s Stat Progress:\n{statProgressDetails}");
        }
    }
}
