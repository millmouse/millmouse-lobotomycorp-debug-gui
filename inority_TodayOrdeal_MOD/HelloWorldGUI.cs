using SageMod;
using UnityEngine;
using System;
using SageMod;
using UnityEngine;

namespace TodayOrdeal
{
    public class HelloWorldGUI : MonoBehaviour
    {
        private Rect windowRect;
        public DebugTab debugTab;
        private bool showWindow = true;
        private int selectedTabIndex = 0;

        private void Start()
        {
            windowRect = new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400);
            DontDestroyOnLoad(this.gameObject);
            debugTab = new DebugTab(); 
        }

        private void OnGUI()
        {
            Render();
        }

        protected void Render()
        {
            windowRect = GUI.Window(0, windowRect, DrawWindow, "Hello World GUI");
        }

        private void DrawWindow(int windowID)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Data Table"))
            {
                selectedTabIndex = 0;
            }
            if (GUILayout.Button("Debug"))
            {
                selectedTabIndex = 1;
            }

            GUILayout.EndHorizontal();

            switch (selectedTabIndex)
            {
                case 1:
                    debugTab.Render(); 
                    break;
            }

            GUI.DragWindow();
        }

        private void OnDestroy()
        {
        }
    }
}
