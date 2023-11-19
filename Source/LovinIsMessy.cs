using HarmonyLib;
using UnityEngine;
using Verse;

namespace LovinIsMessy
{
    public class LovinIsMessy : Mod
    {
        public static LovinIsMessySettings Settings { get; private set; }

        public static bool BadHygieneLoaded { get; private set; }

        public LovinIsMessy(ModContentPack contentPack) : base(contentPack)
        {
            Settings = GetSettings<LovinIsMessySettings>();

            BadHygieneLoaded = ModLister.GetActiveModWithIdentifier("dubwise.dubsbadhygiene") != null;

            new Harmony(Content.PackageIdPlayerFacing).PatchAll();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();

            listing.Begin(inRect);

            listing.Label($"Filth Amount: {Settings.FilthAmount}");
            Settings.FilthAmount = (int)listing.Slider(Settings.FilthAmount, 0f, 20f);

            listing.CheckboxLabeled("Joy Gain Enabled", ref Settings.JoyGainEnabled);
            if (Settings.JoyGainEnabled)
            {
                listing.Label($"Joy Gain: {Settings.JoyGain.ToStringPercent()}");
                Settings.JoyGain = listing.Slider(Settings.JoyGain, 0f, 1f);
            }

            if (BadHygieneLoaded)
            {
                listing.Gap();

                listing.Label($"Hygiene Need Reduction: {Settings.HygieneNeed.ToStringPercent()}");
                Settings.HygieneNeed = listing.Slider(Settings.HygieneNeed, 0f, 1f);

                listing.CheckboxLabeled("Consider Bath After Lovin", ref Settings.ConsiderBathAfterLovin);

                if (Settings.ConsiderBathAfterLovin)
                {
                    listing.CheckboxLabeled("Always Bath After Lovin", ref Settings.AlwaysBathAfterLovin);
                    listing.CheckboxLabeled("Private Bath Only", ref Settings.PrivateBathOnly);
                }
            }

            listing.End();

            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return nameof(LovinIsMessy);
        }
    }
}
