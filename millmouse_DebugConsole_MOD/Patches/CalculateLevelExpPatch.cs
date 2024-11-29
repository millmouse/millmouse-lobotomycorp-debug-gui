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

            if (originalMethod == null)
            {
                Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
                return;
            }

            mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
        }

        public static void Postfix_LoggerPatch(UseSkill __instance, RwbpType rwbpType, float __result)
        {
            var agentObject = Traverse.Create(__instance).Field("agent").GetValue();
            string agentName = (agentObject as AgentModel)?.name ?? "Unknown Agent";
            string monsterName = GetMonsterName(__instance);
            Log.LogAndDebug($"Agent name: {agentName}");
            Log.LogAndDebug($"Monster name: {monsterName}");
            Log.LogAndDebug($"CalculateLevelExp called with RwbpType: {rwbpType}");
            Log.LogAndDebug($"CalculateLevelExp returned: {__result}");
        }

        private static string GetMonsterName(UseSkill instance)
        {
            var targetCreature = Traverse.Create(instance).Field("targetCreature").GetValue();
            if (targetCreature == null) return "Unknown Monster";

            var getUnitNameMethod = targetCreature.GetType().GetMethod("GetUnitName", BindingFlags.Public | BindingFlags.Instance);
            return getUnitNameMethod?.Invoke(targetCreature, null) as string ?? "Unknown Monster";
        }
    }
}
