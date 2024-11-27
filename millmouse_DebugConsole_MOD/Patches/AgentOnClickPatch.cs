using Harmony;
using System;
using System.Reflection;

namespace MyMod.Patches
{
    public class AgentOnClickPatch
    {
        // Class-level readonly field for the target type.
        private static readonly Type targetType = typeof(AgentModel);

        // Class-level constants for method names.
        private const string targetMethodName = "OnClick";
        private const string patchMethodName = "Postfix_LoggerPatch";  

        public AgentOnClickPatch(HarmonyInstance mod)
        {
            Patch(mod);
        }

        private void Patch(HarmonyInstance mod)
        {
            // Use the constant field for the target method name.
            var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Use the constant field for the patch method name.
            var myPatchMethod = typeof(AgentOnClickPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

            if (originalMethod != null)
            {
                mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
            }
            else
            {
                Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
            }
        }

        public static void Postfix_LoggerPatch(AgentModel __instance)
        {
            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {
                Log.LogAndDebug(
                "Printed to show that AgentOnClickPatch worked!");
            }
        }
    }
}
