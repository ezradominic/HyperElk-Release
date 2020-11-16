using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperElk.Core
{
    public class BloodDK : CombatRoutine
    {
        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;

        //DK specific 

        private int CurrentRune => API.PlayerCurrentRunes;
        private int CurrentRP => API.PlayerRunicPower;
        private bool UseDND => (bool)CombatRoutine.GetProperty("UseDND");
        private bool UseAMZ => (bool)CombatRoutine.GetProperty("UseAMZ");
        private bool UseCF => (bool)CombatRoutine.GetProperty("UseCF");
        private bool Healthstone => (bool)CombatRoutine.GetProperty("Healthstone");



        public override void Initialize()
        {
            CombatRoutine.Name = "Blood DK @Mufflon12";
            API.WriteLog("Welcome to Blood DK rotation @ Mufflon12");
            API.WriteLog("DnD Macro to be use : /cast [@player] Death and Decay");
            API.WriteLog("Anti-Magic Zone Macro to be use : /cast [@player] Anti-Magic Zone");
            CombatRoutine.AddProp("UseDND", "Use DND", true, "Should the rotation use Death and Decay");
            CombatRoutine.AddProp("UseAMZ", "Use AMZ", true, "Should the rotation use Anti-Magic Zone");
            CombatRoutine.AddProp("UseCF", "Use CF", true, "Should the rotation use Concentrated Flame");
            CombatRoutine.AddProp("Healthstone", "Healthstone", true, "Should the rotation use Healthstone");

            CombatRoutine.AddSpell("Marrowrend", "D1");
            CombatRoutine.AddSpell("Blood Boil", "D2");
            CombatRoutine.AddSpell("Death Strike", "D3");
            CombatRoutine.AddSpell("Heart Strike", "D4");
            CombatRoutine.AddSpell("Anti-Magic-Shell", "D6");
            CombatRoutine.AddSpell("Vampiric Blood", "D7");
            CombatRoutine.AddSpell("Icebound Fortitude", "D8");
            CombatRoutine.AddSpell("Dancing Rune Weapon", "F10");
            CombatRoutine.AddSpell("Death and Decay", "D5");
            CombatRoutine.AddSpell("Mind Freeze", "F");
            CombatRoutine.AddSpell("Blooddrinker", "F3");
            CombatRoutine.AddSpell("Death's Caress", "F8");
            CombatRoutine.AddSpell("Healthstone", "NumPad1");
            CombatRoutine.AddSpell("Rune Tap", "F7");
            CombatRoutine.AddSpell("Raise Dead", "NumPad5");
            CombatRoutine.AddSpell("Concentrated Flame", "NumPad2");
            CombatRoutine.AddSpell("Anti-Magic Zone", "NumPad6");
            CombatRoutine.AddSpell("Tombstone", "NumPad4");
            CombatRoutine.AddSpell("Consumption", "NumPad9");
            CombatRoutine.AddSpell("Blood Tap", "F1");
            CombatRoutine.AddSpell("Mark of Blood", "F2");
            CombatRoutine.AddSpell("Death Pact", "F3");
            CombatRoutine.AddSpell("Bonestorm", "F4");

            CombatRoutine.AddBuff("Bone Shield");
            CombatRoutine.AddBuff("Crimson Scourge");
            CombatRoutine.AddBuff("Ossuary");
            CombatRoutine.AddBuff("Dancing Rune Weapon");
            CombatRoutine.AddBuff("Haemostasis");
            CombatRoutine.AddBuff("Blood Shield");

            CombatRoutine.AddDebuff("Blood Plague");

        }

        public override void CombatPulse()
        {
            if (!API.PlayerIsCasting)
            {
                if (UseAMZ)
                {
                    if (API.CanCast("Anti-Magic Zone", true, true) && API.TargetIsCasting)
                    {
                        API.CastSpell("Anti-Magic Zone");
                        return;
                    }
                }
                if (UseCF)
                {
                    if (API.CanCast("Concentrated Flame", true, true) && IsMelee && API.TargetRange <= 40 && API.PlayerBuffTimeRemaining("Concentrated Flame") < 300)
                    {
                        API.CastSpell("Concentrated Flame");
                        return;
                    }
                }
                if (UseDND)
                {
                    if (API.PlayerUnitInMeleeRangeCount >= 2 && CurrentRune >= 1 && API.CanCast("Death and Decay", true, true) && IsMelee)
                    {
                        API.CastSpell("Death and Decay");
                        return;
                    }
                }
                if (IsCooldowns)
                {
                    if (PlayerLevel >= 12 && API.CanCast("Raise Dead", true, true))
                    {
                        API.CastSpell("Raise Dead");
                        return;
                    }
                    if (PlayerLevel >= 34 && API.CanCast("Dancing Rune Weapon", true, true))
                    {
                        API.CastSpell("Dancing Rune Weapon");
                        return;
                    }
                    if (API.CanCast("Icebound Fortitude", true, true))
                    {
                        API.CastSpell("Icebound Fortitude");
                        return;
                    }
                    if (API.CanCast("Vampiric Blood", true, true))
                    {
                        API.CastSpell("Vampiric Blood");
                        return;
                    }
                    if (API.CanCast("Rune Tap", true, true) && API.PlayerHealthPercent < 85)
                    {
                        API.CastSpell("Rune Tap");
                        return;
                    }
                    if (API.CanCast("Rune Tap", true, true) && API.PlayerHealthPercent < 50)
                    {
                        API.CastSpell("Rune Tap");
                        return;
                    }
                }
                if (PlayerLevel <= 50)
                {
                    rotation();
                    return;
                }
            }

        }
        public override void OutOfCombatPulse()
        {
            throw new NotImplementedException();
        }

        public override void Pulse()
        {
           
        }


        private void rotation()
        {
            if (CurrentRune >= 2 && API.CanCast("Marrowrend", true, true) && API.PlayerBuffStacks("Bone Shield") < 6)
            {
                API.CastSpell("Marrowrend");
                return;
            }
            if (CurrentRune >= 2 && API.CanCast("Marrowrend", true, true) && API.PlayerBuffTimeRemaining("Bone Shield") < 300)
            {
                API.CastSpell("Marrowrend");
                return;
            }
            if (API.CanCast("Blood Boil", true, true) && IsMelee && API.TargetDebuffRemainingTime("Blood Plague") < 300)
            {
                API.CastSpell("Blood Boil");
                return;
            }
            if (CurrentRune >= 3 && API.CanCast("Death and Decay", true, true) && API.PlayerHasBuff("Crimson Scourge"))
            {
                API.CastSpell("Death and Decay");
                return;
            }
            if (CurrentRune >= 3 && API.CanCast("Heart Strike", true, true))
            {
                API.CastSpell("Heart Strike");
                return;
            }
            if (CurrentRune >= 3 && API.CanCast("Blood Boil", true, true) && API.TargetUnitInRangeCount > 2)
            {
                API.CastSpell("Blood Boil");
                return;
            }
            if (CurrentRP >= 45 && API.CanCast ("Death Strike", true, true) && IsMelee)
            {
                API.CastSpell("Death Strike");
                return;
            }
            if (API.CanCast("Mind Freeze", true, true) && API.TargetIsCasting && IsMelee && API.TargetRange <= 15)
            {
                API.CastSpell("Mind Freeze");
                return;
            }
            if (API.CanCast("Anti-Magic-Shell", true, true) && API.TargetIsCasting)
            {
                API.CastSpell("Anti-Magic-Shell");
                return;
            }
            //TALENTS
            if (API.PlayerIsTalentSelected(1, 2) && API.CanCast("Blooddrinker", true, true) && IsMelee && API.TargetRange <= 40)
            {
                API.CastSpell("Blooddrinker");
                return;
            }
            if (API.PlayerIsTalentSelected(1, 3) && API.CanCast("Tombstone", true, true) && IsMelee && API.PlayerHasBuff("Bone Shield") && API.PlayerHealthPercent >80)
            {
                API.CastSpell("Tombstone");
                return;
            }
            if (API.PlayerIsTalentSelected(2, 3) && API.CanCast("Consumption", true, true) && IsMelee)
            {
                API.CastSpell("Consumption");
                return;
            }
            if (API.PlayerIsTalentSelected(3, 3) && CurrentRune >= 6 && API.CanCast("Blood Tap", true, true))
            {
                API.CastSpell("Blood Tap");
                return;
            }
            if (API.PlayerIsTalentSelected(4, 3) && API.CanCast("Mark of Blood", true, true))
            {
                API.CastSpell("Mark of Blood");
                return;
            }
            if (API.PlayerIsTalentSelected(6, 2) && API.PlayerHealthPercent < 50 && API.CanCast("Death Pact", true, true))
            {
                API.CastSpell("Death Pact");
                return;
            }
            if (API.PlayerIsTalentSelected(7, 3) && CurrentRP >= 100 && API.CanCast("Bonestorm", true, true) && IsMelee)
            {
                API.CastSpell("Bonestorm");
                return;
            }
        }
    }
}
