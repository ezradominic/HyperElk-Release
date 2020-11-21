
namespace HyperElk.Core
{
    public class RetributionPal : CombatRoutine
    {


        //Spells,Buffs,Debuffs
        private string CrusaderStrike = "Crusader Strike";
        private string Judgment = "Judgment";
        private string WakeofAshes = "Wake of Ashes";
        private string BladeofJustice = "Blade of Justice";
        private string Consecration = "Consecration";
        private string TemplarsVerdict = "Templar's Verdict";
        private string AvengingWrath = "Avenging Wrath";
        private string HammerofWrath = "Hammer of Wrath";
        private string DivineStorm = "Divine Storm";

        private string FinalReckoning = "Final Reckoning";
        private string Seraphim = "Seraphim";
        private string ExecutionSentence = "Execution Sentence";
        private string Crusade = "Crusade";
        private string HolyAvenger = "Holy Avenger";
        private string Rebuke = "Rebuke";

        private string WordOfGlory = "Word of Glory";
        private string FlashofLight = "Flash of Light";
        private string LayOnHands = "Lay on Hands";

        private string DivineShield = "Divine Shield";
        private string ShieldofVengeance = "Shield of Vengeance";

        private string CrusaderAura = "Crusader Aura";
        private string DevotionAura = "Devotion Aura";

        private string DivinePurpose = "DivinePurpose";
        private string EmpyreanPower = "Empyrean Power";
        private string Forearance = "Forearance";
        private string SelflessHealer = "Selfless Healer";

        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;

        private bool HasDefenseBuff => API.PlayerHasBuff(ShieldofVengeance, false, false) || API.PlayerHasBuff(DivineShield, false, false);

        //Talents
        private bool TalentHolyAvenger = API.PlayerIsTalentSelected(5, 2);

        //CBProperties


        private int FlashofLightLifePercent => percentListProp[CombatRoutine.GetPropertyInt("FOLOOCPCT")];
        private bool FLashofLightOutofCombat => CombatRoutine.GetPropertyBool("FOLOOC");

        private bool AutoAuraSwitch => CombatRoutine.GetPropertyBool("AURASWITCH");

        private int LayOnHandsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(LayOnHands)];
        private int ShieldofVengeanceLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ShieldofVengeance)];
        private int DivineShieldLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DivineShield)];
        private int WordOfGloryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(WordOfGlory)];
        private int FlashofLightLifePercentProc => percentListProp[CombatRoutine.GetPropertyInt(FlashofLight)];


        public override void Initialize()
        {
            CombatRoutine.Name = "RetributionPal@fmflex";
            API.WriteLog("Welcome to Paladin Retribution rotation @ FmFlex");
            API.WriteLog("Macro required: /cast [@player] Final Reckoning");


            //Spells
            CombatRoutine.AddSpell(CrusaderStrike, "D1");
            CombatRoutine.AddSpell(Judgment, "D2");
            CombatRoutine.AddSpell(WakeofAshes, "D2");
            CombatRoutine.AddSpell(BladeofJustice, "D1");
            CombatRoutine.AddSpell(Consecration, "D4");
            CombatRoutine.AddSpell(TemplarsVerdict, "D3");
            CombatRoutine.AddSpell(DivineStorm, "D3");
            CombatRoutine.AddSpell(FinalReckoning, "D3");
            CombatRoutine.AddSpell(Seraphim, "D3");
            CombatRoutine.AddSpell(ExecutionSentence, "D3");
            CombatRoutine.AddSpell(Crusade, "D3");
            CombatRoutine.AddSpell(HolyAvenger, "D3");

            CombatRoutine.AddSpell(Rebuke, "F");

            CombatRoutine.AddSpell(CrusaderAura, "F5");
            CombatRoutine.AddSpell(DevotionAura, "F6");

            CombatRoutine.AddSpell(AvengingWrath, "F");
            CombatRoutine.AddSpell(HammerofWrath, "D7");

            CombatRoutine.AddSpell(LayOnHands, "F8");
            CombatRoutine.AddSpell(ShieldofVengeance, "S");
            CombatRoutine.AddSpell(DivineShield, "F10");
            CombatRoutine.AddSpell(FlashofLight, "Q");
            CombatRoutine.AddSpell(WordOfGlory, "F7");

            //Buffs
            CombatRoutine.AddBuff(Consecration);
            CombatRoutine.AddBuff(CrusaderAura);
            CombatRoutine.AddBuff(DevotionAura);
            CombatRoutine.AddBuff(AvengingWrath);
            CombatRoutine.AddBuff(Crusade);
            CombatRoutine.AddBuff(EmpyreanPower);
            CombatRoutine.AddBuff(DivinePurpose);
            CombatRoutine.AddBuff(ShieldofVengeance);
            CombatRoutine.AddBuff(DivineShield);
            CombatRoutine.AddBuff(SelflessHealer);

            //Debuffs
            CombatRoutine.AddDebuff(Forearance);
            CombatRoutine.AddDebuff(Judgment);
            CombatRoutine.AddDebuff(ExecutionSentence);
            CombatRoutine.AddDebuff(FinalReckoning);



            //CBProperties
            CombatRoutine.AddProp("FOLOOCPCT", "Out of combat Life Percent", percentListProp, "Life percent at which Flash of Light is used out of combat to heal you between pulls", FlashofLight, 7);
            CombatRoutine.AddProp("FOLOOC", "Out of Combat Healing", true, "Should the bot use Flash of Light out of combat to heal you between pulls", FlashofLight);
            CombatRoutine.AddProp(FlashofLight, "Selfless Healer Life Percent", percentListProp, "Life percent at which " + FlashofLight + " is used with selfless healer procs, set to 0 to disable", FlashofLight, 5);



            CombatRoutine.AddProp("AURASWITCH", "Auto Aura Switch", true, "Auto Switch Aura between Crusader Aura|Devotion Aura", "Generic");

            CombatRoutine.AddProp(LayOnHands, LayOnHands + " Life Percent", percentListProp, "Life percent at which" + LayOnHands + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(ShieldofVengeance, ShieldofVengeance + " Life Percent", percentListProp, "Life percent at which" + ShieldofVengeance + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(DivineShield, DivineShield + " Life Percent", percentListProp, "Life percent at which" + DivineShield + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(WordOfGlory, WordOfGlory + " Life Percent", percentListProp, "Life percent at which Word of Glory is used, set to 0 to disable", "Defense", 4);




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
                if (API.PlayerHealthPercent <= FlashofLightLifePercentProc && API.CanCast(FlashofLight) && API.PlayerBuffStacks(SelflessHealer) >= 4 && PlayerLevel >= 4)
                {
                    API.CastSpell(FlashofLight);
                    return;
                }
            }

        }
        public override void CombatPulse()
        {
            if (isInterrupt && !API.SpellISOnCooldown(Rebuke) && IsMelee && PlayerLevel >= 27)
            {
                API.CastSpell(Rebuke);
                return;
            }
            if (IsCooldowns)
            {
                if (API.PlayerHealthPercent <= LayOnHandsLifePercent && !API.SpellISOnCooldown(LayOnHands) && PlayerLevel >= 9 && !API.PlayerHasDebuff(Forearance, false, false))
                {
                    API.CastSpell(LayOnHands);
                    return;
                }

                if (!API.SpellISOnCooldown(AvengingWrath) && API.PlayerCurrentHolyPower >= 3 && PlayerLevel >= 37)
                {
                    API.CastSpell(AvengingWrath);
                    return;
                }
                if (TalentHolyAvenger && !API.SpellISOnCooldown(HolyAvenger) && API.PlayerCurrentHolyPower == 0)
                {
                    API.CastSpell(HolyAvenger);
                    return;
                }
            }
            if (API.PlayerHealthPercent <= DivineShieldLifePercent && !API.SpellISOnCooldown(DivineShield) && PlayerLevel >= 10 && !HasDefenseBuff && !API.PlayerHasDebuff(Forearance, false, false))
            {
                API.CastSpell(DivineShield);
                return;
            }
            if (API.PlayerHealthPercent <= ShieldofVengeanceLifePercent && !API.SpellISOnCooldown(ShieldofVengeance) && PlayerLevel >= 26 && !HasDefenseBuff)
            {
                API.CastSpell(ShieldofVengeance);
                return;
            }

            if (API.SpellIsCanbeCast(WordOfGlory) && API.PlayerHealthPercent <= WordOfGloryLifePercent && !API.SpellISOnCooldown(WordOfGlory) && PlayerLevel >= 7)
            {
                API.CastSpell(WordOfGlory);
                return;
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



            if (API.PlayerIsTalentSelected(5, 3) && API.SpellIsCanbeCast(Seraphim) && !API.SpellISOnCooldown(Seraphim) && IsMelee &&
                 (API.PlayerHasBuff(AvengingWrath) || API.SpellCDDuration(AvengingWrath) >= 2500 ||
                 API.PlayerBuffStacks(Crusade) >= 7 || (!API.PlayerHasBuff(Crusade) && API.SpellCDDuration(Crusade) >= 2500)) &&
                 (!API.PlayerIsTalentSelected(7, 3) || (API.PlayerIsTalentSelected(7, 3) && API.SpellCDDuration(FinalReckoning) <= 2 * API.SpellGCDTotalDuration)) &&
                 (!API.PlayerIsTalentSelected(1, 3) || (API.PlayerIsTalentSelected(1, 3) && API.SpellCDDuration(ExecutionSentence) <= 3 * API.SpellGCDTotalDuration)))
            {
                API.CastSpell(Seraphim);
                return;
            }

            if (API.PlayerIsTalentSelected(7, 3) && !API.SpellISOnCooldown(FinalReckoning) && API.TargetRange < 30 && API.PlayerCurrentHolyPower >= 3
                && (API.PlayerHasBuff(AvengingWrath) || API.SpellCDDuration(AvengingWrath) >= 1000) && IsMelee &&
                (!API.PlayerIsTalentSelected(1, 3) || (API.PlayerIsTalentSelected(1, 3) && API.SpellCDDuration(ExecutionSentence) <= 2 * API.SpellGCDTotalDuration)))
            {
                API.CastSpell(FinalReckoning);
                return;
            }
            if (API.PlayerIsTalentSelected(1, 3) && API.SpellIsCanbeCast(ExecutionSentence) && !API.SpellISOnCooldown(ExecutionSentence) && API.TargetRange < 30 &&
               (API.PlayerHasBuff(AvengingWrath) || API.SpellCDDuration(AvengingWrath) >= 2500 || API.PlayerHasBuff(Crusade) || API.SpellCDDuration(Crusade) >= 2500))
            {
                API.CastSpell(ExecutionSentence);
                return;
            }

            if (!API.SpellISOnCooldown(TemplarsVerdict) && API.PlayerCurrentHolyPower >= 5 && IsMelee && PlayerLevel >= 10)
            {
                if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && PlayerLevel >= 23)
                    API.CastSpell(DivineStorm);
                else
                    API.CastSpell(TemplarsVerdict);
                return;
            }

            if (!API.SpellISOnCooldown(WakeofAshes) && IsMelee &&
                (API.PlayerCurrentHolyPower == 0 || (API.PlayerCurrentHolyPower <= 2 && API.SpellCDDuration(BladeofJustice) > 2 * API.SpellGCDTotalDuration)) &&
                (!API.PlayerIsTalentSelected(7, 3) || (API.PlayerIsTalentSelected(7, 3) && API.TargetHasDebuff(FinalReckoning))) &&
                (!API.PlayerIsTalentSelected(1, 3) || (API.PlayerIsTalentSelected(1, 3) && API.TargetHasDebuff(ExecutionSentence)))
                && PlayerLevel >= 39)
            {
                API.CastSpell(WakeofAshes);
                return;
            }

            if (!API.SpellISOnCooldown(BladeofJustice) && API.TargetRange <= 12 && API.PlayerCurrentHolyPower <= 3 && PlayerLevel >= 19)
            {
                API.CastSpell(BladeofJustice);
                return;
            }

            if (API.TargetHealthPercent <= 20 && !API.SpellISOnCooldown(HammerofWrath) && API.TargetRange <= 30 && PlayerLevel >= 46)
            {
                API.CastSpell(HammerofWrath);
                return;
            }

            if (!API.SpellISOnCooldown(Judgment) &&
                (API.PlayerCurrentHolyPower < 4 || (API.PlayerCurrentHolyPower == 4 && API.SpellCDDuration(BladeofJustice) >= 2 * API.SpellGCDTotalDuration))
                && !API.TargetHasDebuff(Judgment) && API.TargetRange <= 30 && PlayerLevel >= 3)
            {
                API.CastSpell(Judgment);
                return;
            }
            if (!API.SpellISOnCooldown(DivineStorm) && API.PlayerHasBuff(EmpyreanPower) && !API.PlayerHasBuff(DivinePurpose) &&
                !((API.PlayerUnitInMeleeRangeCount <= AOEUnitNumber || !IsAOE) && API.TargetHasDebuff(Judgment))
                && IsMelee && PlayerLevel >= 23)
            {
                API.CastSpell(DivineStorm);
                return;
            }

            if (!API.SpellISOnCooldown(TemplarsVerdict) && API.SpellIsCanbeCast(TemplarsVerdict) &&
                (API.PlayerHasBuff(AvengingWrath) || API.PlayerHasBuff(Crusade) || API.TargetHealthPercent <= 20 || API.PlayerHasBuff(DivinePurpose))
                && IsMelee && PlayerLevel >= 10)
            {
                if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && PlayerLevel >= 23)
                    API.CastSpell(DivineStorm);
                else
                    API.CastSpell(TemplarsVerdict);
                return;

            }

            if (!API.SpellISOnCooldown(CrusaderStrike) &&
                 (API.PlayerCurrentHolyPower < 4 || (API.PlayerCurrentHolyPower == 3 && API.SpellCDDuration(BladeofJustice) >= 2 * API.SpellGCDTotalDuration)
                 || (API.PlayerCurrentHolyPower == 4 && API.SpellCDDuration(BladeofJustice) >= 2 * API.SpellGCDTotalDuration && (API.TargetHealthPercent > 20 || API.SpellCDDuration(HammerofWrath) >= 2 * API.SpellGCDTotalDuration) && API.SpellCDDuration(Judgment) >= 2 * API.SpellGCDTotalDuration))
                && API.SpellChargeCD(CrusaderStrike) >= 2 && IsMelee)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }
            if (!API.SpellISOnCooldown(TemplarsVerdict) && API.SpellIsCanbeCast(TemplarsVerdict) && API.PlayerCurrentHolyPower <= 4 && IsMelee && PlayerLevel >= 10)
            {
                if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && PlayerLevel >= 23)
                    API.CastSpell(DivineStorm);
                else
                    API.CastSpell(TemplarsVerdict);
                return;
            }
            if (!API.SpellISOnCooldown(CrusaderStrike) && API.PlayerCurrentHolyPower <= 4 && IsMelee)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }
            if (!API.SpellISOnCooldown(Consecration) &&
                (API.SpellCDDuration(BladeofJustice) >= API.SpellGCDTotalDuration && (API.TargetHealthPercent > 20 || API.SpellCDDuration(HammerofWrath) >= API.SpellGCDTotalDuration) && API.SpellCDDuration(Judgment) >= API.SpellGCDTotalDuration)
                && IsMelee && PlayerLevel >= 6)
            {
                API.CastSpell(Consecration);
                return;
            }




        }

    }
}