using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;

namespace MyMod.Patches
{
    public class StartGamePatch
    {
        public StartGamePatch(HarmonyInstance mod)
        {
            Patch(mod);
        }

        private void Patch(HarmonyInstance mod)
        {
            var originalStartGame = typeof(GameManager).GetMethod("StartGame", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var postfixStartGame = typeof(StartGamePatch).GetMethod("Postfix_StartGame", BindingFlags.Static | BindingFlags.Public);

            if (originalStartGame != null)
            {
                mod.Patch(originalStartGame, null, new HarmonyMethod(postfixStartGame), null);
            }
            else
            {
                Log.Error("Failed to find method: StartGame");
            }
        }

        public static void Postfix_StartGame()
        {
            if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
            {
                DebugTab.AddMessage("Game has started!");
            }
        }
    }
}
