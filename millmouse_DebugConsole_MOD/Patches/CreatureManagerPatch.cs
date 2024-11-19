using System;
using System.Diagnostics;
using System.Reflection;

using Harmony;
using MyMod;
using UnityEngine;

namespace MyMod.Patches
{
    public class CreatureManagerPatch
    {
        private static readonly Type targetType = typeof(CreatureManager);

        public CreatureManagerPatch(HarmonyInstance mod)
        {
            string targetMethodName = "RegisterCreature";
            string patchMethodName = "Postfix_LoggerPatch";
            Patch(mod, targetMethodName, patchMethodName);
        }

        private void Patch(HarmonyInstance mod, string targetMethodName, string patchMethodName)
        {
            // Targeting specifically the "RegisterCreature" method in CreatureManager.
            var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var myPatchMethod = typeof(CreatureManagerPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

            if (originalMethod != null)
            {
                mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
            }
            else
            {
                Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
            }
        }

        public static void Postfix_LoggerPatch(CreatureManager __instance, CreatureModel model)
        {
            // Log the method where the patch is applied.
            var originalMethod = new StackTrace().GetFrame(1).GetMethod();
            string targetClassName = originalMethod.DeclaringType?.Name ?? "Unknown Class";
            string targetMethodName = originalMethod.Name;

            // Use the CreatureModel to fetch the name instead of CreatureManager's name
            string creatureName = model?.GetUnitName() ?? "Unknown Creature Name";

            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {
                Log.LogAndDebug($"Logged from class: {targetClassName}, method: {targetMethodName}, Creature Name: {creatureName}", ColorUtils.HexToColor("#f7e160"));
            }
        }
    }
}
