using Harmony;
using MyMod;
using System.Reflection;
using System;
using UnityEngine;
using MyMod.Util.data;

public class FinishWorkSuccessfullyPatch
{
    private static readonly Type targetType = typeof(UseSkill);
    private const string targetMethodName = "FinishWorkSuccessfully";
    private const string patchMethodName = "Postfix_LoggerPatch";

    public FinishWorkSuccessfullyPatch(HarmonyInstance mod)
    {
        Patch(mod);
    }

    private void Patch(HarmonyInstance mod)
    {
        var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var myPatchMethod = typeof(FinishWorkSuccessfullyPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

        if (originalMethod == null)
        {
            Notice.instance.Send("AddSystemLog", new object[] { "<color=#edf4ff>Failed to find method: " + targetMethodName + " in class " + targetType.Name + "</color>" });
            return;
        }

        mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
    }

    public static void Postfix_LoggerPatch(UseSkill __instance)
    {
        var agent = RetrieveAgent(__instance);
        if (agent == null) return;

        var stats = RetrieveStats(agent);

        // First, send a congratulatory message if the level has changed significantly
        CongratulateOnLevelProgress(stats);

        // Send a message related to the agent and monster (excluding the progress part)
        string agentMonsterMessage = HandleAgentMonsterMessage(stats, agent, __instance);
        Notice.instance.Send("AddSystemLog", new object[] { $"<color={ColorUtils.White}>{agentMonsterMessage}</color>" });

        // Send the second message related to the progress and agent's name
        string progressMessage = HandleProgressMessage(stats, agent, __instance);
        Notice.instance.Send("AddSystemLog", new object[] { $"<color={ColorUtils.White}>{progressMessage}</color>" });
    }

    private static string HandleAgentMonsterMessage(StatStats stats, AgentModel agent, UseSkill __instance)
    {
        string agentName = agent != null ? agent.name : "Unknown Agent??";
        string monsterName = StatUtils.GetMonsterName(__instance); // Retrieve monster name from UseSkill instance
        string statName = stats.StatName; // Get the stat name (Strength, Intelligence, etc.)
        RwbpType statType = stats.RwbpType; // Get the RwbpType (Strength, Intelligence, etc.)

        // Get the color for the stat using ColorUtils
        string statColor = ColorUtils.GetStatColor(statType);

        // Apply color to agentName, monsterName, and statName using ColorUtils
        string coloredAgentName = $"<color={ColorUtils.Blue}>{agentName}</color>";  // Blue for Agent
        string coloredMonsterName = $"<color={ColorUtils.Red}>{monsterName}</color>";  // Red for Monster
        string coloredStatName = $"<color={statColor}>{statName}</color>"; // Stat's specific color

        return $"{coloredAgentName} finished Work Process with {coloredMonsterName}.";
    }


    private static AgentModel RetrieveAgent(UseSkill __instance)
    {
        var agent = StatUtils.GetAgent(__instance);
        return agent;
    }

    private static StatStats RetrieveStats(AgentModel agent)
    {
        RwbpType rwbpType = CalculateLevelExpPatch.LastRwbpType;
        string statName = StatUtils.GetStatName(rwbpType);
        float statValue = StatUtils.GetStatEXPValue(agent, rwbpType);
        int currentStatLevel = StatUtils.GetStatLevel(agent, rwbpType);
        int primaryValue = StatUtils.GetStatPrimaryValue(agent, rwbpType);
        int primaryWithExpModifier = Convert.ToInt32(Math.Round(statValue)) + primaryValue;
        string agentName = agent != null ? agent.name : "Unknown Agent??";

        int nextLevel = StatUtils.GetNextLevel(primaryWithExpModifier);
        int minExpForNextLevel = StatUtils.GetMinStatForLevel(nextLevel);

        return new StatStats(rwbpType, statName, statValue, currentStatLevel, primaryValue, primaryWithExpModifier,
                             nextLevel, minExpForNextLevel, agentName);
    }

    private static void CongratulateOnLevelProgress(StatStats stats)
    {
        int reachedLevel = AgentModel.CalculateStatLevel(stats.PrimaryWithExpModifier);
        if (stats.NextLevel >= stats.CurrentStatLevel + 2)
        {
            string message = $"{stats.AgentName} has reached level {reachedLevel} from previous level {stats.CurrentStatLevel}.";
            Notice.instance.Send("AddSystemLog", new object[] { $"<color=#edf4ff>{message}</color>" });
        }
    }

    private static string HandleProgressMessage(StatStats stats, AgentModel agent, UseSkill __instance)
    {
        string progressMessage = string.Empty;
        string agentName = agent != null ? agent.name : "Unknown Agent??";
        string monsterName = StatUtils.GetMonsterName(__instance); // Retrieve monster name from UseSkill instance
        string statName = stats.StatName; // Get the stat name (Strength, Intelligence, etc.)
        RwbpType statType = stats.RwbpType; // Get the RwbpType (Strength, Intelligence, etc.)

        // Get the color for the stat using ColorUtils
        string statColor = ColorUtils.GetStatColor(statType);

        // Apply color to agentName, monsterName, and statName using ColorUtils
        string coloredAgentName = $"<color={ColorUtils.Blue}>{agentName}</color>";  // Blue for Agent
        string coloredStatName = $"<color={statColor}>{statName}</color>"; // Stat's specific color

        if (stats.MinExpForNextLevel == 0)
        {
            progressMessage = $"Stat involved: {coloredStatName}. Progress to level {stats.NextLevel} reached. Maximum level reached.";
        }
        else
        {
            float progressToNextLevel = (float)stats.PrimaryWithExpModifier / stats.MinExpForNextLevel;
            float progressPercentage = progressToNextLevel * 100;

            if (progressPercentage <= 0)
            {
                progressMessage = $"Stat involved: {coloredStatName}. Progress to level {stats.NextLevel} not started.";
            }
            else if (progressPercentage >= 100)
            {
                progressMessage = $"Stat involved: {coloredStatName}. Progress to level {stats.NextLevel} reached. Maximum level reached.";
            }
            else
            {
                // Apply color to the progress part (green)
                string coloredProgress = $"<color={ColorUtils.Green}>Progress to next level: ({stats.PrimaryWithExpModifier} / {stats.MinExpForNextLevel}) = ({progressPercentage:F2}%)</color>";
                progressMessage = $"Stat involved: {coloredStatName}. {coloredAgentName} has reached level {stats.CurrentStatLevel} from previous level {stats.CurrentStatLevel}. {coloredProgress} close to the next level threshold.";
            }
        }

        return progressMessage;
    }

}
