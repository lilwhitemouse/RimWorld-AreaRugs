using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace LWM.Dyeable
{
    public class DyeableFabricTracker : GameComponent {
        Dictionary<ThingDef,Dictionary<uint,List<ThingWithComps>>> fabrics;
        Dictionary<ThingDef,ThingDef> modifiedToOriginalThingDefs;

        public static DyeableFabricTracker Tracker;

        public DyeableFabricTracker(Game game) {
            Log.Warning("New DyeableFabricTracker!");
            fabrics=new Dictionary<ThingDef,Dictionary<uint,List<ThingWithComps>>>(); //Dictionary<ThingDef,List<uint>>();  //List<ThingDef>();
            modifiedToOriginalThingDefs=new Dictionary<ThingDef,ThingDef>();
            Tracker=this;
        }
        
        public override void FinalizeInit() {
            // This should catch everything that was updated after
            //   a game was loaded.
            Log.Warning("DyeableFabricTracker finalizeInit");
            base.FinalizeInit();
            List<Map> maps=new List<Map>();
            foreach (Dictionary<uint,List<ThingWithComps>> fabric in fabrics.Values)
                foreach (List<ThingWithComps> singleColoredItems in fabric.Values)
                    foreach (var item in singleColoredItems)
                        if (item.Map!=null && !maps.Contains(item.Map))
                            maps.Add(item.Map);
//            ResourceCounter.ResetDefs();
            foreach (var map in maps) {
                map.resourceCounter.ResetResourceCounts();
                map.resourceCounter.UpdateResourceCounts();
            }
        }

        public ThingDef Register(ThingWithComps t) {
//            List<uint> colors;
            // Get the original def of the fabric
            ThingDef originalDef=t.GetComp<CompDyeable>().originalDef;
            // in some cases, it may not be correct (e.g., if it's SplitOff() from a modified item)
            if (modifiedToOriginalThingDefs.ContainsKey(originalDef)) {
                originalDef=modifiedToOriginalThingDefs[originalDef];
                t.GetComp<CompDyeable>().originalDef=originalDef;
            }
            Dictionary<uint,List<ThingWithComps>> colors;
            if (!fabrics.ContainsKey(originalDef)) {
                colors=new Dictionary<uint,List<ThingWithComps>>();//List<uint>();
                fabrics.Add(originalDef, colors);
            } else {
                colors=fabrics[originalDef];
            }
            uint colorNumber;
            string colorName = ColorMapper.GetNearestColor(t.DrawColor, out colorNumber);
            List<ThingWithComps> listOfFabricStacks;
            if (colors.ContainsKey(colorNumber)) {
                // then there already exist some stacks with the colored thingDef!
                listOfFabricStacks=colors[colorNumber];
                if (listOfFabricStacks.Contains(t)) {
                    Log.Warning("LWM.Dyeable: listOfFabrics already contains "+t);
                } else {
                    listOfFabricStacks.Add(t);
                }
                Log.Message("Registering "+t+", color & def already present");
                return listOfFabricStacks.First().def;
            }
            
            listOfFabricStacks=new List<ThingWithComps>();
            colors.Add(colorNumber,listOfFabricStacks);
            listOfFabricStacks.Add(t);

            ThingDef newTDef;

            if (modifiedToOriginalThingDefs.ContainsKey(originalDef)) {
                newTDef=modifiedToOriginalThingDefs[originalDef];
                Log.Error("Def for "+t+": already exists as modified");
            } else {
                newTDef=DyedThingDefGenerator.FromDyeableThingWithComps(t);
                modifiedToOriginalThingDefs.Add(newTDef,originalDef);
            }
            if (DefDatabase<ThingDef>.GetNamed(newTDef.defName, false)==null) {
                Log.Message("Def "+newTDef+" added to DefDatabase");
                DefDatabase<ThingDef>.Add(newTDef);
                ResourceCounter.ResetDefs();
                //TODO!!!!!!  Update resources?
            } else {
                Log.Warning("LWM.Dyeable:Tried to add ThingDef to DefDatabase, but it's already there?"+newTDef);
                Log.Warning("  Thing in question: "+t);
            }
            
//            ThingDef dyedThingDef=DyedThingDefGenerator.FromDyeableThingWithComps(t);
//            modifiedThingDefs.Add(dyedThingDef);
            Log.Message("Registered "+t);
            return newTDef;
            // up...what?
            // not sure that there aren't cases where a stack gets created from another
            //   stack (with a modified thingdef) but somehow the other stack gets destroyed
            //   first and so DeRegistered?
/*            if (modifiedThingDefs.Contains(t.def)) {
                Log.Message("Registering "+t+", def already registered once");
                return t.def;
            }

            ThingDef dyedThingDef=DyedThingDefGenerator.FromDyeableThingWithComps(t);
            modifiedThingDefs.Add(dyedThingDef);
            Log.Message("Registering "+t);
            return dyedThingDef;*/
            //CompDyeable cd=t.GetComp<CompDyeable>();
            
            
        }

        public void DeRegister(Thing t) { // I hope RimWorld convention is to capitlize the R

        }


    }


}
