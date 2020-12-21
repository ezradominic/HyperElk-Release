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

using System.Diagnostics;

namespace HyperElk.Core
{
    public class BalanceDruid : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
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

        //Talents
        bool TalentNatureBalance => API.PlayerIsTalentSelected(1, 1);
        bool TalentWarriorOfElune => API.PlayerIsTalentSelected(1, 2);
        bool TalentForceOfNature => API.PlayerIsTalentSelected(1, 3);
        bool TalentRenewal => API.PlayerIsTalentSelected(2, 2);
        bool TalentGuardianAffinity => API.PlayerIsTalentSelected(3, 2);
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
        private int PlayerLevel => API.PlayerLevel;
        private bool isinRange => API.TargetRange < 45;
        private bool isMOinRange => API.MouseoverRange < 45;
        private bool UseStarlord => (TalentStarlord && API.PlayerBuffTimeRemaining(Starlord) == 0 || TalentStarlord && API.PlayerBuffTimeRemaining(Starlord) > 400 && API.PlayerBuffTimeRemaining(Starlord) != 5000000 || !TalentStarlord);
        bool IsCovenant => (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE);
        bool IsIncarnation => (UseIncarnation == "with Cooldowns" && IsCooldowns || UseIncarnation == "always");
        bool IsCelestialAlignment => (UseCelestialAlignment == "with Cooldowns" && IsCooldowns || UseCelestialAlignment == "always");
        bool IsWarriorofElune => (UseWarriorofElune == "with Cooldowns" && IsCooldowns || UseWarriorofElune == "always" || UseWarriorofElune == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE);
        bool IsForceofNature => (UseForceofNature == "with Cooldowns" && IsCooldowns || UseForceofNature == "always" || UseForceofNature == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE);
        bool IsFuryofElune => (UseFuryofElune == "with Cooldowns" && IsCooldowns || UseFuryofElune == "always" || UseFuryofElune == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE);
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isinRange;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isinRange;

        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseIncarnation => CDUsage[CombatRoutine.GetPropertyInt(Incarnation)];
        private string UseCelestialAlignment => CDUsage[CombatRoutine.GetPropertyInt(CelestialAlignment)];
        private string UseWarriorofElune => CDUsageWithAOE[CombatRoutine.GetPropertyInt(WarriorofElune)];
        private string UseForceofNature => CDUsageWithAOE[CombatRoutine.GetPropertyInt(ForceofNature)];
        private string UseFuryofElune => CDUsageWithAOE[CombatRoutine.GetPropertyInt(FuryofElune)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        private bool SpamDots => CombatRoutine.GetPropertyBool("SpamDots");
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private int RegrowthLifePercent => numbList[CombatRoutine.GetPropertyInt(Regrowth)];
        private int BarkskinLifePercent => numbList[CombatRoutine.GetPropertyInt(Barkskin)];
        private int BearFormLifePercent => numbList[CombatRoutine.GetPropertyInt(BearForm)];
        private int RenewalLifePercent => numbList[CombatRoutine.GetPropertyInt(Renewal)];
        private int FrenziedRegenerationLifePercent => numbList[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => numbList[CombatRoutine.GetPropertyInt(Ironfur)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private bool IncaCelestial => (API.PlayerHasBuff(Incarnation) || API.PlayerHasBuff(CelestialAlignment));

        public override void Initialize()
        {
            CombatRoutine.Name = "Balance Druid by smartie";
            API.WriteLog("Welcome to smartie`s Balance Druid v2.1");
            API.WriteLog("Create the following mouseover macros and assigned to the bind:");
            API.WriteLog("MoonfireMO - /cast [@mouseover] Moonfire");
            API.WriteLog("SunfireMO - /cast [@mouseover] Sunfire");
            API.WriteLog("StellarFlareMO - /cast [@mouseover] Stellar Flare");

            //Spells
            CombatRoutine.AddSpell(Moonfire, 8921, "D1");
            CombatRoutine.AddSpell(Sunfire, 93402, "D2");
            CombatRoutine.AddSpell(StellarFlare, 202347, "D7");
            CombatRoutine.AddSpell(Starsurge, 78674, "D5");
            CombatRoutine.AddSpell(Starfall, 191034, "D6");
            CombatRoutine.AddSpell(Starfire, 194153, "D4");
            CombatRoutine.AddSpell(Wrath, 190984, "D3");
            CombatRoutine.AddSpell(NewMoon, 274281, "Oem6");
            CombatRoutine.AddSpell(HalfMoon,274282, "Oem6");
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
            CombatRoutine.AddSpell(RavenousFrenzy,323546, "D1");
            CombatRoutine.AddSpell(ConvoketheSpirits,323764, "D1");
            CombatRoutine.AddSpell(AdaptiveSwarm,325727, "D1");
            CombatRoutine.AddSpell(LoneEmpowerment, 338142, "D1");

            //Macros
            CombatRoutine.AddMacro(Moonfire+"MO", "NumPad7");
            CombatRoutine.AddMacro(Sunfire+"MO", "NumPad8");
            CombatRoutine.AddMacro(StellarFlare+"MO", "NumPad9");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

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


            //Debuff
            CombatRoutine.AddDebuff(Moonfire, 164812);
            CombatRoutine.AddDebuff(Sunfire, 164815);
            CombatRoutine.AddDebuff(StellarFlare, 202347);
            CombatRoutine.AddDebuff(Thrash, 192090);
            CombatRoutine.AddDebuff(AdaptiveSwarm, 325727);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Incarnation, "Use " + Incarnation, CDUsage, "Use " + Incarnation + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(CelestialAlignment, "Use " + CelestialAlignment, CDUsage, "Use " + CelestialAlignment + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(WarriorofElune, "Use " + WarriorofElune, CDUsageWithAOE, "Use " + WarriorofElune + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(ForceofNature, "Use " + ForceofNature, CDUsageWithAOE, "Use " + ForceofNature + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(FuryofElune, "Use " + FuryofElune, CDUsageWithAOE, "Use " + FuryofElune + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("SpamDots", "SpamDots", true, "Will spam Dots while moving", "Generic");
            CombatRoutine.AddProp("AutoForm", "AutoForm", true, "Will auto switch forms", "Generic");
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
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
            if (!Lunarwatch.IsRunning && API.PlayerHasBuff(EclipseLunar) && !API.PlayerHasBuff(EclipseSolar))
            {
                Solarwatch.Stop();
                Solarwatch.Reset();
                Lunarwatch.Start();
                API.WriteLog("Starting Lunarwatch.");
            }
            if (!Solarwatch.IsRunning && API.PlayerHasBuff(EclipseSolar) && !API.PlayerHasBuff(EclipseLunar))
            {
                Lunarwatch.Stop();
                Lunarwatch.Reset();
                Solarwatch.Start();
                API.WriteLog("Starting Solarwatch.");
            }
            if (API.PlayerHasBuff(EclipseLunar) && API.PlayerHasBuff(EclipseSolar))
            {
                Lunarwatch.Stop();
                Lunarwatch.Reset();
                Solarwatch.Stop();
                Solarwatch.Reset();
            }
            if (!API.PlayerIsInCombat)
            {
                Lunarwatch.Stop();
                Lunarwatch.Reset();
                Solarwatch.Stop();
                Solarwatch.Reset();
            }
        }
        public override void CombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40)
                return;
            if (!API.PlayerIsMounted && !API.PlayerHasBuff(TravelForm))
            {
                if (isInterrupt && API.CanCast(SolarBeam) && PlayerLevel >= 26 && isinRange)
                {
                    API.CastSpell(SolarBeam);
                    return;
                }
                if (API.CanCast(RacialSpell1) && isInterrupt && PlayerRaceSettings == "Tauren" && !API.PlayerIsMoving && isRacial && API.TargetRange < 8 && API.SpellISOnCooldown(SolarBeam))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.CanCast(MightyBash) && isInterrupt && TalentMightyBash && API.TargetRange < 10 && API.SpellISOnCooldown(SolarBeam))
                {
                    API.CastSpell(MightyBash);
                    return;
                }
                if (API.PlayerHealthPercent <= RegrowthLifePercent && PlayerLevel >= 3 && API.CanCast(Regrowth))
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
                if (API.PlayerItemCanUse("Healthstone") && API.PlayerItemRemainingCD("Healthstone") == 0 && API.PlayerHealthPercent <= HealthStonePercent)
                {
                    API.CastSpell("Healthstone");
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
                if (API.PlayerHealthPercent <= FrenziedRegenerationLifePercent && API.PlayerRage >= 10 && API.CanCast(FrenziedRegeneration) && API.PlayerHasBuff(BearForm) && TalentGuardianAffinity)
                {
                    API.CastSpell(FrenziedRegeneration);
                    return;
                }
                if (API.PlayerHealthPercent <= IronfurLifePercent && API.PlayerRage >= 40 && API.CanCast(Ironfur) && TalentGuardianAffinity && API.PlayerHasBuff(BearForm))
                {
                    API.CastSpell(Ironfur);
                    return;
                }
                if (API.PlayerHealthPercent <= BearFormLifePercent && PlayerLevel >= 8 && AutoForm && API.CanCast(BearForm) && !API.PlayerHasBuff(BearForm))
                {
                    API.CastSpell(BearForm);
                    return;
                }
                if (API.PlayerHealthPercent > BearFormLifePercent && BearFormLifePercent != 0 && API.CanCast(MoonkinForm) && API.PlayerHasBuff(BearForm) && AutoForm)
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
            if (API.PlayerCurrentCastTimeRemaining > 40)
                return;
            if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !API.PlayerHasBuff(TravelForm))
            {
                API.CastSpell(TravelForm);
                return;
            }
        }
        private void rotation()
        {
            if (API.CanCast(MoonkinForm) && PlayerLevel >= 21 && !API.PlayerHasBuff(MoonkinForm) && !API.PlayerHasBuff(BearForm) && !API.PlayerHasBuff(CatForm) && !API.PlayerHasBuff(Soulshape))
            {
                API.CastSpell(MoonkinForm);
                return;
            }
            if (API.PlayerHasBuff(BearForm))
            {
                if (API.CanCast(Thrash) && API.TargetRange < 9 && TalentGuardianAffinity && PlayerLevel >= 11 && !API.TargetHasDebuff(Thrash))
                {
                    API.CastSpell(Thrash);
                    return;
                }
                if (API.CanCast(Mangle) && API.TargetRange < 6 && PlayerLevel >= 8 && (API.TargetHasDebuff(Thrash) || !TalentGuardianAffinity))
                {
                    API.CastSpell(Mangle);
                    return;
                }
            }
            if (isinRange && (API.PlayerHasBuff(MoonkinForm) || PlayerLevel < 21))
            {
                //actions+=/berserking,if=(!covenant.night_fae|!cooldown.convoke_the_spirits.up)&buff.ca_inc.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && isRacial && isinRange && (PlayerCovenantSettings != "Night Fae" || API.CanCast(ConvoketheSpirits)) && IncaCelestial)
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
                if (API.CanCast(WarriorofElune) && !API.PlayerHasBuff(WarriorofElune) && TalentWarriorOfElune && IsWarriorofElune)
                {
                    API.CastSpell(WarriorofElune);
                    return;
                }
                if (API.CanCast(Incarnation) && API.PlayerAstral >= 90 && !IncaCelestial && IsIncarnation && API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || !TalentStellarFlare) && TalentIncarnation)
                {
                    API.CastSpell(Incarnation);
                    return;
                }
                if (API.CanCast(CelestialAlignment) && PlayerLevel >= 39 && !IncaCelestial && IsCelestialAlignment && API.PlayerAstral >= 90 && API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || !TalentStellarFlare) && !TalentIncarnation)
                {
                    API.CastSpell(CelestialAlignment);
                    return;
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
                if (API.CanCast(ConvoketheSpirits) && isinRange && !API.PlayerIsMoving && PlayerCovenantSettings == "Night Fae" && IsCovenant && ((!API.CanCast(CelestialAlignment) && !TalentIncarnation && IsCelestialAlignment || !IsCelestialAlignment && !TalentIncarnation) || (!API.CanCast(Incarnation) && TalentIncarnation && IsIncarnation || !IsIncarnation && TalentIncarnation) || IncaCelestial) && API.PlayerAstral < 50 && (API.PlayerBuffTimeRemaining(EclipseSolar) > 1000 || API.PlayerBuffTimeRemaining(EclipseLunar) > 1000))
                {
                    API.CastSpell(ConvoketheSpirits);
                    return;
                }
                if (API.CanCast(LoneEmpowerment) && isinRange && API.PlayerHasBuff(LoneSpirit) && PlayerCovenantSettings == "Kyrian" && IsCovenant && (IncaCelestial || TalentIncarnation && API.SpellCDDuration(Incarnation) > 5000 || !TalentIncarnation && API.SpellCDDuration(CelestialAlignment) > 5000 || !IsIncarnation && TalentIncarnation || !IsCelestialAlignment && !TalentIncarnation))
                {
                    API.CastSpell(LoneEmpowerment);
                    return;
                }
                if (API.CanCast(AdaptiveSwarm) && isinRange && PlayerCovenantSettings == "Necrolord" && IsCovenant && !API.TargetHasDebuff(AdaptiveSwarm))
                {
                    API.CastSpell(AdaptiveSwarm);
                    return;
                }
                // Single Target rota
                if (API.TargetUnitInRangeCount < AOEUnitNumber || !IsAOE)
                {
                    if (API.CanCast(Moonfire) && PlayerLevel >= 2 && API.TargetDebuffRemainingTime(Moonfire) < 300)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Moonfire) && PlayerLevel >= 2 && API.PlayerIsMoving && API.TargetDebuffRemainingTime(Sunfire) >= 300 && (!API.PlayerHasBuff(Starfall) && TalentStellarDrift || !TalentStellarDrift) && !API.PlayerHasBuff(WarriorofElune) && API.PlayerAstral <= 40 && SpamDots)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Sunfire) && PlayerLevel >= 23 && API.TargetDebuffRemainingTime(Sunfire) < 300)
                    {
                        API.CastSpell(Sunfire);
                        return;
                    }
                    if (API.CanCast(StellarFlare) && TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) < 300 && API.LastSpellCastInGame != (StellarFlare))
                    {
                        API.CastSpell(StellarFlare);
                        return;
                    }
                    if ( API.CanCast(Starfall) && API.PlayerHasBuff(OnethsPerception) && UseStarlord)
                    {
                        API.CastSpell(Starfall);
                        return;
                    }
                    if (API.CanCast(Starsurge) && API.PlayerHasBuff(OnethsClearVision) && UseStarlord)
                    {
                        API.CastSpell(Starsurge);
                        return;
                    }
                    if (API.CanCast(Starsurge) && PlayerLevel >= 12 && (API.PlayerAstral > 30 && IncaCelestial || API.PlayerAstral > 30 && (!IsCelestialAlignment && !TalentIncarnation && API.PlayerAstral > 30 || !IsIncarnation && TalentIncarnation && API.PlayerAstral > 30) || API.PlayerAstral > 30 && API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.PlayerAstral > 30 && API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                    {
                        API.CastSpell(Starsurge);
                        return;
                    }
                    if (API.CanCast(NewMoon) && TalentNewMoon && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentNewMoon) && API.PlayerAstral <= 90)
                    {
                        API.CastSpell(NewMoon);
                        return;
                    }
                    if (API.CanCast(Starfire) && PlayerLevel >= 10 && API.PlayerHasBuff(WarriorofElune))
                    {
                        API.CastSpell(Starfire);
                        return;
                    }
                    if (API.CanCast(Wrath) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && IncaCelestial)
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                    if (API.CanCast(Starfire) && PlayerLevel >= 10 && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && Lunarwatch.IsRunning && !API.PlayerHasBuff(EclipseSolar) && !API.PlayerHasBuff(EclipseLunar))
                    {
                        API.CastSpell(Starfire);
                        return;
                    }
                    if (API.CanCast(Wrath) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && Solarwatch.IsRunning && !API.PlayerHasBuff(EclipseSolar) && !API.PlayerHasBuff(EclipseLunar))
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                    if (API.CanCast(Starfire) && PlayerLevel >= 10 && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && API.PlayerHasBuff(EclipseLunar))
                    {
                        API.CastSpell(Starfire);
                        return;
                    }
                    if (API.CanCast(Wrath) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift))
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                }
                //AoE Rota
                if (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE)
                {
                    if (API.CanCast(Sunfire) && PlayerLevel >= 23 && API.TargetDebuffRemainingTime(Sunfire) < 300)
                    {
                        API.CastSpell(Sunfire);
                        return;
                    }
                    if (API.CanCast(Moonfire) && PlayerLevel >= 2 && API.TargetDebuffRemainingTime(Moonfire) < 300)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Sunfire) && PlayerLevel >= 23 && API.PlayerIsMoving && API.TargetDebuffRemainingTime(Moonfire) >= 300 && (!API.PlayerHasBuff(Starfall) && TalentStellarDrift || !TalentStellarDrift) && !API.PlayerHasBuff(WarriorofElune) && SpamDots)
                    {
                        API.CastSpell(Sunfire);
                        return;
                    }
                    if (API.CanCast(StellarFlare) && TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) < 300 && API.LastSpellCastInGame != (StellarFlare))
                    {
                        API.CastSpell(StellarFlare);
                        return;
                    }
                    if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                    {
                        if (API.MouseoverDebuffRemainingTime(Sunfire) <= 300 && !API.MacroIsIgnored(Sunfire + "MO") && API.CanCast(Sunfire) && isMOinRange)
                        {
                            API.CastSpell(Sunfire + "MO");
                            return;
                        }
                        if (API.MouseoverDebuffRemainingTime(Moonfire) <= 300 && !API.MacroIsIgnored(Moonfire + "MO") && API.CanCast(Moonfire) && isMOinRange)
                        {
                            API.CastSpell(Moonfire + "MO");
                            return;
                        }
                        if (API.MouseoverDebuffRemainingTime(StellarFlare) <= 300 && !API.MacroIsIgnored(StellarFlare + "MO") && API.CanCast(StellarFlare) && TalentStellarFlare && isMOinRange)
                        {
                            API.CastSpell(StellarFlare + "MO");
                            return;
                        }
                    }
                    if (API.CanCast(Starfall) && API.PlayerHasBuff(OnethsPerception) && UseStarlord)
                    {
                        API.CastSpell(Starfall);
                        return;
                    }
                    if (API.CanCast(Starsurge) && API.PlayerHasBuff(OnethsClearVision) && UseStarlord)
                    {
                        API.CastSpell(Starsurge);
                        return;
                    }
                    if (API.CanCast(Starfall) && API.PlayerBuffTimeRemaining(Starfall) < 100 && PlayerLevel >= 34 && (API.PlayerAstral > 50 && IncaCelestial || API.PlayerAstral > 50 && (!IsCelestialAlignment && !TalentIncarnation && API.PlayerAstral > 40 || !IsIncarnation && TalentIncarnation && API.PlayerAstral > 40) || API.PlayerAstral > 50 && API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.PlayerAstral > 50 && API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                    {
                        API.CastSpell(Starfall);
                        return;
                    }
                    if (API.CanCast(Starsurge) && (API.PlayerBuffTimeRemaining(Starfall) > 400 || PlayerLevel < 34) && PlayerLevel >= 12 && (API.PlayerAstral > 30 && IncaCelestial || API.PlayerAstral > 30 && (!IsCelestialAlignment && !TalentIncarnation && API.PlayerAstral > 30 || !IsIncarnation && TalentIncarnation && API.PlayerAstral > 30) || API.PlayerAstral > 30 && API.SpellCDDuration(Incarnation) > 500 && TalentIncarnation && IsIncarnation || API.PlayerAstral > 30 && API.SpellCDDuration(CelestialAlignment) > 500 && !TalentIncarnation && IsCelestialAlignment) && UseStarlord)
                    {
                        API.CastSpell(Starsurge);
                        return;
                    }
                    if (API.CanCast(NewMoon) && TalentNewMoon && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && API.PlayerAstral <= 90)
                    {
                        API.CastSpell(NewMoon);
                        return;
                    }
                    if (API.CanCast(Starfire) && PlayerLevel >= 10 && API.PlayerHasBuff(WarriorofElune))
                    {
                        API.CastSpell(Starfire);
                        return;
                    }
                    if (API.CanCast(Starfire) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && IncaCelestial)
                    {
                        API.CastSpell(Starfire);
                        return;
                    }
                    if (API.CanCast(Wrath) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && Solarwatch.IsRunning && !API.PlayerHasBuff(EclipseSolar) && !API.PlayerHasBuff(EclipseLunar))
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                    if (API.CanCast(Starfire) && PlayerLevel >= 10 && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && Lunarwatch.IsRunning && !API.PlayerHasBuff(EclipseSolar) && !API.PlayerHasBuff(EclipseLunar))
                    {
                        API.CastSpell(Starfire);
                        return;
                    }
                    if (API.CanCast(Wrath) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && API.PlayerHasBuff(EclipseSolar))
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                    if (API.CanCast(Starfire) && PlayerLevel >= 10 && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift))
                    {
                        API.CastSpell(Starfire);
                        return;
                    }
                }
            }
        }
    }
}