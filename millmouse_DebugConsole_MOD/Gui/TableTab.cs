using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyMod
{
    public class TableTab
    {

        private static TableTab instance;

        public TableTab()
        {
            instance = this;
        }
        public void Render()
        {
            string[] headers = { "Name", "Age", "Occupation" };
            string[,] rows = {
        { "Alice", "25", "Engineer" },
        { "Bob", "30", "Designer" },
        { "Charlie", "28", "Artist" }
    };
            GUILayout.BeginVertical();
            GUILayout.Label("Table View", GUILayout.Height(20));
            using (var scrollView = new GUILayout.ScrollViewScope(Vector2.zero, GUILayout.ExpandHeight(true)))
            {
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                foreach (string header in headers)
                {
                    GUILayout.Label(header, GUILayout.Width(100));
                }
                GUILayout.EndHorizontal();
                for (int i = 0; i < rows.GetLength(0); i++)
                {
                    GUILayout.BeginHorizontal();
                    for (int j = 0; j < rows.GetLength(1); j++)
                    {
                        GUILayout.Label(rows[i, j], GUILayout.Width(100));
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
        }

    }
}
