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
        private string SoulRot = "Soul Rot";
        private string ImpendingCatastrophe = "Impending Catastrophe";
        private string Trinket1 = "Trinket1";
        private string Trinket2 = "Trinket2";

        private string MortalCoil = "Mortal Coil";
        private string DecimatingBolt = "Decimating Bolt";
        private string ShadowEmbrace = "Shadow Embrace";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string FelDomination = "Fel Domination";
        private string InevitableDemise = "Inevitable Demise";
        private string SpellLock = "Spell Lock";
        private string UnendingResolve = "Unending Resolve";
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


        private bool IsRange => API.TargetRange < 40;
        private bool NotMoving => !API.PlayerIsMoving;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool DsM => API.ToggleIsEnabled("DarkSoul");



        //CBProperties      
        bool DotCheck => TargetHasDebuff(Corruption) && TargetHasDebuff(Agony) && TargetHasDebuff(UnstableAffliction);
        bool CurrentCastDrainSoul => API.PlayerCurrentCastSpellID == 198590;
        bool CurrentCastMaleficRapture => API.PlayerCurrentCastSpellID == 324536;
        bool LastCastUnstableAffliction => API.LastSpellCastInGame == UnstableAffliction || API.PlayerCurrentCastSpellID == 316099;
        bool LastCastScouringTithe => API.LastSpellCastInGame == ScouringTithe;
        bool LastCastAgony => API.LastSpellCastInGame == Agony;
        bool LastCastCorruption => API.LastSpellCastInGame == Corruption;
        bool LastCastSiphonLife => API.LastSpellCastInGame == SiphonLife;
        bool LastCastSeedOfCorruption => API.CurrentCastSpellID("player") == 27243 || API.LastSpellCastInGame == SeedofCorruption;
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        private int MortalCoilPercentProc => numbList[CombatRoutine.GetPropertyInt(MortalCoil)];


        string[] MisdirectionList = new string[] { "None", "Imp", "Voidwalker", "Succubus", "Felhunter", };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private bool UseAG => (bool)CombatRoutine.GetProperty("UseAG");
        private bool UseCO => (bool)CombatRoutine.GetProperty("UseCO");
        private bool UseSL => (bool)CombatRoutine.GetProperty("UseSL");

        private int DarkPactPercentProc => numbList[CombatRoutine.GetPropertyInt(DarkPact)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        string[] TrinketList = new string[] { "never", "With Cooldowns", "On Cooldown", "on AOE", "on HP" };

        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        private int Trinket1HP => numbList[CombatRoutine.GetPropertyInt("Trinket1HP")];
        private int Trinket2HP => numbList[CombatRoutine.GetPropertyInt("Trinket2HP")];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private static bool TargetHasDebuff(string buff)
        {
            return API.TargetHasDebuff(buff, false, true);
        }
        private static bool MouseoverHasDebuff(string buff)
        {
            return API.MouseoverHasDebuff(buff, false, true);
        }
        private static int TargetDebuffRemainingTime(string buff)
        {
            return API.TargetDebuffRemainingTime(buff, true);
        }
        private static int MouseoverDebuffRemainingTime(string buff)
        {
            return API.MouseoverDebuffRemainingTime(buff, true);
        }
        float UaTime => 15 / (1f + API.PlayerGetHaste / 10);
        int[] UnendingResolveList =
        {
            345397,
            329455,
            325384,
            337110,
            332687,
            331209,
            332683,
            322236,
            321247,
            321828,
            328125,
            334625,
        };
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
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", TrinketList, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", TrinketList, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket1HP", "Trinket 1" + " Life Percent", numbList, " Life percent at which" + "Trinket 1" + " is used, set to 0 to disable", "Trinkets", 40);
            CombatRoutine.AddProp("Trinket2HP", "Trinket 2" + " Life Percent", numbList, " Life percent at which" + "Trinket 2" + " is used, set to 0 to disable", "Trinkets", 40);

            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 4);

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
            CombatRoutine.AddSpell(SpellLock, 19647);
            CombatRoutine.AddSpell(UnendingResolve, 104773);

            //Macro
            CombatRoutine.AddMacro(Trinket1);
            CombatRoutine.AddMacro(Trinket2);
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

            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);
        }


        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if ((!API.PlayerHasPet || API.PlayerHasPet && API.PetHealthPercent <= 0) && (isMisdirection == "Felhunter" || isMisdirection == "Succubus" || isMisdirection == "Voidwalker" || isMisdirection == "Imp") && API.CanCast(FelDomination))
            {
                API.CastSpell(FelDomination);
                return;
            }
            if (API.PlayerHasBuff(FelDomination) && !TalentGrimoireOfSacrifice && API.CanCast(SummonImp) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && !API.PlayerIsCasting(false))
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonImp);
                return;
            }
            if (API.PlayerHasBuff(FelDomination) && !TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && !API.PlayerIsCasting(false))
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonVoidwalker);
                return;
            }
            if (API.PlayerHasBuff(FelDomination) && !TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && !API.PlayerIsCasting(false))
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonSuccubus);
                return;
            }
            if (API.PlayerHasBuff(FelDomination) && !TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && !API.PlayerIsCasting(false))
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonFelhunter);
                return;
            }

            if (API.PlayerHealthPercent <= DrainLifePercentProc && API.CanCast(DrainLife))
            {
                API.CastSpell(DrainLife);
                return;
            }
            if (API.PlayerHasPet && API.PetHealthPercent > 0 && API.PetHealthPercent <= HealthFunnelPercentProc && API.CanCast(HealthFunnel))
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
            if (API.CanCast(UnendingResolve) && UnendingResolveList.Contains(API.TargetCurrentCastSpellID))
            {
                API.CastSpell(UnendingResolve);
                return;
            }
            rotation();
            return;
        }


        private void rotation()
        {
            if (isInterrupt && API.CanCast(SpellLock) && API.PlayerHasPet && isMisdirection == "Felhunter")
            {
                API.CastSpell(SpellLock);
                return;
            }
            if (API.PlayerItemCanUse(PhialofSerenity) && API.PlayerItemRemainingCD(PhialofSerenity) == 0 && API.PlayerHealthPercent <= PhialofSerenityLifePercent)
            {
                API.CastSpell(PhialofSerenity);
                return;
            }
            if (API.PlayerItemCanUse(SpiritualHealingPotion) && API.PlayerItemRemainingCD(SpiritualHealingPotion) == 0 && API.PlayerHealthPercent <= SpiritualHealingPotionLifePercent)
            {
                API.CastSpell(SpiritualHealingPotion);
                return;
            }
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE || UseTrinket1 == "on HP" && API.PlayerHealthPercent <= Trinket1HP))
            {
                API.CastSpell(Trinket1);
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE || UseTrinket2 == "on HP" && API.PlayerHealthPercent <= Trinket2HP))
            {
                API.CastSpell(Trinket2);
            }

            //# Executed every time the actor is available.
            if (API.CanCast(Haunt) && TalentHaunt && TargetDebuffRemainingTime(Haunt) < 200)
            {
                API.CastSpell(Haunt);
                return;
            }
            //actions=call_action_list,name=aoe,if=active_enemies>3
            if (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE && IsRange)
            {
                //actions.aoe=phantom_singularity
                if (API.CanCast(PhantomSingularity) && TalentPhantomSingularity)
                {
                    API.CastSpell(PhantomSingularity);
                    return;
                }
                //actions.aoe+=/haunt
                if (API.CanCast(Haunt) && TalentHaunt)
                {
                    API.CastSpell(Haunt);
                    return;
                }
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&dot.impending_catastrophe_dot.ticking&cooldown.summon_darkglare.ready&(dot.phantom_singularity.remains>2|!talent.phantom_singularity)
                if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && TargetHasDebuff(ImpendingCatastrophe) && API.CanCast(SummonDarkglare) && (TargetDebuffRemainingTime(PhantomSingularity) > 200 || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint
                    if (API.CanCast(VileTaint) && TalentVileTaint)
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                    {
                        API.CanCast(DarkSoulMisery);
                        return;
                    }
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
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                    if (PlayerCovenantSettings != "Necrolord")
                    {
                        //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&dot.soul_rot.ticking&cooldown.summon_darkglare.ready&(dot.phantom_singularity.remains>2|!talent.phantom_singularity)
                if (IsCooldowns && PlayerCovenantSettings == "Night Fae" && TargetHasDebuff(SoulRot) && API.CanCast(SummonDarkglare) && (TargetDebuffRemainingTime(PhantomSingularity) > 200 || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint
                    if (API.CanCast(VileTaint) && TalentVileTaint)
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                    {
                        API.CanCast(DarkSoulMisery);
                        return;
                    }
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
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                    if (PlayerCovenantSettings != "Necrolord")
                    {
                        //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
                if (IsCooldowns && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && TargetHasDebuff(PhantomSingularity) && TargetDebuffRemainingTime(PhantomSingularity) < 200)
                {
                    //actions.darkglare_prep=vile_taint
                    if (API.CanCast(VileTaint) && TalentVileTaint)
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                    {
                        API.CanCast(DarkSoulMisery);
                        return;
                    }
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
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                    if (PlayerCovenantSettings != "Necrolord")
                    {
                        //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
                //actions.aoe+=/seed_of_corruption,if=talent.sow_the_seeds&can_seed
                if (API.CanCast(SeedofCorruption) && !LastCastSeedOfCorruption && TalentSowTheSeeds && !TargetHasDebuff(SeedofCorruption) && (TargetDebuffRemainingTime(Corruption) <= 400 || TargetDebuffRemainingTime(SeedofCorruption) <= 1000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SeedofCorruption);
                    return;
                }
                //actions.aoe+=/seed_of_corruption,if=!talent.sow_the_seeds&!dot.seed_of_corruption.ticking&!in_flight&dot.corruption.refreshable
                if (!LastCastSeedOfCorruption && !TargetHasDebuff(SeedofCorruption) && !TargetHasDebuff(Corruption) && API.CanCast(SeedofCorruption) && IsRange && API.PlayerCurrentSoulShards >= 1 && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SeedofCorruption);
                    return;
                }
                //actions.aoe+=/agony,cycle_targets=1,if=active_dot.agony<4,target_if=!dot.agony.ticking
                if (IsMouseover && UseAG && !LastCastAgony && API.CanCast(Agony) && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange)
                {
                    API.CastSpell(Agony + "MO");
                    return;
                }
                //actions.aoe+=/agony,cycle_targets=1,if=active_dot.agony>=4,target_if=refreshable&dot.agony.ticking
                if (IsMouseover && UseAG && !LastCastAgony && API.CanCast(Agony) && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange)
                {
                    API.CastSpell(Agony + "MO");
                    return;
                }
                //actions.aoe+=/unstable_affliction,if=dot.unstable_affliction.refreshable
                if (API.CanCast(UnstableAffliction) && !LastCastUnstableAffliction && TargetDebuffRemainingTime(UnstableAffliction) < UaTime + 200)
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
                if (PlayerCovenantSettings != "Necrolord")
                {
                    //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&(cooldown.impending_catastrophe.ready|dot.impending_catastrophe_dot.ticking)&cooldown.summon_darkglare.ready&(dot.phantom_singularity.remains>2|!talent.phantom_singularity)
                if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && (API.CanCast(ImpendingCatastrophe) || TargetHasDebuff(ImpendingCatastrophe)) || API.CanCast(SummonDarkglare) && (TargetDebuffRemainingTime(PhantomSingularity) > 200 || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint
                    if (API.CanCast(VileTaint) && TalentVileTaint)
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                    {
                        API.CanCast(DarkSoulMisery);
                        return;
                    }
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
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                    if (PlayerCovenantSettings != "Necrolord")
                    {
                        //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity)
                if (IsCooldowns && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.SpellCDDuration(SummonDarkglare) < 200 && (TargetDebuffRemainingTime(PhantomSingularity) > 200 || !TalentPhantomSingularity))
                {
                    //actions.darkglare_prep=vile_taint
                    if (API.CanCast(VileTaint) && TalentVileTaint)
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                    {
                        API.CanCast(DarkSoulMisery);
                        return;
                    }
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
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                    if (PlayerCovenantSettings != "Necrolord")
                    {
                        //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
                //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&(cooldown.soul_rot.ready|dot.soul_rot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity)
                if (IsCooldowns && PlayerCovenantSettings == "Night Fae" && (API.CanCast(SoulRot) || TargetHasDebuff(SoulRot) && API.SpellCDDuration(SummonDarkglare) < 200 && (TargetDebuffRemainingTime(PhantomSingularity) > 200 || !TalentPhantomSingularity)))
                {
                    //actions.darkglare_prep=vile_taint
                    if (API.CanCast(VileTaint) && TalentVileTaint)
                    {
                        API.CastSpell(VileTaint);
                        return;
                    }
                    //actions.darkglare_prep+=/dark_soul
                    if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                    {
                        API.CanCast(DarkSoulMisery);
                        return;
                    }
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
                    //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                    if (PlayerCovenantSettings != "Necrolord")
                    {
                        //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(ImpendingCatastrophe);
                            return;
                        }
                        //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                        if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(DecimatingBolt);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                        {
                            API.CastSpell(SoulRot);
                            return;
                        }
                        //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                        if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
                //actions.aoe+=/dark_soul,if=cooldown.summon_darkglare.remains>time_to_die&(!talent.phantom_singularity|cooldown.phantom_singularity.remains>time_to_die)
                //actions.aoe+=/dark_soul,if=cooldown.summon_darkglare.remains+cooldown.summon_darkglare.duration<time_to_die
                //actions.aoe+=/call_action_list,name=item
                //actions.aoe+=/call_action_list,name=delayed_trinkets
                //actions.aoe+=/call_action_list,name=damage_trinkets
                //actions.aoe+=/call_action_list,name=stat_trinkets,if=dot.phantom_singularity.ticking|!talent.phantom_singularity
                //actions.aoe+=/malefic_rapture,if=dot.vile_taint.ticking
                if (API.CanCast(MaleficRapture) && TargetHasDebuff(VileTaint) && TalentVileTaint && !CurrentCastMaleficRapture)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                //actions.aoe+=/malefic_rapture,if=dot.soul_rot.ticking&!talent.sow_the_seeds
                if (API.CanCast(MaleficRapture) && TargetHasDebuff(SoulRot) && !CurrentCastMaleficRapture)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                //actions.aoe+=/malefic_rapture,if=!talent.vile_taint
                if (API.CanCast(MaleficRapture) && !TalentVileTaint && !CurrentCastMaleficRapture)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                //actions.aoe+=/malefic_rapture,if=soul_shard>4
                if (API.CanCast(MaleficRapture) && API.PlayerCurrentSoulShards >= 4 && !CurrentCastMaleficRapture)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
                //actions.aoe+=/siphon_life,cycle_targets=1,if=active_dot.siphon_life<=3,target_if=!dot.siphon_life.ticking
                if (IsMouseover && UseSL && !LastCastSiphonLife && API.CanCast(SiphonLife) && !API.MacroIsIgnored(SiphonLife + "MO") && API.PlayerCanAttackMouseover && TalentSiphonLife && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(SiphonLife) <= 400 && IsRange)
                {
                    API.CastSpell(SiphonLife + "MO");
                    return;
                }

                //actions.aoe+=/call_action_list,name=covenant
                //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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

            //# Action lists for trinket behavior. Stats are saved for before Soul Rot/Impending Catastrophe/Phantom Singularity, otherwise on cooldown
            //actions+=/call_action_list,name=trinket_split_check,if=time<1
            //actions+=/call_action_list,name=delayed_trinkets
            //actions+=/call_action_list,name=stat_trinkets,if=(dot.soul_rot.ticking|dot.impending_catastrophe_dot.ticking|dot.phantom_singularity.ticking)&soul_shard>3|dot.vile_taint.ticking|talent.sow_the_seeds
            //actions+=/call_action_list,name=damage_trinkets,if=covenant.night_fae&(!variable.trinket_split|cooldown.soul_rot.remains>20|(variable.trinket_one&cooldown.soul_rot.remains<trinket.1.cooldown.remains)|(variable.trinket_two&cooldown.soul_rot.remains<trinket.2.cooldown.remains))
            //actions+=/call_action_list,name=damage_trinkets,if=covenant.venthyr&(!variable.trinket_split|cooldown.impending_catastrophe.remains>20|(variable.trinket_one&cooldown.impending_catastrophe.remains<trinket.1.cooldown.remains)|(variable.trinket_two&cooldown.impending_catastrophe.remains<trinket.2.cooldown.remains))
            //actions+=/call_action_list,name=damage_trinkets,if=(covenant.necrolord|covenant.kyrian|covenant.none)&(!variable.trinket_split|cooldown.phantom_singularity.remains>20|(variable.trinket_one&cooldown.phantom_singularity.remains<trinket.1.cooldown.remains)|(variable.trinket_two&cooldown.phantom_singularity.remains<trinket.2.cooldown.remains))
            //actions+=/call_action_list,name=damage_trinkets,if=!talent.phantom_singularity.enabled&(!variable.trinket_split|cooldown.summon_darkglare.remains>20|(variable.trinket_one&cooldown.summon_darkglare.remains<trinket.1.cooldown.remains)|(variable.trinket_two&cooldown.summon_darkglare.remains<trinket.2.cooldown.remains))
            
            
            //# Burn soul shards if fight is almost over
            //actions+=/malefic_rapture,if=time_to_die<execute_time*soul_shard&dot.unstable_affliction.ticking
            if (API.CanCast(MaleficRapture) && !CurrentCastMaleficRapture && API.TargetTimeToDie < API.TargetTimeToExec * API.PlayerCurrentSoulShards && TargetHasDebuff(UnstableAffliction))
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //# If covenant dot/Phantom Singularity is running, use Darkglare to extend the current set
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&dot.impending_catastrophe_dot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity)
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&dot.soul_rot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity)
            //# If using Phantom Singularity on cooldown, make sure to extend it before it runs out
            //actions+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&dot.phantom_singularity.ticking&dot.phantom_singularity.remains<2
            //# Refresh dots early if going into a shard spending phase
            //actions+=/call_action_list,name=dot_prep,if=covenant.night_fae&!dot.soul_rot.ticking&cooldown.soul_rot.remains<4
            if (PlayerCovenantSettings == "Night Fae" && !TargetHasDebuff(SoulRot) && API.SpellCDDuration(SoulRot) < 400)
            {
                //actions.dot_prep=agony,if=dot.agony.remains<8&cooldown.summon_darkglare.remains>dot.agony.remains
                if (API.CanCast(Agony) && TargetDebuffRemainingTime(Agony) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(Agony))
                {
                    API.CastSpell(Agony);
                    return;
                }
                //actions.dot_prep+=/siphon_life,if=dot.siphon_life.remains<8&cooldown.summon_darkglare.remains>dot.siphon_life.remains
                if (API.CanCast(SiphonLife) && TalentSiphonLife && TargetDebuffRemainingTime(SiphonLife) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(SiphonLife))
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
                //actions.dot_prep+=/unstable_affliction,if=dot.unstable_affliction.remains<8&cooldown.summon_darkglare.remains>dot.unstable_affliction.remains
                if (API.CanCast(UnstableAffliction) && !LastCastUnstableAffliction && TargetDebuffRemainingTime(UnstableAffliction) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(UnstableAffliction))
                {
                    API.CastSpell(UnstableAffliction);
                    return;
                }
                //actions.dot_prep+=/corruption,if=dot.corruption.remains<8&cooldown.summon_darkglare.remains>dot.corruption.remain
                if (API.CanCast(Corruption) && TargetDebuffRemainingTime(Corruption) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(Corruption))
                {
                    API.CastSpell(Corruption);
                    return;
                }
            }
            //actions+=/call_action_list,name=dot_prep,if=covenant.venthyr&!dot.impending_catastrophe_dot.ticking&cooldown.impending_catastrophe.remains<4
            if (PlayerCovenantSettings == "Venthyr" && !TargetHasDebuff(ImpendingCatastrophe) && API.SpellCDDuration(ImpendingCatastrophe) < 400)
            {
                //actions.dot_prep=agony,if=dot.agony.remains<8&cooldown.summon_darkglare.remains>dot.agony.remains
                if (API.CanCast(Agony) && TargetDebuffRemainingTime(Agony) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(Agony))
                {
                    API.CastSpell(Agony);
                    return;
                }
                //actions.dot_prep+=/siphon_life,if=dot.siphon_life.remains<8&cooldown.summon_darkglare.remains>dot.siphon_life.remains
                if (API.CanCast(SiphonLife) && TalentSiphonLife && TargetDebuffRemainingTime(SiphonLife) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(SiphonLife))
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
                //actions.dot_prep+=/unstable_affliction,if=dot.unstable_affliction.remains<8&cooldown.summon_darkglare.remains>dot.unstable_affliction.remains
                if (API.CanCast(UnstableAffliction) && !LastCastUnstableAffliction && TargetDebuffRemainingTime(UnstableAffliction) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(UnstableAffliction))
                {
                    API.CastSpell(UnstableAffliction);
                    return;
                }
                //actions.dot_prep+=/corruption,if=dot.corruption.remains<8&cooldown.summon_darkglare.remains>dot.corruption.remain
                if (API.CanCast(Corruption) && TargetDebuffRemainingTime(Corruption) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(Corruption))
                {
                    API.CastSpell(Corruption);
                    return;
                }
            }
            //actions+=/call_action_list,name=dot_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&talent.phantom_singularity&!dot.phantom_singularity.ticking&cooldown.phantom_singularity.remains<4
            if ((PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && TalentPhantomSingularity && !TargetHasDebuff(PhantomSingularity) && API.SpellCDDuration(PhantomSingularity) < 400)
            {
                //actions.dot_prep=agony,if=dot.agony.remains<8&cooldown.summon_darkglare.remains>dot.agony.remains
                if (API.CanCast(Agony) && TargetDebuffRemainingTime(Agony) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(Agony))
                {
                    API.CastSpell(Agony);
                    return;
                }
                //actions.dot_prep+=/siphon_life,if=dot.siphon_life.remains<8&cooldown.summon_darkglare.remains>dot.siphon_life.remains
                if (API.CanCast(SiphonLife) && TalentSiphonLife && TargetDebuffRemainingTime(SiphonLife) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(SiphonLife))
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
                //actions.dot_prep+=/unstable_affliction,if=dot.unstable_affliction.remains<8&cooldown.summon_darkglare.remains>dot.unstable_affliction.remains
                if (API.CanCast(UnstableAffliction) && !LastCastUnstableAffliction && TargetDebuffRemainingTime(UnstableAffliction) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(UnstableAffliction))
                {
                    API.CastSpell(UnstableAffliction);
                    return;
                }
                //actions.dot_prep+=/corruption,if=dot.corruption.remains<8&cooldown.summon_darkglare.remains>dot.corruption.remain
                if (API.CanCast(Corruption) && TargetDebuffRemainingTime(Corruption) < 800 && API.SpellCDDuration(SummonDarkglare) > TargetDebuffRemainingTime(Corruption))
                {
                    API.CastSpell(Corruption);
                    return;
                }
            }
            //# If Phantom Singularity is ticking, it is safe to use Dark Soul
            //actions+=/dark_soul,if=dot.phantom_singularity.ticking
            if (DsM && API.CanCast(DarkSoulMisery) && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity))
            {
                API.CastSpell(DarkSoulMisery);
                return;
            }
            //actions+=/dark_soul,if=!talent.phantom_singularity&(dot.soul_rot.ticking|dot.impending_catastrophe_dot.ticking)
            if (DsM && API.CanCast(DarkSoulMisery) && !TalentPhantomSingularity && (TargetHasDebuff(SoulRot) || TargetHasDebuff(ImpendingCatastrophe)))
            {
                API.CastSpell(DarkSoulMisery);
                return;
            }
            //# Sync Phantom Singularity with Venthyr/Night Fae covenant dot, otherwise use on cooldown. If Empyreal Ordnance buff is incoming, hold until it's ready (18 seconds after use)
            //actions+=/phantom_singularity,if=covenant.night_fae&time>5&cooldown.soul_rot.remains<1&(trinket.empyreal_ordnance.cooldown.remains<162|!equipped.empyreal_ordnance)
            if (API.CanCast(PhantomSingularity) && TalentPhantomSingularity && PlayerCovenantSettings == "Night Fae" && API.PlayerTimeInCombat > 500 && API.SpellCDDuration(SoulRot) < 100) 
            {
                API.CastSpell(PhantomSingularity);
                return;
            }
            //actions+=/phantom_singularity,if=covenant.venthyr&time>5&cooldown.impending_catastrophe.remains<1&(trinket.empyreal_ordnance.cooldown.remains<162|!equipped.empyreal_ordnance)
            if (API.CanCast(PhantomSingularity) && TalentPhantomSingularity && PlayerCovenantSettings == "Venthyr" && API.PlayerTimeInCombat > 500 && API.SpellCDDuration(ImpendingCatastrophe) < 100)
            {
                API.CastSpell(PhantomSingularity);
                return;
            }
            //actions+=/phantom_singularity,if=(covenant.necrolord|covenant.kyrian|covenant.none)&(trinket.empyreal_ordnance.cooldown.remains<162|!equipped.empyreal_ordnance)
            if (API.CanCast(PhantomSingularity) && TalentPhantomSingularity && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None"))
            {
                API.CastSpell(PhantomSingularity);
                return;
            }
            //actions+=/phantom_singularity,if=time_to_die<16
            if (API.CanCast(PhantomSingularity) && TalentPhantomSingularity && API.TargetTimeToDie < 1600)
            {
                API.CastSpell(PhantomSingularity);
                return;
            }
            //# If Phantom Singularity is ticking, it's time to use other major dots
            //actions+=/call_action_list,name=covenant,if=dot.phantom_singularity.ticking&(covenant.night_fae|covenant.venthyr)
            if (TargetHasDebuff(PhantomSingularity) && (PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Venthyr"))
            {
                //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
            //actions+=/agony,cycle_targets=1,target_if=dot.agony.remains<4
            if (API.CanCast(Agony) && TargetDebuffRemainingTime(Agony) < 400)
            {
                API.CastSpell(Agony);
                return;
            }
            if (IsMouseover && UseAG && !LastCastAgony && API.CanCast(Agony) && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange)
            {
                API.CastSpell(Agony + "MO");
                return;
            }
            //actions+=/haunt
            if (API.CanCast(Haunt) && TalentHaunt && !API.PlayerIsCasting(true))
            {
                API.CastSpell(Haunt);
                return;
            }
            //# Sow the Seeds on 3 targets if it isn't currently in flight or on the target. With Siphon Life it's also better to use Seed over manually applying 3 Corruptions.
            //actions+=/seed_of_corruption,if=active_enemies>2&talent.sow_the_seeds&!dot.seed_of_corruption.ticking&!in_flight
            if (API.CanCast(SeedofCorruption) && !LastCastSeedOfCorruption && API.TargetUnitInRangeCount > 2 && TalentSowTheSeeds && !TargetHasDebuff(Corruption) && !API.PlayerIsCasting(true))
            {
                API.CastSpell(SeedofCorruption);
                return;
            }
            //actions+=/seed_of_corruption,if=active_enemies>2&talent.siphon_life&!dot.seed_of_corruption.ticking&!in_flight&dot.corruption.remains<4
            if (API.CanCast(SeedofCorruption) && !LastCastSeedOfCorruption && API.TargetUnitInRangeCount > 2 && TalentSiphonLife && !TargetHasDebuff(SeedofCorruption) && TargetDebuffRemainingTime(Corruption) < 400 && !API.PlayerIsCasting(true))
            {
                API.CastSpell(SeedofCorruption);
                return;
            }
            //actions+=/vile_taint,if=(soul_shard>1|active_enemies>2)&cooldown.summon_darkglare.remains>12
            if (API.CanCast(VileTaint) && TalentVileTaint && (API.PlayerCurrentSoulShards > 1 || API.TargetUnitInRangeCount > 2) && API.SpellCDDuration(SummonDarkglare) > 1200 && !API.PlayerIsCasting(true))
            {
                API.CastSpell(VileTaint);
                return;
            }
            //actions+=/unstable_affliction,if=dot.unstable_affliction.remains<4
            if (API.CanCast(UnstableAffliction) && !LastCastUnstableAffliction && TargetDebuffRemainingTime(UnstableAffliction) < 400)
            {
                API.CastSpell(UnstableAffliction);
                return;
            }
            //actions+=/siphon_life,cycle_targets=1,target_if=dot.siphon_life.remains<4
            if (API.CanCast(SiphonLife) && TalentSiphonLife && TargetDebuffRemainingTime(SiphonLife) < 400)
            {
                API.CastSpell(SiphonLife);
                return;
            }
            if (IsMouseover && UseSL && !LastCastSiphonLife && API.CanCast(SiphonLife) && !API.MacroIsIgnored(SiphonLife + "MO") && API.PlayerCanAttackMouseover && TalentSiphonLife && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(SiphonLife) <= 400 && IsRange)
            {
                API.CastSpell(SiphonLife + "MO");
                return;
            }
            //# If not using Phantom Singularity, don't apply covenant dots until other core dots are safe
            //actions+=/call_action_list,name=covenant,if=!covenant.necrolord
            if (PlayerCovenantSettings != "Necrolord")
            {
                //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
            //# Apply Corruption manually on 1-2 targets, or on 3 with Absolute Corruption
            //actions+=/corruption,cycle_targets=1,if=active_enemies<4-(talent.sow_the_seeds|talent.siphon_life),target_if=dot.corruption.remains<2
            if (API.CanCast(Corruption) && TargetDebuffRemainingTime(Corruption) < 400)
            {
                API.CastSpell(Corruption);
                return;
            }
            if (IsMouseover && UseCO && !LastCastCorruption && API.CanCast(Corruption) && !API.MacroIsIgnored(Corruption + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(Corruption) <= 400 && !TargetHasDebuff(SeedofCorruption) && IsRange)
            {
                API.CastSpell(Corruption + "MO");
                return;
            }
            //# After the opener, spend a shard when at 5 on Malefic Rapture to avoid overcapping
            //actions+=/malefic_rapture,if=soul_shard>4&time>21
            if (API.CanCast(MaleficRapture) && !CurrentCastMaleficRapture && API.PlayerCurrentSoulShards > 4 && API.PlayerTimeInCombat > 2100)
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //# When not syncing Phantom Singularity to Venthyr/Night Fae, Summon Darkglare if all dots are applied
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&!talent.phantom_singularity&dot.impending_catastrophe_dot.ticking&cooldown.summon_darkglare.ready
            if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && TargetHasDebuff(ImpendingCatastrophe) && API.CanCast(SummonDarkglare))
            {
                //actions.darkglare_prep=vile_taint
                if (API.CanCast(VileTaint) && TalentVileTaint)
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                {
                    API.CanCast(DarkSoulMisery);
                    return;
                }
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
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                if (PlayerCovenantSettings != "Necrolord")
                {
                    //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&!talent.phantom_singularity&dot.soul_rot.ticking&cooldown.summon_darkglare.ready
            if (IsCooldowns && PlayerCovenantSettings == "Night Fae" && TargetHasDebuff(SoulRot) && API.CanCast(SummonDarkglare))
            {
                //actions.darkglare_prep=vile_taint
                if (API.CanCast(VileTaint) && TalentVileTaint)
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                {
                    API.CanCast(DarkSoulMisery);
                    return;
                }
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
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                if (PlayerCovenantSettings != "Necrolord")
                {
                    //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
            //actions+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&cooldown.summon_darkglare.ready
            if (IsCooldowns && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.CanCast(SummonDarkglare))
            {
                //actions.darkglare_prep=vile_taint
                if (API.CanCast(VileTaint) && TalentVileTaint)
                {
                    API.CastSpell(VileTaint);
                    return;
                }
                //actions.darkglare_prep+=/dark_soul
                if (DsM && API.CanCast(DarkSoulMisery) && TalentDarkSoulMisery)
                {
                    API.CanCast(DarkSoulMisery);
                    return;
                }
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
                //actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord
                if (PlayerCovenantSettings != "Necrolord")
                {
                    //actions.covenant=impending_catastrophe,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50&cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) > 2500) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/impending_catastrophe,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                    if (!API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt)
                    if (!API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) > 500 && (TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=!talent.phantom_singularity&(cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer)
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && !TalentPhantomSingularity && (API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000) && !API.PlayerIsCasting(true))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/soul_rot,if=talent.phantom_singularity&dot.phantom_singularity.ticking
                    if (!API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && TalentPhantomSingularity && TargetHasDebuff(PhantomSingularity) && !API.PlayerIsCasting(true))
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
            //# Use Dark Soul if Darkglare won't be ready again, or if there will be at least 2 more Darkglare uses
            //actions+=/dark_soul,if=cooldown.summon_darkglare.remains>time_to_die&(!talent.phantom_singularity|cooldown.phantom_singularity.remains>time_to_die)
            if (DsM && API.CanCast(DarkSoulMisery) && API.SpellCDDuration(SummonDarkglare) > API.TargetTimeToDie && (!TalentPhantomSingularity || API.SpellCDDuration(PhantomSingularity) > API.TargetTimeToDie))
            {
                API.CastSpell(DarkSoulMisery);
                return;
            }
            //actions+=/dark_soul,if=!talent.phantom_singularity&cooldown.summon_darkglare.remains+cooldown.summon_darkglare.duration<time_to_die
            //# Catch-all item usage for anything not specified elsewhere
            //actions+=/call_action_list,name=item
            //# Refresh Shadow Embrace before spending shards on Malefic Rapture
            //actions+=/call_action_list,name=se,if=debuff.shadow_embrace.stack<(2-action.shadow_bolt.in_flight)|debuff.shadow_embrace.remains<3
            if (API.TargetDebuffStacks(ShadowEmbrace) < 2 || TargetDebuffRemainingTime(ShadowEmbrace) < 300)
            {
                //actions.se=haunt
                if (API.CanCast(Haunt) && TalentHaunt)
                {
                    API.CastSpell(Haunt);
                    return;
                }
                //actions.se+=/drain_soul,interrupt_global=1,interrupt_if=debuff.shadow_embrace.stack>=3
                if (API.CanCast(DrainSoul) && TalentDrainSoul && NotChanneling)
                {
                    API.CastSpell(DrainSoul);
                    return;
                }
                //actions.se+=/shadow_bolt
                if (API.CanCast(ShadowBolt) && !TalentDrainSoul && NotChanneling)
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
            }
            //# Use Malefic Rapture when major dots are up, or if there will be significant time until the next Phantom Singularity
            //actions+=/malefic_rapture,if=dot.vile_taint.ticking|dot.impending_catastrophe_dot.ticking|dot.soul_rot.ticking
            if (API.CanCast(MaleficRapture) && !CurrentCastMaleficRapture && (TargetHasDebuff(VileTaint) || TargetHasDebuff(ImpendingCatastrophe) || TargetHasDebuff(SoulRot)))
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //actions+=/malefic_rapture,if=talent.phantom_singularity&(dot.phantom_singularity.ticking|cooldown.phantom_singularity.remains>25|time_to_die<cooldown.phantom_singularity.remains)
            if (API.CanCast(MaleficRapture) && !CurrentCastMaleficRapture && TalentPhantomSingularity && (TargetHasDebuff(PhantomSingularity) || API.SpellCDDuration(PhantomSingularity) > 2500 || API.TargetTimeToDie < API.SpellCDDuration(PhantomSingularity)))
            {
                API.CastSpell(MaleficRapture);
                return;
            }
            //actions+=/malefic_rapture,if=talent.sow_the_seeds
            if (API.CanCast(MaleficRapture) && TalentSowTheSeeds && !CurrentCastMaleficRapture)
            {
                API.CastSpell(MaleficRapture);
                return;
            }

            //actions+=/drain_life,if=buff.inevitable_demise.stack>40|buff.inevitable_demise.up&time_to_die<4
            if (API.CanCast(DrainLife) && API.PlayerBuffStacks(InevitableDemise) >40 || API.PlayerHasBuff(InevitableDemise) && API.TargetTimeToDie < 400)
            {
                API.CastSpell(DrainLife);
                return;
            }
            //actions+=/call_action_list,name=covenant
            //actions+=/agony,cycle_targets=1,target_if=refreshable
            if (API.CanCast(Agony) && TargetDebuffRemainingTime(Agony) < 400)
            {
                API.CastSpell(Agony);
                return;
            }
            if (IsMouseover && UseAG && !LastCastAgony && API.CanCast(Agony) && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange)
            {
                API.CastSpell(Agony + "MO");
                return;
            }
            //actions+=/unstable_affliction,if=refreshable
            if (API.CanCast(UnstableAffliction) && !LastCastUnstableAffliction && TargetDebuffRemainingTime(UnstableAffliction) < UaTime + 200)
            {
                API.CastSpell(UnstableAffliction);
            }
            //actions+=/siphon_life,cycle_targets=1,target_if=refreshable
            if (API.CanCast(SiphonLife) && TalentSiphonLife && TargetDebuffRemainingTime(SiphonLife) < 400)
            {
                API.CastSpell(SiphonLife);
                return;
            }
            if (IsMouseover && UseSL && !LastCastSiphonLife && API.CanCast(SiphonLife) && !API.MacroIsIgnored(SiphonLife + "MO") && API.PlayerCanAttackMouseover && TalentSiphonLife && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(SiphonLife) <= 400 && IsRange)
            {
                API.CastSpell(SiphonLife + "MO");
                return;
            }
            //actions+=/corruption,cycle_targets=1,if=active_enemies<4-(talent.sow_the_seeds|talent.siphon_life),target_if=refreshable
            if (API.CanCast(Corruption) && TargetDebuffRemainingTime(Corruption) < 400)
            {
                API.CastSpell(Corruption);
                return;
            }
            if (IsMouseover && UseCO && !LastCastCorruption && API.CanCast(Corruption) && !API.MacroIsIgnored(Corruption + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && MouseoverDebuffRemainingTime(Corruption) <= 400 && !TargetHasDebuff(SeedofCorruption) && IsRange)
            {
                API.CastSpell(Corruption + "MO");
                return;
            }
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