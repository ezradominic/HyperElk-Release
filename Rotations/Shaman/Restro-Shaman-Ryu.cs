using System.Linq;
using System.Diagnostics;

namespace HyperElk.Core
{
    public class RestoShaman : CombatRoutine
    {
        //Spell Strings
        private string ManaTideTotem = "Mana Tide Totem";
        private string WaterShield = "Water Shield";
        private string LightningShield = "Lightning Shield";
        private string PrimalStrike = "Primal Strike";
        private string FlametongueWeapon = "Flametongue Weapon";
        private string FrostShock = "Frost Shock";
        private string EarthShield = "Earth Shield";
        private string Riptide = "Riptide";
        private string HealingSurge = "Healing Surge";
        private string HealingWave = "Healing Wave";
        private string ChainHeal = "Chain Heal";
        private string HealingRain = "Healing Rain";
        private string HealingStreamTotem = "Healing Stream Totem";
        private string FlameShock = "Flame Shock";
        private string LavaBurst = "Lave Burst";
        private string LightningBolt = "Lightning Bolt";
        private string ChainLightning = "Chain Lightning";
        private string HealingTideTotem = "Healing Tide Totem";
        private string SpiritLinkTotem = "Spirit Link Totem";
        private string SpiritWalkersGrace = "Spiritwalker's Grace";
        private string AstralShift = "Astral Shift";
        private string Hex = "Hex";
        private string Purge = "Purge";
        private string WindShear = "Wind Shear";
        private string GhostWolf = "Ghost Wolf";
        private string TremorTotem = "Tremor Totem";
        private string EarthElemental = "Earth Elemental";
        private string VesperTotem = "Vesper Totem";
        private string ChainHarvest = "Chain Harvest";
        private string PrimordialWave = "Primordial Wave";
        private string FaeTransfusion = "Fae Transfusion";
        private string UnleashLife = "Unleash Life";
        private string SurgeofEarth = "Surge of Earth";
        private string EarthgrabTotem = "Earthgrab Totem";
        private string EarthenWallTotem = "Earthen Wall Totem";
        private string AncestralProtectionTotem = "Ancestral Protection Totem";
        private string WindRushTotem = "Wind Rush Totem";
        private string Downpour = "Downpour";
        private string CloudburstTotem = "Cloudburst Totem";
        private string HighTide = "High Tide";
        private string Wellspring = "Wellspring";
        private string Ascendance = "Ascendance";
        private string AoE = "AOE";
        private string AoEP = "AOE Party";
        private string AoER = "AOE Raid";
        private string Soulshape = "Soulshape";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string Trinket1 = "Trinket1";
        private string Trinket2 = "Trinket2";
        private string Mana = "Mana";
        private string Trinket = "Trinket";
        private string Party1 = "party1";
        private string Party2 = "party2";
        private string Party3 = "party3";
        private string Party4 = "party4";
        private string Player = "player";
        private string AoERaid = "AOE Healing Raid";
        private string Fleshcraft = "Fleshcraft";
        private string LavaSurge = "Lava Surge";
        private string DungeonCD = "Dungeon CD AOE";
        private string RaidCD = "Raid CD AoE";
        private string MO = "MO";
        private string Quake = "Quake";
        private string PurifySpirit = "Purify Spirit";

        //Talents
        bool TorrentTalent => API.PlayerIsTalentSelected(1, 1);
        bool UndulationTalent => API.PlayerIsTalentSelected(1, 2);
        bool UnsleashLifeTalent => API.PlayerIsTalentSelected(1, 3);
        bool EchooftheElementsTalent => API.PlayerIsTalentSelected(2, 1);
        bool DelugeTalent => API.PlayerIsTalentSelected(2, 2);
        bool SurgeofEarthTalent => API.PlayerIsTalentSelected(2, 3);
        bool SpiritWolfTalent => API.PlayerIsTalentSelected(3, 1);
        bool EarthGrabTotemTalent => API.PlayerIsTalentSelected(3, 2);
        bool StaticChargeTalent => API.PlayerIsTalentSelected(3, 3);
        bool AncestralVigorTalent => API.PlayerIsTalentSelected(4, 1);
        bool EarthenWallTotemTalent => API.PlayerIsTalentSelected(4, 2);
        bool AncestralProtectionTotemTalent => API.PlayerIsTalentSelected(4, 3);
        bool NaturesGuardianTalent => API.PlayerIsTalentSelected(5, 1);
        bool GracefulSpiritTalent => API.PlayerIsTalentSelected(5, 2);
        bool WindRushTotemTalent => API.PlayerIsTalentSelected(5, 3);
        bool FlashFloodTalent => API.PlayerIsTalentSelected(6, 1);
        bool DownpourTalent => API.PlayerIsTalentSelected(6, 2);
        bool CloudburstTotemTalent => API.PlayerIsTalentSelected(6, 3);
        bool HighTideTalent => API.PlayerIsTalentSelected(7, 1);
        bool WellspringTalent => API.PlayerIsTalentSelected(7, 2);
        bool AscendanceTalent => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        public string[] LegendaryList = new string[] { "None", "Verdant Infustion", "The Dark Titan's Lesson" };
        int PlayerHealth => API.TargetHealthPercent;
        string[] PlayerTargetArray = { "player", "party1", "party2", "party3", "party4" };
        string[] RaidTargetArray = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        string[] DispellList = { "Chilled", "Frozen Binds", "Clinging Darkness", "Rasping Scream", "Heaving Retch", "Goresplatter", "Slime Injection", "Gripping Infection", "Repulsive Visage", "Soul Split", "Anima Injection", "Bewildering Pollen", "Bramblethorn Entanglement", "Dying Breath", "Sinlight Visions", "Siphon Life", "Turn to Stone", "Stony Veins", "Curse of Stone", "Turned to Stone", "Curse of Obliteration", "Cosmic Artifice", "Wailing Grief", "Shadow Word:  Pain", "Soporific Shimmerdust", "Soporific Shimmerdust 2", "Anguished Cries", "Wrack Soul", "Sintouched Anima", "Curse of Suppression", "Explosive Anger", "Dark Lance", "Insidious Venom", "Charged Anima", "Lost Confidence", "Burden of Knowledge", "Internal Strife", "Forced Confession", "Insidious Venom 2", "Soul Corruption", "Spectral Reach", "Death Grasp", "Shadow Vulnerability", "Curse of Desolation", "Hex" };
        private bool QuakingHelper => CombatRoutine.GetPropertyBool("QuakingHelper");

        private static readonly Stopwatch Vesperwatch = new Stopwatch();
        private static readonly Stopwatch TransfuionWatch = new Stopwatch();
        private static readonly Stopwatch CloudburstWatch = new Stopwatch();
        private static readonly Stopwatch FaeWatch = new Stopwatch();

        private string UseLeg => LegendaryList[CombatRoutine.GetPropertyInt("Legendary")];
        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);
        private int UnitBelowManaPercentRaid(int ManaPercent) => raidunits.Count(p => API.UnitManaPercent(p) <= ManaPercent && API.UnitManaPercent(p) > 0);
        private int UnitBelowManaPercentParty(int ManaPercent) => units.Count(p => API.UnitManaPercent(p) <= ManaPercent && API.UnitManaPercent(p) > 0);
        private int UnitBelowManaPercent(int ManaPercent) => API.PlayerIsInRaid ? UnitBelowManaPercentRaid(ManaPercent) : UnitBelowManaPercentParty(ManaPercent);

        private int RiptideRaidTracking(string buff) => raidunits.Count(p => API.UnitHasBuff(buff, p));
        private int RiptidePartyTracking(string buff) => units.Count(p => API.UnitHasBuff(buff, p));
        private int EarthShieldRaidTracking(string buff) => raidunits.Count(p => API.UnitHasBuff(buff, p) && API.UnitBuffPlayerSrc(buff, p));
        private int EarthShieldPartyTracking(string buff) => units.Count(p => API.UnitHasBuff(buff, p));


        bool ChannelingFae => API.CurrentCastSpellID("player") == 328923;
        private bool EarthShieldTracking => API.PlayerIsInRaid ? EarthShieldRaidTracking(EarthShield) < 1 : EarthShieldPartyTracking(EarthShield) < 1;
        private bool ManaAoE => API.PlayerIsInRaid ? UnitBelowManaPercentRaid(ManaPercent) >= AoERaidNumber : API.PlayerMana <= ManaPercent;
        private bool TrinketAoE => UnitBelowHealthPercent(TrinketLifePercent) >= AoENumber;
        private bool VesperAoE => UnitBelowHealthPercent(VesperTotemLifePercent) >= AoENumber;
        private bool DownpourAoE => UnitBelowHealthPercent(DownpourLifePercent) >= AoENumber;
        private bool WellSpringAoE => UnitBelowHealthPercent(WellspringLifePercent) >= AoENumber;
        private bool ChainHealAoE => UnitBelowHealthPercent(ChainHealLifePercent) >= AoENumber;
        private bool SpiritLinkAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(SpiritLinkTotemLifePercent) >= RaidCDNumber : UnitBelowHealthPercentParty(SpiritLinkTotemLifePercent) >= DungeonCDNumber;
        private bool EarthenWallAoE => UnitBelowHealthPercent(EarthenWallTotemLifePercent) >= AoENumber;
        private bool HealingRainAoE => UnitBelowHealthPercent(HealingRainLifePercent) >= AoENumber;
        private bool HealingStreamAoE => UnitBelowHealthPercent(HealingStreamTotemLifePercent) >= AoENumber;
        private bool HealingTideAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealingTideTotemLifePercent) >= RaidCDNumber : UnitBelowHealthPercentParty(HealingTideTotemLifePercent) >= DungeonCDNumber;
        private bool FaeAoE => UnitBelowHealthPercent(FaeLifePercent) >= AoENumber;
        private bool RiptideTracking => API.PlayerIsInRaid ? RiptideRaidTracking(Riptide) >= 3 : RiptidePartyTracking(Riptide) >= 2;
        private bool KyrianCheck => API.CanCast(VesperTotem) && PlayerCovenantSettings == "Kyrian" && VesperAoE && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool NightFaeCheck =>API.CanCast(FaeTransfusion) && PlayerCovenantSettings == "Night Fae" && FaeAoE && NotChanneling && !API.PlayerCanAttackTarget && !API.PlayerIsMoving;
        private bool NecrolordCheck => API.CanCast(PrimordialWave) && PlayerCovenantSettings == "Necrolord" && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && API.TargetHealthPercent <= PrimordialWaveLifePercent && RiptideTracking;
        private bool NecrolordMOCheck => API.CanCast(PrimordialWave) && PlayerCovenantSettings == "Necrolord" && NotChanneling && !API.PlayerCanAttackMouseover && (!API.PlayerIsMoving || API.PlayerIsMoving) && API.MouseoverHealthPercent <= PrimordialWaveLifePercent && RiptideTracking;
        private bool VenthyrCheck => API.CanCast(ChainHarvest) && ChainHealAoE && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && (!API.PlayerCanAttackTarget || API.PlayerCanAttackTarget) && !API.PlayerIsMoving;
        private bool VenthyrMOCheck => API.CanCast(ChainHarvest) && ChainHealAoE && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && (!API.PlayerCanAttackMouseover || API.PlayerCanAttackMouseover) && !API.PlayerIsMoving;
        private bool RiptideCheck => API.CanCast(Riptide) && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !API.PlayerHasBuff(Riptide) && API.TargetHealthPercent <= RiptideLifePercent;
        private bool RiptideMOCheck => API.CanCast(Riptide) && !API.PlayerCanAttackMouseover && (!API.PlayerIsMoving || API.PlayerIsMoving) && !API.MouseoverHasBuff(Riptide) && API.MouseoverHealthPercent <= RiptideLifePercent;
        private bool SpiritLinkCheck => API.CanCast(SpiritLinkTotem) && SpiritLinkAoE;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool IsAutoSwap => API.ToggleIsEnabled("Auto Target");
        private bool IsOOC => API.ToggleIsEnabled("OOC");
        private int TrinketLifePercent => numbList[CombatRoutine.GetPropertyInt(Trinket)];
        private int FaeLifePercent => numbList[CombatRoutine.GetPropertyInt(FaeTransfusion)];
        private int RiptideLifePercent => numbList[CombatRoutine.GetPropertyInt(Riptide)];
        private int WellspringLifePercent => numbList[CombatRoutine.GetPropertyInt(Wellspring)];
        private int DownpourLifePercent => numbList[CombatRoutine.GetPropertyInt(Downpour)];
        private int UnleashLifePercent => numbList[CombatRoutine.GetPropertyInt(UnleashLife)];
        private int HealingSurgeLifePercent => numbList[CombatRoutine.GetPropertyInt(HealingSurge)];
        private int HealingWaveLifePercent => numbList[CombatRoutine.GetPropertyInt(HealingWave)];
        private int PrimordialWaveLifePercent => numbList[CombatRoutine.GetPropertyInt(PrimordialWave)];
        private int SpiritLinkTotemLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritLinkTotem)];
        private int VesperTotemLifePercent => numbList[CombatRoutine.GetPropertyInt(VesperTotem)];
        private int ChainHealLifePercent => numbList[CombatRoutine.GetPropertyInt(ChainHeal)];
        private int HealingRainLifePercent => numbList[CombatRoutine.GetPropertyInt(HealingRain)];
        private int HealingStreamTotemLifePercent => numbList[CombatRoutine.GetPropertyInt(HealingStreamTotem)];
        private int HealingTideTotemLifePercent => numbList[CombatRoutine.GetPropertyInt(HealingTideTotem)];
     //   private int AncestralProtectionTotemLifePercent => numbList[CombatRoutine.GetPropertyInt(AncestralProtectionTotem)];
        private int EarthenWallTotemLifePercent => numbList[CombatRoutine.GetPropertyInt(EarthenWallTotem)];
        private int SurgeofEarthLifePercent => numbList[CombatRoutine.GetPropertyInt(SurgeofEarth)];
        private int ManaPercent => numbList[CombatRoutine.GetPropertyInt(ManaTideTotem)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private string UseAscend => CDUsage[CombatRoutine.GetPropertyInt(Ascendance)];
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoERaid)];
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        private int DungeonCDNumber => numbPartyList[CombatRoutine.GetPropertyInt(DungeonCD)];
        private int RaidCDNumber => numbRaidList[CombatRoutine.GetPropertyInt(RaidCD)];
        //private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoER)];

        private bool Quaking => ((API.PlayerCurrentCastTimeRemaining >= 200 || API.PlayerIsChanneling) && API.PlayerDebuffRemainingTime(Quake) < 200) && PlayerHasDebuff(Quake);
        private bool SaveQuake => (PlayerHasDebuff(Quake) && API.PlayerDebuffRemainingTime(Quake) > 200 && QuakingHelper || !PlayerHasDebuff(Quake) || !QuakingHelper);
        private bool QuakingHS => API.PlayerDebuffRemainingTime(Quake) > HealingSurgeCastTime && PlayerHasDebuff(Quake);
        private bool QuakingHW => API.PlayerDebuffRemainingTime(Quake) > HealingWaveCastTime && PlayerHasDebuff(Quake);
        private bool QuakingCH => API.PlayerDebuffRemainingTime(Quake) > ChainHealCastTime && PlayerHasDebuff(Quake);
        private bool QuakingHR => API.PlayerDebuffRemainingTime(Quake) > HealingRainCastTime && PlayerHasDebuff(Quake);
        private bool QuakingDownpour => API.PlayerDebuffRemainingTime(Quake) > DownpourCastTime && PlayerHasDebuff(Quake);
        private bool QuakingWellspring => API.PlayerDebuffRemainingTime(Quake) > WellspringCastTime && PlayerHasDebuff(Quake);
        private bool QuakingChainHarvest => API.PlayerDebuffRemainingTime(Quake) > ChainHarvestCastTime && PlayerHasDebuff(Quake);
        private bool QuakingFae => API.PlayerDebuffRemainingTime(Quake) > FaeCastTime && PlayerHasDebuff(Quake);
        private bool QuakingLightning => API.PlayerDebuffRemainingTime(Quake) > LightningCastTime && PlayerHasDebuff(Quake);
        private bool QuakingChainLight => API.PlayerDebuffRemainingTime(Quake) > ChainLightningCastTime && PlayerHasDebuff(Quake);
        private bool QuakingLavaburst => API.PlayerDebuffRemainingTime(Quake) > LavaburstCastTime && PlayerHasDebuff(Quake);

        float HealingSurgeCastTime => 150f / (1f + API.PlayerGetHaste);
        float HealingWaveCastTime => 250f / (1f + API.PlayerGetHaste);
        float ChainHealCastTime => 250f / (1f + API.PlayerGetHaste);
        float HealingRainCastTime => 200f / (1f + API.PlayerGetHaste);

        float DownpourCastTime => 150f / (1f + API.PlayerGetHaste);
        float WellspringCastTime => 150f / (1f + API.PlayerGetHaste);
        float ChainHarvestCastTime => 250f / (1f + API.PlayerGetHaste);
        float FaeCastTime => 300f / (1f + API.PlayerGetHaste);
        float LavaburstCastTime => 200f / (1f + API.PlayerGetHaste);
        float LightningCastTime => 250f / (1f + API.PlayerGetHaste);
        float ChainLightningCastTime => 200f / (1f + API.PlayerGetHaste);
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
        bool IsTrinkets1 => (UseTrinket1 == "With Cooldowns" && IsCooldowns && API.TargetHealthPercent <= TrinketLifePercent || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && TrinketAoE);
        bool IsTrinkets2 => (UseTrinket2 == "With Cooldowns" && IsCooldowns && API.TargetHealthPercent <= TrinketLifePercent || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && TrinketAoE);
        private bool AutoWolf => CombatRoutine.GetPropertyBool("AutoWolf");
        private int Level => API.PlayerLevel;
        private bool InRange => IsMouseover ? API.MouseoverRange <= 40 : API.TargetRange <= 40;
        private bool IsMelee => API.TargetRange < 6;
        private bool IsInKickRange => API.FocusCanInterrupted ? API.FocusRange < 31 : API.TargetRange < 31;
        // private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsDispell => API.ToggleIsEnabled("Dispel");



        //  public bool isInterrupt => CombatRoutine.GetPropertyBool("KICK") && API.TargetCanInterrupted && API.TargetIsCasting && (API.TargetIsChanneling ? API.TargetElapsedCastTime >= interruptDelay : API.TargetCurrentCastTimeRemaining <= interruptDelay);
        //  public int interruptDelay => random.Next((int)(CombatRoutine.GetPropertyInt("KICKTime") * 0.9), (int)(CombatRoutine.GetPropertyInt("KICKTime") * 1.1));
        public override void Initialize()
        {
            CombatRoutine.Name = "Resto Shaman by Ryu";
            API.WriteLog("Welcome to Resto Shaman v1.0 by Ryu");
            API.WriteLog("BETA ROTATION : Some things are still missing. Please post feedback in Shaman Channel.");
            API.WriteLog("Mouseover Support is added. Please create /cast [@mouseover] xx whereas xx is your spell and assign it the binds with MO on it in keybinds.");
            API.WriteLog("For all ground spells, either use @Cursor or when it is time to place it, the Bot will pause until you've placed it. If you'd perfer to use your own logic for them, please place them on ignore in the spellbook.");
            API.WriteLog("For the Quaking helper you just need to create an ingame macro with /stopcasting and bind it under the Macros Tab in Elk :-)");
            API.WriteLog("There are two different settings for AoE Numbers. AoE Cooldowns referes to Sprirt Link Totem and Healing Tide Totem. All others use the AoE Healing Number or AoE Raid Healing Number.");
            API.WriteLog("If you wish to use Auto Target, please set your WoW keybinds in the keybinds => Targeting for Self and party, and then match them to the Macro's in the spell book. Enable it in the toggles. You must at least have a target for it swap, friendly or enemy. It will not swap BACK to a enemy. This does work for raid, however, requires the addon Bindpad. See Video in discord.");
            API.WriteLog("Special Thanks to Jom for testing");

            //Buff
            CombatRoutine.AddBuff(GhostWolf, 2645);
            CombatRoutine.AddBuff(Soulshape, 310143);
            CombatRoutine.AddBuff(LavaSurge, 77762);
            CombatRoutine.AddBuff(Ascendance, 114052);
            CombatRoutine.AddBuff(Riptide, 61295);
            CombatRoutine.AddBuff(EarthShield, 974);
            CombatRoutine.AddBuff(WaterShield, 52127);
            CombatRoutine.AddBuff(SpiritWalkersGrace, 79206);
            CombatRoutine.AddBuff(PrimordialWave, 326059);
            CombatRoutine.AddBuff(VesperTotem, 324386);
            CombatRoutine.AddBuff(UnleashLife, 73685);
            CombatRoutine.AddBuff(Quake, 240447);

            //Debuff
            CombatRoutine.AddDebuff(FlameShock, 188389);
            CombatRoutine.AddDebuff(Quake, 240447);

            //Dispell Debuff
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
            CombatRoutine.AddDebuff("Dying Breath", 322968);
            CombatRoutine.AddDebuff("Curse of Stone", 319603);
            CombatRoutine.AddDebuff("Turned to Stone", 319611);
            CombatRoutine.AddDebuff("Curse of Obliteration", 325876);
            CombatRoutine.AddDebuff("Cosmic Artifice", 325725);
            CombatRoutine.AddDebuff("Wailing Grief", 340026);
            CombatRoutine.AddDebuff("Shadow Word:  Pain", 332707);
            CombatRoutine.AddDebuff("Soporific Shimmerdust", 334493);
            CombatRoutine.AddDebuff("Soporific Shimmerdust 2", 334496);
            CombatRoutine.AddDebuff("Hex", 332605);
            CombatRoutine.AddDebuff("Anguished Cries", 325885);
            CombatRoutine.AddDebuff("Wrack Soul", 321038);
            CombatRoutine.AddDebuff("Sintouched Anima", 328494);
            CombatRoutine.AddDebuff("Curse of Suppression", 326836);
            CombatRoutine.AddDebuff("Explosive Anger", 336277);
            CombatRoutine.AddDebuff("Insidious Venom", 323636);
            CombatRoutine.AddDebuff("Charged Anima", 338731);
            CombatRoutine.AddDebuff("Lost Confidence", 322818);
            CombatRoutine.AddDebuff("Burden of Knowledge", 317963);
            CombatRoutine.AddDebuff("Internal Strife", 327648);
            CombatRoutine.AddDebuff("Forced Confession", 328331);
            CombatRoutine.AddDebuff("Insidious Venom 2", 317661);
            CombatRoutine.AddDebuff("Soul Corruption", 333708);
            CombatRoutine.AddDebuff("Spectral Reach", 319669);
            CombatRoutine.AddDebuff("Death Grasp", 323831);
            CombatRoutine.AddDebuff("Shadow Vulnerability", 330725);
            CombatRoutine.AddDebuff("Curse of Desolation", 333299);


            //Spell
            CombatRoutine.AddSpell(Fleshcraft, 324631);
            CombatRoutine.AddSpell(GhostWolf, 2645);
            CombatRoutine.AddSpell(Riptide, 61295);
            CombatRoutine.AddSpell(HealingSurge, 8004);
            CombatRoutine.AddSpell(HealingWave, 77472);
            CombatRoutine.AddSpell(ChainHeal, 1064);
            CombatRoutine.AddSpell(HealingRain, 73920);
            CombatRoutine.AddSpell(HealingStreamTotem, 5394);
            CombatRoutine.AddSpell(WaterShield, 52127);
            CombatRoutine.AddSpell(EarthShield, 974);
            CombatRoutine.AddSpell(FlameShock, 188389);
            CombatRoutine.AddSpell(FrostShock, 196840);
            CombatRoutine.AddSpell(LavaBurst, 51505);
            CombatRoutine.AddSpell(LightningBolt, 188196);
            CombatRoutine.AddSpell(ChainLightning, 188443);
            CombatRoutine.AddSpell(LightningShield, 192106);
            CombatRoutine.AddSpell(HealingTideTotem, 108280);
            CombatRoutine.AddSpell(SpiritLinkTotem, 98008);
            CombatRoutine.AddSpell(SpiritWalkersGrace, 79206);
            CombatRoutine.AddSpell(ManaTideTotem, 16191);
            CombatRoutine.AddSpell(AstralShift, 108271);
            CombatRoutine.AddSpell(WindShear, 57994);
            CombatRoutine.AddSpell(PrimordialWave, 326059, "D1");
            CombatRoutine.AddSpell(VesperTotem, 324386, "D1");
            CombatRoutine.AddSpell(FaeTransfusion, 328923, "D1");
            CombatRoutine.AddSpell(ChainHarvest, 320674, "D1");
            CombatRoutine.AddSpell(Ascendance, 114052, "D1");
            CombatRoutine.AddSpell(UnleashLife, 73685);
            CombatRoutine.AddSpell(SurgeofEarth, 320746);
            CombatRoutine.AddSpell(EarthenWallTotem, 198838);
            CombatRoutine.AddSpell(AncestralProtectionTotem, 207399);
            CombatRoutine.AddSpell(WindRushTotem, 192077);
            CombatRoutine.AddSpell(Downpour, 207778);
            CombatRoutine.AddSpell(CloudburstTotem, 157153);
            CombatRoutine.AddSpell(Wellspring, 197995);
            CombatRoutine.AddSpell(PurifySpirit, 77130);


            //Toggle
            CombatRoutine.AddToggle("Auto Target");
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("OOC");
            CombatRoutine.AddToggle("Dispel");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Macro
            CombatRoutine.AddMacro(Trinket1);
            CombatRoutine.AddMacro(Trinket2);
            CombatRoutine.AddMacro("Stopcast", "F10");
            CombatRoutine.AddMacro(Player);
            CombatRoutine.AddMacro(Party1);
            CombatRoutine.AddMacro(Party2);
            CombatRoutine.AddMacro(Party3);
            CombatRoutine.AddMacro(Party4);
            CombatRoutine.AddMacro(WindShear + "Focus");
            CombatRoutine.AddMacro(FlameShock + MO);
            CombatRoutine.AddMacro(LightningBolt + MO);
            CombatRoutine.AddMacro(ChainLightning + MO);
            CombatRoutine.AddMacro(ChainHeal + MO);
            CombatRoutine.AddMacro(ChainHarvest + MO);
            CombatRoutine.AddMacro(HealingSurge + MO);
            CombatRoutine.AddMacro(HealingWave + MO);
            CombatRoutine.AddMacro(Riptide + MO);
            CombatRoutine.AddMacro(EarthShield + MO);
            CombatRoutine.AddMacro(PrimordialWave + MO);
            CombatRoutine.AddMacro(UnleashLife + MO);
            CombatRoutine.AddMacro(Downpour + MO);
            CombatRoutine.AddMacro(LavaBurst + MO);
            CombatRoutine.AddMacro(SurgeofEarth + MO);
            CombatRoutine.AddMacro(PurifySpirit + MO);
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
            CombatRoutine.AddProp("AutoWolf", "AutoWolf", true, "Will auto switch forms out of Fight", "Generic");
            CombatRoutine.AddProp(AstralShift, AstralShift + " Life Percent", numbList, "Life percent at which" + AstralShift + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 100);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 1);
            CombatRoutine.AddProp(EarthElemental, "Use " + EarthElemental, CDUsage, "Use " + EarthElemental + "On Cooldown, with Cooldowns, Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp(Ascendance, "Use " + Ascendance, CDUsage, "Use " + Ascendance + "On Cooldown, with Cooldowns, Not Used", "Cooldowns", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("QuakingHelper", "Quaking Helper", false, "Will cancel casts on Quaking", "Generic");

            //CombatRoutine.AddProp(PartySwap, PartySwap + " Life Percent", numbList, "Life percent at which" + PartySwap + "is used, set to 0 to disable", "Healing", 0);
            //CombatRoutine.AddProp(TargetChange, TargetChange + " Life Percent", numbList, "Life percent at which" + TargetChange + "is used to change from your current target, when using Auto Swap logic, set to 0 to disable", "Healing", 0);
            // CombatRoutine.AddProp("OOC", "Healing out of Combat", true, "Heal out of combat", "Healing");
            CombatRoutine.AddProp(Riptide, Riptide + " Life Percent", numbList, "Life percent at which " + Riptide + " is used, set to 0 to disable", "Healing", 90);
            CombatRoutine.AddProp(UnleashLife, UnleashLife + " Life Percent", numbList, "Life percent at which " + UnleashLife + " is used if talented, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(HealingSurge, HealingSurge + " Life Percent", numbList, "Life percent at which " + HealingSurge + " is used, set to 0 to disable", "Healing", 70);
            CombatRoutine.AddProp(HealingWave, HealingWave + " Life Percent", numbList, "Life percent at which " + HealingWave + " is used, set to 0 to disable", "Healing", 91);
            CombatRoutine.AddProp(PrimordialWave, PrimordialWave + " Life Percent", numbList, "Life percent at which " + PrimordialWave + " is used, set to 0 to disable", "Healing", 78);
            CombatRoutine.AddProp(ManaTideTotem, ManaTideTotem + " Mana Percent", numbList, "Mana percent at which " + ManaTideTotem + " is used, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(SpiritLinkTotem, SpiritLinkTotem + " Life Percent", numbList, "Life percent at which " + SpiritLinkTotem + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 15);
            CombatRoutine.AddProp(HealingTideTotem, HealingTideTotem + " Life Percent", numbList, "Life percent at which " + HealingTideTotem + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 45);
            CombatRoutine.AddProp(VesperTotem, VesperTotem + " Life Percent", numbList, "Life percent at which " + VesperTotem + " is used when AoE Number of members are at and Cov is Kyrian, set to 0 to disable", "Healing", 55);
            CombatRoutine.AddProp(FaeTransfusion, FaeTransfusion + " Life Percent", numbList, "Life percent at which " + FaeTransfusion + " is used when AoE Number of members are at and Cov is Kyrian, set to 0 to disable", "Healing", 55);
            CombatRoutine.AddProp(ChainHeal, ChainHeal + " Life Percent", numbList, "Life percent at which " + ChainHeal + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(HealingRain, HealingRain + " Life Percent", numbList, "Life percent at which " + HealingRain + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(HealingStreamTotem, HealingStreamTotem + " Life Percent", numbList, "Life percent at which " + HealingStreamTotem + " Or Cloudburst Totem is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 92);
         //   CombatRoutine.AddProp(AncestralProtectionTotem, AncestralProtectionTotem + " Life Percent", numbList, "Life percent at which " + AncestralProtectionTotem + " is used when AoE Number of members are at life percent if talented, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(EarthenWallTotem, EarthenWallTotem + " Life Percent", numbList, "Life percent at which " + EarthenWallTotem + " is used when AoE Number of members are at life percent if talented, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(Downpour, Downpour + " Life Percent", numbList, "Life percent at which " + Downpour + " is used when AoE Number of members are at life percent if talented, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(Wellspring, Wellspring + " Life Percent", numbList, "Life percent at which " + Wellspring + " is used when AoE Number of members are at life percent if talented, set to 0 to disable", "Healing", 70);
            CombatRoutine.AddProp(SurgeofEarth, SurgeofEarth + " Life Percent", numbList, "Life percent at which " + SurgeofEarth + " is used when AoE Number of members are at life percent if talented, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(AoE, "Number of units for AoE Healing ", numbPartyList, " Units for AoE Healing", "Healing", 3);
            CombatRoutine.AddProp(AoERaid, "Number of units for AoE Healing in raid ", numbRaidList, " Units for AoE Healing in raid", "Healing", 7);
            CombatRoutine.AddProp(DungeonCD, "Number of units for Cooldowns Healing in 5-man ", numbPartyList, " Units for Cooldowns Healing", "Healing", 2);
            CombatRoutine.AddProp(RaidCD, "Number of units for Cooldowns Healing in raid ", numbRaidList, " Units for Cooldowns Healing in raid", "Healing", 6);
            // CombatRoutine.AddProp("Legendary", "Select your Legendary", LegendaryList, "Select Your Legendary", "Legendary");
            CombatRoutine.AddProp(Trinket, Trinket + " Life Percent", numbList, "Life percent at which " + "Trinkets" + " should be used, set to 0 to disable", "Healing", 55);
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", CDUsageWithAOE, "When should trinket1 be used", "Trinket", 0);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", CDUsageWithAOE, "When should trinket1 be used", "Trinket", 0);

        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted && !API.PlayerSpellonCursor && !API.PlayerHasBuff(GhostWolf) && (IsOOC || API.PlayerIsInCombat) && !ChannelingFae)
            {
                if (API.PlayerCurrentCastTimeRemaining > 40 && QuakingHelper && Quaking)
                {
                    API.CastSpell("Stopcast");
                    API.WriteLog("Debuff Time Remaining for Quake : " + API.PlayerDebuffRemainingTime(Quake));
                    return;
                }
                if (API.CanCast(Ascendance) && AscendanceTalent && (UseAscend == "With Cooldowns" && IsCooldowns || UseAscend == "On Cooldown"))
                {
                    API.CastSpell(Ascendance);
                    return;
                }
                #region Dispell
                if (IsDispell)
                {
                    if (API.CanCast(PurifySpirit) && !ChannelingFae && NotChanneling)
                    {
                        for (int i = 0; i < DispellList.Length; i++)
                        {
                            if (TargetHasDispellAble(DispellList[i]))
                            {
                                API.CastSpell(PurifySpirit);
                                return;
                            }
                        }
                    }
                    if (API.CanCast(PurifySpirit) && IsMouseover && !ChannelingFae && NotChanneling)
                    {
                        for (int i = 0; i < DispellList.Length; i++)
                        {
                            if (MouseouverHasDispellAble(DispellList[i]))
                            {
                                API.CastSpell(PurifySpirit + "MO");
                                return;
                            }
                        }
                    }
                }
                #endregion
                if (API.CanCast(ManaTideTotem) && ManaAoE && InRange)
                {
                    API.CastSpell(ManaTideTotem);
                    API.WriteLog("Mana Percent : " + API.PlayerMana + "Party 1 Mana : " + API.UnitManaPercent("party1"));
                    return;
                }
                if (API.CanCast(EarthShield) && API.TargetRoleSpec == API.TankRole && !API.TargetHasBuff(EarthShield) && InRange && !API.PlayerCanAttackTarget && API.TargetHealthPercent <= 100 && EarthShieldTracking && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(EarthShield);
                    return;
                }
                if (API.CanCast(EarthShield) && !API.MacroIsIgnored(EarthShield + MO) && IsMouseover && API.MouseoverRoleSpec == API.TankRole && !API.MouseoverHasBuff(EarthShield) && InRange && !API.PlayerCanAttackMouseover && API.MouseoverHealthPercent <= 100 && EarthShieldTracking && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(EarthShield + "MO");
                    return;
                }
                if (RiptideCheck && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Riptide);
                    return;
                }
                if (RiptideMOCheck && !API.MacroIsIgnored(Riptide + MO) && IsMouseover && InRange && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(Riptide + "MO");
                    return;
                }
                if (NecrolordCheck && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(PrimordialWave);
                    return;
                }
                if (NecrolordMOCheck && !API.MacroIsIgnored(PrimordialWave + MO) && IsMouseover && InRange && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(PrimordialWave + "MO");
                    return;
                }
                if (API.CanCast(SurgeofEarth) && SurgeofEarthTalent && PlayerHealth <= SurgeofEarthLifePercent && API.TargetHasBuff(EarthShield) && API.TargetBuffStacks(EarthShield) >= 3 && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(SurgeofEarth);
                    return;
                }
                if (API.CanCast(SurgeofEarth) && !API.MacroIsIgnored(SurgeofEarth + MO) && SurgeofEarthTalent && API.MouseoverHealthPercent <= SurgeofEarthLifePercent && API.MouseoverHasBuff(EarthShield) && API.MouseoverBuffStacks(EarthShield) >= 3 && API.MouseoverHealthPercent > 0)
                {
                    API.CastSpell(SurgeofEarth + "MO");
                    return;
                }
                if (API.CanCast(WaterShield) && !API.PlayerHasBuff(WaterShield) && API.PlayerHealthPercent > 0)
                {
                    API.CastSpell(WaterShield);
                    return;
                }
              if (API.CanCast(HealingRain) && HealingRainAoE && InRange && API.TargetHealthPercent > 0 && (!QuakingHR || QuakingHR && QuakingHelper))
                {
                    API.CastSpell(HealingRain);
                    return;
                }
              if (SpiritLinkCheck && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(SpiritLinkTotem);
                    return;
                }
              if (API.CanCast(HealingStreamTotem) && HealingStreamAoE && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(HealingStreamTotem);
                    return;
                }
              if (API.CanCast(EarthenWallTotem) && EarthenWallTotemTalent && InRange && EarthenWallAoE && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(EarthenWallTotem);
                    return;
                }
              if (API.CanCast(CloudburstTotem) && InRange && CloudburstTotemTalent && HealingStreamAoE && (!CloudburstWatch.IsRunning || CloudburstWatch.ElapsedMilliseconds >= 10000) && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(CloudburstTotem);
                    CloudburstWatch.Reset();
                    CloudburstWatch.Start();
                    return;
                }
              if (API.CanCast(HealingTideTotem) && HealingTideAoE && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(HealingTideTotem);
                    return;
                }
                if (KyrianCheck && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(VesperTotem);
                    return;
                }
                if (NightFaeCheck && InRange && API.TargetHealthPercent > 0 && (!QuakingFae || QuakingFae && QuakingHelper))
                {
                    API.CastSpell(FaeTransfusion);
                    return;
                }
                if (API.CanCast(UnleashLife) && API.TargetHealthPercent > 0 && UnsleashLifeTalent && API.TargetHealthPercent <= UnleashLifePercent && InRange && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(UnleashLife);
                    return;
                }
                if (API.CanCast(UnleashLife) && !API.MacroIsIgnored(UnleashLife + MO) && IsMouseover && API.MouseoverHealthPercent > 0 && UnsleashLifeTalent && API.MouseoverHealthPercent <= UnleashLifePercent && InRange && !API.PlayerCanAttackMouseover)
                {
                    API.CastSpell(UnleashLife + MO);
                    return;
                }
                if (API.CanCast(Downpour) && API.TargetHealthPercent > 0 && DownpourAoE && DownpourTalent && InRange && !API.PlayerCanAttackTarget && (!QuakingDownpour || QuakingDownpour && QuakingHelper))
                {
                    API.CastSpell(Downpour);
                    return;
                }
                if (API.CanCast(Downpour) && !API.MacroIsIgnored(Downpour + MO) && IsMouseover && API.MouseoverHealthPercent > 0 && DownpourAoE && DownpourTalent && InRange && !API.PlayerCanAttackMouseover && (!QuakingDownpour || QuakingDownpour && QuakingHelper))
                {
                    API.CastSpell(Downpour + MO);
                    return;
                }
                if (API.CanCast(ChainHeal) && API.TargetHealthPercent > 0 && (ChainHealAoE || API.PlayerHasBuff(UnleashLife) && UnitBelowHealthPercent(85) >= 2 && API.TargetHasBuff(Riptide)) && (!QuakingCH || QuakingCH && QuakingHelper))
                {
                    API.CastSpell(ChainHeal);
                    return;
                }
                if (API.CanCast(ChainHeal) && !API.MacroIsIgnored(ChainHeal + MO) && IsMouseover && API.MouseoverHealthPercent > 0 && (ChainHealAoE || API.PlayerHasBuff(UnleashLife) && UnitBelowHealthPercent(85) >= 2 && API.MouseoverHasBuff(Riptide)) && (!QuakingCH || QuakingCH && QuakingHelper))
                {
                    API.CastSpell(ChainHeal + MO);
                    return;
                }
                if (VenthyrCheck && InRange && API.TargetHealthPercent > 0 && (!QuakingChainHarvest || QuakingChainHarvest && QuakingHelper))
                {
                    API.CastSpell(ChainHarvest);
                    return;
                }
                if (VenthyrMOCheck && !API.MacroIsIgnored(ChainHarvest + MO) && IsMouseover && InRange && API.MouseoverHealthPercent > 0 && (!QuakingChainHarvest || QuakingChainHarvest && QuakingHelper))
                {
                    API.CastSpell(ChainHarvest + MO);
                    return;
                }
                if (API.CanCast(Wellspring) && WellspringTalent && WellSpringAoE && (!QuakingWellspring || QuakingWellspring && QuakingHelper))
                {
                    API.CastSpell(Wellspring);
                    return;
                }
                if (API.CanCast(HealingSurge) && API.TargetHealthPercent <= HealingSurgeLifePercent && InRange && !API.PlayerCanAttackTarget && (!QuakingHS || QuakingHS && QuakingHelper))
                {
                    API.CastSpell(HealingSurge);
                    return;
                }
                if (API.CanCast(HealingSurge) && !API.MacroIsIgnored(HealingSurge + MO) && IsMouseover && API.MouseoverHealthPercent <= HealingSurgeLifePercent && InRange && !API.PlayerCanAttackMouseover && (!QuakingHS || QuakingHS && QuakingHelper))
                {
                    API.CastSpell(HealingSurge + MO);
                    return;
                }
                if (API.CanCast(HealingWave)  && API.TargetHealthPercent <= HealingWaveLifePercent && InRange && !API.PlayerCanAttackTarget && (!QuakingHW || QuakingHW && QuakingHelper))
                {
                    API.CastSpell(HealingWave);
                    return;
                }
                if (API.CanCast(HealingWave) && !API.MacroIsIgnored(HealingWave + MO) && IsMouseover && API.MouseoverHealthPercent <= HealingWaveLifePercent && InRange && !API.PlayerCanAttackMouseover && (!QuakingHW || QuakingHW && QuakingHelper))
                {
                    API.CastSpell(HealingWave + MO);
                    return;
                }
                //DPS
                if (API.CanCast(FlameShock) && InRange && (!API.TargetHasDebuff(FlameShock) || API.TargetDebuffRemainingTime(FlameShock) < 600) && API.PlayerCanAttackTarget)
                {
                    API.CastSpell(FlameShock);
                    return;
                }
                if (API.CanCast(FlameShock) && IsMouseover && isMouseoverInCombat && InRange && (!API.MouseoverHasDebuff(FlameShock) || API.MouseoverDebuffRemainingTime(FlameShock) < 600) && API.PlayerCanAttackMouseover)
                {
                    API.CastSpell(FlameShock + "MO");
                    return;
                }
                if (API.CanCast(LavaBurst) && InRange && API.PlayerCanAttackTarget && (!API.PlayerHasBuff(LavaSurge) || API.PlayerHasBuff(LavaSurge)) && (!QuakingLavaburst || QuakingLavaburst && QuakingHelper))
                {
                    API.CastSpell(LavaBurst);
                    return;
                }
                if (API.CanCast(LavaBurst) && IsMouseover && isMouseoverInCombat && InRange && API.PlayerCanAttackMouseover && (!API.PlayerHasBuff(LavaSurge) || API.PlayerHasBuff(LavaSurge)) && (!QuakingLavaburst || QuakingLavaburst && QuakingHelper))
                {
                    API.CastSpell(LavaBurst + "MO");
                    return;
                }
                if (API.CanCast(ChainLightning) && InRange && API.PlayerCanAttackTarget && API.TargetUnitInRangeCount >= 3 && (!QuakingChainLight || QuakingChainLight && QuakingHelper))
                {
                    API.CastSpell(ChainLightning);
                    return;
                }
                if (API.CanCast(ChainLightning) && IsMouseover && isMouseoverInCombat && InRange && API.PlayerCanAttackMouseover && API.TargetUnitInRangeCount >= 3 && (!QuakingChainLight || QuakingChainLight && QuakingHelper))
                {
                    API.CastSpell(ChainLightning + "MO");
                    return;
                }
                if (API.CanCast(LightningBolt) && InRange && API.PlayerCanAttackTarget && (!QuakingLightning || QuakingLightning && QuakingHelper))
                {
                    API.CastSpell(LightningBolt);
                    return;
                }
                if (API.CanCast(LightningBolt) && IsMouseover && isMouseoverInCombat && InRange && API.PlayerCanAttackMouseover && (!QuakingLightning || QuakingLightning && QuakingHelper))
                {
                    API.CastSpell(LightningBolt + "MO");
                    return;
                }
                //Auto Target
                if (IsAutoSwap)
                {
                    if (API.PlayerIsInGroup && InRange)
                    {
                        for (int i = 0; i < units.Length; i++)
                        {
                            if (API.UnitHealthPercent(units[i]) <= 15 && (PlayerHealth >= 15 || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= SpiritLinkTotemLifePercent && (PlayerHealth >= SpiritLinkTotemLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= HealingStreamTotemLifePercent && (PlayerHealth >= HealingStreamTotemLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= HealingWaveLifePercent && (PlayerHealth >= HealingWaveLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= HealingSurgeLifePercent && (PlayerHealth >= HealingSurgeLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= RiptideLifePercent && (PlayerHealth >= RiptideLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitRoleSpec(units[i]) == API.TankRole && !API.UnitHasBuff(EarthShield, units[i]) && EarthShieldTracking)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                        }
                    }
                    if (API.PlayerIsInRaid && InRange)
                    {
                        for (int i = 0; i < raidunits.Length; i++)
                        {
                            if (API.UnitHealthPercent(raidunits[i]) <= 15 && (PlayerHealth >= 15 || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= SpiritLinkTotemLifePercent && (PlayerHealth >= SpiritLinkTotemLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= HealingStreamTotemLifePercent && (PlayerHealth >= HealingStreamTotemLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= HealingWaveLifePercent && (PlayerHealth >= HealingWaveLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= HealingSurgeLifePercent && (PlayerHealth >= HealingSurgeLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= RiptideLifePercent && (PlayerHealth >= RiptideLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitRoleSpec(raidunits[i]) == API.TankRole && !API.UnitHasBuff(EarthShield, raidunits[i]) && EarthShieldTracking)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                        }
                    }
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotChanneling && !API.PlayerIsMoving && SaveQuake)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (isInterrupt && API.CanCast(WindShear) && Level >= 12 && IsInKickRange)
            {
                API.CastSpell(WindShear);
                return;
            }
            if (API.CanCast(WindShear) && Level >= 12 && IsInKickRange && CombatRoutine.GetPropertyBool("KICK") && API.FocusIsCasting() && (API.FocusIsChanneling ? API.FocusElapsedCastTimePercent >= interruptDelay : API.FocusCurrentCastTimeRemaining <= interruptDelay))
            {
                API.CastSpell(WindShear + "Focus");
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
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1 && NotChanneling && InRange)
            {
                API.CastSpell("Trinket1");
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2 && NotChanneling && InRange) 
            {
                API.CastSpell("Trinket2");
            }
        }

        public override void OutOfCombatPulse()
        {
            {
                if (API.PlayerCurrentCastTimeRemaining > 40)
                    return;
                if (AutoWolf && API.CanCast(GhostWolf) && !API.PlayerHasBuff(GhostWolf) && !API.PlayerIsMounted && API.PlayerIsMoving)
                {
                    API.CastSpell(GhostWolf);
                    return;
                }
            }

        }

    }
}



