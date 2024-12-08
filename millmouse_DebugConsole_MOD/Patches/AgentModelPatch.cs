using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Harmony;
using MyMod;
using UnityEngine;

namespace MyMod.Patches
{
    public class AgentModelPatch
    {

        private static readonly Type targetType = typeof(AgentModel);

        public AgentModelPatch(HarmonyInstance mod, string targetMethodName)
        {
            string patchMethodName = "Postfix_LoggerPatch";
            Patch(mod, targetMethodName, patchMethodName);
        }

        private void Patch(HarmonyInstance mod, string targetMethodName, string patchMethodName)
        {

            var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var myPatchMethod = typeof(AgentModelPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

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
            // Get the original method's information
            var originalMethod = new StackTrace().GetFrame(1)?.GetMethod();
            string targetClassName = originalMethod?.DeclaringType?.Name ?? "Unknown Class";
            string targetMethodName = originalMethod?.Name ?? "Unknown Method";

            string agentName = __instance?.name ?? "Unknown Agent Name";

            // Get all gifts safely
            List<EGOgiftModel> gifts = __instance?.GetAllGifts();

            // Get the first gift and its meta-info safely
            string egogiftName = "No Gift Available";
            if (gifts != null && gifts.Count > 0)
            {
                var gift = gifts.FirstOrDefault();
                var metaInfoOfGift = gift?.metaInfo;
                egogiftName = metaInfoOfGift?.Name ?? "Unknown Gift Name";
            }

            if (Harmony_Patch.guiInstance?.debugTab != null)
            {
                Log.LogAndDebug($"Logged from class: {targetClassName}, method: {targetMethodName}, Agent Name: {agentName}", ColorUtils.HexToColor("#f7e160"));
                Log.LogAndDebug($"{agentName}'s first or default ego: {egogiftName}", ColorUtils.HexToColor("#f7e160"));
            }
        }
    }

}