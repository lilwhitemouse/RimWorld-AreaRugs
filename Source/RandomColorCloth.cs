using System;
using System.Collections.Generic;
using RimWorld;
using Verse;


namespace LWM.AreaRugs {
    public class RandomColoredCloth : ThingWithComps {
        static RandomColoredCloth() {
            coloredCloths=new List<ThingDef>();
            coloredCloths.Add(DefDatabase<ThingDef>.GetNamed("RedCloth"));
            coloredCloths.Add(DefDatabase<ThingDef>.GetNamed("BlueCloth"));
        }
        public override void PostMake() {
            this.def=coloredCloths[rng.Next(2)];
            base.PostMake();
        }

        static Random rng=new Random();
        public static List<ThingDef> coloredCloths;
    }
}
