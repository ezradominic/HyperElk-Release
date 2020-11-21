using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class DestructionWarlock : CombatRoutine
    {
        //Spells,Buffs,Debuffs
        private string SummonImp = "Summon Imp";
        private string SummonFelhunter = "Summon Felhunter";
        private string SummonSuccubus = "Summon Succubus";
        private string SummonVoidwalker = "Summon Voidwalker";
        private string DrainLife = "Drain Life";
        private string HealthFunnel = "Health Funnel";
        private string Incinerate = "Incinerate";
        private string Immolate = "Immolate";
        private string ChaosBolt = "Chaos Bolt";
        private string Conflagrate = "Conflagrate";
        private string ShadowBurn = "Shadow Burn";
        private string Cataclysm = "Cataclysm";
        private string Havoc = "Havoc";
        private string RainOfFire = "Rain of Fire";
        private string DarkPact = "Dark Pact";
        private string MortalCoil = "Mortal Coil";
        private string HowlofTerror = "Howl Of Terror";
        private string GrimoireOfSacrifice = "Grimoire Of Sacrifice";
        private string DemonFire = "Demon Fire";
        private string DarkSoulMisery = "Dark Soul Misery";
        private string SummonInfernal = "Summon Infernal";
        private string ChangeTarget = "Change Target";
        private string SoulFire = "Soul Fire";
        private string ChannelDemonFire = "Channel DemonFire";




        private string Misdirection = "Misdirection";

        //Talents
        private bool TalentSoulFire => API.PlayerIsTalentSelected(1, 3);
        private bool TalentCataclysm => API.PlayerIsTalentSelected(4, 3);
        private bool TalentDarkPact => API.PlayerIsTalentSelected(3, 3);
        private bool TalentGrimoireOfSacrifice => API.PlayerIsTalentSelected(6, 3);
        private bool TalentChannelDemonFire => API.PlayerIsTalentSelected(7, 2);
        private bool TalentDarkSoulMisery => API.PlayerIsTalentSelected(7, 3);



        //Misc


        //CBProperties
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

        string[] MisdirectionList = new string[] { "Imp", "Voidwalker", "Succubus", "Felhunter", "Infernal" };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private int DarkPactPercentProc => numbList[CombatRoutine.GetPropertyInt(DarkPact)];
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        private bool IsRange => API.TargetRange < 40;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;

        public override void Initialize()
        {
            CombatRoutine.Name = "Destruction Warlock @Mufflon12";
            API.WriteLog("Welcome to Destruction Warlock rotation @ Mufflon12");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            //Options
            CombatRoutine.AddProp(Misdirection, "Wich Pet", MisdirectionList, "Chose your Pet", "PETS", 0);
            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(HealthFunnel, "Health Funnel", numbList, "Life percent at which " + HealthFunnel + " is used, set to 0 to disable", "PETS", 0);
            CombatRoutine.AddProp(DarkPact, "Dark Pact", numbList, "Life percent at which " + DarkPact + " is used, set to 0 to disable", "Healing", 2);


            //Spells
            CombatRoutine.AddSpell(ChangeTarget, "Tab");
            CombatRoutine.AddSpell(Immolate, "D1");
            CombatRoutine.AddSpell(Incinerate, "D2");
            CombatRoutine.AddSpell(Conflagrate, "D3");
            CombatRoutine.AddSpell(ChaosBolt, "D4");
            CombatRoutine.AddSpell(ShadowBurn, "D5");
            CombatRoutine.AddSpell(Cataclysm, "D6");
            CombatRoutine.AddSpell(Havoc, "D7");
            CombatRoutine.AddSpell(RainOfFire, "D8");
            CombatRoutine.AddSpell(SoulFire, "D9");
            CombatRoutine.AddSpell(ChannelDemonFire, "D0");
            CombatRoutine.AddSpell(DarkSoulMisery, "OemOpenBrackets");

            CombatRoutine.AddSpell(DrainLife, "NumPad1");
            CombatRoutine.AddSpell(HealthFunnel, "NumPad2");
            CombatRoutine.AddSpell(SummonInfernal, "NumPad5");
            CombatRoutine.AddSpell(SummonFelhunter, "NumPad6");
            CombatRoutine.AddSpell(SummonSuccubus, "NumPad7");
            CombatRoutine.AddSpell(SummonVoidwalker, "NumPad8");
            CombatRoutine.AddSpell(SummonImp, "NumPad9");


            //Buffs
            CombatRoutine.AddBuff("Grimoire Of Sacrifice");


            //Buffs

            //Debuffs
            CombatRoutine.AddDebuff(Immolate);
            CombatRoutine.AddDebuff(Havoc);

        }

        public override void Pulse()
        {

        }

        public override void CombatPulse()
        {
            if (IsAOE && API.CanCast(Cataclysm) && API.TargetUnitInRangeCount >= AOEUnitNumber && !API.MouseoverHasDebuff(Immolate) && API.CanCast(Cataclysm) && API.TargetDebuffRemainingTime(Immolate) <= 500 && IsRange && TalentCataclysm && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(Cataclysm);
                return;
            }
            // Drain Life
            if (API.PlayerHealthPercent <= DrainLifePercentProc && API.CanCast(DrainLife) && PlayerLevel >= 9 && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(DrainLife);
                return;
            }
            // Health Funnel
            if (API.PetHealthPercent <= HealthFunnelPercentProc && API.PlayerHasPet && API.CanCast(HealthFunnel) && PlayerLevel >= 8 && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(HealthFunnel);
                return;
            }
            //DarkPact
            if (API.PlayerHealthPercent <= DarkPactPercentProc && API.CanCast(DarkPact) && TalentDarkPact)
            {
                API.CastSpell(DarkPact);
                return;
            }
            // Immolate
            if (API.CanCast(Immolate) && API.TargetDebuffRemainingTime(Immolate) <= 500 && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(Immolate);
                return;
            }
            if (IsAOE && API.CanCast(Havoc) && API.TargetUnitInRangeCount >= AOEUnitNumber && API.CanCast(Havoc) && IsRange && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(Havoc);
                API.CastSpell(ChangeTarget);
                return;
            }
            //DarkSoulMisery
            if (IsCooldowns && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
            {
                API.CastSpell(DarkSoulMisery);
                return;
            }
            // Chaos Bolt
            if (API.CanCast(ChaosBolt) && API.PlayerCurrentSoulShards >= 4 && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(ChaosBolt);
                return;
            }
            // Channel DemonFire
            if (API.CanCast(ChannelDemonFire) && TalentChannelDemonFire && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(ChannelDemonFire);
                return;
            }
            //Soul Fire
            if (!API.SpellISOnCooldown(SoulFire) && TalentSoulFire && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(SoulFire);
                return;
            }
            if (IsAOE && API.CanCast(RainOfFire) && API.TargetUnitInRangeCount >= AOEUnitNumber && API.CanCast(RainOfFire) && API.PlayerCurrentSoulShards >= 3 && IsRange && TalentCataclysm && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(RainOfFire);
                return;
            }

            //Conflagrate
            if (API.CanCast(Conflagrate) && API.SpellCharges(Conflagrate) > 1 && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(Conflagrate);
                return;
            }
            //Incinerate
            if (API.CanCast(Incinerate) && API.PlayerCurrentSoulShards < 4 && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(Incinerate);
                return;
            }
        }


        public override void OutOfCombatPulse()
        {
            if (API.PlayerHasPet && TalentGrimoireOfSacrifice && !API.PlayerHasBuff("Grimoire Of Sacrifice"))
            {
                API.CastSpell(GrimoireOfSacrifice);
                return;
            }
            //Summon Imp
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonImp) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 3)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 10)
            {
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 19)
            {
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 23)
            {
                API.CastSpell(SummonFelhunter);
                return;
            }
            //Summon Fellhunter
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonInfernal) && !API.PlayerHasPet && (isMisdirection == "Infernal") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 42)
            {
                API.CastSpell(SummonFelhunter);
                return;
            }
        }
    }
}
