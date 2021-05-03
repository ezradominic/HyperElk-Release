// Changelog
// v1.0 First release
// v1.1 small hotfixes
// v1.2 small adjustments
// v1.3 small hotfix
// v1.35 small sepsis change
// v1.4 aaaaand another one xD

using System.Collections.Generic;
using System.Diagnostics;

namespace HyperElk.Core
{
    public class OutlawRogue : CombatRoutine
    {

        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsAutoStealth => API.ToggleIsEnabled("Auto Stealth");

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
        private string Stealth = "Stealth";
        private string Vanish = "Vanish";
        private string Ambush = "Ambush";
        private string TricksoftheTrade = "Tricks of the Trade";
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
        private string RuthlessPrecision = "Ruthless Precision";
        private string GrandMelee = "Grand Melee";
        private string Broadside = "Broadside";
        private string SkullandCrossbones = "Skull and Crossbones";
        private string BuriedTreasure = "Buried Treasure";
        private string TrueBearing = "True Bearing";
        private string SerratedBoneSpike = "Serrated Bone Spike";
        private string EchoingReprimand = "Echoing Reprimand";
        private string Flagellation = "Flagellation";
        private string Sepsis = "Sepsis";
        private string Soulshape = "Soulshape";
        private string Fleshcraft = "Fleshcraft";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string WoundPoison = "Wound Poison";
        private string NumbingPoison = "Numbing Poison";
        private string CripplingPoison = "Crippling Poison";
        private string InstantPoison = "Instant Poison";
        private string Blind = "Blind";
        private string KidneyShot = "Kidney Shot";
        private string GreenskinsWickers = "Greenskin's Wickers";
        private string ConcealedBlunderbuss = "Concealed Blunderbuss";
        private string Gouge = "Gouge";
        private string MasterAssassinsMark = "Master Assassin's Mark";
        private string Shiv = "Shiv";
        private string DeathlyShadows = "Deathly Shadows";
        private string SepsisBuff = "Sepsis Buff";

        //Talents
        bool TalentQuickDraw => API.PlayerIsTalentSelected(1, 2);
        bool TalentGhostlyStrike => API.PlayerIsTalentSelected(1, 3);
        bool TalentAcrobaticStrikes => API.PlayerIsTalentSelected(2, 1);
        bool TalentVigor => API.PlayerIsTalentSelected(3, 1);
        bool TalentDeeperStratagem => API.PlayerIsTalentSelected(3, 2);
        bool TalentMarkedForDeath => API.PlayerIsTalentSelected(3, 3);
        bool TalentDirtyTricks => API.PlayerIsTalentSelected(5, 1);
        bool TalentPreyontheWeak => API.PlayerIsTalentSelected(5, 3);
        bool TalentDreadblades => API.PlayerIsTalentSelected(6, 3);
        bool TalentBladeRush => API.PlayerIsTalentSelected(7, 2);
        bool TalentKillingSpree => API.PlayerIsTalentSelected(7, 3);

        //Rotation Utilities
        private bool IsStealth => API.PlayerHasBuff(Stealth) || API.PlayerHasBuff(Vanish) || API.PlayerHasBuff(SepsisBuff);

        int MaxEnergy => API.PlayeMaxEnergy;
        int MaxComboPoints => TalentDeeperStratagem ? 6 : 5;
        int ComboPointDeficit => MaxComboPoints - API.PlayerComboPoints;
        int EnergyDefecit => API.PlayeMaxEnergy - API.PlayerEnergy;
        float GCD => API.SpellGCDTotalDuration;

        bool IsMelee => TalentAcrobaticStrikes ? API.TargetRange <= 5 : API.TargetRange <= 8;
        float EnergyRegen => 10f * (1f + API.PlayerGetHaste) * (TalentVigor ? 1.1f : 1f);
        float TimeUntilMaxEnergy => (MaxEnergy - API.PlayerEnergy) * 100f / EnergyRegen;

        List<string> RtBBuffs;

        bool IsVanish => (UseVanish == "with Cooldowns" || UseVanish == "with Cooldowns or AoE" || UseVanish == "on mobcount or Cooldowns") && IsCooldowns || UseVanish == "always" || (UseVanish == "on AOE" || UseVanish == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseVanish == "on mobcount or Cooldowns" || UseVanish == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsBladeRush => (UseBladeRush == "with Cooldowns" || UseBladeRush == "with Cooldowns or AoE" || UseBladeRush == "on mobcount or Cooldowns") && IsCooldowns || UseBladeRush == "always" || (UseBladeRush == "on AOE" || UseBladeRush == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseBladeRush == "on mobcount or Cooldowns" || UseBladeRush == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsAdrenalineRush => (UseAdrenalineRush == "with Cooldowns" || UseAdrenalineRush == "with Cooldowns or AoE" || UseAdrenalineRush == "on mobcount or Cooldowns") && IsCooldowns || UseAdrenalineRush == "always" || (UseAdrenalineRush == "on AOE" || UseAdrenalineRush == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseAdrenalineRush == "on mobcount or Cooldowns" || UseAdrenalineRush == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsDreadblades => (UseDreadblades == "with Cooldowns" || UseDreadblades == "with Cooldowns or AoE" || UseDreadblades == "on mobcount or Cooldowns") && IsCooldowns || UseDreadblades == "always" || (UseDreadblades == "on AOE" || UseDreadblades == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseDreadblades == "on mobcount or Cooldowns" || UseDreadblades == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsKillingSpree => (UseKillingSpree == "with Cooldowns" || UseKillingSpree == "with Cooldowns or AoE" || UseKillingSpree == "on mobcount or Cooldowns") && IsCooldowns || UseKillingSpree == "always" || (UseKillingSpree == "on AOE" || UseKillingSpree == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseKillingSpree == "on mobcount or Cooldowns" || UseKillingSpree == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsCovenant => (UseCovenant == "with Cooldowns" || UseCovenant == "with Cooldowns or AoE" || UseCovenant == "on mobcount or Cooldowns") && IsCooldowns || UseCovenant == "always" || (UseCovenant == "on AOE" || UseCovenant == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseCovenant == "on mobcount or Cooldowns" || UseCovenant == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsTrinkets1 => ((UseTrinket1 == "with Cooldowns" || UseTrinket1 == "with Cooldowns or AoE" || UseTrinket1 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket1 == "always" || (UseTrinket1 == "on AOE" || UseTrinket1 == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseTrinket1 == "on mobcount or Cooldowns" || UseTrinket1 == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount) && IsMelee;
        bool IsTrinkets2 => ((UseTrinket2 == "with Cooldowns" || UseTrinket2 == "with Cooldowns or AoE" || UseTrinket2 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket2 == "always" || (UseTrinket2 == "on AOE" || UseTrinket2 == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseTrinket2 == "on mobcount or Cooldowns" || UseTrinket2 == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount) && IsMelee;
        bool isTricks => UseTricks == "always" || UseTricks == "on AoE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || UseTricks == "on mobcount" && API.PlayerUnitInMeleeRangeCount >= MobCount;
        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public string[] Poison1 = new string[] { "Not Used", "Instant Poison", "Wound Poison" };
        public string[] Poison2 = new string[] { "Not Used", "Crippling Poison", "Numbing Poison" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "with Cooldowns or AoE", "on mobcount", "on mobcount or Cooldowns", "always" };
        public string[] Legendary = new string[] { "No Legendary", "Master Assassin's Mark", "Tiny Toxic Blade", "Celerity Loop", "Deathly Shadows" };
        public string[] Starter = new string[] { "Rota", "Cheap Shot" };
        public string[] TricksList = new string[] { "always", "on AoE", "on mobcount" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        private int MobCount => numbRaidList[CombatRoutine.GetPropertyInt("MobCount")];
        private string UseTricks => TricksList[CombatRoutine.GetPropertyInt(TricksoftheTrade)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UsePoison1 => Poison1[CombatRoutine.GetPropertyInt("First Poison")];
        private string UsePoison2 => Poison2[CombatRoutine.GetPropertyInt("Second Poison")];
        private string UseKillingSpree => CDUsageWithAOE[CombatRoutine.GetPropertyInt(KillingSpree)];
        private string UseDreadblades => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Dreadblades)];
        private string UseAdrenalineRush => CDUsageWithAOE[CombatRoutine.GetPropertyInt(AdrenalineRush)];
        private string UseBladeRush => CDUsageWithAOE[CombatRoutine.GetPropertyInt(BladeRush)];
        private string UseVanish => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Vanish)];
        private string IsLegendary => Legendary[CombatRoutine.GetPropertyInt("IsLegendary")];
        private string IsStarter => Starter[CombatRoutine.GetPropertyInt("IsStarter")];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private int CloakofShadowsLifePercent => numbList[CombatRoutine.GetPropertyInt(CloakofShadows)];
        private int CrimsonVialLifePercent => numbList[CombatRoutine.GetPropertyInt(CrimsonVial)];
        private int EvasionLifePercent => numbList[CombatRoutine.GetPropertyInt(Evasion)];
        private int FeintLifePercent => numbList[CombatRoutine.GetPropertyInt(Feint)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        //actions+=/variable,name=rtb_reroll,value=rtb_buffs<2&(!buff.true_bearing.up&!buff.broadside.up)
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
            if (RolltheBonesCount < 2 && (!API.PlayerHasBuff(TrueBearing) && !API.PlayerHasBuff(Broadside)))
                return true;
            return false;
        }
        //actions+=/variable,name=ambush_condition,value=combo_points.deficit>=2+buff.broadside.up&energy>=50&(!conduit.count_the_odds|buff.roll_the_bones.remains>=10)
        public bool ambush_condition()
        {
            int RolltheBonesRemain = 0;
            foreach (var buff in RtBBuffs)
            {
                if (API.PlayerHasBuff(buff))
                {
                    RolltheBonesRemain = API.PlayerBuffTimeRemaining(buff);
                }
            }
            if (ComboPointDeficit >= 2 + (API.PlayerHasBuff(Broadside) ? 1 : 0) && API.PlayerEnergy >= 50 && RolltheBonesRemain >= 1000) return true;
            return false;
        }
        //actions+=/variable,name=finish_condition,value=combo_points>=cp_max_spend-buff.broadside.up-(buff.opportunity.up*talent.quick_draw.enabled)|combo_points=animacharged_cp
        public bool finish_conditions => API.PlayerComboPoints >= MaxComboPoints - (API.PlayerHasBuff(Broadside) ? 1 : 0) - ((API.PlayerHasBuff(Opportunity) ? 1 : 0) * (TalentQuickDraw ? 1 : 0));
        bool dotsliceanddiceerefreshable => (API.PlayerBuffTimeRemaining(SliceandDice) < 1080 && !TalentDeeperStratagem || API.PlayerBuffTimeRemaining(SliceandDice) < 1260 && TalentDeeperStratagem);
        //actions.cds+=/variable,name=killing_spree_vanish_sync,value=!runeforge.mark_of_the_master_assassin|cooldown.vanish.remains>10|master_assassin_remains>2
        bool killing_spree_vanish_sync => IsLegendary != "Master Assassin's Mark" || API.SpellCDDuration(Vanish) > 1000 || API.PlayerBuffTimeRemaining(MasterAssassinsMark) > 200;
        //actions+=/variable,name=blade_flurry_sync,value=spell_targets.blade_flurry<2&raid_event.adds.in>20|buff.blade_flurry.remains>1+talent.killing_spree.enabled
        bool blade_flurry_sync => API.PlayerUnitInMeleeRangeCount < 2 || API.PlayerBuffTimeRemaining(BladeFlurry) > 1 + (TalentKillingSpree ? 1 : 0);
        //actions.cds+=/variable,name=vanish_ma_condition,if=runeforge.mark_of_the_master_assassin&!talent.marked_for_death.enabled,value=(!cooldown.between_the_eyes.ready&variable.finish_condition)|(cooldown.between_the_eyes.ready&variable.ambush_condition)
        bool vanish_ma_condition => IsLegendary == "Master Assassin's Mark" && ((API.SpellCDDuration(BetweentheEyes) <= GCD && finish_conditions) || API.SpellCDDuration(BetweentheEyes) <= GCD && ambush_condition());
        public override void Initialize()
        {
            CombatRoutine.Name = "Outlaw Rogue by smartie";
            API.WriteLog("Welcome to smartie`s Outlaw Rogue v1.4");
            API.WriteLog("You need the following macros:");
            API.WriteLog("Serrated Bone SpikeMO - /cast [@mouseover] Serrated Bone Spike");
            API.WriteLog("Tricks - /cast [@focus,help][help] Tricks of the Trade");

            //Spells
            CombatRoutine.AddSpell(SinisterStrike, 193315, "D1");
            CombatRoutine.AddSpell(SliceandDice, 315496, "D1");
            CombatRoutine.AddSpell(AdrenalineRush, 13750, "D1");
            CombatRoutine.AddSpell(PistolShot, 185763, "D1");
            CombatRoutine.AddSpell(BladeFlurry, 13877, "D1");
            CombatRoutine.AddSpell(RolltheBones, 315508, "D1");
            CombatRoutine.AddSpell(BetweentheEyes, 315341, "D1");
            CombatRoutine.AddSpell(Dispatch, 2098, "D1");
            CombatRoutine.AddSpell(Kick, 1766, "D1");
            CombatRoutine.AddSpell(Blind, 2094);
            CombatRoutine.AddSpell(KidneyShot, 408);
            CombatRoutine.AddSpell(Ambush, 8676, "D1");
            CombatRoutine.AddSpell(Stealth, 1784, "D1");
            CombatRoutine.AddSpell(Vanish, 1856, "D1");
            CombatRoutine.AddSpell(CheapShot, 1833, "D1");
            CombatRoutine.AddSpell(CloakofShadows, 31224, "D1");
            CombatRoutine.AddSpell(CrimsonVial, 185311, "D1");
            CombatRoutine.AddSpell(Feint, 1966, "D1");
            CombatRoutine.AddSpell(Evasion, 5277, "D1");
            CombatRoutine.AddSpell(KillingSpree, 51690, "D1");
            CombatRoutine.AddSpell(MarkedforDeath, 137619, "D1");
            CombatRoutine.AddSpell(BladeRush, 271877, "D1");
            CombatRoutine.AddSpell(GhostlyStrike, 196937, "D1");
            CombatRoutine.AddSpell(Dreadblades, 343142, "D1");
            CombatRoutine.AddSpell(EchoingReprimand, 323547);
            CombatRoutine.AddSpell(SerratedBoneSpike, 328547);
            CombatRoutine.AddSpell(Flagellation, 323654);
            CombatRoutine.AddSpell(Sepsis, 328305);
            CombatRoutine.AddSpell(CripplingPoison, 3408);
            CombatRoutine.AddSpell(InstantPoison, 315584);
            CombatRoutine.AddSpell(WoundPoison, 8679);
            CombatRoutine.AddSpell(NumbingPoison, 5761);
            CombatRoutine.AddSpell(Gouge, 1776);
            CombatRoutine.AddSpell(Shiv, 5938);
            CombatRoutine.AddSpell(TricksoftheTrade, 57934);

            //Macros
            CombatRoutine.AddMacro(SerratedBoneSpike + "MO");
            CombatRoutine.AddMacro("Trinket1");
            CombatRoutine.AddMacro("Trinket2");

            //Buffs
            CombatRoutine.AddBuff(AdrenalineRush, 13750);
            CombatRoutine.AddBuff(SliceandDice, 315496);
            CombatRoutine.AddBuff(BladeFlurry, 13877);
            CombatRoutine.AddBuff(Stealth, 1784);
            CombatRoutine.AddBuff(Vanish, 11327);
            CombatRoutine.AddBuff(Opportunity, 195627);
            CombatRoutine.AddBuff(LoadedDice, 256170);
            CombatRoutine.AddBuff(RuthlessPrecision, 193357);
            CombatRoutine.AddBuff(GrandMelee, 193358);
            CombatRoutine.AddBuff(Broadside, 193356);
            CombatRoutine.AddBuff(SkullandCrossbones, 199603);
            CombatRoutine.AddBuff(BuriedTreasure, 199600);
            CombatRoutine.AddBuff(TrueBearing, 193359);
            CombatRoutine.AddBuff(Sepsis, 328305);
            CombatRoutine.AddBuff(SepsisBuff, 347037);
            CombatRoutine.AddBuff(EchoingReprimand, 323547);
            CombatRoutine.AddBuff(Flagellation, 323654);
            CombatRoutine.AddBuff(SerratedBoneSpike, 328547);
            CombatRoutine.AddBuff(Fleshcraft, 324631);
            CombatRoutine.AddBuff(Soulshape, 310143);
            CombatRoutine.AddBuff(TricksoftheTrade, 59628);
            CombatRoutine.AddBuff(GreenskinsWickers, 340573);
            CombatRoutine.AddBuff(ConcealedBlunderbuss, 340587);
            CombatRoutine.AddBuff(MasterAssassinsMark, 340094);
            CombatRoutine.AddBuff(DeathlyShadows, 341202);
            CombatRoutine.AddBuff(WoundPoison, 8679);
            CombatRoutine.AddBuff(NumbingPoison, 5761);
            CombatRoutine.AddBuff(CripplingPoison, 3408);
            CombatRoutine.AddBuff(InstantPoison, 315584);
            CombatRoutine.AddBuff(Dreadblades, 343142);
            CombatRoutine.AddBuff(Feint, 1966);

            //Debuffs
            CombatRoutine.AddDebuff(Flagellation, 323654);
            CombatRoutine.AddDebuff(SerratedBoneSpike, 324073);
            CombatRoutine.AddDebuff(BetweentheEyes, 315341);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Auto Stealth");

            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //CB Properties
            CombatRoutine.AddProp("MobCount", "Mobcount to use Cooldowns ", numbRaidList, " Mobcount to use Cooldowns", "Cooldowns", 3);
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(TricksoftheTrade, "Use " + TricksoftheTrade, TricksList, "Use " + TricksoftheTrade + " always, on AoE or mobcount", "Generic", 0);
            CombatRoutine.AddProp(KillingSpree, "Use " + KillingSpree, CDUsageWithAOE, "Use " + KillingSpree + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Dreadblades, "Use " + Dreadblades, CDUsageWithAOE, "Use " + Dreadblades + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(AdrenalineRush, "Use " + AdrenalineRush, CDUsageWithAOE, "Use " + AdrenalineRush + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(BladeRush, "Use " + BladeRush, CDUsageWithAOE, "Use " + BladeRush + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Vanish, "Use " + Vanish, CDUsageWithAOE, "Use " + Vanish + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("IsLegendary", "Choose your Legendary", Legendary, "Please choose your Legendary", "Generic");
            CombatRoutine.AddProp("IsStarter", "Choose the Ability to start combat with", Starter, "Note this is only for Master Assassin Talent", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(CloakofShadows, CloakofShadows + " Life Percent", numbList, "Life percent at which" + CloakofShadows + "is used, set to 0 to disable", "Defense", 20);
            CombatRoutine.AddProp(CrimsonVial, CrimsonVial + " Life Percent", numbList, "Life percent at which" + CrimsonVial + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Evasion, Evasion + " Life Percent", numbList, "Life percent at which" + Evasion + "is used, set to 0 to disable", "Defense", 10);
            CombatRoutine.AddProp(Feint, Feint + " Life Percent", numbList, "Life percent at which" + Feint + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp("First Poison", "First Poison", Poison1, " Choose your first Poison", "Poisons", 0);
            CombatRoutine.AddProp("Second Poison", "Second Poison", Poison2, " Choose your second Poison", "Poisons", 0);

            RtBBuffs = new List<string>(new string[] { Broadside, BuriedTreasure, GrandMelee, RuthlessPrecision, SkullandCrossbones, TrueBearing });
        }

        //Rotation
        public override void Pulse()
        {
            //API.WriteLog("Combo Points?: " + API.PlayerComboPoints);
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 0 || API.PlayerSpellonCursor)
                return;
            if (!API.PlayerIsMounted)
            {
                if (API.CanCast(CripplingPoison) && UsePoison2 == "Crippling Poison" && API.PlayerBuffTimeRemaining(CripplingPoison) < 30000 && !API.PlayerIsMoving && API.LastSpellCastInGame != CripplingPoison && API.PlayerCurrentCastSpellID != 3408)
                {
                    API.CastSpell(CripplingPoison);
                    return;
                }
                if (API.CanCast(InstantPoison) && UsePoison1 == "Instant Poison" && API.PlayerBuffTimeRemaining(InstantPoison) < 30000 && !API.PlayerIsMoving && API.LastSpellCastInGame != InstantPoison && API.PlayerCurrentCastSpellID != 315584)
                {
                    API.CastSpell(InstantPoison);
                    return;
                }
                if (API.CanCast(NumbingPoison) && UsePoison2 == "Numbing Poison" && API.PlayerBuffTimeRemaining(NumbingPoison) < 30000 && !API.PlayerIsMoving && API.LastSpellCastInGame != NumbingPoison && API.PlayerCurrentCastSpellID != 5761)
                {
                    API.CastSpell(NumbingPoison);
                    return;
                }
                if (API.CanCast(WoundPoison) && UsePoison1 == "Wound Poison" && API.PlayerBuffTimeRemaining(WoundPoison) < 30000 && !API.PlayerIsMoving && API.LastSpellCastInGame != WoundPoison && API.PlayerCurrentCastSpellID != 8679)
                {
                    API.CastSpell(WoundPoison);
                    return;
                }
                if (API.CanCast(Stealth) && !IsStealth && IsAutoStealth)
                {
                    API.CastSpell(Stealth);
                    return;
                }
                if (API.PlayerHealthPercent <= CrimsonVialLifePercent && API.CanCast(CrimsonVial) && API.PlayerEnergy >= 20)
                {
                    API.CastSpell(CrimsonVial);
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            if (!API.PlayerIsMounted)
            {
                if (isInterrupt && API.CanCast(Kick) && IsMelee)
                {
                    API.CastSpell(Kick);
                    return;
                }
                if (API.CanCast(KidneyShot) && isInterrupt && IsMelee && API.SpellISOnCooldown(Kick))
                {
                    API.CastSpell(KidneyShot);
                    return;
                }
                if (API.CanCast(Blind) && isInterrupt && IsMelee && API.SpellISOnCooldown(Kick))
                {
                    API.CastSpell(Blind);
                    return;
                }
                if (API.PlayerHealthPercent <= CloakofShadowsLifePercent && API.CanCast(CloakofShadows))
                {
                    API.CastSpell(CloakofShadows);
                    return;
                }
                if (API.PlayerHealthPercent <= CrimsonVialLifePercent && API.CanCast(CrimsonVial) && API.PlayerEnergy >= 20)
                {
                    API.CastSpell(CrimsonVial);
                    return;
                }
                if (API.PlayerHealthPercent <= EvasionLifePercent && API.CanCast(Evasion))
                {
                    API.CastSpell(Evasion);
                    return;
                }
                if (API.PlayerHealthPercent <= FeintLifePercent && API.CanCast(Feint) && !API.PlayerHasBuff(Feint) && API.PlayerEnergy >= 35)
                {
                    API.CastSpell(Feint);
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
                if (API.CanCast(CripplingPoison) && UsePoison2 == "Crippling Poison" && API.PlayerBuffTimeRemaining(CripplingPoison) < 3000 && !API.PlayerIsMoving && API.LastSpellCastInGame != CripplingPoison && API.PlayerCurrentCastSpellID != 3408)
                {
                    API.CastSpell(CripplingPoison);
                    return;
                }
                if (API.CanCast(InstantPoison) && UsePoison1 == "Instant Poison" && API.PlayerBuffTimeRemaining(InstantPoison) < 3000 && !API.PlayerIsMoving && API.LastSpellCastInGame != InstantPoison && API.PlayerCurrentCastSpellID != 315584)
                {
                    API.CastSpell(InstantPoison);
                    return;
                }
                if (API.CanCast(NumbingPoison) && UsePoison2 == "Numbing Poison" && API.PlayerBuffTimeRemaining(NumbingPoison) < 3000 && !API.PlayerIsMoving && API.LastSpellCastInGame != NumbingPoison && API.PlayerCurrentCastSpellID != 5761)
                {
                    API.CastSpell(NumbingPoison);
                    return;
                }
                if (API.CanCast(WoundPoison) && UsePoison1 == "Wound Poison" && API.PlayerBuffTimeRemaining(WoundPoison) < 3000 && !API.PlayerIsMoving && API.LastSpellCastInGame != WoundPoison && API.PlayerCurrentCastSpellID != 8679)
                {
                    API.CastSpell(WoundPoison);
                    return;
                }
                //Focus
                if (API.CanCast(TricksoftheTrade) && isTricks && API.FocusRange < 100 && API.FocusHealthPercent != 0 && IsMelee && !API.PlayerHasBuff(TricksoftheTrade))
                {
                    API.CastSpell(TricksoftheTrade);
                    return;
                }
                rotation();
                return;
            }
        }
        private void rotation()
        {
            //actions+=/run_action_list,name=stealth,if=stealthed.all
            if (IsStealth && IsStarter == "Rota")
            {
                //actions.stealth=dispatch,if=variable.finish_condition
                if (API.CanCast(Dispatch) && IsMelee && finish_conditions)
                {
                    API.CastSpell(Dispatch);
                    return;
                }
                //actions.stealth+=/ambush
                if (API.CanCast(Ambush) && IsMelee)
                {
                    API.CastSpell(Ambush);
                    return;
                }
            }
            if (IsStealth && IsStarter == "Cheap Shot")
            {
                if (API.CanCast(CheapShot) && IsMelee && (API.PlayerEnergy >= 28 && !TalentDirtyTricks || TalentDirtyTricks))
                {
                    API.CastSpell(CheapShot);
                    return;
                }
            }
            if (!IsStealth)
            {
                //actions+=/call_action_list,name=cds
                //actions.cds=blade_flurry,if=spell_targets>=2&!buff.blade_flurry.up
                if (API.CanCast(BladeFlurry) && IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsMelee && !API.PlayerHasBuff(BladeFlurry))
                {
                    API.CastSpell(BladeFlurry);
                    return;
                }
                //actions.cds+=/vanish,if=!runeforge.mark_of_the_master_assassin&!stealthed.all&variable.ambush_condition&(!runeforge.deathly_shadows|buff.deathly_shadows.down&combo_points<=2)
                if (API.CanCast(Vanish) && IsMelee && !IsStealth && ambush_condition() && IsVanish && IsLegendary != "Master Assassin's Mark")
                {
                    API.CastSpell(Vanish);
                    return;
                }
                //actions.cds+=/vanish,if=variable.vanish_ma_condition&master_assassin_remains=0&variable.blade_flurry_sync
                if (API.CanCast(Vanish) && IsMelee && !IsStealth && IsVanish && API.PlayerBuffTimeRemaining(MasterAssassinsMark) == 0 && vanish_ma_condition && blade_flurry_sync)
                {
                    API.CastSpell(Vanish);
                    return;
                }
                //actions.cds+=/adrenaline_rush,if=!buff.adrenaline_rush.up
                if (API.CanCast(AdrenalineRush) && IsMelee && IsAdrenalineRush && !API.PlayerHasBuff(AdrenalineRush))
                {
                    API.CastSpell(AdrenalineRush);
                    return;
                }
                //actions.cds +=/ flagellation,if= !stealthed.all & (variable.finish_condition | target.time_to_die < 13)
                if (API.CanCast(Flagellation) && IsCovenant && PlayerCovenantSettings == "Venthyr" && IsMelee && !IsStealth && (finish_conditions || API.TargetTimeToDie < 1300))
                {
                    API.CastSpell(Flagellation);
                    return;
                }
                //actions.cds+=/dreadblades,if=!stealthed.all&combo_points<=2&(!covenant.venthyr|debuff.flagellation.up)
                if (API.CanCast(Dreadblades) && IsMelee && !IsStealth && API.PlayerComboPoints <= 2 && IsDreadblades && TalentDreadblades && (PlayerCovenantSettings != "Venthyr" || API.TargetHasDebuff(Flagellation)))
                {
                    API.CastSpell(Dreadblades);
                    return;
                }
                //actions.cds+=/roll_the_bones,if=master_assassin_remains=0&buff.dreadblades.down&(buff.roll_the_bones.remains<=3|variable.rtb_reroll)
                if (API.CanCast(RolltheBones) && ShouldRolltheBones() && API.PlayerEnergy >= 25 && API.TargetRange <= 20)
                {
                    API.CastSpell(RolltheBones);
                    return;
                }
                //actions.cds+=/marked_for_death,line_cd=1.5,target_if=min:target.time_to_die,if=raid_event.adds.up&(target.time_to_die<combo_points.deficit|!stealthed.rogue&combo_points.deficit>=cp_max_spend-1)
                if (API.CanCast(MarkedforDeath) && TalentMarkedForDeath && !IsStealth && ComboPointDeficit >= MaxComboPoints - 1 && API.TargetRange <= 30)
                {
                    API.CastSpell(MarkedforDeath);
                    return;
                }
                //actions.cds+=/killing_spree,if=variable.blade_flurry_sync&variable.killing_spree_vanish_sync&!stealthed.rogue&(debuff.between_the_eyes.up&buff.dreadblades.down&energy.deficit>(energy.regen*2+15)|spell_targets.blade_flurry>(2-buff.deathly_shadows.up)|master_assassin_remains>0)
                if (API.CanCast(KillingSpree) && blade_flurry_sync && killing_spree_vanish_sync && !IsStealth && IsKillingSpree && (API.TargetHasDebuff(BetweentheEyes) && !API.PlayerHasBuff(Dreadblades) && EnergyDefecit > (EnergyRegen * 2 + 15) || API.PlayerUnitInMeleeRangeCount > (2 - (API.PlayerHasBuff(DeathlyShadows) ? 1 : 0)) || API.PlayerBuffTimeRemaining(MasterAssassinsMark) > 0))
                {
                    API.CastSpell(KillingSpree);
                    return;
                }
                //actions.cds+=/blade_rush,if=variable.blade_flurry_sync&(energy.time_to_max>2&buff.dreadblades.down|energy<=30|spell_targets>2)
                if (API.CanCast(BladeRush) && blade_flurry_sync && TalentBladeRush && (TimeUntilMaxEnergy > 200 && !API.PlayerHasBuff(Dreadblades) || API.PlayerEnergy <= 30 || API.PlayerUnitInMeleeRangeCount > 2) && IsBladeRush)
                {
                    API.CastSpell(BladeRush);
                    return;
                }
                //actions.cds+=/shadowmeld,if=!stealthed.all&variable.ambush_condition
                if (PlayerRaceSettings == "Night Elf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee && !IsStealth && ambush_condition())
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.cds+=/blood_fury
                if (PlayerRaceSettings == "Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.cds +=/ berserking
                if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.cds +=/ fireblood
                if (PlayerRaceSettings == "Dark Iron Dwarf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.cds +=/ ancestral_call
                if (PlayerRaceSettings == "Mag'har Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.cds+=/use_items,slots=trinket1,if=debuff.between_the_eyes.up|trinket.1.has_stat.any_dps|fight_remains<=20
                if (API.PlayerTrinketIsUsable(1) && !API.MacroIsIgnored("Trinket1") && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
                {
                    API.CastSpell("Trinket1");
                    return;
                }
                //actions.cds+=/use_items,slots=trinket2,if=debuff.between_the_eyes.up|trinket.2.has_stat.any_dps|fight_remains<=20
                if (API.PlayerTrinketIsUsable(2) && !API.MacroIsIgnored("Trinket2") && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
                {
                    API.CastSpell("Trinket2");
                    return;
                }
                //actions+=/run_action_list,name=finish,if=variable.finish_condition
                if (finish_conditions)
                {
                    //actions.finish=between_the_eyes,if=target.time_to_die>3
                    if (API.CanCast(BetweentheEyes) && API.TargetRange <= 20 && API.TargetTimeToDie > 300)
                    {
                        API.CastSpell(BetweentheEyes);
                        return;
                    }
                    //actions.finish+=/slice_and_dice,if=buff.slice_and_dice.remains<fight_remains&refreshable
                    if (API.CanCast(SliceandDice) && API.TargetRange <= 20 && dotsliceanddiceerefreshable)
                    {
                        API.CastSpell(SliceandDice);
                        return;
                    }
                    //actions.finish+=/dispatch
                    if (API.CanCast(Dispatch) && IsMelee)
                    {
                        API.CastSpell(Dispatch);
                        return;
                    }
                }
                //actions+=/call_action_list,name=build
                //actions.build=sepsis
                if (API.CanCast(Sepsis) && PlayerCovenantSettings == "Night Fae" && IsMelee && IsCovenant && API.PlayerEnergy >= 25 && ComboPointDeficit >= 1)
                {
                    API.CastSpell(Sepsis);
                    return;
                }
                //actions.build+=/ghostly_strike
                if (API.CanCast(GhostlyStrike) && TalentGhostlyStrike && API.PlayerEnergy >= 30 && IsMelee)
                {
                    API.CastSpell(GhostlyStrike);
                    return;
                }
                //actions.build+=/shiv,if=runeforge.tiny_toxic_blade
                if (API.CanCast(Shiv) && IsLegendary == "Tiny Toxic Blade" && API.PlayerEnergy >= 20 && IsMelee)
                {
                    API.CastSpell(Shiv);
                    return;
                }
                //actions.build+=/echoing_reprimand
                if (API.CanCast(EchoingReprimand) && IsCovenant && PlayerCovenantSettings == "Kyrian" && IsMelee)
                {
                    API.CastSpell(EchoingReprimand);
                    return;
                }
                //actions.build+=/serrated_bone_spike,cycle_targets=1,if=buff.slice_and_dice.up&!dot.serrated_bone_spike_dot.ticking|fight_remains<=5|cooldown.serrated_bone_spike.charges_fractional>=2.75
                if (API.CanCast(SerratedBoneSpike) && IsCovenant && IsMelee && PlayerCovenantSettings == "Necrolord" && (API.PlayerHasBuff(SliceandDice) && !API.TargetHasDebuff(SerratedBoneSpike) || API.SpellCharges(SerratedBoneSpike) > 2.75))
                {
                    API.CastSpell(SerratedBoneSpike);
                    return;
                }
                if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                {
                    if (API.CanCast(SerratedBoneSpike) && !API.MacroIsIgnored(SerratedBoneSpike + "MO") && IsCovenant && PlayerCovenantSettings == "Necrolord" && API.MouseoverRange < 30 && (API.PlayerHasBuff(SliceandDice) && !API.MouseoverHasDebuff(SerratedBoneSpike) || API.SpellCharges(SerratedBoneSpike) > 2.75))
                    {
                        API.CastSpell(SerratedBoneSpike + "MO");
                        return;
                    }
                }
                //actions.build+=/pistol_shot,if=buff.opportunity.up&(energy.deficit>(energy.regen+10)|combo_points.deficit<=1+buff.broadside.up|talent.quick_draw.enabled)
                if (API.CanCast(PistolShot) && API.PlayerEnergy >= 40 && API.TargetRange <= 20 && API.PlayerHasBuff(Opportunity) && (EnergyDefecit > (EnergyRegen + 10) || ComboPointDeficit <= 1 + (API.PlayerHasBuff(Broadside) ? 1 : 0) || TalentQuickDraw))
                {
                    API.CastSpell(PistolShot);
                    return;
                }
                //actions.build+=/pistol_shot,if=buff.opportunity.up&(buff.greenskins_wickers.up|buff.concealed_blunderbuss.up)
                if (API.CanCast(PistolShot) && API.PlayerEnergy >= 40 && API.TargetRange <= 20 && API.PlayerHasBuff(Opportunity) && (API.PlayerHasBuff(GreenskinsWickers) || API.PlayerHasBuff(ConcealedBlunderbuss)))
                {
                    API.CastSpell(PistolShot);
                    return;
                }
                //actions.build+=/sinister_strike
                if (API.CanCast(SinisterStrike) && API.PlayerEnergy >= 45 && IsMelee)
                {
                    API.CastSpell(SinisterStrike);
                    return;
                }
                //actions.build+=/gouge,if=talent.dirty_tricks.enabled&combo_points.deficit>=1+buff.broadside.up
                /*if (API.CanCast(Gouge) && TalentDirtyTricks && IsMelee && ComboPointDeficit >= 1 + (API.PlayerHasBuff(Broadside) ? 1 : 0))
                {
                    API.CastSpell(Gouge);
                    return;
                }*/
                //actions+=/arcane_torrent,if=energy.deficit>=15+energy.regen
                if (PlayerRaceSettings == "Bloodelf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee && EnergyDefecit >= 15 + EnergyRegen)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/arcane_pulse
                if (PlayerRaceSettings == "Nightborne" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/lights_judgment
                if (PlayerRaceSettings == "Lightforged" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/bag_of_tricks
                if (PlayerRaceSettings == "Vulpera" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
            }
        }
    }
}

