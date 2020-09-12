
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable ArrangeTypeModifiers

using HarmonyLib;
using Oc.OcInput;
using UnityEngine;

namespace CraftopiaStreamIntegration
{
    /*[HarmonyPatch(typeof(OcEmMng), "loadSaveData_2ndPhase")]
    public class OcEmMng_loadSaveData_2ndPhase_Patch
    {
        private static void PostFix()
        {
            Console.WriteLine("Loading!");
        }
    }*/
    
    [HarmonyPatch(typeof(OcInputTrack), "GetMoveValue")]
    public class OcInputTrack_GetMoveValue_Patch
    {
        private static void Postfix(ref Vector2 __result)
        {
            if (Utils.InvertControls)
            {
                __result.Scale(Vector2.left + Vector2.down);
            }
        }
    }
    
    [HarmonyPatch(typeof(OcInputTrack), "GetLookValue")]
    public class OcInputTrack_GetLookValue_Patch
    {
        private static void Postfix(ref Vector2 __result)
        {
            if (Utils.InvertMouse)
            {
                __result.Scale(Vector2.left + Vector2.down);
            }
        }
    }
}