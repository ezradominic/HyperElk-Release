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
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

        //DK specific 

        private int CurrentRune => API.PlayerCurrentRunes;
        private int CurrentRP => API.PlayerRunicPower;
        private bool UseDND => (bool)CombatRoutine.GetProperty("UseDND");
        private bool UseAMZ => (bool)CombatRoutine.GetProperty("UseAMZ");
        private bool UseCF => (bool)CombatRoutine.GetProperty("UseCF");
        private bool KICK => CombatRoutine.GetPropertyBool("KICK");
        private int KICKTime => CombatRoutine.GetPropertyInt("KICKTime");
        private int DeathStrikePercentProc => numbList[CombatRoutine.GetPropertyInt(DeathStrike)];
        private int RuneTap1PercentProc => numbList[CombatRoutine.GetPropertyInt(RuneTap)];
        private int RuneTap2PercentProc => numbList[CombatRoutine.GetPropertyInt(RuneTap2)];
        private int AOEUnitNumner => CombatRoutine.GetPropertyInt("AOEUnitNumner");


        //Spells//Buffs/Debuffs
        private string Marrowrend = "Marrowrend";
        private string BloodBoil = "Blood Boil";
        private string DeathStrike = "Death Strike";
        private string HeartStrike = "Heart Strike";
        private string AntiMagicShell = "Anti-Magic-Shell";
        private string VampiricBlood = "Vampiric Blood";
        private string IceboundFortitude = "Icebound Fortitude";
        private string DancingRuneWeapon = "Dancing Rune Weapon";
        private string DeathandDecay = "Death and Decay";
        private string MindFreez = "Mind Freez";
        private string Blooddrinker = "Blooddrinker";
        private string DeathsCaress = "Death's Caress";
        private string Healthstone = "Healthstone";
        private string RuneTap = "Rune Tap";
        private string RuneTap2 = "Rune Tap2";
        private string RaiseDead = "Raise Dead";
        private string ConcentratedFlame = "Concentrated Flame";
        private string AntiMagicZone = "Anti-Magic Zone";
        private string Tombstone = "Tombstone";
        private string Consumption = "Consumption";
        private string BloodTap = "Blood Tap";
        private string MarkofBlood = "Mark of Blood";
        private string DeathPact = "Death Pact";
        private string Bonestorm = "Bonestorm";
        private string BoneShield = "Bone Shield";
        private string CrimsonScourge = "Crimson Scourge";
        private string Ossuary = "Ossuary";
        private string Haemostasis = "Haemostasis";
        private string BloodShield = "Blood Shield";
        private string BloodPlague = "Blood Plague";



        public override void Initialize()
        {
            CombatRoutine.Name = "Blood DK @Mufflon12";
            API.WriteLog("Welcome to Blood DK rotation @ Mufflon12");
            API.WriteLog("DnD Macro to be use : /cast [@player] Death and Decay");
            API.WriteLog("Anti-Magic Zone Macro to be use : /cast [@player] Anti-Magic Zone");
            CombatRoutine.AddProp("UseDND", "Use DND", true, "Should the rotation use Death and Decay");
            CombatRoutine.AddProp("UseAMZ", "Use AMZ", true, "Should the rotation use Anti-Magic Zone");
            CombatRoutine.AddProp("UseCF", "Use CF", true, "Should the rotation use Concentrated Flame");

            CombatRoutine.AddProp(DeathStrike, "Death Strike", numbList, "Life percent at which " + DeathStrike + " is used, set to 0 to disable", "Healing", 9);
            CombatRoutine.AddProp(RuneTap, "Rune Tap 1st charge", numbList, "Life percent at which 1st " + RuneTap + " charge is used, set to 0 to disable", "Healing", 8);
            CombatRoutine.AddProp(RuneTap2, "Rune Tap 2nd charge", numbList, "Life percent at which 2nd " + RuneTap2 + " charge is used, set to 0 to disable", "Healing", 5);



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
                    if (API.CanCast(AntiMagicZone, true, true) && API.TargetIsCasting)
                    {
                        API.CastSpell(AntiMagicZone);
                        return;
                    }
                }
                //KICK
                if (KICK && API.TargetCanInterrupted && API.TargetIsCasting && API.TargetCurrentCastTimeRemaining < KICKTime && !API.SpellISOnCooldown(MindFreez) && IsMelee && PlayerLevel >= 7)
                {
                    API.CastSpell(MindFreez);
                    return;
                }
                //Death Strike
                if (CurrentRP >= 45 && API.PlayerHealthPercent <= DeathStrikePercentProc && !API.SpellISOnCooldown(DeathStrike) && IsMelee && PlayerLevel >= 4)
                {
                    API.CastSpell(DeathStrike);
                    return;
                }
                if (IsCooldowns)
                {

                    //Raise Dead
                    if (PlayerLevel >= 12 && API.CanCast(RaiseDead))
                    {
                        API.CastSpell(RaiseDead);
                        return;
                    }
                    //Dancing Rune Weapon
                    if (PlayerLevel >= 34 && API.CanCast(DancingRuneWeapon))
                    {
                        API.CastSpell(DancingRuneWeapon);
                        return;
                    }
                    //Icebound Fortitude
                    if (API.CanCast(IceboundFortitude) && PlayerLevel >= 38)
                    {
                        API.CastSpell(IceboundFortitude);
                        return;
                    }
                    //Vampiric Blood
                    if (API.CanCast(VampiricBlood) && PlayerLevel >= 29)
                    {
                        API.CastSpell(VampiricBlood);
                        return;
                    }
                    //Rune Tap 1st charge
                    if (API.CanCast(RuneTap) && API.PlayerHealthPercent <= RuneTap1PercentProc && PlayerLevel >= 19)
                    {
                        API.CastSpell(RuneTap);
                        return;
                    }
                    //Rune Tap 2nd charge
                    if (API.CanCast(RuneTap) && API.PlayerHealthPercent <= RuneTap2PercentProc && PlayerLevel >= 19)
                    {
                        API.CastSpell(RuneTap);
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

            //Concentrated Flame
            if (UseCF)
            {
                if (API.CanCast(ConcentratedFlame) && IsMelee && API.TargetRange <= 40)
                {
                    API.CastSpell(ConcentratedFlame);
                    return;
                }
            }
            //Death and Decay
            if (UseDND)
            {
                if (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumner && CurrentRune >= 1 && API.CanCast(DeathandDecay) && IsMelee && PlayerLevel >= 3)
                {
                    API.CastSpell(DeathandDecay);
                    return;
                }
            }
            //Death's Caress
            if ( API.CanCast(DeathsCaress) && PlayerLevel >= 28 && API.TargetRange <= 30)
            {
                API.CastSpell(Marrowrend);
                return;
            }
            //Marrowrend
            if (CurrentRune >= 2 && API.CanCast(Marrowrend) && API.PlayerBuffStacks(BoneShield) < 6 && PlayerLevel >= 11)
            {
                API.CastSpell(Marrowrend);
                return;
            }
            if (CurrentRune >= 2 && API.CanCast(Marrowrend) && API.PlayerBuffTimeRemaining(Marrowrend) < 300 && PlayerLevel >= 11)
            {
                API.CastSpell(Marrowrend);
                return;
            }
            //Blood Boil
            if (API.CanCast(BloodBoil) && IsMelee && API.TargetDebuffRemainingTime(BloodBoil) < 300 && PlayerLevel >= 17)
            {
                API.CastSpell(BloodBoil);
                return;
            }
            //Death and Decay on Crimson Scourge
            if (CurrentRune >= 3 && API.CanCast(DeathandDecay) && API.PlayerHasBuff(CrimsonScourge) && PlayerLevel >= 3)
            {
                API.CastSpell(DeathandDecay);
                return;
            }
            //Hearth Strike
            if (CurrentRune >= 3 && API.CanCast(HeartStrike) && PlayerLevel >= 10)
            {
                API.CastSpell(HeartStrike);
                return;
            }
            //Blood Boil
            if (CurrentRune >= 3 && API.CanCast(BloodBoil) && API.TargetUnitInRangeCount > 2 && PlayerLevel >= 17)
            {
                API.CastSpell(BloodBoil);
                return;
            }
            //Death Strike to Dumpe RP
            if (CurrentRP >= 45 && API.CanCast(DeathStrike) && IsMelee && PlayerLevel >= 4)
            {
                API.CastSpell(DeathStrike);
                return;
            }
            //Anti-Magic-Shell
            if (API.CanCast(AntiMagicShell) && API.TargetIsCasting && PlayerLevel >= 9)
            {
                API.CastSpell(AntiMagicShell);
                return;
            }
            //TALENTS
            if (API.PlayerIsTalentSelected(1, 2) && API.CanCast(Blooddrinker) && IsMelee && API.TargetRange <= 40)
            {
                API.CastSpell(Blooddrinker);
                return;
            }
            if (API.PlayerIsTalentSelected(1, 3) && API.CanCast(Tombstone) && IsMelee && API.PlayerHasBuff(BoneShield) && API.PlayerHealthPercent > 80)
            {
                API.CastSpell(Tombstone);
                return;
            }
            if (API.PlayerIsTalentSelected(2, 3) && API.CanCast(Consumption) && IsMelee)
            {
                API.CastSpell(Consumption);
                return;
            }
            if (API.PlayerIsTalentSelected(3, 3) && CurrentRune >= 6 && API.CanCast(BloodTap))
            {
                API.CastSpell(BloodTap);
                return;
            }
            if (API.PlayerIsTalentSelected(4, 3) && API.CanCast(MarkofBlood))
            {
                API.CastSpell(MarkofBlood);
                return;
            }
            if (API.PlayerIsTalentSelected(6, 2) && API.PlayerHealthPercent < 50 && API.CanCast(DeathPact))
            {
                API.CastSpell(DeathPact);
                return;
            }
            if (API.PlayerIsTalentSelected(7, 3) && CurrentRP >= 100 && API.CanCast(Bonestorm) && IsMelee)
            {
                API.CastSpell(Bonestorm);
                return;
            }
        }
    }
}
