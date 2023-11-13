using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LovinIsMessy
{
    public static class Dubs
    {
        private static Lazy<NeedDef> hygieneNeed = new Lazy<NeedDef> (() => DefDatabase<NeedDef>.GetNamedSilentFail("Hygiene"));
        private static Lazy<RoomRoleDef> privateBathroom = new Lazy<RoomRoleDef>(() => DefDatabase<RoomRoleDef>.GetNamedSilentFail("PrivateBathroom"));

        public static NeedDef HygieneNeed => hygieneNeed.Value;
        public static RoomRoleDef PrivateBathroom => privateBathroom.Value;

        public static Type WashJobGiver = AccessTools.TypeByName("DubsBadHygiene.JobGiver_HaveWash");
    }
}
