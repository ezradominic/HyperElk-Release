using System.Collections.Generic;

namespace HyperElk.Core
{
    public class FmFlexOutLaw : CombatRoutine
    {


        //Spells, Buff, Debuff
        private string SinisterStrike = "Sinister Strike";
        private string SliceandDice = "Slice and Dice";
        private string PistolShot = "Pistol Shot";
        private string RolltheBones = "Roll the Bones";
        private string BetweentheEyes = "Between the Eyes";
        private string Dispatch = "Dispatch";
        private string AdrenalineRush = "Adrenaline Rush";
        private string Kick = "Kick";

        private string Stealth = "Stealth";
        private string Vanish = "Vanish";
        private string Ambush = "Ambush";

        private string BladeFlurry = "Blade Flurry";
        private string GhostlyStrike = "Ghostly Strike";
        private string BladeRush = "Blade Rush";
        private string Dreadblades = "Dreadblades";
        private string KillingSpree = "Killing Spree";
        private string MarkedforDeath = "Marked for Death";

        private string CloakofShadows = "Cloak of Shadows";
        private string CrimsonVial = "Crimson Vial";
        private string Feint = "Feint";
        private string Evasion = "Evasion";

        private string Opportunity = "Opportunity";
        private string LoadedDice = "Loaded Dice";
        private string AceUpYourSleeve = "Ace Up Your Sleeve";
        private string Deadshot = "Deadshot";
        private string SnakeEyes = "Snake Eyes";
        private string RuthlessPrecision = "Ruthless Precision";
        private string GrandMelee = "Grand Melee";
        private string Broadside = "Broadside";
        private string SkullandCrossbones = "Skull and Crossbones";
        private string BuriedTreasure = "Buried Treasure";
        private string TrueBearing = "True Bearing";

        //Properties
      
        private int EvasionLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Evasion)];
        private int CrimsonVialLifePercent => percentListProp[CombatRoutine.GetPropertyInt(CrimsonVial)];
        private int FeintLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feint)];
        private bool AutoStealth => CombatRoutine.GetPropertyBool("AUTOSTEALTH");


        //Talents
        bool TalentQuickDraw => API.PlayerIsTalentSelected(1, 2);
        bool TalentGhostlyStrike => API.PlayerIsTalentSelected(1, 3);
        bool TalentAcrobaticStrikes => API.PlayerIsTalentSelected(2, 1);
        bool TalentVigor => API.PlayerIsTalentSelected(3, 1);
        bool TalentDeeperStratagem => API.PlayerIsTalentSelected(3, 2);
        bool TalentMarkedForDeath => API.PlayerIsTalentSelected(3, 3);
        bool TalentBladeRush => API.PlayerIsTalentSelected(7, 2);
        bool TalentKillingSpree => API.PlayerIsTalentSelected(7, 3);


        //Rotation Utilities
        private bool FinisherCombopoints => API.PlayerComboPoints >= MaxComboPoints 
            - ((API.PlayerHasBuff(Broadside) ? 1 : 0) + (API.PlayerHasBuff(Opportunity) ? 1 : 0)) 
            * ((TalentQuickDraw && (!TalentMarkedForDeath || API.SpellCDDuration(MarkedforDeath) > 100)) ? 1 : 0);
 
        private bool isStealth => API.PlayerHasBuff(Stealth) || API.PlayerHasBuff(Vanish);

        int MaxEnergy => API.PlayeMaxEnergy;
        int MaxComboPoints => TalentDeeperStratagem ? 6 : 5;
        int ComboPointDeficit => MaxComboPoints - API.PlayerComboPoints;

        bool IsMelee => TalentAcrobaticStrikes ? API.TargetRange <= 5 : API.TargetRange <= 8;
        float EnergyRegen => 10f * (1f + API.PlayerGetHaste) * (TalentVigor ? 1.1f : 1f);
        float TimeUntilMaxEnergy => (MaxEnergy - API.PlayerEnergy) * 100f / EnergyRegen;

        List<string> RtBBuffs;


        public override void Initialize()
        {
            CombatRoutine.Name = "Outlaw Rotation @FmFlex";
            API.WriteLog("Welcome to FmFlex's Outlaw rotation");

            //Add Spell, Buff, Debuffs

            CombatRoutine.AddSpell(SinisterStrike, "D1");
            CombatRoutine.AddSpell(SliceandDice, "D1");
            CombatRoutine.AddSpell(AdrenalineRush, "D1");
            CombatRoutine.AddSpell(PistolShot, "D1");
            CombatRoutine.AddSpell(BladeFlurry, "D1");
            CombatRoutine.AddSpell(RolltheBones, "D1");
            CombatRoutine.AddSpell(BetweentheEyes, "D1");
            CombatRoutine.AddSpell(Dispatch, "D1");
            CombatRoutine.AddSpell(Kick, "D1");
            CombatRoutine.AddSpell(Ambush, "D1");
            CombatRoutine.AddSpell(Stealth, "D1");
            CombatRoutine.AddSpell(Vanish, "D1");

            CombatRoutine.AddSpell(CloakofShadows, "D1");
            CombatRoutine.AddSpell(CrimsonVial, "D1");
            CombatRoutine.AddSpell(Feint, "D1");
            CombatRoutine.AddSpell(Evasion, "D1");

            CombatRoutine.AddSpell(KillingSpree, "D1");
            CombatRoutine.AddSpell(MarkedforDeath, "D1");
            CombatRoutine.AddSpell(BladeRush, "D1");
            CombatRoutine.AddSpell(GhostlyStrike, "D1");
            CombatRoutine.AddSpell(Dreadblades, "D1");

            CombatRoutine.AddBuff(AdrenalineRush);
            CombatRoutine.AddBuff(SliceandDice);
            CombatRoutine.AddBuff(BladeFlurry);
            CombatRoutine.AddBuff(AceUpYourSleeve);
            CombatRoutine.AddBuff(Stealth);
            CombatRoutine.AddBuff(Vanish);
            CombatRoutine.AddBuff(Opportunity);
            CombatRoutine.AddBuff(LoadedDice);
            CombatRoutine.AddBuff(Deadshot);
            CombatRoutine.AddBuff(SnakeEyes);
            CombatRoutine.AddBuff(RuthlessPrecision);
            CombatRoutine.AddBuff(GrandMelee);
            CombatRoutine.AddBuff(Broadside);
            CombatRoutine.AddBuff(SkullandCrossbones);
            CombatRoutine.AddBuff(BuriedTreasure);
            CombatRoutine.AddBuff(TrueBearing);

            
            //CB Properties
            CombatRoutine.AddProp("AUTOSTEALTH", "Auto Stealth", true, "Auto Stealth when not in combat", "Generic");

            CombatRoutine.AddProp(Evasion, Evasion + " Life Percent", percentListProp, "Life percent at which" + Evasion + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(CrimsonVial, CrimsonVial + " Life Percent", percentListProp, "Life percent at which" + CrimsonVial + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Feint, Feint + " Life Percent", percentListProp, "Life percent at which" + Feint + "is used, set to 0 to disable", "Defense", 2);
            
            RtBBuffs = new List<string>(new string[] { Broadside, BuriedTreasure, GrandMelee, RuthlessPrecision, SkullandCrossbones, TrueBearing });
        }


        public override void Pulse()
        {
            if (!API.PlayerIsMounted && !API.PlayerIsCasting)
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
            if (!isStealth || API.PlayerIsCasting)
                return;


                if (isInterrupt && API.TargetCanInterrupted && API.TargetIsCasting && API.TargetCurrentCastTimeRemaining < interruptDelay && !API.SpellISOnCooldown(Kick) && IsMelee)
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
                if (TalentMarkedForDeath && ComboPointDeficit >= MaxComboPoints - 1 && !API.SpellISOnCooldown(MarkedforDeath) && API.TargetRange <= 30)
                {
                    API.CastSpell(MarkedforDeath);
                    return;
                }
                if (ShouldRolltheBones() && API.PlayerEnergy >= 25 && !API.SpellISOnCooldown(RolltheBones) && API.TargetRange <= 20)
                {
                    API.CastSpell(RolltheBones);
                    return;
                }

                if (!API.PlayerHasBuff(SliceandDice) && API.PlayerComboPoints >= 2 && !API.SpellISOnCooldown(SliceandDice) && API.TargetRange <= 20)
                {
                    API.CastSpell(SliceandDice);
                    return;
                }
                if (TalentGhostlyStrike && ComboPointDeficit >= 1 + (API.PlayerHasBuff(Broadside) ? 1 : 0) && !API.SpellISOnCooldown(GhostlyStrike) && API.PlayerEnergy >= 30 && IsMelee)
                {
                    API.CastSpell(GhostlyStrike);
                    return;
                }
                if (IsCooldowns && TalentKillingSpree && (!IsAOE || API.PlayerHasBuff(BladeFlurry)) && TimeUntilMaxEnergy > 200 && !API.SpellISOnCooldown(KillingSpree) && API.TargetRange <= 10)
                {
                    API.CastSpell(KillingSpree);
                    return;
                }
                if (TalentBladeRush && (!IsAOE || API.PlayerHasBuff(BladeFlurry)) && TimeUntilMaxEnergy > 200 && !API.SpellISOnCooldown(BladeRush) && API.TargetRange <= 20)
                {
                    API.CastSpell(BladeRush);
                    return;
                }

                if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && !API.SpellISOnCooldown(BladeFlurry) && IsMelee)
                {
                    API.CastSpell(BladeFlurry);
                    return;
                }
                if (FinisherCombopoints)
                {
                    if (API.PlayerBuffTimeRemaining(SliceandDice) < ((1 + API.PlayerComboPoints) * 100) * 1.8 && !API.SpellISOnCooldown(SliceandDice) && API.TargetRange <= 20)
                    {
                        API.CastSpell(SliceandDice);
                        return;
                    }
                    if (API.PlayerComboPoints >= MaxComboPoints - 1 && !API.SpellISOnCooldown(BetweentheEyes) && API.TargetRange <= 20)
                    {
                        API.CastSpell(BetweentheEyes);
                        return;
                    }

                }
                if (IsCooldowns && !API.SpellISOnCooldown(AdrenalineRush) && IsMelee)
                {
                    API.CastSpell(AdrenalineRush);
                    return;
                }
                if (FinisherCombopoints)
                {
                    if (!API.SpellISOnCooldown(Dispatch) && IsMelee)
                    {
                        API.CastSpell(Dispatch);
                        return;
                    }
                }
                if (TalentQuickDraw && API.PlayerEnergy >= 20 && API.PlayerHasBuff(Opportunity) && API.PlayerComboPoints <= MaxComboPoints - 2 && !API.SpellISOnCooldown(PistolShot) && API.TargetRange <= 20)
                {
                    API.CastSpell(PistolShot);
                    return;
                }

                if (API.PlayerEnergy >= 45 && !API.SpellISOnCooldown(SinisterStrike) && IsMelee)
                {
                    API.CastSpell(SinisterStrike);
                    return;
                }
                if (API.PlayerEnergy >= 20 && API.PlayerHasBuff(Opportunity) && API.PlayerComboPoints <= MaxComboPoints - 2 && !API.SpellISOnCooldown(PistolShot) && API.TargetRange <= 20)
                {
                    API.CastSpell(PistolShot);
                    return;
                }

                if (API.PlayerEnergy >= 80 && !API.SpellISOnCooldown(PistolShot) && API.TargetRange <= 20)
                {
                    API.CastSpell(PistolShot);
                    return;
                }
        }

        public override void OutOfCombatPulse()
        {
            if (AutoStealth && !isStealth && !API.SpellISOnCooldown(Stealth) && !API.PlayerIsCasting)
            {
                API.CastSpell(Stealth);
                return;
            }
        }
        public bool ShouldRolltheBones()
        {
            int RolltheBonesRemain = 0;
            int RolltheBonesCount = 0;
            foreach (var buff in RtBBuffs)
            {
                if (API.PlayerHasBuff(buff))
                {
                    RolltheBonesRemain = API.PlayerBuffTimeRemaining(buff);
                    RolltheBonesCount++;
                    break;
                }
            }
            if (RolltheBonesRemain <= 300) return true;
            if (RolltheBonesCount > 2) return false;
            if (RolltheBonesCount <= 1 && (API.PlayerHasBuff(LoadedDice) || API.PlayerHasBuff(GrandMelee) || API.PlayerHasBuff(BuriedTreasure)))
                return true;
            if (IsAOE && RolltheBonesCount <= 2 && (API.PlayerHasBuff(RuthlessPrecision) || API.PlayerHasBuff(Broadside)))
                return false;
            if(IsAOE && API.PlayerHasBuff(SkullandCrossbones)) return true;
            if (RolltheBonesCount == 2 &&  API.PlayerHasBuff(GrandMelee)  && API.PlayerHasBuff(BuriedTreasure))
                return true;
            return true;

        }


    }

}

