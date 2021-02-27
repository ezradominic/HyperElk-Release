using System.Linq;
using System.Diagnostics;

namespace HyperElk.Core
{
    public class ArcaneMage : CombatRoutine
    {
        //Spell Strings
        private string RoP = "Rune of Power";
        private string AI = "Arcane Intellect";
        private string Counterspell = "Counterspell";
        private string IB = "Ice Block";
        private string MI = "Mirror Image";
        private string PB = "Prismatic Barrier";
        private string ShiftingPower = "Shifting Power";
        private string RadiantSpark = "Radiant Spark";
        private string Deathborne = "Deathborne";
        private string MirrorsofTorment = "Mirrors of Torment";
        private string Fleshcraft = "Fleshcraft";
        private string Trinket1 = "Trinket1";
        private string Trinket2 = "Trinket2";
        private string ManaGem = "Mana Gem";
        private string TimeWarp = "Time Warp";
        private string Temp = "Temporal Displacement";
        private string Exhaustion = "Exhaustion";
        private string Fatigued = "Fatigued";
        private string BL = "Bloodlust";
        private string AH = "Ancient Hysteria";
        private string TW = "Temporal Warp";
        private string AHL = "Arcane Harmony";
        private string Sated = "Sated";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string Spellsteal = "Spellsteal";
        private string RemoveCurse = "Remove Curse";
        private string SoulIgnite = "Soul Ignition";
        private string Quake = "Quake";

        //Talents
        bool RuleofThrees => API.PlayerIsTalentSelected(1, 2);
        bool ArcaneFamiliar => API.PlayerIsTalentSelected(1, 3);
        bool Slipstream => API.PlayerIsTalentSelected(2, 3);
        bool RuneofPower => API.PlayerIsTalentSelected(3, 3);
        bool ArcaneEcho => API.PlayerIsTalentSelected(4, 2);
        bool NetherTempest => API.PlayerIsTalentSelected(4, 3);
        bool ArcaneOrb => API.PlayerIsTalentSelected(6, 2);
        bool SuperNova => API.PlayerIsTalentSelected(6, 3);
        bool Resonace => API.PlayerIsTalentSelected(4, 1);
        //Spell Steal & Curse Removal
        string[] SpellSpealBuffList = { "Bless Weapon", "Death's Embrace", "Turn to Stone", "Wonder Grow", "Stoneskin" };
        string[] CurseList = { "Sintouched Anima", "Curse of Stone" };
        //CBProperties
        private int PBPercentProc => numbList[CombatRoutine.GetPropertyInt(PB)];
        private int IBPercentProc => numbList[CombatRoutine.GetPropertyInt(IB)];
        private int MIPercentProc => numbList[CombatRoutine.GetPropertyInt(MI)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        // public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };

        // public new string[] CDUsageWithAOE = new string[] { "Not Used", "with cooldowns", "on AOE", "always" };

        // public string[] CDUsage = new string[] { "Not Used", "With Cooldowns", "On Cooldown" };
        // public string[] CDUsageWithAOE = new string[] { "Not Used", "With Cooldowns", "on AOE", "On Cooldown" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        public string[] LegendaryList = new string[] { "None", "Temporal Warp", "Arcane Harmony", "Arcane Bombardment" };
        private string UseAP => CDUsage[CombatRoutine.GetPropertyInt("Arcane Power")];
        private string UseROP => CDUsage[CombatRoutine.GetPropertyInt("Rune of Power")];
        private string UseLeg => LegendaryList[CombatRoutine.GetPropertyInt("Legendary")];

        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        private bool QuakingHelper => CombatRoutine.GetPropertyBool("QuakingHelper");
        //General
        private int Level => API.PlayerLevel;
       // private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsTimeWarp => API.ToggleIsEnabled("TimeWarp");
        private bool InRange => API.TargetRange <= 40;
        private bool Burn => API.PlayerMana >= 10;
        private bool Conserve => API.PlayerMana <= 59 && API.SpellISOnCooldown("Evocation");
        private int Mana => API.PlayerMana;
       // bool !ChannelingShift => API.PlayerLastSpell == ShiftingPower;
        bool CastArcanePower => API.PlayerLastSpell == "Arcane Power";
        bool CastRoP => API.PlayerLastSpell == "Rune of Power";
        bool ChannelingShift => API.CurrentCastSpellID("player") == 314791;
        bool ChannelingEvo => API.CurrentCastSpellID("player") == 12051;
        bool ChannelingMissile => API.CurrentCastSpellID("player") == 5143;
        bool IsTrinkets1 => (UseTrinket1 == "With Cooldowns" && IsCooldowns & API.PlayerHasBuff("Arcane Power") && (!API.PlayerHasBuff(SoulIgnite) || API.PlayerHasBuff(SoulIgnite) && API.PlayerBuffTimeRemaining(SoulIgnite) <= 800) || UseTrinket1 == "On Cooldown" && (!API.PlayerHasBuff(SoulIgnite) || API.PlayerHasBuff(SoulIgnite) && API.PlayerBuffTimeRemaining(SoulIgnite) <= 800) || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= 2 && IsAOE);
        bool IsTrinkets2 => (UseTrinket2 == "With Cooldowns" && IsCooldowns && API.PlayerHasBuff("Arcane Power") && (!API.PlayerHasBuff(SoulIgnite) || API.PlayerHasBuff(SoulIgnite) && API.PlayerBuffTimeRemaining(SoulIgnite) <= 800) || UseTrinket2 == "On Cooldown" && (!API.PlayerHasBuff(SoulIgnite) || API.PlayerHasBuff(SoulIgnite) && API.PlayerBuffTimeRemaining(SoulIgnite) <= 800) || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= 2 && IsAOE);
        private bool BLDebuffs => !API.PlayerHasDebuff(Temp) || !API.PlayerHasDebuff(Exhaustion) || !API.PlayerHasDebuff(Fatigued);
        private bool BLBuFfs => !API.PlayerHasBuff(BL) || !API.PlayerHasBuff(AH) || !API.PlayerHasBuff(TimeWarp) || !API.PlayerHasBuff(TW);
        private bool Quaking => (API.PlayerCurrentCastTimeRemaining >= 200 || API.PlayerIsChanneling) && API.PlayerDebuffRemainingTime(Quake) < 200 && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool SaveQuake => (PlayerHasDebuff(Quake) && API.PlayerDebuffRemainingTime(Quake) > 200 && QuakingHelper || !PlayerHasDebuff(Quake) || !QuakingHelper);
        private bool QuakingAB => API.PlayerDebuffRemainingTime(Quake) > ABCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool QuakingAM => API.PlayerDebuffRemainingTime(Quake) > AMCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool QuakingToTM => API.PlayerDebuffRemainingTime(Quake) > ToTMCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool QuakingEvo => API.PlayerDebuffRemainingTime(Quake) > EvoCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool QuakingRune => API.PlayerDebuffRemainingTime(Quake) > RuneCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool QuakingShifting => API.PlayerDebuffRemainingTime(Quake) > ShiftingPowerCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool QuakingMirrors => API.PlayerDebuffRemainingTime(Quake) > MirrorsofTormentCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool QuakingRadiant => API.PlayerDebuffRemainingTime(Quake) > RadiantSparkCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        private bool QuakingDeathborne => API.PlayerDebuffRemainingTime(Quake) > DeathborneCastTime && (PlayerHasDebuff(Quake) || API.PlayerHasDebuff(Quake));
        float ShiftingPowerCastTime => 400f / (1f + API.PlayerGetHaste);
        float RadiantSparkCastTime => 150f / (1f + API.PlayerGetHaste);
        float MirrorsofTormentCastTime => 150f / (1f + API.PlayerGetHaste);
        float DeathborneCastTime => 150f / (1f + API.PlayerGetHaste);
        float RuneCastTime => 150f / (1f + API.PlayerGetHaste);
        float ABCastTime => 225f / (1f + API.PlayerGetHaste);
        float AMCastTime => 250f / (1f + API.PlayerGetHaste);
        float ToTMCastTime => 150f / (1f + API.PlayerGetHaste);
        float EvoCastTime => 600f / (1f + API.PlayerGetHaste);
        private static bool PlayerHasDebuff(string buff)
        {
            return API.PlayerHasDebuff(buff, false, false);
        }
        public override void Initialize()
        {
            CombatRoutine.Name = "Arcane Mage by Ryu";
            API.WriteLog("Welcome to Arcane Mage v1.4 by Ryu989");
            API.WriteLog("Presence of Mind(PoM) will by default cast when Arcane Power as less than 3 seconds left, otherwise, you can check box in settings to cast on CD");
            API.WriteLog("For Mana gem to work, please ensure you always have a fresh conjured one before as the rotation will wig out if it runs outta charges mid fight. Or you may place Mana Gem on ignore and use it yourself via a break macro");
            API.WriteLog("For the Quaking helper you just need to create an ingame macro with /stopcasting and bind it under the Macros Tab in Elk :-)");
            API.WriteLog("All Talents expect Ice Ward, Ring of Frost, Supernova and Mirror Image are supported");
            API.WriteLog("Legendary Support for Temporal Warp, Arcane Harmory and Arcane Bombardment added. If you have it please select it in the settings.");
            API.WriteLog("Rotation supports Auto Spellsteal for certain buffs and auto Remove Curse for certian curses along with Mouseover Support for them, please create the correct Mouseover Marco if you wish to use it. If you DONT want it do that, please check Ignore in the keybinds for SpellSteal/Remove Curse");
            // API.WriteLog("Supported Essences are Memory of Lucids, Concerated Flame and Worldvein");
            //Buff

            CombatRoutine.AddBuff("Arcane Power", 12042);
            CombatRoutine.AddBuff("Clearcasting", 263725);
            CombatRoutine.AddBuff("Rule of Threes", 2647741);
            CombatRoutine.AddBuff("Presence of Mind", 205025);
            CombatRoutine.AddBuff("Rune of Power", 116014);
            CombatRoutine.AddBuff("Prismatic Barrier", 235450);
            CombatRoutine.AddBuff("Arcane Intellect", 1459);
            CombatRoutine.AddBuff("Arcane Familiar", 210126);
            CombatRoutine.AddBuff("Evocation", 12051);
            CombatRoutine.AddBuff(TimeWarp, 80353);
            CombatRoutine.AddBuff(BL, 2825);
            CombatRoutine.AddBuff(AH, 90355);
            CombatRoutine.AddBuff(TW, 327351);
            CombatRoutine.AddBuff(AHL, 332769);
            CombatRoutine.AddBuff("Bless Weapon", 328288);
            CombatRoutine.AddBuff("Death's Embrace", 333875);
            CombatRoutine.AddBuff("Turn to Stone", 326607);
            CombatRoutine.AddBuff("Wonder Grow", 328016);
            CombatRoutine.AddBuff("Stoneskin", 322433);
            CombatRoutine.AddBuff(SoulIgnite, 345211);
            CombatRoutine.AddBuff(Quake, 240447);

            //Debuff
            CombatRoutine.AddDebuff("Nether Tempest", 114923);
            CombatRoutine.AddDebuff("Touch of the Magi", 210824);
            CombatRoutine.AddDebuff(Temp, 80354);
            CombatRoutine.AddDebuff(Fatigued, 264689);
            CombatRoutine.AddDebuff(Exhaustion, 57723);
            CombatRoutine.AddDebuff(Sated, 57724);
            CombatRoutine.AddDebuff("Sintouched Anima", 328494);
            CombatRoutine.AddDebuff("Curse of Stone", 319603);
            CombatRoutine.AddDebuff(Quake, 240447);

            //Spell
            CombatRoutine.AddSpell("Rune of Power", 116011, "None");
            CombatRoutine.AddSpell("Ice Block", 45438);
            CombatRoutine.AddSpell("Arcane Power", 12042);
            CombatRoutine.AddSpell("Arcane Orb", 153626);
            CombatRoutine.AddSpell("Nether Tempest", 114923);
            CombatRoutine.AddSpell("Arcane Barrage", 44425);
            CombatRoutine.AddSpell("Arcane Explosion", 1449);
            CombatRoutine.AddSpell("Arcane Missiles", 5143);
            CombatRoutine.AddSpell("Arcane Blast", 30451, "C");
            CombatRoutine.AddSpell("Evocation", 12051, "C");
            CombatRoutine.AddSpell("Prismatic Barrier", 235450, "C");
            CombatRoutine.AddSpell("Mirror Image", 55342, "C");
            CombatRoutine.AddSpell("Presence of Mind", "None");
            CombatRoutine.AddSpell("Counterspell", 2139, "None");
            CombatRoutine.AddSpell("Arcane Familiar", 205022, "None");
            CombatRoutine.AddSpell("Touch of the Magi", 321507, "None");
            CombatRoutine.AddSpell("Arcane Intellect", 1459, "None");
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
            CombatRoutine.AddItem(ManaGem, 36799);

            //Macro
            CombatRoutine.AddMacro(Trinket1);
            CombatRoutine.AddMacro(Trinket2);
            CombatRoutine.AddMacro(RemoveCurse + "MO");
            CombatRoutine.AddMacro(Spellsteal + "MO");
            CombatRoutine.AddMacro("Stopcast", "F10");
            CombatRoutine.AddMacro(Counterspell + "Focus");

            //Toggle
            CombatRoutine.AddToggle("TimeWarp");
            CombatRoutine.AddToggle("Mouseover");

            //Prop
            CombatRoutine.AddProp(PB, PB, numbList, "Life percent at which " + PB + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(IB, IB, numbList, "Life percent at which " + IB + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(MI, MI, numbList, "Life percent at which " + MI + " is used, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 1);
            CombatRoutine.AddProp("Arcane Power", "Use " + "Arcane Power", CDUsage, "Use " + "Arcane Power" + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("Rune of Power", "Use " + "Rune of Power", CDUsage, "Use " + "Rune of Power" + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("Legendary", "Select your Legendary", LegendaryList, "Select Your Legendary", "Legendary");
            CombatRoutine.AddProp("QuakingHelper", "Quaking Helper", false, "Will cancel casts on Quaking", "Generic");
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket1", CDUsageWithAOE, "Use " + "Trinket 1" + " On Cooldown, With Cooldown, On AOEs or Not Used", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket2", CDUsageWithAOE, "Use " + "Trinket 2" + "On Cooldown, With Cooldown, On AOEs or Not Used", "Trinkets", 0);



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
                API.WriteLog("Debuff Time Remaining for Quake : " + API.PlayerDebuffRemainingTime(Quake));
                return;
            }
            if (isInterrupt && API.CanCast("Counterspell") && Level >= 7  && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && API.PlayerIsCasting(false))
            {
                API.CastSpell("Counterspell");
                return;
            }
            if (API.CanCast(Counterspell) && CombatRoutine.GetPropertyBool("KICK") && API.FocusCanInterrupted && API.FocusIsCasting() && (API.FocusIsChanneling ? API.FocusElapsedCastTimePercent >= interruptDelay : API.FocusCurrentCastTimeRemaining <= interruptDelay))
            {
                API.CastSpell(Counterspell + "Focus");
                return;
            }
            if (API.CanCast(Spellsteal) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
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
            if (API.CanCast(Spellsteal) && IsMouseover && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
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
            if (API.CanCast(RemoveCurse) && !API.PlayerCanAttackTarget && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
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
            if (API.CanCast(RemoveCurse) && !API.PlayerCanAttackMouseover && IsMouseover && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
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
            if (API.CanCast("Mirror Image") && API.PlayerHealthPercent <= MIPercentProc && API.PlayerHealthPercent != 0 && Level >= 44 && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell("Mirror Image");
                return;
            }
            if (API.CanCast("Ice Block") && API.PlayerHealthPercent <= IBPercentProc && API.PlayerHealthPercent != 0 && Level >= 22 && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell("Ice Block");
                return;
            }
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.CanCast("Prismatic Barrier") && Level >= 21 && !API.PlayerHasBuff("Prismatic Barrier") && API.PlayerHealthPercent <= PBPercentProc && API.PlayerHealthPercent != 0 && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell("Prismatic Barrier");
                return;
            }
            if (IsTimeWarp && API.CanCast(TimeWarp) && ((BLDebuffs) || UseLeg == "Temporal Warp") && (BLBuFfs))
            {
                API.CastSpell(TimeWarp);
                return;
            }
            if (API.PlayerItemCanUse(ManaGem) && !API.MacroIsIgnored(ManaGem) && API.PlayerItemRemainingCD(ManaGem) <= 40 && API.PlayerMana < 90 && !API.PlayerIsCasting(true) && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile)
            {
                API.CastSpell(ManaGem);
                return;
            }
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1 && !ChannelingShift && !ChannelingMissile && !ChannelingEvo)
            {
                API.CastSpell(Trinket1);
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2 && !ChannelingShift && !ChannelingMissile && !ChannelingEvo)
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
            if (API.CanCast(RadiantSpark) && InRange && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && SaveQuake)
            {
                API.CastSpell(RadiantSpark);
                return;
            }
            if (API.CanCast(MirrorsofTorment) && InRange && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE"  && IsAOE) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && SaveQuake)
            {
                API.CastSpell(MirrorsofTorment);
                return;
            }
            if (API.CanCast(Deathborne) && InRange && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE"  && IsAOE) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && SaveQuake)
            {
                API.CastSpell(Deathborne);
                return;
            }
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && isRacial && IsCooldowns && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Worgen" && isRacial && IsCooldowns && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && isRacial && IsCooldowns && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && isRacial && IsCooldowns && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Blood Elf" && isRacial && IsCooldowns && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast("Arcane Power") && Level >= 29 && !API.PlayerIsMoving && API.TargetRange <= 40 && (Burn || Conserve && API.PlayerCurrentArcaneCharges == 4) && (IsCooldowns && UseAP == "With Cooldowns" || UseAP == "On Cooldown") && !API.PlayerHasBuff(RoP) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && !API.PlayerIsCasting(true))
            {
                API.CastSpell("Arcane Power");
                return;
            }
            if (API.CanCast("Arcane Missiles") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 13 && InRange && (API.PlayerHasBuff("Clearcasting") && Mana < 95 || API.TargetHasDebuff("Touch of the Magi") && ArcaneEcho) && (Burn || Conserve) && (API.PlayerIsMoving && Slipstream || !API.PlayerIsMoving) && (!QuakingAM || QuakingAM && QuakingHelper))
            {
                API.CastSpell("Arcane Missiles");
                return;
            }
            if (RuneofPower && API.CanCast("Rune of Power") && Mana > 15 && API.SpellCDDuration("Arcane Power") > 1200 && !CastArcanePower && Burn && API.TargetRange <= 40 && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && (IsCooldowns && UseROP == "With Cooldowns" || UseROP == "On Cooldown") && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && (!QuakingRune || QuakingRune && QuakingHelper))
            {
                API.CastSpell("Rune of Power");
                return;
            }
            if (API.CanCast(ShiftingPower) && !API.PlayerHasBuff("Clearcasting") && !CastRoP && InRange && PlayerCovenantSettings == "Night Fae" && API.SpellISOnCooldown("Arcane Power") && API.SpellISOnCooldown("Touch of the Magi") && NotChanneling && !ChannelingEvo && !ChannelingMissile && !API.PlayerHasBuff("Evocation") && (RuneofPower && API.SpellISOnCooldown("Rune of Power") && !API.PlayerHasBuff(RoP) || !RuneofPower) && !API.PlayerHasBuff("Arcane Power") && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= 3 && IsAOE) && (!QuakingShifting || QuakingShifting && QuakingHelper))
            {
                API.CastSpell(ShiftingPower);
                return;
            }
            if (API.CanCast("Touch of the Magi") && Level >= 33 && (Burn || Conserve) && (API.PlayerCurrentArcaneCharges <= 0 || !API.TargetHasDebuff("Touch of the Magi") && ArcaneEcho && API.PlayerCurrentArcaneCharges <= 0) && InRange && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && SaveQuake)
            {
                API.CastSpell("Touch of the Magi");
                return;
            }
            if (API.CanCast("Arcane Orb") && InRange && ArcaneOrb && (Burn || Conserve) && API.PlayerCurrentArcaneCharges <= 3 && (API.PlayerIsMoving || !API.PlayerIsMoving) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell("Arcane Orb");
                return;
            }
            if (API.CanCast("Nether Tempest") && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && InRange && NetherTempest && (Burn || Conserve) && API.PlayerCurrentArcaneCharges == 4 && (!API.TargetHasDebuff("Nether Tempest") || API.TargetDebuffRemainingTime("Nether Tempest") <= 300) && !API.PlayerHasBuff("Arcane Power") && !API.PlayerHasBuff("Rune of Power") && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Nether Tempest");
                return;
            }
            if (API.CanCast("Presence of Mind") && Level >= 42 && API.PlayerHasBuff("Arcane Power") && API.PlayerBuffTimeRemaining("Arcane Power") <= 400 && !API.PlayerHasBuff("Presence of Mind") && Burn && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell("Presence of Mind");
                return;
            }
            if (API.CanCast("Arcane Barrage") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 10 && InRange && (IsAOE && API.TargetUnitInRangeCount >= 3 || IsAOE && API.TargetUnitInRangeCount >= 2 && Resonace) && InRange && API.PlayerCurrentArcaneCharges == 4 && (Burn || Conserve) && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Explosion") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 6 && InRange && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 8 && (Burn || Conserve) && (API.PlayerHasBuff("Clearcasting") || !API.PlayerHasBuff("Clearcasting")) && (API.PlayerIsMoving || !API.PlayerIsMoving) && IsAOE)
            {
                API.CastSpell("Arcane Explosion");
                return;
            }
            if (API.CanCast("Arcane Barrage") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 10 && InRange && (API.SpellISOnCooldown("Evocation") && API.PlayerCurrentArcaneCharges <= 4 && Mana <= 75 || !API.SpellISOnCooldown("Touch of the Magi") && API.PlayerCurrentArcaneCharges == 4 || API.PlayerBuffStacks(AHL) == 15 && API.PlayerCurrentArcaneCharges >= 4 && UseLeg == "Arcane Harmony"  || API.TargetHealthPercent <= 35 && API.TargetHealthPercent > 0 && UseLeg == "Arcane Bombardment" && API.PlayerCurrentArcaneCharges == 4)  && (!API.PlayerHasBuff("Rune of Power") || !API.PlayerHasBuff("Arcane Power")) && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Barrage") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 10 && InRange && API.TargetUnitInRangeCount >= 3 && API.PlayerCurrentArcaneCharges == 4 && Conserve && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Blast") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 10 && InRange && (Burn || Conserve) && (RuleofThrees && (API.PlayerHasBuff("Rule of Threes") || !API.PlayerHasBuff("Rule of Threes")) || !RuleofThrees) && !API.PlayerIsMoving && (!QuakingAB || QuakingAB && QuakingHelper))
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Evocation") && NotChanneling && !ChannelingShift && !ChannelingMissile && Level >= 27 && Mana <= 10 && (API.PlayerIsMoving && Slipstream|| !API.PlayerIsMoving) && (!QuakingEvo || QuakingEvo && QuakingHelper))
            {
                API.CastSpell("Evocation");
                return;
            }
            if (API.CanCast("Arcane Familiar") && NotChanneling && ArcaneFamiliar && !API.PlayerHasBuff("Arcane Familiar") && (Conserve || Burn))
            {
                API.CastSpell("Arcane Familiar");
                return;
            }

        }

    }
}



