
namespace HyperElk.Core
{
    public class RetributionPaladin : CombatRoutine
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

        private string DivinePurpose = "Divine Purpose";
        private string EmpyreanPower = "Empyrean Power";
        private string Forearance = "Forearance";
        private string SelflessHealer = "Selfless Healer";
        private string VanquishersHammer = "Vanquisher's Hammer";
        private string DivineToll = "Divine Toll";
        private string AshenHallow = "Ashen Hallow";

        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private int holy_power => API.PlayerCurrentHolyPower;
        private bool IsMelee => API.TargetRange < 6;
        private int time => API.PlayerTimeInCombat;
        private float gcd => API.SpellGCDTotalDuration;
        private bool HasDefenseBuff => API.PlayerHasBuff(ShieldofVengeance, false, false) || API.PlayerHasBuff(DivineShield, false, false);
        private float CrusaderStrikeCooldown => (6 / (1 + API.PlayerGetHaste / 100)) * 100;
        private float Crusader_Strike_Fractional => (API.SpellCharges(CrusaderStrike) * 100 + ((CrusaderStrikeCooldown - API.SpellChargeCD(CrusaderStrike)) / (CrusaderStrikeCooldown / 100)));
        private bool gcd_to_hpg => API.SpellCDDuration(CrusaderStrike) > gcd && API.SpellCDDuration(BladeofJustice) > gcd && API.SpellCDDuration(Judgment) > gcd && (API.SpellCDDuration(HammerofWrath) > gcd || API.TargetHealthPercent > 20) && API.SpellCDDuration(WakeofAshes) > gcd;

        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }


        //finishers->add_action("variable,name=ds_castable,value=spell_targets.divine_storm>=2|buff.empyrean_power.up&debuff.judgment.down&buff.divine_purpose.down|spell_targets.divine_storm>=2&buff.crusade.up&buff.crusade.stack<10");
        private bool ds_castable => API.TargetUnitInRangeCount >= 2 || PlayerHasBuff(EmpyreanPower) && !API.TargetHasDebuff(Judgment) && !PlayerHasBuff(DivinePurpose) || API.TargetUnitInRangeCount >= 2 && PlayerHasBuff(Crusade) && API.PlayerBuffStacks(Crusade) < 10;

        private void finishers()
        {
            //finishers->add_talent(this, "Seraphim", "if=((!talent.crusade.enabled&buff.avenging_wrath.up|cooldown.avenging_wrath.remains>25)|(buff.crusade.up|cooldown.crusade.remains>25))           &                (!talent.final_reckoning.enabled|cooldown.final_reckoning.remains<10)                   &                   (!talent.execution_sentence.enabled|cooldown.execution_sentence.remains<10)          &time_to_hpg=0");
            if (Talent_Seraphim && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && API.CanCast(Seraphim) && IsMelee && (!IsCooldowns || (((!Talent_Crusade && PlayerHasBuff(AvengingWrath) || (API.SpellCDDuration(AvengingWrath) > 2500)) || (PlayerHasBuff(Crusade) || (API.SpellCDDuration(Crusade) > 2500))) && (!Talent_FinalReckoning || (API.SpellCDDuration(FinalReckoning) < 1000)) && (!API.PlayerIsTalentSelected(1, 3) || (API.SpellCDDuration(ExecutionSentence) < 1000)))))
            {
                API.CastSpell(Seraphim);
                return;
            }
            //finishers->add_action("vanquishers_hammer,if=(!talent.final_reckoning.enabled|cooldown.final_reckoning.remains>gcd*10|debuff.final_reckoning.up)&(!talent.execution_sentence.enabled|cooldown.execution_sentence.remains>gcd*10|debuff.execution_sentence.up)|spell_targets.divine_storm>=2");
            //finishers->add_talent(this, "Execution Sentence", "                                                                                                                                                         if=spell_targets.divine_storm<=3&((!talent.crusade.enabled|buff.crusade.down&cooldown.crusade.remains>10)                     |buff.crusade.stack>=3|cooldown.avenging_wrath.remains>10|debuff.final_reckoning.up)&time_to_hpg=0");
            if (API.PlayerIsTalentSelected(1, 3) &&IsCooldowns && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && API.CanCast(ExecutionSentence) && API.TargetRange < 30 && (!IsCooldowns || (((!IsAOE || IsAOE && API.PlayerUnitInMeleeRangeCount <= 3) && ((!Talent_Crusade || !PlayerHasBuff(Crusade) && (API.SpellCDDuration(Crusade) > 1000)) || API.PlayerBuffStacks(Crusade) >= 3 || (API.SpellCDDuration(AvengingWrath) > 1000) || API.TargetHasDebuff(FinalReckoning))))))
            {
                API.CastSpell(ExecutionSentence);
                return;
            }
            //finishers->add_action(this, "Divine Storm", "if=variable.ds_castable&!buff.vanquishers_hammer.up&((!talent.crusade.enabled|cooldown.crusade.remains>gcd*3)&(!talent.execution_sentence.enabled|cooldown.execution_sentence.remains>gcd*3|spell_targets.divine_storm>=3)|spell_targets.divine_storm>=2&(talent.holy_avenger.enabled&cooldown.holy_avenger.remains<gcd*3|buff.crusade.up&buff.crusade.stack<10))");
            if (API.CanCast(DivineStorm) && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && IsMelee && PlayerLevel >= 23 && ds_castable && !PlayerHasBuff(VanquishersHammer) && (!IsCooldowns || ((!Talent_Crusade || (API.SpellCDDuration(Crusade) > gcd * 3)) && (!API.PlayerIsTalentSelected(1, 3) || (API.SpellCDDuration(ExecutionSentence) > gcd * 3) || IsAOE && API.PlayerUnitInMeleeRangeCount >= 3) || IsAOE && API.PlayerUnitInMeleeRangeCount >= 2 && (Talent_HolyAvenger && (API.SpellCDDuration(HolyAvenger) < gcd * 3) || PlayerHasBuff(Crusade) && API.PlayerBuffStacks(Crusade) < 10))))
            {
                API.CastSpell(DivineStorm);
                return;
            }
            //finishers -> add_action( this, "Templar's Verdict", "if=(!talent.crusade.enabled|cooldown.crusade.remains>gcd*3)&(!talent.execution_sentence.enabled|cooldown.execution_sentence.remains>gcd*3&spell_targets.divine_storm<=3)&(!talent.final_reckoning.enabled|cooldown.final_reckoning.remains>gcd*3)&(!covenant.necrolord.enabled|cooldown.vanquishers_hammer.remains>gcd)|talent.holy_avenger.enabled&cooldown.holy_avenger.remains<gcd*3|buff.holy_avenger.up|buff.crusade.up&buff.crusade.stack<10|buff.vanquishers_hammer.up" );
            if (API.CanCast(TemplarsVerdict) && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && IsMelee && PlayerLevel >= 10 && (!IsCooldowns || ((!Talent_Crusade || (API.SpellCDDuration(Crusade) > gcd * 3)) && (!API.PlayerIsTalentSelected(1, 3) || (API.SpellCDDuration(ExecutionSentence) > gcd * 3) && API.PlayerUnitInMeleeRangeCount <= 3) && (!Talent_FinalReckoning || (API.SpellCDDuration(FinalReckoning) > gcd * 3)) && (PlayerCovenantSettings != "Necrolord" || API.SpellCDDuration(VanquishersHammer) > gcd) || Talent_HolyAvenger && (API.SpellCDDuration(HolyAvenger) < gcd * 3) || PlayerHasBuff(HolyAvenger) || PlayerHasBuff(Crusade) && API.PlayerBuffStacks(Crusade) < 10 || PlayerHasBuff(VanquishersHammer))))
            {
                API.CastSpell(TemplarsVerdict);
                return;
            }
        }
        //Talents
        private bool Talent_ExecutionSentence => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_HolyAvenger => API.PlayerIsTalentSelected(5, 2);
        private bool Talent_Seraphim => API.PlayerIsTalentSelected(5, 3);
        private bool Talent_Crusade => API.PlayerIsTalentSelected(7, 2);
        private bool Talent_FinalReckoning => API.PlayerIsTalentSelected(7, 3);

        //CBProperties

        string[] CovenantList = new string[] { "None", "Venthyr", "Night Fae", "Kyrian", "Necrolord" };
        string[] AlwaysCooldownsList = new string[] { "always", "with Cooldowns", "on AOE" };

        private int FlashofLightLifePercent => percentListProp[CombatRoutine.GetPropertyInt("FOLOOCPCT")];
        private bool FLashofLightOutofCombat => CombatRoutine.GetPropertyBool("FOLOOC");

        private bool AutoAuraSwitch => CombatRoutine.GetPropertyBool("AURASWITCH");

        private int LayOnHandsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(LayOnHands)];
        private int ShieldofVengeanceLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ShieldofVengeance)];
        private int DivineShieldLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DivineShield)];
        private int WordOfGloryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(WordOfGlory)];
        private int FlashofLightLifePercentProc => percentListProp[CombatRoutine.GetPropertyInt(FlashofLight)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseWakeofAshes => AlwaysCooldownsList[CombatRoutine.GetPropertyInt("UseWakeofAshes")];

        public override void Initialize()
        {
            CombatRoutine.Name = "Retribution Paladin by Vec ";
            API.WriteLog("Welcome to Paladin Retribution Rotation");
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
            CombatRoutine.AddSpell(DivineToll, "F8");
            CombatRoutine.AddSpell(AshenHallow, "F8");

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
            CombatRoutine.AddBuff(VanquishersHammer);
            CombatRoutine.AddBuff(HolyAvenger);
            CombatRoutine.AddBuff(Seraphim);

            //Debuffs
            CombatRoutine.AddDebuff(Forearance);
            CombatRoutine.AddDebuff(Judgment);
            CombatRoutine.AddDebuff(ExecutionSentence);
            CombatRoutine.AddDebuff(FinalReckoning);



            //CBProperties
            CombatRoutine.AddProp("FOLOOCPCT", "Out of combat Life Percent", percentListProp, "Life percent at which Flash of Light is used out of combat to heal you between pulls", FlashofLight, 7);
            CombatRoutine.AddProp("FOLOOC", "Out of Combat Healing", true, "Should the bot use Flash of Light out of combat to heal you between pulls", FlashofLight);
            CombatRoutine.AddProp(FlashofLight, "Selfless Healer Life Percent", percentListProp, "Life percent at which " + FlashofLight + " is used with selfless healer procs, set to 0 to disable", FlashofLight, 5);
            
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("UseWakeofAshes", "Use " + "Wake of Ashes", AlwaysCooldownsList, "Use " + WakeofAshes + " always, with Cooldowns", "Cooldowns", 0);

            CombatRoutine.AddProp("AURASWITCH", "Auto Aura Switch", true, "Auto Switch Aura between Crusader Aura and Devotion Aura", "Generic");

            CombatRoutine.AddProp(LayOnHands, LayOnHands + " Life Percent", percentListProp, "Life percent at which" + LayOnHands + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(ShieldofVengeance, ShieldofVengeance + " Life Percent", percentListProp, "Life percent at which" + ShieldofVengeance + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(DivineShield, DivineShield + " Life Percent", percentListProp, "Life percent at which" + DivineShield + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(WordOfGlory, WordOfGlory + " Life Percent", percentListProp, "Life percent at which Word of Glory is used, set to 0 to disable", "Defense", 4);




        }

        public override void Pulse()
        {
            if (API.PlayerIsMounted)
            {
                if (AutoAuraSwitch && API.CanCast(CrusaderAura) && PlayerLevel >= 21 && !PlayerHasBuff(CrusaderAura))
                {
                    API.CastSpell(CrusaderAura);
                    return;
                }
            }
            else
            {
                if (AutoAuraSwitch && API.CanCast(DevotionAura) && PlayerLevel >= 21 && !PlayerHasBuff(DevotionAura))
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

            if (API.PlayerHealthPercent <= LayOnHandsLifePercent && !API.SpellISOnCooldown(LayOnHands) && PlayerLevel >= 9 && !API.PlayerHasDebuff(Forearance, false, false))
            {
                API.CastSpell(LayOnHands);
                return;
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
           //API.WriteLog("debug: buff: " + API.CanCast(ExecutionSentence));
            #region cooldowns
            if (IsCooldowns)
            {
                //cds->add_action(this, "Avenging Wrath", "if=(holy_power>=4&time<5|holy_power>=3&time>5|talent.holy_avenger.enabled&cooldown.holy_avenger.remains=0)&time_to_hpg=0");
                if (API.CanCast(AvengingWrath) && IsMelee && (holy_power >= 4 && time < 500 || holy_power >= 3 && time > 500 || Talent_HolyAvenger && !API.SpellISOnCooldown(HolyAvenger)) && PlayerLevel >= 37)
                {
                    API.CastSpell(AvengingWrath);
                    return;
                }
                //cds->add_talent(this, "Crusade", "if=(holy_power>=4&time<5|holy_power>=3&time>5|talent.holy_avenger.enabled&cooldown.holy_avenger.remains=0)&time_to_hpg=0");
                if (API.CanCast(Crusade) && IsMelee && Talent_Crusade && (holy_power >= 4 && API.PlayerTimeInCombat < 500 || holy_power >= 300 && API.PlayerTimeInCombat > 500 || Talent_HolyAvenger && !API.SpellISOnCooldown(HolyAvenger)))
                {
                    API.CastSpell(Crusade);
                    return;
                }
                //cds->add_action("ashen_hallow");
                if (!API.SpellISOnCooldown(AshenHallow) && !API.PlayerIsMoving && API.TargetRange <= 30 && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
                {
                    API.CastSpell(AshenHallow);
                    return;
                }
                //cds->add_talent(this, "Holy Avenger", "if=time_to_hpg=0&((buff.avenging_wrath.up|buff.crusade.up)|(buff.avenging_wrath.down&cooldown.avenging_wrath.remains>40|buff.crusade.down&cooldown.crusade.remains>40))");
                if (API.CanCast(HolyAvenger) && IsMelee && Talent_HolyAvenger && ((PlayerHasBuff(AvengingWrath) || PlayerHasBuff(Crusade)) || (!PlayerHasBuff(AvengingWrath) && (API.SpellCDDuration(AvengingWrath) > 4000) || !PlayerHasBuff(Crusade) && (API.SpellCDDuration(Crusade) > 4000))))
                {
                    API.CastSpell(HolyAvenger);
                    return;
                }
                //cds->add_talent(this, "Final Reckoning", "if=holy_power>=3&cooldown.avenging_wrath.remains>gcd&time_to_hpg=0&(!talent.seraphim.enabled|buff.seraphim.up)");
                if (Talent_FinalReckoning && IsMelee && !API.SpellISOnCooldown(FinalReckoning) && API.TargetRange < 30 && API.PlayerCurrentHolyPower >= 3 && (API.SpellCDDuration(AvengingWrath) > gcd) && (!Talent_Seraphim || PlayerHasBuff(Seraphim)))
                {
                    API.CastSpell(FinalReckoning);
                    return;
                }
            }
            #endregion

            #region Generators
            // generators -> add_action( "call_action_list,name=finishers,if=holy_power>=5|buff.holy_avenger.up|debuff.final_reckoning.up|debuff.execution_sentence.up|buff.memory_of_lucid_dreams.up|buff.seething_rage.up" );
            if (holy_power >= 5 || PlayerHasBuff(HolyAvenger) || API.TargetHasDebuff(FinalReckoning) || API.TargetHasDebuff(ExecutionSentence) || !IsCooldowns)
            {
                finishers();
            }

                //generators->add_action("divine_toll,if=!debuff.judgment.up&(!raid_event.adds.exists|raid_event.adds.in>30)&(holy_power<=2|holy_power<=4&(cooldown.blade_of_justice.remains>gcd*2|debuff.execution_sentence.up|debuff.final_reckoning.up))&(!talent.final_reckoning.enabled|cooldown.final_reckoning.remains>gcd*10)&(!talent.execution_sentence.enabled|cooldown.execution_sentence.remains>gcd*10)");
                if (!API.SpellISOnCooldown(DivineToll) && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <=30 && (!IsCooldowns || (!API.TargetHasDebuff(Judgment) && (holy_power <= 2 || holy_power <= 4 && (API.SpellCDDuration(BladeofJustice) > gcd * 2 || API.TargetHasDebuff(ExecutionSentence) || API.TargetHasDebuff(FinalReckoning))) && (!Talent_FinalReckoning || (API.SpellCDDuration(FinalReckoning) > gcd * 10)) && (!Talent_ExecutionSentence || (API.SpellCDDuration(ExecutionSentence) > gcd * 10)))))
                {
                    API.CastSpell(DivineToll);
                    return;
                }
                //generators->add_action(this, "Wake of Ashes", "if=(holy_power=0|holy_power<=2&(cooldown.blade_of_justice.remains>gcd*2|debuff.execution_sentence.up|debuff.final_reckoning.up))&(!raid_event.adds.exists|raid_event.adds.in>20)&(!talent.execution_sentence.enabled|cooldown.execution_sentence.remains>15)&(!talent.final_reckoning.enabled|cooldown.final_reckoning.remains>15)");
                if (API.CanCast(WakeofAshes) && IsMelee && PlayerLevel >= 39 && (holy_power == 0 || holy_power <= 2 && (API.SpellCDDuration(BladeofJustice) > gcd * 2 || API.TargetHasDebuff(ExecutionSentence) || API.TargetHasDebuff(FinalReckoning))) && (!API.PlayerIsTalentSelected(1, 3) || (API.SpellCDDuration(ExecutionSentence) > 1500)) && (!Talent_FinalReckoning || (API.SpellCDDuration(FinalReckoning) > 1500)))
                {
                    API.CastSpell(WakeofAshes);
                    return;
                }
                //generators->add_action(this, "Blade of Justice", "if=holy_power<=3");
                if (!API.SpellISOnCooldown(BladeofJustice) && API.TargetRange <= 12 && API.PlayerCurrentHolyPower <= 3 && PlayerLevel >= 19)
                {
                    API.CastSpell(BladeofJustice);
                    return;
                }
                //generators->add_action(this, "Hammer of Wrath", "if=holy_power<=4");
                if (API.CanCast(HammerofWrath) && API.TargetHealthPercent <= 20 && holy_power <= 4 && API.TargetRange <= 30 && PlayerLevel >= 46)
                {
                    API.CastSpell(HammerofWrath);
                    return;
                }
                //generators->add_action(this, "Judgment", "if=!debuff.judgment.up&(holy_power<=2|holy_power<=4&cooldown.blade_of_justice.remains>gcd*2)");
                if (API.CanCast(Judgment) && (!API.TargetHasDebuff(Judgment) && (holy_power <= 2 || holy_power <= 4 && API.SpellCDDuration(BladeofJustice) > gcd * 2)) && API.TargetRange <= 30 && PlayerLevel >= 3)
                {
                    API.CastSpell(Judgment);
                    return;
                }
                //generators->add_action("call_action_list,name=finishers,if=(target.health.pct<=20|buff.avenging_wrath.up|buff.crusade.up|buff.empyrean_power.up)");
                if (API.TargetHealthPercent <= 20 || PlayerHasBuff(AvengingWrath) || PlayerHasBuff(Crusade) || PlayerHasBuff(EmpyreanPower))
                {
                    finishers();
                }
                //generators->add_action(this, "Crusader Strike", "if=cooldown.crusader_strike.charges_fractional>=1.75&(holy_power<=2|holy_power<=3&cooldown.blade_of_justice.remains>gcd*2|holy_power=4&cooldown.blade_of_justice.remains>gcd*2&cooldown.judgment.remains>gcd*2)");
                if (API.CanCast(CrusaderStrike) && IsMelee && (Crusader_Strike_Fractional >= 175 && (holy_power <= 2 || holy_power <= 3 && API.SpellCDDuration(BladeofJustice) > gcd * 2 || holy_power == 4 && API.SpellCDDuration(BladeofJustice) > gcd * 2 && API.SpellCDDuration(Judgment) > gcd * 2)))
                {
                    API.CastSpell(CrusaderStrike);
                    return;
                }
                // generators -> add_action( "call_action_list,name=finishers" );
                finishers();
                //generators->add_action(this, "Crusader Strike", "if=holy_power<=4");
                if (API.CanCast(CrusaderStrike) && !(holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && IsMelee && holy_power <= 4)
                {
                    API.CastSpell(CrusaderStrike);
                    return;
                }
                //generators->add_action("arcane_torrent,if=holy_power<=4");
                //generators->add_action(this, "Consecration", "if=time_to_hpg>gcd");
                if (API.CanCast(Consecration) && !(holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && IsMelee && gcd_to_hpg)
                {
                    API.CastSpell(Consecration);
                    return;
                }
                #endregion
            

        }
    }
}