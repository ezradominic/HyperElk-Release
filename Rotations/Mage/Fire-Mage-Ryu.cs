
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
        private string Trinket1 = "Trinket1";
        private string Trinket2 = "Trinket2";
        private string Firestorm = "Firestorm";
        private string TimeWarp = "Time Warp";
        private string Temp = "Temporal Displacement";
        private string Exhaustion = "Exhaustion";
        private string Fatigued = "Fatigued";
        private string BL = "Bloodlust";
        private string AH = "Ancient Hysteria";
        private string TW = "Temporal Warp";
        private string InfernalCascade = "Infernal Cascade";
        private string Sated = "Sated";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string Spellsteal = "Spellsteal";
        private string RemoveCurse = "Remove Curse";
        private string SoulIgnite = "Soul Ignition";
        private string Quake = "Quake";

        //Talents
        bool FireStarter => API.PlayerIsTalentSelected(1, 1);
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

        //Spell Steal & Curse Removal
        string[] SpellSpealBuffList = { "Bless Weapon", "Death's Embrace", "Turn to Stone", "Wonder Grow", "Stoneskin" };
        string[] CurseList = { "Sintouched Anima", "Curse of Stone" };

        //CBProperties
        public string[] LegendaryList = new string[] { "None", "Temporal Warp" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        private int BBPercentProc => numbList[CombatRoutine.GetPropertyInt(BB)];
        private int IBPercentProc => numbList[CombatRoutine.GetPropertyInt(IB)];
        private int MIPercentProc => numbList[CombatRoutine.GetPropertyInt(MI)];
        private bool QuakingHelper => CombatRoutine.GetPropertyBool("QuakingHelper");
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        private string UseLeg => LegendaryList[CombatRoutine.GetPropertyInt("Legendary")];
        //General
        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private string UseROP => CDUsage[CombatRoutine.GetPropertyInt(RoP)];
        private string UseCom => CDUsage[CombatRoutine.GetPropertyInt("Combustion")];
        bool IsTrinkets1 => (UseTrinket1 == "With Cooldowns" && IsCooldowns & API.PlayerHasBuff("Combustion") && (!API.PlayerHasBuff(SoulIgnite) || API.PlayerHasBuff(SoulIgnite) && API.PlayerBuffTimeRemaining(SoulIgnite) <= 800)  || UseTrinket1 == "On Cooldown" && (!API.PlayerHasBuff(SoulIgnite) || API.PlayerHasBuff(SoulIgnite) && API.PlayerBuffTimeRemaining(SoulIgnite) <= 800) || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= 2 && (IsAOE || IsForceAOE));
        bool IsTrinkets2 => (UseTrinket2 == "With Cooldowns" && IsCooldowns && API.PlayerHasBuff("Combustion") && (!API.PlayerHasBuff(SoulIgnite) || API.PlayerHasBuff(SoulIgnite) && API.PlayerBuffTimeRemaining(SoulIgnite) <= 800) || UseTrinket2 == "On Cooldown" && (!API.PlayerHasBuff(SoulIgnite) || API.PlayerHasBuff(SoulIgnite) && API.PlayerBuffTimeRemaining(SoulIgnite) <= 800) || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= 2 && (IsAOE || IsForceAOE));

        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];

        //private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool Quaking => (API.PlayerCurrentCastTimeRemaining > API.PlayerDebuffRemainingTime(Quake) || API.PlayerCurrentCastTimeRemaining > API.PlayerBuffTimeRemaining(Quake)) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingFireBall => (API.PlayerDebuffRemainingTime(Quake) > FireballCastTime || API.PlayerBuffTimeRemaining(Quake) > FireballCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingFlamestrike => (API.PlayerDebuffRemainingTime(Quake) > FlamestrikeCastTime || API.PlayerBuffTimeRemaining(Quake) > FlamestrikeCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingScorch => (API.PlayerDebuffRemainingTime(Quake) > ScorchCastTime || API.PlayerBuffTimeRemaining(Quake) > ScorchCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingShifting => (API.PlayerDebuffRemainingTime(Quake) > ShiftingPowerCastTime || API.PlayerBuffTimeRemaining(Quake) > ShiftingPowerCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingMirrors => (API.PlayerDebuffRemainingTime(Quake) > MirrorsofTormentCastTime || API.PlayerBuffTimeRemaining(Quake) > MirrorsofTormentCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingRadiant => (API.PlayerDebuffRemainingTime(Quake) > RadiantSparkCastTime || API.PlayerBuffTimeRemaining(Quake) > RadiantSparkCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingDeathborne => (API.PlayerDebuffRemainingTime(Quake) > DeathborneCastTime || API.PlayerBuffTimeRemaining(Quake) > DeathborneCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingPyro => (API.PlayerDebuffRemainingTime(Quake) > PyroBlastCastTime || API.PlayerBuffTimeRemaining(Quake) > PyroBlastCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        private bool QuakingRune => (API.PlayerDebuffRemainingTime(Quake) > RuneCastTime || API.PlayerBuffTimeRemaining(Quake) > RuneCastTime) && (API.PlayerHasDebuff(Quake) || API.PlayerHasBuff(Quake));
        bool ChannelingShift => API.CurrentCastSpellID("player") == 314791 && API.PlayerHasBuff(ShiftingPower);
        bool CastCombustion => API.PlayerLastSpell == "Combustion";
        bool CastingScorch => API.CurrentCastSpellID("player") == 2948;
        bool CastingFlame => API.CurrentCastSpellID("player") == 2120;
        private bool BLDebuffs => (!API.PlayerHasDebuff(Temp) || !API.PlayerHasDebuff(Exhaustion) || !API.PlayerHasDebuff(Fatigued));
        private bool BLBuFfs => (!API.PlayerHasBuff(BL) || !API.PlayerHasBuff(AH) || !API.PlayerHasBuff(TimeWarp) || !API.PlayerHasBuff(TW));
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsTimeWarp => API.ToggleIsEnabled("TimeWarp");
        private bool IsForceAOE => API.ToggleIsEnabled("ForceAOE");
        private bool IsSmallCD => API.ToggleIsEnabled("SmallCD");
        private bool CastRune => API.PlayerLastSpell == "Rune of Power";
        private bool CastFB => API.PlayerLastSpell == "Fire Blast";
        private bool CastPF => API.PlayerLastSpell == "Phoenix Flames";
        private bool CastScorch => API.PlayerLastSpell == "Scorch";
        int FBTime => FlameOn ? 900 : 1200;
        float FBRecharge => FlameOn ? 900f : 1200f / (1f + API.PlayerGetHaste / 1);
        float FireballCastTime => 225f / (1f + API.PlayerGetHaste);
        float ScorchCastTime => 150f / (1f + API.PlayerGetHaste);
        float FlamestrikeCastTime => 400f / (1f + API.PlayerGetHaste);
        float ShiftingPowerCastTime => 400f / (1f + API.PlayerGetHaste);
        float RadiantSparkCastTime => 150f / (1f + API.PlayerGetHaste);
        float MirrorsofTormentCastTime => 150f / (1f + API.PlayerGetHaste);
        float DeathborneCastTime => 150f / (1f + API.PlayerGetHaste);
        float PyroBlastCastTime => 450f / (1f + API.PlayerGetHaste);
        float RuneCastTime => 150f / (1f + API.PlayerGetHaste);





        public override void Initialize()
        {
            CombatRoutine.Name = "Fire Mage by Ryu";
            API.WriteLog("Welcome to Fire Mage v1.8 by Ryu");
            API.WriteLog("Create the following cursor macro for Flamestrike and Meteor");
            API.WriteLog("Flamestrike -- /cast [@cursor] Flamestrike -- Or you may go ahead and not use @cursor, the program will pause until you place it yourself");
            API.WriteLog("Meteor -- /cast [@cursor] Meteor -- Or you may go ahead and not use @cursor, the program will pause until you place it yourself");
            API.WriteLog("For the Quaking helper you just need to create an ingame macro with /stopcasting and bind it under the Macros Tab in Elk :-)");
            API.WriteLog("Create Macro /cast [@player] Arcane Intellect to buff Arcane Intellect so you don't require a target");
            API.WriteLog("Please create a /stopcasting Macro for Pyroblast/Flamestrike and bind both to different keys, one normal and one marco for each spell. It is for the triple scorch bug. Without it, you will cast Scorch three times isntead of twice.");
            API.WriteLog("All Talents expect Ring of Frost and Alexstrasza's Fury supported. All Cooldowns are associated with Cooldown toggle.");
            API.WriteLog("Firestrom is auto supported, no need to select in Legendary List.");
            API.WriteLog("Fireblast and Pheonix Flames WILL not be used if you do not have SmallCd's toggle on, or when you have Combustion Buff.");
            API.WriteLog("If you have Trinkets used With Cooldowns, it will only ever cast them while you have Combustion Buff.");
            API.WriteLog("Rotation supports Auto Spellsteal for certain buffs and auto Remove Curse for certian curses along with Mouseover Support for them, please create the correct Mouseover Marco if you wish to use it. If you DONT want it do that, please check Ignore in the keybinds for SpellSteal/Remove Curse");
            //Buff
            CombatRoutine.AddBuff("Heating Up", 48107);
            CombatRoutine.AddBuff("Pyroclasm", 269651);
            CombatRoutine.AddBuff("Combustion", 190319);
            CombatRoutine.AddBuff("Hot Streak!", 48108);
            CombatRoutine.AddBuff("Rune of Power", 116014);
            CombatRoutine.AddBuff("Blazing Barrier", 235313);
            CombatRoutine.AddBuff("Arcane Intellect", 1459);
            CombatRoutine.AddBuff(Firestorm, 333100);
            CombatRoutine.AddBuff(TimeWarp, 80353);
            CombatRoutine.AddBuff(BL, 2825);
            CombatRoutine.AddBuff(AH, 90355);
            CombatRoutine.AddBuff(TW, 327351);
            CombatRoutine.AddBuff(InfernalCascade, 336821);
            CombatRoutine.AddBuff(ShiftingPower, 314791);
            CombatRoutine.AddBuff("Bless Weapon", 328288);
            CombatRoutine.AddBuff("Death's Embrace", 333875);
            CombatRoutine.AddBuff("Turn to Stone", 326607);
            CombatRoutine.AddBuff("Wonder Grow", 328016);
            CombatRoutine.AddBuff("Stoneskin", 322433);
            CombatRoutine.AddBuff(SoulIgnite, 345211);
            CombatRoutine.AddBuff(Quake, 240447);

            //Debuff
            CombatRoutine.AddDebuff("Ignite", 12654);
            CombatRoutine.AddDebuff(Temp, 80354);
            CombatRoutine.AddDebuff(Fatigued, 264689);
            CombatRoutine.AddDebuff(Exhaustion, 57723);
            CombatRoutine.AddDebuff(Sated, 57724);
            CombatRoutine.AddDebuff("Sintouched Anima", 328494);
            CombatRoutine.AddDebuff("Curse of Stone", 319603);
            CombatRoutine.AddDebuff(Quake, 240447);

            CombatRoutine.AddConduit(InfernalCascade);

            //Spell
            CombatRoutine.AddSpell("Rune of Power", 116011, "None");
            CombatRoutine.AddSpell("Ice Block", 45438);
            CombatRoutine.AddSpell("Frostbolt", 116);
            CombatRoutine.AddSpell("Combustion", 190319);
            CombatRoutine.AddSpell("Dragon's Breath", 31661);
            CombatRoutine.AddSpell("Pyroblast", 11366);
            CombatRoutine.AddSpell("Fire Blast", 108853);
            CombatRoutine.AddSpell("Flamestrike", 2120);
            CombatRoutine.AddSpell("Fireball", 133);
            CombatRoutine.AddSpell("Phoenix Flames", 257541);
            CombatRoutine.AddSpell("Scorch", 2948);
            CombatRoutine.AddSpell("Mirror Image", 55342);
            CombatRoutine.AddSpell("Meteor", 153561, "None");
            CombatRoutine.AddSpell("Living Bomb", 44457, "None");
            CombatRoutine.AddSpell("Blazing Barrier", 235313, "None");
            CombatRoutine.AddSpell("Counterspell", 2139, "None");
            CombatRoutine.AddSpell("Arcane Intellect", 1459, "None");
            CombatRoutine.AddSpell("Blast Wave", 157981);
            CombatRoutine.AddSpell(ShiftingPower, 314791);
            CombatRoutine.AddSpell(RadiantSpark, 307443);
            CombatRoutine.AddSpell(Deathborne, 324220);
            CombatRoutine.AddSpell(MirrorsofTorment, 314793);
            CombatRoutine.AddSpell(Fleshcraft, 324631);
            CombatRoutine.AddSpell(TimeWarp, 80353);
            CombatRoutine.AddSpell(Spellsteal, 30449);
            CombatRoutine.AddSpell(RemoveCurse, 475);

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Macro
            CombatRoutine.AddMacro(Trinket1);
            CombatRoutine.AddMacro(Trinket2);
            CombatRoutine.AddMacro(RemoveCurse + "MO");
            CombatRoutine.AddMacro(Spellsteal + "MO");
            CombatRoutine.AddMacro("Pyroblast" + "Stop");
            CombatRoutine.AddMacro("Flamestrike" + "Stop");
            CombatRoutine.AddMacro(Counterspell + "Focus");
            CombatRoutine.AddMacro("Stopcast", "F10");

            //Toggle
            CombatRoutine.AddToggle("SmallCD");
            CombatRoutine.AddToggle("TimeWarp");
            CombatRoutine.AddToggle("ForceAOE");
            CombatRoutine.AddToggle("Mouseover");


            //Prop
            CombatRoutine.AddProp(BB, BB, numbList, "Life percent at which " + BB + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(IB, IB, numbList, "Life percent at which " + IB + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(MI, MI, numbList, "Life percent at which " + MI + " is used, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 1);
            CombatRoutine.AddProp(RoP, "Use " + RoP, CDUsage, "Use " + RoP + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("QuakingHelper", "Quaking Helper", false, "Will cancel casts on Quaking", "Generic");
            CombatRoutine.AddProp("Combustion", "Use " + "Combustion", CDUsage, "Use " + "Combustion" + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
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
            if (API.PlayerCurrentCastTimeRemaining > 40 && QuakingHelper && Quaking)
            {
                API.CastSpell("Stopcast");
                return;
            }
            if (isInterrupt && API.CanCast("Counterspell") && Level >= 7 && !API.PlayerIsCasting(true) && NotChanneling && !ChannelingShift)
            {
                API.CastSpell("Counterspell");
                return;
            }
            if (API.CanCast(Counterspell) && CombatRoutine.GetPropertyBool("KICK") && API.FocusCanInterrupted && API.FocusIsCasting() && (API.FocusIsChanneling ? API.FocusElapsedCastTimePercent >= interruptDelay : API.FocusCurrentCastTimeRemaining <= interruptDelay))
            {
                API.CastSpell(Counterspell + "Focus");
                return;
            }
            if (API.CanCast(Spellsteal) && !API.PlayerIsCasting(true) && !ChannelingShift && NotChanneling)
            {
                for (int i = 0; i < SpellSpealBuffList.Length; i++)
                {
                    if (API.TargetHasBuff(SpellSpealBuffList[i]))
                    {
                        API.CastSpell(Spellsteal);
                        return;
                    }
                }
            }
            if (API.CanCast(Spellsteal) && IsMouseover && !API.PlayerIsCasting(true) && !ChannelingShift && NotChanneling)
            {
                for (int i = 0; i < SpellSpealBuffList.Length; i++)
                {
                    if (API.MouseoverHasBuff(SpellSpealBuffList[i]))
                    {
                        API.CastSpell(Spellsteal + "MO");
                        return;
                    }
                }
            }
            if (API.CanCast(RemoveCurse) && !API.PlayerCanAttackTarget && !API.PlayerIsCasting(true) && !ChannelingShift && NotChanneling)
            {
                for (int i = 0; i < CurseList.Length; i++)
                {
                    if (API.TargetHasDebuff(CurseList[i]))
                    {
                        API.CastSpell(RemoveCurse);
                        return;
                    }
                }
            }
            if (API.CanCast(RemoveCurse) && !API.PlayerCanAttackMouseover && IsMouseover && !API.PlayerIsCasting(true) && !ChannelingShift && NotChanneling)
            {
                for (int i = 0; i < CurseList.Length; i++)
                {
                    if (API.MouseoverHasDebuff(CurseList[i]))
                    {
                        API.CastSpell(RemoveCurse + "MO");
                        return;
                    }
                }
            }
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
            if (API.CanCast("Mirror Image") && API.PlayerHealthPercent <= MIPercentProc && API.PlayerHealthPercent != 0 && Level >= 44 && !ChannelingShift && NotChanneling)
            {
                API.CastSpell("Mirror Image");
                return;
            }
            if (API.CanCast("Ice Block") && API.PlayerHealthPercent <= IBPercentProc && API.PlayerHealthPercent != 0 && Level >= 22 && !ChannelingShift && NotChanneling)
            {
                API.CastSpell("Ice Block");
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
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1 && !ChannelingShift && NotChanneling)
            {
                API.CastSpell(Trinket1);
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2 && !ChannelingShift && NotChanneling)
            {
                API.CastSpell(Trinket2);
            }
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
            if (!ChannelingShift && NotChanneling && !API.PlayerSpellonCursor)
            {
                if (IsTimeWarp && !API.PlayerIsCasting(true) && API.CanCast(TimeWarp) && (!API.PlayerHasDebuff(Temp) || !API.PlayerHasDebuff(Fatigued) || !API.PlayerHasDebuff(Exhaustion) || UseLeg == "Temporal Warp") && (!API.PlayerHasBuff(TW) || !API.PlayerHasBuff(AH) || !API.PlayerHasBuff(BL)))
                {
                    API.CastSpell(TimeWarp);
                    return;
                }
                if (API.CanCast(RadiantSpark) && InRange && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && (!QuakingRadiant || QuakingRadiant && QuakingHelper))
                {
                    API.CastSpell(RadiantSpark);
                    return;
                }
                if (API.CanCast(MirrorsofTorment) && InRange && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && (!QuakingMirrors || QuakingMirrors && QuakingHelper))
                {
                    API.CastSpell(MirrorsofTorment);
                    return;
                }
                if (API.CanCast(Deathborne) && InRange && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && (!QuakingDeathborne || QuakingDeathborne && QuakingHelper))
                {
                    API.CastSpell(Deathborne);
                    return;
                }
                if (Meteor && !API.PlayerIsCasting(true) && API.CanCast("Meteor") && InRange && (!IsForceAOE || IsForceAOE) && NotChanneling)
                {
                    API.CastSpell("Meteor");
                    return;
                }
                if (API.CanCast("Combustion") && Level >= 29 && (!API.PlayerIsMoving || API.PlayerIsMoving) && (API.PlayerIsCasting(false) || API.PlayerElapsedCastTimePercent <= 85) && API.TargetRange <= 40 && (IsCooldowns && UseCom == "With Cooldowns" || UseCom == "On Cooldown") && Level >= 29 && !API.PlayerHasBuff("Rune of Power") && (FireStarter && API.TargetHealthPercent < 90 || !FireStarter))
                {
                    API.CastSpell("Combustion");
                    return;
                }
                if (API.CanCast("Living Bomb") && !API.PlayerIsCasting(true) && LivingBomb && (IsForceAOE || API.TargetUnitInRangeCount >= 2) && InRange)
                {
                    API.CastSpell("Living Bomb");
                    return;
                }
                if (API.CanCast("Flamestrike") && !API.PlayerIsCasting(true) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && API.PlayerHasBuff("Combustion") && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 3 && IsAOE) || IsForceAOE || API.TargetUnitInRangeCount >= 6 && IsAOE) && Level >= 17)
                {
                    API.CastSpell("Flamestrike");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.CanCast("Flamestrike") && !API.PlayerIsCasting(true) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && !API.PlayerHasBuff("Combustion") && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 3 && IsAOE) || IsForceAOE || API.TargetUnitInRangeCount >= 3 && IsAOE) && Level >= 17)
                {
                    API.CastSpell("Flamestrike");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if ((CastingScorch || CastingFlame) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && API.PlayerHasBuff("Combustion") && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 3 && IsAOE) || IsForceAOE || API.TargetUnitInRangeCount >= 6 && IsAOE) && Level >= 17 && !API.MacroIsIgnored("Flamestrike" + "Stop"))
                {
                    API.CastSpell("Flamestrike" + "Stop");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if ((CastingScorch || CastingFlame) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && !API.PlayerHasBuff("Combustion") && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 3 && IsAOE) || IsForceAOE || API.TargetUnitInRangeCount >= 3 && IsAOE) && Level >= 17 && !API.MacroIsIgnored("Flamestrike" + "Stop"))
                {
                    API.CastSpell("Flamestrike" + "Stop");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.CanCast("Pyroblast") && !API.PlayerIsCasting(true) && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && InRange && Level >= 12 && (API.PlayerIsMoving || !API.PlayerIsMoving) && (!IsAOE || !IsForceAOE || IsAOE && API.TargetUnitInRangeCount == 1))
                {
                    API.CastSpell("Pyroblast");
                    return;
                }
                if (CastingScorch && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && InRange && Level >= 12 && (API.PlayerIsMoving || !API.PlayerIsMoving) && (!IsAOE || !IsForceAOE || IsAOE && API.TargetUnitInRangeCount == 1) && !API.MacroIsIgnored("Pyroblast" + "Stop"))
                {
                    API.CastSpell("Pyroblast" + "Stop");
                    return;
                }
                if (API.CanCast("Pyroblast") && !API.PlayerIsCasting(true) && InRange && Pyroclasm && API.PlayerHasBuff("Pyroclasm") && !API.PlayerIsMoving && !API.PlayerHasBuff("Combustion") && Level >= 12 && (!QuakingPyro || QuakingPyro && QuakingHelper))
                {
                    API.CastSpell("Pyroblast");
                    return;
                }
                // || API.SpellISOnCooldown("Combustion") && API.SpellCDDuration("Combustion") > FBRecharge*2 && !FlameOn
                if (API.CanCast("Fire Blast") && (IsSmallCD || API.PlayerHasBuff("Combustion")) && API.SpellCharges("Fire Blast") == 3 && (!API.SpellISOnCooldown("Combustion") || API.SpellISOnCooldown("Combustion") && API.SpellCDDuration("Combustion") > FBRecharge*2) && (!API.PlayerHasBuff("Combustion") || API.PlayerHasBuff("Combustion")) && !API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && InRange && Level >= 33 && !CastingScorch)
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
                if (API.CanCast("Phoenix Flames") && !API.PlayerIsCasting(true) && InRange && API.PlayerHasBuff("Combustion") && API.SpellCharges("Phoenix Flames") > 2 && API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && API.SpellCharges("Fire Blast") == 0 && Level >= 19 && (!IsForceAOE || IsForceAOE))
                {
                    API.CastSpell("Phoenix Flames");
                    return;
                }
                if (API.CanCast("Phoenix Flames") && !API.PlayerIsCasting(true) && InRange && API.PlayerHasBuff("Combustion") && API.SpellCharges("Phoenix Flames") > 2 && !API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && API.SpellCharges("Fire Blast") == 0 && Level >= 19 && (!IsForceAOE || IsForceAOE))
                {
                    API.CastSpell("Phoenix Flames");
                    return;
                }
                if (RuneOfPower && API.CanCast("Rune of Power") && !API.PlayerIsCasting(true) && API.TargetRange <= 40 && !CastCombustion && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && API.SpellCDDuration("Combustion") > 1200 && (IsCooldowns && UseROP == "With Cooldowns" || UseROP == "On Cooldown") && (!QuakingRune || QuakingRune && QuakingHelper))
                {
                    API.CastSpell("Rune of Power");
                    return;
                }
                if (API.CanCast(ShiftingPower) && PlayerCovenantSettings == "Night Fae" && (API.TargetUnitInRangeCount >= 3 && IsAOE || IsForceAOE) && API.TargetRange <= 15 && (API.PlayerHasBuff("Combustion") && API.PlayerBuffTimeRemaining("Combustion") <= 750 || API.SpellCDDuration("Combustion") >= 1600 && Kindling && !API.PlayerHasBuff("Combustion") || API.SpellCDDuration("Combustion") >= 1000 && !API.PlayerHasBuff("Combustion")) && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff(Firestorm) && API.SpellCharges("Fire Blast") <= 1 && (API.PlayerHasBuff("Rune of Power") || !API.PlayerHasBuff("Rune of Power") || !RuneOfPower) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && !CastCombustion && !API.PlayerHasBuff(Firestorm) && !API.PlayerIsMoving && (!QuakingShifting || QuakingShifting && QuakingHelper))
                {
                    API.CastSpell(ShiftingPower);
                    return;
                }
                if (API.CanCast(ShiftingPower) && PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration("Combustion") >= 1600 && Kindling && !API.PlayerHasBuff("Combustion") || API.SpellCDDuration("Combustion") >= 1100 && !API.PlayerHasBuff("Combustion") && !Kindling) && !API.PlayerHasBuff("Hot Streak!") && API.SpellCharges("Fire Blast") <= 2 && (API.PlayerHasBuff("Rune of Power") || !API.PlayerHasBuff("Rune of Power") && !RuneOfPower) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && !CastCombustion && !API.PlayerHasBuff(Firestorm) && !API.PlayerIsMoving && (!QuakingShifting || QuakingShifting && QuakingHelper))
                {
                    API.CastSpell(ShiftingPower);
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
                if (API.CanCast("Scorch") && Level >= 19 && InRange && !API.PlayerIsCasting(true) && (!API.PlayerHasBuff("Heating Up") || API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!")) && !API.PlayerHasBuff(Firestorm) && ((!API.PlayerHasBuff("Combustion") || API.PlayerHasBuff("Combustion")) && SearingTouch && API.TargetHealthPercent <= 30 && API.TargetHealthPercent > 0 || !SearingTouch && API.PlayerHasBuff("Combustion") || SearingTouch && API.PlayerHasBuff("Combustion")) && (!QuakingScorch || QuakingScorch && QuakingHelper))
                {
                    API.CastSpell("Scorch");
                    return;
                }
                if (API.CanCast("Flamestrike") && !API.PlayerIsCasting(true) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm) || !API.PlayerHasBuff("Hot Streak!")) && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 3) || !FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 3)) && Level >= 17 && (IsAOE || IsForceAOE) && (!QuakingFlamestrike || QuakingFlamestrike && QuakingHelper))
                {
                    API.CastSpell("Flamestrike");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.CanCast("Flamestrike") && !API.PlayerIsCasting(true) && InRange && (API.PlayerHasBuff("Hot Streak!") || API.PlayerHasBuff(Firestorm)) && API.PlayerHasBuff("Combustion") && (FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 3)) || !FlamePatchTalent && (IsForceAOE || API.TargetUnitInRangeCount >= 6) && Level >= 17 && (IsForceAOE || IsAOE) && (!QuakingFlamestrike || QuakingFlamestrike && QuakingHelper))
                {
                    API.CastSpell("Flamestrike");
                    API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                    return;
                }
                if (API.PlayerIsMoving && API.CanCast("Scorch") && !API.PlayerIsCasting(true) && InRange && Level >= 19 && !API.PlayerHasBuff(Firestorm) && (!QuakingScorch || QuakingScorch && QuakingHelper))
                {
                    API.CastSpell("Scorch");
                    API.WriteLog("Scorch while moving");
                    return;
                }
                if (API.CanCast("Fireball") && !API.PlayerIsCasting(true) && !API.PlayerIsMoving && InRange && (API.TargetHealthPercent > 30.0 && SearingTouch || !SearingTouch) && (API.SpellCharges("Phoenix Flames") >= 0 && API.SpellCharges("Fire Blast") >= 0 || !API.PlayerHasBuff("Heating Up") || API.SpellCharges("Phoenix Flames") >= 0 && API.SpellCharges("Fire Blast") >= 0 && API.PlayerHasBuff("Heating Up")) && !API.PlayerHasBuff("Combustion") && Level >= 10 && !API.PlayerHasBuff(Firestorm) && (!QuakingFireBall || QuakingFireBall && QuakingHelper))
                {
                    API.CastSpell("Fireball");
                    return;
                }
            }
        }
    }
}


