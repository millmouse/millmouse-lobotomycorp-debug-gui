using System;
using UnityEngine;

namespace MyMod
{
    public class TableTab
    {
        private static TableTab instance;

        private AgentModel currentAgent;

        public TableTab()
        {
            instance = this;
            currentAgent = null;
        }
        public void SetAgent(AgentModel agent)
        {
            currentAgent = agent;
        }
        public void ResetAgent()
        {
            currentAgent = null;
        }

        public void Render()
        {
            string[] sections = { "HP", "Mental", "Cube Speed", "Work Prob", "Attack Speed", "Movement Speed" };
            string[] rows = { "Max", "Buff", "Ego Bonus", "Title Bonus", "Sefira Ability" };
            string[,] tableData = new string[rows.Length, sections.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < sections.Length; j++)
                {
                    tableData[i, j] = GetStatValue(rows[i], sections[j]);
                }
            }
            GUILayout.BeginVertical();
            GUILayout.Label("Agent Stats Table", GUILayout.Height(20));

            using (var scrollView = new GUILayout.ScrollViewScope(Vector2.zero, GUILayout.ExpandHeight(true)))
            {
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                GUILayout.Label("Type", GUILayout.Width(120));
                foreach (string section in sections)
                {
                    GUILayout.Label(section, GUILayout.Width(120));
                }
                GUILayout.EndHorizontal();
                for (int i = 0; i < rows.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(rows[i], GUILayout.Width(120));

                    for (int j = 0; j < sections.Length; j++)
                    {
                        GUI.color = GetColumnColor(sections[j]);
                        GUILayout.Label(tableData[i, j], GUILayout.Width(120));
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
        }
        private Color GetColumnColor(string section)
        {
            switch (section)
            {
                case "HP":
                    return Color.red;
                case "Mental":
                    return Color.yellow;
                case "Cube Speed":
                case "Work Prob":
                    return new Color(0.5f, 0, 1);
                case "Attack Speed":
                case "Movement Speed":
                    return new Color(0, 1, 1);
                default:
                    return Color.white;
            }
        }
        private string GetStatValue(string row, string section)
        {
            if (currentAgent == null)
            {
                return "N/A";
            }
            switch (section)
            {
                case "HP":
                    return GetHpValue(row);
                case "Mental":
                    return GetMentalValue(row);
                case "Cube Speed":
                    return GetCubeSpeedValue(row);
                case "Work Prob":
                    return GetWorkProbValue(row);
                case "Attack Speed":
                    return GetAttackSpeedValue(row);
                case "Movement Speed":
                    return GetMovementSpeedValue(row);
                default:
                    return "N/A";
            }
        }
        private string GetHpValue(string row)
        {
            switch (row)
            {
                case "Max": return currentAgent.maxHp.ToString();
                case "Buff": return currentAgent.GetMaxHpBuf().ToString();
                case "Ego Bonus": return currentAgent.GetEGObonus().hp.ToString();
                case "Title Bonus": return currentAgent.titleBonus.hp.ToString();
                case "Sefira Ability": return currentAgent.GetFortitudeStatBySefiraAbility().ToString();
                default: return "N/A";
            }
        }

        private string GetMentalValue(string row)
        {
            switch (row)
            {
                case "Max": return currentAgent.primaryStat.maxMental.ToString();
                case "Buff": return currentAgent.GetMaxMentalBuf().ToString();
                case "Ego Bonus": return currentAgent.GetEGObonus().mental.ToString();
                case "Title Bonus": return currentAgent.titleBonus.mental.ToString();
                case "Sefira Ability": return currentAgent.GetPrudenceStatBySefiraAbility().ToString();
                default: return "N/A";
            }
        }

        private string GetCubeSpeedValue(string row)
        {
            switch (row)
            {
                case "Max": return currentAgent.primaryStat.cubeSpeed.ToString();
                case "Buff": return currentAgent.GetCubeSpeedBuf().ToString();
                case "Ego Bonus": return currentAgent.GetEGObonus().cubeSpeed.ToString();
                case "Title Bonus": return currentAgent.titleBonus.cubeSpeed.ToString();
                case "Sefira Ability": return currentAgent.GetTemperanceStatBySefiraAbility().ToString();
                default: return "N/A";
            }
        }

        private string GetWorkProbValue(string row)
        {
            switch (row)
            {
                case "Max": return currentAgent.primaryStat.workProb.ToString();
                case "Buff": return currentAgent.GetWorkProbBuf().ToString();
                case "Ego Bonus": return currentAgent.GetEGObonus().workProb.ToString();
                case "Title Bonus": return currentAgent.titleBonus.workProb.ToString();
                case "Sefira Ability": return "N/A";
                default: return "N/A";
            }
        }

        private string GetAttackSpeedValue(string row)
        {
            switch (row)
            {
                case "Max": return currentAgent.primaryStat.attackSpeed.ToString();
                case "Buff": return currentAgent.GetAttackSpeedBuf().ToString();
                case "Ego Bonus": return currentAgent.GetEGObonus().attackSpeed.ToString();
                case "Title Bonus": return currentAgent.titleBonus.attackSpeed.ToString();
                case "Sefira Ability": return currentAgent.GetJusticeStatBySefiraAbility().ToString();
                default: return "N/A";
            }
        }

        private string GetMovementSpeedValue(string row)
        {
            switch (row)
            {
                case "Max": return currentAgent.primaryStat.movementSpeed.ToString();
                case "Buff": return currentAgent.GetWorkProbBuf().ToString();
                case "Ego Bonus": return currentAgent.GetEGObonus().movement.ToString();
                case "Title Bonus": return currentAgent.titleBonus.movementSpeed.ToString();
                case "Sefira Ability": return "N/A";
                default: return "N/A";
            }
        }

        private string GetDefenseDetails(DefenseInfo defense)
        {
            return defense?.ToString() ?? "No Defense Info";
        }
    }

}
