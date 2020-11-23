// Changelog
// v1.0 First release

namespace HyperElk.Core
{
    public class EnhancementShaman : CombatRoutine
    {
        //Spell,Auras
        private string LavaLash = "Lava Lash";
        private string CrashLightning = "Crash Lightning";
        private string Ascendance = "Ascendance";
        private string Windstrike = "Windstrike";
        private string LightningShield = "Lightning Shield";
        private string FeralSpirit = "Feral Spirit";
        private string Stormstrike = "Stormstrike";
        private string Sundering = "Sundering";
        private string GhostWolf = "Ghost Wolf";
        private string HealingSurge = "Healing Surge";
        private string LightningBolt = "Lightning Bolt";
        private string EarthenSpike = "Earthen Spike";
        private string WindShear = "Wind Shear";
        private string AstralShift = "Astral Shift";
        private string ChainLightning = "Chain Lightning";
        private string ElementalBlast = "Elemental Blast";
        private string FrostShock = "Frost Shock";
        private string IceStrike = "Ice Strike";
        private string FireNova = "Fire Nova";
        private string StormKeeper = "Storm Keeper";
        private string FlameShock = "Flame Shock";
        private string EarthElemental = "Earth Elemental";
        private string EarthShield = "Earth Shield";
        private string WindfuryTotem = "Windfury Totem";
        private string PrimalStrike = "Primal Strike";
        private string Stormbringer = "Stormbringer";
        private string HotHand = "Hot Hand";
        private string Hailstorm = "Hailstorm";
        private string MaelstromWeapon = "Maelstrom Weapon";
        private string HealingStreamTotem = "Healing Stream Totem";
        private string LashingFlames = "Lashing Flames";

        //Talents
        bool TalentLashingFlames => API.PlayerIsTalentSelected(1, 1);
        bool TalentElementalBlast => API.PlayerIsTalentSelected(1, 3);
        bool TalentIceStrike => API.PlayerIsTalentSelected(2, 3);
        bool TalentEarthShield => API.PlayerIsTalentSelected(3, 2);
        bool TalentFireNova => API.PlayerIsTalentSelected(4, 3);
        bool TalentStormkeeper => API.PlayerIsTalentSelected(6, 2);
        bool TalentSundering => API.PlayerIsTalentSelected(6, 3);
        bool TalentEarthenSpike => API.PlayerIsTalentSelected(7, 2);
        bool TalentAscendance => API.PlayerIsTalentSelected(7, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool isMelee => API.TargetRange < 6;
        private bool isinrange => API.TargetRange < 41;
        private bool iskickrange => API.TargetRange < 31;

        //CBProperties
        private bool AutoWolf => CombatRoutine.GetPropertyBool("AutoWolf");
        private bool SelfLightningShield => CombatRoutine.GetPropertyBool("LightningShield");
        private bool SelfEarthShield => CombatRoutine.GetPropertyBool("EarthShield");
        private int AstralShiftLifePercent => percentListProp[CombatRoutine.GetPropertyInt(AstralShift)];
        private int HealingStreamTotemLifePercent => percentListProp[CombatRoutine.GetPropertyInt(HealingStreamTotem)];
        private int HealingSurgeLifePercent => percentListProp[CombatRoutine.GetPropertyInt(HealingSurge)];
        private int HealingSurgeFreeLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Instant" + HealingSurge)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Enhancement Shaman v1.0 by smartie";
            API.WriteLog("Welcome to smartie`s Enhancement Shaman v1.0");

            //Spells
            CombatRoutine.AddSpell(LavaLash, "D3");
            CombatRoutine.AddSpell(CrashLightning, "D4");
            CombatRoutine.AddSpell(Ascendance, "D1");
            CombatRoutine.AddSpell(Windstrike, "D1");
            CombatRoutine.AddSpell(LightningShield, "F2");
            CombatRoutine.AddSpell(FeralSpirit, "D9");
            CombatRoutine.AddSpell(Stormstrike, "D1");
            CombatRoutine.AddSpell(Sundering, "D8");
            CombatRoutine.AddSpell(GhostWolf, "NumPad1");
            CombatRoutine.AddSpell(HealingSurge, "F5");
            CombatRoutine.AddSpell(LightningBolt, "D6");
            CombatRoutine.AddSpell(EarthenSpike, "NumPad8");
            CombatRoutine.AddSpell(WindShear, "F12");
            CombatRoutine.AddSpell(AstralShift, "F1");
            CombatRoutine.AddSpell(ChainLightning, "D5");
            CombatRoutine.AddSpell(ElementalBlast, "NumPad9");
            CombatRoutine.AddSpell(FrostShock, "F9");
            CombatRoutine.AddSpell(IceStrike, "D7");
            CombatRoutine.AddSpell(FireNova, "NumPad6");
            CombatRoutine.AddSpell(StormKeeper, "NumPad7");
            CombatRoutine.AddSpell(FlameShock, "D2");
            CombatRoutine.AddSpell(EarthElemental, "D0");
            CombatRoutine.AddSpell(EarthShield, "F7");
            CombatRoutine.AddSpell(WindfuryTotem, "NumPad2");
            CombatRoutine.AddSpell(PrimalStrike, "D1");
            CombatRoutine.AddSpell(HealingStreamTotem, "F4");

            //Buffs
            CombatRoutine.AddBuff(CrashLightning);
            CombatRoutine.AddBuff(Ascendance);
            CombatRoutine.AddBuff(Stormbringer);
            CombatRoutine.AddBuff(HotHand);
            CombatRoutine.AddBuff(LightningShield);
            CombatRoutine.AddBuff(GhostWolf);
            CombatRoutine.AddBuff(EarthShield);
            CombatRoutine.AddBuff(StormKeeper);
            CombatRoutine.AddBuff(Hailstorm);
            CombatRoutine.AddBuff(MaelstromWeapon);
            CombatRoutine.AddBuff(WindfuryTotem);

            //Debuff
            CombatRoutine.AddDebuff(FlameShock);
            CombatRoutine.AddDebuff(LashingFlames);

            //Prop
            CombatRoutine.AddProp("LightningShield", "LightningShield", true, "Put" + LightningShield + "on ourselfs", "Generic");
            CombatRoutine.AddProp("EarthShield", "EarthShield", true, "Put" + EarthShield + "on ourselfs", "Generic");
            CombatRoutine.AddProp("AutoWolf", "AutoWolf", true, "Will auto switch forms out of Fight", "Generic");
            CombatRoutine.AddProp(AstralShift, AstralShift + " Life Percent", percentListProp, "Life percent at which" + AstralShift + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(HealingStreamTotem, HealingStreamTotem + " Life Percent", percentListProp, "Life percent at which" + HealingStreamTotem + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp("Instant" + HealingSurge, HealingSurge + " Life Percent", percentListProp, "Life percent at which" + HealingSurge + "is used with Maelstorm Weapon Stacks, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(HealingSurge, HealingSurge + " Life Percent", percentListProp, "Life percent at which" + HealingSurge + "is used, set to 0 to disable", "Defense", 0);
        }
        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if (API.PlayerIsCasting || API.PlayerIsChanneling)
                return;
            if (!API.PlayerIsMounted)
            {
                if (isInterrupt && API.CanCast(WindShear) && PlayerLevel >= 12 && iskickrange)
                {
                    API.CastSpell(WindShear);
                    return;
                }
                if (API.CanCast(AstralShift) && PlayerLevel >= 42 && API.PlayerHealthPercent <= AstralShiftLifePercent)
                {
                    API.CastSpell(AstralShift);
                    return;
                }
                if (API.CanCast(HealingSurge) && PlayerLevel >= 4 && API.PlayerMana >= 24 && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && API.PlayerHealthPercent <= HealingSurgeFreeLifePercent)
                {
                    API.CastSpell(HealingSurge);
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
            if (AutoWolf && API.CanCast(GhostWolf) && PlayerLevel > 10 && !API.PlayerHasBuff(GhostWolf) && !API.PlayerIsMounted && API.PlayerIsMoving)
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
            if (API.CanCast(WindfuryTotem) && PlayerLevel >= 49 && API.PlayerMana >= 12 && !API.PlayerHasBuff(WindfuryTotem) && isMelee && !API.PlayerIsMoving)
            {
                API.CastSpell(WindfuryTotem);
                return;
            }
            // Single Target rota
            if (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE)
            {
                if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.PlayerMana >= 2 && !API.TargetHasDebuff(FlameShock) && isinrange)
                {
                    API.CastSpell(FlameShock);
                    return;
                }
                if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerMana >= 1 && isinrange && API.PlayerHasBuff(Hailstorm))
                {
                    API.CastSpell(FrostShock);
                    return;
                }
                if (API.CanCast(EarthenSpike) && isMelee && API.PlayerMana >= 4 && TalentEarthenSpike)
                {
                    API.CastSpell(EarthenSpike);
                    return;
                }
                if (API.CanCast(Ascendance) && isMelee && TalentAscendance && IsCooldowns)
                {
                    API.CastSpell(Ascendance);
                    return;
                }
                if (API.CanCast(Windstrike) && PlayerLevel >= 15 && API.PlayerHasBuff(Ascendance) && isMelee)
                {
                    API.CastSpell(Windstrike);
                    return;
                }
                if (API.CanCast(LightningBolt) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && API.PlayerHasBuff(StormKeeper) && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(LightningBolt);
                    return;
                }
                if (API.CanCast(StormKeeper) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && !API.PlayerIsMoving && IsCooldowns && isinrange && TalentStormkeeper)
                {
                    API.CastSpell(StormKeeper);
                    return;
                }
                if (API.CanCast(LightningBolt) && API.PlayerBuffStacks(MaelstromWeapon) >= 8 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(LightningBolt);
                    return;
                }
                if (API.CanCast(LavaLash) && PlayerLevel >= 11 && API.PlayerMana >= 4 && isMelee && TalentLashingFlames && !API.TargetHasDebuff(LashingFlames))
                {
                    API.CastSpell(LavaLash);
                    return;
                }
                if (API.CanCast(ElementalBlast) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && isinrange && API.PlayerMana >= 3 && TalentElementalBlast)
                {
                    API.CastSpell(ElementalBlast);
                    return;
                }
                if (API.CanCast(LavaLash) && PlayerLevel >= 11 && API.PlayerMana >= 4 && isMelee && API.PlayerHasBuff(HotHand))
                {
                    API.CastSpell(LavaLash);
                    return;
                }
                if (API.CanCast(PrimalStrike) && PlayerLevel < 20 && API.PlayerMana >= 10 && isMelee)
                {
                    API.CastSpell(PrimalStrike);
                    return;
                }
                if (API.CanCast(Stormstrike) && PlayerLevel >= 20 && API.PlayerMana >= 2 && isMelee)
                {
                    API.CastSpell(Stormstrike);
                    return;
                }
                if (API.CanCast(FeralSpirit) && PlayerLevel >= 34 && isMelee && IsCooldowns)
                {
                    API.CastSpell(FeralSpirit);
                    return;
                }
                if (API.CanCast(Sundering) && API.PlayerMana >= 6 && isMelee && TalentSundering)
                {
                    API.CastSpell(Sundering);
                    return;
                }
                if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.PlayerMana >= 2 && API.TargetDebuffRemainingTime(FlameShock) < 500 && isinrange)
                {
                    API.CastSpell(FlameShock);
                    return;
                }
                if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(FrostShock);
                    return;
                }
                if (API.CanCast(LavaLash) && PlayerLevel >= 11 && API.PlayerMana >= 4 && isMelee)
                {
                    API.CastSpell(LavaLash);
                    return;
                }
                if (API.CanCast(CrashLightning) && PlayerLevel >= 38 && API.PlayerMana >= 6 && isMelee)
                {
                    API.CastSpell(CrashLightning);
                    return;
                }
                if (API.CanCast(IceStrike) && isMelee && API.PlayerMana >= 4 && TalentIceStrike)
                {
                    API.CastSpell(IceStrike);
                    return;
                }
                if (API.CanCast(FireNova) && isMelee && API.PlayerMana >= 6 && TalentFireNova && API.TargetHasDebuff(FlameShock))
                {
                    API.CastSpell(FireNova);
                    return;
                }
                if (API.CanCast(EarthElemental) && isMelee && PlayerLevel >= 37 && IsCooldowns)
                {
                    API.CastSpell(EarthElemental);
                    return;
                }
                if (API.CanCast(LightningBolt) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(LightningBolt);
                    return;
                }
            }
            //AoE rota
            if (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE)
            {
                if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerMana >= 1 && isinrange && API.PlayerHasBuff(Hailstorm))
                {
                    API.CastSpell(FrostShock);
                    return;
                }
                if (API.CanCast(Sundering) && API.PlayerMana >= 6 && isMelee && TalentSundering)
                {
                    API.CastSpell(Sundering);
                    return;
                }
                if (API.CanCast(FeralSpirit) && PlayerLevel >= 34 && isMelee && IsCooldowns)
                {
                    API.CastSpell(FeralSpirit);
                    return;
                }
                if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.PlayerMana >= 2 && !API.TargetHasDebuff(FlameShock) && isinrange)
                {
                    API.CastSpell(FlameShock);
                    return;
                }
                if (API.CanCast(Ascendance) && isMelee && TalentAscendance && IsCooldowns)
                {
                    API.CastSpell(Ascendance);
                    return;
                }
                if (API.CanCast(Windstrike) && PlayerLevel >= 15 && API.PlayerHasBuff(Ascendance) && isMelee)
                {
                    API.CastSpell(Windstrike);
                    return;
                }
                if (API.CanCast(FireNova) && isMelee && API.PlayerMana >= 6 && TalentFireNova && API.TargetHasDebuff(FlameShock))
                {
                    API.CastSpell(FireNova);
                    return;
                }
                if (API.CanCast(CrashLightning) && PlayerLevel >= 38 && API.PlayerMana >= 6 && isMelee)
                {
                    API.CastSpell(CrashLightning);
                    return;
                }
                if (API.CanCast(ChainLightning) && PlayerLevel >= 24 && API.PlayerHasBuff(StormKeeper) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(ChainLightning);
                    return;
                }
                if (API.CanCast(ElementalBlast) && API.PlayerUnitInMeleeRangeCount < 3 && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && isinrange && API.PlayerMana >= 3 && TalentElementalBlast)
                {
                    API.CastSpell(ElementalBlast);
                    return;
                }
                if (API.CanCast(StormKeeper) && !API.PlayerIsMoving && IsCooldowns && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && isinrange && TalentStormkeeper)
                {
                    API.CastSpell(StormKeeper);
                    return;
                }
                if (API.CanCast(ChainLightning) && PlayerLevel >= 24 && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(ChainLightning);
                    return;
                }
                if (API.CanCast(LavaLash) && PlayerLevel >= 11 && API.PlayerMana >= 4 && isMelee && TalentLashingFlames && !API.TargetHasDebuff(LashingFlames))
                {
                    API.CastSpell(LavaLash);
                    return;
                }
                if (API.CanCast(Stormstrike) && PlayerLevel >= 20 && API.PlayerMana >= 2 && isMelee)
                {
                    API.CastSpell(Stormstrike);
                    return;
                }
                if (API.CanCast(PrimalStrike) && PlayerLevel < 20 && API.PlayerMana >= 10 && isMelee)
                {
                    API.CastSpell(PrimalStrike);
                    return;
                }
                if (API.CanCast(LavaLash) && PlayerLevel >= 11 && API.PlayerMana >= 4 && isMelee)
                {
                    API.CastSpell(LavaLash);
                    return;
                }
                if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(FrostShock);
                    return;
                }
                if (API.CanCast(IceStrike) && isMelee && API.PlayerMana >= 4 && TalentIceStrike)
                {
                    API.CastSpell(IceStrike);
                    return;
                }
                if (API.CanCast(EarthElemental) && isMelee && PlayerLevel >= 37 && IsCooldowns)
                {
                    API.CastSpell(EarthElemental);
                    return;
                }
                if (API.CanCast(EarthenSpike) && isMelee && API.PlayerMana >= 4 && TalentEarthenSpike)
                {
                    API.CastSpell(EarthenSpike);
                    return;
                }
            }

        }
    }
}
