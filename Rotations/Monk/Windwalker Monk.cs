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
        private bool NotMoving => !API.PlayerIsMoving;


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
        private bool TalentSerenty => API.PlayerIsTalentSelected(7, 3);



        //CBProperties
        private int VivifyLifePercentProc => numbList[CombatRoutine.GetPropertyInt(Vivify)];
        private int ExpelHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(ExpelHarm)];
        private int FortifyingBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(FortifyingBrew)];
        private int DampenHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DampenHarm)];

        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        bool LastTigerPalm => API.PlayerLastSpell == TigerPalm;
        bool LastBlackoutkick => API.PlayerLastSpell == BlackOutKick;
        private string UseTouchofDeath => TouchofDeathList[CombatRoutine.GetPropertyInt(TouchofDeath)];
        string[] TouchofDeathList = new string[] { "always", "with Cooldowns" };

        private string UseInvokeXuen => InvokeXuenList[CombatRoutine.GetPropertyInt(InvokeXuen)];
        string[] InvokeXuenList = new string[] { "always", "with Cooldowns" };
        private string Covenant => CovenantList[CombatRoutine.GetPropertyInt("Covenant")];
        string[] CovenantList = new string[] { "None", "Venthyr", "Night Fae", "Kyrian", "Necrolord" };
        //Kyrian
        private string UseWeaponsofOrder => WeaponsofOrderList[CombatRoutine.GetPropertyInt(WeaponsofOrder)];
        string[] WeaponsofOrderList = new string[] { "always", "with Cooldowns", "AOE" };
        //Necrolords
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        string[] BonedustBrewList = new string[] { "always", "with Cooldowns", "AOE" };
        private string UseBonedustBrew => BonedustBrewList[CombatRoutine.GetPropertyInt(BonedustBrew)];
        //Nigh Fae
        string[] FaelineStompList = new string[] { "always", "with Cooldowns", "AOE" };
        private string UseFaelineStomp => FaelineStompList[CombatRoutine.GetPropertyInt(FaelineStomp)];

        //Venthyr 
        string[] FallenOrderList = new string[] { "always", "with Cooldowns", "AOE" };
        private string UseFallenOrder => FallenOrderList[CombatRoutine.GetPropertyInt(FallenOrder)];

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
        private string ChiWave = "Chi Wave";
        private string MarkoftheCrane = "Mark of the Crane";
        private string DanceofChiJi = "Dance of Chi-Ji";
        private string StormEarthandFire = "Storm Earth and Fire";
        private string Serenity = "Serenity";
        private string DampenHarm = "Dampen Harm";
        private string EnergizingElixir = "Energizing Elixir";
        private string InvokeXuen = "Invoke Xuen,  the White Tiger";
        private string WeaponsofOrder = "Weapons of Order";
        private string BonedustBrew = "Bonedust Brew";
        private string Fleshcraft = "Fleshcraft";
        private string FaelineStomp = "FaelineStomp";
        private string FallenOrder = "Fallen Order";

        public override void Initialize()
        {
            CombatRoutine.Name = "Windwalker Monk @Mufflon12";
            API.WriteLog("Welcome to Windwalker Monk rotation @ Mufflon12");

            CombatRoutine.AddProp(Vivify, "Vivify", numbList, "Life percent at which " + Vivify + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(ExpelHarm, "Expel Harm", numbList, "Life percent at which " + ExpelHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 9);
            CombatRoutine.AddProp(FortifyingBrew, "Fortifying Brew", numbList, "Life percent at which " + FortifyingBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 4);
            CombatRoutine.AddProp(DampenHarm, "Dampen Harm", numbList, "Life percent at which " + DampenHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 4);
            CombatRoutine.AddProp(TouchofDeath, "Use " + TouchofDeath, TouchofDeathList, "Use " + TouchofDeath + "always, with Cooldowns", "Cooldowns", 1);
            CombatRoutine.AddProp(InvokeXuen, "Use " + InvokeXuen, InvokeXuenList, "Use " + InvokeXuenList + "always, with Cooldowns", "Cooldowns", 1);
            //Kyrian
            CombatRoutine.AddProp("Weapons of Order", "Use " + "Weapons of Order", WeaponsofOrderList, "How to use Weapons of Order", "Covenant Kyrian", 0);
            //Necrolords
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Covenant Necrolord", 5);
            CombatRoutine.AddProp(BonedustBrew, "Use " + BonedustBrew, BonedustBrewList, "How to use Bonedust Brew", "Covenant Necrolord", 0);
            //Nigh Fae
            CombatRoutine.AddProp(FaelineStomp, "Use " + FaelineStomp, FaelineStompList, "How to use Faeline Stomp", "Covenant Night Fae", 0);
            //Venthyr 
            CombatRoutine.AddProp(FallenOrder, "Use " + FallenOrder, FallenOrderList, "How to use Fallen Order", "Covenant Venthyr", 0);


            //Spells
            CombatRoutine.AddSpell(TigerPalm, "D1");
            CombatRoutine.AddSpell(BlackOutKick, "D2");
            CombatRoutine.AddSpell(SpinningCraneKick, "D3");
            CombatRoutine.AddSpell(SpearHandStrike, "D4");
            CombatRoutine.AddSpell(FistsofFury, "D5");
            CombatRoutine.AddSpell(FistsoftheWhiteTiger, "D6");
            CombatRoutine.AddSpell(WhirlingDragonPunch, "D7");
            CombatRoutine.AddSpell(TouchofDeath, "D7");
            CombatRoutine.AddSpell(ChiWave, "D7");
            CombatRoutine.AddSpell(StormEarthandFire, "OemOpenBrackets");
            CombatRoutine.AddSpell(Serenity, "OemOpenBrackets");

            CombatRoutine.AddSpell(WeaponsofOrder, "Oem6");
            CombatRoutine.AddSpell(BonedustBrew, "Oem6");
            CombatRoutine.AddSpell(Fleshcraft, "OemOpenBrackets");
            CombatRoutine.AddSpell(FaelineStomp, "Oem6");
            CombatRoutine.AddSpell(FallenOrder, "Oem6");

            CombatRoutine.AddSpell(ChiBurst, "D9");
            CombatRoutine.AddSpell(RisingSunKick, "D0");
            CombatRoutine.AddSpell(RushingJadeWind, "Oem6");

            CombatRoutine.AddSpell(Vivify, "NumPad1");
            CombatRoutine.AddSpell(ExpelHarm, "NumPad2");
            CombatRoutine.AddSpell(EnergizingElixir, "NumPad3");
            CombatRoutine.AddSpell(DampenHarm, "F1");
            CombatRoutine.AddSpell(FortifyingBrew, "F2");
            CombatRoutine.AddSpell(InvokeXuen, "F3");



            //Buffs
            CombatRoutine.AddBuff("Blackout Kick!");
            CombatRoutine.AddBuff("Dance of Chi-Ji");
            CombatRoutine.AddBuff("Storm Earth and Fire");
            CombatRoutine.AddBuff("Serenity");

            //Debuffs
            CombatRoutine.AddDebuff("Mark of the Crane");


        }

        public override void Pulse()
        {

        }
        public override void CombatPulse()
        {
            //COOLDOWNS
            //Covenant Kyrian
            //WeaponsofOrder
            if (API.CanCast(WeaponsofOrder) && IsCooldowns && Covenant == "Kyrian" && UseWeaponsofOrder == "with Cooldowns")
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            //Covenant Necrolord
            //BonedustBrew
            if (API.CanCast(BonedustBrew) && IsCooldowns && Covenant == "Necrolord" && UseBonedustBrew == "with Cooldowns")
            {
                API.CastSpell(BonedustBrew);
                return;
            }
            //Covenant Night Fae
            //FaelineStomp
            if (API.CanCast(FaelineStomp) && IsCooldowns && Covenant == "Night Fae" && UseFaelineStomp == "with Cooldowns")
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            //Covenant Venthyr
            //Fallen Order
            if (API.CanCast(FallenOrder) && IsCooldowns && Covenant == "Venthyr" && UseFallenOrder == "with Cooldowns")
            {
                API.CastSpell(FallenOrder);
                return;
            }
            //InvokeXuen
            if (!API.SpellISOnCooldown(InvokeXuen) && PlayerLevel >= 42 && (UseInvokeXuen == "with Cooldowns"))
            {
                API.CastSpell(InvokeXuen);
                return;
            }
            //InvokeXuen
            if (!API.SpellISOnCooldown(InvokeXuen) && PlayerLevel >= 42 && (UseInvokeXuen == "always"))
            {
                API.CastSpell(InvokeXuen);
                return;
            }
            //Touch of Death
            if (IsCooldowns && !API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 0 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10 && (UseTouchofDeath == "with Cooldowns"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //Touch of Death
            if (!API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 0 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10 && (UseTouchofDeath == "always"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //StormEarthandFire
            if (IsCooldowns && !API.SpellISOnCooldown(StormEarthandFire) && !TalentSerenty && API.SpellCharges(StormEarthandFire) >= 1 && !API.PlayerHasBuff(StormEarthandFire) && IsMelee && API.PlayerCurrentChi >= 3 && PlayerLevel >= 27)
            {
                API.CastSpell(StormEarthandFire);
                return;
            }
            //EnergizingElixir
            if (IsCooldowns && !API.SpellISOnCooldown(EnergizingElixir) && TalentEnergizingElixir && IsMelee && API.PlayerCurrentChi <= 2 && API.PlayerCurrentChi < 50)
            {
                API.CastSpell(EnergizingElixir);
                return;
            }

            //AOE
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber)
            {
                //Covenant Kyrian
                //WeaponsofOrder
                if (API.CanCast(WeaponsofOrder) && Covenant == "Kyrian" && (UseWeaponsofOrder == "AOE" || UseWeaponsofOrder == "always"))
                {
                    API.CastSpell(WeaponsofOrder);
                    return;
                }
                //Covenant Necrolord
                //BonedustBrew
                if (API.CanCast(BonedustBrew) && Covenant == "Necrolord" && (UseBonedustBrew == "AOE" || UseBonedustBrew == "always"))
                {
                    API.CastSpell(BonedustBrew);
                    return;
                }
                //Covenant Night Fae
                //FaelineStomp
                if (API.CanCast(FaelineStomp) && Covenant == "Night Fae" && (UseFaelineStomp == "AOE" || UseFaelineStomp == "always"))
                {
                    API.CastSpell(FaelineStomp);
                    return;
                }
                //Covenant Venthyr
                //Fallen Order
                if (API.CanCast(FallenOrder) && Covenant == "Venthyr" && (UseFallenOrder == "AOE" || UseFallenOrder == "always"))
                {
                    API.CastSpell(FallenOrder);
                    return;
                }
                //WhirlingDragonPunch
                if (API.CanCast(WhirlingDragonPunch) && TalentWhirlingDragonPunch && API.SpellCDDuration(FistsofFury) > 50 && API.SpellCDDuration(RisingSunKick) > 50 && NotChanneling && IsMelee)
                {
                    API.CastSpell(WhirlingDragonPunch);
                    return;
                }
                //Rising Sun Kick
                if (API.CanCast(RisingSunKick) && NotChanneling && API.PlayerCurrentChi >= 5 && API.PlayerLevel >= 10 && IsMelee && NotChanneling)
                {
                    API.CastSpell(RisingSunKick);
                    return;
                }
                //Fists of Fury
                if (API.CanCast(FistsofFury) && API.PlayerCurrentChi >= 3 && API.PlayerLevel >= 12 && IsMelee && NotChanneling)
                {
                    API.CastSpell(FistsofFury);
                    return;
                }
                //Rising Sun Kick
                if (API.CanCast(RisingSunKick) && NotChanneling && API.PlayerCurrentChi >= 2 && API.PlayerLevel >= 10 && IsMelee && NotChanneling)
                {
                    API.CastSpell(RisingSunKick);
                    return;
                }
                //Fist of The White Tiger
                if (API.CanCast(FistsoftheWhiteTiger) && TalentFistoftheWhiteTiger && NotChanneling && API.PlayerCurrentChi <= 2 && API.PlayerEnergy >= 40)
                {
                    API.CastSpell(FistsoftheWhiteTiger);
                    return;
                }
                //Spinnging Crane Kick
                if (API.CanCast(SpinningCraneKick) && NotChanneling && IsMelee && PlayerLevel >=7 && API.PlayerCurrentChi >=2 && API.TargetHasDebuff(MarkoftheCrane) && API.PlayerHasBuff(DanceofChiJi))
                {
                    API.CastSpell(SpinningCraneKick);
                    return;
                }
                //Rushing Jadewind
                if (API.CanCast(RushingJadeWind) && NotChanneling && IsMelee && TalentRushingJadeWind && API.PlayerCurrentChi >=1)
                {
                    API.CastSpell(RushingJadeWind);
                    return;
                }
                //BlackOutKick
                if (API.CanCast(BlackOutKick) && !LastBlackoutkick && NotChanneling && API.PlayerCurrentChi >= 3 && API.PlayerLevel >= 2 && NotChanneling)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //BlackOutKick with buff
                if (API.CanCast(BlackOutKick) && NotChanneling && IsMelee && API.PlayerHasBuff(BlackOutKickBuff) && API.PlayerLevel >= 2)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //Chiwave
                if (API.CanCast(ChiWave) && NotChanneling && TalentChiWave)
                {
                    API.CastSpell(ChiWave);
                    return;
                }
                //ChiBurst
                if (API.CanCast(ChiBurst) && TalentChiBurst && NotChanneling && IsMelee && NotMoving && API.PlayerCurrentChi < 5)
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                //Tiger Palm
                if (API.CanCast(TigerPalm) && !LastTigerPalm && NotChanneling && IsMelee && API.PlayerEnergy >= 50 && API.PlayerCurrentChi <= 3)
                {
                    API.CastSpell(TigerPalm);
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
            //Expel Harm
            if (API.PlayerHealthPercent <= DampenHarmLifePercentProc && !API.SpellISOnCooldown(DampenHarm) && !API.PlayerIsMounted && TalentDampenHarm && NotChanneling)
            {
                API.CastSpell(DampenHarm);
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
            //Covenant Kyrian
            //WeaponsofOrder
            if (API.CanCast(WeaponsofOrder) && Covenant == "Kyrian" && UseWeaponsofOrder == "always")
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            //Covenant Necrolord
            //BonedustBrew
            if (API.CanCast(BonedustBrew) && Covenant == "Necrolord" && UseBonedustBrew == "always")
            {
                API.CastSpell(BonedustBrew);
                return;
            }
            //Covenant Night Fae
            //FaelineStomp
            if (API.CanCast(FaelineStomp) && Covenant == "Night Fae" && UseFaelineStomp == "always")
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            //Covenant Venthyr
            //Fallen Order
            if (API.CanCast(FallenOrder) && Covenant == "Venthyr" && UseFaelineStomp == "always")
            {
                API.CastSpell(FallenOrder);
                return;
            }
            //WhirlingDragonPunch
            if (API.CanCast(WhirlingDragonPunch) && TalentWhirlingDragonPunch && API.SpellCDDuration(FistsofFury) > 50 && API.SpellCDDuration(RisingSunKick) > 50 && NotChanneling && IsMelee)
            {
                API.CastSpell(WhirlingDragonPunch);
                return;
            }
            //Rising Sun Kick
            if (API.CanCast(RisingSunKick) && NotChanneling && API.PlayerCurrentChi >= 5 && API.PlayerLevel >= 10 && IsMelee && NotChanneling)
            {
                API.CastSpell(RisingSunKick);
                return;
            }
            //Fists of Fury
            if (API.CanCast(FistsofFury) && API.PlayerCurrentChi >= 3 && API.PlayerLevel >= 12 && IsMelee && NotChanneling)
            {
                API.CastSpell(FistsofFury);
                return;
            }
            //Rising Sun Kick
            if (API.CanCast(RisingSunKick) && NotChanneling && API.PlayerCurrentChi >= 2 && API.PlayerLevel >= 10 && IsMelee && NotChanneling)
            {
                API.CastSpell(RisingSunKick);
                return;
            }
            //Fist of The White Tiger
            if (API.CanCast(FistsoftheWhiteTiger) && TalentFistoftheWhiteTiger && NotChanneling && API.PlayerCurrentChi <= 2 && API.PlayerEnergy >=40)
            {
                API.CastSpell(FistsoftheWhiteTiger);
                return;
            }
            //BlackOutKick
            if (API.CanCast(BlackOutKick) && !LastBlackoutkick && NotChanneling && API.PlayerCurrentChi >= 3 && API.PlayerLevel >= 2 && NotChanneling)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //BlackOutKick with buff
            if (API.CanCast(BlackOutKick) && NotChanneling && IsMelee && API.PlayerHasBuff(BlackOutKickBuff) && API.PlayerLevel >= 2)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //Chiwave
            if (API.CanCast(ChiWave) && NotChanneling && TalentChiWave)
            {
                API.CastSpell(ChiWave);
                return;
            }
            //ChiBurst
            if (API.CanCast(ChiBurst) && TalentChiBurst && NotChanneling && IsMelee && NotMoving && API.PlayerCurrentChi < 5)
            {
                API.CastSpell(ChiBurst);
                return;
            }   
            //Tiger Palm
            if (API.CanCast(TigerPalm) && !LastTigerPalm && NotChanneling && IsMelee && API.PlayerEnergy >= 50 && API.PlayerCurrentChi <= 3)
            {
                API.CastSpell(TigerPalm);
                return;
            }

            //Serenity ROTATION
            if (TalentSerenty)
            {
                //Serenty
                if (API.CanCast(Serenity) && IsMelee && NotChanneling)
                {
                    API.CastSpell(Serenity);
                    return;
                }
                if (API.PlayerHasBuff(Serenity))
                {
                    //Fists of Fury
                    if (API.CanCast(FistsofFury) && IsMelee && NotChanneling)
                    {
                        API.CastSpell(FistsofFury);
                        return;
                    }
                    //Rising Sun Kick
                    if (API.CanCast(RisingSunKick) && NotChanneling && IsMelee)
                    {
                        API.CastSpell(RisingSunKick);
                        return;
                    }
                    //BlackOutKick
                    if (API.CanCast(BlackOutKick) && !LastBlackoutkick && NotChanneling && IsMelee)
                    {
                        API.CastSpell(BlackOutKick);
                        return;
                    }
                }

                if (!API.PlayerHasBuff(Serenity))
                {
                    //Covenant Kyrian
                    //WeaponsofOrder
                    if (API.CanCast(WeaponsofOrder) && Covenant == "Kyrian" && UseWeaponsofOrder == "always")
                    {
                        API.CastSpell(WeaponsofOrder);
                        return;
                    }
                    //Covenant Necrolord
                    //BonedustBrew
                    if (API.CanCast(BonedustBrew) && Covenant == "Necrolord" && UseBonedustBrew == "always")
                    {
                        API.CastSpell(BonedustBrew);
                        return;
                    }
                    //Covenant Night Fae
                    //FaelineStomp
                    if (API.CanCast(FaelineStomp) && Covenant == "Night Fae" && UseFaelineStomp == "always")
                    {
                        API.CastSpell(FaelineStomp);
                        return;
                    }
                    //Covenant Venthyr
                    //Fallen Order
                    if (API.CanCast(FallenOrder) && Covenant == "Venthyr" && UseFaelineStomp == "always")
                    {
                        API.CastSpell(FallenOrder);
                        return;
                    }
                    //WhirlingDragonPunch
                    if (API.CanCast(WhirlingDragonPunch) && TalentWhirlingDragonPunch && API.SpellCDDuration(FistsofFury) > 50 && API.SpellCDDuration(RisingSunKick) > 50 && NotChanneling && IsMelee)
                    {
                        API.CastSpell(WhirlingDragonPunch);
                        return;
                    }
                    //Rising Sun Kick
                    if (API.CanCast(RisingSunKick) && NotChanneling && API.PlayerCurrentChi >= 5 && API.PlayerLevel >= 10 && IsMelee && NotChanneling)
                    {
                        API.CastSpell(RisingSunKick);
                        return;
                    }
                    //Fists of Fury
                    if (API.CanCast(FistsofFury) && API.PlayerCurrentChi >= 3 && API.PlayerLevel >= 12 && IsMelee && NotChanneling)
                    {
                        API.CastSpell(FistsofFury);
                        return;
                    }
                    //Rising Sun Kick
                    if (API.CanCast(RisingSunKick) && NotChanneling && API.PlayerCurrentChi >= 2 && API.PlayerLevel >= 10 && IsMelee && NotChanneling)
                    {
                        API.CastSpell(RisingSunKick);
                        return;
                    }
                    //Fist of The White Tiger
                    if (API.CanCast(FistsoftheWhiteTiger) && TalentFistoftheWhiteTiger && NotChanneling && API.PlayerCurrentChi <= 2 && API.PlayerEnergy >= 40)
                    {
                        API.CastSpell(FistsoftheWhiteTiger);
                        return;
                    }
                    //BlackOutKick
                    if (API.CanCast(BlackOutKick) && !LastBlackoutkick && NotChanneling && API.PlayerCurrentChi >= 3 && API.PlayerLevel >= 2 && NotChanneling)
                    {
                        API.CastSpell(BlackOutKick);
                        return;
                    }
                    //BlackOutKick with buff
                    if (API.CanCast(BlackOutKick) && NotChanneling && IsMelee && API.PlayerHasBuff(BlackOutKickBuff) && API.PlayerLevel >= 2)
                    {
                        API.CastSpell(BlackOutKick);
                        return;
                    }
                    //Chiwave
                    if (API.CanCast(ChiWave) && NotChanneling && TalentChiWave)
                    {
                        API.CastSpell(ChiWave);
                        return;
                    }
                    //ChiBurst
                    if (API.CanCast(ChiBurst) && TalentChiBurst && NotChanneling && IsMelee && NotMoving && API.PlayerCurrentChi < 5)
                    {
                        API.CastSpell(ChiBurst);
                        return;
                    }
                    //Tiger Palm
                    if (API.CanCast(TigerPalm) && !LastTigerPalm && NotChanneling && IsMelee && API.PlayerEnergy >= 50 && API.PlayerCurrentChi <= 3)
                    {
                        API.CastSpell(TigerPalm);
                        return;
                    }
                }
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
