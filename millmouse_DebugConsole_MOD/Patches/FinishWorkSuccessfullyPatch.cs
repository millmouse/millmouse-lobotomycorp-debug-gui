using Harmony;
using MyMod;
using System.Reflection;
using System;
using UnityEngine;
using MyMod.Util.data;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

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
            Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
            return;
        }

        mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
    }
    public static void Postfix_LoggerPatch(UseSkill __instance)
    {
        LogMethodName();

        var agent = RetrieveAgent(__instance);
        if (agent == null) return;

        var stats = RetrieveStats(agent);

        LogStatInformation(stats);

        //send notice board messages
        CongratulateOnLevelProgress(stats);
        SendProgressMessage(stats, agent, __instance);

        string progressMessage = HandleProgressMessage(stats);
        Log.LogAndDebug(progressMessage);

        LogMonsterName(__instance);

    }


    private static void SendProgressMessage(StatStats stats, AgentModel agent, UseSkill __instance)
    {
        float progressToNextLevel = (float)stats.PrimaryWithExpModifier / stats.MinExpForNextLevel;
        float progressPercentage = progressToNextLevel * 100;

        // Only send the progress message if the agent is making significant progress
        if (progressPercentage > 0 && progressPercentage < 100)
        {
            // Create the progress message using the provided stats and agent
            string progressMessage = CreateProgressMessage(stats, agent, __instance);

            // Send the progress message to the notice board with the appropriate color
            Notice.instance.Send("AddSystemLog", new object[] { $"<color={ColorUtils.MymessageColor}>{progressMessage}</color>" });
        }
        else if (progressPercentage >= 100)
        {
            string statColor = ColorUtils.GetStatColor(stats.RwbpType);
            string coloredStatName = $"<color={statColor}>{stats.StatName}</color>";

            string completedMessage = $"{agent.name} has reached the maximum level for {coloredStatName}.";
            Notice.instance.Send("AddSystemLog", new object[] { $"<color={ColorUtils.MymessageColor}>{completedMessage}</color>" });
        }
    }


    private static void LogMethodName()
    {
        Log.LogAndDebug($"Target Method: {targetMethodName}");
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

        return new StatStats(rwbpType, statName, statValue, currentStatLevel, primaryValue, primaryWithExpModifier, nextLevel, minExpForNextLevel, agentName);
    }

    private static void LogStatInformation(StatStats stats)
    {
        Log.LogAndDebug($"Current Stat Value ({stats.StatName}): {stats.StatValue}");
        Log.LogAndDebug($"Current Stat Level ({stats.StatName}): {stats.CurrentStatLevel}");
        Log.LogAndDebug($"Current Primary+Exp ({stats.StatName}): {stats.PrimaryWithExpModifier}");
        Log.LogAndDebug($"Next Level for ({stats.StatName}): {stats.NextLevel}");
        Log.LogAndDebug($"Min Exp For next level in stat ({stats.StatName}): {stats.MinExpForNextLevel}");
    }

    private static void CongratulateOnLevelProgress(StatStats stats)
    {
        int reachedLevel = AgentModel.CalculateStatLevel(stats.PrimaryWithExpModifier);

        if (stats.NextLevel >= stats.CurrentStatLevel + 2)
        {
            string message = CreateLevelUpMessage(stats, reachedLevel);
            Notice.instance.Send("AddSystemLog", new object[] { $"<color={ColorUtils.MymessageColor}>{message}</color>" });
        }
    }


    private static string HandleProgressMessage(StatStats stats)
    {
        string progressMessage;

        if (stats.MinExpForNextLevel == 0)
        {
            progressMessage = $"Progress to level {stats.NextLevel} reached. Maximum level reached.";
        }
        else
        {
            float progressToNextLevel = (float)stats.PrimaryWithExpModifier / stats.MinExpForNextLevel;
            float progressPercentage = progressToNextLevel * 100;

            if (progressPercentage <= 0)
            {
                progressMessage = $"Progress to level {stats.NextLevel} not started.";
            }
            else if (progressPercentage >= 100)
            {
                progressMessage = $"Progress to level {stats.NextLevel} reached. Maximum level reached.";
            }
            else
            {
                progressMessage = $"{stats.StatName} progress to next level: ({stats.PrimaryWithExpModifier} / {stats.MinExpForNextLevel}) = ({progressPercentage:F2}%) close to the next level threshold.";
            }
        }

        return progressMessage;
    }

    private static void LogMonsterName(UseSkill __instance)
    {
        string monsterName = StatUtils.GetMonsterName(__instance);
        Log.LogAndDebug($"Monster name: {monsterName}");
    }

    private static string CreateAgentMonsterMessage(StatStats stats, AgentModel agent, UseSkill __instance)
    {
        return $"{FormatWithColor(agent.name, ColorUtils.Blue)} finished Work Process with {FormatWithColor(StatUtils.GetMonsterName(__instance), ColorUtils.Red)}.";
    }

    private static string CreateProgressMessage(StatStats stats, AgentModel agent, UseSkill __instance)
    {
        // Get the names and their corresponding colors
        string agentName = agent != null ? agent.name : "Unknown Agent??";
        string monsterName = StatUtils.GetMonsterName(__instance);
        string statName = stats.StatName;

        // Color the agent and monster names
        string coloredAgentName = $"<color={ColorUtils.Blue}>{agentName}</color>";
        string coloredMonsterName = $"<color={ColorUtils.Red}>{monsterName}</color>";

        // Get the color for the stat name
        string statColor = ColorUtils.GetStatColor(stats.RwbpType);
        string coloredStatName = $"<color={statColor}>{statName}</color>";

        string progressMessage = string.Empty;

        if (stats.MinExpForNextLevel == 0)
        {
            progressMessage = $"{coloredAgentName} reached maximum {coloredStatName} level.";
        }
        else
        {
            float progressToNextLevel = (float)stats.PrimaryWithExpModifier / stats.MinExpForNextLevel;
            float progressPercentage = progressToNextLevel * 100;

            if (progressPercentage <= 0)
            {
                progressMessage += "not started.";
            }
            else if (progressPercentage >= 100)
            {
                progressMessage += $"reached. Maximum {coloredStatName} level reached.";
            }
            else
            {
                string coloredProgress = $"\n\n<color={ColorUtils.Green}>({stats.PrimaryWithExpModifier} / {stats.MinExpForNextLevel}) = ({progressPercentage:F2}%)</color>";
                progressMessage += $"{coloredAgentName} finished work on {monsterName}. {coloredProgress} to next {coloredStatName} level. Next Level: ({stats.NextLevel}).";
            }
        }

        // Return the complete message with the final color (MymessageColor)
        return $"<color={ColorUtils.MymessageColor}>{progressMessage}</color>";
    }


    private static string FormatWithColor(string text, string color)
    {
        return $"<color={color}>{text}</color>";
    }

    private static string CreateLevelUpMessage(StatStats stats, int reachedLevel)
    {
        // Get the appropriate color for the stat
        string statColor = ColorUtils.GetStatColor(stats.RwbpType);

        // Construct the level-up message with the stat name colored
        return $"~-~ Congratulations! ~-~ {stats.AgentName} has reached level {reachedLevel} in <color={statColor}>{stats.StatName}</color>.";
    }


    private static string CreateProgressMessageWithoutLevelPhrase(StatStats stats, int reachedLevel)
    {
        // Provide a formatted message that doesn't repeat the phrase "has reached level"
        return $"{stats.AgentName} is now at level {reachedLevel} in {stats.StatName}.";
    }
}