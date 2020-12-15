// Changelog
// v1.0 First release
// v1.1 frenzied regeneration fix
// v1.2 covenants added + cd managment
// v1.3 covenant update :-)
// v1.4 Racials and Trinkets
// v1.5 Mighty Bash added

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
        private string RavenousFrenzy = "Ravenous Frenzy";
        private string ConvoketheSpirits = "Convoke the Spirits";
        private string AdaptiveSwarm = "Adaptive Swarm";
        private string LoneProtection = "Lone Protection";
        private string LoneSpirit = "Lone Spirit";
        private string Soulshape = "Soulshape";
        private string MightyBash = "Mighty Bash";


        //Talents
        bool TalentBristlingFur => API.PlayerIsTalentSelected(1, 3);
        bool TalentRenewal => API.PlayerIsTalentSelected(2, 2);
        bool TalentBalanceAffinity => API.PlayerIsTalentSelected(3, 1);
        bool TalentMightyBash => API.PlayerIsTalentSelected(4, 1);
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
        bool IsBerserk => (UseBerserk == "with Cooldowns" && IsCooldowns || UseBerserk == "always");
        bool IsIncarnation => (UseIncarnation == "with Cooldowns" && IsCooldowns || UseIncarnation == "always");
        bool IsCovenant => (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE);
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isMelee;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isMelee;

        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseIncarnation => CDUsage[CombatRoutine.GetPropertyInt(Incarnation)];
        private string UseBerserk => CDUsage[CombatRoutine.GetPropertyInt(Berserk)];
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private int BarkskinLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Barkskin)];
        private int RenewalLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Renewal)];
        private int SurvivalInstinctsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(SurvivalInstincts)];
        private int FrenziedRegenerationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Ironfur)];
        private int BristlingFurLifePercent => percentListProp[CombatRoutine.GetPropertyInt(BristlingFur)];
        private int PulverizeLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Pulverize)];
        private int LoneProtectionLifePercent => percentListProp[CombatRoutine.GetPropertyInt(LoneProtection)];
        public override void Initialize()
        {
            CombatRoutine.Name = "Guardian Druid by smartie";
            API.WriteLog("Welcome to smartie`s Guardian Druid v1.5");

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
            CombatRoutine.AddSpell(MightyBash, "D1");
            CombatRoutine.AddSpell(RavenousFrenzy, "D1");
            CombatRoutine.AddSpell(ConvoketheSpirits, "D1");
            CombatRoutine.AddSpell(AdaptiveSwarm, "D1");
            CombatRoutine.AddSpell(LoneProtection, "D1");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

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
            CombatRoutine.AddBuff(RavenousFrenzy);
            CombatRoutine.AddBuff(LoneSpirit);
            CombatRoutine.AddBuff(Soulshape);

            //Debuff
            CombatRoutine.AddDebuff(Thrash);
            CombatRoutine.AddDebuff(Moonfire);
            CombatRoutine.AddDebuff(AdaptiveSwarm);

            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Incarnation, "Use " + Incarnation, CDUsage, "Use " + Incarnation + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Berserk, "Use " + Berserk, CDUsage, "Use " + Berserk + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("AutoForm", "AutoForm", true, "Will auto switch forms", "Generic");
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
            CombatRoutine.AddProp(Barkskin, Barkskin + " Life Percent", percentListProp, "Life percent at which" + Barkskin + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", percentListProp, "Life percent at which" + Renewal + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(SurvivalInstincts, SurvivalInstincts + " Life Percent", percentListProp, "Life percent at which" + SurvivalInstincts + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(FrenziedRegeneration, FrenziedRegeneration + " Life Percent", percentListProp, "Life percent at which" + FrenziedRegeneration + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Ironfur, Ironfur + " Life Percent", percentListProp, "Life percent at which" + Ironfur + "is used, set to 0 to disable", "Defense", 9);
            CombatRoutine.AddProp(BristlingFur, BristlingFur + " Life Percent", percentListProp, "Life percent at which" + BristlingFur + "is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(Pulverize, Pulverize + " Life Percent", percentListProp, "Life percent at which" + Pulverize + "is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(LoneProtection, LoneProtection + " Life Percent", percentListProp, "Life percent at which" + LoneProtection + "is used, set to 0 to disable", "Defense", 5);
        }
        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40)
                return;
            if (!API.PlayerIsMounted && !API.PlayerHasBuff(TravelForm))
            {
                if (isInterrupt && API.CanCast(SkullBash) && PlayerLevel >= 26 && isKickRange)
                {
                    API.CastSpell(SkullBash);
                    return;
                }
                if (API.CanCast(RacialSpell1) && isInterrupt && PlayerRaceSettings == "Tauren" && !API.PlayerIsMoving && isRacial && isMelee && API.SpellISOnCooldown(SkullBash))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.CanCast(MightyBash) && isInterrupt && TalentMightyBash && isMelee && API.SpellISOnCooldown(SkullBash))
                {
                    API.CastSpell(MightyBash);
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
                if (API.PlayerHealthPercent <= FrenziedRegenerationLifePercent && PlayerLevel >= 21 && API.PlayerRage >= 10 && API.CanCast(FrenziedRegeneration) && API.PlayerHasBuff(BearForm) && API.PlayerBuffTimeRemaining(FrenziedRegeneration) == 0)
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
                if (API.PlayerHealthPercent <= LoneProtectionLifePercent && API.CanCast(LoneProtection) && isMelee && API.PlayerHasBuff(LoneSpirit) && PlayerCovenantSettings == "Kyrian")
                {
                    API.CastSpell(LoneProtection);
                    return;
                }
                rotation();
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40)
                return;
            if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !API.PlayerHasBuff(TravelForm))
            {
                API.CastSpell(TravelForm);
                return;
            }
        }
        private void rotation()
        {
            if ((!API.PlayerHasBuff(BearForm) && PlayerLevel >= 8) && !API.PlayerHasBuff(CatForm) && !API.PlayerHasBuff(Soulshape) && AutoForm)
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
                //actions.cooldown +=/ berserking,if= buff.tigers_fury.up | buff.bs_inc.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && isRacial && IsCooldowns && isMelee &&  IncaBerserk)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
                {
                    API.CastSpell("Trinket2");
                }
                if (API.CanCast(Incarnation) && TalentIncarnation && isMelee && IsIncarnation)
                {
                    API.CastSpell(Incarnation);
                    return;
                }
                if (API.CanCast(Berserk) && !TalentIncarnation && isMelee && IsBerserk)
                {
                    API.CastSpell(Berserk);
                    return;
                }
                if (API.CanCast(RavenousFrenzy) && isMelee && IncaBerserk && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                {
                    API.CastSpell(RavenousFrenzy);
                    return;
                }
                //actions.cooldown+=/convoke_the_spirits,if=(dot.rip.remains>4&(buff.tigers_fury.down|buff.tigers_fury.remains<4)&combo_points=0&dot.thrash_cat.ticking&dot.rake.ticking)|fight_remains<5
                if (API.CanCast(ConvoketheSpirits) && isMelee && !API.PlayerIsMoving && PlayerCovenantSettings == "Night Fae" && IsCovenant)
                {
                    API.CastSpell(ConvoketheSpirits);
                    return;
                }
                if (API.CanCast(AdaptiveSwarm) && isMelee && PlayerCovenantSettings == "Necrolord" && IsCovenant && !API.TargetHasDebuff(AdaptiveSwarm))
                {
                    API.CastSpell(AdaptiveSwarm);
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

