using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
namespace Surgery
{
    public class HediffCompProperties_WakeAnesthetic : CompProperties_Drug
    {
        public HediffCompProperties_WakeAnesthetic()
        {
            compClass = typeof(HediffComp_WakeAnesthetic);
        }
    }
    public class HediffComp_WakeAnesthetic:CompDrug
    {
        public override void PostIngested(Pawn ingester)
        {
            if(ingester.health.hediffSet.HasHediff(HediffDefOf.Anesthetic))
            {
                Hediff h = ingester.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Anesthetic);
                if (h != null)
                {
                    ingester.health.RemoveHediff(h);
                }
            }
            base.PostIngested(ingester);
        }
    }
}
