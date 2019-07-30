using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace Surgery
{
    [DefOf]
    public static class Hediffs
    {
        public static HediffDef ChemicalDamageModerate;
    }

    public class HediffCompProperties_WakeAnesthetic : CompProperties_Drug
    {
        public float damageChance = 0.1f;
        public float deathChance = 0f;
        public HediffCompProperties_WakeAnesthetic()
        {
            compClass = typeof(HediffComp_WakeAnesthetic);
        }
    }
    public class HediffComp_WakeAnesthetic:CompDrug
    {
        protected void SendLetter(Pawn pawn, Hediff cause, Hediff hediff)
        {
            if (PawnUtility.ShouldSendNotificationAbout(pawn))
            {
                if (cause == null)
                {
                    Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(pawn.LabelShort, hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHediffFromRandomHediffGiver".Translate(pawn.LabelShort, hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn);
                }
                else
                {
                    Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(pawn.LabelShort, hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHealthComplications".Translate(pawn.LabelShort, hediff.LabelCap, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn);
                }
            }
        }
        public HediffCompProperties_WakeAnesthetic Props => (HediffCompProperties_WakeAnesthetic)props;
        public override void PostIngested(Pawn ingester)
        {
            if (ingester.health.hediffSet.HasHediff(HediffDefOf.Anesthetic, false))
            {
                Hediff anesthetic = null;
                foreach (Hediff h in ingester.health.hediffSet.hediffs)
                {
                    if (h.def == HediffDefOf.Anesthetic)
                    {
                        anesthetic = h;
                        break;
                    }
                }
                ingester.health.RemoveHediff(anesthetic);
            }

            double damage = Convert.ToDouble(Props.damageChance);
            double death = Convert.ToDouble(Props.deathChance);
            death = death * 10;
            damage = damage * 10;
            Random r = new Random();
            if (r.Next(1, 10) <= damage)
            {
                BodyPartRecord b = ingester.health.hediffSet.GetBrain();
                Hediff dmg = HediffMaker.MakeHediff(Hediffs.ChemicalDamageModerate,ingester,b);
            ingester.health.AddHediff(dmg);
            SendLetter(ingester, null, dmg);
            }
            else if (r.Next(1, 10) <= death)
            {
                BodyPartRecord b = ingester.health.hediffSet.GetBrain();
                Hediff dmg = HediffMaker.MakeHediff(HediffDefOf.DrugOverdose,ingester,b);
                dmg.Severity = 1f;
                ingester.health.AddHediff(dmg);
            }
            base.PostIngested(ingester);
        }
    }
}
