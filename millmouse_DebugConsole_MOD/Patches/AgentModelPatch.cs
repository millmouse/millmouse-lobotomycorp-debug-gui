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
            var originalMethod = new StackTrace().GetFrame(1)?.GetMethod();
            string targetClassName = originalMethod?.DeclaringType?.Name ?? "Unknown Class";
            string targetMethodName = originalMethod?.Name ?? "Unknown Method";

            string agentName = __instance?.name ?? "Unknown Agent Name";

            List<EGOgiftModel> gifts = __instance?.GetAllGifts();
            List<string> egogiftNames = new List<string>();
            int giftCount = 0;

            if (gifts != null && gifts.Count > 0)
            {
                foreach (var gift in gifts)
                {
                    string giftName = gift?.metaInfo?.Name ?? "Unknown Gift Name";
                    egogiftNames.Add(giftName);
                }
                giftCount = gifts.Count;
            }

            string giftNamesString = egogiftNames.Count > 0 ? string.Join(", ", egogiftNames.ToArray()) : "No Gifts Available";

            if (Harmony_Patch.guiInstance?.debugTab != null)
            {
                Log.LogAndDebug($"Logged from class: {targetClassName}, method: {targetMethodName}, Agent Name: {agentName}", ColorUtils.HexToColor("#f7e160"));
                Log.LogAndDebug($"{agentName} has {giftCount} EGO gifts: {giftNamesString}", ColorUtils.HexToColor("#f7e160"));
            }
        }
    }

}
