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

        var agent = StatUtils.GetAgent(__instance);
        if (agent == null) return;

        RwbpType rwbpType = CalculateLevelExpPatch.LastRwbpType;
        string statName = StatUtils.GetStatName(rwbpType);
        float statValue = StatUtils.GetStatEXPValue(agent, rwbpType);
        int statLevel = StatUtils.GetStatLevel(agent, rwbpType);
        //int expNeeded = GetExpNeededForNextLevel(statLevel);
        int primaryWithExpModifier = Mathf.RoundToInt(statValue) + StatUtils.GetStatPrimaryValue(agent,rwbpType);
        int nextLevel = StatUtils.GetNextLevel(primaryWithExpModifier);
        int minExpForNextLevel = StatUtils.GetMinStatForLevel(nextLevel);


        // Log the relevant stat info
        Log.LogAndDebug($"Current Stat Value ({statName}): {statValue}");
        Log.LogAndDebug($"Current Stat Level ({statName}): {statLevel}");
        Log.LogAndDebug($"Current Primary+Exp ({statName}): {primaryWithExpModifier}");
        Log.LogAndDebug($"Next Level for ({statName}): {nextLevel}");
        Log.LogAndDebug($"Min Exp For next level in stat ({statName}): {minExpForNextLevel}");


        //Log.LogAndDebug($"EXP Needed for next level ({statName}): {expNeeded}");

        LogMonsterName(__instance);
    }

    //private static int GetExpNeededForNextLevel(int statLevel)
    //{
    //    //AgentModel.CalculateStatLevel(statValue);
    //    return StatUtils.CalculateExpForLevel(statLevel);
    //}

    private static void LogMonsterName(UseSkill __instance)
    {
        string monsterName = StatUtils.GetMonsterName(__instance);
        Log.LogAndDebug($"Monster name: {monsterName}");
    }

}