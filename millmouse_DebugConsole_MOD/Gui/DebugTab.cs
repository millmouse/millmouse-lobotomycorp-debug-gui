//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace MyMod
//{
//    public class DebugTab
//    {
//        private List<DebugMessage> debugMessages;
//        private bool autoScroll = true;
//        private static DebugTab instance;
//        private Vector2 scrollPosition;
//        private MessageDispatcher<DebugMessage> messageDispatcher;

//        public string Name => "Debug";

//        public DebugTab()
//        {
//            debugMessages = new List<DebugMessage>();
//            instance = this;

//            messageDispatcher = new MessageDispatcher<DebugMessage>(0.2f, AddToDebugMessages);
//        }

//        public static void AddMessage(string message)
//        {
//            instance?.AddDebugMessage(message, Color.white);
//        }

//        public static void AddMessage(string message, Color color)
//        {
//            instance?.AddDebugMessage(message, color);
//        }

//        private void AddDebugMessage(string message, Color color)
//        {
//            string timestampedMessage = $"{DateTime.Now:HH:mm:ss} - {message}";
//            var newMessage = new DebugMessage(timestampedMessage, color);

//            messageDispatcher.Enqueue(newMessage);
//        }

//        public void Render()
//        {
//            messageDispatcher.ProcessQueue();

//            Rect boxRect = new Rect(0, 0, 600, 300);
//            GUI.Box(boxRect, "", GUIStyle.none);
//            GUI.backgroundColor = Color.black;

//            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(600), GUILayout.Height(300));

//            foreach (var debugMessage in debugMessages)
//            {
//                GUI.contentColor = debugMessage.Color;
//                GUILayout.Label(debugMessage.Message);
//            }

//            GUILayout.EndScrollView();

//            autoScroll = GUILayout.Toggle(autoScroll, "Auto-scroll");

//            if (autoScroll && debugMessages.Count > 0)
//            {
//                scrollPosition.y = Mathf.Infinity;
//            }
//        }

//        private void AddToDebugMessages(DebugMessage message)
//        {
//            // Avoid duplicates if necessary.
//            if (!debugMessages.Exists(dm => dm.Message == message.Message && dm.Color == message.Color))
//            {
//                debugMessages.Add(message);
//            }
//        }
//    }
//}
