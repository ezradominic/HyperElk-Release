
namespace HyperElk.Core
{
    public class MMHunter : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        //Spells,Buffs,Debuffs
        private string Steady_Shot = "Steady Shot";
        private string Arcane_Shot = "Arcane Shot";

        private string Aimed_Shot = "Aimed Shot";
        private string Trueshot = "Trueshot";
        private string Rapid_Fire = "Rapid Fire";
        private string Bursting_Shot = "Bursting Shot";


        private string Mend_Pet = "Mend Pet";
        private string Kill_Shot = "Kill Shot";
        private string Multi_Shot = "Multi-Shot";
        private string Misdirection = "Misdirection";


        private string Exhilaration = "Exhilaration";
        private string Survival_of_the_Fittest = "Survival of the Fittest";
        private string Feign_Death = "Feign Death";
        private string Counter_Shot = "Counter Shot";

        private string Double_Tap = "Double Tap";
        private string Chimaera_Shot = "Chimaera Shot";
        private string A_Murder_of_Crows = "A Murder of Crows";
        private string Barrage = "Barrage";
        private string Volley = "Volley";
        private string Explosive_Shot = "Explosive Shot";
        private string Serpent_Sting = "Serpent Sting";
        private string Wild_Spirits = "Wild Spirits";
        private string Resonating_Arrow = "Resonating Arrow";
        private string Flayed_Shot = "Flayed Shot";
        private string Death_Chakram = "Death Chakram";


        private string Aspect_of_the_Turtle = "Aspect of the Turtle";
        private string Precise_Shots = "Precise Shots";
        private string Trick_Shots = "Trick Shots";
        private string Steady_Focus = "Steady Focus";
        private string Lock_and_Load = "Lock and Load";
        private string Dead_Eye = "Dead Eye";
        private string FlayersMark = "Flayer's Mark";
        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 43;
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");


        //Talents
        private bool Talent_A_Murder_of_Crows => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_Serpent_Sting => API.PlayerIsTalentSelected(1, 2);
        private bool Talent_CarefulAim => API.PlayerIsTalentSelected(2, 1);
        private bool Talent_Barrage => API.PlayerIsTalentSelected(2, 2);
        private bool Talent_Explosive_Shot => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_Chimaera_Shot => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_Steady_Focus => API.PlayerIsTalentSelected(4, 1);
        private bool Talent_Dead_Eye => API.PlayerIsTalentSelected(6, 2);
        private bool Talent_Double_Tap => API.PlayerIsTalentSelected(6, 3);
        private bool Talent_Volley => API.PlayerIsTalentSelected(7, 3);



        //CBProperties
        string[] MisdirectionList = new string[] { "Off", "On AOE", "On" };
        string[] TrueshotList = new string[] { "always", "with Cooldowns" };
        string[] DoubleTapList = new string[] { "always", "with Cooldowns" };
        string[] AMurderofCrowsList = new string[] { "always", "with Cooldowns" };
        string[] VolleyList = new string[] { "always", "with Cooldowns", "On AOE", "never" };
        string[] BloodshedList = new string[] { "always", "with Cooldowns" };


        private int Survival_of_the_FittestLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Survival_of_the_Fittest)];
        private int ExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration)];
        private int PetExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration + "PET")];
        private int AspectoftheTurtleLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Aspect_of_the_Turtle)];
        private int FeignDeathLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feign_Death)];
        private int MendPetLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Mend_Pet)];
        private string UseMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private string UseDoubleTap => DoubleTapList[CombatRoutine.GetPropertyInt(Double_Tap)];
        private string UseTrueshot => TrueshotList[CombatRoutine.GetPropertyInt(Trueshot)];
        private string UseAMurderofCrows => AMurderofCrowsList[CombatRoutine.GetPropertyInt(A_Murder_of_Crows)];
        private string UseVolley => VolleyList[CombatRoutine.GetPropertyInt(Volley)];
        private bool UseCallPet => CombatRoutine.GetPropertyBool("CallPet");
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];

        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];

        private bool LastSpell(string spellname, int spellid)
        {
            return API.LastSpellCastInGame == spellname || API.PlayerCurrentCastSpellID == spellid;
        }
        private float FocusRegen => 10f * (1f + API.PlayerGetHaste);
        private float FocusTimeToMax => (API.PlayerMaxFocus - API.PlayerFocus) * 100f / FocusRegen;
        private float AimedShotCastTime => 250f / (1f + (API.PlayerGetHaste/ 100f));
        private float RapidFireChannelTime => 300f / (1f + (API.PlayerGetHaste / 100f));
        private float gcd => API.SpellGCDTotalDuration;
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private int ca_values => (Talent_CarefulAim ? 1 : 0) + (API.TargetHealthPercent > 70 ? 1 : 0);
        private bool ca_active => ca_values == 2;

        public override void Initialize()
        {
            CombatRoutine.Name = "Marksman Hunter by Vec";
            API.WriteLog("Welcome to Marksman Hunter Rotation");
            API.WriteLog("Misdirection Macro : /cast [@focus,help][help][@pet,exists] Misdirection");
            API.WriteLog("Mend Pet Macro (revive/call): /cast [mod]Revive Pet; [@pet,dead]Revive Pet; [nopet]Call Pet 1; Mend Pet");
            API.WriteLog("Kill Shot Mouseover - /cast [@mouseover] Kill Shot");


            //Spells
            CombatRoutine.AddSpell(Steady_Shot, "D1");
            CombatRoutine.AddSpell(Arcane_Shot, "D2");
            CombatRoutine.AddSpell(Aimed_Shot, "D3");
            CombatRoutine.AddSpell(Trueshot, "Q");
            CombatRoutine.AddSpell(Rapid_Fire, "Q");
            CombatRoutine.AddSpell(Bursting_Shot, "D7");

            CombatRoutine.AddSpell(Kill_Shot, "D5");
            CombatRoutine.AddSpell(Multi_Shot, "D6");

            CombatRoutine.AddSpell(Counter_Shot, "F");
            CombatRoutine.AddSpell(Exhilaration, "F9");
            CombatRoutine.AddSpell(Survival_of_the_Fittest, "F8");
            CombatRoutine.AddSpell(Misdirection, "D4");

            CombatRoutine.AddSpell(Double_Tap, "D1");
            CombatRoutine.AddSpell(Chimaera_Shot, "D1");
            CombatRoutine.AddSpell(A_Murder_of_Crows, "D1");
            CombatRoutine.AddSpell(Barrage, "D1");
            CombatRoutine.AddSpell(Volley, "D1");
            CombatRoutine.AddSpell(Explosive_Shot, "D1");
            CombatRoutine.AddSpell(Serpent_Sting, "D1");
            CombatRoutine.AddSpell(Feign_Death, "F2");
            CombatRoutine.AddSpell(Aspect_of_the_Turtle, "G");

            CombatRoutine.AddSpell(Mend_Pet, "F5");

            CombatRoutine.AddSpell(Wild_Spirits, "F10");
            CombatRoutine.AddSpell(Resonating_Arrow, "F10");
            CombatRoutine.AddSpell(Flayed_Shot, "F10");
            CombatRoutine.AddSpell(Death_Chakram, "F10");


            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            //Buffs

            CombatRoutine.AddBuff(Aspect_of_the_Turtle);
            CombatRoutine.AddBuff(Feign_Death);
            CombatRoutine.AddBuff(Misdirection);
            CombatRoutine.AddBuff(Precise_Shots);
            CombatRoutine.AddBuff(Trick_Shots);
            CombatRoutine.AddBuff(Steady_Focus);
            CombatRoutine.AddBuff(Trueshot);
            CombatRoutine.AddBuff(Double_Tap);
            CombatRoutine.AddBuff(Lock_and_Load);
            CombatRoutine.AddBuff(Dead_Eye);
            CombatRoutine.AddBuff(FlayersMark);


            //Debuffs

            CombatRoutine.AddDebuff(Serpent_Sting);

            //Macros
            CombatRoutine.AddMacro(Kill_Shot + "MO", "NumPad7");


            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", true, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");

            //Settings
            CombatRoutine.AddProp(Survival_of_the_Fittest,"Use " + Survival_of_the_Fittest + " below:", percentListProp, "Life percent at which " + Survival_of_the_Fittest + " is used, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(Misdirection, "Use Misdirection", MisdirectionList, "Use " + Misdirection + " Off, On AOE, On", "Generic", 0);
            CombatRoutine.AddProp(Trueshot, "Use " + Trueshot, TrueshotList, "Use " + Trueshot + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Double_Tap, "Use " + Double_Tap, DoubleTapList, "Use " + Double_Tap + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(A_Murder_of_Crows, "Use " + A_Murder_of_Crows, AMurderofCrowsList, "Use " + A_Murder_of_Crows + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Volley, "Use " + Volley, VolleyList, "Use " + Volley + " always, with Cooldowns, On AOE, never", "Cooldowns", 0);

            CombatRoutine.AddProp("CallPet", "Call/Ressurect Pet", false, "Should the rotation try to ressurect/call your Pet", "Pet");
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp(Exhilaration, "Use " + Exhilaration + " below:", percentListProp, "Life percent at which " + Exhilaration + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Exhilaration + "PET", "Use " + Exhilaration + " below:", percentListProp, "Life percent at which " + Exhilaration + " is used to heal your pet, set to 0 to disable", "Pet", 2);
            CombatRoutine.AddProp(Aspect_of_the_Turtle, "Use " + Aspect_of_the_Turtle + " below:", percentListProp, "Life percent at which " + Aspect_of_the_Turtle + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Feign_Death, "Use " + Feign_Death + " below:", percentListProp, "Life percent at which " + Feign_Death + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(Mend_Pet, "Use " + Mend_Pet + " below:", percentListProp, "Life percent at which " + Mend_Pet + " is used, set to 0 to disable", "Pet", 6);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Cooldowns", 0);
        }

        public override void Pulse()
        {
            if (API.PlayerIsMounted || API.PlayerIsCasting || API.PlayerHasBuff(Aspect_of_the_Turtle) || API.PlayerHasBuff(Feign_Death))
            {
                return;
            }
            if (UseCallPet && (!API.PlayerHasPet || API.PetHealthPercent <1))
            {
                API.CastSpell(Mend_Pet);
                return;
            }
            if (API.PlayerHasPet && API.PetHealthPercent >= 1 && API.PetHealthPercent <= MendPetLifePercent && API.CanCast(Mend_Pet))
            {
                API.CastSpell(Mend_Pet);
                return;
            }
        }
        public override void CombatPulse()
        {
            if (API.FocusHealthPercent > 0 && API.CanCast(Misdirection) && !API.PlayerHasBuff(Misdirection) && PlayerLevel >= 21 && (UseMisdirection == "On" || (UseMisdirection == "On AOE" & IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)))
            {
                API.CastSpell(Misdirection);
                return;
            }

            if (API.CanCast(Exhilaration) && API.PlayerHealthPercent <= ExhilarationLifePercent && PlayerLevel >= 9)
            {
                API.CastSpell(Exhilaration);
                return;
            }
            if (API.CanCast(Survival_of_the_Fittest) && API.PlayerHealthPercent <= Survival_of_the_FittestLifePercent && PlayerLevel >= 9)
            {
                API.CastSpell(Survival_of_the_Fittest);
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
            if (!API.PlayerHasBuff(Aspect_of_the_Turtle))
            {
                if (isInterrupt && API.CanCast(Counter_Shot) && InRange && PlayerLevel >= 18)
                {
                    API.CastSpell(Counter_Shot);
                    return;
                }
                if (!API.PlayerIsCasting)
                    rotation();
                return;
            }
        }

        public override void OutOfCombatPulse()
        {
        }


        private void rotation()
        {
           // API.WriteLog("trick shots up? " + API.PlayerHasBuff(Trick_Shots));
            if (!IsAOE || API.TargetUnitInRangeCount < AOEUnitNumber)
            {
                #region cooldowns
                #endregion
                #region ST
                //st->add_action("steady_shot,if=Talent_.steady_focus.enabled&prev_gcd.1.steady_shot&buff.steady_focus.remains<5");
                if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && (API.LastSpellCastInGame == Steady_Shot || API.PlayerCurrentCastSpellID == 56641) && (API.PlayerHasBuff(Steady_Focus, false, false)
                    && API.PlayerBuffTimeRemaining(Steady_Focus) < 500 || !PlayerHasBuff(Steady_Focus))
                    && InRange)
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
                //st->add_action("kill_shot");
                if ((API.TargetHealthPercent <= 20 || PlayerHasBuff(FlayersMark)) && API.CanCast(Kill_Shot) && InRange && PlayerLevel >= 42 && API.PlayerFocus >= 10)
                {
                    API.CastSpell(Kill_Shot);
                    return;
                }
                if (API.CanCast(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && (API.MouseoverHealthPercent <= 20 || API.MouseoverHasBuff(FlayersMark))) && API.PlayerFocus >= 10 && PlayerLevel >= 42)
                {
                    API.CastSpell(Kill_Shot + "MO");
                    return;
                }
                //st->add_action("double_tap");
                if (API.CanCast(Double_Tap) && (UseDoubleTap == "always" || (UseDoubleTap == "with Cooldowns" && IsCooldowns)) && InRange && Talent_Double_Tap)
                {
                    API.CastSpell(Double_Tap);
                    return;
                }
                //st->add_action("tar_trap,if=runeforge.soulforge_embers.equipped&tar_trap.remains<gcd&cooldown.flare.remains<gcd");
                //st->add_action("flare,if=tar_trap.up");
                //st->add_action("wild_spirits");
                if (!API.SpellISOnCooldown(Wild_Spirits) && PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Wild_Spirits);
                    return;
                }
                //st->add_action("flayed_shot");
                if (!API.SpellISOnCooldown(Flayed_Shot) && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Flayed_Shot);
                    return;
                }
                //st->add_action("death_chakram,if=focus+cast_regen<focus.max");
                if (!API.SpellISOnCooldown(Death_Chakram) && API.PlayerFocus + FocusRegen * gcd / 100 < API.PlayerMaxFocus && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Death_Chakram);
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
                //st->add_action("explosive_shot");
                if (API.CanCast(Explosive_Shot) && InRange && Talent_Explosive_Shot)
                {
                    API.CastSpell(Explosive_Shot);
                    return;
                }
                //st->add_action("volley,if=buff.precise_shots.down|!talent.chimaera_shot.enabled");
                if (API.CanCast(Volley) && UseVolley != "never" && (UseVolley == "always" || (UseVolley == "with Cooldowns" && IsCooldowns) || (UseVolley == "On AOE" && IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)) && InRange && Talent_Volley && (!API.PlayerHasBuff(Precise_Shots, false, false) || !Talent_Chimaera_Shot))
                {
                    API.CastSpell(Volley);
                    return;
                }
                //st->add_action("a_murder_of_crows");
                if (Talent_A_Murder_of_Crows && (UseAMurderofCrows == "always" || (UseAMurderofCrows == "with Cooldowns" && IsCooldowns)) && API.CanCast(A_Murder_of_Crows) && InRange && API.PlayerFocus >= 20)
                {
                    API.CastSpell(A_Murder_of_Crows);
                    return;
                }
                //st->add_action("resonating_arrow,if=buff.precise_shots.down|!talent.chimaera_shot.enabled");
                if (!API.SpellISOnCooldown(Resonating_Arrow) && PlayerCovenantSettings == "Kyrian" && (!PlayerHasBuff(Precise_Shots) || !Talent_Chimaera_Shot) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Resonating_Arrow);
                    return;
                }
                //st->add_action("Trueshot,if=buff.precise_shots.down|!talent.chimaera_shot.enabled");
                if (API.CanCast(Trueshot) && (UseTrueshot == "always" || (UseTrueshot == "with Cooldowns" && IsCooldowns)) && InRange && (!API.PlayerHasBuff(Precise_Shots, false, false) || !Talent_Chimaera_Shot))
                {
                    API.CastSpell(Trueshot);
                    return;
                }
                //st->add_action("aimed_shot,if=buff.precise_shots.down|(!talent.chimaera_shot.enabled|ca_active)&buff.Trueshot.up|buff.trick_shots.remains>execute_time&(active_enemies>1|runeforge.serpentstalkers_trickery.equipped)");
                if (API.CanCast(Aimed_Shot) && InRange && (API.PlayerHasBuff(Lock_and_Load) || !API.PlayerIsMoving) &&
                    ((!API.PlayerHasBuff(Precise_Shots, false, false)&& !LastSpell(Aimed_Shot, 19434) && !LastSpell(Rapid_Fire, 257044)) || ((!Talent_Chimaera_Shot || ca_active) && API.PlayerHasBuff(Trueshot)))
                    && API.PlayerFocus >= (API.PlayerHasBuff(Lock_and_Load) ? 0 : 35))
                {
                    API.CastSpell(Aimed_Shot);
                    return;
                }

                //st->add_action("rapid_fire,if=buff.double_tap.down&focus+cast_regen<focus.max");
                if (API.CanCast(Rapid_Fire) && InRange && !API.PlayerHasBuff(Double_Tap, false, false) && API.PlayerFocus + FocusRegen * 2 < API.PlayerMaxFocus)
                {
                    API.CastSpell(Rapid_Fire);
                    return;
                }
                //st->add_action("chimaera_shot,if=(buff.precise_shots.up|focus>cost+action.aimed_shot.cost)&(buff.Trueshot.down|active_enemies>1|!ca_active)");
                if (Talent_Chimaera_Shot && API.CanCast(Chimaera_Shot) && InRange && (API.PlayerHasBuff(Precise_Shots) || API.PlayerFocus > (API.PlayerHasBuff(Lock_and_Load) ? 0 : 35)) &&
                    (!API.PlayerHasBuff(Trueshot, false, false) || IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber || !ca_active))
                {
                    API.CastSpell(Chimaera_Shot);
                    return;
                }
                //st->add_action("arcane_shot,if=(buff.precise_shots.up|focus>cost+action.aimed_shot.cost)&(buff.Trueshot.down|!ca_active)");
                if (API.CanCast(Arcane_Shot) && API.PlayerFocus >=20 && InRange && (API.PlayerHasBuff(Precise_Shots) || API.PlayerFocus > 20 + (API.PlayerHasBuff(Lock_and_Load) ? 0 : 35)) &&
                    (!API.PlayerHasBuff(Trueshot, false, false) || !ca_active))
                {
                    API.CastSpell(Arcane_Shot);
                    return;
                }
                //st->add_action("serpent_sting,target_if=min:remains,if=refreshable&target.time_to_die>duration");
                if (Talent_Serpent_Sting && API.CanCast(Serpent_Sting) && API.PlayerFocus > 10 && InRange && !API.TargetHasDebuff(Serpent_Sting) && API.TargetTimeToDie > 1800)
                {
                    API.CastSpell(Serpent_Sting);
                    return;
                }
                //st->add_action("barrage,if=active_enemies>1");
                if (Talent_Barrage && API.CanCast(Barrage) && InRange && IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber && API.PlayerFocus >= 30)
                {
                    API.CastSpell(Barrage);
                    return;
                }
                //st->add_action("steady_shot");
                if (API.CanCast(Steady_Shot) && InRange)
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
            }
            #endregion
            else
            {
                //Trick_Shots->add_action("double_tap,if=cooldown.aimed_shot.up|cooldown.rapid_fire.remains>cooldown.aimed_shot.remains");
                if (API.CanCast(Double_Tap) && (UseDoubleTap == "always" || (UseDoubleTap == "with Cooldowns" && IsCooldowns)) && InRange && Talent_Double_Tap && (API.SpellISOnCooldown(Aimed_Shot) || API.SpellCDDuration(Rapid_Fire) > API.SpellCDDuration(Aimed_Shot)))
                {
                    API.CastSpell(Double_Tap);
                    return;
                }
                //Trick_Shots->add_action("tar_trap,if=runeforge.soulforge_embers.equipped&tar_trap.remains<gcd&cooldown.flare.remains<gcd");
                //Trick_Shots->add_action("flare,if=tar_trap.up");
                //Trick_Shots->add_action("explosive_shot");
                if (API.CanCast(Explosive_Shot) && InRange && Talent_Explosive_Shot)
                {
                    API.CastSpell(Explosive_Shot);
                    return;
                }
                //Trick_Shots->add_action("wild_spirits");
                if (!API.SpellISOnCooldown(Wild_Spirits) && PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Wild_Spirits);
                    return;
                }
                //Trick_Shots->add_action("volley");
                if (API.CanCast(Volley) && UseVolley != "never" && (UseVolley == "always" || (UseVolley == "with Cooldowns" && IsCooldowns) || UseVolley == "On AOE") && InRange && Talent_Volley)
                {
                    API.CastSpell(Volley);
                    return;
                }
                //Trick_Shots->add_action("resonating_arrow");
                if (!API.SpellISOnCooldown(Resonating_Arrow) && PlayerCovenantSettings == "Kyrian"  && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Resonating_Arrow);
                    return;
                }
                //Trick_Shots->add_action("barrage");
                if (Talent_Barrage && API.CanCast(Barrage) && InRange && API.PlayerFocus >= 30)
                {
                    API.CastSpell(Barrage);
                    return;
                }
                //Trick_Shots->add_action("Trueshot,if=cooldown.rapid_fire.remains|focus+action.rapid_fire.cast_regen>focus.max|target.time_to_die<15");
                if (API.CanCast(Trueshot) && (UseTrueshot == "always" || (UseTrueshot == "with Cooldowns" && IsCooldowns)) && InRange && (API.SpellCDDuration(Rapid_Fire) <= gcd||
                    API.PlayerFocus + 7 > API.PlayerMaxFocus || API.TargetTimeToDie < 1500))
                {
                    API.CastSpell(Trueshot);
                    return;
                }
                //Trick_Shots->add_action("aimed_shot,if=(buff.trick_shots.remains>=execute_time)&(buff.precise_shots.down|full_recharge_time<cast_time+gcd|buff.Trueshot.up)");

                if (API.CanCast(Aimed_Shot) && !LastSpell(Aimed_Shot, 19434) && !LastSpell(Rapid_Fire, 257044) && InRange && API.PlayerBuffTimeRemaining(Trick_Shots) >= AimedShotCastTime &&
                 (!API.PlayerHasBuff(Precise_Shots, false, false) || (API.SpellCharges(Aimed_Shot) >= 1 && API.SpellChargeCD(Aimed_Shot) < AimedShotCastTime+gcd)|| API.PlayerHasBuff(Trueshot))
                 && API.PlayerFocus >= (API.PlayerHasBuff(Lock_and_Load) ? 0 : 35) && (API.PlayerHasBuff(Lock_and_Load) || !API.PlayerIsMoving))
                {
                    API.CastSpell(Aimed_Shot);
                    return;
                }//Trick_Shots->add_action("death_chakram,if=focus+cast_regen<focus.max");
                if (!API.SpellISOnCooldown(Death_Chakram) && API.PlayerFocus + FocusRegen * gcd / 100 < API.PlayerMaxFocus && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Death_Chakram);
                    return;
                }
                //Trick_Shots->add_action("rapid_fire,if=(buff.trick_shots.remains>=execute_time)&buff.double_tap.down");
                if (API.CanCast(Rapid_Fire) && !LastSpell(Aimed_Shot, 19434) && !LastSpell(Rapid_Fire, 257044) && InRange && !API.PlayerHasBuff(Double_Tap, false, false) && API.PlayerBuffTimeRemaining(Trick_Shots) >= RapidFireChannelTime)
                {
                    API.CastSpell(Rapid_Fire);
                    return;
                }
                //Trick_Shots->add_action("Multi_Shot,if=buff.trick_shots.down|buff.precise_shots.up");
                if (API.CanCast(Multi_Shot) && InRange && API.PlayerFocus >= 20 && (!API.PlayerHasBuff(Trick_Shots) || API.PlayerHasBuff(Precise_Shots)))
                {
                    API.CastSpell(Multi_Shot);
                    return;
                }
                //Trick_Shots->add_action("kill_shot,if=buff.dead_eye.down");
                if (API.CanCast(Kill_Shot) && (API.TargetHealthPercent <= 20 || PlayerHasBuff(FlayersMark)) && InRange && PlayerLevel >= 42 && API.PlayerFocus >= 10 && !API.PlayerHasBuff(Dead_Eye))
                {
                    API.CastSpell(Kill_Shot);
                    return;
                }
                if (API.CanCast(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && (API.MouseoverHealthPercent <= 20 || API.MouseoverHasBuff(FlayersMark))) && API.PlayerFocus >= 10 && PlayerLevel >= 42 && !API.PlayerHasBuff(Dead_Eye))
                {
                    API.CastSpell(Kill_Shot + "MO");
                    return;
                }
                //Trick_Shots->add_action("Multi_Shot,if=focus-cost+cast_regen>action.aimed_shot.cost");
                if (API.CanCast(Multi_Shot) && InRange && API.PlayerFocus >= 20 && (API.PlayerFocus - 20 + FocusRegen * API.SpellGCDTotalDuration / 100 > (API.PlayerHasBuff(Lock_and_Load) ? 0 : 35)))
                {
                    API.CastSpell(Multi_Shot);
                    return;
                }
                //Trick_Shots->add_action("a_murder_of_crows");
                if (Talent_A_Murder_of_Crows && (UseAMurderofCrows == "always" || (UseAMurderofCrows == "with Cooldowns" && IsCooldowns)) && API.CanCast(A_Murder_of_Crows) && InRange && API.PlayerFocus >= 20)
                {
                    API.CastSpell(A_Murder_of_Crows);
                    return;
                }
                // Trick_Shots->add_action("flayed_shot");
                if (!API.SpellISOnCooldown(Flayed_Shot) && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Flayed_Shot);
                    return;
                }
                //Trick_Shots->add_action("serpent_sting,target_if=min:dot.serpent_sting.remains,if=refreshable");
                if (Talent_Serpent_Sting && API.CanCast(Serpent_Sting) && API.PlayerFocus > 10 && InRange && !API.TargetHasDebuff(Serpent_Sting) && API.TargetTimeToDie > 1800)
                {
                    API.CastSpell(Serpent_Sting);
                    return;
                }
                //Trick_Shots->add_action("steady_shot");
                if (API.CanCast(Steady_Shot) && InRange)
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
            }

        }


    }
}