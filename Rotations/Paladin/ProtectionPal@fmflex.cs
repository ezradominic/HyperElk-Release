
namespace HyperElk.Core
{
    public class ProtectionPal : CombatRoutine
    {

        //Spells,Buffs,Debuffs
        private string CrusaderStrike = "Crusader Strike";
        private string Judgment = "Judgment";
        private string AvengersShield = "Avenger's Shield";
        private string HammeroftheRighteous = "Hammer of the Righteous";
        private string Consecration = "Consecration";
        private string ShieldoftheRighteous = "Shield of the Righteous";
        private string AvengingWrath = "Avenging Wrath";
        private string HammerofWrath = "Hammer of Wrath";
        private string Rebuke = "Rebuke";

        private string WordOfGlory = "Word of Glory";
        private string FlashofLight = "Flash of Light";
        private string LayOnHands = "Lay on Hands";
        private string ArdentDefender = "Ardent Defender";
        private string GuardianofAncientKings = "Guardian of Ancient Kings";
        private string DivineShield = "Divine Shield";

        private string CrusaderAura = "Crusader Aura";
        private string DevotionAura = "Devotion Aura";

        private string DivinePurpose = "DivinePurpose";
        private string Forearance = "Forearance";

        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;

        private bool HasDefenseBuff => API.PlayerHasBuff(ArdentDefender, false, false) || API.PlayerHasBuff(GuardianofAncientKings, false, false) || API.PlayerHasBuff(DivineShield, false, false);

        //Talents
        private bool CrusaderJudgment = API.PlayerIsTalentSelected(2, 2);

        //CBProperties


        private int FlashofLightLifePercent => percentListProp[CombatRoutine.GetPropertyInt("FOLOOCPCT")];
        private bool FLashofLightOutofCombat => CombatRoutine.GetPropertyBool("FOLOOC");
        private int WordOfGloryLifePercent => percentListProp[CombatRoutine.GetPropertyInt("WOGPCT")];
        private bool AutoAuraSwitch => CombatRoutine.GetPropertyBool("AURASWITCH");
        private bool IsAvengingWrath => CombatRoutine.GetPropertyBool(AvengingWrath);

        private int LayOnHandsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(LayOnHands)];
        private int ArdentDefenderLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ArdentDefender)];
        private int DivineShieldLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DivineShield)];
        private int GuardianofAncientKingsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(GuardianofAncientKings)];


        public override void Initialize()
        {
            CombatRoutine.Name = "ProtectionPal@fmflex";
            API.WriteLog("Welcome to Paladin leveling rotation @ FmFlex");

            //Spells
            CombatRoutine.AddSpell(CrusaderStrike, "D1");
            CombatRoutine.AddSpell(Judgment, "D2");
            CombatRoutine.AddSpell(AvengersShield, "D2");
            CombatRoutine.AddSpell(HammeroftheRighteous, "D1");
            CombatRoutine.AddSpell(Consecration, "D4");
            CombatRoutine.AddSpell(ShieldoftheRighteous, "D3");
            CombatRoutine.AddSpell(WordOfGlory, "F7");
            CombatRoutine.AddSpell(FlashofLight, "Q");
            CombatRoutine.AddSpell(Rebuke, "F");

            CombatRoutine.AddSpell(CrusaderAura, "F5");
            CombatRoutine.AddSpell(DevotionAura, "F6");

            CombatRoutine.AddSpell(AvengingWrath, "F");
            CombatRoutine.AddSpell(HammerofWrath, "D7");

            CombatRoutine.AddSpell(LayOnHands, "F8");
            CombatRoutine.AddSpell(ArdentDefender, "S");
            CombatRoutine.AddSpell(GuardianofAncientKings, "F9");
            CombatRoutine.AddSpell(DivineShield, "F10");

            //Buffs
            CombatRoutine.AddBuff(Consecration);
            CombatRoutine.AddBuff(CrusaderAura);
            CombatRoutine.AddBuff(DevotionAura);
            CombatRoutine.AddBuff(AvengingWrath);
            CombatRoutine.AddBuff(ShieldoftheRighteous);
            CombatRoutine.AddBuff(DivinePurpose);
            CombatRoutine.AddBuff(ArdentDefender);
            CombatRoutine.AddBuff(GuardianofAncientKings);
            CombatRoutine.AddBuff(DivineShield);

            //Debuffs
            CombatRoutine.AddDebuff(Forearance);


            //CBProperties
            CombatRoutine.AddProp("FOLOOCPCT", "Out of combat Life Percent", percentListProp, "Life percent at which Flash of Light is used out of combat to heal you between pulls", FlashofLight, 7);
            CombatRoutine.AddProp("FOLOOC", "Out of Combat Healing", true, "Should the bot use Flash of Light out of combat to heal you between pulls", FlashofLight);



            CombatRoutine.AddProp("AURASWITCH", "Auto Aura Switch", true, "Auto Switch Aura between Crusader Aura|Devotion Aura", "Generic");
            CombatRoutine.AddProp(AvengingWrath, "Use Avenging Wrath", true, "Use Avenging Wrath with cooldowns", "Generic");
            CombatRoutine.AddProp("AOEUnitNumner", "AOE Unit Numner", 2, "How many units around to use AOE rotation", "Generic");


            CombatRoutine.AddProp(LayOnHands, LayOnHands + " Life Percent", percentListProp, "Life percent at which" + LayOnHands + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(ArdentDefender, ArdentDefender + " Life Percent", percentListProp, "Life percent at which" + ArdentDefender + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(DivineShield, DivineShield + " Life Percent", percentListProp, "Life percent at which" + DivineShield + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(GuardianofAncientKings, GuardianofAncientKings + " Life Percent", percentListProp, "Life percent at which" + GuardianofAncientKings + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp("WOGPCT", "Life Percent", percentListProp, "Life percent at which Word of Glory is used", "Defense", 5);


        }

        public override void Pulse()
        {
            if (API.PlayerIsMounted)
            {
                if (AutoAuraSwitch && API.CanCast(CrusaderAura) && PlayerLevel >= 21 && !API.PlayerHasBuff(CrusaderAura))
                {
                    API.CastSpell(CrusaderAura);
                    return;
                }
            }
            else
            {

                if (AutoAuraSwitch && API.CanCast(DevotionAura) && PlayerLevel >= 21 && !API.PlayerHasBuff(DevotionAura))
                {
                    API.CastSpell(DevotionAura);
                    return;
                }


            }

        }

        public override void CombatPulse()
        {
            if (isInterrupt && API.CanCast(Rebuke) && IsMelee && PlayerLevel >= 27)
            {
                API.CastSpell(Rebuke);
                return;
            }
            if (IsCooldowns)
            {
                if (API.PlayerHealthPercent <= LayOnHandsLifePercent && API.CanCast(LayOnHands) && PlayerLevel >= 9 && !API.PlayerHasDebuff(Forearance, false, false))
                {
                    API.CastSpell(LayOnHands);
                    return;
                }
                if (IsAvengingWrath && API.CanCast(AvengingWrath) && PlayerLevel >= 37)
                {
                    API.CastSpell(AvengingWrath);
                    return;
                }
                if (API.PlayerHealthPercent <= DivineShieldLifePercent && API.CanCast(DivineShield) && PlayerLevel >= 10 && !HasDefenseBuff && !API.PlayerHasDebuff(Forearance, false, false))
                {
                    API.CastSpell(DivineShield);
                    return;
                }
                if (API.PlayerHealthPercent <= GuardianofAncientKingsLifePercent && API.CanCast(GuardianofAncientKings) && PlayerLevel >= 39 && !HasDefenseBuff)
                {
                    API.CastSpell(GuardianofAncientKings);
                    return;
                }
                if (API.PlayerHealthPercent <= ArdentDefenderLifePercent && API.CanCast(ArdentDefender) && PlayerLevel >= 42 && !HasDefenseBuff)
                {
                    API.CastSpell(ArdentDefender);
                    return;
                }
            }
            rotation();
            return;
        }

        public override void OutOfCombatPulse()
        {
            if (FLashofLightOutofCombat && API.PlayerHealthPercent <= FlashofLightLifePercent && !API.PlayerIsMoving && API.CanCast(FlashofLight) && PlayerLevel >= 4)
            {
                API.CastSpell(FlashofLight);
                return;
            }
        }
        private void rotation()
        {
            if (API.CanCast(Consecration) && IsMelee && !API.PlayerHasBuff(Consecration) && PlayerLevel >= 14)
            {
                API.CastSpell(Consecration);
                return;
            }
            if (API.PlayerHealthPercent <= WordOfGloryLifePercent && API.CanCast(WordOfGlory,true,true) && PlayerLevel >= 7)
            {
                API.CastSpell(WordOfGlory);
                return;
            }
            if (API.PlayerHealthPercent > WordOfGloryLifePercent && API.CanCast(ShieldoftheRighteous,true) && IsMelee
                && (API.PlayerBuffTimeRemaining(ShieldoftheRighteous) <=950 || API.PlayerCurrentHolyPower >= 4 || API.PlayerHasBuff(DivinePurpose)) 
                && PlayerLevel >= 2)
            {
                API.CastSpell(ShieldoftheRighteous);
                return;
            }
            //AOE AVENGERS SHIELD
            if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)
            {
                if (API.CanCast(AvengersShield) && API.TargetRange <= 30 && PlayerLevel >= 10)
                {
                    API.CastSpell(AvengersShield);
                    return;
                }
            }
            if (API.CanCast(Judgment) && API.TargetRange <= 30 && PlayerLevel >= 3)
            {
                API.CastSpell(Judgment);
                return;
            }
            if (API.TargetHealthPercent <= 20 && API.CanCast(HammerofWrath) && API.TargetRange <= 30 && PlayerLevel >= 46)
            {
                API.CastSpell(HammerofWrath);
                return;
            }
            if (API.CanCast(AvengersShield) && API.TargetRange <= 30 && PlayerLevel >= 10)
            {
                API.CastSpell(AvengersShield);
                return;
            }

            if (API.CanCast(HammeroftheRighteous) && IsMelee && PlayerLevel >= 14)
            {
                API.CastSpell(HammeroftheRighteous);
                return;
            }
            if (API.CanCast(CrusaderStrike) && IsMelee && PlayerLevel < 14)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }

            if (API.CanCast(Consecration) && IsMelee && PlayerLevel >= 6)
            {
                API.CastSpell(Consecration);
                return;
            }




        }

    }
}