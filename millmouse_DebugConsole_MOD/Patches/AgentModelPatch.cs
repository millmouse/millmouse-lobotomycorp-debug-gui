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
            var originalMethod = new StackTrace().GetFrame(1).GetMethod();
            string targetClassName = originalMethod.DeclaringType?.Name ?? "Unknown Class";
            string targetMethodName = originalMethod.Name;

            string maxHp = __instance.maxHp.ToString();
            string maxHpNoMod = __instance.primaryStat.maxHP.ToString();
            string maxHPBuf = __instance.GetMaxHpBuf().ToString();
            string hpEgoBonus = __instance.GetEGObonus().hp.ToString();
            string maxHPPrimaryStatBuf = __instance.GetPrimaryStatBuf().maxHP.ToString();
            string hpTitleBonus = __instance.titleBonus.hp.ToString();
            string hpSefiraAbility = __instance.GetFortitudeStatBySefiraAbility().ToString();

            string maxMental = __instance.maxMental.ToString();
            string maxMentalNoMod = __instance.primaryStat.maxMental.ToString();
            string maxMentalBuf = __instance.GetMaxMentalBuf().ToString();
            string mentalEgoBonus = __instance.GetEGObonus().mental.ToString();
            string maxMentalPrimaryStatBuf = __instance.GetPrimaryStatBuf().maxMental.ToString();
            string mentalTitleBonus = __instance.titleBonus.mental.ToString();
            string mentalSefiraAbility = __instance.GetPrudenceStatBySefiraAbility().ToString();

            string cubeSpeed = __instance.workSpeed.ToString();
            string cubeSpeedNoMod = __instance.primaryStat.cubeSpeed.ToString();
            string cubeSpeedBuf = __instance.GetCubeSpeedBuf().ToString();
            string cubeEgoBonus = __instance.GetEGObonus().cubeSpeed.ToString();
            string cubeSpeedPrimaryStatBuf = __instance.GetPrimaryStatBuf().cubeSpeed.ToString();
            string cubeSpeedTitleBonus = __instance.titleBonus.cubeSpeed.ToString();
            string cubeSpeedSefiraAbility = __instance.GetTemperanceStatBySefiraAbility().ToString();

            string workProb = __instance.workProb.ToString();
            string workProbNoMod = __instance.primaryStat.workProb.ToString();
            string workProbBuf = __instance.GetWorkProbBuf().ToString();
            string workProbEgoBonus = __instance.GetEGObonus().workProb.ToString();
            string workProbPrimaryStatBuf = __instance.GetPrimaryStatBuf().workProb.ToString();
            string workProbTitleBonus = __instance.titleBonus.workProb.ToString();

            string attackSpeed = __instance.attackSpeed.ToString();
            string attackSpeedNoMod = __instance.primaryStat.attackSpeed.ToString();
            string attackSpeedBuf = __instance.GetAttackSpeedBuf().ToString();
            string attackSpeedEgoBonus = __instance.GetEGObonus().attackSpeed.ToString();
            string attackSpeedPrimaryStatBuf = __instance.GetPrimaryStatBuf().attackSpeed.ToString();
            string attackSpeedTitleBonus = __instance.titleBonus.attackSpeed.ToString();
            string attackSpeedSefiraAbility = __instance.GetJusticeStatBySefiraAbility().ToString();

            string movementSpeed = __instance.movement.ToString();
            string movementSpeedNoMod = __instance.primaryStat.movementSpeed.ToString();
            string movementSpeedBuf = __instance.GetWorkProbBuf().ToString();
            string movementSpeedEgoBonus = __instance.GetEGObonus().movement.ToString();
            string movementSpeedPrimaryStatBuf = __instance.GetPrimaryStatBuf().movementSpeed.ToString();
            string movementSpeedTitleBonus = __instance.titleBonus.movementSpeed.ToString();
            string agentName = __instance?.name ?? "Unknown Agent Name";
            string defenseDetails = GetDefenseDetails(__instance?.defense);

            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {
                Log.LogAndDebug(
                    $"Logged from class: {targetClassName}, method: {targetMethodName}," +
                    $"\n{{{{ HP (RED) }}}}" +
                    $"\nAgent Name: {agentName}," +
                    $"\nMax HP: {maxHp}," +
                    $"\nMax HP (No Modifier): {maxHpNoMod}," +
                    $"\nMax HP (Max HP Buf): {maxHPBuf}," +
                    $"\nHP Ego Bonus: {hpEgoBonus}," +
                    $"\nMax HP Primary Stat Buf: {maxHPPrimaryStatBuf}," +
                    $"\nHP Title Bonus: {hpTitleBonus}," +
                    $"\nHP Sefira Ability: {hpSefiraAbility}," +
                    $"\n" + new string('-', 50) +
                    $"\n{{{{ Mental (WHITE) }}}}" +
                    $"\nAgent Name: {agentName}," +
                    $"\nMax Mental: {maxMental}," +
                    $"\nMax Mental (No Modifier): {maxMentalNoMod}," +
                    $"\nMax Mental Buf: {maxMentalBuf}," +
                    $"\nMental Ego Bonus: {mentalEgoBonus}," +
                    $"\nMax Mental Primary Stat Buf: {maxMentalPrimaryStatBuf}," +
                    $"\nMental Title Bonus: {mentalTitleBonus}," +
                    $"\nMental Sefira Ability: {mentalSefiraAbility}," +
                    $"\n" + new string('-', 50) +
                    $"\n{{{{ Work (BLACK) }}}}" +
                    $"\nAgent Name: {agentName}," +
                    $"\nCube Speed: {cubeSpeed}," +
                    $"\nCube Speed (No Modifier): {cubeSpeedNoMod}," +
                    $"\nCube Speed Buf: {cubeSpeedBuf}," +
                    $"\nCube Ego Bonus: {cubeEgoBonus}," +
                    $"\nCube Speed Primary Stat Buf: {cubeSpeedPrimaryStatBuf}," +
                    $"\nCube Speed Title Bonus: {cubeSpeedTitleBonus}," +
                    $"\nCube Speed Sefira Ability: {cubeSpeedSefiraAbility}," +
                    $"\nWork Probability: {workProb}," +
                    $"\nWork Prob (No Modifier): {workProbNoMod}," +
                    $"\nWork Prob Buf: {workProbBuf}," +
                    $"\nWork Prob Ego Bonus: {workProbEgoBonus}," +
                    $"\nWork Prob Primary Stat Buf: {workProbPrimaryStatBuf}," +
                    $"\nWork Prob Title Bonus: {workProbTitleBonus}," +
                    $"\n" + new string('-', 50) +
                    $"\n{{{{ Attack/Move (PALE) }}}}" +
                    $"\nAgent Name: {agentName}," +
                    $"\nAttack Speed: {attackSpeed}," +
                    $"\nAttack Speed (No Modifier): {attackSpeedNoMod}," +
                    $"\nAttack Speed Buf: {attackSpeedBuf}," +
                    $"\nAttack Speed Ego Bonus: {attackSpeedEgoBonus}," +
                    $"\nAttack Speed Primary Stat Buf: {attackSpeedPrimaryStatBuf}," +
                    $"\nAttack Speed Title Bonus: {attackSpeedTitleBonus}," +
                    $"\nAttack Speed Sefira Ability: {attackSpeedSefiraAbility}," +
                    $"\nMovement Speed: {movementSpeed}," +
                    $"\nMovement Speed (No Modifier): {movementSpeedNoMod}," +
                    $"\nMovement Speed Buf: {movementSpeedBuf}," +
                    $"\nMovement Speed Ego Bonus: {movementSpeedEgoBonus}," +
                    $"\nMovement Speed Primary Stat Buf: {movementSpeedPrimaryStatBuf}," +
                    $"\nMovement Speed Title Bonus: {movementSpeedTitleBonus}," +
                    $"\n" + new string('-', 50),

                    ColorUtils.HexToColor("#ed9172")
                );


            }
        }
    }
}