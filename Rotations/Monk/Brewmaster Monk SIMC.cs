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
        private bool NotChanneling => !API.PlayerIsChanneling;


        //CLASS SPECIFIC
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        //CBProperties
        private int VivifyLifePercentProc => numbList[CombatRoutine.GetPropertyInt(Vivify)];
        private int ExpelHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(ExpelHarm)];
        private int CelestialBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(CelestialBrew)];
        private int FortifyingBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(FortifyingBrew)];
        private int HealingElixirLifePercentProc => numbList[CombatRoutine.GetPropertyInt(HealingElixir)];
        private int ChiWaveLifePercentProc => numbList[CombatRoutine.GetPropertyInt(HealingElixir)];
        private int DampenHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DampenHarm)];
        private int Stagger1 => API.PlayerMaxHealth / PurifyingBrewStaggerPercentProc ;
        //Talents
        private bool TalentEyeOfTheTiger => API.PlayerIsTalentSelected(1, 1);
        private bool TalentChiWave => API.PlayerIsTalentSelected(1, 2);
        private bool TalentChiBurst => API.PlayerIsTalentSelected(1, 3);

        private bool TalentLightBrewing => API.PlayerIsTalentSelected(3, 1);
        private bool TalentSpitfire => API.PlayerIsTalentSelected(3, 2);
        private bool TalentBlackOxBrew => API.PlayerIsTalentSelected(3, 3);

        private bool TalentSummonBlackOxStatue => API.PlayerIsTalentSelected(4, 2);

        private bool TalentHealingElixir => API.PlayerIsTalentSelected(5, 2);
        private bool TalentDampenHarm => API.PlayerIsTalentSelected(5, 3);

        private bool TalentRushingJadeWind => API.PlayerIsTalentSelected(6, 2);
        private bool TalentExplodingKeg => API.PlayerIsTalentSelected(6, 3);

        private bool TalentBlackoutCombo => API.PlayerIsTalentSelected(7, 3);

        string[] InvokeNiuzaoList = new string[] { "always", "with Cooldowns", "On AOE", "Manual" };
        string[] StaggerList = new string[] { "always", "Light Stagger", "Moderate Stagger", "Heavy Stagger" };
        string[] TouchofDeathList = new string[] { "always", "with Cooldowns", "Manual" };
        private string UseInvokeNiuzao => InvokeNiuzaoList[CombatRoutine.GetPropertyInt(InvokeNiuzao)];
        private string UseTouchofDeath => TouchofDeathList[CombatRoutine.GetPropertyInt(TouchofDeath)];
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
        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "AOE", "never" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        float EnergyRegen => 10f * (1f + API.PlayerGetHaste);
        float TigerPalmMath => API.PlayerEnergy + (EnergyRegen * (API.SpellCDDuration(KegSmash) + API.SpellGCDDuration));
        float CastTigerPalm => TigerPalmMath / 100;
        float SpinningCraneKickMath => API.PlayerEnergy + (EnergyRegen * (API.SpellCDDuration(KegSmash) + API.TargetTimeToExec));
        float CastSpinningCraneKick => SpinningCraneKickMath / 100;
        string[] DetoxList = {"Chilled", "Frozen Binds", "Clinging Darkness", "Rasping Scream", "Heaving Retch", "Goresplatter", "Slime Injection", "Gripping Infection", "Debilitating Plague", "Burning Strain", "Blightbeak", "Corroded Claws", "Wasting Blight", "Hurl Spores", "Corrosive Gunk", "Cytotoxic Slash", "Venompiercer", "Wretched Phlegm", "Bewildering Pollen", "Repulsive Visage", "Soul Split", "Anima Injection", "Bewildering Pollen2", "Bramblethorn Entanglement", "Debilitating Poison", "Sinlight Visions", "Siphon Life", "Turn to Stone", "Stony Veins", "Cosmic Artifice", "Wailing Grief", "Shadow Word:  Pain", "Anguished Cries", "Wrack Soul", "Dark Lance", "Insidious Venom", "Charged Anima", "Lost Confidence", "Burden of Knowledge", "Internal Strife", "Forced Confession", "Insidious Venom2", "Soul Corruption", "Genetic Alteration", "Withering Blight", "Decaying Blight"};
        private static bool CanDetoxPlayer(string debuff)
        {
            return API.PlayerHasDebuff(debuff, false, true);
        }
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
        private string ChiWave = "Chi Wave";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string DampenHarm = "Dampen Harm";
        private string LegSweep = "Leg Sweep";
        private string BlackoutCombo = "Blackout Combo";
        private string Bloodlust = "Bloodlust";
        private string GiftOfTheOx = "Gift of the Ox";
        public override void Initialize()
        {
            CombatRoutine.Name = "Brewmaster Monk @Mufflon12";
            API.WriteLog("Welcome to Brewmaster Monk rotation @ Mufflon12");

            CombatRoutine.AddProp(Vivify, "Vivify", numbList, "Life percent at which " + Vivify + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(ChiWave, "Chi Wave", numbList, "Life percent at which " + ChiWave + " is used, set to 0 to disable", "Healing", 5);

            CombatRoutine.AddProp(ExpelHarm, "Expel Harm", numbList, "Life percent at which " + ExpelHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 90);
            CombatRoutine.AddProp(CelestialBrew, "Celestial Brew", numbList, "Life percent at which " + CelestialBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 50);
            CombatRoutine.AddProp(FortifyingBrew, "Fortifying Brew", numbList, "Life percent at which " + FortifyingBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 40);
            CombatRoutine.AddProp(HealingElixir, "Healing Elixir", numbList, "Life percent at which " + HealingElixir + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 80);
            CombatRoutine.AddProp("PurifyingBrewStaggerPercentProc", "PurifyingBrew", 9, "Use PurifyingBrew, compared to max life.", "Stagger Management");
            CombatRoutine.AddProp(InvokeNiuzao, "Use " + InvokeNiuzao, InvokeNiuzaoList, "Use " + InvokeNiuzao + "always, with Cooldowns, On AOE", "Cooldowns", 0);
            CombatRoutine.AddProp(Stagger, "Use " + PurifyingBrew, StaggerList, "Use " + PurifyingBrew + " 2nd charge always, Light / Moderate / Heavy Stagger", "Stagger Management", 1);
            CombatRoutine.AddProp(TouchofDeath, "Use " + TouchofDeath, TouchofDeathList, "Use " + TouchofDeath + "always, with Cooldowns", "Cooldowns", 1);
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
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(DampenHarm, "Dampen Harm", numbList, "Life percent at which " + DampenHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 40);



            //Spells
            CombatRoutine.AddSpell(TigerPalm, 100780,"D1");
            CombatRoutine.AddSpell(BlackOutKick, 205523,"D2");
            CombatRoutine.AddSpell(SpinningCraneKick, 101546,"D3");
            CombatRoutine.AddSpell(SpearHandStrike, 116705,"D4");
            CombatRoutine.AddSpell(BreathOfFire, 115181,"D5");
            CombatRoutine.AddSpell(KegSmash, 121253,"D6");
            CombatRoutine.AddSpell(TouchofDeath, 322109,"D7");
            CombatRoutine.AddSpell(InvokeNiuzao, 132578,"D8");
            CombatRoutine.AddSpell(ChiBurst, 123986,"D9");
            CombatRoutine.AddSpell(RushingJadeWind, 116847,"D0");
            CombatRoutine.AddSpell(ExplodingKeg, 325153,"Oem6");


            CombatRoutine.AddSpell(Vivify, 116670,"NumPad1");
            CombatRoutine.AddSpell(ChiWave, 115098,"NumPad1");

            CombatRoutine.AddSpell(ExpelHarm, 322101,"NumPad2");
            CombatRoutine.AddSpell(PurifyingBrew, 119582,"NumPad3");
            CombatRoutine.AddSpell(CelestialBrew, 322507,"NumPad4");
            CombatRoutine.AddSpell(FortifyingBrew, 115203,"NumPad5");
            CombatRoutine.AddSpell(BlackOxBrew, 115399,"NumPad6");
            CombatRoutine.AddSpell(HealingElixir, 122281,"NumPad7");
            CombatRoutine.AddSpell(Detox, 115450,"NumPad8");
            CombatRoutine.AddSpell(WeaponsofOrder, 310454,"Oem6");
            CombatRoutine.AddSpell(BonedustBrew, 325216,"Oem6");
            CombatRoutine.AddSpell(Fleshcraft, 324631,"OemOpenBrackets");
            CombatRoutine.AddSpell(FaelineStomp, 327104,"Oem6");
            CombatRoutine.AddSpell(FallenOrder, 326860,"Oem6");
            CombatRoutine.AddSpell(DampenHarm, 122278, "F1");
            CombatRoutine.AddSpell(LegSweep, 119381);
            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);

            //Buffs
            CombatRoutine.AddBuff(BlackoutCombo, 196736);
            CombatRoutine.AddBuff(WeaponsofOrder, 310454);
            CombatRoutine.AddBuff(RushingJadeWind, 116847);
            CombatRoutine.AddBuff(Bloodlust, 2825);
            CombatRoutine.AddBuff(GiftOfTheOx, 124507);

            //Debuffs
            CombatRoutine.AddDebuff(LightStagger, 124275);
            CombatRoutine.AddDebuff(ModerateStagger, 124274);
            CombatRoutine.AddDebuff(HeavyStagger, 124273);
            //Debuffs / Detox
            CombatRoutine.AddDebuff("Chilled", 328664);
            CombatRoutine.AddDebuff("Frozen Binds", 320788);
            CombatRoutine.AddDebuff("Clinging Darkness", 323347);
            CombatRoutine.AddDebuff("Rasping Scream", 324293);
            CombatRoutine.AddDebuff("Heaving Retch", 320596);
            CombatRoutine.AddDebuff("Goresplatter", 338353);
            CombatRoutine.AddDebuff("Slime Injection", 329110);
            CombatRoutine.AddDebuff("Gripping Infection", 328180);
            CombatRoutine.AddDebuff("Debilitating Plague", 324652);
            CombatRoutine.AddDebuff("Burning Strain", 322358);
            CombatRoutine.AddDebuff("Blightbeak", 327882);
            CombatRoutine.AddDebuff("Corroded Claws", 320512);
            CombatRoutine.AddDebuff("Wasting Blight", 320542);
            CombatRoutine.AddDebuff("Hurl Spores", 328002);
            CombatRoutine.AddDebuff("Corrosive Gunk", 319070);
            CombatRoutine.AddDebuff("Cytotoxic Slash", 325552);
            CombatRoutine.AddDebuff("Venompiercer", 328395);
            CombatRoutine.AddDebuff("Wretched Phlegm", 334926);
            CombatRoutine.AddDebuff("Bewildering Pollen", 323137);
            CombatRoutine.AddDebuff("Repulsive Visage", 328756);
            CombatRoutine.AddDebuff("Soul Split", 322557);
            CombatRoutine.AddDebuff("Anima Injection", 325224);
            CombatRoutine.AddDebuff("Bewildering Pollen2", 321968);
            CombatRoutine.AddDebuff("Bramblethorn Entanglement", 324859);
            CombatRoutine.AddDebuff("Debilitating Poison", 326092);
            CombatRoutine.AddDebuff("Sinlight Visions", 339237);
            CombatRoutine.AddDebuff("Siphon Life", 325701);
            CombatRoutine.AddDebuff("Turn to Stone", 326607);
            CombatRoutine.AddDebuff("Stony Veins", 326632);
            CombatRoutine.AddDebuff("Cosmic Artifice", 325725);
            CombatRoutine.AddDebuff("Wailing Grief", 340026);
            CombatRoutine.AddDebuff("Shadow Word:  Pain", 332707);
            CombatRoutine.AddDebuff("Anguished Cries", 325885);
            CombatRoutine.AddDebuff("Wrack Soul", 321038);
            CombatRoutine.AddDebuff("Dark Lance", 327481);
            CombatRoutine.AddDebuff("Insidious Venom", 323636);
            CombatRoutine.AddDebuff("Charged Anima", 338731);
            CombatRoutine.AddDebuff("Lost Confidence", 322818);
            CombatRoutine.AddDebuff("Burden of Knowledge", 317963);
            CombatRoutine.AddDebuff("Internal Strife", 327648);
            CombatRoutine.AddDebuff("Forced Confession", 328331);
            CombatRoutine.AddDebuff("Insidious Venom2", 317661);
            CombatRoutine.AddDebuff("Soul Corruption", 333708);
            CombatRoutine.AddDebuff("Genetic Alteration", 320248);
            CombatRoutine.AddDebuff("Withering Blight", 341949);
            CombatRoutine.AddDebuff("Decaying Blight", 330700);
            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

        }

        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if (API.CanCast(Detox))
            {
                for (int i = 0; i < DetoxList.Length; i++)
                {
                    if (CanDetoxPlayer(DetoxList[i]))
                    {
                        API.CastSpell(Detox);
                        return;
                    }
                }
            }
            if (API.PlayerItemCanUse("Healthstone") && API.PlayerItemRemainingCD("Healthstone") == 0 && API.PlayerHealthPercent <= HealthStonePercent)
            {
                API.CastSpell("Healthstone");
                return;
            }
            //HEALING
            if (API.PlayerItemCanUse(PhialofSerenity) && API.PlayerItemRemainingCD(PhialofSerenity) == 0 && API.PlayerHealthPercent <= PhialofSerenityLifePercent)
            {
                API.CastSpell(PhialofSerenity);
                return;
            }
            if (API.PlayerItemCanUse(SpiritualHealingPotion) && API.PlayerItemRemainingCD(SpiritualHealingPotion) == 0 && API.PlayerHealthPercent <= SpiritualHealingPotionLifePercent)
            {
                API.CastSpell(SpiritualHealingPotion);
                return;
            }
            if (API.PlayerHealthPercent <= DampenHarmLifePercentProc && !API.SpellISOnCooldown(DampenHarm) && !API.PlayerIsMounted && TalentDampenHarm && NotChanneling)
            {
                API.CastSpell(DampenHarm);
                return;
            }
            //NECROLORDS FLESHCRAFT
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotChanneling)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            //Expel Harm
            if (API.PlayerHealthPercent <= ExpelHarmLifePercentProc && !API.SpellISOnCooldown(ExpelHarm) && !API.PlayerIsMounted && API.PlayerEnergy > 30 && PlayerLevel >= 8 && NotChanneling)
            {
                API.CastSpell(ExpelHarm);
                return;
            }


            //Purifying Brew
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 23 && PlayerLevel <= 47 && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && API.SpellCharges(PurifyingBrew) >= 2 && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "always") && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Light stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Light Stagger") && API.PlayerHasDebuff(LightStagger) && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Moderate stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Moderate Stagger") && API.PlayerHasDebuff(ModerateStagger) && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Heavy stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Heavy Stagger") && API.PlayerHasDebuff(HeavyStagger) && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Fortifying Brew
            if (API.PlayerHealthPercent <= FortifyingBrewLifePercentProc && !API.SpellISOnCooldown(FortifyingBrew) && PlayerLevel >= 28 && NotChanneling)
            {
                API.CastSpell(FortifyingBrew);
                return;
            }
            //Vivify
            if (API.PlayerHealthPercent <= VivifyLifePercentProc && API.CanCast(Vivify) && PlayerLevel >= 4 && NotChanneling)
            {
                API.CastSpell(Vivify);
                return;
            }
            //COOLDOWNN
            if (IsCooldowns && UseTrinket1 == "Cooldowns" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (IsCooldowns && UseTrinket2 == "Cooldowns" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
            //Touch of Death
            if (IsCooldowns && !API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 50 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10 && (UseTouchofDeath == "with Cooldowns"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //actions+=/spear_hand_strike,if=target.debuff.casting.react
            if (isInterrupt && !API.SpellISOnCooldown(SpearHandStrike) && IsMelee && PlayerLevel >= 18)
            {
                API.CastSpell(SpearHandStrike);
                return;
            }
            if (isInterrupt && API.SpellISOnCooldown(SpearHandStrike) && API.CanCast(LegSweep) && IsMelee && PlayerLevel >= 18)
            {
                API.CastSpell(LegSweep);
                return;
            }
            rotation();
            return;
        }
        private void rotation()
        {
            //# Executed every time the actor is available.
            //actions=auto_attack
            //actions+=/gift_of_the_ox,if=health<health.max*0.65

            //actions+=/dampen_harm,if=incoming_damage_1500ms&buff.fortifying_brew.down
            //actions+=/fortifying_brew,if=incoming_damage_1500ms&(buff.dampen_harm.down|buff.diffuse_magic.down)
            //actions+=/use_item,name=dreadfire_vessel
            //actions+=/potion
            //actions+=/blood_fury
            if (IsCooldowns && API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions+=/berserking
            if (IsCooldowns && API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions+=/lights_judgment
            //actions+=/fireblood
            if (IsCooldowns && API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions+=/ancestral_call
            if (IsCooldowns && API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions+=/bag_of_tricks
            if (IsCooldowns && API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Vulpera")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions+=/invoke_niuzao_the_black_ox,if=target.time_to_die>25
            if (API.CanCast(InvokeNiuzao) && (API.TargetTimeToDie > 2500 && UseInvokeNiuzao == "always" || UseInvokeNiuzao == "with Cooldowns" && IsCooldowns))
            {
                API.CastSpell(InvokeNiuzao);
                return;
            }
            //actions+=/touch_of_death,if=target.health.pct<=15
            if (API.CanCast(TouchofDeath) && API.TargetHealthPercent <= 15 && (UseTouchofDeath == "with Cooldowns" && IsCooldowns || UseTouchofDeath == "always"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //actions+=/weapons_of_order
            if (API.CanCast(WeaponsofOrder) && PlayerCovenantSettings == "Kyrian" && (API.TargetHealthPercent <= 15 && UseWeaponsofOrder == "with Cooldowns" && IsCooldowns || UseWeaponsofOrder == "always"))
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            //actions+=/fallen_order
            if (API.CanCast(FallenOrder) && PlayerCovenantSettings == "Venthyr" && (UseFallenOrder == "always" || UseFallenOrder == "with Cooldowns" && IsCooldowns))
            {
                API.CastSpell(FallenOrder);
                return;
            }
            //actions+=/bonedust_brew
            if (API.CanCast(BonedustBrew) && PlayerCovenantSettings == "Necrolord" && (UseBonedustBrew == "always" || UseBonedustBrew == "with Cooldowns" && IsCooldowns))
            {
                API.CastSpell(BonedustBrew);
                return;
            }
            //actions+=/purifying_brew
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //# Black Ox Brew is currently used to either replenish brews based on less than half a brew charge available, or low energy to enable Keg Smash
            //actions+=/black_ox_brew,if=cooldown.purifying_brew.charges_fractional<0.5
            if (API.CanCast(BlackOxBrew) && TalentBlackOxBrew && API.SpellCharges(PurifyingBrew) < 0.5)
            {
                API.CastSpell(BlackOxBrew);
                return;
            }
            //actions+=/black_ox_brew,if=(energy+(energy.regen*cooldown.keg_smash.remains))<40&buff.blackout_combo.down&cooldown.keg_smash.up
            if (API.CanCast(BlackOxBrew) && TalentBlackOxBrew && (API.PlayerEnergy + (EnergyRegen * API.SpellCDDuration(KegSmash))) < 4000 && !API.PlayerHasBuff(BlackoutCombo))
            {
                API.CastSpell(BlackOxBrew);
                return;
            }
            //# Offensively, the APL prioritizes KS on cleave, BoS else, with energy spenders and cds sorted below
            //actions+=/keg_smash,if=spell_targets>=2
            if (IsAOE && API.CanCast(KegSmash) && API.PlayerUnitInMeleeRangeCount >= 2)
            {
                API.CastSpell(KegSmash);
                return;
            }
            //actions+=/faeline_stomp,if=spell_targets>=2
            if (IsAOE && API.CanCast(FaelineStomp) && PlayerCovenantSettings == "Night Fae" && API.PlayerUnitInMeleeRangeCount >= 2 && (UseFaelineStomp == "AOE" || UseFaelineStomp == "always"))
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            //# cast KS at top prio during WoO buff
            //actions+=/keg_smash,if=buff.weapons_of_order.up
            if (API.CanCast(KegSmash) && API.PlayerHasBuff(WeaponsofOrder))
            {
                API.CastSpell(KegSmash);
                return;
            }
            //# Celestial Brew priority whenever it took significant damage (adjust the health.max coefficient according to intensity of damage taken), and to dump excess charges before BoB.
            //actions+=/celestial_brew,if=buff.blackout_combo.down&incoming_damage_1999ms>(health.max*0.1+stagger.last_tick_damage_4)&buff.elusive_brawler.stack<2
            if (API.PlayerHealthPercent <= CelestialBrewLifePercentProc && !API.SpellISOnCooldown(CelestialBrew) && PlayerLevel >= 27)
            {
                API.CastSpell(CelestialBrew);
                return;
            }
            //actions+=/tiger_palm,if=talent.rushing_jade_wind.enabled&buff.blackout_combo.up&buff.rushing_jade_wind.up
            if (API.CanCast(TigerPalm) && TalentRushingJadeWind && API.PlayerHasBuff(BlackoutCombo) && API.PlayerHasBuff(RushingJadeWind))
            {
                API.CastSpell(TigerPalm);
                return;
            }
            //actions+=/breath_of_fire,if=buff.charred_passions.down&runeforge.charred_passions.equipped
            //actions+=/blackout_kick
            if (API.CanCast(BlackOutKick))
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //actions+=/keg_smash
            if (API.CanCast(KegSmash))
            {
                API.CastSpell(KegSmash);
                return;
            }
            //actions+=/faeline_stomp
            if (IsAOE && API.CanCast(FaelineStomp) && PlayerCovenantSettings == "Night Fae" && UseFaelineStomp == "always")
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            //actions+=/expel_harm,if=buff.gift_of_the_ox.stack>=3
            //actions+=/touch_of_death
            if (!API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 50 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10 && (UseTouchofDeath == "with Cooldowns" && IsCooldowns || UseTouchofDeath == "always"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //actions+=/rushing_jade_wind,if=buff.rushing_jade_wind.down
            if (API.CanCast(RushingJadeWind) && TalentRushingJadeWind && !API.PlayerHasBuff(RushingJadeWind))
            {
                API.CastSpell(RushingJadeWind);
                return;
            }
            //actions+=/spinning_crane_kick,if=buff.charred_passions.up

            //actions+=/breath_of_fire,if=buff.blackout_combo.down&(buff.bloodlust.down|(buff.bloodlust.up&dot.breath_of_fire_dot.refreshable))
            if (API.CanCast(BreathOfFire) && !API.PlayerHasBuff(BlackoutCombo) && !API.PlayerHasBuff(Bloodlust))
            {
                API.CastSpell(BreathOfFire);
                return;
            }
            //actions+=/chi_burst
            if (API.CanCast(ChiBurst) && TalentChiBurst)
            {
                API.CastSpell(ChiBurst);
                return;
            }
            //actions+=/chi_wave
            if (API.CanCast(ChiWave) && TalentChiWave)
            {
                API.CastSpell(ChiWave);
                return;
            }
            //actions+=/spinning_crane_kick,if=active_enemies>=3&cooldown.keg_smash.remains>gcd&(energy+(energy.regen*(cooldown.keg_smash.remains+execute_time)))>=65&(!talent.spitfire.enabled|!runeforge.charred_passions.equipped)
            if (IsAOE && API.CanCast(SpinningCraneKick) && API.PlayerUnitInMeleeRangeCount >= 3 && API.SpellCDDuration(KegSmash) > API.SpellGCDDuration && CastSpinningCraneKick >= 65)
            {
                API.CastSpell(SpinningCraneKick);
                return;
            }
            //actions+=/tiger_palm,if=!talent.blackout_combo&cooldown.keg_smash.remains>gcd&(energy+(energy.regen*(cooldown.keg_smash.remains+gcd)))>=65
            if (API.CanCast(TigerPalm) && !TalentBlackoutCombo && API.SpellCDDuration(KegSmash) > API.SpellGCDDuration && CastTigerPalm >= 65)
            {
                API.CastSpell(TigerPalm);
                return;
            }
            //actions+=/arcane_torrent,if=energy<31
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Blood Elf" && API.PlayerEnergy < 31)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions+=/rushing_jade_wind
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
