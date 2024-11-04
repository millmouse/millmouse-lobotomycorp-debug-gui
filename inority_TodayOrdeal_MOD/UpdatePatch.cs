using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace MyMod
{
    public class UpdatePatch : MonoBehaviour
    {
        private float messageInterval = 2.0f;
        private float nextMessageTime;

        public void Init(HarmonyInstance mod)
        {
            Patch(mod);
            nextMessageTime = Time.time + messageInterval;
            StartCoroutine(Tick());
        }

        private void Patch(HarmonyInstance mod)
        {
            string[] methodNames = { "GetStageRank", "RestartGame", "StartGame" };
            Type[][] parameterTypes = {
                new[] { typeof(float) },
                new Type[0],
                new Type[0]
            };

            for (int i = 0; i < methodNames.Length; i++)
            {
                var originalMethod = typeof(GameManager).GetMethod(methodNames[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes[i], null);
                if (originalMethod != null)
                {
                    Log.Error($"{methodNames[i]} method found and patched.");
                    mod.Patch(originalMethod, null, new HarmonyMethod(typeof(UpdatePatch).GetMethod("Postfix_" + methodNames[i])), null);
                }
                else
                {
                    LogErrorAndMessage($"Failed to find method: {methodNames[i]}");
                }
            }

            CheckPrivateField("stageEnded");
            CheckPrivateField("elapsed");
        }

        private void CheckPrivateField(string fieldName)
        {
            var fieldInfo = typeof(GameManager).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Log.Error($"Private variable '{fieldName}' found.");
                LogDebugTabMessage($"Private variable '{fieldName}' found in GameManager.");
            }
            else
            {
                Log.Error($"Private variable '{fieldName}' not found.");
            }
        }

        private System.Collections.IEnumerator Tick()
        {
            while (true)
            {
                if (Time.time >= nextMessageTime)
                {
                    foreach (var methodName in new[] { "GetStageRank", "RestartGame", "StartGame" })
                    {
                        CheckMethodAndLog(methodName);
                    }
                    nextMessageTime += messageInterval;
                }
                yield return null;
            }
        }

        private void CheckMethodAndLog(string methodName)
        {
            var originalMethod = typeof(GameManager).GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (originalMethod != null)
            {
                Log.Error($"{methodName} method found.");
                LogDebugTabMessage($"{methodName} method found.");
            }
            else
            {
                LogErrorAndMessage($"Failed to find method: {methodName}");
            }
        }

        private void LogErrorAndMessage(string message)
        {
            Log.Error(message);
            LogDebugTabMessage(message);
        }

        private void LogDebugTabMessage(string message)
        {
            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {
                DebugTab.AddMessage(message);
            }
        }

        public static void Postfix_GetStageRank(float rate)
        {
            LogPostfixMethod("GetStageRank", rate.ToString());
        }

        public static void Postfix_RestartGame()
        {
            LogPostfixMethod("RestartGame");
        }

        public static void Postfix_StartGame()
        {
            LogPostfixMethod("StartGame");
        }

        private static void LogPostfixMethod(string methodName, string additionalInfo = null)
        {
            var message = additionalInfo != null ? $"{methodName} called with rate: {additionalInfo}" : $"{methodName} called.";
            Log.Error(message);
            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {
                DebugTab.AddMessage(message);
            }
        }
    }
}
