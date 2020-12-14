using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;





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
        private string PhantomSingularity = "Phantom Singularity";
        private string VileTaint = "Vile Taint";
        private string SummonFelhunter = "Summon Felhunter";
        private string SummonDarkglare = "Summon Darkglare";
        private string Haunt = "Haunt";
        private string DarkSoulMisery = "Dark Soul:  Misery";
        private string GrimoireOfSacrifice = "Grimoire Of Sacrifice";
        private string ScouringTithe = "Scouring Tithe";
        private string Misdirection = "Misdirection";
        private string CovenantAbility = "Covenant Ability";
        private string SoulRot = "Soul Rot";
        private string ImpendingCatastrophe = "Impending Catastrophe";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        private string MortalCoil = "Mortal Coil";
        private string DecimatingBolt = "Decimating Bolt";
        //Talents
        private bool TalentDrainSoul => API.PlayerIsTalentSelected(1, 3);
        private bool TalentSiphonLife => API.PlayerIsTalentSelected(2, 3);
        private bool TalentDarkPact => API.PlayerIsTalentSelected(3, 3);
        private bool TalentPhantomSingularity => API.PlayerIsTalentSelected(4, 2);
        private bool TalentVileTaint => API.PlayerIsTalentSelected(4, 3);
        private bool TalentHaunt => API.PlayerIsTalentSelected(6, 2);
        private bool TalentGrimoireOfSacrifice => API.PlayerIsTalentSelected(6, 3);
        private bool TalentDarkSoulMisery => API.PlayerIsTalentSelected(7, 3);
        private bool TalentMortalCoil => API.PlayerIsTalentSelected(5, 2);


        //Misc
        private static readonly Stopwatch DumpWatch = new Stopwatch();

        private bool IsRange => API.TargetRange < 40;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
//        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private int ShoulShardNumberMaleficRapture => CombatRoutine.GetPropertyInt("SoulShardNumberMaleficRapture");
        private int ShoulShardNumberDrainSoul => CombatRoutine.GetPropertyInt("SoulShardNumberDrainSoul");
        private int Trinket1Usage => CombatRoutine.GetPropertyInt("Trinket1");
        private int Trinket2Usage => CombatRoutine.GetPropertyInt("Trinket2");



        //CBProperties      
        bool DotCheck => API.TargetHasDebuff(Corruption) && API.TargetHasDebuff(Agony) && API.TargetHasDebuff(UnstableAffliction) && (API.TargetHasDebuff(SoulRot) || !API.TargetHasDebuff(SoulRot));
        bool CastingSOC => API.PlayerLastSpell == SeedofCorruption;
        bool CastingSOC1 => API.LastSpellCastInGame == SeedofCorruption;
        bool CastingAgony => API.PlayerLastSpell == Agony;
        bool CastingCorruption => API.PlayerLastSpell == Corruption;
        bool CastingSL => API.PlayerLastSpell == SiphonLife;
        bool LastSeed => API.CurrentCastSpellID("player") == 27243;
        bool LastMR => API.PlayerLastSpell == MaleficRapture;

        bool LastUnstableAffliction => API.PlayerLastSpell == UnstableAffliction;
        //Trinket1
        private string UseTrinket1 => TrinketList1[CombatRoutine.GetPropertyInt(trinket1)];
        string[] TrinketList1 = new string[] { "always", "Cooldowns", "AOE", "never" };
        //Trinket2
        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "AOE", "never" };
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int MortalCoilPercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];


        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        string[] MisdirectionList = new string[] { "None", "Imp", "Voidwalker", "Succubus", "Felhunter", };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private bool UseUA => (bool)CombatRoutine.GetProperty("UseUA");
        private bool UseAG => (bool)CombatRoutine.GetProperty("UseAG");
        private bool UseCO => (bool)CombatRoutine.GetProperty("UseCO");
        private bool UseSL => (bool)CombatRoutine.GetProperty("UseSL");
        private bool DumpShards => (bool)CombatRoutine.GetProperty("DumpShards");

        private int DarkPactPercentProc => numbList[CombatRoutine.GetPropertyInt(DarkPact)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseCovenantAbility => CovenantAbilityList[CombatRoutine.GetPropertyInt(CovenantAbility)];

        string[] CovenantAbilityList = new string[] { "None", "always", "with Cooldowns", "AOE" };


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
            API.WriteLog("/cast [@cursor] Vile Taint");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("Put a Meele Pet Ability on your Action Bar for the AOE Detection");


            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(MortalCoil, "Mortal Coil", numbList, "Life percent at which " + MortalCoil + " is used, set to 0 to disable", "Healing", 5);

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
            CombatRoutine.AddProp("Covenant Ability", "Use " + "Covenant Ability", CovenantAbilityList, "How to use Covenant Spell", "Covenant", 0);
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", TrinketList1, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", TrinketList2, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp("DumpShards", "Dump Shards", true, "Collect 5 Soul Shards and befor using Malefic Rapture", "Class specific");


            //Spells
            CombatRoutine.AddSpell(ShadowBolt, "D1");
            CombatRoutine.AddSpell(DrainSoul, "D1");
            CombatRoutine.AddSpell(Corruption, "D2");
            CombatRoutine.AddSpell(Agony, "D3");
            CombatRoutine.AddSpell(MaleficRapture, "D4");
            CombatRoutine.AddSpell(UnstableAffliction, "D5");
            CombatRoutine.AddSpell(SeedofCorruption, "D6");
            CombatRoutine.AddSpell(SiphonLife, "D7");
            CombatRoutine.AddSpell(PhantomSingularity, "D8");
            CombatRoutine.AddSpell(Haunt, "D8");
            CombatRoutine.AddSpell(DarkSoulMisery, "D8");
            CombatRoutine.AddSpell(MortalCoil, "D9");
            CombatRoutine.AddSpell(VileTaint, "D8");
            CombatRoutine.AddSpell(ScouringTithe, "F1");
            CombatRoutine.AddSpell(SoulRot, "F1");
            CombatRoutine.AddSpell(ImpendingCatastrophe, "F1");
            CombatRoutine.AddSpell(DecimatingBolt, "F1");

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);
            CombatRoutine.AddMacro(Agony + "MO", "F1");
            CombatRoutine.AddMacro(Corruption + "MO", "F2");
            CombatRoutine.AddMacro(SiphonLife + "MO", "F3");



            CombatRoutine.AddSpell("Drain Life", "NumPad1");
            CombatRoutine.AddSpell("Health Funnel", "NumPad2");
            CombatRoutine.AddSpell("Dark Pact", "NumPad3");
            CombatRoutine.AddSpell("Grimoire Of Sacrifice", "NumPad4");
            CombatRoutine.AddSpell("Summon Darkglare", "NumPad5");
            CombatRoutine.AddSpell("Summon Felhunter", "NumPad6");
            CombatRoutine.AddSpell("Summon Succubus", "NumPad7");
            CombatRoutine.AddSpell("Summon Voidwalker", "NumPad8");
            CombatRoutine.AddSpell("Summon Imp", "NumPad9");


            //Buffs
            CombatRoutine.AddBuff("Grimoire Of Sacrifice");

            //Debuffs
            CombatRoutine.AddDebuff(ImpendingCatastrophe);
            CombatRoutine.AddDebuff(Corruption);
            CombatRoutine.AddDebuff(Agony);
            CombatRoutine.AddDebuff(UnstableAffliction);
            CombatRoutine.AddDebuff(SiphonLife);
            CombatRoutine.AddDebuff(SeedofCorruption);
            CombatRoutine.AddDebuff(VileTaint);
            CombatRoutine.AddDebuff(PhantomSingularity);
            CombatRoutine.AddDebuff(Haunt);
            CombatRoutine.AddDebuff(SoulRot);



        }


        public override void Pulse()
        {

        }
        public override void CombatPulse()
        {
            if (DumpShards && API.PlayerCurrentSoulShards <= 0)
            {
                API.WriteLog("No More Shards left.");
                DumpWatch.Stop();
                DumpWatch.Reset();
            }
            if (DumpShards && DumpWatch.IsRunning && API.CanCast(MaleficRapture) && DotCheck && IsRange && API.PlayerCurrentSoulShards >= 1)
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            if (IsMouseover)
            {
                if (UseCO)
                {
                    if (!CastingCorruption && API.CanCast(Corruption) && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Corruption) <= 400 && !API.TargetHasDebuff(SeedofCorruption) && IsRange && PlayerLevel >= 2)
                    {
                        API.CastSpell(Corruption + "MO");
                        return;
                    }
                }
                if (UseAG)
                {
                    if (!CastingAgony && API.CanCast(Agony) && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                    {
                        API.CastSpell(Agony + "MO");
                        return;
                    }
                }
                if (UseSL)
                {
                    if (!CastingSL && API.CanCast(SiphonLife) && API.PlayerCanAttackMouseover && TalentSiphonLife && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(SiphonLife) <= 400 && IsRange && PlayerLevel >= 10)
                    {
                        API.CastSpell(SiphonLife + "MO");
                        return;
                    }
                }
            }
                // Dark Pact
                if (API.PlayerHealthPercent <= DarkPactPercentProc && API.CanCast(DarkPact) && TalentDarkPact)
                {
                    API.CastSpell(DarkPact);
                    return;
                }
                //MortalCoil
                if (API.CanCast(MortalCoil) && TalentMortalCoil && API.PlayerHealthPercent <= MortalCoilPercentProc && PlayerLevel >= 40)
                {
                    API.CastSpell(MortalCoil);
                    return;
                }
                // Drain Life
                if (API.PlayerHealthPercent <= DrainLifePercentProc && API.CanCast(DrainLife) && PlayerLevel >= 9 && NotChanneling)
                {
                    API.CastSpell(DrainLife);
                    return;
                }
                // Health Funnel
                if (API.PlayerHasPet && API.PetHealthPercent >= 1 && API.PetHealthPercent <= HealthFunnelPercentProc && API.PlayerHasPet && API.CanCast(HealthFunnel) && PlayerLevel >= 8 && NotChanneling)
                {
                    API.CastSpell(HealthFunnel);
                    return;
                }

            //Trinkets
            if (UseTrinket1 == "AOE" && IsAOE && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 || UseTrinket1 == "always" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 || UseTrinket1 == "Cooldowns" && IsCooldowns && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (UseTrinket2 == "AOE" && IsAOE && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 || UseTrinket2 == "always" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 || UseTrinket2 == "Cooldowns" && IsCooldowns && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
                rotation();
                return;         
        }


        private void rotation()
        {

            //ROTATION AOE
            if (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE && IsRange)
            {
                //actions.aoe=phantom_singularity
                //PhantomSingularity
                if (TalentPhantomSingularity && API.CanCast(PhantomSingularity) && !API.TargetHasDebuff(PhantomSingularity))
                {
                    API.CastSpell(PhantomSingularity);
                    return;
                }
                //actions.aoe+=/haunt
                //Haunt 
                if (API.CanCast(Haunt) && !API.SpellISOnCooldown(Haunt) && API.PlayerCurrentCastTimeRemaining > 40 && TalentHaunt && NotMoving && IsRange && NotChanneling && PlayerLevel >= 45)
                {
                    API.CastSpell(Haunt);
                    return;
                }
                //actions+=/seed_of_corruption,if=active_enemies>2
                //Seed of Corruption
                if (!CastingSOC && !CastingSOC1 && !LastSeed && !API.TargetHasDebuff(SeedofCorruption) && API.CanCast(SeedofCorruption) && API.TargetDebuffRemainingTime(Corruption) <= 400 && IsRange && API.PlayerCurrentSoulShards >= 1 && API.PlayerLevel >= 27)
                {
                    API.CastSpell(SeedofCorruption);
                    return;
                }
                //actions+=/agony,if=dot.agony.remains<4
                //Agony
                if (!CastingAgony && !CastingSOC && !LastSeed && API.CanCast(Agony) && API.TargetDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony);
                    return;
                }
                //SoulRot
                if (API.CanCast(SoulRot) && PlayerCovenantSettings == "Night Fae" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions+=/unstable_affliction,if=dot.unstable_affliction.remains<4
                //Unstable Affliction
                if (UseUA)
                {
                    if (!LastUnstableAffliction && API.CanCast(UnstableAffliction) && API.PlayerCurrentCastTimeRemaining > 40 && API.TargetDebuffRemainingTime(UnstableAffliction) <= 400 && NotMoving && IsRange && NotChanneling && PlayerLevel >= 13)
                    {
                        API.CastSpell(UnstableAffliction);
                        return;
                    }
                }
                //actions+=/vile_taint,if=(soul_shard>1|active_enemies>2)
                //VileTaint
                if (TalentVileTaint && !API.TargetHasDebuff(VileTaint) && API.CanCast(VileTaint) && IsRange && API.PlayerCurrentSoulShards >= 1)
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.aoe+=/dark_soul,if=cooldown.summon_darkglare.remains>time_to_die
                //DarkSoulMisery
                if (API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery && API.SpellCDDuration(SummonDarkglare) >= API.TargetTimeToDie)
                {
                    API.CastSpell(DarkSoulMisery);
                    return;
                }
                //Malefic Rapture Check
                if (!DumpShards && API.CanCast(MaleficRapture) && API.PlayerCurrentSoulShards >= ShoulShardNumberMaleficRapture && DotCheck && PlayerLevel >= 11)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                if (DumpShards && API.CanCast(MaleficRapture) && API.PlayerCurrentSoulShards >= 5 && DotCheck && PlayerLevel >= 11)
                {
                    DumpWatch.Start();
                    API.WriteLog("Starting Dump Shards.");
                    return;
                }
                //DecimatingBolt
                if (API.CanCast(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.aoe+=/siphon_life,cycle_targets=1,if=active_dot.siphon_life<=3,target_if=!dot.siphon_life.ticking
                if (!CastingSL && API.CanCast(SiphonLife) && TalentSiphonLife && API.TargetDebuffRemainingTime(SiphonLife) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
                //ImpendingCatastrophe
                if (API.CanCast(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //ScouringTithe
                if (API.CanCast(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
                //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                //ImpendingCatastrophe
                if (API.CanCast(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && API.SpellCDDuration(SummonDarkglare) <= 1000 || API.SpellCDDuration(SummonDarkglare) >= 5000 && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                //DecimatingBolt
                if (API.CanCast(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) >= 500 && API.TargetDebuffRemainingTime(Haunt) >= 400 && TalentHaunt && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                //SoulRot
                if (API.CanCast(SoulRot) && PlayerCovenantSettings == "Night Fae" && API.SpellCDDuration(SummonDarkglare) <= 500 || API.SpellCDDuration(SummonDarkglare) >= 50000 || API.SpellCDDuration(SummonDarkglare) >= 25 && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(SoulRot);
                    return;

                }
                //actions.covenant+=/scouring_tithe
                //ScouringTithe
                if (PlayerCovenantSettings == "Kyrian" && API.CanCast(ScouringTithe) && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (IsCooldowns && API.CanCast(SummonDarkglare) && !API.SpellISOnCooldown(SummonDarkglare) && PlayerLevel >= 42)
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
                //actions+=/drain_soul,interrupt=1
                if (DumpShards && API.CanCast(DrainSoul) && TalentDrainSoul && API.PlayerCurrentSoulShards <= 4 && NotChanneling)
                {
                    API.CastSpell(DrainSoul);
                    return;
                }
                //actions+=/drain_soul,interrupt=1
                if (!DumpShards && API.CanCast(DrainSoul) && TalentDrainSoul && API.PlayerCurrentSoulShards <= ShoulShardNumberDrainSoul && NotChanneling)
                {
                    API.CastSpell(DrainSoul);
                    return;
                }
                //actions.aoe+=/shadow_bolt
                //ShadowBolt
                if (API.CanCast(ShadowBolt) && !TalentDrainSoul && API.PlayerCurrentCastTimeRemaining > 40 && PlayerLevel >= 1)
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
            }
            //ROTATION SINGLE TARGET
            if (IsAOE || !IsAOE && IsRange && API.TargetUnitInRangeCount <= AOEUnitNumber)
            {
                //actions+=/agony,if=dot.agony.remains<4
                //Agony
                if (!CastingAgony && !CastingSOC && !LastSeed && API.CanCast(Agony) && API.TargetDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony);
                    return;
                }
                //actions+=/Corruption,if=dot.Corruption.remains<4
                //Corruption
                if (!CastingCorruption && !CastingSOC && !LastSeed && API.CanCast(Corruption) && API.TargetDebuffRemainingTime(Corruption) <= 400 && !API.TargetHasDebuff(SeedofCorruption) && IsRange && PlayerLevel >= 2)
                {
                    API.CastSpell(Corruption);
                    return;
                }
                //actions+=/phantom_singularity,if=time>30
                //PhantomSingularity
                if (TalentPhantomSingularity && API.CanCast(PhantomSingularity) && !API.TargetHasDebuff(PhantomSingularity))
                {
                    API.CastSpell(PhantomSingularity);
                    return;
                }
                //VileTaint
                if (TalentVileTaint && !API.TargetHasDebuff(VileTaint) && API.CanCast(VileTaint) && IsRange && API.PlayerCurrentSoulShards >= 1)
                {
                    API.CastSpell(VileTaint);
                    return;
                }

                //actions+=/haunt
                //Haunt 
                if (API.CanCast(Haunt) && !API.SpellISOnCooldown(Haunt) && TalentHaunt && PlayerLevel >= 45)
                {
                    API.CastSpell(Haunt);
                    return;
                }
                //actions+=/unstable_affliction,if=dot.unstable_affliction.remains<4
                //Unstable Affliction
                if (UseUA)
                {
                    if (!LastUnstableAffliction && API.CanCast(UnstableAffliction) && API.TargetDebuffRemainingTime(UnstableAffliction) <= 400 && PlayerLevel >= 13)
                    {
                        API.CastSpell(UnstableAffliction);
                        return;
                    }
                }
                //ImpendingCatastrophe
                if (API.CanCast(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && UseCovenantAbility == "always")
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //SoulRot
                if (API.CanCast(SoulRot) && PlayerCovenantSettings == "Night Fae" && UseCovenantAbility == "always")
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //ScouringTithe
                if (API.CanCast(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && UseCovenantAbility == "always")
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
                //actions+=/siphon_life,if=dot.siphon_life.remains<4
                if (!CastingSL && API.CanCast(SiphonLife) && TalentSiphonLife && API.TargetDebuffRemainingTime(SiphonLife) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
                //SoulRot
                if (API.CanCast(SoulRot) && PlayerCovenantSettings == "Night Fae" && UseCovenantAbility == "with Cooldowns" && IsCooldowns)
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (IsCooldowns && API.CanCast(SummonDarkglare) && !API.SpellISOnCooldown(SummonDarkglare) && PlayerLevel >= 42)
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
                //SoulRot
                if (API.CanCast(SoulRot) && PlayerCovenantSettings == "Night Fae" && UseCovenantAbility == "with Cooldowns" && IsCooldowns)
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions+=/dark_soul,if=cooldown.summon_darkglare.remains>time_to_die
                //DarkSoulMisery
                if (API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery && API.SpellCDDuration(SummonDarkglare) >= API.TargetTimeToDie)
                {
                    API.CastSpell(DarkSoulMisery);
                    return;
                }
                //Malefic Rapture Check
                if (!DumpShards && API.CanCast(MaleficRapture) && API.PlayerCurrentSoulShards >= ShoulShardNumberMaleficRapture && DotCheck && PlayerLevel >= 11)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                if (DumpShards && API.CanCast(MaleficRapture) && API.PlayerCurrentSoulShards >= 5 && DotCheck && PlayerLevel >= 11)
                {
                    DumpWatch.Start();
                    API.WriteLog("Starting Dump Shards.");
                    return;
                }
                //DecimatingBolt
                if (API.CanCast(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && UseCovenantAbility == "always")
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions+=/drain_soul,interrupt=1

                if (DumpShards && API.CanCast(DrainSoul) && TalentDrainSoul && API.PlayerCurrentSoulShards <= 4 && NotChanneling)
                {
                    API.CastSpell(DrainSoul);
                    return;
                }
                //actions+=/drain_soul,interrupt=1

                if (!DumpShards && API.CanCast(DrainSoul) && TalentDrainSoul && API.PlayerCurrentSoulShards <= ShoulShardNumberDrainSoul && NotChanneling)
                {
                    API.CastSpell(DrainSoul);
                    return;
                }
                //actions+=/shadow_bolt
                //ShadowBolt
                if (API.CanCast(ShadowBolt) && NotMoving && IsRange && NotChanneling && !TalentDrainSoul && PlayerLevel >= 1)
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
            }
        }


        public override void OutOfCombatPulse()
        {
            //Grimoire Of Sacrifice
            if (API.PlayerHasPet && TalentGrimoireOfSacrifice && API.PlayerHasBuff("Grimoire Of Sacrifice"))
            {
                API.CastSpell(GrimoireOfSacrifice);
                return;
            }
            //Summon Imp
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonImp) && API.PlayerCurrentCastTimeRemaining > 40 && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 3)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker) && API.PlayerCurrentCastTimeRemaining > 40 && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 10)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && API.PlayerCurrentCastTimeRemaining > 40 && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 19)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && API.PlayerCurrentCastTimeRemaining > 40 && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 23)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonFelhunter);
                return;
            }
        }
    }
}
