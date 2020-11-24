//Changelog test@test.de
// v1.0 First release
// v1.1 Victory Rush fix

namespace HyperElk.Core
{
    public class ArmsWarrior : CombatRoutine
    {
        //Speel,Auras
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
        private string StoneHeart = "Stone Heart";
        private string Victorious = "Victorious";

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

        //CBProperties
        private int VictoryRushLifePercent => percentListProp[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        private int DiebytheSwordLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DiebytheSword)];
        private int DefensiveStanceLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DefensiveStance)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Arms Warrior by smartie";
            API.WriteLog("Welcome to smartie`s Arms Warrior v1.1");
            API.WriteLog("All Talents are supported and auto detected");

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
            CombatRoutine.AddSpell(SweepingStrikes, "None");
            CombatRoutine.AddSpell(Skullsplitter, "D8");
            CombatRoutine.AddSpell(Overpower, "NumPad3");
            CombatRoutine.AddSpell(DeadlyCalm, "NumPad5");
            CombatRoutine.AddSpell(DefensiveStance, "F6");
            CombatRoutine.AddSpell(BattleShout, "None");
            CombatRoutine.AddSpell(Avatar, "None");
            CombatRoutine.AddSpell(StormBolt, "F7");
            //Buffs
            CombatRoutine.AddBuff(BattleShout);
            CombatRoutine.AddBuff(DeadlyCalm);
            CombatRoutine.AddBuff(Overpower);
            CombatRoutine.AddBuff(SuddenDeath);
            CombatRoutine.AddBuff(SweepingStrikes);
            CombatRoutine.AddBuff(VictoryRush);
            CombatRoutine.AddBuff(StoneHeart);
            CombatRoutine.AddBuff(Victorious);
            //Debuff
            CombatRoutine.AddDebuff(ColossusSmash);
            CombatRoutine.AddDebuff(DeepWounds);
            CombatRoutine.AddDebuff(Rend);
            //Prop
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", percentListProp, "Life percent at which" + VictoryRush + "is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", percentListProp, "Life percent at which" + ImpendingVictory + "is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(DiebytheSword, DiebytheSword + " Life Percent", percentListProp, "Life percent at which" + DiebytheSword + "is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(DefensiveStance, DefensiveStance + " Life Percent", percentListProp, "Life percent at which" + DefensiveStance + "is used, set to 0 to disable", "Defense", 8);
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
            if (IsMelee)
            {
                if (API.CanCast(Avatar) && TalentAvatar && (API.SpellCDDuration(ColossusSmash) < 800 && !TalentWarbreaker || API.SpellCDDuration(Warbreaker) < 800 && TalentWarbreaker) && IsCooldowns)
                {
                    API.CastSpell(Avatar);
                    return;
                }
                if (TalentMassacre && API.TargetHealthPercent < 35 || !TalentMassacre && API.TargetHealthPercent < 20)
                {
                    if (API.CanCast(DeadlyCalm) && TalentDeadlyCalm && IsCooldowns)
                    {
                        API.CastSpell(DeadlyCalm);
                        return;
                    }
                    if (API.CanCast(SweepingStrikes) && PlayerLevel >= 22 && !API.PlayerHasBuff(SweepingStrikes) && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                    {
                        API.CastSpell(SweepingStrikes);
                        return;
                    }
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 20 && TalentSkullsplitter && !API.PlayerHasBuff(DeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    if (API.CanCast(Ravager) && TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && (API.SpellCDDuration(ColossusSmash) < 200 && !TalentWarbreaker || TalentWarbreaker && API.SpellCDDuration(Warbreaker) < 200) && IsCooldowns)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && API.TargetDebuffRemainingTime(DeepWounds) < 100 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(Cleave) && TalentCleave && API.PlayerRage >= 20 && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE))
                    {
                        API.CastSpell(Cleave);
                        return;
                    }
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !TalentRavager && API.PlayerRage < 30 && !API.PlayerHasBuff(DeadlyCalm) && IsCooldowns)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    if (API.CanCast(Overpower) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    if (API.CanCast(Execute) && PlayerLevel >= 10 && (API.PlayerHasBuff(DeadlyCalm) || API.PlayerRage >= 20 || API.PlayerHasBuff(SuddenDeath) || API.PlayerHasBuff(StoneHeart)))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                }
                if ((API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE) && (TalentMassacre && API.TargetHealthPercent >= 35 || !TalentMassacre && API.TargetHealthPercent >= 20))
                {
                    if (API.CanCast(Rend) && API.TargetDebuffRemainingTime(Rend) <= 360 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && TalentRend)
                    {
                        API.CastSpell(Rend);
                        return;
                    }
                    if (API.CanCast(DeadlyCalm) && TalentDeadlyCalm && IsCooldowns)
                    {
                        API.CastSpell(DeadlyCalm);
                        return;
                    }
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 20 && TalentSkullsplitter && (API.SpellCDDuration(DeadlyCalm) > 300 || !TalentDeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    if (API.CanCast(Ravager) && TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && (API.SpellCDDuration(ColossusSmash) < 200 && !TalentWarbreaker || TalentWarbreaker && API.SpellCDDuration(Warbreaker) < 200) && IsCooldowns)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && API.TargetDebuffRemainingTime(DeepWounds) < 200 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    if (API.CanCast(Execute) && PlayerLevel >= 10 && (API.PlayerHasBuff(SuddenDeath) || API.PlayerHasBuff(StoneHeart)))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !TalentRavager && API.SpellCDDuration(MortalStrike) > 50 && IsCooldowns)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && TalentFevorofBattle && API.TargetHasDebuff(ColossusSmash) && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Whirlwind);
                        return;
                    }
                    if (API.CanCast(Slam) && !TalentFevorofBattle && (API.PlayerRage >= 40 || API.PlayerRage >= 20 && API.TargetHasDebuff(ColossusSmash) || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Slam);
                        return;
                    }
                    if (API.CanCast(Overpower) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Overpower);
                        return;
                    }
                    if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && TalentFevorofBattle && (API.PlayerRage >= 60 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Whirlwind);
                        return;
                    }
                    if (API.CanCast(Slam) && !TalentFevorofBattle && (API.PlayerRage >= 20 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Slam);
                        return;
                    }
                }
                if ((API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE) && (TalentMassacre && API.TargetHealthPercent >= 35 || !TalentMassacre && API.TargetHealthPercent >= 20))
                {
                    if (API.CanCast(SweepingStrikes) && PlayerLevel >= 22 && !API.PlayerHasBuff(SweepingStrikes))
                    {
                        API.CastSpell(SweepingStrikes);
                        return;
                    }
                    if (API.CanCast(Skullsplitter) && API.PlayerRage < 60 && TalentSkullsplitter && (API.SpellCDDuration(DeadlyCalm) > 300 || !TalentDeadlyCalm))
                    {
                        API.CastSpell(Skullsplitter);
                        return;
                    }
                    if (API.CanCast(Ravager) && TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && (API.SpellCDDuration(ColossusSmash) < 200 && !TalentWarbreaker || TalentWarbreaker && API.SpellCDDuration(Warbreaker) < 200) && IsCooldowns)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    if (API.CanCast(ColossusSmash) && PlayerLevel >= 19 && !API.TargetHasDebuff(ColossusSmash) && !TalentWarbreaker)
                    {
                        API.CastSpell(ColossusSmash);
                        return;
                    }
                    if (API.CanCast(Warbreaker) && !API.TargetHasDebuff(ColossusSmash) && TalentWarbreaker)
                    {
                        API.CastSpell(Warbreaker);
                        return;
                    }
                    if (API.CanCast(Bladestorm) && PlayerLevel >= 38 && !TalentRavager && !API.PlayerHasBuff(DeadlyCalm) && IsCooldowns)
                    {
                        API.CastSpell(Bladestorm);
                        return;
                    }
                    if (API.CanCast(DeadlyCalm) && TalentDeadlyCalm && IsCooldowns)
                    {
                        API.CastSpell(DeadlyCalm);
                        return;
                    }
                    if (API.CanCast(Cleave) && TalentCleave && (API.PlayerRage >= 20 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Cleave);
                        return;
                    }
                    if (API.CanCast(Execute) && PlayerLevel >= 10 && (API.PlayerHasBuff(SuddenDeath) || API.PlayerHasBuff(StoneHeart)) && (API.PlayerHasBuff(SweepingStrikes) || API.SpellCDDuration(SweepingStrikes) > 800))
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    if (API.CanCast(MortalStrike) && PlayerLevel >= 10 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)) && ((!TalentCleave && API.TargetDebuffRemainingTime(DeepWounds) < 200) || API.PlayerHasBuff(SweepingStrikes) && API.PlayerBuffStacks(Overpower) == 2 && TalentDreadnaught))
                    {
                        API.CastSpell(MortalStrike);
                        return;
                    }
                    if (API.CanCast(Whirlwind) && PlayerLevel >= 9 && (API.PlayerRage >= 30 || API.PlayerHasBuff(DeadlyCalm)))
                    {
                        API.CastSpell(Whirlwind);
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
