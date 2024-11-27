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
        private static string GetDefenseDetails(DefenseInfo defenseInfo)
        {
            if (defenseInfo == null)
                return "Defense Info: Not Available";

            return
                "\n" +
                $"- R: {defenseInfo.R} ({defenseInfo.GetDefenseType(RwbpType.R)})\n" +
                $"- W: {defenseInfo.W} ({defenseInfo.GetDefenseType(RwbpType.W)})\n" +
                $"- B: {defenseInfo.B} ({defenseInfo.GetDefenseType(RwbpType.B)})\n" +
                $"- P: {defenseInfo.P} ({defenseInfo.GetDefenseType(RwbpType.P)})";
        }

        private static string GetStatDetails(UnitStatBuf statBuf)
        {
            if (statBuf == null)
                return "UnitStatBuf Info: Not Available";

            return
                "\n" +
                $"- Max HP: {statBuf.maxHp}\n" +
                $"- Max Mental: {statBuf.maxMental}\n" +
                $"- Cube Speed: {statBuf.cubeSpeed}\n" +
                $"- Work Probability: {statBuf.workProb}\n" +
                $"- Movement Speed: {statBuf.movementSpeed}\n" +
                $"- Attack Speed: {statBuf.attackSpeed}";
        }




        public static void Postfix_LoggerPatch(AgentModel __instance)
        {



        }
    }
}