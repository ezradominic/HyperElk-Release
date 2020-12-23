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
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
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
        private int Trinket1Usage => CombatRoutine.GetPropertyInt("Trinket1");
        private int Trinket2Usage => CombatRoutine.GetPropertyInt("Trinket2");
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
        private bool BLDebuffs => !API.PlayerHasDebuff(Temp) || !API.PlayerHasDebuff(Exhaustion) || !API.PlayerHasDebuff(Fatigued);
        private bool BLBuFfs => !API.PlayerHasBuff(BL) || !API.PlayerHasBuff(AH) || !API.PlayerHasBuff(TimeWarp) || !API.PlayerHasBuff(TW);
        public override void Initialize()
        {
            CombatRoutine.Name = "Arcane Mage by Ryu";
            API.WriteLog("Welcome to Arcane Mage v1.4 by Ryu989");
            API.WriteLog("Presence of Mind(PoM) will by default cast when Arcane Power as less than 3 seconds left, otherwise, you can check box in settings to cast on CD");
            API.WriteLog("All Talents expect Ice Ward, Ring of Frost, Supernova and Mirror Image are supported");
            API.WriteLog("Legendary Support for Temporal Warp, Arcane Harmory and Arcane Bombardment added. If you have it please select it in the settings.");

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
          
            //Debuff
            CombatRoutine.AddDebuff("Nether Tempest", 114923);
            CombatRoutine.AddDebuff("Touch of the Magi", 210824);
            CombatRoutine.AddDebuff(Temp, 80354);
            CombatRoutine.AddDebuff(Fatigued, 264689);
            CombatRoutine.AddDebuff(Exhaustion, 57723);
            CombatRoutine.AddDebuff(Sated, 57724);

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

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);
            CombatRoutine.AddItem(ManaGem, 36799);

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);

            //Toggle
            CombatRoutine.AddToggle("TimeWarp");

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
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", CDUsage, "When should trinket1 be used", "Trinket", 0);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", CDUsage, "When should trinket1 be used", "Trinket", 0);



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
            if (isInterrupt && API.CanCast("Counterspell") && Level >= 7  && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling && API.PlayerIsCasting(false))
            {
                API.CastSpell("Counterspell");
                return;
            }
            if (API.PlayerItemCanUse("Healthstone") && API.PlayerItemRemainingCD("Healthstone") == 0 && API.PlayerHealthPercent <= HealthStonePercent)
            {
                API.CastSpell("Healthstone");
                return;
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
            if (API.PlayerItemCanUse(ManaGem) && API.PlayerItemRemainingCD(ManaGem) == 0 && API.PlayerMana < 90 && API.PlayerIsCasting(false) && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile)
            {
                API.CastSpell(ManaGem);
                return;
            }
            if (Trinket1Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !ChannelingShift && !ChannelingEvo && !ChannelingMissile)
                API.CastSpell(trinket1);
            if (Trinket1Usage == 2 && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !ChannelingShift && !ChannelingEvo && !ChannelingMissile)
                API.CastSpell(trinket1);
            if (Trinket2Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !ChannelingShift && !ChannelingEvo && !ChannelingMissile)
                API.CastSpell(trinket2);
            if (Trinket2Usage == 2 && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !ChannelingShift && !ChannelingEvo && !ChannelingMissile)
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
            if (API.CanCast(RadiantSpark) && InRange && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell(RadiantSpark);
                return;
            }
            if (API.CanCast(MirrorsofTorment) && InRange && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE"  && IsAOE) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell(MirrorsofTorment);
                return;
            }
            if (API.CanCast(Deathborne) && InRange && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE"  && IsAOE) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
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
            if (API.CanCast("Arcane Missiles") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 13 && InRange && (API.PlayerHasBuff("Clearcasting") && Mana < 95 || API.TargetHasDebuff("Touch of the Magi") && ArcaneEcho) && (Burn || Conserve) && (API.PlayerIsMoving && Slipstream || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Missiles");
                return;
            }
            if (API.CanCast("Arcane Power") && Level >= 29 && !API.PlayerIsMoving && API.TargetRange <= 40 && (Burn || Conserve && API.PlayerCurrentArcaneCharges == 4) && (IsCooldowns && UseAP == "With Cooldowns" || UseAP == "On Cooldown") && !API.PlayerHasBuff(RoP) && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell("Arcane Power");
                return;
            }
            if (RuneofPower && API.CanCast("Rune of Power") && Mana > 15 && API.SpellCDDuration("Arcane Power") > 1200 && !CastArcanePower && Burn && API.TargetRange <= 40 && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && (IsCooldowns && UseROP == "With Cooldowns" || UseROP == "On Cooldown") && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
            {
                API.CastSpell("Rune of Power");
                return;
            }
            if (API.CanCast(ShiftingPower) && !API.PlayerHasBuff("Clearcasting") && !CastRoP && InRange && PlayerCovenantSettings == "Night Fae" && API.SpellISOnCooldown("Arcane Power") && API.SpellISOnCooldown("Touch of the Magi") && NotChanneling && !ChannelingEvo && !ChannelingMissile && !API.PlayerHasBuff("Evocation") && (RuneofPower && API.SpellISOnCooldown("Rune of Power") && !API.PlayerHasBuff(RoP) || !RuneofPower) && !API.PlayerHasBuff("Arcane Power") && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= 3 && IsAOE))
            {
                API.CastSpell(ShiftingPower);
                return;
            }
            if (API.CanCast("Touch of the Magi") && Level >= 33 && (Burn || Conserve) && (API.PlayerCurrentArcaneCharges <= 0 || !API.TargetHasDebuff("Touch of the Magi") && ArcaneEcho && API.PlayerCurrentArcaneCharges <= 0) && InRange && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && NotChanneling)
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
            if (API.CanCast("Arcane Barrage") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 10 && InRange && (API.SpellISOnCooldown("Evocation") && API.PlayerCurrentArcaneCharges <= 4 && Mana <= 60 || !API.SpellISOnCooldown("Touch of the Magi") && API.PlayerCurrentArcaneCharges == 4 || API.PlayerBuffStacks(AHL) == 15 && API.PlayerCurrentArcaneCharges >= 4 && UseLeg == "Arcane Harmony"  || API.TargetHealthPercent <= 35 && UseLeg == "Arcane Bombardment" && API.PlayerCurrentArcaneCharges == 4)  && (!API.PlayerHasBuff("Rune of Power") || !API.PlayerHasBuff("Arcane Power")) && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Barrage") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 10 && InRange && API.TargetUnitInRangeCount >= 3 && API.PlayerCurrentArcaneCharges == 4 && Conserve && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Blast") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 10 && InRange && (Burn || Conserve) && !API.PlayerIsMoving)
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Arcane Blast") && NotChanneling && !ChannelingShift && !ChannelingEvo && !ChannelingMissile && Level >= 10 && InRange && RuleofThrees && API.PlayerHasBuff("Rule of Threes") && (Burn || Conserve) && !API.PlayerIsMoving)
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Evocation") && NotChanneling && !ChannelingShift && !ChannelingMissile && Level >= 27 && Mana <= 10 && (API.PlayerIsMoving && Slipstream|| !API.PlayerIsMoving))
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



