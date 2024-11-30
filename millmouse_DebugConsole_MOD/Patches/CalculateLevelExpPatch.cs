using Harmony;
using MyMod;
using System;
using System.Reflection;

public class CalculateLevelExpPatch
{
    private static readonly Type targetType = typeof(UseSkill);
    private const string targetMethodName = "CalculateLevelExp";
    private const string patchMethodName = "Postfix_LoggerPatch";
    public static RwbpType LastRwbpType { get; private set; }

    public CalculateLevelExpPatch(HarmonyInstance mod)
    {
        Patch(mod);
    }

    private void Patch(HarmonyInstance mod)
    {
        var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var myPatchMethod = typeof(CalculateLevelExpPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

        if (originalMethod == null)
        {
            Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
            return;
        }

        mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
    }

    public static void Postfix_LoggerPatch(UseSkill __instance, RwbpType rwbpType, float __result)
    {
        // Log method name for debugging purposes
        Log.LogAndDebug($"Target Method: {targetMethodName}");

        var agentObject = Traverse.Create(__instance).Field("agent").GetValue();
        if (agentObject == null)
        {
            Log.LogAndDebug("Agent is null or invalid.");
            return;
        }

        var agent = agentObject as AgentModel;
        if (agent == null)
        {
            Log.LogAndDebug("Failed to cast agent.");
            return;
        }

        // Store the RwbpType for later 
        LastRwbpType = rwbpType;

        string statName = StatUtils.GetStatName(rwbpType);
        float statValue = StatUtils.GetStatValue(agent, rwbpType);
        int statLevel = StatUtils.GetStatLevel(agent, rwbpType); // Get the corresponding stat level
        string monsterName = StatUtils.GetMonsterName(__instance);

        Log.LogAndDebug($"Agent Name: {agent.name}");
        Log.LogAndDebug($"Monster name: {monsterName}");
        Log.LogAndDebug($"RwbpType: {rwbpType} ({statName})");
        Log.LogAndDebug($"Current Stat Value: {statValue}");
        Log.LogAndDebug($"Current Stat Level: {statLevel}");
        Log.LogAndDebug($"CalculateLevelExp Result: {__result}");
    }

}
