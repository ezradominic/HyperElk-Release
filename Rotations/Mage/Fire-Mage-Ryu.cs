
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

        //Talents
        bool SearingTouch => API.PlayerIsTalentSelected(1, 3);
        bool BlastWave => API.PlayerIsTalentSelected(2, 3);
        bool RuneOfPower => API.PlayerIsTalentSelected(3, 3);
        bool AlexstraszaFury => API.PlayerIsTalentSelected(4, 2);
        bool LivingBomb => API.PlayerIsTalentSelected(6, 3);
        bool Pyroclasm => API.PlayerIsTalentSelected(7, 2);
        bool Meteor => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        private int BBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(BB)];
        private int IBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(IB)];
        private int MIPercentProc => percentListProp[CombatRoutine.GetPropertyInt(MI)];
        private int FleshcraftPercentProc => percentListProp[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private int Trinket1Usage => CombatRoutine.GetPropertyInt("Trinket1");
        private int Trinket2Usage => CombatRoutine.GetPropertyInt("Trinket2");
        //General
        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private string UseROP => CDUsage[CombatRoutine.GetPropertyInt(RoP)];
        private string UseCom => CDUsage[CombatRoutine.GetPropertyInt("Combustion")];

        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        bool ChannelingShift => API.CurrentCastSpellID("player") == 314791;
        bool CastCombustion => API.PlayerLastSpell == "Combustion";
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");


        public override void Initialize()
        {
            CombatRoutine.Name = "Fire Mage by Ryu";
            API.WriteLog("Welcome to Fire Mage by Ryu");
            API.WriteLog("Create the following cursor macro for Flamestrike and Meteor");
            API.WriteLog("Flamestrike -- /cast [@cursor] Flamestrike");
            API.WriteLog("Meteor -- /cast [@cursor] Meteor");
            API.WriteLog("Create Macro /cast [@player] Arcane Intellect to buff Arcane Intellect so you don't require a target");
            API.WriteLog("All Talents expect Ring of Frost and Alexstrasza's Fury supported. All Cooldowns are associated with Cooldown toggle.");
            API.WriteLog("Will add settings and customizing later");
            //Buff
            CombatRoutine.AddBuff("Heating Up");
            CombatRoutine.AddBuff("Pyroclasm");
            CombatRoutine.AddBuff("Combustion");
            CombatRoutine.AddBuff("Hot Streak!");
            CombatRoutine.AddBuff("Rune of Power");
            CombatRoutine.AddBuff("Blazing Barrier");
            CombatRoutine.AddBuff("Arcane Intellect");

            //Debuff
            CombatRoutine.AddDebuff("Ignite");

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

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);


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
            if (isInterrupt && API.CanCast("Counterspell") && Level >= 7 && NotCasting )
            {
                API.CastSpell("Counterspell");
                return;
            }
            if (API.CanCast("Ice Block") && API.PlayerHealthPercent <= IBPercentProc && API.PlayerHealthPercent != 0 && Level >= 22 && NotCasting  && !ChannelingShift)
            {
                API.CastSpell("Ice Block");
                return;
            }
            if (API.CanCast("Mirror Image") && API.PlayerHealthPercent <= MIPercentProc && API.PlayerHealthPercent != 0 && Level >= 44 && NotCasting  && !ChannelingShift)
            {
                API.CastSpell("Mirror Image");
                return;
            }
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotCasting  && !ChannelingShift)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.CanCast("Blazing Barrier") && Level >= 21 && !API.PlayerHasBuff("Blazing Barrier") && API.PlayerHealthPercent <= BBPercentProc && API.PlayerHealthPercent != 0 && NotCasting  && !ChannelingShift)
            {
                API.CastSpell("Blazing Barrier");
                return;
            }
            if (Trinket1Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !ChannelingShift)
                API.CastSpell(trinket1);
            if (Trinket1Usage == 2 && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !ChannelingShift)
                API.CastSpell(trinket1);
            if (Trinket2Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !ChannelingShift)
                API.CastSpell(trinket2);
            if (Trinket2Usage == 2 && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !ChannelingShift)
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
            if (API.CanCast(RadiantSpark) && InRange && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE)  && !ChannelingShift)
            {
                API.CastSpell(RadiantSpark);
                return;
            }
            if (API.CanCast(MirrorsofTorment) && InRange && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE)  && !ChannelingShift)
            {
                API.CastSpell(MirrorsofTorment);
                return;
            }
            if (API.CanCast(Deathborne) && InRange && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE)  && !ChannelingShift)
            {
                API.CastSpell(Deathborne);
                return;
            }
            if (API.CanCast("Combustion") && Level >= 29 && !API.PlayerIsMoving && API.TargetRange <= 40 && (IsCooldowns && UseCom == "With Cooldowns" || UseCom == "On Cooldown") && Level >= 29  && !ChannelingShift)
            {
                API.CastSpell("Combustion");
                return;
            }
            if (API.CanCast(ShiftingPower) && InRange && PlayerCovenantSettings == "Night Fae" && API.SpellISOnCooldown("Combustion") && !API.PlayerHasBuff("Combustion") && !API.PlayerHasBuff("Hot Streak!") && API.PlayerHasBuff("Heating Up") && API.SpellCharges("Fire Blast") > 0 && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && !CastCombustion)
            {
                API.CastSpell(ShiftingPower);
                return;
            }
            if (API.CanCast("Living Bomb") && LivingBomb && API.TargetUnitInRangeCount >= 2 && InRange  && !ChannelingShift)
            {
                API.CastSpell("Living Bomb");
                return;
            }
            if (API.CanCast("Flamestrike") && InRange && API.PlayerHasBuff("Hot Streak!") && API.TargetUnitInRangeCount >= 3 && Level >= 17  && IsAOE && !ChannelingShift)
            {
                API.CastSpell("Flamestrike");
                API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                return;
            }
            if (API.CanCast("Pyroblast") && API.PlayerHasBuff("Hot Streak!") && InRange && Level >= 12  && !ChannelingShift)
            {
                API.CastSpell("Pyroblast");
                return;
            }
            if (API.CanCast("Fire Blast") && API.SpellCharges("Fire Blast") > 0 && API.PlayerHasBuff("Combustion") && API.PlayerHasBuff("Heating Up") && InRange && Level >= 33  && !ChannelingShift)
            {
                API.CastSpell("Fire Blast");
                return;
            }
            if (RuneOfPower && API.CanCast("Rune of Power") && API.TargetRange <= 40 && !CastCombustion && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && (IsCooldowns && UseROP == "With Cooldowns" || UseROP == "On Cooldown") && !ChannelingShift)
            {
                API.CastSpell("Rune of Power");
                return;
            }
            if (Meteor && API.CanCast("Meteor") && InRange  && !ChannelingShift)
            {
                API.CastSpell("Meteor");
                return;
            }
            if (API.CanCast("Fire Blast") && API.SpellCharges("Fire Blast") > 0 && API.PlayerHasBuff("Heating Up") && InRange && Level >= 33  && !ChannelingShift)
            {
                API.CastSpell("Fire Blast");
                return;
            }
            if (BlastWave && API.CanCast("Blast Wave") && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 8  && IsAOE && !ChannelingShift)
            {
                API.CastSpell("Blast Wave");
                API.WriteLog("Blast Wave Targets :" + API.TargetUnitInRangeCount);
                return;
            }
            if (API.CanCast("Pyroblast") && InRange && Pyroclasm && API.PlayerHasBuff("Pyroclasm") && !API.PlayerIsMoving && !API.PlayerHasBuff("Combustion") && Level >= 12  && !ChannelingShift)
            {
                API.CastSpell("Pyroblast");
                return;
            }
            if (API.CanCast("Phoenix Flames") && InRange && API.SpellCharges("Phoenix Flames") >= 1 && API.PlayerHasBuff("Heating Up") && API.SpellCharges("Fire Blast") == 0 && Level >= 19  && !ChannelingShift)
            {
                API.CastSpell("Phoenix Flames");
                return;
            }
            if (API.CanCast("Phoenix Flames") && InRange && !API.PlayerHasBuff("Heating Up") && API.SpellCharges("Phoenix Flames") > 0 && Level >= 19  && !ChannelingShift)
            {
                API.CastSpell("Phoenix Flames");
                return;
            }
            if (API.CanCast("Scorch") && (!API.PlayerHasBuff("Heating Up") || API.PlayerHasBuff("Heating Up") && API.SpellCharges("Fire Blast") == 0 && API.SpellCharges("Phoenix Flames") == 0) && API.PlayerHasBuff("Combustion") && InRange && Level >= 13  && !ChannelingShift)
            {
                API.CastSpell("Scorch");
                API.WriteLog("Scorch While Combustion is up");
                return;
            }
            if (API.PlayerIsMoving && API.CanCast("Scorch") && InRange && Level >= 19  && !ChannelingShift)
            {
                API.CastSpell("Scorch");
                API.WriteLog("Scorch while moving");
                return;
            }
            if (API.CanCast("Scorch") && SearingTouch && API.TargetHealthPercent <= 30 && !API.PlayerHasBuff("Heating Up") && InRange && Level >= 13  && !ChannelingShift)
            {
                API.CastSpell("Scorch");
                API.WriteLog("Scorch under 30%");
                return;
            }
            if (API.CanCast("Scorch")  && SearingTouch && InRange && API.TargetHealthPercent <= 30 && (API.SpellCharges("Fire Blast") == 0 && API.SpellCharges("Phoenix Flames") == 0 || API.PlayerHasBuff("Heating Up")) && (!API.PlayerHasBuff("Combustion") || API.PlayerHasBuff("Combustion")) && !API.PlayerHasBuff("Pyroclasm") && Level >= 19 && !ChannelingShift)
            {
                API.CastSpell("Scorch");
                API.WriteLog("Scorch when no Fire Blast Charges");
                return;
            }
            if (API.CanCast("Fireball") && !API.PlayerIsMoving && InRange && (API.TargetHealthPercent > 30.0 && SearingTouch || !SearingTouch) && (API.SpellCharges("Phoenix Flames") == 0 && API.SpellCharges("Fire Blast") == 0 || !API.PlayerHasBuff("Heating Up")) && !API.PlayerHasBuff("Combustion")  && Level >= 10 && !ChannelingShift)
            {
                API.CastSpell("Fireball");
                return;
            }
        }
    }
}



