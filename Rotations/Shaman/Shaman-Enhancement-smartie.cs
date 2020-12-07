// Changelog
// v1.0 First release
// v1.1 covenants and cd managment
// v1.2 vesper totem fix
// v1.3 legendary preperation
// v1.4 Racials and Trinkets

using System.Diagnostics;
namespace HyperElk.Core
{
    public class EnhancementShaman : CombatRoutine
    {
        private bool WindfuryToggle => API.ToggleIsEnabled("Windfury Totem");
        private bool SunderingToggle => API.ToggleIsEnabled("Sundering");
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
        private string StormKeeper = "Stormkeeper";
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
        private string PrimordialWave = "Primordial Wave";
        private string VesperTotem = "Vesper Totem";
        private string FaeTransfusion = "Fae Transfusion";
        private string ChainHarvest = "Chain Harvest";
        private string DoomWinds = "Doom Winds";

        //Talents
        bool TalentLashingFlames => API.PlayerIsTalentSelected(1, 1);
        bool TalentElementalBlast => API.PlayerIsTalentSelected(1, 3);
        bool TalentIceStrike => API.PlayerIsTalentSelected(2, 3);
        bool TalentEarthShield => API.PlayerIsTalentSelected(3, 2);
        bool TalentFireNova => API.PlayerIsTalentSelected(4, 3);
        bool TalentStormkeeper => API.PlayerIsTalentSelected(6, 2);
        bool TalentCrashingStorm => API.PlayerIsTalentSelected(6, 1);
        bool TalentSundering => API.PlayerIsTalentSelected(6, 3);
        bool TalentEarthenSpike => API.PlayerIsTalentSelected(7, 2);
        bool TalentAscendance => API.PlayerIsTalentSelected(7, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool isMelee => API.TargetRange < 6;
        private bool isinrange => API.TargetRange < 41;
        private bool iskickrange => API.TargetRange < 31;
        bool IsAscendance => (UseAscendance == "with Cooldowns" && IsCooldowns || UseAscendance == "always");
        bool IsFeralSpirit => (UseFeralSpirit == "with Cooldowns" && IsCooldowns || UseFeralSpirit == "always");
        bool IsEarthElemental => (UseEarthElemental == "with Cooldowns" && IsCooldowns || UseEarthElemental == "always");
        bool IsSundering => (UseSundering == "with Cooldowns" && IsCooldowns || UseSundering == "always" || UseSundering == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE);
        bool IsFireNova => (UseFireNova == "with Cooldowns" && IsCooldowns || UseFireNova == "always" || UseFireNova == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE);
        bool IsStormKeeper => (UseStormKeeper == "with Cooldowns" && IsCooldowns || UseStormKeeper == "always" || UseStormKeeper == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE);

        bool IsCovenant => (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE);
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isMelee;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isMelee;


        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseAscendance => CDUsage[CombatRoutine.GetPropertyInt(Ascendance)];
        private string UseFeralSpirit => CDUsage[CombatRoutine.GetPropertyInt(FeralSpirit)];
        private string UseEarthElemental => CDUsage[CombatRoutine.GetPropertyInt(EarthElemental)];
        private string UseSundering => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Sundering)];
        private string UseFireNova => CDUsageWithAOE[CombatRoutine.GetPropertyInt(FireNova)];
        private string UseStormKeeper => CDUsageWithAOE[CombatRoutine.GetPropertyInt(StormKeeper)];
        private bool AutoWolf => CombatRoutine.GetPropertyBool("AutoWolf");
        private bool DoomWindLeggy => CombatRoutine.GetPropertyBool("Doom Winds");
        private bool SelfLightningShield => CombatRoutine.GetPropertyBool("LightningShield");
        private bool SelfEarthShield => CombatRoutine.GetPropertyBool("EarthShield");
        private int AstralShiftLifePercent => percentListProp[CombatRoutine.GetPropertyInt(AstralShift)];
        private int HealingStreamTotemLifePercent => percentListProp[CombatRoutine.GetPropertyInt(HealingStreamTotem)];
        private int HealingSurgeLifePercent => percentListProp[CombatRoutine.GetPropertyInt(HealingSurge)];
        private int HealingSurgeFreeLifePercent => percentListProp[CombatRoutine.GetPropertyInt("InstantHealingSurge")];


        private static readonly Stopwatch vesperwatch = new Stopwatch();
        public override void Initialize()
        {
            CombatRoutine.Name = "Enhancement Shaman by smartie";
            API.WriteLog("Welcome to smartie`s Enhancement Shaman v1.4");

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
            CombatRoutine.AddSpell(PrimordialWave, "D1");
            CombatRoutine.AddSpell(VesperTotem, "D1");
            CombatRoutine.AddSpell(FaeTransfusion, "D1");
            CombatRoutine.AddSpell(ChainHarvest, "D1");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

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
            CombatRoutine.AddBuff(PrimordialWave);
            CombatRoutine.AddBuff(VesperTotem);

            //Debuff
            CombatRoutine.AddDebuff(FlameShock);
            CombatRoutine.AddDebuff(LashingFlames);
            CombatRoutine.AddDebuff(DoomWinds);

            //Toggle
            CombatRoutine.AddToggle("Windfury Totem");
            CombatRoutine.AddToggle("Sundering");

            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Ascendance, "Use " + Ascendance, CDUsage, "Use " + Ascendance + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(FeralSpirit, "Use " + FeralSpirit, CDUsage, "Use " + FeralSpirit + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(EarthElemental, "Use " + EarthElemental, CDUsage, "Use " + EarthElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Sundering, "Use " + Sundering, CDUsageWithAOE, "Use " + Sundering + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(FireNova, "Use " + FireNova, CDUsageWithAOE, "Use " + FireNova + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(StormKeeper, "Use " + StormKeeper, CDUsageWithAOE, "Use " + StormKeeper + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("LightningShield", "LightningShield", true, "Put" + LightningShield + " on ourselfs", "Generic");
            CombatRoutine.AddProp("EarthShield", "EarthShield", true, "Put" + EarthShield + " on ourselfs", "Generic");
            CombatRoutine.AddProp("AutoWolf", "AutoWolf", true, "Will auto switch forms out of Fight", "Generic");
            CombatRoutine.AddProp("Doom Winds", "Doom Winds Legendary", false, "Pls enable if you have that Legendary", "Generic");
            CombatRoutine.AddProp(AstralShift, AstralShift + " Life Percent", percentListProp, "Life percent at which" + AstralShift + " is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(HealingStreamTotem, HealingStreamTotem + " Life Percent", percentListProp, "Life percent at which" + HealingStreamTotem + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp("InstantHealingSurge", "Instant HealingSurge" + "Life Percent", percentListProp, "Life percent at which" + HealingSurge + " is used with Maelstorm Weapon Stacks, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(HealingSurge, HealingSurge + " Life Percent", percentListProp, "Life percent at which" + HealingSurge + " is used, set to 0 to disable", "Defense", 0);
        }
        public override void Pulse()
        {
            if (!vesperwatch.IsRunning && API.LastSpellCastInGame == VesperTotem)
            {
                vesperwatch.Restart();
                API.WriteLog("Starting Vespermwatch.");
            }
            if (vesperwatch.IsRunning && vesperwatch.ElapsedMilliseconds > 30000)
            {
                vesperwatch.Reset();
                API.WriteLog("Resetting Vespermwatch.");
            }
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
                if (API.CanCast(RacialSpell1) && isInterrupt && PlayerRaceSettings == "Tauren" && isRacial && isMelee && API.SpellISOnCooldown(WindShear))
                {
                    API.CastSpell(RacialSpell1);
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
        {                //actions +=/ blood_fury,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && isRacial && IsCooldowns && isMelee && (!TalentAscendance || API.PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions +=/ berserking,if= !talent.ascendance.enabled | buff.ascendance.up
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && isRacial && IsCooldowns && isMelee && (!TalentAscendance || API.PlayerHasBuff(Ascendance)))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions +=/ fireblood,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && isRacial && IsCooldowns && isMelee && (!TalentAscendance || API.PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions +=/ ancestral_call,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && isRacial && IsCooldowns && isMelee && (!TalentAscendance || API.PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions +=/ bag_of_tricks,if= !talent.ascendance.enabled | !buff.ascendance.up
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Vulpera" && isRacial && IsCooldowns && isMelee && (!TalentAscendance || API.PlayerHasBuff(Ascendance)))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
            {
                API.CastSpell("Trinket1");
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
            {
                API.CastSpell("Trinket2");
            }
            if (API.CanCast(WindfuryTotem) && PlayerLevel >= 49 && WindfuryToggle && !DoomWindLeggy && API.PlayerMana >= 12 && !API.PlayerHasBuff(WindfuryTotem) && isMelee && !API.PlayerIsMoving)
            {
                API.CastSpell(WindfuryTotem);
                return;
            }
            if (API.CanCast(WindfuryTotem) && PlayerLevel >= 49 && WindfuryToggle && DoomWindLeggy && !API.PlayerHasDebuff(DoomWinds) && API.PlayerMana >= 12 && isMelee && !API.PlayerIsMoving)
            {
                API.CastSpell(WindfuryTotem);
                return;
            }
            if (API.CanCast(Ascendance) && isMelee && TalentAscendance && IsAscendance)
            {
                API.CastSpell(Ascendance);
                return;
            }
            if (API.CanCast(Windstrike) && PlayerLevel >= 15 && API.PlayerHasBuff(Ascendance) && isMelee)
            {
                API.CastSpell(Windstrike);
                return;
            }
            if (API.CanCast(FeralSpirit) && PlayerLevel >= 34 && isMelee && IsFeralSpirit)
            {
                API.CastSpell(FeralSpirit);
                return;
            }
            // Single Target rota
            if (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE)
            {
                if (API.CanCast(PrimordialWave) && IsCovenant && PlayerCovenantSettings == "Necrolord" && API.PlayerMana >= 3 && !API.PlayerHasBuff(PrimordialWave) && isinrange)
                {
                    API.CastSpell(PrimordialWave);
                    return;
                }
                if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.PlayerMana >= 2 && !API.TargetHasDebuff(FlameShock) && isinrange)
                {
                    API.CastSpell(FlameShock);
                    return;
                }
                if (API.CanCast(VesperTotem) && PlayerCovenantSettings == "Kyrian" && IsCovenant && !API.PlayerIsMoving && API.PlayerMana >= 10 && isMelee && !vesperwatch.IsRunning)
                {
                    API.CastSpell(VesperTotem);
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
                if (API.CanCast(FaeTransfusion) && isMelee && !API.PlayerIsMoving && API.PlayerMana >= 8 && PlayerCovenantSettings == "Night Fae" && IsCovenant)
                {
                    API.CastSpell(FaeTransfusion);
                    return;
                }
                if (API.CanCast(LightningBolt) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && API.PlayerHasBuff(StormKeeper) && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(LightningBolt);
                    return;
                }
                if (API.CanCast(ElementalBlast) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && isinrange && API.PlayerMana >= 3 && TalentElementalBlast)
                {
                    API.CastSpell(ElementalBlast);
                    return;
                }
                if (API.CanCast(ChainHarvest) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && isinrange && API.PlayerMana >= 10 && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                {
                    API.CastSpell(ChainHarvest);
                    return;
                }
                if (API.CanCast(LightningBolt) && API.PlayerBuffStacks(MaelstromWeapon) == 10 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(LightningBolt);
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
                if (API.CanCast(StormKeeper) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && !API.PlayerIsMoving && isinrange && TalentStormkeeper && IsStormKeeper)
                {
                    API.CastSpell(StormKeeper);
                    return;
                }
                if (API.CanCast(LightningBolt) && API.PlayerBuffStacks(MaelstromWeapon) >= 8 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(LightningBolt);
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
                if (API.CanCast(IceStrike) && isMelee && API.PlayerMana >= 4 && TalentIceStrike)
                {
                    API.CastSpell(IceStrike);
                    return;
                }
                if (API.CanCast(Sundering) && API.PlayerMana >= 6 && SunderingToggle && isMelee && TalentSundering && IsSundering)
                {
                    API.CastSpell(Sundering);
                    return;
                }
                if (API.CanCast(FireNova) && isMelee && API.PlayerMana >= 6 && TalentFireNova && API.TargetHasDebuff(FlameShock) && IsFireNova)
                {
                    API.CastSpell(FireNova);
                    return;
                }
                if (API.CanCast(LightningBolt) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(LightningBolt);
                    return;
                }
                if (API.CanCast(EarthElemental) && isMelee && PlayerLevel >= 37 && IsEarthElemental)
                {
                    API.CastSpell(EarthElemental);
                    return;
                }
                if (API.CanCast(WindfuryTotem) && PlayerLevel >= 49 && WindfuryToggle && API.PlayerMana >= 12 && API.PlayerBuffTimeRemaining(WindfuryTotem) < 3000 && isMelee && !API.PlayerIsMoving)
                {
                    API.CastSpell(WindfuryTotem);
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
                if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.PlayerMana >= 2 && !API.TargetHasDebuff(FlameShock) && isinrange && (TalentFireNova || TalentLashingFlames || PlayerCovenantSettings == "Necrolord"))
                {
                    API.CastSpell(FlameShock);
                    return;
                }
                if (API.CanCast(PrimordialWave) && IsCovenant && PlayerCovenantSettings == "Necrolord" && API.PlayerMana >= 3 && !API.PlayerHasBuff(PrimordialWave) && isinrange)
                {
                    API.CastSpell(PrimordialWave);
                    return;
                }
                if (API.CanCast(FireNova) && isMelee && API.PlayerMana >= 6 && TalentFireNova && API.TargetHasDebuff(FlameShock) && IsFireNova)
                {
                    API.CastSpell(FireNova);
                    return;
                }
                if (API.CanCast(VesperTotem) && PlayerCovenantSettings == "Kyrian" && IsCovenant && !API.PlayerIsMoving && API.PlayerMana >= 10 && isMelee && !vesperwatch.IsRunning)
                {
                    API.CastSpell(VesperTotem);
                    return;
                }
                if (API.CanCast(LightningBolt) && API.PlayerHasBuff(PrimordialWave) && (API.PlayerBuffStacks(MaelstromWeapon) >= 5 || API.PlayerHasBuff(StormKeeper)) && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(LightningBolt);
                    return;
                }
                if (API.CanCast(LavaLash) && PlayerLevel >= 11 && API.PlayerMana >= 4 && isMelee && TalentLashingFlames && API.TargetDebuffRemainingTime(LashingFlames) < 150)
                {
                    API.CastSpell(LavaLash);
                    return;
                }
                if (API.CanCast(CrashLightning) && PlayerLevel >= 38 && API.PlayerMana >= 6 && isMelee)
                {
                    API.CastSpell(CrashLightning);
                    return;
                }
                if (API.CanCast(ChainHarvest) && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && isinrange && API.PlayerMana >= 10 && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                {
                    API.CastSpell(ChainHarvest);
                    return;
                }
                if (API.CanCast(ElementalBlast) && API.PlayerUnitInMeleeRangeCount < 3 && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && isinrange && API.PlayerMana >= 3 && TalentElementalBlast)
                {
                    API.CastSpell(ElementalBlast);
                    return;
                }
                if (API.CanCast(StormKeeper) && !API.PlayerIsMoving && IsStormKeeper && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && isinrange && TalentStormkeeper)
                {
                    API.CastSpell(StormKeeper);
                    return;
                }
                if (API.CanCast(ChainLightning) && PlayerLevel >= 24 && API.PlayerBuffStacks(MaelstromWeapon) == 10 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(ChainLightning);
                    return;
                }
                if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.PlayerMana >= 2 && API.TargetDebuffRemainingTime(FlameShock) < 100 && TalentFireNova && isinrange)
                {
                    API.CastSpell(FlameShock);
                    return;
                }
                if (API.CanCast(Sundering) && API.PlayerMana >= 6 && SunderingToggle && isMelee && TalentSundering && IsSundering)
                {
                    API.CastSpell(Sundering);
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
                if (API.CanCast(FaeTransfusion) && isMelee && !API.PlayerIsMoving && API.PlayerMana >= 8 && PlayerCovenantSettings == "Night Fae" && IsCovenant)
                {
                    API.CastSpell(FaeTransfusion);
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
                if (API.CanCast(ChainLightning) && PlayerLevel >= 24 && API.PlayerBuffStacks(MaelstromWeapon) >= 5 && API.PlayerMana >= 1 && isinrange)
                {
                    API.CastSpell(ChainLightning);
                    return;
                }
                if (API.CanCast(EarthenSpike) && isMelee && API.PlayerMana >= 4 && TalentEarthenSpike)
                {
                    API.CastSpell(EarthenSpike);
                    return;
                }
                if (API.CanCast(EarthElemental) && isMelee && PlayerLevel >= 37 && IsEarthElemental)
                {
                    API.CastSpell(EarthElemental);
                    return;
                }
                if (API.CanCast(WindfuryTotem) && PlayerLevel >= 49 && WindfuryToggle && API.PlayerMana >= 12 && API.PlayerBuffTimeRemaining(WindfuryTotem) < 3000 && isMelee && !API.PlayerIsMoving)
                {
                    API.CastSpell(WindfuryTotem);
                    return;
                }
            }
        }
    }
}