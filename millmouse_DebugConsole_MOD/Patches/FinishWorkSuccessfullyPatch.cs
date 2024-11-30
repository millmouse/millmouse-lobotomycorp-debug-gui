using Harmony;
using MyMod;
using System.Reflection;
using System;
using UnityEngine;

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
        // Log method name for debugging purposes
        Log.LogAndDebug($"Target Method: {targetMethodName}");

        // Retrieve agent and verify it exists
        var agent = StatUtils.GetAgent(__instance);
        if (agent == null) return;

        // Retrieve required stats
        RwbpType rwbpType = CalculateLevelExpPatch.LastRwbpType;
        string statName = StatUtils.GetStatName(rwbpType);
        float statValue = StatUtils.GetStatEXPValue(agent, rwbpType);
        int currentStatLevel = StatUtils.GetStatLevel(agent, rwbpType);
        int primaryValue = StatUtils.GetStatPrimaryValue(agent, rwbpType);
        int primaryWithExpModifier = Mathf.RoundToInt(statValue) + primaryValue;

        // Determine next level based on Primary+Exp
        int nextLevel = StatUtils.GetNextLevel(primaryWithExpModifier);
        int minExpForNextLevel = StatUtils.GetMinStatForLevel(nextLevel);

        // Log relevant stat information
        Log.LogAndDebug($"Current Stat Value ({statName}): {statValue}");
        Log.LogAndDebug($"Current Stat Level ({statName}): {currentStatLevel}");
        Log.LogAndDebug($"Current Primary+Exp ({statName}): {primaryWithExpModifier}");
        Log.LogAndDebug($"Next Level for ({statName}): {nextLevel}");
        Log.LogAndDebug($"Min Exp For next level in stat ({statName}): {minExpForNextLevel}");

        // Congratulate if next level is higher than current level
        int reachedLevel = AgentModel.CalculateStatLevel(primaryWithExpModifier);
        if (nextLevel >= currentStatLevel + 2)
        {
            Log.LogAndDebug($"Congratulations! {statName} has reached level {reachedLevel}, from previous level {currentStatLevel}.");
        }

        // Handle edge cases for progressPercentage
        string progressMessage;
        if (minExpForNextLevel == 0) // Avoid division by zero
        {
            progressMessage = $"Progress to level {nextLevel} reached. Maximum level reached.";
        }
        else
        {
            float progressToNextLevel = (float)primaryWithExpModifier / minExpForNextLevel;
            float progressPercentage = progressToNextLevel * 100;

            if (progressPercentage <= 0)
            {
                progressMessage = $"Progress to level {nextLevel} not started.";
            }
            else if (progressPercentage >= 100)
            {
                progressMessage = $"Progress to level {nextLevel} reached. Maximum level reached.";
            }
            else
            {
                progressMessage = $"{statName} progress to next level: ({primaryWithExpModifier} / {minExpForNextLevel}) = ({progressPercentage:F2}%) close to the next level threshold.";
            }
        }

        // Log the progress message
        Log.LogAndDebug(progressMessage);

        // Log the monster name if needed
        LogMonsterName(__instance);
    }




    private static void LogMonsterName(UseSkill __instance)
    {
        string monsterName = StatUtils.GetMonsterName(__instance);
        Log.LogAndDebug($"Monster name: {monsterName}");
    }

}