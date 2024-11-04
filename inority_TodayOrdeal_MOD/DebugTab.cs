using System;
using System.Collections.Generic;
using UnityEngine;

namespace SageMod
{
    public class DebugTab
    {
        private List<string> debugMessages;
        private Queue<string> messageQueue;
        private bool autoScroll = true;
        private static DebugTab instance;
        private float throttleInterval = 2.0f;
        private float lastMessageTime;
        private Vector2 scrollPosition; 

        public string Name => "Debug";

        public DebugTab()
        {
            debugMessages = new List<string>();
            messageQueue = new Queue<string>();
            instance = this;
        }

        public static void AddMessage(string message)
        {
            instance?.AddDebugMessage(message);
        }

        private void AddDebugMessage(string message)
        {
            messageQueue.Enqueue(message);
        }

        public void Render()
        {
            ThrottleMessages();

            Rect boxRect = new Rect(0, 0, 600, 300);

            GUI.Box(boxRect, "", GUIStyle.none); 
            GUI.backgroundColor = Color.black; 

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(600), GUILayout.Height(300));
            foreach (var message in debugMessages)
            {
                GUILayout.Label(message);
            }
            GUILayout.EndScrollView();

            autoScroll = GUILayout.Toggle(autoScroll, "Auto-scroll");

            if (autoScroll && debugMessages.Count > 0)
            {
                scrollPosition.y = Mathf.Infinity;
            }
        }


        private void ThrottleMessages()
        {
            float currentTime = Time.time;
            if (currentTime - lastMessageTime >= throttleInterval && messageQueue.Count > 0)
            {
                lastMessageTime = currentTime;
                string nextMessage = messageQueue.Dequeue();
                string timestampedMessage = $"{DateTime.Now:HH:mm:ss} - {nextMessage}";
                debugMessages.Add(timestampedMessage);
            }
        }
    }
}
