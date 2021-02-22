using System.Linq;
using System.Diagnostics;

namespace HyperElk.Core
{
    public class DiscPriest : CombatRoutine
    {
        //Spell Strings
        private string PowerWordRadiance = "Power Word: Radiance";
        private string PurgetheWicked = "Purge the Wicked";
        private string MindSear = "Mind Sear";
        private string PainSupression = "Pain Supression";
        private string Rapture = "Rapture";
        private string Penance = "Penance";
        private string Shadowmend = "Shadow Mend";
        private string Shadowfiend = "Shadowfiend";
        private string Mindbender = "Mindbender";
        private string Atonement = "Atonement";
        private string PoweroftheDarkSide = "Power of the Dark Side";
        private string PowerWordBarrier = "Power Word: Barrier";
        private string Schism = "Schism";
        private string PowerWordSolace = "Power Word: Solace";
        private string ShadowCovenant = "Shadow Covenant";
        private string DivineStar = "Divine Star";
        private string SpiritShell = "Spirit Shell";
        private string Evangelism = "Evangelism";
        private string Halo = "Halo";
        private string ShadowWordPain = "Shadow Word: Pain";
        private string Smite = "Smite";
        private string HolyNova = "Holy Nova";
        private string PowerInfusion = "Power Infusion";
        private string ShadowWordDeath = "Shadow Word: Death";
        private string PowerWordShield = "Power Word: Shield";
        private string LeapOfFaith = "Leap of Faith";
        private string MindControl = "Mind Control";
        private string MassDispel = "Mass Dispel";
        private string ShackleUndead = "Shackle Undead";
        private string MindSoothe = "Mind Soothe";
        private string PowerWordFortitude = "Power Word: Fortitude";
        private string Fade = "Fade";
        private string DispelMagic = "Dispel Magic";
        private string PsychicScream = "Psychic Scream";
        private string BoonoftheAscended = "Boon of the Ascended";
        private string AscendedNova = "Ascended Nova";
        private string AscendedBlast = "Ascended Blast";
        private string Mindgames = "Mindgames";
        private string UnholyNova = "Unholy Nova";
        private string FaeGuardians = "Fae Guardians";
        private string Fleshcraft = "Fleshcraft";
        private string WeakenedSoul = "Weakened Soul";
        private string AoE = "AOE";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string Trinket1 = "Trinket1";
        private string Trinket2 = "Trinket2";
        private string AoERaid = "AOE Healing Raid";
        private string AoEDPS = "AOEDPS";
        private string AoEDPSRaid = "AOEDPS Raid";
        private string AoEDPSH = "AOEDPS Health";
        private string AoEDPSHRaid = "AOEDPS Health Raid";
        private string Party1 = "party1";
        private string Party2 = "party2";
        private string Party3 = "party3";
        private string Party4 = "party4";
        private string Player = "player";
        private string PartySwap = "Target Swap";
        private string AtonementParty = "Atonement Party";
        private string AtonementRaid = "Atonement Raid";
        private string Quake = "Quake";
        private string Purify = "Purify";
        private string MindBlast = "Mind Blast";
        private string DesperatePrayer = "Desperate Prayer";
        private string AngelicFeather = "Angelic Feather";


        //Talents
        bool CastigationTalent => API.PlayerIsTalentSelected(1, 1);
        bool TwistofFateTalent => API.PlayerIsTalentSelected(1, 2);
        bool SchismTalent => API.PlayerIsTalentSelected(1, 3);
        bool BodyandSoulTalent => API.PlayerIsTalentSelected(2, 1);
        bool MascoshimTalent => API.PlayerIsTalentSelected(2, 2);
        bool AngelicFeatherTalent => API.PlayerIsTalentSelected(2, 3);
        bool ShieldDisiplineTalent => API.PlayerIsTalentSelected(3, 1);
        bool MindbenderTalent => API.PlayerIsTalentSelected(3, 2);
        bool PowerWordSolaceTalent => API.PlayerIsTalentSelected(3, 3);
        bool PsychicVoiceTalent => API.PlayerIsTalentSelected(4, 1);
        bool DominantMindTalent => API.PlayerIsTalentSelected(4, 2);
        bool ShiningForceTalent => API.PlayerIsTalentSelected(4, 3);
        bool SinsoftheManyTalent => API.PlayerIsTalentSelected(5, 1);
        bool ContritionTalent => API.PlayerIsTalentSelected(5, 2);
        bool ShadowCovenantTalent => API.PlayerIsTalentSelected(5, 3);
        bool PurgetheWickedTalent => API.PlayerIsTalentSelected(6, 1);
        bool DivineStarTalent => API.PlayerIsTalentSelected(6, 2);
        bool HaloTalent => API.PlayerIsTalentSelected(6, 3);
        bool LenienceTalent => API.PlayerIsTalentSelected(7, 1);
        bool SpiritShellTalent => API.PlayerIsTalentSelected(7, 2);
        bool EvangelismTalent => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        int[] SwapSpeedList = new int[] { 0, 1000, 1250, 1500, 1750, 2000, 2250, 2500, 2750, 3000 };

        private int LowestHpPartyUnit()
        {
            int lowest = 100;

            for (int i = 0; i < units.Length; i++)
            {
                if (API.UnitHealthPercent(units[i]) < lowest && API.UnitHealthPercent(units[i]) > 0)
                    lowest = API.UnitHealthPercent(units[i]);
            }
            return lowest;
        }
        private int LowestHpRaidUnit()
        {
            int lowest = 100;

            for (int i = 0; i < raidunits.Length; i++)
            {
                if (API.UnitHealthPercent(raidunits[i]) < lowest && API.UnitHealthPercent(raidunits[i]) > 0)
                    lowest = API.UnitHealthPercent(raidunits[i]);
            }
            return lowest;
        }
        private string LowestParty(string[] units)
        {
            string lowest = units[0];
            int health = 100;
            foreach (string unit in units)
            {
                if (API.UnitHealthPercent(unit) < health && API.UnitRange(unit) <= 40 && API.UnitHealthPercent(unit) > 0)
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
                if (API.UnitHealthPercent(raidunit) < health && API.UnitRange(raidunit) <= 40 && API.UnitHealthPercent(raidunit) > 0)
                {
                    lowest = raidunit;
                    health = API.UnitHealthPercent(raidunit);
                }
            }
            return lowest;
        }

        private static readonly Stopwatch SwapWatch = new Stopwatch();
        int PlayerHealth => API.TargetHealthPercent;
        string[] PlayerTargetArray = { "player", "party1", "party2", "party3", "party4" };
        string[] RaidTargetArray = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };

        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        string[] DispellList = { "Chilled", "Frozen Binds", "Clinging Darkness", "Rasping Scream", "Heaving Retch", "Goresplatter", "Slime Injection", "Gripping Infection", "Repulsive Visage", "Soul Split", "Anima Injection", "Bewildering Pollen", "Bramblethorn Entanglement", "Sinlight Visions", "Siphon Life", "Turn to Stone", "Stony Veins", "Cosmic Artifice", "Wailing Grief", "Shadow Word:  Pain", "Anguished Cries", "Wrack Soul", "Dark Lance", "Insidious Venom", "Charged Anima", "Lost Confidence", "Burden of Knowledge", "Internal Strife", "Forced Confession", "Insidious Venom 2", "Soul Corruption", "Debilitating Plague", "Burning Strain", "Blightbeak", "Corroded Claws", "Wasting Blight", "Hurl Spores", "Corrosive Gunk", "Genetic Alteration", "Withering Blight", "Decaying Blight", "Burst" };
        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);
        private int UnitAboveHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) >= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitAboveHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) >= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitAboveHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitAboveHealthPercentRaid(HealthPercent) : UnitAboveHealthPercentParty(HealthPercent);
        private int BuffRaidTracking(string buff) => raidunits.Count(p => API.UnitHasBuff(buff, p));
        private int BuffPartyTracking(string buff) => units.Count(p => API.UnitHasBuff(buff, p));
        private int BuffTracking(string buff) => API.PlayerIsInRaid ? BuffRaidTracking(buff) : BuffPartyTracking(buff);
        private int RangePartyTracking(int Range) => units.Count(p => API.UnitRange(p) <= Range);
        private int RangeRaidTracking(int Range) => raidunits.Count(p => API.UnitRange(p) <= Range);
        private int RangeTracking(int Range) => API.PlayerIsInRaid ? RangeRaidTracking(Range) : RangePartyTracking(Range);

        // bool ChannelingCov => API.CurrentCastSpellID("player") == 323764;
        bool ChannelingPenance => API.CurrentCastSpellID("player") == 47540;
        bool ChannelingMindSear => API.CurrentCastSpellID("player") == 48045;

        
        private bool AttonementTracking => API.PlayerIsInRaid ? BuffRaidTracking(Atonement) >= AtoneRaidNumber : BuffPartyTracking(Atonement) >= AtonePartyNumber;
        private bool PowerWordRadAoE => UnitBelowHealthPercent(PowerWordRadLifePercent) >= AoENumber;
        private bool PowerWordBarrierAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(PowerWordBarLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(PowerWordBarLifePercent) >= AoENumber;
        private bool ShadowCovAoE => UnitBelowHealthPercent(ShadowCovLifePercent) >= AoENumber;
        private bool SchimAoE => UnitBelowHealthPercent(SchismLifePercent) >= AoENumber;
        private bool HaloAoE => UnitBelowHealthPercent(HaloLifePercent) >= AoENumber && !API.PlayerCanAttackTarget;
        private bool BoonAoE => UnitBelowHealthPercent(BoonLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool UnholyAoE => UnitBelowHealthPercent(UnholyNovaLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool EvagAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(EvangelismLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(EvangelismLifePercent) >= AoENumber;
        private bool SSAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(SSLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(SSLifePercent) >= AoENumber;
        private bool RapAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(RaptureLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(RaptureLifePercent) >= AoENumber;
        private bool DivineStarAoE => UnitBelowHealthPercent(DivineStarLifePercent) >= AoENumber && API.PlayerIsInRaid ? RangeRaidTracking(24) >= AoERaidNumber : RangePartyTracking(24) >= AoENumber;

        private bool HaloCheck => API.CanCast(Halo) && HaloAoE && !ChannelingPenance && Mana >= 3 && HaloTalent && !API.PlayerIsMoving;
        private bool PWSCheck => API.CanCast(PowerWordShield) && PlayerHealth <= PowerWordShieldifePercent && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingMindSear && !ChannelingPenance && !API.PlayerCanAttackTarget && (API.PlayerHasBuff(Rapture) || !API.TargetHasDebuff(WeakenedSoul));
        private bool PWRCheck => API.CanCast(PowerWordRadiance) && PowerWordRadAoE && !API.PlayerIsMoving && !ChannelingPenance && !ChannelingMindSear && !API.PlayerCanAttackTarget && RangeTracking(30) >= 3;
        private bool EvagCheck => API.CanCast(Evangelism) && EvangelismTalent && AttonementTracking && EvagAoE && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingMindSear && !ChannelingPenance;
        private bool SchismCheck => API.CanCast(Schism) && SchismTalent && AttonementTracking && !API.PlayerIsMoving && !ChannelingPenance && !ChannelingMindSear && API.PlayerCanAttackTarget && API.TargetHealthPercent > 0;
        private bool SpiritCheck => API.CanCast(SpiritShell) && SpiritShellTalent && SSAoE && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingMindSear && !ChannelingPenance;
        private bool RaptureCheck => API.CanCast(Rapture) && RapAoE && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingMindSear && !ChannelingPenance && !API.PlayerCanAttackTarget;
        private bool ShadowMendCheck => API.CanCast(Shadowmend) && !API.PlayerIsMoving && PlayerHealth <= ShadowMendLifePercent && !API.PlayerCanAttackTarget;
        private bool PowerWordBarrierCheck => API.CanCast(PowerWordBarrier) && PowerWordBarrierAoE && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool PainSupressionCheck => API.CanCast(PainSupression) && PlayerHealth <= PainSupressionLifePercent && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool KyrianCheck => API.CanCast(BoonoftheAscended) && PlayerCovenantSettings == "Kyrian" && BoonAoE && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE") && NotChanneling  && !API.PlayerIsMoving && !ChannelingPenance;
        private bool NightFaeCheck => API.CanCast(FaeGuardians) && PlayerCovenantSettings == "Night Fae" && Mana >= 2 && API.TargetHealthPercent >= FaeLifePercent && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !ChannelingPenance;
        private bool NecrolordCheck => API.CanCast(UnholyNova) && PlayerCovenantSettings == "Necrolord" && UnholyAoE && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingPenance;
        private bool VenthyrCheck => API.CanCast(Mindgames) && PlayerCovenantSettings == "Venthyr" && Mana >= 2 && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingPenance;


        private bool Quaking => ((API.PlayerCurrentCastTimeRemaining >= 200 || API.PlayerIsChanneling) && API.PlayerDebuffRemainingTime(Quake) < 200) && PlayerHasDebuff(Quake);
        private bool SaveQuake => (PlayerHasDebuff(Quake) && API.PlayerDebuffRemainingTime(Quake) > 200 && QuakingHelper || !PlayerHasDebuff(Quake) || !QuakingHelper);
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool IsAutoSwap => API.ToggleIsEnabled("Auto Target");
        private bool IsOOC => API.ToggleIsEnabled("OOC");
        private int HaloLifePercent => numbList[CombatRoutine.GetPropertyInt(Halo)];
        private int BoonLifePercent => numbList[CombatRoutine.GetPropertyInt(BoonoftheAscended)];
      //  private int MindgamesLifePercent => numbList[CombatRoutine.GetPropertyInt(Mindgames)];
        private int UnholyNovaLifePercent => numbList[CombatRoutine.GetPropertyInt(UnholyNova)];
        private int PowerWordRadLifePercent => numbList[CombatRoutine.GetPropertyInt(PowerWordRadiance)];
        private int PenanceLifePercent => numbList[CombatRoutine.GetPropertyInt(Penance)];
        private int PowerWordBarLifePercent => numbList[CombatRoutine.GetPropertyInt(PowerWordBarrier)];
        private int PowerWordShieldifePercent => numbList[CombatRoutine.GetPropertyInt(PowerWordShield)];
        private int PainSupressionLifePercent => numbList[CombatRoutine.GetPropertyInt(PainSupression)];
        private int ShadowMendLifePercent => numbList[CombatRoutine.GetPropertyInt(Shadowmend)];
        private int RaptureLifePercent => numbList[CombatRoutine.GetPropertyInt(Rapture)];
        private int DivineStarLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineStar)];
        private int ShadowfiendLifePercent => numbList[CombatRoutine.GetPropertyInt(Shadowfiend)];
        private int ShadowCovLifePercent => numbList[CombatRoutine.GetPropertyInt(ShadowCovenant)];
        private int SSLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritShell)];
        private int EvangelismLifePercent => numbList[CombatRoutine.GetPropertyInt(Evangelism)];
        private int SchismLifePercent => numbList[CombatRoutine.GetPropertyInt(Schism)];
        private int FaeLifePercent => numbList[CombatRoutine.GetPropertyInt(FaeGuardians)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoERaid)];
        private int AtonePartyNumber => numbPartyList[CombatRoutine.GetPropertyInt(AtonementParty)];
        private int AtoneRaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AtonementRaid)];
        private int AoEDPSNumber => numbPartyList[CombatRoutine.GetPropertyInt(AoEDPS)];
        private int AoEDPSRaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoEDPSRaid)];
        private int AoEDPSHLifePercent => numbList[CombatRoutine.GetPropertyInt(AoEDPSH)];
        private int AoEDPSHRaidLifePercent => numbList[CombatRoutine.GetPropertyInt(AoEDPSHRaid)];
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private int DesperatePrayerProc => numbList[CombatRoutine.GetPropertyInt(DesperatePrayer)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        //private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoER)];
        private bool IsDispell => API.ToggleIsEnabled("Dispel");
        private bool QuakingMind => API.PlayerDebuffRemainingTime(Quake) > MindCastTime && PlayerHasDebuff(Quake);
        private bool QuakingSmite => API.PlayerDebuffRemainingTime(Quake) > SmiteCastTime && PlayerHasDebuff(Quake);
        private bool QuakingBoon => API.PlayerDebuffRemainingTime(Quake) > BoonCastTime && PlayerHasDebuff(Quake);
        private bool QuakingPowerWordRad => API.PlayerDebuffRemainingTime(Quake) > PowerWordRadCastTime && PlayerHasDebuff(Quake);
        private bool QuakingHalo => API.PlayerDebuffRemainingTime(Quake) > HaloCastTime && PlayerHasDebuff(Quake);
        private bool QuakingSchism => API.PlayerDebuffRemainingTime(Quake) > SchismCastTime && PlayerHasDebuff(Quake);
        private bool QuakingShadow => API.PlayerDebuffRemainingTime(Quake) > ShadowMendCastTime && PlayerHasDebuff(Quake);
        private bool QuakingPenance => API.PlayerDebuffRemainingTime(Quake) > PenanceCastTime && PlayerHasDebuff(Quake);
        private bool QuakingMindblast => API.PlayerDebuffRemainingTime(Quake) > MindblastCastTime && PlayerHasDebuff(Quake);
        private bool QuakingMindSear => API.PlayerDebuffRemainingTime(Quake) > MindSearCastTime && PlayerHasDebuff(Quake);
        float PowerWordRadCastTime => 200f / (1f + API.PlayerGetHaste);
        float HaloCastTime => 150f / (1f + API.PlayerGetHaste);
        float SchismCastTime => 150f / (1f + API.PlayerGetHaste);
        float PenanceCastTime => 200f / (1f + API.PlayerGetHaste);
        float MindblastCastTime => 150f / (1f + API.PlayerGetHaste);
        float SmiteCastTime => 150f / (1f + API.PlayerGetHaste);
        float BoonCastTime => 150f / (1f + API.PlayerGetHaste);
        float MindCastTime => 150f / (1f + API.PlayerGetHaste);
        float MindSearCastTime => 450f / (1f + API.PlayerGetHaste);
        float ShadowMendCastTime => 150f / (1f + API.PlayerGetHaste);
        private bool QuakingHelper => CombatRoutine.GetPropertyBool("QuakingHelper");
        private bool UseAngel => CombatRoutine.GetPropertyBool(AngelicFeather);
        private static bool TargetHasDispellAble(string debuff)
        {
            return API.TargetHasDebuff(debuff, false, true);
        }
        private static bool MouseouverHasDispellAble(string debuff)
        {
            return API.MouseoverHasDebuff(debuff, false, true);
        }
        private static bool UnitHasDispellAble(string debuff, string unit)
        {
            return API.UnitHasDebuff(debuff, unit, false, true);
        }
        private static bool UnitHasBuff(string buff, string unit)
        {
            return API.UnitHasBuff(buff, unit, true, true);
        }
        private static bool PlayerHasDebuff(string buff)
        {
            return API.PlayerHasDebuff(buff, false, false);
        }

        //General
        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private bool IsMelee => API.TargetRange < 12;
       // private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        bool IsTrinkets1 => (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= 2 && IsAOE);
        bool IsTrinkets2 => (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= 2 && IsAOE);
        private int Mana => API.PlayerMana;
        private bool IsSpread => API.ToggleIsEnabled("Attonement Spread");
        


        //  public bool isInterrupt => CombatRoutine.GetPropertyBool("KICK") && API.TargetCanInterrupted && API.TargetIsCasting && (API.TargetIsChanneling ? API.TargetElapsedCastTime >= interruptDelay : API.TargetCurrentCastTimeRemaining <= interruptDelay);
        //  public int interruptDelay => random.Next((int)(CombatRoutine.GetPropertyInt("KICKTime") * 0.9), (int)(CombatRoutine.GetPropertyInt("KICKTime") * 1.1));

        public override void Initialize()
        {
            CombatRoutine.Name = "Disc Priest by Ryu";
            API.WriteLog("Welcome to Disc Priest by Ryu");
            API.WriteLog("MAJOR BETA ROTATION : Things may be missing or not work correctly yet. Please post feedback in Priest channel.");
            API.WriteLog("If you are aware of incoming damage and wish to prep for it, please use the Attonment Spread Toggle. Otherwise, it will spread atonement via the health percents you've set for PWS, PWR and ShadowMend");
            // API.WriteLog("Mouseover Support is added. Please create /cast [@mouseover] xx whereas xx is your spell and assign it the binds with MO on it in keybinds.");
            API.WriteLog("If you use Angelic Feather, please create a @player marco, it will use it while moving. If you want to use it, change the setting to True. It is false by default.");
            API.WriteLog("For all ground spells, either use @Cursor or when it is time to place it, the Bot will pause until you've placed it. If you'd perfer to use your own logic for them, please place them on ignore in the spellbook.");
            API.WriteLog("If you wish to use Auto Target, please set your WoW keybinds in the keybinds => Targeting for Self, Party, and Assist Target and then match them to the Macro's's in the spell book. Enable it the Toggles. You must at least have a target for it to swap, friendly or enemy. UNDER TESTING : It can swap back to an enemy, but YOU WILL NEED TO ASSIGN YOUR ASSIST TARGET KEY IT WILL NOT WORK IF YOU DONT DO THIS. If you DO NOT want it to do target enemy swapping, please IGNORE Assist Macro in the Spellbook. This works for both raid and party, however, you must set up the binds. Please watch video in the Discord");

            //Buff
            CombatRoutine.AddBuff(PowerWordFortitude, 21562);
            CombatRoutine.AddBuff(PowerWordShield, 17);
            CombatRoutine.AddBuff(PoweroftheDarkSide, 198069);
            CombatRoutine.AddBuff(Atonement, 194384);
            CombatRoutine.AddBuff(Rapture, 47536);




          //Debuff
          CombatRoutine.AddDebuff(WeakenedSoul, 6788);
            CombatRoutine.AddDebuff(ShadowWordPain, 589);
            CombatRoutine.AddDebuff(Quake, 240447);
            CombatRoutine.AddDebuff(PurgetheWicked, 204213);
            //Debuff Dispell
            CombatRoutine.AddDebuff("Chilled", 328664);
            CombatRoutine.AddDebuff("Frozen Binds", 320788);
            CombatRoutine.AddDebuff("Clinging Darkness", 323347);
            CombatRoutine.AddDebuff("Rasping Scream", 324293);
            CombatRoutine.AddDebuff("Heaving Retch", 320596);
            CombatRoutine.AddDebuff("Goresplatter", 338353);
            CombatRoutine.AddDebuff("Slime Injection", 329110);
            CombatRoutine.AddDebuff("Gripping Infection", 328180);
            CombatRoutine.AddDebuff("Repulsive Visage", 328756);
            CombatRoutine.AddDebuff("Soul Split", 322557);
            CombatRoutine.AddDebuff("Anima Injection", 325224);
            CombatRoutine.AddDebuff("Bewildering Pollen", 321968);
            CombatRoutine.AddDebuff("Bramblethorn Entanglement", 324859);
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
            CombatRoutine.AddDebuff("Insidious Venom 2", 317661);
            CombatRoutine.AddDebuff("Soul Corruption", 333708);
            CombatRoutine.AddDebuff("Debilitating Plague", 324652);
            CombatRoutine.AddDebuff("Burning Strain", 322358);
            CombatRoutine.AddDebuff("Blightbeak", 327882);
            CombatRoutine.AddDebuff("Corroded Claws", 320512);
            CombatRoutine.AddDebuff("Wasting Blight", 320542);
            CombatRoutine.AddDebuff("Hurl Spores", 328002);
            CombatRoutine.AddDebuff("Corrosive Gunk", 319070);
            CombatRoutine.AddDebuff("Genetic Alteration", 320248);
            CombatRoutine.AddDebuff("Withering Blight", 341949);
            CombatRoutine.AddDebuff("Decaying Blight", 330700);
            CombatRoutine.AddDebuff("Gluttonous Miasma", 329298);
            CombatRoutine.AddDebuff("Burst", 240443);
            //Spell
            CombatRoutine.AddSpell(Smite, 585);
            CombatRoutine.AddSpell(HolyNova, 132157);
            CombatRoutine.AddSpell(Fade, 586);
            CombatRoutine.AddSpell(DispelMagic, 528);
            CombatRoutine.AddSpell(LeapOfFaith, 73325);
            CombatRoutine.AddSpell(MindControl, 136287);
            CombatRoutine.AddSpell(MassDispel, 32375);
            CombatRoutine.AddSpell(ShackleUndead, 9484);
            CombatRoutine.AddSpell(Fleshcraft, 324631);
            CombatRoutine.AddSpell(MindSoothe, 453);
            CombatRoutine.AddSpell(PowerInfusion, 10060);
            CombatRoutine.AddSpell(PsychicScream, 8122);
            CombatRoutine.AddSpell(BoonoftheAscended, 325013);
            CombatRoutine.AddSpell(AscendedBlast, 325315);
            CombatRoutine.AddSpell(AscendedNova, 325020);
            CombatRoutine.AddSpell(Mindgames, 323673);
            CombatRoutine.AddSpell(UnholyNova, 347788);
            CombatRoutine.AddSpell(FaeGuardians, 327661);
            CombatRoutine.AddSpell(Halo, 120517);
            CombatRoutine.AddSpell(ShadowWordPain, 589);
            CombatRoutine.AddSpell(PowerWordFortitude, 21562);
            CombatRoutine.AddSpell(PowerWordRadiance, 194509);
            CombatRoutine.AddSpell(PurgetheWicked, 204197);
            CombatRoutine.AddSpell(MindSear, 48045);
            CombatRoutine.AddSpell(PainSupression, 33206);
            CombatRoutine.AddSpell(Rapture, 47536);
            CombatRoutine.AddSpell(Penance, 47540);
            CombatRoutine.AddSpell(Shadowmend, 186263);
            CombatRoutine.AddSpell(PowerWordBarrier, 62618);
            CombatRoutine.AddSpell(Shadowfiend, 34433);
            CombatRoutine.AddSpell(Schism, 214621);
            CombatRoutine.AddSpell(Mindbender, 123040);
            CombatRoutine.AddSpell(PowerWordSolace, 129250);
            CombatRoutine.AddSpell(ShadowCovenant, 314867);
            CombatRoutine.AddSpell(DivineStar, 110744);
            CombatRoutine.AddSpell(SpiritShell, 109964);
            CombatRoutine.AddSpell(Evangelism, 246287);
            CombatRoutine.AddSpell(MindBlast, 8092);
            CombatRoutine.AddSpell(Purify, 527);
            CombatRoutine.AddSpell(PowerWordShield, 17);
            CombatRoutine.AddSpell(AngelicFeather, 121536);
            CombatRoutine.AddSpell(DesperatePrayer, 19236);
            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Toggle
            CombatRoutine.AddToggle("Auto Target");
            CombatRoutine.AddToggle("OOC");
            CombatRoutine.AddToggle("Dispel");
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Attonement Spread");

            //Macro
            CombatRoutine.AddMacro(Trinket1);
            CombatRoutine.AddMacro(Trinket2);
            CombatRoutine.AddMacro(Purify + "MO");
            CombatRoutine.AddMacro("Stopcast", "F10");
            CombatRoutine.AddMacro(Player);
            CombatRoutine.AddMacro(Party1);
            CombatRoutine.AddMacro(Party2);
            CombatRoutine.AddMacro(Party3);
            CombatRoutine.AddMacro(Party4);
            CombatRoutine.AddMacro("Assist");
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

            //Prop
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 0);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 0);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 0);
            CombatRoutine.AddProp(DesperatePrayer, DesperatePrayer + " Life Percent", numbList, " Life percent at which" + DesperatePrayer + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used and meets the below healing perecents", "Cooldowns", 1);
            CombatRoutine.AddProp("QuakingHelper", "Quaking Helper", false, "Will cancel casts on Quaking", "Generic");
            CombatRoutine.AddProp(AngelicFeather, AngelicFeather, false, "Use Angelic Feather if talented", "Movement");


            CombatRoutine.AddProp(FaeGuardians, FaeGuardians + " Life Percent", numbList, "Life percent at which " + FaeGuardians + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(Shadowmend, Shadowmend + " Life Percent", numbList, "Life percent at which " + Shadowmend + " is used, set to 0 to disable", "Healing", 75);
            CombatRoutine.AddProp(PowerWordShield, PowerWordShield + " Life Percent", numbList, "Life percent at which " + PowerWordShield + " is used, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(PainSupression, PainSupression + " Life Percent", numbList, "Life percent at which " + PainSupression + " is used, set to 0 to disable", "Healing", 25);
            CombatRoutine.AddProp(PowerWordBarrier, PowerWordBarrier + " Life Percent", numbList, "Life percent at which " + PowerWordBarrier + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 45);
            CombatRoutine.AddProp(UnholyNova, UnholyNova + " Life Percent", numbList, "Life percent at which " + UnholyNova + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 45);
            CombatRoutine.AddProp(PowerWordRadiance, PowerWordRadiance + " Life Percent", numbList, "Life percent at which " + PowerWordRadiance + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(Rapture, Rapture + " Life Percent", numbList, "Life percent at which " + Rapture + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 50);
            CombatRoutine.AddProp(SpiritShell, SpiritShell + " Life Percent", numbList, "Life percent at which " + SpiritShell + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 50);
          //  CombatRoutine.AddProp(ShadowCovenant, ShadowCovenant + " Life Percent", numbList, "Life percent at which " + ShadowCovenant + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 50);
            CombatRoutine.AddProp(Evangelism, Evangelism + " Life Percent", numbList, "Life percent at which " + Evangelism + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 50);
            CombatRoutine.AddProp(Halo, Halo + " Life Percent", numbList, "Life percent at which " + Halo + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(DivineStar, DivineStar + " Life Percent", numbList, "Life percent at which " + DivineStar + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(BoonoftheAscended, BoonoftheAscended + " Life Percent", numbList, "Life percent at which " + BoonoftheAscended + " is used when AoE Number of members are at life percent, if is your Cov, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", CDUsageWithAOE, "When should trinket1 be used", "Trinket", 1);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", CDUsageWithAOE, "When should trinket2 be used", "Trinket", 1);
            CombatRoutine.AddProp(AoE, "Number of units for AoE Healing ", numbPartyList, " Units for AoE Healing", "Healing", 3);
            CombatRoutine.AddProp(AtonementParty, "Number of units that have attonement for DPS in party ", numbPartyList, " Units for Attonment", "Healing", 3);
            CombatRoutine.AddProp(AtonementRaid, "Number of units for that have attonement for DPS in raid ", numbRaidList, " Units for Attonement in raid", "Healing", 7);
            CombatRoutine.AddProp(AoEDPS, "Number of units needed to be above DPS Health Percent to DPS in party ", numbPartyList, " Units above for DPS ", "Healing", 2);
            CombatRoutine.AddProp(AoEDPSH, "Life Percent for units to be above for DPS", numbList, "Health percent at which DPS in party" + "is used,", "Healing", 80);
            CombatRoutine.AddProp(AoEDPSRaid, "Number of units needed to be above DPS Health Percent to DPS in Raid ", numbRaidList, " Units above for DPS ", "Healing", 4);
            CombatRoutine.AddProp(AoEDPSHRaid, "Life Percent for units to be above for DPS in raid", numbList, "Health percent at which DPS" + "is used,", "Healing", 70);
            CombatRoutine.AddProp(AoERaid, "Number of units for AoE Healing in raid ", numbRaidList, " Units for AoE Healing in raid", "Healing", 7);


        }

        public override void Pulse()
        {

            if (!API.PlayerIsMounted && !API.PlayerSpellonCursor && (IsOOC || API.PlayerIsInCombat) && (!API.TargetHasDebuff("Gluttonous Miasma") || !API.MouseoverHasDebuff("Gluttonous Miasma") && IsMouseover))
            {
                if (API.PlayerCurrentCastTimeRemaining > 40 && QuakingHelper && Quaking)
                {
                    API.CastSpell("Stopcast");
                    API.WriteLog("Debuff Time Remaining for Quake : " + API.PlayerDebuffRemainingTime(Quake));
                    return;
                }
                if (API.CanCast(DesperatePrayer) && API.PlayerHealthPercent <= DesperatePrayerProc)
                {
                    API.CastSpell(DesperatePrayer);
                    return;
                }
                if (API.CanCast(AngelicFeather) && AngelicFeatherTalent && API.PlayerIsMoving && UseAngel)
                {
                    API.CastSpell(AngelicFeather);
                    return;
                }
                #region Dispell
                if (IsDispell)
                {
                    if (API.CanCast(Purify) && !ChannelingPenance && !ChannelingMindSear && NotChanneling)
                    {
                        for (int i = 0; i < DispellList.Length; i++)
                        {
                            if (TargetHasDispellAble(DispellList[i]))
                            {
                                API.CastSpell(Purify);
                                return;
                            }
                        }
                    }
                    if (API.CanCast(Purify) && IsMouseover && !ChannelingPenance && !ChannelingMindSear && NotChanneling)
                    {
                        for (int i = 0; i < DispellList.Length; i++)
                        {
                            if (MouseouverHasDispellAble(DispellList[i]))
                            {
                                API.CastSpell(Purify + "MO");
                                return;
                            }
                        }
                    }
                }
                #endregion
                if (IsSpread)
                {
                    for (int i = 0; i < units.Length; i++)
                    for (int t = 0; t < raidunits.Length; t++)
                        {
                            if (API.CanCast(PowerWordRadiance) && API.SpellCharges(PowerWordRadiance) == 2 && (API.PlayerIsInGroup && BuffPartyTracking(Atonement) < 5 || API.PlayerIsInRaid && BuffRaidTracking(Atonement) < 12) && InRange && !API.PlayerCanAttackTarget && (!QuakingPowerWordRad || QuakingPowerWordRad && QuakingHelper))
                            {
                                API.CastSpell(PowerWordRadiance);
                                return;
                            }
                            if (API.CanCast(PowerWordRadiance) && API.SpellCharges(PowerWordRadiance) == 1 && InRange && (API.PlayerIsInGroup && BuffPartyTracking(Atonement) < 5 || API.PlayerIsInRaid && BuffRaidTracking(Atonement) < 12) && !API.PlayerCanAttackTarget && (!QuakingPowerWordRad || QuakingPowerWordRad && QuakingHelper))
                        {
                            API.CastSpell(PowerWordRadiance);
                            return;
                        }
                        if (API.CanCast(PowerWordShield) && !API.TargetHasBuff(Atonement) && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingMindSear && !ChannelingPenance && !API.PlayerCanAttackTarget && (API.PlayerHasBuff(Rapture) || !API.TargetHasDebuff(WeakenedSoul)))
                            {
                                API.CastSpell(PowerWordShield);
                                return;
                            }
                        if (API.CanCast(Shadowmend) && !API.TargetHasBuff(Atonement) && !API.PlayerIsMoving && (!QuakingShadow || QuakingShadow && QuakingHelper))
                        {
                            API.CastSpell(Shadowmend);
                            return;
                        }
                    }
                }
                if (PWRCheck && InRange && (!QuakingPowerWordRad || QuakingPowerWordRad && QuakingHelper) && API.PlayerLastSpell != PowerWordRadiance)
                {
                    API.CastSpell(PowerWordRadiance);
                    return;
                }
                if (PowerWordBarrierCheck && InRange)
                {
                    API.CastSpell(PowerWordBarrier);
                    return;
                }
                if (RaptureCheck && InRange)
                {
                    API.CastSpell(Rapture);
                    return;
                }
                if (SpiritCheck && InRange)
                {
                    API.CastSpell(SpiritShell);
                    return;
                }
                if (EvagCheck && InRange)
                {
                    API.CastSpell(Evangelism);
                    return;
                }
                if (PainSupressionCheck && InRange)
                {
                    API.CastSpell(PainSupression);
                    return;
                }
                if (HaloCheck && InRange && (!QuakingHalo || QuakingHalo && QuakingHelper))
                {
                    API.CastSpell(Halo);
                    return;
                }
                if (API.CanCast(DivineStar) && DivineStarTalent && DivineStarAoE && InRange && (API.PlayerCanAttackTarget || !API.PlayerCanAttackTarget))
                {
                    API.CastSpell(DivineStar);
                    return;
                }
                if (PWSCheck && InRange)
                {
                    API.CastSpell(PowerWordShield);
                    return;
                }
                if (ShadowMendCheck && InRange && (!QuakingShadow || QuakingShadow && QuakingHelper))
                {
                    API.CastSpell(Shadowmend);
                    return;
                }
                if (NecrolordCheck && InRange)
                {
                    API.CastSpell(UnholyNova);
                    return;
                }
                if (KyrianCheck && InRange)
                {
                    API.CastSpell(BoonoftheAscended);
                    return;
                }
                if (NightFaeCheck && InRange)
                {
                    API.CastSpell(FaeGuardians);
                    return;
                }
 
                //DPS
                if (VenthyrCheck && InRange && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget && (!QuakingMind || QuakingMind && QuakingHelper))
                {
                    API.CastSpell(Mindgames);
                    return;
                }
                if (API.CanCast(Shadowfiend) && API.PlayerCanAttackTarget && API.PlayerMana <= 65 && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Shadowfiend);
                    return;
                }
                if (API.CanCast(Mindbender) && MindbenderTalent && API.PlayerCanAttackTarget && API.PlayerMana <= 80 && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Mindbender);
                    return;
                }
                if (API.CanCast(ShadowWordPain) && InRange && Mana >= 1 && !API.TargetHasDebuff(ShadowWordPain) && (API.PlayerIsMoving || !API.PlayerIsMoving) && !ChannelingPenance && !ChannelingMindSear && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget && !PurgetheWickedTalent)
                {
                    API.CastSpell(ShadowWordPain);
                    return;
                }
                if (API.CanCast(PurgetheWicked) && InRange && Mana >= 1 && !API.TargetHasDebuff(PurgetheWicked) && (API.PlayerIsMoving || !API.PlayerIsMoving) && !ChannelingPenance && !ChannelingMindSear && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget && PurgetheWickedTalent)
                {
                    API.CastSpell(PurgetheWicked);
                    return;
                }
                if (SchismCheck && InRange && (!QuakingSchism || QuakingSchism && QuakingHelper))
                {
                    API.CastSpell(Schism);
                    return;
                }
                if (API.CanCast(PowerWordSolace) && PowerWordSolaceTalent && AttonementTracking && InRange && API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(PowerWordSolace);
                    return;
                }
                if (API.CanCast(AscendedNova) && PlayerCovenantSettings == "Kyrian" && API.TargetRange <= 8 && !ChannelingPenance && !ChannelingMindSear && (API.PlayerIsMoving || !API.PlayerIsMoving) && API.TargetHealthPercent > 0)
                {
                    API.CanCast(AscendedNova);
                    return;
                }
                if (API.CanCast(AscendedBlast) && PlayerCovenantSettings == "Kyrian" && !ChannelingPenance && !ChannelingMindSear && (API.PlayerIsMoving || !API.PlayerIsMoving) && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget && (!QuakingBoon || QuakingBoon && QuakingHelper))
                {
                    API.CastSpell(AscendedBlast);
                    return;
                }
                if (API.CanCast(Penance) && InRange && (API.TargetHasDebuff(PurgetheWicked) && PurgetheWickedTalent || !API.TargetHasDebuff(PurgetheWicked) && !PurgetheWickedTalent) && !API.PlayerIsMoving && !ChannelingMindSear && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget && AttonementTracking && (!QuakingPenance || QuakingPenance && QuakingHelper))
                {
                    API.CastSpell(Penance);
                    return;
                }
                if (API.CanCast(MindBlast) && InRange && !API.PlayerIsMoving && !ChannelingPenance && !ChannelingMindSear && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget && (!QuakingMindblast || QuakingMindblast && QuakingHelper))
                {
                    API.CastSpell(MindBlast);
                    return;
                }
                if (API.CanCast(Smite) && !ChannelingPenance && Mana >= 1 && !ChannelingPenance && !API.PlayerIsMoving && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget && (!QuakingSmite || QuakingSmite && QuakingHelper))
                {
                    API.CastSpell(Smite);
                    return;
                }
                if (API.CanCast(HolyNova) && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 12 && !ChannelingPenance && !ChannelingMindSear && Mana >= 2 && (API.PlayerIsMoving || !API.PlayerIsMoving) && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget)
                {
                    API.CastSpell(HolyNova);
                    return;
                }
                if (API.CanCast(MindSear) && InRange && API.TargetUnitInRangeCount >= 3 && !ChannelingPenance && !ChannelingMindSear && !API.PlayerIsMoving && API.TargetHealthPercent > 0 && API.PlayerCanAttackTarget && (!QuakingMindSear || QuakingMindSear && QuakingHelper))
                {
                    API.CastSpell(MindSear);
                    return;
                }
                // Auto Target
                if (IsAutoSwap && (IsOOC || API.PlayerIsInCombat))
                {
                    if (API.PlayerIsInGroup)
                    {
                        for (int i = 0; i < units.Length; i++)
                        {
                            if (IsSpread && !API.UnitHasBuff(Atonement, units[i]) && API.UnitHealthPercent(units[i]) > 0 && API.UnitRange(units[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= ShadowMendCastTime*10))
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                SwapWatch.Restart();
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= 10 && (PlayerHealth >= 10 || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0 && API.UnitRange(units[i]) <= 40)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (!API.PlayerCanAttackTarget && API.UnitRoleSpec(units[i]) == API.TankRole && !API.MacroIsIgnored("Assist") && (UnitAboveHealthPercentParty(35) == API.CurrentGroupSize && AttonementTracking || BuffPartyTracking(Atonement) >= 5) && API.UnitRange(units[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= API.SpellGCDTotalDuration*10) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                API.CastSpell("Assist");
                                SwapWatch.Restart();
                                return;
                            }
                            if (!API.PlayerCanAttackTarget && API.UnitRoleSpec(units[i]) == API.TankRole && !API.MacroIsIgnored("Assist") && UnitAboveHealthPercentParty(AoEDPSHLifePercent) >= AoEDPSNumber && API.UnitRange(units[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= API.SpellGCDTotalDuration * 10) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                API.CastSpell("Assist");
                                SwapWatch.Restart();
                                return;
                            }
                            if ((!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= API.SpellGCDTotalDuration * 10) && !AttonementTracking)
                            {
                                API.CastSpell(LowestParty(units));
                                SwapWatch.Restart();
                                return;
                            }
                        }
                    }
                }
                if (API.PlayerIsInRaid)
                {
                    for (int i = 0; i < raidunits.Length; i++)
                    {
                        if (IsSpread && !API.UnitHasBuff(Atonement, raidunits[i]) && API.UnitHealthPercent(raidunits[i]) > 0 && API.UnitRange(units[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= ShadowMendCastTime * 10))
                        {
                            API.CastSpell(RaidTargetArray[i]);
                            SwapWatch.Restart();
                            return;
                        }
                        if (API.UnitHealthPercent(raidunits[i]) <= 10 && (PlayerHealth >= 10 || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0 && API.UnitRange(raidunits[i]) <= 40)
                        {
                            API.CastSpell(RaidTargetArray[i]);
                            return;
                        }
                        if (!API.PlayerCanAttackTarget && API.UnitRange(raidunits[i]) <= 40 && API.UnitRoleSpec(raidunits[i]) == API.TankRole && !API.MacroIsIgnored("Assist") && (AttonementTracking || UnitAboveHealthPercentRaid(35) >= API.CurrentGroupSize) && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= API.SpellGCDTotalDuration * 10))
                        {
                            API.CastSpell(RaidTargetArray[i]);
                            API.CastSpell("Assist");
                            SwapWatch.Restart();
                            return;
                        }
                        if (!API.PlayerCanAttackTarget && API.UnitRange(raidunits[i]) <= 40 && API.UnitRoleSpec(raidunits[i]) == API.TankRole && !API.MacroIsIgnored("Assist") && UnitAboveHealthPercentRaid(AoEDPSHRaidLifePercent) >= AoEDPSRaidNumber && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= API.SpellGCDTotalDuration * 10) && !AttonementTracking && API.UnitHealthPercent(raidunits[i]) > 0)
                        {
                            API.CastSpell(RaidTargetArray[i]);
                            API.CastSpell("Assist");
                            SwapWatch.Restart();
                            return;
                        }
                        if ((!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= API.SpellGCDTotalDuration * 10) && !AttonementTracking)
                        {
                            API.CastSpell(LowestRaid(raidunits));
                            SwapWatch.Restart();
                            return;
                        }
                    }
                }
            }

        }
        public override void CombatPulse()
        {
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotChanneling && !ChannelingPenance && !API.PlayerIsMoving)
            {
                API.CastSpell(Fleshcraft);
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
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
            {
                API.CastSpell("Trinket1");
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
            {
                API.CastSpell("Trinket2");
            }
        }

        public override void OutOfCombatPulse()
        {
            if (!API.PlayerHasBuff(PowerWordFortitude) && API.CanCast(PowerWordFortitude))
            {
                API.CastSpell(PowerWordFortitude);
                return;
            }

        }

    }
}



