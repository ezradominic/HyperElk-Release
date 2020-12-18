
namespace HyperElk.Core
{
    public class FireMage : CombatRoutine
    {
        //Spell Strings
        private string RoP = "Rune of Power";
        private string AI = "Arcane Intellect";
        private string Counterspell = "Counterspell";
        private string IB = "Ice Block";
        private string MI = "Mirror Image";
        private string BB = "Blazing Barrier";
        private string ShiftingPower = "Shifting Power";
        private string RadiantSpark = "Radiant Spark";
        private string Deathborne = "Deathborne";
        private string MirrorsofTorment = "Mirrors of Torment";
        private string Fleshcraft = "Fleshcraft";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        private string Firestorm = "Firestorm";
        private string TimeWarp = "Time Warp";
        private string Temp = "Temporal Displacement";
        private string Exhaustion = "Exhaustion";
        private string Fatigued = "Fatigued";
        private string BL = "Bloodlust";
        private string AH = "Ancient Hysteria";
        private string TW = "Temporal Warp";
        private string InfernalCascade = "Infernal Cascade";

        //Talents
        bool SearingTouch => API.PlayerIsTalentSelected(1, 3);
        bool BlastWave => API.PlayerIsTalentSelected(2, 3);
        bool RuneOfPower => API.PlayerIsTalentSelected(3, 3);
        bool FlameOn => API.PlayerIsTalentSelected(4, 1);
        bool AlexstraszaFury => API.PlayerIsTalentSelected(4, 2);
        bool FlamePatchTalent => API.PlayerIsTalentSelected(6, 1);
        bool LivingBomb => API.PlayerIsTalentSelected(6, 3);
        bool Kindling => API.PlayerIsTalentSelected(7, 1);
        bool Pyroclasm => API.PlayerIsTalentSelected(7, 2);
        bool Meteor => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        public string[] LegendaryList = new string[] { "None", "Temporal Warp" };
        private int BBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(BB)];
        private int IBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(IB)];
        private int MIPercentProc => percentListProp[CombatRoutine.GetPropertyInt(MI)];
        private int FleshcraftPercentProc => percentListProp[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private int Trinket1Usage => CombatRoutine.GetPropertyInt("Trinket1");
        private int Trinket2Usage => CombatRoutine.GetPropertyInt("Trinket2");
        private string UseLeg => LegendaryList[CombatRoutine.GetPropertyInt("Legendary")];
        //General
        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private string UseROP => CDUsage[CombatRoutine.GetPropertyInt(RoP)];
        private string UseCom => CDUsage[CombatRoutine.GetPropertyInt("Combustion")];

        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];

        //private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;


        bool ChannelingShift => API.CurrentCastSpellID("player") == 314791 && API.PlayerHasBuff(ShiftingPower);
        bool CastCombustion => API.PlayerLastSpell == "Combustion";
        private bool BLDebuffs => (!API.PlayerHasDebuff(Temp) || !API.PlayerHasDebuff(Exhaustion) || !API.PlayerHasDebuff(Fatigued));
        private bool BLBuFfs => (!API.PlayerHasBuff(BL) || !API.PlayerHasBuff(AH) || !API.PlayerHasBuff(TimeWarp) || !API.PlayerHasBuff(TW));
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsTimeWarp => API.ToggleIsEnabled("TimeWarp");
        private bool IsForceAOE => API.ToggleIsEnabled("ForceAOE");
        private bool IsSmallCD => API.ToggleIsEnabled("SmallCD");
        private bool CastRune => API.PlayerLastSpell == "Rune of Power";
        private bool CastFB => API.PlayerLastSpell == "Fire Blast";
        private bool CastPF => API.PlayerLastSpell == "Phoenix Flames";
        int FBTime => FlameOn ? 900 : 1200;
        float FBRecharge => FlameOn ? 900f : 1200f / (1f + API.PlayerGetHaste / 1);



        public override void Initialize()
        {
            CombatRoutine.Name = "Fire Mage by Ryu";
            API.WriteLog("Welcome to Fire Mage v1.7 by Ryu");
            API.WriteLog("Create the following cursor macro for Flamestrike and Meteor");
            API.WriteLog("Flamestrike -- /cast [@cursor] Flamestrike");
            API.WriteLog("Meteor -- /cast [@cursor] Meteor");
            API.WriteLog("Create Macro /cast [@player] Arcane Intellect to buff Arcane Intellect so you don't require a target");
            API.WriteLog("All Talents expect Ring of Frost and Alexstrasza's Fury supported. All Cooldowns are associated with Cooldown toggle.");
            API.WriteLog("Firestrom is auto supported, no need to select in Legendary List.");
            API.WriteLog("Fireblast and Pheonix Flames WILL not be used if you do not have SmallCd's toggle on, or when you have Combustion Buff.");
            //Buff
            CombatRoutine.AddBuff("Heating Up");
            CombatRoutine.AddBuff("Pyroclasm");
            CombatRoutine.AddBuff("Combustion");
            CombatRoutine.AddBuff("Hot Streak!");
            CombatRoutine.AddBuff("Rune of Power");
            CombatRoutine.AddBuff("Blazing Barrier");
            CombatRoutine.AddBuff("Arcane Intellect");
            CombatRoutine.AddBuff(Firestorm);
            CombatRoutine.AddBuff(TimeWarp);
            CombatRoutine.AddBuff(BL);
            CombatRoutine.AddBuff(AH);
            CombatRoutine.AddBuff(TW);
            CombatRoutine.AddBuff(InfernalCascade);
            CombatRoutine.AddBuff(ShiftingPower);

            //Debuff
            CombatRoutine.AddDebuff("Ignite");
            CombatRoutine.AddDebuff(Temp);
            CombatRoutine.AddDebuff(Fatigued);
            CombatRoutine.AddDebuff(Exhaustion);

            CombatRoutine.AddConduit(InfernalCascade);

            //Spell
            CombatRoutine.AddSpell("Rune of Power", "None");
            CombatRoutine.AddSpell("Ice Block");
            CombatRoutine.AddSpell("Frostbolt");
            CombatRoutine.AddSpell("Combustion");
            CombatRoutine.AddSpell("Dragon's Breath");
            CombatRoutine.AddSpell("Pyroblast");
            CombatRoutine.AddSpell("Fire Blast");
            CombatRoutine.AddSpell("Flamestrike");
            CombatRoutine.AddSpell("Fireball");
            CombatRoutine.AddSpell("Phoenix Flames");
            CombatRoutine.AddSpell("Scorch");
            CombatRoutine.AddSpell("Mirror Image");
            CombatRoutine.AddSpell("Meteor", "None");
            CombatRoutine.AddSpell("Living Bomb", "None");
            CombatRoutine.AddSpell("Blazing Barrier", "None");
            CombatRoutine.AddSpell("Hyperthread Wristwraps", "None");
            CombatRoutine.AddSpell("Counterspell", "None");
            CombatRoutine.AddSpell("Arcane Intellect", "None");
            CombatRoutine.AddSpell("Blast Wave");
            CombatRoutine.AddSpell(ShiftingPower);
            CombatRoutine.AddSpell(RadiantSpark);
            CombatRoutine.AddSpell(Deathborne);
            CombatRoutine.AddSpell(MirrorsofTorment);
            CombatRoutine.AddSpell(Fleshcraft);
            CombatRoutine.AddSpell(TimeWarp);

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);

            //Toggle
            CombatRoutine.AddToggle("SmallCD");
            CombatRoutine.AddToggle("TimeWarp");
            CombatRoutine.AddToggle("ForceAOE");


            //Prop
            CombatRoutine.AddProp(BB, BB, percentListProp, "Life percent at which " + BB + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(IB, IB, percentListProp, "Life percent at which " + IB + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(MI, MI, percentListProp, "Life percent at which " + MI + " is used, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", percentListProp, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 1);
            CombatRoutine.AddProp(RoP, "Use " + RoP, CDUsage, "Use " + RoP + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("Combustion", "Use " + "Combustion", CDUsage, "Use " + "Combustion" + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", CDUsage, "When should trinket1 be used", "Trinket", 0);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", CDUsage, "When should trinket1 be used", "Trinket", 0);
            CombatRoutine.AddProp("Legendary", "Select your Legendary", LegendaryList, "Select Your Legendary", "Legendary");


        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted)
            {
                if (API.CanCast("Arcane Intellect") && Level >= 8 && !API.PlayerHasBuff("Arcane Intellect"))
                {
                    API.CastSpell("Arcane Intellect");
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (isInterrupt && API.CanCast("Counterspell") && Level >= 7 && API.PlayerIsCasting(false) && NotChanneling && !ChannelingShift)
            {
                API.CastSpell("Counterspell");
                return;
            }
            if (API.CanCast("Ice Block") && API.PlayerHealthPercent <= IBPercentProc && API.PlayerHealthPercent != 0 && Level >= 22 && !ChannelingShift && NotChanneling)
            {
                API.CastSpell("Ice Block");
                return;
            }
            if (API.CanCast("Mirror Image") && API.PlayerHealthPercent <= MIPercentProc && API.PlayerHealthPercent != 0 && Level >= 44 && !ChannelingShift && NotChanneling)
            {
                API.CastSpell("Mirror Image");
                return;
            }
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && !ChannelingShift && NotChanneling)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.CanCast("Blazing Barrier") && Level >= 21 && !API.PlayerHasBuff("Blazing Barrier") && API.PlayerHealthPercent <= BBPercentProc && API.PlayerHealthPercent != 0 && !ChannelingShift && NotChanneling)
            {
                API.CastSpell("Blazing Barrier");
                return;
            }
            if (Trinket1Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !ChannelingShift && NotChanneling)
                API.CastSpell(trinket1);
            if (Trinket1Usage == 2 && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !ChannelingShift)
                API.CastSpell(trinket1);
            if (Trinket2Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !ChannelingShift && NotChanneling)
                API.CastSpell(trinket2);
            if (Trinket2Usage == 2 && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !ChannelingShift && NotChanneling)
                API.CastSpell(trinket2);
            if (Level <= 60)
            {
                rotation();
                return;
            }
        }

        public override void OutOfCombatPulse()
        {

        }

        private void rotation()
        {
            if (!ChannelingShift && NotChanneling)
            {
                if (IsTimeWarp && !API.PlayerIsCasting(true) && API.CanCast(TimeWarp) && (!API.PlayerHasDebuff(Temp) || !API.PlayerHasDebuff(Fatigued) || !API.PlayerHasDebuff(Exhaustion) || UseLeg == "Temporal Warp") && (!API.PlayerHasBuff(TW) || !API.PlayerHasBuff(AH) || !API.PlayerHasBuff(BL)))
                {
                    API.CastSpell(TimeWarp);
                    return;
                }
                if (API.CanCast(RadiantSpark) && InRange && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE))
                {
                    API.CastSpell(RadiantSpark);
                    return;
                }
                if (API.CanCast(MirrorsofTorment) && InRange && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE))
                {
                    API.CastSpell(MirrorsofTorment);
                    return;
                }
                if (API.CanCast(Deathborne) && InRange && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE))
                {
                    API.CastSpell(Deathborne);
                    return;
                }
                if (Meteor && !API.PlayerIsCasting(true) && API.CanCast("Meteor") && InRange && (!IsForceAOE || IsForceAOE) && NotChanneling)
                {
                    API.CastSpell("Meteor");
                    return;
                }
                if (API.CanCast("Combustion") && Level >= 29 && (!API.PlayerIsMoving || API.PlayerIsMoving) && (API.PlayerIsCasting(false) || API.PlayerIsCasting(true) && API.PlayerElapsedCastTimePercent <= 95) && API.TargetRange <= 40 && (IsCooldowns && UseCom == "With Cooldowns" || UseCom == "On Cooldown") && Level >= 29 && !API.PlayerHasBuff("Rune of Power") && API.SpellCharges("Fire Blast") >= 2)
                {
                    API.CastSpell("Combustion");
                    return;
                }
                if (API.CanCast("Living Bomb") && !API.PlayerIsCasting(true) && LivingBomb && (IsForceAOE || API.TargetUnitInRangeCount >= 2) && InRange)
                {
                    API.CastSpell("Living Bomb");
                    return;
                }
                if (API.CanCast("Flamestrike") && !API.PlayerIsCasting(true) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && (API.PlayerHasBuff("Combustion") || !API.PlayerHasBuff("Combustion")) && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 2 && IsAOE) || (IsForceAOE || API.TargetUnitInRangeCount >= 6 && IsAOE)) && Level >= 17)
                {
                    API.CastSpell("Flamestrike");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.CanCast("Pyroblast") && !API.PlayerIsCasting(true) && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && InRange && Level >= 12 && (API.PlayerIsMoving || !API.PlayerIsMoving) && (!IsAOE || !IsForceAOE || IsAOE && API.TargetUnitInRangeCount == 1))
                {
                    API.CastSpell("Pyroblast");
                    return;
                }
                // || API.SpellISOnCooldown("Combustion") && API.SpellCDDuration("Combustion") > FBRecharge*2 && !FlameOn
                if (API.CanCast("Fire Blast") && (IsForceAOE || IsAOE) && API.SpellCharges("Fire Blast") <= 3 && (!API.SpellISOnCooldown("Combustion") || API.SpellISOnCooldown("Combustion") && API.SpellCDDuration("Combustion") > FBRecharge*2) && (!API.PlayerHasBuff("Combustion") || API.PlayerHasBuff("Combustion")) && !API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && InRange && Level >= 33)
                 {
                     API.CastSpell("Fire Blast");
                     return;
                 }
                if (API.CanCast("Fire Blast") && (IsSmallCD || API.PlayerHasBuff("Combustion")) && API.SpellCharges("Fire Blast") > 0 && (API.PlayerHasBuff("Combustion") || !API.PlayerHasBuff("Combustion")) && (!API.SpellISOnCooldown("Combustion") || API.SpellISOnCooldown("Combustion") && API.SpellCDDuration("Combustion") > FBRecharge*2) && API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && InRange && Level >= 33 && (!IsForceAOE || IsForceAOE))
                {
                    API.CastSpell("Fire Blast");
                    return;
                }
                if (API.CanCast(RacialSpell1) && !API.PlayerIsCasting(true) && PlayerRaceSettings == "Troll" && isRacial && IsCooldowns && API.PlayerHasBuff("Combustion"))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.CanCast("Phoenix Flames") && !API.PlayerIsCasting(true) && InRange && API.PlayerHasBuff("Combustion") && API.SpellCharges("Phoenix Flames") > 0 && API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && API.SpellCharges("Fire Blast") == 0 && Level >= 19 && (!IsForceAOE || IsForceAOE))
                {
                    API.CastSpell("Phoenix Flames");
                    return;
                }
                if (API.CanCast("Phoenix Flames") && !API.PlayerIsCasting(true) && InRange && API.PlayerHasBuff("Combustion") && API.SpellCharges("Phoenix Flames") > 0 && !API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && API.SpellCharges("Fire Blast") == 0 && Level >= 19 && (!IsForceAOE || IsForceAOE))
                {
                    API.CastSpell("Phoenix Flames");
                    return;
                }
                if (API.CanCast("Phoenix Flames") && !API.PlayerIsCasting(true) && InRange && !API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && IsSmallCD && (API.SpellCharges("Phoenix Flames") >= 2 && API.SpellChargeCD("Phoenix Flames") <= 500 || API.TargetHasDebuff("Ignite") && (API.TargetUnitInRangeCount >= 3 && IsAOE || IsForceAOE)) && Level >= 19 && (!IsForceAOE || IsForceAOE) && !CastPF)
                {
                    API.CastSpell("Phoenix Flames");
                    return;
                }
                if (RuneOfPower && API.CanCast("Rune of Power") && !API.PlayerIsCasting(true) && API.TargetRange <= 40 && !CastCombustion && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && API.SpellCDDuration("Combustion") > 1200 && (IsCooldowns && UseROP == "With Cooldowns" || UseROP == "On Cooldown"))
                {
                    API.CastSpell("Rune of Power");
                    return;
                }
                if (API.CanCast(ShiftingPower) && PlayerCovenantSettings == "Night Fae" && ((API.TargetUnitInRangeCount >= 2 || IsForceAOE) && API.TargetRange <= 15 && API.PlayerHasBuff("Combustion") && API.PlayerBuffTimeRemaining("Combustion") <= 150 || API.SpellCDDuration("Combustion") >= 1600 && Kindling && !API.PlayerHasBuff("Combustion") || API.SpellCDDuration("Combustion") >= 1000 && !API.PlayerHasBuff("Combustion")) && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && API.SpellCharges("Fire Blast") <= 1 && ((API.TargetUnitInRangeCount >= 2 || IsForceAOE) && API.TargetRange <= 15 && API.PlayerHasBuff("Rune of Power") || API.PlayerHasBuff("Rune of Power") || !API.PlayerHasBuff("Rune of Power") || !RuneOfPower) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && !CastCombustion && !API.PlayerHasBuff(Firestorm) && !API.PlayerIsMoving)
                {
                    API.CastSpell(ShiftingPower);
                    return;
                }
                if (API.CanCast("Fire Blast") && (IsSmallCD || API.PlayerHasBuff("Combustion")) && API.SpellCharges("Fire Blast") > 0 && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff("Heating Up") && API.PlayerIsConduitSelected(InfernalCascade) && API.PlayerHasBuff("Combustion") && (!API.PlayerHasBuff(InfernalCascade) || API.PlayerHasBuff(InfernalCascade) && API.PlayerBuffTimeRemaining(InfernalCascade) < 300) && InRange && Level >= 33 && (!IsForceAOE || IsForceAOE))
                {
                    API.CastSpell("Fire Blast");
                    return;
                }
                if (BlastWave && API.CanCast("Blast Wave") && !API.PlayerIsCasting(true) && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 8 && (IsAOE || IsForceAOE))
                {
                    API.CastSpell("Blast Wave");
                    API.WriteLog("Blast Wave Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.CanCast("Dragon's Breath") && !API.PlayerIsCasting(true) && (API.PlayerIsInRaid ? API.TargetUnitInRangeCount >= 3 : API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE || IsForceAOE) && API.TargetRange <= 8 && (!API.PlayerHasBuff("Combustion") || API.PlayerHasBuff("Combustion") && API.PlayerBuffTimeRemaining("Combustion") <= 150))
                {
                    API.CastSpell("Dragon's Breath");
                    API.WriteLog("Dragon's Breath Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.CanCast("Pyroblast") && !API.PlayerIsCasting(true) && InRange && Pyroclasm && API.PlayerHasBuff("Pyroclasm") && !API.PlayerIsMoving && !API.PlayerHasBuff("Combustion") && Level >= 12)
                {
                    API.CastSpell("Pyroblast");
                    return;
                }
                if (API.CanCast("Scorch") && !API.PlayerIsCasting(true) && (!API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && API.SpellCharges("Fire Blast") == 0 && API.SpellCharges("Phoenix Flames") == 0 || API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && API.SpellCharges("Fire Blast") == 0 && API.SpellCharges("Phoenix Flames") == 0 || !API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && API.SpellCharges("Fire Blast") == 0 && API.SpellCharges("Phoenix Flames") == 0) && API.PlayerHasBuff("Combustion") && API.PlayerBuffTimeRemaining("Combustion") >= 80 && !API.PlayerHasBuff(Firestorm) && InRange && Level >= 13)
                {
                    API.CastSpell("Scorch");
                    return;
                }
                if (API.CanCast("Scorch") && !API.PlayerIsCasting(true) && !API.PlayerHasBuff(Firestorm) && SearingTouch && API.TargetHealthPercent <= 30 && (!API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && API.SpellCharges("Fire Blast") == 0) && InRange && Level >= 13  && !ChannelingShift)
                      {
                         API.CastSpell("Scorch");
                        API.WriteLog("Scorch under 30%");
                        return;
                   }
                //   if (API.CanCast("Scorch")  && SearingTouch && InRange && API.TargetHealthPercent <= 30 && (API.SpellCharges("Fire Blast") == 0 && API.SpellCharges("Phoenix Flames") == 0 || API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") || !API.PlayerHasBuff("Hot Streak!")) && (!API.PlayerHasBuff("Combustion") || API.PlayerHasBuff("Combustion")) && !API.PlayerHasBuff("Pyroclasm") && Level >= 19 && !ChannelingShift)
                //{
                //    API.CastSpell("Scorch");
                //    API.WriteLog("Scorch when no Fire Blast Charges");
                //    return;
                // }
                if (API.CanCast("Flamestrike") && !API.PlayerIsCasting(true) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm) || !API.PlayerHasBuff("Hot Streak!")) && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 2) || (IsForceAOE || API.TargetUnitInRangeCount >= 3)) && Level >= 17 && (IsAOE || IsForceAOE))
                {
                    API.CastSpell("Flamestrike");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.CanCast("Flamestrike") && !API.PlayerIsCasting(true) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm) || !API.PlayerHasBuff("Hot Streak!")) && API.PlayerHasBuff("Combustion") && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 2)) || (IsForceAOE || API.TargetUnitInRangeCount >= 6) && Level >= 17 && (IsForceAOE || IsAOE))
                {
                    API.CastSpell("Flamestrike");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.PlayerIsMoving && API.CanCast("Scorch") && !API.PlayerIsCasting(true) && InRange && Level >= 19 && !API.PlayerHasBuff(Firestorm))
                {
                    API.CastSpell("Scorch");
                    API.WriteLog("Scorch while moving");
                    return;
                }
                if (API.CanCast("Fireball") && !API.PlayerIsCasting(true) && !API.PlayerIsMoving && InRange && (API.TargetHealthPercent > 30.0 && SearingTouch || !SearingTouch) && (API.SpellCharges("Phoenix Flames") >= 0 && API.SpellCharges("Fire Blast") >= 0 || !API.PlayerHasBuff("Heating Up") || API.SpellCharges("Phoenix Flames") >= 0 && API.SpellCharges("Fire Blast") >= 0 && API.PlayerHasBuff("Heating Up")) && !API.PlayerHasBuff("Combustion") && Level >= 10 && !API.PlayerHasBuff(Firestorm))
                {
                    API.CastSpell("Fireball");
                    return;
                }
            }
        }
    }
}


