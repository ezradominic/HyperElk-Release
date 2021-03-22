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
        private string ShadowEmbrace = "Shadow Embrace";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string FelDomination = "Fel Domination";
        private string InevitableDemise = "Inevitable Demise";
        //Talents
        private bool TalentDrainSoul => API.PlayerIsTalentSelected(1, 3);
        private bool TalentSiphonLife => API.PlayerIsTalentSelected(2, 3);
        private bool TalentDarkPact => API.PlayerIsTalentSelected(3, 3);
        private bool TalentPhantomSingularity => API.PlayerIsTalentSelected(4, 2);
        private bool TalentVileTaint => API.PlayerIsTalentSelected(4, 3);
        private bool TalentHaunt => API.PlayerIsTalentSelected(6, 2);
        private bool TalentGrimoireOfSacrifice => API.PlayerIsTalentSelected(6, 3);
        private bool TalentDarkSoulMisery => API.PlayerIsTalentSelected(7, 3);
        private bool TalentSowTheSeeds => API.PlayerIsTalentSelected(4, 1);
        private bool TalentMortalCoil => API.PlayerIsTalentSelected(5, 2);
        //Misc
        private static readonly Stopwatch DumpWatchLow = new Stopwatch();
        private static readonly Stopwatch DumpWatchHigh = new Stopwatch();


        private bool IsRange => API.TargetRange < 40;
        private bool NotMoving => !API.PlayerIsMoving;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool DsM => API.ToggleIsEnabled("DarkSoul");



        //CBProperties      
        bool DotCheck => API.TargetHasDebuff(Corruption) && API.TargetHasDebuff(Agony) && API.TargetHasDebuff(UnstableAffliction) && (API.TargetHasDebuff(SoulRot) || !API.TargetHasDebuff(SoulRot));
        bool LastCastUnstableAffliction => API.LastSpellCastInGame == UnstableAffliction || API.CurrentCastSpellID("player") == 316099;
        bool LastCastScouringTithe => API.LastSpellCastInGame == ScouringTithe;
        bool LastCastAgony => API.LastSpellCastInGame == Agony;
        bool LastCastCorruption => API.LastSpellCastInGame == Corruption;
        bool LastCastSiphonLife => API.LastSpellCastInGame == SiphonLife;
        bool LastCastSeedOfCorruption => API.CurrentCastSpellID("player") == 27243 || API.LastSpellCastInGame == SeedofCorruption;
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        private int MortalCoilPercentProc => numbList[CombatRoutine.GetPropertyInt(MortalCoil)];


        string[] MisdirectionList = new string[] { "None", "Imp", "Voidwalker", "Succubus", "Felhunter", };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private bool UseAG => (bool)CombatRoutine.GetProperty("UseAG");
        private bool UseCO => (bool)CombatRoutine.GetProperty("UseCO");
        private bool UseSL => (bool)CombatRoutine.GetProperty("UseSL");
        private bool DumpShards => (bool)CombatRoutine.GetProperty("DumpShards");

        private int DarkPactPercentProc => numbList[CombatRoutine.GetPropertyInt(DarkPact)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];


        public override void Initialize()
        {
            CombatRoutine.Name = "Affliction Warlock @Mufflon12";
            API.WriteLog("Welcome to Affliction Warlock rotation @ Mufflon12");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("Create the following mouseover macro and assigned to the bind:");
            API.WriteLog("/cast [@mouseover] Agony");
            API.WriteLog("/cast [@mouseover] Corruption");
            API.WriteLog("/cast [@cursor] Vile Taint");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");


            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(MortalCoil, "Mortal Coil", numbList, "Life percent at which " + MortalCoil + " is used, set to 0 to disable", "Healing", 5);

            CombatRoutine.AddProp(HealthFunnel, "Health Funnel", numbList, "Life percent at which " + HealthFunnel + " is used, set to 0 to disable", "PETS", 0);
            CombatRoutine.AddProp(Misdirection, "Wich Pet", MisdirectionList, "Chose your Pet", "PETS", 0);
            CombatRoutine.AddProp(DarkPact, "Dark Pact", numbList, "Life percent at which " + DarkPact + " is used, set to 0 to disable", "Healing", 2);
            CombatRoutine.AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("UseAG", "Use Agony", true, "Use Agony for mouseover Multidots", "MultiDOTS");
            CombatRoutine.AddProp("UseCO", "Use Corruption", true, "Use Corruption for mouseover Multidots", "MultiDOTS");
            CombatRoutine.AddProp("UseSL", "Use Siphon Life", true, "Use Siphon Life for mouseover Multidots", "MultiDOTS");
            CombatRoutine.AddProp("DumpShards", "Dump Shards", true, "Collect 5 Soul Shards and befor using Malefic Rapture", "Class specific");
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            //Spells
            CombatRoutine.AddSpell(ShadowBolt, 686, "D1");
            CombatRoutine.AddSpell(DrainSoul, 198590, "D1");
            CombatRoutine.AddSpell(Corruption, 172, "D2");
            CombatRoutine.AddSpell(Agony, 980, "D3");
            CombatRoutine.AddSpell(MaleficRapture, 324536, "D4");
            CombatRoutine.AddSpell(UnstableAffliction, 316099, "D5");
            CombatRoutine.AddSpell(SeedofCorruption, 27243, "D6");
            CombatRoutine.AddSpell(SiphonLife, 63106, "D7");
            CombatRoutine.AddSpell(PhantomSingularity, 205179, "D8");
            CombatRoutine.AddSpell(Haunt, 48181, "D8");
            CombatRoutine.AddSpell(DarkSoulMisery, 113860, "D8");
            CombatRoutine.AddSpell(MortalCoil, 6789, "D9");
            CombatRoutine.AddSpell(VileTaint, 278350, "D8");
            CombatRoutine.AddSpell(ScouringTithe, 312321, "F1");
            CombatRoutine.AddSpell(SoulRot, 325640, "F1");
            CombatRoutine.AddSpell(ImpendingCatastrophe, 321792, "F1");
            CombatRoutine.AddSpell(DecimatingBolt, 325289, "F1");
            CombatRoutine.AddSpell(FelDomination, 333889);

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);
            CombatRoutine.AddMacro(Agony + "MO", "F1");
            CombatRoutine.AddMacro(Corruption + "MO", "F2");
            CombatRoutine.AddMacro(SiphonLife + "MO", "F3");



            CombatRoutine.AddSpell("Drain Life", 234153, "NumPad1");
            CombatRoutine.AddSpell("Health Funnel", 755, "NumPad2");
            CombatRoutine.AddSpell("Dark Pact", 108416, "NumPad3");
            CombatRoutine.AddSpell("Grimoire Of Sacrifice", 108503, "NumPad4");
            CombatRoutine.AddSpell("Summon Darkglare", 205180, "NumPad5");
            CombatRoutine.AddSpell("Summon Felhunter", 691, "NumPad6");
            CombatRoutine.AddSpell("Summon Succubus", 712, "NumPad7");
            CombatRoutine.AddSpell("Summon Voidwalker", 697, "NumPad8");
            CombatRoutine.AddSpell("Summon Imp", 688, "NumPad9");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            //Buffs
            CombatRoutine.AddBuff("Grimoire Of Sacrifice", 108503);
            CombatRoutine.AddBuff(FelDomination, 333889);
            CombatRoutine.AddBuff(InevitableDemise, 334320);

            //Debuffs
            CombatRoutine.AddDebuff(ImpendingCatastrophe, 321792);
            CombatRoutine.AddDebuff(Corruption, 146739);
            CombatRoutine.AddDebuff(Agony, 980);
            CombatRoutine.AddDebuff(UnstableAffliction, 316099);
            CombatRoutine.AddDebuff(SiphonLife, 63106);
            CombatRoutine.AddDebuff(SeedofCorruption, 27243);
            CombatRoutine.AddDebuff(VileTaint, 278350);
            CombatRoutine.AddDebuff(PhantomSingularity, 205179);
            CombatRoutine.AddDebuff(Haunt, 48181);
            CombatRoutine.AddDebuff(SoulRot, 325640);
            CombatRoutine.AddDebuff(ShadowEmbrace, 32390);

            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("DarkSoul");

        }


        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if (API.PlayerHasBuff(FelDomination) && !TalentGrimoireOfSacrifice && API.CanCast(SummonImp) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonImp);
                return;
            }
            if (API.PlayerHasBuff(FelDomination) && !TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonVoidwalker);
                return;
            }
            if (API.PlayerHasBuff(FelDomination) && !TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonSuccubus);
                return;
            }
            if (API.PlayerHasBuff(FelDomination) && !TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonFelhunter);
                return;
            }
            if (!API.PlayerHasPet && (isMisdirection == "Felhunter" || isMisdirection == "Succubus" || isMisdirection == "Voidwalker" || isMisdirection == "Imp") && API.CanCast(FelDomination))
            {
                API.CastSpell(FelDomination);
                return;
            }
            if (IsMouseover && UseCO && !LastCastCorruption && API.CanCast(Corruption) && !API.MacroIsIgnored(Corruption + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Corruption) <= 400 && !API.TargetHasDebuff(SeedofCorruption) && IsRange)
            {
                API.CastSpell(Corruption + "MO");
                return;
            }
            if (IsMouseover && UseAG && !LastCastAgony && API.CanCast(Agony) && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange)
            {
                API.CastSpell(Agony + "MO");
                return;
            }
            if (IsMouseover && UseSL && !LastCastSiphonLife && API.CanCast(SiphonLife) && !API.MacroIsIgnored(SiphonLife + "MO") && API.PlayerCanAttackMouseover && TalentSiphonLife && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(SiphonLife) <= 400 && IsRange)
            {
                API.CastSpell(SiphonLife + "MO");
                return;
            }
            if (API.PlayerHealthPercent <= DrainLifePercentProc && API.CanCast(DrainLife))
            {
                API.CastSpell(DrainLife);
                return;
            }
            if (API.PlayerHasPet && API.PetHealthPercent >= 0 && API.PetHealthPercent <= HealthFunnelPercentProc && API.CanCast(HealthFunnel))
            {
                API.CastSpell(HealthFunnel);
                return;
            }
            if (API.PlayerHealthPercent <= DarkPactPercentProc && TalentDarkPact & API.CanCast(DarkPact))
            {
                API.CastSpell(DarkPact);
                return;
            }
            if (API.PlayerHealthPercent <= MortalCoilPercentProc && TalentMortalCoil && API.CanCast(MortalCoil))
            {
                API.CastSpell(MortalCoil);
                return;
            }
            rotation();
            return;
        }

        private void rotation()
        {
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell("Trinket1");
                return;
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell("Trinket2");
                return;
            }
            if (DumpShards && DumpWatchHigh.IsRunning && API.PlayerCurrentSoulShards <= 0)
            {
                DumpWatchHigh.Reset();
            }
            if (DumpShards && DumpWatchHigh.IsRunning && API.CanCast(MaleficRapture) && DotCheck && IsRange)
            {
                if (!API.SpellISOnCooldown(PhantomSingularity) && TalentPhantomSingularity)
                {
                    API.CastSpell(PhantomSingularity);
                }
                if (!API.SpellISOnCooldown(VileTaint) && TalentVileTaint)
                {
                    API.CastSpell(VileTaint);
                }
                if (TalentPhantomSingularity && (API.TargetHasDebuff(PhantomSingularity) || API.SpellISOnCooldown(PhantomSingularity)) || TalentVileTaint && (API.TargetHasDebuff(VileTaint) || API.SpellISOnCooldown(VileTaint)))
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }

            }
            if (API.CanCast(MaleficRapture) && API.TargetDebuffStacks(ShadowEmbrace) == 3 && DotCheck && IsRange && API.PlayerCurrentSoulShards >= 5)
            {
                DumpWatchHigh.Start();
                return;
            }

            //# Executed every time the actor is available.
            //actions=call_action_list,name=aoe,if=active_enemies>3
            if (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE && IsRange)
            {
                //actions.aoe=phantom_singularity
                if (API.CanCast(PhantomSingularity) && TalentPhantomSingularity && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(PhantomSingularity);
                    return;
                }
                //actions.aoe+=/haunt
                if (API.CanCast(Haunt) && TalentHaunt && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(Haunt);
                    return;
                }
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&dot.impending_catastrophe_dot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
                if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && API.TargetHasDebuff(ImpendingCatastrophe) && API.SpellCDDuration(SummonDarkglare) < 200 && (API.TargetDebuffRemainingTime(PhantomSingularity) > 200 || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                    if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    //actions.darkglare_prep+=/potion
                    //actions.darkglare_prep+=/fireblood
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/blood_fury
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/berserking
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                    if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                    {
                        //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/scouring_tithe
                        if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ScouringTithe);
                            return;
                        }
                    }
                    //actions.darkglare_prep+=/summon_darkglare
                    if (API.CanCast(SummonDarkglare))
                    {
                        API.CastSpell(SummonDarkglare);
                        return;
                    }

                }
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&dot.soul_rot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
                if (IsCooldowns && PlayerCovenantSettings == "Night Fae" && API.TargetHasDebuff(SoulRot) && API.SpellCDDuration(SummonDarkglare) < 200 && (API.SpellCDDuration(PhantomSingularity) > 200 && TalentPhantomSingularity || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                    if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    //actions.darkglare_prep+=/potion
                    //actions.darkglare_prep+=/fireblood
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/blood_fury
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/berserking
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                    if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                    {
                        //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/scouring_tithe
                        if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ScouringTithe);
                            return;
                        }
                    }
                    //actions.darkglare_prep+=/summon_darkglare
                    if (API.CanCast(SummonDarkglare))
                    {
                        API.CastSpell(SummonDarkglare);
                        return;
                    }
                }
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&dot.phantom_singularity.ticking&dot.phantom_singularity.remains<2
                if (IsCooldowns && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.TargetHasDebuff(PhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                    if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    //actions.darkglare_prep+=/potion
                    //actions.darkglare_prep+=/fireblood
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/blood_fury
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/berserking
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                    if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                    {
                        //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/scouring_tithe
                        if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ScouringTithe);
                            return;
                        }
                    }
                    //actions.darkglare_prep+=/summon_darkglare
                    if (API.CanCast(SummonDarkglare))
                    {
                        API.CastSpell(SummonDarkglare);
                        return;
                    }
                }
                //actions.aoe+=/seed_of_corruption,if=talent.sow_the_seeds.enabled&can_seed
                if (API.CanCast(SeedofCorruption) && !LastCastSeedOfCorruption && TalentSowTheSeeds && !API.TargetHasDebuff(SeedofCorruption) && (API.TargetDebuffRemainingTime(Corruption) <= 400 || API.TargetDebuffRemainingTime(SeedofCorruption) <= 1000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SeedofCorruption);
                    return;
                }
                //actions.aoe+=/seed_of_corruption,if=!talent.sow_the_seeds.enabled&!dot.seed_of_corruption.ticking&!in_flight&dot.corruption.refreshable
                if (!LastCastSeedOfCorruption && !API.TargetHasDebuff(SeedofCorruption) && !API.TargetHasDebuff(Corruption) && API.CanCast(SeedofCorruption) && IsRange && API.PlayerCurrentSoulShards >= 1 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SeedofCorruption);
                    return;
                }
                //actions.aoe+=/agony,cycle_targets=1,if=active_dot.agony<4,target_if=!dot.agony.ticking
                //actions.aoe+=/agony,cycle_targets=1,if=active_dot.agony>=4,target_if=refreshable&dot.agony.ticking
                //actions.aoe+=/unstable_affliction,if=dot.unstable_affliction.refreshable
                if (API.CanCast(UnstableAffliction) && !LastCastUnstableAffliction && API.TargetDebuffRemainingTime(UnstableAffliction) < 200)
                {
                    API.CastSpell(UnstableAffliction);
                }
                //actions.aoe+=/vile_taint,if=soul_shard>1
                if (API.CanCast(VileTaint) && TalentVileTaint && API.PlayerCurrentSoulShards > 1 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.aoe+=/call_action_list,name=covenant,if=!covenant.necrolord
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr")
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&(cooldown.impending_catastrophe.ready|dot.impending_catastrophe_dot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
                if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && (!API.SpellISOnCooldown(ImpendingCatastrophe) || API.TargetHasDebuff(ImpendingCatastrophe)) && API.SpellCDDuration(SummonDarkglare) < 200 && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                    if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    //actions.darkglare_prep+=/potion
                    //actions.darkglare_prep+=/fireblood
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/blood_fury
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/berserking
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                    if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                    {
                        //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/scouring_tithe
                        if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ScouringTithe);
                            return;
                        }
                    }
                    //actions.darkglare_prep+=/summon_darkglare
                    if (API.CanCast(SummonDarkglare))
                    {
                        API.CastSpell(SummonDarkglare);
                        return;
                    }
                }
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
                if (IsCooldowns && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.SpellCDDuration(SummonDarkglare) < 200 && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                    if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    //actions.darkglare_prep+=/potion
                    //actions.darkglare_prep+=/fireblood
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/blood_fury
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/berserking
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                    if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                    {
                        //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/scouring_tithe
                        if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ScouringTithe);
                            return;
                        }
                    }
                    //actions.darkglare_prep+=/summon_darkglare
                    if (API.CanCast(SummonDarkglare))
                    {
                        API.CastSpell(SummonDarkglare);
                        return;
                    }
                }
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&(cooldown.soul_rot.ready|dot.soul_rot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
                if (IsCooldowns && PlayerCovenantSettings == "Night Fae" && (!API.SpellISOnCooldown(SoulRot) || API.TargetHasDebuff(SoulRot)) && API.SpellCDDuration(SummonDarkglare) < 200 && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                    if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    //actions.darkglare_prep+=/potion
                    //actions.darkglare_prep+=/fireblood
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/blood_fury
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/berserking
                    if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                    if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                    {
                        //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/scouring_tithe
                        if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ScouringTithe);
                            return;
                        }
                    }
                    //actions.darkglare_prep+=/summon_darkglare
                    if (API.CanCast(SummonDarkglare))
                    {
                        API.CastSpell(SummonDarkglare);
                        return;
                    }
                }
                //actions.aoe+=/dark_soul,if=cooldown.summon_darkglare.remains>time_to_die
                if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery && API.SpellISOnCooldown(SummonDarkglare) && (API.TargetIsBoss && API.TargetHealthPercent >= 10 || !API.TargetIsBoss && API.TargetHealthPercent > 30) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(DarkSoulMisery);
                    return;
                }
                //actions.aoe+=/call_action_list,name=item
                //actions.aoe+=/malefic_rapture,if=dot.vile_taint.ticking
                if (!DumpShards && API.CanCast(MaleficRapture) && API.TargetHasDebuff(VileTaint) && TalentVileTaint && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                //actions.aoe+=/malefic_rapture,if=dot.soul_rot.ticking&!talent.sow_the_seeds.enabled
                if (!DumpShards && API.CanCast(MaleficRapture) && API.TargetHasDebuff(SoulRot) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                //actions.aoe+=/malefic_rapture,if=!talent.vile_taint.enabled
                //actions.aoe+=/malefic_rapture,if=soul_shard>4
                if (!DumpShards && API.CanCast(MaleficRapture) && DotCheck && API.PlayerCurrentSoulShards >= 4 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                //actions.aoe+=/siphon_life,cycle_targets=1,if=active_dot.siphon_life<=3,target_if=!dot.siphon_life.ticking
                //actions.aoe+=/call_action_list,name=covenant
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr")
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.aoe+=/drain_life,if=buff.inevitable_demise.stack>=50|buff.inevitable_demise.up&time_to_die<5|buff.inevitable_demise.stack>=35&dot.soul_rot.ticking
                if (API.CanCast(DrainLife) && NotChanneling && API.PlayerBuffStacks(InevitableDemise) > 50 || (API.PlayerBuffStacks(InevitableDemise) >= 1 && API.TargetTimeToDie < 500) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(DrainLife);
                    return;
                }
                //actions.aoe+=/drain_soul,interrupt=1
                //actions.aoe+=/shadow_bolt
                if (API.CanCast(ShadowBolt) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
            }
            //actions+=/phantom_singularity,if=time>30
            if (API.CanCast(PhantomSingularity) && TalentPhantomSingularity && !DumpShards && API.PlayerTimeInCombat > 30000 && !API.PlayerIsCasting(true))
            {
                API.CastSpell(PhantomSingularity);
                return;
            }
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&dot.impending_catastrophe_dot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && (!API.SpellISOnCooldown(ImpendingCatastrophe) || API.TargetHasDebuff(ImpendingCatastrophe)) && API.SpellCDDuration(SummonDarkglare) < 200 && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&dot.soul_rot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (IsCooldowns && PlayerCovenantSettings == "Night Fae" && API.TargetHasDebuff(SoulRot) && API.SpellCDDuration(SummonDarkglare) < 200 && (API.SpellCDDuration(PhantomSingularity) > 200 && TalentPhantomSingularity || !TalentPhantomSingularity))
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&dot.phantom_singularity.ticking&dot.phantom_singularity.remains<2
            if (IsCooldowns && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/agony,if=dot.agony.remains<4
            if (API.CanCast(Agony) && !LastCastAgony && API.TargetDebuffRemainingTime(Agony) <= 400)
            {
                API.CastSpell(Agony);
                return;
            }
            //actions+=/agony,cycle_targets=1,if=active_enemies>1,target_if=dot.agony.remains<4
            //actions+=/haunt
            if (API.CanCast(Haunt) && TalentHaunt && !API.PlayerIsCasting(true))
            {
                API.CastSpell(Haunt);
                return;
            }
            //actions+=/call_action_list,name=darkglare_prep,if=active_enemies>2&covenant.venthyr&(cooldown.impending_catastrophe.ready|dot.impending_catastrophe_dot.ticking)&(dot.phantom_singularity.ticking|!talent.phantom_singularity.enabled)
            if (IsCooldowns && API.TargetUnitInRangeCount > 2 && PlayerCovenantSettings == "Venthyr" && (!API.SpellISOnCooldown(ImpendingCatastrophe) || API.TargetHasDebuff(ImpendingCatastrophe)) && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/call_action_list,name=darkglare_prep,if=active_enemies>2&(covenant.necrolord|covenant.kyrian|covenant.none)&(dot.phantom_singularity.ticking|!talent.phantom_singularity.enabled)
            if (IsCooldowns && API.TargetUnitInRangeCount > 2 && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity)
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/call_action_list,name=darkglare_prep,if=active_enemies>2&covenant.night_fae&(cooldown.soul_rot.ready|dot.soul_rot.ticking)&(dot.phantom_singularity.ticking|!talent.phantom_singularity.enabled)
            if (IsCooldowns && API.TargetUnitInRangeCount > 2 && PlayerCovenantSettings == "Night Fae" && (!API.SpellISOnCooldown(SoulRot) || API.TargetHasDebuff(SoulRot)) && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/seed_of_corruption,if=active_enemies>2&talent.sow_the_seeds.enabled&!dot.seed_of_corruption.ticking&!in_flight
            if (API.CanCast(SeedofCorruption) && !LastCastSeedOfCorruption && API.TargetUnitInRangeCount > 2 && TalentSowTheSeeds && !API.TargetHasDebuff(Corruption) && !API.PlayerIsCasting(true))
            {
                API.CastSpell(SeedofCorruption);
                return;
            }
            //actions+=/seed_of_corruption,if=active_enemies>2&talent.siphon_life.enabled&!dot.seed_of_corruption.ticking&!in_flight&dot.corruption.remains<4
            if (API.CanCast(SeedofCorruption) && !LastCastSeedOfCorruption && API.TargetUnitInRangeCount > 2 && TalentSiphonLife && !API.TargetHasDebuff(SeedofCorruption) && API.TargetDebuffRemainingTime(Corruption) < 400 && !API.PlayerIsCasting(true))
            {
                API.CastSpell(SeedofCorruption);
                return;
            }
            //actions+=/vile_taint,if=(soul_shard>1|active_enemies>2)&cooldown.summon_darkglare.remains>12
            if (API.CanCast(VileTaint) && TalentVileTaint && (API.PlayerCurrentSoulShards > 1 || API.TargetUnitInRangeCount > 2) && !API.PlayerIsCasting(true))
            {
                API.CastSpell(VileTaint);
                return;
            }
            //actions+=/unstable_affliction,if=dot.unstable_affliction.remains<4
            if (API.CanCast(UnstableAffliction) && !LastCastUnstableAffliction && API.TargetDebuffRemainingTime(UnstableAffliction) < 200)
            {
                API.CastSpell(UnstableAffliction);
                return;
            }
            //actions+=/siphon_life,if=dot.siphon_life.remains<4
            if (API.CanCast(SiphonLife) && !LastCastSiphonLife && TalentSiphonLife && API.TargetDebuffRemainingTime(SiphonLife) < 400)
            {
                API.CastSpell(SiphonLife);
                return;
            }
            //actions+=/siphon_life,cycle_targets=1,if=active_enemies>1,target_if=dot.siphon_life.remains<4
            //actions+=/call_action_list,name=covenant,if=!covenant.necrolord
            if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr")
            {
                //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions.covenant+=/scouring_tithe
                if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
            }
            //actions+=/corruption,if=active_enemies<4-(talent.sow_the_seeds.enabled|talent.siphon_life.enabled)&dot.corruption.remains<2
            if (API.CanCast(Corruption) && !LastCastCorruption && API.TargetDebuffRemainingTime(Corruption) <= 200)
            {
                API.CastSpell(Corruption);
                return;
            }
            //actions+=/corruption,cycle_targets=1,if=active_enemies<4-(talent.sow_the_seeds.enabled|talent.siphon_life.enabled),target_if=dot.corruption.remains<2
            //actions+=/phantom_singularity
            if (API.CanCast(PhantomSingularity) && TalentPhantomSingularity)
            {
                API.CastSpell(PhantomSingularity);
                return;
            }
            //actions+=/malefic_rapture,if=soul_shard>4
            if (!DumpShards && API.CanCast(MaleficRapture) && DotCheck && API.PlayerCurrentSoulShards > 4 && !API.PlayerIsCasting(true))
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&(cooldown.impending_catastrophe.ready|dot.impending_catastrophe_dot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && (!API.SpellISOnCooldown(ImpendingCatastrophe) || API.TargetHasDebuff(ImpendingCatastrophe)) && API.SpellCDDuration(SummonDarkglare) < 200 && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None" && API.SpellCDDuration(SummonDarkglare) < 200 && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&(cooldown.soul_rot.ready|dot.soul_rot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (IsCooldowns && PlayerCovenantSettings == "Night Fae" && (!API.SpellISOnCooldown(SoulRot) || API.TargetHasDebuff(SoulRot)) && API.SpellCDDuration(SummonDarkglare) < 200 && (API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity))
            {
                //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
                if (API.CanCast(VileTaint) && TalentVileTaint && API.SpellCDDuration(SummonDarkglare) < 200 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                //actions.darkglare_prep+=/potion
                //actions.darkglare_prep+=/fireblood
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/blood_fury
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/berserking
                if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
                if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr" || API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe
                    if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                }
                //actions.darkglare_prep+=/summon_darkglare
                if (API.CanCast(SummonDarkglare))
                {
                    API.CastSpell(SummonDarkglare);
                    return;
                }
            }
            //actions+=/dark_soul,if=cooldown.summon_darkglare.remains>time_to_die
            if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery && API.SpellISOnCooldown(SummonDarkglare) && (API.TargetIsBoss && API.TargetHealthPercent >= 10 || !API.TargetIsBoss && API.TargetHealthPercent > 30) && !API.PlayerIsCasting(true))
            {
                API.CastSpell(DarkSoulMisery);

            }
            //actions+=/call_action_list,name=item
            //actions+=/call_action_list,name=se,if=debuff.shadow_embrace.stack<(2-action.shadow_bolt.in_flight)|debuff.shadow_embrace.remains<3
            if (API.TargetDebuffStacks(ShadowEmbrace) < 3 || API.TargetDebuffRemainingTime(ShadowEmbrace) < 300 && !API.PlayerIsCasting(true))
            {
                //actions.se = haunt
                if (API.CanCast(Haunt) && TalentHaunt)
                {
                    API.CastSpell(Haunt);
                    return;
                }
                //actions.se +=/ drain_soul,interrupt_global = 1,interrupt_if = debuff.shadow_embrace.stack >= 3
                if (API.CanCast(DrainSoul) && TalentDrainSoul)
                {
                    API.CastSpell(DrainSoul);
                    return;
                }
                //actions.se +=/ shadow_bolt
                if (API.CanCast(ShadowBolt) && !TalentDrainSoul)
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
            }
            //actions+=/malefic_rapture,if=dot.vile_taint.ticking
            if (!DumpShards && API.CanCast(MaleficRapture) && API.TargetHasDebuff(VileTaint) && TalentVileTaint && !API.PlayerIsCasting(true))
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //actions+=/malefic_rapture,if=dot.impending_catastrophe_dot.ticking
            if (!DumpShards && API.CanCast(MaleficRapture) && API.TargetHasDebuff(ImpendingCatastrophe) && !API.PlayerIsCasting(true))
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //actions+=/malefic_rapture,if=dot.soul_rot.ticking
            if (!DumpShards && API.CanCast(MaleficRapture) && API.TargetHasDebuff(SoulRot) && !API.PlayerIsCasting(true))
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //actions+=/malefic_rapture,if=talent.phantom_singularity.enabled&(dot.phantom_singularity.ticking|soul_shard>3|time_to_die<cooldown.phantom_singularity.remains)
            if (!DumpShards && API.CanCast(MaleficRapture) && TalentPhantomSingularity && (API.TargetHasDebuff(PhantomSingularity) || API.PlayerCurrentSoulShards > 3 || API.TargetTimeToDie < API.SpellCDDuration(PhantomSingularity)) && !API.PlayerIsCasting(true))
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //actions+=/malefic_rapture,if=talent.sow_the_seeds.enabled
            //actions+=/drain_life,if=buff.inevitable_demise.stack>40|buff.inevitable_demise.up&time_to_die<4
            //actions+=/call_action_list,name=covenant
            if (PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Night Fae" ||PlayerCovenantSettings == "Venthyr")
            {
                //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
                if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
                if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
                if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(SummonDarkglare) < 5000 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions.covenant+=/scouring_tithe
                if (!API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
            }
            //actions+=/agony,if=refreshable
            if (API.CanCast(Agony) && API.TargetDebuffRemainingTime(Agony) < 200)
            {
                API.CastSpell(Agony);
                return;
            }
            //actions+=/agony,cycle_targets=1,if=active_enemies>1,target_if=refreshable
            //actions+=/corruption,if=refreshable&active_enemies<4-(talent.sow_the_seeds.enabled|talent.siphon_life.enabled)
            //actions+=/unstable_affliction,if=refreshable
            if (API.CanCast(UnstableAffliction) && API.TargetDebuffRemainingTime(UnstableAffliction) < 200)
            {
                API.CastSpell(UnstableAffliction);
            }
            //actions+=/siphon_life,if=refreshable
            //actions+=/siphon_life,cycle_targets=1,if=active_enemies>1,target_if=refreshable
            //actions+=/corruption,cycle_targets=1,if=active_enemies<4-(talent.sow_the_seeds.enabled|talent.siphon_life.enabled),target_if=refreshable

            //actions+=/drain_soul,interrupt=1
            if (API.CanCast(DrainSoul) && TalentDrainSoul && NotChanneling)
            {
                API.CastSpell(DrainSoul);
                return;
            }
            //actions+=/shadow_bolt
            if (API.CanCast(ShadowBolt) && !TalentDrainSoul)
            {
                API.CastSpell(ShadowBolt);
                return;
            }
        }

        public override void OutOfCombatPulse()
        {
            if (DumpWatchHigh.IsRunning)
            {
                DumpWatchHigh.Reset();
            }
            //Grimoire Of Sacrifice
            if (API.PlayerHasPet && TalentGrimoireOfSacrifice && API.PlayerHasBuff("Grimoire Of Sacrifice"))
            {
                API.CastSpell(GrimoireOfSacrifice);
                return;
            }
            //Summon Imp
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonImp) && !API.PlayerIsCasting(true) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && IsRange && NotChanneling)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker) && !API.PlayerIsCasting(true) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && IsRange && NotChanneling)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && !API.PlayerIsCasting(true) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && IsRange && NotChanneling)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && !API.PlayerIsCasting(true) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && IsRange && NotChanneling)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonFelhunter);
                return;
            }
        }
    }
}