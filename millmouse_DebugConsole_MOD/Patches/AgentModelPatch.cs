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
            details.AppendLine($"Work Days: {agentHistory.WorkDay}");

            details.AppendLine("One Day History:");
            details.Append(GetHistoryDetails(agentHistory.Oneday));

            details.AppendLine("Total History:");
            details.Append(GetHistoryDetails(agentHistory.Total));

            details.AppendLine("=================================");
            return details.ToString();
        }

        private static string GetHistoryDetails(AgentHistory.History history)
        {
            if (history == null)
            {
                return "History is null.\n";
            }

            return $@"
    Work Success: {history.workSuccess}
    Work Cube Counts: {GetWorkCubeCountsString(history.workCubeCounts)}
    Work Fail: {history.workFail}
    Physical Damage: {history.takePhysicalDamage}
    Mental Damage: {history.takeMentalDamage}
    Deaths by Creature: {history.deathByCreature}
    Panics by Creature: {history.panicByCreature}
    Deaths by Worker: {history.deathByWorker}
    Panic Count: {history.panic}
    Damage by Creatures: {history.creatureDamage}
    Damage by Workers: {history.workerDamage}
    Panic Worker Damage: {history.panicWorkerDamage}
    Suppression Damage: {history.suppressDamage}
    Disposal Count: {history.disposal}
    Promotion Value: {history.promotionVal}
";
        }

        private static string GetWorkCubeCountsString(Dictionary<RwbpType, int> workCubeCounts)
        {
            if (workCubeCounts == null || workCubeCounts.Count == 0)
            {
                return "None";
            }

            var result = new System.Text.StringBuilder();
            foreach (var kvp in workCubeCounts)
            {
                result.AppendLine($"    {kvp.Key}: {kvp.Value}");
            }

            return result.ToString();
        }

        public static void Postfix_LoggerPatch(AgentModel __instance)
        {

            var originalMethod = new StackTrace().GetFrame(1).GetMethod();
            string targetClassName = originalMethod.DeclaringType?.Name ?? "Unknown Class";
            string targetMethodName = originalMethod.Name;

            string agentName = __instance?.name ?? "Unknown Agent Name";
            //AgentHistory history = __instance.history;
            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {
                Log.LogAndDebug($"Logged from class: {targetClassName}, method: {targetMethodName}, Agent Name: {agentName}", ColorUtils.HexToColor("#f7e160"));
                if (__instance?.history != null)
                {
                    string agentHistoryDetails = GetAgentHistoryDetails(__instance.history);
                    Log.LogAndDebug($"Agent Details:\n{agentHistoryDetails}", ColorUtils.HexToColor("#f7e160"));
                }

            }
        }
    }
}