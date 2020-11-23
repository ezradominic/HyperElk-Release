// Changelog
// v1.0 First release
// v1.1 Victory Rush fix
// v1.2 covenant support beta

namespace HyperElk.Core
{
    public class FuryWarrior : CombatRoutine
    {
        //Speel,Auras
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
        private string Condemn = "Condemn";
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

        //CBProperties
        private bool IsLineUp => CombatRoutine.GetPropertyBool("LineUp");
        private int EnragedRegenerationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(EnragedRegeneration)];
        private int VictoryRushLifePercent => percentListProp[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        string[] RecklessnessList = new string[] { "always", "with Cooldowns" };
        string[] SiegebreakerhList = new string[] { "always", "with Cooldowns" };
        string[] CovenantList = new string[] { "None", "Venthyr", "Night Fae", "Kyrian", "Necrolord" };
        private string UseRecklessness => RecklessnessList[CombatRoutine.GetPropertyInt(Recklessness)];
        private string UseSiegebreaker => SiegebreakerhList[CombatRoutine.GetPropertyInt(Siegebreaker)];
        private string Covenant => CovenantList[CombatRoutine.GetPropertyInt("Covenant")];

        public override void Initialize()
        {
            CombatRoutine.Name = "Fury Warrior v1.2 by smartie";
            API.WriteLog("Welcome to smartie`s Fury Warrior v1.2");
            API.WriteLog("All Talents are supported and auto detected");

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
            CombatRoutine.AddSpell(Condemn, "D1");
            CombatRoutine.AddSpell(ConquerorsBanner, "D1");
            CombatRoutine.AddSpell(AncientAftershock, "D1");
            CombatRoutine.AddSpell(SpearofBastion, "D1");
            CombatRoutine.AddSpell(Slam, "None");

            //Buffs
            CombatRoutine.AddBuff(Enrage);
            CombatRoutine.AddBuff(Whirlwind);
            CombatRoutine.AddBuff(Recklessness);
            CombatRoutine.AddBuff(DragonRoar);
            CombatRoutine.AddBuff(SuddenDeath);
            CombatRoutine.AddBuff(VictoryRush);
            CombatRoutine.AddBuff(BattleShout);
            CombatRoutine.AddBuff(Victorious);

            //Debuff
            CombatRoutine.AddDebuff(Siegebreaker);

            //Prop
            CombatRoutine.AddProp("LineUp", "LineUp CDS", true, "Lineup Recklessness and Siegebreaker", "Generic");
            CombatRoutine.AddProp("Covenant", "Covenant", CovenantList, "Choose your Covenant: None, Venthyr, Night Fae, Kyrian, Necrolord", "Generic", 0);
            CombatRoutine.AddProp(Recklessness, "Use " + Recklessness, RecklessnessList, "Use " + Recklessness + "always, with Cooldowns", "Cooldowns", 2);
            CombatRoutine.AddProp(Siegebreaker, "Use " + Siegebreaker, SiegebreakerhList, "Use " + Siegebreaker+ "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(EnragedRegeneration, EnragedRegeneration + " Life Percent", percentListProp, "Life percent at which" + EnragedRegeneration + "is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", percentListProp, "Life percent at which" + VictoryRush + "is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", percentListProp, "Life percent at which" + ImpendingVictory + "is used, set to 0 to disable", "Defense", 8);

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
            if (isInterrupt && API.CanCast(Pummel) && PlayerLevel >= 7)
            {
                API.CastSpell(Pummel);
                return;
            }
            if (API.PlayerHealthPercent <= EnragedRegenerationLifePercent && PlayerLevel >= 23 && API.CanCast(EnragedRegeneration))
            {
                API.CastSpell(EnragedRegeneration);
                return;
            }
            if (API.PlayerHealthPercent <= VictoryRushLifePercent && PlayerLevel >= 5 && API.CanCast(VictoryRush) && API.PlayerHasBuff(Victorious))
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
            if (IsMelee)
            {
                if (API.CanCast(ConquerorsBanner) && Covenant == "Necrolord" && !API.PlayerIsMoving && IsCooldowns)
                {
                    API.CastSpell(ConquerorsBanner);
                    return;
                }
                if (API.CanCast(SpearofBastion) && Covenant == "Kyrian" && !API.PlayerIsMoving && IsCooldowns)
                {
                    API.CastSpell(SpearofBastion);
                    return;
                }
                if (API.CanCast(AncientAftershock) && Covenant == "Night Fae" && !API.PlayerIsMoving && IsCooldowns)
                {
                    API.CastSpell(AncientAftershock);
                    return;
                }
                if (API.CanCast(Recklessness) && PlayerLevel >= 38 && (IsCooldowns && UseRecklessness == "with Cooldowns" || UseRecklessness == "always" ))
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
                if (API.CanCast(Siegebreaker) && TalentSiegebreaker && IsCooldowns && (IsLineUp && API.SpellCDDuration(Recklessness) > 3000 || !IsLineUp) && (IsCooldowns && UseSiegebreaker== "with Cooldowns" || UseSiegebreaker == "always"))
                {
                    API.CastSpell(Siegebreaker);
                    return;
                }
                if (API.CanCast(Rampage) && PlayerLevel >= 19 && (API.PlayerRage >= 80 && !API.PlayerHasBuff(Enrage) || API.PlayerRage >= 90))
                {
                    API.CastSpell(Rampage);
                    return;
                }
                if (API.CanCast(Bladestorm) && IsCooldowns && API.PlayerLastSpell == Rampage && TalentBladestorm)
                {
                    API.CastSpell(Bladestorm);
                    return;
                }
                if (API.CanCast(Execute) && Covenant != "Venthyr" && PlayerLevel >= 9 && (!TalentMassacre && API.TargetHealthPercent < 20 || TalentMassacre && API.TargetHealthPercent < 35 || API.PlayerHasBuff(SuddenDeath)))
                {
                    API.CastSpell(Execute);
                    return;
                }
                if (API.CanCast(Condemn) && Covenant == "Venthyr" && (!TalentMassacre && API.TargetHealthPercent < 20 && API.TargetHealthPercent > 80 || TalentMassacre && API.TargetHealthPercent < 35 && API.TargetHealthPercent > 80 || API.PlayerHasBuff(SuddenDeath)))
                {
                    API.CastSpell(Condemn);
                    return;
                }
                if (API.CanCast(DragonRoar) && TalentDragonRoar && API.PlayerHasBuff(Enrage) && IsCooldowns)
                {
                    API.CastSpell(DragonRoar);
                    return;
                }
                if (API.CanCast(RagingBlow) && PlayerLevel >= 12 && API.SpellCharges(RagingBlow) == 2)
                {
                    API.CastSpell(RagingBlow);
                    return;
                }
                if (API.CanCast(Bloodthirst) && PlayerLevel >= 10)
                {
                    API.CastSpell(Bloodthirst);
                    return;
                }
                if (API.CanCast(Onslaught) && API.PlayerHasBuff(Enrage) && TalentOnslaught)
                {
                    API.CastSpell(Onslaught);
                    return;
                }
                if (API.CanCast(RagingBlow) && PlayerLevel >= 12)
                {
                    API.CastSpell(RagingBlow);
                    return;
                }
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

