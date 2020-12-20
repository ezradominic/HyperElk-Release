// Changelog
// v1.0 First release
// v1.1 covenants + cd managment
// v1.2 vesper totem fix
// v1.3 complete new apl
// v1.4 Racials and Trinkets
// v1.5 mouseover flameshock
// v1.6 overcap maelstrom fix
// v1.7 Master of the Elements workaround
// v1.8 chain lightning with stormkeeper change

using System.Diagnostics;


namespace HyperElk.Core
{
    public class ElementalShaman : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
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
        private string WindGust = "Wind Gust";
        private string EchoesofGreatSundering = "Echoes of Great Sundering";
        private string LavaBeam = "Lava Beam";

        //Talents
        bool TalentEchoingShock => API.PlayerIsTalentSelected(2, 2);
        bool TalentElementalBlast => API.PlayerIsTalentSelected(2, 3);
        bool TalentEarthShield => API.PlayerIsTalentSelected(3, 2);
        bool TalentMasterofTheElements => API.PlayerIsTalentSelected(4, 1);
        bool TalentStormElemental => API.PlayerIsTalentSelected(4, 2);
        bool TalentLiquidManaTotem => API.PlayerIsTalentSelected(4, 3);
        bool TalentPrimalElementalist => API.PlayerIsTalentSelected(6, 2);
        bool TalentIcefury => API.PlayerIsTalentSelected(6, 3);
        bool TalentStormkeeper => API.PlayerIsTalentSelected(7, 2);
        bool TalentAscendance => API.PlayerIsTalentSelected(7, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsInRange => API.TargetRange < 41;
        private bool isMOinRange => API.MouseoverRange < 41;
        private bool IsInKickRange => API.TargetRange < 31;
        bool IsAscendance => (UseAscendance == "with Cooldowns" && IsCooldowns || UseAscendance == "always");
        bool IsStormElemental => (UseStormElemental == "with Cooldowns" && IsCooldowns || UseStormElemental == "always");
        bool IsEarthElemental => (UseEarthElemental == "with Cooldowns" && IsCooldowns || UseEarthElemental == "always");
        bool IsFireElemental => (UseFireElemental == "with Cooldowns" && IsCooldowns || UseFireElemental == "always");
        bool IsStormkeeper => (UseStormkeeper == "with Cooldowns" && IsCooldowns || UseStormkeeper == "always" || UseStormkeeper == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE);

        bool IsCovenant => (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE);
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsInRange;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsInRange;

        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseAscendance => CDUsage[CombatRoutine.GetPropertyInt(Ascendance)];
        private string UseStormElemental => CDUsage[CombatRoutine.GetPropertyInt(StormElemental)];
        private string UseEarthElemental => CDUsage[CombatRoutine.GetPropertyInt(EarthElemental)];
        private string UseFireElemental => CDUsage[CombatRoutine.GetPropertyInt(FireElemental)];
        private string UseStormkeeper => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Stormkeeper)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool AutoWolf => CombatRoutine.GetPropertyBool("AutoWolf");
        private bool EchoLeggy => CombatRoutine.GetPropertyBool("EchoLeggy");
        private bool SelfLightningShield => CombatRoutine.GetPropertyBool("LightningShield");
        private bool SelfEarthShield => CombatRoutine.GetPropertyBool("EarthShield");
        private int AstralShiftLifePercent => percentListProp[CombatRoutine.GetPropertyInt(AstralShift)];
        private int HealingStreamTotemLifePercent => percentListProp[CombatRoutine.GetPropertyInt(HealingStreamTotem)];
        private int HealingSurgeLifePercent => percentListProp[CombatRoutine.GetPropertyInt(HealingSurge)];

        private static readonly Stopwatch stormwatch = new Stopwatch();
        private static readonly Stopwatch vesperwatch = new Stopwatch();
        private static readonly Stopwatch Masterwatch = new Stopwatch();
        private bool LastWasNotLavaBurst => API.LastSpellCastInGame != LavaBurst && API.PlayerCurrentCastSpellID != 51505;
        private bool MasterUP => (TalentMasterofTheElements && (!Masterwatch.IsRunning || API.PlayerMaelstrom < 60) || !TalentMasterofTheElements);
        private float gcd => API.SpellGCDTotalDuration;
        public override void Initialize()
        {
            CombatRoutine.Name = "Elemental Shaman by smartie";
            API.WriteLog("Welcome to smartie`s Elemental Shaman v1.8");

            //Spells
            CombatRoutine.AddSpell(ChainLightning, "D7");
            CombatRoutine.AddSpell(LavaBeam, "D7");
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

            //Macros
            CombatRoutine.AddMacro(FlameShock + "MO", "NumPad7");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

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
            CombatRoutine.AddBuff(WindGust);
            CombatRoutine.AddBuff(EchoesofGreatSundering);
            CombatRoutine.AddBuff(EchoingShock);

            //Debuff
            CombatRoutine.AddDebuff(FlameShock);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");


            //Prop
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, " Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Ascendance, "Use " + Ascendance, CDUsage, "Use " + Ascendance + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(StormElemental, "Use " + StormElemental, CDUsage, "Use " + StormElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(EarthElemental, "Use " + EarthElemental, CDUsage, "Use " + EarthElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(FireElemental, "Use " + FireElemental, CDUsage, "Use " + FireElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Stormkeeper, "Use " + Stormkeeper, CDUsageWithAOE, "Use " + Stormkeeper + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("LightningShield", "LightningShield", true, "Put" + LightningShield + "on ourselfs", "Generic");
            CombatRoutine.AddProp("EarthShield", "EarthShield", true, "Put" + EarthShield + "on ourselfs", "Generic");
            CombatRoutine.AddProp("AutoWolf", "AutoWolf", true, "Will auto switch forms out of Fight", "Generic");
            CombatRoutine.AddProp("EchoLeggy", "Echoes of Great Sundering Legendary", false, "Enable if you have the Legendary", "Generic");
            CombatRoutine.AddProp(AstralShift, AstralShift + " Life Percent", percentListProp, "Life percent at which" + AstralShift + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(HealingStreamTotem, HealingStreamTotem + " Life Percent", percentListProp, "Life percent at which" + HealingStreamTotem + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(HealingSurge, HealingSurge + " Life Percent", percentListProp, "Life percent at which" + HealingSurge + "is used, set to 0 to disable", "Defense", 0);
        }
        public override void Pulse()
        {
            API.WriteLog("Maelstrom: " + API.PlayerMaelstrom);
            if (!Masterwatch.IsRunning && TalentMasterofTheElements && (API.PlayerCurrentCastSpellID == 51505 || API.LastSpellCastInGame == LavaBurst))
            {
                Masterwatch.Restart();
                API.WriteLog("Starting Mastermwatch.");
            }
            if (Masterwatch.IsRunning && API.PlayerIsMoving)
            {
                Masterwatch.Reset();
                API.WriteLog("Resetting Masterwatch.");
            }
            if (Masterwatch.IsRunning && Masterwatch.ElapsedMilliseconds > 2500)
            {
                Masterwatch.Reset();
                API.WriteLog("Resetting Masterwatch.");
            }
            if (API.LastSpellCastInGame == StormElemental)
            {
                stormwatch.Restart();
                API.WriteLog("Starting Stormwatch.");
            }
            if (stormwatch.IsRunning && stormwatch.ElapsedMilliseconds > 30000)
            {
                stormwatch.Reset();
                API.WriteLog("Resetting Stormwatch.");
            }
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
            if (API.PlayerCurrentCastTimeRemaining > 40)
                return;
            if (!API.PlayerIsMounted)
            {
                if (isInterrupt && API.CanCast(WindShear) && PlayerLevel >= 12 && IsInKickRange)
                {
                    API.CastSpell(WindShear);
                    return;
                }
                if (API.CanCast(RacialSpell1) && isInterrupt && PlayerRaceSettings == "Tauren" && !API.PlayerIsMoving && isRacial && API.TargetRange < 8 && API.SpellISOnCooldown(WindShear))
                {
                    API.CastSpell(RacialSpell1);
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
            if (API.PlayerCurrentCastTimeRemaining > 40)
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
                //actions +=/ blood_fury,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || API.PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ berserking,if= !talent.ascendance.enabled | buff.ascendance.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || API.PlayerHasBuff(Ascendance)))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ fireblood,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || API.PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ ancestral_call,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || API.PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ bag_of_tricks,if= !talent.ascendance.enabled | !buff.ascendance.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Vulpera" && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || API.PlayerHasBuff(Ascendance)))
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
                //actions+=/fire_elemental
                if (API.CanCast(FireElemental) && PlayerLevel >= 34 && !TalentStormElemental && IsFireElemental)
                {
                    API.CastSpell(FireElemental);
                    return;
                }
                //actions+=/storm_elemental
                if (API.CanCast(StormElemental) && TalentStormElemental && IsStormElemental)
                {
                    API.CastSpell(StormElemental);
                    return;
                }
                //actions+=/primordial_wave,target_if=min:dot.flame_shock.remains,cycle_targets=1,if=!buff.primordial_wave.up
                if (API.CanCast(PrimordialWave) && IsCovenant && PlayerCovenantSettings == "Necrolord" && API.PlayerMana >= 3 && API.TargetDebuffRemainingTime(FlameShock) < gcd*2 && !API.PlayerHasBuff(PrimordialWave) && IsInRange)
                {
                    API.CastSpell(PrimordialWave);
                    return;
                }
                //actions+=/vesper_totem,if=covenant.kyrian
                if (API.CanCast(VesperTotem) && PlayerCovenantSettings == "Kyrian" && IsCovenant && API.PlayerMana >= 10 && IsInRange && !vesperwatch.IsRunning)
                {
                    API.CastSpell(VesperTotem);
                    return;
                }
                //actions+=/fae_transfusion,if=covenant.night_fae
                if (API.CanCast(FaeTransfusion) && IsInRange && !API.PlayerIsMoving && API.PlayerMana >= 8 && PlayerCovenantSettings == "Night Fae" && IsCovenant)
                {
                    API.CastSpell(FaeTransfusion);
                    return;
                }
                // Single Target rota
                if (API.TargetUnitInRangeCount < AOEUnitNumber || !IsAOE)
                {
                    if (TalentStormElemental)
                    {
                        //actions.se_single_target=flame_shock,target_if=(remains<=gcd)&(buff.lava_surge.up|!buff.bloodlust.up)
                        if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.TargetDebuffRemainingTime(FlameShock) < gcd*2)
                        {
                            API.CastSpell(FlameShock);
                            return;
                        }
                        //actions.single_target +=/ ascendance,if= talent.ascendance.enabled & (time >= 60 | buff.bloodlust.up) & (cooldown.lava_burst.remains > 0) & (!talent.icefury.enabled | !buff.icefury.up & !cooldown.icefury.up)
                        if (API.CanCast(Ascendance) && TalentAscendance && IsAscendance && (API.SpellCDDuration(LavaBurst) > 0) && (!TalentIcefury || TalentIcefury && !API.PlayerHasBuff(Icefury) && !API.CanCast(Icefury)))
                        {
                            API.CastSpell(Ascendance);
                            return;
                        }
                        //actions.se_single_target +=/ elemental_blast,if= talent.elemental_blast.enabled
                        if (API.CanCast(ElementalBlast) && API.PlayerMaelstrom < 70 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentElementalBlast)
                        {
                            API.CastSpell(ElementalBlast);
                            return;
                        }
                        //actions.se_single_target +=/ stormkeeper,if= talent.stormkeeper.enabled & (maelstrom < 44)
                        if (API.CanCast(Stormkeeper) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentStormkeeper && IsStormkeeper && API.PlayerMaelstrom < 44)
                        {
                            API.CastSpell(Stormkeeper);
                            return;
                        }
                        //actions.se_single_target +=/ echoing_shock,if= talent.echoing_shock.enabled
                        if (API.CanCast(EchoingShock) && TalentEchoingShock)
                        {
                            API.CastSpell(EchoingShock);
                            return;
                        }
                        //actions.se_single_target +=/ lava_burst,if= buff.wind_gust.stack < 18 | buff.lava_surge.up
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && LastWasNotLavaBurst && API.PlayerMaelstrom < 90 && (API.PlayerBuffStacks(WindGust) < 18 || API.PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.se_single_target +=/ lightning_bolt,if= buff.stormkeeper.up
                        if (API.CanCast(LightningBolt) && API.PlayerMaelstrom < 90 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && API.PlayerHasBuff(Stormkeeper))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.se_single_target +=/ earthquake,if= buff.echoes_of_great_sundering.up
                        if (API.CanCast(Earthquake) && PlayerLevel >= 38 && API.PlayerHasBuff(EchoesofGreatSundering))
                        {
                            API.CastSpell(Earthquake);
                            return;
                        }
                        //actions.se_single_target +=/ earth_shock,if= spell_targets.chain_lightning < 2 & maelstrom >= 60 & (buff.wind_gust.stack < 20 | maelstrom > 90)
                        if (API.CanCast(EarthShock) && PlayerLevel >= 10 && API.PlayerMaelstrom >= 60 && (API.PlayerBuffStacks(WindGust) < 20 || API.PlayerMaelstrom > 90))
                        {
                            API.CastSpell(EarthShock);
                            return;
                        }
                        //actions.se_single_target +=/ lava_burst,if= buff.ascendance.up
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && LastWasNotLavaBurst && API.PlayerMaelstrom < 90 && API.PlayerHasBuff(Ascendance))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.se_single_target +=/ lava_burst,if= cooldown_react & !talent.master_of_the_elements.enabled
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && LastWasNotLavaBurst && API.PlayerMaelstrom < 90)
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.se_single_target +=/ icefury,if= talent.icefury.enabled & !(maelstrom > 75 & cooldown.lava_burst.remains <= 0)
                        if (API.CanCast(Icefury) && TalentIcefury && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && !(API.PlayerMaelstrom > 75 && API.SpellCDDuration(LavaBurst) <= 0))
                        {
                            API.CastSpell(Icefury);
                            return;
                        }
                        //actions.se_single_target +=/ frost_shock,if= talent.icefury.enabled & buff.icefury.up
                        if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerHasBuff(Icefury))
                        {
                            API.CastSpell(FrostShock);
                            return;
                        }
                        //actions.se_single_target +=/ chain_harvest
                        if (API.CanCast(ChainHarvest) && IsInRange && API.PlayerMana >= 10 && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                        {
                            API.CastSpell(ChainHarvest);
                            return;
                        }
                        //actions.se_single_target +=/ earth_elemental,if= !talent.primal_elementalist.enabled | talent.primal_elementalist.enabled & (!pet.storm_elemental.active)
                        if (API.CanCast(EarthElemental) && PlayerLevel >= 37 && IsEarthElemental && (!TalentPrimalElementalist || TalentPrimalElementalist && !stormwatch.IsRunning))
                        {
                            API.CastSpell(EarthElemental);
                            return;
                        }
                        //actions.se_single_target +=/ lightning_bolt
                        if (API.CanCast(LightningBolt) && API.PlayerMaelstrom < 90 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.se_single_target +=/ flame_shock,moving = 1,if= movement.distance > 6
                        if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.TargetDebuffRemainingTime(FlameShock) < 600 && API.PlayerIsMoving)
                        {
                            API.CastSpell(FlameShock);
                            return;
                        }
                        //actions.se_single_target +=/ frost_shock,moving = 1
                        if (API.CanCast(FrostShock) && PlayerLevel >= 17 && PlayerLevel >= 38 && API.PlayerIsMoving)
                        {
                            API.CastSpell(FrostShock);
                            return;
                        }
                    }
                    if (!TalentStormElemental)
                    {
                        //actions.single_target = flame_shock,target_if = (!ticking | dot.flame_shock.remains <= gcd | talent.ascendance.enabled & dot.flame_shock.remains < (cooldown.ascendance.remains + buff.ascendance.duration) & cooldown.ascendance.remains < 4) & (buff.lava_surge.up | !buff.bloodlust.up)
                        if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.TargetDebuffRemainingTime(FlameShock) < gcd*2)
                        {
                            API.CastSpell(FlameShock);
                            return;
                        }
                        //actions.single_target +=/ ascendance,if= talent.ascendance.enabled & (time >= 60 | buff.bloodlust.up) & (cooldown.lava_burst.remains > 0) & (!talent.icefury.enabled | !buff.icefury.up & !cooldown.icefury.up)
                        if (API.CanCast(Ascendance) && TalentAscendance && IsAscendance && (API.SpellCDDuration(LavaBurst) > 0) && (!TalentIcefury || TalentIcefury && !API.PlayerHasBuff(Icefury) && !API.CanCast(Icefury)))
                        {
                            API.CastSpell(Ascendance);
                            return;
                        }
                        //actions.single_target +=/ elemental_blast,if= talent.elemental_blast.enabled & (talent.master_of_the_elements.enabled & (buff.master_of_the_elements.up & maelstrom < 60 | !buff.master_of_the_elements.up) | !talent.master_of_the_elements.enabled)
                        if (API.CanCast(ElementalBlast) && API.PlayerMaelstrom < 70 && MasterUP && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentElementalBlast && (TalentMasterofTheElements && (API.PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom < 60 || !API.PlayerHasBuff(MasteroftheElements)) || !TalentMasterofTheElements))
                        {
                            API.CastSpell(ElementalBlast);
                            return;
                        }
                        //actions.single_target +=/ stormkeeper,if= talent.stormkeeper.enabled & (raid_event.adds.count < 3 | raid_event.adds.in> 50)&(maelstrom < 44)
                        if (API.CanCast(Stormkeeper) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentStormkeeper && IsStormkeeper && API.PlayerMaelstrom < 44)
                        {
                            API.CastSpell(Stormkeeper);
                            return;
                        }
                        //actions.single_target +=/ echoing_shock,if= talent.echoing_shock.enabled & cooldown.lava_burst.remains <= 0
                        if (API.CanCast(EchoingShock) && TalentEchoingShock && API.CanCast(LavaBurst))
                        {
                            API.CastSpell(EchoingShock);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= talent.echoing_shock.enabled & buff.echoing_shock.up
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && LastWasNotLavaBurst && !API.PlayerHasBuff(MasteroftheElements) && API.PlayerHasBuff(EchoingShock) && API.PlayerMaelstrom < 90)
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ liquid_magma_totem,if= talent.liquid_magma_totem.enabled
                        if (API.CanCast(LiquidManaTotem) && TalentLiquidManaTotem)
                        {
                            API.CastSpell(LiquidManaTotem);
                            return;
                        }
                        //actions.single_target +=/ lightning_bolt,if= buff.stormkeeper.up & spell_targets.chain_lightning < 2 & (buff.master_of_the_elements.up)
                        if (API.CanCast(LightningBolt) && API.PlayerHasBuff(Stormkeeper) && API.PlayerMaelstrom < 90 && API.PlayerHasBuff(MasteroftheElements))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.single_target +=/ earthquake,if= buff.echoes_of_great_sundering.up & (!talent.master_of_the_elements.enabled | buff.master_of_the_elements.up)
                        if (API.CanCast(Earthquake) && PlayerLevel >= 38 && API.PlayerHasBuff(EchoesofGreatSundering) && (!TalentMasterofTheElements || API.PlayerHasBuff(MasteroftheElements)))
                        {
                            API.CastSpell(Earthquake);
                            return;
                        }
                        //actions.single_target +=/ earth_shock,if= talent.master_of_the_elements.enabled & (buff.master_of_the_elements.up | cooldown.lava_burst.remains > 0 & maelstrom >= 92 | spell_targets.chain_lightning < 2 & buff.stormkeeper.up & cooldown.lava_burst.remains <= gcd) | !talent.master_of_the_elements.enabled
                        if (API.CanCast(EarthShock) && PlayerLevel >= 10 && (TalentMasterofTheElements && (API.PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom >= 60 || API.PlayerMaelstrom >= 92 || API.PlayerHasBuff(Stormkeeper) && API.SpellCDDuration(LavaBurst) >= gcd) && API.PlayerMaelstrom >= 60 || !TalentMasterofTheElements && API.PlayerMaelstrom >= 60))
                        {
                            API.CastSpell(EarthShock);
                            return;
                        }
                        //actions.single_target+=/lightning_bolt,if=(buff.stormkeeper.remains<1.1*gcd*buff.stormkeeper.stack|buff.stormkeeper.up&buff.master_of_the_elements.up)
                        if (API.CanCast(LightningBolt) && API.PlayerHasBuff(Stormkeeper) && API.PlayerMaelstrom < 90 && API.PlayerBuffTimeRemaining(Stormkeeper) < 110*gcd*API.PlayerBuffStacks(Stormkeeper))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.single_target+=/frost_shock,if=talent.icefury.enabled&talent.master_of_the_elements.enabled&buff.icefury.up&buff.master_of_the_elements.up
                        if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerHasBuff(Icefury) && API.PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom < 60)
                        {
                            API.CastSpell(FrostShock);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= buff.ascendance.up
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && API.PlayerMaelstrom < 90 && API.PlayerHasBuff(Ascendance) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace) || API.PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= cooldown_react & !talent.master_of_the_elements.enabled
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && LastWasNotLavaBurst && !TalentMasterofTheElements && API.PlayerMaelstrom < 90 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace) || API.PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ icefury,if= talent.icefury.enabled & !(maelstrom > 75 & cooldown.lava_burst.remains <= 0)
                        if (API.CanCast(Icefury) && TalentIcefury && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && !(API.PlayerMaelstrom > 75 && API.CanCast(LavaBurst)))
                        {
                            API.CastSpell(Icefury);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= cooldown_react & charges > talent.echo_of_the_elements.enabled
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !API.PlayerHasBuff(MasteroftheElements) && LastWasNotLavaBurst && API.SpellCharges(LavaBurst) > 1 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace) || API.PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ frost_shock,if= talent.icefury.enabled & buff.icefury.up & buff.icefury.remains < 1.1 * gcd * buff.icefury.stack
                        if (API.CanCast(FrostShock) && PlayerLevel >= 17 && API.PlayerHasBuff(Icefury) && MasterUP)
                        {
                            API.CastSpell(FrostShock);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= cooldown_react
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !API.PlayerHasBuff(MasteroftheElements) && LastWasNotLavaBurst && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace) || API.PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ chain_harvest
                        if (API.CanCast(ChainHarvest) && IsInRange && API.PlayerMana >= 10 && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                        {
                            API.CastSpell(ChainHarvest);
                            return;
                        }
                        //actions.single_target +=/ earth_elemental,if= !talent.primal_elementalist.enabled | !pet.fire_elemental.active
                        if (API.CanCast(EarthElemental) && PlayerLevel >= 37 && IsEarthElemental && (!TalentPrimalElementalist || TalentPrimalElementalist && !stormwatch.IsRunning))
                        {
                            API.CastSpell(EarthElemental);
                            return;
                        }
                        //actions.single_target +=/ lightning_bolt
                        if (API.CanCast(LightningBolt) && API.PlayerMaelstrom < 90 && MasterUP && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.single_target +=/ flame_shock,moving = 1,target_if = refreshable
                        if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.TargetDebuffRemainingTime(FlameShock) < 600 && API.PlayerIsMoving)
                        {
                            API.CastSpell(FlameShock);
                            return;
                        }
                        // actions.single_target +=/ frost_shock,moving = 1
                        if (API.CanCast(FrostShock) && PlayerLevel >= 17 && PlayerLevel >= 38 && API.PlayerIsMoving && MasterUP)
                        {
                            API.CastSpell(FrostShock);
                            return;
                        }
                    }
                }
                //AoE rota
                if (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE)
                {
                    //actions.aoe = earthquake,if= buff.echoing_shock.up
                    if (API.CanCast(Earthquake) && PlayerLevel >= 38 && API.PlayerMaelstrom >= 60 && API.PlayerHasBuff(EchoingShock))
                    {
                        API.CastSpell(Earthquake);
                        return;
                    }
                    //actions.aoe +=/ chain_harvest
                    if (API.CanCast(ChainHarvest) && IsInRange && API.PlayerMana >= 10 && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                    {
                        API.CastSpell(ChainHarvest);
                        return;
                    }
                    //actions.aoe +=/ stormkeeper,if= talent.stormkeeper.enabled
                    if (API.CanCast(Stormkeeper) && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentStormkeeper && IsStormkeeper)
                    {
                        API.CastSpell(Stormkeeper);
                        return;
                    }
                    //actions.aoe +=/ flame_shock,if= !active_dot.flame_shock
                    if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.TargetDebuffRemainingTime(FlameShock) < gcd*2)
                    {
                        API.CastSpell(FlameShock);
                        return;
                    }
                    if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                    {
                        if (API.MouseoverDebuffRemainingTime(FlameShock) < gcd*2 && API.CanCast(FlameShock) && isMOinRange)
                        {
                            API.CastSpell(FlameShock + "MO");
                            return;
                        }
                    }
                    //actions.aoe +=/ echoing_shock,if= talent.echoing_shock.enabled & maelstrom >= 60
                    if (API.CanCast(EchoingShock) && API.PlayerMaelstrom >= 60 && TalentEchoingShock)
                    {
                        API.CastSpell(EchoingShock);
                        return;
                    }
                    //actions.aoe +=/ ascendance,if= talent.ascendance.enabled & (!pet.storm_elemental.active) & (!talent.icefury.enabled | !buff.icefury.up & !cooldown.icefury.up)
                    if (API.CanCast(Ascendance) && TalentAscendance && IsAscendance && !stormwatch.IsRunning && (!TalentIcefury || TalentIcefury && !API.PlayerHasBuff(Icefury) && !API.CanCast(Icefury)))
                    {
                        API.CastSpell(Ascendance);
                        return;
                    }
                    //actions.aoe +=/ liquid_magma_totem,if= talent.liquid_magma_totem.enabled
                    if (API.CanCast(LiquidManaTotem) && TalentLiquidManaTotem)
                    {
                        API.CastSpell(LiquidManaTotem);
                        return;
                    }
                    //actions.aoe +=/ earth_shock,if= runeforge.echoes_of_great_sundering.equipped & !buff.echoes_of_great_sundering.up
                    if (API.CanCast(EarthShock) && PlayerLevel >= 10 && API.PlayerMaelstrom >= 60 && EchoLeggy && !API.PlayerHasBuff(EchoesofGreatSundering))
                    {
                        API.CastSpell(EarthShock);
                        return;
                    }
                    //actions.aoe+=/earth_elemental,if=runeforge.deeptremor_stone.equipped&(!talent.primal_elementalist.enabled|(!pet.storm_elemental.active&!pet.fire_elemental.active))
                    if (API.CanCast(EarthElemental) && PlayerLevel >= 37 && IsEarthElemental && (!TalentPrimalElementalist || TalentPrimalElementalist && !API.PlayerHasPet))
                    {
                        API.CastSpell(EarthElemental);
                        return;
                    }
                    //actions.aoe +=/ lava_burst,target_if = dot.flame_shock.remains,if= spell_targets.chain_lightning < 4 | buff.lava_surge.up | (talent.master_of_the_elements.enabled & !buff.master_of_the_elements.up & maelstrom >= 60)
                    if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && LastWasNotLavaBurst && (API.PlayerBuffTimeRemaining(Stormkeeper) > 300 * gcd * API.PlayerBuffStacks(Stormkeeper) || !API.PlayerHasBuff(Stormkeeper)) && !API.PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom < 90 && API.TargetDebuffRemainingTime(FlameShock) > gcd*2 && (API.TargetUnitInRangeCount < 4 || API.PlayerHasBuff(LavaSurge) || (TalentMasterofTheElements && !API.PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom >= 60)))
                    {
                        API.CastSpell(LavaBurst);
                        return;
                    }
                    //actions.aoe +=/ earthquake,if= !talent.master_of_the_elements.enabled | buff.stormkeeper.up | maelstrom >= (100 - 4 * spell_targets.chain_lightning) | buff.master_of_the_elements.up | spell_targets.chain_lightning > 3
                    if (API.CanCast(Earthquake) && PlayerLevel >= 38 && API.PlayerMaelstrom >= 60 && (!TalentMasterofTheElements || API.PlayerHasBuff(Stormkeeper) || API.PlayerMaelstrom >= 90 || TalentMasterofTheElements && API.PlayerHasBuff(MasteroftheElements) || API.TargetUnitInRangeCount > 3))
                    {
                        API.CastSpell(Earthquake);
                        return;
                    }
                    //actions.aoe +=/ chain_lightning,if= buff.stormkeeper.remains < 3 * gcd * buff.stormkeeper.stack
                    if (API.CanCast(ChainLightning) && PlayerLevel >= 24 && API.PlayerBuffTimeRemaining(Stormkeeper) < 300 * gcd * API.PlayerBuffStacks(Stormkeeper) && API.PlayerMaelstrom < 90)
                    {
                        API.CastSpell(ChainLightning);
                        return;
                    }
                    //actions.aoe +=/ lava_burst,if= buff.lava_surge.up & spell_targets.chain_lightning < 4 & (!pet.storm_elemental.active) & dot.flame_shock.ticking
                    if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && LastWasNotLavaBurst && !API.PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom < 90 && API.TargetDebuffRemainingTime(FlameShock) > gcd * 2 &&  API.PlayerHasBuff(LavaSurge) && API.TargetUnitInRangeCount < 4 && !stormwatch.IsRunning)
                    {
                        API.CastSpell(LavaBurst);
                        return;
                    }
                    //actions.aoe +=/ elemental_blast,if= talent.elemental_blast.enabled & spell_targets.chain_lightning < 5 & (!pet.storm_elemental.active)
                    if (API.CanCast(ElementalBlast) && API.PlayerMaelstrom < 70 && MasterUP && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)) && TalentElementalBlast && API.TargetUnitInRangeCount < 5 && !stormwatch.IsRunning)
                    {
                        API.CastSpell(ElementalBlast);
                        return;
                    }
                    //actions.aoe +=/ lava_beam,if= talent.ascendance.enabled
                    if (API.CanCast(LavaBeam) && TalentAscendance && API.PlayerHasBuff(Ascendance) && API.PlayerMaelstrom < 90 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)))
                    {
                        API.CastSpell(LavaBeam);
                        return;
                    }
                    //actions.aoe +=/ chain_lightning
                    if (API.CanCast(ChainLightning) && API.PlayerMaelstrom < 90 && MasterUP && PlayerLevel >= 24 && (!API.PlayerIsMoving || API.PlayerHasBuff(SpiritwalkersGrace)))
                    {
                        API.CastSpell(ChainLightning);
                        return;
                    }
                    //actions.aoe +=/ flame_shock,moving = 1,target_if = refreshable
                    if (API.CanCast(FlameShock) && PlayerLevel >= 3 && API.TargetDebuffRemainingTime(FlameShock) < 600 && API.PlayerIsMoving)
                    {
                        API.CastSpell(FlameShock);
                        return;
                    }
                    //actions.aoe +=/ frost_shock,moving = 1
                    if (API.CanCast(FrostShock) && PlayerLevel >= 17 && PlayerLevel >= 38 && API.PlayerIsMoving && MasterUP)
                    {
                        API.CastSpell(FrostShock);
                        return;
                    }
                }
            }
        }
    }
}

