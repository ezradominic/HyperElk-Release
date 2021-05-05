using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace HyperElk.Core
{
    public class FistweaverMonk : CombatRoutine
    {
        //Spell Strings
        //Combat
        private string TigerPalm = "Tiger Palm";
        private string BlackoutKick = "Blackout Kick";
        private string SpinningCraneKick = "Spinning Crane Kick";
        private string ExpelHarm = "Expel Harm";
        private string RisingSunKick = "Rising Sun Kick";
        private string DampenHarm = "Dampen Harm";
        private string HealingElixir = "Healing Elixir";
        private string TouchOfDeath = "Touch of Death";
        private string LegSweep = "Leg Sweep";
        //Heal
        private string Vivify = "Vivify";
        private string EnvelopingMist = "Enveloping Mist";
        private string RenewingMist = "Renewing Mist";
        private string EssenceFont = "Essence Font";
        private string SoothingMist = "Soothing Mist";
        private string LifeCocoon = "Life Cocoon";
        private string Revival = "Revival";
        private string Yulon = "Invoke Yu'lon, the Jade Serpent";
        private string ChiJi = "Invoke Chi-Ji, the Red Crane";
        private string ManaTea = "Mana Tea";
        private string ThunderFocusTea = "Thunder Focus Tea";
        private string ChiWave = "Chi Wave";
        private string ChiBurst = "Chi Burst";
        private string RefreshingJadeWind = "Refreshing Jade Wind";
        private string SummonJadeSerpentStatue = "Summon Jade Serpent Statue";
        private string Detox = "Detox";
        private string SpiritualManaPotion = "Spiritual Mana Potion";
        private string WeaponsofOrder = "Weapons of Order";
        private string WeaponsofOrderAOE = "Weapons of Order AOE";
        private string LCT = "Life Cocoon on Tank Only";



        private string Fleshcraft = "Fleshcraft";
        private string FaelineStomp = "FaelineStomp";
        private string FallenOrder = "Fallen Order";

        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        //Target Misc
        private string Party1 = "party1";
        private string Party2 = "party2";
        private string Party3 = "party3";
        private string Party4 = "party4";
        private string Player = "player";
        private string AoE = "AOE";
        private string ChiBurstAOE = "Chi Burst AOE";
        private string ChiJiAOE = "ChiJi AOE";
        private string YuLonAOE = "YuLon AOE";
        private string RJWAOE = "Refreshing Jade Wind AOE";
        private string EFAOE = "Essence Font AOE";
        private string RAOE = "Revival AOE";
        private string FW = "FistWeaving";
        private string TAB = "TAB";
        private string AoEDPS = "AOEDPS";
        private string AoEDPSH = "AOEDPS Health";
        private string AoEDPSHRaid = "AOEDPS Health Raid";
        private string AoEDPSRaid = "AOEDPS Raid";
        private string Assist = "Assist";
        private string AncientTeachingsOfTheMonastery = "Ancient Teachings of the Monastery";
        private string TeachingsOfTheMonMonastery => "Teachings of the monastery";
        private string EmergencyGroupNumber => "Emergency Group Number";
        private string EmergencyGroupPercent => "Emergency Group Percent";

        private string EmergencyRaidNumber => "Emergency Raid Number";
        private string EmergencyRaidPercent => "Emergency Raid Percent";
        //Talents
        private bool TalentChiWave => API.PlayerIsTalentSelected(1, 2);
        private bool TalentChiBurst => API.PlayerIsTalentSelected(1, 3);

        private bool TalentManaTea => API.PlayerIsTalentSelected(3, 3);
        private bool TalentDampenHarm => API.PlayerIsTalentSelected(5, 3);
        private bool TalentHealingElixir => API.PlayerIsTalentSelected(5, 1);
        private bool TalentRefreshingJadeWind => API.PlayerIsTalentSelected(6, 2);
        private bool TalentSummonJadeSerpentStatue => API.PlayerIsTalentSelected(6, 1);
        private bool TalentInvokeChiJi => API.PlayerIsTalentSelected(6, 3);



        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        string[] units = { "player", "party1", "party2", "party3", "party4" };
        string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        int PlayerHealth => API.TargetHealthPercent;
        string[] PlayerTargetArray = { "player", "party1", "party2", "party3", "party4" };
        string[] RaidTargetArray = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        string[] DetoxList = { "Chilled", "Frozen Binds", "Clinging Darkness", "Rasping Scream", "Heaving Retch", "Goresplatter", "Slime Injection", "Gripping Infection", "Debilitating Plague", "Burning Strain", "Blightbeak", "Corroded Claws", "Wasting Blight", "Hurl Spores", "Corrosive Gunk", "Cytotoxic Slash", "Venompiercer", "Wretched Phlegm", "Bewildering Pollen", "Repulsive Visage", "Soul Split", "Anima Injection", "Bewildering Pollen2", "Bramblethorn Entanglement", "Debilitating Poison", "Sinlight Visions", "Siphon Life", "Turn to Stone", "Stony Veins", "Cosmic Artifice", "Wailing Grief", "Shadow Word:  Pain", "Anguished Cries", "Wrack Soul", "Dark Lance", "Insidious Venom", "Charged Anima", "Lost Confidence", "Burden of Knowledge", "Internal Strife", "Forced Confession", "Insidious Venom2", "Soul Corruption", "Genetic Alteration", "Withering Blight", "Decaying Blight" };

        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);
        private bool AutoTarget => API.ToggleIsEnabled("Auto Target");
        private bool AoEHeal => API.ToggleIsEnabled("AOE Heal");
        private bool OOC => API.ToggleIsEnabled("OOC");
        private bool RangeHeal => API.ToggleIsEnabled("Range HEAL");
        private bool NPCHeal => API.ToggleIsEnabled("NPC Heal");

        //General
        private static readonly Stopwatch ChiJiWatch = new Stopwatch();

        string[] ThunderFocusTeaList = new string[] {"Rising Sun Kick Cooldown", "Manual", };
        private string UseThunderFocusTea => ThunderFocusTeaList[CombatRoutine.GetPropertyInt(ThunderFocusTea)];
        private bool NotChanneling => API.PlayerCurrentCastTimeRemaining == 0;
        private bool NotCasting => !API.PlayerIsCasting(false);
        private int ChiJiNumber => numbPartyList[CombatRoutine.GetPropertyInt(ChiJiAOE)];
        private int YuLonNumber => numbPartyList[CombatRoutine.GetPropertyInt(YuLonAOE)];
        private int RAOEumber => numbPartyList[CombatRoutine.GetPropertyInt(RAOE)];
        private int RJWAOENumber => numbPartyList[CombatRoutine.GetPropertyInt(RJWAOE)];
        private int EFAOENumber => numbPartyList[CombatRoutine.GetPropertyInt(EFAOE)];
        private int ChiBurstNumber => numbPartyList[CombatRoutine.GetPropertyInt(ChiBurstAOE)];
        private int EmergencyGroupNumb => numbPartyList[CombatRoutine.GetPropertyInt(EmergencyGroupNumber)];
        private int EmergencyGroupPerc => numbPartyList[CombatRoutine.GetPropertyInt(EmergencyGroupPercent)];
        private int EmergencyRaidNumb => numbPartyList[CombatRoutine.GetPropertyInt(EmergencyRaidNumber)];
        private int EmergencyRaidPerc => numbPartyList[CombatRoutine.GetPropertyInt(EmergencyRaidPercent)];
        private int FWHP => numbPartyList[CombatRoutine.GetPropertyInt(FW)];

        private int ExpelHarmtPercent => numbList[CombatRoutine.GetPropertyInt(ExpelHarm)];
        private bool ChiBurstAoE => UnitBelowHealthPercent(ChiBurstPercent) >= ChiBurstNumber;
        private bool YuLonAoE => UnitBelowHealthPercent(YuLonPercent) >= YuLonNumber;
        private bool ChiJiAoE => UnitBelowHealthPercent(ChiJiPercent) >= ChiJiNumber;
        private bool RevivalAoE => UnitBelowHealthPercent(RevivalPercent) >= RAOEumber;
        private bool Fistweaving => API.PlayerIsInRaid ? UnitBelowHealthPercent(EmergencyRaidPerc) <= EmergencyRaidNumb : UnitBelowHealthPercent(EmergencyGroupPerc) <= EmergencyGroupNumb;
        private bool Healing => API.PlayerIsInRaid ? UnitBelowHealthPercent(EmergencyRaidPerc) >= EmergencyRaidNumb : UnitBelowHealthPercent(EmergencyGroupPerc) >= EmergencyGroupNumb;
        private bool EssenceFontAoE => UnitBelowHealthPercent(EssenceFontPercent) >= EFAOENumber;
        private bool RefreshingJadeWindAoE => UnitBelowHealthPercent(RefreshingJadeWindPercent) >= RJWAOENumber;
        private int WeaponsofOrderPercent => numbList[CombatRoutine.GetPropertyInt(WeaponsofOrderAOE)];
        private int EnvelopingMistPercent => numbList[CombatRoutine.GetPropertyInt(EnvelopingMist)];
        private int VivifyPercent => numbList[CombatRoutine.GetPropertyInt(Vivify)];
        private int SoothingMistPercent => numbList[CombatRoutine.GetPropertyInt(SoothingMist)];
        private int LifeCocoonPercent => numbList[CombatRoutine.GetPropertyInt(LifeCocoon)];
        private int RenewingMistPercent => numbList[CombatRoutine.GetPropertyInt(RenewingMist)];
        private int RevivalPercent => numbList[CombatRoutine.GetPropertyInt(Revival)];
        private int EssenceFontPercent => numbList[CombatRoutine.GetPropertyInt(EssenceFont)];
        private int ManaTeaPercent => numbList[CombatRoutine.GetPropertyInt(ManaTea)];
        private int ChiWavePercent => numbList[CombatRoutine.GetPropertyInt(ChiWave)];
        private int ChiBurstPercent => numbList[CombatRoutine.GetPropertyInt(ChiBurst)];
        private int YuLonPercent => numbList[CombatRoutine.GetPropertyInt(Yulon)];
        private int ChiJiPercent => numbList[CombatRoutine.GetPropertyInt(ChiJi)];
        private int DampenHarmPercent => numbList[CombatRoutine.GetPropertyInt(DampenHarm)];
        private int HealingElixirPercent => numbList[CombatRoutine.GetPropertyInt(HealingElixir)];
        private int RefreshingJadeWindPercent => numbList[CombatRoutine.GetPropertyInt(RefreshingJadeWind)];
        private int SpiritualManaPotionManaPercent => numbList[CombatRoutine.GetPropertyInt(SpiritualManaPotion)];
        private bool LCTank => CombatRoutine.GetPropertyBool(LCT);
        private string UseWeaponsofOrder => WeaponsofOrderList[CombatRoutine.GetPropertyInt(WeaponsofOrder)];
        string[] WeaponsofOrderList = new string[] { "always", "Cooldowns", "Manual", "AOE", };
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        bool ChannelSoothingMist => API.CurrentCastSpellID("player") == 115175;

        string[] FaelineStompList = new string[] { "always", "Cooldowns", "AOE", "AOEHeal" };
        private string UseFaelineStomp => FaelineStompList[CombatRoutine.GetPropertyInt(FaelineStomp)];
        string[] FallenOrderList = new string[] { "always", "Cooldowns", "AOE" };
        private string UseFallenOrder => FallenOrderList[CombatRoutine.GetPropertyInt(FallenOrder)];
        private string UseTrinket1 => TrinketList1[CombatRoutine.GetPropertyInt(trinket1)];
        string[] TrinketList1 = new string[] { "always", "Cooldowns", "never" };

        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "never" };
        private static bool CanDetoxTarget(string debuff)
        {
            return API.TargetHasDebuff(debuff, false, true);
        }
        private static bool CanDetoxTarget(string debuff, string unit)
        {
            return API.UnitHasDebuff(debuff, unit, false, true);
        }
        private int UnitAboveHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) >= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitAboveHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) >= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int AoEDPSHLifePercent => numbList[CombatRoutine.GetPropertyInt(AoEDPSH)];
        private int AoEDPSHRaidLifePercent => numbList[CombatRoutine.GetPropertyInt(AoEDPSHRaid)];
        private int AoEDPSNumber => numbPartyList[CombatRoutine.GetPropertyInt(AoEDPS)];
        private int AoEDPSRaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoEDPSRaid)];
        private bool RangeCheck => API.TargetRange <= 40;
        private static readonly Stopwatch SwapWatch = new Stopwatch();
        private int SwapSpeed => CombatRoutine.GetPropertyInt("SwapSpeed");
        private int RangePartyTracking(int Range) => units.Count(p => API.UnitRange(p) <= Range);
        private int RangeRaidTracking(int Range) => raidunits.Count(p => API.UnitRange(p) <= Range);
        private int RangeTracking(int Range) => API.PlayerIsInRaid ? RangeRaidTracking(Range) : RangePartyTracking(Range);
        bool LastCastEssenceFont => API.LastSpellCastInGame == "Essence Font";
        bool CurrentCastEssenceFont => API.CurrentCastSpellID("player") == 191837;
        private bool EssenceFontRange => RangeTracking(25) >= EFAOENumber;
        private bool RefreshingJadeWindRange => RangeTracking(10) >= RJWAOENumber;
        private bool ChiBurstRange => RangeTracking(10) >= ChiBurstNumber;


        private string GetTankParty(string[] units)
        {
            string lowest = units[0];

            int Tank = 999;
            foreach (string unit in units)
            {
                if (API.UnitRoleSpec(unit) == Tank && API.UnitRange(unit) <= 40 && API.UnitHealthPercent(unit) > 0)
                {
                    Tank = API.UnitRoleSpec(unit);
                    lowest = unit;
                }
            }
            return lowest;
        }
        private string GetTankRaid(string[] raidunits)
        {
            string lowest = raidunits[0];

            int Tank = 999;
            foreach (string raidunit in raidunits)
            {
                if (API.UnitRoleSpec(raidunit) == Tank && API.UnitRange(raidunit) <= 20 && API.UnitHealthPercent(raidunit) > 0)
                {
                    Tank = API.UnitRoleSpec(raidunit);
                    lowest = raidunit;
                }
            }
            return lowest;
        }
        private string RenewingMistBuffParty(string[] units)
        {
            string lowest = units[0];

            bool buff = false;
            foreach (string unit in units)
            {
                if (API.UnitHasBuff(RenewingMist, unit) == buff && API.UnitRange(unit) <= 34 && API.UnitHealthPercent(unit) > 0)
                {
                    buff = API.UnitHasBuff(RenewingMist, unit);
                    lowest = unit;
                }

            }
            return lowest;
        }
        private string RenewingMistBuffPartyTank(string[] units)
        {
            string lowest = units[0];

            bool buff = false;
            foreach (string unit in units)
            {
                if (API.UnitHasBuff(RenewingMist, unit) == buff && API.UnitRange(unit) <= 34 && API.UnitRoleSpec(unit) == 999 && API.UnitHealthPercent(unit) > 0)
                {
                    buff = API.UnitHasBuff(RenewingMist, unit);
                    lowest = unit;
                }
            }
            return lowest;
        }
        private string RenewingMistBuffRaidRaidTank(string[] raidunits)
        {
            string lowest = raidunits[0];
            bool buff = false;

            foreach (string raidunit in raidunits)
            {
                if (API.UnitHasBuff(RenewingMist, raidunit) == buff && API.UnitRange(raidunit) <= 34 && API.UnitRoleSpec(raidunit) == 999 && API.UnitHealthPercent(raidunit) > 0)
                {
                    buff = API.UnitHasBuff(RenewingMist, raidunit);
                    lowest = raidunit;
                }
            }
            return lowest;
        }
        private string RenewingMistBuffRaid(string[] raidunits)
        {
            string lowest = raidunits[0];
            bool buff = false;

            foreach (string raidunit in raidunits)
            {
                if (API.UnitHasBuff(RenewingMist, raidunit) == buff && API.UnitRange(raidunit) <= 34 && API.UnitHealthPercent(raidunit) > 0)
                {
                    buff = API.UnitHasBuff(RenewingMist, raidunit);
                    lowest = raidunit;
                }
            }
                return lowest;
        }
        private string LowestParty(string[] units)
        {
            string lowest = units[0];
            int health = 100;
            foreach (string unit in units)
            {
                if (API.UnitHealthPercent(unit) < health && API.UnitRange(unit) <= 39 && API.UnitHealthPercent(unit) > 0 && API.UnitHealthPercent(unit) != 100)
                {
                    lowest = unit;
                    health = API.UnitHealthPercent(unit);
                }
            }
            return lowest;
        }
        private string LowestRaid(string[] raidunits)
        {
            string lowest = raidunits[0];
            int health = 100;
            foreach (string raidunit in raidunits)
            {
                if (API.UnitHealthPercent(raidunit) < health && API.UnitRange(raidunit) <= 39 && API.UnitHealthPercent(raidunit) > 0 && API.UnitHealthPercent(raidunit) != 100)
                {
                    lowest = raidunit;
                    health = API.UnitHealthPercent(raidunit);
                }
            }
            return lowest;
        }
        public override void Initialize()
        {
            CombatRoutine.Name = "Mistweaver Monk by Mufflon12";
            API.WriteLog("Welcome to Fistweaver Monk by Mufflon12");
            API.WriteLog("Be advised this a Beta Rotation");
            API.WriteLog("Ancient Teachings of the Monastery is higly recommend for this Playstyle");
            API.WriteLog("A lot of the Rotation stuff is still Hardcoded life emergency heal");
            API.WriteLog("Hugh Thanks to Cookiez for beta teststing");
            API.WriteLog("Please report any issues in the Discord");
            //Combat
            CombatRoutine.AddSpell(TigerPalm, 100780, "D1");
            CombatRoutine.AddSpell(BlackoutKick, 100784, "D2");
            CombatRoutine.AddSpell(SpinningCraneKick, 101546, "D3");
            CombatRoutine.AddSpell(ExpelHarm, 322101, "D4");
            CombatRoutine.AddSpell(RisingSunKick, 107428, "D5");
            CombatRoutine.AddSpell(HealingElixir, 122281);
            CombatRoutine.AddSpell(DampenHarm, 122278);
            CombatRoutine.AddSpell(TouchOfDeath, 322109);
            CombatRoutine.AddSpell(LegSweep, 119381);
            //heal
            CombatRoutine.AddSpell(Vivify, 116670, "NumPad1");
            CombatRoutine.AddSpell(EnvelopingMist, 124682, "NumPad2");
            CombatRoutine.AddSpell(RenewingMist, 115151, "NumPad3");
            CombatRoutine.AddSpell(EssenceFont, 191837, "NumPad4");
            CombatRoutine.AddSpell(Yulon, 322118, "NumPad5");
            CombatRoutine.AddSpell(ChiJi, 325197, "NumPad5");
            CombatRoutine.AddSpell(RefreshingJadeWind, 196725, "NumPad6");
            CombatRoutine.AddSpell(SummonJadeSerpentStatue, 115313, "NumPad6");
            CombatRoutine.AddSpell(Revival, 115310, "NumPad7");
            CombatRoutine.AddSpell(SoothingMist, 115175, "NumPad8");
            CombatRoutine.AddSpell(ManaTea, 197908, "NumPad9");
            CombatRoutine.AddSpell(LifeCocoon, 116849, "F");
            CombatRoutine.AddSpell(ThunderFocusTea, 116680, "D6");
            CombatRoutine.AddSpell(ChiWave, 115098);
            CombatRoutine.AddSpell(ChiBurst, 123986);
            CombatRoutine.AddSpell(Detox, 115450);
            //Cov
            CombatRoutine.AddSpell(WeaponsofOrder, 310454, "Oem6");
            CombatRoutine.AddSpell(Fleshcraft, 324631, "OemOpenBrackets");
            CombatRoutine.AddSpell(FaelineStomp, 327104, "Oem6");
            CombatRoutine.AddSpell(FallenOrder, 326860, "Oem6");



            //Macros
            CombatRoutine.AddMacro(TAB, "Tab");
            CombatRoutine.AddMacro(Assist);
            CombatRoutine.AddMacro("Dismiss Totem");

            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);

            CombatRoutine.AddMacro(Player, "F8");
            CombatRoutine.AddMacro(Party1, "F9");
            CombatRoutine.AddMacro(Party2, "F10");
            CombatRoutine.AddMacro(Party3, "F11");
            CombatRoutine.AddMacro(Party4, "F12");

            CombatRoutine.AddMacro("stopcasting");
            CombatRoutine.AddMacro("raid1");
            CombatRoutine.AddMacro("raid2");
            CombatRoutine.AddMacro("raid3");
            CombatRoutine.AddMacro("raid4");
            CombatRoutine.AddMacro("raid5");
            CombatRoutine.AddMacro("raid6");
            CombatRoutine.AddMacro("raid7");
            CombatRoutine.AddMacro("raid8");
            CombatRoutine.AddMacro("raid9");
            CombatRoutine.AddMacro("raid10");
            CombatRoutine.AddMacro("raid11");
            CombatRoutine.AddMacro("raid12");
            CombatRoutine.AddMacro("raid13");
            CombatRoutine.AddMacro("raid14");
            CombatRoutine.AddMacro("raid15");
            CombatRoutine.AddMacro("raid16");
            CombatRoutine.AddMacro("raid17");
            CombatRoutine.AddMacro("raid18");
            CombatRoutine.AddMacro("raid19");
            CombatRoutine.AddMacro("raid20");
            CombatRoutine.AddMacro("raid21");
            CombatRoutine.AddMacro("raid22");
            CombatRoutine.AddMacro("raid23");
            CombatRoutine.AddMacro("raid24");
            CombatRoutine.AddMacro("raid25");
            CombatRoutine.AddMacro("raid26");
            CombatRoutine.AddMacro("raid27");
            CombatRoutine.AddMacro("raid28");
            CombatRoutine.AddMacro("raid29");
            CombatRoutine.AddMacro("raid30");
            CombatRoutine.AddMacro("raid31");
            CombatRoutine.AddMacro("raid32");
            CombatRoutine.AddMacro("raid33");
            CombatRoutine.AddMacro("raid34");
            CombatRoutine.AddMacro("raid35");
            CombatRoutine.AddMacro("raid36");
            CombatRoutine.AddMacro("raid37");
            CombatRoutine.AddMacro("raid38");
            CombatRoutine.AddMacro("raid39");
            CombatRoutine.AddMacro("raid40");

            //Buffs
            CombatRoutine.AddBuff(EnvelopingMist, 124682);
            CombatRoutine.AddBuff(RenewingMist, 119611);
            CombatRoutine.AddBuff(ThunderFocusTea, 116680);
            CombatRoutine.AddBuff(RefreshingJadeWind, 196725);
            CombatRoutine.AddBuff(EssenceFont, 191840);
            CombatRoutine.AddBuff(SoothingMist, 115175);
            CombatRoutine.AddBuff(AncientTeachingsOfTheMonastery, 347553);
            CombatRoutine.AddBuff(ChiJi, 343820);
            CombatRoutine.AddBuff(TeachingsOfTheMonMonastery, 202090);


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


            //Toggle
            //CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Range HEAL");
            CombatRoutine.AddToggle("Auto Detox");
            CombatRoutine.AddToggle("AOE Heal");
            CombatRoutine.AddToggle("Auto Target");
            CombatRoutine.AddToggle("NPC Heal");

            CombatRoutine.AddProp(RefreshingJadeWind, RefreshingJadeWind + " Life Percent", numbList, "Life percent at which" + RefreshingJadeWind + "is used when three members are at life percent, set to 0 to disable", "AOE Refreshing Jade Wind", 85);
            CombatRoutine.AddProp(RJWAOE, "Refreshing Jade Wind AoE Units ", numbPartyList, " Number of units for Refreshing Jade Wind AoE Healing", "AOE Refreshing Jade Wind", 3);



            CombatRoutine.AddProp(EssenceFont, EssenceFont + " Life Percent", numbList, "Life percent at which" + EssenceFont + "is used when three members are at life percent, set to 0 to disable", "AOE Essence Font", 85);
            CombatRoutine.AddProp(EFAOE, "Essence Font AoE Units ", numbPartyList, " Number of units for Essence Font AoE Healing", "AOE Essence Font", 2);

            CombatRoutine.AddProp(Revival, Revival + " Life Percent", numbList, "Life percent at which" + Revival + "is used, set to 0 to disable", "AOE Revival", 10);
            CombatRoutine.AddProp(RAOE, "Revival AoE Units ", numbPartyList, " Number of units for Essence Font AoE Healing", "AOE Revival", 4);

            CombatRoutine.AddProp(ChiJi, ChiJi + " Life Percent", numbList, "Life percent at which" + ChiJi + "is used when three members are at life percent, set to 0 to disable", "AOE ChiJi", 50);
            CombatRoutine.AddProp(ChiJiAOE, "ChiJi AoE Units ", numbPartyList, " Number of units for ChiJi AoE Healing", "AOE ChiJi", 4);

            CombatRoutine.AddProp(Yulon, Yulon + " Life Percent", numbList, "Life percent at which" + Yulon + "is used when three members are at life percent, set to 0 to disable", "AOE YuLon", 50);
            CombatRoutine.AddProp(YuLonAOE, "YuLon AoE Units ", numbPartyList, " Number of units for YuLon AoE Healing", "AOE YuLon", 4);


            CombatRoutine.AddProp(ChiBurst, ChiBurst + " Life Percent", numbList, "Life percent at which" + ChiBurst + "is used when three members are at life percent, set to 0 to disable", "AOE Chi Burst", 85);
            CombatRoutine.AddProp(ChiBurstAOE, "Chi Burst AoE Units ", numbPartyList, " Number of units for YuLon AoE Healing", "AOE Chi Burst", 1);

            CombatRoutine.AddProp(LifeCocoon, LifeCocoon + " Life Percent", numbList, "Life percent at which" + LifeCocoon + "is, set to 0 to disable", "Healing", 50);
            CombatRoutine.AddProp(LCT, "Life Cocoon Tank", true, "Use Life Cocoon only on Tank ? change to false, set to true by default", "Healing");

            CombatRoutine.AddProp(ThunderFocusTea, "Use " + ThunderFocusTea, ThunderFocusTeaList, "Use " + ThunderFocusTea + "always, Cooldowns, AOE", "Healing", 0);
            CombatRoutine.AddProp("SwapSpeed", "SwapSpeed", 750, "SwapSpeed", "Swap Speed");

            CombatRoutine.AddProp(ExpelHarm, ExpelHarm + " Life Percent", numbList, "Life percent at which" + ExpelHarm + "is used, set to 0 to disable", "Range Heal", 80);
            CombatRoutine.AddProp(EnvelopingMist, EnvelopingMist + " Life Percent", numbList, "Life percent at which" + EnvelopingMist + "is used, set to 0 to disable", "Range Heal", 80);
            CombatRoutine.AddProp(Vivify, Vivify + " Life Percent", numbList, "Life percent at which" + Vivify + "is used, set to 0 to disable", "Range Heal", 50);
            CombatRoutine.AddProp(SoothingMist, SoothingMist + " Life Percent", numbList, "Life percent at which" + SoothingMist + "is, set to 0 to disable", "Range Heal", 95);
            CombatRoutine.AddProp(RenewingMist, RenewingMist + " Life Percent", numbList, "Life percent at which" + RenewingMist + "is used, set to 0 to disable", "Range Heal", 95);

            CombatRoutine.AddProp(EmergencyGroupNumber, " Group Number", numbPartyList, " at which" + " We Stop Fistweaving", "Emergency Group", 1);
            CombatRoutine.AddProp(EmergencyGroupPercent, " Health Percent", numbList, " at which" + " We Stop Fistweaving", "Emergency Group", 50);

            CombatRoutine.AddProp(EmergencyRaidNumber, " Raid Number", numbList, " at which" + " We Stop Fistweaving", "Emergency Raid", 5);
            CombatRoutine.AddProp(EmergencyRaidPercent, " Health Percent", numbList, " at which" + " We Stop Fistweaving", "Emergency Raid", 50);

        }
        public override void Pulse()
        {
            if (RangeHeal)
            {
                if (AoEHeal && API.PlayerIsInCombat)
                {
                    if (API.CanCast(Yulon) && !TalentInvokeChiJi && YuLonAoE)
                    {
                        API.CastSpell(Yulon);
                        return;
                    }
                    if (API.CanCast(ChiJi) && TalentInvokeChiJi && ChiJiAoE)
                    {
                        API.CastSpell(ChiJi);
                        return;
                    }
                    if (API.CanCast(Revival) && RevivalAoE && !API.PlayerCanAttackTarget && !CurrentCastEssenceFont)
                    {
                        API.CastSpell(Revival);
                        return;
                    }
                    if (API.CanCast(RefreshingJadeWind) && TalentRefreshingJadeWind && RefreshingJadeWindAoE && !API.PlayerCanAttackTarget && !CurrentCastEssenceFont && API.PlayerIsInCombat && RefreshingJadeWindRange)
                    {
                        API.CastSpell(RefreshingJadeWind);
                        return;
                    }
                    if (API.CanCast(EssenceFont) && EssenceFontAoE && !API.PlayerCanAttackTarget && EssenceFontRange)
                    {
                        API.CastSpell(EssenceFont);
                        return;
                    }
                }
                if (API.CanCast(RenewingMist) && !ChannelSoothingMist && !CurrentCastEssenceFont && API.TargetHealthPercent <= RenewingMistPercent && !API.TargetHasBuff(RenewingMist) && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && (API.TargetIsIncombat || !API.TargetIsIncombat && NPCHeal))
                {
                    API.CastSpell(RenewingMist);
                    return;
                }
                if (API.CanCast(SoothingMist) && !CurrentCastEssenceFont && !API.TargetHasBuff(SoothingMist) && API.TargetHealthPercent <= SoothingMistPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && (API.TargetIsIncombat || !API.TargetIsIncombat && NPCHeal) && RangeCheck)
                {
                    API.CastSpell(SoothingMist);
                    return;
                }

                if (API.CanCast(EnvelopingMist) && ChannelSoothingMist && API.TargetHasBuff(SoothingMist) && !CurrentCastEssenceFont && !API.TargetHasBuff(EnvelopingMist) && API.TargetHealthPercent <= EnvelopingMistPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && (API.TargetIsIncombat || !API.TargetIsIncombat && NPCHeal) && RangeCheck)
                {
                    API.CastSpell(EnvelopingMist);
                    return;
                }
                if (API.CanCast(ExpelHarm) && ChannelSoothingMist && API.TargetHasBuff(SoothingMist) && !CurrentCastEssenceFont && API.TargetHealthPercent <= ExpelHarmtPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && (API.TargetIsIncombat || !API.TargetIsIncombat && NPCHeal) && RangeCheck)
                {
                    API.CastSpell(ExpelHarm);
                    return;
                }
                if (API.CanCast(Vivify) && ChannelSoothingMist && API.TargetHasBuff(SoothingMist) && !CurrentCastEssenceFont && API.TargetHealthPercent <= VivifyPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && (API.TargetIsIncombat || !API.TargetIsIncombat && NPCHeal) && RangeCheck)
                {
                    API.CastSpell(Vivify);
                    return;
                }
                if (AutoTarget && API.PlayerIsInGroup && !API.PlayerIsInRaid && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeed))
                {
                    API.CastSpell(LowestParty(units));
                    SwapWatch.Restart();
                    return;
                }
                if (AutoTarget && API.PlayerIsInRaid && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeed))
                {
                    API.CastSpell(LowestRaid(raidunits));
                    SwapWatch.Restart();
                    return;
                }
            }
            if (!RangeHeal && TalentInvokeChiJi && API.PlayerTotemPetDuration >= 1 && API.PlayerBuffStacks(ChiJi) < 3)
            {
                if (AutoTarget && API.PlayerIsInRaid && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(GetTankRaid(raidunits));
                    API.CastSpell(Assist);
                    return;
                }
                if (AutoTarget && API.PlayerIsInGroup && !API.PlayerIsInRaid && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(GetTankParty(units));
                    API.CastSpell(Assist);
                    return;
                }
                if (API.CanCast(RisingSunKick) && API.PlayerCanAttackTarget && !CurrentCastEssenceFont)
                {
                    API.CastSpell(RisingSunKick);
                    return;
                }
                if (API.CanCast(BlackoutKick) && API.PlayerCanAttackTarget && !CurrentCastEssenceFont)
                {
                    API.CastSpell(BlackoutKick);
                    return;
                }
                if (API.CanCast(SpinningCraneKick) && API.SpellISOnCooldown(RisingSunKick) && API.SpellISOnCooldown(BlackoutKick))
                {
                    API.CastSpell(SpinningCraneKick);
                }
            }

            if (!RangeHeal && Healing || API.PlayerBuffStacks(ChiJi) == 3)
            {
                if (API.CanCast(EnvelopingMist) && !CurrentCastEssenceFont && API.PlayerBuffStacks(ChiJi) == 3 && !API.TargetHasBuff(EnvelopingMist) && API.TargetHealthPercent <= 100 && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat && RangeCheck)
                {
                    API.CastSpell(EnvelopingMist);
                    return;
                }
                if (API.CanCast(LifeCocoon) && API.TargetHealthPercent <= LifeCocoonPercent && API.PlayerIsInGroup && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat && (LCTank && API.TargetRoleSpec == 999 || !LCTank) && RangeCheck)
                {
                    API.CastSpell(LifeCocoon);
                    return;
                }
                if (API.PlayerIsInGroup && !API.PlayerIsInRaid && API.CanCast(SoothingMist) && !CurrentCastEssenceFont && !API.TargetHasBuff(SoothingMist) && API.TargetHealthPercent <= 50 && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && RangeCheck)
                {
                    API.CastSpell(SoothingMist);
                    return;
                }
                if (API.PlayerIsInRaid && API.CanCast(SoothingMist) && !CurrentCastEssenceFont && !API.TargetHasBuff(SoothingMist) && API.TargetHealthPercent <= 30 && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && RangeCheck)
                {
                    API.CastSpell(SoothingMist);
                    return;
                }
                if (API.CanCast(EnvelopingMist) && ChannelSoothingMist && API.TargetHasBuff(SoothingMist) && !CurrentCastEssenceFont && !API.TargetHasBuff(EnvelopingMist) && API.TargetHealthPercent <= 51 && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat && RangeCheck)
                {
                    API.CastSpell(EnvelopingMist);
                    return;
                }
                if (API.CanCast(ExpelHarm) && ChannelSoothingMist && API.TargetHasBuff(SoothingMist) && !CurrentCastEssenceFont && API.TargetHealthPercent <= 75 && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat && RangeCheck)
                {
                    API.CastSpell(ExpelHarm);
                    return;
                }
                if (API.CanCast(Vivify) && ChannelSoothingMist && API.TargetHasBuff(SoothingMist) && !CurrentCastEssenceFont && API.TargetHealthPercent <= 60 && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat && RangeCheck)
                {
                    API.CastSpell(Vivify);
                    return;
                }
                if (AutoTarget && API.PlayerIsInGroup && !API.PlayerIsInRaid && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeed))
                {
                    API.CastSpell(LowestParty(units));
                    SwapWatch.Restart();
                    return;
                }
                if (AutoTarget && API.PlayerIsInRaid && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeed))
                {
                    API.CastSpell(LowestRaid(raidunits));
                    SwapWatch.Restart();
                    return;
                }
            }
            if (!RangeHeal && !API.PlayerCanAttackTarget && API.SpellCharges(RenewingMist) == 0)
            {
                if (AutoTarget && API.PlayerIsInGroup && !API.PlayerIsInRaid && !CurrentCastEssenceFont)
                {
                    if (API.PlayerIsTargetTarget)
                    {
                        API.CastSpell(GetTankParty(units));
                        API.CastSpell(Assist);
                        return;
                    }
                    if (AutoTarget && !API.PlayerIsTargetTarget && API.TargetIsIncombat)
                    {
                        API.CastSpell(GetTankParty(units));
                        API.CastSpell(Assist);
                        return;
                    }
                }
                if (AutoTarget && API.PlayerIsInRaid && !CurrentCastEssenceFont)
                {
                    if (API.PlayerIsTargetTarget)
                    {
                        API.CastSpell(GetTankRaid(raidunits));
                        API.CastSpell(Assist);
                        return;
                    }
                    if (!API.PlayerIsTargetTarget && API.TargetIsIncombat)
                    {
                        API.CastSpell(GetTankRaid(raidunits));
                        API.CastSpell(Assist);
                        return;
                    }
                }
            }


            if (!RangeHeal && Fistweaving && !ChannelSoothingMist)
            {
                if (AoEHeal && API.PlayerIsInCombat)
                {
                    if (API.CanCast(Yulon) && !TalentInvokeChiJi && YuLonAoE)
                    {
                        API.CastSpell(Yulon);
                        return;
                    }
                    if (API.CanCast(ChiJi) && TalentInvokeChiJi && ChiJiAoE)
                    {
                        API.CastSpell(ChiJi);
                        return;
                    }
                    if (API.CanCast(Revival) && RevivalAoE && !API.PlayerCanAttackTarget && !CurrentCastEssenceFont)
                    {
                        API.CastSpell(Revival);
                        return;
                    }
                    if (API.CanCast(RefreshingJadeWind) && TalentRefreshingJadeWind && RefreshingJadeWindAoE && !API.PlayerCanAttackTarget && !CurrentCastEssenceFont && API.PlayerIsInCombat && RefreshingJadeWindRange)
                    {
                        API.CastSpell(RefreshingJadeWind);
                        return;
                    }
                    if (API.CanCast(EssenceFont) && EssenceFontAoE && !API.PlayerCanAttackTarget && EssenceFontRange)
                    {
                        API.CastSpell(EssenceFont);
                        return;
                    }
                    if (API.CanCast(FaelineStomp) && PlayerCovenantSettings == "Night Fae" && UseFaelineStomp == "AOEHeal" && !CurrentCastEssenceFont)
                    {
                        API.CastSpell(FaelineStomp);
                        return;
                    }
                }
                if (API.TargetIsIncombat && API.CanCast(RenewingMist) && !CurrentCastEssenceFont && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(RenewingMist);
                    return;
                }

                if (AutoTarget && API.PlayerIsInGroup && !API.PlayerIsInRaid)
                {
                    if ((!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeed) && API.CanCast(RenewingMist) && API.PlayerIsInCombat && !CurrentCastEssenceFont)
                    {
                        API.CastSpell(RenewingMistBuffPartyTank(units));
                        SwapWatch.Restart();
                        return;
                    }
                    if ((!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeed) && API.CanCast(RenewingMist) && API.PlayerIsInCombat && !CurrentCastEssenceFont)
                    {
                        API.CastSpell(RenewingMistBuffParty(units));
                        SwapWatch.Restart();
                        return;
                    }
                }
                if (AutoTarget && API.PlayerIsInRaid)
                {
                    if ((!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeed) && API.CanCast(RenewingMist) && API.PlayerIsInCombat && !CurrentCastEssenceFont)
                    {
                        API.CastSpell(RenewingMistBuffRaidRaidTank(raidunits));
                        SwapWatch.Restart();
                        return;
                    }
                    if ((!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeed) && API.CanCast(RenewingMist) && API.PlayerIsInCombat && !CurrentCastEssenceFont)
                    {
                        API.CastSpell(RenewingMistBuffRaid(raidunits));
                        SwapWatch.Restart();
                        return;
                    }
                }
                //DPS
                if (API.CanCast(ThunderFocusTea) && API.CanCast(RisingSunKick) && API.SpellCharges(RenewingMist) == 0)
                {
                    API.CastSpell(ThunderFocusTea);
                    return;
                }
                if (isInterrupt && API.CanCast(LegSweep) && API.PlayerCanAttackTarget)
                {
                    API.CastSpell(LegSweep);
                    return;
                }
                if (API.CanCast(ChiBurst) && TalentChiBurst && ChiBurstAoE && API.PlayerIsInCombat && ChiBurstRange && !CurrentCastEssenceFont)
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                if (IsCooldowns && API.CanCast(TouchOfDeath) && API.TargetHealthPercent < 15 && API.PlayerCanAttackTarget)
                {
                    API.CastSpell(TouchOfDeath);
                    return;
                }
                if (API.CanCast(RisingSunKick) && API.PlayerCanAttackTarget && !CurrentCastEssenceFont && API.PlayerIsInCombat)
                {
                    API.CastSpell(RisingSunKick);
                    return;
                }
                if (API.CanCast(BlackoutKick) && API.PlayerBuffStacks(TeachingsOfTheMonMonastery) == 3 && API.PlayerCanAttackTarget && !CurrentCastEssenceFont && API.PlayerIsInCombat)
                {
                    API.CastSpell(BlackoutKick);
                    return;
                }
                if (IsAOE && API.PlayerUnitInMeleeRangeCount >= 2 && API.CanCast(SpinningCraneKick) && !CurrentCastEssenceFont && API.SpellISOnCooldown(RisingSunKick) && API.SpellISOnCooldown(BlackoutKick) && !API.PlayerHasBuff(AncientTeachingsOfTheMonastery))
                {
                    API.CastSpell(SpinningCraneKick);
                    return;
                }
                if (API.CanCast(TigerPalm) && API.PlayerCanAttackTarget && !CurrentCastEssenceFont && API.PlayerIsInCombat)
                {
                    API.CastSpell(TigerPalm);
                    return;
                }

            }
        }
        public override void CombatPulse()
        {

        }
        public override void OutOfCombatPulse()
        {

        }
    }
}
