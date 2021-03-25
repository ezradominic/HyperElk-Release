
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
        private string FinalVerdict = "Final Verdict";
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
        private string Forbearance = "Forbearance";
        private string SelflessHealer = "Selfless Healer";
        private string VanquishersHammer = "Vanquisher's Hammer";
        private string DivineToll = "Divine Toll";
        private string AshenHallow = "Ashen Hallow";
        private string BlessingofSpring = "Blessing of Spring";
        private string BlessingofSummer = "Blessing of Summer";
        private string BlessingofAutumn = "Blessing of Autumn";
        private string BlessingofWinter = "Blessing of Winter";
        private string RingingClarity = "Ringing Clarity";
        private string Mindgames = "Mindgames";
        private string BlessingofProtection = "Blessing of Protection";

        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";

        private bool IsMouseover => API.ToggleIsEnabled("MO Heal");
        private bool UseSmallCD => API.ToggleIsEnabled("Small CDs");
        private bool HealFocus => API.ToggleIsEnabled("Focus Heal");

        //Misc
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        private static bool Buff_or_CDmorethan(string spellname, int time)
        {
            return PlayerHasBuff(spellname) || API.SpellCDDuration(spellname) > time;
        }

        private int PlayerLevel => API.PlayerLevel;
        private int holy_power => API.PlayerCurrentHolyPower;
        private bool IsMelee => API.TargetRange < 6;
        private int time => API.PlayerTimeInCombat;
        private float gcd => API.SpellGCDTotalDuration;
        private bool HasDefenseBuff => API.PlayerHasBuff(ShieldofVengeance, false, false) || API.PlayerHasBuff(DivineShield, false, false);
        private float CrusaderStrikeCooldown => (6 / (1 + API.PlayerGetHaste / 100)) * 100;
        private float Crusader_Strike_Fractional => (API.SpellCharges(CrusaderStrike) * 100 + ((CrusaderStrikeCooldown - API.SpellChargeCD(CrusaderStrike)) / (CrusaderStrikeCooldown / 100)));
        private bool gcd_to_hpg => API.SpellCDDuration(CrusaderStrike) >= gcd && API.SpellCDDuration(BladeofJustice) >= gcd && API.SpellCDDuration(Judgment) >= gcd && (API.SpellCDDuration(HammerofWrath) >= gcd || API.TargetHealthPercent > 20) && (API.SpellCDDuration(WakeofAshes) >= gcd || !(UseWakeofAshes == "With Cooldowns" && (IsCooldowns || UseSmallCD) || UseWakeofAshes == "On Cooldown" || UseWakeofAshes == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE));
        private bool Conduit_enabled(string conduit)
        {
            return API.PlayerIsConduitSelected(conduit);
        }

        //finishers->add_action("variable,name=ds_castable,value=spell_targets.divine_storm>=2|buff.empyrean_power.up&debuff.judgment.down&buff.divine_purpose.down|spell_targets.divine_storm>=2&buff.crusade.up&buff.crusade.stack<10");
        private bool ds_castable => API.TargetUnitInRangeCount >= 2 || PlayerHasBuff(EmpyreanPower) && !API.TargetHasDebuff(Judgment) && !PlayerHasBuff(DivinePurpose) || API.TargetUnitInRangeCount >= 2 && PlayerHasBuff(Crusade) && API.PlayerBuffStacks(Crusade) < 10;


        //Talents
        private bool Talent_ExecutionSentence => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_HolyAvenger => API.PlayerIsTalentSelected(5, 2);
        private bool Talent_Seraphim => API.PlayerIsTalentSelected(5, 3);
        private bool Talent_Crusade => API.PlayerIsTalentSelected(7, 2);
        private bool Talent_FinalReckoning => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        private bool Playeriscasting => API.PlayerCurrentCastTimeRemaining > 40;
        string[] AlwaysCooldownsList = new string[] { "always", "with Cooldowns", "on AOE" };
        string[] CDUsagewithAOEandSmallCDs = new string[] { "Not Used", "With Cooldowns", "On Cooldown", "With Small CDs", "on AOE" };

        private int FlashofLightLifePercent => numbList[CombatRoutine.GetPropertyInt("FOLOOCPCT")];
        private bool FLashofLightOutofCombat => CombatRoutine.GetPropertyBool("FOLOOC");
        private bool AutoAuraSwitch => CombatRoutine.GetPropertyBool("AURASWITCH");

        private int LayOnHandsPlayerLifePercent => numbList[CombatRoutine.GetPropertyInt(LayOnHands + "player")];
        private int LayOnHandsFocusLifePercent => numbList[CombatRoutine.GetPropertyInt(LayOnHands + "focus")];
        private int LayOnHandsMouseoverLifePercent => numbList[CombatRoutine.GetPropertyInt(LayOnHands + "mouseover")];
        private int ShieldofVengeanceLifePercent => numbList[CombatRoutine.GetPropertyInt(ShieldofVengeance)];
        private int DivineShieldLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineShield)];
        private int WordOfGloryPlayerLifePercent => numbList[CombatRoutine.GetPropertyInt("WOGplayer")];
        private int WordOfGloryFocusLifePercent => numbList[CombatRoutine.GetPropertyInt("WOGfocus")];
        private int WordOfGloryMouseoverLifePercent => numbList[CombatRoutine.GetPropertyInt("WOGmouseover")];
        private int FlashofLightLifePercentProcPlayer => numbList[CombatRoutine.GetPropertyInt(FlashofLight + "player")];
        private int FlashofLightLifePercentProcFocus => numbList[CombatRoutine.GetPropertyInt(FlashofLight + "focus")];
        private int FlashofLightLifePercentProcMouseover => numbList[CombatRoutine.GetPropertyInt(FlashofLight + "mouseover")];
        private int ConsecrationLifePercent => numbList[CombatRoutine.GetPropertyInt(Consecration)];
        private string UseCovenant => CDUsagewithAOEandSmallCDs[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseSeraphim => CDUsagewithAOEandSmallCDs[CombatRoutine.GetPropertyInt("UseSeraphim")];
        private string UseWakeofAshes => CDUsagewithAOEandSmallCDs[CombatRoutine.GetPropertyInt("UseWakeofAshes")];
        private string UseTrinket1 => CDUsagewithAOEandSmallCDs[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsagewithAOEandSmallCDs[CombatRoutine.GetPropertyInt("Trinket2")];
        private int BlessingofProtectionPlayerPercent => numbList[CombatRoutine.GetPropertyInt(BlessingofProtection + "player")];
        private int BlessingofProtectionFocusPercent => numbList[CombatRoutine.GetPropertyInt(BlessingofProtection + "focus")];
        private int BlessingofProtectionMouseoverPercent => numbList[CombatRoutine.GetPropertyInt(BlessingofProtection + "mouseover")];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];


        public override void Initialize()
        {
            CombatRoutine.Name = "Retribution Paladin by Vec ";
            API.WriteLog("Welcome to Paladin Retribution Rotation");
            API.WriteLog("Macro required: /cast [@player] Final Reckoning");


            //Spells
            CombatRoutine.AddSpell(CrusaderStrike, 35395, "D1");
            CombatRoutine.AddSpell(Judgment, 20271, "D2");
            CombatRoutine.AddSpell(WakeofAshes, 255937, "D2");
            CombatRoutine.AddSpell(BladeofJustice, 184575, "D1");
            CombatRoutine.AddSpell(Consecration, 26573, "D4");
            CombatRoutine.AddSpell(TemplarsVerdict, 85256, "D3");
            CombatRoutine.AddSpell(FinalVerdict, 336872, "D3");
            CombatRoutine.AddSpell(DivineStorm, 53385, "D3");
            CombatRoutine.AddSpell(FinalReckoning, 343721, "D3");
            CombatRoutine.AddSpell(Seraphim, 152262, "D3");
            CombatRoutine.AddSpell(ExecutionSentence, 343527, "D3");
            CombatRoutine.AddSpell(Crusade, 231895, "D3");
            CombatRoutine.AddSpell(HolyAvenger, 105809, "D3");

            CombatRoutine.AddSpell(Rebuke, 96231, "F");

            CombatRoutine.AddSpell(CrusaderAura, 32223, "F5");
            CombatRoutine.AddSpell(DevotionAura, 465, "F6");

            CombatRoutine.AddSpell(AvengingWrath, 31884, "F");
            CombatRoutine.AddSpell(HammerofWrath, 24275, "D7");

            CombatRoutine.AddSpell(LayOnHands, 633, "F8");
            CombatRoutine.AddSpell(ShieldofVengeance, 184662, "S");
            CombatRoutine.AddSpell(DivineShield, 642, "F10");
            CombatRoutine.AddSpell(FlashofLight, 19750, "Q");
            CombatRoutine.AddSpell(WordOfGlory, 85673, "F7");
            CombatRoutine.AddSpell(DivineToll, 304971, "F8");
            CombatRoutine.AddSpell(AshenHallow, 316958, "F8");
            CombatRoutine.AddSpell(BlessingofSpring, 328282, "F8");
            CombatRoutine.AddSpell(BlessingofSummer, 328620, "F8");
            CombatRoutine.AddSpell(BlessingofAutumn, 328622, "F8");
            CombatRoutine.AddSpell(BlessingofWinter, 328281, "F8");

            CombatRoutine.AddSpell(VanquishersHammer, 328204, "F8");
            CombatRoutine.AddSpell(BlessingofProtection, 1022, "F11");
            //Buffs
            CombatRoutine.AddBuff(Consecration, 26573);
            CombatRoutine.AddBuff(CrusaderAura, 32223);
            CombatRoutine.AddBuff(DevotionAura, 465);
            CombatRoutine.AddBuff(AvengingWrath, 31884);
            CombatRoutine.AddBuff(Crusade, 231895);
            CombatRoutine.AddBuff(EmpyreanPower, 326733);
            CombatRoutine.AddBuff(DivinePurpose, 223817);
            CombatRoutine.AddBuff(ShieldofVengeance, 184662);
            CombatRoutine.AddBuff(DivineShield, 642);
            CombatRoutine.AddBuff(SelflessHealer, 114250);
            CombatRoutine.AddBuff(VanquishersHammer, 328204);
            CombatRoutine.AddBuff(HolyAvenger, 105809);
            CombatRoutine.AddBuff(Seraphim, 152262);

            //Debuffs
            CombatRoutine.AddDebuff(Forbearance, 25771);
            CombatRoutine.AddDebuff(Judgment, 197277);
            CombatRoutine.AddDebuff(ExecutionSentence, 343527);
            CombatRoutine.AddDebuff(FinalReckoning, 343721);
            CombatRoutine.AddDebuff(Mindgames, 323673);

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            CombatRoutine.AddMacro(LayOnHands + " MO", "F10");
            CombatRoutine.AddMacro(LayOnHands + " Focus", "F10");
            CombatRoutine.AddMacro(WordOfGlory + " MO", "F10");
            CombatRoutine.AddMacro(WordOfGlory + " Focus", "F10");
            CombatRoutine.AddMacro(BlessingofProtection + " MO", "F11");
            CombatRoutine.AddMacro(BlessingofProtection + " Focus", "F11");
            CombatRoutine.AddMacro(FlashofLight + " Focus", "F12");
            CombatRoutine.AddMacro(FlashofLight + " MO", "F12");

            CombatRoutine.AddConduit(RingingClarity);
            CombatRoutine.AddConduit("Golden Path");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            CombatRoutine.AddToggle("Small CDs");
            CombatRoutine.AddToggle("MO Heal");
            CombatRoutine.AddToggle("Focus Heal");
            //CBProperties
            CombatRoutine.AddProp("FOLOOCPCT", "Out of combat Life Percent", numbList, "Life percent at which Flash of Light is used out of combat to heal you between pulls", FlashofLight, 7);
            CombatRoutine.AddProp("FOLOOC", "Out of Combat Healing", true, "Should the bot use Flash of Light out of combat to heal you between pulls", FlashofLight);
            CombatRoutine.AddProp(FlashofLight + "player", "Selfless Healer Player", numbList, "Life percent at which " + FlashofLight + " is used with selfless healer procs, set to 0 to disable", "Defense - Flash of Light", 50);
            CombatRoutine.AddProp(FlashofLight + "focus", "Selfless Healer Focus", numbList, "Life percent at which " + FlashofLight + " is used with selfless healer procs, set to 0 to disable", "Defense - Flash of Light", 50);
            CombatRoutine.AddProp(FlashofLight + "mouseover", "Selfless Healer Mouseover", numbList, "Life percent at which " + FlashofLight + " is used with selfless healer procs, set to 0 to disable", "Defense - Flash of Light", 50);

            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);

            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsagewithAOEandSmallCDs, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 2);
            CombatRoutine.AddProp("UseSeraphim", "Use " + Seraphim, CDUsagewithAOEandSmallCDs, "Use " + Seraphim + " always, with Cooldowns", "Cooldowns", 2);
            CombatRoutine.AddProp("UseWakeofAshes", "Use " + "Wake of Ashes", CDUsagewithAOEandSmallCDs, "Use " + WakeofAshes + " always, with Cooldowns", "Cooldowns", 2);
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsagewithAOEandSmallCDs, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 2);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsagewithAOEandSmallCDs, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 2);
            CombatRoutine.AddProp("AURASWITCH", "Auto Aura Switch", true, "Auto Switch Aura between Crusader Aura and Devotion Aura", "Generic");

            CombatRoutine.AddProp(LayOnHands + "player", LayOnHands + " Player" + " Life Percent", numbList, "Life percent at which" + LayOnHands + "is used, set to 0 to disable", "Defense - Lay on Hands", 20);
            CombatRoutine.AddProp(LayOnHands + "focus", LayOnHands + " Focus" + " Life Percent", numbList, "Life percent at which" + LayOnHands + "is used, set to 0 to disable", "Defense - Lay on Hands", 20);
            CombatRoutine.AddProp(LayOnHands + "mouseover", LayOnHands + " Mouseover" + " Life Percent", numbList, "Life percent at which" + LayOnHands + "is used, set to 0 to disable", "Defense - Lay on Hands", 20);
            CombatRoutine.AddProp(Consecration, "Consecration | Golden Path" + " Life Percent", numbList, "Life percent at which" + Consecration + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(BlessingofProtection + "player", BlessingofProtection + " Player" + " Life Percent", numbList, "Life percent at which" + BlessingofProtection + "is used, set to 0 to disable", "Defense - Blessing of Protection", 20);
            CombatRoutine.AddProp(BlessingofProtection + "focus", BlessingofProtection + " Focus" + " Life Percent", numbList, "Life percent at which" + BlessingofProtection + "is used, set to 0 to disable", "Defense - Blessing of Protection", 20);
            CombatRoutine.AddProp(BlessingofProtection + "mouseover", BlessingofProtection + " Mouseover" + " Life Percent", numbList, "Life percent at which" + BlessingofProtection + "is used, set to 0 to disable", "Defense - Blessing of Protection", 20);

            CombatRoutine.AddProp(ShieldofVengeance, ShieldofVengeance + " Life Percent", numbList, "Life percent at which" + ShieldofVengeance + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp(DivineShield, DivineShield + " Life Percent", numbList, "Life percent at which" + DivineShield + "is used, set to 0 to disable", "Defense", 30);
            CombatRoutine.AddProp("WOGplayer", WordOfGlory + " Player", numbList, "Life percent at which Word of Glory is used", "Defense - Word of Glory", 50);
            CombatRoutine.AddProp("WOGfocus", WordOfGlory + " Focus", numbList, "Life percent at which Word of Glory is used", "Defense - Word of Glory", 50);
            CombatRoutine.AddProp("WOGmouseover", WordOfGlory + " Mouseover", numbList, "Life percent at which Word of Glory is used", "Defense - Word of Glory", 50);



        }

        public override void Pulse()
        {
            //API.WriteLog("blessing: " + API.CanCast(BlessingofSpring) + " " + API.CanCast(BlessingofSummer) + " " + API.CanCast(BlessingofAutumn) + " " + API.CanCast(BlessingofWinter));
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

            }

        }
        public override void CombatPulse()
        {
            if (isInterrupt && API.CanCast(Rebuke) && IsMelee && PlayerLevel >= 27)
            {
                API.CastSpell(Rebuke);
                return;
            }
            #region healing
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
            if (API.PlayerHealthPercent <= FlashofLightLifePercentProcPlayer && !API.PlayerHasDebuff(Mindgames) && API.CanCast(FlashofLight) && API.PlayerBuffStacks(SelflessHealer) >= 4 && PlayerLevel >= 4)
            {
                API.CastSpell(FlashofLight);
                return;
            }
            if (API.PlayerHealthPercent <= LayOnHandsPlayerLifePercent && API.CanCast(LayOnHands) && PlayerLevel >= 9 && !API.PlayerHasDebuff(Forbearance, false, false))
            {
                API.CastSpell(LayOnHands);
                return;
            }
            if (API.PlayerHealthPercent <= DivineShieldLifePercent && API.CanCast(DivineShield) && PlayerLevel >= 10 && !HasDefenseBuff && !API.PlayerHasDebuff(Forbearance, false, false))
            {
                API.CastSpell(DivineShield);
                return;
            }
            if (API.PlayerHealthPercent <= ShieldofVengeanceLifePercent && API.CanCast(ShieldofVengeance) && PlayerLevel >= 26 && !HasDefenseBuff)
            {
                API.CastSpell(ShieldofVengeance);
                return;
            }
            if (API.SpellIsCanbeCast(WordOfGlory) && !API.PlayerHasDebuff(Mindgames) && API.PlayerHealthPercent <= WordOfGloryPlayerLifePercent && API.CanCast(WordOfGlory) && PlayerLevel >= 7)
            {
                API.CastSpell(WordOfGlory);
                return;
            }
            if (API.CanCast(Consecration) && API.PlayerIsConduitSelected("Golden Path") && API.PlayerHealthPercent <= ConsecrationLifePercent)
            {
                API.CastSpell(Consecration);
                return;
            }
            if (HealFocus)
            {
                if (!API.MacroIsIgnored(FlashofLight + " Focus") && API.FocusHealthPercent <= FlashofLightLifePercentProcFocus && !API.FocusHasDebuff(Mindgames) && API.CanCast(FlashofLight) && API.FocusRange <= 40 && API.PlayerBuffStacks(SelflessHealer) >= 4 && PlayerLevel >= 4)
                {
                    API.CastSpell(FlashofLight + " Focus");
                    return;
                }
                if (!API.MacroIsIgnored(LayOnHands + " Focus") && API.FocusHealthPercent <= LayOnHandsFocusLifePercent && API.FocusRange <= 40 && API.CanCast(LayOnHands) && PlayerLevel >= 9 && !API.FocusHasDebuff(Forbearance, false, false))
                {
                    API.CastSpell(LayOnHands + " Focus");
                    return;
                }
                if (!API.MacroIsIgnored(WordOfGlory + " Focus") && API.SpellIsCanbeCast(WordOfGlory) && API.FocusRange <= 40 && !API.FocusHasDebuff(Mindgames) && API.FocusHealthPercent <= WordOfGloryFocusLifePercent && API.CanCast(WordOfGlory) && PlayerLevel >= 7)
                {
                    API.CastSpell(WordOfGlory + " Focus");
                    return;
                }
                if (!API.MacroIsIgnored(BlessingofProtection + " Focus") && API.FocusRange <= 40 && !API.FocusHasDebuff(Mindgames) && API.FocusHealthPercent <= BlessingofProtectionFocusPercent && API.CanCast(BlessingofProtection) && PlayerLevel >= 10)
                {
                    API.CastSpell(BlessingofProtection + " Focus");
                    return;
                }
            }
            if (IsMouseover)
            {
                if (!API.MacroIsIgnored(FlashofLight + " MO") && !API.PlayerCanAttackMouseover && API.MouseoverHealthPercent <= FlashofLightLifePercentProcMouseover && !API.MouseoverHasDebuff(Mindgames) && API.CanCast(FlashofLight) && API.PlayerBuffStacks(SelflessHealer) >= 4 && PlayerLevel >= 4)
                {
                    API.CastSpell(FlashofLight + " MO");
                    return;
                }
                if (!API.MacroIsIgnored(LayOnHands + " MO") && !API.PlayerCanAttackMouseover && API.MouseoverHealthPercent <= LayOnHandsMouseoverLifePercent && API.MouseoverRange <= 40 && API.CanCast(LayOnHands) && PlayerLevel >= 9 && !API.MouseoverHasDebuff(Forbearance, false, false))
                {
                    API.CastSpell(LayOnHands + " MO");
                    return;
                }
                if (!API.MacroIsIgnored(WordOfGlory + " MO") && !API.PlayerCanAttackMouseover && API.SpellIsCanbeCast(WordOfGlory) && API.MouseoverRange <= 40 && !API.MouseoverHasDebuff(Mindgames) && API.MouseoverHealthPercent <= WordOfGloryMouseoverLifePercent && API.CanCast(WordOfGlory) && PlayerLevel >= 7)
                {
                    API.CastSpell(WordOfGlory + " MO");
                    return;
                }
                if (!API.MacroIsIgnored(BlessingofProtection + " MO") && !API.PlayerCanAttackMouseover && API.SpellIsCanbeCast(BlessingofProtection) && API.MouseoverRange <= 40 && API.MouseoverHealthPercent <= BlessingofProtectionMouseoverPercent && API.CanCast(BlessingofProtection) && PlayerLevel >= 10)
                {
                    API.CastSpell(BlessingofProtection + " MO");
                    return;
                }
            }
            #endregion
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

            if (isRacial)
            {
                //actions.cooldowns +=/ lights_judgment,if= spell_targets.lights_judgment >= 2 )
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged Draenei" && IsMelee && API.PlayerUnitInMeleeRangeCount >= 2)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.cooldowns +=/ fireblood,if= buff.avenging_wrath.up | buff.crusade.up & buff.crusade.stack = 10
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && (PlayerHasBuff(AvengingWrath) || PlayerHasBuff(Crusade) && API.PlayerBuffStacks(Crusade) == 10) && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
            }
            //API.WriteLog("conduit "+ API.CanCast(HammerofWrath));
            if (IsCooldowns)
            {
                //cds->add_action(this, "Avenging Wrath", "if=(holy_power>=4&time<5|holy_power>=3&time>5|talent.holy_avenger.enabled&cooldown.holy_avenger.remains=0)&time_to_hpg=0");
                if (API.CanCast(AvengingWrath) && !PlayerHasBuff(AvengingWrath) && !Talent_Crusade && IsMelee && (holy_power >= 4 && time < 500 || holy_power >= 3 && time > 500) && PlayerLevel >= 37)
                {
                    API.CastSpell(AvengingWrath);
                    return;
                }
                //cds->add_talent(this, "Crusade", "if=(holy_power>=4&time<5|holy_power>=3&time>5|talent.holy_avenger.enabled&cooldown.holy_avenger.remains=0)&time_to_hpg=0");
                if (API.CanCast(Crusade) && !PlayerHasBuff(Crusade) && IsMelee && Talent_Crusade && (holy_power >= 4 && API.PlayerTimeInCombat < 500 || holy_power >= 3 && API.PlayerTimeInCombat > 500))
                {
                    API.CastSpell(Crusade);
                    return;
                }
                //cds->add_talent(this, "Holy Avenger", "if=time_to_hpg=0&((buff.avenging_wrath.up|buff.crusade.up)|(buff.avenging_wrath.down&cooldown.avenging_wrath.remains>40|buff.crusade.down&cooldown.crusade.remains>40))");
                if (API.CanCast(HolyAvenger) && IsMelee && Talent_HolyAvenger && ((PlayerHasBuff(AvengingWrath) || PlayerHasBuff(Crusade)) || API.SpellCDDuration(AvengingWrath) > API.TargetTimeToDie || API.TargetTimeToDie < API.SpellCDDuration(Crusade)))
                {
                    API.CastSpell(HolyAvenger);
                    return;
                }
            }
            //Final Reckoning with at least 3HP, and if if Avenging Wrath / Crusade are active OR remain on cooldown for greater than 10 seconds.
            if (Talent_FinalReckoning && (IsCooldowns || UseSmallCD) && !gcd_to_hpg && IsMelee && API.CanCast(FinalReckoning) && API.TargetRange < 30 && API.PlayerCurrentHolyPower >= 3 && (!IsCooldowns || Buff_or_CDmorethan(AvengingWrath, 1000) || Buff_or_CDmorethan(Crusade, 1000)) && (PlayerHasBuff(Seraphim) || !Talent_Seraphim))
            {
                API.CastSpell(FinalReckoning);
                return;
            }
            //generators->add_action("divine_toll,if=!debuff.judgment.up&(!raid_event.adds.exists|raid_event.adds.in>30)&(holy_power<=2|holy_power<=4&(cooldown.blade_of_justice.remains>gcd*2|debuff.execution_sentence.up|debuff.final_reckoning.up))&(!talent.final_reckoning.enabled|cooldown.final_reckoning.remains>gcd*10)&(!talent.execution_sentence.enabled|cooldown.execution_sentence.remains>gcd*10)");
            if (API.CanCast(DivineToll) && (holy_power <= 4 - ((Conduit_enabled(RingingClarity) || API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) ? 2 : 0)) && (!Conduit_enabled(RingingClarity) || !IsAOE || API.TargetUnitInRangeCount < AOEUnitNumber && IsAOE || (Conduit_enabled(RingingClarity) || API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && holy_power <= 2) && !TargetHasDebuff(Judgment) && (!Talent_ExecutionSentence || TargetHasDebuff(ExecutionSentence)) && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "With Small CDs" && UseSmallCD || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <= 30)
            {
                API.CastSpell(DivineToll);
                return;
            }
            //cds->add_action("ashen_hallow");
            if (API.CanCast(AshenHallow) && !API.PlayerIsMoving && API.TargetRange <= 30 && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "With Small CDs" && UseSmallCD || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(AshenHallow);
                return;
            }
            if (API.CanCast(VanquishersHammer) && API.TargetRange <= 30 && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "With Small CDs" && UseSmallCD || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(VanquishersHammer);
                return;
            }
            if (PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "With Small CDs" && UseSmallCD || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
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
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "With Small CDs" && UseSmallCD || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee)
            {
                API.CastSpell("Trinket1");
                return;
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "With Small CDs" && UseSmallCD || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee)
            {
                API.CastSpell("Trinket2");
                return;
            }
            //Seraphim if Avenging Wrath / Crusade are active OR remain on cooldown for greater than 25 seconds. &&(!talent.final_reckoning.enabled|cooldown.final_reckoning.remains<10)&(!talent.execution_sentence.enabled|cooldown.execution_sentence.remains<10)
            if (Talent_Seraphim && !gcd_to_hpg && API.CanCast(Seraphim) && UseSeraphim != "Not Used" && (UseSeraphim == "On Cooldown" || UseSeraphim == "With Cooldowns" && IsCooldowns || UseSeraphim == "With Small CDs" && UseSmallCD) && IsMelee && (!IsCooldowns || Buff_or_CDmorethan(AvengingWrath, 2500) || Buff_or_CDmorethan(Crusade, 2500)) && (!Talent_FinalReckoning || API.SpellCDDuration(FinalReckoning) < 1000) && (!Talent_ExecutionSentence || API.SpellCDDuration(ExecutionSentence) < 100))
            {
                API.CastSpell(Seraphim);
                return;
            }
            //Execution Sentence if Avenging Wrath / Crusade are active OR remain on cooldown for greater than 10 seconds.
            if (API.PlayerIsTalentSelected(1, 3) && (!Talent_FinalReckoning || TargetHasDebuff(FinalReckoning)) && (IsCooldowns || UseSmallCD) && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && API.CanCast(ExecutionSentence) && API.TargetRange < 30 && (!IsCooldowns || Buff_or_CDmorethan(AvengingWrath, 1000) || Buff_or_CDmorethan(Crusade, 1000)))
            {
                API.CastSpell(ExecutionSentence);
                return;
            }
            //Templar's Verdict with 5HP.
            if ((holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && holy_power >= 5 && IsMelee && PlayerLevel >= 10)
            {
                if (API.CanCast(DivineStorm) && IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerLevel >= 23)
                {
                    API.CastSpell(DivineStorm);
                    return;
                }
                else if (API.CanCast(TemplarsVerdict))
                {
                    API.CastSpell(TemplarsVerdict);
                    return;
                }
                else if (API.CanCast(FinalVerdict))
                {
                    API.CastSpell(FinalVerdict);
                    return;
                }
            }
            //Wake of Ashes at 0HP OR at 2HP or less if Blade of Justice remains on CD for greater than 2 GCDs. saved for Execution Sentence and/or Final Reckoning.
            if (API.CanCast(WakeofAshes) && (API.PlayerCurrentHolyPower == 0 || (API.PlayerCurrentHolyPower <= 2 && API.SpellCDDuration(BladeofJustice) > 2 * gcd)) && (!IsCooldowns || (!Talent_FinalReckoning || TargetHasDebuff(FinalReckoning)) && (TargetHasDebuff(ExecutionSentence) || !Talent_ExecutionSentence)) && IsMelee && PlayerLevel >= 39 && (UseWakeofAshes == "With Cooldowns" && IsCooldowns || UseWakeofAshes == "With Small CDs" && UseSmallCD || UseWakeofAshes == "On Cooldown" || UseWakeofAshes == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(WakeofAshes);
                return;
            }
            //Hammer of Wrath at 4HP or less.
            if (API.CanCast(HammerofWrath) && holy_power <= 4 && API.TargetRange <= 30 && PlayerLevel >= 46)
            {
                API.CastSpell(HammerofWrath);
                return;
            }
            //Blade of Justice at 3HP or less.
            if (API.CanCast(BladeofJustice) && API.TargetRange <= 12 && API.PlayerCurrentHolyPower <= 3 && PlayerLevel >= 19)
            {
                API.CastSpell(BladeofJustice);
                return;
            }
            //Judgment at 4HP or less and the Judgment debuff is not up.
            if (API.CanCast(Judgment) && !TargetHasDebuff(Judgment) && holy_power <= 4 && API.TargetRange <= 30 && PlayerLevel >= 3)
            {
                API.CastSpell(Judgment);
                return;
            }
            //Templar's Verdict if Avenging Wrath/Crusade are active, target is below 20% health, or with a Divine Purpose proc. Divine Storm with Empyrean Power proc.
            if ((holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && (PlayerHasBuff(AvengingWrath) || PlayerHasBuff(Crusade) || API.TargetHealthPercent < 20 || PlayerHasBuff(DivinePurpose)) && IsMelee && PlayerLevel >= 10)
            {
                if (API.CanCast(DivineStorm) && IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerLevel >= 23)
                {
                    API.CastSpell(DivineStorm);
                    return;
                }
                else if (API.CanCast(TemplarsVerdict))
                {
                    API.CastSpell(TemplarsVerdict);
                    return;
                }
                else if (API.CanCast(FinalVerdict))
                {
                    API.CastSpell(FinalVerdict);
                    return;
                }
            }
            //Divine Storm with Empyrean Power proc.
            if (API.CanCast(DivineStorm) && PlayerHasBuff(EmpyreanPower) && IsMelee && PlayerLevel >= 23)
            {
                API.CastSpell(DivineStorm);
                return;
            }
            //Crusader Strike at 2 Charges and at 4HP or less.
            if (API.CanCast(CrusaderStrike) && API.SpellCharges(CrusaderStrike) == 2 && holy_power <= 4 && IsMelee && holy_power <= 4)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }
            //Templar's Verdict at 4HP or less.
            if ((holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && holy_power <= 4 && IsMelee && PlayerLevel >= 10)
            {
                if (API.CanCast(DivineStorm) && IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerLevel >= 23)
                {
                    API.CastSpell(DivineStorm);
                    return;
                }
                else if (API.CanCast(TemplarsVerdict))
                {
                    API.CastSpell(TemplarsVerdict);
                    return;
                }
                else if (API.CanCast(FinalVerdict))
                {
                    API.CastSpell(FinalVerdict);
                    return;
                }
            }
            //Crusader Strike regardless of charges at 4HP or less.
            if (API.CanCast(CrusaderStrike) && holy_power <= 4 && IsMelee && holy_power <= 4)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }
            //actions.generators +=/ arcane_torrent,if= holy_power <= 4
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Blood Elf" && holy_power <= 4 && IsMelee && holy_power <= 4)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //Consecration if all HP builders are on CD for greater than or equal to 1 GCD.
            if (API.CanCast(Consecration) && IsMelee && gcd_to_hpg)
            {
                API.CastSpell(Consecration);
                return;
            }
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Tauren" && IsMelee && holy_power <= 4)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
        }
    }
}