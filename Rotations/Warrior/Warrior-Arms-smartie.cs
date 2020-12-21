//Changelog
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
// v2.0 new apl
// v2.1 small adjustments
// v2.2 ravager fix
// v2.25 small apl change
// v2.3 another ravager fix
// v2.4 spell ids and alot of other stuff

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
        private string MassacreExecute = "Massacre Execute";
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
        private string BerserkerRage = "Berserker Rage";
        private string Avatar = "Avatar";
        private string StormBolt = "Storm Bolt";
        private string SuddenDeath = "Sudden Death";
        private string DeepWounds = "Deep Wounds";
        private string Victorious = "Victorious";
        private string Condemn = "Condemn";
        private string MassacreCondemn = "Massacre Condemn";
        private string SpearofBastion = "Spear of Bastion";
        private string AncientAftershock = "Ancient Aftershock";
        private string ConquerorsBanner = "Conqueror's Banner";
        private string HeroicThrow = "Heroic Throw";
        private string Exploiter = "Exploiter";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";

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
        private float gcd => API.SpellGCDTotalDuration;

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
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseAvatar => CDUsage[CombatRoutine.GetPropertyInt(Avatar)];
        private string UseRavager => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Ravager)];
        private string UseBladestorm => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Bladestorm)];
        private string UseColossusSmash => CDUsage[CombatRoutine.GetPropertyInt(ColossusSmash)];
        private string UseWarbreaker => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Warbreaker)];
        private string UseDeadlyCalm => CDUsage[CombatRoutine.GetPropertyInt(DeadlyCalm)];
        private int VictoryRushLifePercent => numbList[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => numbList[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        private int DiebytheSwordLifePercent => numbList[CombatRoutine.GetPropertyInt(DiebytheSword)];
        private int DefensiveStanceLifePercent => numbList[CombatRoutine.GetPropertyInt(DefensiveStance)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Arms Warrior by smartie";
            API.WriteLog("Welcome to smartie`s Arms Warrior v2.4");
            API.WriteLog("The Bladestorm toggle will also toggle Ravager");
            API.WriteLog("The Colossus Smash toggle will also toggle Warbreaker");

            //Spells
            CombatRoutine.AddSpell(MortalStrike, 12294, "D4");
            CombatRoutine.AddSpell(ColossusSmash, 167105, "D3");
            CombatRoutine.AddSpell(Warbreaker, 262161, "D3");
            CombatRoutine.AddSpell(Slam, 1464, "F12");
            CombatRoutine.AddSpell(Execute,163201, "D6");
            CombatRoutine.AddSpell(MassacreExecute, 281000, "D7");
            CombatRoutine.AddSpell(Cleave, 845, "NumPad4");
            CombatRoutine.AddSpell(Whirlwind, 1680, "D7");
            CombatRoutine.AddSpell(Bladestorm, 227847, "D5");
            CombatRoutine.AddSpell(Ravager, 152277, "D5");
            CombatRoutine.AddSpell(Rend, 772, "D8");
            CombatRoutine.AddSpell(VictoryRush, 34428, "F3");
            CombatRoutine.AddSpell(ImpendingVictory, 202168, "F3");
            CombatRoutine.AddSpell(DiebytheSword, 118038, "F2");
            CombatRoutine.AddSpell(Pummel, 6552, "F11");
            CombatRoutine.AddSpell(BerserkerRage, 18499, "F1");
            CombatRoutine.AddSpell(HeroicThrow, 57755, "F6");
            CombatRoutine.AddSpell(SweepingStrikes, 260708, "None");
            CombatRoutine.AddSpell(Skullsplitter, 260643, "D8");
            CombatRoutine.AddSpell(Overpower, 7384, "NumPad3");
            CombatRoutine.AddSpell(DeadlyCalm, 262228, "NumPad5");
            CombatRoutine.AddSpell(DefensiveStance, 197690, "F6");
            CombatRoutine.AddSpell(BattleShout, 6673, "None");
            CombatRoutine.AddSpell(Avatar, 107574, "None");
            CombatRoutine.AddSpell(StormBolt, 107570, "F7");
            CombatRoutine.AddSpell(Condemn, 317349, "D6");
            CombatRoutine.AddSpell(MassacreCondemn, 330334, "D6");
            CombatRoutine.AddSpell(ConquerorsBanner, 324143, "D1");
            CombatRoutine.AddSpell(AncientAftershock, 325886, "D1");
            CombatRoutine.AddSpell(SpearofBastion, 307865, "D1");

            //Macros
            CombatRoutine.AddMacro(HeroicThrow + "MO", "D2");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

            //Buffs
            CombatRoutine.AddBuff(BattleShout, 6673);
            CombatRoutine.AddBuff(DeadlyCalm, 262228);
            CombatRoutine.AddBuff(Overpower, 7384);
            CombatRoutine.AddBuff(SuddenDeath, 52437);
            CombatRoutine.AddBuff(SweepingStrikes, 260708);
            CombatRoutine.AddBuff(Victorious, 32216);
            CombatRoutine.AddBuff(Avatar, 107574);
            CombatRoutine.AddBuff(Exploiter, 335451);

            //Debuff
            CombatRoutine.AddDebuff(ColossusSmash, 208086);
            CombatRoutine.AddDebuff(DeepWounds, 262115);
            CombatRoutine.AddDebuff(Rend, 772);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Bladestorm");
            CombatRoutine.AddToggle("Colossus Smash");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

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
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", numbList, "Life percent at which" + VictoryRush + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", numbList, "Life percent at which" + ImpendingVictory + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(DiebytheSword, DiebytheSword + " Life Percent", numbList, "Life percent at which" + DiebytheSword + " is used, set to 0 to disable", "Defense", 35);
            CombatRoutine.AddProp(DefensiveStance, DefensiveStance + " Life Percent", numbList, "Life percent at which" + DefensiveStance + " is used, set to 0 to disable", "Defense", 20);
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
            if (API.PlayerCurrentCastTimeRemaining > 40)
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
            if (API.CanCast(RacialSpell1) && isInterrupt && PlayerRaceSettings == "Tauren" && !API.PlayerIsMoving && isRacial && IsMelee && API.SpellISOnCooldown(Pummel))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.PlayerItemCanUse("Healthstone") && API.PlayerItemRemainingCD("Healthstone") == 0 && API.PlayerHealthPercent <= HealthStonePercent)
            {
                API.CastSpell("Healthstone");
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
            if (API.CanCast(DiebytheSword) && PlayerLevel >= 23 && API.PlayerHealthPercent <= DiebytheSwordLifePercent)
            {
                API.CastSpell(DiebytheSword);
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
                //actions+=/sweeping_strikes,if=spell_targets.whirlwind>1&(cooldown.bladestorm.remains>15|talent.ravager.enabled)
                if (API.CanCast(SweepingStrikes) && PlayerLevel >= 22 && !API.PlayerHasBuff(SweepingStrikes) && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE && (API.SpellCDDuration(Bladestorm) > 1500 && IsBladestorm && BladestormToggle && !TalentRavager || !BladestormToggle || !IsBladestorm || TalentRavager)))
                {
                    API.CastSpell(SweepingStrikes);
                    return;
                }
                if (IsExecute)
                {
                    //actions.execute=deadly_calm
                    if (API.CanCast(DeadlyCalm) && TalentDeadlyCalm && IsDeadlyCalm)
                    {
                        API.CastSpell(DeadlyCalm);
                        return;
                    }
                    //actions.execute+=/rend,if=remains<=duration*0.3
                    if (API.CanCast(Rend) && API.TargetDebuffRemainingTime(Rend) <= 360 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && TalentRend)
                    {
                        API.CastSpell(Rend);
                        return;
                    }
                    //actions.execute+=/skullsplitter,if=rage<60&(!talent.deadly_calm.enabled|buff.deadly_calm.down)
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 60 && TalentSkullsplitter && !API.PlayerHasBuff(DeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    //actions.execute+=/avatar,if=cooldown.colossus_smash.remains<8&gcd.remains=0
                    if (API.CanCast(Avatar) && TalentAvatar && !API.PlayerHasBuff(Avatar) && (API.SpellCDDuration(ColossusSmash) < 800 && !TalentWarbreaker || API.SpellCDDuration(Warbreaker) < 800 && TalentWarbreaker) && IsAvatar)
                    {
                        API.CastSpell(Avatar);
                        return;
                    }
                    //actions.execute+=/ravager,if=buff.avatar.remains<18&!dot.ravager.remains
                    if (API.CanCast(Ravager) && TalentRavager && IsRavager && BladestormToggle && (API.PlayerBuffTimeRemaining(Avatar) < 1800 || !TalentAvatar))
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    //actions.execute+=/cleave,if=spell_targets.whirlwind>1&dot.deep_wounds.remains<gcd
                    if (API.CanCast(Cleave) && TalentCleave && API.PlayerRage >= 20 && API.TargetDebuffRemainingTime(DeepWounds) < gcd && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                    {
                        API.CastSpell(Cleave);
                        return;
                    }
                    //actions.execute+=/warbreaker
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker && IsWarbreaker && ColossusToggle)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    //actions.execute+=/colossus_smash
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker && IsColossusSmash && ColossusToggle)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    //actions.execute+=/condemn,if=debuff.colossus_smash.up|buff.sudden_death.react|rage>65
                    if (API.CanCast(Condemn, true, false) && PlayerCovenantSettings == "Venthyr" && !TalentMassacre && (API.TargetHasDebuff(ColossusSmash) && API.PlayerRage > 20 || API.PlayerHasBuff(DeadlyCalm) || API.PlayerRage >= 65 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(Condemn);
                        return;
                    }
                    //actions.execute+=/condemn,if=debuff.colossus_smash.up|buff.sudden_death.react|rage>65
                    if (API.CanCast(MassacreCondemn, true, false) && PlayerCovenantSettings == "Venthyr" && TalentMassacre && (API.TargetHasDebuff(ColossusSmash) && API.PlayerRage > 20 || API.PlayerHasBuff(DeadlyCalm) || API.PlayerRage >= 65 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(MassacreCondemn);
                        return;
                    }
                    //actions.execute+=/overpower,if=charges=2
                    if (API.CanCast(Overpower) && PlayerLevel >= 12 && API.SpellCharges(Overpower) == 2)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    //actions.execute+=/bladestorm,if=buff.deadly_calm.down&rage<50
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !API.PlayerHasBuff(DeadlyCalm) && !API.PlayerHasBuff(SweepingStrikes) && !TalentRavager && API.PlayerRage < 50 && IsBladestorm && BladestormToggle)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    //actions.execute+=/mortal_strike,if=dot.deep_wounds.remains<=gcd
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && API.TargetDebuffRemainingTime(DeepWounds) < gcd && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    //actions.execute+=/skullsplitter,if=rage<40
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 40 && TalentSkullsplitter)
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    //actions.execute+=/overpower
                    if (API.CanCast(Overpower) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    //actions.execute+=/execute
                    if (API.CanCast(Execute) && PlayerLevel >= 10 && PlayerCovenantSettings != "Venthyr" && !TalentMassacre && (API.PlayerHasBuff(DeadlyCalm) || API.PlayerRage >= 20 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    //actions.execute+=/execute
                    if (API.CanCast(MassacreExecute) && PlayerLevel >= 10 && PlayerCovenantSettings != "Venthyr" && TalentMassacre && (API.PlayerHasBuff(DeadlyCalm) || API.PlayerRage >= 20 || API.PlayerHasBuff(SuddenDeath)))
                    {
                        API.CastSpell(MassacreExecute);
                        return;
                    }
                    //actions.execute+=/condemn
                    if (API.CanCast(Condemn, true, false) && !TalentMassacre && PlayerCovenantSettings == "Venthyr" && API.PlayerRage >= 20)
                    {
                        API.CastSpell(Condemn);
                        return;
                    }
                    if (API.CanCast(MassacreCondemn, true, false) && TalentMassacre && PlayerCovenantSettings == "Venthyr" && API.PlayerRage >= 20)
                    {
                        API.CastSpell(MassacreCondemn);
                        return;
                    }
                }
                if ((API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE) && !IsExecute)
                {
                    //actions.single_target=avatar,if=cooldown.colossus_smash.remains<8&gcd.remains=0
                    if (API.CanCast(Avatar) && TalentAvatar && !API.PlayerHasBuff(Avatar) && (API.SpellCDDuration(ColossusSmash) < 800 && !TalentWarbreaker || API.SpellCDDuration(Warbreaker) < 800 && TalentWarbreaker) && IsAvatar)
                    {
                        API.CastSpell(Avatar);
                        return;
                    }
                    //actions.single_target=avatar,if=cooldown.colossus_smash.remains<8&gcd.remains=0
                    if (API.CanCast(Rend) && API.TargetDebuffRemainingTime(Rend) <= 360 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && TalentRend)
                    {
                        API.CastSpell(Rend);
                        return;
                    }
                    //actions.single_target+=/warbreaker
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker && IsWarbreaker && ColossusToggle)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    //actions.single_target+=/colossus_smash
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker && IsColossusSmash && ColossusToggle)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    //actions.single_target+=/ravager,if=buff.avatar.remains<18&!dot.ravager.remains
                    if (API.CanCast(Ravager) && TalentRavager && IsRavager && BladestormToggle && (API.PlayerBuffTimeRemaining(Avatar) < 1800 || !TalentAvatar))
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    //actions.single_target+=/overpower,if=charges=2
                    if (API.CanCast(Overpower) && PlayerLevel >= 12 && API.SpellCharges(Overpower) == 2)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    //actions.single_target+=/bladestorm,if=buff.deadly_calm.down&(debuff.colossus_smash.up&rage<30|rage<70)
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && !API.PlayerHasBuff(SweepingStrikes) && IsBladestorm && BladestormToggle && (!API.TargetHasDebuff(ColossusSmash) && API.PlayerRage < 70 || API.TargetHasDebuff(ColossusSmash) && API.PlayerRage < 30))
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    //actions.single_target+=/mortal_strike,if=buff.overpower.stack>=2&buff.deadly_calm.down|(dot.deep_wounds.remains<=gcd&cooldown.colossus_smash.remains>gcd)
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && API.PlayerRage >= 30 && (API.PlayerBuffStacks(Overpower) >= 2 && !API.PlayerHasBuff(DeadlyCalm) || (API.TargetDebuffRemainingTime(DeepWounds) < gcd && (API.SpellCDDuration(ColossusSmash) > gcd && !TalentWarbreaker || TalentWarbreaker && API.SpellCDDuration(Warbreaker) > gcd))))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    //actions.single_target+=/deadly_calm
                    if (API.CanCast(DeadlyCalm) && TalentDeadlyCalm && IsDeadlyCalm)
                    {
                        API.CastSpell(DeadlyCalm);
                        return;
                    }
                    //actions.single_target+=/skullsplitter,if=rage<60&buff.deadly_calm.down
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 60 && TalentSkullsplitter && !API.PlayerHasBuff(DeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    //actions.single_target+=/overpower
                    if (API.CanCast(Overpower) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    //actions.single_target+=/condemn,if=buff.sudden_death.react
                    if (API.CanCast(Condemn, true, false) && !TalentMassacre && PlayerCovenantSettings == "Venthyr" && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(Condemn);
                        return;
                    }
                    //actions.single_target+=/condemn,if=buff.sudden_death.react
                    if (API.CanCast(MassacreCondemn, true, false) && TalentMassacre && PlayerCovenantSettings == "Venthyr" && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(MassacreCondemn);
                        return;
                    }
                    //actions.single_target+=/execute,if=buff.sudden_death.react
                    if (API.CanCast(Execute) && !TalentMassacre && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 10 && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    //actions.single_target+=/execute,if=buff.sudden_death.react
                    if (API.CanCast(MassacreExecute) && TalentMassacre && PlayerCovenantSettings != "Venthyr" && PlayerLevel >= 10 && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(MassacreExecute);
                        return;
                    }
                    //actions.single_target+=/mortal_strike
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    //actions.single_target+=/whirlwind,if=talent.fervor_of_battle.enabled&rage>60
                    if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && TalentFevorofBattle && API.PlayerRage > 60)
                    {
                        API.CastSpell(Whirlwind);
                        return;
                    }
                    //actions.single_target+=/slam
                    if (API.CanCast(Slam) && !TalentFevorofBattle && API.PlayerRage > 20)
                    {
                        API.CastSpell(Slam);
                        return;
                    }
                }
                if ((API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE) && !IsExecute)
                {
                    //actions.hac=skullsplitter,if=rage<60&buff.deadly_calm.down
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 60 && TalentSkullsplitter && !API.PlayerHasBuff(DeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    //actions.hac+=/avatar,if=cooldown.colossus_smash.remains<1
                    if (API.CanCast(Avatar) && TalentAvatar && !API.PlayerHasBuff(Avatar) && (API.SpellCDDuration(ColossusSmash) < 100 && !TalentWarbreaker || API.SpellCDDuration(Warbreaker) < 100 && TalentWarbreaker) && IsAvatar)
                    {
                        API.CastSpell(Avatar);
                        return;
                    }
                    //actions.hac+=/cleave,if=dot.deep_wounds.remains<=gcd
                    if (API.CanCast(Cleave) && TalentCleave && API.TargetDebuffRemainingTime(DeepWounds) < gcd && (API.PlayerRage >= 20 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Cleave);
                        return;
                    }
                    //actions.hac+=/warbreaker
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker && IsWarbreaker && ColossusToggle)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    //actions.hac+=/bladestorm
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && !API.PlayerHasBuff(SweepingStrikes) && IsBladestorm && BladestormToggle)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    //actions.hac+=/ravager
                    if (API.CanCast(Ravager) && TalentRavager && IsRavager && BladestormToggle)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    //actions.hac+=/colossus_smash
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker && IsColossusSmash && ColossusToggle)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    //actions.hac+=/rend,if=remains<=duration*0.3&buff.sweeping_strikes.up
                    if (API.CanCast(Rend) && API.TargetDebuffRemainingTime(Rend) <= 360 && API.PlayerHasBuff(SweepingStrikes) && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && TalentRend)
                    {
                        API.CastSpell(Rend);
                        return;
                    }
                    //actions.hac+=/cleave
                    if (API.CanCast(Cleave) && TalentCleave && (API.PlayerRage >= 20 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Cleave);
                        return;
                    }
                    //actions.hac+=/mortal_strike,if=buff.sweeping_strikes.up|dot.deep_wounds.remains<gcd&!talent.cleave.enabled
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && (API.PlayerHasBuff(SweepingStrikes) || !TalentCleave && API.TargetDebuffRemainingTime(DeepWounds) < gcd))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    //actions.hac+=/overpower,if=talent.dreadnaught.enabled
                    if (API.CanCast(Overpower) && PlayerLevel >= 12 && TalentDreadnaught)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    //actions.hac+=/condemn
                    if (API.CanCast(Condemn, true, false) && !TalentMassacre && PlayerCovenantSettings == "Venthyr" && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(Condemn);
                        return;
                    }
                    //actions.hac+=/condemn
                    if (API.CanCast(MassacreCondemn, true, false) && TalentMassacre && PlayerCovenantSettings == "Venthyr" && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(MassacreCondemn);
                        return;
                    }
                    //actions.hac+=/execute,if=buff.sweeping_strikes.up
                    if (API.CanCast(Execute) && !TalentMassacre && PlayerCovenantSettings != "Venthyr" && API.PlayerHasBuff(SweepingStrikes) && PlayerLevel >= 10 && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    //actions.hac+=/execute,if=buff.sweeping_strikes.up
                    if (API.CanCast(MassacreExecute) && TalentMassacre && PlayerCovenantSettings != "Venthyr" && API.PlayerHasBuff(SweepingStrikes) && PlayerLevel >= 10 && API.PlayerHasBuff(SuddenDeath))
                    {
                        API.CastSpell(MassacreExecute);
                        return;
                    }
                    //actions.hac+=/overpower
                    if (API.CanCast(Overpower) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    //actions.hac+=/whirlwind
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
