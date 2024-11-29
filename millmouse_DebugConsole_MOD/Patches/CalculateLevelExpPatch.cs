using Harmony;
using MyMod;
using System;
using System.Reflection;

public class CalculateLevelExpPatch
{
    private static readonly Type targetType = typeof(UseSkill);
    private const string targetMethodName = "CalculateLevelExp";
    private const string patchMethodName = "Postfix_LoggerPatch";

    // Static field to store the RwbpType
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

        // Get the RwbpType stat and its corresponding value
        string statName = GetStatName(rwbpType);
        float statValue = GetStatValue(agent, rwbpType);
        int statLevel = GetStatLevel(agent, rwbpType); // Get the corresponding stat level
        string monsterName = GetMonsterName(__instance);

        // Log the gathered information
        Log.LogAndDebug($"Agent Name: {agent.name}");
        Log.LogAndDebug($"Monster name: {monsterName}");
        Log.LogAndDebug($"RwbpType: {rwbpType} ({statName})");
        Log.LogAndDebug($"Current Stat Value: {statValue}");
        Log.LogAndDebug($"Current Stat Level: {statLevel}");
        Log.LogAndDebug($"CalculateLevelExp Result: {__result}");
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
        // Retrieve the corresponding stat level based on RwbpType
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
