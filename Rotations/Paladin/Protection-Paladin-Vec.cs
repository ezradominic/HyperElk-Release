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
        private string Forbearance = "Forbearance";
        private string BlessedHammer = "Blessed Hammer";
        private string ShiningLight = "Shining Light";
        private string Seraphim = "Seraphim";
        private string HolyAvenger = "Holy Avenger";
        private string MomentOfGlory = "Moment of Glory";
        private string vengeful_shock = "Vengeful Shock";
        private string SanctifiedWrath = "Sanctified Wrath";
        private string VanquishersHammer = "Vanquisher's Hammer";
        private string DivineToll = "Divine Toll";
        private string AshenHallow = "Ashen Hallow";
        private string BlessingofSpring = "Blessing of Spring";
        private string BlessingofSummer = "Blessing of Summer";
        private string BlessingofAutumn = "Blessing of Autumn";
        private string BlessingofWinter = "Blessing of Winter";
        private string RingingClarity = "Ringing Clarity";
        private string RoyalDecree = "Royal Decree";
        private string BlessingofProtection = "Blessing of Protection";
        private string BlessingofSacrifice = "Blessing of Sacrifice";

        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";

        private bool IsMouseover => API.ToggleIsEnabled("MO Heal");
        private bool HealFocus => API.ToggleIsEnabled("Focus Heal");

        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;

        private bool HasDefenseBuff => API.PlayerHasBuff(ArdentDefender, false, false) || API.PlayerHasBuff(GuardianofAncientKings, false, false) || API.PlayerHasBuff(DivineShield, false, false);
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        //Talents
        private bool Talent_CrusadersJudgment => API.PlayerIsTalentSelected(2, 2);
        private bool Talent_MomentOfGlory => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_BlessedHammer => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_HolyAvenger => API.PlayerIsTalentSelected(5, 2);
        private bool Talent_Seraphim => API.PlayerIsTalentSelected(5, 3);
        private bool Talent_SanctifiedWrath => API.PlayerIsTalentSelected(7, 1);

        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };

        //CBProperties


        private int FlashofLightLifePercentooc => percentListProp[CombatRoutine.GetPropertyInt("FOLOOCPCTOOC")];
        private int FlashofLightLifePercentic => percentListProp[CombatRoutine.GetPropertyInt("FOLOOCPCTIC")];
        private bool FLashofLightOutofCombat => CombatRoutine.GetPropertyBool("FOLOOC");
        private bool FLashofLightInCombat => CombatRoutine.GetPropertyBool("FOLIC");
        private bool VengefulShockConduit => CombatRoutine.GetPropertyBool("VengefulShockConduit");
        private int WordOfGloryLifePercent => percentListProp[CombatRoutine.GetPropertyInt("WOGPCT")];
        private bool AutoAuraSwitch => CombatRoutine.GetPropertyBool("AURASWITCH");
        private bool IsAvengingWrath => CombatRoutine.GetPropertyBool(AvengingWrath);
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];

        private int LayOnHandsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(LayOnHands)];
        private int ArdentDefenderLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ArdentDefender)];
        private int DivineShieldLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DivineShield)];
        private int GuardianofAncientKingsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(GuardianofAncientKings)];

        private int BlessingofProtectionPercent => percentListProp[CombatRoutine.GetPropertyInt(BlessingofProtection)];
        private int BlessingofSacrificePercent => percentListProp[CombatRoutine.GetPropertyInt(BlessingofProtection)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Protection Paladin by Vec";
            API.WriteLog("Welcome to Protection Paladin by Vec");

            //Spells
            CombatRoutine.AddSpell(Judgment, 275779, "D2");
            CombatRoutine.AddSpell(AvengersShield, 31935, "D2");
            CombatRoutine.AddSpell(HammeroftheRighteous, 53595, "D1");
            CombatRoutine.AddSpell(BlessedHammer, 204019, "D1");
            CombatRoutine.AddSpell(Consecration, 26573, "D4");
            CombatRoutine.AddSpell(ShieldoftheRighteous, 53600, "D3");
            CombatRoutine.AddSpell(WordOfGlory, 85673, "F7");
            CombatRoutine.AddSpell(FlashofLight, 19750, "Q");
            CombatRoutine.AddSpell(Rebuke, 96231, "F");

            CombatRoutine.AddSpell(CrusaderAura, 32223, "F5");
            CombatRoutine.AddSpell(DevotionAura, 465, "F6");

            CombatRoutine.AddSpell(AvengingWrath, 31884, "F");
            CombatRoutine.AddSpell(HammerofWrath, 24275, "D7");

            CombatRoutine.AddSpell(LayOnHands, 633, "F8");
            CombatRoutine.AddSpell(ArdentDefender, 31850, "S");
            CombatRoutine.AddSpell(GuardianofAncientKings, 86659, "F9");
            CombatRoutine.AddSpell(DivineShield, 642, "F10");
            CombatRoutine.AddSpell(HolyAvenger, 105809, "F11");
            CombatRoutine.AddSpell(Seraphim, 152262, "F11");
            CombatRoutine.AddSpell(MomentOfGlory, 327193, "D6");
            CombatRoutine.AddSpell(SanctifiedWrath, 171648, "F1");
            CombatRoutine.AddSpell(DivineToll, 304971, "F8");
            CombatRoutine.AddSpell(AshenHallow, 316958, "F8");
            CombatRoutine.AddSpell(BlessingofSpring, 328282, "F8");
            CombatRoutine.AddSpell(BlessingofSummer, 328620, "F8");
            CombatRoutine.AddSpell(BlessingofAutumn, 328622, "F8");
            CombatRoutine.AddSpell(BlessingofWinter, 328281, "F8");
            CombatRoutine.AddSpell(VanquishersHammer, 328204, "F8");
            CombatRoutine.AddSpell(BlessingofProtection, 1022, "F11");
            CombatRoutine.AddSpell(BlessingofSacrifice, 6940, "F12");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            CombatRoutine.AddMacro(LayOnHands + " MO", "F10");
            CombatRoutine.AddMacro(LayOnHands + " Focus", "F10");
            CombatRoutine.AddMacro(BlessingofProtection + " MO", "F11");
            CombatRoutine.AddMacro(BlessingofProtection + " Focus", "F11");
            CombatRoutine.AddMacro(BlessingofSacrifice + " MO", "F11");
            CombatRoutine.AddMacro(BlessingofSacrifice + " Focus", "F11");
            CombatRoutine.AddMacro(WordOfGlory + " MO", "F10");
            CombatRoutine.AddMacro(WordOfGlory + " Focus", "F10");
            //Buffs
            CombatRoutine.AddBuff(Consecration, 188370);
            CombatRoutine.AddBuff(CrusaderAura, 32223);
            CombatRoutine.AddBuff(DevotionAura, 465);
            CombatRoutine.AddBuff(AvengingWrath, 31884);
            CombatRoutine.AddBuff(ShieldoftheRighteous, 132403);
            CombatRoutine.AddBuff(DivinePurpose, 223817);
            CombatRoutine.AddBuff(ArdentDefender, 31850);
            CombatRoutine.AddBuff(GuardianofAncientKings, 86659);
            CombatRoutine.AddBuff(DivineShield, 642);
            CombatRoutine.AddBuff(ShiningLight, 327510);
            CombatRoutine.AddBuff(HolyAvenger, 105809);
            CombatRoutine.AddBuff(VanquishersHammer, 328204);
            CombatRoutine.AddBuff(Seraphim, 152262);
            CombatRoutine.AddBuff(RoyalDecree, 340147);
            //Debuffs
            CombatRoutine.AddDebuff(Forbearance, 25771);
            CombatRoutine.AddDebuff(Judgment, 197277);
            CombatRoutine.AddDebuff(vengeful_shock, 340007);
            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            CombatRoutine.AddToggle("MO Heal");
            CombatRoutine.AddToggle("Focus Heal");

            //CBProperties
            CombatRoutine.AddProp("FOLOOCPCTOOC", "Out of combat Life Percent", percentListProp, "Life percent at which Flash of Light is used out of combat to heal you between pulls", FlashofLight, 7);
            CombatRoutine.AddProp("FOLOOC", "Out of Combat Healing", true, "Should the bot use Flash of Light out of combat to heal you between pulls", FlashofLight);
            CombatRoutine.AddProp("FOLIC", "Combat Healing", true, "Should the bot use Flash of Light in combat to heal yo", FlashofLight);
            CombatRoutine.AddProp("FOLOOCPCTIC", "In combat Life Percent", percentListProp, "Life percent at which Flash of Light is used in combat to heal you", FlashofLight, 7);
            CombatRoutine.AddProp("VengefulShockConduit", "Vengeful Shock Conduit", false, "Do you have the Vengeful Shock Conduit?", "Conduit");

            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);

            CombatRoutine.AddProp("AURASWITCH", "Auto Aura Switch", true, "Auto Switch Aura between Crusader Aura|Devotion Aura", "Generic");
            CombatRoutine.AddProp(AvengingWrath, "Use Avenging Wrath", true, "Use Avenging Wrath with cooldowns", "Generic");
            CombatRoutine.AddProp("AOEUnitNumner", "AOE Unit Numner", 2, "How many units around to use AOE rotation", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp(LayOnHands, LayOnHands + " Life Percent", percentListProp, "Life percent at which" + LayOnHands + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(ArdentDefender, ArdentDefender + " Life Percent", percentListProp, "Life percent at which" + ArdentDefender + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(DivineShield, DivineShield + " Life Percent", percentListProp, "Life percent at which" + DivineShield + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(GuardianofAncientKings, GuardianofAncientKings + " Life Percent", percentListProp, "Life percent at which" + GuardianofAncientKings + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp("WOGPCT", WordOfGlory, percentListProp, "Life percent at which Word of Glory is used", "Defense", 5);
            CombatRoutine.AddProp(BlessingofProtection, BlessingofProtection + " Life Percent", percentListProp, "Life percent at which" + BlessingofProtection + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(BlessingofSacrifice, BlessingofSacrifice + " Life Percent", percentListProp, "Life percent at which" + BlessingofSacrifice + "is used, set to 0 to disable", "Defense", 2);

        }

        public override void Pulse()
        {
            //API.WriteLog("debug " + API.SpellCDDuration(AvengingWrath) + " " + API.CanCast(AvengingWrath));
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


            }
            rotation();
            return;
        }

        public override void OutOfCombatPulse()
        {
            if (FLashofLightOutofCombat && API.PlayerHealthPercent <= FlashofLightLifePercentooc && !API.PlayerIsMoving && API.CanCast(FlashofLight) && PlayerLevel >= 4)
            {
                API.CastSpell(FlashofLight);
                return;
            }
        }
        private void rotation()
        {
            if (API.PlayerItemCanUse(PhialofSerenity) && API.PlayerItemRemainingCD(PhialofSerenity) == 0 && API.PlayerHealthPercent <= PhialofSerenityLifePercent)
            {
                API.CastSpell(PhialofSerenity);
                return;
            }
            if (API.PlayerItemCanUse(SpiritualHealingPotion) && API.PlayerItemRemainingCD(SpiritualHealingPotion) == 0 && API.PlayerHealthPercent <= SpiritualHealingPotionLifePercent)
            {
                API.CastSpell(SpiritualHealingPotion);
                return;
            }
            if (API.PlayerHealthPercent <= LayOnHandsLifePercent && API.CanCast(LayOnHands) && PlayerLevel >= 9 && !API.PlayerHasDebuff(Forbearance, false, false))
            {
                API.CastSpell(LayOnHands);
                return;
            }
            if (API.PlayerHealthPercent <= DivineShieldLifePercent && API.CanCast(DivineShield) && PlayerLevel >= 10 && !HasDefenseBuff && !API.PlayerHasDebuff(Forbearance, false, false))
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

            if (API.PlayerHealthPercent <= WordOfGloryLifePercent && (API.PlayerCurrentHolyPower >= 3 || API.PlayerHasBuff(ShiningLight)) && !API.SpellISOnCooldown(WordOfGlory) && PlayerLevel >= 7)
            {
                API.CastSpell(WordOfGlory);
                return;
            }
            if (FLashofLightOutofCombat && API.PlayerHealthPercent <= FlashofLightLifePercentic && !API.PlayerIsMoving && API.CanCast(FlashofLight) && PlayerLevel >= 4)
            {
                API.CastSpell(FlashofLight);
                return;
            }
            if (HealFocus)
            {
                if (!API.MacroIsIgnored(LayOnHands + " Focus") && API.FocusHealthPercent <= LayOnHandsLifePercent && API.FocusRange <= 40 && API.CanCast(LayOnHands) && PlayerLevel >= 9 && !API.FocusHasDebuff(Forbearance, false, false))
                {
                    API.CastSpell(LayOnHands + " Focus");
                    return;
                }
                if (!API.MacroIsIgnored(BlessingofProtection + " Focus") && API.FocusRange <= 40 && API.FocusHealthPercent <= BlessingofProtectionPercent && API.CanCast(BlessingofProtection) && PlayerLevel >= 10)
                {
                    API.CastSpell(BlessingofProtection + " Focus");
                    return;
                }
                if (!API.MacroIsIgnored(BlessingofSacrifice + " Focus") && API.FocusRange <= 40 && API.FocusHealthPercent <= BlessingofSacrificePercent && API.CanCast(BlessingofSacrifice))
                {
                    API.CastSpell(BlessingofSacrifice + " Focus");
                    return;
                }
            }
            if (IsMouseover)
            {
                if (!API.MacroIsIgnored(LayOnHands + " MO") && API.MouseoverHealthPercent <= LayOnHandsLifePercent && API.MouseoverRange <= 40 && API.CanCast(LayOnHands) && PlayerLevel >= 9 && !API.MouseoverHasDebuff(Forbearance, false, false))
                {
                    API.CastSpell(LayOnHands + " MO");
                    return;
                }
                if (!API.MacroIsIgnored(BlessingofProtection + " MO") && API.SpellIsCanbeCast(BlessingofProtection) && API.MouseoverRange <= 40 && API.MouseoverHealthPercent <= BlessingofProtectionPercent && API.CanCast(BlessingofProtection) && PlayerLevel >= 10)
                {
                    API.CastSpell(BlessingofProtection + " MO");
                    return;
                }
                if (!API.MacroIsIgnored(BlessingofSacrifice + " MO") && API.SpellIsCanbeCast(BlessingofSacrifice) && API.MouseoverRange <= 40 && API.MouseoverHealthPercent <= BlessingofSacrificePercent && API.CanCast(BlessingofSacrifice))
                {
                    API.CastSpell(BlessingofSacrifice + " MO");
                    return;
                }
            }
            if (IsCooldowns)
            {
                //cds->add_action("fireblood,if=buff.avenging_wrath.up");
                //cds->add_talent(this, "Seraphim");
                if (API.CanCast(Seraphim) && API.PlayerCurrentHolyPower >= 3 && Talent_Seraphim && IsMelee)
                {
                    API.CastSpell(Seraphim);
                    return;
                }
                //cds->add_action(this, "Avenging Wrath");
                if (IsAvengingWrath && API.CanCast(AvengingWrath) && PlayerLevel >= 37 && IsMelee)
                {
                    API.CastSpell(AvengingWrath);
                    return;
                }
                //cds->add_talent(this, "Holy Avenger", "if=buff.avenging_wrath.up|cooldown.avenging_wrath.remains>60");
                if (API.CanCast(HolyAvenger) && Talent_HolyAvenger && (API.PlayerHasBuff(AvengingWrath) || API.SpellCDDuration(AvengingWrath) > 6000) && IsMelee)
                {
                    API.CastSpell(HolyAvenger);
                    return;
                }
                //cds->add_action("potion,if=buff.avenging_wrath.up");
                //cds->add_talent(this, "Moment of Glory", "if=prev_gcd.1.avengers_shield&cooldown.avengers_shield.remains");
                if (API.CanCast(MomentOfGlory) && Talent_MomentOfGlory && (API.LastSpellCastInGame == AvengersShield || API.PlayerCurrentCastSpellID == 56641) && IsMelee)
                {
                    API.CastSpell(MomentOfGlory);
                    return;
                }
            }
            //cds->add_action("use_items,if=buff.seraphim.up|!talent.seraphim.enabled");
            if (PlayerHasBuff(Seraphim) || !Talent_Seraphim)
            {
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee)
                {
                    API.CastSpell("Trinket2");
                }
            }
            if (API.SpellCDDuration(Seraphim) > API.SpellGCDDuration || !Talent_Seraphim || !IsCooldowns)
            {
                //std->add_action(this, "Shield of the Righteous", "if=debuff.judgment.up&(debuff.vengeful_shock.up|!conduit.vengeful_shock.enabled)");
                if (API.PlayerHealthPercent > WordOfGloryLifePercent && API.CanCast(ShieldoftheRighteous, true, true) && IsMelee && (API.PlayerCurrentHolyPower >= 3 || API.PlayerHasBuff(DivinePurpose)) && PlayerLevel >= 2 && API.TargetHasDebuff(Judgment) && (API.TargetHasDebuff(vengeful_shock) || !VengefulShockConduit))
                {
                    API.CastSpell(ShieldoftheRighteous);
                    return;
                }
                //std->add_action(this, "Shield of the Righteous", "if=holy_power=5|buff.holy_avenger.up|holy_power=4&talent.sanctified_wrath.enabled&buff.avenging_wrath.up");
                if (API.PlayerHealthPercent > WordOfGloryLifePercent && API.CanCast(ShieldoftheRighteous, true, true) && IsMelee && (API.PlayerCurrentHolyPower >= 3 || API.PlayerHasBuff(DivinePurpose)) && PlayerLevel >= 2 && (API.PlayerCurrentHolyPower == 5 || API.PlayerHasBuff(HolyAvenger) || API.PlayerCurrentHolyPower == 4 && Talent_SanctifiedWrath || API.PlayerHasBuff(AvengingWrath)))
                {
                    API.CastSpell(ShieldoftheRighteous);
                    return;
                }
            }
            //std->add_action(this, "Judgment", "target_if=min:debuff.judgment.remains,if=charges=2|!talent.crusaders_judgment.enabled");
            if (API.CanCast(Judgment) && API.TargetRange <= 30 && PlayerLevel >= 3 && (API.SpellCharges(Judgment) == 2 || !Talent_CrusadersJudgment))
            {
                API.CastSpell(Judgment);
                return;
            }
            //std->add_action(this, "Avenger's Shield", "if=debuff.vengeful_shock.down&conduit.vengeful_shock.enabled");
            if (API.CanCast(AvengersShield) && !API.SpellISOnCooldown(AvengersShield) && API.TargetRange <= 30 && PlayerLevel >= 10 && !API.TargetHasDebuff(vengeful_shock) && VengefulShockConduit)
            {
                API.CastSpell(AvengersShield);
                return;
            }
            //std->add_action(this, "Hammer of Wrath");
            if ((API.TargetHealthPercent <= 20 || (API.PlayerHasBuff(AvengingWrath) && PlayerLevel >= 58)) && API.CanCast(HammerofWrath) && API.TargetRange <= 30 && PlayerLevel >= 46)
            {
                API.CastSpell(HammerofWrath);
                return;
            }
            //std->add_action(this, "Avenger's Shield");
            if (API.CanCast(AvengersShield) && !API.SpellISOnCooldown(AvengersShield) && API.TargetRange <= 30 && PlayerLevel >= 10)
            {
                API.CastSpell(AvengersShield);
                return;
            }
            //std->add_action(this, "Judgment", "target_if=min:debuff.judgment.remains");
            if (API.CanCast(Judgment) && API.TargetRange <= 30 && PlayerLevel >= 3)
            {
                API.CastSpell(Judgment);
                return;
            }
            if (PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                if (API.CanCast(BlessingofSpring))
                {
                    API.CastSpell(BlessingofSpring);
                    return;
                }
                if (API.CanCast(BlessingofSummer))
                {
                    API.CastSpell(BlessingofSummer);
                    return;
                }
                if (API.CanCast(BlessingofAutumn))
                {
                    API.CastSpell(BlessingofAutumn);
                    return;
                }
                if (API.CanCast(BlessingofWinter))
                {
                    API.CastSpell(BlessingofWinter);
                    return;
                }
            }
            //std->add_action("vanquishers_hammer");
            if (!API.SpellISOnCooldown(VanquishersHammer) && API.TargetRange <= 30 && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(VanquishersHammer);
                return;
            }
            //std->add_action(this, "Consecration", "if=!consecration.up");
            if (API.CanCast(Consecration) && IsMelee && PlayerLevel >= 14 && !API.PlayerHasBuff(Consecration))
            {
                API.CastSpell(Consecration);
                return;
            }
            //std->add_action("divine_toll");
            if (!API.SpellISOnCooldown(DivineToll) && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <= 30)
            {
                API.CastSpell(DivineToll);
                return;
            }
            //std->add_talent(this, "Blessed Hammer", "strikes=2.4,if=charges=3");
            if (API.CanCast(BlessedHammer) && Talent_BlessedHammer && API.SpellCharges(BlessedHammer) == 3 && IsMelee)
            {
                API.CastSpell(BlessedHammer);
                return;
            }
            //std->add_action("ashen_hallow");
            if (!API.SpellISOnCooldown(AshenHallow) && !API.PlayerIsMoving && API.TargetRange <= 30 && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(AshenHallow);
                return;
            }
            //std->add_action(this, "Hammer of the Righteous", "if=charges=2");
            if (API.CanCast(HammeroftheRighteous) && !Talent_BlessedHammer && API.SpellCharges(HammeroftheRighteous) == 2 && IsMelee && PlayerLevel >= 14)
            {
                API.CastSpell(HammeroftheRighteous);
                return;
            }
            //std->add_action(this, "Word of Glory", "if=buff.vanquishers_hammer.up");
            if ((API.PlayerCurrentHolyPower >= 3 || API.PlayerHasBuff(ShiningLight)) && API.PlayerHasBuff(VanquishersHammer) && API.CanCast(WordOfGlory, true, true) && PlayerLevel >= 7)
            {
                API.CastSpell(WordOfGlory);
                return;
            }
            //std->add_talent(this, "Blessed Hammer", "strikes=2.4");
            if (API.CanCast(BlessedHammer) && Talent_BlessedHammer && IsMelee)
            {
                API.CastSpell(BlessedHammer);
                return;
            }
            //std->add_action(this, "Hammer of the Righteous");
            if (API.CanCast(HammeroftheRighteous) && !Talent_BlessedHammer && IsMelee && PlayerLevel >= 14)
            {
                API.CastSpell(HammeroftheRighteous);
                return;
            }
            //std->add_action("lights_judgment");
            //std->add_action("arcane_torrent");
            //std->add_action(this, "Consecration");
            if (API.CanCast(Consecration) && IsMelee && PlayerLevel >= 14)
            {
                API.CastSpell(Consecration);
                return;
            }
            //std->add_action(this, "Word of Glory", "if=buff.shining_light_free.up&!covenant.necrolord");
            if ((API.PlayerHasBuff(ShiningLight) || API.PlayerHasBuff(RoyalDecree)) && PlayerCovenantSettings != "Necrolord" && API.CanCast(WordOfGlory, true, true) && PlayerLevel >= 7)
            {
                API.CastSpell(WordOfGlory);
                return;
            }
        }
    }
}
