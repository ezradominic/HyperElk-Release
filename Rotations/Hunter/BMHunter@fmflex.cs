
namespace HyperElk.Core
{
    public class BMHunter : CombatRoutine
    {
        //Spells,Buffs,Debuffs
        private string SteadyShot = "Steady Shot";
        private string ArcaneShot = "Arcane Shot";
        private string KillCommand = "Kill Command";
        private string BarbedShot = "Barbed Shot";
        private string CobraShot = "Cobra Shot";
        private string MendPet = "Mend Pet";
        private string BestialWrath = "Bestial Wrath";
        private string AspectoftheWild = "Aspect of the Wild";
        private string KillShot = "Kill Shot";
        private string MultiShot = " Multi-Shot";
        private string Misdirection = "Misdirection";


        private string Exhilaration = "Exhilaration";
        private string CounterShot = "Counter Shot";

        private string DireBeast = "Dire Beast";
        private string ChimaeraShot = "Chimaera Shot";
        private string AMurderofCrows = "A Murder of Crows";
        private string Barrage = "Barrage";
        private string Stampede = "Stampede";
        private string Bloodshed = "Bloodshed";

        private string Frenzy = "Frenzy";
        private string BeastCleave = "Beast Cleave";
        private string AspectoftheTurtle = "Aspect of the Turtle";

        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool InRange => API.TargetRange < 40;


        //Talents
        private bool TalentDireBeast => API.PlayerIsTalentSelected(1, 3);
        private bool TalentScentOfBlood => API.PlayerIsTalentSelected(2, 1);
        private bool TalentChimaeraShot => API.PlayerIsTalentSelected(2, 3);
        private bool TalentAMurderOfCrows => API.PlayerIsTalentSelected(4, 3);
        private bool TalentBarrage => API.PlayerIsTalentSelected(6, 2);
        private bool TalentStampede => API.PlayerIsTalentSelected(6, 3);
        private bool TalentBloodshed => API.PlayerIsTalentSelected(7, 3);
        //CBProperties

        string[] MisdirectionList = new string[] { "Off", "On AOE", "On" };

        private int ExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration)];
        private int PetExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration + "PET")];
        private int MendPetLifePercent => percentListProp[CombatRoutine.GetPropertyInt(MendPet)];
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private bool isCallPet => CombatRoutine.GetPropertyBool("CallPet");

        private float FocusRegen => 10f * (1f + API.PlayerGetHaste);
        private float FocusTimeToMax => (API.PlayerMaxFocus - API.PlayerFocus) * 100f / FocusRegen;

        public override void Initialize()
        {
            CombatRoutine.Name = "BMHunter@fmflex";
            API.WriteLog("Welcome to BM Hunter rotation @ FmFlex");
            API.WriteLog("Misdirection Macro : / cast[@focus, help][@pet, nodead, exists] Misdirection ");
            API.WriteLog("Mend Pet Macro (revive/call): /cast [mod]Revive Pet; [@pet,dead]Revive Pet; [nopet]Call Pet 1; Mend Pet");

            //Spells
            CombatRoutine.AddSpell(SteadyShot, "D1");
            CombatRoutine.AddSpell(ArcaneShot, "D2");
            CombatRoutine.AddSpell(KillCommand, "D1");
            CombatRoutine.AddSpell(BarbedShot, "D3");
            CombatRoutine.AddSpell(MendPet, "F5");
            CombatRoutine.AddSpell(CobraShot, "D2");
            CombatRoutine.AddSpell(BestialWrath, "Q");
            CombatRoutine.AddSpell(AspectoftheWild, "F7");
            CombatRoutine.AddSpell(KillShot, "D5");
            CombatRoutine.AddSpell(MultiShot, "D6");

            CombatRoutine.AddSpell(CounterShot, "F");
            CombatRoutine.AddSpell(Exhilaration, "F9");
            CombatRoutine.AddSpell(Misdirection, "D4");

            CombatRoutine.AddSpell(DireBeast, "D1");
            CombatRoutine.AddSpell(ChimaeraShot, "D1");
            CombatRoutine.AddSpell(AMurderofCrows, "D1");
            CombatRoutine.AddSpell(Barrage, "D1");
            CombatRoutine.AddSpell(Stampede, "D1");
            CombatRoutine.AddSpell(Bloodshed, "D1");

            //Buffs
            CombatRoutine.AddBuff(Frenzy);
            CombatRoutine.AddBuff(BeastCleave);
            CombatRoutine.AddBuff(AspectoftheTurtle);
            CombatRoutine.AddBuff(Misdirection);
            CombatRoutine.AddBuff(BestialWrath);

            //Debuffs



            CombatRoutine.AddProp(Misdirection, "Use Misdirection", MisdirectionList, "Use " + Misdirection + "Off, On AOE, On", "Generic", 0);
            CombatRoutine.AddProp("CallPet", "Call/Ressurect Pet", true, "Should the rotation try to ressurect/call your Pet", "Generic");



            CombatRoutine.AddProp(Exhilaration, Exhilaration + " Life Percent", percentListProp, "Life percent at which" + Exhilaration + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Exhilaration + "PET", Exhilaration + "  Pet Life Percent", percentListProp, "Life percent at which" + Exhilaration + "is used to heal your pet, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(MendPet, MendPet + " Pet Life Percent", percentListProp, "Life percent at which" + MendPet + "is used, set to 0 to disable", "Defense", 6);
        }

        public override void Pulse()
        {
            if (API.PlayerIsMounted || API.PlayerIsCasting)
            {
                return;
            }
            if (!API.PlayerHasPet)
            {
                API.CastSpell(MendPet);
                return;
            }
            if (API.PlayerHasPet && API.PetHealthPercent <= MendPetLifePercent && !API.SpellISOnCooldown(MendPet))
            {
                API.CastSpell(MendPet);
                return;
            }
        }
        public override void CombatPulse()
        {
            if (API.PlayerHasPet && !API.SpellISOnCooldown(Misdirection) && !API.PlayerHasBuff(Misdirection) && PlayerLevel >= 21 && (isMisdirection == "On" || (isMisdirection == "On AOE" & IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)))
            {
                API.CastSpell(Misdirection);
                return;
            }
            if (IsCooldowns)
            {
                if (!API.SpellISOnCooldown(Exhilaration) && ((API.PlayerHealthPercent <= ExhilarationLifePercent && PlayerLevel >= 9) || (API.PetHealthPercent <= PetExhilarationLifePercent && PlayerLevel >= 44)))
                {
                    API.CastSpell(Exhilaration);
                    return;
                }
            }
            if (!API.PlayerHasBuff(AspectoftheTurtle))
            {
                if (isInterrupt && API.TargetCanInterrupted && API.TargetIsCasting && API.TargetCurrentCastTimeRemaining < interruptDelay && !API.SpellISOnCooldown(CounterShot) && InRange && PlayerLevel >= 18)
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
            if (API.PlayerHasPet && !API.SpellISOnCooldown(BarbedShot) &&
                (!API.PetHasBuff(Frenzy) ||
                API.SpellCharges(BarbedShot) >= 2 ||
                (API.SpellChargeCD(BarbedShot) <= API.SpellGCDTotalDuration && API.SpellCharges(BarbedShot) >= 1) ||
                (API.SpellCharges(BarbedShot) > 0 && TalentScentOfBlood && IsCooldowns && !API.SpellISOnCooldown(BestialWrath)))
                && InRange && PlayerLevel >= 12)
            {
                API.CastSpell(BarbedShot);
                return;
            }
            if (IsAOE && API.PlayerHasPet && API.TargetUnitInRangeCount >= AOEUnitNumber && API.TargetUnitInRangeCount >= 3)
            {
                if (API.SpellGCDTotalDuration - API.PlayerDebuffRemainingTime(BeastCleave) > 25 && !API.SpellISOnCooldown(MultiShot) && API.PlayerFocus >= 40 && InRange && PlayerLevel >= 32)
                {
                    API.CastSpell(MultiShot);
                    return;
                }
            }
            if (API.PlayerHasPet)
            {
                if (IsCooldowns && !API.SpellISOnCooldown(BestialWrath) && InRange && PlayerLevel >= 20 && PlayerLevel < 38)
                {
                    API.CastSpell(BestialWrath);
                    return;
                }
                if (!IsCooldowns && API.SpellISOnCooldown(BestialWrath) && (API.SpellCDDuration(AspectoftheWild) > 1500 || !API.SpellISOnCooldown(AspectoftheWild)) && InRange && PlayerLevel >= 38)
                {
                    API.CastSpell(BestialWrath);
                    return;
                }
                if (TalentBloodshed && !API.SpellISOnCooldown(Bloodshed) && (API.SpellCDDuration(BestialWrath) >= 5000|| !IsCooldowns || API.PlayerHasBuff(BestialWrath)) && InRange)
                {
                    API.CastSpell(Bloodshed);
                    return;
                }
                if (IsCooldowns && !API.SpellISOnCooldown(AspectoftheWild) && (API.SpellCDDuration(BestialWrath) < 500 || API.PlayerHasBuff(BestialWrath)) && InRange && PlayerLevel >= 38)
                {
                    API.CastSpell(AspectoftheWild);
                    return;
                }
                if (TalentStampede && IsCooldowns && !API.SpellISOnCooldown(Stampede) && API.TargetRange <= 30)
                {
                    if ((API.PlayerHasBuff(AspectoftheWild) && API.PlayerHasBuff(BestialWrath)) || API.TargetTimeToDie < 1500)
                    {
                        API.CastSpell(Stampede);
                        return;
                    }
                }
            }
            if (API.TargetHealthPercent <= 20 && !API.SpellISOnCooldown(KillShot) && API.PlayerFocus >= 10 && InRange && PlayerLevel >= 42)
            {
                API.CastSpell(KillShot);
                return;
            }
            if (API.PlayerHasPet && !API.SpellISOnCooldown(KillCommand) && API.PlayerFocus >= 30 && InRange && PlayerLevel >= 10)
            {
                API.CastSpell(KillCommand);
                return;
            }

            if (TalentChimaeraShot && !API.SpellISOnCooldown(ChimaeraShot) && API.PlayerFocus <=60 && InRange)
            {
                API.CastSpell(ChimaeraShot);
                return;
            }
            if (TalentDireBeast && !API.SpellISOnCooldown(DireBeast) && InRange)
            {
                API.CastSpell(DireBeast);
                return;
            }
            if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)
            {
                if (API.SpellCDDuration(KillCommand) >= 200 && !API.SpellISOnCooldown(MultiShot) && API.PlayerFocus >= 40 && InRange && PlayerLevel >= 23 && PlayerLevel < 32)
                {
                    API.CastSpell(MultiShot);
                    return;
                }
            }
            if (!API.SpellISOnCooldown(BarbedShot) &&
                API.PetBuffTimeRemaining(Frenzy) - API.SpellGCDTotalDuration > API.SpellChargeCD(BarbedShot)
                && InRange && PlayerLevel >= 12)
            {
                API.CastSpell(BarbedShot);
                return;
            }
            if (TalentBarrage && !API.SpellISOnCooldown(Barrage) && InRange)
            {
                API.CastSpell(Barrage);
                return;
            }
            //if=(focus-cost+focus.regen*(cooldown.kill_command.remains-1)>action.kill_command.cost|cooldown.kill_command.remains>1+gcd&cooldown.bestial_wrath.remains_guess>focus.time_to_max|buff.memory_of_lucid_dreams.up)&cooldown.kill_command.remains>1|target.time_to_die<3" );
            if (((((API.PlayerFocus - 35 + FocusRegen * (API.SpellCDDuration(KillCommand) - 100)) > 30 || (API.SpellCDDuration(KillCommand) > 100 + API.SpellGCDTotalDuration && API.SpellCDDuration(BestialWrath) / 2 > (int)FocusTimeToMax)) 
                && API.SpellCDDuration(KillCommand) > 100)
                    || API.TargetTimeToDie < 300)
                    && API.PlayerFocus >= 35
                    && (!IsAOE || API.TargetUnitInRangeCount <= AOEUnitNumber)
                && InRange && PlayerLevel >= 14)
            {
                API.CastSpell(CobraShot);
                return;
            }

            if (!API.SpellISOnCooldown(ArcaneShot) && API.PlayerFocus >= 70 && InRange && PlayerLevel >= 2 && PlayerLevel < 14)
            {
                API.CastSpell(ArcaneShot);
                return;
            }
            if (!API.SpellISOnCooldown(SteadyShot) && API.SpellCDDuration(KillCommand) >= 250 && InRange && PlayerLevel < 10)
            {
                API.CastSpell(SteadyShot);
                return;
            }

        }


    }
}