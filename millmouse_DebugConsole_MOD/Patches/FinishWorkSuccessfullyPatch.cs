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

        CongratulateOnLevelProgress(stats);

        string progressMessage = HandleProgressMessage(stats, agent, __instance);
        Notice.instance.Send("AddSystemLog", new object[] { $"<color=#edf4ff>{progressMessage}</color>" });

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

        if (stats.MinExpForNextLevel == 0)
        {
            progressMessage = $"{agentName} finished Work Process with {monsterName}. Progress to level {stats.NextLevel} reached. Maximum level reached.";
        }
        else
        {
            float progressToNextLevel = (float)stats.PrimaryWithExpModifier / stats.MinExpForNextLevel;
            float progressPercentage = progressToNextLevel * 100;

            if (progressPercentage <= 0)
            {
                progressMessage = $"{agentName} finished Work Process with {monsterName}. Progress to level {stats.NextLevel} not started.";
            }
            else if (progressPercentage >= 100)
            {
                progressMessage = $"{agentName} finished Work Process with {monsterName}. Progress to level {stats.NextLevel} reached. Maximum level reached.";
            }
            else
            {
                progressMessage = $"{agentName} finished Work Process with {monsterName}. {agentName} has reached level {stats.CurrentStatLevel} from previous level {stats.CurrentStatLevel}. Progress to next level: ({stats.PrimaryWithExpModifier} / {stats.MinExpForNextLevel}) = ({progressPercentage:F2}%) close to the next level threshold.";
            }
        }

        return progressMessage;
    }

}
