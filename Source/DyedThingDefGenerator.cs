using System;
using Verse;
using System.Collections.Generic;
using System.Linq;

using System.Reflection; //fun fun fun!

namespace LWM.Dyeable
{
    
    public class DyedThingDefGenerator {


        private static readonly MethodInfo CloneMethod = typeof(Object)
            .GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo GiveShortHash = typeof(ShortHashGiver)
            .GetMethod("GiveShortHash", BindingFlags.NonPublic | BindingFlags.Static);
        
        public static ThingDef FromDyeableThingWithComps(ThingWithComps t) {
            ThingDef n = (ThingDef)(CloneMethod.Invoke(t.GetComp<CompDyeable>().originalDef,null));  //shallow copy via MemberwiseClone.
            n.stuffProps=(StuffProperties)(CloneMethod.Invoke(t.def.stuffProps,null));
            uint uColor;
            ColorMapper.GetNearestColor(t.DrawColor, out uColor);
            n.stuffProps.color=ColorMapper.GetUnityColor(uColor);
            n.defName=t.GetComp<CompDyeable>().originalDef.defName+"_"+uColor.ToString("X6")+"dyed"; // ending in numbers causes problems with thing IDs
            n.shortHash=0;
            GiveShortHash.Invoke(null, new object[]{n,typeof(ThingDef)});
            Log.Message("DyedThingDefGenerator: Took "+t.def.defName+" (originally "+
                        t.GetComp<CompDyeable>().originalDef.defName+") and made "+n.defName);
            return n;
        }

    }

    public static class ExtendThingDef {
  //      public static ThingDef FromThingDef(this ThingDef def, ThingWithComps t) {

//            def.Mem

    //    }



    }


}
