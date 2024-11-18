using System;
using System.Diagnostics;
using System.Reflection;

using Harmony;
using MyMod;
using UnityEngine;

public class AgentModelPatch
{
    // Define the AgentModel type explicitly within the class
    private static readonly Type targetType = typeof(AgentModel);

    // Constructor now only needs targetMethodName, as the targetType is fixed to AgentModel
    public AgentModelPatch(HarmonyInstance mod, string targetMethodName)
    {
        string patchMethodName = "Postfix_LoggerPatch"; // Patch method to be called
        Patch(mod, targetMethodName, patchMethodName);
    }

    private void Patch(HarmonyInstance mod, string targetMethodName, string patchMethodName)
    {
        // Get the method to be patched in the AgentModel class
        var originalMethod = targetType.GetMethod(targetMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        // Get the patch method to be applied
        var myPatchMethod = typeof(AgentModelPatch).GetMethod(patchMethodName, BindingFlags.Static | BindingFlags.Public);

        if (originalMethod != null)
        {
            // Apply the patch using Harmony
            mod.Patch(originalMethod, null, new HarmonyMethod(myPatchMethod), null);
        }
        else
        {
            // Log an error if the method is not found
            Log.Error($"Failed to find method: {targetMethodName} in class {targetType.Name}");
        }
    }

    public static void Postfix_LoggerPatch(AgentModel __instance)
    {
        // Get the calling method's details
        var originalMethod = new StackTrace().GetFrame(1).GetMethod();
        string targetClassName = originalMethod.DeclaringType?.Name ?? "Unknown Class";
        string targetMethodName = originalMethod.Name;

        // Log the details including the agent's name
        string agentName = __instance?.name ?? "Unknown Agent Name";  // Ensure we handle potential null values
        if (Harmony_Patch.guiInstance != null && Harmony_Patch.guiInstance.debugTab != null)
        {
            Log.LogAndDebug($"Logged from class: {targetClassName}, method: {targetMethodName}, Agent Name: {agentName}",Color.blue);
        }
    }
}
