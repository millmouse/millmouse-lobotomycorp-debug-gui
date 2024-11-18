//using System;
//using System.Collections;
//using System.Runtime.InteropServices;
//using Harmony;
//using UnityEngine;

//namespace MyMod
//{
//    public class GameManagerPatch : MonoBehaviour
//    {
//        // This interval defines how often we log the GameManager address (in seconds).
//        private readonly float logInterval = 2.0f;

//        public void Init(HarmonyInstance mod)
//        {
//            // Start the coroutine to log the GameManager address
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
//                    Log.Error(ex); // Use Log.Error to log the exception
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
//                    Log.Error(ex); // Use Log.Error to log the exception
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
//                    // Uncomment these lines if you want to log the address
//                    // IntPtr address = GCHandle.Alloc(gameManagerInstance).AddrOfPinnedObject();
//                    // string addressHex = address.ToString("X");
//                    // LogDebugTabMessage($"GameManager Address: 0x{addressHex}");
//                }
//                catch (Exception ex)
//                {
//                    Log.Error(ex); // Use Log.Error to log the exception
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
