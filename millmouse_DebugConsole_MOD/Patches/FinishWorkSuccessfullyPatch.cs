using Harmony;
using MyMod;
using System.Reflection;
using System;

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
        float statValue = StatUtils.GetStatValue(agent, rwbpType);
        int statLevel = StatUtils.GetStatLevel(agent, rwbpType);

        // Log the relevant stat info
        Log.LogAndDebug($"Current Stat Value ({statName}): {statValue}");
        Log.LogAndDebug($"Current Stat Level ({statName}): {statLevel}");

        LogMonsterName(__instance);
    }

    private static AgentModel GetAgent(UseSkill __instance)
    {
        var agentObject = Traverse.Create(__instance).Field("agent").GetValue();
        if (agentObject == null)
        {
            Log.LogAndDebug("Agent is null or invalid.");
            return null;
        }

        var agent = agentObject as AgentModel;
        if (agent == null)
        {
            Log.LogAndDebug("Failed to cast agent.");
            return null;
        }

        return agent;
    }

    private static void LogMonsterName(UseSkill __instance)
    {
        string monsterName = StatUtils.GetMonsterName(__instance);
        Log.LogAndDebug($"Monster name: {monsterName}");
    }

}
