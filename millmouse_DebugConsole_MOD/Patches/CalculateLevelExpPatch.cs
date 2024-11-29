using Harmony;
using System;
using System.Reflection;

namespace MyMod.Patches
{
    public class CalculateLevelExpPatch
    {
        private static readonly Type targetType = typeof(UseSkill);
        private const string targetMethodName = "CalculateLevelExp";
        private const string patchMethodName = "Postfix_LoggerPatch";

        public CalculateLevelExpPatch(HarmonyInstance mod)
        {
            Patch(mod);
        }

        private void Patch(HarmonyInstance mod)
        {
            var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var myPatchMethod = typeof(CalculateLevelExpPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

            if (originalMethod != null)
            {
                mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
            }
            else
            {
                Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
            }
        }

        public static void Postfix_LoggerPatch(UseSkill __instance, RwbpType rwbpType, float __result)
        {
            // Access the agent field without type argument
            var agentObject = Traverse.Create(__instance).Field("agent").GetValue();

            if (agentObject is AgentModel agent)
            {
                Log.LogAndDebug($"Agent name: {agent.name}");
            }
            else
            {
                Log.LogAndDebug("Agent is null or not an AgentModel.");
            }

            Log.LogAndDebug($"CalculateLevelExp called with RwbpType: {rwbpType}");

            Log.LogAndDebug($"CalculateLevelExp returned: {__result}");
        }
    }
}
