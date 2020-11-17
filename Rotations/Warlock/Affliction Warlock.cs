using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class AfflictionWarlock : CombatRoutine
    {
        //General

        private bool IsRange => API.TargetRange < 40;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;


        //CBProperties
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        string[] MisdirectionList = new string[] { "Imp", "Voidwalker" };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];





        //Spells,Buffs,Debuffs
        private string ShadowBolt = "Shadow Bolt";
        private string Corruption = "Corruption";
        private string DrainLife = "Drain Life";
        private string Agony = "Agony";
        private string HealthFunnel = "Health Funnel";
        private string SummonImp = "Summon Imp";
        private string SummonVoidwalker = "Summon Voidwalker";
        private string MaleficRapture = "Malefic Rapture";
        private string UnstableAffliction = "Unstable Affliction";
        private string SeedofCorruption = "Seed of Corruption";

        private string Misdirection = "Misdirection";




        public override void Initialize()
        {
            CombatRoutine.Name = "Affliction Warlock @Mufflon12";
            API.WriteLog("Welcome to Affliction Warlock rotation @ Mufflon12");

            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(HealthFunnel, "Health Funnel", numbList, "Life percent at which " + HealthFunnel + " is used, set to 0 to disable", "PETS", 0);
            CombatRoutine.AddProp(Misdirection, "Wich Pet", MisdirectionList, "Chose your Pet", "PETS", 0);



            //Spells
            CombatRoutine.AddSpell("Shadow Bolt", "D1");
            CombatRoutine.AddSpell("Corruption", "D2");
            CombatRoutine.AddSpell("Agony", "D3");
            CombatRoutine.AddSpell("Malefic Rapture", "D4");
            CombatRoutine.AddSpell("Unstable Affliction", "D5");
            CombatRoutine.AddSpell("Seed of Corruption", "D6");



            CombatRoutine.AddSpell("Drain Life", "NumPad1");
            CombatRoutine.AddSpell("Health Funnel", "NumPad2");






            CombatRoutine.AddSpell("Summon Voidwalker", "NumPad8");
            CombatRoutine.AddSpell("Summon Imp", "NumPad9");


            //Buffs



            //Debuffs
            CombatRoutine.AddDebuff("Corruption");
            CombatRoutine.AddDebuff("Agony");
            CombatRoutine.AddDebuff("Unstable Affliction");

            //Debuffs


        }

        public override void Pulse()
        {
            {
                // Drain Life
                if (API.PlayerHealthPercent <= DrainLifePercentProc && API.CanCast(DrainLife) && PlayerLevel >= 9)
                {
                    API.CastSpell(DrainLife);
                    return;
                }
                // Health Funnel
                if (API.PetHealthPercent <= HealthFunnelPercentProc && API.PlayerHasPet && API.CanCast(HealthFunnel) && PlayerLevel >= 8)
                {
                    API.CastSpell(HealthFunnel);
                    return;
                }
            }
        }

        public override void CombatPulse()
        {
            {
                //Cooldowns
                if (IsCooldowns)
                {

                }
                if (API.CanCast(SeedofCorruption) && API.TargetDebuffRemainingTime(Corruption) < 500 && API.TargetUnitInRangeCount >= AOEUnitNumber && API.PlayerCurrentSoulShards > 1 && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 27)
                {
                    API.CastSpell(UnstableAffliction);
                    return;
                }
                if (API.CanCast(UnstableAffliction) && API.TargetDebuffRemainingTime(UnstableAffliction) < 500 && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 13)
                {
                    API.CastSpell(UnstableAffliction);
                    return;
                }
                if (API.CanCast(Corruption) && API.TargetDebuffRemainingTime(Corruption) < 500 && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 2)
                {
                    API.CastSpell(Corruption);
                    return;
                }
                if (API.CanCast(Agony) && API.TargetDebuffRemainingTime(Agony) < 500 && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony);
                    return;
                }
                if (API.CanCast(MaleficRapture) && API.PlayerCurrentSoulShards > 1 && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 11)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                if (API.CanCast(ShadowBolt) && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 1)
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
            }
        }
        public override void OutOfCombatPulse()
        {
            //Summon Pet
            if (API.CanCast(SummonImp) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 3)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Pet
            if (API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 10)
            {
                API.CastSpell(SummonVoidwalker);
                return;
            }
        }
    }
}
