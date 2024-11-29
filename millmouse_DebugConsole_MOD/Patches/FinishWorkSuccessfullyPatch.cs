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

        var agent = GetAgent(__instance);
        if (agent == null) return;

        // Only log the stat related to the RwbpType stored in CalculateLevelExpPatch
        RwbpType rwbpType = CalculateLevelExpPatch.LastRwbpType;
        string statName = GetStatName(rwbpType);
        float statValue = GetStatValue(agent, rwbpType);
        int statLevel = GetStatLevel(agent, rwbpType);

        // Log the relevant stat info
        Log.LogAndDebug($"Current Stat Value ({statName}): {statValue}");
        Log.LogAndDebug($"Current Stat Level ({statName}): {statLevel}");

        // Log the monster name
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
        string monsterName = GetMonsterName(__instance);
        Log.LogAndDebug($"Monster name: {monsterName}");
    }

    private static string GetMonsterName(UseSkill instance)
    {
        var targetCreature = Traverse.Create(instance).Field("targetCreature").GetValue();
        if (targetCreature == null) return "Unknown Monster";

        var getUnitNameMethod = targetCreature.GetType().GetMethod("GetUnitName", BindingFlags.Public | BindingFlags.Instance);
        return getUnitNameMethod?.Invoke(targetCreature, null) as string ?? "Unknown Monster";
    }

    private static string GetStatName(RwbpType rwbpType)
    {
        if (rwbpType == RwbpType.R)
            return "Health (HP)";
        else if (rwbpType == RwbpType.W)
            return "Mental";
        else if (rwbpType == RwbpType.B)
            return "Work";
        else if (rwbpType == RwbpType.P)
            return "Battle";
        else
            return "Unknown";
    }

    private static float GetStatValue(AgentModel agent, RwbpType rwbpType)
    {
        var primaryStatExp = Traverse.Create(agent).Field("primaryStatExp").GetValue();

        if (primaryStatExp == null)
            return 0f;

        if (rwbpType == RwbpType.R)
            return Traverse.Create(primaryStatExp).Field("hp").GetValue<float>();
        else if (rwbpType == RwbpType.W)
            return Traverse.Create(primaryStatExp).Field("mental").GetValue<float>();
        else if (rwbpType == RwbpType.B)
            return Traverse.Create(primaryStatExp).Field("work").GetValue<float>();
        else if (rwbpType == RwbpType.P)
            return Traverse.Create(primaryStatExp).Field("battle").GetValue<float>();
        else
            return 0f;
    }

    private static int GetStatLevel(AgentModel agent, RwbpType rwbpType)
    {
        if (rwbpType == RwbpType.R)
            return agent.fortitudeLevel;
        else if (rwbpType == RwbpType.W)
            return agent.prudenceLevel;
        else if (rwbpType == RwbpType.B)
            return agent.temperanceLevel;
        else if (rwbpType == RwbpType.P)
            return agent.justiceLevel;
        else
            return 0;
    }
}
