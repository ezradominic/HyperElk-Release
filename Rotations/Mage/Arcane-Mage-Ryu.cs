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
        private int PBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(PB)];
        private int IBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(IB)];
        private int MIPercentProc => percentListProp[CombatRoutine.GetPropertyInt(MI)];
        private int FleshcraftPercentProc => percentListProp[CombatRoutine.GetPropertyInt(Fleshcraft)];
        // public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };

        // public new string[] CDUsageWithAOE = new string[] { "Not Used", "with cooldowns", "on AOE", "always" };

        // public string[] CDUsage = new string[] { "Not Used", "With Cooldowns", "On Cooldown" };
        // public string[] CDUsageWithAOE = new string[] { "Not Used", "With Cooldowns", "on AOE", "On Cooldown" };
        private string UseAP => CDUsage[CombatRoutine.GetPropertyInt("Arcane Power")];
        private string UseROP => CDUsage[CombatRoutine.GetPropertyInt("Rune of Power")];

        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private int Trinket1Usage => CombatRoutine.GetPropertyInt("Trinket1");
        private int Trinket2Usage => CombatRoutine.GetPropertyInt("Trinket2");
        //General
        private int Level => API.PlayerLevel;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool InRange => API.TargetRange <= 40;
        private bool Burn => API.PlayerMana >= 10;
        private bool Conserve => API.PlayerMana <= 59 && API.SpellISOnCooldown("Evocation");
        private int Mana => API.PlayerMana;
        bool CastShifting => API.PlayerLastSpell == ShiftingPower;
        bool CastArcanePower => API.PlayerLastSpell == "Arcane Power";
        bool CastRoP => API.PlayerLastSpell == "Rune of Power";
        public override void Initialize()
        {
            CombatRoutine.Name = "Arcane Mage by Ryu";
            API.WriteLog("Welcome to Arcane Mage v1.2 by Ryu989");
            API.WriteLog("Presence of Mind(PoM) will by default cast when Arcane Power as less than 3 seconds left, otherwise, you can check box in settings to cast on CD");
            API.WriteLog("All Talents expect Ice Ward, Ring of Frost, Supernova and Mirror Image are supported");
            API.WriteLog("Current Mana :" + API.PlayerMana);

            // API.WriteLog("Supported Essences are Memory of Lucids, Concerated Flame and Worldvein");
            //Buff

            CombatRoutine.AddBuff("Arcane Power");
            CombatRoutine.AddBuff("Clearcasting");
            CombatRoutine.AddBuff("Rule of Threes");
            CombatRoutine.AddBuff("Presence of Mind");
            CombatRoutine.AddBuff("Rune of Power");
            CombatRoutine.AddBuff("Prismatic Barrier");
            CombatRoutine.AddBuff("Arcane Intellect");
            CombatRoutine.AddBuff("Arcane Familiar");
            CombatRoutine.AddBuff("Evocation");

            //Debuff
            CombatRoutine.AddDebuff("Nether Tempest");
            CombatRoutine.AddDebuff("Touch of the Magi");

            //Spell
            CombatRoutine.AddSpell("Rune of Power", "None");
            CombatRoutine.AddSpell("Ice Block");
            CombatRoutine.AddSpell("Arcane Power");
            CombatRoutine.AddSpell("Arcane Orb");
            CombatRoutine.AddSpell("Nether Tempest");
            CombatRoutine.AddSpell("Arcane Barrage");
            CombatRoutine.AddSpell("Arcane Explosion");
            CombatRoutine.AddSpell("Arcane Missiles");
            CombatRoutine.AddSpell("Arcane Blast", "C");
            CombatRoutine.AddSpell("Evocation", "C");
            CombatRoutine.AddSpell("Prismatic Barrier", "C");
            CombatRoutine.AddSpell("Mirror Image", "C");
            CombatRoutine.AddSpell("Presence of Mind", "None");
            CombatRoutine.AddSpell("Counterspell", "None");
            CombatRoutine.AddSpell("Arcane Familiar", "None");
            CombatRoutine.AddSpell("Touch of the Magi", "None");
            CombatRoutine.AddSpell("Arcane Intellect", "None");
            CombatRoutine.AddSpell(ShiftingPower);
            CombatRoutine.AddSpell(RadiantSpark);
            CombatRoutine.AddSpell(Deathborne);
            CombatRoutine.AddSpell(MirrorsofTorment);
            CombatRoutine.AddSpell(Fleshcraft);
            CombatRoutine.AddSpell(ManaGem);

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);


            //Prop
            CombatRoutine.AddProp(PB, PB, percentListProp, "Life percent at which " + PB + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(IB, IB, percentListProp, "Life percent at which " + IB + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(MI, MI, percentListProp, "Life percent at which " + MI + " is used, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", percentListProp, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 1);
            CombatRoutine.AddProp("Arcane Power", "Use " + "Arcane Power", CDUsage, "Use " + "Arcane Power" + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("Rune of Power", "Use " + "Rune of Power", CDUsage, "Use " + "Rune of Power" + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
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
            if (isInterrupt && API.CanCast("Counterspell") && Level >= 7 && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell("Counterspell");
                return;
            }
            if (API.CanCast("Ice Block") && API.PlayerHealthPercent <= IBPercentProc && API.PlayerHealthPercent != 0 && Level >= 22 && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell("Ice Block");
                return;
            }
            if (API.CanCast("Mirror Image") && API.PlayerHealthPercent <= MIPercentProc && API.PlayerHealthPercent != 0 && Level >= 44 && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell("Mirror Image");
                return;
            }
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.CanCast("Prismatic Barrier") && Level >= 21 && !API.PlayerHasBuff("Prismatic Barrier") && API.PlayerHealthPercent <= PBPercentProc && API.PlayerHealthPercent != 0 && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell("Prismatic Barrier");
                return;
            }
            if (Trinket1Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !CastShifting && NotCasting && NotChanneling)
                API.CastSpell(trinket1);
            if (Trinket1Usage == 2 && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !CastShifting && NotCasting && NotChanneling)
                API.CastSpell(trinket1);
            if (Trinket2Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !CastShifting && NotCasting && NotChanneling)
                API.CastSpell(trinket2);
            if (Trinket2Usage == 2 && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !CastShifting && NotCasting && NotChanneling)
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
            if (API.CanCast(RadiantSpark) && InRange && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && NotCasting)
            {
                API.CastSpell(RadiantSpark);
                return;
            }
            if (API.CanCast(MirrorsofTorment) && InRange && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE"  && IsAOE) && NotChanneling && NotCasting)
            {
                API.CastSpell(MirrorsofTorment);
                return;
            }
            if (API.CanCast(Deathborne) && InRange && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE"  && IsAOE) && NotChanneling && NotCasting)
            {
                API.CastSpell(Deathborne);
                return;
            }
            if (API.CanCast("Arcane Power") && Level >= 29 && !API.PlayerIsMoving && API.TargetRange <= 40 && NotCasting && NotChanneling && (IsCooldowns && UseAP == "With Cooldowns" || UseAP == "On Cooldown") && !CastShifting)
            {
                API.CastSpell("Arcane Power");
                return;
            }
            if (RuneofPower && API.CanCast("Rune of Power") && !CastArcanePower && Burn && API.TargetRange <= 40 && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && (IsCooldowns && UseROP == "With Cooldowns" || UseROP == "On Cooldown") && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell("Rune of Power");
                return;
            }
            if (API.CanCast(ShiftingPower) && !CastRoP && InRange && PlayerCovenantSettings == "Night Fae" && API.SpellISOnCooldown("Arcane Power") && API.SpellISOnCooldown("Touch of the Magi") && NotCasting && NotChanneling && !API.PlayerHasBuff("Evocation") && (RuneofPower && API.SpellISOnCooldown("Rune of Power") && !API.PlayerHasBuff(RoP) || !RuneofPower) && !API.PlayerHasBuff("Arcane Power") && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= 3 && IsAOE))
            {
                API.CastSpell(ShiftingPower);
                return;
            }
            if (API.CanCast("Touch of the Magi") && Level >= 33 && (Burn || Conserve) && (API.PlayerCurrentArcaneCharges <= 0 || !API.TargetHasDebuff("Touch of the Magi") && ArcaneEcho) && InRange && NotCasting && NotChanneling)
            {
                API.CastSpell("Touch of the Magi");
                return;
            }
            if (API.CanCast("Arcane Orb") && InRange && ArcaneOrb && (Burn || Conserve) && API.PlayerCurrentArcaneCharges <= 3 && (API.PlayerIsMoving || !API.PlayerIsMoving) && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell("Arcane Orb");
                return;
            }
            if (API.CanCast("Nether Tempest") && !CastShifting && NotCasting && NotChanneling && InRange && NetherTempest && (Burn || Conserve) && API.PlayerCurrentArcaneCharges == 4 && (!API.TargetHasDebuff("Nether Tempest") || API.TargetDebuffRemainingTime("Nether Tempest") <= 300) && !API.PlayerHasBuff("Arcane Power") && !API.PlayerHasBuff("Rune of Power") && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Nether Tempest");
                return;
            }
            if (API.CanCast("Presence of Mind") && Level >= 42 && API.PlayerHasBuff("Arcane Power") && API.PlayerBuffTimeRemaining("Arcane Power") <= 300 && !API.PlayerHasBuff("Presence of Mind") && Burn && !CastShifting)
            {
                API.CastSpell("Presence of Mind");
                return;
            }
            if (API.CanCast("Arcane Barrage") && !CastShifting && NotCasting && NotChanneling && Level >= 10 && InRange && (IsAOE && API.TargetUnitInRangeCount >= 3 || IsAOE && API.TargetUnitInRangeCount >= 2 && Resonace) && InRange && API.PlayerCurrentArcaneCharges == 4 && (Burn || Conserve) && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Explosion") && !CastShifting && NotCasting && NotChanneling && Level >= 6 && InRange && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 8 && (Burn || Conserve) && (API.PlayerHasBuff("Clearcasting") || !API.PlayerHasBuff("Clearcasting")) && (API.PlayerIsMoving || !API.PlayerIsMoving) && IsAOE)
            {
                API.CastSpell("Arcane Explosion");
                return;
            }
            if (API.CanCast("Arcane Missiles") && !CastShifting && NotCasting && NotChanneling && Level >= 13 && InRange && API.PlayerHasBuff("Clearcasting") && (Mana < 95 || API.TargetHasDebuff("Touch of the Magi") && ArcaneEcho && IsAOE) && (Burn || Conserve) && (API.PlayerIsMoving && Slipstream || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Missiles");
                return;
            }
            if (API.CanCast("Arcane Barrage") && !CastShifting && NotCasting && NotChanneling && Level >= 10 && InRange && API.SpellISOnCooldown("Evocation") && API.PlayerCurrentArcaneCharges <= 4 && Mana <= 60 && (!API.PlayerHasBuff("Rune of Power") || !API.PlayerHasBuff("Arcane Power")) && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Barrage") && !CastShifting && NotCasting && NotChanneling && Level >= 10 && InRange && API.TargetUnitInRangeCount >= 3 && API.PlayerCurrentArcaneCharges == 4 && Conserve && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Blast") && NotCasting && NotChanneling && Level >= 10 && InRange && (Burn || Conserve) && !API.PlayerIsMoving)
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Arcane Blast") && NotCasting && NotChanneling && Level >= 10 && InRange && RuleofThrees && API.PlayerHasBuff("Rule of Threes") && (Burn || Conserve) && !API.PlayerIsMoving)
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Evocation") && !CastShifting && NotCasting && NotChanneling && Level >= 27 && Mana <= 10 && (API.PlayerIsMoving && Slipstream|| !API.PlayerIsMoving))
            {
                API.CastSpell("Evocation");
                return;
            }
            if (API.CanCast("Arcane Familiar") && NotCasting && NotChanneling && ArcaneFamiliar && !API.PlayerHasBuff("Arcane Familiar") && (Conserve && API.SpellISOnCooldown("Evocation") || Burn))
            {
                API.CastSpell("Arcane Familiar");
                return;
            }

        }

    }
}



