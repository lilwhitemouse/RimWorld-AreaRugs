using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;

namespace LWM.Dyeable {
    public class Properties : Verse.CompProperties {
        public Properties() {
            this.compClass=typeof(LWM.Dyeable.CompDyeable);
        }
    }

    public class CompDyeable : ThingComp {

        public string ColorName=null;

        public override bool AllowStackWith(Thing other) {
            if (Math.Abs(parent.DrawColor.r - other.DrawColor.r) > .05 ||
                Math.Abs(parent.DrawColor.g - other.DrawColor.g) > .05 ||
                Math.Abs(parent.DrawColor.b - other.DrawColor.b) > .05) {
                
                return false;
            }
            return true;

            CompColorable cc1, cc2;
            if ((cc1=this.parent.GetComp<CompColorable>())==null ||
                (cc2=other.TryGetComp<CompColorable>())==null) {
                Log.Error("LWM.AreaRugs: "+this.parent+" or "+
                          other+" has CompDyeable but not CompColorable - oops.");
                return true;
            }
            if (Math.Abs(cc2.Color.r-cc2.Color.r) > .05 ||
                Math.Abs(cc1.Color.g-cc2.Color.g) > .05 ||
                Math.Abs(cc1.Color.b-cc2.Color.b) > .05) {
                return false;
            }
            return true;
        }

        public override void Initialize(CompProperties props) { // Register color
            Log.Warning("Cloth: "+parent.stackCount+parent+" Initialized (PostMake)...");
            CompColorable CC=parent.GetComp<CompColorable>(); // if null, bad bad problems
            if (CC==null) {
                Log.Error("Item "+parent+" (def "+parent.def+") does not have CompColorable,"+
                          " but has CompDyeable.  This should never happen.  Destroying.");
                // This will cause more errors:
                parent.Destroy(DestroyMode.Vanish);
                // but then, so would anything else
            } else {
//                if (
                originalDef=parent.def;
                if (CC.Color == parent.def.stuffProps.color) {
                    Log.Message("Not registering: it's undyed");
                } else {
                    ColorName=ColorMapper.GetNearestName(CC.Color);
                    Log.Warning("  is of color "+ColorName+"; registering...");
                    parent.def=DyeableFabricTracker.Tracker.Register(parent);
                }
            }

        }

        public override void PostSplitOff(Thing piece) {
            Log.Message(""+parent+" split off some to form "+piece);
            CompDyeable cd=piece.TryGetComp<CompDyeable>();
            if (cd==null) { Log.Error("Piece split off from "+parent+" doesn't have CompDyeable??"); return;}
            cd.originalDef=this.originalDef;
        }

        public override void PostExposeData() {
            if (Scribe.mode==LoadSaveMode.LoadingVars) {
                CompColorable CC=parent.GetComp<CompColorable>();
                if (CC.Color==parent.def.stuffProps.color) {
                    Log.Message("Post-load, not registering "+parent+": it's undyed");
                } else {
                    ColorName=ColorMapper.GetNearestName(CC.Color);
                    Log.Message("Post-load, registering "+parent.stackCount+parent+", with color "+ColorName);
                    parent.def=DyeableFabricTracker.Tracker.Register(parent);
                }
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad) {
            base.PostSpawnSetup(respawningAfterLoad);

// TODO: check
            Log.Warning("Cloth: "+parent.stackCount+parent+" has color "+ColorMapper.GetNearestName(parent.DrawColor));
        }

        public override string TransformLabel(string label) {
            CompColorable CC=parent.GetComp<CompColorable>(); // not null, that was checked earlier
            return label+" ("+ColorName+")";
        }

        public override void PostDeSpawn(Map map) {
            Log.Warning("XXX:  Cloth "+parent.stackCount+parent+" is DeSpawning");
        }
        public override void PostDestroy(DestroyMode mode, Map previousMap) { // Deregister color
            Log.Warning("XXX:  Cloth "+parent.stackCount+parent+" is Destroying via mode "+mode);
            DyeableFabricTracker.Tracker.DeRegister(parent);
        }

        public ThingDef originalDef;

    } // /CompDyeable
    

}
