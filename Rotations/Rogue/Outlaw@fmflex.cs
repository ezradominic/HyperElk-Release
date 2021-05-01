using System.Collections.Generic;

namespace HyperElk.Core
{
    public class FmFlexOutLaw : CombatRoutine
    {

        private bool CheapshotToggle => API.ToggleIsEnabled("Cheap Shot");
        private bool AutoStealth=> API.ToggleIsEnabled("Auto Stealth");

        //Spells
        private string SinisterStrike = "Sinister Strike";
        private string SliceandDice = "Slice and Dice";
        private string PistolShot = "Pistol Shot";
        private string RolltheBones = "Roll the Bones";
        private string BetweentheEyes = "Between the Eyes";
        private string Dispatch = "Dispatch";
        private string AdrenalineRush = "Adrenaline Rush";
        private string CheapShot = "Cheap Shot";
        private string Kick = "Kick";


        //Utility
        private string Stealth = "Stealth";
        private string Vanish = "Vanish";
        private string Ambush = "Ambush";
        private string TricksoftheTrade = "Tricks of the Trade";

        //Talents
        private string BladeFlurry = "Blade Flurry";
        private string GhostlyStrike = "Ghostly Strike";
        private string BladeRush = "Blade Rush";
        private string Dreadblades = "Dreadblades";
        private string KillingSpree = "Killing Spree";
        private string MarkedforDeath = "Marked for Death";

        //Defensives
        private string CloakofShadows = "Cloak of Shadows";
        private string CrimsonVial = "Crimson Vial";
        private string Feint = "Feint";
        private string Evasion = "Evasion";

        //Buffs
        private string Opportunity = "Opportunity";
        private string LoadedDice = "Loaded Dice";
        private string RuthlessPrecision = "Ruthless Precision";
        private string GrandMelee = "Grand Melee";
        private string Broadside = "Broadside";
        private string SkullandCrossbones = "Skull and Crossbones";
        private string BuriedTreasure = "Buried Treasure";
        private string TrueBearing = "True Bearing";

        //Covenant
        private string SerratedBoneSpike = "Serrated Bone Spike";
        private string EchoingReprimand = "Echoing Reprimand";
        private string Flagellation = "Flagellation";
        private string Sepsis = "Sepsis";

        private string Soulshape = "Soulshape";
        private string Fleshcraft = "Fleshcraft";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";

        //Properties

        private int EvasionLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Evasion)];
        private int CrimsonVialLifePercent => percentListProp[CombatRoutine.GetPropertyInt(CrimsonVial)];
        private int FeintLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feint)];
        private int FleshcraftLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Fleshcraft)];

        private int PhialofSerenityLifePercent => percentListProp[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => percentListProp[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];

        //Talents
        bool TalentQuickDraw => API.PlayerIsTalentSelected(1, 2);
        bool TalentGhostlyStrike => API.PlayerIsTalentSelected(1, 3);
        bool TalentAcrobaticStrikes => API.PlayerIsTalentSelected(2, 1);
        bool TalentVigor => API.PlayerIsTalentSelected(3, 1);
        bool TalentDeeperStratagem => API.PlayerIsTalentSelected(3, 2);
        bool TalentMarkedForDeath => API.PlayerIsTalentSelected(3, 3);
        bool TalentBladeRush => API.PlayerIsTalentSelected(7, 2);
        bool TalentKillingSpree => API.PlayerIsTalentSelected(7, 3);
        bool TalentPreyontheWeak => API.PlayerIsTalentSelected(5, 3);

        //Rotation Utilities
        private bool FinisherCombopoints => API.PlayerComboPoints >= MaxComboPoints
            - ((API.PlayerHasBuff(Broadside) ? 1 : 0) + (API.PlayerHasBuff(Opportunity) ? 1 : 0))
            * ((TalentQuickDraw && (!TalentMarkedForDeath || API.SpellCDDuration(MarkedforDeath) > 100)) ? 1 : 0);

        private bool IsStealth => API.PlayerHasBuff(Stealth) || API.PlayerHasBuff(Vanish) || API.PlayerHasBuff(Sepsis);

        int MaxEnergy => API.PlayeMaxEnergy;
        int MaxComboPoints => TalentDeeperStratagem ? 6 : 5;
        int ComboPointDeficit => MaxComboPoints - API.PlayerComboPoints;

        bool IsMelee => TalentAcrobaticStrikes ? API.TargetRange <= 5 : API.TargetRange <= 8;
        float EnergyRegen => 10f * (1f + API.PlayerGetHaste) * (TalentVigor ? 1.1f : 1f);
        float TimeUntilMaxEnergy => (MaxEnergy - API.PlayerEnergy) * 100f / EnergyRegen;

        bool SnDCondition => API.PlayerHasBuff(SliceandDice) || API.PlayerUnitInMeleeRangeCount >= 6;
        bool PotWCondition => IsStealth && TalentPreyontheWeak && API.CanCast(CheapShot);

        //Serrated Bone Spikes and Echoing Reprimand: Edited
        bool SerratedBoneSpikeThreshold => (API.SpellCharges(SerratedBoneSpike) + ((3000 - API.SpellChargeCD(SerratedBoneSpike)) / 3000)) >= 2.75;
        int current_effective_cp => PlayerCovenantSettings == "Kyrian" && API.PlayerBuffStacks(EchoingReprimand) == API.PlayerComboPoints ? 7 : API.PlayerComboPoints;

        List<string> RtBBuffs;


        public override void Initialize()
        {
            CombatRoutine.Name = "Outlaw Rotation @FmFlex";
            API.WriteLog("Welcome to FmFlex's Outlaw rotation modified by Wyrm");

            //Builders, Spenders
            CombatRoutine.AddSpell(SinisterStrike, 193315, "D1");
            CombatRoutine.AddSpell(SliceandDice, 315496, "D1");
            CombatRoutine.AddSpell(AdrenalineRush, 13750, "D1");
            CombatRoutine.AddSpell(PistolShot, 185763, "D1");
            CombatRoutine.AddSpell(BladeFlurry, 13877, "D1");
            CombatRoutine.AddSpell(RolltheBones, 315508, "D1");
            CombatRoutine.AddSpell(BetweentheEyes, 315341, "D1");
            CombatRoutine.AddSpell(Dispatch, 2098, "D1");
            CombatRoutine.AddSpell(Kick, 1766, "D1");
            CombatRoutine.AddSpell(Ambush, 8676, "D1");
            CombatRoutine.AddSpell(Stealth, 1784, "D1");
            CombatRoutine.AddSpell(Vanish, 1856, "D1");
            CombatRoutine.AddSpell(CheapShot, 1833, "D1");

            //Defensives
            CombatRoutine.AddSpell(CloakofShadows, 31224, "D1");
            CombatRoutine.AddSpell(CrimsonVial, 185311, "D1");
            CombatRoutine.AddSpell(Feint, 1966, "D1");
            CombatRoutine.AddSpell(Evasion, 5277, "D1");
            CombatRoutine.AddSpell(TricksoftheTrade, 57934);
            //Talents
            CombatRoutine.AddSpell(KillingSpree, 51690, "D1");
            CombatRoutine.AddSpell(MarkedforDeath, 137619, "D1");
            CombatRoutine.AddSpell(BladeRush, 271877, "D1");
            CombatRoutine.AddSpell(GhostlyStrike, 196937, "D1");
            CombatRoutine.AddSpell(Dreadblades, 343142, "D1");

            //Covenants
            CombatRoutine.AddSpell(EchoingReprimand, 323547);
            CombatRoutine.AddSpell(SerratedBoneSpike, 328547);
            CombatRoutine.AddSpell(Flagellation, 323654);
            CombatRoutine.AddSpell(Sepsis, 328305);

            //Buffs
            CombatRoutine.AddBuff(AdrenalineRush, 13750);
            CombatRoutine.AddBuff(SliceandDice, 315496);
            CombatRoutine.AddBuff(BladeFlurry, 13877);
            CombatRoutine.AddBuff(Stealth, 1784);
            CombatRoutine.AddBuff(Vanish, 1856);
            CombatRoutine.AddBuff(Opportunity, 195627);
            CombatRoutine.AddBuff(LoadedDice, 256170);
            CombatRoutine.AddBuff(RuthlessPrecision, 193357);
            CombatRoutine.AddBuff(GrandMelee, 193358);
            CombatRoutine.AddBuff(Broadside, 193356);
            CombatRoutine.AddBuff(SkullandCrossbones, 199603);
            CombatRoutine.AddBuff(BuriedTreasure, 199600);
            CombatRoutine.AddBuff(TrueBearing, 193359);
            CombatRoutine.AddBuff(Sepsis, 328305);
            CombatRoutine.AddBuff(EchoingReprimand, 323547);
            CombatRoutine.AddBuff(Flagellation, 323654);
            CombatRoutine.AddBuff(SerratedBoneSpike, 328547);
            CombatRoutine.AddBuff(Fleshcraft, 324631);
            CombatRoutine.AddBuff(Soulshape, 310143);
            CombatRoutine.AddBuff(TricksoftheTrade, 59628);
            //Debuff/s
            CombatRoutine.AddDebuff(Flagellation, 323654);
			CombatRoutine.AddDebuff(SerratedBoneSpike, 324073);

            //Toggle
            CombatRoutine.AddToggle("Cheap Shot");
            CombatRoutine.AddToggle("Auto Stealth");

            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //CB Properties

            CombatRoutine.AddProp(Evasion, Evasion + " Life Percent", percentListProp, "Life percent at which" + Evasion + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(CrimsonVial, CrimsonVial + " Life Percent", percentListProp, "Life percent at which" + CrimsonVial + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Feint, Feint + " Life Percent", percentListProp, "Life percent at which" + Feint + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(Fleshcraft, Fleshcraft + " Life Percent", percentListProp, "Life percent at which" + "is used, set to 0 to disable", "Defense", 0);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", percentListProp, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", percentListProp, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 4);

            RtBBuffs = new List<string>(new string[] { Broadside, BuriedTreasure, GrandMelee, RuthlessPrecision, SkullandCrossbones, TrueBearing });
        }

        //Rotation
        public override void Pulse()
        {
            if (!API.PlayerIsMounted && !API.PlayerIsCasting())
            {
                if (API.PlayerHealthPercent <= CrimsonVialLifePercent && !API.SpellISOnCooldown(CrimsonVial) && API.PlayerEnergy >= 20)
                {
                    API.CastSpell(CrimsonVial);
                    return;
                }
                if (API.PlayerItemCanUse(PhialofSerenity) && API.PlayerItemRemainingCD(PhialofSerenity) == 0 && API.PlayerHealthPercent <= PhialofSerenityLifePercent)
                {
                    API.CastSpell(PhialofSerenity);
                    return;
                }
                if (API.PlayerItemCanUse(SpiritualHealingPotion) && !API.MacroIsIgnored(SpiritualHealingPotion) && API.PlayerItemRemainingCD(SpiritualHealingPotion) == 0 && API.PlayerHealthPercent <= SpiritualHealingPotionLifePercent)
                {
                    API.CastSpell(SpiritualHealingPotion);
                    return;
                }
                //Talent Prey on the Weak: cast Cheap shot if target selected and toggle enabled.
                if (PotWCondition && CheapshotToggle && API.TargetRange <= 5)
                {
                    API.CastSpell(CheapShot);
                    return;
                }
            }
        }

        public override void CombatPulse()
        {
            if (IsStealth || API.PlayerIsCasting(false))
                return;

            //Talent Prey on the Weak: cast Cheap shot if target selected.
            if (PotWCondition && API.TargetRange <= 5)
            {
                API.CastSpell(CheapShot);
                return;
            }

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
            if (API.CanCast(TricksoftheTrade) && API.FocusRange < 100 && API.FocusHealthPercent != 0 && IsMelee && !API.PlayerHasBuff(TricksoftheTrade, false, false))
            {
                API.CastSpell(TricksoftheTrade);
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

            //Edited
            if (IsAOE && PlayerCovenantSettings == "Night Fae" && IsMelee && API.CanCast(Sepsis) && SnDCondition && ComboPointDeficit >= 1)
            {
                API.CastSpell(Sepsis);
                return;
            }

            if (PlayerCovenantSettings == "Necrolord" && IsMelee && API.CanCast(SerratedBoneSpike) && !API.TargetHasDebuff(SerratedBoneSpike) && SnDCondition && ComboPointDeficit >= 1 && API.TargetTimeToDie > 2100)
            {
                API.CastSpell(SerratedBoneSpike);
                return;
            }

            if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && IsMelee && !API.TargetHasDebuff(Flagellation) && API.PlayerComboPoints >= MaxComboPoints - 1 && API.CanCast(Flagellation) && IsMelee)
            {
                API.CastSpell(Flagellation);
                return;
            }

            if (IsCooldowns && PlayerCovenantSettings == "Kyrian" && IsMelee && API.CanCast(EchoingReprimand) && SnDCondition && ComboPointDeficit >= 2 && (API.PlayerUnitInMeleeRangeCount <= 4 || !IsAOE) && IsMelee)
            {
                API.CastSpell(EchoingReprimand);
                return;
            }

            if (PlayerCovenantSettings == "Kyrian" && API.PlayerBuffStacks(EchoingReprimand) == API.PlayerComboPoints)
            {
                if (!API.SpellISOnCooldown(Dispatch) && IsMelee)
                {
                    API.CastSpell(Dispatch);
                    return;
                }

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
        }
        //Auto Stealth
        public override void OutOfCombatPulse()
        {
            if (AutoStealth && !IsStealth && !API.SpellISOnCooldown(Stealth) && !API.PlayerIsCasting())
            {
                API.CastSpell(Stealth);
                return;
            }
        }
        //Roll Them Bones
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
                }
            }
            if (RolltheBonesRemain <= 300) return true;
            if (RolltheBonesCount > 2) return false;
            if (RolltheBonesCount <= 1 && (API.PlayerHasBuff(LoadedDice) || API.PlayerHasBuff(GrandMelee) || API.PlayerHasBuff(BuriedTreasure)))
                return true;
            if (IsAOE && RolltheBonesCount <= 2 && (API.PlayerHasBuff(RuthlessPrecision) || API.PlayerHasBuff(Broadside)))
                return false;
            if (IsAOE && API.PlayerHasBuff(SkullandCrossbones)) return true;
            if (RolltheBonesCount == 2 && API.PlayerHasBuff(GrandMelee) && API.PlayerHasBuff(BuriedTreasure))
                return true;
            return true;

        }


    }

}

