// Changelog
// v1.0 First release

namespace HyperElk.Core
{
    public class GuardianDruid : CombatRoutine
    {
        //Spell,Auras
        private string Moonfire = "Moonfire";
        private string Swipe = "Swipe";
        private string Thrash = "Thrash";
        private string Mangle = "Mangle";
        private string Maul = "Maul";
        private string Ironfur = "Ironfur";
        private string BearForm = "Bear Form";
        private string TravelForm = "Travel Form";
        private string Barkskin = "Barkskin";
        private string SurvivalInstincts = "Survival Instincts";
        private string FrenziedRegeneration = "Frenzied Regeneration";
        private string BristlingFur = "Bristling Fur";
        private string Pulverize = "Pulverize";
        private string Incarnation = "Incarnation: Guardian of Ursoc";
        private string SkullBash = "Skull Bash";
        private string StampedingRoar = "Stampeding Roar";
        private string Typhoon = "Typhoon";
        private string GalacticGuardian = "Galactic Guardian";
        private string CatForm = "Cat Form";
        private string ToothandClaw = "Tooth and Claw";
        private string Berserk = "Berserk";
        private string Renewal = "Renewal";


        //Talents
        bool TalentBristlingFur => API.PlayerIsTalentSelected(1, 3);
        bool TalentRenewal => API.PlayerIsTalentSelected(2, 2);
        bool TalentBalanceAffinity => API.PlayerIsTalentSelected(3, 1);
        bool TalentSouloftheForest => API.PlayerIsTalentSelected(5, 1);
        bool TalentGalacticGuardian => API.PlayerIsTalentSelected(5, 2);
        bool TalentIncarnation => API.PlayerIsTalentSelected(5, 3);
        bool TalentPulverize => API.PlayerIsTalentSelected(7, 3);
        bool TalentRendandTear => API.PlayerIsTalentSelected(7, 1);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool isMelee => (TalentBalanceAffinity && API.TargetRange < 9 || !TalentBalanceAffinity && API.TargetRange < 6);

        private bool isThrashMelee => (TalentBalanceAffinity && API.TargetRange < 12 || !TalentBalanceAffinity && API.TargetRange < 9);

        private bool isKickRange => (TalentBalanceAffinity && API.TargetRange < 17 || !TalentBalanceAffinity && API.TargetRange < 14);

        private bool IncaBerserk => (API.PlayerHasBuff(Incarnation) || API.PlayerHasBuff(Berserk));
        //CBProperties
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private int BarkskinLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Barkskin)];
        private int RenewalLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Renewal)];
        private int SurvivalInstinctsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(SurvivalInstincts)];
        private int FrenziedRegenerationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Ironfur)];
        private int BristlingFurLifePercent => percentListProp[CombatRoutine.GetPropertyInt(BristlingFur)];
        private int PulverizeLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Pulverize)];
        public override void Initialize()
        {
            CombatRoutine.Name = "Guardian Druid by smartie";
            API.WriteLog("Welcome to smartie`s Guardian Druid v1.0");

            //Spells
            CombatRoutine.AddSpell(Moonfire, "D3");
            CombatRoutine.AddSpell(Swipe, "D5");
            CombatRoutine.AddSpell(Thrash, "D4");
            CombatRoutine.AddSpell(Renewal, "NumPad4");
            CombatRoutine.AddSpell(Mangle, "D6");
            CombatRoutine.AddSpell(Maul, "D7");
            CombatRoutine.AddSpell(Ironfur, "F6");
            CombatRoutine.AddSpell(BearForm, "NumPad1");
            CombatRoutine.AddSpell(TravelForm, "NumPad2");
            CombatRoutine.AddSpell(Barkskin, "F4");
            CombatRoutine.AddSpell(SurvivalInstincts, "F1");
            CombatRoutine.AddSpell(FrenziedRegeneration, "F5");
            CombatRoutine.AddSpell(BristlingFur, "D0");
            CombatRoutine.AddSpell(Pulverize, "D9");
            CombatRoutine.AddSpell(Incarnation, "D8");
            CombatRoutine.AddSpell(Berserk, "D8");
            CombatRoutine.AddSpell(SkullBash, "F12");
            CombatRoutine.AddSpell(StampedingRoar, "NumPad5");
            CombatRoutine.AddSpell(Typhoon, "F8");

            //Buffs
            CombatRoutine.AddBuff(GalacticGuardian);
            CombatRoutine.AddBuff(CatForm);
            CombatRoutine.AddBuff(BearForm);
            CombatRoutine.AddBuff(TravelForm);
            CombatRoutine.AddBuff(SurvivalInstincts);
            CombatRoutine.AddBuff(Barkskin);
            CombatRoutine.AddBuff(Ironfur);
            CombatRoutine.AddBuff(FrenziedRegeneration);
            CombatRoutine.AddBuff(Incarnation);
            CombatRoutine.AddBuff(Berserk);
            CombatRoutine.AddBuff(BristlingFur);
            CombatRoutine.AddBuff(Pulverize);
            CombatRoutine.AddBuff(ToothandClaw);

            //Debuff
            CombatRoutine.AddDebuff(Thrash);
            CombatRoutine.AddDebuff(Moonfire);

            //Prop
            CombatRoutine.AddProp("AutoForm", "AutoForm", true, "Will auto switch forms", "Generic");
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
            CombatRoutine.AddProp(Barkskin, Barkskin + " Life Percent", percentListProp, "Life percent at which" + Barkskin + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", percentListProp, "Life percent at which" + Renewal + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(SurvivalInstincts, SurvivalInstincts + " Life Percent", percentListProp, "Life percent at which" + SurvivalInstincts + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(FrenziedRegeneration, FrenziedRegeneration + " Life Percent", percentListProp, "Life percent at which" + FrenziedRegeneration + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Ironfur, Ironfur + " Life Percent", percentListProp, "Life percent at which" + Ironfur + "is used, set to 0 to disable", "Defense", 9);
            CombatRoutine.AddProp(BristlingFur, BristlingFur + " Life Percent", percentListProp, "Life percent at which" + BristlingFur + "is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(Pulverize, Pulverize + " Life Percent", percentListProp, "Life percent at which" + Pulverize + "is used, set to 0 to disable", "Defense", 5);
        }
        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if (API.PlayerIsCasting || API.PlayerIsChanneling)
                return;
            if (!API.PlayerIsMounted && !API.PlayerHasBuff(TravelForm))
            {
                if (isInterrupt && API.CanCast(SkullBash) && PlayerLevel >= 26 && isKickRange)
                {
                    API.CastSpell(SkullBash);
                    return;
                }
                if (API.PlayerHealthPercent <= BarkskinLifePercent && PlayerLevel >= 24 && API.CanCast(Barkskin))
                {
                    API.CastSpell(Barkskin);
                    return;
                }
                if (API.CanCast(SurvivalInstincts) && PlayerLevel >= 32 && API.PlayerHealthPercent <= SurvivalInstinctsLifePercent && !API.PlayerHasBuff(SurvivalInstincts))
                {
                    API.CastSpell(SurvivalInstincts);
                    return;
                }
                if (API.CanCast(Renewal) && API.PlayerHealthPercent <= RenewalLifePercent && TalentRenewal)
                {
                    API.CastSpell(Renewal);
                    return;
                }
                if (API.PlayerHealthPercent <= FrenziedRegenerationLifePercent && PlayerLevel >= 21 && API.PlayerRage >= 10 && API.CanCast(FrenziedRegeneration) && API.PlayerHasBuff(BearForm))
                {
                    API.CastSpell(FrenziedRegeneration);
                    return;
                }
                if (API.PlayerHealthPercent <= IronfurLifePercent && PlayerLevel >= 18 && (API.PlayerRage >= 40 || API.PlayerRage >= 20 && IncaBerserk) && API.CanCast(Ironfur) && API.PlayerHasBuff(BearForm))
                {
                    API.CastSpell(Ironfur);
                    return;
                }
                if (API.PlayerHealthPercent <= BristlingFurLifePercent && PlayerLevel >= 8 && API.CanCast(BristlingFur) && API.PlayerHasBuff(BearForm) && TalentBristlingFur)
                {
                    API.CastSpell(BristlingFur);
                    return;
                }
                if (API.PlayerHealthPercent <= PulverizeLifePercent && API.CanCast(Pulverize) && API.PlayerHasBuff(BearForm) && TalentPulverize && API.TargetBuffStacks(Thrash) >= 2)
                {
                    API.CastSpell(Pulverize);
                    return;
                }
                rotation();
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerIsCasting || API.PlayerIsChanneling)
                return;
            if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !API.PlayerHasBuff(TravelForm))
            {
                API.CastSpell(TravelForm);
                return;
            }
        }
        private void rotation()
        {
            if ((!API.PlayerHasBuff(BearForm) && PlayerLevel >= 8) && !API.PlayerHasBuff(CatForm) && AutoForm)
            {
                API.CastSpell(BearForm);
                return;
            }
            if (PlayerLevel < 8)
            {
                API.WriteLog("Your Current Level is:" + PlayerLevel);
                API.WriteLog("Rota will work once you are Level 8");
            }
            if (API.PlayerHasBuff(BearForm) && PlayerLevel >= 8)
            {
                if (API.CanCast(Incarnation) && TalentIncarnation && isMelee && IsCooldowns)
                {
                    API.CastSpell(Incarnation);
                    return;
                }
                if (API.CanCast(Berserk) && !TalentIncarnation && isMelee && IsCooldowns)
                {
                    API.CastSpell(Berserk);
                    return;
                }
                // Single Target rota
                if (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE)
                {
                    if (API.CanCast(Moonfire) && API.PlayerHasBuff(GalacticGuardian) && API.TargetRange < 40)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Thrash) && PlayerLevel >= 11 && isThrashMelee && (API.TargetDebuffRemainingTime(Thrash) < 250 || API.TargetDebuffStacks(Thrash) < 3))
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                    if (API.CanCast(Maul) && PlayerLevel >= 10 && IncaBerserk && API.PlayerRage >= 40 && isMelee)
                    {
                        API.CastSpell(Maul);
                        return;
                    }
                    if (API.CanCast(Mangle) && PlayerLevel >= 8 && API.PlayerHasBuff(Incarnation) && isMelee)
                    {
                        API.CastSpell(Mangle);
                        return;
                    }
                    if (API.CanCast(Moonfire) && API.TargetDebuffRemainingTime(Moonfire) < 200 && API.TargetRange < 40)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Maul) && PlayerLevel >= 10 && (API.PlayerBuffStacks(ToothandClaw) >= 2 || API.PlayerBuffTimeRemaining(ToothandClaw) < 150) && API.PlayerRage >= 40 && isMelee)
                    {
                        API.CastSpell(Maul);
                        return;
                    }
                    if (API.CanCast(Mangle) && PlayerLevel >= 8 && (API.PlayerRage < 90 || API.PlayerRage < 85 && TalentSouloftheForest) && isMelee)
                    {
                        API.CastSpell(Mangle);
                        return;
                    }
                    if (API.CanCast(Thrash) && PlayerLevel >= 11 && isThrashMelee)
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                    if (API.CanCast(Maul) && PlayerLevel >= 10 && API.PlayerRage >= 40 && isMelee)
                    {
                        API.CastSpell(Maul);
                        return;
                    }
                    if (API.CanCast(Swipe) && PlayerLevel >= 36 && isThrashMelee)
                    {
                        API.CastSpell(Swipe);
                        return;
                    }
                }
                //AoE rota
                if (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE)
                {
                    if (API.CanCast(Moonfire) && API.PlayerHasBuff(GalacticGuardian) && API.TargetRange < 40 && API.PlayerUnitInMeleeRangeCount < 3)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Thrash) && PlayerLevel >= 11 && isThrashMelee && (API.TargetDebuffRemainingTime(Thrash) < 250 || API.TargetDebuffStacks(Thrash) < 3 || API.PlayerUnitInMeleeRangeCount > 4))
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                    if (API.CanCast(Swipe) && PlayerLevel >= 36 && isThrashMelee && !IncaBerserk && API.PlayerUnitInMeleeRangeCount >= 4)
                    {
                        API.CastSpell(Swipe);
                        return;
                    }
                    if (API.CanCast(Maul) && PlayerLevel >= 10 && API.PlayerRage >= 40 && isMelee && API.PlayerHasBuff(ToothandClaw) && API.PlayerUnitInMeleeRangeCount == 2)
                    {
                        API.CastSpell(Maul);
                        return;
                    }
                    if (API.CanCast(Mangle) && PlayerLevel >= 8 && API.PlayerHasBuff(Incarnation) && isMelee && API.PlayerUnitInMeleeRangeCount <= 3)
                    {
                        API.CastSpell(Mangle);
                        return;
                    }
                    if (API.CanCast(Moonfire) && API.TargetDebuffRemainingTime(Moonfire) < 200 && API.TargetRange < 40 && API.PlayerUnitInMeleeRangeCount <= 3)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Maul) && PlayerLevel >= 10 && (API.PlayerBuffStacks(ToothandClaw) >= 2 || API.PlayerBuffTimeRemaining(ToothandClaw) < 150) && API.PlayerRage >= 40 && isMelee && API.PlayerUnitInMeleeRangeCount < 3)
                    {
                        API.CastSpell(Maul);
                        return;
                    }
                    if (API.CanCast(Thrash) && PlayerLevel >= 11 && isThrashMelee)
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                    if (API.CanCast(Mangle) && PlayerLevel >= 8 && (API.PlayerRage < 90 || API.PlayerRage < 85 && TalentSouloftheForest) && API.PlayerUnitInMeleeRangeCount < 3 && isMelee)
                    {
                        API.CastSpell(Mangle);
                        return;
                    }
                    if (API.CanCast(Swipe) && PlayerLevel >= 36 && isThrashMelee)
                    {
                        API.CastSpell(Swipe);
                        return;
                    }
                }
            }
        }
    }
}

