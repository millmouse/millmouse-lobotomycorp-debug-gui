using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Harmony;

namespace MyMod.Patches
{
    public interface IPatch
    {
        void Patch(HarmonyInstance mod, string targetMethodName, string patchMethodName);
        void Postfix_LoggerPatch(object instance);
    }

    public static class PatchConstants
    {
        public static readonly string PatchMethodName = "Postfix_LogPatch";
    }
}

