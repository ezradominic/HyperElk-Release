// Changelog
// v1.0 First release
// v1.1 small adjustments


using System.Diagnostics;

namespace HyperElk.Core
{
    public class AssaRogue : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsAutoStealth => API.ToggleIsEnabled("Auto Stealth");
        //Spell,Auras
        private string Garrote = "Garrote";
        private string FanofKnives = "Fan of Knives";
        private string PoisonedKnife = "Poisoned Knife";
        private string Envenom = "Envenom";
        private string Mutilate = "Mutilate";
        private string Rupture = "Rupture";
        private string Ambush = "Ambush";
        private string Shiv = "Shiv";
        private string SliceandDice = "Slice and Dice";
        private string Vendetta = "Vendetta";
        private string Exsanguinate = "Exsanguinate";
        private string Kick = "Kick";
        private string Blind = "Blind";
        private string KidneyShot = "Kidney Shot";
        private string TricksoftheTrade = "Tricks of the Trade";
        private string Feint = "Feint";
        private string Evasion = "Evasion";
        private string CrimsonVial = "Crimson Vial";
        private string CloakofShadows = "Cloak of Shadows";
        private string Stealth = "Stealth";
        private string Vanish = "Vanish";
        private string CheapShot = "Cheap Shot";
        private string CripplingPoison = "Crippling Poison";
        private string DeadlyPoison = "Deadly Poison";
        private string MarkedforDeath = "Marked for Death";
        private string CrimsonTempest = "Crimson Tempest";
        private string Sepsis = "Sepsis";
        private string SerratedBoneSpike = "Serrated Bone Spike";
        private string EchoingReprimand = "Echoing Reprimand";
        private string Flagellation = "Flagellation";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string Blindside = "Blindside";
        private string Nightstalker = "Nightstalker";
        private string Subterfuge = "Subterfuge";
        private string HiddenBlades = "Hidden Blades";
        private string MasterAssassin = "Master Assassin";
        private string WoundPoison = "Wound Poison";
        private string NumbingPoison = "Numbing Poison";


        //Talents
        bool TalentNightStalker => API.PlayerIsTalentSelected(2, 1);
        bool TalentSubterfuge => API.PlayerIsTalentSelected(2, 2);
        bool TalentMasterAssassin => API.PlayerIsTalentSelected(2, 3);
        bool TalentVigor => API.PlayerIsTalentSelected(3, 1);
        bool TalentDeeperStratagem => API.PlayerIsTalentSelected(3, 2);
        bool TalentMarkedforDeath => API.PlayerIsTalentSelected(3, 3);
        bool TalentExsanguinate => API.PlayerIsTalentSelected(6, 3);
        bool TalentCrimsonTempest => API.PlayerIsTalentSelected(7, 3);
        bool TalentPoisonBomb => API.PlayerIsTalentSelected(7, 1);


        //General
        private int PlayerLevel => API.PlayerLevel;
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        int EnergyDefecit => API.PlayeMaxEnergy - API.PlayerEnergy;
        float GCD => API.SpellGCDTotalDuration;
        float EnergyRegen => 10f * (1f + API.PlayerGetHaste);
        private bool isMelee => API.TargetRange < 6;
        bool IsVanish => (UseVanish == "with Cooldowns" || UseVanish == "with Cooldowns or AoE" || UseVanish == "on mobcount or Cooldowns") && IsCooldowns || UseVanish == "always" || (UseVanish == "on AOE" || UseVanish == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseVanish == "on mobcount or Cooldowns" || UseVanish == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsExsanguinate => (UseExsanguinate == "with Cooldowns" || UseExsanguinate == "with Cooldowns or AoE" || UseExsanguinate == "on mobcount or Cooldowns") && IsCooldowns || UseExsanguinate == "always" || (UseExsanguinate == "on AOE" || UseExsanguinate == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseExsanguinate == "on mobcount or Cooldowns" || UseExsanguinate == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsVendetta => (UseVendetta == "with Cooldowns" || UseVendetta == "with Cooldowns or AoE" || UseVendetta == "on mobcount or Cooldowns") && IsCooldowns || UseVendetta == "always" || (UseVendetta == "on AOE" || UseVendetta == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseVendetta == "on mobcount or Cooldowns" || UseVendetta == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsCovenant => (UseCovenant == "with Cooldowns" || UseCovenant == "with Cooldowns or AoE" || UseCovenant == "on mobcount or Cooldowns") && IsCooldowns || UseCovenant == "always" || (UseCovenant == "on AOE" || UseCovenant == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseCovenant == "on mobcount or Cooldowns" || UseCovenant == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsTrinkets1 => ((UseTrinket1 == "with Cooldowns" || UseTrinket1 == "with Cooldowns or AoE" || UseTrinket1 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket1 == "always" || (UseTrinket1 == "on AOE" || UseTrinket1 == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseTrinket1 == "on mobcount or Cooldowns" || UseTrinket1 == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount) && isMelee;
        bool IsTrinkets2 => ((UseTrinket2 == "with Cooldowns" || UseTrinket2 == "with Cooldowns or AoE" || UseTrinket2 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket2 == "always" || (UseTrinket2 == "on AOE" || UseTrinket2 == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseTrinket2 == "on mobcount or Cooldowns" || UseTrinket2 == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount) && isMelee;


        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public string[] Poison1 = new string[] { "Not Used", "Deadly Poison", "Wound Poison"};
        public string[] Poison2 = new string[] { "Not Used", "Crippling Poison", "Numbing Poison"};
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "with Cooldowns or AoE", "on mobcount", "on mobcount or Cooldowns", "always" };
        public string[] Legendary = new string[] { "No Legendary", "Doomblade", "Deathly Shadows" };
        public string[] Starter = new string[] { "Manual", "Ambush", "Cheap Shot" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        private int MobCount => numbRaidList[CombatRoutine.GetPropertyInt("MobCount")];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UsePoison1 => Poison1[CombatRoutine.GetPropertyInt("First Poison")];
        private string UsePoison2 => Poison2[CombatRoutine.GetPropertyInt("Second Poison")];

        private string UseVendetta => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Vendetta)];
        private string UseExsanguinate => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Exsanguinate)];
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
        bool dotrupturerefreshable => (API.TargetDebuffRemainingTime(Rupture) < 720 && !TalentDeeperStratagem || API.TargetDebuffRemainingTime(Rupture) < 840 && TalentDeeperStratagem);
        bool dotrupturerefreshableMO => (API.MouseoverDebuffRemainingTime(Rupture) < 720 && !TalentDeeperStratagem || API.MouseoverDebuffRemainingTime(Rupture) < 840 && TalentDeeperStratagem);
        bool dotGarroteerefreshable => API.TargetDebuffRemainingTime(Garrote) < 540;
        bool dotGarroteerefreshableMO => API.MouseoverDebuffRemainingTime(Garrote) < 540;
        bool dotcrimsontempestrefreshable => (API.TargetDebuffRemainingTime(CrimsonTempest) < 360 && !TalentDeeperStratagem || API.TargetDebuffRemainingTime(CrimsonTempest) < 420 && TalentDeeperStratagem);
        bool dotenvenomerefreshable => (API.TargetDebuffRemainingTime(Envenom) < 180 && !TalentDeeperStratagem || API.TargetDebuffRemainingTime(Envenom) < 210 && TalentDeeperStratagem);
        bool dotsliceanddiceerefreshable => (API.PlayerBuffTimeRemaining(SliceandDice) < 1080 && !TalentDeeperStratagem || API.PlayerBuffTimeRemaining(SliceandDice) < 1260 && TalentDeeperStratagem);
        bool IsStealth => PlayerHasBuff(Stealth) || PlayerHasBuff(Vanish);
        //actions.direct+=/variable,name=use_filler,value=combo_points.deficit>1|energy.deficit<=25+variable.energy_regen_combined|!variable.single_target
        bool usefiller => API.PlayerComboPoints < 5 || EnergyDefecit <= 25 || API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE;
        //actions.vanish=variable,name=nightstalker_cp_condition,value=(!runeforge.deathly_shadows&effective_combo_points>=cp_max_spend)|(runeforge.deathly_shadows&combo_points<2)
        bool nightstalkercpcondition => IsLegendary != "Deathly Shadows" && API.PlayerComboPoints >= 5 || IsLegendary == "Deathly Shadows" && API.PlayerComboPoints < 2;
        bool maxcp => API.PlayerComboPoints == 5 && !TalentDeeperStratagem || API.PlayerComboPoints == 6 && TalentDeeperStratagem;
        public override void Initialize()
        {
            CombatRoutine.Name = "Assassination Rogue by smartie";
            API.WriteLog("Welcome to smartie`s Assassination Rogue v1.1");
            API.WriteLog("You need the following macros:");
            API.WriteLog("GarroteMO - /cast [@mouseover] Garrote");
            API.WriteLog("RuptureMO - /cast [@mouseover] Rupture");
            API.WriteLog("Serrated Bone SpikeMO - /cast [@mouseover] Serrated Bone Spike");
            API.WriteLog("Tricks - /cast [@focus,help][help] Tricks of the Trade");


            //Spells
            CombatRoutine.AddSpell(Garrote, 703);
            CombatRoutine.AddSpell(FanofKnives, 51723);
            CombatRoutine.AddSpell(PoisonedKnife, 185565);
            CombatRoutine.AddSpell(Envenom, 32645);
            CombatRoutine.AddSpell(Mutilate, 1329);
            CombatRoutine.AddSpell(Rupture, 1943);
            CombatRoutine.AddSpell(Ambush, 8676);
            CombatRoutine.AddSpell(Shiv, 5938);
            CombatRoutine.AddSpell(SliceandDice, 315496);
            CombatRoutine.AddSpell(Vendetta, 79140);
            CombatRoutine.AddSpell(Exsanguinate, 200806);
            CombatRoutine.AddSpell(Kick, 1766);
            CombatRoutine.AddSpell(Blind, 2094);
            CombatRoutine.AddSpell(KidneyShot, 408);
            CombatRoutine.AddSpell(TricksoftheTrade, 57934);
            CombatRoutine.AddSpell(Feint, 1966);
            CombatRoutine.AddSpell(Evasion, 5277);
            CombatRoutine.AddSpell(CrimsonVial, 185311);
            CombatRoutine.AddSpell(CloakofShadows, 31224);
            CombatRoutine.AddSpell(Stealth, 1784);
            CombatRoutine.AddSpell(Vanish, 1856);
            CombatRoutine.AddSpell(CheapShot, 1833);
            CombatRoutine.AddSpell(CripplingPoison, 3408);
            CombatRoutine.AddSpell(DeadlyPoison, 2823);
            CombatRoutine.AddSpell(WoundPoison, 8679);
            CombatRoutine.AddSpell(NumbingPoison, 5761);
            CombatRoutine.AddSpell(MarkedforDeath, 137619);
            CombatRoutine.AddSpell(CrimsonTempest, 121411);
            CombatRoutine.AddSpell(Sepsis, 328305);
            CombatRoutine.AddSpell(SerratedBoneSpike, 328547);
            CombatRoutine.AddSpell(EchoingReprimand, 323547);
            CombatRoutine.AddSpell(Flagellation, 323654);

            //Macros
            CombatRoutine.AddMacro(Garrote + "MO");
            CombatRoutine.AddMacro(Rupture + "MO");
            CombatRoutine.AddMacro(SerratedBoneSpike + "MO");
            CombatRoutine.AddMacro("Trinket1");
            CombatRoutine.AddMacro("Trinket2");

            //Buffs
            CombatRoutine.AddBuff(Stealth, 1784);
            CombatRoutine.AddBuff(Vanish, 11327);
            CombatRoutine.AddBuff(CripplingPoison, 3408);
            CombatRoutine.AddBuff(DeadlyPoison, 2823);
            CombatRoutine.AddBuff(Feint, 1966);
            CombatRoutine.AddBuff(Blindside, 121153);
            CombatRoutine.AddBuff(SliceandDice, 315496);
            CombatRoutine.AddBuff(TricksoftheTrade, 59628);
            CombatRoutine.AddBuff(Envenom, 32645);
            CombatRoutine.AddBuff(HiddenBlades, 270070);
            CombatRoutine.AddBuff(MasterAssassin, 256735);
            CombatRoutine.AddBuff(Subterfuge, 115192);
            CombatRoutine.AddBuff(WoundPoison, 8679);
            CombatRoutine.AddBuff(NumbingPoison, 5761);

            //Debuff
            CombatRoutine.AddDebuff(SerratedBoneSpike, 324073);
            CombatRoutine.AddDebuff(Sepsis, 328305);
            CombatRoutine.AddDebuff(Flagellation, 323654);
            CombatRoutine.AddDebuff(MarkedforDeath, 137619);
            CombatRoutine.AddDebuff(DeadlyPoison, 2818);
            CombatRoutine.AddDebuff(Vendetta, 79140);
            CombatRoutine.AddDebuff(Garrote, 703);
            CombatRoutine.AddDebuff(Rupture, 1943);
            CombatRoutine.AddDebuff(Shiv, 319504);
            CombatRoutine.AddDebuff(CrimsonTempest, 121411);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Auto Stealth");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);


            //Prop
            CombatRoutine.AddProp("MobCount", "Mobcount to use Cooldowns ", numbRaidList, " Mobcount to use Cooldowns", "Cooldowns", 3);
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Vendetta, "Use " + Vendetta, CDUsageWithAOE, "Use " + Vendetta + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Exsanguinate, "Use " + Exsanguinate, CDUsageWithAOE, "Use " + Exsanguinate + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Vanish, "Use " + Vanish, CDUsageWithAOE, "Use " + Vanish + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("IsLegendary", "Choose your Legendary", Legendary, "Please choose your Legendary", "Generic");
            CombatRoutine.AddProp("IsStarter", "Choose the Ability to start combat with", Starter, "Note this is only for Master Assassin Talent", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(CloakofShadows, CloakofShadows + " Life Percent", numbList, "Life percent at which" + CloakofShadows + "is used, set to 0 to disable", "Defense", 20);
            CombatRoutine.AddProp(CrimsonVial, CrimsonVial + " Life Percent", numbList, "Life percent at which" + CrimsonVial + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Evasion, Evasion + " Life Percent", numbList, "Life percent at which" + Evasion + "is used, set to 0 to disable", "Defense", 10);
            CombatRoutine.AddProp(Feint, Feint + " Life Percent", numbList, "Life percent at which" + Feint + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp("First Poison", "First Poison", Poison1," Choose your first Poison", "Poisons", 0);
            CombatRoutine.AddProp("Second Poison", "Second Poison", Poison2, " Choose your second Poison", "Poisons", 0);
        }
        public override void Pulse()
        {
            //API.WriteLog("Conduit: " + API.PlayerIsConduitSelected());
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
                if (API.CanCast(DeadlyPoison) && UsePoison1 == "Deadly Poison" && API.PlayerBuffTimeRemaining(DeadlyPoison) < 30000 && !API.PlayerIsMoving && API.LastSpellCastInGame != DeadlyPoison && API.PlayerCurrentCastSpellID != 2823 && API.PlayerCurrentCastSpellID != 315584)
                {
                    API.CastSpell(DeadlyPoison);
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
                if (isInterrupt && API.CanCast(Kick) && isMelee)
                {
                    API.CastSpell(Kick);
                    return;
                }
                if (API.CanCast(KidneyShot) && isInterrupt && isMelee && API.SpellISOnCooldown(Kick))
                {
                    API.CastSpell(KidneyShot);
                    return;
                }
                if (API.CanCast(Blind) && isInterrupt && isMelee && API.SpellISOnCooldown(Kick))
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
                if (API.PlayerHealthPercent <= FeintLifePercent && API.CanCast(Feint) && !PlayerHasBuff(Feint) && API.PlayerEnergy >= 35)
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
                if (API.CanCast(DeadlyPoison) && UsePoison1 == "Deadly Poison" && API.PlayerBuffTimeRemaining(DeadlyPoison) < 3000 && !API.PlayerIsMoving && API.LastSpellCastInGame != DeadlyPoison && API.PlayerCurrentCastSpellID != 2823 && API.PlayerCurrentCastSpellID != 315584)
                {
                    API.CastSpell(DeadlyPoison);
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
                if (API.CanCast(TricksoftheTrade) && API.FocusRange < 100 && API.FocusHealthPercent != 0 && isMelee && !PlayerHasBuff(TricksoftheTrade))
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
            if (isMelee)
            {
                //actions+=/call_action_list,name=stealthed,if=stealthed.rogue
                if (IsStealth)
                {
                    //actions.stealthed=crimson_tempest,if=talent.nightstalker.enabled&spell_targets>=3&combo_points>=4&target.time_to_die-remains>6
                    if (API.CanCast(CrimsonTempest) && TalentCrimsonTempest && TalentNightStalker && API.PlayerUnitInMeleeRangeCount >= 3 && IsAOE && API.PlayerComboPoints >= 4 && API.PlayerEnergy >= 35)
                    {
                        API.CastSpell(CrimsonTempest);
                        return;
                    }
                    //actions.stealthed+=/rupture,if=talent.nightstalker.enabled&combo_points>=4&target.time_to_die-remains>6
                    if (API.CanCast(Rupture) && TalentNightStalker && API.PlayerComboPoints >= 4 && API.PlayerEnergy >= 25)
                    {
                        API.CastSpell(Rupture);
                        return;
                    }
                    //actions.stealthed+=/Garrotee,target_if=min:remains,if=talent.subterfuge.enabled&(remains<12|pmultiplier<=1)&target.time_to_die-remains>2
                    if (API.CanCast(Garrote) && TalentSubterfuge && API.TargetDebuffRemainingTime(Garrote) < 1200 && API.PlayerEnergy >= 45)
                    {
                        API.CastSpell(Garrote);
                        return;
                    }
                    //actions.stealthed+=/Garrotee,if=talent.subterfuge.enabled&talent.exsanguinate.enabled&active_enemies=1&buff.subterfuge.remains<1.3
                    if (API.CanCast(Garrote) && TalentSubterfuge && TalentExsanguinate && (API.PlayerUnitInMeleeRangeCount == 1 || !IsAOE) && API.PlayerBuffTimeRemaining(Subterfuge) < 130 && API.PlayerEnergy >= 45)
                    {
                        API.CastSpell(Garrote);
                        return;
                    }
                    //actions.stealthed+=/mutilate,if=talent.subterfuge.enabled&combo_points<=3
                    if (API.CanCast(Mutilate) && TalentSubterfuge && API.PlayerComboPoints <= 3 && API.PlayerEnergy >= 50)
                    {
                        API.CastSpell(Mutilate);
                        return;
                    }
                    if (API.CanCast(CheapShot) && IsStarter == "Cheap Shot" && API.PlayerEnergy >= 40)
                    {
                        API.CastSpell(CheapShot);
                        return;
                    }
                    if (API.CanCast(Ambush) && IsStarter == "Ambush" && (API.PlayerEnergy >= 50 || PlayerHasBuff(Blindside)))
                    {
                        API.CastSpell(Ambush);
                        return;
                    }
                }
                if (!IsStealth || IsStarter == "Manual")
                {
                    //actions+=/call_action_list,name=cds,if=(!talent.master_assassin.enabled|dot.Garrotee.ticking)
                    if (!TalentMasterAssassin || TargetHasDebuff(Garrote))
                    {
                        //actions.cds+=/marked_for_death,if=raid_event.adds.in>30-raid_event.adds.duration&combo_points.deficit>=cp_max_spend
                        if (API.CanCast(MarkedforDeath) && TalentMarkedforDeath && API.PlayerComboPoints == 0)
                        {
                            API.CastSpell(MarkedforDeath);
                            return;
                        }
                        //actions.cds+=/flagellation,if=!stealthed.rogue&(cooldown.vendetta.remains<3&effective_combo_points>=4&target.time_to_die>10|debuff.vendetta.up|fight_remains<24)
                        if (API.CanCast(Flagellation) && PlayerCovenantSettings == "Venthyr" && IsCovenant && (API.SpellCDDuration(Vendetta) < 300 && API.PlayerComboPoints >= 4 && API.TargetTimeToDie > 1000 || TargetHasDebuff(Vendetta) || API.TargetTimeToDie < 2400))
                        {
                            API.CastSpell(Flagellation);
                            return;
                        }
                        //actions.cds+=/sepsis,if=!stealthed.rogue&(cooldown.vendetta.remains<1&target.time_to_die>10|debuff.vendetta.up|fight_remains<10)
                        if (API.CanCast(Sepsis) && PlayerCovenantSettings == "Night Fae" && IsCovenant && !PlayerHasBuff(Stealth) && (API.SpellCDDuration(Vendetta) < 100 && API.TargetTimeToDie > 1000 || TargetHasDebuff(Vendetta) || API.TargetTimeToDie < 1000) && API.PlayerEnergy >= 25)
                        {
                            API.CastSpell(Sepsis);
                            return;
                        }
                        //actions.cds+=/vendetta,if=!stealthed.rogue&dot.rupture.ticking&!debuff.vendetta.up&variable.vendetta_nightstalker_condition&variable.vendetta_covenant_condition
                        if (API.CanCast(Vendetta) && IsVendetta && !PlayerHasBuff(Stealth) && TargetHasDebuff(Rupture) && !TargetHasDebuff(Vendetta) && (!TalentExsanguinate || API.SpellCDDuration(Exsanguinate) < 500 && !TalentDeeperStratagem || API.SpellCDDuration(Exsanguinate) < 300 && TalentDeeperStratagem))
                        {
                            API.CastSpell(Vendetta);
                            return;
                        }
                        //actions.cds+=/exsanguinate,if=!stealthed.rogue&(!dot.Garrotee.refreshable&dot.rupture.remains>4+4*cp_max_spend|dot.rupture.remains*0.5>target.time_to_die)&target.time_to_die>4
                        if (API.CanCast(Exsanguinate) && IsExsanguinate && !PlayerHasBuff(Stealth) && (!dotGarroteerefreshable && API.PlayerDebuffRemainingTime(Rupture) > 400) && API.PlayerEnergy >= 25)
                        {
                            API.CastSpell(Exsanguinate);
                            return;
                        }
                        //actions.cds+=/shiv,if=dot.rupture.ticking&(!cooldown.sepsis.ready|cooldown.vendetta.remains>12)|dot.sepsis.ticking
                        if (API.CanCast(Shiv) && (TargetHasDebuff(Rupture) && (API.SpellCDDuration(Sepsis) > GCD || API.SpellCDDuration(Vendetta) > 1200 && IsVendetta) || TargetHasDebuff(Sepsis)) && API.PlayerEnergy >= 20)
                        {
                            API.CastSpell(Shiv);
                            return;
                        }
                        //actions.cds+=/blood_fury,if=debuff.vendetta.up
                        if (PlayerRaceSettings == "Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee && TargetHasDebuff(Vendetta))
                        {
                            API.CastSpell(RacialSpell1);
                            return;
                        }
                        //actions.cds+=/berserking,if=debuff.vendetta.up
                        if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee && TargetHasDebuff(Vendetta))
                        {
                            API.CastSpell(RacialSpell1);
                            return;
                        }
                        //actions.cds+=/fireblood,if=debuff.vendetta.up
                        if (PlayerRaceSettings == "Dark Iron Dwarf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee && TargetHasDebuff(Vendetta))
                        {
                            API.CastSpell(RacialSpell1);
                            return;
                        }
                        //actions.cds+=/ancestral_call,if=debuff.vendetta.up
                        if (PlayerRaceSettings == "Mag'har Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee && TargetHasDebuff(Vendetta))
                        {
                            API.CastSpell(RacialSpell1);
                            return;
                        }
                        //actions.cds+=/call_action_list,name=vanish,if=!stealthed.all&master_assassin_remains=0
                        if (API.CanCast(Vanish) && IsVanish)
                        {
                            //actions.vanish+=/vanish,if=talent.exsanguinate.enabled&talent.nightstalker.enabled&variable.nightstalker_cp_condition&cooldown.exsanguinate.remains<1
                            if (API.CanCast(Vanish) && TalentExsanguinate && TalentNightStalker && API.SpellCDDuration(Exsanguinate) < 100 && nightstalkercpcondition)
                            {
                                API.CastSpell(Vanish);
                                return;
                            }
                            //actions.vanish+=/vanish,if=talent.nightstalker.enabled&!talent.exsanguinate.enabled&variable.nightstalker_cp_condition&debuff.vendetta.up
                            if (API.CanCast(Vanish) && !TalentExsanguinate && TalentNightStalker && TargetHasDebuff(Vendetta) && nightstalkercpcondition)
                            {
                                API.CastSpell(Vanish);
                                return;
                            }
                            //actions.vanish+=/vanish,if=talent.subterfuge.enabled&cooldown.Garrotee.up&(dot.Garrotee.refreshable|debuff.vendetta.up&dot.Garrotee.pmultiplier<=1)&combo_points.deficit>=(spell_targets.fan_of_knives>?4)&raid_event.adds.in>12
                            if (API.CanCast(Vanish) && TalentSubterfuge && API.SpellCDDuration(Garrote) <= GCD && (TargetHasDebuff(Garrote) && dotGarroteerefreshable || TargetHasDebuff(Vendetta)) && API.PlayerComboPoints < 5)
                            {
                                API.CastSpell(Vanish);
                                return;
                            }
                            //actions.vanish+=/vanish,if=(talent.master_assassin.enabled|runeforge.mark_of_the_master_assassin)&!dot.rupture.refreshable&dot.Garrotee.remains>3&debuff.vendetta.up&(debuff.shiv.up|debuff.vendetta.remains<4|dot.sepsis.ticking)&dot.sepsis.remains<3
                            if (API.CanCast(Vanish) && TalentMasterAssassin && !dotrupturerefreshable && API.TargetDebuffRemainingTime(Garrote) > 300 && TargetHasDebuff(Vendetta) && (TargetHasDebuff(Shiv) || API.TargetDebuffRemainingTime(Vendetta) < 400 || TargetHasDebuff(Sepsis)) && API.TargetDebuffRemainingTime(Sepsis) < 300)
                            {
                                API.CastSpell(Vanish);
                                return;
                            }
                        }
                        //actions.cds+=/use_items,slots=trinket1,if=variable.trinket_sync_slot=1&(debuff.vendetta.up|fight_remains<=20)|(variable.trinket_sync_slot=2&!trinket.2.cooldown.ready)|!variable.trinket_sync_slot
                        if (API.PlayerTrinketIsUsable(1) && !API.MacroIsIgnored("Trinket1") && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1 && (TargetHasDebuff(Vendetta) && IsVendetta || !IsVendetta))
                        {
                            API.CastSpell("Trinket1");
                            return;
                        }
                        //actions.cooldown+=/use_items
                        if (API.PlayerTrinketIsUsable(2) && !API.MacroIsIgnored("Trinket2") && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2 && (TargetHasDebuff(Vendetta) && IsVendetta || !IsVendetta))
                        {
                            API.CastSpell("Trinket2");
                            return;
                        }
                    }
                    //actions+=/slice_and_dice,if=!buff.slice_and_dice.up&combo_points>=3
                    if (API.CanCast(SliceandDice) && API.PlayerComboPoints >= 3 && !PlayerHasBuff(SliceandDice) && API.PlayerEnergy >= 25)
                    {
                        API.CastSpell(SliceandDice);
                        return;
                    }
                    //actions+=/envenom,if=buff.slice_and_dice.up&buff.slice_and_dice.remains<5&combo_points>=4
                    if (API.CanCast(Envenom) && API.PlayerComboPoints >= 4 && PlayerHasBuff(SliceandDice) && API.PlayerBuffTimeRemaining(SliceandDice) < 500 && API.PlayerEnergy >= 35)
                    {
                        API.CastSpell(Envenom);
                        return;
                    }
                    //actions+=/call_action_list,name=dot
                    //actions.dot+=/Garrotee,if=talent.exsanguinate.enabled&!exsanguinated.Garrotee&dot.Garrotee.pmultiplier<=1&cooldown.exsanguinate.remains<2&spell_targets.fan_of_knives=1&raid_event.adds.in>6&dot.Garrotee.remains*0.5<target.time_to_die
                    if (API.CanCast(Garrote) && TalentExsanguinate && API.SpellCDDuration(Exsanguinate) < 200 && (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE) && dotGarroteerefreshable && API.PlayerEnergy >= 45)
                    {
                        API.CastSpell(Garrote);
                        return;
                    }
                    //actions.dot+=/rupture,if=talent.exsanguinate.enabled&(effective_combo_points>=cp_max_spend&cooldown.exsanguinate.remains<1&dot.rupture.remains*0.5<target.time_to_die)
                    if (API.CanCast(Rupture) && TalentExsanguinate && (maxcp && API.SpellCDDuration(Exsanguinate) < 100 && dotrupturerefreshable) && API.PlayerEnergy >= 25)
                    {
                        API.CastSpell(Rupture);
                        return;
                    }
                    //actions.dot+=/Garrotee,if=refreshable&combo_points.deficit>=1&(pmultiplier<=1|remains<=tick_time&spell_targets.fan_of_knives>=3)&(!exsanguinated|remains<=tick_time*2&spell_targets.fan_of_knives>=3)&(target.time_to_die-remains)>4&master_assassin_remains=0
                    if (API.CanCast(Garrote) && dotGarroteerefreshable && (API.PlayerComboPoints < 5 && !TalentDeeperStratagem || API.PlayerComboPoints < 6 && TalentDeeperStratagem) && API.PlayerEnergy >= 45)
                    {
                        API.CastSpell(Garrote);
                        return;
                    }
                    //actions.dot+=/crimson_tempest,if=spell_targets>=2&remains<2+(spell_targets>=5)&effective_combo_points>=4
                    if (API.CanCast(CrimsonTempest) && TalentCrimsonTempest && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE && dotcrimsontempestrefreshable && API.PlayerComboPoints >= 4 && API.PlayerEnergy >= 35)
                    {
                        API.CastSpell(CrimsonTempest);
                        return;
                    }
                    //actions.dot+=/rupture,cycle_targets=1,if=!variable.skip_cycle_rupture&!variable.skip_rupture&target!=self.target&effective_combo_points>=4&refreshable&(pmultiplier<=1|remains<=tick_time&spell_targets.fan_of_knives>=3)&(!exsanguinated|remains<=tick_time*2&spell_targets.fan_of_knives>=3)&target.time_to_die-remains>(4+runeforge.dashing_scoundrel*9+runeforge.doomblade*6)
                    if (API.CanCast(Rupture) && API.PlayerComboPoints >= 4 && dotrupturerefreshable && API.PlayerEnergy >= 25)
                    {
                        API.CastSpell(Rupture);
                        return;
                    }
                    if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                    {
                        if (API.CanCast(Garrote) && dotGarroteerefreshableMO && API.MouseoverRange < 6 && (API.PlayerComboPoints < 5 && !TalentDeeperStratagem || API.PlayerComboPoints < 6 && TalentDeeperStratagem) && API.PlayerEnergy >= 45)
                        {
                            API.CastSpell(Garrote + "MO");
                            return;
                        }
                        if (API.CanCast(Rupture) && API.PlayerComboPoints >= 4 && API.MouseoverRange < 6 && dotrupturerefreshableMO && API.PlayerEnergy >= 25)
                        {
                            API.CastSpell(Rupture + "MO");
                            return;
                        }
                        if (API.CanCast(SerratedBoneSpike) && !API.MacroIsIgnored(SerratedBoneSpike + "MO") && IsCovenant && PlayerCovenantSettings == "Necrolord" && API.MouseoverRange < 30 && !PlayerHasBuff(MasterAssassin) && (PlayerHasBuff(SliceandDice) && !API.MouseoverHasDebuff(SerratedBoneSpike) || API.SpellCharges(SerratedBoneSpike) > 2.75))
                        {
                            API.CastSpell(SerratedBoneSpike + "MO");
                            return;
                        }
                    }
                    //actions+=/call_action_list,name=direct
                    //actions.direct=envenom,if=effective_combo_points>=4+talent.deeper_stratagem.enabled&(debuff.vendetta.up|debuff.shiv.up|debuff.flagellation.up|energy.deficit<=25+variable.energy_regen_combined|!variable.single_target)&(!talent.exsanguinate.enabled|cooldown.exsanguinate.remains>2)
                    if (API.CanCast(Envenom) && (API.PlayerComboPoints >= 4 && !TalentDeeperStratagem || TalentDeeperStratagem && API.PlayerComboPoints >= 5) && (TargetHasDebuff(Vendetta) || TargetHasDebuff(Shiv) || TargetHasDebuff(Flagellation) || EnergyDefecit <= 25 || API.PlayerUnitInMeleeRangeCount > AOEUnitNumber && IsAOE) && (!TalentExsanguinate || API.SpellCDDuration(Exsanguinate) > 200) && API.PlayerEnergy >= 35)
                    {
                        API.CastSpell(Envenom);
                        return;
                    }
                    //actions.direct+=/serrated_bone_spike,cycle_targets=1,if=master_assassin_remains=0&(buff.slice_and_dice.up&!dot.serrated_bone_spike_dot.ticking|fight_remains<=5|cooldown.serrated_bone_spike.charges_fractional>=2.75|soulbind.lead_by_example.enabled&!buff.lead_by_example.up)
                    if (API.CanCast(SerratedBoneSpike) && IsCovenant && PlayerCovenantSettings == "Necrolord" && !PlayerHasBuff(MasterAssassin) && (PlayerHasBuff(SliceandDice) && !TargetHasDebuff(SerratedBoneSpike) || API.SpellCharges(SerratedBoneSpike) > 2.75))
                    {
                        API.CastSpell(SerratedBoneSpike);
                        return;
                    }
                    //actions.direct+=/fan_of_knives,if=variable.use_filler&(buff.hidden_blades.stack>=19|(!priority_rotation&spell_targets.fan_of_knives>=4+stealthed.rogue))
                    if (API.CanCast(FanofKnives) && usefiller && (API.PlayerBuffStacks(HiddenBlades) >= 19 || API.PlayerUnitInMeleeRangeCount >= 4 && IsAOE) && API.PlayerEnergy >= 35)
                    {
                        API.CastSpell(FanofKnives);
                        return;
                    }
                    //actions.direct+=/fan_of_knives,target_if=!dot.deadly_poison_dot.ticking,if=variable.use_filler&spell_targets.fan_of_knives>=3
                    if (API.CanCast(FanofKnives) && !TargetHasDebuff(DeadlyPoison) && usefiller && API.PlayerUnitInMeleeRangeCount >= 3 && IsAOE && API.PlayerEnergy >= 35)
                    {
                        API.CastSpell(FanofKnives);
                        return;
                    }
                    //actions.direct+=/echoing_reprimand,if=variable.use_filler&cooldown.vendetta.remains>10
                    if (API.CanCast(EchoingReprimand) && IsCovenant && PlayerCovenantSettings == "Kyrian" && usefiller && (API.SpellCDDuration(Vendetta) > 1000 && IsVendetta || !IsVendetta) && API.PlayerEnergy >= 10)
                    {
                        API.CastSpell(EchoingReprimand);
                        return;
                    }
                    //actions.direct+=/ambush,if=variable.use_filler&(master_assassin_remains=0&!runeforge.doomblade|buff.blindside.up)
                    if (API.CanCast(Ambush) && usefiller && (API.PlayerEnergy >= 50 || PlayerHasBuff(Blindside)) && (!PlayerHasBuff(MasterAssassin) && IsLegendary != "Doomblade" || PlayerHasBuff(Blindside)))
                    {
                        API.CastSpell(Ambush);
                        return;
                    }
                    //actions.direct+=/mutilate,target_if=!dot.deadly_poison_dot.ticking,if=variable.use_filler&spell_targets.fan_of_knives=2
                    if (API.CanCast(Mutilate) && !TargetHasDebuff(DeadlyPoison) && usefiller && API.PlayerUnitInMeleeRangeCount >= 2 && IsAOE && API.PlayerEnergy >= 50)
                    {
                        API.CastSpell(Mutilate);
                        return;
                    }
                    //actions.direct+=/mutilate,if=variable.use_filler
                    if (API.CanCast(Mutilate) && usefiller && API.PlayerEnergy >= 50)
                    {
                        API.CastSpell(Mutilate);
                        return;
                    }
                    //actions+=/arcane_torrent,if=energy.deficit>=15+variable.energy_regen_combined
                    if (PlayerRaceSettings == "Bloodelf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee && EnergyDefecit >= 15)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions+=/arcane_pulse
                    if (PlayerRaceSettings == "Nightborne" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions+=/lights_judgment
                    if (PlayerRaceSettings == "Lightforged" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions+=/bag_of_tricks
                    if (PlayerRaceSettings == "Vulpera" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                }
            }
        }
    }
}