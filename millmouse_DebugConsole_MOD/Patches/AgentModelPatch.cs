using System;
using System.Diagnostics;
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

        //custom printing methods:
        private static string GetDefenseDetails(DefenseInfo defenseInfo)
        {
            if (defenseInfo == null)
                return "Defense Info: Not Available";

            return
                "\n"+
                $"- R: {defenseInfo.R} ({defenseInfo.GetDefenseType(RwbpType.R)})\n" +
                $"- W: {defenseInfo.W} ({defenseInfo.GetDefenseType(RwbpType.W)})\n" +
                $"- B: {defenseInfo.B} ({defenseInfo.GetDefenseType(RwbpType.B)})\n" +
                $"- P: {defenseInfo.P} ({defenseInfo.GetDefenseType(RwbpType.P)})";
        }



        public static void Postfix_LoggerPatch(AgentModel __instance)
        {

            var originalMethod = new StackTrace().GetFrame(1).GetMethod();
            string targetClassName = originalMethod.DeclaringType?.Name ?? "Unknown Class";
            string targetMethodName = originalMethod.Name;

            //variables to print 

            //workermodel
            //agentmodel
            //unitmodel
            // Retrieve details
            string maxHp = __instance.maxHp.ToString();
            string maxMental = __instance.maxMental.ToString();
            string attackSpeed = __instance.attackSpeed.ToString();
            string agentName = __instance?.name ?? "Unknown Agent Name";
            
            // Call the method to get defense details
            string defenseDetails = GetDefenseDetails(__instance?.defense);
            string additioanlDefenseDetails = GetDefenseDetails(__instance?.additionalDef);

            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {
                Log.LogAndDebug(
                    $"Logged from class: {targetClassName}, method: {targetMethodName}," +
                    $"\nAgent Name: {agentName}," +
                    //$"\nMax HP: {maxHp},\nMax Mental: {maxMental}," +
                    //$"\nAttack Speed: {attackSpeed}"+
                    $"\nDefenseInfo: {defenseDetails}" +
                    $"\nAdditional DefenseInfo: {additioanlDefenseDetails}",

                    ColorUtils.HexToColor("#f7e160")
                );
            }


        }
    }
}