using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class BrewmasterMonk : CombatRoutine
    {
        //Toggles
        private bool IsPause => API.ToggleIsEnabled("Pause");

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

        private int PurifyingBrewStaggerPercentProc => CombatRoutine.GetPropertyInt("PurifyingBrewStaggerPercentProc");




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

        public override void Initialize()
        {
            CombatRoutine.Name = "Brewmaster Monk @Mufflon12";
            API.WriteLog("Welcome to Brewmaster Monk rotation @ Mufflon12");

            CombatRoutine.AddProp(Vivify, "Vivify", numbList, "Life percent at which " + Vivify + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(ExpelHarm, "Expel Harm", numbList, "Life percent at which " + ExpelHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 10);
            CombatRoutine.AddProp(CelestialBrew, "Celestial Brew", numbList, "Life percent at which " + CelestialBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 5);
            CombatRoutine.AddProp(FortifyingBrew, "Fortifying Brew", numbList, "Life percent at which " + FortifyingBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 4);
            CombatRoutine.AddProp(HealingElixir, "Healing Elixir", numbList, "Life percent at which " + HealingElixir + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 8);
            CombatRoutine.AddProp("PurifyingBrewStaggerPercentProc", "PurifyingBrew", 4, "Use PurifyingBrew, compared to max life. On 1000 HP 20% means Cast if stagger is higher than 200", "Healing");


            //Spells
            CombatRoutine.AddSpell("Tiger Palm", "D1");
            CombatRoutine.AddSpell("Blackout Kick", "D2");
            CombatRoutine.AddSpell("Spinning Crane Kick", "D3");
            CombatRoutine.AddSpell("Spear Hand Strike", "D4");
            CombatRoutine.AddSpell("Breath of Fire", "D5");
            CombatRoutine.AddSpell("Keg Smash", "D6");
            CombatRoutine.AddSpell("Touch of Death", "D7");



            CombatRoutine.AddSpell("Vivify", "NumPad1");
            CombatRoutine.AddSpell("Expel Harm", "NumPad2");
            CombatRoutine.AddSpell("Purifying Brew", "NumPad3");
            CombatRoutine.AddSpell("Celestial Brew", "NumPad4");
            CombatRoutine.AddSpell("Fortifying Brew", "NumPad5");
            CombatRoutine.AddSpell("Black Ox Brew", "NumPad6");
            CombatRoutine.AddSpell("Healing Elixir", "NumPad7");

            //Buffs



            //Debuffs



        }

        public override void Pulse()
        {
            //Cooldowns
            if (IsCooldowns)
            {
                //BlackOxBrew
                if (API.SpellISOnCooldown(CelestialBrew) && !API.SpellISOnCooldown(BlackOxBrew) && API.PlayerIsTalentSelected(3, 3))
                {
                    API.CastSpell(BlackOxBrew);
                    return;
                }
                //Touch of Death
                if (!API.SpellISOnCooldown(TouchofDeath) && API.TargetMaxHealth <= API.PlayerMaxHealth && PlayerLevel >= 10)
                {
                    API.CastSpell(TouchofDeath);
                    return;
                }
            }
            //AOE
            if (IsAOE)
            {

            }

            //KICK
            if (isInterrupt && !API.SpellISOnCooldown(SpearHandStrike) && IsMelee && PlayerLevel >= 18)
            {
                API.CastSpell(SpearHandStrike);
                return;
            }
            //Healing Elixir
            if (API.PlayerHealthPercent <= HealingElixirLifePercentProc && !API.SpellISOnCooldown(HealingElixir) && API.PlayerIsTalentSelected(5, 2))
            {
                API.CastSpell(HealingElixir);
                return;
            }
            //Expel Harm
            if (API.PlayerHealthPercent <= ExpelHarmLifePercentProc && !API.SpellISOnCooldown(ExpelHarm) && API.PlayerEnergy > 30 && PlayerLevel >= 8)
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
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 23)
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
        }
        public override void CombatPulse()
        {
            //Keg Smash
            if (!API.SpellISOnCooldown(KegSmash) && API.PlayerEnergy > 40 && API.PlayerLevel >= 21)
            {
                API.CastSpell(KegSmash);
                return;
            }
            //Breath of Fire
            if (!API.SpellISOnCooldown(BreathOfFire) && API.PlayerLevel >= 29)
            {
                API.CastSpell(BreathOfFire);
                return;
            }
            //Blackout Kick
            if (API.CanCast(BlackOutKick) && API.PlayerLevel >= 2)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //Tiger Palm
            if (API.CanCast(TigerPalm) && API.PlayerEnergy >= 50)
            {
                API.CastSpell(TigerPalm);
                return;
            }
            //Spinning Crane Kick
            if (API.CanCast(SpinningCraneKick) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerEnergy > 40 && API.PlayerLevel >= 7)
            {
                API.CastSpell(SpinningCraneKick);
                return;
            }
        }
        public override void OutOfCombatPulse()
        {

        }
    }
}
