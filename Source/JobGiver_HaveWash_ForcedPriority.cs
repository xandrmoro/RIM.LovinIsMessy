using HarmonyLib;
using System.Reflection;
using Verse;
using Verse.AI;

namespace LovinIsMessy
{
    [HarmonyPatch]
    public class JobGiver_HaveWash_ForcedPriority
    {
        public static bool Prepare()
        {
            return Dubs.WashJobGiver != null;
        }

        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(Dubs.WashJobGiver, "GetPriority");
        }

        public static bool Prefix(ref float __result, ThinkNode_JobGiver __instance)
        {
            if (__instance.priority > 0f)
            {
                __result = __instance.priority;
                return false;
            }

            return true;
        }
    }
}
