//using System;
//using System.Collections;
//using System.Runtime.InteropServices;
//using Harmony;
//using UnityEngine;

//namespace MyMod
//{
//    public class GameManagerPatch : MonoBehaviour
//    {

//        private readonly float logInterval = 2.0f;

//        public void Init(HarmonyInstance mod)
//        {

//            StartCoroutine(LogGameManagerAddressCoroutine());
//        }

//        private IEnumerator LogGameManagerAddressCoroutine()
//        {
//            while (true)
//            {
//                try
//                {
//                    PrintGameManagerAddress();
//                }
//                catch (Exception ex)
//                {
//                    Log.Error(ex);
//                }
//                yield return new WaitForSeconds(logInterval);
//            }
//        }

//        public static GameManager CurrentGameManager
//        {
//            get
//            {
//                try
//                {
//                    return GameManager.currentGameManager;
//                }
//                catch (Exception ex)
//                {
//                    Log.Error(ex);
//                    return null;
//                }
//            }
//        }

//        private static void PrintGameManagerAddress()
//        {
//            var gameManagerInstance = CurrentGameManager;
//            if (gameManagerInstance != null)
//            {
//                try
//                {
//                    LogDebugTabMessage("GameManager instance is NOT null.");

//                }
//                catch (Exception ex)
//                {
//                    Log.Error(ex);
//                }
//            }
//            else
//            {
//                LogDebugTabMessage("GameManager instance is null.");
//            }
//        }

//        private static void LogDebugTabMessage(string message)
//        {
//            if (Harmony_Patch.guiInstance?.debugTab != null)
//            {
//                DebugTab.AddMessage(message);
//            }
//        }
//    }
//}