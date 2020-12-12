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
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseHeroicThrow => heroiclist[CombatRoutine.GetPropertyInt(HeroicThrow)];
        string[] heroiclist = new string[] {"Not Used", "when out of melee", "only Mouseover", "both"};
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private int LastStandLifePercent => percentListProp[CombatRoutine.GetPropertyInt(LastStand)];
        private int ShieldWallLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ShieldWall)];
        private int VictoryRushLifePercent => percentListProp[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        private int IgnorePainLifePercent => percentListProp[CombatRoutine.GetPropertyInt(IgnorePain)];
        private int DemoralizingShoutLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DemoralizingShout)];
        private int ShieldBlockFirstChargeLifePercent => percentListProp[CombatRoutine.GetPropertyInt("FirstShieldBlock")];
        private int ShieldBlockSecondChargeLifePercent => percentListProp[CombatRoutine.GetPropertyInt("SecondShieldBlock")];


        public override void Initialize()
        {
            CombatRoutine.Name = "Protection Warrior by smartie";
            API.WriteLog("Welcome to smartie`s Protection Warrior v2.0");

            //Spells
            CombatRoutine.AddSpell(ShieldSlam, "D4");
            CombatRoutine.AddSpell(Devastate, "D5");
            CombatRoutine.AddSpell(ShieldBlock, "D6");
            CombatRoutine.AddSpell(ThunderClap, "D7");
            CombatRoutine.AddSpell(Revenge, "D8");
            CombatRoutine.AddSpell(IgnorePain, "D9");
            CombatRoutine.AddSpell(Execute, "D0");
            CombatRoutine.AddSpell(Avatar, "D2");
            CombatRoutine.AddSpell(Shockwave, "F12");
            CombatRoutine.AddSpell(Pummel, "F11");
            CombatRoutine.AddSpell(RallyingCry, "F7");
            CombatRoutine.AddSpell(SpellReflection, "F6");
            CombatRoutine.AddSpell(VictoryRush, "F5");
            CombatRoutine.AddSpell(ImpendingVictory, "F5");
            CombatRoutine.AddSpell(ShieldWall, "F4");
            CombatRoutine.AddSpell(LastStand, "F2");
            CombatRoutine.AddSpell(DemoralizingShout, "F1");
            CombatRoutine.AddSpell(HeroicThrow, "NumPad1");
            CombatRoutine.AddSpell(BattleShout, "NumPad3");
            CombatRoutine.AddSpell(StormBolt, "F8");
            CombatRoutine.AddSpell(DragonRoar, "NumPad5");
            CombatRoutine.AddSpell(Ravager, "NumPad6");
            CombatRoutine.AddSpell(Condemn, "D0");
            CombatRoutine.AddSpell(ConquerorsBanner, "D1");
            CombatRoutine.AddSpell(AncientAftershock, "D1");
            CombatRoutine.AddSpell(SpearofBastion, "D1");

            //Macros
            CombatRoutine.AddMacro(HeroicThrow + "MO", "D2");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");


            //Buffs
            CombatRoutine.AddBuff(FreeRevenge);
            CombatRoutine.AddBuff(VengeanceRevenge);
            CombatRoutine.AddBuff(VengeanceIgnorePain);
            CombatRoutine.AddBuff(IgnorePain);
            CombatRoutine.AddBuff(RenewedFury);
            CombatRoutine.AddBuff(ShieldBlock);
            CombatRoutine.AddBuff(LastStand);
            CombatRoutine.AddBuff(SpellReflection);
            CombatRoutine.AddBuff(VictoryRush);
            CombatRoutine.AddBuff(BattleShout);
            CombatRoutine.AddBuff(Victorious);

            //Debuff
            CombatRoutine.AddDebuff(DeepWounds);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Ravager");
            CombatRoutine.AddToggle("DPS");

            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, " Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp(HeroicThrow, "Use Heroic Throw", heroiclist, "Use " + HeroicThrow + " ,when out of melee, only Mousover or both", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(LastStand, LastStand + " Life Percent", percentListProp, "Life percent at which" + LastStand + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(ShieldWall, ShieldWall + " Life Percent", percentListProp, "Life percent at which" + ShieldWall + " is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", percentListProp, "Life percent at which" + VictoryRush + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", percentListProp, "Life percent at which" + ImpendingVictory + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(DemoralizingShout, DemoralizingShout + " Life Percent", percentListProp, "Life percent at which" + DemoralizingShout + " is used without Booming Voice Talent, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(IgnorePain, IgnorePain + " Life Percent", percentListProp, "Life percent at which" + IgnorePain + " is used, set to 0 to disable", "Defense", 9);
            CombatRoutine.AddProp("FirstShieldBlock", "ShieldBlock 1. Charge" + " Life Percent", percentListProp, "Life percent at which" + ShieldBlock + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp("SecondShieldBlock", "ShieldBlock 2. Charge" + " Life Percent", percentListProp, "Life percent at which" + ShieldBlock + " is used, set to 0 to disable", "Defense", 5);

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
            if (isInterrupt && API.CanCast(Shockwave) && IsMelee && API.SpellISOnCooldown(Pummel))
            {
                API.CastSpell(Shockwave);
                return;
            }
            if (API.CanCast(RacialSpell1) && isInterrupt && PlayerRaceSettings == "Tauren" && isRacial && IsMelee && API.SpellISOnCooldown(Pummel))
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
            if (API.CanCast(ImpendingVictory) && API.PlayerHealthPercent <= ImpendingVictoryLifePercent && !IsDPS && TalentImpendingVictory)
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
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/lights_judgment,if=buff.recklessness.down&debuff.siegebreaker.down
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged" && isRacial && IsCooldowns && IsMelee)
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
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Vulpera" && isRacial && IsCooldowns && IsMelee)
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