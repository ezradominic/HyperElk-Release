// Changelog
// v1.0 First release
// v1.1 Victory Rush fix
// v1.2 covenant support beta
// v1.3 a few fixes
// v1.4 Condemn fix
// v1.5 covenant ability always or with cds
// v1.6 covenant changes
// v1.7 Heroic Throw toggle
// v1.8 condemn fix
// v1.9 back to condemn id
// v2.0 Bladestorm toggle and execute/condemn mouseover
// v2.1 update for legendarys
// v2.2 Racials and Trinkets
// v2.3 new apl
// v2.4 spell ids added + Berserker rage in fear + storm bolt as kick
// v2.5 quick hotfix on phone lul
// v2.6 Rallying Cry added
// v2.7 Ignore Pain added
// v2.8 Racials and a few other things
// v2.9 some small adjustments

using System.Linq;

namespace HyperElk.Core
{
    public class FuryWarrior : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool BladestormToggle => API.ToggleIsEnabled("Bladestorm");
        //Spell,Auras
        private string Bloodthirst = "Bloodthirst";
        private string Onslaught = "Onslaught";
        private string RagingBlow = "Raging Blow";
        private string Rampage = "Rampage";
        private string Recklessness = "Recklessness";
        private string Execute = "Execute";
        private string MassacreExecute = "MassacreExecute";
        private string Whirlwind = "Whirlwind";
        private string BerserkerRage = "Berserker Rage";
        private string DragonRoar = "Dragon Roar";
        private string Pummel = "Pummel";
        private string HeroicThrow = "Heroic Throw";
        private string EnragedRegeneration = "Enraged Regeneration";
        private string Siegebreaker = "Siegebreaker";
        private string VictoryRush = "Victory Rush";
        private string ImpendingVictory = "Impending Victory";
        private string BattleShout = "Battle Shout";
        private string StormBolt = "Storm Bolt";
        private string RallyingCry = "Rallying Cry";
        private string Bladestorm = "Bladestorm";
        private string Slam = "Slam";
        private string Enrage = "Enrage";
        private string SuddenDeath = "Sudden Death";
        private string Victorious = "Victorious";
        private string Condemn = "Condemn";
        private string MassacreCondemn = "MassacreCondemn";
        private string SpearofBastion = "Spear of Bastion";
        private string AncientAftershock = "Ancient Aftershock";
        private string ConquerorsBanner = "Conqueror's Banner";
        private string CrushingBlow = "Crushing Blow";
        private string Bloodbath = "Bloodbath";
        private string WilloftheBerserker = "Will of the Berserker";
        private string PhialofSerenity = "Phial of Serenity";
        private string Frenzy = "Frenzy";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string AoE = "AOE";
        private string AoERaid = "AoERaid";
        private string IgnorePain = "Ignore Pain";

        //Talents
        bool TalentImpendingVictory => API.PlayerIsTalentSelected(2, 2);
        bool TalentStormBolt => API.PlayerIsTalentSelected(2, 3);
        bool TalentMassacre => API.PlayerIsTalentSelected(3, 1);
        bool TalentOnslaught => API.PlayerIsTalentSelected(3, 3);
        bool TalentDragonRoar => API.PlayerIsTalentSelected(6, 2);
        bool TalentBladestorm => API.PlayerIsTalentSelected(6, 3);
        bool TalentRecklessAbandon => API.PlayerIsTalentSelected(7, 2);
        bool TalentSiegebreaker => API.PlayerIsTalentSelected(7, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;
        private float gcd => API.SpellGCDTotalDuration;

        bool WWup => (API.PlayerHasBuff(Whirlwind) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE || API.PlayerUnitInMeleeRangeCount < AOEUnitNumber && IsAOE || !IsAOE);
        bool IsRecklessness => UseRecklessness == "with Cooldowns" && IsCooldowns || UseRecklessness == "always";
        bool IsSiegebreaker => UseSiegebreaker == "with Cooldowns" && IsCooldowns || UseSiegebreaker == "always";
        bool IsCovenant => (UseCovenant == "with Cooldowns" || UseDragonRoar == "with Cooldowns and AoE") && IsCooldowns || UseCovenant == "always" || (UseCovenant == "on AOE" || UseDragonRoar == "with Cooldowns and AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber;
        bool IsDragonRoar => (UseDragonRoar == "with Cooldowns" || UseDragonRoar == "with Cooldowns and AoE") && IsCooldowns || UseDragonRoar == "always" || (UseDragonRoar == "on AOE" || UseDragonRoar == "with Cooldowns and AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber;
        bool IsBladestorm => (UseBladestorm == "with Cooldowns" || UseDragonRoar == "with Cooldowns and AoE") && IsCooldowns || UseBladestorm == "always" || (UseBladestorm == "on AOE" || UseDragonRoar == "with Cooldowns and AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber;
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee;


        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseHeroicThrow => heroiclist[CombatRoutine.GetPropertyInt(HeroicThrow)];
        private bool IsLineUp => CombatRoutine.GetPropertyBool("LineUp");
        private int EnragedRegenerationLifePercent => numbList[CombatRoutine.GetPropertyInt(EnragedRegeneration)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private int VictoryRushLifePercent => numbList[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => numbList[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };

        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);

        private bool RallyAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(RallyLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(RallyLifePercent) >= AoENumber;

        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoERaid)];
        private int RallyLifePercent => numbList[CombatRoutine.GetPropertyInt(RallyingCry)];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "with Cooldowns and AoE", "always" };
        string[] heroiclist = new string[] { "Not Used", "when out of melee", "only Mouseover", "both" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseRecklessness => CDUsage[CombatRoutine.GetPropertyInt(Recklessness)];
        private string UseSiegebreaker => CDUsage[CombatRoutine.GetPropertyInt(Siegebreaker)];
        private string UseDragonRoar => CDUsageWithAOE[CombatRoutine.GetPropertyInt(DragonRoar)];
        private string UseBladestorm => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Bladestorm)];
        private int IgnorePainLifePercent => numbList[CombatRoutine.GetPropertyInt(IgnorePain)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Fury Warrior by smartie";
            API.WriteLog("Welcome to smartie`s Fury Warrior v2.9");

            //Spells
            CombatRoutine.AddSpell(Bloodthirst, 23881, "D1");
            CombatRoutine.AddSpell(Bloodbath, 335096, "D1");
            CombatRoutine.AddSpell(Onslaught,315720, "D2");
            CombatRoutine.AddSpell(RagingBlow, 85288, "D3");
            CombatRoutine.AddSpell(CrushingBlow, 335097, "D3");
            CombatRoutine.AddSpell(Rampage,184367, "D4");
            CombatRoutine.AddSpell(Recklessness,1719, "D5");
            CombatRoutine.AddSpell(Execute,5308, "D7");
            CombatRoutine.AddSpell(MassacreExecute,280735, "D7");
            CombatRoutine.AddSpell(Whirlwind,190411, "D8");
            CombatRoutine.AddSpell(BerserkerRage,18499, "F1");
            CombatRoutine.AddSpell(DragonRoar,118000, "F2");
            CombatRoutine.AddSpell(Pummel,6552, "F5");
            CombatRoutine.AddSpell(HeroicThrow,57755, "F6");
            CombatRoutine.AddSpell(EnragedRegeneration,184364, "F6");
            CombatRoutine.AddSpell(Siegebreaker,280772, "F9");
            CombatRoutine.AddSpell(VictoryRush,34428, "NumPad3");
            CombatRoutine.AddSpell(ImpendingVictory,202168, "F3");
            CombatRoutine.AddSpell(BattleShout,6673, "Q");
            CombatRoutine.AddSpell(StormBolt,107570, "F7");
            CombatRoutine.AddSpell(RallyingCry,97462, "F2");
            CombatRoutine.AddSpell(Bladestorm,46924, "None");
            CombatRoutine.AddSpell(Condemn,317485, "D6");
            CombatRoutine.AddSpell(MassacreCondemn,330325, "D6");
            CombatRoutine.AddSpell(ConquerorsBanner,324143, "D1");
            CombatRoutine.AddSpell(AncientAftershock,325886, "D1");
            CombatRoutine.AddSpell(SpearofBastion,307865, "D1");
            CombatRoutine.AddSpell(Slam,1464, "None");
            CombatRoutine.AddSpell(IgnorePain, 190456, "D9");

            //Macros
            CombatRoutine.AddMacro(HeroicThrow + "MO", "D2");
            CombatRoutine.AddMacro(Execute + "MO", "D6");
            CombatRoutine.AddMacro(MassacreExecute + "MO", "D6");
            CombatRoutine.AddMacro(Condemn + "MO", "D6");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

            //Buffs
            CombatRoutine.AddBuff(Enrage, 184362);
            CombatRoutine.AddBuff(Whirlwind,85739);
            CombatRoutine.AddBuff(Recklessness,1719);
            CombatRoutine.AddBuff(SuddenDeath,280776);
            CombatRoutine.AddBuff(BattleShout,6673);
            CombatRoutine.AddBuff(Victorious, 32216);
            CombatRoutine.AddBuff(WilloftheBerserker, 335597);
            CombatRoutine.AddBuff(Frenzy, 335077);
            CombatRoutine.AddBuff(IgnorePain, 190456);

            //Debuff
            CombatRoutine.AddDebuff(Siegebreaker,280773);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Bladestorm");

            //Item
            CombatRoutine.AddItem(PhialofSerenity,177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);


            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, " Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp(HeroicThrow, "Use Heroic Throw", heroiclist, "Use " + HeroicThrow + " ,when out of melee, only Mousover or both", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Recklessness, "Use " + Recklessness, CDUsage, "Use " + Recklessness + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Siegebreaker, "Use " + Siegebreaker, CDUsage, "Use " + Siegebreaker + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(DragonRoar, "Use " + DragonRoar, CDUsageWithAOE, "Use " + DragonRoar + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Bladestorm, "Use " + Bladestorm, CDUsageWithAOE, "Use " + Bladestorm + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("LineUp", "LineUp CDS", true, " Lineup Recklessness and Siegebreaker", "Cooldowns");
            CombatRoutine.AddProp(EnragedRegeneration, EnragedRegeneration + " Life Percent", numbList, " Life percent at which" + EnragedRegeneration + " is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", numbList, "Life percent at which" + VictoryRush + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", numbList, "Life percent at which" + ImpendingVictory + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(IgnorePain, IgnorePain + " Life Percent", numbList, "Life percent at which" + IgnorePain + " is used, set to 0 to disable", "Defense", 0);
            CombatRoutine.AddProp(AoE, "Rallying Cry Party Units ", numbPartyList, " in Party", "Rallying", 3);
            CombatRoutine.AddProp(AoERaid, "Rallying Cry Raid Units ", numbRaidList, "  in Raid", "Rallying", 3);
            CombatRoutine.AddProp(RallyingCry, RallyingCry + "Life Percent", numbList, "Life percent at which" + RallyingCry + " is used, set to 0 to disable", "Rallying", 50);
        }
        public override void Pulse()
        {
            if (!API.PlayerIsMounted)
            {
                if (PlayerLevel >= 39 && API.PlayerBuffTimeRemaining(BattleShout) < 30000)
                {
                    API.CastSpell(BattleShout);
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            if (isInterrupt && API.CanCast(Pummel) && IsMelee && PlayerLevel >= 7)
            {
                API.CastSpell(Pummel);
                return;
            }
            if (isInterrupt && API.CanCast(StormBolt) && API.TargetRange <= 20 && TalentStormBolt && (API.SpellISOnCooldown(Pummel) || !IsMelee))
            {
                API.CastSpell(StormBolt);
                return;
            }
            if (PlayerRaceSettings == "Tauren" && API.CanCast(RacialSpell1) && isInterrupt && !API.PlayerIsMoving && isRacial && IsMelee && API.SpellISOnCooldown(Pummel))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast(IgnorePain) && API.PlayerRage >= 60 && !API.PlayerHasBuff(IgnorePain) && API.PlayerHealthPercent <= IgnorePainLifePercent)
            {
                API.CastSpell(IgnorePain);
                return;
            }
            if (API.CanCast(RallyingCry) && RallyAoE)
            {
                API.CastSpell(RallyingCry);
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
            if (API.PlayerHealthPercent <= EnragedRegenerationLifePercent && IsMelee && PlayerLevel >= 23 && API.CanCast(EnragedRegeneration))
            {
                API.CastSpell(EnragedRegeneration);
                return;
            }
            if (API.PlayerHealthPercent <= VictoryRushLifePercent && PlayerLevel >= 5 && API.CanCast(VictoryRush) && API.PlayerHasBuff(Victorious) && IsMelee)
            {
                API.CastSpell(VictoryRush);
                return;
            }
            if (API.PlayerHealthPercent <= ImpendingVictoryLifePercent && TalentImpendingVictory && API.CanCast(ImpendingVictory) && IsMelee)
            {
                API.CastSpell(ImpendingVictory);
                return;
            }
            if (API.CanCast(BerserkerRage) && API.PlayerIsCC(CCList.FEAR_MECHANIC))
            {
                API.CastSpell(BerserkerRage);
                return;
            }
            rotation();
            return;
        }
        public override void OutOfCombatPulse()
        {
        }
        private void rotation()
         {
            if (API.CanCast(HeroicThrow) && PlayerLevel >= 24 && API.TargetRange >= 8 && API.TargetRange <= 30 && (UseHeroicThrow == "when out of melee" || UseHeroicThrow == "both"))
            {
                API.CastSpell(HeroicThrow);
                return;
            }
            if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0 && (UseHeroicThrow == "only Mouseover" || UseHeroicThrow == "both"))
            {
                if (API.CanCast(HeroicThrow) && PlayerLevel >= 24 && API.MouseoverRange >= 8 && API.MouseoverRange <= 30)
                {
                    API.CastSpell(HeroicThrow + "MO");
                    return;
                }
            }
            if (IsMelee)
            {
                //actions+=/rampage,if=cooldown.recklessness.remains<3&talent.reckless_abandon.enabled
                if (API.CanCast(Rampage) && PlayerLevel >= 19 && WWup && IsRecklessness && API.SpellCDDuration(Recklessness) < 300 && TalentRecklessAbandon && API.PlayerRage >= 80)
                {
                    API.CastSpell(Rampage);
                    return;
                }
                if (API.CanCast(Recklessness) && PlayerLevel >= 38 && !API.PlayerHasBuff(Recklessness) && (!TalentRecklessAbandon || TalentRecklessAbandon && API.PlayerRage < 50) && IsRecklessness)
                {
                    API.CastSpell(Recklessness);
                    return;
                }
                if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && (PlayerLevel < 22 && API.PlayerRage >= 30 || PlayerLevel >= 22) && (!API.PlayerHasBuff(Whirlwind) && PlayerLevel >= 37 || PlayerLevel < 37) && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                {
                    API.CastSpell(Whirlwind);
                    return;
                }
                //actions+=/blood_fury
                if (PlayerRaceSettings == "Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/berserking,if=buff.recklessness.up
                if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee && API.PlayerHasBuff(Recklessness))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/lights_judgment,if=buff.recklessness.down&debuff.siegebreaker.down
                if (PlayerRaceSettings == "Lightforged" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee && !API.TargetHasDebuff(Siegebreaker) && !API.PlayerHasBuff(Recklessness))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/fireblood
                if (PlayerRaceSettings == "Dark Iron Dwarf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/ancestral_call
                if (PlayerRaceSettings == "Mag'har Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/bag_of_tricks,if=buff.recklessness.down&debuff.siegebreaker.down&buff.enrage.up
                if (PlayerRaceSettings == "Vulpera" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee && !API.TargetHasDebuff(Siegebreaker) && !API.PlayerHasBuff(Recklessness) && API.PlayerHasBuff(Enrage))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
                {
                    API.CastSpell("Trinket1");
                    return;
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
                {
                    API.CastSpell("Trinket2");
                    return;
                }
                //Single Target Rota
                //actions.single_target=raging_blow,if=runeforge.will_of_the_berserker.equipped&buff.will_of_the_berserker.remains<gcd
                if (API.CanCast(RagingBlow) && WWup && PlayerLevel >= 12 && !(TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)) && API.PlayerHasBuff(WilloftheBerserker) && API.PlayerBuffTimeRemaining(WilloftheBerserker) < gcd)
                {
                    API.CastSpell(RagingBlow);
                    return;
                }
                //actions.single_target+=/crushing_blow,if=runeforge.will_of_the_berserker.equipped&buff.will_of_the_berserker.remains<gcd
                if (API.CanCast(CrushingBlow) && WWup && (TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)) && API.PlayerHasBuff(WilloftheBerserker) && API.PlayerBuffTimeRemaining(WilloftheBerserker) < gcd)
                {
                    API.CastSpell(CrushingBlow);
                    return;
                }
                //actions.single_target+=/siegebreaker,if=spell_targets.whirlwind>1|raid_event.adds.in>15
                if (API.CanCast(Siegebreaker) && WWup && TalentSiegebreaker && (IsLineUp && API.SpellCDDuration(Recklessness) > 3000 || !IsLineUp || UseRecklessness == "with Cooldowns" && !IsCooldowns) && IsSiegebreaker)
                {
                    API.CastSpell(Siegebreaker);
                    return;
                }
                //actions.single_target+=/rampage,if=buff.recklessness.up|(buff.enrage.remains<gcd|rage>90)|buff.frenzy.remains<1.5
                if (API.CanCast(Rampage) && WWup && PlayerLevel >= 19 && API.PlayerRage >= 80 && ((API.PlayerHasBuff(Recklessness) || API.PlayerBuffTimeRemaining(Enrage) < gcd || API.PlayerRage > 90) || API.PlayerHasBuff(Frenzy) && API.PlayerBuffTimeRemaining(Frenzy) < 150))
                {
                    API.CastSpell(Rampage);
                    return;
                }
                if (API.CanCast(ConquerorsBanner) && API.PlayerHasBuff(Enrage) && PlayerCovenantSettings == "Necrolord" && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(ConquerorsBanner);
                    return;
                }
                //actions.single_target+=/condemn
                if (API.CanCast(Condemn) && WWup && PlayerCovenantSettings == "Venthyr" && !TalentMassacre && (API.TargetHealthPercent < 20 || API.TargetHealthPercent > 80 || API.PlayerHasBuff(SuddenDeath)))
                {
                    API.CastSpell(Condemn);
                    return;
                }
                //actions.single_target+=/condemn
                if (API.CanCast(MassacreCondemn) && WWup && PlayerCovenantSettings == "Venthyr" && TalentMassacre && (API.TargetHealthPercent < 35 || API.TargetHealthPercent > 80 || API.PlayerHasBuff(SuddenDeath)))
                {
                    API.CastSpell(MassacreCondemn);
                    return;
                }
                //actions.single_target+=/execute
                if (API.CanCast(Execute) && WWup && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 9 && !TalentMassacre && (API.TargetHealthPercent < 20 || API.PlayerHasBuff(SuddenDeath)))
                {
                    API.CastSpell(Execute);
                    return;
                }
                //actions.single_target+=/execute
                if (API.CanCast(MassacreExecute) && WWup && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 9 && TalentMassacre && (API.TargetHealthPercent < 35 || API.PlayerHasBuff(SuddenDeath)))
                {
                    API.CastSpell(MassacreExecute);
                    return;
                }
                //Mouseover stuff
                if (IsMouseover && WWup && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                {
                    if (API.CanCast(Execute) && !API.MacroIsIgnored(Execute + "MO") && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 9 && !TalentMassacre && (API.MouseoverHealthPercent < 20 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(Execute + "MO");
                        return;
                    }
                    if (API.CanCast(MassacreExecute) && !API.MacroIsIgnored(MassacreExecute + "MO") && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 9 && TalentMassacre && (API.MouseoverHealthPercent < 35 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(MassacreExecute + "MO");
                        return;
                    }
                    if (API.CanCast(Condemn) && !API.MacroIsIgnored(Condemn + "MO") && PlayerCovenantSettings == "Venthyr" && !TalentMassacre && (API.MouseoverHealthPercent < 20 || API.MouseoverHealthPercent > 80 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(Condemn + "MO");
                        return;
                    }
                    if (API.CanCast(MassacreCondemn) && !API.MacroIsIgnored(MassacreCondemn + "MO") && PlayerCovenantSettings == "Venthyr" && TalentMassacre && (API.MouseoverHealthPercent < 35 || API.MouseoverHealthPercent > 80 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(MassacreCondemn + "MO");
                        return;
                    }
                }
                if (API.CanCast(SpearofBastion) && API.PlayerHasBuff(Enrage) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(SpearofBastion);
                    return;
                }
                if (API.CanCast(AncientAftershock) && API.PlayerHasBuff(Enrage) && PlayerCovenantSettings == "Night Fae" && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(AncientAftershock);
                    return;
                }
                //actions.single_target+=/bladestorm,if=buff.enrage.up&(spell_targets.whirlwind>1|raid_event.adds.in>45)
                if (API.CanCast(Bladestorm) && API.PlayerHasBuff(Enrage) && TalentBladestorm && IsBladestorm && BladestormToggle)
                {
                    API.CastSpell(Bladestorm);
                    return;
                }
                //actions.single_target +=/ bloodthirst,if= buff.enrage.down | conduit.vicious_contempt.rank > 5 & target.health.pct < 35 & !talent.cruelty.enabled
                if (API.CanCast(Bloodthirst) && WWup && PlayerLevel >= 10 && !API.PlayerHasBuff(Enrage,false, false) && !(TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)))
                {
                    API.CastSpell(Bloodthirst);
                    return;
                }
                //actions.single_target +=/ bloodbath,if= buff.enrage.down | conduit.vicious_contempt.rank > 5 & target.health.pct < 35 & !talent.cruelty.enabled
                if (API.CanCast(Bloodbath) && WWup && !API.PlayerHasBuff(Enrage, false, false) && (TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)))
                {
                    API.CastSpell(Bloodbath);
                    return;
                }
                //actions.single_target +=/ dragon_roar,if= buff.enrage.up & (spell_targets.whirlwind > 1 | raid_event.adds.in> 15)
                if (API.CanCast(DragonRoar) && TalentDragonRoar && API.PlayerHasBuff(Enrage) && IsDragonRoar)
                {
                    API.CastSpell(DragonRoar);
                    return;
                }
                //actions.single_target+=/onslaught
                if (API.CanCast(Onslaught) && WWup && API.PlayerHasBuff(Enrage) && TalentOnslaught)
                {
                    API.CastSpell(Onslaught);
                    return;
                }
                //actions.single_target+=/raging_blow,if=charges=2
                if (API.CanCast(RagingBlow) && WWup && PlayerLevel >= 12 && API.SpellCharges(RagingBlow) == 2 && !(TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)))
                {
                    API.CastSpell(RagingBlow);
                    return;
                }
                //actions.single_target+=/crushing_blow,if=charges=2
                if (API.CanCast(CrushingBlow) && WWup && API.SpellCharges(RagingBlow) == 2 && (TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)))
                {
                    API.CastSpell(CrushingBlow);
                    return;
                }
                //actions.single_target+=/bloodthirst
                if (API.CanCast(Bloodthirst) && WWup && PlayerLevel >= 10 && !(TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)))
                {
                    API.CastSpell(Bloodthirst);
                    return;
                }
                //actions.single_target+=/bloodbath
                if (API.CanCast(Bloodbath) && WWup && (TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)))
                {
                    API.CastSpell(Bloodbath);
                    return;
                }
                //actions.single_target+=/raging_blow
                if (API.CanCast(RagingBlow) && WWup && PlayerLevel >= 12 && !(TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)))
                {
                    API.CastSpell(RagingBlow);
                    return;
                }
                //actions.single_target+=/crushing_blow
                if (API.CanCast(CrushingBlow) && WWup && (TalentRecklessAbandon && API.PlayerHasBuff(Recklessness)))
                {
                    API.CastSpell(CrushingBlow);
                    return;
                }
                //actions.single_target+=/whirlwind
                if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && (PlayerLevel < 22 && API.PlayerRage >= 30 || PlayerLevel >= 22))
                {
                    API.CastSpell(Whirlwind);
                    return;
                }
                //Slam low level
                if (API.CanCast(Slam) && API.PlayerRage >= 20 && PlayerLevel < 19)
                {
                    API.CastSpell(Slam);
                    return;
                }
            }
        }
    }
}

