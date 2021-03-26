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
// v1.9 spell ids and alot of other stuff
// v2.0 Racials and few other small fixes
// v2.1 Echoes of Great Sundering fixed
// v2.2 Quaking helper added
// v2.3 Quaking helper fixed
// v2.4 auto enchant weapon and alot of other small changes
// v2.5 some love for the ele
// v2.6 new simc apl and new settings options
// v2.65 small hotfix for mobcount
// v2.7 fix typo
// v2.8 stopcasting fix

using System.Diagnostics;


namespace HyperElk.Core
{
    public class ElementalShaman : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsFocus => API.ToggleIsEnabled("Focus ES");
        //Spell,Auras
        private string ChainLightning = "Chain Lightning";
        private string Earthquake = "Earthquake";
        private string Icefury = "Icefury";
        private string LavaBurst = "Lava Burst";
        private string FrostShock = "Frost Shock";
        private string Stormkeeper = "Stormkeeper";
        private string LiquidMagmaTotem = "Liquid Magma Totem";
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
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string Quake = "Quake";
        private string Stopcast = "Stopcast Macro";
        private string FlametongueWeapon = "Flametongue Weapon";

        //Talents
        bool TalentEchoingShock => API.PlayerIsTalentSelected(2, 2);
        bool TalentElementalBlast => API.PlayerIsTalentSelected(2, 3);
        bool TalentEarthShield => API.PlayerIsTalentSelected(3, 2);
        bool TalentMasterofTheElements => API.PlayerIsTalentSelected(4, 1);
        bool TalentStormElemental => API.PlayerIsTalentSelected(4, 2);
        bool TalentLiquidMagmaTotem => API.PlayerIsTalentSelected(4, 3);
        bool TalentPrimalElementalist => API.PlayerIsTalentSelected(6, 2);
        bool TalentIcefury => API.PlayerIsTalentSelected(6, 3);
        bool TalentStormkeeper => API.PlayerIsTalentSelected(7, 2);
        bool TalentAscendance => API.PlayerIsTalentSelected(7, 3);

        //General
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool PlayerHasDebuff(string buff)
        {
            return API.PlayerHasDebuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        private int PlayerLevel => API.PlayerLevel;
        private bool IsInRange => API.TargetRange < 41;
        private bool isMOinRange => API.MouseoverRange < 41;
        private bool IsInKickRange => API.TargetRange < 31;
        bool IsAscendance => (UseAscendance == "with Cooldowns" || UseAscendance == "with Cooldowns or AoE" || UseAscendance == "on mobcount or Cooldowns") && IsCooldowns || UseAscendance == "always" || (UseAscendance == "on AOE" || UseAscendance == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseAscendance == "on mobcount or Cooldowns" || UseAscendance == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsEarthElemental => (UseEarthElemental == "with Cooldowns" || UseEarthElemental == "with Cooldowns or AoE" || UseEarthElemental == "on mobcount or Cooldowns") && IsCooldowns || UseEarthElemental == "always" || (UseEarthElemental == "on AOE" || UseEarthElemental == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseEarthElemental == "on mobcount or Cooldowns" || UseEarthElemental == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsStormElemental => (UseStormElemental == "with Cooldowns" || UseStormElemental == "with Cooldowns or AoE" || UseStormElemental == "on mobcount or Cooldowns") && IsCooldowns || UseStormElemental == "always" || (UseStormElemental == "on AOE" || UseStormElemental == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseStormElemental == "on mobcount or Cooldowns" || UseStormElemental == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsFireElemental => (UseFireElemental == "with Cooldowns" || UseFireElemental == "with Cooldowns or AoE" || UseFireElemental == "on mobcount or Cooldowns") && IsCooldowns || UseFireElemental == "always" || (UseFireElemental == "on AOE" || UseFireElemental == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseFireElemental == "on mobcount or Cooldowns" || UseFireElemental == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsStormkeeper => (UseStormkeeper == "with Cooldowns" || UseStormkeeper == "with Cooldowns or AoE" || UseStormkeeper == "on mobcount or Cooldowns") && IsCooldowns || UseStormkeeper == "always" || (UseStormkeeper == "on AOE" || UseStormkeeper == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseStormkeeper == "on mobcount or Cooldowns" || UseStormkeeper == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;

        bool IsCovenant => (UseCovenant == "with Cooldowns" || UseCovenant == "with Cooldowns or AoE" || UseCovenant == "on mobcount or Cooldowns") && IsCooldowns || UseCovenant == "always" || (UseCovenant == "on AOE" || UseCovenant == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseCovenant == "on mobcount or Cooldowns" || UseCovenant == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsTrinkets1 => ((UseTrinket1 == "with Cooldowns" || UseTrinket1 == "with Cooldowns or AoE" || UseTrinket1 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket1 == "always" || (UseTrinket1 == "on AOE" || UseTrinket1 == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseTrinket1 == "on mobcount or Cooldowns" || UseTrinket1 == "on mobcount") && API.TargetUnitInRangeCount >= MobCount) && IsInRange;
        bool IsTrinkets2 => ((UseTrinket2 == "with Cooldowns" || UseTrinket2 == "with Cooldowns or AoE" || UseTrinket2 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket2 == "always" || (UseTrinket2 == "on AOE" || UseTrinket2 == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseTrinket2 == "on mobcount or Cooldowns" || UseTrinket2 == "on mobcount") && API.TargetUnitInRangeCount >= MobCount) && IsInRange;

        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "with Cooldowns or AoE", "on mobcount", "on mobcount or Cooldowns", "always" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        private int MobCount => numbRaidList[CombatRoutine.GetPropertyInt("MobCount")];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseAscendance => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Ascendance)];
        private string UseStormElemental => CDUsageWithAOE[CombatRoutine.GetPropertyInt(StormElemental)];
        private string UseEarthElemental => CDUsageWithAOE[CombatRoutine.GetPropertyInt(EarthElemental)];
        private string UseFireElemental => CDUsageWithAOE[CombatRoutine.GetPropertyInt(FireElemental)];
        private string UseStormkeeper => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Stormkeeper)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool AutoWolf => CombatRoutine.GetPropertyBool("AutoWolf");
        private bool QuakingHelper => CombatRoutine.GetPropertyBool("QuakingHelper");
        private bool EchoLeggy => CombatRoutine.GetPropertyBool("EchoLeggy");
        private bool SelfLightningShield => CombatRoutine.GetPropertyBool("LightningShield");
        private bool SelfEarthShield => CombatRoutine.GetPropertyBool("EarthShield");
        private int AstralShiftLifePercent => numbList[CombatRoutine.GetPropertyInt(AstralShift)];
        private int HealingStreamTotemLifePercent => numbList[CombatRoutine.GetPropertyInt(HealingStreamTotem)];
        private int HealingSurgeLifePercent => numbList[CombatRoutine.GetPropertyInt(HealingSurge)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private bool WeaponEnchant => CombatRoutine.GetPropertyBool("WeaponEnchant");

        private static readonly Stopwatch stormwatch = new Stopwatch();
        private static readonly Stopwatch vesperwatch = new Stopwatch();
        private static readonly Stopwatch Masterwatch = new Stopwatch();
        private bool LastCastlavaBurst => (API.LastSpellCastInGame == LavaBurst || API.PlayerCurrentCastSpellID == 51505);
        private bool Quaking => ((API.PlayerCurrentCastTimeRemaining >= 200 || API.PlayerIsChanneling) && API.PlayerDebuffRemainingTime(Quake) < 200) && PlayerHasDebuff(Quake);
        private bool SaveQuake => (PlayerHasDebuff(Quake) && API.PlayerDebuffRemainingTime(Quake) > 200 && QuakingHelper || !PlayerHasDebuff(Quake) || !QuakingHelper);
        private bool MasterUP => (TalentMasterofTheElements && (!Masterwatch.IsRunning || API.PlayerMaelstrom < 60) || !TalentMasterofTheElements);
        private float gcd => API.SpellGCDTotalDuration;
        public override void Initialize()
        {
            CombatRoutine.Name = "Elemental Shaman by smartie";
            API.WriteLog("Welcome to smartie`s Elemental Shaman v2.8");
            API.WriteLog("For this rota you need to following macros");
            API.WriteLog("For stopcasting (which is important): /stopcasting");
            API.WriteLog("For Earthquake (optional but recommended): /cast [@cursor] Earthquake");
            API.WriteLog("For Earthshield on Focus: /cast [@focus,help] Earth shield");
            API.WriteLog("For Mouseover Flame Shock: /cast [@mouseover] Flame Shock");

            //Spells
            CombatRoutine.AddSpell(ChainLightning, 188443, "D7");
            CombatRoutine.AddSpell(LavaBeam, 114074, "D7");
            CombatRoutine.AddSpell(Earthquake, 61882, "D5");
            CombatRoutine.AddSpell(Icefury, 210714, "D9");
            CombatRoutine.AddSpell(LavaBurst, 51505, "D3");
            CombatRoutine.AddSpell(FrostShock, 196840, "D6");
            CombatRoutine.AddSpell(Stormkeeper, 191634, "NumPad2");
            CombatRoutine.AddSpell(LiquidMagmaTotem, 192222, "D1");
            CombatRoutine.AddSpell(ElementalBlast, 117014, "D8");
            CombatRoutine.AddSpell(EarthShock, 8042, "D4");
            CombatRoutine.AddSpell(EarthElemental, 198103, "D1");
            CombatRoutine.AddSpell(StormElemental, 192249, "D0");
            CombatRoutine.AddSpell(FireElemental, 198067, "D0");
            CombatRoutine.AddSpell(FlameShock, 188389, "D2");
            CombatRoutine.AddSpell(Ascendance, 114050, "D1");
            CombatRoutine.AddSpell(GhostWolf, 2645, "NumPad1");
            CombatRoutine.AddSpell(HealingSurge, 8004, "F1");
            CombatRoutine.AddSpell(LightningBolt, 188196, "D1");
            CombatRoutine.AddSpell(WindShear, 57994, "F12");
            CombatRoutine.AddSpell(AstralShift, 108271, "F4");
            CombatRoutine.AddSpell(EchoingShock, 320125, "NumPad3");
            CombatRoutine.AddSpell(EarthShield, 974, "F7");
            CombatRoutine.AddSpell(HealingStreamTotem, 5394, "NumPad6");
            CombatRoutine.AddSpell(LightningShield, 192106, "F2");
            CombatRoutine.AddSpell(PrimordialWave, 326059, "D1");
            CombatRoutine.AddSpell(VesperTotem, 324386, "D1");
            CombatRoutine.AddSpell(FaeTransfusion, 328923, "D1");
            CombatRoutine.AddSpell(ChainHarvest, 320674, "D1");
            CombatRoutine.AddSpell(FlametongueWeapon, 318038);

            //Macros
            CombatRoutine.AddMacro(FlameShock + "MO", "NumPad7");
            CombatRoutine.AddMacro(EarthShield + "Focus", "NumPad7");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            CombatRoutine.AddMacro(Stopcast, "F10");

            //Buffs
            CombatRoutine.AddBuff(LavaSurge, 77762);
            CombatRoutine.AddBuff(MasteroftheElements, 260734);
            CombatRoutine.AddBuff(Stormkeeper, 191634);
            CombatRoutine.AddBuff(Icefury, 210714);
            CombatRoutine.AddBuff(SurgeofPower, 285514);
            CombatRoutine.AddBuff(Ascendance, 114050);
            CombatRoutine.AddBuff(GhostWolf, 2645);
            CombatRoutine.AddBuff(EarthShield, 974);
            CombatRoutine.AddBuff(LightningShield, 192106);
            CombatRoutine.AddBuff(SpiritwalkersGrace, 79206);
            CombatRoutine.AddBuff(PrimordialWave, 326059);
            CombatRoutine.AddBuff(WindGust, 263806);
            CombatRoutine.AddBuff(EchoesofGreatSundering, 336217);
            CombatRoutine.AddBuff(EchoingShock, 320125);
            CombatRoutine.AddBuff(VesperTotem, 324386);

            //Debuff
            CombatRoutine.AddDebuff(FlameShock, 188389);
            CombatRoutine.AddDebuff(Quake, 240447);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Focus ES");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);


            //Prop
            CombatRoutine.AddProp("MobCount", "Mobcount to use Cooldowns ", numbRaidList, " Mobcount to use Cooldowns", "Cooldowns", 3);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, " Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Ascendance, "Use " + Ascendance, CDUsageWithAOE, "Use " + Ascendance + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(StormElemental, "Use " + StormElemental, CDUsageWithAOE, "Use " + StormElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(EarthElemental, "Use " + EarthElemental, CDUsageWithAOE, "Use " + EarthElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(FireElemental, "Use " + FireElemental, CDUsageWithAOE, "Use " + FireElemental + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Stormkeeper, "Use " + Stormkeeper, CDUsageWithAOE, "Use " + Stormkeeper + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("LightningShield", "LightningShield", true, "Put" + LightningShield + "on ourselfs", "Generic");
            CombatRoutine.AddProp("WeaponEnchant", "WeaponEnchant", true, "Will auto enchant your Weapons", "Generic");
            CombatRoutine.AddProp("EarthShield", "EarthShield", true, "Put" + EarthShield + "on ourselfs", "Generic");
            CombatRoutine.AddProp("AutoWolf", "AutoWolf", true, "Will auto switch forms out of Fight", "Generic");
            CombatRoutine.AddProp("QuakingHelper", "Quaking Helper", false, "Will cancle casts on Quaking", "Generic");
            CombatRoutine.AddProp("EchoLeggy", "Echoes of Great StormElemental Legendary", false, "Enable if you have the Legendary", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(AstralShift, AstralShift + " Life Percent", numbList, "Life percent at which" + AstralShift + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(HealingStreamTotem, HealingStreamTotem + " Life Percent", numbList, "Life percent at which" + HealingStreamTotem + "is used, set to 0 to disable", "Defense", 20);
            CombatRoutine.AddProp(HealingSurge, HealingSurge + " Life Percent", numbList, "Life percent at which" + HealingSurge + "is used, set to 0 to disable", "Defense", 0);
        }
        public override void Pulse()
        {
            //API.WriteLog("Lava cd: " + API.SpellCDDuration(LavaBurst));
            //API.WriteLog("GCD" + gcd);
            if (!Masterwatch.IsRunning && TalentMasterofTheElements && (API.PlayerCurrentCastSpellID == 51505 || API.LastSpellCastInGame == LavaBurst))
            {
                Masterwatch.Restart();
                //API.WriteLog("Starting Mastermwatch.");
            }
            if (Masterwatch.IsRunning && API.PlayerIsMoving)
            {
                Masterwatch.Reset();
                //API.WriteLog("Resetting Masterwatch.");
            }
            if (Masterwatch.IsRunning && Masterwatch.ElapsedMilliseconds > 2500)
            {
                Masterwatch.Reset();
                //API.WriteLog("Resetting Masterwatch.");
            }
            if (!stormwatch.IsRunning && API.LastSpellCastInGame == StormElemental)
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
            if (API.PlayerCurrentCastTimeRemaining > 40 && !API.MacroIsIgnored(Stopcast) && QuakingHelper && Quaking)
            {
                API.CastSpell(Stopcast);
                return;
            }
            if ((API.PlayerCurrentCastTimeRemaining > 40 && (API.PlayerCurrentCastSpellID == 188196 || API.PlayerCurrentCastSpellID == 188443)) && !API.MacroIsIgnored(Stopcast) && TalentMasterofTheElements && API.PlayerMaelstrom >= 60 && API.PlayerHasBuff(MasteroftheElements) && !API.PlayerHasBuff(Stormkeeper))
            {
                API.CastSpell(Stopcast);
                return;
            }
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            if (!API.PlayerIsMounted)
            {
                if (isInterrupt && API.CanCast(WindShear) && PlayerLevel >= 12 && IsInKickRange)
                {
                    API.CastSpell(WindShear);
                    return;
                }
                if (PlayerRaceSettings == "Tauren" && API.CanCast(RacialSpell1) && isInterrupt && !API.PlayerIsMoving && isRacial && API.TargetRange < 8 && API.SpellISOnCooldown(WindShear))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.CanCast(AstralShift) && PlayerLevel >= 42 && API.PlayerHealthPercent <= AstralShiftLifePercent)
                {
                    API.CastSpell(AstralShift);
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
                if (API.CanCast(HealingSurge) && SaveQuake && PlayerLevel >= 4 && API.PlayerMana >= 24 && !API.PlayerIsMoving && API.PlayerHealthPercent <= HealingSurgeLifePercent)
                {
                    API.CastSpell(HealingSurge);
                    return;
                }
                if (API.CanCast(HealingStreamTotem) && PlayerLevel >= 28 && API.PlayerMana >= 9 && !API.PlayerIsMoving && API.PlayerHealthPercent <= HealingStreamTotemLifePercent)
                {
                    API.CastSpell(HealingStreamTotem);
                    return;
                }
                if (API.CanCast(EarthShield) && API.PlayerMana >= 10 && SelfEarthShield && !IsFocus && !PlayerHasBuff(LightningShield) && !PlayerHasBuff(EarthShield) && TalentEarthShield)
                {
                    API.CastSpell(EarthShield);
                    return;
                }
                if (API.CanCast(LightningShield) && PlayerLevel >= 9 && API.PlayerMana >= 2 && SelfLightningShield && !PlayerHasBuff(EarthShield) && !PlayerHasBuff(LightningShield) && API.PlayerHealthPercent > 0)
                {
                    API.CastSpell(LightningShield);
                    return;
                }
                //Focus
                if (API.CanCast(EarthShield) && IsFocus && API.FocusRange < 40 && API.FocusHealthPercent != 0 && !API.FocusHasBuff(EarthShield) && API.PlayerMana >= 10 && TalentEarthShield)
                {
                    API.CastSpell(EarthShield + "Focus");
                    return;
                }
                rotation();
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            if (AutoWolf && API.CanCast(GhostWolf) && !PlayerHasBuff(GhostWolf) && !API.PlayerIsMounted && API.PlayerIsMoving)
            {
                API.CastSpell(GhostWolf);
                return;
            }
            if (API.CanCast(EarthShield) && API.PlayerMana >= 10 && SelfEarthShield && !IsFocus && !PlayerHasBuff(LightningShield) && !PlayerHasBuff(EarthShield) && TalentEarthShield)
            {
                API.CastSpell(EarthShield);
                return;
            }
            if (API.CanCast(LightningShield) && PlayerLevel >= 9 && API.PlayerMana >= 2 && SelfLightningShield && !PlayerHasBuff(EarthShield) && !PlayerHasBuff(LightningShield) && API.PlayerHealthPercent > 0)
            {
                API.CastSpell(LightningShield);
                return;
            }
            //Focus
            if (API.CanCast(EarthShield) && IsFocus && API.FocusRange < 40 && API.FocusHealthPercent != 0 && !API.FocusHasBuff(EarthShield) && API.PlayerMana >= 10 && TalentEarthShield)
            {
                API.CastSpell(EarthShield + "Focus");
                return;
            }
            if (API.CanCast(FlametongueWeapon) && WeaponEnchant && API.LastSpellCastInGame != (FlametongueWeapon) && API.PlayerWeaponBuffDuration(true) < 30000)
            {
                API.CastSpell(FlametongueWeapon);
                return;
            }
        }
        private void rotation()
        {
            if (IsInRange)
            {
                if (API.CanCast(FlametongueWeapon) && WeaponEnchant && API.LastSpellCastInGame != (FlametongueWeapon) && API.PlayerWeaponBuffDuration(true) < 3000)
                {
                    API.CastSpell(FlametongueWeapon);
                    return;
                }
                //actions+=/flame_shock,if=!ticking
                if (API.CanCast(FlameShock) && PlayerLevel >= 3 && !API.TargetHasDebuff(FlameShock))
                {
                    API.CastSpell(FlameShock);
                    return;
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
                //actions +=/ blood_fury,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
                if (PlayerRaceSettings == "Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ berserking,if= !talent.ascendance.enabled | buff.ascendance.up
                if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || PlayerHasBuff(Ascendance)))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ fireblood,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
                if (PlayerRaceSettings == "Dark Iron Dwarf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ ancestral_call,if= !talent.ascendance.enabled | buff.ascendance.up | cooldown.ascendance.remains > 50
                if (PlayerRaceSettings == "Mag'har Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || PlayerHasBuff(Ascendance) || TalentAscendance && API.SpellCDDuration(Ascendance) > 5000))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ bag_of_tricks,if= !talent.ascendance.enabled | !buff.ascendance.up
                if (PlayerRaceSettings == "Vulpera" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsInRange && (!TalentAscendance || PlayerHasBuff(Ascendance)))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
                {
                    API.CastSpell("Trinket1");
                    return;
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
                {
                    API.CastSpell("Trinket2");
                    return;
                }
                //actions+=/primordial_wave,target_if=min:dot.flame_shock.remains,cycle_targets=1,if=!buff.primordial_wave.up
                if (API.CanCast(PrimordialWave) && IsCovenant && PlayerCovenantSettings == "Necrolord" && API.PlayerMana >= 3 && API.TargetDebuffRemainingTime(FlameShock) < gcd*2 && !PlayerHasBuff(PrimordialWave) && IsInRange)
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
                if (API.CanCast(FaeTransfusion) && IsInRange  && SaveQuake && !API.PlayerIsMoving && API.PlayerMana >= 8 && PlayerCovenantSettings == "Night Fae" && IsCovenant)
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
                        if (API.CanCast(Ascendance) && TalentAscendance && IsAscendance && (API.SpellCDDuration(LavaBurst) > 0) && (!TalentIcefury || TalentIcefury && !PlayerHasBuff(Icefury) && !API.CanCast(Icefury)))
                        {
                            API.CastSpell(Ascendance);
                            return;
                        }
                        //actions.se_single_target +=/ elemental_blast,if= talent.elemental_blast.enabled
                        if (API.CanCast(ElementalBlast)  && SaveQuake && API.PlayerMaelstrom < 70 && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && TalentElementalBlast)
                        {
                            API.CastSpell(ElementalBlast);
                            return;
                        }
                        //actions.se_single_target +=/ stormkeeper,if= talent.stormkeeper.enabled & (maelstrom < 44)
                        if (API.CanCast(Stormkeeper)  && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && TalentStormkeeper && IsStormkeeper && API.PlayerMaelstrom < 44)
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
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !LastCastlavaBurst && (SaveQuake || PlayerHasBuff(LavaSurge)) && API.PlayerMaelstrom < 90 && (API.PlayerBuffStacks(WindGust) < 18 || PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.se_single_target +=/ lightning_bolt,if= buff.stormkeeper.up
                        if (API.CanCast(LightningBolt) && API.PlayerMaelstrom < 90 && PlayerHasBuff(Stormkeeper))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.se_single_target +=/ earthquake,if= buff.echoes_of_great_StormElemental.up
                        if (API.CanCast(Earthquake) && PlayerLevel >= 38 && PlayerHasBuff(EchoesofGreatSundering))
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
                        //actions.se_single_target+=/lightning_bolt,if=(buff.stormkeeper.remains<1.1*gcd*buff.stormkeeper.stack|buff.stormkeeper.up&buff.master_of_the_elements.up)
                        if (API.CanCast(LightningBolt) && PlayerHasBuff(Stormkeeper) && API.PlayerMaelstrom < 90 && (API.PlayerBuffTimeRemaining(Stormkeeper) < gcd * 2 || PlayerHasBuff(Stormkeeper) && PlayerHasBuff(MasteroftheElements)))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.se_single_target +=/ lava_burst,if= buff.ascendance.up
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !LastCastlavaBurst && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && (SaveQuake || PlayerHasBuff(LavaSurge)) && API.PlayerMaelstrom < 90 && PlayerHasBuff(Ascendance))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.se_single_target +=/ lava_burst,if= cooldown_react & !talent.master_of_the_elements.enabled
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !LastCastlavaBurst && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && (SaveQuake || PlayerHasBuff(LavaSurge)) && API.PlayerMaelstrom < 90)
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.se_single_target +=/ icefury,if= talent.icefury.enabled & !(maelstrom > 75 & cooldown.lava_burst.remains <= 0)
                        if (API.CanCast(Icefury) && TalentIcefury && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && !(API.PlayerMaelstrom > 75 && API.SpellCDDuration(LavaBurst) <= 0))
                        {
                            API.CastSpell(Icefury);
                            return;
                        }
                        //actions.se_single_target +=/ frost_shock,if= talent.icefury.enabled & buff.icefury.up
                        if (API.CanCast(FrostShock) && PlayerLevel >= 17 && PlayerHasBuff(Icefury))
                        {
                            API.CastSpell(FrostShock);
                            return;
                        }
                        //actions.se_single_target +=/ chain_harvest
                        if (API.CanCast(ChainHarvest) && IsInRange && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace))  && SaveQuake && API.PlayerMana >= 10 && PlayerCovenantSettings == "Venthyr" && IsCovenant)
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
                        if (API.CanCast(LightningBolt) && API.PlayerMaelstrom < 90  && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)))
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
                        if (API.CanCast(Ascendance) && TalentAscendance && IsAscendance && (API.SpellCDDuration(LavaBurst) > 0) && (!TalentIcefury || TalentIcefury && !PlayerHasBuff(Icefury) && !API.CanCast(Icefury)))
                        {
                            API.CastSpell(Ascendance);
                            return;
                        }
                        //actions.single_target +=/ elemental_blast,if= talent.elemental_blast.enabled & (talent.master_of_the_elements.enabled & (buff.master_of_the_elements.up & maelstrom < 60 | !buff.master_of_the_elements.up) | !talent.master_of_the_elements.enabled)
                        if (API.CanCast(ElementalBlast)  && SaveQuake && API.PlayerMaelstrom < 70 && MasterUP && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && TalentElementalBlast && (TalentMasterofTheElements && (PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom < 60 || !PlayerHasBuff(MasteroftheElements)) || !TalentMasterofTheElements))
                        {
                            API.CastSpell(ElementalBlast);
                            return;
                        }
                        //actions.single_target +=/ stormkeeper,if= talent.stormkeeper.enabled & (raid_event.adds.count < 3 | raid_event.adds.in> 50)&(maelstrom < 44)
                        if (API.CanCast(Stormkeeper)  && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && TalentStormkeeper && IsStormkeeper && API.PlayerMaelstrom < 44)
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
                        if (API.CanCast(LavaBurst) && (SaveQuake || PlayerHasBuff(LavaSurge)) && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && PlayerLevel >= 11 && !LastCastlavaBurst && !PlayerHasBuff(MasteroftheElements) && PlayerHasBuff(EchoingShock) && API.PlayerMaelstrom < 90)
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ liquid_magma_totem,if= talent.liquid_magma_totem.enabled
                        if (API.CanCast(LiquidMagmaTotem) && TalentLiquidMagmaTotem)
                        {
                            API.CastSpell(LiquidMagmaTotem);
                            return;
                        }
                        //actions.single_target +=/ lightning_bolt,if= buff.stormkeeper.up & spell_targets.chain_lightning < 2 & (buff.master_of_the_elements.up)
                        if (API.CanCast(LightningBolt) && PlayerHasBuff(Stormkeeper) && API.PlayerMaelstrom < 90 && PlayerHasBuff(MasteroftheElements))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.single_target +=/ earthquake,if= buff.echoes_of_great_StormElemental.up & (!talent.master_of_the_elements.enabled | buff.master_of_the_elements.up)
                        if (API.CanCast(Earthquake) && PlayerLevel >= 38 && PlayerHasBuff(EchoesofGreatSundering) && (!TalentMasterofTheElements || PlayerHasBuff(MasteroftheElements)))
                        {
                            API.CastSpell(Earthquake);
                            return;
                        }
                        //actions.single_target +=/ earth_shock,if= talent.master_of_the_elements.enabled & (buff.master_of_the_elements.up | cooldown.lava_burst.remains > 0 & maelstrom >= 92 | spell_targets.chain_lightning < 2 & buff.stormkeeper.up & cooldown.lava_burst.remains <= gcd) | !talent.master_of_the_elements.enabled
                        if (API.CanCast(EarthShock) && PlayerLevel >= 10 && API.PlayerMaelstrom >= 60 && ((PlayerHasBuff(MasteroftheElements) || API.SpellCDDuration(LavaBurst) > gcd && API.PlayerMaelstrom >= 92 || PlayerHasBuff(Stormkeeper) && API.SpellCDDuration(LavaBurst) <= gcd) || !TalentMasterofTheElements))
                        {
                            API.CastSpell(EarthShock);
                            return;
                        }
                        //actions.single_target+=/lightning_bolt,if=(buff.stormkeeper.remains<1.1*gcd*buff.stormkeeper.stack|buff.stormkeeper.up&buff.master_of_the_elements.up)
                        if (API.CanCast(LightningBolt) && PlayerHasBuff(Stormkeeper) && API.PlayerMaelstrom < 90 && (API.PlayerBuffTimeRemaining(Stormkeeper) < gcd*2 || PlayerHasBuff(Stormkeeper) && PlayerHasBuff(MasteroftheElements)))
                        {
                            API.CastSpell(LightningBolt);
                            return;
                        }
                        //actions.single_target+=/frost_shock,if=talent.icefury.enabled&talent.master_of_the_elements.enabled&buff.icefury.up&buff.master_of_the_elements.up
                        if (API.CanCast(FrostShock) && PlayerLevel >= 17 && PlayerHasBuff(Icefury) && PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom < 60)
                        {
                            API.CastSpell(FrostShock);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= buff.ascendance.up
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && API.PlayerMaelstrom < 90 && PlayerHasBuff(Ascendance) && (SaveQuake || PlayerHasBuff(LavaSurge)) && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace) || PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= cooldown_react & !talent.master_of_the_elements.enabled
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !LastCastlavaBurst && !TalentMasterofTheElements && (SaveQuake || PlayerHasBuff(LavaSurge)) && API.PlayerMaelstrom < 90 && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace) || PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ icefury,if= talent.icefury.enabled & !(maelstrom > 75 & cooldown.lava_burst.remains <= 0)
                        if (API.CanCast(Icefury) && TalentIcefury && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && !(API.PlayerMaelstrom > 75 && API.SpellCDDuration(LavaBurst) < gcd))
                        {
                            API.CastSpell(Icefury);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= cooldown_react & charges > talent.echo_of_the_elements.enabled
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !PlayerHasBuff(MasteroftheElements) && (SaveQuake || PlayerHasBuff(LavaSurge)) && !LastCastlavaBurst && API.SpellCharges(LavaBurst) > 1 && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace) || PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ frost_shock,if= talent.icefury.enabled & buff.icefury.up & buff.icefury.remains < 1.1 * gcd * buff.icefury.stack
                        if (API.CanCast(FrostShock) && PlayerLevel >= 17 && PlayerHasBuff(Icefury) && MasterUP)
                        {
                            API.CastSpell(FrostShock);
                            return;
                        }
                        //actions.single_target +=/ lava_burst,if= cooldown_react
                        if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !PlayerHasBuff(MasteroftheElements) && (SaveQuake || PlayerHasBuff(LavaSurge)) && !LastCastlavaBurst && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace) || PlayerHasBuff(LavaSurge)))
                        {
                            API.CastSpell(LavaBurst);
                            return;
                        }
                        //actions.single_target +=/ chain_harvest
                        if (API.CanCast(ChainHarvest) && IsInRange && API.PlayerMana >= 10  && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && PlayerCovenantSettings == "Venthyr" && IsCovenant)
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
                        if (API.CanCast(LightningBolt) && API.PlayerMaelstrom < 90  && SaveQuake && MasterUP && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)))
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
                    if (API.CanCast(Earthquake) && PlayerLevel >= 38 && API.PlayerMaelstrom >= 60 && PlayerHasBuff(EchoingShock))
                    {
                        API.CastSpell(Earthquake);
                        return;
                    }
                    //actions.aoe +=/ chain_harvest
                    if (API.CanCast(ChainHarvest) && IsInRange  && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && API.PlayerMana >= 10 && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                    {
                        API.CastSpell(ChainHarvest);
                        return;
                    }
                    //actions.aoe +=/ stormkeeper,if= talent.stormkeeper.enabled
                    if (API.CanCast(Stormkeeper)  && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && TalentStormkeeper && IsStormkeeper)
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
                    if (API.CanCast(Ascendance) && TalentAscendance && IsAscendance && !stormwatch.IsRunning && (!TalentIcefury || TalentIcefury && !PlayerHasBuff(Icefury) && !API.CanCast(Icefury)))
                    {
                        API.CastSpell(Ascendance);
                        return;
                    }
                    //actions.aoe +=/ liquid_magma_totem,if= talent.liquid_magma_totem.enabled
                    if (API.CanCast(LiquidMagmaTotem) && TalentLiquidMagmaTotem)
                    {
                        API.CastSpell(LiquidMagmaTotem);
                        return;
                    }
                    //actions.aoe +=/ earth_shock,if= runeforge.echoes_of_great_StormElemental.equipped & !buff.echoes_of_great_StormElemental.up
                    if (API.CanCast(EarthShock) && PlayerLevel >= 10 && API.PlayerMaelstrom >= 60 && EchoLeggy && !PlayerHasBuff(EchoesofGreatSundering))
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
                    if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && (SaveQuake || PlayerHasBuff(LavaSurge)) && !LastCastlavaBurst && (API.PlayerBuffTimeRemaining(Stormkeeper) > 300 * gcd * API.PlayerBuffStacks(Stormkeeper) || !PlayerHasBuff(Stormkeeper)) && !PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom < 90 && API.TargetDebuffRemainingTime(FlameShock) > gcd*2 && (API.TargetUnitInRangeCount < 4 || PlayerHasBuff(LavaSurge) || (TalentMasterofTheElements && !PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom >= 60)))
                    {
                        API.CastSpell(LavaBurst);
                        return;
                    }
                    //actions.aoe +=/ earthquake,if= !talent.master_of_the_elements.enabled | buff.stormkeeper.up | maelstrom >= (100 - 4 * spell_targets.chain_lightning) | buff.master_of_the_elements.up | spell_targets.chain_lightning > 3
                    if (API.CanCast(Earthquake) && PlayerLevel >= 38 && API.PlayerMaelstrom >= 60 && (!TalentMasterofTheElements || PlayerHasBuff(Stormkeeper) || API.PlayerMaelstrom >= 90 || TalentMasterofTheElements && PlayerHasBuff(MasteroftheElements) || API.TargetUnitInRangeCount > 3))
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
                    if (API.CanCast(LavaBurst) && PlayerLevel >= 11 && !LastCastlavaBurst && !PlayerHasBuff(MasteroftheElements) && API.PlayerMaelstrom < 90 && API.TargetDebuffRemainingTime(FlameShock) > gcd * 2 &&  PlayerHasBuff(LavaSurge) && API.TargetUnitInRangeCount < 4 && !stormwatch.IsRunning)
                    {
                        API.CastSpell(LavaBurst);
                        return;
                    }
                    //actions.aoe +=/ elemental_blast,if= talent.elemental_blast.enabled & spell_targets.chain_lightning < 5 & (!pet.storm_elemental.active)
                    if (API.CanCast(ElementalBlast) && API.PlayerMaelstrom < 70  && SaveQuake && MasterUP && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)) && TalentElementalBlast && API.TargetUnitInRangeCount < 5 && !stormwatch.IsRunning)
                    {
                        API.CastSpell(ElementalBlast);
                        return;
                    }
                    //actions.aoe +=/ lava_beam,if= talent.ascendance.enabled
                    if (API.CanCast(LavaBeam) && TalentAscendance && PlayerHasBuff(Ascendance) && API.PlayerMaelstrom < 90  && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)))
                    {
                        API.CastSpell(LavaBeam);
                        return;
                    }
                    //actions.aoe +=/ chain_lightning
                    if (API.CanCast(ChainLightning) && API.PlayerMaelstrom < 90 && MasterUP && PlayerLevel >= 24  && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(SpiritwalkersGrace)))
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

