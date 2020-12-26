using System.Diagnostics;
using System.Linq;

namespace HyperElk.Core
{
    public class BMHunter : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool SmallCDs => API.ToggleIsEnabled("Small CDs");
        //stopwatch
        private readonly Stopwatch pullwatch = new Stopwatch();
        private readonly Stopwatch CallPetTimer = new Stopwatch();

        //Spells,Buffs,Debuffs
        private string Steady_Shot = "Steady Shot";
        private string Arcane_Shot = "Arcane Shot";
        private string Kill_Command = "Kill Command";
        private string Barbed_Shot = "Barbed Shot";
        private string Cobra_Shot = "Cobra Shot";
        private string Mend_Pet = "Mend Pet";
        private string Bestial_Wrath = "Bestial Wrath";
        private string Aspect_of_the_Wild = "Aspect of the Wild";
        private string Kill_Shot = "Kill Shot";
        private string Multi_Shot = "Multi-Shot";
        private string Misdirection = "Misdirection";
        private string Exhilaration = "Exhilaration";
        private string Feign_Death = "Feign Death";
        private string Counter_Shot = "Counter Shot";
        private string Dire_Beast = "Dire Beast";
        private string Chimaera_Shot = "Chimaera Shot";
        private string A_Murder_of_Crows = "A Murder of Crows";
        private string Barrage = "Barrage";
        private string Stampede = "Stampede";
        private string Bloodshed = "Bloodshed";
        private string Frenzy = "Frenzy";
        private string Beast_Cleave = "Beast Cleave";
        private string Aspect_of_the_Turtle = "Aspect of the Turtle";
        private string Revive_Pet = "Revive Pet";
        private string Wild_Spirits = "Wild Spirits";
        private string Resonating_Arrow = "Resonating Arrow";
        private string Flayed_Shot = "Flayed Shot";
        private string Death_Chakram = "Death Chakram";
        private string FlayersMark = "Flayer's Mark";
        private string WildMark = "Wild Mark";
        private string HuntersMark = "Hunter's Mark";
        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool InRange => API.TargetRange <= 40;
        private float gcd => API.SpellGCDTotalDuration;
        private float BarbedShotCount => (API.PlayerHasBuff("246152", false, false) ? 1 : 0) + (API.PlayerHasBuff("246851", false, false) ? 1 : 0) + (API.PlayerHasBuff("217200", false, false) ? 1 : 0);
        private float FocusRegen => 10f * (1f + API.PlayerGetHaste);
        private float RealFocusRegen => FocusRegen + BarbedShotCount * 2.5f;
        private float RealFocusTimeToMax => ((120f - API.PlayerFocus) / ((FocusRegen + BarbedShotCount * 2.5f) + (5 * API.PlayerBuffStacks(Aspect_of_the_Wild)))) * 100f;
        private float Barrage_ExecuteTime => 300f / (1f + (API.PlayerGetHaste));
        private float BarbedShotCooldown => 1200f / (1f + (API.PlayerGetHaste));
        private float Barbed_Shot_Fractional => (API.SpellCharges(Barbed_Shot) * 100 + ((BarbedShotCooldown - API.SpellChargeCD(Barbed_Shot)) / (BarbedShotCooldown / 100)));
        private float Barbed_Shot_FullRechargeTime => (2 - API.SpellCharges(Barbed_Shot)) * BarbedShotCooldown + API.SpellCDDuration(Barbed_Shot);
        //Talents
        private bool Talent_KillerInstinct => API.PlayerIsTalentSelected(1, 1);
        private bool Talent_DireBeast => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_ScentOfBlood => API.PlayerIsTalentSelected(2, 1);
        private bool Talent_OnewiththePack => API.PlayerIsTalentSelected(2, 2);
        private bool Talent_ChimaeraShot => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_AMurderOfCrows => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_Barrage => API.PlayerIsTalentSelected(6, 2);
        private bool Talent_Stampede => API.PlayerIsTalentSelected(6, 3);
        private bool Talent_Bloodshed => API.PlayerIsTalentSelected(7, 3);


        private bool Playeriscasting => API.PlayerCurrentCastTimeRemaining > 40;
        private static void CastSpell(string spell)
        {
            if (API.CanCast(spell))
            {
                API.CastSpell(spell);
                return;
            }
        }
        private bool Race(string race)
        {
            return API.PlayerRaceName == race && PlayerRaceSettings == race;
        }
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool PetHasBuff(string buff)
        {
            return API.PetHasBuff(buff, false, false);
        }

        //CBProperties


        string[] MisdirectionList = new string[] { "Off", "On AOE", "On" };
        string[] AlwaysCooldownsList = new string[] { "always", "with Cooldowns" };
        string[] AspectoftheWildList = new string[] { "always", "with Cooldowns" };
        string[] AMurderofCrowsList = new string[] { "always", "with Cooldowns" };
        string[] StampedeList = new string[] { "always", "with Cooldowns", "on AOE" };
        string[] BloodshedList = new string[] { "always", "with Cooldowns" };
        string[] combatList = new string[] { "In Combat", "Out Of Combat", "Everytime" };


        private int ExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration)];
        private int PetExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration + "PET")];
        private int AspectoftheTurtleLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Aspect_of_the_Turtle)];
        private int FeignDeathLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feign_Death)];
        private int MendPetLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Mend_Pet)];
        private string UseMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private string UseAspectoftheWild => AspectoftheWildList[CombatRoutine.GetPropertyInt(Aspect_of_the_Wild)];
        private string UseBestialWrath => AlwaysCooldownsList[CombatRoutine.GetPropertyInt(Bestial_Wrath)];
        private string UseAMurderofCrows => AMurderofCrowsList[CombatRoutine.GetPropertyInt(A_Murder_of_Crows)];
        private string UseStampede => StampedeList[CombatRoutine.GetPropertyInt(Stampede)];
        private string UseBloodshed => BloodshedList[CombatRoutine.GetPropertyInt(Bloodshed)];
        private bool BarbedShotPetInRange => CombatRoutine.GetPropertyBool("BarbedShot");
        private bool UseCallPet => CombatRoutine.GetPropertyBool("CallPet");
        private string UseRevivePet => combatList[CombatRoutine.GetPropertyInt(Revive_Pet)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];

        public override void Initialize()
        {
            CombatRoutine.Name = "Beast Mastery Hunter by Vec";
            API.WriteLog("Welcome to Beast Mastery Hunter Rotation");
            API.WriteLog("Misdirection Macro : /cast [@focus,help][help][@pet,exists] Misdirection");
            API.WriteLog("Kill Shot Mouseover - /cast [@mouseover] Kill Shot");

            //Spells
            CombatRoutine.AddSpell(Steady_Shot,56641, "D1");
            CombatRoutine.AddSpell(Arcane_Shot,185358, "D2");
            CombatRoutine.AddSpell(Kill_Command,34026, "D2");
            CombatRoutine.AddSpell(Barbed_Shot,217200, "R");
            CombatRoutine.AddSpell(Mend_Pet,136, "X");
            CombatRoutine.AddSpell(Revive_Pet,982, "X");
            CombatRoutine.AddSpell("Call Pet", "F1");
            CombatRoutine.AddSpell(Cobra_Shot,193455, "D5");
            CombatRoutine.AddSpell(Bestial_Wrath,19574, "C");
            CombatRoutine.AddSpell(Aspect_of_the_Wild,193530, "V");
            CombatRoutine.AddSpell(Kill_Shot,53351, "D6");
            CombatRoutine.AddSpell(Multi_Shot,2643, "D4");

            CombatRoutine.AddSpell(Counter_Shot,147362, "D0");
            CombatRoutine.AddSpell(Exhilaration,109304, "NumPad9");
            CombatRoutine.AddSpell(Misdirection,34477, "Q");

            CombatRoutine.AddSpell(Dire_Beast,120679, "F8");
            CombatRoutine.AddSpell(Chimaera_Shot,53209, "D3");
            CombatRoutine.AddSpell(A_Murder_of_Crows,131894, "F");
            CombatRoutine.AddSpell(Barrage,120360, "F7");
            CombatRoutine.AddSpell(Stampede,201430, "F7");
            CombatRoutine.AddSpell(Bloodshed,321530, "F11");
            CombatRoutine.AddSpell(Feign_Death,5384, "F2");
            CombatRoutine.AddSpell(Aspect_of_the_Turtle,186265, "G");

            CombatRoutine.AddSpell(Wild_Spirits,328231, "F10");
            CombatRoutine.AddSpell(Resonating_Arrow, 308491, "F10");
            CombatRoutine.AddSpell(Flayed_Shot, 324149, "F10");
            CombatRoutine.AddSpell(Death_Chakram, 325028, "F10");




            //Macros
            CombatRoutine.AddMacro(Kill_Shot + "MO", "NumPad7");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            //Buffs

            CombatRoutine.AddBuff("246152");
            CombatRoutine.AddBuff("246851");
            CombatRoutine.AddBuff("217200");
            CombatRoutine.AddBuff(Frenzy,272790);
            CombatRoutine.AddBuff(Beast_Cleave,118455);
            CombatRoutine.AddBuff(Aspect_of_the_Turtle,186265);
            CombatRoutine.AddBuff(Aspect_of_the_Wild,193530);
            CombatRoutine.AddBuff(Misdirection,34477);
            CombatRoutine.AddBuff(Bestial_Wrath,19574);
            CombatRoutine.AddBuff(Feign_Death,5384);
            CombatRoutine.AddBuff(FlayersMark, 324156);

            //Debuffs

            CombatRoutine.AddDebuff(WildMark, 328275);
            CombatRoutine.AddDebuff(Resonating_Arrow, 308491);
            CombatRoutine.AddDebuff(HuntersMark, 257284);
            //Toggle
            CombatRoutine.AddToggle("Small CDs");
            CombatRoutine.AddToggle("Mouseover");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", true, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            //Settings
            CombatRoutine.AddProp(Misdirection, "Use Misdirection", MisdirectionList, "Use " + Misdirection + "Off, On AOE, On", "Generic", 0);
            CombatRoutine.AddProp(Bestial_Wrath, "Use " + Bestial_Wrath, AlwaysCooldownsList, "Use " + Bestial_Wrath + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Aspect_of_the_Wild, "Use " + Aspect_of_the_Wild, AspectoftheWildList, "Use " + Aspect_of_the_Wild + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(A_Murder_of_Crows, "Use " + A_Murder_of_Crows, AMurderofCrowsList, "Use " + A_Murder_of_Crows + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Stampede, "Use " + Stampede, StampedeList, "Use " + Stampede + "always, with Cooldowns, on AOE", "Cooldowns", 0);
            CombatRoutine.AddProp(Bloodshed, "Use " + Bloodshed, BloodshedList, "Use " + Bloodshed + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Revive_Pet, "Use " + Revive_Pet, combatList, "Use " + "Revive/Call Pet" + "In Combat, Out Of Combat, Everytime", "Pet", 0);

            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Cooldowns", 0);

            CombatRoutine.AddProp("BarbedShot", "Barbed Shot", false, "Use Barbed Shot with pet in range", "Pet");
            CombatRoutine.AddProp("CallPet", "Call/Ressurect Pet", true, "Should the rotation try to ressurect/call your Pet", "Pet");
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
            //API.WriteLog("race  " +PlayerRaceSettings + " cd " + API.CanCast(RacialSpell1) );
            if (CallPetTimer.ElapsedMilliseconds > 10000)
            {
                CallPetTimer.Stop();
                CallPetTimer.Reset();
            }
            if (!API.PlayerIsMounted && !Playeriscasting  && !PlayerHasBuff(Aspect_of_the_Turtle) && !PlayerHasBuff(Feign_Death))
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
                if (API.CanCast(Exhilaration) && ((API.PlayerHealthPercent <= ExhilarationLifePercent && PlayerLevel >= 9) || (API.PetHealthPercent <= PetExhilarationLifePercent && API.PetHealthPercent >= 1 && PlayerLevel >= 44)))
                {
                    API.CastSpell(Exhilaration);
                    return;
                }
                if (API.CanCast(Aspect_of_the_Turtle) && API.PlayerHealthPercent <= AspectoftheTurtleLifePercent && PlayerLevel >= 8)
                {
                    API.CastSpell(Aspect_of_the_Turtle);
                    return;
                }
                if (API.CanCast(Feign_Death) && API.PlayerHealthPercent <= FeignDeathLifePercent && PlayerLevel >= 6)
                {
                    API.CastSpell(Feign_Death);
                    return;
                }
            }
        }

        public override void CombatPulse()
        {
            if (API.PetHealthPercent >= 1 && !API.PlayerIsMounted && !Playeriscasting  && !PlayerHasBuff(Aspect_of_the_Turtle) && !PlayerHasBuff(Feign_Death))
            {
                if (API.CanCast(Misdirection) && !API.PlayerHasBuff(Misdirection) && API.PlayerHasPet && PlayerLevel >= 21 && (UseMisdirection == "On" || (UseMisdirection == "On AOE" & IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)))
                {
                    API.CastSpell(Misdirection);
                    return;
                }
                if (API.CanCast(Counter_Shot) && isInterrupt && InRange && PlayerLevel >= 18)
                {
                    API.CastSpell(Counter_Shot);
                    return;
                }
                if (isRacial && IsCooldowns)
                {
                    // cds->add_action("ancestral_call,if=cooldown.bestial_wrath.remains>30");
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && API.SpellCDDuration(Bestial_Wrath) > 3000 && InRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    // cds->add_action("fireblood,if=cooldown.bestial_wrath.remains>30");
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && API.SpellCDDuration(Bestial_Wrath) > 3000 && InRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    // cds->add_action("berserking,if=(buff.wild_spirits.up|!covenant.night_fae&buff.aspect_of_the_wild.up&buff.bestial_wrath.up)&(target.time_to_die>cooldown.berserking.duration+duration|(target.health.pct<35|!talent.killer_instinct))|target.time_to_die<13");
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && InRange && (API.TargetHasDebuff(WildMark) || PlayerCovenantSettings != "Night Fae" && PlayerHasBuff(Aspect_of_the_Wild) && PlayerHasBuff(Bestial_Wrath)) && (API.TargetTimeToDie > API.SpellCDDuration(RacialSpell1) + 1200 || (API.TargetHealthPercent < 35 || !Talent_KillerInstinct)) || API.TargetTimeToDie < 1300)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    // cds->add_action("lights_judgment");
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged Draenei" && InRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }

                    // cds->add_action("blood_fury,if=(buff.wild_spirits.up|!covenant.night_fae&buff.aspect_of_the_wild.up&buff.bestial_wrath.up)&(target.time_to_die>cooldown.blood_fury.duration+duration|(target.health.pct<35|!talent.killer_instinct))|target.time_to_die<16");

                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && InRange && (API.TargetHasDebuff(WildMark) || PlayerCovenantSettings != "Night Fae" && PlayerHasBuff(Aspect_of_the_Wild) && PlayerHasBuff(Bestial_Wrath)) && (API.TargetTimeToDie > API.SpellCDDuration(RacialSpell1) + 1200 || (API.TargetHealthPercent < 35 || !Talent_KillerInstinct)) || API.TargetTimeToDie < 1300)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }


                }
                // cds->add_action("potion,if=buff.aspect_of_the_wild.up|target.time_to_die<26");
                if (!IsAOE || API.TargetUnitInRangeCount < AOEUnitNumber)
                {
                    //st->add_action("aspect_of_the_wild");
                    if (API.CanCast(Aspect_of_the_Wild) && (UseAspectoftheWild == "always" || (UseAspectoftheWild == "with Cooldowns" && IsCooldowns)) && InRange && PlayerLevel >= 38)
                    {
                        API.CastSpell(Aspect_of_the_Wild);
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
                    //st->add_action("barbed_shot,if=pet.main.buff.frenzy.up&pet.main.buff.frenzy.remains<=gcd");
                    if (API.CanCast(Barbed_Shot) && (!BarbedShotPetInRange&& InRange || BarbedShotPetInRange && API.TargetUnitInRangeCount >0) && API.PlayerLevel >= 12 && PetHasBuff(Frenzy) && API.PetBuffTimeRemaining(Frenzy) < 200)
                    {
                        API.CastSpell(Barbed_Shot);
                        return;
                    }
                    //st->add_action("tar_trap,if=runeforge.soulforge_embers&tar_trap.remains<gcd&cooldown.flare.remains<gcd");
                    //st->add_action("flare,if=tar_trap.up&runeforge.soulforge_embers");
                    //st->add_action("bloodshed");
                    if (API.CanCast(Bloodshed) && (UseBloodshed == "always" || (UseBloodshed == "with Cooldowns" && IsCooldowns)) && Talent_Bloodshed && InRange)
                    {
                        API.CastSpell(Bloodshed);
                        return;
                    }
                    //st->add_action("wild_spirits");
                    if (!API.SpellISOnCooldown(Wild_Spirits) && PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Wild_Spirits);
                        return;
                    }
                    //st->add_action("flayed_shot");
                    if (!API.SpellISOnCooldown(Flayed_Shot) && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Flayed_Shot);
                        return;
                    }
                    //st->add_action("kill_shot,if=buff.flayers_mark.remains<5|target.health.pct<=20");
                    if (API.CanCast(Kill_Shot) && (API.TargetHealthPercent < 20 || PlayerHasBuff(FlayersMark)) && API.PlayerFocus >= 10 && InRange && PlayerLevel >= 42)
                    {
                        API.CastSpell(Kill_Shot);
                        return;
                    }
                    if (!API.SpellISOnCooldown(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && (API.MouseoverHealthPercent < 20 || API.MouseoverHasBuff(FlayersMark))) && API.PlayerFocus >= 10 && PlayerLevel >= 42)
                    {
                        API.CastSpell(Kill_Shot + "MO");
                        return;
                    }
                    //st->add_action("barbed_shot,if=(cooldown.wild_spirits.remains>full_recharge_time|!covenant.night_fae)&(cooldown.bestial_wrath.remains<12*charges_fractional+gcd&talent.scent_of_blood|full_recharge_time<gcd&cooldown.bestial_wrath.remains)|target.time_to_die<9");
                    if (API.CanCast(Barbed_Shot) && (!BarbedShotPetInRange && InRange || BarbedShotPetInRange && API.TargetUnitInRangeCount > 0) && API.PlayerLevel >= 12 && (API.SpellCDDuration(Wild_Spirits) > Barbed_Shot_FullRechargeTime || PlayerCovenantSettings != "Night Fae") && ((API.SpellCDDuration(Bestial_Wrath) < (12 * Barbed_Shot_Fractional / 100 + gcd / 100) * 100 && Talent_ScentOfBlood || (API.SpellCharges(Barbed_Shot) >= 1 && API.SpellChargeCD(Barbed_Shot) < gcd && API.SpellISOnCooldown(Bestial_Wrath))) || API.TargetTimeToDie < 900))
                    {
                        API.CastSpell(Barbed_Shot);
                        return;
                    }
                    //st->add_action("death_chakram,if=focus+cast_regen<focus.max");
                    if (!API.SpellISOnCooldown(Death_Chakram) && API.PlayerFocus + RealFocusRegen * gcd / 100 < API.PlayerMaxFocus && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Death_Chakram);
                        return;
                    }
                    //st->add_action("stampede,if=buff.aspect_of_the_wild.up|target.time_to_die<15");
                    if (API.CanCast(Stampede) && (UseStampede == "always" || (UseStampede == "with Cooldowns" && IsCooldowns)) && Talent_Stampede && IsCooldowns && (PlayerHasBuff(Aspect_of_the_Wild) || API.TargetTimeToDie < 1500) && API.TargetRange <= 30)
                    {
                        API.CastSpell(Stampede);
                        return;
                    }
                    //st->add_action("a_murder_of_crows");
                    if (API.CanCast(A_Murder_of_Crows) && (UseAMurderofCrows == "always" || (UseAMurderofCrows == "with Cooldowns" && IsCooldowns)) && Talent_AMurderOfCrows && InRange && API.PlayerFocus >= 30)
                    {
                        API.CastSpell(A_Murder_of_Crows);
                        return;
                    }
                    //st->add_action("resonating_arrow,if=buff.bestial_wrath.up|target.time_to_die<10");
                    if (!API.SpellISOnCooldown(Resonating_Arrow) && PlayerCovenantSettings == "Kyrian" && (PlayerHasBuff(Bestial_Wrath) || API.TargetTimeToDie < 1000) && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Resonating_Arrow);
                        return;
                    }
                    //st->add_action("bestial_wrath,if=cooldown.wild_spirits.remains>15|!covenant.night_fae|target.time_to_die<15");
                    if (API.CanCast(Bestial_Wrath) && (UseBestialWrath == "always" || (UseBestialWrath == "with Cooldowns" && (IsCooldowns || SmallCDs))) && InRange)
                    {
                        API.CastSpell(Bestial_Wrath);
                        return;
                    }
                    //st->add_action("chimaera_shot");
                    if (API.CanCast(Chimaera_Shot) && Talent_ChimaeraShot && InRange)
                    {
                        API.CastSpell(Chimaera_Shot);
                        return;
                    }
                    //st->add_action("kill_command");
                    if (API.CanCast(Kill_Command) && API.PlayerLevel >= 10 && InRange && API.PlayerFocus >= 30)
                    {
                        API.CastSpell(Kill_Command);
                        return;
                    }
                    //st->add_action("bag_of_tricks,if=buff.bestial_wrath.down|target.time_to_die<5");
                    //st->add_action("dire_beast");
                    if (API.CanCast(Dire_Beast) && Talent_DireBeast && InRange)
                    {
                        API.CastSpell(Dire_Beast);
                        return;
                    }
                    //st->add_action("cobra_shot,if=(focus-cost+focus.regen*(cooldown.kill_command.remains-1)>action.kill_command.cost|cooldown.kill_command.remains>1+gcd)|(buff.bestial_wrath.up|buff.nesingwarys_trapping_apparatus.up)&!runeforge.qapla_eredun_war_order|target.time_to_die<3");
                    if (API.CanCast(Cobra_Shot) && API.PlayerFocus >= 35 && API.PlayerLevel >= 14 && InRange && (API.PlayerFocus - 35 + RealFocusRegen * (API.SpellCDDuration(Kill_Command) / 100 - 1) > 30 || API.SpellCDDuration(Kill_Command) > 100 + gcd) || (PlayerHasBuff(Bestial_Wrath)) || API.TargetTimeToDie < 300)
                    {
                        API.CastSpell(Cobra_Shot);
                        return;
                    }
                    //st->add_action("barbed_shot,if=buff.wild_spirits.up");
                    if (API.CanCast(Barbed_Shot) && (!BarbedShotPetInRange && InRange || BarbedShotPetInRange && API.TargetUnitInRangeCount > 0) && API.PlayerLevel >= 12 && API.TargetHasDebuff(WildMark))
                    {
                        API.CastSpell(Barbed_Shot);
                        return;
                    }
                    //st->add_action("arcane_pulse,if=buff.bestial_wrath.down|target.time_to_die<5");
                    //st->add_action("tar_trap,if=runeforge.soulforge_embers|runeforge.nessingwarys_trapping_apparatus");
                    //st->add_action("freezing_trap,if=runeforge.nessingwarys_trapping_apparatus");
                }
                //SL - AOE
                if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)
                {
                    //cleave->add_action("aspect_of_the_wild");
                    if (API.CanCast(Aspect_of_the_Wild) && (UseAspectoftheWild == "always" || (UseAspectoftheWild == "with Cooldowns" && IsCooldowns)) && InRange && PlayerLevel >= 38)
                    {
                        API.CastSpell(Aspect_of_the_Wild);
                        return;
                    }
                    //cleave->add_action("barbed_shot,target_if=min:dot.barbed_shot.remains,if=pet.main.buff.frenzy.up&pet.main.buff.frenzy.remains<=gcd");
                    if (API.CanCast(Barbed_Shot) && (!BarbedShotPetInRange && InRange || BarbedShotPetInRange && API.TargetUnitInRangeCount > 0) && API.PlayerLevel >= 12 && PetHasBuff(Frenzy) && API.PetBuffTimeRemaining(Frenzy) < 200)
                    {
                        API.CastSpell(Barbed_Shot);
                        return;
                    }
                    //cleave->add_action("multishot,if=gcd-pet.main.buff.beast_cleave.remains>0.25");
                    if (API.CanCast(Multi_Shot) && 150 - API.PlayerBuffTimeRemaining(Beast_Cleave) > 25 && API.PlayerFocus >= 40 && InRange && PlayerLevel >= 32)
                    {
                        API.CastSpell(Multi_Shot);
                        return;
                    }
                    //cleave->add_action("tar_trap,if=runeforge.soulforge_embers&tar_trap.remains<gcd&cooldown.flare.remains<gcd");
                    //cleave->add_action("flare,if=tar_trap.up&runeforge.soulforge_embers");
                    //cleave->add_action("death_chakram,if=focus+cast_regen<focus.max");
                    if (!API.SpellISOnCooldown(Death_Chakram) && API.PlayerFocus + RealFocusRegen * gcd / 100 + 3 * API.TargetUnitInRangeCount < API.PlayerMaxFocus && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Death_Chakram);
                        return;
                    }

                    //cleave->add_action("wild_spirits");
                    if (!API.SpellISOnCooldown(Wild_Spirits) && PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Wild_Spirits);
                        return;
                    }
                    //cleave->add_action("barbed_shot,target_if=min:dot.barbed_shot.remains,if=full_recharge_time<gcd&cooldown.bestial_wrath.remains|cooldown.bestial_wrath.remains<12+gcd&talent.scent_of_blood");
                    if (API.CanCast(Barbed_Shot) && (!BarbedShotPetInRange && InRange || BarbedShotPetInRange && API.TargetUnitInRangeCount > 0) && API.PlayerLevel >= 12 && (API.SpellCharges(Barbed_Shot) == 1 && API.SpellChargeCD(Barbed_Shot) < gcd && API.SpellISOnCooldown(Bestial_Wrath) || API.SpellCDDuration(Bestial_Wrath) < 1200 + gcd && Talent_ScentOfBlood))
                    {
                        API.CastSpell(Barbed_Shot);
                        return;
                    }
                    //cleave->add_action("bestial_wrath");
                    if (API.CanCast(Bestial_Wrath) && (UseBestialWrath == "always" || (UseBestialWrath == "with Cooldowns" && (IsCooldowns || SmallCDs))) && InRange)
                    {
                        API.CastSpell(Bestial_Wrath);
                        return;
                    }
                    //cleave->add_action("resonating_arrow");
                    if (!API.SpellISOnCooldown(Resonating_Arrow) && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Resonating_Arrow);
                        return;
                    }
                    //cleave->add_action("stampede,if=buff.aspect_of_the_wild.up|target.time_to_die<15");
                    if (API.CanCast(Stampede) && (UseAspectoftheWild == "on AOE" || UseAspectoftheWild == "always" || (UseAspectoftheWild == "with Cooldowns" && IsCooldowns)) && Talent_Stampede && IsCooldowns && (PlayerHasBuff(Aspect_of_the_Wild) || API.TargetTimeToDie < 1500) && API.TargetRange <= 30)
                    {
                        API.CastSpell(Stampede);
                        return;
                    }
                    //cleave->add_action("flayed_shot");
                    if (!API.SpellISOnCooldown(Flayed_Shot) && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && (IsCooldowns || SmallCDs) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Flayed_Shot);
                        return;
                    }
                    //cleave->add_action("kill_shot");
                    if (API.CanCast(Kill_Shot) && (API.TargetHealthPercent < 20 || PlayerHasBuff(FlayersMark)) && API.PlayerFocus >= 10 && InRange && PlayerLevel >= 42)
                    {
                        API.CastSpell(Kill_Shot);
                        return;
                    }
                    if (!API.SpellISOnCooldown(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && (API.MouseoverHealthPercent < 20 || PlayerHasBuff(FlayersMark))) && API.PlayerFocus >= 10 && PlayerLevel >= 42)
                    {
                        API.CastSpell(Kill_Shot + "MO");
                        return;
                    }
                    //cleave->add_action("chimaera_shot");
                    if (API.CanCast(Chimaera_Shot) && Talent_ChimaeraShot && InRange)
                    {
                        API.CastSpell(Chimaera_Shot);
                        return;
                    }
                    //cleave->add_action("bloodshed");
                    if (API.CanCast(Bloodshed) && (UseBloodshed == "always" || (UseBloodshed == "with Cooldowns" && IsCooldowns)) && Talent_Bloodshed && InRange)
                    {
                        API.CastSpell(Bloodshed);
                        return;
                    }
                    //cleave->add_action("a_murder_of_crows");
                    if (API.CanCast(A_Murder_of_Crows) && (UseAMurderofCrows == "always" || (UseAMurderofCrows == "with Cooldowns" && IsCooldowns)) && Talent_AMurderOfCrows && InRange && API.PlayerFocus >= 30)
                    {
                        API.CastSpell(A_Murder_of_Crows);
                        return;
                    }
                    //cleave->add_action("barrage,if=pet.main.buff.frenzy.remains>execute_time");
                    if (API.CanCast(Barrage) && InRange && API.PetBuffTimeRemaining(Frenzy) > Barrage_ExecuteTime && Talent_Barrage)
                    {
                        API.CastSpell(Barrage);
                        return;
                    }
                    //cleave->add_action("kill_command,if=focus>cost+action.multishot.cost");
                    if (API.CanCast(Kill_Command) && API.PlayerLevel >= 10 && InRange && API.PlayerFocus >= 30 + 40 - (RealFocusRegen*(gcd/100)))
                    {
                        API.CastSpell(Kill_Command);
                        return;
                    }
                    //cleave->add_action("bag_of_tricks,if=buff.bestial_wrath.down|target.time_to_die<5");
                    //cleave->add_action("dire_beast");
                    if (API.CanCast(Dire_Beast) && Talent_DireBeast && InRange)
                    {
                        API.CastSpell(Dire_Beast);
                        return;
                    }
                    //cleave->add_action("barbed_shot,target_if=min:dot.barbed_shot.remains,if=target.time_to_die<9");
                    if (API.CanCast(Barbed_Shot) && (!BarbedShotPetInRange && InRange || BarbedShotPetInRange && API.TargetUnitInRangeCount > 0) && API.PlayerLevel >= 12 && API.TargetTimeToDie < 900)
                    {
                        API.CastSpell(Barbed_Shot);
                        return;
                    }
                    //cleave->add_action("cobra_shot,if=focus.time_to_max<gcd*2");
                    if (API.CanCast(Cobra_Shot) && API.PlayerFocus >= 35 && API.PlayerBuffTimeRemaining(Beast_Cleave) > gcd && API.PlayerLevel >= 14 && InRange && API.PlayerFocus >= 35 && RealFocusTimeToMax < gcd * 2)
                    {
                        API.CastSpell(Cobra_Shot);
                        return;
                    }
                    //cleave->add_action("tar_trap,if=runeforge.soulforge_embers|runeforge.nessingwarys_trapping_apparatus");
                    //cleave->add_action("freezing_trap,if=runeforge.nessingwarys_trapping_apparatus");
                }
            }
        }


        public override void OutOfCombatPulse()
        {

        }
    }
}



    
