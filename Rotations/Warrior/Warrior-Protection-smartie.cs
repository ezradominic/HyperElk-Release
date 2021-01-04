// Changelog
// v1.0 First release
// v1.1 Victory Rush fix
// v1.2 covenants added - shield block changed
// v1.3 covenant update
// v1.4 heroic throw update
// v1.5 condemn fix
// v1.6 dps toggle and alot more fine tuning for more defensives
// v1.7 rage update
// v1.8 Racials and Trinkets
// v1.9 condemn fix hopefully
// v2.0 typo fix
// v2.1 Spell ids and alot of other stuff
// v2.2 Rallying cry added
// v2.3 Racials and a few other things

using System.Linq;

namespace HyperElk.Core
{
    public class ProtWarrior : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsDPS => API.ToggleIsEnabled("DPS");
        private bool IsRavager => API.ToggleIsEnabled("Ravager");
        //Spell,Auras
        private string ShieldSlam = "Shield Slam";
        private string Devastate = "Devastate";
        private string ShieldBlock = "Shield Block";
        private string ThunderClap = "Thunder Clap";
        private string Revenge = "Revenge";
        private string IgnorePain = "Ignore Pain";
        private string Execute = "Execute";
        private string Avatar = "Avatar";
        private string Shockwave = "Shockwave";
        private string Pummel = "Pummel";
        private string RallyingCry = "Rallying Cry";
        private string SpellReflection = "Spell Reflection";
        private string VictoryRush = "Victory Rush";
        private string ImpendingVictory = "Impending Victory";
        private string ShieldWall = "Shield Wall";
        private string LastStand = "Last Stand";
        private string DemoralizingShout = "Demoralizing Shout";
        private string HeroicThrow = "Heroic Throw";
        private string BattleShout = "Battle Shout";
        private string StormBolt = "Storm Bolt";
        private string DragonRoar = "Dragon Roar";
        private string Ravager = "Ravager";
        private string DeepWounds = "Deep Wounds";
        private string FreeRevenge = "Revenge!";
        private string VengeanceRevenge = "Vengeance: Revenge";
        private string VengeanceIgnorePain = "Vengeance: Ignore Pain";
        private string RenewedFury = "Renewed Fury";
        private string Victorious = "Victorious";
        private string Condemn = "Condemn";
        private string SpearofBastion = "Spear of Bastion";
        private string AncientAftershock = "Ancient Aftershock";
        private string ConquerorsBanner = "Conqueror's Banner";
        private string PhialofSerenity = "Phial of Serenity";
        private string BerserkerRage = "Berserker Rage";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string AoE = "AOE";
        private string AoERaid = "AoERaid";

        //Talents
        bool TalentDevastator => API.PlayerIsTalentSelected(1, 3);
        bool TalentStormBolt => API.PlayerIsTalentSelected(2, 3);
        bool TalentDragonRoar => API.PlayerIsTalentSelected(3, 3);
        bool TalentBoomingVoice => API.PlayerIsTalentSelected(3, 2);
        bool TalentImpendingVictory => API.PlayerIsTalentSelected(5, 3);
        bool TalentRavager => API.PlayerIsTalentSelected(6, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee;


        //CBProperties
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
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseHeroicThrow => heroiclist[CombatRoutine.GetPropertyInt(HeroicThrow)];
        string[] heroiclist = new string[] {"Not Used", "when out of melee", "only Mouseover", "both"};
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private int LastStandLifePercent => numbList[CombatRoutine.GetPropertyInt(LastStand)];
        private int ShieldWallLifePercent => numbList[CombatRoutine.GetPropertyInt(ShieldWall)]; 
        private int RallyLifePercent => numbList[CombatRoutine.GetPropertyInt(RallyingCry)];
        private int VictoryRushLifePercent => numbList[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => numbList[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        private int IgnorePainLifePercent => numbList[CombatRoutine.GetPropertyInt(IgnorePain)];
        private int DemoralizingShoutLifePercent => numbList[CombatRoutine.GetPropertyInt(DemoralizingShout)];
        private int ShieldBlockFirstChargeLifePercent => numbList[CombatRoutine.GetPropertyInt("FirstShieldBlock")];
        private int ShieldBlockSecondChargeLifePercent => numbList[CombatRoutine.GetPropertyInt("SecondShieldBlock")];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];


        public override void Initialize()
        {
            CombatRoutine.Name = "Protection Warrior by smartie";
            API.WriteLog("Welcome to smartie`s Protection Warrior v2.3");

            //Spells
            CombatRoutine.AddSpell(ShieldSlam,23922, "D4");
            CombatRoutine.AddSpell(Devastate,20243, "D5");
            CombatRoutine.AddSpell(ShieldBlock,2565, "D6");
            CombatRoutine.AddSpell(ThunderClap,6343, "D7");
            CombatRoutine.AddSpell(Revenge,6572, "D8");
            CombatRoutine.AddSpell(IgnorePain,190456, "D9");
            CombatRoutine.AddSpell(Execute, 163201, "D0");
            CombatRoutine.AddSpell(Avatar,107574, "D2");
            CombatRoutine.AddSpell(Shockwave,46968, "F12");
            CombatRoutine.AddSpell(Pummel,6552, "F11");
            CombatRoutine.AddSpell(BerserkerRage, 18499, "F1");
            CombatRoutine.AddSpell(RallyingCry,97462, "F7");
            CombatRoutine.AddSpell(SpellReflection,23920, "F6");
            CombatRoutine.AddSpell(VictoryRush,34428, "F5");
            CombatRoutine.AddSpell(ImpendingVictory,202168, "F5");
            CombatRoutine.AddSpell(ShieldWall,871, "F4");
            CombatRoutine.AddSpell(LastStand,12975, "F2");
            CombatRoutine.AddSpell(DemoralizingShout,1160, "F1");
            CombatRoutine.AddSpell(HeroicThrow,57755, "NumPad1");
            CombatRoutine.AddSpell(BattleShout,6673, "NumPad3");
            CombatRoutine.AddSpell(StormBolt,107570, "F8");
            CombatRoutine.AddSpell(DragonRoar,118000, "NumPad5");
            CombatRoutine.AddSpell(Ravager,228920, "NumPad6");
            CombatRoutine.AddSpell(Condemn,317349, "D0");
            CombatRoutine.AddSpell(ConquerorsBanner, 324143, "D1");
            CombatRoutine.AddSpell(AncientAftershock, 325886, "D1");
            CombatRoutine.AddSpell(SpearofBastion, 307865, "D1");

            //Macros
            CombatRoutine.AddMacro(HeroicThrow + "MO", "D2");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");


            //Buffs
            CombatRoutine.AddBuff(FreeRevenge, 5302);
            CombatRoutine.AddBuff(Avatar, 107574);
            CombatRoutine.AddBuff(VengeanceRevenge, 202573);
            CombatRoutine.AddBuff(VengeanceIgnorePain, 202574);
            CombatRoutine.AddBuff(IgnorePain, 190456);
            CombatRoutine.AddBuff(RenewedFury, 202288);
            CombatRoutine.AddBuff(ShieldBlock, 132404);
            CombatRoutine.AddBuff(LastStand, 12975);
            CombatRoutine.AddBuff(SpellReflection, 23920);
            CombatRoutine.AddBuff(BattleShout, 6673);
            CombatRoutine.AddBuff(Victorious, 32216);

            //Debuff
            CombatRoutine.AddDebuff(DeepWounds,115767);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Ravager");
            CombatRoutine.AddToggle("DPS");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, " Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp(HeroicThrow, "Use Heroic Throw", heroiclist, "Use " + HeroicThrow + " ,when out of melee, only Mousover or both", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(LastStand, LastStand + " Life Percent", numbList, "Life percent at which" + LastStand + " is used, set to 0 to disable", "Defense", 20);
            CombatRoutine.AddProp(ShieldWall, ShieldWall + " Life Percent", numbList, "Life percent at which" + ShieldWall + " is used, set to 0 to disable", "Defense", 30);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", numbList, "Life percent at which" + VictoryRush + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", numbList, "Life percent at which" + ImpendingVictory + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(DemoralizingShout, DemoralizingShout + " Life Percent", numbList, "Life percent at which" + DemoralizingShout + " is used without Booming Voice Talent, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(IgnorePain, IgnorePain + " Life Percent", numbList, "Life percent at which" + IgnorePain + " is used, set to 0 to disable", "Defense", 90);
            CombatRoutine.AddProp("FirstShieldBlock", "ShieldBlock 1. Charge" + " Life Percent", numbList, "Life percent at which" + ShieldBlock + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp("SecondShieldBlock", "ShieldBlock 2. Charge" + " Life Percent", numbList, "Life percent at which" + ShieldBlock + " is used, set to 0 to disable", "Defense", 50);
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
            if (isInterrupt && API.CanCast(Shockwave) && IsMelee && API.SpellISOnCooldown(Pummel))
            {
                API.CastSpell(Shockwave);
                return;
            }
            if (API.CanCast(RallyingCry) && RallyAoE)
            {
                API.CastSpell(RallyingCry);
                return;
            }
            if (PlayerRaceSettings == "Tauren" && API.CanCast(RacialSpell1) && isInterrupt && !API.PlayerIsMoving && isRacial && IsMelee && API.SpellISOnCooldown(Pummel))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast(ShieldBlock) && API.PlayerHealthPercent <= ShieldBlockFirstChargeLifePercent && !IsDPS && API.SpellCharges(ShieldBlock) == 2 && PlayerLevel >= 6 && API.PlayerRage >= 30 && !API.PlayerHasBuff(ShieldBlock))
            {
                API.CastSpell(ShieldBlock);
                return;
            }
            if (API.CanCast(ShieldBlock) && API.PlayerHealthPercent <= ShieldBlockSecondChargeLifePercent && !IsDPS && API.SpellCharges(ShieldBlock) < 2 && PlayerLevel >= 6 && API.PlayerRage >= 30 && !API.PlayerHasBuff(ShieldBlock))
            {
                API.CastSpell(ShieldBlock);
                return;
            }
            if (API.CanCast(IgnorePain) && !IsDPS && (API.PlayerRage >= 40 || API.PlayerRage >= 26 && API.PlayerHasBuff(VengeanceIgnorePain)) && (!API.PlayerHasBuff(IgnorePain) || API.PlayerHasBuff(IgnorePain) && API.PlayerBuffTimeRemaining(IgnorePain) < 200) && API.PlayerHealthPercent <= IgnorePainLifePercent && PlayerLevel >= 17)
            {
                API.CastSpell(IgnorePain);
                return;
            }
            if (API.CanCast(LastStand) && API.PlayerHealthPercent <= LastStandLifePercent && !IsDPS && PlayerLevel >= 38)
            {
                API.CastSpell(LastStand);
                return;
            }
            if (API.CanCast(ShieldWall) && API.PlayerHealthPercent <= ShieldWallLifePercent && !IsDPS && PlayerLevel >= 23 && !API.PlayerHasBuff(LastStand))
            {
                API.CastSpell(ShieldWall);
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
            if (API.CanCast(DemoralizingShout) && API.PlayerHealthPercent <= DemoralizingShoutLifePercent && IsMelee && PlayerLevel >= 27 && !TalentBoomingVoice)
            {
                API.CastSpell(DemoralizingShout);
                return;
            }
            if (API.CanCast(VictoryRush) && API.PlayerHealthPercent <= VictoryRushLifePercent && API.PlayerHasBuff(Victorious) && PlayerLevel >= 5 && IsMelee)
            {
                API.CastSpell(VictoryRush);
                return;
            }
            if (API.CanCast(ImpendingVictory) && API.PlayerHealthPercent <= ImpendingVictoryLifePercent && !IsDPS && TalentImpendingVictory && IsMelee)
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
                //actions+=/blood_fury
                if (PlayerRaceSettings == "Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/berserking,if=buff.recklessness.up
                if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/lights_judgment,if=buff.recklessness.down&debuff.siegebreaker.down
                if (PlayerRaceSettings == "Lightforged" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
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
                if (PlayerRaceSettings == "Vulpera" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
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
                if (API.CanCast(ConquerorsBanner) && PlayerCovenantSettings == "Necrolord" && !API.PlayerIsMoving && (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                {
                    API.CastSpell(ConquerorsBanner);
                    return;
                }
                if (API.CanCast(SpearofBastion) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsMoving && (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                {
                    API.CastSpell(SpearofBastion);
                    return;
                }
                if (API.CanCast(AncientAftershock) && PlayerCovenantSettings == "Night Fae" && !API.PlayerIsMoving && (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                {
                    API.CastSpell(AncientAftershock);
                    return;
                }
                if (API.CanCast(Avatar) && IsCooldowns && PlayerLevel >= 32 && API.PlayerRage < 70)
                {
                    API.CastSpell(Avatar);
                    return;
                }
                if (API.CanCast(DemoralizingShout) && TalentBoomingVoice && API.PlayerRage < 60)
                {
                    API.CastSpell(DemoralizingShout);
                    return;
                }
                if (API.CanCast(Ravager) && TalentRavager && API.PlayerRage < 70 && IsRavager)
                {
                    API.CastSpell(Ravager);
                    return;
                }
                if (API.CanCast(DragonRoar) && TalentDragonRoar && API.PlayerRage < 80)
                {
                    API.CastSpell(DragonRoar);
                    return;
                }
                if (API.CanCast(Revenge) && API.PlayerRage >= 20 && IsDPS && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE) && PlayerLevel >= 12)
                {
                    API.CastSpell(Revenge);
                    return;
                }
                if (API.CanCast(Revenge) && API.PlayerRage >= 70 && !IsDPS && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE) && PlayerLevel >= 12)
                {
                    API.CastSpell(Revenge);
                    return;
                }
                if (API.CanCast(ThunderClap) && PlayerLevel >= 19 && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                {
                    API.CastSpell(ThunderClap);
                    return;
                }
                if (API.CanCast(ShieldSlam) && PlayerLevel >= 3)
                {
                    API.CastSpell(ShieldSlam);
                    return;
                }
                if (API.CanCast(ThunderClap) && PlayerLevel >= 19)
                {
                    API.CastSpell(ThunderClap);
                    return;
                }
                if (API.CanCast(Execute) && PlayerCovenantSettings != "Venthyr" && API.PlayerRage > 20 && API.TargetHealthPercent < 20 && IsDPS && PlayerLevel >= 10)
                {
                    API.CastSpell(Execute);
                    return;
                }
                if (API.CanCast(Condemn, true, false) && PlayerCovenantSettings == "Venthyr" && API.PlayerRage > 20 && (API.TargetHealthPercent < 20 || API.TargetHealthPercent > 80) && IsDPS)
                {
                    API.CastSpell(Condemn);
                    return;
                }
                if (API.CanCast(Execute) && PlayerCovenantSettings != "Venthyr" && API.PlayerRage > 70 && API.TargetHealthPercent < 20 && !IsDPS && PlayerLevel >= 10)
                {
                    API.CastSpell(Execute);
                    return;
                }
                if (API.CanCast(Condemn, true, false) && PlayerCovenantSettings == "Venthyr" && API.PlayerRage > 70 && (API.TargetHealthPercent < 20 || API.TargetHealthPercent > 80) && !IsDPS)
                {
                    API.CastSpell(Condemn);
                    return;
                }
                if (API.CanCast(Revenge) && API.PlayerHasBuff(FreeRevenge) && PlayerLevel >= 12)
                {
                    API.CastSpell(Revenge);
                    return;
                }
                if (API.CanCast(Revenge) && TalentDevastator && API.PlayerRage >= 20 && IsDPS && PlayerLevel >= 14)
                {
                    API.CastSpell(Revenge);
                    return;
                }
                if (API.CanCast(Revenge) && TalentDevastator && API.PlayerRage >= 70 && !IsDPS && PlayerLevel >= 14)
                {
                    API.CastSpell(Revenge);
                    return;
                }
                if (API.CanCast(Devastate) && !TalentDevastator && PlayerLevel >= 14)
                {
                    API.CastSpell(Devastate);
                    return;
                }
            }
        }
	}
}