using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class AfflictionWarlock : CombatRoutine
    {
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
        private string DrainSoul = "Drain Soul";
        private string SummonSuccubus = "Summon Succubus";
        private string SiphonLife = "Siphon Life";
        private string DarkPact = "Dark Pact";
        private string PhantomSingularity = "Dark Pact";
        private string VileTaint = "Dark Pact";
        private string SummonFelhunter = "Summon Felhunter";
        private string SummonDarkglare = "Summon Darkglare";
        private string Haunt = "Haunt";
        private string DarkSoulMisery = "Dark Soul Misery";
        private string ShadowBulwark = "Shadow Bulwark";


        private string Misdirection = "Misdirection";

        //Talents
        private bool TalentDrainSoul => API.PlayerIsTalentSelected(1, 3);
        private bool TalentSiphonLife => API.PlayerIsTalentSelected(2, 3);
        private bool TalentDarkPact => API.PlayerIsTalentSelected(3, 3);
        private bool TalentPhantomSingularity => API.PlayerIsTalentSelected(4, 2);
        private bool TalentVileTaint => API.PlayerIsTalentSelected(4, 3);
        private bool TalentMortalCoil => API.PlayerIsTalentSelected(5, 2);
        private bool TalentHowlOfTerror => API.PlayerIsTalentSelected(5, 3);
        private bool TalentHaunt => API.PlayerIsTalentSelected(6, 2);
        private bool TalentGrimoireOfSacrifice => API.PlayerIsTalentSelected(6, 3);
        private bool TalentDarkSoulMisery => API.PlayerIsTalentSelected(7, 3);

        //Misc
        private bool IsRange => API.TargetRange < 40;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private int ShoulShardNumberMaleficRapture => CombatRoutine.GetPropertyInt("SoulShardNumberMaleficRapture");
        private int ShoulShardNumberDrainSoul => CombatRoutine.GetPropertyInt("SoulShardNumberDrainSoul");
        bool LastSeed => API.CurrentCastSpellID("player") == 27243;



        //CBProperties
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        string[] MisdirectionList = new string[] { "Imp", "Voidwalker", "Succubus", "Felhunter", "Darkglare" };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private bool UseUA => (bool)CombatRoutine.GetProperty("UseUA");
        private bool UseAG => (bool)CombatRoutine.GetProperty("UseAG");
        private bool UseCO => (bool)CombatRoutine.GetProperty("UseCO");
        private bool UseSL => (bool)CombatRoutine.GetProperty("UseSL");

        private int DarkPactPercentProc => numbList[CombatRoutine.GetPropertyInt(DarkPact)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");


        public override void Initialize()
        {
            CombatRoutine.Name = "Affliction Warlock @Mufflon12";
            API.WriteLog("Welcome to Affliction Warlock rotation @ Mufflon12");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            API.WriteLog("Use /stopcasting /cast agony macro");
            API.WriteLog("Use /stopcasting /cast corruption macro");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("Create the following mouseover macro and assigned to the bind:");
            API.WriteLog("/cast [@mouseover] Agony");
            API.WriteLog("/cast [@mouseover] Corruption");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(HealthFunnel, "Health Funnel", numbList, "Life percent at which " + HealthFunnel + " is used, set to 0 to disable", "PETS", 0);
            CombatRoutine.AddProp(Misdirection, "Wich Pet", MisdirectionList, "Chose your Pet", "PETS", 0);
            CombatRoutine.AddProp("UseUA", "Use Unstable Affliction", true, "Should the rotation use Unstable Affliction", "Class specific");
            CombatRoutine.AddProp(DarkPact, "Dark Pact", numbList, "Life percent at which " + DarkPact + " is used, set to 0 to disable", "Healing", 2);
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddProp("SoulShardNumberMaleficRapture", "Soul Shards Malefic Rapture", 2, "How many Soul Shards to use Malefic Rapture", "Class specific");
            CombatRoutine.AddProp("SoulShardNumberDrainSoul", "Soul Shards Drain Shoul", 1, "How many Soul Shards to use Drain Shoul", "Class specific");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("UseAG", "Use Agony", true, "Use Agony for mouseover Multidots", "MultiDOTS");
            CombatRoutine.AddProp("UseCO", "Use Corruption", true, "Use Corruption for mouseover Multidots", "MultiDOTS");
            CombatRoutine.AddProp("UseSL", "Use Siphon Life", true, "Use Siphon Life for mouseover Multidots", "MultiDOTS");


            //Spells
            CombatRoutine.AddSpell("Shadow Bolt", "D1");
            CombatRoutine.AddSpell("Drain Soul", "D1");
            CombatRoutine.AddSpell("Corruption", "D2");
            CombatRoutine.AddSpell("Agony", "D3");
            CombatRoutine.AddSpell("Malefic Rapture", "D4");
            CombatRoutine.AddSpell("Unstable Affliction", "D5");
            CombatRoutine.AddSpell("Seed of Corruption", "D6");
            CombatRoutine.AddSpell("Siphon Life", "D7");
            CombatRoutine.AddSpell("Phantom Singularity", "D8");
            CombatRoutine.AddSpell("Vile Taint", "D9");
            CombatRoutine.AddSpell("Haunt", "F1");
            CombatRoutine.AddSpell("Dark Soul Misery", "F2");
            CombatRoutine.AddSpell(Agony+"MO", "F1");
            CombatRoutine.AddSpell(Corruption+"MO", "F2");
            CombatRoutine.AddSpell(SiphonLife + "MO", "F3");



            CombatRoutine.AddSpell("Drain Life", "NumPad1");
            CombatRoutine.AddSpell("Health Funnel", "NumPad2");
            CombatRoutine.AddSpell("Dark Pact", "NumPad3");
            CombatRoutine.AddSpell("Shadow Bulwark", "NumPad4");


            CombatRoutine.AddSpell("Summon Darkglare", "NumPad5");
            CombatRoutine.AddSpell("Summon Felhunter", "NumPad6");
            CombatRoutine.AddSpell("Summon Succubus", "NumPad7");
            CombatRoutine.AddSpell("Summon Voidwalker", "NumPad8");
            CombatRoutine.AddSpell("Summon Imp", "NumPad9");


            //Buffs



            //Debuffs
            CombatRoutine.AddDebuff("Corruption");
            CombatRoutine.AddDebuff("Agony");
            CombatRoutine.AddDebuff("Unstable Affliction");
            CombatRoutine.AddDebuff("Siphon Life");
            CombatRoutine.AddDebuff("Seed of Corruption");

            //Debuffs


        }

        public override void Pulse()
        {
            {

            }
        }

        public override void CombatPulse()
        {
            {
                //Cooldowns
                if (IsCooldowns)
                {
                    //Dark Soul Misery
                    if (API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                    {
                        API.CastSpell(DarkSoulMisery);
                        return;
                    }
                }
                if (IsMouseover)
                {
                    if (UseCO)
                    {
                        if (API.CanCast(Corruption) && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Corruption) <= 400 && !API.TargetHasDebuff(SeedofCorruption) && IsRange && PlayerLevel >= 2)
                        {
                            API.CastSpell(Corruption + "MO");
                            return;
                        }
                    }
                    if (UseAG)
                { 
                        if (API.CanCast(Agony) && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                        {
                            API.CastSpell(Agony + "MO");
                            return;
                        }
                }
                    if (UseSL)
                    {
                        if (API.CanCast(SiphonLife) && TalentSiphonLife && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(SiphonLife) <= 400 && IsRange && PlayerLevel >= 10)
                        {
                            API.CastSpell(Agony + "MO");
                            return;
                        }
                    }

                }
                //Seed of Corruption
                if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber && !API.TargetHasDebuff(SeedofCorruption) && API.CanCast(SeedofCorruption) && API.TargetDebuffRemainingTime(Corruption) <= 400 && IsRange && API.PlayerCurrentSoulShards >=1)
                {
                    API.CastSpell(SeedofCorruption);
                    return;
                }
                //Agony
                if (API.CanCast(Agony) && API.TargetDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony);
                    return;
                }
                //Corruption
                if (!LastSeed && API.CanCast(Corruption) && API.TargetDebuffRemainingTime(Corruption) <= 400 && !API.TargetHasDebuff(SeedofCorruption) && IsRange && PlayerLevel >= 2)
                {
                    API.CastSpell(Corruption);
                    return;
                }
                //SiphonLife
                if (API.CanCast(SiphonLife) && API.TargetDebuffRemainingTime(SiphonLife) <= 400 && IsRange && TalentSiphonLife)
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
                // Dark Pact
                if (API.PlayerHealthPercent <= DarkPactPercentProc && API.CanCast(DarkPact) && TalentDarkPact)
                {
                    API.CastSpell(DarkPact);
                    return;
                }
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
                //Unstable Affliction
                if (UseUA)
                {
                    if (API.CanCast(UnstableAffliction) && API.TargetDebuffRemainingTime(UnstableAffliction) <= 500 && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 13)
                    {
                        API.CastSpell(UnstableAffliction);
                        return;
                    }
                }
                //Haunt
                if (API.CanCast(Haunt) && NotMoving && NotCasting && IsRange && NotChanneling && TalentHaunt)
                {
                    API.CastSpell(Haunt);
                    return;
                }
                //Vile Taint
                if (API.CanCast(VileTaint) && API.PlayerCurrentSoulShards >= 2 && NotMoving && NotCasting && IsRange && NotChanneling && TalentVileTaint)
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //Phantom Singularity
                if (API.CanCast(PhantomSingularity) && NotCasting && IsRange && NotChanneling && TalentPhantomSingularity)
                {
                    API.CastSpell(PhantomSingularity);
                    return;
                }
                //Malefic Rapture
                if (API.CanCast(MaleficRapture) && API.PlayerCurrentSoulShards >= ShoulShardNumberMaleficRapture && API.TargetHasDebuff(Corruption) && API.TargetHasDebuff(Agony) && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 11)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                //Drain Soul
                if (API.CanCast(DrainSoul) && API.PlayerCurrentSoulShards <= ShoulShardNumberDrainSoul)
                {
                    API.CastSpell(DrainSoul);
                    return;
                }
                //Shadow Bolt
                if (API.CanCast(ShadowBolt) && NotMoving && NotCasting && IsRange && NotChanneling && !TalentDrainSoul && PlayerLevel >= 1)
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
            }
        }
        public override void OutOfCombatPulse()
        {
            //Summon Imp
            if (API.CanCast(SummonImp) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 3)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 10)
            {
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (API.CanCast(SummonSuccubus) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 19)
            {
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (API.CanCast(SummonFelhunter) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 23)
            {
                API.CastSpell(SummonFelhunter);
                return;
            }
            //Summon Darkglare
            if (API.CanCast(SummonDarkglare) && !API.PlayerHasPet && (isMisdirection == "Darkglare") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 42)
            {
                API.CastSpell(SummonDarkglare);
                return;
            }
        }
    }
}
