﻿using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyMod
{
    public static class StatUtils
    {


        public static int CalculateExpForLevel(int level)
        {
            switch (level)
            {
                case 1:
                    return 30; 
                case 2:
                    return 45; 
                case 3:
                    return 65;
                case 4:
                    return 85;
                case 5: // Level EX is gained supposedly through EGO Gifts, not leveling.
                    return 0; 
                default:
                    //throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 5.");
                    return 0;
            }
        }

        public static string GetStatName(RwbpType rwbpType)
        {
            if (rwbpType == RwbpType.R)
                return "Health (HP)";
            else if (rwbpType == RwbpType.W)
                return "Mental";
            else if (rwbpType == RwbpType.B)
                return "Work";
            else if (rwbpType == RwbpType.P)
                return "Battle";
            else
                return "Unknown";
        }

        public static float GetStatEXPValue(AgentModel agent, RwbpType rwbpType)
        {
            var primaryStatExp = agent.primaryStatExp;
            if (primaryStatExp == null)
                return 0f;

            if (rwbpType == RwbpType.R)
                return primaryStatExp.hp;
            else if (rwbpType == RwbpType.W)
                return primaryStatExp.mental;
            else if (rwbpType == RwbpType.B)
                return primaryStatExp.work; 
            else if (rwbpType == RwbpType.P)
                return primaryStatExp.battle;
            else
                return 0f;
        }

        public static int GetStatPrimaryValue(AgentModel agent, RwbpType rwbpType)
        {
            var primaryStat = agent.primaryStat;
            if (primaryStat == null)
                return 0;

            if (rwbpType == RwbpType.R)
                return primaryStat.hp;
            else if (rwbpType == RwbpType.W)
                return primaryStat.mental;
            else if (rwbpType == RwbpType.B)
                return primaryStat.work; 
            else if (rwbpType == RwbpType.P)
                return primaryStat.battle;
            else
                return 0;
        }

        public static int GetStatLevel(AgentModel agent, RwbpType rwbpType)
        {
            if (rwbpType == RwbpType.R)
                return agent.fortitudeLevel;
            else if (rwbpType == RwbpType.W)
                return agent.prudenceLevel;
            else if (rwbpType == RwbpType.B)
                return agent.temperanceLevel;
            else if (rwbpType == RwbpType.P)
                return agent.justiceLevel;
            else
                return 0;
        }

        public static string GetMonsterName(UseSkill instance)
        {
            var targetCreature = Traverse.Create(instance).Field("targetCreature").GetValue();
            if (targetCreature == null) return "Unknown Monster";

            var getUnitNameMethod = targetCreature.GetType().GetMethod("GetUnitName", BindingFlags.Public | BindingFlags.Instance);
            return getUnitNameMethod != null ? (string)getUnitNameMethod.Invoke(targetCreature, null) : "Unknown Monster";
        }

        public static AgentModel GetAgent(UseSkill __instance)
        {
            var agentObject = Traverse.Create(__instance).Field("agent").GetValue();
            if (agentObject == null)
            {
                Log.LogAndDebug("Agent is null or invalid.");
                return null;
            }

            var agent = agentObject as AgentModel;
            if (agent == null)
            {
                Log.LogAndDebug("Failed to cast agent.");
                return null;
            }

            return agent;
        }
    }
}