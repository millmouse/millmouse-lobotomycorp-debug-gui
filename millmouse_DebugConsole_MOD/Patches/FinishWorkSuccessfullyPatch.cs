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
        LogMethodName();

        var agent = RetrieveAgent(__instance);
        if (agent == null) return;

        var stats = RetrieveStats(agent);

        LogStatInformation(stats);

        CongratulateOnLevelProgress(stats);

        string progressMessage = HandleProgressMessage(stats);
        Notice.instance.Send("AddSystemLog", new object[] { "<color=#edf4ff>" + progressMessage + "</color>" });

        LogMonsterName(__instance);
    }

    private static void LogMethodName()
    {
        Notice.instance.Send("AddSystemLog", new object[] { "<color=#edf4ff>Target Method: " + targetMethodName + "</color>" });
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

    private static void LogStatInformation(StatStats stats)
    {
        Notice.instance.Send("AddSystemLog", new object[] { $"<color=#edf4ff>{stats.AgentName}: Next Level for ({stats.StatName}): {stats.NextLevel}</color>" });
        Notice.instance.Send("AddSystemLog", new object[] { $"<color=#edf4ff>{stats.AgentName}: Min Exp For next level in stat ({stats.StatName}): {stats.MinExpForNextLevel}</color>" });
    }

    private static void CongratulateOnLevelProgress(StatStats stats)
    {
        int reachedLevel = AgentModel.CalculateStatLevel(stats.PrimaryWithExpModifier);
        if (stats.NextLevel >= stats.CurrentStatLevel + 2)
        {
            Notice.instance.Send("AddSystemLog", new object[] { "<color=#edf4ff>Congratulations! " + stats.StatName + " has reached level " + reachedLevel + ", from previous level " + stats.CurrentStatLevel + ".</color>" });
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
        Notice.instance.Send("AddSystemLog", new object[] { "<color=#edf4ff>Monster name: " + monsterName + "</color>" });
    }

}
