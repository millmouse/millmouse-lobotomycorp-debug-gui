using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyMod
{
    public class DebugTab
    {
        private List<DebugMessage> debugMessages;
        private Queue<DebugMessage> messageQueue; // Change queue to hold DebugMessage objects
        private bool autoScroll = true;
        private static DebugTab instance;
        private float throttleInterval = 0.5f;
        private float lastMessageTime;
        private Vector2 scrollPosition;

        public string Name => "Debug";

        public DebugTab()
        {
            debugMessages = new List<DebugMessage>();
            messageQueue = new Queue<DebugMessage>();
            instance = this;
        }

        public static void AddMessage(string message)
        {
            instance?.AddDebugMessage(message, Color.white);
        }

        public static void AddMessage(string message, Color color)
        {
            instance?.AddDebugMessage(message, color);
        }

        private void AddDebugMessage(string message, Color color)
        {
            string timestampedMessage = $"{DateTime.Now:HH:mm:ss} - {message}";

            // Only add the message if it's not already in the queue or debugMessages
            if (!messageQueue.Contains(new DebugMessage(timestampedMessage, color)) &&
                !debugMessages.Exists(dm => dm.Message == timestampedMessage && dm.Color == color))
            {
                messageQueue.Enqueue(new DebugMessage(timestampedMessage, color));
            }
        }

        public void Render()
        {
            ThrottleMessages();

            Rect boxRect = new Rect(0, 0, 600, 300);

            GUI.Box(boxRect, "", GUIStyle.none);
            GUI.backgroundColor = Color.black;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(600), GUILayout.Height(300));

            foreach (var debugMessage in debugMessages)
            {
                GUI.contentColor = debugMessage.Color;
                GUILayout.Label(debugMessage.Message);
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

            // Process a new message only if the throttle interval has passed
            if (currentTime - lastMessageTime >= throttleInterval && messageQueue.Count > 0)
            {
                lastMessageTime = currentTime;
                DebugMessage nextMessage = messageQueue.Dequeue();

                // Add the message with color to debugMessages after the throttle interval
                debugMessages.Add(nextMessage);
            }
        }
    }

    public class DebugMessage
    {
        public string Message { get; }
        public Color Color { get; }

        public DebugMessage(string message, Color color)
        {
            Message = message;
            Color = color;
        }

        // Override Equals and GetHashCode to handle message comparisons correctly in collections
        public override bool Equals(object obj)
        {
            if (obj is DebugMessage other)
            {
                return Message == other.Message && Color == other.Color;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Message.GetHashCode() ^ Color.GetHashCode();
        }
    }
}
