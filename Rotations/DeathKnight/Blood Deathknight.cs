using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperElk.Core
{
    public class BloodDK : CombatRoutine
    {
        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange <= 6;

        private bool IsDefensive => API.ToggleIsEnabled("Defensive");

        //DK specific 

        private int CurrentRune => API.PlayerCurrentRunes;
        private int CurrentRP => API.PlayerRunicPower;
        private int DeathStrikePercentLife => percentListProp[CombatRoutine.GetPropertyInt(DeathStrike)];
        private int VampiricBloodPercentLife => percentListProp[CombatRoutine.GetPropertyInt(VampiricBlood)];
        private int AntiMagicShellPercentLife => percentListProp[CombatRoutine.GetPropertyInt(AntiMagicShell)];
        private int AntiMagicZonePercentLife => percentListProp[CombatRoutine.GetPropertyInt(AntiMagicZone)];
        private int IceboundFortitudePercentLife => percentListProp[CombatRoutine.GetPropertyInt(IceboundFortitude)];
        private int BlooddrinkerPercentLife => percentListProp[CombatRoutine.GetPropertyInt(Blooddrinker)];
        private int DeathPactPercentLife => percentListProp[CombatRoutine.GetPropertyInt(DeathPact)];
        private int RuneTap1PercentLife=> percentListProp[CombatRoutine.GetPropertyInt(RuneTap)];
        private int RuneTap2PercentLife => percentListProp[CombatRoutine.GetPropertyInt(RuneTap2)];
        private int TombstonePercentLife => percentListProp[CombatRoutine.GetPropertyInt(Tombstone)];

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
        private string MindFreeze = "Mind Freeze";
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

            CombatRoutine.AddProp(AntiMagicZone, AntiMagicZone + "%", percentListProp, "Life percent at which " + AntiMagicZone + " is used, set to 0 to disable", "Healing", 0);
            CombatRoutine.AddProp(AntiMagicShell, AntiMagicShell + "%", percentListProp, "Life percent at which " + AntiMagicShell + " is used, set to 0 to disable", "Healing", 7);
            CombatRoutine.AddProp(DeathStrike, DeathStrike + "%", percentListProp, "Life percent at which " + DeathStrike + " is used, set to 0 to disable", "Healing", 9);
            CombatRoutine.AddProp(IceboundFortitude, IceboundFortitude + "%", percentListProp, "Life percent at which " + IceboundFortitude + " is used, set to 0 to disable", "Healing", 4);
            CombatRoutine.AddProp(VampiricBlood, VampiricBlood + "%", percentListProp, "Life percent at which " + VampiricBlood + " is used, set to 0 to disable", "Healing", 6);
            CombatRoutine.AddProp(Blooddrinker, Blooddrinker + "%", percentListProp, "Life percent at which " + Blooddrinker + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(DeathPact, DeathPact + "%", percentListProp, "Life percent at which " + DeathPact + " is used, set to 0 to disable", "Healing", 3);
            CombatRoutine.AddProp(Tombstone, Tombstone + "%", percentListProp, "Life percent at which " + Tombstone + " is used, set to 0 to disable", "Healing", 7);


            CombatRoutine.AddProp(RuneTap, "Rune Tap 1st charge %", percentListProp, "Life percent at which 1st " + RuneTap + " charge is used, set to 0 to disable", "Healing", 8);
            CombatRoutine.AddProp(RuneTap2, "Rune Tap 2nd charge %", percentListProp, "Life percent at which 2nd " + RuneTap2 + " charge is used, set to 0 to disable", "Healing", 5);



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
            CombatRoutine.AddBuff(DancingRuneWeapon);
            CombatRoutine.AddBuff("Haemostasis");
            CombatRoutine.AddBuff("Blood Shield");


            CombatRoutine.AddDebuff("Blood Plague");
            CombatRoutine.AddDebuff(MarkofBlood);


            CombatRoutine.AddToggle("Defensive");
        }

        public override void CombatPulse()
        {
            if (!API.PlayerIsCasting)
            {
                //KICK
                if (isInterrupt && !API.SpellISOnCooldown(MindFreeze) && IsMelee && PlayerLevel >= 7)
                {
                    API.CastSpell(MindFreeze);
                    return;
                }
                if (IsCooldowns)
                {

                    //Raise Dead
                    if (IsCooldowns && PlayerLevel >= 12 && API.CanCast(RaiseDead))
                    {
                        API.CastSpell(RaiseDead);
                        return;
                    }
                    //Dancing Rune Weapon
                    if (IsCooldowns && PlayerLevel >= 34 && API.CanCast(DancingRuneWeapon))
                    {
                        API.CastSpell(DancingRuneWeapon);
                        return;
                    }
                }
                if (IsDefensive)
                {
                    if (API.CanCast(AntiMagicZone) && API.PlayerHealthPercent <= AntiMagicZonePercentLife && API.TargetIsCasting)
                    {
                        API.CastSpell(AntiMagicZone);
                        return;
                    }
                    //Anti-Magic-Shell
                    if (API.CanCast(AntiMagicShell) && API.PlayerHealthPercent <= AntiMagicShellPercentLife && API.TargetIsCasting && PlayerLevel >= 9)
                    {
                        API.CastSpell(AntiMagicShell);
                        return;
                    }
                    //Icebound Fortitude
                    if (API.CanCast(IceboundFortitude) && API.PlayerHealthPercent <= IceboundFortitudePercentLife && PlayerLevel >= 38)
                    {
                        API.CastSpell(IceboundFortitude);
                        return;
                    }
                    //Vampiric Blood
                    if (API.CanCast(VampiricBlood)&& API.PlayerHealthPercent <= VampiricBloodPercentLife && PlayerLevel >= 29)
                    {
                        API.CastSpell(VampiricBlood);
                        return;
                    }
                    //Rune Tap 1st charge
                    if (API.CanCast(RuneTap) && API.PlayerHealthPercent <= RuneTap1PercentLife && PlayerLevel >= 19)
                    {
                        API.CastSpell(RuneTap);
                        return;
                    }
                    //Rune Tap 2nd charge
                    if (API.CanCast(RuneTap) && API.PlayerHealthPercent <= RuneTap2PercentLife && PlayerLevel >= 19)
                    {
                        API.CastSpell(RuneTap);
                        return;
                    }
                    if (API.PlayerIsTalentSelected(6, 2) && API.PlayerHealthPercent < DeathPactPercentLife && API.CanCast(DeathPact))
                    {
                        API.CastSpell(DeathPact);
                        return;
                    }
                    if (IsCooldowns && API.PlayerIsTalentSelected(1, 3) && API.PlayerHealthPercent < TombstonePercentLife && API.CanCast(Tombstone) && IsMelee && API.PlayerBuffStacks(BoneShield) >= 9 && API.PlayerHealthPercent < 90)
                    {
                        API.CastSpell(Tombstone);
                        return;
                    }
                }
                    rotation();
                    return;
            }

        }
        public override void OutOfCombatPulse()
        {

        }

        public override void Pulse()
        {

        }


        private void rotation()
        {
            if (CurrentRune >= 2 && API.CanCast(Marrowrend) && IsMelee && API.PlayerBuffTimeRemaining(BoneShield) < 300 && PlayerLevel >= 11)
            {
                API.CastSpell(Marrowrend);
                return;
            }
            //Death Strike
            if (CurrentRP >= 45 && API.PlayerHealthPercent <= DeathStrikePercentLife && !API.SpellISOnCooldown(DeathStrike) && IsMelee && PlayerLevel >= 4)
            {
                API.CastSpell(DeathStrike);
                return;
            }
            if (API.PlayerIsTalentSelected(1, 2) && !API.PlayerHasBuff(DancingRuneWeapon) && API.PlayerHealthPercent <= BlooddrinkerPercentLife && API.CanCast(Blooddrinker) && API.TargetRange <= 40)
            {
                API.CastSpell(Blooddrinker);
                return;
            }
            if (API.PlayerIsTalentSelected(4, 3) && API.CanCast(MarkofBlood) && !API.TargetHasDebuff(MarkofBlood) && API.TargetRange <= 15)
            {
                API.CastSpell(MarkofBlood);
                return;
            }
            if (IsCooldowns && API.PlayerIsTalentSelected(2, 3) && API.CanCast(Consumption) && IsMelee)
            {
                API.CastSpell(Consumption);
                return;
            }
            if (IsAOE && IsCooldowns && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerIsTalentSelected(7, 3) && CurrentRP >= 100 && API.CanCast(Bonestorm) && IsMelee)
            {
                API.CastSpell(Bonestorm);
                return;
            }

            //Blood Boil
            if (API.CanCast(BloodBoil) && API.TargetRange <= 10 && (!API.TargetHasDebuff(BloodPlague) || API.SpellCharges(BloodBoil)>=2) && PlayerLevel >= 17)
            {
                API.CastSpell(BloodBoil);
                return;
            }

            if (CurrentRune >= 2 && API.CanCast(Marrowrend) && IsMelee && API.PlayerBuffStacks(BoneShield) <= (API.PlayerHasBuff(DancingRuneWeapon)?4:7) && PlayerLevel >= 11)
            {
                API.CastSpell(Marrowrend);
                return;
            }

            //Death and Decay
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && CurrentRune >= 3 && API.CanCast(DeathandDecay) && IsMelee && PlayerLevel >= 3)
            {
                API.CastSpell(DeathandDecay);
                return;
            }
            //Hearth Strike
            if (CurrentRune >= 3 && API.CanCast(HeartStrike) && IsMelee && PlayerLevel >= 10)
            {
                API.CastSpell(HeartStrike);
                return;
            }
            //Death's Caress
            if (CurrentRune >= 3 && API.CanCast(DeathsCaress) && PlayerLevel >= 28 && !API.TargetHasDebuff(BloodPlague) && API.TargetRange <= 30 && API.TargetRange > 30)
            {
                API.CastSpell(DeathsCaress);
                return;
            }
            if (CurrentRP >= 90 && API.CanCast(DeathStrike) && IsMelee && PlayerLevel >= 4)
            {
                API.CastSpell(DeathStrike);
                return;
            }

            if (API.PlayerIsTalentSelected(3, 3) && CurrentRune <= 3 && API.CanCast(BloodTap) && IsMelee)
            {
                API.CastSpell(BloodTap);
                return;
            }
            if (API.CanCast(BloodBoil) && API.PlayerHasBuff(DancingRuneWeapon) && API.TargetRange <= 10 && PlayerLevel >= 17)
            {
                API.CastSpell(BloodBoil);
                return;
            }

            //Death and Decay on Crimson Scourge
            if (API.CanCast(DeathandDecay) && IsMelee && API.PlayerHasBuff(CrimsonScourge) && PlayerLevel >= 3)
            {
                API.CastSpell(DeathandDecay);
                return;
            }

            if (API.CanCast(BloodBoil) && API.TargetRange <= 10 && PlayerLevel >= 17)
            {
                API.CastSpell(BloodBoil);
                return;
            }

        }
    }
}
