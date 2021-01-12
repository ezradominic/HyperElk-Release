using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;


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
        float EnergyReg => 10f * (1f + API.PlayerGetHaste) * 1f;
        float EnergyTimeToMax => (API.PlayeMaxEnergy - API.PlayerEnergy) * 100f / EnergyReg;
        private int ChiDeficit => 5 - API.PlayerCurrentChi;

        //CLASS SPECIFIC
        private bool TalentHitCombo => API.PlayerIsTalentSelected(6, 1);
        private bool TalentChiWave => API.PlayerIsTalentSelected(1, 2);
        private bool TalentChiBurst => API.PlayerIsTalentSelected(1, 3);
        private bool TalentFistoftheWhiteTiger => API.PlayerIsTalentSelected(3, 2);
        private bool TalentEnergizingElixir => API.PlayerIsTalentSelected(3, 3);
        private bool TalentDampenHarm => API.PlayerIsTalentSelected(5, 3);
        private bool TalentRushingJadeWind => API.PlayerIsTalentSelected(6, 2);
        private bool TalentDanceofChiJi => API.PlayerIsTalentSelected(6, 3);
        private bool TalentWhirlingDragonPunch => API.PlayerIsTalentSelected(7, 2);
        private bool TalentSerenty => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private int VivifyLifePercentProc => numbList[CombatRoutine.GetPropertyInt(Vivify)];
        private int FortifyingBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(FortifyingBrew)];
        private int DampenHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DampenHarm)];
        private int TouchofKarmaPercentProc => numbList[CombatRoutine.GetPropertyInt(TouchofKarma)];

        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        private bool NotCasting => !API.PlayerIsCasting(false);
        bool LastCastTigerPalm => API.LastSpellCastInGame == TigerPalm;
        bool LastCastBlackoutkick => API.LastSpellCastInGame == BlackOutKick;
        bool LastCastSpinningCraneKick => API.LastSpellCastInGame == SpinningCraneKick;
        bool LastCastRisingSunKick => API.LastSpellCastInGame == RisingSunKick;
        bool LastCastChiWave => API.LastSpellCastInGame == ChiWave;
        bool CurrenCastFistsOfFury => API.CurrentCastSpellID("player") == 113656;
        private string UseTouchofDeath => TouchofDeathList[CombatRoutine.GetPropertyInt(TouchofDeath)];
        string[] TouchofDeathList = new string[] {"with Cooldowns", "Manual" };
        //Trinket1
        private string UseTrinket1 => TrinketList1[CombatRoutine.GetPropertyInt(trinket1)];
        string[] TrinketList1 = new string[] { "always", "Cooldowns", "AOE", "never" };
        //Trinket2
        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "AOE", "never" };
        private string UseInvokeXuen => InvokeXuenList[CombatRoutine.GetPropertyInt(InvokeXuen)];
        string[] InvokeXuenList = new string[] {"with Cooldowns", "Manual" };
        //Kyrian
        private string UseWeaponsofOrder => WeaponsofOrderList[CombatRoutine.GetPropertyInt(WeaponsofOrder)];
        string[] WeaponsofOrderList = new string[] {"with Cooldowns", "AOE" };
        //Necrolords
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        string[] BonedustBrewList = new string[] {"with Cooldowns" };
        private string UseBonedustBrew => BonedustBrewList[CombatRoutine.GetPropertyInt(BonedustBrew)];
        //Nigh Fae
        string[] FaelineStompList = new string[] {"with Cooldowns" };
        private string UseFaelineStomp => FaelineStompList[CombatRoutine.GetPropertyInt(FaelineStomp)];

        //Venthyr 
        string[] FallenOrderList = new string[] {"with Cooldowns" };
        private string UseFallenOrder => FallenOrderList[CombatRoutine.GetPropertyInt(FallenOrder)];

        public string[] LegendaryList = new string[] { "None", "Last Emperor's Capacitor", "Keefer's Skyreach", "Jade Ignition" };

        private string UseLeg => LegendaryList[CombatRoutine.GetPropertyInt("Legendary")];


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
        private string StormEarthandFire = "Storm,  Earth,  and Fire";
        private string Serenity = "Serenity";
        private string DampenHarm = "Dampen Harm";
        private string EnergizingElixir = "Energizing Elixir";
        private string InvokeXuen = "Invoke Xuen,  the White Tiger";
        private string WeaponsofOrder = "Weapons of Order";
        private string BonedustBrew = "Bonedust Brew";
        private string Fleshcraft = "Fleshcraft";
        private string FaelineStomp = "FaelineStomp";
        private string FallenOrder = "Fallen Order";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string TouchofKarma = "Touch of Karma";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        private string StormEarthAndFire = "Storm,  Earth,  and Fire";
        private string BloodFury = "Blood Fury";
        private string Berserking = "Berserking";
        private string ArcaneTorrent = "Arcane Torrent";
        private string Fireblood = "Fireblood";
        private string CracklingJadeLightning = "Crackling Jade Lightning";
        private string TheEmperorsCapacitor = "The Emperor's Capacitor";
        private string SkyreachExhaustion = "Skyreach Exhaustion";
        private string Stopcast = "Stopcast";
        private string ChiEnergy = "Chi Energy";
        private string CoordinatedOffensive = "Coordinated Offensive";
        private string AncestralCall = "Ancestral Call";
        private string BagOfTricks = "Bag of Tricks";
        public override void Initialize()
        {
            CombatRoutine.Name = "Windwalker Monk @Mufflon12";
            API.WriteLog("Welcome to Windwalker Monk rotation @ Mufflon12");

            CombatRoutine.AddProp(Vivify, "Vivify", numbList, "Life percent at which " + Vivify + " is used, set to 0 to disable", "Healing", 50);
            CombatRoutine.AddProp(FortifyingBrew, "Fortifying Brew", numbList, "Life percent at which " + FortifyingBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 40);
            CombatRoutine.AddProp(TouchofKarma, "Touch of Karma", numbList, "Life percent at which " + TouchofKarma + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 70);
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", TrinketList1, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", TrinketList2, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp(DampenHarm, "Dampen Harm", numbList, "Life percent at which " + DampenHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 40);
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
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp("Legendary", "Select your Legendary", LegendaryList, "Select Your Legendary", "Legendary");


            //Spells

            CombatRoutine.AddSpell(TigerPalm, 100780, "D1");
            CombatRoutine.AddSpell(BlackOutKick, 100784, "D2");
            CombatRoutine.AddSpell(SpinningCraneKick, 101546, "D3");
            CombatRoutine.AddSpell(SpearHandStrike, 116705, "D4");
            CombatRoutine.AddSpell(FistsofFury, 113656,"D5");
            CombatRoutine.AddSpell(FistsoftheWhiteTiger, 261947,"D6");
            CombatRoutine.AddSpell(WhirlingDragonPunch, 152175,"D7");
            CombatRoutine.AddSpell(TouchofDeath, 322109,"D7");
            CombatRoutine.AddSpell(ChiWave, 115098,"D7");
            CombatRoutine.AddSpell(StormEarthandFire, 137639,"OemOpenBrackets");
            CombatRoutine.AddSpell(Serenity, 152173,"OemOpenBrackets");

            CombatRoutine.AddSpell(WeaponsofOrder, 310454, "Oem6");
            CombatRoutine.AddSpell(BonedustBrew, 325216, "Oem6");
            CombatRoutine.AddSpell(Fleshcraft, 324631, "OemOpenBrackets");
            CombatRoutine.AddSpell(FaelineStomp, 327104, "Oem6");
            CombatRoutine.AddSpell(FallenOrder, 326860, "Oem6");

            CombatRoutine.AddSpell(ChiBurst, 123986,"D9");
            CombatRoutine.AddSpell(RisingSunKick, 107428,"D0");
            CombatRoutine.AddSpell(RushingJadeWind, 116847,"Oem6");
            CombatRoutine.AddSpell(CracklingJadeLightning, 172724);
            CombatRoutine.AddSpell(Vivify, 116670,"NumPad1");
            CombatRoutine.AddSpell(ExpelHarm, 322101,"NumPad2");
            CombatRoutine.AddSpell(EnergizingElixir, 115288,"NumPad3");
            CombatRoutine.AddSpell(DampenHarm, 122278,"F1");
            CombatRoutine.AddSpell(FortifyingBrew, 243435,"F2");
            CombatRoutine.AddSpell(InvokeXuen, 123904,"F3");
            CombatRoutine.AddSpell(TouchofKarma, 122470, "NumPad3");
//            CombatRoutine.AddSpell(BloodFury, 20572);
//            CombatRoutine.AddSpell(Berserking, 26297);
//            CombatRoutine.AddSpell(ArcaneTorrent, 28730);
//            CombatRoutine.AddSpell(Fireblood, 265221);
//            CombatRoutine.AddSpell(AncestralCall, 274738);
//            CombatRoutine.AddSpell(BagOfTricks, 312411);

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);
            CombatRoutine.AddMacro(Stopcast);


            //Buffs
            CombatRoutine.AddBuff(BlackOutKickBuff, 116768);
            CombatRoutine.AddBuff(DanceofChiJi, 325202);
            CombatRoutine.AddBuff("Storm,  Earth,  and Fire", 137639);
            CombatRoutine.AddBuff(Serenity, 152173);
            CombatRoutine.AddBuff(WeaponsofOrder, 310454);
            CombatRoutine.AddBuff(RushingJadeWind, 116847);
            CombatRoutine.AddBuff(TheEmperorsCapacitor, 337291);
            CombatRoutine.AddBuff(ChiEnergy, 337571);

            //Debuffs
            CombatRoutine.AddDebuff(MarkoftheCrane, 228287);
            CombatRoutine.AddDebuff(BonedustBrew, 325216);
            CombatRoutine.AddDebuff(SkyreachExhaustion, 337341);
            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Condition
            CombatRoutine.AddConduit(CoordinatedOffensive);


        }

        public override void Pulse()
        {
//            API.WriteLog("ChiDeficit " + ChiDeficit);

        }

        public override void CombatPulse()
        {
            if (IsCooldowns && UseTrinket1 == "Cooldowns" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (IsCooldowns && UseTrinket2 == "Cooldowns" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
            //actions+=/spear_hand_strike,if=target.debuff.casting.react
            if (isInterrupt && !API.SpellISOnCooldown(SpearHandStrike) && IsMelee && PlayerLevel >= 18 && NotChanneling)
            {
                API.CastSpell(SpearHandStrike);
                return;
            }
            if (API.PlayerHealthPercent <= VivifyLifePercentProc && API.CanCast(Vivify) && PlayerLevel >= 4 && NotChanneling)
            {
                API.CastSpell(Vivify);
                return;
            }
            if (API.PlayerHealthPercent <= FortifyingBrewLifePercentProc && !API.SpellISOnCooldown(FortifyingBrew) && PlayerLevel >= 28 && NotChanneling)
            {
                API.CastSpell(FortifyingBrew);
                return;
            }
            if (API.PlayerHealthPercent <= DampenHarmLifePercentProc && !API.SpellISOnCooldown(DampenHarm) && !API.PlayerIsMounted && TalentDampenHarm && NotChanneling)
            {
                API.CastSpell(DampenHarm);
                return;
            }
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
            //actions+=/call_action_list,name=cd_sef,if=!talent.serenity
            if (IsCooldowns && !TalentSerenty)
            {
                Cooldowns();
            }
            //actions+=/call_action_list,name=cd_serenity,if=talent.serenity
            if (IsCooldowns && TalentSerenty)
            {
                CooldownsSerenty();
            }
            //actions+=/call_action_list,name=serenity,if=buff.serenity.up
            if (API.PlayerHasBuff(Serenity))
            {
                SerentyRotation();
            }
            //actions+=/call_action_list,name=weapons_of_order,if=buff.weapons_of_order.up
            if (API.PlayerHasBuff(WeaponsofOrder))
            {
                WeaponsOfOrderRotation();
            }

            //# Executed every time the actor is available.
            //actions=auto_attack
            //actions+=/fist_of_the_white_tiger,target_if=min:debuff.mark_of_the_crane.remains,if=chi.max-chi>=3&(energy.time_to_max<1|energy.time_to_max<4&cooldown.fists_of_fury.remains<1.5|cooldown.weapons_of_order.remains<2)
            if (API.CanCast(FistsoftheWhiteTiger) && TalentFistoftheWhiteTiger && ChiDeficit >= 3 && (EnergyTimeToMax < 1000 || EnergyTimeToMax < 4000 && API.SpellCDDuration(FistsofFury) < 1500 || API.SpellCDDuration(WeaponsofOrder) < 2000) && NotChanneling)
            {
                API.CastSpell(FistsoftheWhiteTiger);
                return;
            }
            //actions+=/expel_harm,if=chi.max-chi>=1&(energy.time_to_max<1|cooldown.serenity.remains<2|energy.time_to_max<4&cooldown.fists_of_fury.remains<1.5|cooldown.weapons_of_order.remains<2)
            if (API.CanCast(ExpelHarm) && ChiDeficit >= 1 && (EnergyTimeToMax < 1000 || API.SpellCDDuration(Serenity) < 2000 || EnergyTimeToMax < 4000 && API.SpellCDDuration(FistsofFury) < 1500 || API.SpellCDDuration(WeaponsofOrder) < 2000) && NotChanneling)
            {
                API.CastSpell(ExpelHarm);
                return;
            }
            //actions+=/tiger_palm,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike&chi.max-chi>=2&(energy.time_to_max<1|cooldown.serenity.remains<2|energy.time_to_max<4&cooldown.fists_of_fury.remains<1.5|cooldown.weapons_of_order.remains<2)
            if (API.CanCast(TigerPalm) && !LastCastTigerPalm && ChiDeficit >= 2 && (EnergyTimeToMax < 1000 || API.SpellCDDuration(Serenity) < 2000 || EnergyTimeToMax < 4000 && API.SpellCDDuration(FistsofFury) < 1500 || API.SpellCDDuration(WeaponsofOrder) < 2000) && NotChanneling)
            {
                API.CastSpell(TigerPalm);
                return;
            }



            //actions+=/call_action_list,name=aoe,if=active_enemies>=3
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber)
            {
                if (API.CanCast(WeaponsofOrder) && UseWeaponsofOrder == "AOE")
                {
                    API.CastSpell(WeaponsofOrder);
                    return;
                }
                //actions.aoe=whirling_dragon_punch
                if (API.CanCast(WhirlingDragonPunch) && TalentWhirlingDragonPunch && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(WhirlingDragonPunch);
                    return;
                }
                //actions.aoe+=/energizing_elixir,if=chi.max-chi>=2&energy.time_to_max>2|chi.max-chi>=4
                if (API.CanCast(EnergizingElixir) && TalentEnergizingElixir && ChiDeficit >= 2 && (EnergyTimeToMax > 2000 || ChiDeficit >= 4) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(EnergizingElixir);
                    return;
                }
                //actions.aoe+=/spinning_crane_kick,if=combo_strike&(buff.dance_of_chiji.up|debuff.bonedust_brew.up)
                if (API.CanCast(SpinningCraneKick) && !LastCastSpinningCraneKick && (API.PlayerHasBuff(DanceofChiJi) && TalentDanceofChiJi || API.TargetHasDebuff(BonedustBrew)) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(SpinningCraneKick);
                    return;
                }
                //actions.aoe+=/fists_of_fury,if=energy.time_to_max>execute_time|chi.max-chi<=1
                if (API.CanCast(FistsofFury) && (EnergyTimeToMax > API.TargetTimeToExec || ChiDeficit <= 1) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(FistsofFury);
                    return;
                }
                //actions.aoe+=/rising_sun_kick,target_if=min:debuff.mark_of_the_crane.remains,if=(talent.whirling_dragon_punch&cooldown.rising_sun_kick.duration>cooldown.whirling_dragon_punch.remains+4)&(cooldown.fists_of_fury.remains>3|chi>=5)
                if (API.CanCast(RisingSunKick) && (TalentWhirlingDragonPunch && API.SpellCDDuration(RisingSunKick) > API.SpellCDDuration(WhirlingDragonPunch) + 4000) && (API.SpellCDDuration(FistsofFury) > 3000 || API.PlayerCurrentChi >= 5) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(RisingSunKick);
                    return;
                }
                //actions.aoe+=/rushing_jade_wind,if=buff.rushing_jade_wind.down
                if (API.CanCast(RushingJadeWind) && TalentRushingJadeWind && !API.PlayerHasBuff(RushingJadeWind) && !CurrenCastFistsOfFury && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(RushingJadeWind);
                    return;
                }
                //actions.aoe+=/spinning_crane_kick,if=combo_strike&((cooldown.bonedust_brew.remains>2&(chi>3|cooldown.fists_of_fury.remains>6)&(chi>=5|cooldown.fists_of_fury.remains>2))|energy.time_to_max<=3)
                if (API.CanCast(SpinningCraneKick) && !LastCastSpinningCraneKick && (API.SpellCDDuration(BonedustBrew) > 2000 && (API.PlayerCurrentChi > 3 || API.SpellCDDuration(FistsofFury) > 6000) && (API.PlayerCurrentChi >= 5 || API.SpellCDDuration(FistsofFury) > 2000) || EnergyTimeToMax <= 3000) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(SpinningCraneKick);
                    return;
                }
                //actions.aoe+=/expel_harm,if=chi.max-chi>=1
                if (API.CanCast(ExpelHarm) && ChiDeficit >= 1 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(ExpelHarm);
                    return;
                }
                //actions.aoe+=/fist_of_the_white_tiger,target_if=min:debuff.mark_of_the_crane.remains,if=chi.max-chi>=3
                if (API.CanCast(FistsoftheWhiteTiger) && TalentFistoftheWhiteTiger && ChiDeficit >= 3 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(FistsoftheWhiteTiger);
                    return;
                }
                //actions.aoe+=/chi_burst,if=chi.max-chi>=2
                if (API.CanCast(ChiBurst) && TalentChiBurst && ChiDeficit >= 2 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                //actions.aoe+=/crackling_jade_lightning,if=buff.the_emperors_capacitor.stack>19&energy.time_to_max>execute_time-1&cooldown.fists_of_fury.remains>execute_time
                //actions.aoe+=/tiger_palm,target_if=min:debuff.mark_of_the_crane.remains+(debuff.recently_rushing_tiger_palm.up*20),if=chi.max-chi>=2&(!talent.hit_combo|combo_strike)
                if (API.CanCast(TigerPalm) && ChiDeficit >= 2 && !LastCastTigerPalm && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(TigerPalm);
                    return;
                }
                //actions.aoe+=/chi_wave,if=combo_strike
                if (API.CanCast(ChiWave) && TalentChiWave && !LastCastChiWave && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(ChiWave);
                    return;
                }
                //actions.aoe+=/flying_serpent_kick,if=buff.bok_proc.down,interrupt=1
                //actions.aoe+=/blackout_kick,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike&(buff.bok_proc.up|talent.hit_combo&prev_gcd.1.tiger_palm&chi=2&cooldown.fists_of_fury.remains<3|chi.max-chi<=1&prev_gcd.1.spinning_crane_kick&energy.time_to_max<3)
                if (API.CanCast(BlackOutKick) && !LastCastBlackoutkick && (API.PlayerHasBuff(BlackOutKickBuff) || TalentHitCombo) && API.SpellCDDuration(FistsofFury) < 3000 || ChiDeficit <= 1 || EnergyTimeToMax < 3000)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }

            }


            //actions+=/call_action_list,name=st,if=active_enemies<3
            if (API.PlayerUnitInMeleeRangeCount <= AOEUnitNumber || !IsAOE)
            {
                //actions.st=whirling_dragon_punch,if=raid_event.adds.in>cooldown.whirling_dragon_punch.duration*0.8|raid_event.adds.up
                if (API.CanCast(WhirlingDragonPunch) && TalentWhirlingDragonPunch && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(WhirlingDragonPunch);
                    return;
                }
                //actions.st+=/energizing_elixir,if=chi.max-chi>=2&energy.time_to_max>3|chi.max-chi>=4&(energy.time_to_max>2|!prev_gcd.1.tiger_palm)
                if (API.CanCast(EnergizingElixir) && TalentEnergizingElixir && ChiDeficit >= 2 && EnergyTimeToMax > 3000 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(EnergizingElixir);
                    return;
                }
                //actions.st+=/spinning_crane_kick,if=combo_strike&buff.dance_of_chiji.up&(raid_event.adds.in>buff.dance_of_chiji.remains-2|raid_event.adds.up)
                if (API.CanCast(SpinningCraneKick) && LastCastSpinningCraneKick && API.PlayerHasBuff(DanceofChiJi) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(SpinningCraneKick);
                    return;
                }
                //actions.st+=/rising_sun_kick,target_if=min:debuff.mark_of_the_crane.remains,if=cooldown.serenity.remains>1|!talent.serenity
                if (API.CanCast(RisingSunKick) && (!TalentSerenty || TalentSerenty && API.SpellCDDuration(Serenity) > 1000) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(RisingSunKick);
                    return;
                }
                //actions.st+=/fists_of_fury,if=(raid_event.adds.in>cooldown.fists_of_fury.duration*0.8|raid_event.adds.up)&(energy.time_to_max>execute_time-1|chi.max-chi<=1|buff.storm_earth_and_fire.remains<execute_time+1)|fight_remains<execute_time+1
                if (API.CanCast(FistsofFury) && (EnergyTimeToMax > API.TargetTimeToExec - 1000 || ChiDeficit <= 1 || API.PlayerBuffTimeRemaining(StormEarthandFire) < API.TargetTimeToExec + 1000) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(FistsofFury);
                    return;
                }
                //actions.st+=/crackling_jade_lightning,if=buff.the_emperors_capacitor.stack>19&energy.time_to_max>execute_time-1&cooldown.rising_sun_kick.remains>execute_time|buff.the_emperors_capacitor.stack>14&(cooldown.serenity.remains<5&talent.serenity|cooldown.weapons_of_order.remains<5&covenant.kyrian|fight_remains<5)
                if (API.CanCast(CracklingJadeLightning) && UseLeg == "Last Emperor's Capacitor" && API.PlayerBuffStacks(TheEmperorsCapacitor) > 19 && EnergyTimeToMax > API.TargetTimeToExec - 1000 && API.SpellCDDuration(RisingSunKick) > API.TargetTimeToExec && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.WriteLog("Last Emperor's Capacitor");
                    API.CastSpell(CracklingJadeLightning);
                    return;
                }
                //actions.st+=/rushing_jade_wind,if=buff.rushing_jade_wind.down&active_enemies>1
                if (API.CanCast(RushingJadeWind) &&TalentRushingJadeWind && !API.PlayerHasBuff(RushingJadeWind) && API.PlayerUnitInMeleeRangeCount > 1 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(RushingJadeWind);
                    return;
                }
                //actions.st+=/fist_of_the_white_tiger,target_if=min:debuff.mark_of_the_crane.remains,if=chi<3
                if (API.CanCast(FistsoftheWhiteTiger) && TalentFistoftheWhiteTiger && API.PlayerCurrentChi < 3 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(FistsoftheWhiteTiger);
                    return;
                }
                //actions.st+=/expel_harm,if=chi.max-chi>=1
                if (API.CanCast(ExpelHarm) && ChiDeficit >= 1)
                {
                    API.CastSpell(ExpelHarm);
                    return;
                }
                //actions.st+=/chi_burst,if=chi.max-chi>=1&active_enemies=1&raid_event.adds.in>20|chi.max-chi>=2&active_enemies>=2
                if (API.CanCast(ChiBurst) && TalentChiBurst && (ChiDeficit >= 1 && API.PlayerUnitInMeleeRangeCount == 1 || ChiDeficit >= 2 && API.PlayerUnitInMeleeRangeCount >= 2) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                //actions.st+=/chi_wave
                if (API.CanCast(ChiWave) && TalentChiWave && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(ChiWave);
                    return;
                }
                //actions.st+=/tiger_palm,target_if=min:debuff.mark_of_the_crane.remains+(debuff.recently_rushing_tiger_palm.up*20),if=combo_strike&chi.max-chi>=2&buff.storm_earth_and_fire.down
                if (API.CanCast(TigerPalm) && UseLeg == "Keefer's Skyreach" && API.TargetHasDebuff(SkyreachExhaustion) && !LastCastTigerPalm && ChiDeficit >= 2 && !API.PlayerHasBuff(StormEarthandFire) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.WriteLog("Use Legendary Keefer's Skyreach");
                    API.CastSpell(TigerPalm);
                    return;
                }
                //actions.st+=/spinning_crane_kick,if=buff.chi_energy.stack>30-5*active_enemies&buff.storm_earth_and_fire.down&(cooldown.rising_sun_kick.remains>2&cooldown.fists_of_fury.remains>2|cooldown.rising_sun_kick.remains<3&cooldown.fists_of_fury.remains>3&chi>3|cooldown.rising_sun_kick.remains>3&cooldown.fists_of_fury.remains<3&chi>4|chi.max-chi<=1&energy.time_to_max<2)|buff.chi_energy.stack>10&fight_remains<7
                if (API.CanCast(SpinningCraneKick) && UseLeg == "Jade Ignition" && API.PlayerBuffStacks(ChiEnergy)> 30 - 5 * API.PlayerUnitInMeleeRangeCount && !API.PlayerHasBuff(StormEarthandFire) && (API.SpellCDDuration(RisingSunKick) > 2000 && API.SpellCDDuration(FistsofFury) > 2000 || API.SpellCDDuration(RisingSunKick) < 3000 && API.SpellCDDuration(FistsofFury) > 3000 && API.PlayerCurrentChi > 3 || API.SpellCDDuration(RisingSunKick) >3000 && API.SpellCDDuration(FistsofFury) < 3000 && API.PlayerCurrentChi > 4 || ChiDeficit <= 1 && EnergyTimeToMax < 2000) && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.WriteLog("Jade Ignition");
                    API.CanCast(SpinningCraneKick);
                    return;
                }
                //actions.st+=/blackout_kick,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike&(talent.serenity&cooldown.serenity.remains<3|cooldown.rising_sun_kick.remains>1&cooldown.fists_of_fury.remains>1|cooldown.rising_sun_kick.remains<3&cooldown.fists_of_fury.remains>3&chi>2|cooldown.rising_sun_kick.remains>3&cooldown.fists_of_fury.remains<3&chi>3|chi>5|buff.bok_proc.up)
                if (API.CanCast(BlackOutKick) && !LastCastBlackoutkick && (TalentSerenty && API.SpellCDDuration(Serenity) < 3000 || API.SpellCDDuration(RisingSunKick) > 1000 && API.SpellCDDuration(FistsofFury) > 1000 || API.SpellCDDuration(RisingSunKick) < 3000 && API.SpellCDDuration(FistsofFury) > 3000 && API.PlayerCurrentChi == 2 || API.SpellCDDuration(RisingSunKick) > 3000 && API.SpellCDDuration(FistsofFury) < 3000 && API.PlayerCurrentChi > 3 || API.PlayerCurrentChi > 5) && NotChanneling && !CurrenCastFistsOfFury || API.PlayerHasBuff(BlackOutKickBuff))
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //actions.st+=/tiger_palm,target_if=min:debuff.mark_of_the_crane.remains+(debuff.recently_rushing_tiger_palm.up*20),if=combo_strike&chi.max-chi>=2
                if (API.CanCast(TigerPalm) && UseLeg == "Keefer's Skyreach" && API.TargetHasDebuff(SkyreachExhaustion) && !LastCastTigerPalm && ChiDeficit >= 2 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.WriteLog("Use Legendary Keefer's Skyreach");
                    API.CastSpell(TigerPalm);
                    return;
                }
                //actions.st+=/flying_serpent_kick,interrupt=1
                //actions.st+=/blackout_kick,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike&cooldown.fists_of_fury.remains<3&chi=2&prev_gcd.1.tiger_palm&energy.time_to_50<1
                if (API.CanCast(BlackOutKick) && !LastCastBlackoutkick && API.SpellCDDuration(FistsofFury) < 3000 && API.PlayerCurrentChi == 2 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //actions.st+=/blackout_kick,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike&energy.time_to_max<2&(chi.max-chi<=1|prev_gcd.1.tiger_palm)
                if (API.CanCast(BlackOutKick) && !LastCastBlackoutkick && EnergyTimeToMax < 2000 && ChiDeficit <= 1 && NotChanneling && !CurrenCastFistsOfFury)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
            }
        }

        private void SerentyRotation()
        {
            //actions.serenity=fists_of_fury,if=buff.serenity.remains<1
            if (API.CanCast(FistsofFury) && API.PlayerBuffTimeRemaining(Serenity) < 1000 && NotChanneling)
            {
                API.CastSpell(FistsofFury);
                return;
            }
            //actions.serenity+=/spinning_crane_kick,if=combo_strike&(active_enemies>=3|active_enemies>1&!cooldown.rising_sun_kick.up)
            if (API.CanCast(SpinningCraneKick) && !LastCastSpinningCraneKick && (API.PlayerUnitInMeleeRangeCount >= 3 || API.PlayerUnitInMeleeRangeCount > 1 && API.SpellISOnCooldown(RisingSunKick)) && NotChanneling)
            {
                API.CastSpell(SpinningCraneKick);
                return;
            }
            //actions.serenity+=/rising_sun_kick,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike
            if (API.CanCast(RisingSunKick) && !LastCastRisingSunKick && NotChanneling)
            {
                API.WriteLog("Rising Sun Kick Serenty");
                API.CastSpell(RisingSunKick);
                return;
            }
            //actions.serenity+=/fists_of_fury,if=active_enemies>=3
            if (API.CanCast(FistsofFury) && API.PlayerUnitInMeleeRangeCount >= 3 && NotChanneling)
            {
                API.CastSpell(FistsofFury);
                return;
            }
            //actions.serenity+=/spinning_crane_kick,if=combo_strike&buff.dance_of_chiji.up
            if (API.CanCast(SpinningCraneKick) && API.PlayerHasBuff(DanceofChiJi) && NotChanneling)
            {
                API.CanCast(SpinningCraneKick);
                return;
            }
            //actions.serenity+=/blackout_kick,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike&buff.weapons_of_order_ww.up&cooldown.rising_sun_kick.remains>2
            if (API.CanCast(BlackOutKick) && !LastCastBlackoutkick && API.PlayerHasBuff(WeaponsofOrder) && API.SpellCDDuration(RisingSunKick) > 2000 && NotChanneling)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //actions.serenity+=/fists_of_fury,interrupt_if=!cooldown.rising_sun_kick.up

            //actions.serenity+=/spinning_crane_kick,if=combo_strike&debuff.bonedust_brew.up
            if (API.CanCast(SpinningCraneKick) && !LastCastSpinningCraneKick && API.TargetHasDebuff(BonedustBrew) && NotChanneling)
            {
                API.CastSpell(SpinningCraneKick);
                return;
            }
            //actions.serenity+=/fist_of_the_white_tiger,target_if=min:debuff.mark_of_the_crane.remains,if=chi<3
            if (API.CanCast(FistsoftheWhiteTiger) && TalentFistoftheWhiteTiger && API.PlayerCurrentChi < 3 && NotChanneling)
            {
                API.CastSpell(FistsoftheWhiteTiger);
                return;
            }
            //actions.serenity+=/blackout_kick,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike|!talent.hit_combo
            if (API.CanCast(BlackOutKick) && !LastCastBlackoutkick && NotChanneling)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //actions.serenity+=/spinning_crane_kick
            if (API.CanCast(SpinningCraneKick) && NotChanneling)
            {
                API.CanCast(SpinningCraneKick);
                return;
            }
        }

        private void WeaponsOfOrderRotation()
        {
            //actions.weapons_of_order+=/energizing_elixir,if=chi.max-chi>=2&energy.time_to_max>3
            if (API.CanCast(EnergizingElixir) && TalentEnergizingElixir && ChiDeficit >= 2 && EnergyTimeToMax > 3000 && NotChanneling && NotCasting)
            {
                API.CastSpell(EnergizingElixir);
                return;
            }
            //actions.weapons_of_order+=/rising_sun_kick,target_if=min:debuff.mark_of_the_crane.remains
            if (API.CanCast(RisingSunKick) && NotChanneling && NotCasting)
            {
                API.CastSpell(RisingSunKick);
                return;
            }
            //actions.weapons_of_order+=/spinning_crane_kick,if=combo_strike&buff.dance_of_chiji.up
            if (API.CanCast(SpinningCraneKick) && !LastCastSpinningCraneKick && TalentDanceofChiJi && API.PlayerHasBuff(DanceofChiJi) && NotChanneling && NotCasting)
            {
                API.CastSpell(SpinningCraneKick);
                return;
            }
            //actions.weapons_of_order+=/fists_of_fury,if=active_enemies>=2&buff.weapons_of_order_ww.remains<1
            if (API.CanCast(FistsofFury) && API.PlayerUnitInMeleeRangeCount >= 2 && NotChanneling && NotCasting)
            {
                API.CastSpell(FistsofFury);
                return;
            }
            //actions.weapons_of_order+=/whirling_dragon_punch,if=active_enemies>=2
            if (API.CanCast(WhirlingDragonPunch) && TalentWhirlingDragonPunch && API.PlayerUnitInMeleeRangeCount >= 2 && NotChanneling && NotCasting)
            {
                API.CastSpell(WhirlingDragonPunch);
                return;
            }
            //actions.weapons_of_order+=/spinning_crane_kick,if=combo_strike&active_enemies>=3&buff.weapons_of_order_ww.up
            if (API.CanCast(SpinningCraneKick) && !LastCastSpinningCraneKick && API.PlayerUnitInMeleeRangeCount >= 3)
            {
                API.CastSpell(SpinningCraneKick);
                return;
            }
            //actions.weapons_of_order+=/blackout_kick,target_if=min:debuff.mark_of_the_crane.remains,if=combo_strike&active_enemies<=2
            if (API.CanCast(BlackOutKick) && !LastCastBlackoutkick && API.PlayerUnitInMeleeRangeCount <= 2 && NotChanneling && NotCasting)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //actions.weapons_of_order+=/whirling_dragon_punch
            if (API.CanCast(WhirlingDragonPunch) && TalentWhirlingDragonPunch && NotChanneling && NotCasting)
            {
                API.CastSpell(WhirlingDragonPunch);
                return;
            }
            //actions.weapons_of_order+=/fists_of_fury,interrupt=1,if=buff.storm_earth_and_fire.up&raid_event.adds.in>cooldown.fists_of_fury.duration*0.6
            //actions.weapons_of_order+=/spinning_crane_kick,if=buff.chi_energy.stack>30-5*active_enemies
            if (API.CanCast(SpinningCraneKick) && UseLeg == "Jade Ignition" && API.PlayerBuffStacks(ChiEnergy) > 30 - 5 * API.PlayerUnitInMeleeRangeCount)
            {
                API.WriteLog("Jade Ignition");
                API.CastSpell(SpinningCraneKick);
                return;
            }
            //actions.weapons_of_order+=/fist_of_the_white_tiger,target_if=min:debuff.mark_of_the_crane.remains,if=chi<3
            if (API.CanCast(FistsoftheWhiteTiger) && TalentFistoftheWhiteTiger && API.PlayerCurrentChi < 3 && NotChanneling && NotCasting)
            {
                API.CastSpell(FistsoftheWhiteTiger);
                return;
            }
            //actions.weapons_of_order+=/expel_harm,if=chi.max-chi>=1
            if (API.CanCast(ExpelHarm) && ChiDeficit >= 1 && NotChanneling && NotCasting)
            {
                API.CastSpell(ExpelHarm);
                return;
            }
            //actions.weapons_of_order+=/chi_burst,if=chi.max-chi>=(1+active_enemies>1)
            if (API.CanCast(ChiBurst) && TalentChiBurst && ChiDeficit >= 1 && NotChanneling && NotCasting)
            {
                API.CastSpell(ChiBurst);
                return;
            }
            //actions.weapons_of_order+=/tiger_palm,target_if=min:debuff.mark_of_the_crane.remains+(debuff.recently_rushing_tiger_palm.up*20),if=(!talent.hit_combo|combo_strike)&chi.max-chi>=2
            if (API.CanCast(TigerPalm) && UseLeg == "Keefer's Skyreach" && API.TargetHasDebuff(SkyreachExhaustion) && ChiDeficit >= 2)
            {
                API.CastSpell(TigerPalm);
                return;
            }
            //actions.weapons_of_order+=/chi_wave
            if (API.CanCast(ChiWave) && TalentChiWave && NotChanneling && NotCasting)
            {
                API.CastSpell(ChiWave);
                return;
            }
            //actions.weapons_of_order+=/blackout_kick,target_if=min:debuff.mark_of_the_crane.remains,if=chi>=3|buff.weapons_of_order_ww.up
            if (API.CanCast(BlackOutKick) && API.PlayerCurrentChi >= 3 && NotChanneling && NotCasting)
            {
                API.CastSpell(BlackOutKick);
                return;
            }
            //actions.weapons_of_order+=/flying_serpent_kick,interrupt=1
        }

        private void CooldownsSerenty()
        {
            //actions.cd_serenity+=/invoke_xuen_the_white_tiger,if=!variable.hold_xuen|fight_remains<25
            if (API.CanCast(InvokeXuen) && UseInvokeXuen == "with cooldowns")
            {
                API.CastSpell(InvokeXuen);
                return;
            }
            //actions.cd_serenity+=/blood_fury,if=variable.serenity_burst
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_serenity+=/berserking,if=variable.serenity_burst
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_serenity+=/arcane_torrent,if=chi.max-chi>=1
            if (API.CanCast(RacialSpell1) && isRacial && ChiDeficit >= 1 && PlayerRaceSettings == "Blood Elf")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_serenity+=/fireblood,if=variable.serenity_burst
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_serenity+=/ancestral_call,if=variable.serenity_burst
            if (API.CanCast(RacialSpell1) && isRacial)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_serenity+=/bag_of_tricks,if=variable.serenity_burst
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Vulpera")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_serenity+=/touch_of_death,if=fight_remains>180|pet.xuen_the_white_tiger.active|fight_remains<10
            if (IsCooldowns && !API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 0 && API.TargetMaxHealth < API.PlayerMaxHealth && (UseTouchofDeath == "with Cooldowns"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //actions.cd_serenity+=/touch_of_karma,if=fight_remains>90|pet.xuen_the_white_tiger.active|fight_remains<10
            //actions.cd_serenity+=/weapons_of_order,if=cooldown.rising_sun_kick.remains<execute_time
            if (API.CanCast(WeaponsofOrder) && UseWeaponsofOrder == "with Cooldowns" && API.SpellCDDuration(RisingSunKick) < API.TargetTimeToExec)
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            //actions.cd_serenity+=/faeline_stomp
            if (API.CanCast(FaelineStomp) && UseFaelineStomp == "with Cooldowns" && PlayerCovenantSettings == "Night Fae")
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            //actions.cd_serenity+=/fallen_order
            if (API.CanCast(FallenOrder) && UseFallenOrder == "with Cooldowns" && PlayerCovenantSettings == "Venthyr")
            {
                API.CastSpell(FallenOrder);
                return;
            }
            //actions.cd_serenity+=/bonedust_brew
            if (API.CanCast(BonedustBrew) && UseBonedustBrew == "with Cooldowns" && PlayerCovenantSettings == "Necrolord")
            {
                API.CastSpell(BonedustBrew);
                return;
            }
            //actions.cd_serenity+=/serenity,if=cooldown.rising_sun_kick.remains<2|fight_remains<15
            if (API.CanCast(Serenity) && API.SpellCDDuration(RisingSunKick) < 2000)
            {
                API.CastSpell(Serenity);
                return;
            }
            //actions.cd_serenity+=/bag_of_tricks
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Vulpera")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
        }
        private void Cooldowns()
        {
            //actions.cd_sef=invoke_xuen_the_white_tiger,if=!variable.hold_xuen|fight_remains<25
            if (API.CanCast(InvokeXuen) && UseInvokeXuen == "with Cooldowns")
            {
                API.CastSpell(InvokeXuen);
                return;
            }
            //actions.cd_sef+=/arcane_torrent,if=chi.max-chi>=1
            if (API.CanCast(RacialSpell1) && isRacial && ChiDeficit >= 1 && PlayerRaceSettings == "Blood Elf")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_sef+=/touch_of_death,if=buff.storm_earth_and_fire.down&pet.xuen_the_white_tiger.active|fight_remains<10|fight_remains>180
            if (IsCooldowns && !API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 0 && API.TargetMaxHealth < API.PlayerMaxHealth && (UseTouchofDeath == "with Cooldowns"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //actions.cd_sef+=/weapons_of_order,if=(raid_event.adds.in>45|raid_event.adds.up)&cooldown.rising_sun_kick.remains<execute_time
            if (API.CanCast(WeaponsofOrder) && UseWeaponsofOrder == "with Cooldowns")
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            //actions.cd_sef+=/faeline_stomp,if=combo_strike&(raid_event.adds.in>10|raid_event.adds.up)
            if (API.CanCast(FaelineStomp) && UseFaelineStomp == "with Cooldowns" && PlayerCovenantSettings == "Night Fae")
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            //actions.cd_sef+=/fallen_order,if=raid_event.adds.in>30|raid_event.adds.up
            if (API.CanCast(FallenOrder) && UseFallenOrder == "with Cooldowns" && PlayerCovenantSettings == "Venthyr")
            {
                API.CastSpell(FallenOrder);
                return;
            }
            //actions.cd_sef+=/bonedust_brew,if=raid_event.adds.in>50|raid_event.adds.up,line_cd=60
            if (API.CanCast(BonedustBrew) && UseBonedustBrew == "with Cooldowns" && PlayerCovenantSettings == "Necrolord")
            {
                API.CanCast(BonedustBrew);
                return;
            }
            //actions.cd_sef+=/storm_earth_and_fire_fixate,if=conduit.coordinated_offensive.enabled
            if (API.CanCast(StormEarthAndFire) && API.PlayerIsConduitSelected(CoordinatedOffensive))
            {
                API.CastSpell(StormEarthAndFire);
                return;
            }
            //actions.cd_sef+=/storm_earth_and_fire,if=cooldown.storm_earth_and_fire.charges=2|fight_remains<20|(raid_event.adds.remains>15|!covenant.kyrian&((raid_event.adds.in>cooldown.storm_earth_and_fire.full_recharge_time|!raid_event.adds.exists)&(cooldown.invoke_xuen_the_white_tiger.remains>cooldown.storm_earth_and_fire.full_recharge_time|variable.hold_xuen))&cooldown.fists_of_fury.remains<=9&chi>=2&cooldown.whirling_dragon_punch.remains<=12)
            if (API.CanCast(StormEarthandFire))
            {
                API.CastSpell(StormEarthandFire);
                return;
            }
            //actions.cd_sef+=/storm_earth_and_fire,if=covenant.kyrian&(buff.weapons_of_order.up|(fight_remains<cooldown.weapons_of_order.remains|cooldown.weapons_of_order.remains>cooldown.storm_earth_and_fire.full_recharge_time)&cooldown.fists_of_fury.remains<=9&chi>=2&cooldown.whirling_dragon_punch.remains<=12)
            if (API.CanCast(StormEarthandFire) && PlayerCovenantSettings == "Kyrian" && API.PlayerHasBuff(WeaponsofOrder))
            {
                API.CastSpell(StormEarthandFire);
                return;
            }
            //actions.cd_sef+=/touch_of_karma,if=fight_remains>159|pet.xuen_the_white_tiger.active|variable.hold_xuen
            //actions.cd_sef+=/blood_fury,if=cooldown.invoke_xuen_the_white_tiger.remains>30|variable.hold_xuen|fight_remains<20
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_sef+=/berserking,if=cooldown.invoke_xuen_the_white_tiger.remains>30|variable.hold_xuen|fight_remains<15
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll" && API.SpellCDDuration(InvokeXuen) > 30000)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_sef+=/fireblood,if=cooldown.invoke_xuen_the_white_tiger.remains>30|variable.hold_xuen|fight_remains<10
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Dark Iron Dwarf" && API.SpellCDDuration(InvokeXuen) > 30000)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_sef+=/ancestral_call,if=cooldown.invoke_xuen_the_white_tiger.remains>30|variable.hold_xuen|fight_remains<20
            if (API.CanCast(RacialSpell1) && API.SpellCDDuration(InvokeXuen) > 30000)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cd_sef+=/bag_of_tricks,if=buff.storm_earth_and_fire.down
            if (API.CanCast(RacialSpell1) && isRacial && !API.PlayerHasBuff(StormEarthAndFire) && PlayerRaceSettings == "Vulpera")
            {
                API.CastSpell(RacialSpell1);
                return;
            }
        }
        public override void OutOfCombatPulse()
        {

        }
    }
}
