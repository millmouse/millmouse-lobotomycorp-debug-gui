using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyMod
{
    public static class StatUtils
    {
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

        public static float GetStatValue(AgentModel agent, RwbpType rwbpType)
        {
            var primaryStatExp = Traverse.Create(agent).Field("primaryStatExp").GetValue();
            if (primaryStatExp == null)
                return 0f;

            if (rwbpType == RwbpType.R)
                return Traverse.Create(primaryStatExp).Field("hp").GetValue<float>();
            else if (rwbpType == RwbpType.W)
                return Traverse.Create(primaryStatExp).Field("mental").GetValue<float>();
            else if (rwbpType == RwbpType.B)
                return Traverse.Create(primaryStatExp).Field("work").GetValue<float>();
            else if (rwbpType == RwbpType.P)
                return Traverse.Create(primaryStatExp).Field("battle").GetValue<float>();
            else
                return 0f;
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
