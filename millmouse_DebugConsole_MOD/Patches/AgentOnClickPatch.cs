using Harmony;
using System;
using System.Reflection;

namespace MyMod.Patches
{
    public class AgentOnClickPatch
    {
        private static readonly Type targetType = typeof(AgentModel);
        private const string targetMethodName = "OnClick";
        private const string patchMethodName = "Postfix_LoggerPatch";  

        public AgentOnClickPatch(HarmonyInstance mod)
        {
            Patch(mod);
        }

        private void Patch(HarmonyInstance mod)
        {
            var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
            if (!IsTableTabActive())
                return;

            Log.LogAndDebug("Printed to show that AgentOnClickPatch called!");
            Log.LogAndDebug("TableTab is initialized and active.");
            Harmony_Patch.guiInstance.tableTab.SetAgent(__instance);

        }

        private static bool IsTableTabActive()
        {
            if (Harmony_Patch.guiInstance.tableTab == null)
            {
                Log.LogAndDebug("TableTab is not initialized.");
                return false;
            }

            return true;
        }
    }
}
