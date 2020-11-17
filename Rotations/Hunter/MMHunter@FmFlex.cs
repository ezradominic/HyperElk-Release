
namespace HyperElk.Core
{
    public class MMHunter : CombatRoutine
    {
        //Spells,Buffs,Debuffs
        private string SteadyShot = "Steady Shot";
        private string ArcaneShot = "Arcane Shot";

        private string AimedShot = "Aimed Shot";
        private string TrueShot = "TrueShot";
        private string RapidFire = "Rapid Fire";
        private string BurstingShot = "Bursting Shot";


        private string MendPet = "Mend Pet";
        private string KillShot = "Kill Shot";
        private string MultiShot = "Multi-Shot";
        private string Misdirection = "Misdirection";


        private string Exhilaration = "Exhilaration";
        private string SurvivaloftheFittest = "Survival of the Fittest";
        private string CounterShot = "Counter Shot";

        private string DoubleTap = "Double Tap";
        private string ChimaeraShot = "Chimaera Shot";
        private string AMurderofCrows = "A Murder of Crows";
        private string Barrage = "Barrage";
        private string Volley = "Volley";
        private string ExplosiveShot = "Explosive Shot";
        private string SerpentSting = "Serpent Sting"; 



        private string AspectoftheTurtle = "Aspect of the Turtle";
        private string PreciseShots = "Precise Shots";
        private string TrickShots = "Trick Shots";
        private string SteadyFocus = "Steady Focus";
        private string LockandLoad = "Lock and Load";
        private string DeadEye = "Dead Eye";
        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 43;


        //Talents
        private bool TalentAMurderOfCrows => API.PlayerIsTalentSelected(1, 3);
        private bool TalentSerpentSting => API.PlayerIsTalentSelected(1, 2);
        private bool TalentCarefulAim => API.PlayerIsTalentSelected(2, 1);
        private bool TalentBarrage => API.PlayerIsTalentSelected(2, 2);
        private bool TalentExplosiveShot => API.PlayerIsTalentSelected(2, 3);
        private bool TalentChimaeraShot => API.PlayerIsTalentSelected(3, 3);
        private bool TalentSteadyFocus => API.PlayerIsTalentSelected(4, 1);
        private bool TalentDeadEye => API.PlayerIsTalentSelected(6, 2);
        private bool TalentDoubleTap => API.PlayerIsTalentSelected(6, 3);
        private bool TalentVolley => API.PlayerIsTalentSelected(7, 3);



        //CBProperties

        string[] MisdirectionList = new string[] { "Off", "On AOE", "On" };

        private int ExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration)];
        private int SurvivaloftheFittestLifePercent => percentListProp[CombatRoutine.GetPropertyInt(SurvivaloftheFittest)];
        private int MendPetLifePercent => percentListProp[CombatRoutine.GetPropertyInt(MendPet)];
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private bool isCallPet => CombatRoutine.GetPropertyBool("CallPet");

        private float FocusRegen => 10f * (1f + API.PlayerGetHaste);
        private float FocusTimeToMax => (API.PlayerMaxFocus - API.PlayerFocus) * 100f / FocusRegen;

        public override void Initialize()
        {
            CombatRoutine.Name = "MMHunter@fmflex";
            API.WriteLog("Welcome to MM Hunter rotation @ FmFlex");
            API.WriteLog("Misdirection Macro : / cast[@focus, help][@pet, nodead, exists] Misdirection ");


            //Spells
            CombatRoutine.AddSpell(SteadyShot, "D1");
            CombatRoutine.AddSpell(ArcaneShot, "D2");
            CombatRoutine.AddSpell(AimedShot, "D3");
            CombatRoutine.AddSpell(TrueShot, "Q");
            CombatRoutine.AddSpell(RapidFire, "Q");
            CombatRoutine.AddSpell(BurstingShot, "D7");

            CombatRoutine.AddSpell(KillShot, "D5");
            CombatRoutine.AddSpell(MultiShot, "D6");

            CombatRoutine.AddSpell(CounterShot, "F");
            CombatRoutine.AddSpell(Exhilaration, "F9");
            CombatRoutine.AddSpell(SurvivaloftheFittest, "F8");
            CombatRoutine.AddSpell(Misdirection, "D4");

            CombatRoutine.AddSpell(DoubleTap, "D1");
            CombatRoutine.AddSpell(ChimaeraShot, "D1");
            CombatRoutine.AddSpell(AMurderofCrows, "D1");
            CombatRoutine.AddSpell(Barrage, "D1");
            CombatRoutine.AddSpell(Volley, "D1");
            CombatRoutine.AddSpell(ExplosiveShot, "D1");
            CombatRoutine.AddSpell(SerpentSting, "D1");

            CombatRoutine.AddSpell(MendPet, "F5");
            //Buffs

            CombatRoutine.AddBuff(AspectoftheTurtle);
            CombatRoutine.AddBuff(Misdirection);
            CombatRoutine.AddBuff(PreciseShots);
            CombatRoutine.AddBuff(TrickShots);
            CombatRoutine.AddBuff(SteadyFocus);
            CombatRoutine.AddBuff(TrueShot);
            CombatRoutine.AddBuff(DoubleTap);
            CombatRoutine.AddBuff(LockandLoad);
            CombatRoutine.AddBuff(DeadEye);


            //Debuffs

            CombatRoutine.AddDebuff(SerpentSting);

            CombatRoutine.AddProp(Misdirection, "Use Misdirection", MisdirectionList, "Use " + Misdirection + "Off, On AOE, On", "Generic", 0);
            CombatRoutine.AddProp("CallPet", "Call/Ressurect Pet", false, "Should the rotation try to ressurect/call your Pet", "Generic");


            CombatRoutine.AddProp(SurvivaloftheFittest, SurvivaloftheFittest + " Life Percent", percentListProp, "Life percent at which" + SurvivaloftheFittest + "is used, set to 0 to disable", "Defense", 7);

            CombatRoutine.AddProp(Exhilaration, Exhilaration + " Life Percent", percentListProp, "Life percent at which" + Exhilaration + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(MendPet, MendPet + " Pet Life Percent", percentListProp, "Life percent at which" + MendPet + "is used, set to 0 to disable", "Defense", 6);
        }

        public override void Pulse()
        {
            if (API.PlayerIsMounted || API.PlayerIsCasting)
            {
                return;
            }
            if (isCallPet && !API.PlayerHasPet)
            {
                API.CastSpell(MendPet);
                return;
            }
            if (API.PlayerHasPet && API.PetHealthPercent <= MendPetLifePercent && API.CanCast(MendPet))
            {
                API.CastSpell(MendPet);
                return;
            }
        }
        public override void CombatPulse()
        {
            if (API.FocusHealthPercent>0 && API.CanCast(Misdirection) && !API.PlayerHasBuff(Misdirection) && PlayerLevel >= 21 && (isMisdirection == "On" || (isMisdirection == "On AOE" & IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)))
            {
                API.CastSpell(Misdirection);
                return;
            }

            if (API.CanCast(Exhilaration) && API.PlayerHealthPercent <= ExhilarationLifePercent && PlayerLevel >= 9)
            {
                API.CastSpell(Exhilaration);
                return;
            }
            if (API.CanCast(SurvivaloftheFittest) && API.PlayerHealthPercent <= SurvivaloftheFittestLifePercent && PlayerLevel >= 9)
            {
                API.CastSpell(Exhilaration);
                return;
            }

            if (!API.PlayerHasBuff(AspectoftheTurtle))
            {
                if (isInterrupt && API.CanCast(CounterShot) && InRange && PlayerLevel >= 18)
                {
                    API.CastSpell(CounterShot);
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

            if (!IsAOE || API.TargetUnitInRangeCount < AOEUnitNumber )
            {

                //st->add_action("steady_shot,if=talent.steady_focus.enabled&prev_gcd.1.steady_shot&buff.steady_focus.remains<5");
                if (TalentSteadyFocus &&  API.CanCast(SteadyShot) && API.LastSpellCastInGame == SteadyShot && API.PlayerHasBuff(SteadyFocus,false,false) 
                    && API.PlayerBuffTimeRemaining(SteadyFocus) < 500
                    && InRange && PlayerLevel < 10)
                {
                    API.CastSpell(SteadyShot);
                    return;
                }
                //st->add_action("kill_shot");
                if (API.TargetHealthPercent <= 20 && API.CanCast(KillShot) && InRange && PlayerLevel >= 42 && API.PlayerFocus >= 10)
                {
                    API.CastSpell(KillShot);
                    return;
                }
                //st->add_action("double_tap");
                if (API.CanCast(DoubleTap) && InRange && TalentDoubleTap)
                {
                    API.CastSpell(DoubleTap);
                    return;
                }
                //st->add_action("tar_trap,if=runeforge.soulforge_embers.equipped&tar_trap.remains<gcd&cooldown.flare.remains<gcd");
                //st->add_action("flare,if=tar_trap.up");
                //st->add_action("wild_spirits");
                //st->add_action("flayed_shot");
                //st->add_action("death_chakram,if=focus+cast_regen<focus.max");
                //st->add_action("explosive_shot");
                if (API.CanCast(ExplosiveShot) && InRange && TalentExplosiveShot)
                {
                    API.CastSpell(ExplosiveShot);
                    return;
                }
                //st->add_action("volley,if=buff.precise_shots.down|!talent.chimaera_shot.enabled");
                if (API.CanCast(Volley) && InRange && TalentVolley && (!API.PlayerHasBuff(PreciseShots, false, false) || !TalentChimaeraShot))
                {
                    API.CastSpell(Volley);
                    return;
                }
                //st->add_action("a_murder_of_crows");
                if (TalentAMurderOfCrows && API.CanCast(AMurderofCrows) && InRange && API.PlayerFocus >= 20)
                {
                    API.CastSpell(AMurderofCrows);
                    return;
                }
                //st->add_action("resonating_arrow,if=buff.precise_shots.down|!talent.chimaera_shot.enabled");
                //st->add_action("trueshot,if=buff.precise_shots.down|!talent.chimaera_shot.enabled");
                if (IsCooldowns && API.CanCast(TrueShot) && InRange && (!API.PlayerHasBuff(PreciseShots, false, false) || !TalentChimaeraShot))
                {
                    API.CastSpell(TrueShot);
                    return;
                }
                //st->add_action("aimed_shot,if=buff.precise_shots.down|(!talent.chimaera_shot.enabled|ca_active)&buff.trueshot.up|buff.trick_shots.remains>execute_time&(active_enemies>1|runeforge.serpentstalkers_trickery.equipped)");
                if (API.CanCast(AimedShot) && InRange && (API.PlayerHasBuff(LockandLoad) || !API.PlayerIsMoving) &&
                    (!API.PlayerHasBuff(PreciseShots, false, false) || ((!TalentChimaeraShot || TalentCarefulAim) && API.PlayerHasBuff(TrueShot)))
                    && API.PlayerFocus >= (API.PlayerHasBuff(LockandLoad) ? 0 : 35))
                {
                    API.CastSpell(AimedShot);
                    return;
                }

                //st->add_action("rapid_fire,if=buff.double_tap.down&focus+cast_regen<focus.max");
                if (API.CanCast(RapidFire) && InRange && !API.PlayerHasBuff(DoubleTap, false, false) && API.PlayerFocus + FocusRegen * 2 < API.PlayerMaxFocus)
                {
                    API.CastSpell(RapidFire);
                    return;
                }
                //st->add_action("chimaera_shot,if=(buff.precise_shots.up|focus>cost+action.aimed_shot.cost)&(buff.trueshot.down|active_enemies>1|!ca_active)");
                if (TalentChimaeraShot && API.CanCast(ChimaeraShot) && InRange && (API.PlayerHasBuff(PreciseShots) || API.PlayerFocus > (API.PlayerHasBuff(LockandLoad) ? 0 : 35)) &&
                    (!API.PlayerHasBuff(TrueShot,false,false) || IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber||!TalentCarefulAim))
                {
                    API.CastSpell(ChimaeraShot);
                    return;
                }
                //st->add_action("arcane_shot,if=(buff.precise_shots.up|focus>cost+action.aimed_shot.cost)&(buff.trueshot.down|!ca_active)");
                if (API.CanCast(ArcaneShot) && InRange && (API.PlayerHasBuff(PreciseShots) || API.PlayerFocus > 20 + (API.PlayerHasBuff(LockandLoad)?0:35) ) &&
                    (!API.PlayerHasBuff(TrueShot, false, false) || !TalentCarefulAim))
                {
                    API.CastSpell(ArcaneShot);
                    return;
                }
                //st->add_action("serpent_sting,target_if=min:remains,if=refreshable&target.time_to_die>duration");
                if (TalentSerpentSting && API.CanCast(SerpentSting) && API.PlayerFocus > 10 && InRange && !API.TargetHasDebuff(SerpentSting) && API.TargetTimeToDie > 1800)
                {
                    API.CastSpell(SerpentSting);
                    return;
                }
                //st->add_action("barrage,if=active_enemies>1");
                if (TalentBarrage && API.CanCast(Barrage) && InRange && IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber && API.PlayerFocus >= 30)
                {
                    API.CastSpell(Barrage);
                    return;
                }
                //st->add_action("steady_shot");
                if (API.CanCast(SteadyShot) && InRange && PlayerLevel < 10)
                {
                    API.CastSpell(SteadyShot);
                    return;
                }
            }
            else
            {
                //trickshots->add_action("double_tap,if=cooldown.aimed_shot.up|cooldown.rapid_fire.remains>cooldown.aimed_shot.remains");
                if (API.CanCast(DoubleTap) && InRange && TalentDoubleTap && (API.SpellCDDuration(AimedShot) <= API.SpellGCDTotalDuration || API.SpellCDDuration(RapidFire) > API.SpellCDDuration(AimedShot)))
                {
                    API.CastSpell(DoubleTap);
                    return;
                }
                //trickshots->add_action("tar_trap,if=runeforge.soulforge_embers.equipped&tar_trap.remains<gcd&cooldown.flare.remains<gcd");
                //trickshots->add_action("flare,if=tar_trap.up");
                //trickshots->add_action("explosive_shot");
                if (API.CanCast(ExplosiveShot) && InRange && TalentExplosiveShot)
                {
                    API.CastSpell(ExplosiveShot);
                    return;
                }
                //trickshots->add_action("wild_spirits");
                //trickshots->add_action("volley");
                if (API.CanCast(Volley) && InRange && TalentVolley)
                {
                    API.CastSpell(Volley);
                    return;
                }
                //trickshots->add_action("resonating_arrow");
                //trickshots->add_action("barrage");
                if (TalentBarrage && API.CanCast(Barrage) && InRange && API.PlayerFocus >= 30)
                {
                    API.CastSpell(Barrage);
                    return;
                }
                //trickshots->add_action("trueshot,if=cooldown.rapid_fire.remains|focus+action.rapid_fire.cast_regen>focus.max|target.time_to_die<15");
                if (API.CanCast(TrueShot) && InRange && (API.SpellCDDuration(RapidFire) <= API.SpellGCDTotalDuration ||
                    API.PlayerFocus +200 > API.PlayerMaxFocus || API.TargetTimeToDie < 150))
                {
                    API.CastSpell(TrueShot);
                    return;
                }
                //trickshots->add_action("aimed_shot,if=(buff.trick_shots.remains>=execute_time)&(buff.precise_shots.down|full_recharge_time<cast_time+gcd|buff.trueshot.up)");

                if (API.CanCast(AimedShot) && InRange && API.PlayerBuffTimeRemaining(TrickShots) >= API.TargetTimeToExec && (API.PlayerHasBuff(LockandLoad) || !API.PlayerIsMoving) &&
                 (!API.PlayerHasBuff(PreciseShots, false, false) || API.PlayerFocus + FocusRegen * (2 +API.SpellGCDTotalDuration/100) < API.PlayerMaxFocus|| API.PlayerHasBuff(TrueShot))
                 && API.PlayerFocus >= (API.PlayerHasBuff(LockandLoad) ? 0 : 35))
                {
                    API.CastSpell(AimedShot);
                    return;
                }//trickshots->add_action("death_chakram,if=focus+cast_regen<focus.max");
                 //trickshots->add_action("rapid_fire,if=(buff.trick_shots.remains>=execute_time)&buff.double_tap.down");
                if (API.CanCast(RapidFire) && InRange && !API.PlayerHasBuff(DoubleTap, false, false) && API.PlayerBuffTimeRemaining(TrickShots) >= API.TargetTimeToExec)
                {
                    API.CastSpell(RapidFire);
                    return;
                }
                //trickshots->add_action("multishot,if=buff.trick_shots.down|buff.precise_shots.up");
                if (API.CanCast(MultiShot) && InRange && API.PlayerFocus >= 20 && (!API.PlayerHasBuff(TrickShots) || API.PlayerHasBuff(PreciseShots)))
                {
                    API.CastSpell(MultiShot);
                    return;
                }
                //trickshots->add_action("kill_shot,if=buff.dead_eye.down");
                if (TalentDeadEye && API.TargetHealthPercent <= 20 && API.CanCast(KillShot) && InRange && PlayerLevel >= 42 && API.PlayerFocus >= 10 && API.PlayerHasBuff(DeadEye))
                {
                    API.CastSpell(KillShot);
                    return;
                }
                //trickshots->add_action("multishot,if=focus-cost+cast_regen>action.aimed_shot.cost");
                if (API.CanCast(MultiShot) && InRange && API.PlayerFocus >= 20 && (API.PlayerFocus -20 + FocusRegen *API.SpellGCDTotalDuration/100 > (API.PlayerHasBuff(LockandLoad) ? 0 : 35)))
                {
                    API.CastSpell(MultiShot);
                    return;
                }
                //trickshots->add_action("a_murder_of_crows");
                if (TalentAMurderOfCrows && API.CanCast(AMurderofCrows) && InRange && API.PlayerFocus >= 20)
                {
                    API.CastSpell(AMurderofCrows);
                    return;
                }
                // trickshots->add_action("flayed_shot");
                //trickshots->add_action("serpent_sting,target_if=min:dot.serpent_sting.remains,if=refreshable");
                if (TalentSerpentSting && API.CanCast(SerpentSting) && API.PlayerFocus > 10 && InRange && !API.TargetHasDebuff(SerpentSting) && API.TargetTimeToDie > 1800)
                {
                    API.CastSpell(SerpentSting);
                    return;
                }
                //trickshots->add_action("steady_shot");
                if (API.CanCast(SteadyShot) && InRange && PlayerLevel < 10)
                {
                    API.CastSpell(SteadyShot);
                    return;
                }
            }

        }


    }
}