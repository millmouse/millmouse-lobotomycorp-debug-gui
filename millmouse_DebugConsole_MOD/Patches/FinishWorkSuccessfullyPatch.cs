using Harmony;
using System;
using System.Reflection;

namespace MyMod.Patches
{
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

        // Postfix method for the parameterless method
        public static void Postfix_LoggerPatch(UseSkill __instance)
        {
            // Log method name first
            LogMethodName();

            // Gather the agent and log relevant information
            var agent = GetAgent(__instance);
            if (agent == null) return;

            // Log stat values and stat levels for each RwbpType (Health, Mental, Work, Battle)
            LogStatInfo(agent);

            // Log the monster name
            LogMonsterName(__instance);
        }

        // Log the target method name
        private static void LogMethodName()
        {
            Log.LogAndDebug($"Target Method: {targetMethodName}");
        }

        // Get the agent from the UseSkill instance
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

        // Log stat values and levels for each RwbpType
        private static void LogStatInfo(AgentModel agent)
        {
            foreach (RwbpType statType in Enum.GetValues(typeof(RwbpType)))
            {
                string statName = GetStatName(statType);

                // Only log if the statName is not null (i.e., it's a valid stat)
                if (statName != null)
                {
                    float statValue = GetStatValue(agent, statType);
                    int statLevel = GetStatLevel(agent, statType);

                    // Log the values
                    Log.LogAndDebug($"Current Stat Value ({statName}): {statValue}");
                    Log.LogAndDebug($"Current Stat Level ({statName}): {statLevel}");
                }
            }
        }

        // Log the monster name
        private static void LogMonsterName(UseSkill __instance)
        {
            string monsterName = GetMonsterName(__instance);
            Log.LogAndDebug($"Monster name: {monsterName}");
        }

        // Method to retrieve the monster's name
        private static string GetMonsterName(UseSkill instance)
        {
            var targetCreature = Traverse.Create(instance).Field("targetCreature").GetValue();
            if (targetCreature == null) return "Unknown Monster";

            var getUnitNameMethod = targetCreature.GetType().GetMethod("GetUnitName", BindingFlags.Public | BindingFlags.Instance);
            return getUnitNameMethod != null ? (string)getUnitNameMethod.Invoke(targetCreature, null) : "Unknown Monster";
        }

        // Method to get the stat name from the RwbpType
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
                return null;  // Return null for unknown RwbpType
        }

        // Method to get the stat value from the agent's data
        private static float GetStatValue(AgentModel agent, RwbpType rwbpType)
        {
            var primaryStatExp = Traverse.Create(agent).Field("primaryStatExp").GetValue();

            if (primaryStatExp == null)
                return 0f; // Default to 0 if the field is not accessible.

            if (rwbpType == RwbpType.R)
                return Traverse.Create(primaryStatExp).Field("hp").GetValue<float>();
            else if (rwbpType == RwbpType.W)
                return Traverse.Create(primaryStatExp).Field("mental").GetValue<float>();
            else if (rwbpType == RwbpType.B)
                return Traverse.Create(primaryStatExp).Field("work").GetValue<float>();
            else if (rwbpType == RwbpType.P)
                return Traverse.Create(primaryStatExp).Field("battle").GetValue<float>();
            else
                return 0f; // Default for unknown types.
        }

        // Method to get the stat level from the agent's data
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
}
