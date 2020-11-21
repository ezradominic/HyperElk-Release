
namespace HyperElk.Core
{
    public class SubRogueFmFlex : CombatRoutine
    {

        //Spells, Buff, Debuff
        private string Backstab = "Backstab";
        private string ShurikenStorm = "Shuriken Storm";
        private string Shadowstrike = "Shadowstrike";
        private string Rupture = "Rupture";
        private string Eviscerate = "Eviscerate";

        private string ShadowDance = "Shadow Dance";
        private string SymbolsofDeath = "Symbols of Death";
        private string ShadowBlades = "Shadow Blades";
        private string SliceandDice = "Slice and Dice";
        private string Stealth = "Stealth";
        private string Vanish = "Vanish";
        private string Ambush = "Ambush";
        private string GhostlyStrike = "Ghostly Strike";

        private string Kick = "Kick";
       
        private string MarkedforDeath = "Marked for Death";
        private string Gloomblade = "Gloomblade";

        private string CloakofShadows = "Cloak of Shadows";
        private string CrimsonVial = "Crimson Vial";
        private string Feint = "Feint";
        private string Evasion = "Evasion";

        private string FindWeakness = "Find Weakness";



        //Properties
        private int EvasionLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Evasion)];
        private int CrimsonVialLifePercent => percentListProp[CombatRoutine.GetPropertyInt(CrimsonVial)];
        private int FeintLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feint)];
        private bool AutoStealth => CombatRoutine.GetPropertyBool("AUTOSTEALTH");


        //Talents

        bool TalentGloomblade = API.PlayerIsTalentSelected(1, 3);
        bool TalentNightstalker = API.PlayerIsTalentSelected(2, 1);
        bool TalentShadowFocus = API.PlayerIsTalentSelected(2, 3);
        bool TalentVigor = API.PlayerIsTalentSelected(3, 1);
        bool TalentMarkedForDeath = API.PlayerIsTalentSelected(3, 3);
        bool TalentDeeperStratagem = API.PlayerIsTalentSelected(3, 2);
        bool TalentDarkShadow = API.PlayerIsTalentSelected(6, 1);
        bool TalentAlacrity = API.PlayerIsTalentSelected(6, 2);
        bool TalentEnvelopingShadows = API.PlayerIsTalentSelected(6, 3);
        bool TalentMasterOfShadows = API.PlayerIsTalentSelected(7, 1);
        bool TalentShurikenTornado = API.PlayerIsTalentSelected(7, 3);


        //Rotation Utilities

        private bool isStealth => API.PlayerHasBuff(Stealth) || API.PlayerHasBuff(Vanish);

        int MaxEnergy => API.PlayeMaxEnergy;
        int MaxComboPoints => TalentDeeperStratagem ? 6 : 5;
        int ComboPointDeficit => MaxComboPoints - API.PlayerComboPoints;
        
        double RuptureMaxTime;
        bool IsMelee => API.TargetRange <= 6;
        float EnergyRegen => 10f * (1f + API.PlayerGetHaste) * (TalentVigor ? 1.1f : 1f);

        bool SnDCondition  => API.PlayerHasBuff(SliceandDice) || API.PlayerUnitInMeleeRangeCount >= 6;
		int StealthThreshold => (25 + ((TalentVigor? 1 : 0) * 20) + ((TalentMasterOfShadows? 1 : 0) * 20) + ((TalentShadowFocus? 1 : 0) * 25) + ((TalentAlacrity? 1 : 0) * 20) + (25*(API.PlayerUnitInMeleeRangeCount >= 4 ? 1 : 0)));
		bool SkipRupture => (!TalentNightstalker && TalentDarkShadow && API.PlayerHasBuff(ShadowDance) || (API.PlayerUnitInMeleeRangeCount >= 6 && IsAOE));
		bool RuptureRefreshable => API.TargetDebuffRemainingTime(Rupture)<(RuptureMaxTime* 0.30) || !API.TargetHasDebuff(Rupture);
		bool shdThreshold => (API.SpellCharges(ShadowDance) + ((60 - API.SpellChargeCD("Shadow Dance"))/60)) >= 1.75; //The Fractional equivalent for charges (hope it works)
		bool shdComboPoints => ComboPointDeficit >= 4;



        public override void Initialize()
        {
            CombatRoutine.Name = "Sub Rotation @FmFlex";
            API.WriteLog("Welcome to FmFlex's Outlaw rotation");


            //Add Spell, Buff, Debuffs


            //ComboPoint-Generator
            CombatRoutine.AddSpell(Backstab, "D1");
            CombatRoutine.AddSpell(ShurikenStorm, "D1");
            CombatRoutine.AddSpell(Shadowstrike, "D1");
            CombatRoutine.AddSpell(Ambush, "D1");
            //ComboPoint-Spender
            CombatRoutine.AddSpell(Rupture, "D1");
            CombatRoutine.AddSpell(Eviscerate, "D1");
            CombatRoutine.AddSpell(SliceandDice, "D1");

            CombatRoutine.AddSpell(ShadowDance, "D1");
            CombatRoutine.AddSpell(SymbolsofDeath, "D1");
            CombatRoutine.AddSpell(ShadowBlades, "D1");

            CombatRoutine.AddSpell(Kick, "D1");
            CombatRoutine.AddSpell(Stealth, "D1");
            CombatRoutine.AddSpell(Vanish, "D1");

            CombatRoutine.AddSpell(CloakofShadows, "D1");
            CombatRoutine.AddSpell(CrimsonVial, "D1");
            CombatRoutine.AddSpell(Feint, "D1");
            CombatRoutine.AddSpell(Evasion, "D1");

            CombatRoutine.AddSpell(MarkedforDeath, "D1");
            CombatRoutine.AddSpell(GhostlyStrike, "D1");
            CombatRoutine.AddSpell(Gloomblade, "D1");



            CombatRoutine.AddBuff(SliceandDice);
            CombatRoutine.AddBuff(ShadowDance);
            CombatRoutine.AddBuff(ShadowBlades);
            CombatRoutine.AddBuff(SymbolsofDeath);

            CombatRoutine.AddBuff(Stealth);
            CombatRoutine.AddBuff(Vanish);

            CombatRoutine.AddDebuff(Rupture);
            CombatRoutine.AddDebuff(FindWeakness);


            //CB Properties

            CombatRoutine.AddProp("AUTOSTEALTH", "Auto Stealth", true, "Auto Stealth when not in combat", "Generic");

            CombatRoutine.AddProp(Evasion, Evasion + " Life Percent", percentListProp, "Life percent at which" + Evasion + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(CrimsonVial, CrimsonVial + " Life Percent", percentListProp, "Life percent at which" + CrimsonVial + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Feint, Feint + " Life Percent", percentListProp, "Life percent at which" + Feint + "is used, set to 0 to disable", "Defense", 2);

            
        }


        public override void Pulse()
        {
            if (!API.PlayerIsMounted && !API.PlayerIsCasting )
            {
                if (API.PlayerHealthPercent <= CrimsonVialLifePercent && !API.SpellISOnCooldown(CrimsonVial) && API.PlayerEnergy >= 20)
                {
                    API.CastSpell(CrimsonVial);
                    return;
                }

               

            }
        }

        public override void CombatPulse()
        {
            if (!API.PlayerIsCasting)
            {

                if (isInterrupt && !API.SpellISOnCooldown(Kick) && IsMelee)
                {
                    API.CastSpell(Kick);
                    return;
                }

                if (IsCooldowns && API.PlayerHealthPercent <= EvasionLifePercent && !API.SpellISOnCooldown(Evasion))
                {
                    API.CastSpell(Evasion);
                    return;
                }
                if (API.PlayerHealthPercent <= FeintLifePercent && !API.SpellISOnCooldown(Feint) && API.PlayerEnergy >= 35)
                {
                    API.CastSpell(Feint);
                    return;
                }

                if (IsCooldowns && !API.SpellISOnCooldown(SymbolsofDeath) && SnDCondition &&
                    (TalentEnvelopingShadows || API.SpellCharges(ShadowDance) >= 1) &&
                    (!TalentShurikenTornado || TalentShadowFocus || API.SpellCDDuration(ShurikenStorm) > 200) &&
                    (API.PlayerComboPoints <= 2))
                {
                    API.CastSpell(SymbolsofDeath);
                    return;
                }

                if (!isStealth && ((MaxEnergy - API.PlayerEnergy) <= StealthThreshold && (!API.PlayerHasBuff(Vanish) && !API.SpellISOnCooldown(Vanish)) ||
                    (!API.PlayerHasBuff(SymbolsofDeath) && !API.PlayerHasBuff(ShadowDance) && API.SpellCharges(ShadowDance) > 0) ||
                    (API.SpellCDDuration(SymbolsofDeath) > 1000)))
                {
                    if (!API.SpellISOnCooldown(ShadowDance) && shdComboPoints && (shdThreshold || API.SpellCDDuration(SymbolsofDeath) >= 120 || IsAOE && API.PlayerUnitInMeleeRangeCount >= 4 && API.SpellCDDuration(SymbolsofDeath) > 1000))
                    {
                        API.CastSpell(ShadowDance);
                        return;
                    }
                }
                if (TalentMarkedForDeath && ComboPointDeficit >= MaxComboPoints - 1 && !API.SpellISOnCooldown(MarkedforDeath) && API.TargetRange <= 30)
                {
                    API.CastSpell(MarkedforDeath);
                    return;
                }


                if ((API.PlayerUnitInMeleeRangeCount < 6 || !IsAOE) && API.PlayerBuffTimeRemaining(SliceandDice) < API.SpellGCDTotalDuration && API.PlayerComboPoints >= 4 - (API.PlayerTimeInCombat < 1000 ? 1 : 0) * 2)
                {
                    API.CastSpell(SliceandDice);
                    return;
                }

                if (isStealth || API.PlayerHasBuff(ShadowDance))
                {
                    API.CastSpell(Shadowstrike);
                    return;
                }
                if ((isStealth || API.PlayerHasBuff(ShadowDance)))
                {
                    if (IsAOE && API.PlayerUnitInMeleeRangeCount == 4 && API.PlayerComboPoints >= 4)
                    {
                        Finisher();
                    }
                    else if (ComboPointDeficit <= 1 - ((TalentDeeperStratagem && API.PlayerHasBuff(Vanish)) ? 1 : 0))
                    {
                        Finisher();
                    }
                    else if (IsAOE && API.TargetDebuffRemainingTime(FindWeakness) < 100 && API.PlayerUnitInMeleeRangeCount <= 3)
                    {
                        API.CastSpell(Shadowstrike);
                        return;
                    }
                    else if (API.TargetDebuffRemainingTime(FindWeakness) <= 100 || API.SpellCDDuration(SymbolsofDeath) < 1800 && API.TargetDebuffRemainingTime(FindWeakness) < API.SpellCDDuration(SymbolsofDeath))
                    {
                        API.CastSpell(Shadowstrike);
                        return;
                    }
                    else if (IsAOE && API.PlayerUnitInMeleeRangeCount >= 3)
                    {
                        API.CastSpell(ShurikenStorm);
                        return;
                    }
                    else
                    {
                        API.CastSpell(Shadowstrike);
                        return;
                    }
                }
                if ((API.SpellISOnCooldown(SymbolsofDeath) && API.SpellCDDuration(SymbolsofDeath) <= 200 && API.PlayerComboPoints >= 2) ||
                    (ComboPointDeficit <= 1 && API.PlayerComboPoints >= 3) || (IsAOE && API.PlayerUnitInMeleeRangeCount == 4 && API.PlayerComboPoints >= 4))
                {
                    Finisher();
                }

                if (API.PlayeMaxEnergy - API.PlayerEnergy <= StealthThreshold)
                {
                    if (IsAOE && API.PlayerUnitInMeleeRangeCount >= 2)
                    {
                        API.CastSpell(ShurikenStorm);
                        return;
                    }
                    if (TalentGloomblade)
                    {
                        API.CastSpell(Gloomblade);
                        return;
                    }
                    else
                    {
                        API.CastSpell(Backstab);
                        return;
                    }
                }
            }
            }

        public override void OutOfCombatPulse()
        {
            if (AutoStealth && !isStealth && !API.PlayerHasBuff(ShadowDance) && !API.SpellISOnCooldown(Stealth) && !API.PlayerIsCasting)
            {
                API.CastSpell(Stealth);
                return;
            }
        }
        public void Finisher()
        {
                if ((API.PlayerUnitInMeleeRangeCount < 6 || !IsAOE) && !API.PlayerHasBuff(ShadowDance) && API.PlayerBuffTimeRemaining(SliceandDice) < (1 + API.PlayerComboPoints) * 1.8)
                {
                    API.CastSpell(SliceandDice);
                    return;
                }
                else if (!SkipRupture && RuptureRefreshable)
                {
                    RuptureMaxTime = 400 + (API.PlayerComboPoints * 400);
                    API.CastSpell(Rupture);
                    return;
                }
                else if (!SkipRupture && API.TargetDebuffRemainingTime(Rupture) < API.SpellCDDuration(SymbolsofDeath) + 1000 && API.SpellCDDuration(SymbolsofDeath) <= 500)
                {
                    RuptureMaxTime = 400 + (API.PlayerComboPoints * 400);
                    API.CastSpell(Rupture);
                    return;
                }
                else
                {
                    API.CastSpell(Eviscerate);
                    return;
                }
            }


    }

}

