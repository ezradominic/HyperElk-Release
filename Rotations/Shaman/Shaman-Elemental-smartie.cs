// Changelog
// v1.0 First release
// v1.1 covenants + cd managment
using System.Diagnostics;


namespace HyperElk.Core
{
    public class ElementalShaman : CombatRoutine
    {
        //Spell,Auras
        private string ChainLightning = "Chain Lightning";
        private string Earthquake = "Earthquake";
        private string Icefury = "Icefury";
        private string LavaBurst = "Lava Burst";
        private string FrostShock = "Frost Shock";
        private string Stormkeeper = "Stormkeeper";
        private string LiquidManaTotem = "Liquid Mana Totem";
        private string ElementalBlast = "Elemental Blast";
        private string EarthShock = "Earth Shock";
        private string EarthElemental = "Earth Elemental";
        private string StormElemental = "Storm Elemental";
        private string FireElemental = "Fire Elemental";
        private string FlameShock = "Flame Shock";
        private string Ascendance = "Ascendance";
        private string GhostWolf = "Ghost Wolf";
        private string HealingSurge = "Healing Surge";
        private string LightningBolt = "Lightning Bolt";
        private string WindShear = "Wind Shear";
        private string AstralShift = "Astral Shift";
        private string EchoingShock = "Echoing Shock";
        private string EarthShield = "Earth Shield";
        private string LavaSurge = "Lava Surge";
        private string MasteroftheElements = "Master of the Elements";
        private string SurgeofPower = "Surge of Power";
        private string SpiritwalkersGrace = "Spiritwalker's Grace";
        private string HealingStreamTotem = "Healing Stream Totem";
        private string LightningShield = "Lightning Shield";
        private string PrimordialWave = "Primordial Wave";
        private string VesperTotem = "Vesper Totem";
        private string FaeTransfusion = "Fae Transfusion";
        private string ChainHarvest = "Chain Harvest";

        //Talents
        bool TalentEchoingShock => API.PlayerIsTalentSelected(2, 2);
        bool TalentElementalBlast => API.PlayerIsTalentSelected(2, 3);
        bool TalentEarthShield => API.PlayerIsTalentSelected(3, 2);
        bool TalentMasterofTheElements => API.PlayerIsTalentSelected(4, 1);
        bool TalentStormElemental => API.PlayerIsTalentSelected(4, 2);
        bool TalentLiquidManaTotem => API.PlayerIsTalentSelected(4, 3);
        bool TalentIcefury => API.PlayerIsTalentSelected(6, 3);
        bool TalentStormkeeper => API.PlayerIsTalentSelected(7, 2);
        bool TalentAscendance => API.PlayerIsTalentSelected(7, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsInRange => API.TargetRange < 41;
        private bool IsInKickRange => API.TargetRange < 31;
        bool IsAscendance => (UseAscendance == "with Cooldowns" && IsCooldowns || UseAscendance == "always");
        bool IsStormElemental => (UseStormElemental == "with Cooldowns" && IsCooldowns || UseStormElemental == "always");
        bool IsEarthElemental => (UseEarthElemental == "with Cooldowns" && IsCooldowns || UseEarthElemental == "always");
        bool IsFireElemental => (UseFireElemental == "with Cooldowns" && IsCooldowns || UseFireElemental == "always");
        bool IsStormkeeper => (UseStormkeeper == "with Cooldowns" && IsCooldowns || UseStormkeeper == "always" || UseStormkeeper == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE);

        bool IsCovenant => (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE);

        //CBProperties
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseAscendance => CDUsage[CombatRoutine.GetPropertyInt(Ascendance)];
        private string UseStormElemental => CDUsage[CombatRoutine.GetPropertyInt(StormElemental)];
        private string UseEarthElemental => CDUsage[CombatRoutine.GetPropertyInt(EarthElemental)];
        private string UseFireElemental => CDUsage[CombatRoutine.GetPropertyInt(FireElemental)];
        private string UseStormkeeper => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Stormkeeper)];
        private bool AutoWolf => CombatRoutine.GetPropertyBool("AutoWolf");
        private bool SelfLightningShield => CombatRoutine.GetPropertyBool("LightningShield");
        private bool SelfEarthShield => CombatRoutine.GetPropertyBool("EarthShield");
        private int AstralShiftLifePercent => percentListProp[CombatRoutine.GetPropertyInt(AstralShift)];
        private int HealingStreamTotemLifePercent => percentListProp[CombatRoutine.GetPropertyInt(HealingStreamTotem)];
        private int HealingSurgeLifePercent => percentListProp[CombatRoutine.GetPropertyInt(HealingSurge)];

        private static readonly Stopwatch stormwatch = new Stopwatch();
        private static readonly Stopwatch pullwatch = new Stopwatch();
        public override void Initialize()
        {
            CombatRoutine.Name = "Elemental Shaman by smartie";
            API.WriteLog("Welcome to smartie`s Elemental Shaman v1.1");

            //Spells
            CombatRoutine.AddSpell(ChainLightning, "D7");
            CombatRoutine.AddSpell(Earthquake, "D5");
            CombatRoutine.AddSpell(Icefury, "D9");
            CombatRoutine.AddSpell(LavaBurst, "D3");
            CombatRoutine.AddSpell(FrostShock, "D6");
            CombatRoutine.AddSpell(Stormkeeper, "NumPad2");
            CombatRoutine.AddSpell(LiquidManaTotem, "D1");
            CombatRoutine.AddSpell(ElementalBlast, "D8");
            CombatRoutine.AddSpell(EarthShock, "D4");
            CombatRoutine.AddSpell(EarthElemental, "D1");
            CombatRoutine.AddSpell(StormElemental, "D0");
            CombatRoutine.AddSpell(FireElemental, "D0");
            CombatRoutine.AddSpell(FlameShock, "D2");
            CombatRoutine.AddSpell(Ascendance, "D1");
            CombatRoutine.AddSpell(GhostWolf, "NumPad1");
            CombatRoutine.AddSpell(HealingSurge, "F1");
            CombatRoutine.AddSpell(LightningBolt, "D1");
            CombatRoutine.AddSpell(WindShear, "F12");
            CombatRoutine.AddSpell(AstralShift, "F4");
            CombatRoutine.AddSpell(EchoingShock, "NumPad3");
            CombatRoutine.AddSpell(EarthShield, "F7");
            CombatRoutine.AddSpell(HealingStreamTotem, "NumPad6");
            CombatRoutine.AddSpell(LightningShield, "F2");
            CombatRoutine.AddSpell(PrimordialWave, "D1");
            CombatRoutine.AddSpell(VesperTotem, "D1");
            CombatRoutine.AddSpell(FaeTransfusion, "D1");
            CombatRoutine.AddSpell(ChainHarvest, "D1");

            //Buffs
            CombatRoutine.AddBuff(LavaSurge);
            CombatRoutine.AddBuff(MasteroftheElements);
            CombatRoutine.AddBuff(Stormkeeper);
            CombatRoutine.AddBuff(Icefury);
            CombatRoutine.AddBuff(SurgeofPower);
            CombatRoutine.AddBuff(Ascendance);
            CombatRoutine.AddBuff(GhostWolf);
            CombatRoutine.AddBuff(EarthShield);
            CombatRoutine.AddBuff(LightningShield);
            CombatRoutine.AddBuff(SpiritwalkersGrace);
            CombatRoutine.AddBuff(PrimordialWave);

            //Debuff
            CombatRoutine.AddDebuff(FlameShock);


            //Prop
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Ascendance, "Use " + Ascendance, CDUsage, "Use " + Ascendance + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(StormElemental, "Use " + StormElemental, CDUsage, "Use " + StormElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(EarthElemental, "Use " + EarthElemental, CDUsage, "Use " + EarthElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(FireElemental, "Use " + FireElemental, CDUsage, "Use " + FireElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Stormkeeper, "Use " + Stormkeeper, CDUsageWithAOE, "Use " + Stormkeeper + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("LightningShield", "LightningShield", true, "Put" + LightningShield + "on ourselfs", "Generic");
            CombatRoutine.AddProp("EarthShield", "EarthShield", true, "Put" + EarthShield + "on ourselfs", "Generic");
            CombatRoutine.AddProp("AutoWolf", "AutoWolf", true, "Will auto switch forms out of Fight", "Generic");
            CombatRoutine.AddProp(AstralShift, AstralShift + " Life Percent", percentListProp, "Life percent at which" + AstralShift + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(HealingStreamTotem, HealingStreamTotem + " Life Percent", percentListProp, "Life percent at which" + HealingStreamTotem + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(HealingSurge, HealingSurge + " Life Percent", percentListProp, "Life percent at which" + HealingSurge + "is used, set to 0 to disable", "Defense", 0);
        }
        public override void Pulse()
        {
            if (!pullwatch.IsRunning && API.PlayerIsInCombat)
            {
                pullwatch.Start();
                API.WriteLog("Entering Combat, Starting opener timer.");
            }
            // Stopwatch stop
            if (!API.PlayerIsInCombat && pullwatch.ElapsedMilliseconds >= 1000)
            {
                pullwatch.Reset();
                API.WriteLog("Leaving Combat, Resetting opener timer.");
            }
            if (stormwatch.IsRunning && stormwatch.ElapsedMilliseconds > 30000)
            {
                stormwatch.Reset();
                API.WriteLog("Resetting Stormwatch.");
            }
        }
        public override void CombatPulse()
        {
            if (API.PlayerIsCasting || API.PlayerIsChanneling)
                return;
            if (!API.PlayerIsMounted)
            {
                if (isInterrupt && API.CanCast(WindShear) && PlayerLevel >= 12 && IsInKickRange)
                {
                    API.CastSpell(WindShear);
                    return;
                }
                if (API.CanCast(AstralShift) && PlayerLevel >= 42 && API.PlayerHealthPercent <= AstralShiftLifePercent)
                {
                    API.CastSpell(AstralShift);
                    return;
                }
                if (API.CanCast(HealingSurge) && PlayerLevel >= 4 && API.PlayerMana >= 24 && !API.PlayerIsMoving && API.PlayerHealthPercent <= HealingSurgeLifePercent)
                {
                    API.CastSpell(HealingSurge);
                    return;
                }
                if (API.CanCast(HealingStreamTotem) && PlayerLevel >= 28 && API.PlayerMana >= 9 && !API.PlayerIsMoving && API.PlayerHealthPercent <= HealingStreamTotemLifePercent)
                {
                    API.CastSpell(HealingStreamTotem);
                    return;
                }
                if (API.CanCast(LightningShield) && PlayerLevel >= 9 && API.PlayerMana >= 2 && SelfLightningShield && !API.PlayerHasBuff(EarthShield) && !API.PlayerHasBuff(LightningShield) && API.PlayerHealthPercent > 0)
                {
                    API.CastSpell(LightningShield);
                    return;
                }
                if (API.CanCast(EarthShield) && API.PlayerMana >= 10 && SelfEarthShield && !API.PlayerHasBuff(LightningShield) && !API.PlayerHasBuff(EarthShield) && TalentEarthShield)
                {
                    API.CastSpell(EarthShield);
                    return;
                }
                rotation();
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerIsCasting || API.PlayerIsChanneling)
                return;
            if (AutoWolf && API.CanCast(GhostWolf) && !API.PlayerHasBuff(GhostWolf) && !API.PlayerIsMounted && API.PlayerIsMoving)
            {
                API.CastSpell(GhostWolf);
                return;
            }
            if (API.CanCast(LightningShield) && PlayerLevel >= 9 && API.PlayerMana >= 2 && SelfLightningShield && !API.PlayerHasBuff(EarthShield) && !API.PlayerHasBuff(LightningShield) && API.PlayerHealthPercent > 0)
            {
                API.CastSpell(LightningShield);
                return;
            }
            if (API.CanCast(EarthShield) && API.PlayerMana >= 10 && SelfEarthShield && !API.PlayerHasBuff(LightningShield) && !API.PlayerHasBuff(EarthShield) && TalentEarthShield)
            {
                API.CastSpell(EarthShield);
                return;
            }
        }
        private void rotation()
        {
            if (IsInRange)
            {
                if (API.CanCast(FireElemental) && PlayerLevel >= 34 && !TalentStormElemental && IsFireElemental)
                {
                    API.CastSpell(FireElemental);
                    return;
                }
                if (API.CanCast(StormElemental) && TalentStormElemental && IsStormElemental)
                {
                    API.CastSpell(StormElemental);
                    stormwatch.Start();
                    API.WriteLog("Starting Stormwatch.");
                    return;
                }
                if (API.CanCast(EarthElemental) && PlayerLevel >= 37 && (!API.PlayerHasPet && TalentStormElemental || !TalentStormElemental) && IsEarthElemental)
                {
                    API.CastSpell(EarthElemental);
                    return;
                }
                if (API.CanCast(PrimordialWave) && IsCovenant && PlayerCovenantSettings == "Necrolord" && API.PlayerMana >= 3 && !API.PlayerHasBuff(PrimordialWave) && IsInRange)
                {
                    API.CastSpell(PrimordialWave);
                    return;
                }
                if (API.CanCast(VesperTotem) && PlayerCovenantSettings == "Kyrian" && IsCovenant && API.PlayerMana >= 10 && IsInRange)
                {
                    API.CastSpell(VesperTotem);
                    return;
                }
                if (API.CanCast(FaeTransfusion) && IsInRange && !API.PlayerIsMoving && API.PlayerMana >= 8 && PlayerCovenantSettings == "Night Fae" && IsCovenant)
                {
                    API.CastSpell(FaeTransfusion);
                    return;
                }
                // Single Target rota
                if (API.TargetUnitInRangeCount < AOEUnitNumber || !IsAOE)
                {
                    if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.TargetDebuffRemainingTime(FlameShock) < 600)
                    {
                        API.CastSpell(FlameShock);
                        return;
                    }
                    if (API.CanCast(LightningBolt) && API.PlayerHasBuff(Stormkeeper) && API.PlayerHasBuff(MasteroftheElements))
                    {
                        API.CastSpell(LightningBolt);
                        return;
                    }
                    if (API.CanCast(EchoingShock) && TalentEchoingShock && (API.PlayerMaelstrom >= 60 || API.CanCast(LavaBurst)))
                    {
                        API.CastSpell(EchoingShock);
                        return;
                    }
                    if (API.CanCast(EarthShock) && PlayerLevel >= 10 && API.PlayerMaelstrom >= 60 && API.PlayerHasBuff(MasteroftheElements) && TalentMasterofTheElements)
                    {
                        API.CastSpell(EarthShock);
                        return;
                    }
                    if (API.CanCast(ElementalBlast) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentElementalBlast && !API.PlayerHasBuff(MasteroftheElements))
                    {
                        API.CastSpell(ElementalBlast);
                        return;
                    }
                    if (API.CanCast(LiquidManaTotem) && TalentLiquidManaTotem)
                    {
                        API.CastSpell(LiquidManaTotem);
                        return;
                    }
                    if (API.CanCast(Stormkeeper) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentStormkeeper && IsStormkeeper)
                    {
                        API.CastSpell(Stormkeeper);
                        return;
                    }
                    if (API.CanCast(EarthShock) && PlayerLevel >= 10 && API.PlayerMaelstrom >= 90)
                    {
                        API.CastSpell(EarthShock);
                        return;
                    }
                    if (API.CanCast(LightningBolt) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && stormwatch.IsRunning)
                    {
                        API.CastSpell(LightningBolt);
                        return;
                    }
                    if (API.CanCast(Ascendance) && TalentAscendance && IsAscendance)
                    {
                        API.CastSpell(Ascendance);
                        return;
                    }
                    if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && API.PlayerHasBuff(Ascendance))
                    {
                        API.CastSpell(LavaBurst);
                        return;
                    }
                    if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerHasBuff(Icefury) && API.PlayerHasBuff(MasteroftheElements))
                    {
                        API.CastSpell(FrostShock);
                        return;
                    }
                    if (API.CanCast(ChainHarvest) && IsInRange && API.PlayerMana >= 10 && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                    {
                        API.CastSpell(ChainHarvest);
                        return;
                    }
                    if (API.CanCast(LightningBolt) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && API.PlayerHasBuff(SurgeofPower))
                    {
                        API.CastSpell(LightningBolt);
                        return;
                    }
                    if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace) || API.PlayerHasBuff(LavaSurge)))
                    {
                        API.CastSpell(LavaBurst);
                        return;
                    }
                    if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerHasBuff(Icefury))
                    {
                        API.CastSpell(FrostShock);
                        return;
                    }
                    if (API.CanCast(Icefury) && TalentIcefury && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)))
                    {
                        API.CastSpell(Icefury);
                        return;
                    }
                    if (API.CanCast(LightningBolt) && API.PlayerMaelstrom < 90 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)))
                    {
                        API.CastSpell(LightningBolt);
                        return;
                    }
                    if (API.CanCast(FrostShock) && PlayerLevel >= 17 && PlayerLevel >= 38 && API.PlayerIsMoving)
                    {
                        API.CastSpell(FrostShock);
                        return;
                    }
                }
                //AoE rota
                if (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE)
                {
                    if (API.CanCast(Stormkeeper) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentStormkeeper && IsStormkeeper)
                    {
                        API.CastSpell(Stormkeeper);
                        return;
                    }
                    if (API.CanCast(LiquidManaTotem) && TalentLiquidManaTotem)
                    {
                        API.CastSpell(LiquidManaTotem);
                        return;
                    }
                    if (API.CanCast(EchoingShock) && API.PlayerMaelstrom >= 60 && TalentEchoingShock)
                    {
                        API.CastSpell(EchoingShock);
                        return;
                    }
                    if (API.CanCast(FlameShock) && PlayerLevel >= 3 && (!API.TargetHasDebuff(FlameShock) || API.TargetDebuffRemainingTime(FlameShock) < 600))
                    {
                        API.CastSpell(FlameShock);
                        return;
                    }
                    if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && API.PlayerMaelstrom >= 50 && API.PlayerHasBuff(LavaSurge) && TalentMasterofTheElements)
                    {
                        API.CastSpell(LavaBurst);
                        return;
                    }
                    if (API.CanCast(Earthquake) && PlayerLevel >= 38 && API.PlayerMaelstrom >= 60)
                    {
                        API.CastSpell(Earthquake);
                        return;
                    }
                    if (API.CanCast(EarthShock) && PlayerLevel >= 10 && PlayerLevel < 38 && API.PlayerMaelstrom >= 60)
                    {
                        API.CastSpell(EarthShock);
                        return;
                    }
                    if (API.CanCast(ChainLightning) && PlayerLevel >= 24 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace) || API.PlayerHasBuff(Stormkeeper)))
                    {
                        API.CastSpell(ChainLightning);
                        return;
                    }
                    if (API.CanCast(LightningBolt) && PlayerLevel < 24 && API.PlayerMaelstrom < 90 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)))
                    {
                        API.CastSpell(LightningBolt);
                        return;
                    }
                    if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerIsMoving)
                    {
                        API.CastSpell(FrostShock);
                        return;
                    }
                }
            }
        }
    }
}

