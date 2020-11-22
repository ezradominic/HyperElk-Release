// Changelog
// v1.0 First release
// v1.1 Starlord and Stellar Drift fixes

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
        private string EclipseSolar = "48517";
        private string EclipseLunar = "48518";
        private string TravelForm = "Travel Form";
        private string CatForm = "Cat Form";
        private string Starlord = "Starlord";

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

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool isinRange => API.TargetRange < 45;
        private bool isMOinRange => API.MouseoverRange < 45;
        private bool UseStarlord => (TalentStarlord && API.PlayerBuffTimeRemaining(Starlord) == 0 || TalentStarlord && API.PlayerBuffTimeRemaining(Starlord) > 400 && API.PlayerBuffTimeRemaining(Starlord) != 5000000 || !TalentStarlord);

        //CBProperties
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        private bool SpamDots => CombatRoutine.GetPropertyBool("SpamDots");
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private int RegrowthLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Regrowth)];
        private int BarkskinLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Barkskin)];
        private int BearFormLifePercent => percentListProp[CombatRoutine.GetPropertyInt(BearForm)];
        private int RenewalLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Renewal)];
        private int FrenziedRegenerationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Ironfur)];
        private bool IncaCelestial => (API.PlayerHasBuff(Incarnation) || API.PlayerHasBuff(CelestialAlignment));

        public override void Initialize()
        {
            CombatRoutine.Name = "Balance Druid v1.1 by smartie";
            API.WriteLog("Welcome to smartie`s Balance Druid v1.1");
            API.WriteLog("Create the following mouseover macros and assigned to the bind:");
            API.WriteLog("MoonfireMO - /cast [@mouseover] Moonfire");
            API.WriteLog("SunfireMO - /cast [@mouseover] Sunfire");
            API.WriteLog("StellarFlareMO - /cast [@mouseover] Stellar Flare");

            //Spells
            CombatRoutine.AddSpell(Moonfire, "D1");
            CombatRoutine.AddSpell(Sunfire, "D2");
            CombatRoutine.AddSpell(StellarFlare, "D7");
            CombatRoutine.AddSpell(Starsurge, "D5");
            CombatRoutine.AddSpell(Starfall, "D6");
            CombatRoutine.AddSpell(Starfire, "D4");
            CombatRoutine.AddSpell(Wrath, "D3");
            CombatRoutine.AddSpell(NewMoon, "Oem6");
            CombatRoutine.AddSpell(HalfMoon, "Oem6");
            CombatRoutine.AddSpell(FullMoon, "Oem6");
            CombatRoutine.AddSpell(Incarnation, "D8");
            CombatRoutine.AddSpell(CelestialAlignment, "D8");
            CombatRoutine.AddSpell(MoonkinForm, "NumPad1");
            CombatRoutine.AddSpell(FuryofElune, "NumPad4");
            CombatRoutine.AddSpell(WarriorofElune, "NumPad2");
            CombatRoutine.AddSpell(ForceofNature, "NumPad3");
            CombatRoutine.AddSpell(Renewal, "F1");
            CombatRoutine.AddSpell(Regrowth, "F6");
            CombatRoutine.AddSpell(Barkskin, "F1");
            CombatRoutine.AddSpell(SolarBeam, "F12");
            CombatRoutine.AddSpell(Typhoon, "D0");
            CombatRoutine.AddSpell(BearForm, "NumPad5");
            CombatRoutine.AddSpell(Thrash, "D4");
            CombatRoutine.AddSpell(Mangle, "D3");
            CombatRoutine.AddSpell(FrenziedRegeneration, "D6");
            CombatRoutine.AddSpell(Ironfur, "D5");
            CombatRoutine.AddSpell(TravelForm, "NumPad6");

            //Macros
            CombatRoutine.AddMacro(Moonfire+"MO", "NumPad7");
            CombatRoutine.AddMacro(Sunfire+"MO", "NumPad8");
            CombatRoutine.AddMacro(StellarFlare+"MO", "NumPad9");

            //Buffs
            CombatRoutine.AddBuff(EclipseLunar);
            CombatRoutine.AddBuff(EclipseSolar);
            CombatRoutine.AddBuff(MoonkinForm);
            CombatRoutine.AddBuff(CelestialAlignment);
            CombatRoutine.AddBuff(Incarnation);
            CombatRoutine.AddBuff(Starfall);
            CombatRoutine.AddBuff(TravelForm);
            CombatRoutine.AddBuff(WarriorofElune);
            CombatRoutine.AddBuff(BearForm);
            CombatRoutine.AddBuff(CatForm);
            CombatRoutine.AddBuff(FrenziedRegeneration);
            CombatRoutine.AddBuff(Starlord);

            //Debuff
            CombatRoutine.AddDebuff(Moonfire);
            CombatRoutine.AddDebuff(Sunfire);
            CombatRoutine.AddDebuff(StellarFlare);
            CombatRoutine.AddDebuff(Thrash);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");

            //Prop
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("SpamDots", "SpamDots", true, "Will spam Dots while moving", "Generic");
            CombatRoutine.AddProp("AutoForm", "AutoForm", true, "Will auto switch forms", "Generic");
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", percentListProp, "Life percent at which" + Renewal + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(Regrowth, Regrowth + " Life Percent", percentListProp, "Life percent at which" + Regrowth + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(Barkskin, Barkskin + " Life Percent", percentListProp, "Life percent at which" + Barkskin + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(BearForm, BearForm + " Life Percent", percentListProp, "Life percent at which rota will go into" + BearForm + "set to 0 to disable", "Defense", 1);
            CombatRoutine.AddProp(FrenziedRegeneration, FrenziedRegeneration + " Life Percent", percentListProp, "Life percent at which" + FrenziedRegeneration + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Ironfur, Ironfur + " Life Percent", percentListProp, "Life percent at which" + Ironfur + "is used, set to 0 to disable", "Defense", 9);
        }
        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if (API.PlayerIsCasting || API.PlayerIsChanneling)
                return;
            if (!API.PlayerIsMounted && !API.PlayerHasBuff(TravelForm))
            {
                if (isInterrupt && API.CanCast(SolarBeam) && PlayerLevel >= 26 && isinRange)
                {
                    API.CastSpell(SolarBeam);
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
                if (API.PlayerHealthPercent <= FrenziedRegenerationLifePercent && API.PlayerRage >= 10 && API.CanCast(FrenziedRegeneration) && API.PlayerHasBuff(BearForm) && TalentGuardianAffinity)
                {
                    API.CastSpell(FrenziedRegeneration);
                    return;
                }
                if (API.PlayerHealthPercent <= IronfurLifePercent && API.PlayerRage >= 40 && API.CanCast(Ironfur) && API.PlayerHasBuff(BearForm))
                {
                    API.CastSpell(Ironfur);
                    return;
                }
                if (API.PlayerHealthPercent <= BearFormLifePercent && PlayerLevel >= 8 && AutoForm && API.CanCast(BearForm) && !API.PlayerHasBuff(BearForm))
                {
                    API.CastSpell(BearForm);
                    return;
                }
                if (API.PlayerHealthPercent > BearFormLifePercent && API.CanCast(MoonkinForm) && API.PlayerHasBuff(BearForm) && AutoForm)
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
            if (API.PlayerIsCasting || API.PlayerIsChanneling)
                return;
            if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !API.PlayerHasBuff(TravelForm))
            {
                API.CastSpell(TravelForm);
                return;
            }
        }
        private void rotation()
        {
            if (API.CanCast(MoonkinForm) && PlayerLevel >= 21 && !API.PlayerHasBuff(MoonkinForm) && !API.PlayerHasBuff(BearForm) && !API.PlayerHasBuff(CatForm))
            {
                API.CastSpell(MoonkinForm);
                return;
            }
            if (API.PlayerHasBuff(BearForm))
            {
                if (API.CanCast(Thrash) && API.TargetRange < 9 && PlayerLevel >= 11 && !API.TargetHasDebuff(Thrash))
                {
                    API.CastSpell(Thrash);
                    return;
                }
                if (API.CanCast(Mangle) && API.TargetRange < 6 && PlayerLevel >= 8 && API.TargetHasDebuff(Thrash))
                {
                    API.CastSpell(Mangle);
                    return;
                }
            }
            if (isinRange && (API.PlayerHasBuff(MoonkinForm) || PlayerLevel < 21))
            {
                if (IsCooldowns)
                {
                    if (API.CanCast(WarriorofElune) && !API.PlayerHasBuff(WarriorofElune) && TalentWarriorOfElune)
                    {
                        API.CastSpell(WarriorofElune);
                        return;
                    }
                    if (API.CanCast(Incarnation) && API.PlayerAstral >= 90 && !IncaCelestial && API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || !TalentStellarFlare) && TalentIncarnation)
                    {
                        API.CastSpell(Incarnation);
                        return;
                    }
                    if (API.CanCast(CelestialAlignment) && PlayerLevel >= 39 && !IncaCelestial && API.PlayerAstral >= 90 && API.TargetDebuffRemainingTime(Moonfire) > 300 && API.TargetDebuffRemainingTime(Sunfire) > 300 && (TalentStellarFlare && API.TargetDebuffRemainingTime(StellarFlare) > 300 || !TalentStellarFlare) && !TalentIncarnation)
                    {
                        API.CastSpell(CelestialAlignment);
                        return;
                    }
                    if (API.CanCast(FuryofElune) && TalentFuryOfElune)
                    {
                        API.CastSpell(FuryofElune);
                        return;
                    }
                    if (API.CanCast(ForceofNature) && TalentForceOfNature && (IncaCelestial || (API.SpellCDDuration(Incarnation) > 3000 && TalentIncarnation || API.SpellCDDuration(CelestialAlignment) > 3000 && !TalentIncarnation)))
                    {
                        API.CastSpell(ForceofNature);
                        return;
                    }
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
                    if (API.CanCast(Starsurge) && PlayerLevel >= 12 && (API.PlayerAstral > 40 && IncaCelestial || API.PlayerAstral > 40 && !IsCooldowns|| API.PlayerAstral > 40 && API.SpellISOnCooldown(Incarnation) && TalentIncarnation || API.PlayerAstral > 40 && API.SpellISOnCooldown(CelestialAlignment) && !TalentIncarnation) && UseStarlord)
                    {
                        API.CastSpell(Starsurge);
                        return;
                    }
                    if (API.CanCast(Wrath) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && API.PlayerHasBuff(EclipseSolar))
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                    if (API.CanCast(Starfire) && PlayerLevel >= 10 && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && API.PlayerHasBuff(EclipseLunar))
                    {
                        API.CastSpell(Starfire);
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
                    if (API.CanCast(Wrath) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift))
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                }
                //AoE Rota
                if (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE)
                {
                    if (API.CanCast(Moonfire) && PlayerLevel >= 2 && API.TargetDebuffRemainingTime(Moonfire) < 300)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Sunfire) && PlayerLevel >= 23 && API.TargetDebuffRemainingTime(Sunfire) < 300)
                    {
                        API.CastSpell(Sunfire);
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
                        if (API.MouseoverDebuffRemainingTime(Moonfire) <= 300 && API.CanCast(Moonfire) && isMOinRange)
                        {
                            API.CastSpell(Moonfire + "MO");
                            return;
                        }
                        if (API.MouseoverDebuffRemainingTime(Sunfire) <= 300 && API.CanCast(Sunfire) && isMOinRange)
                        {
                            API.CastSpell(Sunfire + "MO");
                            return;
                        }
                        if (API.MouseoverDebuffRemainingTime(StellarFlare) <= 300 && API.CanCast(StellarFlare) && TalentStellarFlare && isMOinRange)
                        {
                            API.CastSpell(StellarFlare + "MO");
                            return;
                        }
                    }
                    if (API.CanCast(Starsurge) && PlayerLevel >= 12 && PlayerLevel < 34 && (API.PlayerAstral > 40 && IncaCelestial || API.PlayerAstral > 40 && !IsCooldowns || API.PlayerAstral > 40 && API.SpellISOnCooldown(Incarnation) && TalentIncarnation || API.PlayerAstral > 40 && API.SpellISOnCooldown(CelestialAlignment) && !TalentIncarnation) && UseStarlord)
                    {
                        API.CastSpell(Starsurge);
                        return;
                    }
                    if (API.CanCast(Starfall) && API.PlayerBuffTimeRemaining(Starfall) < 100 && PlayerLevel >= 34 && (API.PlayerAstral > 50 && IncaCelestial || API.PlayerAstral > 50 && !IsCooldowns || API.PlayerAstral > 50 && API.SpellISOnCooldown(Incarnation) && TalentIncarnation || API.PlayerAstral > 50 && API.SpellISOnCooldown(CelestialAlignment) && !TalentIncarnation) && UseStarlord)
                    {
                        API.CastSpell(Starfall);
                        return;
                    }
                    if (API.CanCast(Starsurge) && API.PlayerBuffTimeRemaining(Starfall) > 400 && PlayerLevel >= 12 && (API.PlayerAstral > 40 && IncaCelestial || API.PlayerAstral > 40 && !IsCooldowns || API.PlayerAstral > 40 && API.SpellISOnCooldown(Incarnation) && TalentIncarnation || API.PlayerAstral > 40 && API.SpellISOnCooldown(CelestialAlignment) && !TalentIncarnation) && UseStarlord)
                    {
                        API.CastSpell(Starsurge);
                        return;
                    }
                    if (API.CanCast(Wrath) && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && API.PlayerHasBuff(EclipseSolar))
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                    if (API.CanCast(Starfire) && PlayerLevel >= 10 && (!API.PlayerIsMoving || API.PlayerHasBuff(Starfall) && TalentStellarDrift) && API.PlayerHasBuff(EclipseLunar))
                    {
                        API.CastSpell(Starfire);
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