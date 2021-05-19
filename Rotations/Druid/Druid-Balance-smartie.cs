// Changelog
// v1.0 First release
// v1.1 Starlord and Stellar Drift fixes
// v1.2 switch out of bear fix
// v1.3 covenants added + cd managment
// v1.35 small hotfix
// v1.4 covenant update
// v1.5 legendary prep
// v1.6 eclipse update
// v1.65 another eclipse update
// v1.7 convoke update
// v1.8 Racials and Trinkets
// v1.9 Mighty bash added
// v2.0 core update
// v2.1 spell ids and alot of other stuff
// v2.2 Save AP Toggle
// v2.3 Stellar Flare double cast fix
// v2.4 Dont dot Toggle added
// v2.5 aoe detection changed and alot of other small things
// v2.6 aoe changed a bit
// v2.7 eclpise fix and some small things
// v2.8 Racials and a few small fixes
// v2.9 Balance of all Things support
// v3.0 Quaking Helper and small adjustment
// v3.1 Quaking channeling fix
// v3.2 Night Fae simp update
// v3.3 small adjustment
// v3.4 small adjustment
// v3.5 no leggy fix
// v3.6 small aoe change
// v3.7 aoe tweaks and a few small bugfixes
// v3.8 Dot Spam fix
// v3.9 convoke update
// v4.0 woe update
// v4.1 small update to a few things
// v4.2 another small tweak
// v4.3 various little changes
// v4.4 some small tweaks especially for aoe
// v4.5 few single target tweaks
// v4.55 potential racial fix
// v4.6 stopcasting bug fixed
// v4.7 auto break roots
// v4.75 root break adjustment
// v4.8 explosive killer and some aoe adjustment for cds
// v4.85 AOE cd fix
// v4.9 aoe adjustment
// v4.95 additional small change
// v5.0 precast change for hardaoe
// v5.1 trinket adjustment and small hotfix
// v5.2 hard aoe cd fix
// v5.3 rewrite of some things
// v5.4 small adjustment

using System.Diagnostics;

namespace HyperElk.Core
{
    public class BalanceDruid : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool DontDOT => API.ToggleIsEnabled("Don`t Dot");
        private bool SaveAP => API.ToggleIsEnabled("Save AP");
        //Spell,Auras
        private string Moonfire = "Moonfire";
        private string Sunfire = "Sunfire";
        private string StellarFlare = "Stellar Flare";
        private string Starsurge = "Starsurge";
        private string Starfall = "Starfall";
        private string Starfire = "Starfire";
        private string Wrath = "Wrath";
        private string NewMoon = "New Moon";
        private string HalfMoon = "Half Moon";
        private string FullMoon = "Full Moon";
        private string Incarnation = "Incarnation: Chosen of Elune";
        private string CelestialAlignment = "Celestial Alignment";
        private string MoonkinForm = "Moonkin Form";
        private string WarriorofElune = "Warrior of Elune";
        private string ForceofNature = "Force of Nature";
        private string FuryofElune = "Fury of Elune";
        private string Renewal = "Renewal";
        private string Regrowth = "Regrowth";
        private string Barkskin = "Barkskin";
        private string SolarBeam = "Solar Beam";
        private string Typhoon = "Typhoon";
        private string BearForm = "Bear Form";
        private string Thrash = "Thrash";
        private string Mangle = "Mangle";
        private string FrenziedRegeneration = "Frenzied Regeneration";
        private string Ironfur = "Ironfur";
        private string EclipseSolar = "EclipseSolar";
        private string EclipseLunar = "EclipseLunar";
        private string TravelForm = "Travel Form";
        private string CatForm = "Cat Form";
        private string Starlord = "Starlord";
        private string RavenousFrenzy = "Ravenous Frenzy";
        private string ConvoketheSpirits = "Convoke the Spirits";
        private string AdaptiveSwarm = "Adaptive Swarm";
        private string LoneEmpowerment = "Lone Empowerment";
        private string LoneSpirit = "Lone Spirit";
        private string Soulshape = "Soulshape";
        private string OnethsClearVision = "Oneth's Clear Vision";
        private string OnethsPerception = "Oneth's Perception";
        private string MightyBash = "Mighty Bash";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string EmpowerBond = "Empower Bond";
        private string KindredSpirits = "Kindred Spirits";
        private string CancelStarlord = "Cancel Starlord Macro";
        private string Quake = "Quake";
        private string Stopcast = "Stopcast Macro";
        private string BalanceofallThings = "Balance of all Things";
        private string BalanceofallThings2 = "Balance of all Things2";
        private string Potion = "Potion of Spectral Intellect";

        //Talents
        bool TalentNatureBalance => API.PlayerIsTalentSelected(1, 1);
        bool TalentWarriorOfElune => API.PlayerIsTalentSelected(1, 2);
        bool TalentForceOfNature => API.PlayerIsTalentSelected(1, 3);
        bool TalentRenewal => API.PlayerIsTalentSelected(2, 2);
        bool TalentGuardianAffinity => API.PlayerIsTalentSelected(3, 2);
        bool TalentSouloftheForest => API.PlayerIsTalentSelected(5, 1);
        bool TalentIncarnation => API.PlayerIsTalentSelected(5, 3);
        bool TalentStarlord => API.PlayerIsTalentSelected(5, 2);
        bool TalentStellarDrift => API.PlayerIsTalentSelected(6, 1);
        bool TalentStellarFlare => API.PlayerIsTalentSelected(6, 3);
        bool TalentFuryOfElune => API.PlayerIsTalentSelected(7, 2);
        bool TalentNewMoon => API.PlayerIsTalentSelected(7, 3);
        bool TalentMightyBash => API.PlayerIsTalentSelected(4, 1);

        //General
        private static readonly Stopwatch Solarwatch = new Stopwatch();
        private static readonly Stopwatch Lunarwatch = new Stopwatch();
        private static readonly Stopwatch movingwatch = new Stopwatch();
        private int PlayerLevel => API.PlayerLevel;
        private bool isExplosive => API.TargetMaxHealth <= 600 && API.TargetMaxHealth != 0 && PlayerLevel == 60;
        private bool HardAoE => API.TargetUnitInRangeCount >= 5;
        private bool isinRange => API.TargetRange <= 45;
        private bool isMOinRange => API.MouseoverRange < 45;
        float GCD => API.SpellGCDTotalDuration;
        private bool UseStarlord => (TalentStarlord && API.PlayerBuffTimeRemaining(Starlord) == 0 || TalentStarlord && API.PlayerBuffTimeRemaining(Starlord) > 400 && API.PlayerBuffTimeRemaining(Starlord) != 5000000 || !TalentStarlord);
        bool IsIncarnation => (UseIncarnation == "with Cooldowns" || UseIncarnation == "with Cooldowns or AoE" || UseIncarnation == "on mobcount or Cooldowns") && IsCooldowns || UseIncarnation == "always" || (UseIncarnation == "on AOE" || UseIncarnation == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseIncarnation == "on mobcount or Cooldowns" || UseIncarnation == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsCelestialAlignment => (UseCelestialAlignment == "with Cooldowns" || UseCelestialAlignment == "with Cooldowns or AoE" || UseCelestialAlignment == "on mobcount or Cooldowns") && IsCooldowns || UseCelestialAlignment == "always" || (UseCelestialAlignment == "on AOE" || UseCelestialAlignment == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseCelestialAlignment == "on mobcount or Cooldowns" || UseCelestialAlignment == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsWarriorofElune => (UseWarriorofElune == "with Cooldowns" || UseWarriorofElune == "with Cooldowns or AoE" || UseWarriorofElune == "on mobcount or Cooldowns") && IsCooldowns || UseWarriorofElune == "always" || (UseWarriorofElune == "on AOE" || UseWarriorofElune == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseWarriorofElune == "on mobcount or Cooldowns" || UseWarriorofElune == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsForceofNature => (UseForceofNature == "with Cooldowns" || UseForceofNature == "with Cooldowns or AoE" || UseForceofNature == "on mobcount or Cooldowns") && IsCooldowns || UseForceofNature == "always" || (UseForceofNature == "on AOE" || UseForceofNature == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseForceofNature == "on mobcount or Cooldowns" || UseForceofNature == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsFuryofElune => (UseFuryofElune == "with Cooldowns" || UseFuryofElune == "with Cooldowns or AoE" || UseFuryofElune == "on mobcount or Cooldowns") && IsCooldowns || UseFuryofElune == "always" || (UseFuryofElune == "on AOE" || UseFuryofElune == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseFuryofElune == "on mobcount or Cooldowns" || UseFuryofElune == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsCovenant => (UseCovenant == "with Cooldowns" || UseCovenant == "with Cooldowns or AoE" || UseCovenant == "on mobcount or Cooldowns") && IsCooldowns || UseCovenant == "always" || (UseCovenant == "on AOE" || UseCovenant == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseCovenant == "on mobcount or Cooldowns" || UseCovenant == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsTrinkets1 => ((UseTrinket1 == "with Cooldowns" || UseTrinket1 == "with Cooldowns or AoE" || UseTrinket1 == "on mobcount or Cooldowns") && IsCooldowns && (IncaCelestial || TalentIncarnation && API.SpellCDDuration(Incarnation) > 9000 || !TalentIncarnation && API.SpellCDDuration(CelestialAlignment) > 9000) || UseTrinket1 == "always" || (UseTrinket1 == "on AOE" || UseTrinket1 == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseTrinket1 == "on mobcount or Cooldowns" || UseTrinket1 == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount) && isinRange;
        bool IsTrinkets2 => ((UseTrinket2 == "with Cooldowns" || UseTrinket2 == "with Cooldowns or AoE" || UseTrinket2 == "on mobcount or Cooldowns") && IsCooldowns && (IncaCelestial || TalentIncarnation && API.SpellCDDuration(Incarnation) > 9000 || !TalentIncarnation && API.SpellCDDuration(CelestialAlignment) > 9000) || UseTrinket2 == "always" || (UseTrinket2 == "on AOE" || UseTrinket2 == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseTrinket2 == "on mobcount or Cooldowns" || UseTrinket2 == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount) && isinRange;

        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "with Cooldowns or AoE", "on mobcount", "on mobcount or Cooldowns", "always" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        private int MobCount => numbRaidList[CombatRoutine.GetPropertyInt("MobCount")];
        public string[] Legendary = new string[] { "None", "Balance of all things", "Dreambinder" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseIncarnation => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Incarnation)];
        private string UseCelestialAlignment => CDUsageWithAOE[CombatRoutine.GetPropertyInt(CelestialAlignment)];
        private string UseWarriorofElune => CDUsageWithAOE[CombatRoutine.GetPropertyInt(WarriorofElune)];
        private string UseForceofNature => CDUsageWithAOE[CombatRoutine.GetPropertyInt(ForceofNature)];
        private string UseFuryofElune => CDUsageWithAOE[CombatRoutine.GetPropertyInt(FuryofElune)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        public bool rootbreaker => CombatRoutine.GetPropertyBool("Rootbreaker");
        public bool UseDPSPotion => CombatRoutine.GetPropertyBool("UseDPSPotion");
        private string IsLegendary => Legendary[CombatRoutine.GetPropertyInt("Legendary")];
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        private bool SpamDots => CombatRoutine.GetPropertyBool("SpamDots");
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private bool QuakingHelper => CombatRoutine.GetPropertyBool("QuakingHelper");
        private int RegrowthLifePercent => numbList[CombatRoutine.GetPropertyInt(Regrowth)];
        private int BarkskinLifePercent => numbList[CombatRoutine.GetPropertyInt(Barkskin)];
        private int BearFormLifePercent => numbList[CombatRoutine.GetPropertyInt(BearForm)];
        private int RenewalLifePercent => numbList[CombatRoutine.GetPropertyInt(Renewal)];
        private int FrenziedRegenerationLifePercent => numbList[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => numbList[CombatRoutine.GetPropertyInt(Ironfur)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private bool IncaCelestial => (PlayerHasBuff(Incarnation) || PlayerHasBuff(CelestialAlignment));
        private bool Eclipses => (API.PlayerBuffTimeRemaining(EclipseLunar) > 100 || API.PlayerBuffTimeRemaining(EclipseSolar) > 100);
        bool BOAT => API.PlayerBuffTimeRemaining(BalanceofallThings) > 100 || API.PlayerBuffTimeRemaining(BalanceofallThings2) > 100;
        bool BOAT2 => API.PlayerBuffTimeRemaining(BalanceofallThings) > 350 || API.PlayerBuffTimeRemaining(BalanceofallThings2) > 350;
        private bool Quaking => ((API.PlayerCurrentCastTimeRemaining >= 200 || API.PlayerIsChanneling) && API.PlayerDebuffRemainingTime(Quake) < 200) && PlayerHasDebuff(Quake);
        private bool SaveQuake => (PlayerHasDebuff(Quake) && API.PlayerDebuffRemainingTime(Quake) > 200 && QuakingHelper || !PlayerHasDebuff(Quake) || !QuakingHelper);
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        private static bool PlayerHasDebuff(string buff)
        {
            return API.PlayerHasDebuff(buff, false, false);
        }
        bool covlegy1 => PlayerCovenantSettings == "Night Fae" && IsLegendary == "Balance of all things" && IsCovenant && API.SpellCDDuration(ConvoketheSpirits) <= GCD;
        bool covlegy2 => PlayerCovenantSettings != "Night Fae" || IsLegendary != "Balance of all things" || PlayerCovenantSettings == "Night Fae" && IsLegendary == "Balance of all things" && IsCovenant && API.SpellCDDuration(ConvoketheSpirits) > GCD;
        bool incarntrigger1 => API.CanCast(Incarnation) && API.PlayerAstral <= 30 && !IncaCelestial && IsIncarnation && (API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || (API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347) || !TalentStellarFlare) || HardAoE && IsAOE && TargetHasDebuff(Sunfire) || API.TargetTimeToDie < 1350) && TalentIncarnation;
        bool incarntrigger2 => API.CanCast(Incarnation) && API.PlayerAstral >= 90 && !IncaCelestial && IsIncarnation && (API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || (API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347) || !TalentStellarFlare) || HardAoE && IsAOE && TargetHasDebuff(Sunfire) || API.TargetTimeToDie < 1350) && TalentIncarnation;
        bool celesttrigger1 => API.CanCast(CelestialAlignment) && !IncaCelestial && IsCelestialAlignment && API.PlayerAstral <= 30 && (API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || (API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347) || !TalentStellarFlare) || HardAoE && IsAOE && TargetHasDebuff(Sunfire) || API.TargetTimeToDie < 1350) && !TalentIncarnation;
        bool celesttrigger2 => API.CanCast(CelestialAlignment) && !IncaCelestial && IsCelestialAlignment && API.PlayerAstral >= 90 && (API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || (API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347) || !TalentStellarFlare) || HardAoE && IsAOE && TargetHasDebuff(Sunfire) || API.TargetTimeToDie < 1350) && !TalentIncarnation;
        bool incarcelerdy => (covlegy1 && (incarntrigger1 || celesttrigger1) || covlegy2 && (incarntrigger2 || celesttrigger2));
        bool SolarEclipseNext => !Eclipses && Lunarwatch.IsRunning;
        bool LunarEclipseNext => !Eclipses && Solarwatch.IsRunning;
        bool AnyEclipseNext => !Eclipses && !Lunarwatch.IsRunning && !Solarwatch.IsRunning;
        bool ChannelingConvoke => API.CurrentCastSpellID("player") == 323764;

        public override void Initialize()
        {
            CombatRoutine.Name = "Balance Druid by smartie";
            API.WriteLog("Welcome to smartie`s Balance Druid v5.4");
            API.WriteLog("For this rota you need to following macros");
            API.WriteLog("MoonfireMO - /cast [@mouseover] Moonfire");
            API.WriteLog("SunfireMO - /cast [@mouseover] Sunfire");
            API.WriteLog("StellarFlareMO - /cast [@mouseover] Stellar Flare");
            API.WriteLog("Stopcasting - /stopcasting");
            API.WriteLog("Cancel Starlord - /cancelaura Starlord");
            API.WriteLog("these macros are required and not having them might break the rota");

            //Spells
            CombatRoutine.AddSpell(Moonfire, 8921, "D1");
            CombatRoutine.AddSpell(Sunfire, 93402, "D2");
            CombatRoutine.AddSpell(StellarFlare, 202347, "D7");
            CombatRoutine.AddSpell(Starsurge, 78674, "D5");
            CombatRoutine.AddSpell(Starfall, 191034, "D6");
            CombatRoutine.AddSpell(Starfire, 194153, "D4");
            CombatRoutine.AddSpell(Wrath, 190984, "D3");
            CombatRoutine.AddSpell(NewMoon, 274281, "Oem6");
            CombatRoutine.AddSpell(HalfMoon, 274282, "Oem6");
            CombatRoutine.AddSpell(FullMoon, 274283, "Oem6");
            CombatRoutine.AddSpell(Incarnation, 102560, "D8");
            CombatRoutine.AddSpell(CelestialAlignment, 194223, "D8");
            CombatRoutine.AddSpell(MoonkinForm, 24858, "NumPad1");
            CombatRoutine.AddSpell(FuryofElune, 202770, "NumPad4");
            CombatRoutine.AddSpell(WarriorofElune, 202425, "NumPad2");
            CombatRoutine.AddSpell(ForceofNature, 205636, "NumPad3");
            CombatRoutine.AddSpell(Renewal, 108238, "F1");
            CombatRoutine.AddSpell(Regrowth, 8936, "F6");
            CombatRoutine.AddSpell(Barkskin, 22812, "F1");
            CombatRoutine.AddSpell(SolarBeam, 78675, "F12");
            CombatRoutine.AddSpell(MightyBash, 5211, "D1");
            CombatRoutine.AddSpell(Typhoon, 132469, "D0");
            CombatRoutine.AddSpell(BearForm, 5487, "NumPad5");
            CombatRoutine.AddSpell(Thrash, 77758, "D4");
            CombatRoutine.AddSpell(Mangle, 33917, "D3");
            CombatRoutine.AddSpell(FrenziedRegeneration, 22842, "D6");
            CombatRoutine.AddSpell(Ironfur, 192081, "D5");
            CombatRoutine.AddSpell(TravelForm, 783, "NumPad6");
            CombatRoutine.AddSpell(RavenousFrenzy, 323546, "D1");
            CombatRoutine.AddSpell(ConvoketheSpirits, 323764, "D1");
            CombatRoutine.AddSpell(AdaptiveSwarm, 325727, "D1");
            CombatRoutine.AddSpell(LoneEmpowerment, 338142, "D1");
            CombatRoutine.AddSpell(EmpowerBond, "D1");

            //Macros
            CombatRoutine.AddMacro(Moonfire + "MO", "NumPad7");
            CombatRoutine.AddMacro(Sunfire + "MO", "NumPad8");
            CombatRoutine.AddMacro(StellarFlare + "MO", "NumPad9");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            CombatRoutine.AddMacro(CancelStarlord);
            CombatRoutine.AddMacro(Stopcast, "F10");

            //Buffs
            CombatRoutine.AddBuff(EclipseLunar, 48518);
            CombatRoutine.AddBuff(EclipseSolar, 48517);
            CombatRoutine.AddBuff(MoonkinForm, 24858);
            CombatRoutine.AddBuff(CelestialAlignment, 194223);
            CombatRoutine.AddBuff(Incarnation, 102560);
            CombatRoutine.AddBuff(Starfall, 191034);
            CombatRoutine.AddBuff(TravelForm, 783);
            CombatRoutine.AddBuff(WarriorofElune, 202425);
            CombatRoutine.AddBuff(BearForm, 5487);
            CombatRoutine.AddBuff(CatForm, 768);
            CombatRoutine.AddBuff(FrenziedRegeneration, 22842);
            CombatRoutine.AddBuff(Starlord, 279709);
            CombatRoutine.AddBuff(RavenousFrenzy, 323546);
            CombatRoutine.AddBuff(LoneSpirit, 338041);
            CombatRoutine.AddBuff(Soulshape, 310143);
            CombatRoutine.AddBuff(OnethsClearVision, 339797);
            CombatRoutine.AddBuff(OnethsPerception, 339800);
            CombatRoutine.AddBuff(KindredSpirits, 326967);
            CombatRoutine.AddBuff(BalanceofallThings, 339946);
            CombatRoutine.AddBuff(BalanceofallThings2, 339943);

            //Debuff
            CombatRoutine.AddDebuff(Moonfire, 164812);
            CombatRoutine.AddDebuff(Sunfire, 164815);
            CombatRoutine.AddDebuff(StellarFlare, 202347);
            CombatRoutine.AddDebuff(Thrash, 192090);
            CombatRoutine.AddDebuff(AdaptiveSwarm, 325727);
            CombatRoutine.AddDebuff(Quake, 240447);
            CombatRoutine.AddDebuff("Frozen Binds", 320788);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Save AP");
            CombatRoutine.AddToggle("Don`t Dot");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);
            CombatRoutine.AddItem(Potion, 171273);

            //Prop
            CombatRoutine.AddProp("MobCount", "Mobcount to use Cooldowns ", numbRaidList, " Mobcount to use Cooldowns", "Cooldowns", 3);
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("Rootbreaker", "Break roots", false, "Break roots with shapeshift", "Generic");
            CombatRoutine.AddProp("UseDPSPotion", "Use DPS Potion", false, "Will use DPS potion with cds", "Cooldowns");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Incarnation, "Use " + Incarnation, CDUsageWithAOE, "Use " + Incarnation + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(CelestialAlignment, "Use " + CelestialAlignment, CDUsageWithAOE, "Use " + CelestialAlignment + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(WarriorofElune, "Use " + WarriorofElune, CDUsageWithAOE, "Use " + WarriorofElune + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(ForceofNature, "Use " + ForceofNature, CDUsageWithAOE, "Use " + ForceofNature + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(FuryofElune, "Use " + FuryofElune, CDUsageWithAOE, "Use " + FuryofElune + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("Legendary", "Choose " + "Legendary", Legendary, "Choose your Legendary", "Legendary");
            CombatRoutine.AddProp("SpamDots", "SpamDots", true, "Will spam Dots while moving", "Generic");
            CombatRoutine.AddProp("AutoForm", "AutoForm", true, "Will auto switch forms", "Generic");
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
            CombatRoutine.AddProp("QuakingHelper", "Quaking Helper", false, "Will cancle casts on Quaking", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", numbList, "Life percent at which" + Renewal + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Regrowth, Regrowth + " Life Percent", numbList, "Life percent at which" + Regrowth + "is used, set to 0 to disable", "Defense", 30);
            CombatRoutine.AddProp(Barkskin, Barkskin + " Life Percent", numbList, "Life percent at which" + Barkskin + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(BearForm, BearForm + " Life Percent", numbList, "Life percent at which rota will go into" + BearForm + "set to 0 to disable", "Defense", 10);
            CombatRoutine.AddProp(FrenziedRegeneration, FrenziedRegeneration + " Life Percent", numbList, "Life percent at which" + FrenziedRegeneration + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp(Ironfur, Ironfur + " Life Percent", numbList, "Life percent at which" + Ironfur + "is used, set to 0 to disable", "Defense", 90);
        }
        public override void Pulse()
        {
            //API.WriteLog("eclipse cd: " + API.PlayerBuffTimeRemaining(EclipseSolar));
            if (!Lunarwatch.IsRunning && PlayerHasBuff(EclipseLunar) && !PlayerHasBuff(EclipseSolar))
            {
                Solarwatch.Stop();
                Solarwatch.Reset();
                Lunarwatch.Restart();
                API.WriteLog("Starting Lunarwatch.");
            }
            if (!Solarwatch.IsRunning && PlayerHasBuff(EclipseSolar) && !PlayerHasBuff(EclipseLunar))
            {
                Lunarwatch.Stop();
                Lunarwatch.Reset();
                Solarwatch.Restart();
                API.WriteLog("Starting Solarwatch.");
            }
            if ((Solarwatch.IsRunning || Lunarwatch.IsRunning) && PlayerHasBuff(EclipseLunar) && PlayerHasBuff(EclipseSolar))
            {
                Lunarwatch.Stop();
                Lunarwatch.Reset();
                Solarwatch.Stop();
                Solarwatch.Reset();
                API.WriteLog("Reset Lunar/Solarwatch.");
            }
            if (!movingwatch.IsRunning && API.PlayerIsMoving && API.PlayerIsInCombat)
            {
                movingwatch.Restart();
            }
            if (movingwatch.IsRunning && !API.PlayerIsMoving)
            {
                movingwatch.Stop();
                movingwatch.Reset();
            }
            if (movingwatch.IsRunning && !API.PlayerIsInCombat)
            {
                movingwatch.Stop();
                movingwatch.Reset();
            }
            /*if ((Solarwatch.IsRunning || Lunarwatch.IsRunning) && !API.PlayerIsInCombat && !(PlayerHasBuff(EclipseLunar) || PlayerHasBuff(EclipseSolar)))
            {
                Lunarwatch.Stop();
                Lunarwatch.Reset();
                Solarwatch.Stop();
                Solarwatch.Reset();
                API.WriteLog("Reset Lunar/Solarwatch.");
            }*/
        }
        public override void CombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 && !API.MacroIsIgnored(Stopcast) && QuakingHelper && Quaking)
            {
                API.CastSpell(Stopcast);
                return;
            }
            if (BOAT2 && API.PlayerAstral >= 30 && (API.PlayerCurrentCastTimeRemaining > 40 && (API.PlayerCurrentCastSpellID == 194153 || API.PlayerCurrentCastSpellID == 190984)) && !API.MacroIsIgnored(Stopcast) && (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE))
            {
                API.CastSpell(Stopcast);
                return;
            }
            if ((ChannelingConvoke && API.PlayerCurrentCastTimeRemaining > 0 || API.PlayerCurrentCastTimeRemaining > 40 && Eclipses && !ChannelingConvoke || !Eclipses && API.PlayerCurrentCastTimeRemaining > 0 || HardAoE && IsAOE && !Eclipses && !ChannelingConvoke && Lunarwatch.IsRunning && API.PlayerCurrentCastTimeRemaining > 40) || API.PlayerSpellonCursor)
                return;
            if (!API.PlayerIsMounted && !PlayerHasBuff(TravelForm))
            {
                if (isInterrupt && API.CanCast(SolarBeam) && PlayerLevel >= 26 && isinRange)
                {
                    API.CastSpell(SolarBeam);
                    return;
                }
                if (PlayerRaceSettings == "Tauren" && API.CanCast(RacialSpell1) && isInterrupt && !API.PlayerIsMoving && isRacial && API.TargetRange < 8 && API.SpellISOnCooldown(SolarBeam))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.CanCast(MightyBash) && isInterrupt && TalentMightyBash && API.TargetRange < 10 && API.SpellISOnCooldown(SolarBeam))
                {
                    API.CastSpell(MightyBash);
                    return;
                }
                if (API.PlayerHealthPercent <= RegrowthLifePercent && SaveQuake && PlayerLevel >= 3 && API.CanCast(Regrowth))
                {
                    API.CastSpell(Regrowth);
                    return;
                }
                if (API.CanCast(Barkskin) && PlayerLevel >= 24 && API.PlayerHealthPercent <= BarkskinLifePercent)
                {
                    API.CastSpell(Barkskin);
                    return;
                }
                if (API.CanCast(Renewal) && API.PlayerHealthPercent <= RenewalLifePercent && TalentRenewal)
                {
                    API.CastSpell(Renewal);
                    return;
                }
                if (API.PlayerItemCanUse(PhialofSerenity) && !API.MacroIsIgnored(PhialofSerenity) && API.PlayerItemRemainingCD(PhialofSerenity) == 0 && API.PlayerHealthPercent <= PhialofSerenityLifePercent)
                {
                    API.CastSpell(PhialofSerenity);
                    return;
                }
                if (API.PlayerItemCanUse(SpiritualHealingPotion) && !API.MacroIsIgnored(SpiritualHealingPotion) && API.PlayerItemRemainingCD(SpiritualHealingPotion) == 0 && API.PlayerHealthPercent <= SpiritualHealingPotionLifePercent)
                {
                    API.CastSpell(SpiritualHealingPotion);
                    return;
                }
                if (API.PlayerHealthPercent <= FrenziedRegenerationLifePercent && API.PlayerRage >= 10 && API.CanCast(FrenziedRegeneration) && PlayerHasBuff(BearForm) && TalentGuardianAffinity)
                {
                    API.CastSpell(FrenziedRegeneration);
                    return;
                }
                if (API.PlayerHealthPercent <= IronfurLifePercent && API.PlayerRage >= 40 && API.CanCast(Ironfur) && TalentGuardianAffinity && PlayerHasBuff(BearForm))
                {
                    API.CastSpell(Ironfur);
                    return;
                }
                if (API.PlayerHealthPercent <= BearFormLifePercent && PlayerLevel >= 8 && AutoForm && API.CanCast(BearForm) && !PlayerHasBuff(BearForm))
                {
                    API.CastSpell(BearForm);
                    return;
                }
                if (API.CanCast(MoonkinForm) && API.PlayerIsCC(CCList.ROOT) && !API.PlayerHasDebuff("Frozen Binds") && AutoForm && rootbreaker)
                {
                    API.CastSpell(MoonkinForm);
                    return;
                }
                if (API.PlayerHealthPercent > BearFormLifePercent && BearFormLifePercent != 0 && API.CanCast(MoonkinForm) && PlayerHasBuff(BearForm) && AutoForm)
                {
                    API.CastSpell(MoonkinForm);
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
            if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !PlayerHasBuff(TravelForm))
            {
                API.CastSpell(TravelForm);
                return;
            }
        }
        private void rotation()
        {
            if (isExplosive && PlayerHasBuff(MoonkinForm))
            {
                if (API.CanCast(Moonfire) && isinRange)
                {
                    API.CastSpell(Moonfire);
                    API.WriteLog("Explosive killer");
                    return;
                }
            }
            if (!isExplosive || PlayerHasBuff(MoonkinForm))
            {
                if (API.CanCast(MoonkinForm) && PlayerLevel >= 21 && !PlayerHasBuff(MoonkinForm) && !PlayerHasBuff(BearForm) && !PlayerHasBuff(CatForm) && !PlayerHasBuff(Soulshape))
                {
                    API.CastSpell(MoonkinForm);
                    return;
                }
                if (PlayerHasBuff(BearForm))
                {
                    if (API.CanCast(Thrash) && API.TargetRange < 9 && TalentGuardianAffinity && PlayerLevel >= 11 && !TargetHasDebuff(Thrash))
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                    if (API.CanCast(Mangle) && API.TargetRange < 6 && PlayerLevel >= 8 && (TargetHasDebuff(Thrash) || !TalentGuardianAffinity))
                    {
                        API.CastSpell(Mangle);
                        return;
                    }
                }
                if (isinRange && (PlayerHasBuff(MoonkinForm) || PlayerLevel < 21))
                {
                    //actions+=/berserking,if=(!covenant.night_fae|!cooldown.convoke_the_spirits.up)&buff.ca_inc.up
                    if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && isinRange && (PlayerCovenantSettings != "Night Fae" || API.SpellCDDuration(ConvoketheSpirits) <= GCD) && IncaCelestial)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1 && Eclipses && !API.MacroIsIgnored("Trinket1"))
                    {
                        API.CastSpell("Trinket1");
                        return;
                    }
                    if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2 && Eclipses && !API.MacroIsIgnored("Trinket2"))
                    {
                        API.CastSpell("Trinket2");
                        return;
                    }
                    if (!API.MacroIsIgnored(Potion) && UseDPSPotion && API.PlayerItemCanUse(Potion) && API.PlayerItemRemainingCD(Potion) == 0 && IncaCelestial)
                    {
                        API.CastSpell(Potion);
                        return;
                    }
                    if (PlayerCovenantSettings == "Night Fae" && IsLegendary == "Balance of all things" && IsCovenant && API.SpellCDDuration(ConvoketheSpirits) <= GCD && API.SpellGCDDuration == 0)
                    {
                        if (API.CanCast(Incarnation) && API.PlayerAstral <= 30 && !IncaCelestial && IsIncarnation && (API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || (API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347) || !TalentStellarFlare) || HardAoE && IsAOE && TargetHasDebuff(Sunfire) || API.TargetTimeToDie < 1350) && TalentIncarnation)
                        {
                            API.CastSpell(Incarnation);
                            return;
                        }
                        if (API.CanCast(CelestialAlignment) && !IncaCelestial && IsCelestialAlignment && API.PlayerAstral <= 30 && (API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || (API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347) || !TalentStellarFlare) || HardAoE && IsAOE && TargetHasDebuff(Sunfire) || API.TargetTimeToDie < 1350) && !TalentIncarnation)
                        {
                            API.CastSpell(CelestialAlignment);
                            return;
                        }
                    }
                    if (PlayerCovenantSettings != "Night Fae" || IsLegendary != "Balance of all things" || PlayerCovenantSettings == "Night Fae" && IsLegendary == "Balance of all things" && IsCovenant && API.SpellCDDuration(ConvoketheSpirits) > GCD && API.SpellGCDDuration == 0)
                    {
                        if (API.CanCast(Incarnation) && API.PlayerAstral >= 90 && !IncaCelestial && IsIncarnation && (API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || (API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347) || !TalentStellarFlare) || HardAoE && IsAOE && TargetHasDebuff(Sunfire) || API.TargetTimeToDie < 1350) && TalentIncarnation)
                        {
                            API.CastSpell(Incarnation);
                            return;
                        }
                        if (API.CanCast(CelestialAlignment) && !IncaCelestial && IsCelestialAlignment && API.PlayerAstral >= 90 && (API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || (API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347) || !TalentStellarFlare) || HardAoE && IsAOE && TargetHasDebuff(Sunfire) || API.TargetTimeToDie < 1350) && !TalentIncarnation)
                        {
                            API.CastSpell(CelestialAlignment);
                            return;
                        }
                    }
                    if (API.CanCast(FuryofElune) && TalentFuryOfElune && IsFuryofElune)
                    {
                        API.CastSpell(FuryofElune);
                        return;
                    }
                    if (API.CanCast(ForceofNature) && TalentForceOfNature && IsForceofNature && (IncaCelestial || ((API.SpellCDDuration(Incarnation) > 3000 && TalentIncarnation && IsIncarnation || !IsIncarnation && TalentIncarnation) || (API.SpellCDDuration(CelestialAlignment) > 3000 && !TalentIncarnation && IsCelestialAlignment || !IsCelestialAlignment && !TalentIncarnation))))
                    {
                        API.CastSpell(ForceofNature);
                        return;
                    }
                    if (API.CanCast(RavenousFrenzy) && isinRange && IncaCelestial && PlayerCovenantSettings == "Venthyr" && IsCovenant)
                    {
                        API.CastSpell(RavenousFrenzy);
                        return;
                    }
                    //actions.st+=/convoke_the_spirits,if=(variable.convoke_desync&!cooldown.ca_inc.ready|buff.ca_inc.up)&astral_power<50&(buff.eclipse_lunar.remains>10|buff.eclipse_solar.remains>10)|fight_remains<10
                    if (API.CanCast(ConvoketheSpirits) && isinRange && SaveQuake && PlayerCovenantSettings == "Night Fae" && IsCovenant && IncaCelestial && (IsLegendary != "Balance of all things" && API.PlayerAstral <= 30 && (API.PlayerBuffTimeRemaining(EclipseSolar) > 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) > 1000) || IsLegendary == "Balance of all things" && BOAT))
                    {
                        API.CastSpell(ConvoketheSpirits);
                        return;
                    }
                    if (API.CanCast(LoneEmpowerment) && isinRange && PlayerHasBuff(LoneSpirit) && PlayerCovenantSettings == "Kyrian" && IsCovenant && (IncaCelestial || TalentIncarnation && API.SpellCDDuration(Incarnation) > 5000 && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000) || !TalentIncarnation && API.SpellCDDuration(CelestialAlignment) > 5000 && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000) || !IsIncarnation && TalentIncarnation && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000) || !IsCelestialAlignment && !TalentIncarnation && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000)))
                    {
                        API.CastSpell(LoneEmpowerment);
                        return;
                    }
                    if (API.CanCast(EmpowerBond) && isinRange && PlayerHasBuff(KindredSpirits) && PlayerCovenantSettings == "Kyrian" && IsCovenant && (IncaCelestial || TalentIncarnation && API.SpellCDDuration(Incarnation) > 5000 && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000) || !TalentIncarnation && API.SpellCDDuration(CelestialAlignment) > 5000 && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000) || !IsIncarnation && TalentIncarnation && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000) || !IsCelestialAlignment && !TalentIncarnation && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000)))
                    {
                        API.CastSpell(EmpowerBond);
                        return;
                    }
                    if (API.CanCast(AdaptiveSwarm) && isinRange && PlayerCovenantSettings == "Necrolord" && IsCovenant && !TargetHasDebuff(AdaptiveSwarm))
                    {
                        API.CastSpell(AdaptiveSwarm);
                        return;
                    }
                    if (API.CanCast(WarriorofElune) && !PlayerHasBuff(WarriorofElune) && TalentWarriorOfElune && IsWarriorofElune && API.PlayerAstral <= 30 && (PlayerHasBuff(EclipseLunar) || HardAoE && IsAOE))
                    {
                        API.CastSpell(WarriorofElune);
                        return;
                    }
                    if (PlayerHasBuff(Starlord) && API.PlayerBuffTimeRemaining(Starlord) > 100 && !API.MacroIsIgnored(CancelStarlord) && API.PlayerAstral >= 85 && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 1000))
                    {
                        API.CastSpell(CancelStarlord);
                        return;
                    }
                    if (PlayerHasBuff(Starlord) && API.PlayerBuffTimeRemaining(Starlord) < 400 && (API.PlayerBuffTimeRemaining(BalanceofallThings) >= 450 || API.PlayerBuffTimeRemaining(BalanceofallThings2) >= 450) && !API.MacroIsIgnored(CancelStarlord))
                    {
                        API.CastSpell(CancelStarlord);
                        return;
                    }
                    // Single Target rota
                    if (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE)
                    {
                        if (PlayerCovenantSettings == "Night Fae")
                        {
                            if (API.CanCast(Starsurge) && !SaveAP && IsLegendary == "Balance of all things" && API.PlayerAstral >= 30 && BOAT && UseStarlord)
                            {
                                API.CastSpell(Starsurge);
                                return;
                            }
                            if (API.CanCast(Starsurge) && !SaveAP && IsLegendary == "Balance of all things" && API.PlayerAstral >= 80 && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 400 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 400) && UseStarlord)
                            {
                                API.CastSpell(Starsurge);
                                return;
                            }
                            if (API.CanCast(Starsurge) && !SaveAP && IsLegendary == "Balance of all things" && API.PlayerAstral >= 30 && API.PlayerAstral < 60 && API.TargetHasDebuff(Moonfire) && API.TargetHasDebuff(Sunfire) && (IsCelestialAlignment && API.CanCast(CelestialAlignment) && !TalentIncarnation || IsIncarnation && API.CanCast(Incarnation) && TalentIncarnation) && UseStarlord)
                            {
                                API.CastSpell(Starsurge);
                                return;
                            }
                        }
                        if (PlayerCovenantSettings != "Night Fae")
                        {
                            if (API.CanCast(Starsurge) && PlayerLevel >= 12 && !SaveAP && IsLegendary == "Balance of all things" && API.PlayerAstral >= 30 && BOAT && (IncaCelestial || (!IsCelestialAlignment && !TalentIncarnation || !IsIncarnation && TalentIncarnation) || API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                            {
                                API.CastSpell(Starsurge);
                                return;
                            }
                            if (API.CanCast(Starsurge) && PlayerLevel >= 12 && !SaveAP && IsLegendary == "Balance of all things" && API.PlayerAstral >= 70 && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 500 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 500) && (IncaCelestial || (!IsCelestialAlignment && !TalentIncarnation || !IsIncarnation && TalentIncarnation) || API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                            {
                                API.CastSpell(Starsurge);
                                return;
                            }
                        }
                        if (IsLegendary == "Balance of all things")
                        {
                            if (API.CanCast(Moonfire) && PlayerLevel >= 2 && (API.PlayerHasBuff(EclipseSolar) && API.PlayerBuffTimeRemaining(EclipseSolar) < 300 || API.PlayerHasBuff(EclipseLunar) && API.PlayerBuffTimeRemaining(EclipseLunar) < 400) && !DontDOT && API.TargetDebuffRemainingTime(Moonfire) < 700 && API.TargetTimeToDie >= 1350)
                            {
                                API.CastSpell(Moonfire);
                                return;
                            }
                            if (API.CanCast(Sunfire) && PlayerLevel >= 23 && (API.PlayerHasBuff(EclipseSolar) && API.PlayerBuffTimeRemaining(EclipseSolar) < 300 || API.PlayerHasBuff(EclipseLunar) && API.PlayerBuffTimeRemaining(EclipseLunar) < 400) && !DontDOT && API.TargetDebuffRemainingTime(Sunfire) < 600 && API.TargetTimeToDie >= 1600)
                            {
                                API.CastSpell(Sunfire);
                                return;
                            }
                            if (API.CanCast(StellarFlare) && TalentStellarFlare && (API.PlayerHasBuff(EclipseSolar) && API.PlayerBuffTimeRemaining(EclipseSolar) < 300 || API.PlayerHasBuff(EclipseLunar) && API.PlayerBuffTimeRemaining(EclipseLunar) < 400) && !API.PlayerIsMoving && SaveQuake && !DontDOT && API.TargetDebuffRemainingTime(StellarFlare) < 800 && API.TargetTimeToDie >= 1600 && !(API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347))
                            {
                                API.CastSpell(StellarFlare);
                                return;
                            }
                        }
                        if (API.CanCast(Moonfire) && PlayerLevel >= 2 && Eclipses && !DontDOT && API.TargetDebuffRemainingTime(Moonfire) < 300 && API.TargetTimeToDie >= 1350)
                        {
                            API.CastSpell(Moonfire);
                            return;
                        }
                        if (API.CanCast(Sunfire) && PlayerLevel >= 23 && Eclipses && !DontDOT && API.TargetDebuffRemainingTime(Sunfire) < 300 && API.TargetTimeToDie >= 1600)
                        {
                            API.CastSpell(Sunfire);
                            return;
                        }
                        if (API.CanCast(StellarFlare) && TalentStellarFlare && Eclipses && !API.PlayerIsMoving && SaveQuake && !DontDOT && API.TargetDebuffRemainingTime(StellarFlare) < 300 && API.TargetTimeToDie >= 1600 && !(API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347))
                        {
                            API.CastSpell(StellarFlare);
                            return;
                        }
                        if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0 && !DontDOT)
                        {
                            if (API.MouseoverDebuffRemainingTime(Moonfire) <= 300 && Eclipses && !API.MacroIsIgnored(Moonfire + "MO") && API.CanCast(Moonfire) && isMOinRange)
                            {
                                API.CastSpell(Moonfire + "MO");
                                return;
                            }
                            if (API.MouseoverDebuffRemainingTime(Sunfire) <= 300 && Eclipses && !API.MacroIsIgnored(Sunfire + "MO") && API.CanCast(Sunfire) && isMOinRange)
                            {
                                API.CastSpell(Sunfire + "MO");
                                return;
                            }
                            if (API.MouseoverDebuffRemainingTime(StellarFlare) <= 300 && Eclipses && !API.PlayerIsMoving && SaveQuake && !API.MacroIsIgnored(StellarFlare + "MO") && API.CanCast(StellarFlare) && TalentStellarFlare && isMOinRange && !(API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347))
                            {
                                API.CastSpell(StellarFlare + "MO");
                                return;
                            }
                        }
                        if (!incarcelerdy)
                        {
                            if (API.CanCast(Starfall) && PlayerHasBuff(OnethsPerception) && UseStarlord)
                            {
                                API.CastSpell(Starfall);
                                return;
                            }
                            if (API.CanCast(Starsurge) && PlayerHasBuff(OnethsClearVision) && UseStarlord)
                            {
                                API.CastSpell(Starsurge);
                                return;
                            }
                            if (API.CanCast(Starsurge) && PlayerLevel >= 12 && API.PlayerAstral > 30 && !SaveAP && (IsLegendary != "Balance of all things" || IncaCelestial) && (API.PlayerBuffTimeRemaining(EclipseSolar) >= 300 || API.PlayerBuffTimeRemaining(EclipseLunar) >= 300) && (IncaCelestial || (!IsCelestialAlignment && !TalentIncarnation || !IsIncarnation && TalentIncarnation) || API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                            {
                                API.CastSpell(Starsurge);
                                return;
                            }
                            if (API.CanCast(Moonfire) && PlayerLevel >= 2 && !DontDOT && API.PlayerIsMoving && movingwatch.ElapsedMilliseconds > 1000 && (!PlayerHasBuff(Starfall) && TalentStellarDrift || !TalentStellarDrift) && !PlayerHasBuff(WarriorofElune) && SpamDots)
                            {
                                API.CastSpell(Moonfire);
                                return;
                            }
                            if (API.CanCast(NewMoon) && TalentNewMoon && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(Starfall) && TalentNewMoon) && API.PlayerAstral <= 90)
                            {
                                API.CastSpell(NewMoon);
                                return;
                            }
                            //actions.boat+=/starfire,if=eclipse.in_lunar|eclipse.solar_next|eclipse.any_next|buff.warrior_of_elune.up&buff.eclipse_lunar.up|(buff.ca_inc.remains<action.wrath.execute_time&buff.ca_inc.up)
                            if (API.CanCast(Starfire) && SaveQuake && (PlayerHasBuff(EclipseLunar) && !IncaCelestial || SolarEclipseNext || AnyEclipseNext && !IncaCelestial && (IsCelestialAlignment && !TalentIncarnation && !API.CanCast(CelestialAlignment) || IsIncarnation && TalentIncarnation && !API.CanCast(Incarnation)) || PlayerHasBuff(WarriorofElune) && PlayerHasBuff(EclipseLunar) || (PlayerHasBuff(EclipseSolar) && API.PlayerBuffTimeRemaining(EclipseSolar) < 200 && PlayerHasBuff(EclipseLunar) && API.PlayerBuffTimeRemaining(EclipseLunar) < 200 && !Solarwatch.IsRunning && !Lunarwatch.IsRunning)))
                            {
                                API.CastSpell(Starfire);
                                return;
                            }
                            //actions.boat+=/wrath
                            if (API.CanCast(Wrath) && (!API.PlayerIsMoving || PlayerHasBuff(Starfall) && TalentStellarDrift) && SaveQuake)
                            {
                                API.CastSpell(Wrath);
                                return;
                            }
                        }
                    }
                    if (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE)
                    {
                        if (!incarcelerdy)
                        {
                            if (HardAoE)
                            {
                                if (PlayerCovenantSettings == "Night Fae" && IsLegendary == "Balance of all things")
                                {
                                    if (API.CanCast(Starfall) && !SaveAP && API.PlayerBuffTimeRemaining(Starfall) <= 100 && API.PlayerAstral >= 50 && UseStarlord)
                                    {
                                        API.CastSpell(Starfall);
                                        return;
                                    }
                                }
                                if (PlayerCovenantSettings != "Night Fae" || IsLegendary != "Balance of all things")
                                {
                                    if (API.CanCast(Starfall) && !SaveAP && API.PlayerBuffTimeRemaining(Starfall) < 100 && API.PlayerAstral >= 50 && (IncaCelestial || (!IsCelestialAlignment && !TalentIncarnation || !IsIncarnation && TalentIncarnation) || API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                                    {
                                        API.CastSpell(Starfall);
                                        return;
                                    }
                                }
                                if (API.CanCast(Starsurge) && !SaveAP && API.PlayerAstral >= 30 && API.TargetHasDebuff(Sunfire) && (IsCelestialAlignment && API.CanCast(CelestialAlignment) && !TalentIncarnation || IsIncarnation && API.CanCast(Incarnation) && TalentIncarnation) && UseStarlord)
                                {
                                    API.CastSpell(Starsurge);
                                    return;
                                }
                                if (API.CanCast(Sunfire) && PlayerLevel >= 23 && !DontDOT && API.TargetDebuffRemainingTime(Sunfire) < 300)
                                {
                                    API.CastSpell(Sunfire);
                                    return;
                                }
                                if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0 && !DontDOT)
                                {
                                    if (API.MouseoverDebuffRemainingTime(Sunfire) <= 300 && Eclipses && !API.MacroIsIgnored(Sunfire + "MO") && API.CanCast(Sunfire) && isMOinRange)
                                    {
                                        API.CastSpell(Sunfire + "MO");
                                        return;
                                    }
                                }
                                if (API.CanCast(Wrath) && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(Starfall) && TalentStellarDrift) && (LunarEclipseNext || AnyEclipseNext && !IncaCelestial && (IsCelestialAlignment && !TalentIncarnation && !API.CanCast(CelestialAlignment) || IsIncarnation && TalentIncarnation && !API.CanCast(Incarnation)) || (PlayerHasBuff(EclipseSolar) && API.PlayerBuffTimeRemaining(EclipseSolar) < 200 && PlayerHasBuff(EclipseLunar) && API.PlayerBuffTimeRemaining(EclipseLunar) < 200 && !Solarwatch.IsRunning && !Lunarwatch.IsRunning)))
                                {
                                    API.CastSpell(Wrath);
                                    return;
                                }
                                if ((!API.PlayerIsMoving || PlayerHasBuff(Starfall) && TalentStellarDrift || PlayerHasBuff(WarriorofElune)) && SaveQuake)
                                {
                                    if (API.CanCast(Starfire))
                                    {
                                        API.CastSpell(Starfire);
                                        return;
                                    }
                                }
                            }
                            if (!HardAoE)
                            {
                                if (PlayerCovenantSettings == "Night Fae" && IsLegendary == "Balance of all things")
                                {
                                    if (API.CanCast(Starfall) && !SaveAP && API.PlayerBuffTimeRemaining(Starfall) <= 100 && API.PlayerAstral >= 50 && UseStarlord)
                                    {
                                        API.CastSpell(Starfall);
                                        return;
                                    }
                                    if (API.CanCast(Starsurge) && !SaveAP && (API.PlayerAstral >= 80 || API.PlayerAstral >= 50 && BOAT && API.PlayerBuffTimeRemaining(Starfall) >= 600 || API.PlayerAstral >= 30 && API.TargetHasDebuff(Moonfire) && API.TargetHasDebuff(Sunfire) && (IsCelestialAlignment && API.CanCast(CelestialAlignment) && !TalentIncarnation || IsIncarnation && API.CanCast(Incarnation) && TalentIncarnation)) && UseStarlord)
                                    {
                                        API.CastSpell(Starsurge);
                                        return;
                                    }
                                }
                                if (PlayerCovenantSettings != "Night Fae" || IsLegendary != "Balance of all things")
                                {
                                    if (API.CanCast(Starfall) && !SaveAP && API.PlayerBuffTimeRemaining(Starfall) < 100 && API.PlayerAstral >= 50 && (IncaCelestial || (!IsCelestialAlignment && !TalentIncarnation || !IsIncarnation && TalentIncarnation) || API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                                    {
                                        API.CastSpell(Starfall);
                                        return;
                                    }
                                    if (API.CanCast(Starsurge) && !SaveAP && API.PlayerAstral >= 80 && (IncaCelestial || (!IsCelestialAlignment && !TalentIncarnation || !IsIncarnation && TalentIncarnation) || API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                                    {
                                        API.CastSpell(Starsurge);
                                        return;
                                    }
                                }
                                if (API.CanCast(Sunfire) && PlayerLevel >= 23 && Eclipses && !DontDOT && API.TargetDebuffRemainingTime(Sunfire) < 300 && API.TargetTimeToDie >= 1600)
                                {
                                    API.CastSpell(Sunfire);
                                    return;
                                }
                                if (API.CanCast(Moonfire) && PlayerLevel >= 2 && Eclipses && !DontDOT && API.TargetDebuffRemainingTime(Moonfire) < 300 && API.TargetTimeToDie >= 1350)
                                {
                                    API.CastSpell(Moonfire);
                                    return;
                                }
                                if (API.CanCast(StellarFlare) && Eclipses && TalentStellarFlare && !API.PlayerIsMoving && SaveQuake && !DontDOT && API.TargetDebuffRemainingTime(StellarFlare) < 300 && API.TargetTimeToDie >= 1600 && !(API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347))
                                {
                                    API.CastSpell(StellarFlare);
                                    return;
                                }
                                if (API.CanCast(Starfall) && !PlayerHasBuff(Starfall) && PlayerHasBuff(OnethsPerception) && UseStarlord)
                                {
                                    API.CastSpell(Starfall);
                                    return;
                                }
                                if (API.CanCast(Starsurge) && PlayerHasBuff(OnethsClearVision) && UseStarlord)
                                {
                                    API.CastSpell(Starsurge);
                                    return;
                                }
                                if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0 && !DontDOT)
                                {
                                    if (API.MouseoverDebuffRemainingTime(Sunfire) <= 300 && Eclipses && !API.MacroIsIgnored(Sunfire + "MO") && API.CanCast(Sunfire) && isMOinRange)
                                    {
                                        API.CastSpell(Sunfire + "MO");
                                        return;
                                    }
                                    if (API.MouseoverDebuffRemainingTime(Moonfire) <= 300 && Eclipses && PlayerHasBuff(EclipseSolar) && !API.MacroIsIgnored(Moonfire + "MO") && API.CanCast(Moonfire) && isMOinRange)
                                    {
                                        API.CastSpell(Moonfire + "MO");
                                        return;
                                    }
                                    if (API.MouseoverDebuffRemainingTime(StellarFlare) <= 300 && Eclipses && !API.PlayerIsMoving && API.PlayerUnitInMeleeRangeCount < 4 && SaveQuake && !API.MacroIsIgnored(StellarFlare + "MO") && API.CanCast(StellarFlare) && TalentStellarFlare && isMOinRange && !(API.LastSpellCastInGame == StellarFlare || API.PlayerCurrentCastSpellID == 202347))
                                    {
                                        API.CastSpell(StellarFlare + "MO");
                                        return;
                                    }
                                }
                                if (API.CanCast(Sunfire) && PlayerLevel >= 23 && !DontDOT && API.PlayerIsMoving && movingwatch.ElapsedMilliseconds > 1000 && (!PlayerHasBuff(Starfall) && TalentStellarDrift || !TalentStellarDrift) && !PlayerHasBuff(WarriorofElune) && SpamDots)
                                {
                                    API.CastSpell(Sunfire);
                                    return;
                                }
                                if (API.CanCast(NewMoon) && SaveQuake && TalentNewMoon && (!API.PlayerIsMoving || PlayerHasBuff(Starfall) && TalentStellarDrift) && API.PlayerAstral <= 90)
                                {
                                    API.CastSpell(NewMoon);
                                    return;
                                }
                                //actions.aoe+=/wrath,if=(eclipse.lunar_next|eclipse.any_next&variable.is_cleave)&(target.time_to_die>4|eclipse.lunar_in_2|fight_remains<10)|buff.eclipse_solar.remains<action.starfire.execute_time&buff.eclipse_solar.up|eclipse.in_solar&!variable.starfire_in_solar|buff.ca_inc.remains<action.starfire.execute_time&!variable.is_cleave&buff.ca_inc.remains<execute_time&buff.ca_inc.up|buff.ravenous_frenzy.up&spell_haste>0.6&(spell_targets<=3|!talent.soul_of_the_forest.enabled)|!variable.is_cleave&buff.ca_inc.remains>execute_time
                                if (API.CanCast(Wrath) && SaveQuake && (!API.PlayerIsMoving || PlayerHasBuff(Starfall) && TalentStellarDrift) && (PlayerHasBuff(EclipseSolar) && !IncaCelestial || LunarEclipseNext || AnyEclipseNext && !IncaCelestial && (IsCelestialAlignment && !TalentIncarnation && !API.CanCast(CelestialAlignment) || IsIncarnation && TalentIncarnation && !API.CanCast(Incarnation)) || (PlayerHasBuff(EclipseSolar) && API.PlayerBuffTimeRemaining(EclipseSolar) < 200 && PlayerHasBuff(EclipseLunar) && API.PlayerBuffTimeRemaining(EclipseLunar) < 200 && !Solarwatch.IsRunning && !Lunarwatch.IsRunning)))
                                {
                                    API.CastSpell(Wrath);
                                    return;
                                }
                                //actions.aoe+=/starfire
                                if (API.CanCast(Starfire) && SaveQuake)
                                {
                                    API.CastSpell(Starfire);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
