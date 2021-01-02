using System.Linq;

namespace HyperElk.Core
{

    public class HolyPally : CombatRoutine
    {

        //Spell Strings
        private string DivineShield = "Divine Shield";
        private string DivineProtection = "Divine Protection";
        private string HolyShock = "Holy Shock";
        private string HolyShockHealing = "Holy Shock Healing";
        private string HolyLight = "Holy Light";
        private string FoL = "Flash of Light";
        private string WoG = "Word of Glory";
        private string BoS = "Blessing of Sacrifice";
        private string LoH = "Lay on Hands";
        private string BF = "Bestow Faith";
        private string LoD = "Light of Dawn";
        private string HoLI = "Holy Light /w Infusion of Light";
        private string FoLI = "Flash of Light /w Infusion of Light";
        private string BoV = "Beacon of Virtue";
        private string CrusaderStrike = "Crusader Strike";
        private string Judgment = "Judgment";
        private string AvengingWrath = "Avenging Wrath";
        private string HolyAvenger = "Holy Avenger";
        private string AuraMastery = "Aura Mastery";
        private string HammerofJustice = "Hammer of Justice";
        private string Cons = "Consecration";
        private string Seraphim = "Seraphim";
        private string HammerofWrath = "Hammer of Wrath";
        private string HolyPrism = "Holy Prism";
        private string LightsHammer = "Light's Hammer";
        private string Forbearance = "Forbearance";
        private string Infusion = "Infusion of Light";
        private string AvengingCrusader = "Avenging Crusader";
        private string PartySwap = "Target Swap";
        private string Fleshcraft = "Fleshcraft";
        private string DivineToll = "Divine Toll";
        private string DivineTollHealing = "Divine Toll Healing";
        private string VanqusihersHammer = "Vanquisher's Hammer";
        private string AshenHallow = "Ashen Hallow";
        private string LoTM = "Light of the Martyr";
        private string LoTMH = "Light of the Martyr Health";
        private string LoTMM = "Light of the Martyr Moving";
        private string AoE = "AOE";
        private string Party1 = "party1";
        private string Party2 = "party2";
        private string Party3 = "party3";
        private string Party4 = "party4";
        private string Player = "player";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        private string AoEDPS = "AOEDPS";
        private string AoEDPSRaid = "AOEDPS Raid";
        private string AoEDPSH = "AOEDPS Health";
        private string AoEDPSHRaid = "AOEDPS Health Raid";
        private string TargetChange = "Target Change";
        private string AoERaid = "AOE Healing Raid";
        private string WOGMax = "Word of Glory Healing at Max HP";
        private string LODMax = "Light of Dawn Healing at Max HP";
        private string TargetHostile = "Target Last Hostile";
        private string RuleofLaw = "Rule of Law";
        private string GlimmerofLight = "Glimmer of Light";
        private string ShockBarrier = "Shock Barrier";
        private string HolyShockLeggoSpread = "Holy Shock Legendary Barrier Spread";

        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";

        private string CrusaderAura = "Crusader Aura";
        private string DevotionAura = "Devotion Aura";


        //Talents
        bool CrusadersMight => API.PlayerIsTalentSelected(1, 1);
        bool BestowFaith => API.PlayerIsTalentSelected(1, 2);
        bool LightsHammerT => API.PlayerIsTalentSelected(1, 3);
        bool Savedbythelight => API.PlayerIsTalentSelected(2, 1);
        bool JudgementofLight => API.PlayerIsTalentSelected(2, 2);
        bool HolyPrismT => API.PlayerIsTalentSelected(2, 3);
        bool FistofJustice => API.PlayerIsTalentSelected(3, 1);
        bool Repentance => API.PlayerIsTalentSelected(3, 2);
        bool BlindingLight => API.PlayerIsTalentSelected(3, 3);
        bool UnbreakableSpirit => API.PlayerIsTalentSelected(4, 1);
        bool Calvalier => API.PlayerIsTalentSelected(4, 2);
        bool RuleofLawTalent => API.PlayerIsTalentSelected(4, 3);
        bool DivinePurpose => API.PlayerIsTalentSelected(5, 1);
        bool HolyAvengerT => API.PlayerIsTalentSelected(5, 2);
        bool SeraphimT => API.PlayerIsTalentSelected(5, 3);
        bool SancifiedWrath => API.PlayerIsTalentSelected(6, 1);
        bool AvengingCrusaderT => API.PlayerIsTalentSelected(6, 2);
        bool Awakening => API.PlayerIsTalentSelected(6, 3);
        bool GlimmerofLightTalent => API.PlayerIsTalentSelected(7, 1);
        bool BeaconofFaith => API.PlayerIsTalentSelected(7, 2);
        bool BeaconofVirtue => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        public string[] LegendaryList = new string[] { "None", "Shock Barrier"};
        private string UseLeg => LegendaryList[CombatRoutine.GetPropertyInt("Legendary")];
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        string[] units = { "player", "party1", "party2", "party3", "party4" };
        string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        int PlayerHealth => API.TargetHealthPercent;
        string[] PlayerTargetArray = { "player", "party1", "party2", "party3", "party4" };
        string[] RaidTargetArray = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };


        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);
        private int BuffRaidTracking(string buff) => raidunits.Count(p => API.UnitHasBuff(buff, p));
        private int BuffPartyTracking(string buff) => units.Count(p => API.UnitHasBuff(buff, p));

        private bool GlimmerTracking => API.PlayerIsInRaid ? BuffRaidTracking(GlimmerofLight) <= 8 : BuffPartyTracking(GlimmerofLight) <= 5;
        private bool DPSHealthCheck => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(AoEDPSHRaidLifePercent) >= AoEDPSRaidNumber : UnitBelowHealthPercentParty(AoEDPSHLifePercent) >= AoEDPSNumber;
        private bool DPSMouseOver => API.MouseoverHealthPercent <= AoEDPSHLifePercent;
        private bool LoDAoE => UnitBelowHealthPercent(LoDLifePercent) >= AoENumber;
        private bool LoDMaxAoE => UnitBelowHealthPercent(LoDMaxLifePercent) >= AoENumber;
        private bool BoVAoE => UnitBelowHealthPercent(BoVLifePercent) >= AoENumber;
        private bool HPAoE => UnitBelowHealthPercent(HPLifePercent) >= AoENumber;
        private bool DTAoE => UnitBelowHealthPercent(DTLifePercent) >= AoENumber;
        private bool AMAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(AMLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(AMLifePercent) >= AoENumber;
        private bool LHAoE => UnitBelowHealthPercent(LightsHammerLifePercent) >= AoENumber;

        private bool LodParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= LoDLifePercent && units.Count(p => API.UnitHealthPercent(p) <= LoDLifePercent) >= 3;
        }

        private bool LoDParty1 => LodParty(1);
        private bool LoDRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= LoDLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= LoDLifePercent) >= 3;
        }

        private bool LoDRaid1 => LoDRaid(1);
        private bool BoVParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= BoVLifePercent && units.Count(p => API.UnitHealthPercent(p) <= BoVLifePercent) >= 3;
        }

        private bool BoVParty1 => BoVParty(1);
        private bool BoVRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= BoVLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= BoVLifePercent) >= 3;
        }

        private bool BoVRaid1 => BoVRaid(1);
        private bool HPParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= HPLifePercent && units.Count(p => API.UnitHealthPercent(p) <= HPLifePercent) >= 3;
        }

        private bool HPParty1 => HPParty(1);
        private bool HPRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= HPLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= HPLifePercent) >= 3;

        }
        private bool HPRaid1 => HPRaid(1);
        private bool DTParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= DTLifePercent && units.Count(p => API.UnitHealthPercent(p) <= DTLifePercent) >= 3;
        }

        private bool DTParty1 => DTParty(1);
        private bool DTRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= DTLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= DTLifePercent) >= 3;

        }
        private bool DTRaid1 => DTRaid(1);
        private bool IsAutoSwap => API.ToggleIsEnabled("Auto Target");
        private bool PlayerSwap => API.UnitHealthPercent(Player) <= PartySwapPercent;
        private bool Party1Swap => API.UnitHealthPercent(Party1) <= PartySwapPercent;
        private bool Party2Swap => API.UnitHealthPercent(Party2) <= PartySwapPercent;
        private bool Party3Swap => API.UnitHealthPercent(Party3) <= PartySwapPercent;
        private bool Party4Swap => API.UnitHealthPercent(Party4) <= PartySwapPercent;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private int DivineShieldLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineShield)];
        private int DivineProtectionLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineProtection)];
        private int HolyShockLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyShock)];
        private int LightsHammerLifePercent => numbList[CombatRoutine.GetPropertyInt(LightsHammer)];
        private int HolyLightLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyLight)];
        private int FoLLifePercent => numbList[CombatRoutine.GetPropertyInt(FoL)];
        private int WoGLifePercent => numbList[CombatRoutine.GetPropertyInt(WoG)];
        private int WoGMaxLifePercent => numbList[CombatRoutine.GetPropertyInt(WOGMax)];
        private int BoSLifePercent => numbList[CombatRoutine.GetPropertyInt(BoS)];
        private int FolILifePercent => numbList[CombatRoutine.GetPropertyInt(FoLI)];
        private int HoLILifePercent => numbList[CombatRoutine.GetPropertyInt(HoLI)];
        private int LoHLifePercent => numbList[CombatRoutine.GetPropertyInt(LoH)];
        private int BFLifePercent => numbList[CombatRoutine.GetPropertyInt(BF)];
        private int LoDLifePercent => numbList[CombatRoutine.GetPropertyInt(LoD)];
        private int LoDMaxLifePercent => numbList[CombatRoutine.GetPropertyInt(LODMax)];
        private int BoVLifePercent => numbList[CombatRoutine.GetPropertyInt(BoV)];
        private int HPLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyPrism)];
        private int PartySwapPercent => numbList[CombatRoutine.GetPropertyInt(PartySwap)];
        private int TargetChangePercent => numbList[CombatRoutine.GetPropertyInt(TargetChange)];
        private int DTLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineToll)];
        private int LoTMHealthPercent => numbList[CombatRoutine.GetPropertyInt(LoTMH)];
        private int AMLifePercent => numbList[CombatRoutine.GetPropertyInt(AuraMastery)];
        private int LoTMLifePercent => numbList[CombatRoutine.GetPropertyInt(LoTM)];
        private int LoTMMovingLifePercent => numbList[CombatRoutine.GetPropertyInt(LoTMM)];
        private int AoEDPSHLifePercent => numbList[CombatRoutine.GetPropertyInt(AoEDPSH)];
        private int AoEDPSHRaidLifePercent => numbList[CombatRoutine.GetPropertyInt(AoEDPSHRaid)];
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoERaid)];
        private int AoEDPSNumber => numbPartyList[CombatRoutine.GetPropertyInt(AoEDPS)];
        private int AoEDPSRaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoEDPSRaid)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        //General
        private int Level => API.PlayerLevel;
        private bool InRange => IsMouseover ? API.MouseoverRange <= 40 : API.TargetRange <= 40 || API.PlayerHasBuff(RuleofLaw) && IsMouseover ? API.MouseoverRange <= 60 : API.TargetRange <= 60;
        private bool IsMelee => API.TargetRange < 6;

        private bool IsMoMelee = API.MouseoverRange < 6;
       // private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private int Trinket1Usage => CombatRoutine.GetPropertyInt("Trinket1");
        private int Trinket2Usage => CombatRoutine.GetPropertyInt("Trinket2");
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool DTHealing => CombatRoutine.GetPropertyBool(DivineTollHealing);
        private bool HSHealing => CombatRoutine.GetPropertyBool(HolyShockHealing);
        private bool HSLeggo => CombatRoutine.GetPropertyBool(HolyShockHealing);
        private bool IsOOC => CombatRoutine.GetPropertyBool("OOC");
        private bool AutoAuraSwitch => CombatRoutine.GetPropertyBool("Aura Switch");
        
        //Spell Check Bools
        private bool LoTMCheck => API.CanCast(LoTM)  && InRange && !API.PlayerCanAttackTarget && !API.PlayerIsTargetTarget && (API.PlayerIsMoving && API.TargetHealthPercent <= LoTMMovingLifePercent || API.TargetHealthPercent <= LoTMLifePercent) && API.PlayerHealthPercent >= LoTMHealthPercent && API.TargetHealthPercent > 0;
        private bool LoTMCheckMO => API.CanCast(LoTM) && IsMouseover && InRange && !API.PlayerCanAttackMouseover && !API.PlayerIsTargetTarget && (API.PlayerIsMoving && API.MouseoverHealthPercent <= LoTMMovingLifePercent || API.MouseoverHealthPercent <= LoTMLifePercent) && API.PlayerHealthPercent >= LoTMHealthPercent && API.MouseoverHealthPercent > 0;
        private bool HolyShockCheck => API.CanCast(HolyShock) && InRange  && (API.TargetHealthPercent <= HolyShockLifePercent || GlimmerTracking && !API.TargetHasBuff(GlimmerofLight) && GlimmerofLightTalent || UseLeg == "Shock Barrier" && GlimmerTracking && GlimmerofLightTalent && !API.TargetHasBuff(ShockBarrier) && HSLeggo && (API.TargetHasBuff(GlimmerofLight) || UseLeg == "Shock Barrier" && !API.TargetHasBuff(ShockBarrier) && HSLeggo)) && API.TargetHealthPercent > 0 && !API.PlayerCanAttackTarget && API.PlayerCurrentHolyPower <= 4;
        private bool HolyShockCheckMO => API.CanCast(HolyShock) && InRange && IsMouseover && (API.MouseoverHealthPercent <= HolyShockLifePercent || GlimmerTracking && !API.MouseoverHasBuff(GlimmerofLight) && GlimmerofLightTalent || UseLeg == "Shock Barrier" && GlimmerTracking && GlimmerofLightTalent && !API.MouseoverHasBuff(ShockBarrier) && HSLeggo && (API.MouseoverHasBuff(GlimmerofLight)) || UseLeg == "Shock Barrier" && HSLeggo && !API.MouseoverHasBuff(ShockBarrier)) && API.MouseoverHealthPercent > 0 && !API.PlayerCanAttackMouseover && API.PlayerCurrentHolyPower <= 4;
        private bool HolyLightCheck => API.CanCast(HolyLight) && InRange  && API.TargetHealthPercent <= HolyLightLifePercent && API.TargetHealthPercent > 0 && !API.PlayerIsMoving && !API.PlayerCanAttackTarget;
        private bool HolyLightCheckMO => API.CanCast(HolyLight) && InRange && IsMouseover && API.MouseoverHealthPercent <= HolyLightLifePercent && API.MouseoverHealthPercent > 0 && !API.PlayerIsMoving && !API.PlayerCanAttackMouseover;
        private bool HolyLightInfusionCheck => API.CanCast(HolyLight) && API.PlayerHasBuff(Infusion) && InRange  && API.TargetHealthPercent <= HoLILifePercent && API.TargetHealthPercent > 0 && !API.PlayerIsMoving && !API.PlayerCanAttackTarget;
        private bool HolyLightInfusionCheckMO => API.CanCast(HolyLight) && API.PlayerHasBuff(Infusion) && IsMouseover && InRange && API.MouseoverHealthPercent <= HoLILifePercent && API.MouseoverHealthPercent > 0 && !API.PlayerIsMoving && !API.PlayerCanAttackMouseover;
        private bool FlashofLightCheck => API.CanCast(FoL) && InRange  && API.TargetHealthPercent <= FoLLifePercent && API.TargetHealthPercent > 0 && !API.PlayerIsMoving && !API.PlayerCanAttackTarget;
        private bool FlashofLightCheckMO => API.CanCast(FoL) && InRange && IsMouseover && API.MouseoverHealthPercent <= FoLLifePercent && API.MouseoverHealthPercent > 0 && !API.PlayerIsMoving && !API.PlayerCanAttackMouseover;
        private bool FlashofLightInfusionCheck => API.CanCast(FoL) && API.PlayerHasBuff(Infusion) && InRange  && API.TargetHealthPercent <= FolILifePercent && API.TargetHealthPercent > 0 && !API.PlayerIsMoving && !API.PlayerCanAttackTarget;
        private bool FlashofLightInfusionCheckMO => API.CanCast(FoL) && API.PlayerHasBuff(Infusion) && IsMouseover && InRange && API.MouseoverHealthPercent <= FolILifePercent && API.MouseoverHealthPercent > 0 && !API.PlayerIsMoving && !API.PlayerCanAttackMouseover;
        private bool WoGCheck => API.CanCast(WoG) && InRange  && API.PlayerCurrentHolyPower >= 3 && (API.TargetHealthPercent <= WoGLifePercent || API.TargetHealthPercent <= WoGMaxLifePercent && API.PlayerCurrentHolyPower == 5) && API.TargetHealthPercent > 0 && !API.PlayerCanAttackTarget;
        private bool WoGCheckMO => API.CanCast(WoG) && InRange && IsMouseover && API.PlayerCurrentHolyPower >= 3 && (API.MouseoverHealthPercent <= WoGLifePercent || API.MouseoverHealthPercent <= WoGMaxLifePercent && API.PlayerCurrentHolyPower == 5) && API.MouseoverHealthPercent > 0 && !API.PlayerCanAttackMouseover;
        private bool BFCheck => API.CanCast(BF) && BestowFaith && InRange  && API.TargetHealthPercent <= BFLifePercent && API.TargetHealthPercent > 0 && !API.PlayerCanAttackTarget;
        private bool BFCheckMO => API.CanCast(BF) && BestowFaith && InRange && IsMouseover && API.MouseoverHealthPercent <= BFLifePercent && API.MouseoverHealthPercent > 0 && !API.PlayerCanAttackMouseover;
        private bool LoDCheck => API.CanCast(LoD) && (IsMouseover || !IsMouseover) && API.PlayerCurrentHolyPower >= 3 && (API.TargetRange <= 15 || API.MouseoverRange <= 15) && (LoDAoE || LoDMaxAoE && API.PlayerCurrentHolyPower == 5) && (API.TargetHealthPercent > 0 || API.MouseoverHealthPercent > 0) && (API.PlayerCanAttackTarget || !API.PlayerCanAttackTarget || API.PlayerCanAttackMouseover || !API.PlayerCanAttackMouseover);
        private bool BoVCheck => API.CanCast(BoV) && BeaconofVirtue  && InRange && BoVAoE && API.TargetHealthPercent > 0 && !API.PlayerCanAttackTarget;
        private bool BoVCheckMO => API.CanCast(BoV) && BeaconofVirtue && InRange && IsMouseover && BoVAoE && API.MouseoverHealthPercent > 0 && !API.PlayerCanAttackMouseover;
        private bool DTCheck => API.CanCast(DivineToll) && DTAoE && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && InRange;
        private bool DTCheckMO => API.CanCast(DivineToll) && IsMouseover && DTAoE && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0 && InRange;
        private bool HolyPrismCheck => API.CanCast(HolyPrism)  && HolyPrismT && InRange && HPAoE && API.TargetHealthPercent > 0 && !API.PlayerCanAttackTarget;
        private bool HolyPrismCheckMO => API.CanCast(HolyPrism) && HolyPrismT && InRange && IsMouseover && HPAoE && API.MouseoverHealthPercent > 0 && !API.PlayerCanAttackMouseover;
        private bool LoHCheck => API.CanCast(LoH) && InRange  && API.TargetHealthPercent <= LoHLifePercent && API.TargetHealthPercent > 0 && !API.TargetHasBuff(Forbearance) && !API.PlayerCanAttackTarget;
        private bool LoHCheckMO => API.CanCast(LoH) && InRange && IsMouseover && API.MouseoverHealthPercent <= LoHLifePercent && API.MouseoverHealthPercent > 0 && !API.TargetHasBuff(Forbearance) && !API.PlayerCanAttackMouseover;
        private bool BoSCheck => API.CanCast(BoS) && InRange  && API.TargetHealthPercent <= BoSLifePercent && API.TargetHealthPercent > 0 && !API.PlayerCanAttackTarget && !API.PlayerIsTargetTarget;
        private bool BoSCheckMO => API.CanCast(BoS) && InRange && IsMouseover && API.MouseoverHealthPercent <= BoSLifePercent && API.MouseoverHealthPercent > 0 && !API.PlayerCanAttackTarget && !API.PlayerIsTargetTarget;
        private bool AuraMasteryCheck => API.CanCast(AuraMastery) && InRange && (IsMouseover || !IsMouseover) && AMAoE && (API.TargetHealthPercent > 0 || API.MouseoverHealthPercent > 0) && (API.PlayerCanAttackTarget || !API.PlayerCanAttackTarget || API.PlayerCanAttackMouseover || !API.PlayerCanAttackMouseover);
        //Auto Target Checks
        private bool LoHAutoCheck => API.CanCast(LoH) && InRange && !API.TargetHasBuff(Forbearance);

        //  public bool isInterrupt => CombatRoutine.GetPropertyBool("KICK") && API.TargetCanInterrupted && API.TargetIsCasting && (API.TargetIsChanneling ? API.TargetElapsedCastTime >= interruptDelay : API.TargetCurrentCastTimeRemaining <= interruptDelay);
        //  public int interruptDelay => random.Next((int)(CombatRoutine.GetPropertyInt("KICKTime") * 0.9), (int)(CombatRoutine.GetPropertyInt("KICKTime") * 1.1));

        public override void Initialize()
        {
            CombatRoutine.Name = "Holy Pally by Ryu";
            API.WriteLog("Welcome to Holy Pally by Ryu");
            API.WriteLog("Be advised this a Beta Rotation: Out of Combat Healing is turned on by default. Please use your Pause key when not in a party.");
            API.WriteLog("All Talents expect PVP Talents and Row 3 talents are supported. All Cooldowns are associated with Cooldown toggle.");
            API.WriteLog("For all ground spells, either use @Cursor or when it is time to place it, the Bot will pause until you've placed it. If you'd perfer to use your own logic for them, please place them on ignore in the spellbook.");
            API.WriteLog("Maunual targeting, Auto Tareting, or Mouseover Supported. You need to create /cast [@mouseover] xxxx where xxx is each of the spells that have MO in the bindings in order for Mouseover to work");
            API.WriteLog("Venthyr and Night Fae Cov's are not supported. You can create a /xxx break marco to use those abilties when you would like at this time.");
            API.WriteLog("If you wish to use Auto Target, please set your WoW keybinds in the keybinds => Targeting for Self, Party, and Target Last Hostile and then match them to the Macro's's in the spell book. Enable it the Toggles. You must at least have a target for it swap, friendly or enemy. It will NOT swap back to Enemeies. This works for both raid and party, however, you must set up the binds. Please watch video in the Discord");
            //Buff
            CombatRoutine.AddBuff(Infusion, 54149);
            CombatRoutine.AddBuff(AvengingWrath, 31884);
            CombatRoutine.AddBuff(AvengingCrusader, 216331);
            CombatRoutine.AddBuff(Forbearance, 25771);
            CombatRoutine.AddBuff(CrusaderAura, 32223);
            CombatRoutine.AddBuff(DevotionAura, 465);
            CombatRoutine.AddBuff(RuleofLaw, 214202);
            CombatRoutine.AddBuff(GlimmerofLight, 287280);
            CombatRoutine.AddBuff(ShockBarrier, 337824);

            //Debuff
            CombatRoutine.AddDebuff(Forbearance, 25771);
            CombatRoutine.AddDebuff(Cons, 26573);

            //Spell
            CombatRoutine.AddSpell(HolyShock, 20473, "D2");
            CombatRoutine.AddSpell(FoL, 19750, "D4");
            CombatRoutine.AddSpell(HolyLight, 82326, "D3");
            CombatRoutine.AddSpell(CrusaderStrike, 35395, "D6");
            CombatRoutine.AddSpell(Judgment, 275773, "D5");
            CombatRoutine.AddSpell(LoD, 85222, "R");
            CombatRoutine.AddSpell(AvengingWrath, 31884, "F2");
            CombatRoutine.AddSpell(HolyAvenger, 105809, "F8");
            CombatRoutine.AddSpell(AuraMastery, 31821, "T");
            CombatRoutine.AddSpell(BoS, 6940, "F1");
            CombatRoutine.AddSpell(LoH, 633, "D8");
            CombatRoutine.AddSpell(BoV, 200025, "None");
            CombatRoutine.AddSpell(DivineShield, 642, "D0");
            CombatRoutine.AddSpell(BF, 223306, "None");
            CombatRoutine.AddSpell(HammerofJustice, 853, "F");
            CombatRoutine.AddSpell(Cons, 26573, "None");
            CombatRoutine.AddSpell(DivineProtection, 498, "None");
            CombatRoutine.AddSpell(HammerofWrath, 24275, "None");
            CombatRoutine.AddSpell(Seraphim, 152262, "None");
            CombatRoutine.AddSpell(AvengingCrusader, 216331, "F2");
            CombatRoutine.AddSpell(HolyPrism, 114165, "None");
            CombatRoutine.AddSpell(WoG, 85673, "None");
            CombatRoutine.AddSpell(LoTM, 183998);
            CombatRoutine.AddSpell(CrusaderAura, 32223);
            CombatRoutine.AddSpell(DevotionAura, 465);
            CombatRoutine.AddSpell(Fleshcraft, 324631);
            CombatRoutine.AddSpell(DivineToll, 304971);
            CombatRoutine.AddSpell(VanqusihersHammer, 328204);
            CombatRoutine.AddSpell(AshenHallow, 316958);
            CombatRoutine.AddSpell(RuleofLaw, 214202);
            

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Auto Target");

            //Mouseover
            CombatRoutine.AddMacro(HolyLight + "MO", "None");
            CombatRoutine.AddMacro(FoL + "MO", "None");
            CombatRoutine.AddMacro(HolyShock + "MO", "None");
            CombatRoutine.AddMacro(BoS + "MO", "None");
            CombatRoutine.AddMacro(LoH + "MO", "None");
            CombatRoutine.AddMacro(BoV + "MO", "None");
            CombatRoutine.AddMacro(HolyPrism + "MO", "None");
            CombatRoutine.AddMacro(LoD + "MO", "R");
            CombatRoutine.AddMacro(CrusaderStrike + "MO");
            CombatRoutine.AddMacro(Judgment + "MO");
            CombatRoutine.AddMacro(HammerofWrath + "MO");
            CombatRoutine.AddMacro(DivineToll + "MO");
            CombatRoutine.AddMacro(LoTM + "MO");
            CombatRoutine.AddMacro(WoG + "MO");
            CombatRoutine.AddMacro(BF + "MO");
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);
            CombatRoutine.AddMacro(TargetHostile);
            CombatRoutine.AddMacro(Player);
            CombatRoutine.AddMacro(Party1);
            CombatRoutine.AddMacro(Party2);
            CombatRoutine.AddMacro(Party3);
            CombatRoutine.AddMacro(Party4);
            CombatRoutine.AddMacro("raid1");
            CombatRoutine.AddMacro("raid2");
            CombatRoutine.AddMacro("raid3");
            CombatRoutine.AddMacro("raid4");
            CombatRoutine.AddMacro("raid5");
            CombatRoutine.AddMacro("raid6");
            CombatRoutine.AddMacro("raid7");
            CombatRoutine.AddMacro("raid8");
            CombatRoutine.AddMacro("raid9");
            CombatRoutine.AddMacro("raid10");
            CombatRoutine.AddMacro("raid11");
            CombatRoutine.AddMacro("raid12");
            CombatRoutine.AddMacro("raid13");
            CombatRoutine.AddMacro("raid14");
            CombatRoutine.AddMacro("raid15");
            CombatRoutine.AddMacro("raid16");
            CombatRoutine.AddMacro("raid17");
            CombatRoutine.AddMacro("raid18");
            CombatRoutine.AddMacro("raid19");
            CombatRoutine.AddMacro("raid20");
            CombatRoutine.AddMacro("raid21");
            CombatRoutine.AddMacro("raid22");
            CombatRoutine.AddMacro("raid23");
            CombatRoutine.AddMacro("raid24");
            CombatRoutine.AddMacro("raid25");
            CombatRoutine.AddMacro("raid26");
            CombatRoutine.AddMacro("raid27");
            CombatRoutine.AddMacro("raid28");
            CombatRoutine.AddMacro("raid29");
            CombatRoutine.AddMacro("raid30");
            CombatRoutine.AddMacro("raid31");
            CombatRoutine.AddMacro("raid32");
            CombatRoutine.AddMacro("raid33");
            CombatRoutine.AddMacro("raid34");
            CombatRoutine.AddMacro("raid35");
            CombatRoutine.AddMacro("raid36");
            CombatRoutine.AddMacro("raid37");
            CombatRoutine.AddMacro("raid38");
            CombatRoutine.AddMacro("raid39");
            CombatRoutine.AddMacro("raid40");




            //Prop
            CombatRoutine.AddProp(DivineShield, DivineShield + " Life Percent", numbList, "Life percent at which" + DivineShield + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(DivineProtection, DivineProtection + " Life Percent", numbList, "Life percent at which" + DivineProtection + "is used, set to 0 to disable", "Defense", 50);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 0);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("Aura Switch", "Auto Aura Switch", false, "Auto Switch Aura between Crusader Aura|Devotion Aura", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);

            CombatRoutine.AddProp("Legendary", "Select your Legendary", LegendaryList, "Select Your Legendary", "Legendary");

            CombatRoutine.AddProp("OOC", "Healing out of Combat", true, "Heal out of combat", "Healing");
          //  CombatRoutine.AddProp(PartySwap, PartySwap + " Life Percent", numbList, "Life percent at which" + PartySwap + "is used, set to 0 to disable", "Healing", 0);
         //   CombatRoutine.AddProp(TargetChange, TargetChange + " Life Percent", numbList, "Life percent at which" + TargetChange + "is used to change from your current target, when using Auto Swap logic, set to 0 to disable", "Healing", 0);
            CombatRoutine.AddProp(HolyShock, HolyShock + " Life Percent", numbList, "Life percent at which" + HolyShock + "is used, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(HolyShockHealing, HolyShock, true, "If Holy Shock should be on Healing, if for both, change to false, set to true by default for healing", "Healing");
            CombatRoutine.AddProp(HolyShockLeggoSpread, HolyShock, false, "If Shock barrier should should be spread at max health, set to false by default", "Healing");
            CombatRoutine.AddProp(HolyLight, HolyLight + " Life Percent", numbList, "Life percent at which" + HolyLight + "is used, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(LoTM, LoTM + " Life Percent", numbList, "Life percent at which" + LoTM + "is used, set to 0 to disable", "Healing", 60);
            CombatRoutine.AddProp(LoTMH, LoTM + " Player Health Percent", numbList, "Player Health percent at which" + LoTM + "is used, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(LoTMM, LoTM + " Player Health Percent", numbList, "Target Health percent at which" + LoTM + "is used while moving, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(HoLI, HoLI + " Life Percent", numbList, "Life percent at which" + HoLI + "is used, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(FoL, FoL + " Life Percent", numbList, "Life percent at which" + FoL + "is used, set to 0 to disable", "Healing", 75);
            CombatRoutine.AddProp(FoLI, FoLI + " Life Percent", numbList, "Life percent at which" + FoLI + "is used, set to 0 to disable", "Healing", 90);
            CombatRoutine.AddProp(WoG, WoG + " Life Percent", numbList, "Life percent at which" + WoG + "is used, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(WOGMax , WoG + " Life Percent", numbList, "Life percent at which" + WoG + "is used when at max holy power, set to 0 to disable", "Healing", 100);
            CombatRoutine.AddProp(BoS, BoS + " Life Percent", numbList, "Life percent at which" + BoS + "is used, set to 0 to disable", "Healing", 20);
            CombatRoutine.AddProp(LoH, LoH + " Life Percent", numbList, "Life percent at which" + LoH + "is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(BF, BF + " Life Percent", numbList, "Life percent at which" + BF + "is used, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(LoD, LoD + " Life Percent", numbList, "Life percent at which" + LoD + "is used when three members are at life percent, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(LODMax, LoD + " Life Percent", numbList, "Life percent at which" + LoD + "is used when three members are at life percent and you have max holy power, set to 0 to disable", "Healing", 90);
            CombatRoutine.AddProp(BoV, BoV + " Life Percent", numbList, "Life percent at which" + BoV + "is used when three members are at life percent, set to 0 to disable", "Healing", 75);
            CombatRoutine.AddProp(LightsHammer, LightsHammer + " Life Percent", numbList, "Life percent at which" + LightsHammer + "is used when three members are at life percent, set to 0 to disable", "Healing", 75);
            CombatRoutine.AddProp(AuraMastery, AuraMastery + " Life Percent", numbList, "Life percent at which" + AuraMastery + "is used, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(HolyPrism, HolyPrism + " Life Percent", numbList, "Life percent at which" + HolyPrism + "is used when three members are at life percent, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(DivineToll, DivineToll + " Life Percent", numbList, "Life percent at which" + DivineToll + "is used when three members are at life percent, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(DivineTollHealing, DivineToll, true, "If Divine Toll should be on Healing, if for both, change to false, set to true by default for healing", "Healing");
            CombatRoutine.AddProp(AoE, "Number of units for AoE Healing ", numbPartyList, " Units for AoE Healing", "Healing", 3);
            CombatRoutine.AddProp(AoERaid, "Number of units for AoE Healing in raid ", numbRaidList, " Units for AoE Healing in raid", "Healing", 7);
          //  CombatRoutine.AddProp(AoEDPS, "Number of units needed to be above DPS Health Percent to DPS ", numbPartyList, " Units above for DPS ", "Healing", 2);
          //  CombatRoutine.AddProp(AoEDPSH, "Life Percent for units to be above for DPS", numbList, "Health percent at which DPS" + "is used,", "Healing", 50);
          //  CombatRoutine.AddProp(AoEDPSRaid, "Number of units needed to be above DPS Health Percent to DPS in Raid for Targeting to previous hostile ", numbRaidList, " Units above for DPS ", "Healing", 2);
          //  CombatRoutine.AddProp(AoEDPSHRaid, "Life Percent for units to be above for DPS in raid for targeting to previous hostile", numbList, "Health percent at which DPS" + "is used,", "Healing", 50);

            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", CDUsage, "When should trinket1 be used", "Trinket", 0);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", CDUsage, "When should trinket1 be used", "Trinket", 0);

            //                 if (PlayerSwap && API.TargetHealthPercent <= PartySwapPercent)
            //{
            //    API.CastSpell(Player);
            //    API.WriteLog("Player Health : " + API.UnitHealthPercent(Player));
            //}
            //if (Party1Swap && API.TargetHealthPercent <= PartySwapPercent)
            // {
            //     API.CastSpell(Party1);
            //     API.WriteLog("Party1 Health : " + API.UnitHealthPercent(Party1));
            // }
            // if (Party2Swap && API.TargetHealthPercent <= PartySwapPercent)
            // {
            //     API.CastSpell(Party2);
            //     API.WriteLog("Party2 Health : " + API.UnitHealthPercent(Party2));
            // }
            // if (Party3Swap && API.TargetHealthPercent <= PartySwapPercent)
            // {
            //     API.CastSpell(Party3);
            //     API.WriteLog("Party3 Health : " + API.UnitHealthPercent(Party3));
            // }
            // if (Party4Swap && API.TargetHealthPercent <= PartySwapPercent)
            // {
            //  API.CastSpell(Party4);
            //   API.WriteLog("Party4 Health : " + API.UnitHealthPercent(Party4));
            //}
        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted && (IsOOC || API.PlayerIsInCombat))
            {
                if (API.PlayerIsMounted)
                {
                    if (AutoAuraSwitch && API.CanCast(CrusaderAura) && Level >= 21 && !API.PlayerHasBuff(CrusaderAura))
                    {
                        API.CastSpell(CrusaderAura);
                        return;
                    }
                }
                else
                {

                    if (AutoAuraSwitch && API.CanCast(DevotionAura) && Level >= 21 && !API.PlayerHasBuff(DevotionAura))
                    {
                        API.CastSpell(DevotionAura);
                        return;
                    }
                }
                if (Trinket1Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                    API.CastSpell(trinket1);
                if (Trinket1Usage == 2 && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                    API.CastSpell(trinket1);
                if (Trinket2Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                    API.CastSpell(trinket2);
                if (Trinket2Usage == 2 && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                    API.CastSpell(trinket2);
                if (API.CanCast(AvengingWrath) && !API.PlayerHasBuff(AvengingWrath) && InRange && IsCooldowns)
                {
                    API.CastSpell(AvengingWrath);
                    return;
                }
                if (API.CanCast(AvengingCrusader) && AvengingCrusaderT && !API.PlayerHasBuff(AvengingCrusader) && InRange && IsCooldowns)
                {
                    API.CastSpell(AvengingCrusader);
                    return;
                }
                if (API.CanCast(HolyAvenger) && HolyAvengerT && InRange && IsCooldowns)
                {
                    API.CastSpell(HolyAvenger);
                    return;
                }
                if (API.CanCast(Seraphim) && SeraphimT && InRange && IsCooldowns && API.PlayerCurrentHolyPower >= 3)
                {
                    API.CastSpell(Seraphim);
                    return;
                }
                if (API.CanCast(DivineShield) && API.PlayerHealthPercent <= DivineShieldLifePercent && API.PlayerHealthPercent > 0 && !API.PlayerHasDebuff(Forbearance))
                {
                    API.CastSpell(DivineShield);
                    return;
                }
                if (API.CanCast(DivineProtection) && API.PlayerHealthPercent <= DivineProtectionLifePercent && API.PlayerHealthPercent > 0)
                {
                    API.CastSpell(DivineProtection);
                    return;
                }
                //Healing
                if (API.CanCast(RuleofLaw) && RuleofLawTalent && !API.PlayerHasBuff(RuleofLaw) && (API.TargetRange > 40 || IsMouseover && API.MouseoverRange > 40))
                {
                    API.CastSpell(RuleofLaw);
                    return;
                }
                if (API.CanCast(LightsHammer) && LightsHammerT && InRange && LHAoE)
                {
                    API.CastSpell(LightsHammer);
                    return;
                }
                if (AuraMasteryCheck)
                {
                    API.CastSpell(AuraMastery);
                    return;
                }
                if (DTCheck)
                {
                    API.CastSpell(DivineToll);
                    return;
                }
                if (DTCheckMO)
                {
                    API.CastSpell(DivineToll + "MO");
                    return;
                }
                if (BoVCheck)
                {
                    API.CastSpell(BoV);
                    return;
                }
                if (BoVCheckMO)
                {
                    API.CastSpell(BoV + "MO");
                    return;
                }
                if (HolyPrismCheck)
                {
                    API.CastSpell(HolyPrism);
                    return;
                }
                if (HolyPrismCheckMO)
                {
                    API.CastSpell(HolyPrism + "MO");
                    return;
                }
                if (LoDCheck)
                {
                    API.CastSpell(LoD);
                    return;
                }
                if (LoHCheck)
                {
                    API.CastSpell(LoH);
                    return;
                }
                if (LoHCheckMO)
                {
                    API.CastSpell(LoH + "MO");
                    return;
                }
                if (BoSCheck)
                {
                    API.CastSpell(BoS);
                    return;
                }
                if (BoSCheckMO)
                {
                    API.CastSpell(BoS + "MO");
                    return;
                }
                if (HolyShockCheck)
                {
                    API.CastSpell(HolyShock);
                    return;
                }
                if (HolyShockCheckMO)
                {
                    API.CastSpell(HolyShock + "MO");
                    return;
                }
                if (WoGCheck)
                {
                    API.CastSpell(WoG);
                    return;
                }
                if (WoGCheckMO)
                {
                    API.CastSpell(WoG + "MO");
                    return;
                }
                if (API.CanCast(CrusaderStrike) && CrusadersMight && API.SpellISOnCooldown(HolyShock) && IsMelee && API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(CrusaderStrike);
                    return;
                }
                if (API.CanCast(CrusaderStrike) && IsMouseover && CrusadersMight && API.SpellISOnCooldown(HolyShock) && IsMoMelee && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(CrusaderStrike + "MO");
                    return;
                }
                if (API.CanCast(CrusaderStrike) && Level <= 25 && !CrusadersMight && IsMelee && API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(CrusaderStrike);
                    return;
                }
                if (API.CanCast(CrusaderStrike) && IsMouseover && Level <= 25 && !CrusadersMight && IsMoMelee && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(CrusaderStrike + "MO");
                    return;
                }
                if (FlashofLightInfusionCheck)
                {
                    API.CastSpell(FoL);
                    return;
                }
                if (FlashofLightInfusionCheckMO)
                {
                    API.CastSpell(FoL + "MO");
                    return;
                }
                if (HolyLightInfusionCheck)
                {
                    API.CastSpell(HolyLight);
                    return;
                }
                if (HolyLightInfusionCheckMO)
                {
                    API.CastSpell(HolyLight + "MO");
                    return;
                }
                if (FlashofLightCheck)
                {
                    API.CastSpell(FoL);
                    return;
                }
                if (FlashofLightCheckMO)
                {
                    API.CastSpell(FoL + "MO");
                    return;
                }
                if (HolyLightCheck)
                {
                    API.CastSpell(HolyLight);
                    return;
                }
                if (HolyLightCheckMO)
                {
                    API.CastSpell(HolyLight + "MO");
                    return;
                }
                if (BFCheck)
                {
                    API.CastSpell(BF);
                    return;
                }
                if (BFCheckMO)
                {
                    API.CastSpell(BF + "MO");
                    return;
                }
                if (LoTMCheck)
                {
                    API.CastSpell(LoTM);
                    return;
                }
                if (LoTMCheckMO)
                {
                    API.CastSpell(LoTM + "MO");
                    return;
                }
                //DPS
                if (API.CanCast(Judgment) && InRange && API.PlayerCanAttackTarget && (JudgementofLight || !JudgementofLight) && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Judgment);
                    return;
                }
                if (API.CanCast(Judgment) && IsMouseover && InRange && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && (JudgementofLight || !JudgementofLight) && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(Judgment + "MO");
                    return;
                }
                if (API.CanCast(HolyShock) && !HSHealing && InRange && API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(HolyShock);
                    return;
                }
                if (API.CanCast(HolyShock) && !HSHealing && IsMouseover && InRange && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(HolyShock + "MO");
                    return;
                }
                if (API.CanCast(DivineToll) && !DTHealing && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && API.PlayerCanAttackTarget && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(DivineToll);
                    return;
                }
                if (API.CanCast(DivineToll) && IsMouseover && !DTHealing && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && API.PlayerCanAttackMouseover && InRange && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(DivineToll + "MO");
                    return;
                }
                if (API.CanCast(VanqusihersHammer) && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !DPSHealthCheck && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(VanqusihersHammer);
                    return;
                }
                if (API.CanCast(HammerofWrath) && API.PlayerCanAttackTarget && InRange && API.TargetHealthPercent <= 20 && Level >= 46 || (Level >= 58 && API.PlayerHasBuff(AvengingWrath) && InRange) && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(HammerofWrath);
                    return;
                }
                if (API.CanCast(HammerofWrath) && InRange && IsMouseover && API.MouseoverHealthPercent <= 20 && Level >= 46 || (Level >= 58 && API.PlayerHasBuff(AvengingWrath) && InRange) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(HammerofWrath + "MO");
                    return;
                }
                if (API.CanCast(Cons) && API.PlayerUnitInMeleeRangeCount >= 2 && IsMelee && !API.TargetHasDebuff(Cons) && !API.PlayerIsMoving && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Cons);
                    return;
                }
                if (API.CanCast(Cons) && API.PlayerUnitInMeleeRangeCount >= 2 && IsMouseover && IsMoMelee && !API.MouseoverHasDebuff(Cons) && !API.PlayerIsMoving && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(Cons + "MO");
                    return;
                }
                // Auto Target
                if (IsAutoSwap)
                {
                    if (API.PlayerIsInGroup)
                    {
                        for (int i = 0; i < units.Length; i++)
                        {
                            if (API.UnitHealthPercent(units[i]) <= LoHLifePercent && (PlayerHealth >= LoHLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= BoSLifePercent && (PlayerHealth >= BoSLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= WoGLifePercent && (PlayerHealth >= WoGLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= FolILifePercent && (PlayerHealth >= FolILifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= HoLILifePercent && (PlayerHealth >= HoLILifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= FoLLifePercent && (PlayerHealth >= FoLLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]); ;
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= HolyLightLifePercent && (PlayerHealth >= HolyLightLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= BFLifePercent && (PlayerHealth >= BFLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= LoTMLifePercent && (PlayerHealth >= LoTMLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= HolyShockLifePercent && (PlayerHealth >= HolyShockLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            //   if (DPSHealthCheck && (!API.PlayerCanAttackTarget || !API.PlayerCanAttackMouseover))
                            // {
                            //    API.CastSpell(TargetHostile);
                            //    return;
                            // }
                            // }
                        }
                        if (API.PlayerIsInRaid)
                        {
                            for (int i = 0; i < raidunits.Length; i++)
                            {
                                if (API.UnitHealthPercent(raidunits[i]) <= LoHLifePercent && (PlayerHealth >= LoHLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= BoSLifePercent && (PlayerHealth >= BoSLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= WoGLifePercent && (PlayerHealth >= WoGLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= FolILifePercent && (PlayerHealth >= FolILifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= HoLILifePercent && (PlayerHealth >= HoLILifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= FoLLifePercent && (PlayerHealth >= FoLLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]); ;
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= HolyLightLifePercent && (PlayerHealth >= HolyLightLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= BFLifePercent && (PlayerHealth >= BFLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= LoTMLifePercent && (PlayerHealth >= LoTMLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(raidunits[i]) <= HolyShockLifePercent && (PlayerHealth >= HolyShockLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                {
                                    API.CastSpell(RaidTargetArray[i]);
                                    return;
                                }
                            }
                        }
                    }
             }  }
        }
        public override void CombatPulse()
        {
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc)
            {
                API.CastSpell(Fleshcraft);
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

        }

        public override void OutOfCombatPulse()
        {

        }
        private void AutoTarget()
        {
            for (int i = 0; i < units.Length; i++)
            {
                if (API.UnitHealthPercent(units[i]) <= PartySwapPercent && (PlayerHealth >= TargetChangePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                {
                    API.CastSpell(PlayerTargetArray[i]);
                    API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    return;
                }
            }

            if (API.PlayerIsInRaid)
            {
                for (int i = 0; i < raidunits.Length; i++)
                {
                    if (API.UnitHealthPercent(raidunits[i]) <= PartySwapPercent && (PlayerHealth >= TargetChangePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                    {
                        API.CastSpell(RaidTargetArray[i]);
                        return;
                    }
                }
            }
        

        }
    }    
} 




