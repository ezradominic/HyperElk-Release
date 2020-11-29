using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class BrewmasterMonk : CombatRoutine
    {
        //Toggles

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;


        //CLASS SPECIFIC
        //CBProperties
        private int VivifyLifePercentProc => numbList[CombatRoutine.GetPropertyInt(Vivify)];
        private int ExpelHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(ExpelHarm)];
        private int CelestialBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(CelestialBrew)];
        private int FortifyingBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(FortifyingBrew)];
        private int HealingElixirLifePercentProc => numbList[CombatRoutine.GetPropertyInt(HealingElixir)];
        string[] InvokeNiuzaoList = new string[] { "always", "with Cooldowns", "On AOE" };
        string[] StaggerList = new string[] { "always", "Light Stagger", "Moderate Stagger", "Heavy Stagger" };
        string[] TouchofDeathList = new string[] { "always", "with Cooldowns" };

        private string UseInvokeNiuzao => InvokeNiuzaoList[CombatRoutine.GetPropertyInt(InvokeNiuzao)];
        private string UseTouchofDeath => TouchofDeathList[CombatRoutine.GetPropertyInt(TouchofDeath)];
        private string Covenant => CovenantList[CombatRoutine.GetPropertyInt("Covenant")];
        string[] CovenantList = new string[] { "None", "Venthyr", "Night Fae", "Kyrian", "Necrolord" };
        private string UseStagger => StaggerList[CombatRoutine.GetPropertyInt(Stagger)];
        private int PurifyingBrewStaggerPercentProc => CombatRoutine.GetPropertyInt("PurifyingBrewStaggerPercentProc");
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
        //Trinket1
        private string UseTrinket1 => TrinketList1[CombatRoutine.GetPropertyInt(trinket1)];
        string[] TrinketList1 = new string[] { "always", "Cooldowns", "AOE", "never" };
        //Trinket2
        //Trinket1
        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "AOE", "never" };
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };


        //Spells,Buffs,Debuffs
        private string TigerPalm = "Tiger Palm";
        private string BlackOutKick = "Blackout Kick";
        private string Vivify = "Vivify";
        private string SpinningCraneKick = "Spinning Crane Kick";
        private string ExpelHarm = "Expel Harm";
        private string SpearHandStrike = "Spear Hand Strike";
        private string PurifyingBrew = "Purifying Brew";
        private string CelestialBrew = "Celestial Brew";
        private string FortifyingBrew = "Fortifying Brew";
        private string KegSmash = "Keg Smash";
        private string BreathOfFire = "Breath of Fire";
        private string ZenMeditation = "Zen Meditation";
        private string TouchofDeath = "Touch of Death";
        private string BlackOxBrew = "Black Ox Brew";
        private string HealingElixir = "Healing Elixir";
        private string ChiBurst = "Chi Burst";
        private string RushingJadeWind = "Rushing Jade Wind";
        private string InvokeNiuzao = "Invoke Niuzao,  the Black Ox";
        private string ExplodingKeg = "Exploding Keg";
        private string Stagger = "Stagger";
        private string WeaponsofOrder = "Weapons of Order";
        private string BonedustBrew = "Bonedust Brew";
        private string Fleshcraft = "Fleshcraft";
        private string FaelineStomp = "FaelineStomp";
        private string FallenOrder = "Fallen Order";
        private string Detox = "Detox";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        private string LightStagger = "Light Stagger";
        private string ModerateStagger = "Moderate Stagger";
        private string HeavyStagger = "Heavy Stagger";
        public override void Initialize()
        {
            CombatRoutine.Name = "Brewmaster Monk @Mufflon12";
            API.WriteLog("Welcome to Brewmaster Monk rotation @ Mufflon12");

            CombatRoutine.AddProp(Vivify, "Vivify", numbList, "Life percent at which " + Vivify + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(ExpelHarm, "Expel Harm", numbList, "Life percent at which " + ExpelHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 10);
            CombatRoutine.AddProp(CelestialBrew, "Celestial Brew", numbList, "Life percent at which " + CelestialBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 5);
            CombatRoutine.AddProp(FortifyingBrew, "Fortifying Brew", numbList, "Life percent at which " + FortifyingBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 4);
            CombatRoutine.AddProp(HealingElixir, "Healing Elixir", numbList, "Life percent at which " + HealingElixir + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 8);
            CombatRoutine.AddProp("PurifyingBrewStaggerPercentProc", "PurifyingBrew", 4, "Use PurifyingBrew, compared to max life. On 1000 HP 20% means Cast if stagger is higher than 200", "Stagger Management");
            CombatRoutine.AddProp(InvokeNiuzao, "Use " + InvokeNiuzao, InvokeNiuzaoList, "Use " + InvokeNiuzao + "always, with Cooldowns, On AOE", "Cooldowns", 0);
            CombatRoutine.AddProp(Stagger, "Use " + PurifyingBrew, StaggerList, "Use " + PurifyingBrew + " 2nd charge always, Light / Moderate / Heavy Stagger", "Stagger Management", 1);
            CombatRoutine.AddProp(TouchofDeath, "Use " + TouchofDeath, TouchofDeathList, "Use " + TouchofDeath + "always, with Cooldowns", "Cooldowns", 1);
            CombatRoutine.AddProp("Covenant", "Covenant", CovenantList, "Covenant: None, Venthyr, Night Fae, Kyrian, Necrolord", "Covenant Stuff", 0);
            //Kyrian
            CombatRoutine.AddProp("Weapons of Order", "Use " + "Weapons of Order", WeaponsofOrderList, "How to use Weapons of Order", "Covenant Kyrian", 0);
            //Necrolords
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Covenant Necrolord", 5);
            CombatRoutine.AddProp(BonedustBrew, "Use " + BonedustBrew, BonedustBrewList, "How to use Bonedust Brew", "Covenant Necrolord", 0);
            //Nigh Fae
            CombatRoutine.AddProp(FaelineStomp, "Use " + FaelineStomp, FaelineStompList, "How to use Faeline Stomp", "Covenant Night Fae", 0);
            //Venthyr 
            CombatRoutine.AddProp(FallenOrder, "Use " + FallenOrder, FallenOrderList, "How to use Fallen Order", "Covenant Venthyr", 0);

            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", TrinketList1, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", TrinketList2, "When should trinket1 be used", "Trinket", 3);



            //Spells
            CombatRoutine.AddSpell(TigerPalm, "D1");
            CombatRoutine.AddSpell(BlackOutKick, "D2");
            CombatRoutine.AddSpell(SpinningCraneKick, "D3");
            CombatRoutine.AddSpell(SpearHandStrike, "D4");
            CombatRoutine.AddSpell(BreathOfFire, "D5");
            CombatRoutine.AddSpell(KegSmash, "D6");
            CombatRoutine.AddSpell(TouchofDeath, "D7");
            CombatRoutine.AddSpell(InvokeNiuzao, "D8");
            CombatRoutine.AddSpell(ChiBurst, "D9");
            CombatRoutine.AddSpell(RushingJadeWind, "D0");
            CombatRoutine.AddSpell(ExplodingKeg, "Oem6");


            CombatRoutine.AddSpell(Vivify, "NumPad1");
            CombatRoutine.AddSpell(ExpelHarm, "NumPad2");
            CombatRoutine.AddSpell(PurifyingBrew, "NumPad3");
            CombatRoutine.AddSpell(CelestialBrew, "NumPad4");
            CombatRoutine.AddSpell(FortifyingBrew, "NumPad5");
            CombatRoutine.AddSpell(BlackOxBrew, "NumPad6");
            CombatRoutine.AddSpell(HealingElixir, "NumPad7");
            CombatRoutine.AddSpell(Detox, "NumPad8");
            CombatRoutine.AddSpell(WeaponsofOrder, "Oem6");
            CombatRoutine.AddSpell(BonedustBrew, "Oem6");
            CombatRoutine.AddSpell(Fleshcraft, "OemOpenBrackets");
            CombatRoutine.AddSpell(FaelineStomp, "Oem6");
            CombatRoutine.AddSpell(FallenOrder, "Oem6");

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);


            //Buffs



            //Debuffs
            CombatRoutine.AddDebuff(LightStagger);
            CombatRoutine.AddDebuff(ModerateStagger);
            CombatRoutine.AddDebuff(HeavyStagger);


        }

        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if (IsCooldowns && UseTrinket1 == "Cooldowns" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (IsCooldowns && UseTrinket2 == "Cooldowns" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
            //HEALING
            //NECROLORDS FLESHCRAFT
            if (API.CanCast(Fleshcraft) && Covenant == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            //Healing Elixir
            if (API.PlayerHealthPercent <= HealingElixirLifePercentProc && !API.SpellISOnCooldown(HealingElixir) && API.PlayerIsTalentSelected(5, 2))
            {
                API.CastSpell(HealingElixir);
                return;
            }
            //Expel Harm
            if (API.PlayerHealthPercent <= ExpelHarmLifePercentProc && !API.SpellISOnCooldown(ExpelHarm) && !API.PlayerIsMounted && API.PlayerEnergy > 30 && PlayerLevel >= 8)
            {
                API.CastSpell(ExpelHarm);
                return;
            }
            //Celestial Brew
            if (API.PlayerHealthPercent <= CelestialBrewLifePercentProc && !API.SpellISOnCooldown(CelestialBrew) && PlayerLevel >= 27)
            {
                API.CastSpell(CelestialBrew);
                return;
            }
            //Purifying Brew
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 23 && PlayerLevel <= 47)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && API.SpellCharges(PurifyingBrew) >= 2)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "always"))
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Light stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Light Stagger") && API.PlayerHasDebuff(LightStagger))
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Moderate stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Moderate Stagger") && API.PlayerHasDebuff(ModerateStagger))
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Heavy stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Heavy Stagger") && API.PlayerHasDebuff(HeavyStagger))
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Fortifying Brew
            if (API.PlayerHealthPercent <= FortifyingBrewLifePercentProc && !API.SpellISOnCooldown(FortifyingBrew) && PlayerLevel >= 28)
            {
                API.CastSpell(FortifyingBrew);
                return;
            }
            //Vivify
            if (API.PlayerHealthPercent <= VivifyLifePercentProc && API.CanCast(Vivify) && PlayerLevel >= 4)
            {
                API.CastSpell(Vivify);
                return;
            }
            //COOLDOWNN
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
            //BlackOxBrew
            if (API.SpellISOnCooldown(CelestialBrew) && !API.SpellISOnCooldown(BlackOxBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && API.PlayerIsTalentSelected(3, 3))
            {
            API.CastSpell(BlackOxBrew);
            return;
            }
            //Touch of Death
            if (IsCooldowns && !API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 0 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10 && (UseTouchofDeath == "with Cooldowns"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //KICK
            if (isInterrupt && !API.SpellISOnCooldown(SpearHandStrike) && IsMelee && PlayerLevel >= 18)
            {
                API.CastSpell(SpearHandStrike);
                return;
            }
            rotation();
            return;
        }
        private void rotation()
        {
            //ROTATION AOE
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber)
            {
                if (UseTrinket1 == "AOE" || UseTrinket1 == "always" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                    API.CastSpell(trinket1);
                if (UseTrinket2 == "AOE" || UseTrinket1 == "always" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                    API.CastSpell(trinket2);
                //Tiger Palm -> nothing else to do 
                if (API.CanCast(TigerPalm) && API.PlayerEnergy >= 40 && API.SpellCharges(PurifyingBrew) < 2)
                {
                    API.CastSpell(TigerPalm);
                    return;
                }
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
                //Rushing Jade Wind
                if (API.CanCast(RushingJadeWind) && !API.SpellISOnCooldown(RushingJadeWind) && API.PlayerIsTalentSelected(6, 2))
                {
                    API.CastSpell(RushingJadeWind);
                    return;
                }
                //InvokeNiuzao
                if (!API.SpellISOnCooldown(InvokeNiuzao) && API.PlayerLevel >= 42 && (UseInvokeNiuzao == "always" || UseInvokeNiuzao == "On AOE") || UseInvokeNiuzao == "with Cooldowns" && IsCooldowns)
                {
                    API.CastSpell(InvokeNiuzao);
                    return;
                }

                //KegSmash
                if (!API.SpellISOnCooldown(KegSmash) && API.PlayerEnergy > 40 && API.PlayerLevel >= 21)
                {
                    API.CastSpell(KegSmash);
                    return;
                }
                //BlackOutKick
                if (API.CanCast(BlackOutKick) && API.PlayerLevel >= 2)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //BlackOutKick
                if (API.CanCast(BlackOutKick) && API.PlayerLevel >= 2)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //Breath of Fire
                if (!API.SpellISOnCooldown(BreathOfFire) && API.PlayerLevel >= 29)
                {
                    API.CastSpell(BreathOfFire);
                    return;
                }
                //ChiBurst
                if (API.CanCast(ChiBurst) && API.PlayerIsTalentSelected(1, 3))
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                //Exploping Kek
                if (API.CanCast(ExplodingKeg) && API.PlayerIsTalentSelected(6, 3))
                {
                    API.CastSpell(ExplodingKeg);
                    return;
                }
                //Tiger Palm -> nothing else to do 
                if (API.CanCast(TigerPalm) && API.PlayerEnergy >= 40)
                {
                    API.CastSpell(TigerPalm);
                    return;
                }
                //Spinning Crane Kick
                if (API.CanCast(SpinningCraneKick) && API.PlayerEnergy >= 40 && API.PlayerEnergy > 40 && API.PlayerLevel >= 7)
                {
                    API.CastSpell(SpinningCraneKick);
                    return;
                }
            }
            //ROTATION  SINGLE TARGET
            if ((API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE))
            {
                if (UseTrinket1 == "always" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                    API.CastSpell(trinket1);
                if (UseTrinket1 == "always" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                    API.CastSpell(trinket2);
                //Tiger Palm -> nothing else to do 
                if (API.CanCast(TigerPalm) && API.PlayerEnergy >= 40 && API.SpellCharges(PurifyingBrew) < 2)
                {
                    API.CastSpell(TigerPalm);
                    return;
                }
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
                //Touch of Death
                if (!API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 0 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10 && (UseTouchofDeath == "always"))
                {
                    API.CastSpell(TouchofDeath);
                    return;
                }
                //InvokeNiuzao
                if (!API.SpellISOnCooldown(InvokeNiuzao) && API.PlayerLevel >= 42 && (UseInvokeNiuzao == "always" || UseInvokeNiuzao == "with Cooldowns" && IsCooldowns))
                {
                    API.CastSpell(InvokeNiuzao);
                    return;
                }
                //Keg Smash
                if (!API.SpellISOnCooldown(KegSmash) && API.PlayerEnergy > 40 && API.PlayerLevel >= 21)
                {
                    API.CastSpell(KegSmash);
                    return;
                }
                //Blackout Kick
                if (API.CanCast(BlackOutKick) && API.PlayerLevel >= 2)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //Breath of Fire
                if (!API.SpellISOnCooldown(BreathOfFire) && API.PlayerLevel >= 29)
                {
                    API.CastSpell(BreathOfFire);
                    return;
                }
                //ChiBurst
                if (API.CanCast(ChiBurst) && API.PlayerIsTalentSelected(1, 3))
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                //Tiger Palm -> nothing else to do 
                if (API.CanCast(TigerPalm) && API.PlayerEnergy >= 40)
                {
                    API.CastSpell(TigerPalm);
                    return;
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
