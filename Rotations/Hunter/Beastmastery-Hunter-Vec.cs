using System.Diagnostics;
using System.Linq;

namespace HyperElk.Core
{
    public class BMHunter : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        //stopwatch
        private readonly Stopwatch pullwatch = new Stopwatch();

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
        private string Blood_Fury = "Blood Fury";
        private string Bag_of_Tricks = "Bag of Tricks";
        private string Entropic_Embrace = "Entropic Embrace";
        private string Ancestral_Call = "Ancestral Call";
        private string Lights_Judgment = "Lights Judgment";
        private string Arcane_Torrent = "Arcane Torrent";
        private string Berserking = "Berserking";
        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool InRange => API.TargetRange <= 40;
        private float gcd => API.SpellGCDTotalDuration;
        private float Barbed_Shot_Fractional => (API.SpellCharges(Barbed_Shot) * 100 + ((1000 - API.SpellChargeCD(Barbed_Shot)) / 10));
        private float BarbedShotCount => (API.PlayerHasBuff("246152") ? 1 : 0) + (API.PlayerHasBuff("246851") ? 1 : 0) + (API.PlayerHasBuff("217200") ? 1 : 0);
        private float FocusRegen => 10f * (1f + API.PlayerGetHaste / 100f);
        private float RealFocusRegen => FocusRegen + BarbedShotCount * 2.5f;
        private float RealFocusTimeToMax => ((120f - API.PlayerFocus) / ((FocusRegen + BarbedShotCount * 2.5f) + (5 * API.PlayerBuffStacks(Aspect_of_the_Wild)))) * 100f;
        //Talents
        private bool Talent_DireBeast => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_ScentOfBlood => API.PlayerIsTalentSelected(2, 1);
        private bool Talent_OnewiththePack => API.PlayerIsTalentSelected(2, 2);
        private bool Talent_ChimaeraShot => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_AMurderOfCrows => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_Barrage => API.PlayerIsTalentSelected(6, 2);
        private bool Talent_Stampede => API.PlayerIsTalentSelected(6, 3);
        private bool Talent_Bloodshed => API.PlayerIsTalentSelected(7, 3);

        private static bool PetHasBuff(string buff)
        {
            return API.PetBuffTimeRemaining(buff) > 0 && API.PetBuffTimeRemaining(buff) != 5000000;
        }

    //CBProperties


        string[] MisdirectionList = new string[] { "Off", "On AOE", "On" };
        string[] BestialWrathList = new string[] { "always", "with Cooldowns"};
        string[] AspectoftheWildList = new string[] { "always", "with Cooldowns" };
        string[] AMurderofCrowsList = new string[] { "always", "with Cooldowns" };
        string[] StampedeList = new string[] { "always", "with Cooldowns", "on AOE" };
        string[] BloodshedList = new string[] { "always", "with Cooldowns" };

        private int ExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration)];
        private int PetExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration + "PET")];
        private int AspectoftheTurtleLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Aspect_of_the_Turtle)];
        private int FeignDeathLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feign_Death)];
        private int MendPetLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Mend_Pet)];
        private string UseMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private string UseAspectoftheWild => AspectoftheWildList[CombatRoutine.GetPropertyInt(Aspect_of_the_Wild)];
        private string UseBestialWrath => BestialWrathList[CombatRoutine.GetPropertyInt(Bestial_Wrath)];
        private string UseAMurderofCrows => AMurderofCrowsList[CombatRoutine.GetPropertyInt(A_Murder_of_Crows)];
        private string UseStampede => StampedeList[CombatRoutine.GetPropertyInt(Stampede)];
        private string UseBloodshed => BloodshedList[CombatRoutine.GetPropertyInt(Bloodshed)];
        private bool UseCallPet => CombatRoutine.GetPropertyBool("CallPet");

        public override void Initialize()
        {
            CombatRoutine.Name = "Beast Mastery Hunter by Vec";
            API.WriteLog("Welcome to Beast Masetery Hunter Rotation");
            API.WriteLog("Misdirection Macro : /cast [@focus,help][help][@pet,exists] Misdirection");
            API.WriteLog("Mend Pet Macro (revive/call): /cast [mod]Revive Pet; [@pet,dead]Revive Pet; [nopet]Call Pet 1; Mend Pet");
            API.WriteLog("Kill Shot Mouseover - /cast [@mouseover] Kill Shot");

            //Spells
            CombatRoutine.AddSpell(Steady_Shot, "D1");
            CombatRoutine.AddSpell(Arcane_Shot, "D2");
            CombatRoutine.AddSpell(Kill_Command, "D2");
            CombatRoutine.AddSpell(Barbed_Shot, "R");
            CombatRoutine.AddSpell(Mend_Pet, "X");
            CombatRoutine.AddSpell(Cobra_Shot, "D5");
            CombatRoutine.AddSpell(Bestial_Wrath, "C");
            CombatRoutine.AddSpell(Aspect_of_the_Wild, "V");
            CombatRoutine.AddSpell(Kill_Shot, "D6");
            CombatRoutine.AddSpell(Multi_Shot, "D4");

            CombatRoutine.AddSpell(Counter_Shot, "D0");
            CombatRoutine.AddSpell(Exhilaration, "NumPad9");
            CombatRoutine.AddSpell(Misdirection, "Q");

            CombatRoutine.AddSpell(Dire_Beast, "F8");
            CombatRoutine.AddSpell(Chimaera_Shot, "D3");
            CombatRoutine.AddSpell(A_Murder_of_Crows, "F");
            CombatRoutine.AddSpell(Barrage, "F7");
            CombatRoutine.AddSpell(Stampede, "F7");
            CombatRoutine.AddSpell(Bloodshed, "F11");
            CombatRoutine.AddSpell(Feign_Death, "F2");
            CombatRoutine.AddSpell(Aspect_of_the_Turtle, "G");

            //Macros
            CombatRoutine.AddMacro(Kill_Command+ "MO", "NumPad7");

            //Buffs

            CombatRoutine.AddBuff("246152");
            CombatRoutine.AddBuff("246851");
            CombatRoutine.AddBuff("217200");
            CombatRoutine.AddBuff(Frenzy);
            CombatRoutine.AddBuff(Beast_Cleave);
            CombatRoutine.AddBuff(Aspect_of_the_Turtle);
            CombatRoutine.AddBuff(Aspect_of_the_Wild);
            CombatRoutine.AddBuff(Misdirection);
            CombatRoutine.AddBuff(Bestial_Wrath);
            CombatRoutine.AddBuff(Barbed_Shot);

            //Debuffs

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            //Settings
            CombatRoutine.AddProp(Misdirection, "Use Misdirection", MisdirectionList, "Use " + Misdirection + "Off, On AOE, On", "Generic", 0);
            CombatRoutine.AddProp(Bestial_Wrath, "Use " + Bestial_Wrath, BestialWrathList, "Use " + Bestial_Wrath + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Aspect_of_the_Wild, "Use " + Aspect_of_the_Wild, AspectoftheWildList, "Use " + Aspect_of_the_Wild + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(A_Murder_of_Crows, "Use " + A_Murder_of_Crows, AMurderofCrowsList, "Use " + A_Murder_of_Crows + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Stampede, "Use " + Stampede, StampedeList, "Use " + Stampede + "always, with Cooldowns, on AOE", "Cooldowns", 0);
            CombatRoutine.AddProp(Bloodshed, "Use " + Bloodshed, BloodshedList, "Use " + Bloodshed + "always, with Cooldowns", "Cooldowns", 0);

            CombatRoutine.AddProp("CallPet", "Call/Ressurect Pet", true, "Should the rotation try to ressurect/call your Pet", "Pet");

            CombatRoutine.AddProp(Exhilaration, "Use " + Exhilaration + " below:", percentListProp, "Life percent at which " + Exhilaration + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Exhilaration + "PET", "Use " +Exhilaration + " below:", percentListProp, "Life percent at which " + Exhilaration + " is used to heal your pet, set to 0 to disable", "Pet", 2);
            CombatRoutine.AddProp(Aspect_of_the_Turtle, "Use " + Aspect_of_the_Turtle + " below:", percentListProp, "Life percent at which " + Aspect_of_the_Turtle + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Feign_Death, "Use " + Feign_Death + " below:", percentListProp, "Life percent at which " + Feign_Death + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(Mend_Pet, "Use " + Mend_Pet + " below:", percentListProp, "Life percent at which " + Mend_Pet + " is used, set to 0 to disable", "Pet", 6);
        }

        public override void Pulse()
        {
            //API.WriteLog("frenzy" + HasPetBuff(Frenzy));

            if (API.PlayerIsMounted || API.PlayerIsCasting || API.PlayerHasBuff(Aspect_of_the_Turtle))
            {
                return;
            }
            if (API.CanCast(Mend_Pet) && !API.PlayerHasPet && UseCallPet)
            {
                API.CastSpell(Mend_Pet);
                return;
            }
            if (API.CanCast(Mend_Pet) && API.PlayerHasPet && API.PetHealthPercent <= MendPetLifePercent)
            {
                API.CastSpell(Mend_Pet);
                return;
            }
            if (API.CanCast(Exhilaration) && ((API.PlayerHealthPercent <= ExhilarationLifePercent && PlayerLevel >= 9) || (API.PetHealthPercent <= PetExhilarationLifePercent && PlayerLevel >= 44)))
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

        public override void CombatPulse()
        {
            if (API.CanCast(Misdirection) && API.PlayerHasPet && PlayerLevel >= 21 && (UseMisdirection == "On" || (UseMisdirection == "On AOE" & IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)))
            {
                API.CastSpell(Misdirection);
                return;
            }
            if (API.CanCast(Counter_Shot) && isInterrupt && InRange && PlayerLevel >= 18)
            {
                API.CastSpell(Counter_Shot);
                return;
            }
            #region PULL
            if (API.PlayerTimeInCombat < 10000)
            {

            }
            #endregion
            #region leveling
            if (API.CanCast(Arcane_Shot) && API.PlayerFocus >=40 && API.PlayerLevel < 14 && InRange)
            {
                API.CastSpell(Arcane_Shot);
                return;
            }
            if (API.CanCast(Steady_Shot) && API.PlayerLevel < 10 && InRange)
            {
                API.CastSpell(Steady_Shot);
                return;
            }
            #endregion
            #region Cooldowns
            //Racials   

            #endregion
            #region ST
            if (!IsAOE || API.TargetUnitInRangeCount < AOEUnitNumber)
            {
                // //actions.st = kill_shot
                if (API.CanCast(Kill_Shot) && API.TargetHealthPercent < 20 && API.PlayerFocus >= 10 && InRange && PlayerLevel >= 42)
                {
                    API.CastSpell(Kill_Shot);
                    return;
                }
                if (API.CanCast(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent < 20) && API.PlayerFocus >= 10 && PlayerLevel >= 42)
                {
                    API.CastSpell(Kill_Shot + "MO");
                    return;
                }
                //actions.st +=/ bloodshed
                if (API.CanCast(Bloodshed) && (UseBloodshed == "always" || (UseBloodshed == "with Cooldowns" && IsCooldowns)) && Talent_Bloodshed && InRange)
                {
                    API.CastSpell(Bloodshed);
                    return;
                }
                ////actions.st+=/barbed_shot,if=pet.main.buff.frenzy.up&pet.main.buff.frenzy.remains<gcd|cooldown.bestial_wrath.remains&(full_recharge_time<gcd)|cooldown.bestial_wrath.remains<12+gcd&talent.scent_of_blood.enabled
                if (API.CanCast(Barbed_Shot) && InRange && API.PlayerLevel >= 12 && (PetHasBuff(Frenzy) && API.PetBuffTimeRemaining(Frenzy) < 250 || API.SpellISOnCooldown(Bestial_Wrath) && (API.SpellCharges(Barbed_Shot) == 1 && API.SpellChargeCD(Barbed_Shot) <= gcd) || API.SpellCharges(Barbed_Shot) == 2 || API.SpellCDDuration(Bestial_Wrath) < 1200 + gcd && Talent_ScentOfBlood))
                {
                    API.CastSpell(Barbed_Shot);
                    return;
                }
                //actions.st +=/ aspect_of_the_wild,if= buff.aspect_of_the_wild.down & (cooldown.barbed_shot.charges < 1 | !azerite.primal_instincts.enabled)
                if (API.CanCast(Aspect_of_the_Wild) && (UseAspectoftheWild == "always" || (UseAspectoftheWild == "with Cooldowns" && IsCooldowns)) && !API.PlayerHasBuff(Aspect_of_the_Wild) && InRange && PlayerLevel >= 38)
                {
                    API.CastSpell(Aspect_of_the_Wild);
                    return;
                }
                //actions.st +=/ stampede,if= buff.aspect_of_the_wild.up & buff.bestial_wrath.up | target.time_to_die < 15
                if (API.CanCast(Stampede) && (UseStampede == "always" || (UseStampede == "with Cooldowns" && IsCooldowns)) && Talent_Stampede && IsCooldowns && (API.PlayerHasBuff(Aspect_of_the_Wild) && API.PlayerHasBuff(Bestial_Wrath) || API.TargetTimeToDie < 1500) && API.TargetRange <= 30)
                {
                    API.CastSpell(Stampede);
                    return;
                }
                //actions.st +=/ a_murder_of_crows
                if (API.CanCast(A_Murder_of_Crows) && (UseAMurderofCrows == "always" || (UseAMurderofCrows == "with Cooldowns" && IsCooldowns)) && Talent_AMurderOfCrows && InRange && API.PlayerFocus >= 30)
                {
                    API.CastSpell(A_Murder_of_Crows);
                    return;
                }
                //actions.st +=/ bestial_wrath,if= talent.scent_of_blood.enabled | talent.one_with_the_pack.enabled & buff.bestial_wrath.remains < gcd | buff.bestial_wrath.down & cooldown.aspect_of_the_wild.remains > 15 | target.time_to_die < 15 + gcd
                if (API.CanCast(Bestial_Wrath) && (UseBestialWrath == "always" || (UseBestialWrath == "with Cooldowns" && IsCooldowns)) && InRange && (Talent_ScentOfBlood || Talent_OnewiththePack && API.PlayerBuffTimeRemaining(Bestial_Wrath) < gcd || !API.PlayerHasBuff(Bestial_Wrath) && API.SpellCDDuration(Aspect_of_the_Wild) > 1500 || API.TargetTimeToDie < 1500 + gcd))
                {
                    API.CastSpell(Bestial_Wrath);
                    return;
                }
                //actions.st +=/ kill_command
                if (API.CanCast(Kill_Command) && API.PlayerLevel >= 10 && InRange && API.PlayerFocus >= 30)
                {
                    API.CastSpell(Kill_Command);
                    return;
                }
                //actions.st +=/ bag_of_tricks,if= buff.bestial_wrath.down | target.time_to_die < 5
                /*  if (API.CanCast(Bag_of_Tricks) && API.PlayerRaceName == "Vulpera" && InRange)
                  {
                      API.CastSpell(Bag_of_Tricks);
                      return;
                  }*/
                //actions.st +=/ chimaera_shot
                if (API.CanCast(Chimaera_Shot) && Talent_ChimaeraShot && InRange)
                {
                    API.CastSpell(Chimaera_Shot);
                    return;
                }
                //actions.st +=/ dire_beast
                if (API.CanCast(Dire_Beast) && Talent_DireBeast && InRange)
                {
                    API.CastSpell(Dire_Beast);
                    return;
                }
                //actions.st +=/ barbed_shot,if= talent.one_with_the_pack.enabled & charges_fractional > 1.5 | charges_fractional > 1.8 | cooldown.aspect_of_the_wild.remains < pet.main.buff.frenzy.duration - gcd & azerite.primal_instincts.enabled | target.time_to_die < 9
                if (API.CanCast(Barbed_Shot) && InRange && API.PlayerLevel >= 12 && (Talent_OnewiththePack && Barbed_Shot_Fractional > 150 || Barbed_Shot_Fractional > 180 || API.TargetTimeToDie < 900))
                {
                    API.CastSpell(Barbed_Shot);
                    return;
                }
                //actions.st +=/ barrage
                if (API.CanCast(Barrage) && InRange && Talent_Barrage)
                {
                    API.CastSpell(Barrage);
                    return;
                }
                //actions.st +=/ cobra_shot,if= (focus - cost + focus.regen * (cooldown.kill_command.remains - 1) > action.kill_command.cost | cooldown.kill_command.remains > 1 + gcd & cooldown.bestial_wrath.remains_guess > focus.time_to_max | buff.memory_of_lucid_dreams.up) & cooldown.kill_command.remains > 1 | target.time_to_die < 3
                if (API.CanCast(Cobra_Shot) && API.PlayerLevel >= 14 && API.PlayerLevel >= 14 && InRange && (API.PlayerFocus - 35 + RealFocusRegen * (API.SpellCDDuration(Kill_Command) - 100) > 30 || API.SpellCDDuration(Kill_Command) > 100 + gcd && API.SpellCDDuration("Bstial Wrath") / 2 > RealFocusTimeToMax) && API.SpellCDDuration(Kill_Command) > 100 || API.TargetTimeToDie < 300)
                {
                    API.CastSpell(Cobra_Shot);
                    return;
                }
                //actions.st +=/ barbed_shot,if= pet.main.buff.frenzy.duration - gcd > full_recharge_time
                if (API.CanCast(Barbed_Shot) && InRange && API.PlayerLevel >= 12 && API.SpellCharges(Barbed_Shot) == 1 && (API.PetBuffTimeRemaining(Frenzy) - gcd > API.SpellChargeCD(Barbed_Shot)))
                {
                    API.CastSpell(Barbed_Shot);
                    return;
                }

            }

            #endregion
            #region CLEAVE
            if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)
            {
                // //actions.st = kill_shot
                if (API.CanCast(Kill_Shot) && API.TargetHealthPercent < 20 && API.PlayerFocus >= 10 && InRange && PlayerLevel >= 42)
                {
                    API.CastSpell(Kill_Shot);
                    return;
                }
                if (API.CanCast(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent < 20) && API.PlayerFocus >= 10 && PlayerLevel >= 42)
                {
                    API.CastSpell(Kill_Shot + "MO");
                    return;
                }
                //actions.st +=/ bloodshed
                if (API.CanCast(Bloodshed) && (UseBloodshed == "always" || (UseBloodshed == "with Cooldowns" && IsCooldowns)) && Talent_Bloodshed && InRange)
                {
                    API.CastSpell(Bloodshed);
                    return;
                }
                //actions.cleave = barbed_shot,target_if = min:dot.barbed_shot.remains,if= pet.main.buff.frenzy.up & pet.main.buff.frenzy.remains <= gcd.max | cooldown.bestial_wrath.remains < 12 + gcd & talent.scent_of_blood.enabled
                if (API.CanCast(Barbed_Shot) && InRange && API.PlayerLevel >= 12 && (PetHasBuff(Frenzy) && API.PetBuffTimeRemaining(Frenzy) < 250 || API.SpellCDDuration(Bestial_Wrath) < 1200 + gcd && Talent_ScentOfBlood))
                {
                    API.CastSpell(Barbed_Shot);
                    return;
                }
                //actions.cleave +=/ multishot,if= gcd.max - pet.main.buff.beast_cleave.remains > 0.25
                if (API.CanCast(Multi_Shot) && 150 - API.PlayerBuffTimeRemaining(Beast_Cleave) > 25 && API.PlayerFocus >= 40 && InRange && PlayerLevel >= 32)
                {
                    API.CastSpell(Multi_Shot);
                    return;
                }
                //actions.cleave +=/ barbed_shot,target_if = min:dot.barbed_shot.remains,if= full_recharge_time < gcd.max & cooldown.bestial_wrath.remains
                if (API.CanCast(Barbed_Shot) && InRange && API.PlayerLevel >= 12 && API.SpellCharges(Barbed_Shot) == 1 && API.SpellChargeCD(Barbed_Shot) < 150)
                {
                    API.CastSpell(Barbed_Shot);
                    return;
                }
                //actions.cleave +=/ aspect_of_the_wild
                if (API.CanCast(Aspect_of_the_Wild) && (UseAspectoftheWild == "always" || (UseAspectoftheWild == "with Cooldowns" && IsCooldowns)) && !API.PlayerHasBuff(Aspect_of_the_Wild) && InRange && PlayerLevel >= 38)
                {
                    API.CastSpell(Aspect_of_the_Wild);
                    return;
                }
                //actions.cleave +=/ stampede,if= buff.aspect_of_the_wild.up & buff.bestial_wrath.up | target.time_to_die < 15
                if (API.CanCast(Stampede) && (UseAspectoftheWild == "on AOE" || UseAspectoftheWild == "always" || (UseAspectoftheWild == "with Cooldowns" && IsCooldowns)) && Talent_Stampede && IsCooldowns && (API.PlayerHasBuff(Aspect_of_the_Wild) && API.PlayerHasBuff(Bestial_Wrath) || API.TargetTimeToDie < 1500) && API.TargetRange <= 30)
                {
                    API.CastSpell(Stampede);
                    return;
                }
                //actions.cleave +=/ bestial_wrath,if= talent.scent_of_blood.enabled | cooldown.aspect_of_the_wild.remains_guess > 20 | talent.one_with_the_pack.enabled | target.time_to_die < 15
                if (API.CanCast(Bestial_Wrath) && (UseBestialWrath == "always" || (UseBestialWrath == "with Cooldowns" && IsCooldowns)) && InRange && (Talent_ScentOfBlood || Talent_OnewiththePack || API.SpellCDDuration(Aspect_of_the_Wild) > 2000 || API.TargetTimeToDie < 1500))
                {
                    API.CastSpell(Bestial_Wrath);
                    return;
                }
                //actions.cleave +=/ chimaera_shot
                if (API.CanCast(Chimaera_Shot) && Talent_ChimaeraShot && InRange)
                {
                    API.CastSpell(Chimaera_Shot);
                    return;
                }
                //actions.cleave +=/ a_murder_of_crows
                if (API.CanCast(A_Murder_of_Crows) && (UseAMurderofCrows == "always" || (UseAMurderofCrows == "with Cooldowns" && IsCooldowns)) && Talent_AMurderOfCrows && InRange && API.PlayerFocus >= 30)
                {
                    API.CastSpell(A_Murder_of_Crows);
                    return;
                }
                //actions.cleave +=/ barrage
                if (API.CanCast(Barrage) && InRange && Talent_Barrage)
                {
                    API.CastSpell(Barrage);
                    return;
                }
                //actions.cleave +=/ kill_command,if= active_enemies < 4 | !azerite.rapid_reload.enabled
                if (API.CanCast(Kill_Command) && API.PlayerLevel >= 10 && InRange && API.PlayerFocus >= 30)
                {
                    API.CastSpell(Kill_Command);
                    return;
                }
                //actions.cleave +=/ dire_beast
                if (API.CanCast(Dire_Beast) && Talent_DireBeast && InRange)
                {
                    API.CastSpell(Dire_Beast);
                    return;
                }
                //actions.cleave +=/ barbed_shot,target_if = min:dot.barbed_shot.remains,if= pet.main.buff.frenzy.down & (charges_fractional > 1.8 | buff.bestial_wrath.up) | cooldown.aspect_of_the_wild.remains < pet.main.buff.frenzy.duration - gcd & azerite.primal_instincts.enabled | charges_fractional > 1.4 | target.time_to_die < 9
                if (API.CanCast(Barbed_Shot) && InRange && API.PlayerLevel >= 12 && API.PetBuffTimeRemaining(Frenzy) == 0 && (Barbed_Shot_Fractional > 180 || API.PlayerHasBuff(Bestial_Wrath)) || Barbed_Shot_Fractional > 140 || API.TargetTimeToDie < 900)
                {
                    API.CastSpell(Barbed_Shot);
                    return;
                }
                //actions.cleave +=/ cobra_shot,if= cooldown.kill_command.remains > focus.time_to_max & (active_enemies < 3 | !azerite.rapid_reload.enabled)
                if (API.CanCast(Cobra_Shot) && API.PlayerBuffTimeRemaining(Beast_Cleave) > gcd && API.PlayerLevel >= 14 && InRange && API.PlayerFocus >= 35 && API.SpellCDDuration(Kill_Command) > RealFocusTimeToMax)
                {
                    API.CastSpell(Cobra_Shot);
                    return;
                }
            }
            #endregion
        }

        public override void OutOfCombatPulse()
        {

        }
    }
}    


    
