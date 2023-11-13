using Verse;

namespace LovinIsMessy
{
    public class LovinIsMessySettings : ModSettings
    {
        public float HygieneNeed = 0.4f;
        public bool ConsiderBathAfterLovin = true;
        public bool AlwaysBathAfterLovin = false;
        public bool PrivateBathOnly = true;

        public int FilthAmount = 7;
        public float JoyGain = 0.3f;
        public bool JoyGainEnabled = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref HygieneNeed, "HygieneNeed", 0.4f);
            Scribe_Values.Look(ref ConsiderBathAfterLovin, "ConsiderBathAfterLovin", true);
            Scribe_Values.Look(ref AlwaysBathAfterLovin, "AlwaysBathAfterLovin", false);
            Scribe_Values.Look(ref PrivateBathOnly, "PrivateBathOnly", true);

            Scribe_Values.Look(ref FilthAmount, "FilthAmount", 7);
            Scribe_Values.Look(ref JoyGain, "JoyGain", 0.3f);
            Scribe_Values.Look(ref JoyGainEnabled, "JoyGainEnabled", true);

            base.ExposeData();
        }
    }
}
