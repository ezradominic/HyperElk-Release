using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class FrostMage : CombatRoutine
    {
        //Toggles
        private bool IsPause => API.ToggleIsEnabled("Pause");

        //General
        private int PlayerLevel => API.PlayerLevel;


        //CLASS SPECIFIC
        private bool UseFB => (bool)CombatRoutine.GetProperty("UseFB");
        private bool UseFN => (bool)CombatRoutine.GetProperty("UseFN");



        //Spells,Buffs,Debuffs
        private string Frostbolt = "Frostbolt";
        private string FireBlast = "Fire Blast";
        private string ArcaneIntellect = "Arcane Intellect";
        private string Fireball = "Fireball";
        private string Pyroblast = "Pyroblast";
        private string HotStreak = "Hot Streak!";
        private string Scorch = "Scorch";
        private string FlameStrike = "Flame Strike";
        private string Combustion = "Combustion";
        private string FrostNova = "Frost Nova";
        private string DragonsBreath = "Dragon's Breath";
        private string PhoenixFlames = "Phoenix Flames";


        public override void Initialize()
        {
            CombatRoutine.Name = "Frost Mage @Mufflon12";
            API.WriteLog("Welcome to Frost Mage rotation @ Mufflon12");
            API.WriteLog("Use /cast [@Player] Arcane Intellect to buff Arcane Intellect");
            API.WriteLog("Use /cast [@cursor] Flame Strike for Flame Strike AEO");

            CombatRoutine.AddProp("UseFB", "Use Fire Blast", true, "Should the rotation use Fire Blast");
            CombatRoutine.AddProp("UseFN", "Use Frost Nova", true, "Should the rotation use Frost Nova if target range is below 10 Yards");



            //Spells
            CombatRoutine.AddSpell("Frostbolt", "D1");
            CombatRoutine.AddSpell("Fireball", "D2");
            CombatRoutine.AddSpell("Pyroblast", "D3");
            CombatRoutine.AddSpell("Scorch", "D4");
            CombatRoutine.AddSpell("Dragon's Breath", "D4");
            CombatRoutine.AddSpell("Phoenix Flames", "D5");



            CombatRoutine.AddSpell("Fire Blast", "NumPad1");
            CombatRoutine.AddSpell("Frost Nova", "NumPad2");
            CombatRoutine.AddSpell("Arcane Intellect", "NumPad3");
            CombatRoutine.AddSpell("Flame Strike", "NumPad4");
            CombatRoutine.AddSpell("Combustion", "NumPad5");




            //Buffs
            CombatRoutine.AddBuff("Arcane Intellect");
            CombatRoutine.AddBuff("Hot Streak!");



            //Debuffs
            CombatRoutine.AddDebuff("Frost Nova");


            //TOGGLES
            CombatRoutine.AddToggle("Pause");
            CombatRoutine.AddToggle("AOE");
            CombatRoutine.AddToggle("Cooldowns");
        }

        public override void Pulse()
        {
            if (!IsPause && API.PlayerIsInCombat && !API.PlayerIsCasting && !API.PlayerIsMounted)
            {
                //Cooldowns
                if (IsCooldowns)
                {
                    if (API.CanCast(Combustion) && API.PlayerLevel >= 28)
                    {
                        API.CastSpell(Combustion);
                        return;
                    }
                }
                //AOE
                if (IsAOE)
                {
                    if (API.CanCast(FlameStrike) && API.PlayerLevel >= 16)
                    {
                        API.CastSpell(FlameStrike);
                        return;
                    }
                }
                if ((!API.PlayerIsInCombat || API.PlayerIsInCombat) && (!API.TargetIsIncombat || API.TargetIsIncombat) && API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
                {
                    CombatPulse();
                }

                if (!API.PlayerIsMounted)
                {
                    if (!API.PlayerIsInCombat)
                    {
                        OutOfCombatPulse();
                    }

                    rotation();
                    return;

                }
            }
        }

        public override void OutOfCombatPulse()
        {

        }
        public override void CombatPulse()
        {
            if (!IsPause && API.PlayerIsInCombat && !API.PlayerIsCasting && !API.PlayerIsMounted)
            {
                //ACANE INTELLECT
                if (API.CanCast(ArcaneIntellect) && !API.PlayerHasBuff(ArcaneIntellect) && API.TargetUnitInRangeCount > 2 && API.PlayerLevel >= 3)
                {
                    API.CastSpell(ArcaneIntellect);
                    return;
                }

                //FROST NOVA
                if (UseFN)
                {
                    if (!API.SpellISOnCooldown(FrostNova) && PlayerLevel >= 3 && API.TargetRange < 10)
                    {
                        API.CastSpell(FrostNova);
                        return;
                    }
                }

                //PYROBLAST ON HOT STREAK
                if (API.CanCast(Pyroblast) && API.PlayerHasBuff(HotStreak) && !API.PlayerIsMoving && API.TargetRange < 40 && PlayerLevel >= 14)
                {
                    API.CastSpell(Pyroblast);
                    return;
                }

                //PHOENIX FLAMES
                if (API.CanCast(PhoenixFlames) && API.TargetRange < 40 && PlayerLevel >= 19)
                {
                    API.CastSpell(PhoenixFlames);
                    return;
                }

                //DRAGONS BREATH
                if (API.CanCast(DragonsBreath) && API.TargetRange < 10 && PlayerLevel >= 27)
                {
                    API.CastSpell(DragonsBreath);
                    return;
                }

                //FIREBLAST
                if (UseFB)
                {
                    if (!API.SpellISOnCooldown(FireBlast) && PlayerLevel >= 3)
                    {
                        API.CastSpell(FireBlast);
                        return;
                    }
                }

                //SNORCH BELOW 30%
                if (API.CanCast(Scorch) && API.PlayerIsMoving && API.TargetRange < 40 && PlayerLevel >= 14 && API.TargetHealthPercent < 30)
                {
                    API.CastSpell(Scorch);
                    return;
                }

                //FIREBALL
                if (API.CanCast(Fireball) && !API.PlayerIsMoving && API.TargetRange < 40 && PlayerLevel >= 9)
                {
                    API.CastSpell(Fireball);
                    return;
                }

                //FROSTBOLT
                if (API.CanCast(Frostbolt) && !API.PlayerIsMoving && API.TargetRange < 40 && PlayerLevel <= 10)
                {
                    API.CastSpell(Frostbolt);
                    return;
                }
            }
        }


        // ROTATION
        private void rotation()
        {

        }
    }
}
