//Changelog test@test.de
// v1.0 First release
// v1.1 Victory Rush fix
// v1.2 covenant support
// v1.3 big apl update + heroic throw stuff
// v1.4 condemn fix
// v1.5 Bladestorm toggle added
// v1.6 colossus smash toogle and legendary prep
// v1.7 Racials and Trinkets
// v1.8 sweeping strikes fix
// v1.9 condemn fix hopefully

namespace HyperElk.Core
{
    public class ArmsWarrior : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool BladestormToggle => API.ToggleIsEnabled("Bladestorm");
        private bool ColossusToggle => API.ToggleIsEnabled("Colossus Smash");
        //Spell,Auras
        private string MortalStrike = "Mortal Strike";
        private string ColossusSmash = "Colossus Smash";
        private string Warbreaker = "Warbreaker";
        private string Slam = "Slam";
        private string Execute = "Execute";
        private string Cleave = "Cleave";
        private string Whirlwind = "Whirlwind";
        private string Bladestorm = "Bladestorm";
        private string Ravager = "Ravager";
        private string Rend = "Rend";
        private string VictoryRush = "Victory Rush";
        private string ImpendingVictory = "Impending Victory";
        private string DiebytheSword = "Die by the Sword";
        private string Pummel = "Pummel";
        private string SweepingStrikes = "Sweeping Strikes";
        private string Skullsplitter = "Skullsplitter";
        private string Overpower = "Overpower";
        private string DeadlyCalm = "Deadly Calm";
        private string DefensiveStance = "Defensive Stance";
        private string BattleShout = "Battle Shout";
        private string Avatar = "Avatar";
        private string StormBolt = "Storm Bolt";
        private string SuddenDeath = "Sudden Death";
        private string DeepWounds = "Deep Wounds";
        private string Victorious = "Victorious";
        private string Condemn = "Condemn";
        private string SpearofBastion = "Spear of Bastion";
        private string AncientAftershock = "Ancient Aftershock";
        private string ConquerorsBanner = "Conqueror's Banner";
        private string HeroicThrow = "Heroic Throw";
        private string Exploiter = "Exploiter";

        //Talents
        bool TalentSkullsplitter => API.PlayerIsTalentSelected(1, 3);
        bool TalentImpendingVictory => API.PlayerIsTalentSelected(2, 2);
        bool TalentMassacre => API.PlayerIsTalentSelected(3, 1);
        bool TalentStormBolt => API.PlayerIsTalentSelected(2, 3);
        bool TalentRend => API.PlayerIsTalentSelected(3, 3);
        bool TalentFevorofBattle => API.PlayerIsTalentSelected(3, 2);
        bool TalentDefensiveStance => API.PlayerIsTalentSelected(4, 3);
        bool TalentWarbreaker => API.PlayerIsTalentSelected(5, 2);
        bool TalentCleave => API.PlayerIsTalentSelected(5, 3);
        bool TalentAvatar => API.PlayerIsTalentSelected(6, 2);
        bool TalentDeadlyCalm => API.PlayerIsTalentSelected(6, 3);
        bool TalentDreadnaught => API.PlayerIsTalentSelected(7, 2);
        bool TalentRavager => API.PlayerIsTalentSelected(7, 3);

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;

        //rota bools
        bool IsAvatar => UseAvatar == "with Cooldowns" && IsCooldowns || UseAvatar == "always";
        bool IsRavager => UseRavager == "with Cooldowns" && IsCooldowns || UseRavager == "always" || UseRavager == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber;
        bool IsBladestorm => UseBladestorm == "with Cooldowns" && IsCooldowns || UseBladestorm == "always" || UseBladestorm == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber;
        bool IsColossusSmash => UseColossusSmash == "with Cooldowns" && IsCooldowns || UseColossusSmash == "always";
        bool IsWarbreaker => UseWarbreaker == "with Cooldowns" && IsCooldowns || UseWarbreaker == "always" || UseWarbreaker == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber;
        bool IsDeadlyCalm => UseDeadlyCalm == "with Cooldowns" && IsCooldowns || UseDeadlyCalm == "always";
        bool IsCovenant => UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE;
        bool IsExecute => (TalentMassacre && API.TargetHealthPercent < 35 || !TalentMassacre && API.TargetHealthPercent < 20 || PlayerCovenantSettings == "Venthyr" && API.TargetHealthPercent > 80);
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee;


        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseHeroicThrow => heroiclist[CombatRoutine.GetPropertyInt(HeroicThrow)];
        string[] heroiclist = new string[] { "Not Used", "when out of melee", "only Mouseover", "both" };
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseAvatar => CDUsage[CombatRoutine.GetPropertyInt(Avatar)];
        private string UseRavager => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Ravager)];
        private string UseBladestorm => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Bladestorm)];
        private string UseColossusSmash => CDUsage[CombatRoutine.GetPropertyInt(ColossusSmash)];
        private string UseWarbreaker => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Warbreaker)];
        private string UseDeadlyCalm => CDUsage[CombatRoutine.GetPropertyInt(DeadlyCalm)];
        private int VictoryRushLifePercent => percentListProp[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        private int DiebytheSwordLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DiebytheSword)];
        private int DefensiveStanceLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DefensiveStance)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Arms Warrior by smartie";
            API.WriteLog("Welcome to smartie`s Arms Warrior v1.9");
            API.WriteLog("The Bladestorm toggle will also toggle Ravager");
            API.WriteLog("The Colossus Smash toggle will also toggle Warbreaker");

            //Spells
            CombatRoutine.AddSpell(MortalStrike, "D4");
            CombatRoutine.AddSpell(ColossusSmash, "D3");
            CombatRoutine.AddSpell(Warbreaker, "D3");
            CombatRoutine.AddSpell(Slam, "F12");
            CombatRoutine.AddSpell(Execute, "D6");
            CombatRoutine.AddSpell(Cleave, "NumPad4");
            CombatRoutine.AddSpell(Whirlwind, "D7");
            CombatRoutine.AddSpell(Bladestorm, "D5");
            CombatRoutine.AddSpell(Ravager, "D5");
            CombatRoutine.AddSpell(Rend, "D8");
            CombatRoutine.AddSpell(VictoryRush, "F3");
            CombatRoutine.AddSpell(ImpendingVictory, "F3");
            CombatRoutine.AddSpell(DiebytheSword, "F2");
            CombatRoutine.AddSpell(Pummel, "F11");
            CombatRoutine.AddSpell(HeroicThrow, "F6");
            CombatRoutine.AddSpell(SweepingStrikes, "None");
            CombatRoutine.AddSpell(Skullsplitter, "D8");
            CombatRoutine.AddSpell(Overpower, "NumPad3");
            CombatRoutine.AddSpell(DeadlyCalm, "NumPad5");
            CombatRoutine.AddSpell(DefensiveStance, "F6");
            CombatRoutine.AddSpell(BattleShout, "None");
            CombatRoutine.AddSpell(Avatar, "None");
            CombatRoutine.AddSpell(StormBolt, "F7");
            CombatRoutine.AddSpell(Condemn, "D6");
            CombatRoutine.AddSpell(ConquerorsBanner, "D1");
            CombatRoutine.AddSpell(AncientAftershock, "D1");
            CombatRoutine.AddSpell(SpearofBastion, "D1");

            //Macros
            CombatRoutine.AddMacro(HeroicThrow + "MO", "D2");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

            //Buffs
            CombatRoutine.AddBuff(BattleShout);
            CombatRoutine.AddBuff(DeadlyCalm);
            CombatRoutine.AddBuff(Overpower);
            CombatRoutine.AddBuff(SuddenDeath);
            CombatRoutine.AddBuff(SweepingStrikes);
            CombatRoutine.AddBuff(VictoryRush);
            CombatRoutine.AddBuff(Victorious);
            CombatRoutine.AddBuff(Avatar);
            CombatRoutine.AddBuff(Exploiter);

            //Debuff
            CombatRoutine.AddDebuff(ColossusSmash);
            CombatRoutine.AddDebuff(DeepWounds);
            CombatRoutine.AddDebuff(Rend);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Bladestorm");
            CombatRoutine.AddToggle("Colossus Smash");

            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, " Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp(HeroicThrow, "Use Heroic Throw", heroiclist, "Use " + HeroicThrow + " ,when out of melee, only Mousover or both", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Avatar, "Use " + Avatar, CDUsage, "Use " + Avatar + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Ravager, "Use " + Ravager, CDUsageWithAOE, "Use " + Ravager + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Bladestorm, "Use " + Bladestorm, CDUsageWithAOE, "Use " + Bladestorm + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(ColossusSmash, "Use " + ColossusSmash, CDUsage, "Use " + ColossusSmash + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Warbreaker, "Use " + Warbreaker, CDUsageWithAOE, "Use " + Warbreaker + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(DeadlyCalm, "Use " + DeadlyCalm, CDUsage, "Use " + DeadlyCalm + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", percentListProp, "Life percent at which" + VictoryRush + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", percentListProp, "Life percent at which" + ImpendingVictory + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(DiebytheSword, DiebytheSword + " Life Percent", percentListProp, "Life percent at which" + DiebytheSword + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(DefensiveStance, DefensiveStance + " Life Percent", percentListProp, "Life percent at which" + DefensiveStance + " is used, set to 0 to disable", "Defense", 8);
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
            if (API.CanCast(DiebytheSword) && PlayerLevel >= 23 && API.PlayerHealthPercent <= DiebytheSwordLifePercent)
            {
                API.CastSpell(DiebytheSword);
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
                //actions +=/ blood_fury,if= debuff.colossus_smash.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && isRacial && IsCooldowns && IsMelee && API.TargetHasDebuff(ColossusSmash))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ berserking,if= debuff.colossus_smash.remains > 6
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && isRacial && IsCooldowns && IsMelee && API.TargetDebuffRemainingTime(ColossusSmash) > 600)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ arcane_torrent,if= cooldown.mortal_strike.remains > 1.5 & rage < 50
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Blood Elf" && isRacial && IsCooldowns && IsMelee && API.SpellCDDuration(MortalStrike) > 150 && API.PlayerRage < 50)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ lights_judgment,if= debuff.colossus_smash.down & cooldown.mortal_strike.remains
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged" && isRacial && IsCooldowns && IsMelee && !API.TargetHasDebuff(ColossusSmash) && API.SpellISOnCooldown(MortalStrike))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ fireblood,if= debuff.colossus_smash.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && isRacial && IsCooldowns && IsMelee && API.TargetHasDebuff(ColossusSmash))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ ancestral_call,if= debuff.colossus_smash.up
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && isRacial && IsCooldowns && IsMelee && API.TargetHasDebuff(ColossusSmash))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions +=/ bag_of_tricks,if= debuff.colossus_smash.down & cooldown.mortal_strike.remains
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Vulpera" && isRacial && IsCooldowns && IsMelee && !API.TargetHasDebuff(ColossusSmash) && API.SpellISOnCooldown(MortalStrike))
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
                if (API.CanCast(ConquerorsBanner) && PlayerCovenantSettings == "Necrolord" && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(ConquerorsBanner);
                    return;
                }
                if (API.CanCast(SpearofBastion) && PlayerCovenantSettings == "Kyrian" && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(SpearofBastion);
                    return;
                }
                if (API.CanCast(AncientAftershock) && PlayerCovenantSettings == "Night Fae" && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(AncientAftershock);
                    return;
                }
                if (IsExecute)
                {
                    if (API.CanCast(DeadlyCalm) && TalentDeadlyCalm && IsDeadlyCalm)
                    {
                        API.CastSpell(DeadlyCalm);
                        return;
                    }
                    if (API.CanCast(Rend) && API.TargetDebuffRemainingTime(Rend) <= 360 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && TalentRend)
                    {
                        API.CastSpell(Rend);
                        return;
                    }
                    if (API.CanCast(SweepingStrikes) && PlayerLevel >= 22 && !API.PlayerHasBuff(SweepingStrikes) && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE && (API.SpellCDDuration(Bladestorm) > 1200 && IsBladestorm && !TalentRavager || !IsBladestorm || TalentRavager)))
                    {
                        API.CastSpell(SweepingStrikes);
                        return;
                    }
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 60 && TalentSkullsplitter && !API.PlayerHasBuff(DeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    if (API.CanCast(Avatar) && TalentAvatar && !API.PlayerHasBuff(Avatar) && (API.SpellCDDuration(ColossusSmash) < 100 && !TalentWarbreaker || API.SpellCDDuration(Warbreaker) < 100 && TalentWarbreaker) && IsAvatar)
                    {
                        API.CastSpell(Avatar);
                        return;
                    }
                    if (API.CanCast(Cleave) && TalentCleave && API.PlayerRage >= 20 && API.TargetDebuffRemainingTime(DeepWounds) < 150 && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                    {
                        API.CastSpell(Cleave);
                        return;
                    }
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker && IsWarbreaker && ColossusToggle)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker && IsColossusSmash && ColossusToggle)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    if (API.CanCast(Overpower) && PlayerLevel >= 12 && API.SpellCharges(Overpower) == 2)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && API.TargetDebuffRemainingTime(DeepWounds) < 150 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 40 && TalentSkullsplitter)
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    if (API.CanCast(Overpower) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && API.PlayerBuffStacks(Exploiter) == 2 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && (API.PlayerHasBuff(SweepingStrikes) || !TalentCleave && API.TargetDebuffRemainingTime(DeepWounds) < 150))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(Execute) && PlayerLevel >= 10 && PlayerCovenantSettings != "Venthyr" && (API.PlayerHasBuff(DeadlyCalm) || API.PlayerRage >= 20 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    if (API.CanCast(Condemn, true, false) && PlayerCovenantSettings == "Venthyr" && (API.PlayerHasBuff(DeadlyCalm) || API.PlayerRage >= 20 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(Condemn);
                        return;
                    }
                    if (API.CanCast(Ravager) && TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && API.PlayerRage < 80 && IsRavager && BladestormToggle)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !API.PlayerHasBuff(DeadlyCalm) && !API.PlayerHasBuff(SweepingStrikes) && !TalentRavager && API.PlayerRage < 80 && IsBladestorm && BladestormToggle)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                }
                if ((API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE) && (!IsExecute))
                {
                    if (API.CanCast(Avatar) && TalentAvatar && !API.PlayerHasBuff(Avatar) && (API.SpellCDDuration(ColossusSmash) < 100 && !TalentWarbreaker || API.SpellCDDuration(Warbreaker) < 100 && TalentWarbreaker) && IsAvatar)
                    {
                        API.CastSpell(Avatar);
                        return;
                    }
                    if (API.CanCast(Rend) && API.TargetDebuffRemainingTime(Rend) <= 360 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && TalentRend)
                    {
                        API.CastSpell(Rend);
                        return;
                    }
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker && IsColossusSmash && ColossusToggle)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker && IsWarbreaker && ColossusToggle)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && !API.PlayerHasBuff(SweepingStrikes) && API.TargetHasDebuff(ColossusSmash) && PlayerCovenantSettings != "Venthyr" && IsBladestorm && BladestormToggle)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    if (API.CanCast(Ravager) && TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && API.TargetHasDebuff(ColossusSmash) && PlayerCovenantSettings != "Venthyr" && IsRavager && BladestormToggle)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    if (API.CanCast(Overpower) && PlayerLevel >= 12 && API.SpellCharges(Overpower) == 2)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && API.PlayerRage >= 30 && (API.PlayerBuffStacks(Overpower) >= 2 && !API.PlayerHasBuff(DeadlyCalm) || API.TargetDebuffRemainingTime(DeepWounds) < 150))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(DeadlyCalm) && TalentDeadlyCalm && IsDeadlyCalm)
                    {
                        API.CastSpell(DeadlyCalm);
                        return;
                    }
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 60 && TalentSkullsplitter && !API.PlayerHasBuff(DeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    if (API.CanCast(Overpower) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && API.TargetDebuffRemainingTime(DeepWounds) < 200 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(Condemn, true, false) && PlayerCovenantSettings == "Venthyr" && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(Condemn);
                        return;
                    }
                    if (API.CanCast(Execute) && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 10 && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && !API.PlayerHasBuff(SweepingStrikes) && API.TargetHasDebuff(ColossusSmash) && PlayerCovenantSettings == "Venthyr" && IsBladestorm && BladestormToggle)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    if (API.CanCast(Ravager) && TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && API.TargetHasDebuff(ColossusSmash) && PlayerCovenantSettings == "Venthyr" && IsRavager && BladestormToggle)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && TalentFevorofBattle && API.PlayerRage > 60)
                    {
                        API.CastSpell(Whirlwind);
                        return;
                    }
                    if (API.CanCast(Slam) && !TalentFevorofBattle && API.PlayerRage > 50)
                    {
                        API.CastSpell(Slam);
                        return;
                    }
                }
                if ((API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE) && (!IsExecute))
                {
                    if (API.CanCast(SweepingStrikes) && PlayerLevel >= 22 && !API.PlayerHasBuff(SweepingStrikes) && (API.SpellCDDuration(Bladestorm) > 1200 && IsBladestorm && !TalentRavager || !IsBladestorm || TalentRavager))
                    {
                        API.CastSpell(SweepingStrikes);
                        return;
                    }
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 60 && TalentSkullsplitter && !API.PlayerHasBuff(DeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    if (API.CanCast(Avatar) && TalentAvatar && !API.PlayerHasBuff(Avatar) && (API.SpellCDDuration(ColossusSmash) < 100 && !TalentWarbreaker || API.SpellCDDuration(Warbreaker) < 100 && TalentWarbreaker) && IsAvatar)
                    {
                        API.CastSpell(Avatar);
                        return;
                    }
                    if (API.CanCast(Cleave) && TalentCleave && API.TargetDebuffRemainingTime(DeepWounds) < 150 && (API.PlayerRage >= 20 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Cleave);
                        return;
                    }
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker && IsWarbreaker && ColossusToggle)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && !API.PlayerHasBuff(SweepingStrikes) && IsBladestorm && BladestormToggle)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    if (API.CanCast(Ravager) && TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && IsRavager && BladestormToggle)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker && IsColossusSmash && ColossusToggle)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    if (API.CanCast(Rend) && API.TargetDebuffRemainingTime(Rend) <= 360 && API.PlayerHasBuff(SweepingStrikes) && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && TalentRend)
                    {
                        API.CastSpell(Rend);
                        return;
                    }
                    if (API.CanCast(Cleave) && TalentCleave && (API.PlayerRage >= 20 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Cleave);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && (API.PlayerHasBuff(SweepingStrikes) || !TalentCleave && API.TargetDebuffRemainingTime(DeepWounds) < 150))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(Overpower) && PlayerLevel >= 12 && TalentDreadnaught)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    if (API.CanCast(Condemn, true, false) && PlayerCovenantSettings == "Venthyr" && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(Condemn);
                        return;
                    }
                    if (API.CanCast(Execute) && PlayerCovenantSettings != "Venthyr" && API.PlayerHasBuff(SweepingStrikes) && PlayerLevel >= 10 && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    if (API.CanCast(Overpower) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Whirlwind);
                        return;
                    }
                }
            }
        }
    }
}
