using Harmony;
using MyMod;
using System;
using System.Diagnostics;
using System.Reflection;

namespace MyMod.Patches
{
    public class AnyPatch
    {
        public AnyPatch(HarmonyInstance mod, Type targetType, string targetMethodName)
        {

            Type patchType = typeof(AnyPatch);
            string patchMethodName = PatchConstants.PatchMethodName;

            Patch(mod, targetType, targetMethodName, patchType, patchMethodName);
        }

        private void Patch(HarmonyInstance mod, Type targetType, string targetMethodName, Type patchType, string patchMethodName)
        {

            var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var myPatchMethod = patchType.GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

            if (originalMethod != null)
            {

                mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
            }
            else
            {

                Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
            }
        }

        public static void Postfix_LogPatch()
        {

            var originalMethod = new StackTrace().GetFrame(1).GetMethod();

            string targetClassName = originalMethod.DeclaringType?.Name ?? "Unknown Class";
            string targetMethodName = originalMethod.Name;

            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {

                Log.LogAndDebug($"Logged from class: {targetClassName}, method: {targetMethodName}");
            }
        }
    }
}