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
        private string Condemn = "330325";
        private string SpearofBastion = "Spear of Bastion";
        private string AncientAftershock = "Ancient Aftershock";
        private string ConquerorsBanner = "Conqueror's Banner";



        //Talents
        bool TalentImpendingVictory => API.PlayerIsTalentSelected(2, 2);
        bool TalentStormBolt => API.PlayerIsTalentSelected(2, 3);
        bool TalentMassacre => API.PlayerIsTalentSelected(3, 1);
        bool TalentOnslaught => API.PlayerIsTalentSelected(3, 3);
        bool TalentDragonRoar => API.PlayerIsTalentSelected(6, 2);
        bool TalentBladestorm => API.PlayerIsTalentSelected(6, 3);
        bool TalentSiegebreaker => API.PlayerIsTalentSelected(7, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;
        private float gcd => API.SpellGCDTotalDuration;

        bool IsRecklessness => UseRecklessness == "with Cooldowns" && IsCooldowns || UseRecklessness == "always";
        bool IsSiegebreaker => UseSiegebreaker == "with Cooldowns" && IsCooldowns || UseSiegebreaker == "always";
        bool IsDragonRoar => UseDragonRoar == "with Cooldowns" && IsCooldowns || UseDragonRoar == "always" || UseDragonRoar == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber;
        bool IsBladestorm => UseBladestorm == "with Cooldowns" && IsCooldowns || UseBladestorm == "always" || UseBladestorm == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber;
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee;


        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseHeroicThrow => heroiclist[CombatRoutine.GetPropertyInt(HeroicThrow)];
        private bool IsLineUp => CombatRoutine.GetPropertyBool("LineUp");
        private int EnragedRegenerationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(EnragedRegeneration)];
        private int VictoryRushLifePercent => percentListProp[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        string[] heroiclist = new string[] { "Not Used", "when out of melee", "only Mouseover", "both" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseRecklessness => CDUsage[CombatRoutine.GetPropertyInt(Recklessness)];
        private string UseSiegebreaker => CDUsage[CombatRoutine.GetPropertyInt(Siegebreaker)];
        private string UseDragonRoar => CDUsageWithAOE[CombatRoutine.GetPropertyInt(DragonRoar)];
        private string UseBladestorm => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Bladestorm)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Fury Warrior by smartie";
            API.WriteLog("Welcome to smartie`s Fury Warrior v2.3");
            API.WriteLog("Condemn is buggy for Fury currently and was added as id instead of name to fix it");

            //Spells
            CombatRoutine.AddSpell(Bloodthirst, "D1");
            CombatRoutine.AddSpell(Onslaught, "D2");
            CombatRoutine.AddSpell(RagingBlow, "D3");
            CombatRoutine.AddSpell(Rampage, "D4");
            CombatRoutine.AddSpell(Recklessness, "D5");
            CombatRoutine.AddSpell(Execute, "D7");
            CombatRoutine.AddSpell(Whirlwind, "D8");
            CombatRoutine.AddSpell(BerserkerRage, "F1");
            CombatRoutine.AddSpell(DragonRoar, "F2");
            CombatRoutine.AddSpell(Pummel, "F5");
            CombatRoutine.AddSpell(HeroicThrow, "F6");
            CombatRoutine.AddSpell(EnragedRegeneration, "F6");
            CombatRoutine.AddSpell(Siegebreaker, "F9");
            CombatRoutine.AddSpell(VictoryRush, "NumPad3");
            CombatRoutine.AddSpell(ImpendingVictory, "F3");
            CombatRoutine.AddSpell(BattleShout, "Q");
            CombatRoutine.AddSpell(StormBolt, "F7");
            CombatRoutine.AddSpell(RallyingCry, "F2");
            CombatRoutine.AddSpell(Bladestorm, "None");
            CombatRoutine.AddSpell(Condemn, "D6");
            CombatRoutine.AddSpell(ConquerorsBanner, "D1");
            CombatRoutine.AddSpell(AncientAftershock, "D1");
            CombatRoutine.AddSpell(SpearofBastion, "D1");
            CombatRoutine.AddSpell(Slam, "None");

            //Macros
            CombatRoutine.AddMacro(HeroicThrow + "MO", "D2");
            CombatRoutine.AddMacro(Execute + "MO", "D6");
            CombatRoutine.AddMacro(Condemn + "MO", "D6");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

            //Buffs
            CombatRoutine.AddBuff(Enrage);
            CombatRoutine.AddBuff(Whirlwind);
            CombatRoutine.AddBuff(Recklessness);
            CombatRoutine.AddBuff(DragonRoar);
            CombatRoutine.AddBuff(SuddenDeath);
            CombatRoutine.AddBuff(VictoryRush);
            CombatRoutine.AddBuff(BattleShout);
            CombatRoutine.AddBuff(Victorious);
            CombatRoutine.AddBuff(Bladestorm);

            //Debuff
            CombatRoutine.AddDebuff(Siegebreaker);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Bladestorm");

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
            CombatRoutine.AddProp(EnragedRegeneration, EnragedRegeneration + " Life Percent", percentListProp, " Life percent at which" + EnragedRegeneration + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", percentListProp, "Life percent at which" + VictoryRush + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", percentListProp, "Life percent at which" + ImpendingVictory + " is used, set to 0 to disable", "Defense", 8);

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
            if (isInterrupt && API.CanCast(Pummel) && IsMelee && PlayerLevel >= 7)
            {
                API.CastSpell(Pummel);
                return;
            }
            if (API.CanCast(RacialSpell1) && isInterrupt && PlayerRaceSettings == "Tauren" && isRacial && IsMelee && API.SpellISOnCooldown(Pummel))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.PlayerHealthPercent <= EnragedRegenerationLifePercent && PlayerLevel >= 23 && API.CanCast(EnragedRegeneration))
            {
                API.CastSpell(EnragedRegeneration);
                return;
            }
            if (API.PlayerHealthPercent <= VictoryRushLifePercent && PlayerLevel >= 5 && API.CanCast(VictoryRush) && API.PlayerHasBuff(Victorious) && IsMelee)
            {
                API.CastSpell(VictoryRush);
                return;
            }
            if (API.PlayerHealthPercent <= ImpendingVictoryLifePercent && TalentImpendingVictory && API.CanCast(ImpendingVictory))
            {
                API.CastSpell(ImpendingVictory);
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
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/berserking,if=buff.recklessness.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && isRacial && IsCooldowns && IsMelee && API.PlayerHasBuff(Recklessness))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/lights_judgment,if=buff.recklessness.down&debuff.siegebreaker.down
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged" && isRacial && IsCooldowns && IsMelee && !API.TargetHasDebuff(Siegebreaker) && !API.PlayerHasBuff(Recklessness))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/fireblood
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/ancestral_call
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/bag_of_tricks,if=buff.recklessness.down&debuff.siegebreaker.down&buff.enrage.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Vulpera" && isRacial && IsCooldowns && IsMelee && !API.TargetHasDebuff(Siegebreaker) && !API.PlayerHasBuff(Recklessness) && API.PlayerHasBuff(Enrage))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
                {
                    API.CastSpell("Trinket2");
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
                if (API.CanCast(Recklessness) && PlayerLevel >= 38 && !API.PlayerHasBuff(Recklessness) && IsRecklessness)
                {
                    API.CastSpell(Recklessness);
                    return;
                }
                if (API.CanCast(Rampage) && PlayerLevel >= 19 && API.PlayerRage >= 80 && API.SpellCDDuration(Recklessness) < 300)
                {
                    API.CastSpell(Rampage);
                    return;
                }
                if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && (PlayerLevel < 22 && API.PlayerRage >= 30 || PlayerLevel >= 22) && (!API.PlayerHasBuff(Whirlwind) && PlayerLevel >= 37 || PlayerLevel < 37) && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                {
                    API.CastSpell(Whirlwind);
                    return;
                }
                //actions.single_target+=/siegebreaker,if=spell_targets.whirlwind>1|raid_event.adds.in>15
                if (API.CanCast(Siegebreaker) && TalentSiegebreaker && !API.TargetHasDebuff(Siegebreaker) && (IsLineUp && API.SpellCDDuration(Recklessness) > 3000 || !IsLineUp || UseRecklessness == "with Cooldowns" && !IsCooldowns) && IsSiegebreaker)
                {
                    API.CastSpell(Siegebreaker);
                    return;
                }
                //actions.single_target+=/rampage,if=buff.recklessness.up|(buff.enrage.remains<gcd|rage>90)|buff.frenzy.remains<1.5
                if (API.CanCast(Rampage) && PlayerLevel >= 19 && (API.PlayerHasBuff(Recklessness) && API.PlayerRage >= 80 || API.PlayerBuffTimeRemaining(Enrage) < gcd && API.PlayerRage >= 80 || API.PlayerRage > 90))
                {
                    API.CastSpell(Rampage);
                    return;
                }
                //actions.single_target+=/execute
                if (API.CanCast(Execute) && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 9 && (!TalentMassacre && API.TargetHealthPercent < 20 || TalentMassacre && API.TargetHealthPercent < 35 || API.PlayerHasBuff(SuddenDeath)))
                {
                    API.CastSpell(Execute);
                    return;
                }
                //actions.single_target+=/condemn
                if (!API.SpellISOnCooldown(Condemn) && PlayerCovenantSettings == "Venthyr" && (!TalentMassacre && (API.TargetHealthPercent < 20 || API.TargetHealthPercent > 80) || TalentMassacre && (API.TargetHealthPercent < 35 || API.TargetHealthPercent > 80) || API.PlayerHasBuff(SuddenDeath)))
                {
                    API.CastSpell(Condemn);
                    return;
                }
                if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                {
                    if (API.CanCast(Execute) && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 9 && (!TalentMassacre && API.MouseoverHealthPercent < 20 || TalentMassacre && API.MouseoverHealthPercent < 35 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(Execute + "MO");
                        return;
                    }
                    if (!API.SpellISOnCooldown(Condemn) && PlayerCovenantSettings == "Venthyr" && (!TalentMassacre && (API.MouseoverHealthPercent < 20 || API.MouseoverHealthPercent > 80) || TalentMassacre && (API.MouseoverHealthPercent < 35 || API.MouseoverHealthPercent > 80) || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(Condemn + "MO");
                        return;
                    }
                }
                //actions.single_target+=/bladestorm,if=buff.enrage.up&(spell_targets.whirlwind>1|raid_event.adds.in>45)
                if (API.CanCast(Bladestorm) && API.PlayerHasBuff(Enrage) && TalentBladestorm && IsBladestorm && BladestormToggle)
                {
                    API.CastSpell(Bladestorm);
                    return;
                }
                //actions.single_target +=/ bloodthirst,if= buff.enrage.down | conduit.vicious_contempt.rank > 5 & target.health.pct < 35 & !talent.cruelty.enabled
                //actions.single_target +=/ bloodbath,if= buff.enrage.down | conduit.vicious_contempt.rank > 5 & target.health.pct < 35 & !talent.cruelty.enabled
                //actions.single_target +=/ dragon_roar,if= buff.enrage.up & (spell_targets.whirlwind > 1 | raid_event.adds.in> 15)
                if (API.CanCast(DragonRoar) && TalentDragonRoar && API.PlayerHasBuff(Enrage) && IsDragonRoar)
                {
                    API.CastSpell(DragonRoar);
                    return;
                }
                //actions.single_target+=/onslaught
                if (API.CanCast(Onslaught) && API.PlayerHasBuff(Enrage) && TalentOnslaught)
                {
                    API.CastSpell(Onslaught);
                    return;
                }
                //actions.single_target+=/raging_blow,if=charges=2
                if (API.CanCast(RagingBlow) && PlayerLevel >= 12 && API.SpellCharges(RagingBlow) == 2)
                {
                    API.CastSpell(RagingBlow);
                    return;
                }
                //actions.single_target+=/crushing_blow,if=charges=2
                //actions.single_target+=/bloodthirst
                if (API.CanCast(Bloodthirst) && PlayerLevel >= 10)
                {
                    API.CastSpell(Bloodthirst);
                    return;
                }
                //actions.single_target+=/bloodbath
                //actions.single_target+=/raging_blow
                if (API.CanCast(RagingBlow) && PlayerLevel >= 12)
                {
                    API.CastSpell(RagingBlow);
                    return;
                }
                //actions.single_target+=/crushing_blow
                //actions.single_target+=/whirlwind
                if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && (PlayerLevel < 22 && API.PlayerRage >= 30 || PlayerLevel >= 22))
                {
                    API.CastSpell(Whirlwind);
                    return;
                }
                if (API.CanCast(Slam) && API.PlayerRage >= 20 && PlayerLevel < 19)
                {
                    API.CastSpell(Slam);
                    return;
                }
            }
        }
    }
}

