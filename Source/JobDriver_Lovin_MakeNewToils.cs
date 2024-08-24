using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace LovinIsMessy
{
    [HarmonyPatch(typeof(JobDriver_Lovin), nameof(JobDriver_Lovin.MakeNewToils))]
    public class JobDriver_Lovin_MakeNewToils
    {
        private static ThingDef _filthDef = null;
        public static ThingDef FilthDef => _filthDef == null ? _filthDef = DefDatabase<ThingDef>.GetNamed("lim_Filth_LoveJuice") : _filthDef;

        private static NeedDef _joyDef = null;
        public static NeedDef JoyDef => _joyDef == null ? _joyDef = DefDatabase<NeedDef>.GetNamed("Joy") : _joyDef;

        public static IEnumerable<Toil> Postfix(IEnumerable<Toil> __result, JobDriver_Lovin __instance)
        {
            foreach (var toil in __result) yield return toil;

            __instance.AddFinishAction((_) => {
                var pawn = __instance.pawn;
                var bed = __instance.Bed;
                var position = pawn.Position;

                if (bed.Rotation == Rot4.East)
                    position.x += 1;
                if (bed.Rotation == Rot4.West)
                    position.x -= 1;
                if (bed.Rotation == Rot4.North)
                    position.z += 1;
                if (bed.Rotation == Rot4.South)
                    position.z -= 1;

                FilthMaker.TryMakeFilth(position, pawn.Map, FilthDef, LovinIsMessy.Settings.FilthAmount);

                var recreation = pawn.needs.TryGetNeed(JoyDef);

                if (recreation != null)
                    recreation.CurLevelPercentage += LovinIsMessy.Settings.JoyGain;
            });

            if (LovinIsMessy.BadHygieneLoaded && Dubs.HygieneNeed != null)
            {
                __instance.AddFinishAction((_) =>
                {
                    var pawn = __instance.pawn;
                    var hygiene = pawn.needs.TryGetNeed(Dubs.HygieneNeed);

                    if (hygiene != null)
                    {
                        hygiene.CurLevelPercentage -= LovinIsMessy.Settings.HygieneNeed;

                        if (LovinIsMessy.Settings.ConsiderBathAfterLovin)
                        {
                            var washGiver = Activator.CreateInstance(Dubs.WashJobGiver) as ThinkNode_JobGiver;

                            if (LovinIsMessy.Settings.AlwaysBathAfterLovin)
                            {
                                washGiver.priority = 10f;
                            }

                            if (washGiver.GetPriority(pawn) > 0)
                            {
                                var washJob = washGiver.TryGiveJob(pawn);

                                if (LovinIsMessy.Settings.PrivateBathOnly)
                                {
                                    var room = washJob?.targetA.Cell.GetRoom(pawn.Map);

                                    if (room == null || room.Role != Dubs.PrivateBathroom)
                                    {
                                        washJob = null;
                                    }
                                }

                                if (washJob != null)
                                {
                                    washJob.TryMakePreToilReservations(pawn, false);

                                    pawn.jobs.jobQueue.EnqueueFirst(new Job(JobDriver_WakePartner.JobDef, __instance.Partner, __instance.job.targetB));
                                    pawn.jobs.jobQueue.EnqueueFirst(washJob);
                                }
                            }
                        }
                    }
                });
            }
        }
    }
}
