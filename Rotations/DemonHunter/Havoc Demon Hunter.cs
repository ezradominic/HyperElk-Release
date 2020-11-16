using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class HacocDemonHunter : CombatRoutine
    {
        //Toggles
        private bool IsPause => API.ToggleIsEnabled("Pause");

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;



        //CLASS SPECIFIC
        private int Vify => numbList[CombatRoutine.GetPropertyInt("FOLOOCPCT")];

        //CBProperties

        private bool UseCF => (bool)CombatRoutine.GetProperty("UseCF");
        private bool KICK => CombatRoutine.GetPropertyBool("KICK");
        private int KICKTime => CombatRoutine.GetPropertyInt("KICKTime");



        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };


        //Spells,Buffs,Debuffs
        private string Metamorphosis = "Metamorphosis";
        private string ImmolationAura = "Immolation Aura";
        private string EyeBeam = "Eye Beam";
        private string ChaosStrike = "Chaos Strike";
        private string Annihilation = "Annihilation";
        private string DemonsBite = "Demon's Bite";
        private string Disrupt = "Disrupt";
        private string Felblade = "Felblade";
        private string ConcentratedFlame = "Concentrated Flame";
        private string BladeDance = "Blade Dance";



        public override void Initialize()
        {
            CombatRoutine.Name = "Havoc DemonHunter  @Mufflon12";
            API.WriteLog("Welcome to Havoc DemonHunter rotation @ Mufflon12");

            //Concentrated Flame
            CombatRoutine.AddProp("UseCF", "Use CF", true, "Should the rotation use Concentrated Flame");

            //Spells
            CombatRoutine.AddSpell("Demon's Bite", "D1");
            CombatRoutine.AddSpell("Felblade", "D2");
            CombatRoutine.AddSpell("Chaos Strike", "D3");
            CombatRoutine.AddSpell("Eye Beam", "D4");
            CombatRoutine.AddSpell("Immolation Aura", "D5");
            CombatRoutine.AddSpell("Metamorphosis", "D6");
            CombatRoutine.AddSpell("Blade Dance", "D6");





            CombatRoutine.AddSpell("Disrupt", "NumPad1");


            CombatRoutine.AddSpell("Concentrated Flame", "NumPad4");


            //Buffs



            //Debuffs

        }

        public override void Pulse()
        {
            if (!IsPause && API.PlayerIsInCombat && !API.PlayerIsCasting && !API.PlayerIsMounted)
            {
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
        public override void CombatPulse()
        {
            if (!IsPause && API.PlayerIsInCombat && !API.PlayerIsCasting && !API.PlayerIsMounted)
            {
                //Cooldowns

                //AOE
                if (IsAOE)
                {

                }

                //KICK
                if (KICK && API.TargetCanInterrupted && API.TargetIsCasting && API.TargetCurrentCastTimeRemaining < KICKTime && !API.SpellISOnCooldown(Disrupt) && IsMelee && PlayerLevel >= 18)
                {
                    API.CastSpell(Disrupt);
                    return;
                }

                rotation();
                return;

            }
        }
        public override void OutOfCombatPulse()
        {

        }

        // ROTATION
        private void rotation()
        {
            //Concentrated Flame
            if (UseCF)
            {
                if (API.CanCast(ConcentratedFlame) && API.TargetRange <= 40)
                {
                    API.CastSpell(ConcentratedFlame);
                    return;
                }
            }
            {
                if (!API.SpellISOnCooldown(Metamorphosis))
                {
                    API.CastSpell(Metamorphosis);
                    return;
                }
            }
            if (!API.SpellISOnCooldown(ImmolationAura))
            {
                API.CastSpell(ImmolationAura);

            }
            if (!API.SpellISOnCooldown(DemonsBite) && API.PlayerFury < 40)
            {
                API.CastSpell(DemonsBite);

            }
            if (!API.SpellISOnCooldown(Felblade))
            {
                API.CastSpell(Felblade);

            }
            if (!API.SpellISOnCooldown(ChaosStrike) && API.PlayerFury > 40)
            {
                API.CastSpell(ChaosStrike);

            }
            if (!API.SpellISOnCooldown(EyeBeam) && API.PlayerFury > 30)
            {
                API.CastSpell(EyeBeam);

            }
            if (!API.SpellISOnCooldown(BladeDance) && API.PlayerFury > 15)
            {
                API.CastSpell(BladeDance);

            }
        }
    }
}

