
namespace HyperElk.Core
{
    public class FrostMage : CombatRoutine
    {
        //Spell Strings
        private string Flurry = "Flurry";
        private string Icicles = "Icicles";
        private string BrainFreeze = "Brain Freeze";
        private string FoF = "Fingers of Frost";
        private string IF = "Ice Floes";
        private string RoP = "Rune of Power";
        private string IceBarrier = "Ice Barrier";
        private string AI = "Arcane Intellect";
        private string Frostbolt = "Frostbolt";
        private string IL = "Ice Lance";
        private string CoC = "Cone of Cold";
        private string EB = "Ebonbolt";
        private string  Blizzard = "Blizzard";
        private string  GS = "Glacial Spike";
        private string  IV = "Icy Veins";
        private string MI = "Mirror Image";
        private string  IN = "Ice Nova";
        private string  FO ="Frozen Orb";
        private string  CS = "Comet Storm";
        private string RoF = "Ray of Frost";
        private string  Freeze = "Freeze";
        private string  WE = "Water Elemental";
        private string Counterspell = "Counterspell";
        private string WC = "Winter's Chill";
        private string IB = "Ice Block";
        private string ShiftingPower = "Shifting Power";
        private string RadiantSpark = "Radiant Spark";
        private string Deathborne = "Deathborne";
        private string MirrorsofTorment = "Mirrors of Torment";
        private string AE = "Arcane Explosion";
        private string Fleshcraft = "Fleshcraft";
        private string FR = "Freezing Rain";

        //Talents
        bool LonelyWinter => API.PlayerIsTalentSelected(1, 2);
        bool IceNova => API.PlayerIsTalentSelected(1, 3);
        bool IceFloes => API.PlayerIsTalentSelected(2, 3);
        bool RuneOfPower => API.PlayerIsTalentSelected(3, 3);
        bool Ebonbolt => API.PlayerIsTalentSelected(4, 3);
        bool Cometstorm => API.PlayerIsTalentSelected(6, 3);
        bool RayofFrost => API.PlayerIsTalentSelected(7, 2);
        bool GlacialSpike => API.PlayerIsTalentSelected(7, 3);
        bool FreezingRain => API.PlayerIsTalentSelected(6, 1);

        //CBProperties
        private int IceBarrierPercentProc => percentListProp[CombatRoutine.GetPropertyInt(IceBarrier)];
        private int IBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(IB)];
        private int MIPercentProc => percentListProp[CombatRoutine.GetPropertyInt(MI)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private string UseROP => CDUsage[CombatRoutine.GetPropertyInt(RoP)];
        private string UseIV => CDUsage[CombatRoutine.GetPropertyInt(IV)];
        private int FleshcraftPercentProc => percentListProp[CombatRoutine.GetPropertyInt(Fleshcraft)];
        //General
        bool CastFlurry => API.PlayerLastSpell == Flurry;
        bool CastShifting => API.PlayerLastSpell == ShiftingPower;
        private int Level => API.PlayerLevel;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool InRange => API.TargetRange <= 40;



        public override void Initialize()
        {
            CombatRoutine.Name = "Frost Mage by Ryu";
            API.WriteLog("Welcome to Frost Mage by Ryu");
            API.WriteLog("Create the following cursor macro for Blizzard");
            API.WriteLog("Blizzard -- /cast [@cursor] Blizzard");
            API.WriteLog("Create Macro /cast [@Player] Arcane Intellect to buff Arcane Intellect so you don't require a target");
            API.WriteLog("All Talents expect Ring of Frost supported. All Cooldowns are associated with Cooldown toggle.");
            //Buff

            CombatRoutine.AddBuff(Icicles);
            CombatRoutine.AddBuff(BrainFreeze);
            CombatRoutine.AddBuff(FoF);
            CombatRoutine.AddBuff(IF);
            CombatRoutine.AddBuff(RoP);
            CombatRoutine.AddBuff(IceBarrier);
            CombatRoutine.AddBuff(AI);
            CombatRoutine.AddBuff(IV);
            CombatRoutine.AddBuff(FR);

            //Debuff
            CombatRoutine.AddDebuff(WC);

            //Spell
            CombatRoutine.AddSpell(RoP, "None");
            CombatRoutine.AddSpell(IB);
            CombatRoutine.AddSpell(Frostbolt);
            CombatRoutine.AddSpell(IL);
            CombatRoutine.AddSpell(Flurry);
            CombatRoutine.AddSpell(CoC);
            CombatRoutine.AddSpell(EB);
            CombatRoutine.AddSpell(Blizzard);
            CombatRoutine.AddSpell(GS, "C");
            CombatRoutine.AddSpell(IV, "C");
            CombatRoutine.AddSpell(IceBarrier, "C");
            CombatRoutine.AddSpell(MI, "C");
            CombatRoutine.AddSpell(IN, "None");
            CombatRoutine.AddSpell(FO, "None");
            CombatRoutine.AddSpell(CS, "None");
            CombatRoutine.AddSpell(RoF, "None");
            CombatRoutine.AddSpell(Freeze, "None");
            CombatRoutine.AddSpell(WE, "None");
            CombatRoutine.AddSpell(IF, "None");
            CombatRoutine.AddSpell(AI, "None");
            CombatRoutine.AddSpell(Counterspell, "None");
            CombatRoutine.AddSpell(ShiftingPower);
            CombatRoutine.AddSpell(RadiantSpark);
            CombatRoutine.AddSpell(Deathborne);
            CombatRoutine.AddSpell(MirrorsofTorment);
            CombatRoutine.AddSpell(AE);
            CombatRoutine.AddSpell(Fleshcraft);

            //Macro

            //Prop
            CombatRoutine.AddProp(IceBarrier, IceBarrier, percentListProp, "Life percent at which " + IceBarrier + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(IB, IB, percentListProp, "Life percent at which " + IB + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(MI, MI, percentListProp, "Life percent at which " + MI + " is used, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", percentListProp, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 1);
            CombatRoutine.AddProp(RoP, "Use " + RoP, CDUsage, "Use " + RoP + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp(IV, "Use " + IV, CDUsage, "Use " + IV + "On Cooldown, With Cooldowns or Not Used", "Cooldowns", 0);

        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted)
            {
                if (API.CanCast(AI) && Level >= 8 && !API.PlayerHasBuff(AI))
                {
                    API.CastSpell(AI);
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (isInterrupt && API.CanCast(Counterspell) && Level >= 7)
            {
                API.CastSpell(Counterspell);
                return;
            }
            if (API.CanCast(IB) && API.PlayerHealthPercent <= IBPercentProc && API.PlayerHealthPercent != 0 && Level >= 22 && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(IB);
                return;
            }
            if (API.CanCast(MI) && API.PlayerHealthPercent <= MIPercentProc && API.PlayerHealthPercent !=0 && Level >= 44 && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(MI);
                return;
            }
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.CanCast(IceBarrier) && Level >= 21 && !API.PlayerHasBuff(IceBarrier) && API.PlayerHealthPercent <= IceBarrierPercentProc && API.PlayerHealthPercent != 0 && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(IceBarrier);
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
            if (API.CanCast(Deathborne) && InRange && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !CastShifting)
            {
                API.CastSpell(Deathborne);
                return;
            }
            if (API.CanCast(IV) && Level >= 29 && !API.PlayerIsMoving && API.TargetRange <= 40 && (IsCooldowns && UseIV == "With Cooldowns" || UseIV == "On Cooldown") && NotChanneling && !CastShifting)
            {
                API.CastSpell(IV);
                return;
            }
            if (API.CanCast(ShiftingPower) && InRange && PlayerCovenantSettings == "Night Fae" && (API.SpellISOnCooldown(RoP) || !RuneOfPower) && API.SpellISOnCooldown(IV) && !API.PlayerHasBuff(IV) && !API.PlayerHasBuff(RoP) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE))
            {
                API.CastSpell(ShiftingPower);
                return;
            }
            if (RuneOfPower && API.CanCast(RoP) && API.TargetRange <= 40 && !API.PlayerHasBuff(RoP) && !API.PlayerIsMoving && (IsCooldowns && UseROP == "With Cooldowns" || UseROP == "On Cooldown") && NotChanneling && API.SpellCDDuration(IV) >= 1500 && !CastShifting) 
            {
                API.CastSpell(RoP);
                return;
            }
            if (API.CanCast(IF) && IceFloes && API.PlayerIsMoving && !API.PlayerHasBuff(IF) && NotChanneling && !CastShifting)
            {
                API.CastSpell(IF);
                return;
            }
            if (!API.PlayerHasPet && !LonelyWinter && API.CanCast(WE) && !API.PlayerIsMoving && Level >= 12 && NotChanneling)
            {
                API.CastSpell(WE);
                return;
            }
            if (API.CanCast(Flurry) && Level >= 19 && API.PlayerHasBuff(BrainFreeze) && !API.TargetHasDebuff(WC) && API.TargetRange <= 40 && NotChanneling)
            {
                API.CastSpell(Flurry);
                return;
            }
            if (API.CanCast(FO) && Level >= 38 && API.TargetRange <= 40 && NotChanneling && !CastShifting)
            {
                API.CastSpell(FO);
                return;
            }
            if (API.CanCast(Blizzard) && Level >= 14 && API.TargetRange <= 40 && !API.PlayerIsMoving && (API.TargetUnitInRangeCount >= 3 && IsAOE || FreezingRain && API.PlayerHasBuff(FR)) && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(Blizzard);
                return;
            }
            if (API.CanCast(Blizzard) && Level >= 14 && API.TargetRange <= 40 && API.PlayerHasBuff(IF) && API.PlayerIsMoving && API.TargetUnitInRangeCount >= 3 && IsAOE && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(Blizzard);
                return;
            }
            if (Cometstorm && API.CanCast(CS) && API.TargetRange <= 40 && !API.PlayerIsMoving && NotChanneling && !CastShifting)
            {
                API.CastSpell(CS);
                return;
            }
            if (IceNova && API.CanCast(IN) && API.TargetRange <= 40 && (API.PlayerIsMoving || !API.PlayerIsMoving) && NotChanneling && !CastShifting)
            {
                API.CastSpell(IN);
                return;
            }
            if (API.CanCast(CoC) && Level >= 18 && API.TargetRange <= 10 && API.TargetUnitInRangeCount >= 3 && IsAOE && NotChanneling && !CastShifting)
            {
                API.CastSpell(CoC);
                return;
            }
            if (RayofFrost && API.CanCast(RoF) && (API.TargetHasDebuff(WC) && API.TargetDebuffStacks(WC) <= 1) && API.PlayerHasBuff(IF) && API.PlayerIsMoving && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(RoF);
                return;
            }
            if (RayofFrost && API.CanCast(RoF) && API.TargetHasDebuff(WC) && !API.PlayerIsMoving && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(RoF);
                return;
            }
            if (GlacialSpike && API.CanCast(GS) && API.TargetHasDebuff(WC) && API.TargetRange <= 40 && API.PlayerBuffStacks(Icicles) > 4 && !API.PlayerIsMoving && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(GS);
                return;
            }
            if (GlacialSpike && API.CanCast(GS) && API.TargetHasDebuff(WC) && API.TargetRange <= 40 && API.PlayerBuffStacks(Icicles) > 4 && API.PlayerHasBuff(IF) && API.PlayerIsMoving && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(GS);
                return;
            }
            if (API.CanCast(IL) && Level >= 10 && API.TargetRange <= 40 && API.TargetHasDebuff(WC) && NotChanneling && !CastShifting) 
            {
                API.CastSpell(IL);
                return;
            }
            if (API.CanCast(IL) && Level >= 10 && API.TargetRange <= 40 && API.PlayerHasBuff(FoF) && NotChanneling && !CastShifting)
            {
                API.CastSpell(IL);
                return;
            }
            if (Ebonbolt && API.CanCast(EB) && API.TargetRange <= 40 && !API.PlayerHasBuff(BrainFreeze) && API.PlayerBuffStacks(Icicles) > 4 && !API.PlayerIsMoving && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(EB);
                return;
            }
            if (Ebonbolt && API.CanCast(EB) && API.TargetRange <= 40 && !API.PlayerHasBuff(BrainFreeze) && API.PlayerBuffStacks(Icicles) > 4 && API.PlayerHasBuff(IF) && API.PlayerIsMoving && NotCasting && NotChanneling && !CastShifting)
            {
                API.CastSpell(EB);
                return;
            }
            if (API.CanCast(RadiantSpark) && API.PlayerHasBuff(BrainFreeze) && InRange && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling)
            {
                API.CastSpell(RadiantSpark);
                return;
            }
            if (API.CanCast(MirrorsofTorment) && InRange && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling)
            {
                API.CastSpell(MirrorsofTorment);
                return;
            }
            if (API.CanCast(AE) && API.TargetRange <= 3 && IsAOE && API.PlayerUnitInMeleeRangeCount >= 3 && !CastShifting)
            {
                API.CastSpell(AE);
                return;
            }
            if (API.CanCast(Frostbolt) && Level >= 1 && API.TargetRange <= 40 && !API.PlayerIsMoving && NotCasting && NotChanneling)
            {
                API.CastSpell(Frostbolt);
                return;
            }
            if (API.CanCast(Frostbolt) && Level >= 1 && API.TargetRange <= 40 && API.PlayerIsMoving && API.PlayerHasBuff(IF) && NotCasting && NotChanneling)
            {
                API.CastSpell(Frostbolt);
                return;
            }
            if (API.PlayerIsMoving && API.CanCast(IL) && Level >= 10 && !API.PlayerHasBuff(IF) && API.TargetRange <= 40 && NotChanneling)
            {
                API.CastSpell(IL);
                return;
            }

        }

    }
}



