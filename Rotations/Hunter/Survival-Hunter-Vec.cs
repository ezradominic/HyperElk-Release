using System.Diagnostics;
using System.Linq;

namespace HyperElk.Core
{
    public class BMHunter : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool SmallCDs => API.ToggleIsEnabled("Small CDs");
        private bool MultiDot => API.ToggleIsEnabled("Multidot");
        //stopwatch
        private readonly Stopwatch pullwatch = new Stopwatch();
        private readonly Stopwatch CallPetTimer = new Stopwatch();

        //Spells,Buffs,Debuffs
        private string SerpentSting = "Serpent Sting";
        private string AspectOfTheEagle = "Aspect of the Eagle";
        private string CoordinatedAssault = "Coordinated Assault";
        private string Kill_Command = "Kill Command";
        private string WildfireBomb = "Wildfire Bomb";
        private string RaptorStrike = "Raptor Strike";
        private string Carve = "Carve";

        private string Steady_Shot = "Steady Shot";
        private string Arcane_Shot = "Arcane Shot";
        private string Mend_Pet = "Mend Pet";
        private string Kill_Shot = "Kill Shot";
        private string Misdirection = "Misdirection";
        private string Exhilaration = "Exhilaration";
        private string Feign_Death = "Feign Death";
        private string Muzzle = "Muzzle";
        private string A_Murder_of_Crows = "A Murder of Crows";
        private string Aspect_of_the_Turtle = "Aspect of the Turtle";
        private string Revive_Pet = "Revive Pet";

        private string Wild_Spirits = "Wild Spirits";
        private string Resonating_Arrow = "Resonating Arrow";
        private string Flayed_Shot = "Flayed Shot";
        private string Death_Chakram = "Death Chakram";
        private string FlayersMark = "Flayer's Mark";
        private string TipOfTheSpear = "Tip of the Spear";
        private string ShrapnelBomb = "Shrapnel Bomb";
        private string internal_bleeding = "Internal Bleeding";
        private string MongooseFury = "Mongoose Fury";
        private string MongooseBite = "Mongoose Bite";
        private string FlankingStrike = "Flanking Strike";
        private string PheromoneBomb = "Pheromone Bomb";
        private string VolatileBomb = "Volatile Bomb";
        private string Chakrams = "Chakrams";
        private string SteelTrap = "Steel Trap";
        private string vipers_venom = "Viper's Venom";
        private string Butchery = "Butchery";
        private string Predator = "Predator";
        private string latent_poison_injection = "Latent Poison Injection";
        private string Tab = "Tab";
        private string KillCommandDebuff = "259277";

        //Misc
        private int Level => API.PlayerLevel;
        private int Focus => API.PlayerFocus;
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool MeleeRange => API.TargetRange <= 5;
        private bool InRange => API.TargetRange <= 40;
        private bool CanCastRaptorStrike => API.CanCast(RaptorStrike) && Focus >= 30 && Level >= 10 && (MeleeRange || PlayerHasBuff(AspectOfTheEagle) && API.TargetRange <=40);
        private bool CanCastSerpentSting => API.CanCast(SerpentSting) && Focus >= 20 && Level >= 12 && API.TargetRange <= 40;
        private bool CanCastKillCommand => API.CanCast(Kill_Command) && !Talent_FlankingStrike && Level >= 11 && API.TargetRange <= 50;
        private bool CanCastWildfireBomb => API.CanCast(WildfireBomb) && Level >= 20 && API.TargetRange <= 40;
        private bool CanCastCarve => API.CanCast(Carve) && Focus >= 35 && Level >= 23 && API.TargetRange <= 8;
        private bool CanCastAspectOfTheEagle => API.CanCast(AspectOfTheEagle) && Level >= 23 && API.TargetRange <= 40;
        private bool CanCastCoordinatedAssault => API.CanCast(CoordinatedAssault) && Level >= 34 && (API.TargetRange <= 5 || API.TargetRange <= 40 && PlayerHasBuff(AspectOfTheEagle));
        private bool CanCastKillShot => API.CanCast(Kill_Shot) && Level >= 42 && API.TargetRange <= 40;
        private bool next_wi_bomb(string spellname) => API.SpellIsCanbeCast(spellname);
        private bool wildfirebomb_ticking => TargetHasDebuff(WildfireBomb) || TargetHasDebuff(ShrapnelBomb) || TargetHasDebuff(VolatileBomb) || TargetHasDebuff(PheromoneBomb);

        private float gcd => API.SpellGCDTotalDuration;
        // private float BarbedShotCount => (API.PlayerHasBuff("246152", false, false) ? 1 : 0) + (API.PlayerHasBuff("246851", false, false) ? 1 : 0) + (API.PlayerHasBuff("217200", false, false) ? 1 : 0);
        private float FocusRegen => 10f * (1f + API.PlayerGetHaste / 100f);
        // private float RealFocusRegen => FocusRegen + BarbedShotCount * 2.5f;
        private float FocusTimeToMax => (API.PlayerMaxFocus - API.PlayerFocus) * 100f / FocusRegen;
        private float Barrage_ExecuteTime => 300f / (1f + (API.PlayerGetHaste / 100f));
        private float WildfireBombCooldown => 1800f / (1f + (API.PlayerGetHaste / 100f));
        private float KillCommandCooldown => 600f / (1f + (API.PlayerGetHaste / 100f));
        private float ButcheryCooldown => 900f / (1f + (API.PlayerGetHaste / 100f));
        private float WildfireBombFractional => (API.SpellCharges(WildfireBomb) * 100 + ((WildfireBombCooldown - API.SpellChargeCD(WildfireBomb)) / (WildfireBombCooldown / 100)));
        private float KillCommandFactional => (API.SpellCharges(Kill_Command) * 100 + ((KillCommandCooldown - API.SpellChargeCD(Kill_Command)) / (KillCommandCooldown / 100)));
        private float WildfireBomb_FullRechargeTime => (API.SpellMaxCharges(WildfireBomb) - API.SpellCharges(WildfireBomb)) * WildfireBombCooldown + API.SpellCDDuration(WildfireBomb);
        private float ButcheryFactional => (API.SpellCharges(Butchery) * 100 + ((ButcheryCooldown - API.SpellChargeCD(Butchery)) / (ButcheryCooldown / 100)));
        //Talents
        private bool Talent_ViperVenom => API.PlayerIsTalentSelected(1, 1);
        private bool Talent_AlphaPredator => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_GuerrillaTactics => API.PlayerIsTalentSelected(2, 1);
        private bool Talent_HydrasBite => API.PlayerIsTalentSelected(2, 2);
        private bool Talent_Butchery => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_NaturalMending => API.PlayerIsTalentSelected(3, 2);
        private bool Talent_Predator => API.PlayerIsTalentSelected(4, 1);
        private bool Talent_SteelTrap => API.PlayerIsTalentSelected(4, 2);
        private bool Talent_AMurderofCrows => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_TipOfTheSpear => API.PlayerIsTalentSelected(6, 1);
        private bool Talent_MongooseBite => API.PlayerIsTalentSelected(6, 2);
        private bool Talent_FlankingStrike => API.PlayerIsTalentSelected(6, 3);
        private bool Talent_BirdsOfPrey => API.PlayerIsTalentSelected(7, 1);
        private bool Talent_WildfireInfusion => API.PlayerIsTalentSelected(7, 2);
        private bool Talent_Chakrams => API.PlayerIsTalentSelected(7, 3);

        private static void CastSpell(string spell)
        {
            if (API.CanCast(spell))
            {
                API.CastSpell(spell);
                return;
            }
        }

        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool PetHasBuff(string buff)
        {
            return API.PetHasBuff(buff, false, false);
        }

        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        //CBProperties


        string[] MisdirectionList = new string[] { "Off", "On AOE", "On" };
        string[] combatList = new string[] { "In Combat", "Out Of Combat", "Everytime" };


        private int ExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration)];
        private int PetExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration + "PET")];
        private int AspectoftheTurtleLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Aspect_of_the_Turtle)];
        private int FeignDeathLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feign_Death)];
        private int MendPetLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Mend_Pet)];
        private string UseMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private string UseAMurderofCrows => CDUsageWithAOE[CombatRoutine.GetPropertyInt(A_Murder_of_Crows)];
        private string UseCoordinatedAssault => CDUsage[CombatRoutine.GetPropertyInt(CoordinatedAssault)];
        private string UseAspectoftheEagle => CDUsage[CombatRoutine.GetPropertyInt(AspectOfTheEagle)];
        private bool UseCallPet => CombatRoutine.GetPropertyBool("CallPet");
        private bool nessingwarys_trapping_apparatus_enabled => CombatRoutine.GetPropertyBool("nessingwarys_trapping_apparatus"); 
        private bool rylakstalkers_confounding_strikes_enabled => CombatRoutine.GetPropertyBool("rylakstalkers_confounding_strikes");
        private bool latent_poison_injectors_enabled => CombatRoutine.GetPropertyBool("latent_poison_injectors");
        private string UseRevivePet => combatList[CombatRoutine.GetPropertyInt(Revive_Pet)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];

        public override void Initialize()
        {
            CombatRoutine.Name = "Survival Hunter by Vec";
            API.WriteLog("Welcome to Survival Hunter Rotation");
            API.WriteLog("Misdirection Macro : /cast [@focus,help][help][@pet,exists] Misdirection");
            API.WriteLog("Kill Shot Mouseover - /cast [@mouseover] Kill Shot");
            API.WriteLog("Multidot with Kill Command");

            //Spells
            CombatRoutine.AddSpell(Steady_Shot, "D1");
            CombatRoutine.AddSpell(Arcane_Shot, "D2");
            CombatRoutine.AddSpell(Kill_Command, "D2");
            CombatRoutine.AddSpell(Mend_Pet, "X");
            CombatRoutine.AddSpell(Revive_Pet, "X");
            CombatRoutine.AddSpell("Call Pet", "F1");
            CombatRoutine.AddSpell(Kill_Shot, "D6");
            CombatRoutine.AddSpell(Carve, "F11");
            CombatRoutine.AddSpell(CoordinatedAssault, "F5");
            CombatRoutine.AddSpell(AspectOfTheEagle, "F4");
            CombatRoutine.AddSpell(WildfireBomb, "D3");
            CombatRoutine.AddSpell(VolatileBomb, "D3");
            CombatRoutine.AddSpell(PheromoneBomb, "D3");
            CombatRoutine.AddSpell(ShrapnelBomb, "D3");
            CombatRoutine.AddSpell(SerpentSting, "D1");
            CombatRoutine.AddSpell(RaptorStrike, "D4");
            CombatRoutine.AddSpell(MongooseBite, "D4");
            CombatRoutine.AddSpell(FlankingStrike, "D5");
            CombatRoutine.AddSpell(Chakrams, "F9");
            CombatRoutine.AddSpell(Butchery, "F11");

            CombatRoutine.AddSpell(Muzzle, "F10");
            CombatRoutine.AddSpell(Exhilaration, "NumPad9");
            CombatRoutine.AddSpell(Misdirection, "Q");

            CombatRoutine.AddSpell(A_Murder_of_Crows, "F");
            CombatRoutine.AddSpell(SteelTrap, "F");
            CombatRoutine.AddSpell(Feign_Death, "F2");
            CombatRoutine.AddSpell(Aspect_of_the_Turtle, "G");

            CombatRoutine.AddSpell(Wild_Spirits, "F10");
            CombatRoutine.AddSpell(Resonating_Arrow, "F10");
            CombatRoutine.AddSpell(Flayed_Shot, "F10");
            CombatRoutine.AddSpell(Death_Chakram, "F10");

            //Macros
            CombatRoutine.AddMacro(Kill_Shot + "MO", "NumPad7");
            CombatRoutine.AddMacro(Tab, "Tab");


            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            //Buffs


            CombatRoutine.AddBuff(Aspect_of_the_Turtle);
            CombatRoutine.AddBuff(Misdirection);
            CombatRoutine.AddBuff(Feign_Death);
            CombatRoutine.AddBuff(FlayersMark);
            CombatRoutine.AddBuff(AspectOfTheEagle);
            CombatRoutine.AddBuff(CoordinatedAssault);
            CombatRoutine.AddBuff(TipOfTheSpear);
            CombatRoutine.AddBuff(MongooseFury);
            CombatRoutine.AddBuff(Predator);
            //Debuffs
            CombatRoutine.AddDebuff(SerpentSting);
            CombatRoutine.AddDebuff(ShrapnelBomb);
            CombatRoutine.AddDebuff(internal_bleeding);
            CombatRoutine.AddDebuff(vipers_venom);
            CombatRoutine.AddDebuff(WildfireBomb);
            CombatRoutine.AddDebuff(VolatileBomb);
            CombatRoutine.AddDebuff(PheromoneBomb);
            CombatRoutine.AddDebuff(latent_poison_injection);
            CombatRoutine.AddDebuff(Kill_Command);
            //Toggle
            CombatRoutine.AddToggle("Small CDs");
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Multidot");

            AddProp("MouseoverInCombat", "Only Mouseover in combat", true, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");

            //Settings
            CombatRoutine.AddProp("CallPet", "Call/Ressurect Pet", true, "Should the rotation try to ressurect/call your Pet", "Pet");
            CombatRoutine.AddProp("nessingwarys_trapping_apparatus", "Nessingwarys Trapping Apparatus", false, "Enable if you have this Runeforge", "Legendary");
            CombatRoutine.AddProp("rylakstalkers_confounding_strikes", "Rylakstalkers Confounding Strikes", false, "Enable if you have this Runeforge", "Legendary");
            CombatRoutine.AddProp("latent_poison_injectors", "Latent Poison Injectors", false, "Enable if you have this Runeforge", "Legendary");


            CombatRoutine.AddProp(Misdirection, "Use Misdirection", MisdirectionList, "Use " + Misdirection + "Off, On AOE, On", "Generic", 0);
            CombatRoutine.AddProp(AspectOfTheEagle, "Use " + AspectOfTheEagle, CDUsage, "Use " + AspectOfTheEagle + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(CoordinatedAssault, "Use " + CoordinatedAssault, CDUsage, "Use " + CoordinatedAssault + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(A_Murder_of_Crows, "Use " + A_Murder_of_Crows, CDUsageWithAOE, "Use " + A_Murder_of_Crows + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Revive_Pet, "Use " + Revive_Pet, combatList, "Use " + "Revive/Call Pet" + "In Combat, Out Of Combat, Everytime", "Pet", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);

            CombatRoutine.AddProp(Exhilaration, "Use " + Exhilaration + " below:", percentListProp, "Life percent at which " + Exhilaration + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Exhilaration + "PET", "Use " + Exhilaration + " below:", percentListProp, "Life percent at which " + Exhilaration + " is used to heal your pet, set to 0 to disable", "Pet", 2);
            CombatRoutine.AddProp(Aspect_of_the_Turtle, "Use " + Aspect_of_the_Turtle + " below:", percentListProp, "Life percent at which " + Aspect_of_the_Turtle + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Feign_Death, "Use " + Feign_Death + " below:", percentListProp, "Life percent at which " + Feign_Death + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(Mend_Pet, "Use " + Mend_Pet + " below:", percentListProp, "Life percent at which " + Mend_Pet + " is used, set to 0 to disable", "Pet", 6);
        }

        public override void Pulse()
        {
            //API.WriteLog("debug" + MultiDot + API.CanCast(Kill_Command) + API.TargetHasDebuff(Kill_Command, false, false)+ " " + API.PlayerUnitInMeleeRangeCount  +" "+ API.PlayerBuffStacks(Predator));
            if (CallPetTimer.ElapsedMilliseconds > 10000)
            {
                CallPetTimer.Stop();
                CallPetTimer.Reset();
            }
            if (!API.PlayerIsMounted && !API.PlayerIsCasting && !API.PlayerIsChanneling && !PlayerHasBuff(Aspect_of_the_Turtle) && !PlayerHasBuff(Feign_Death))
            {
                if ((!API.PlayerHasPet || API.PetHealthPercent < 1) && CallPetTimer.ElapsedMilliseconds > gcd * 20 && UseCallPet && ((API.PlayerIsInCombat && UseRevivePet == "In Combat") || (!API.PlayerIsInCombat && UseRevivePet == "Out Of Combat") || UseRevivePet == "Everytime")
                      && API.CanCast(Revive_Pet))
                {
                    API.CastSpell(Revive_Pet);
                    return;
                }

                if ((!API.PlayerHasPet || API.PetHealthPercent < 1) && (CallPetTimer.ElapsedMilliseconds <= gcd * 20 || !CallPetTimer.IsRunning) && UseCallPet)
                {
                    API.CastSpell("Call Pet");
                    CallPetTimer.Start();
                    return;
                }
                if (API.CanCast(Mend_Pet) && API.PlayerHasPet && API.PetHealthPercent <= MendPetLifePercent && API.PetHealthPercent >= 1)
                {
                    API.CastSpell(Mend_Pet);
                    return;
                }
                if (API.CanCast(Exhilaration) && ((API.PlayerHealthPercent <= ExhilarationLifePercent && Level >= 9) || (API.PetHealthPercent <= PetExhilarationLifePercent && API.PetHealthPercent >= 1 && Level >= 44)))
                {
                    API.CastSpell(Exhilaration);
                    return;
                }
                if (API.CanCast(Aspect_of_the_Turtle) && API.PlayerHealthPercent <= AspectoftheTurtleLifePercent && Level >= 8)
                {
                    API.CastSpell(Aspect_of_the_Turtle);
                    return;
                }
                if (API.CanCast(Feign_Death) && API.PlayerHealthPercent <= FeignDeathLifePercent && Level >= 6)
                {
                    API.CastSpell(Feign_Death);
                    return;
                }
            }
        }

        public override void CombatPulse()
        {
            if (API.PetHealthPercent >= 1 && !API.PlayerIsMounted && !API.PlayerIsCasting && !API.PlayerIsChanneling && !PlayerHasBuff(Aspect_of_the_Turtle) && !PlayerHasBuff(Feign_Death))
            {
                if (API.CanCast(Misdirection) && !API.PlayerHasBuff(Misdirection) && API.PlayerHasPet && Level >= 21 && (UseMisdirection == "On" || (UseMisdirection == "On AOE" & IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)))
                {
                    API.CastSpell(Misdirection);
                    return;
                }
                if (API.CanCast(Muzzle) && isInterrupt && API.TargetRange <= 40 && Level >= 18)
                {
                    API.CastSpell(Muzzle);
                    return;
                }
                if (isRacial && IsCooldowns)
                {
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && API.SpellCDDuration(CoordinatedAssault) > 3000 && InRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && InRange && API.SpellCDDuration(CoordinatedAssault) > 3000)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //  cds->add_action("lights_judgment");
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged Draenei" && InRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //  cds->add_action("berserking,if=cooldown.coordinated_assault.remains>60|time_to_die<13");
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && InRange && (API.SpellCDDuration(CoordinatedAssault) > 6000 || API.TargetTimeToDie < 1300))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                }
                    //  cds->add_action(this, "Aspect of the Eagle", "if=target.distance>=6");
                    if(API.CanCast(AspectOfTheEagle) && (UseAspectoftheEagle == "With Cooldowns" && (IsCooldowns) || UseAspectoftheEagle == "On Cooldown") && API.TargetRange >=6)
                    {
                        API.CastSpell(AspectOfTheEagle);
                        return;
                    }
                
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell("Trinket2");
                }
                if (!IsAOE || API.TargetUnitInRangeCount < AOEUnitNumber)
                {
                    //st->add_action("harpoon,if=talent.terms_of_engagement&focus<focus.max");
                    //st->add_action("kill_command,if=focus+cast_regen<focus.max&buff.tip_of_the_spear.stack<3&!talent.alpha_predator");
                    if (CanCastKillCommand && Focus + 15 + FocusRegen < API.PlayerMaxFocus && API.PlayerBuffStacks(TipOfTheSpear) < 3 && !Talent_AlphaPredator)
                    {
                        API.CastSpell(Kill_Command);
                        return;
                    }
                    //st->add_action("mongoose_bite,if=dot.shrapnel_bomb.ticking&(dot.internal_bleeding.stack<2|dot.shrapnel_bomb.remains<gcd)|buff.mongoose_fury.up&buff.mongoose_fury.remains<focus%30*gcd|talent.birds_of_prey&buff.coordinated_assault.up&buff.coordinated_assault.remains<gcd");
                    if (API.CanCast(MongooseBite) && TargetHasDebuff(ShrapnelBomb) && ((API.TargetDebuffStacks(internal_bleeding) < 2 || API.TargetDebuffRemainingTime(ShrapnelBomb) < gcd) || PlayerHasBuff(MongooseFury) && API.PlayerBuffTimeRemaining(MongooseFury) < (Focus % 30) * 100 * gcd || Talent_BirdsOfPrey && PlayerHasBuff(CoordinatedAssault) && API.PlayerBuffTimeRemaining(CoordinatedAssault) < gcd))
                    {
                        API.CastSpell(MongooseBite);
                        return;
                    }
                    //st->add_action("raptor_strike,if=dot.shrapnel_bomb.ticking&(dot.internal_bleeding.stack<2|dot.shrapnel_bomb.remains<gcd)|talent.birds_of_prey&buff.coordinated_assault.up&buff.coordinated_assault.remains<gcd");
                    if (CanCastRaptorStrike && TargetHasDebuff(ShrapnelBomb) && ((API.TargetDebuffStacks(internal_bleeding) < 2 || API.TargetDebuffRemainingTime(ShrapnelBomb) < gcd) || Talent_BirdsOfPrey && PlayerHasBuff(CoordinatedAssault) && API.PlayerBuffTimeRemaining(CoordinatedAssault) < gcd))
                    {
                        API.CastSpell(RaptorStrike);
                        return;
                    }
                    //st->add_action("wild_spirits");
                    if (API.CanCast(Wild_Spirits) && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Wild_Spirits);
                        return;
                    }
                    //st->add_action("tar_trap,if=runeforge.soulforge_embers&tar_trap.remains<gcd&cooldown.flare.remains<gcd");
                    //st->add_action("flare,if=runeforge.soulforge_embers&tar_trap.up");
                    //st->add_action("coordinated_assault");
                    if (CanCastCoordinatedAssault && (UseCoordinatedAssault == "With Cooldowns" && (IsCooldowns) || UseCoordinatedAssault == "On Cooldown"))
                    {
                        API.CastSpell(CoordinatedAssault);
                        return;
                    }
                    //st->add_action("resonating_arrow");
                    if (API.CanCast(Resonating_Arrow) && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Resonating_Arrow);
                        return;
                    }
                    //st->add_action("flayed_shot");
                    if (API.CanCast(Flayed_Shot) && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Flayed_Shot);
                        return;
                    }
                    //st->add_action("death_chakram,if=focus+cast_regen<focus.max");
                    if (API.CanCast(Death_Chakram) && Focus + FocusRegen * gcd / 100 < API.FocusMaxHealth && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Death_Chakram);
                        return;
                    }
                    //st->add_action("kill_shot");
                    if (API.CanCast(Kill_Shot) && (API.TargetHealthPercent < 20 || PlayerHasBuff(FlayersMark)) && API.PlayerFocus >= 10 && InRange && Level >= 42)
                    {
                        API.CastSpell(Kill_Shot);
                        return;
                    }
                    if (!API.SpellISOnCooldown(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && (API.MouseoverHealthPercent < 20 || API.MouseoverHasBuff(FlayersMark))) && API.PlayerFocus >= 10 && Level >= 42)
                    {
                        API.CastSpell(Kill_Shot + "MO");
                        return;
                    }
                    //st->add_action("flanking_strike,if=focus+cast_regen<focus.max");
                    if (API.CanCast(FlankingStrike) && Talent_FlankingStrike && Focus + 15 + FocusRegen * gcd / 100 < API.PlayerMaxFocus)
                    {
                        API.CastSpell(FlankingStrike);
                        return;
                    }
                    //st->add_action("kill_command,if=focus+cast_regen<focus.max&buff.tip_of_the_spear.stack<3&full_recharge_time<2*gcd");
                    if (CanCastKillCommand && Focus + 15 + FocusRegen < API.PlayerMaxFocus && API.PlayerBuffStacks(TipOfTheSpear) < 3 && KillCommandFactional > 170)
                    {
                        API.CastSpell(Kill_Command);
                        return;
                    }
                    //st->add_action("chakrams");
                    if (API.CanCast(Chakrams) && Talent_Chakrams && API.TargetRange <= 40 && Focus >= 15)
                    {
                        API.CastSpell(Chakrams);
                        return;
                    }
                    //st->add_action("a_murder_of_crows");
                    if (API.CanCast(A_Murder_of_Crows) && Talent_AMurderofCrows && API.TargetRange <= 40 && Focus >= 30)
                    {
                        API.CastSpell(A_Murder_of_Crows);
                        return;
                    }
                    //st->add_action("wildfire_bomb,if=next_wi_bomb.volatile&dot.serpent_sting.ticking&dot.serpent_sting.refreshable");
                    if (CanCastWildfireBomb && next_wi_bomb(VolatileBomb) && TargetHasDebuff(SerpentSting) && API.TargetDebuffRemainingTime(SerpentSting) < 200)
                    {
                        API.CastSpell(WildfireBomb);
                        return;
                    }
                    //st->add_action("serpent_sting,if=refreshable&(buff.mongoose_fury.stack<5|!talent.mongoose_bite)&(!buff.coordinated_assault.up|!talent.birds_of_prey)|buff.vipers_venom.up");
                    if (CanCastSerpentSting && API.TargetDebuffRemainingTime(SerpentSting) < 200 && ((API.PlayerBuffStacks(MongooseFury) < 5 || !Talent_MongooseBite) && (!PlayerHasBuff(CoordinatedAssault) || !Talent_BirdsOfPrey) || PlayerHasBuff(vipers_venom)))
                    {
                        API.CastSpell(SerpentSting);
                        return;
                    }
                    //st->add_action("wildfire_bomb,if=next_wi_bomb.shrapnel&focus>60|next_wi_bomb.pheromone&(focus+cast_regen+action.kill_command.cast_regen*3<focus.max|talent.mongoose_bite&!buff.mongoose_fury.up)|!dot.wildfire_bomb.ticking&full_recharge_time<gcd");
                    if (CanCastWildfireBomb && (next_wi_bomb(ShrapnelBomb) && Focus > 60 || next_wi_bomb(PheromoneBomb) && (Focus + FocusRegen * gcd / 100 + 15 * 3 < API.PlayerMaxFocus || Talent_MongooseBite && !PlayerHasBuff(MongooseFury)) || !TargetHasDebuff(WildfireBomb) && WildfireBomb_FullRechargeTime < 200))
                    {
                        API.CastSpell(WildfireBomb);
                        return;
                    }
                    //st->add_action("kill_command,if=focus+cast_regen<focus.max&buff.tip_of_the_spear.stack<3");
                    if (CanCastKillCommand && Focus + 15 + FocusRegen < API.PlayerMaxFocus && API.PlayerBuffStacks(TipOfTheSpear) < 3)
                    {
                        API.CastSpell(Kill_Command);
                        return;
                    }
                    //st->add_action("steel_trap,if=!runeforge.nessingwarys_trapping_apparatus|focus+cast_regen+25<focus.max");
                    if (API.CanCast(SteelTrap) && Talent_SteelTrap && API.TargetRange <= 40 && (!nessingwarys_trapping_apparatus_enabled || Focus + FocusRegen + 25 < API.PlayerMaxFocus))
                    {
                        API.CastSpell(SteelTrap);
                        return;
                    }
                    //st->add_action("tar_trap,if=runeforge.nessingwarys_trapping_apparatus&focus+cast_regen+25<focus.max");
                    //st->add_action("freezing_trap,if=runeforge.nessingwarys_trapping_apparatus&focus+cast_regen+25<focus.max");
                    //st->add_action("mongoose_bite,if=buff.mongoose_fury.up|focus+action.kill_command.cast_regen>focus.max");
                    if (API.CanCast(MongooseBite) && Focus >= 30 && (MeleeRange || PlayerHasBuff(AspectOfTheEagle) && API.TargetRange <= 40) && TargetHasDebuff(ShrapnelBomb) && (PlayerHasBuff(MongooseFury) || Focus + FocusRegen * gcd / 100 + 15 > API.PlayerMaxFocus))
                    {
                        API.CastSpell(MongooseBite);
                        return;
                    }
                    //st->add_action("raptor_strike,if=!next_wi_bomb.shrapnel|buff.tip_of_the_spear.stack>2|focus+action.kill_command.cast_regen>focus.max");
                    if (CanCastRaptorStrike && (!next_wi_bomb(ShrapnelBomb) || API.PlayerBuffStacks(TipOfTheSpear) > 2 || Focus + FocusRegen * gcd / 100 + 15 > API.PlayerMaxFocus))
                    {
                        API.CastSpell(RaptorStrike);
                        return;
                    }
                    //st->add_action("kill_command,if=buff.tip_of_the_spear.stack<3");
                    if (CanCastKillCommand && API.PlayerBuffStacks(TipOfTheSpear) < 3)
                    {
                        API.CastSpell(Kill_Command);
                        return;
                    }
                    //st->add_action("wildfire_bomb,if=!dot.wildfire_bomb.ticking&!talent.wildfire_infusion");
                    if (CanCastWildfireBomb && !wildfirebomb_ticking && !Talent_WildfireInfusion)
                    {
                        API.CastSpell(WildfireBomb);
                        return;
                    }

                }
                if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)
                {
                    if (MultiDot && API.CanCast(Kill_Command) && API.TargetHasDebuff(Kill_Command, false, false) && API.PlayerUnitInMeleeRangeCount > API.PlayerBuffStacks(Predator))
                    {
                        API.CastSpell(Tab);
                    }
                    // cleave->add_action("harpoon,if=talent.terms_of_engagement&focus<focus.max");
                    // cleave->add_action("wild_spirits");
                    if (API.CanCast(Wild_Spirits) && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Wild_Spirits);
                        return;
                    }
                    // cleave->add_action("tar_trap,if=runeforge.soulforge_embers&tar_trap.remains<gcd&cooldown.flare.remains<gcd");
                    // cleave->add_action("flare,if=runeforge.soulforge_embers&tar_trap.up");
                    // cleave->add_action("resonating_arrow");
                    if (API.CanCast(Resonating_Arrow) && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Resonating_Arrow);
                        return;
                    }
                    // cleave->add_action("wildfire_bomb,if=full_recharge_time<gcd");
                    if (CanCastWildfireBomb && WildfireBomb_FullRechargeTime < 200)
                    {
                        API.CastSpell(WildfireBomb);
                        return;
                    }
                    // cleave->add_action("chakrams");
                    if (API.CanCast(Chakrams) && Talent_Chakrams && API.TargetRange <= 40 && Focus >= 15)
                    {
                        API.CastSpell(Chakrams);
                        return;
                    }
                    // cleave->add_action("butchery,if=dot.shrapnel_bomb.ticking&(dot.internal_bleeding.stack<2|dot.shrapnel_bomb.remains<gcd)");
                    if (API.CanCast(Butchery) && API.TargetHasDebuff(ShrapnelBomb) && (API.TargetDebuffStacks(internal_bleeding) < 2 || API.TargetDebuffRemainingTime(ShrapnelBomb) < 200))
                    {
                        API.CastSpell(Butchery);
                        return;
                    }
                    // cleave->add_action("carve,if=dot.shrapnel_bomb.ticking");
                    if (API.CanCast(Carve) && API.TargetHasDebuff(ShrapnelBomb))
                    {
                        API.CastSpell(Carve);
                        return;
                    }
                    // cleave->add_action("death_chakram,if=focus+cast_regen<focus.max");
                    if (API.CanCast(Death_Chakram) && Focus + FocusRegen * gcd / 100 < API.FocusMaxHealth && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Death_Chakram);
                        return;
                    }
                    // cleave->add_action("coordinated_assault");
                    if (CanCastCoordinatedAssault && (UseCoordinatedAssault == "With Cooldowns" && (IsCooldowns) || UseCoordinatedAssault == "On Cooldown"))
                    {
                        API.CastSpell(CoordinatedAssault);
                        return;
                    }
                    // cleave->add_action("butchery,if=charges_fractional>2.5&cooldown.wildfire_bomb.full_recharge_time>spell_targets%2");
                    if (API.CanCast(Butchery) && WildfireBombFractional > 250 && WildfireBomb_FullRechargeTime / 100 > API.PlayerUnitInMeleeRangeCount % 2)
                    {
                        API.CastSpell(Butchery);
                        return;
                    }
                    // cleave->add_action("flanking_strike,if=focus+cast_regen<focus.max");
                    if (API.CanCast(FlankingStrike) && Talent_FlankingStrike && Focus + 15 + FocusRegen * gcd / 100 < API.PlayerMaxFocus)
                    {
                        API.CastSpell(FlankingStrike);
                        return;
                    }
                    // cleave->add_action("kill_command,target_if=min:Predator.remains,if=focus+cast_regen<focus.max&full_recharge_time<gcd");
                    if (CanCastKillCommand && Focus + 15 + FocusRegen < API.PlayerMaxFocus && KillCommandFactional > 180)
                    {
                        API.CastSpell(Kill_Command);
                        return;
                    }
                    // cleave->add_action("wildfire_bomb,if=!dot.wildfire_bomb.ticking");
                    if (CanCastWildfireBomb && !wildfirebomb_ticking)
                    {
                        API.CastSpell(WildfireBomb);
                        return;
                    }
                    // cleave->add_action("butchery,if=(!next_wi_bomb.shrapnel|!talent.wildfire_infusion)&cooldown.wildfire_bomb.full_recharge_time>spell_targets%2");
                    if (API.CanCast(Butchery) && ((!next_wi_bomb(ShrapnelBomb) || !Talent_WildfireInfusion) && WildfireBomb_FullRechargeTime / 100 > API.PlayerUnitInMeleeRangeCount % 2))
                    {
                        API.CastSpell(Butchery);
                        return;
                    }
                    // cleave->add_action("carve,if=cooldown.wildfire_bomb.full_recharge_time>spell_targets%2");
                    if (API.CanCast(Carve) && WildfireBomb_FullRechargeTime / 100 > API.PlayerUnitInMeleeRangeCount % 2)
                    {
                        API.CastSpell(Carve);
                        return;
                    }
                    // cleave->add_action("kill_shot");
                    if (API.CanCast(Kill_Shot) && (API.TargetHealthPercent < 20 || PlayerHasBuff(FlayersMark)) && API.PlayerFocus >= 10 && InRange && Level >= 42)
                    {
                        API.CastSpell(Kill_Shot);
                        return;
                    }
                    if (!API.SpellISOnCooldown(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && (API.MouseoverHealthPercent < 20 || API.MouseoverHasBuff(FlayersMark))) && API.PlayerFocus >= 10 && Level >= 42)
                    {
                        API.CastSpell(Kill_Shot + "MO");
                        return;
                    }
                    // cleave->add_action("flayed_shot");
                    if (API.CanCast(Flayed_Shot) && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Flayed_Shot);
                        return;
                    }
                    // cleave->add_action("a_murder_of_crows");
                    if (API.CanCast(A_Murder_of_Crows) && Talent_AMurderofCrows && API.TargetRange <= 40 && Focus >= 30)
                    {
                        API.CastSpell(A_Murder_of_Crows);
                        return;
                    }
                    // cleave->add_action("steel_trap,if=!runeforge.nessingwarys_trapping_apparatus|focus+cast_regen+25<focus.max");
                    if (API.CanCast(SteelTrap) && Talent_SteelTrap && API.TargetRange <= 40 && (!nessingwarys_trapping_apparatus_enabled || Focus + FocusRegen + 25 < API.PlayerMaxFocus))
                    {
                        API.CastSpell(SteelTrap);
                        return;
                    }
                    // cleave->add_action("serpent_sting,target_if=min:remains,if=refreshable&talent.hydras_bite");
                    if (CanCastSerpentSting && API.TargetDebuffRemainingTime(SerpentSting) < 200 && Talent_HydrasBite)
                    {
                        API.CastSpell(SerpentSting);
                        return;
                    }
                    // cleave->add_action("carve");
                    if (API.CanCast(Carve))
                    {
                        API.CastSpell(Carve);
                        return;
                    }
                    // cleave->add_action("kill_command,target_if=min:Predator.remains,if=focus+cast_regen<focus.max");
                    if (CanCastKillCommand && Focus + 15 + FocusRegen < API.PlayerMaxFocus)
                    {
                        API.CastSpell(Kill_Command);
                        return;
                    }
                    // cleave->add_action("serpent_sting,target_if=min:remains,if=refreshable&(!talent.birds_of_prey|!buff.coordinated_assault.up)&(!runeforge.rylakstalkers_confounding_strikes|next_wi_bomb.volatile|talent.butchery)|buff.vipers_venom.up");
                    if (CanCastSerpentSting && API.TargetDebuffRemainingTime(SerpentSting) < 200 && (!Talent_BirdsOfPrey||!PlayerHasBuff(CoordinatedAssault)&&(!rylakstalkers_confounding_strikes_enabled||next_wi_bomb(VolatileBomb)||Talent_Butchery)||PlayerHasBuff(vipers_venom)))
                    {
                        API.CastSpell(SerpentSting);
                        return;
                    }
                    // cleave->add_action("mongoose_bite,target_if=max:debuff.latent_poison_injection.stack,if=debuff.latent_poison_injection.stack>9");
                    if (API.CanCast(MongooseBite) && Focus>30&& (MeleeRange || PlayerHasBuff(AspectOfTheEagle) && API.TargetRange <= 40) && API.TargetDebuffStacks(latent_poison_injection) >9)
                    {
                        API.CastSpell(MongooseBite);
                        return;
                    }
                    // cleave->add_action("raptor_strike,target_if=max:debuff.latent_poison_injection.stack,if=debuff.latent_poison_injection.stack>9");
                    if (CanCastRaptorStrike && Focus >=30 && MeleeRange && API.TargetDebuffStacks(latent_poison_injection) > 9)
                    {
                        API.CastSpell(RaptorStrike);
                        return;
                    }
                    // cleave->add_action("serpent_sting,target_if=min:remains,if=!talent.birds_of_prey&!talent.vipers_venom&!runeforge.latent_poison_injectors&!runeforge.rylakstalkers_confounding_strikes");
                    if (CanCastSerpentSting && API.TargetDebuffRemainingTime(SerpentSting) < 200 && !Talent_BirdsOfPrey&&!Talent_ViperVenom&&!latent_poison_injectors_enabled&&!rylakstalkers_confounding_strikes_enabled)
                    {
                        API.CastSpell(SerpentSting);
                        return;
                    }
                    // cleave->add_action("mongoose_bite,target_if=max:debuff.latent_poison_injection.stack");
                    if (API.CanCast(MongooseBite) && (MeleeRange || PlayerHasBuff(AspectOfTheEagle) && API.TargetRange <= 40) && API.TargetDebuffStacks(latent_poison_injection) > 9)
                    {
                        API.CastSpell(MongooseBite);
                        return;
                    }
                    // cleave->add_action("raptor_strike,target_if=max:debuff.latent_poison_injection.stack");
                }
            }

        }

        public override void OutOfCombatPulse()
        {

        }
    }
}




