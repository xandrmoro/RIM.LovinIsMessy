using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace LovinIsMessy
{
    public class JobDriver_WakePartner : JobDriver
    {
        public static JobDef JobDef => DefDatabase<JobDef>.GetNamed("lim_WakePartner");

        public override IEnumerable<Toil> MakeNewToils()
        {
            var partner = TargetA.Thing as Pawn;

            if (partner == null)
            {
                yield break;
            }

            if (partner.CurJobDef != JobDefOf.LayDown)
            {
                yield break;
            }

            var gotoToil = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);

            gotoToil.AddFinishAction(() =>
            {
                var washGiver = Activator.CreateInstance(Dubs.WashJobGiver) as ThinkNode_JobGiver;

                if (LovinIsMessy.Settings.AlwaysBathAfterLovin)
                {
                    washGiver.priority = 10f;
                }
                
                if (partner.needs.TryGetNeed(Dubs.HygieneNeed).CurLevelPercentage < 0.95f && washGiver.GetPriority(partner) > 0)
                {
                    var washJob = washGiver.TryGiveJob(partner);
                    partner.jobs.StartJob(washJob, JobCondition.InterruptForced);
                }
            });

            yield return gotoToil;
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(TargetA, job, 1, -1, null, errorOnFailed);
        }
    }
}
