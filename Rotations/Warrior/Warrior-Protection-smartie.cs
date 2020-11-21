// Changelog
// v1.0 First release
// v1.1 shield block and execute fix

namespace HyperElk.Core
{
    public class ProtWarrior : CombatRoutine
    {
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

        //CBProperties
        private int LastStandLifePercent => percentListProp[CombatRoutine.GetPropertyInt(LastStand)];
        private int ShieldWallLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ShieldWall)];
        private int VictoryRushLifePercent => percentListProp[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        private int IgnorePainLifePercent => percentListProp[CombatRoutine.GetPropertyInt(IgnorePain)];
        private int ShieldBlockLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ShieldBlock)];


        public override void Initialize()
        {
            CombatRoutine.Name = "Protection Warrior v1.1 by smartie";
            API.WriteLog("Welcome to smartie`s Protection Warrior v1.1");
            API.WriteLog("All Talents are supported and auto detected");

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

            //Debuff
            CombatRoutine.AddDebuff(DeepWounds);

            //Prop
            CombatRoutine.AddProp(LastStand, LastStand + " Life Percent", percentListProp, "Life percent at which" + LastStand + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(ShieldWall, ShieldWall + " Life Percent", percentListProp, "Life percent at which" + ShieldWall + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", percentListProp, "Life percent at which" + VictoryRush + "is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", percentListProp, "Life percent at which" + ImpendingVictory + "is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(IgnorePain, IgnorePain + " Life Percent", percentListProp, "Life percent at which" + IgnorePain + "is used, set to 0 to disable", "Defense", 9);
            CombatRoutine.AddProp(ShieldBlock, ShieldBlock + " Life Percent", percentListProp, "Life percent at which" + ShieldBlock + "is used, set to 0 to disable", "Defense", 6);

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
            if (API.CanCast(LastStand) && API.PlayerHealthPercent <= LastStandLifePercent && PlayerLevel >= 38)
            {
                API.CastSpell(LastStand);
                return;
            }
            if (API.CanCast(ShieldWall) && API.PlayerHealthPercent <= ShieldWallLifePercent && PlayerLevel >= 23 && !API.PlayerHasBuff(LastStand))
            {
                API.CastSpell(ShieldWall);
                return;
            }
            if (API.CanCast(ShieldBlock) && API.PlayerHealthPercent <= ShieldBlockLifePercent && PlayerLevel >= 6 && !API.PlayerHasBuff(ShieldBlock) && API.PlayerRage >= 30)
            {
                API.CastSpell(ShieldBlock);
                return;
            }
            if (API.CanCast(VictoryRush) && API.PlayerHealthPercent <= VictoryRushLifePercent && API.PlayerHasBuff(VictoryRush) && PlayerLevel >= 5)
            {
                API.CastSpell(VictoryRush);
                return;
            }
            if (API.CanCast(ImpendingVictory) && API.PlayerHealthPercent <= ImpendingVictoryLifePercent && TalentImpendingVictory)
            {
                API.CastSpell(ImpendingVictory);
                return;
            }
            if (API.CanCast(IgnorePain) && (API.PlayerRage >= 40 || API.PlayerRage >= 26 && API.PlayerHasBuff(VengeanceIgnorePain)) && (!API.PlayerHasBuff(IgnorePain) || API.PlayerHasBuff(IgnorePain) && API.PlayerBuffTimeRemaining(IgnorePain) < 200) && API.PlayerHealthPercent <= IgnorePainLifePercent && PlayerLevel >= 17)
            {
                API.CastSpell(IgnorePain);
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
            if (API.CanCast(HeroicThrow) && API.TargetRange >= 8 && API.TargetRange <= 30)
            {
                API.CastSpell(HeroicThrow);
                return;
            }
            if (IsMelee)
            {
                if (API.CanCast(Avatar) && IsCooldowns && PlayerLevel >= 32)
                {
                    API.CastSpell(Avatar);
                    return;
                }
                if (API.CanCast(DemoralizingShout) && TalentBoomingVoice)
                {
                    API.CastSpell(DemoralizingShout);
                    return;
                }
                if (API.CanCast(Ravager) && TalentRavager)
                {
                    API.CastSpell(Ravager);
                    return;
                }
                if (API.CanCast(DragonRoar) && TalentDragonRoar)
                {
                    API.CastSpell(DragonRoar);
                    return;
                }
                if (API.CanCast(Revenge) && API.PlayerRage >= 20 && (API.PlayerHasBuff(IgnorePain) && API.PlayerBuffTimeRemaining(IgnorePain) > 300 || API.PlayerHealthPercent > IgnorePainLifePercent) && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE) && PlayerLevel >= 12)
                {
                    API.CastSpell(Revenge);
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
                if (API.CanCast(Execute) && API.PlayerRage > 20 && API.TargetHealthPercent < 20 && (API.PlayerHasBuff(IgnorePain) && API.PlayerBuffTimeRemaining(IgnorePain) > 300 || API.PlayerHealthPercent > IgnorePainLifePercent) && PlayerLevel >= 10)
                {
                    API.CastSpell(Execute);
                    return;
                }
                if (API.CanCast(Revenge) && API.PlayerHasBuff(FreeRevenge) && PlayerLevel >= 12)
                {
                    API.CastSpell(Revenge);
                    return;
                }
                if (API.CanCast(Revenge) && TalentDevastator && API.PlayerRage >= 20 && (API.PlayerHasBuff(IgnorePain) && API.PlayerBuffTimeRemaining(IgnorePain) > 300 || API.PlayerHealthPercent > IgnorePainLifePercent) && PlayerLevel >= 14)
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