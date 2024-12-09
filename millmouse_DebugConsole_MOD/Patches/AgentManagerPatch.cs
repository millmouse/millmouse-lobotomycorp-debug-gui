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
        private static readonly TimeSpan logCooldown = TimeSpan.FromSeconds(30);


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

            string logMessage = GetEgoGiftListLog(__result);
            logMessage += "\n";
            logMessage += GetBestToWorstStatsListLog(__result);
            Log.LogAndDebug(logMessage);
        }

        private static string GetEgoGiftListLog(IList<AgentModel> agents)
        {
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine("AgentManagerPatch: EGO gift counts ordered by most to least:");

            var agentGiftCounts = new List<KeyValuePair<string, int>>();

            foreach (var agent in agents)
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

            return logMessage.ToString();
        }

        private static string GetBestToWorstStatsListLog(IList<AgentModel> agents)
        {
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine("AgentManagerPatch: Agent stats ordered by most to least:");

            // List to store agents and their stats
            var agentStatsList = new List<KeyValuePair<string, KeyValuePair<int, KeyValuePair<string, int>>>>();

            foreach (var agent in agents)
            {
                if (agent == null || agent.primaryStat == null)
                {
                    continue;
                }

                string agentName = agent.name ?? "Unknown Agent";

                // Retrieve stats
                int cubeSpeed = agent.primaryStat.cubeSpeed;
                int workProb = agent.primaryStat.workProb;
                int attackSpeed = agent.primaryStat.attackSpeed;
                int movementSpeed = agent.primaryStat.movementSpeed;
                int maxHP = agent.primaryStat.maxHP;
                int maxMental = agent.primaryStat.maxMental;

                int totalStats = cubeSpeed + workProb + attackSpeed + movementSpeed + maxHP + maxMental;

                string bestStatName = "Cube Speed";
                int bestStatValue = cubeSpeed;

                if (workProb > bestStatValue)
                {
                    bestStatName = "Work Prob";
                    bestStatValue = workProb;
                }
                if (attackSpeed > bestStatValue)
                {
                    bestStatName = "Attack Speed";
                    bestStatValue = attackSpeed;
                }
                if (movementSpeed > bestStatValue)
                {
                    bestStatName = "Movement Speed";
                    bestStatValue = movementSpeed;
                }
                if (maxHP > bestStatValue)
                {
                    bestStatName = "Max HP";
                    bestStatValue = maxHP;
                }
                if (maxMental > bestStatValue)
                {
                    bestStatName = "Max Mental";
                    bestStatValue = maxMental;
                }

                agentStatsList.Add(new KeyValuePair<string, KeyValuePair<int, KeyValuePair<string, int>>>(
                    agentName,
                    new KeyValuePair<int, KeyValuePair<string, int>>(
                        totalStats,
                        new KeyValuePair<string, int>(bestStatName, bestStatValue)
                    )
                ));
            }

            agentStatsList.Sort((x, y) => y.Value.Key.CompareTo(x.Value.Key));

            // Build log message
            foreach (var agentStats in agentStatsList)
            {
                string agentName = agentStats.Key;
                int totalStats = agentStats.Value.Key;
                string bestStatName = agentStats.Value.Value.Key;
                int bestStatValue = agentStats.Value.Value.Value;

                logMessage.AppendLine($"- {agentName}: Total Stats = {totalStats}, Best Stat = {bestStatName} ({bestStatValue})");
            }

            return logMessage.ToString();
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