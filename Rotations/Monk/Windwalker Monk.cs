using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class WindwalkerMonk : CombatRoutine
    {
        //Toggles

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;
        private bool NotChanneling => !API.PlayerIsChanneling;


        //CLASS SPECIFIC
        private bool TalentChiWave => API.PlayerIsTalentSelected(1, 2);
        private bool TalentChiBurst => API.PlayerIsTalentSelected(1, 3);
        private bool TalentFistoftheWhiteTiger => API.PlayerIsTalentSelected(3, 2);
        private bool TalentEnergizingElixir => API.PlayerIsTalentSelected(3, 3);
        private bool TalentRingofPeace => API.PlayerIsTalentSelected(4, 3);
        private bool TalentDampenHarm => API.PlayerIsTalentSelected(5, 3);
        private bool TalentRushingJadeWind => API.PlayerIsTalentSelected(6, 2);
        private bool TalentDanceofChiJi => API.PlayerIsTalentSelected(6, 3);
        private bool TalentWhirlingDragonPunch => API.PlayerIsTalentSelected(7, 2);



        //CBProperties
        private int VivifyLifePercentProc => numbList[CombatRoutine.GetPropertyInt(Vivify)];
        private int ExpelHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(ExpelHarm)];
        private int FortifyingBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(FortifyingBrew)];
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };


        //Spells,Buffs,Debuffs
        private string TigerPalm = "Tiger Palm";
        private string BlackOutKick = "Blackout Kick";
        private string BlackOutKickBuff = "Blackout Kick!";
        private string Vivify = "Vivify";
        private string SpinningCraneKick = "Spinning Crane Kick";
        private string ExpelHarm = "Expel Harm";
        private string SpearHandStrike = "Spear Hand Strike";
        private string TouchofDeath = "Touch of Death";
        private string RisingSunKick = "Rising Sun Kick";
        private string FistsofFury = "Fists of Fury";
        private string FortifyingBrew = "Fortifying Brew";
        private string RushingJadeWind = "Rushing Jade Wind";
        private string ChiBurst = "Chi Burst";
        private string FistsoftheWhiteTiger = "Fist of the White Tiger";
        private string WhirlingDragonPunch = "Whirling Dragon Punch";
        public override void Initialize()
        {
            CombatRoutine.Name = "Windwalker Monk @Mufflon12";
            API.WriteLog("Welcome to Windwalker Monk rotation @ Mufflon12");

            CombatRoutine.AddProp(Vivify, "Vivify", numbList, "Life percent at which " + Vivify + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(ExpelHarm, "Expel Harm", numbList, "Life percent at which " + ExpelHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 9);
            CombatRoutine.AddProp(FortifyingBrew, "Fortifying Brew", numbList, "Life percent at which " + FortifyingBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 4);


            //Spells
            CombatRoutine.AddSpell(TigerPalm, "D1");
            CombatRoutine.AddSpell(BlackOutKick, "D2");
            CombatRoutine.AddSpell(SpinningCraneKick, "D3");
            CombatRoutine.AddSpell(SpearHandStrike, "D4");
            CombatRoutine.AddSpell(FistsofFury, "D5");
            CombatRoutine.AddSpell(FistsoftheWhiteTiger, "D6");
            CombatRoutine.AddSpell(WhirlingDragonPunch, "D7");
            CombatRoutine.AddSpell(TouchofDeath, "D7");



            CombatRoutine.AddSpell(ChiBurst, "D9");
            CombatRoutine.AddSpell(RisingSunKick, "D0");
            CombatRoutine.AddSpell(RushingJadeWind, "Oem6");

            CombatRoutine.AddSpell(Vivify, "NumPad1");
            CombatRoutine.AddSpell(ExpelHarm, "NumPad2");

            CombatRoutine.AddSpell(FortifyingBrew, "NumPad5");



            //Buffs
            CombatRoutine.AddBuff("Blackout Kick!");
            CombatRoutine.AddBuff("Dance of Chi-Ji");

            //Debuffs



        }

        public override void Pulse()
        {

        }
        public override void CombatPulse()
        {
            //COOLDOWNS
            //Touch of Death
            if (IsCooldowns && !API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 0 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10)
            {
                API.CastSpell(TouchofDeath);
                return;
            }

            //AOE
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber)
            {
                //WhirlingDragonPunch
                if (API.CanCast(WhirlingDragonPunch) && NotChanneling && TalentWhirlingDragonPunch && API.SpellISOnCooldown(FistsofFury) && API.SpellISOnCooldown(RisingSunKick))
                {
                    API.CastSpell(WhirlingDragonPunch);
                    return;
                }
                //Tiger Palm
                if (API.CanCast(TigerPalm) && NotChanneling && API.PlayerEnergy >= 45 && API.PlayerCurrentChi <= 4 && NotChanneling)
                {
                    API.CastSpell(TigerPalm);
                    return;
                }
                //ChiBurst
                if (API.CanCast(ChiBurst) && TalentChiBurst && API.PlayerCurrentChi <= 5 && NotChanneling)
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                //Rising Sun Kick
                if (API.CanCast(RisingSunKick) && NotChanneling && !API.SpellISOnCooldown(RisingSunKick) && API.PlayerCurrentChi >= 2 && API.PlayerUnitInMeleeRangeCount <= 2 && API.PlayerLevel >= 2 && NotChanneling)
                {
                    API.CastSpell(RisingSunKick);
                    return;
                }
                //Fists of Fury
                if (API.CanCast(FistsofFury) && API.PlayerCurrentChi >= 3 && API.PlayerLevel >= 10 && NotChanneling)
                {
                    API.CastSpell(FistsofFury);
                    return;
                }
                //Spinning Crane Kick Dance of the Chi
                if (IsAOE && API.CanCast(SpinningCraneKick) && API.PlayerEnergy >= 40 && API.PlayerHasBuff("Dance of Chi-Ji") && TalentDanceofChiJi && API.PlayerLevel >= 45 && NotChanneling)
                {
                    API.CastSpell(SpinningCraneKick);
                    return;
                }
                //Spinning Crane Kick
                if (IsAOE && API.CanCast(SpinningCraneKick) && API.PlayerEnergy >= 40 && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerEnergy > 40 && API.PlayerCurrentChi >=2 && API.PlayerLevel >= 7 && NotChanneling)
                {
                API.CastSpell(SpinningCraneKick);
                return;
                }
                //Rushing Jade Wind
                if (IsAOE && API.CanCast(RushingJadeWind) && !API.SpellISOnCooldown(RushingJadeWind) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerCurrentChi >=1 && TalentRushingJadeWind && NotChanneling)
                {
                API.CastSpell(RushingJadeWind);
                return;
                }
                //BlackOutKick
                if (API.CanCast(BlackOutKick) && NotChanneling && API.PlayerCurrentChi >= 4 && API.PlayerLevel >= 2 && NotChanneling)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //BlackOutKick with buff
                if (API.CanCast(BlackOutKick) && NotChanneling && API.PlayerHasBuff(BlackOutKickBuff) && API.PlayerLevel >= 2 && NotChanneling)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //FistsoftheWhiteTiger 
                if (API.CanCast(FistsoftheWhiteTiger) && NotChanneling && API.PlayerEnergy >= 65 && API.PlayerCurrentChi <= 3 && NotChanneling)
                {
                    API.CastSpell(FistsoftheWhiteTiger);
                    return;
                }
            }
            //KICK
            if (isInterrupt && !API.SpellISOnCooldown(SpearHandStrike) && IsMelee && PlayerLevel >= 18 && NotChanneling)
            {
                API.CastSpell(SpearHandStrike);
                return;
            }

            //HEALING
            //Expel Harm
            if (API.PlayerHealthPercent <= ExpelHarmLifePercentProc && !API.SpellISOnCooldown(ExpelHarm) && !API.PlayerIsMounted && API.PlayerEnergy > 30 && PlayerLevel >= 8 && NotChanneling)
            {
                API.CastSpell(ExpelHarm);
                return;
            }
            //Vivify
            if (API.PlayerHealthPercent <= VivifyLifePercentProc && API.CanCast(Vivify) && PlayerLevel >= 4 && NotChanneling)
            {
                API.CastSpell(Vivify);
                return;
            }
            //Fortifying Brew
            if (API.PlayerHealthPercent <= FortifyingBrewLifePercentProc && !API.SpellISOnCooldown(FortifyingBrew) && PlayerLevel >= 28 && NotChanneling)
            {
                API.CastSpell(FortifyingBrew);
                return;
            }

            //ROTATION
            //WhirlingDragonPunch
            if (API.CanCast(WhirlingDragonPunch) && NotChanneling && TalentWhirlingDragonPunch && API.SpellISOnCooldown(FistsofFury) && API.SpellISOnCooldown(RisingSunKick) && NotChanneling)
            {
                API.CastSpell(WhirlingDragonPunch);
                return;
            }
            //BlackOutKick with buff
            if (API.CanCast(BlackOutKick) &&NotChanneling && API.PlayerHasBuff(BlackOutKickBuff) && API.PlayerLevel >= 2 && NotChanneling)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //Rising Sun Kick
            if (API.CanCast(RisingSunKick) && NotChanneling && !API.SpellISOnCooldown(RisingSunKick) && API.PlayerCurrentChi >= 2 && API.PlayerLevel >= 2 && NotChanneling)
            {
                API.CastSpell(RisingSunKick);
                return;
            }
            //ChiBurst
            if (API.CanCast(ChiBurst) && TalentChiBurst && API.PlayerCurrentChi >= 5 && NotChanneling)
            {
                API.CastSpell(ChiBurst);
                return;
            }
            //Fists of Fury
            if (API.CanCast(FistsofFury) && API.PlayerCurrentChi >= 3 && API.PlayerLevel >= 10 && NotChanneling)
            {
                API.CastSpell(FistsofFury);
                return;
            }
            //BlackOutKick
            if (API.CanCast(BlackOutKick) && NotChanneling && API.PlayerCurrentChi >= 4 && API.PlayerLevel >= 2 && NotChanneling)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //Tiger Palm
            if (API.CanCast(TigerPalm) && NotChanneling && API.PlayerEnergy >= 45 && API.PlayerCurrentChi <= 4 && NotChanneling)
            {
                API.CastSpell(TigerPalm);
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            //Vivify
            if (API.PlayerHealthPercent <= VivifyLifePercentProc && API.CanCast(Vivify) && PlayerLevel >= 4)
            {
                API.CastSpell(Vivify);
                return;
            }
        }
    }
}
