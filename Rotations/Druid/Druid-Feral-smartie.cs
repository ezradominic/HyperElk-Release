// Changelog
// v1.0 First release
// v1.1 bearform fix

namespace HyperElk.Core
{
    public class FuryWarrior : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        //Spell,Auras
        private string CatForm = "Cat Form";
        private string Rip = "Rip";
        private string Rake = "Rake";
        private string Prowl = "Prowl";
        private string TigersFury = "Tiger's Fury";
        private string Shred = "Shred";
        private string Regrowth = "Regrowth";
        private string FerociousBite = "Ferocious Bite";
        private string SavageRoar = "Savage Roar";
        private string Thrash = "Thrash";
        private string Swipe = "Swipe";
        private string BrutalSlash = "Brutal Slash";
        private string Moonfire = "Moonfire";
        private string FeralFrenzy = "Feral Frenzy";
        private string Berserk = "Berserk";
        private string Incarnation = "Incarnation: King of the Jungle";
        private string SkullBash = "Skull Bash";
        private string SurvivalInstincts = "Survival Instincts";
        private string Renewal = "Renewal";
        private string StampedingRoar = "Stampeding Roar";
        private string Maim = "Maim";
        private string Typhoon = "Typhoon";
        private string PrimalWrath = "Primal Wrath";
        private string BearForm = "Bear Form";
        private string Mangle = "Mangle";
        private string FrenziedRegeneration = "Frenzied Regeneration";
        private string Ironfur = "Ironfur";
        private string PredatorySwiftness = "Predatory Swiftness";
        private string Bloodtalons = "Bloodtalons";
        private string Clearcasting = "Clearcasting";
        private string TravelForm = "Travel Form";
        private string ScentofBlood = "Scent of Blood";
        private string Shadowmeld = "Shadowmeld";

        //Talents
        bool TalentLunarInspiration => API.PlayerIsTalentSelected(1, 3);
        bool TalentPredator => API.PlayerIsTalentSelected(1, 1);
        bool TalentRenewal => API.PlayerIsTalentSelected(2, 2);
        bool TalentBalanceAffinity => API.PlayerIsTalentSelected(3, 1);
        bool TalentGuardianAffinity => API.PlayerIsTalentSelected(3, 2);
        bool TalentIncarnation => API.PlayerIsTalentSelected(5, 3);
        bool TalentSavageRoar => API.PlayerIsTalentSelected(5, 2);
        bool TalentScentofBlood => API.PlayerIsTalentSelected(6, 1);
        bool TalentBrutalSlash => API.PlayerIsTalentSelected(6, 2);
        bool TalentPrimalWrath => API.PlayerIsTalentSelected(6, 3);
        bool TalentBloodtalons => API.PlayerIsTalentSelected(7, 2);
        bool TalentFeralFrenzy => API.PlayerIsTalentSelected(7, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool isMelee
        {
            get
            {
                if (TalentBalanceAffinity && API.TargetRange < 9 || !TalentBalanceAffinity && API.TargetRange < 6)
                    return true;
                return false;
            }
        }
        private bool isMOMelee
        {
            get
            {
                if (TalentBalanceAffinity && API.MouseoverRange < 9 || !TalentBalanceAffinity && API.MouseoverRange < 6)
                    return true;
                return false;
            }
        }
        private bool isThrashMelee
        {
            get
            {
                if (TalentBalanceAffinity && API.TargetRange < 12 || !TalentBalanceAffinity && API.TargetRange < 9)
                    return true;
                return false;
            }
        }
        private bool isMOThrashMelee
        {
            get
            {
                if (TalentBalanceAffinity && API.MouseoverRange < 12 || !TalentBalanceAffinity && API.MouseoverRange < 9)
                    return true;
                return false;
            }
        }
        private bool isKickRange
        {
            get
            {
                if (TalentBalanceAffinity && API.TargetRange < 17 || !TalentBalanceAffinity && API.TargetRange < 14)
                    return true;
                return false;
            }
        }
        private bool IncaBerserk
        {
            get
            {
                if (API.PlayerHasBuff(Incarnation) || API.PlayerHasBuff(Berserk))
                    return true;
                return false;
            }
        }
        //CBProperties
		public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool ProwlOOC => CombatRoutine.GetPropertyBool("ProwlOOC");
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private int RegrowthLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Regrowth)];
        private int RenewalLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Renewal)];
        private int SurvivalInstinctsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(SurvivalInstincts)];
        private int FrenziedRegenerationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Ironfur)];
        private int BearFormLifePercent => percentListProp[CombatRoutine.GetPropertyInt(BearForm)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Feral Druid v1.1 by smartie";
            API.WriteLog("Welcome to smartie`s Feral Druid v1.1");
            API.WriteLog("Create the following mouseover macros and assigned to the bind:");
            API.WriteLog("RakeMO - /cast [@mouseover] Rake");
            API.WriteLog("ThrashMO - /cast [@mouseover] Thrash");
            API.WriteLog("RipMO - /cast [@mouseover] Rip");

            //Spells
            CombatRoutine.AddSpell(CatForm, "NumPad1");
            CombatRoutine.AddSpell(Rip, "D6");
            CombatRoutine.AddSpell(Rake, "D2");
            CombatRoutine.AddSpell(Prowl, "NumPad3");
            CombatRoutine.AddSpell(TigersFury, "D1");
            CombatRoutine.AddSpell(Shred, "D3");
            CombatRoutine.AddSpell(Regrowth, "F2");
            CombatRoutine.AddSpell(FerociousBite, "D7");
            CombatRoutine.AddSpell(SavageRoar, "D9");
            CombatRoutine.AddSpell(Thrash, "D4");
            CombatRoutine.AddSpell(Swipe, "D5");
            CombatRoutine.AddSpell(BrutalSlash, "D5");
            CombatRoutine.AddSpell(Moonfire, "NumPad2");
            CombatRoutine.AddSpell(FeralFrenzy, "NumPad4");
            CombatRoutine.AddSpell(Berserk, "D1");
            CombatRoutine.AddSpell(Incarnation, "D1");
            CombatRoutine.AddSpell(SkullBash, "F12");
            CombatRoutine.AddSpell(SurvivalInstincts, "F1");
            CombatRoutine.AddSpell(Renewal, "F6");
            CombatRoutine.AddSpell(StampedingRoar, "F7");
            CombatRoutine.AddSpell(Maim, "F9");
            CombatRoutine.AddSpell(Typhoon, "D1");
            CombatRoutine.AddSpell(PrimalWrath, "D0");
            CombatRoutine.AddSpell(BearForm, "NumPad4");
            CombatRoutine.AddSpell(Mangle, "D3");
            CombatRoutine.AddSpell(FrenziedRegeneration, "D7");
            CombatRoutine.AddSpell(Ironfur, "D8");
			CombatRoutine.AddSpell(TravelForm, "NumPad6");

			//Macros
            CombatRoutine.AddMacro(Rake+"MO", "D2");
            CombatRoutine.AddMacro(Thrash+"MO", "D6");
            CombatRoutine.AddMacro(Rip+"MO", "D6");


            //Buffs
            CombatRoutine.AddBuff(Prowl);
            CombatRoutine.AddBuff(CatForm);
            CombatRoutine.AddBuff(BearForm);
            CombatRoutine.AddBuff(TigersFury);
            CombatRoutine.AddBuff(SavageRoar);
            CombatRoutine.AddBuff(PredatorySwiftness);
            CombatRoutine.AddBuff(Berserk);
            CombatRoutine.AddBuff(Bloodtalons);
            CombatRoutine.AddBuff(Clearcasting);
            CombatRoutine.AddBuff(SurvivalInstincts);
            CombatRoutine.AddBuff(TravelForm);
            CombatRoutine.AddBuff(Ironfur);
            CombatRoutine.AddBuff(FrenziedRegeneration);
            CombatRoutine.AddBuff(Incarnation);
            CombatRoutine.AddBuff(ScentofBlood);
            CombatRoutine.AddBuff(Shadowmeld);

            //Debuff
            CombatRoutine.AddDebuff(Rip);
            CombatRoutine.AddDebuff(Thrash);
            CombatRoutine.AddDebuff(Rake);
            CombatRoutine.AddDebuff(Moonfire);
			
			//Toggle
			CombatRoutine.AddToggle("Mouseover");

            //Prop
			AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("ProwlOOC", "ProwlOOC", true, "Use Prowl out of Combat", "Generic");
            CombatRoutine.AddProp("AutoForm", "AutoForm", true, "Will auto switch forms", "Generic");
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
            CombatRoutine.AddProp(Regrowth, Regrowth + " Life Percent", percentListProp, "Life percent at which" + Regrowth + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", percentListProp, "Life percent at which" + Renewal + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(SurvivalInstincts, SurvivalInstincts + " Life Percent", percentListProp, "Life percent at which" + SurvivalInstincts + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(FrenziedRegeneration, FrenziedRegeneration + " Life Percent", percentListProp, "Life percent at which" + FrenziedRegeneration + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Ironfur, Ironfur + " Life Percent", percentListProp, "Life percent at which" + Ironfur + "is used, set to 0 to disable", "Defense", 9);
            CombatRoutine.AddProp(BearForm, BearForm + " Life Percent", percentListProp, "Life percent at which rota will go into" + BearForm + "set to 0 to disable", "Defense", 1);
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
                if (isInterrupt && API.CanCast(SkullBash) && PlayerLevel >= 26 && isKickRange)
                {
                    API.CastSpell(SkullBash);
                    return;
                }
                if (API.PlayerHealthPercent <= RegrowthLifePercent && PlayerLevel >= 3 && API.CanCast(Regrowth) && API.PlayerHasBuff(PredatorySwiftness))
                {
                    API.CastSpell(Regrowth);
                    return;
                }
                if (API.CanCast(SurvivalInstincts) && PlayerLevel >= 32 && API.PlayerHealthPercent <= SurvivalInstinctsLifePercent && !API.PlayerHasBuff(SurvivalInstincts))
                {
                    API.CastSpell(SurvivalInstincts);
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
                if (API.PlayerHealthPercent > BearFormLifePercent && BearFormLifePercent != 0 && API.CanCast(CatForm) && API.PlayerHasBuff(BearForm) && AutoForm)
                {
                    API.CastSpell(CatForm);
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
            if (ProwlOOC && API.CanCast(Prowl) && PlayerLevel >= 17 && !API.PlayerHasBuff(Prowl) && !API.PlayerIsMounted && !API.PlayerHasBuff(TravelForm))
            {
                API.CastSpell(Prowl);
                return;
            }
            if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !API.PlayerHasBuff(TravelForm))
            {
                API.CastSpell(TravelForm);
                return;
            }
        }
        private void rotation()
        {
            if ((!API.PlayerHasBuff(CatForm) && PlayerLevel >= 5) && !API.PlayerHasBuff(BearForm) && AutoForm)
            {
                API.CastSpell(CatForm);
                return;
            }
            if (API.CanCast(Rake) && API.PlayerHasBuff(Prowl) && PlayerLevel >= 10 && (!IncaBerserk && API.PlayerEnergy >= 35 || IncaBerserk && API.PlayerEnergy >= 21))
            {
                API.CastSpell(Rake);
                return;
            }
            if (API.PlayerHasBuff(BearForm))
            {
                if (API.CanCast(Thrash) && isThrashMelee && PlayerLevel >= 11 && !API.TargetHasDebuff(Thrash))
                {
                    API.CastSpell(Thrash);
                    return;
                }
                if (API.CanCast(Mangle) && isMelee && PlayerLevel >= 8 && API.TargetHasDebuff(Thrash))
                {
                    API.CastSpell(Mangle);
                    return;
                }
                if (API.CanCast(Swipe) && isThrashMelee && PlayerLevel >= 42 && API.TargetHasDebuff(Thrash))
                {
                    API.CastSpell(Swipe);
                    return;
                }
            }
            if (PlayerLevel < 5)
            {
                API.WriteLog("Current Level:" + PlayerLevel);
                API.WriteLog("Rota will start working once you reach Level 5");
            }
            if (API.PlayerHasBuff(CatForm) && PlayerLevel >= 5)
            {
                if (API.CanCast(Rake) && PlayerLevel >= 10 && isMelee && (!IncaBerserk && API.PlayerEnergy >= 35 || IncaBerserk && API.PlayerEnergy >= 21) && API.PlayerHasBuff(Shadowmeld))
                {
                    API.CastSpell(Rake);
                    return;
                }
                //Cooldowns
                if (IsCooldowns)
                {
                    if (!TalentIncarnation && PlayerLevel >= 34 && API.PlayerEnergy >= 30 && API.CanCast(Berserk) && (API.PlayerHasBuff(TigersFury) || API.SpellCDDuration(TigersFury) > 500))
                    {
                        API.CastSpell(Berserk);
                        return;
                    }
                    if (TalentIncarnation && API.PlayerEnergy >= 30 && API.CanCast(Incarnation) && (API.PlayerHasBuff(TigersFury) || API.SpellCDDuration(TigersFury) > 1500))
                    {
                        API.CastSpell(Incarnation);
                        return;
                    }
                }
                if (API.CanCast(TigersFury) && PlayerLevel >= 12 && isMelee &&
                (!TalentPredator && (API.PlayerEnergy <= 40 || IncaBerserk) ||
                TalentPredator && (!API.PlayerHasBuff(TigersFury) || API.PlayerHasBuff(TigersFury) && API.PlayerEnergy <= 40)))
                {
                    API.CastSpell(TigersFury);
                    return;
                }
                if (isMelee && TalentFeralFrenzy && API.CanCast(FeralFrenzy) && API.PlayerComboPoints == 0)
                {
                    API.CastSpell(FeralFrenzy);
                    return;
                }
                // Single Target rota
                if (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE)
                {
                    //Finisher
                    if (API.PlayerComboPoints > 4)
                    {
                        if (isMelee && PlayerLevel >= 7 && API.CanCast(FerociousBite) && (!IncaBerserk && API.PlayerEnergy >= 50 || IncaBerserk && API.PlayerEnergy >= 25) && (API.TargetHasDebuff(Rip) && API.TargetDebuffRemainingTime(Rip) > 300))
                        {
                            API.CastSpell(FerociousBite);
                            return;
                        }
                        if (TalentSavageRoar && isMelee && API.CanCast(SavageRoar) && (!IncaBerserk && API.PlayerEnergy >= 30 || IncaBerserk && API.PlayerEnergy >= 18) && !API.PlayerHasBuff(SavageRoar))
                        {
                            API.CastSpell(SavageRoar);
                            return;
                        }
                        if (isMelee && PlayerLevel >= 21 && API.CanCast(Rip) && (!IncaBerserk && API.PlayerEnergy >= 20 || IncaBerserk && API.PlayerEnergy >= 12) && (!API.TargetHasDebuff(Rip) || API.TargetDebuffRemainingTime(Rip) <= 300))
                        {
                            API.CastSpell(Rip);
                            return;
                        }
                    }
                    //Generator
                    if (API.PlayerComboPoints < 5)
                    {
                        if (TalentBrutalSlash && API.TargetDebuffRemainingTime(Thrash) > 300 && !API.PlayerHasBuff(Prowl) && isThrashMelee && (API.TargetDebuffRemainingTime(Rake) > 360 || API.TargetHasDebuff(Rake) && API.PlayerHasBuff(Clearcasting)) && API.CanCast(BrutalSlash) && (!IncaBerserk && API.PlayerEnergy >= 25 || IncaBerserk && API.PlayerEnergy >= 15 || API.PlayerHasBuff(Clearcasting)))
                        {
                            API.CastSpell(BrutalSlash);
                            return;
                        }
                        if (API.CanCast(Rake) && PlayerLevel >= 10 && isMelee && (!IncaBerserk && API.PlayerEnergy >= 35 || IncaBerserk && API.PlayerEnergy >= 21) && API.TargetDebuffRemainingTime(Rake) <= 360)
                        {
                            API.CastSpell(Rake);
                            return;
                        }
                        if (API.CanCast(Moonfire) && !API.PlayerHasBuff(Prowl) && TalentLunarInspiration && API.LastSpellCastInGame != (Moonfire) && (!API.TargetHasDebuff(Moonfire) || API.TargetDebuffRemainingTime(Moonfire) <= 200) && API.TargetRange < 40 && (!IncaBerserk && API.PlayerEnergy >= 30 || IncaBerserk && API.PlayerEnergy >= 18))
                        {
                            API.CastSpell(Moonfire);
                            return;
                        }
                        if (isThrashMelee && PlayerLevel >= 11 && !API.PlayerHasBuff(Prowl) && API.CanCast(Thrash) && (!API.TargetHasDebuff(Thrash) || API.TargetDebuffRemainingTime(Thrash) <= 300) && (!IncaBerserk && API.PlayerEnergy >= 40 || IncaBerserk && API.PlayerEnergy >= 24 || API.PlayerHasBuff(Clearcasting)))
                        {
                            API.CastSpell(Thrash);
                            return;
                        }
                        if (isMelee && PlayerLevel >= 5 && API.TargetDebuffRemainingTime(Thrash) > 300 && !API.PlayerHasBuff(Prowl) && (API.TargetDebuffRemainingTime(Rake) > 360 || API.TargetHasDebuff(Rake) && API.PlayerHasBuff(Clearcasting)) && API.CanCast(Shred) && (!IncaBerserk && API.PlayerEnergy >= 40 || IncaBerserk && API.PlayerEnergy >= 24 || API.PlayerHasBuff(Clearcasting)))
                        {
                            API.CastSpell(Shred);
                            return;
                        }
                    }
                }
                if (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE)
                {
                    //Finisher
                    if (API.PlayerComboPoints > 4)
                    {
						if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                        {
                            if (API.MouseoverDebuffRemainingTime(Rip) <= 360 && API.CanCast(Rip) && !TalentPrimalWrath && isMOMelee)
                            {
                                API.CastSpell(Rip + "MO");
                                return;
                            }
                        }
                        if (isMelee && PlayerLevel >= 7 && API.CanCast(FerociousBite) && !TalentPrimalWrath && (!IncaBerserk && API.PlayerEnergy >= 50 || IncaBerserk && API.PlayerEnergy >= 25) && (API.TargetHasDebuff(Rip) && API.TargetDebuffRemainingTime(Rip) > 300))
                        {
                            API.CastSpell(FerociousBite);
                            return;
                        }
                        if (TalentSavageRoar && isMelee && API.CanCast(SavageRoar) && (!IncaBerserk && API.PlayerEnergy >= 30 || IncaBerserk && API.PlayerEnergy >= 18) && !API.PlayerHasBuff(SavageRoar))
                        {
                            API.CastSpell(SavageRoar);
                            return;
                        }
                        if (isMelee && TalentPrimalWrath && API.CanCast(PrimalWrath) && (!IncaBerserk && API.PlayerEnergy >= 20 || IncaBerserk && API.PlayerEnergy >= 12))
                        {
                            API.CastSpell(PrimalWrath);
                            return;
                        }
                        if (isMelee && PlayerLevel >= 21 && !TalentPrimalWrath && API.CanCast(Rip) && (!IncaBerserk && API.PlayerEnergy >= 20 || IncaBerserk && API.PlayerEnergy >= 12) && (!API.TargetHasDebuff(Rip) || API.TargetDebuffRemainingTime(Rip) <= 300))
                        {
                            API.CastSpell(Rip);
                            return;
                        }
                    }
                    //Generator
                    if (API.PlayerComboPoints < 5)
                    {
                        if (isThrashMelee && !API.PlayerHasBuff(Prowl) && API.TargetDebuffRemainingTime(Rake) > 360 && TalentBrutalSlash && API.CanCast(BrutalSlash) && API.TargetHasDebuff(Thrash) && (API.PlayerHasBuff(Clearcasting) || !IncaBerserk && API.PlayerEnergy >= 25 || IncaBerserk && API.PlayerEnergy >= 15))
                        {
                            API.CastSpell(BrutalSlash);
                            return;
                        }
                        if (isThrashMelee && PlayerLevel >= 11 && !API.PlayerHasBuff(Prowl) && API.CanCast(Thrash) && (!API.TargetHasDebuff(Thrash) || API.TargetDebuffRemainingTime(Thrash) <= 300) && (!IncaBerserk && API.PlayerEnergy >= 40 || IncaBerserk && API.PlayerEnergy >= 24 || API.PlayerHasBuff(Clearcasting)))
                        {
                            API.CastSpell(Thrash);
                            return;
                        }
                        if (isThrashMelee && PlayerLevel >= 11 && !API.PlayerHasBuff(Prowl) && API.CanCast(Thrash) && TalentScentofBlood && !API.PlayerHasBuff(ScentofBlood) && (!IncaBerserk && API.PlayerEnergy >= 40 || IncaBerserk && API.PlayerEnergy >= 24 || API.PlayerHasBuff(Clearcasting)))
                        {
                            API.CastSpell(Thrash);
                            return;
                        }
                        if (isThrashMelee && PlayerLevel >= 42 && !API.PlayerHasBuff(Prowl) && TalentScentofBlood && API.CanCast(Swipe) && API.PlayerHasBuff(ScentofBlood))
                        {
                            API.CastSpell(Swipe);
                            return;
                        }
                        if (API.CanCast(Rake) && PlayerLevel >= 10 && isMelee && (!IncaBerserk && API.PlayerEnergy >= 35 || IncaBerserk && API.PlayerEnergy >= 21) && API.TargetDebuffRemainingTime(Rake) <= 360)
                        {
                            API.CastSpell(Rake);
                            return;
                        }
                        if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                        {
                            if (API.MouseoverDebuffRemainingTime(Rake) <= 360 && API.CanCast(Rake) && isMOMelee)
                            {
                                API.CastSpell(Rake + "MO");
                                return;
                            }
                            if (API.MouseoverDebuffRemainingTime(Thrash) <= 300 && API.CanCast(Thrash) && isMOThrashMelee)
                            {
                                API.CastSpell(Thrash + "MO");
                                return;
                            }
                        }
                        if (API.CanCast(Moonfire) && !API.PlayerHasBuff(Prowl) && TalentLunarInspiration && API.LastSpellCastInGame != (Moonfire) && (!API.TargetHasDebuff(Moonfire) || API.TargetDebuffRemainingTime(Moonfire) <= 200) && API.TargetRange < 40 && (!IncaBerserk && API.PlayerEnergy >= 30 || IncaBerserk && API.PlayerEnergy >= 18))
                        {
                            API.CastSpell(Moonfire);
                            return;
                        }
                        if (isThrashMelee && PlayerLevel >= 42 && !API.PlayerHasBuff(Prowl) && !TalentBrutalSlash && API.CanCast(Swipe) && API.TargetHasDebuff(Thrash) && (!IncaBerserk && API.PlayerEnergy >= 35 || API.PlayerHasBuff(Clearcasting) || IncaBerserk && API.PlayerEnergy >= 21))
                        {
                            API.CastSpell(Swipe);
                            return;
                        }
                        if (isThrashMelee && PlayerLevel >= 11 && !API.PlayerHasBuff(Prowl) && API.CanCast(Thrash) && (!IncaBerserk && API.PlayerEnergy >= 40 || IncaBerserk && API.PlayerEnergy >= 24 || API.PlayerHasBuff(Clearcasting)))
                        {
                            API.CastSpell(Thrash);
                            return;
                        }
                    }
                }
            }
        }
    }
}