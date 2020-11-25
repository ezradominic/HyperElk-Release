
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
        //General
        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;

        string[] CovenantList = new string[] { "None", "Venthyr", "Night Fae", "Kyrian", "Necrolord" };
        private string Covenant => CovenantList[CombatRoutine.GetPropertyInt("Covenant")];
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
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


            //Prop
            CombatRoutine.AddProp(BB, BB, percentListProp, "Life percent at which " + BB + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(IB, IB, percentListProp, "Life percent at which " + IB + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(MI, MI, percentListProp, "Life percent at which " + MI + " is used, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp("Covenant", "Covenant", CovenantList, "Choose your Covenant: None, Venthyr, Night Fae, Kyrian, Necrolord", "Generic", 0);


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
                if (API.CanCast("Blazing Barrier") && Level >= 21 && !API.PlayerHasBuff("Blazing Barrier") && API.PlayerHealthPercent <= BBPercentProc && API.PlayerHealthPercent != 0)
                {
                    API.CastSpell("Blazing Barrier");
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (isInterrupt && API.CanCast("Counterspell") && Level >= 7)
            {
                API.CastSpell("Counterspell");
                return;
            }
            if (API.CanCast("Ice Block") && API.PlayerHealthPercent <= IBPercentProc && API.PlayerHealthPercent != 0 && Level >= 22)
            {
                API.CastSpell("Ice Block");
                return;
            }
            if (API.CanCast("Mirror Image") && API.PlayerHealthPercent <= MIPercentProc && API.PlayerHealthPercent != 0 && Level >= 44)
            {
                API.CastSpell("Mirror Image");
                return;
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
            if (API.CanCast(RadiantSpark) && InRange && Covenant == "Kyrian")
            {
                API.CastSpell(RadiantSpark);
                return;
            }
            if (API.CanCast(MirrorsofTorment) && InRange && Covenant == "Venthyr" && IsCooldowns && NotChanneling)
            {
                API.CastSpell(MirrorsofTorment);
                return;
            }
            if (API.CanCast(Deathborne) && InRange && Covenant == "Necrolord" && IsCooldowns && NotChanneling)
            {
                API.CastSpell(Deathborne);
                return;
            }
            if (API.CanCast("Combustion") && Level >= 29 && !API.PlayerIsMoving && API.TargetRange <= 40 && IsCooldowns && Level >= 29)
            {
                API.CastSpell("Combustion");
                return;
            }
            if (API.CanCast(ShiftingPower) && InRange && Covenant == "Night Fae" && API.SpellISOnCooldown("Combustion") && !API.PlayerHasBuff("Combustion") && IsCooldowns)
            {
                API.CastSpell(ShiftingPower);
                return;
            }
            if (RuneOfPower && API.CanCast("Rune of Power") && API.TargetRange <= 40 && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && IsCooldowns && NotCasting && NotChanneling)
            {
                API.CastSpell("Rune of Power");
                return;
            }
            if (API.CanCast("Living Bomb") && LivingBomb && API.TargetUnitInRangeCount >= 2 && InRange && NotChanneling)
            {
                API.CastSpell("Living Bomb");
                return;
            }
            if (API.CanCast("Flamestrike") && InRange && API.PlayerHasBuff("Hot Streak!") && API.TargetUnitInRangeCount >= 3 && Level >= 17 && NotChanneling)
            {
                API.CastSpell("Flamestrike");
                API.WriteLog("Flamestrike Targets :" + API.TargetUnitInRangeCount);
                return;
            }
            if (API.CanCast("Pyroblast") && API.PlayerHasBuff("Hot Streak!") && InRange && Level >= 12 && NotChanneling)
            {
                API.CastSpell("Pyroblast");
                return;
            }
            if (API.CanCast("Fire Blast") && API.SpellCharges("Fire Blast") > 0 && API.PlayerHasBuff("Combustion") && API.PlayerHasBuff("Heating Up") && InRange && Level >= 33 && NotChanneling)
            {
                API.CastSpell("Fire Blast");
                return;
            }
            if (Meteor && API.CanCast("Meteor") && InRange && NotChanneling)
            {
                API.CastSpell("Meteor");
                return;
            }
            if (API.CanCast("Fire Blast") && API.SpellCharges("Fire Blast") > 0 && API.PlayerHasBuff("Heating Up") && InRange && Level >= 33 && NotChanneling) 
            {
                API.CastSpell("Fire Blast");
                return;
            }
            if (API.CanCast("Dragon's Breath") && API.TargetUnitInRangeCount >= 5 && API.TargetRange <= 10 && Level >= 27 && NotChanneling)
            {
                API.CastSpell("Dragon's Breath");
                API.WriteLog("Dragon's Breath Targets :" + API.TargetUnitInRangeCount);
                return;
            }
            if (BlastWave && API.CanCast("Blast Wave") && API.TargetUnitInRangeCount >= 5 && API.TargetRange <= 8 && NotChanneling)
            {
                API.CastSpell("Blast Wave");
                API.WriteLog("Blast Wave Targets :" + API.TargetUnitInRangeCount);
                return;
            }
            if (API.CanCast("Pyroblast") && InRange && Pyroclasm && API.PlayerHasBuff("Pyroclasm") && !API.PlayerIsMoving && !API.PlayerHasBuff("Combustion") && Level >= 12 && NotChanneling)
            {
                API.CastSpell("Pyroblast");
                return;
            }
            if (API.CanCast("Phoenix Flames") && InRange && API.SpellCharges("Phoenix Flames") >= 2 && Level >= 19 && NotChanneling) 
            {
                API.CastSpell("Phoenix Flames");
                return;
            }
            if (API.CanCast("Phoenix Flames") && InRange && API.SpellCharges("Phoenix Flames") >= 1 && !API.PlayerHasBuff("Heating Up") && API.SpellCharges("Fire Blast") == 0 && Level >= 19 && NotChanneling) 
            {
                API.CastSpell("Phoenix Flames");
                return;
            }
            if (API.CanCast("Phoenix Flames") && InRange && (API.PlayerHasBuff("Heating Up") && API.SpellCharges("Phoenix Flames") >= 1) && Level >= 19 && NotChanneling)
            {
                API.CastSpell("Phoenix Flames");
                return;
            }
            if (API.CanCast("Phoenix Flames") && InRange && (!API.PlayerHasBuff("Heating Up") && API.SpellCharges("Phoenix Flames") > 0) && Level >= 19 && NotChanneling) 
            {
                API.CastSpell("Phoenix Flames");
                return;
            }
            if (API.CanCast("Scorch") && !API.PlayerHasBuff("Heating Up") && API.PlayerHasBuff("Combustion") && InRange && Level >= 13 && NotCasting && NotChanneling)
            {
                API.CastSpell("Scorch");
                return;
            }
            if (API.CanCast("Scorch") && SearingTouch && API.TargetHealthPercent <= 30 && !API.PlayerHasBuff("Heating Up") && InRange && Level >= 13 && NotCasting && NotChanneling)  
            {
                API.CastSpell("Scorch");
                API.WriteLog("Scorch under 30%");
                return;
            }
            if (API.PlayerIsMoving && API.CanCast("Scorch") && InRange && Level >= 19 && NotCasting && NotChanneling)
            {
                API.CastSpell("Scorch");
                API.WriteLog("Scorch while moving");
                return;
            }
            if (API.CanCast("Scorch") && NotCasting && NotChanneling && SearingTouch && InRange && API.TargetHealthPercent <= 30 && (!API.PlayerHasBuff("Heating Up") || API.SpellCharges("Fire Blast") == 0) && !API.PlayerHasBuff("Combustion") && !API.PlayerHasBuff("Pyroclasm") && Level >= 19)
            {
                API.CastSpell("Scorch");
                API.WriteLog("Scorch when no Fire Blast Charges");
                return;
            }
            if (API.CanCast("Fireball") && NotCasting && NotChanneling && !API.PlayerIsMoving && InRange && API.TargetHealthPercent > 30.0 && !API.PlayerHasBuff("Heating Up") && !API.PlayerHasBuff("Hot Streak!") && !API.PlayerHasBuff("Combustion") || API.SpellCharges("Fire Blast") == 0 && !API.PlayerHasBuff("Combustion") && !API.PlayerHasBuff("Hot Streak!") && API.TargetHealthPercent > 30.0 && Level >= 10)
            {
                API.CastSpell("Fireball");
                return;
            }
        }

    }
}



