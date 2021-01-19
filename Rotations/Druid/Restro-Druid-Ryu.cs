using System.Linq;
using System.Diagnostics;

namespace HyperElk.Core
{
    public class RestoDruid : CombatRoutine
    {
        //Spell Strings
        private string Rejuvenation = "Rejuvenation";
        private string Regrowth = "Regrowth";
        private string Lifebloom = "Lifebloom";
        private string WildGrowth = "Wild Growth";
        private string Swiftmend = "Swiftmend";
        private string Tranquility = "Tranquility";
        private string Moonfire = "Moonfire";
        private string Sunfire = "Sunfire";
        private string Wrath = "Wrath";
        private string Innervate = "Innervate";
        private string Ironbark = "Ironbark";
        private string Natureswiftness = "Nature's Swiftness";
        private string Barkskin = "Barkskin";
        private string Bearform = "Bear Form";
        private string Catform = "Cat Form";
        private string NaturesCure = "Nature's Cure";
        private string EntanglingRoots = "Entanngling Roots";
        private string Soothe = "Soothe";
        private string KindredSprirts = "Kindred Spirits";
        private string AdaptiveSwarm = "Adaptive Swarm";
        private string Fleshcraft = "Fleshcraft";
        private string Convoke = "Convoke the Spirits";
        private string RavenousFrenzy = "Ravenous Frenzy";
        private string Nourish = "Nourish";
        private string CenarionWard = "Cenarion Ward";
        private string TreeofLife = "Incarnation: Tree of Life";
        private string Overgrowth = "Overgrowth";
        private string Flourish = "Flourish";
        private string Renewal = "Renewal";
        private string AoE = "AOE";
        private string AoEP = "AOE Party";
        private string AoER = "AOE Raid";
        private string AoEDPS = "AOEDPS";
        private string AoEDPSRaid = "AOEDPS Raid";
        private string AoEDPSH = "AOEDPS Health";
        private string AoEDPSHRaid = "AOEDPS Health Raid";
        private string GerminationHoT = "Rejuvenation (Germination)";
        private string Clear = "Clearcasting";
        private string HeartoftheWild = "Heart of the Wild";
        private string TravelForm = "Travel Form";
        private string Soulshape = "Soulshape";
        private string CatForm = "Cat Form";
        private string BearForm = "Bear Form";
        private string MoonkinForm = "Moonkin Form";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string Trinket1 = "Trinket1";
        private string Trinket2 = "Trinket2";
        private string Mana = "Mana";
        private string FrenziedRegeneration = "Frenzied Regeneration";
        private string Ironfur = "Ironfur";
        private string Trinket = "Trinket";
        private string SouloftheForest = "Soul of the Forest";
        private string CenarionWardPlayer = "Cenarian Ward Player";
        private string Party1 = "party1";
        private string Party2 = "party2";
        private string Party3 = "party3";
        private string Party4 = "party4";
        private string Player = "player";
        private string PartySwap = "Target Swap";
        private string LifebloomL = "LifebloomL";
        private string TargetChange = "Target Change";
        private string AoERaid = "AOE Healing Raid";
        private string EclispeLunar = "Eclispe (Lunar)";
        private string EclispleSolar = "Eclispe (Solar)";
        private string Starfire = "Starfire";
        private string Starsurge = "Starsurge";
        private string Efflor = "Effloresence";
        private string Thrashbear = "Thrash Bear";
        private string Swipebear = "Swipe Bear";
        private string Mangle = "Mangle";
        private string Rip = "Rip";
        private string Rake = "Rake";
        private string Shred = "Shred";
        private string FerociousBite = "Ferocious Bite";
        private string Swipekitty = "Swipe Cat";
        private string Thrashkitty = "Thrash Cat";
        private string Quake = "Quake";
        private string Wake = "The Necrotic Wake";
        private string OtherSide = "De Other Side";
        private string Halls = "Halls of Atonement";
        private string Mists = "Mists of Tirna Scithe";
        private string Depths = "Sanguine Depths";
        private string Plague = "Plaguefall";
        private string Spires = "Spires of Ascension";
        private string ToP = "Theater of Pain";
        private string SwapSpeed = "Target Swap Speed";

        //Talents
        bool AbundanceTalent => API.PlayerIsTalentSelected(1, 1);
        bool NourishTalent => API.PlayerIsTalentSelected(1, 2);
        bool CenarionWardTalent => API.PlayerIsTalentSelected(1, 3);
        bool TigerDashTalent => API.PlayerIsTalentSelected(2, 1);
        bool RenewalTalent => API.PlayerIsTalentSelected(2, 2);
        bool WildChargeTalent => API.PlayerIsTalentSelected(2, 3);
        bool BalanceAffinity => API.PlayerIsTalentSelected(3, 1);
        bool FeralAffinity => API.PlayerIsTalentSelected(3, 2);
        bool GuardianAffintiy => API.PlayerIsTalentSelected(3, 3);
        bool MightyBashTalent => API.PlayerIsTalentSelected(4, 1);
        bool MassEntanglementTalent => API.PlayerIsTalentSelected(4, 2);
        bool HeartoftheWildTalent => API.PlayerIsTalentSelected(4, 3);
        bool SouloftheForestTalent => API.PlayerIsTalentSelected(5, 1);
        bool CultivationTalent => API.PlayerIsTalentSelected(5, 2);
        bool TreeofLifeTalent => API.PlayerIsTalentSelected(5, 3);
        bool InnerPeaceTalent => API.PlayerIsTalentSelected(6, 1);
        bool SpringBlossomsTalent => API.PlayerIsTalentSelected(6, 2);
        bool OvergrowthTalent => API.PlayerIsTalentSelected(6, 3);
        bool PhotosynthesisTalent => API.PlayerIsTalentSelected(7, 1);
        bool GerminationTalent => API.PlayerIsTalentSelected(7, 2);
        bool FlourishTalent => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        int[] SwapSpeedList = new int[] { 1000, 1250, 1500, 1750, 2000, 2250, 2500, 2750, 3000 };
        public string[] LegendaryList = new string[] { "None", "Verdant Infustion", "The Dark Titan's Lesson" };
        int PlayerHealth => API.TargetHealthPercent;
        string[] PlayerTargetArray = { "player", "party1", "party2", "party3", "party4" };
        string[] RaidTargetArray = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        string[] NecoritcWakeDispell = { "Chilled", "Frozen Binds", "Clinging Darkness", "Rasping Scream", "Heaving Retch", "Goresplatter" };
        string[] PlaugeFallDispell = {"Slime Injection", "Gripping Infection", "Cytotoxic Slash", "Venompiercer", "Wretched Phlegm"  };
        string[] MistsofTirnaScitheDispell = { "Repulsive Visage", "Soul Split", "Anima Injection", "Bewildering Pollen", "Bramblethorn Entanglement", "Dying Breath","Debilitating Poison",   };
        string[] HallofAtonementDispell = {"Sinlight Visions", "Siphon Life", "Turn to Stone" ,"Stony Veins","Curse of Stone", "Turned to Stone", "Curse of Obliteration"};
        string[] SanguineDepthsDispell = {"Anguished Cries","Wrack Soul","Sintouched Anima", "Curse of Suppression","Explosive Anger"  };
        string[] TheaterofPainDispell = {"Soul Corruption","Spectral Reach","Death Grasp","Shadow Vulnerability", "Curse of Desolation" };
        string[] DeOtherSideDispell = {"Cosmic Artifice", "Wailing Grief","Shadow Word:  Pain", "Soporific Shimmerdust", "Soporific Shimmerdust 2", "Hex" };
        string[] SpireofAscensionDispell = {"Dark Lance","Insidious Venom","Charged Anima","Lost Confidence","Burden of Knowledge","Internal Strife","Forced Confession", "Insidious Venom 2" };
        string[] DispellList = { "Chilled", "Frozen Binds", "Clinging Darkness", "Rasping Scream", "Heaving Retch", "Goresplatter", "Slime Injection", "Gripping Infection", "Cytotoxic Slash", "Venompiercer", "Wretched Phlegm",  "Repulsive Visage", "Soul Split", "Anima Injection", "Bewildering Pollen", "Bramblethorn Entanglement", "Dying Breath", "Debilitating Poison", "Sinlight Visions", "Siphon Life", "Turn to Stone", "Stony Veins", "Curse of Stone", "Turned to Stone", "Curse of Obliteration", "Anguished Cries", "Wrack Soul", "Sintouched Anima", "Curse of Suppression", "Explosive Anger", "Soul Corruption", "Spectral Reach", "Death Grasp", "Shadow Vulnerability", "Curse of Desolation", "Cosmic Artifice", "Wailing Grief", "Shadow Word:  Pain", "Soporific Shimmerdust", "Soporific Shimmerdust 2", "Hex", "Dark Lance", "Insidious Venom", "Charged Anima", "Lost Confidence", "Burden of Knowledge", "Internal Strife", "Forced Confession", "Insidious Venom 2" };
      //  public string[] InstanceList = { "The Necrotic Wake", "De Other Side", "Halls of Atonement", "Mists of Tirna Scithe", "Plaguefall", "Sanguine Depths", "Spires of Ascension", "Theater of Pain" };
        private static readonly Stopwatch player = new Stopwatch();
        private static readonly Stopwatch party1 = new Stopwatch();
        private static readonly Stopwatch party2 = new Stopwatch();
        private static readonly Stopwatch party3 = new Stopwatch();
        private static readonly Stopwatch party4 = new Stopwatch();
        private static readonly Stopwatch LifeBloomwatch = new Stopwatch();
        private static readonly Stopwatch EfflorWatch = new Stopwatch();
        private static readonly Stopwatch SwapWatch = new Stopwatch();


        private string UseLeg => LegendaryList[CombatRoutine.GetPropertyInt("Legendary")];
       // private string UseDispell => InstanceList[CombatRoutine.GetPropertyInt("Instance List")];
        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);
        private int UnitAboveHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) >= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitAboveHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) >= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitAboveHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitAboveHealthPercentRaid(HealthPercent) : UnitAboveHealthPercentParty(HealthPercent);

        private int FlourishRaidTracking(string buff) => raidunits.Count(p => API.UnitHasBuff(buff, p) && API.UnitBuffPlayerSrc(buff, p));
        private int FlourishPartyTracking(string buff) => units.Count(p => API.UnitHasBuff(buff, p) && API.UnitBuffPlayerSrc(buff, p));
        private int BuffRaidTracking(string buff) => raidunits.Count(p => API.UnitHasBuff(buff, p) && API.UnitBuffPlayerSrc(buff, p));
        private int BuffPartyTracking(string buff) => units.Count(p => API.UnitHasBuff(buff, p) && API.UnitBuffPlayerSrc(buff, p));
        private int BuffTracking(string buff) => API.PlayerIsInRaid ? BuffRaidTracking(buff) : BuffPartyTracking(buff);
        private int RangePartyTracking(int Range) => units.Count(p => API.UnitRange(p) <= Range);
        private int RangeRaidTracking(int Range) => raidunits.Count(p => API.UnitRange(p) <= Range);
        private int RangeTracking(int Range) => API.PlayerIsInRaid ? RangeRaidTracking(Range) : RangePartyTracking(Range);


        // private int FlourishTracking(string buff) => API.PlayerIsInRaid ? FlourishRaidTracking(Rejuvenation) : FlourishPartyTracking(Rejuvenation);

        private bool QuakingHelper => CombatRoutine.GetPropertyBool("QuakingHelper");
        bool ChannelingCov => API.CurrentCastSpellID("player") == 323764;
        bool ChannelingTranq => API.CurrentCastSpellID("player") == 740;
        private bool LifebloomPartyLTracking => BuffPartyTracking(LifebloomL) < 2;
        private bool LifebloomRaidLTracking => BuffRaidTracking(LifebloomL) < 2;
        private bool LifeBloomLTracking => API.PlayerIsInRaid ? LifebloomRaidLTracking : LifebloomPartyLTracking;
        private bool LifeBloomTracking => API.PlayerIsInRaid ? BuffRaidTracking(Lifebloom) < 1 : BuffPartyTracking(Lifebloom) < 1;
        private bool TrinketAoE => UnitBelowHealthPercent(TrinketLifePercent) >= AoENumber;
        private bool ConvokeAoE => UnitBelowHealthPercent(ConvLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling && !API.PlayerIsMoving && !ChannelingTranq;
        private bool WGAoE => UnitBelowHealthPercent(WGLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling && !API.PlayerIsMoving;
        private bool ToLAoE => UnitBelowHealthPercent(ToLLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool TranqAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(TranqLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(TranqLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && !API.PlayerIsMoving && !ChannelingCov;
        private bool FloruishRejTracking => API.PlayerIsInRaid ? FlourishRaidTracking(Rejuvenation) >= AoERaidNumber : FlourishPartyTracking(Rejuvenation) >= AoENumber;
        private bool FlourishRegTracking => API.PlayerIsInRaid ? FlourishRaidTracking(Regrowth) >= 1 : FlourishPartyTracking(Regrowth) >= 1;
        private bool FlourishLifeTracking => API.PlayerIsInRaid ? FlourishRaidTracking(Lifebloom) >= 1 : FlourishPartyTracking(Lifebloom) >= 1;
        private bool FlourishWGTracking => API.PlayerIsInRaid ? FlourishRaidTracking(WildGrowth) >= AoENumber : FlourishPartyTracking(WildGrowth) >= AoENumber;
        private bool FlourishTranqTracking => API.PlayerIsInRaid ? FlourishRaidTracking(Tranquility) >= AoERaidNumber : FlourishPartyTracking(Tranquility) >= AoENumber;
        private bool FloruishAoE => UnitBelowHealthPercent(FloruishLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool CWCheck => CenarionWardTalent && (API.TargetRoleSpec == API.TankRole && API.TargetHealthPercent <= CWTankLifePercent || API.TargetHealthPercent <= CWPlayerLifePercent) && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool NatureSwiftCheck => API.TargetHealthPercent <= NSLifePercent && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool OvergrowthCheck => API.TargetHealthPercent <= OvergrowthLifePercent && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving) && (!API.PlayerHasBuff(Lifebloom) || !API.PlayerHasBuff(WildGrowth) ||!API.PlayerHasBuff(Rejuvenation) || !API.PlayerHasBuff(Regrowth));
        private bool InnervateCheck => API.PlayerMana <= ManaPercent && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool IBCheck => API.TargetHealthPercent <= IronBarkLifePercent && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool NourishCheck => NourishTalent && API.TargetHealthPercent <= NourishLifePercent && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving;
        private bool RegrowthCheck => (API.PlayerHasBuff(Clear) && API.TargetHealthPercent <= 90 ||API.TargetHealthPercent <= RegrowthLifePercent) && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving;
        private bool RejCheck => API.TargetHealthPercent <= RejLifePercent && !API.PlayerCanAttackTarget && !API.TargetHasBuff(Rejuvenation) && (!GerminationTalent || API.TargetHasBuff(Rejuvenation) && GerminationTalent && !API.TargetHasBuff(GerminationHoT) && API.TargetHealthPercent <= RejGermLifePercent) && !ChannelingCov && !ChannelingTranq && NotChanneling && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool SwiftCheck => API.TargetHealthPercent <= SwiftmendLifePercent && !API.PlayerCanAttackTarget && API.SpellCharges(Swiftmend) > 0 && (API.TargetHasBuff(Rejuvenation) || API.TargetHasBuff(Regrowth) || API.TargetHasBuff(WildGrowth)) && (!API.PlayerIsMoving || API.PlayerIsMoving) && NotChanneling && !ChannelingCov && !ChannelingTranq;
        private bool LifeBloomCheck => API.TargetHealthPercent <= LifebloomLifePercent && UseLeg != "The Dark Titan's Lesson" && !API.PlayerCanAttackTarget && !API.TargetHasBuff(Lifebloom) && (!PhotosynthesisTalent && API.TargetRoleSpec == API.TankRole || PhotosynthesisTalent && (API.TargetRoleSpec == API.HealerRole  || API.TargetRoleSpec == API.TankRole)) && LifeBloomTracking && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);

        private bool LifeBloom2Check => API.TargetHealthPercent <= LifebloomLifePercent && !API.PlayerCanAttackTarget && !API.TargetHasBuff(Lifebloom) && (UseLeg == "The Dark Titan's Lesson" && (API.TargetRoleSpec == API.HealerRole || API.TargetRoleSpec == API.TankRole) || PhotosynthesisTalent && (!LifeBloomwatch.IsRunning || LifeBloomwatch.ElapsedMilliseconds >= 15000) && API.TargetRoleSpec == API.HealerRole || API.TargetRoleSpec == API.TankRole && !API.TargetHasBuff(Lifebloom)) && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool LifeBloomLegCheck => API.TargetHealthPercent <= LifebloomLifePercent && !API.PlayerCanAttackTarget && !API.TargetHasBuff(LifebloomL) && LifeBloomLTracking && UseLeg == "The Dark Titan's Lesson" && (API.TargetRoleSpec == API.HealerRole || API.TargetRoleSpec == API.TankRole);
        private bool FloruishCheck => (FloruishRejTracking && FlourishLifeTracking && FlourishRegTracking || FlourishWGTracking && FlourishTranqTracking) && !API.PlayerCanAttackTarget && FloruishAoE && FlourishTalent;
        private bool KyrianCheck => PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && !API.PlayerIsMoving && !ChannelingTranq;
        private bool NightFaeCheck => PlayerCovenantSettings == "Night Fae" && ConvokeAoE;
        private bool NecrolordCheck => PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingCov && !ChannelingTranq;
        private bool VenthyrCheck => PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingCov && !ChannelingTranq;
        private bool Forms => !API.PlayerHasBuff(BearForm) || !API.PlayerHasBuff(CatForm) || !API.PlayerHasBuff(MoonkinForm) || !API.PlayerHasBuff(Soulshape);
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool IsAutoSwap => API.ToggleIsEnabled("Auto Target");
        private bool IsOOC => API.ToggleIsEnabled("OOC");
        private bool PlayerSwap => API.UnitHealthPercent(Player) <= PartySwapPercent && API.TargetHealthPercent >= PartySwapPercent;
        private bool Party1Swap => API.UnitHealthPercent(Party1) <= PartySwapPercent && API.TargetHealthPercent >= PartySwapPercent;
        private bool Party2Swap => API.UnitHealthPercent(Party2) <= PartySwapPercent && API.TargetHealthPercent >= PartySwapPercent;
        private bool Party3Swap => API.UnitHealthPercent(Party3) <= PartySwapPercent && API.TargetHealthPercent >= PartySwapPercent;
        private bool Party4Swap => API.UnitHealthPercent(Party4) <= PartySwapPercent && API.TargetHealthPercent >= PartySwapPercent;
        private int FrenziedRegenerationLifePercent => numbList[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => numbList[CombatRoutine.GetPropertyInt(Ironfur)];
        private int BarkskinLifePercent => numbList[CombatRoutine.GetPropertyInt(Barkskin)];
        private int BearFormLifePercent => numbList[CombatRoutine.GetPropertyInt(BearForm)];
        private int RenewalLifePercent => numbList[CombatRoutine.GetPropertyInt(Renewal)];
        private int RejLifePercent => numbList[CombatRoutine.GetPropertyInt(Rejuvenation)];
        private int RejGermLifePercent => numbList[CombatRoutine.GetPropertyInt(GerminationHoT)];
        private int RegrowthLifePercent => numbList[CombatRoutine.GetPropertyInt(Regrowth)];
        private int LifebloomLifePercent => numbList[CombatRoutine.GetPropertyInt(Lifebloom)];
        private int CWTankLifePercent => numbList[CombatRoutine.GetPropertyInt(CenarionWard)];
        private int CWPlayerLifePercent => numbList[CombatRoutine.GetPropertyInt(CenarionWardPlayer)];
        private int IronBarkLifePercent => numbList[CombatRoutine.GetPropertyInt(Ironbark)];
        private int NourishLifePercent => numbList[CombatRoutine.GetPropertyInt(Nourish)];
        private int SwiftmendLifePercent => numbList[CombatRoutine.GetPropertyInt(Swiftmend)];
        private int FloruishLifePercent => numbList[CombatRoutine.GetPropertyInt(Flourish)];
        private int OvergrowthLifePercent => numbList[CombatRoutine.GetPropertyInt(Overgrowth)];
        private int WGLifePercent => numbList[CombatRoutine.GetPropertyInt(WildGrowth)];
        private int TranqLifePercent => numbList[CombatRoutine.GetPropertyInt(Tranquility)];
        private int ToLLifePercent => numbList[CombatRoutine.GetPropertyInt(TreeofLife)];
        private int NSLifePercent => numbList[CombatRoutine.GetPropertyInt(Natureswiftness)];
        private int ConvLifePercent => numbList[CombatRoutine.GetPropertyInt(Convoke)];
        private int TrinketLifePercent => numbList[CombatRoutine.GetPropertyInt(Trinket)];
        private int ManaPercent => numbList[CombatRoutine.GetPropertyInt(Innervate)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoERaid)];
        private int AoEDPSHLifePercent => numbList[CombatRoutine.GetPropertyInt(AoEDPSH)];
        private int AoEDPSNumber => numbPartyList[CombatRoutine.GetPropertyInt(AoEDPS)];
        private int AoEDPSRaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoEDPSRaid)];
        private int AoEDPSHRaidLifePercent => numbList[CombatRoutine.GetPropertyInt(AoEDPSHRaid)];
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private int PartySwapPercent => numbList[CombatRoutine.GetPropertyInt(PartySwap)];
        private int TargetChangePercent => numbList[CombatRoutine.GetPropertyInt(TargetChange)];
        private int SwapSpeedSetting => SwapSpeedList[CombatRoutine.GetPropertyInt(SwapSpeed)];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        private string UseHeart => CDUsage[CombatRoutine.GetPropertyInt(HeartoftheWild)];

        //private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoER)];
        private bool Quaking => ((API.PlayerCurrentCastTimeRemaining >= 200 || API.PlayerIsChanneling) && API.PlayerDebuffRemainingTime(Quake) < 200) && PlayerHasDebuff(Quake);
        private bool SaveQuake => (PlayerHasDebuff(Quake) && API.PlayerDebuffRemainingTime(Quake) > 200 && QuakingHelper || !PlayerHasDebuff(Quake) || !QuakingHelper);
        private bool QuakingWG => API.PlayerDebuffRemainingTime(Quake) > WGCastTime && PlayerHasDebuff(Quake);
        private bool QuakingRegrowth => API.PlayerDebuffRemainingTime(Quake) > RegrowthCastTime && PlayerHasDebuff(Quake);
        private bool QuakingConvoke => API.PlayerDebuffRemainingTime(Quake) > ConvokeCastTime && PlayerHasDebuff(Quake);
        private bool QuakingTranq => API.PlayerDebuffRemainingTime(Quake) > TranqCastTime && PlayerHasDebuff(Quake);
        private bool QuakingNourish => API.PlayerDebuffRemainingTime(Quake) > NourishCastTime && PlayerHasDebuff(Quake);
        private bool QuakingWrath => API.PlayerDebuffRemainingTime(Quake) > WrathCastTime && PlayerHasDebuff(Quake);
        private bool QuakingStar => API.PlayerDebuffRemainingTime(Quake) > StarfireCastTime && PlayerHasDebuff(Quake);
        float WGCastTime => 150f / (1f + API.PlayerGetHaste);
        float RegrowthCastTime => 150f / (1f + API.PlayerGetHaste);
        float ConvokeCastTime => 400f / (1f + API.PlayerGetHaste);
        float TranqCastTime => 800f / (1f + API.PlayerGetHaste);
        float NourishCastTime => 200f / (1f + API.PlayerGetHaste);
        float WrathCastTime => 150f / (1f + API.PlayerGetHaste);
        float StarfireCastTime => 225f / (1f + API.PlayerGetHaste);
        //General
        bool IsTrinkets1 => (UseTrinket1 == "With Cooldowns" && IsCooldowns && API.TargetHealthPercent <= TrinketLifePercent || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && TrinketAoE);
        bool IsTrinkets2 => (UseTrinket2 == "With Cooldowns" && IsCooldowns && API.TargetHealthPercent <= TrinketLifePercent || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && TrinketAoE);
        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private bool IsMelee => API.TargetRange < 6;
       // private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsDispell => API.ToggleIsEnabled("Dispel");
        public bool SootheList => API.TargetHasBuff("Raging") || API.TargetHasBuff("Unholy Frenzy") || API.TargetHasBuff("Renew") || API.TargetHasBuff("Additional Treads") || API.TargetHasBuff("Slime Coated") || API.TargetHasBuff("Stimulate Resistance") || API.TargetHasBuff("Unholy Fervor") || API.TargetHasBuff("Raging Tantrum") || API.TargetHasBuff("Loyal Beasts") || API.TargetHasBuff("Motivational Clubbing") || API.TargetHasBuff("Forsworn Doctrine") || API.TargetHasBuff("Seething Rage") || API.TargetHasBuff("Dark Shroud");
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

        //  public bool isInterrupt => CombatRoutine.GetPropertyBool("KICK") && API.TargetCanInterrupted && API.TargetIsCasting && (API.TargetIsChanneling ? API.TargetElapsedCastTime >= interruptDelay : API.TargetCurrentCastTimeRemaining <= interruptDelay);
        //  public int interruptDelay => random.Next((int)(CombatRoutine.GetPropertyInt("KICKTime") * 0.9), (int)(CombatRoutine.GetPropertyInt("KICKTime") * 1.1));
        public override void Initialize()
        {
            CombatRoutine.Name = "Resto Druid by Ryu";
            API.WriteLog("Welcome to Resto Druid v1.3 by Ryu");
            API.WriteLog("BETA ROTATION : Some things are still missing. Please post feedback in Druid Channel.");
            API.WriteLog("Mouseover Support is added FOR DISPELLS ONLY. Please create /cast [@mouseover] xx whereas xx is your Dispell and assign it the bind with MO on it in keybinds.");
            API.WriteLog("For all ground spells, either use @Cursor or when it is time to place it, the Bot will pause until you've placed it. If you'd perfer to use your own logic for them, please place them on ignore in the spellbook.");
            API.WriteLog("For using Dark Titan's Lesson, please BIND LifebloomL in your bindings to YOUR Lifebloom and select in the Legendary select. It changes the ID of the spell and that will mess the rotation if these things aren't done.");
            API.WriteLog("For the Quaking helper you just need to create an ingame macro with /stopcasting and bind it under the Macros Tab in Elk :-)");
            API.WriteLog("Please us a /cast [target=player] macro for Innervate to work properly or it will cast on your current target");
            API.WriteLog("If you wish to use Auto Target, please set your WoW keybinds in the keybinds => Targeting for Self and party, and then match them to the Macro's in the spell book. Enable it in the toggles. You must at least have a target for it swap, friendly or enemy. It will not swap BACK to a enemy. This does work for raid, however, requires the addon Bindpad. See Video in discord.");
            API.WriteLog("Special Thanks to Ajax and Goose/Zero for testing");

            //Buff
            CombatRoutine.AddBuff(Rejuvenation, 774);
            CombatRoutine.AddBuff(Regrowth, 8936);
            CombatRoutine.AddBuff(Lifebloom, 33763);
            CombatRoutine.AddBuff(WildGrowth, 48438);
            CombatRoutine.AddBuff(GerminationHoT, 155777);
            CombatRoutine.AddBuff(Clear, 16870);
            CombatRoutine.AddBuff(BearForm, 5487);
            CombatRoutine.AddBuff(Catform, 768);
            CombatRoutine.AddBuff(MoonkinForm, 197625);
            CombatRoutine.AddBuff(TravelForm, 783);
            CombatRoutine.AddBuff(Soulshape, 310143);
            CombatRoutine.AddBuff(FrenziedRegeneration, 22842);
            CombatRoutine.AddBuff(HeartoftheWild, 108291);
            CombatRoutine.AddBuff(Tranquility, 157982);
            CombatRoutine.AddBuff(SouloftheForest, 114108);
            CombatRoutine.AddBuff(LifebloomL, 188550);
            CombatRoutine.AddBuff(EclispeLunar, 48518);
            CombatRoutine.AddBuff(EclispleSolar, 48517);
            CombatRoutine.AddBuff(Quake, 240447);

            //Debuff
            CombatRoutine.AddDebuff(Sunfire, 164815);
            CombatRoutine.AddDebuff(Moonfire, 164812);
            CombatRoutine.AddDebuff(Thrashbear, 192090);
            CombatRoutine.AddDebuff(Thrashkitty, 106830);
            CombatRoutine.AddDebuff(Rip, 1079);
            CombatRoutine.AddDebuff(Rake, 155722);
            CombatRoutine.AddDebuff(Quake, 240447);
            //Soothe
            CombatRoutine.AddBuff("Raging", 132345);
            CombatRoutine.AddBuff("Unholy Frenzy", 136224);
            CombatRoutine.AddBuff("Renew", 135953);
            CombatRoutine.AddBuff("Additional Treads", 965900);
            CombatRoutine.AddBuff("Slime Coated", 3459153);
            CombatRoutine.AddBuff("Stimulate Resistance", 1769069);
            CombatRoutine.AddBuff("Stimulate Regeneration", 136079);
            CombatRoutine.AddBuff("Unholy Fervor", 2576093);
            CombatRoutine.AddBuff("Raging Tantrum", 132126);
            CombatRoutine.AddBuff("Loyal Beasts", 458967);
            CombatRoutine.AddBuff("Motivational Clubbing", 3554193);
            CombatRoutine.AddBuff("Forsworn Doctrine", 3528444);
            CombatRoutine.AddBuff("Seething Rage", 136225);
            CombatRoutine.AddBuff("Dark Shroud", 2576096);

            //Dispels
            CombatRoutine.AddDebuff("Chilled", 328664);
            CombatRoutine.AddDebuff("Frozen Binds", 320788);
            CombatRoutine.AddDebuff("Clinging Darkness", 323347);
            CombatRoutine.AddDebuff("Rasping Scream", 324293);
            CombatRoutine.AddDebuff("Heaving Retch", 320596);
            CombatRoutine.AddDebuff("Goresplatter", 338353);
            CombatRoutine.AddDebuff("Slime Injection", 329110);
            CombatRoutine.AddDebuff("Gripping Infection", 328180);
            CombatRoutine.AddDebuff("Cytotoxic Slash", 325552);
            CombatRoutine.AddDebuff("Venompiercer", 328395);
            CombatRoutine.AddDebuff("Wretched Phlegm", 334926);
            CombatRoutine.AddDebuff("Repulsive Visage", 328756);
            CombatRoutine.AddDebuff("Soul Split", 322557);
            CombatRoutine.AddDebuff("Anima Injection", 325224);
            CombatRoutine.AddDebuff("Bewildering Pollen", 321968);
            CombatRoutine.AddDebuff("Bramblethorn Entanglement", 324859);
            CombatRoutine.AddDebuff("Dying Breath", 322968);
            CombatRoutine.AddDebuff("Debilitating Poison", 326092);
            CombatRoutine.AddDebuff("Sinlight Visions", 339237);
            CombatRoutine.AddDebuff("Siphon Life", 325701);
            CombatRoutine.AddDebuff("Turn to Stone", 326607);
            CombatRoutine.AddDebuff("Stony Veins", 326632);
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
            CombatRoutine.AddDebuff("Dark Lance", 327481);
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
            CombatRoutine.AddSpell(Rejuvenation, 774);
            CombatRoutine.AddSpell(Regrowth, 8936);
            CombatRoutine.AddSpell(Lifebloom, 33763);
            CombatRoutine.AddSpell(WildGrowth, 48438);
            CombatRoutine.AddSpell(Swiftmend, 18562);
            CombatRoutine.AddSpell(Tranquility, 740);
            CombatRoutine.AddSpell(Innervate, 29166);
            CombatRoutine.AddSpell(Ironbark, 102342);
            CombatRoutine.AddSpell(Natureswiftness, 132158);
            CombatRoutine.AddSpell(Barkskin, 22812);
            CombatRoutine.AddSpell(NaturesCure, 88423);
            CombatRoutine.AddSpell(EntanglingRoots, 339);
            CombatRoutine.AddSpell(Soothe, 2908);
            CombatRoutine.AddSpell(AdaptiveSwarm, 325727);
            CombatRoutine.AddSpell(Fleshcraft, 324631);
            CombatRoutine.AddSpell(Convoke, 323764);
            CombatRoutine.AddSpell(RavenousFrenzy, 323546);
            CombatRoutine.AddSpell(Nourish, 50464);
            CombatRoutine.AddSpell(CenarionWard, 102351);
            CombatRoutine.AddSpell(TreeofLife, 33891);
            CombatRoutine.AddSpell(Overgrowth, 203651);
            CombatRoutine.AddSpell(Flourish, 197721);
            CombatRoutine.AddSpell(Renewal, 108238);
            CombatRoutine.AddSpell(TravelForm, 783);
            CombatRoutine.AddSpell(HeartoftheWild, 319454);
            CombatRoutine.AddSpell(LifebloomL, 188550);
            CombatRoutine.AddSpell(Efflor, 145205);

            //OWL
            CombatRoutine.AddSpell(MoonkinForm, 24858);
            CombatRoutine.AddSpell(Moonfire, 8921);
            CombatRoutine.AddSpell(Sunfire, 93402);
            CombatRoutine.AddSpell(Wrath, 5176);
            CombatRoutine.AddSpell(Starfire, 197628);
            CombatRoutine.AddSpell(Starsurge, 197626);

            //Kitty
            CombatRoutine.AddSpell(Catform, 768);
            CombatRoutine.AddSpell(Rip, 1079);
            CombatRoutine.AddSpell(Rake, 1822);
            CombatRoutine.AddSpell(Shred, 5221);
            CombatRoutine.AddSpell(FerociousBite, 22568);
            CombatRoutine.AddSpell(Thrashkitty, 106830);
            CombatRoutine.AddSpell(Swipekitty, 106785);

            //Bear
            CombatRoutine.AddSpell(Bearform, 5487);
            CombatRoutine.AddSpell(FrenziedRegeneration, 22842);
            CombatRoutine.AddSpell(Ironfur, 192081);
            CombatRoutine.AddSpell(Thrashbear, 77758);
            CombatRoutine.AddSpell(Mangle, 33917);
            CombatRoutine.AddSpell(Swipebear, 213771);

            //Toggle
            CombatRoutine.AddToggle("Auto Target");
            CombatRoutine.AddToggle("OOC");
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Dispel");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Macro
            CombatRoutine.AddMacro(NaturesCure + "MO");
            CombatRoutine.AddMacro(Trinket1);
            CombatRoutine.AddMacro(Trinket2);
            CombatRoutine.AddMacro("Stopcast", "F10");
            CombatRoutine.AddMacro("Assist");
            CombatRoutine.AddMacro(Player);
            CombatRoutine.AddMacro(Party1);
            CombatRoutine.AddMacro(Party2);
            CombatRoutine.AddMacro(Party3);
            CombatRoutine.AddMacro(Party4);
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
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
            CombatRoutine.AddProp("AutoForm", "AutoForm", false, "Will auto switch forms", "Generic");
            CombatRoutine.AddProp(Barkskin, Barkskin + " Life Percent", numbList, "Life percent at which" + Barkskin + "is used, set to 0 to disable", "Defense", 25);
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", numbList, "Life percent at which" + Renewal + "is used, if talented, set to 0 to disable", "Defense", 45);
            CombatRoutine.AddProp(FrenziedRegeneration, FrenziedRegeneration + " Life Percent", numbList, "Life percent at which" + FrenziedRegeneration + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp(Ironfur, Ironfur + " Life Percent", numbList, "Life percent at which" + Ironfur + "is used, set to 0 to disable", "Defense", 90);
            CombatRoutine.AddProp(BearForm, BearForm + " Life Percent", numbList, "Life percent at which rota will go into" + BearForm + "set to 0 to disable", "Defense", 10);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 1);
            CombatRoutine.AddProp(HeartoftheWild, "Use " + HeartoftheWild, CDUsage, "Use " + HeartoftheWild + "On Cooldown, with Cooldowns, Not Used", "Cooldowns", 1);
            CombatRoutine.AddProp("QuakingHelper", "Quaking Helper", false, "Will cancel casts on Quaking", "Generic");
            CombatRoutine.AddProp(SwapSpeed, SwapSpeed + "Speed ", SwapSpeedList, "Speed at which to change targets, it is in Milliseconds, to convert to seconds please divide by 1000. If you don't understand, please leave at at default setting", "Targeting", 1250);

            //CombatRoutine.AddProp(PartySwap, PartySwap + " Life Percent", numbList, "Life percent at which" + PartySwap + "is used, set to 0 to disable", "Healing", 0);
            //CombatRoutine.AddProp(TargetChange, TargetChange + " Life Percent", numbList, "Life percent at which" + TargetChange + "is used to change from your current target, when using Auto Swap logic, set to 0 to disable", "Healing", 0);
            CombatRoutine.AddProp("OOC", "Healing out of Combat", true, "Heal out of combat", "Healing");
            CombatRoutine.AddProp(Rejuvenation, Rejuvenation + " Life Percent", numbList, "Life percent at which " + Rejuvenation + " is used, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(Regrowth, Regrowth + " Life Percent", numbList, "Life percent at which " + Regrowth + " is used, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(Lifebloom, Lifebloom + " Life Percent", numbList, "Life percent at which " + Lifebloom + " is used, set to 0 to disable", "Healing", 100);
            CombatRoutine.AddProp(CenarionWard, CenarionWard + " Life Percent", numbList, "Life percent at which " + CenarionWard + " is used for the tank, if talented, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(CenarionWardPlayer, CenarionWard + " Life Percent", numbList, "Life percent at which " + CenarionWard + " is used for other players, if talented, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(Ironbark, Ironbark + " Life Percent", numbList, "Life percent at which " + Ironbark + " is used, set to 0 to disable", "Healing", 25);
            CombatRoutine.AddProp(Nourish, Nourish + " Life Percent", numbList, "Life percent at which " + Nourish + " is used, set to 0 to disable", "Healing", 55);
            CombatRoutine.AddProp(Swiftmend, Swiftmend + " Life Percent", numbList, "Life percent at which " + Swiftmend + " is used, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(Natureswiftness, Natureswiftness + " Life Percent", numbList, "Life percent at which " + Natureswiftness + " is used, set to 0 to disable", "Healing", 45);
            CombatRoutine.AddProp(Overgrowth, Overgrowth + " Life Percent", numbList, "Life percent at which " + Overgrowth + " is used, set to 0 to disable", "Healing", 75);
            CombatRoutine.AddProp(Innervate, Innervate + " Mana Percent", numbList, "Mana percent at which " + Innervate + " is used, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(Flourish, Flourish + " Life Percent", numbList, "Life percent at which " + Flourish + " is used when AoE Number of members are at and have the HoTs needed, set to 0 to disable", "Healing", 45);
            CombatRoutine.AddProp(Convoke, Convoke + " Life Percent", numbList, "Life percent at which " + Convoke + " is used when AoE Number of members are at and have the HoTs needed, set to 0 to disable", "Healing", 55);
            CombatRoutine.AddProp(WildGrowth, WildGrowth + " Life Percent", numbList, "Life percent at which " + WildGrowth + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 65);
            CombatRoutine.AddProp(Tranquility, Tranquility + " Life Percent", numbList, "Life percent at which " + Tranquility + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 20);
            CombatRoutine.AddProp(TreeofLife, TreeofLife + " Life Percent", numbList, "Life percent at which " + TreeofLife + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 45);
            CombatRoutine.AddProp(AoE, "Number of units for AoE Healing ", numbPartyList, " Units for AoE Healing", "Healing", 3);
            CombatRoutine.AddProp(AoERaid, "Number of units for AoE Healing in raid ", numbRaidList, " Units for AoE Healing in raid", "Healing", 7);
            CombatRoutine.AddProp(AoEDPS, "Number of units needed to be above DPS Health Percent to DPS in party ", numbPartyList, " Units above for DPS ", "Healing", 2);
            CombatRoutine.AddProp(AoEDPSH, "Life Percent for units to be above for DPS", numbList, "Health percent at which DPS in party" + "is used,", "Healing", 80);
            CombatRoutine.AddProp(AoEDPSRaid, "Number of units needed to be above DPS Health Percent to DPS in Raid ", numbRaidList, " Units above for DPS ", "Healing", 4);
            CombatRoutine.AddProp(AoEDPSHRaid, "Life Percent for units to be above for DPS in raid", numbList, "Health percent at which DPS" + "is used,", "Healing", 70);
            CombatRoutine.AddProp("Legendary", "Select your Legendary", LegendaryList, "Select Your Legendary", "Legendary");
            CombatRoutine.AddProp(Trinket, Trinket + " Life Percent", numbList, "Life percent at which " + "Trinkets" + " should be used, set to 0 to disable", "Healing", 55);
          //  CombatRoutine.AddProp("Instance List", "Select your instance", InstanceList, "Select Your Instance for Dispells", "Dispell");
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", CDUsageWithAOE, "When should trinket 1 be used", "Trinket", 0);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", CDUsageWithAOE, "When should trinket 2 be used", "Trinket", 0);

        }

        public override void Pulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 && QuakingHelper && Quaking)
            {
                API.CastSpell("Stopcast");
                API.WriteLog("Debuff Time Remaining for Quake : " + API.PlayerDebuffRemainingTime(Quake));
                return;
            }
            if (API.LastSpellCastInGame == Efflor)
            {
                EfflorWatch.Restart();
            }
            if (!API.PlayerIsMounted && !API.PlayerSpellonCursor && !API.PlayerHasBuff(TravelForm) && !API.PlayerHasBuff(BearForm) && !API.PlayerHasBuff(CatForm) && !API.PlayerHasBuff(Soulshape) && (IsOOC || API.PlayerIsInCombat))
            {
                #region Dispell
                if (IsDispell)
                {
                    if (API.CanCast(NaturesCure) && !ChannelingTranq && !ChannelingCov && NotChanneling)
                    {
                        for (int i = 0; i < DispellList.Length; i++)
                        {
                            if (TargetHasDispellAble(DispellList[i]))
                            {
                                API.CastSpell(NaturesCure);
                                return;
                            }
                        }
                    }
                    if (API.CanCast(NaturesCure) && IsMouseover && !ChannelingTranq && !ChannelingCov && NotChanneling)
                    {
                        for (int i = 0; i < DispellList.Length; i++)
                        {
                            if (MouseouverHasDispellAble(DispellList[i]))
                            {
                                API.CastSpell(NaturesCure + "MO");
                                return;
                            }
                        }
                    }
                }
                #endregion
                if (API.CanCast(Soothe) && (SootheList) && InRange)
                {
                    API.CastSpell(Soothe);
                    return;
                }
                if (API.CanCast(Efflor) && API.PlayerIsInCombat && (!EfflorWatch.IsRunning || EfflorWatch.ElapsedMilliseconds >= 30000))
                {
                    API.CastSpell(Efflor);
                    EfflorWatch.Start();
                    return;
                }
                if (API.CanCast(Convoke) && NightFaeCheck && InRange && (!QuakingConvoke || QuakingConvoke && QuakingHelper))
                {
                    API.CastSpell(Convoke);
                    return;
                }
                if (API.CanCast(AdaptiveSwarm) && NecrolordCheck && InRange)
                {
                    API.CastSpell(AdaptiveSwarm);
                    return;
                }
                if (API.CanCast(RavenousFrenzy) && VenthyrCheck && InRange)
                {
                    API.CastSpell(RavenousFrenzy);
                    return;
                }
                if (API.CanCast(Innervate) && InnervateCheck && InRange) 
                {
                    API.CastSpell(Innervate);
                    return;
                }
                if (API.CanCast(Overgrowth) && OvergrowthCheck && InRange)
                {
                    API.CastSpell(Overgrowth);
                    return;
                }
                if (API.CanCast(Natureswiftness) && NatureSwiftCheck && InRange)
                {
                    API.CastSpell(Natureswiftness);
                    return;
                }
                if (API.CanCast(TreeofLife) && TreeofLifeTalent && InRange && ToLAoE)
                {
                    API.CastSpell(TreeofLife); ;
                    return;
                }
                if (API.CanCast(Lifebloom) && InRange && LifeBloomCheck)
                {
                    API.CastSpell(Lifebloom);
                    return;
                }
                if (API.CanCast(LifebloomL) && InRange && LifeBloomLegCheck)
                {
                    API.CastSpell(LifebloomL);
                    return;
                }
                if (API.CanCast(WildGrowth) && InRange && WGAoE || API.CanCast(WildGrowth) && API.PlayerHasBuff(SouloftheForest) && UnitBelowHealthPercent(65) >= 3 && InRange && (!QuakingWG || QuakingWG && QuakingHelper))
                {
                    API.CastSpell(WildGrowth);
                    return;
                }
                if (API.CanCast(Tranquility) && InRange && TranqAoE && (!QuakingTranq || QuakingTranq && QuakingHelper))
                {
                    API.CastSpell(Tranquility);
                    return;
                }
                if (API.CanCast(Flourish) && InRange && FloruishCheck)
                {
                    API.CastSpell(Flourish);
                    return;
                }
                if (API.CanCast(Ironbark) && InRange && IBCheck)
                {
                    API.CastSpell(Ironbark);
                    return;
                }
                if (API.CanCast(CenarionWard) && InRange && CWCheck)
                {
                    API.CastSpell(CenarionWard);
                    return;
                }
                if (API.CanCast(Swiftmend) && InRange && SwiftCheck)
                {
                    API.CastSpell(Swiftmend);
                    return;
                }
                if (API.CanCast(Rejuvenation) && InRange && RejCheck)
                {
                    API.CastSpell(Rejuvenation);
                    return;
                }
                if (API.CanCast(Regrowth) && InRange && RegrowthCheck && (!QuakingRegrowth || QuakingRegrowth && QuakingHelper))
                {
                    API.CastSpell(Regrowth);
                    return;
                }
                if (API.CanCast(Nourish) && InRange && NourishCheck && (!QuakingNourish || QuakingNourish && QuakingHelper))
                {
                    API.CastSpell(Nourish);
                    return;
                }
                //DPS
                if (API.CanCast(HeartoftheWild) && HeartoftheWildTalent && !ChannelingCov && !ChannelingTranq && (UseHeart == "With Cooldowns" && IsCooldowns || UseHeart == "On Cooldown"))
                {
                    API.CastSpell(HeartoftheWild);
                    return;
                }
                if (API.CanCast(MoonkinForm) && !API.PlayerHasBuff(MoonkinForm) && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(MoonkinForm);
                    return;
                }
                if (API.CanCast(Starsurge) && API.PlayerHasBuff(MoonkinForm) && BalanceAffinity && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && (API.PlayerHasBuff(EclispeLunar) || API.PlayerHasBuff(EclispleSolar)) && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Starsurge);
                    return;
                }
                if (API.CanCast(Starfire) && API.PlayerHasBuff(MoonkinForm) && BalanceAffinity && InRange && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && API.TargetHasDebuff(Sunfire) && API.TargetHasDebuff(Moonfire) && API.TargetHealthPercent > 0 && API.PlayerHasBuff(EclispeLunar) && (!QuakingStar || QuakingStar && QuakingHelper))
                {
                    API.CastSpell(Starfire);
                    return;
                }
                if (API.CanCast(Wrath) && InRange && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && API.PlayerHasBuff(EclispleSolar) && API.TargetHealthPercent > 0 && (!QuakingWrath || QuakingWrath && QuakingHelper))
                {
                    API.CastSpell(Wrath);
                    return;
                }
                if (API.CanCast(Wrath) && InRange && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && API.TargetHasDebuff(Sunfire) && API.TargetHasDebuff(Moonfire) && API.TargetHealthPercent > 0 && (!QuakingWrath || QuakingWrath && QuakingHelper))
                {
                    API.CastSpell(Wrath);
                    return;
                }
                if (API.CanCast(Starfire) && API.PlayerHasBuff(MoonkinForm) && BalanceAffinity && InRange && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && API.TargetHasDebuff(Sunfire) && API.TargetHasDebuff(Moonfire) && API.TargetHealthPercent > 0 && API.TargetUnitInRangeCount >= 3 && (!QuakingStar || QuakingStar && QuakingHelper))
                {
                    API.CastSpell(Starfire);
                    return;
                }
                if (API.CanCast(Moonfire) && !API.PlayerIsCasting(true) && InRange && !API.TargetHasDebuff(Moonfire) && API.PlayerCanAttackTarget && NotChanneling && !ChannelingTranq && !ChannelingCov && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Moonfire);
                    return;
                }
                if (API.CanCast(Sunfire) && !API.PlayerIsCasting(true) && InRange && !API.TargetHasDebuff(Sunfire) && API.PlayerCanAttackTarget && NotChanneling && !ChannelingTranq && !ChannelingCov && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Sunfire);
                    return;
                }
                //Auto Target
                if (IsAutoSwap)
                {
                    if (API.PlayerIsInGroup && InRange)
                    {
                        for (int i = 0; i < units.Length; i++)
                        {
                            if (API.UnitHealthPercent(units[i]) <= 15 && (PlayerHealth >= 15 || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0 && API.UnitRange(units[i]) <= 40)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= IronBarkLifePercent && (PlayerHealth >= IronBarkLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0 && API.UnitRange(units[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= IronBarkLifePercent && (PlayerHealth >= IronBarkLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0 && API.UnitRange(units[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= RegrowthLifePercent && (PlayerHealth >= RegrowthLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0 && API.UnitRange(units[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= RejLifePercent && (PlayerHealth >= RejLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0 && API.UnitRange(units[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitRoleSpec(units[i]) == API.TankRole && !API.UnitHasBuff(Lifebloom, units[i]) && LifeBloomTracking && UseLeg != "The Dark Titan's Lesson" && API.UnitRange(units[i]) <= 40)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitRoleSpec(units[i]) == API.TankRole && UseLeg == "The Dark Titan's Lesson" && !UnitHasBuff(LifebloomL, units[i]) && API.UnitRange(units[i]) <= 40)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (!API.PlayerHasBuff(LifebloomL) && UseLeg == "The Dark Titan's Lesson" && API.UnitRange(units[i]) <= 40)
                            {
                                API.CastSpell(Player);
                                return;
                            }
                            if (!API.PlayerCanAttackTarget && API.UnitRoleSpec(units[i]) == API.TankRole && !API.MacroIsIgnored("Assist") && UnitAboveHealthPercentParty(AoEDPSHLifePercent) >= AoEDPSNumber && API.UnitRange(units[i]) <= 4 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                API.CastSpell("Assist");
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                        }
                    }
                    if (API.PlayerIsInRaid && InRange)
                    {
                        for (int i = 0; i < raidunits.Length; i++)
                        {
                            if (API.UnitHealthPercent(raidunits[i]) <= 15 && (PlayerHealth >= 15 || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0 && API.UnitRange(raidunits[i]) <= 40)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= IronBarkLifePercent && (PlayerHealth >= IronBarkLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0 && API.UnitRange(raidunits[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= SwiftmendLifePercent && (PlayerHealth >= SwiftmendLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0 && API.UnitRange(raidunits[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= RegrowthLifePercent && (PlayerHealth >= RegrowthLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0 && API.UnitRange(raidunits[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting)) 
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= RejLifePercent && (PlayerHealth >= RejLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0 && API.UnitRange(raidunits[i]) <= 40 && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitRoleSpec(raidunits[i]) == API.TankRole && !API.UnitHasBuff(Lifebloom, raidunits[i]) && LifeBloomTracking && UseLeg != "The Dark Titan's Lesson" && API.UnitRange(raidunits[i]) <= 40)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                return;
                            }
                            if (API.UnitRoleSpec(raidunits[i]) == API.TankRole && UseLeg == "The Dark Titan's Lesson" && !UnitHasBuff(LifebloomL, raidunits[i]) && LifebloomRaidLTracking && API.UnitRange(raidunits[i]) <= 40)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (!API.PlayerHasBuff(LifebloomL) && UseLeg == "The Dark Titan's Lesson" && LifebloomRaidLTracking && API.UnitRange(raidunits[i]) <= 40)
                            {
                                API.CastSpell(Player);
                                return;
                            }
                            if (!API.PlayerCanAttackTarget && API.UnitRange(raidunits[i]) <= 4 && API.UnitRoleSpec(raidunits[i]) == API.TankRole && !API.MacroIsIgnored("Assist") && UnitAboveHealthPercentRaid(AoEDPSHRaidLifePercent) >= AoEDPSRaidNumber && (!SwapWatch.IsRunning || SwapWatch.ElapsedMilliseconds >= SwapSpeedSetting))
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                SwapWatch.Stop();
                                SwapWatch.Start();
                                API.CastSpell("Assist");
                                return;
                            }
                        }
                    }
                    
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotChanneling && !ChannelingTranq && !ChannelingCov && !API.PlayerIsMoving)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.PlayerHealthPercent <= BarkskinLifePercent && Level >= 24 && API.CanCast(Barkskin))
            {
                API.CastSpell(Barkskin);
                return;
            }
            if (API.CanCast(Renewal) && API.PlayerHealthPercent <= RenewalLifePercent && RenewalTalent)
            {
                API.CastSpell(Renewal);
                return;
            }
            if (API.PlayerHealthPercent <= FrenziedRegenerationLifePercent && API.PlayerRage >= 10 && API.CanCast(FrenziedRegeneration) && API.PlayerHasBuff(BearForm) && GuardianAffintiy)
            {
                API.CastSpell(FrenziedRegeneration);
                return;
            }
            if (API.PlayerHealthPercent <= IronfurLifePercent && API.PlayerRage >= 40 && API.CanCast(Ironfur) && GuardianAffintiy && API.PlayerHasBuff(BearForm))
            {
                API.CastSpell(Ironfur);
                return;
            }
            if (API.PlayerHealthPercent <= BearFormLifePercent && Level >= 8 && AutoForm && API.CanCast(BearForm) && !API.PlayerHasBuff(BearForm))
            {
                API.CastSpell(BearForm);
                return;
            }
            if (API.PlayerHealthPercent > BearFormLifePercent && BearFormLifePercent != 0 && API.CanCast(BearForm) && API.PlayerHasBuff(BearForm) && AutoForm)
            {
                API.CastSpell(BearForm);
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
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1 && !ChannelingCov && !ChannelingTranq && NotChanneling && InRange)
            {
                API.CastSpell("Trinket1");
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2 && !ChannelingCov && !ChannelingTranq && NotChanneling && InRange)
            {
                API.CastSpell("Trinket2");
            }
            if (API.PlayerHasBuff(BearForm))
            {
                if (API.CanCast(Thrashbear) && API.TargetRange < 9 && GuardianAffintiy && API.PlayerCanAttackTarget)
                {
                    API.CastSpell(Thrashbear);
                    return;
                }
                if (API.CanCast(Mangle) && API.TargetRange < 6 && API.PlayerCanAttackTarget) 
                {
                    API.CastSpell(Mangle);
                    return;
                }
                if (API.CanCast(Swipebear) && FeralAffinity && API.TargetRange < 9 && API.PlayerCanAttackTarget)
                {
                    API.CastSpell(Swipebear);
                    return;
                }
            }
            if (API.PlayerHasBuff(CatForm))
            {
                if (API.PlayerComboPoints == 5)
                {
                    if (API.CanCast(Rip) && FeralAffinity && API.TargetRange < 6 && API.PlayerEnergy >= 20 && (!API.TargetHasDebuff(Rip) || API.TargetDebuffRemainingTime(Rip) < 600) && API.PlayerCanAttackTarget)
                    {
                        API.CastSpell(Rip);
                        return;
                    }
                    if (API.CanCast(FerociousBite) && API.TargetRange < 6 && API.PlayerEnergy >= 50 && API.PlayerCanAttackTarget)
                    {
                        API.CastSpell(FerociousBite);
                        return;
                    }
                }
                if (API.PlayerComboPoints < 5)
                {
                    if (API.CanCast(Rake) && FeralAffinity && API.TargetRange < 6 && API.PlayerEnergy >= 35 && API.TargetDebuffRemainingTime(Rake) <= 360 && API.PlayerCanAttackTarget)
                    {
                        API.CastSpell(Rake);
                        return;
                    }
                    if (API.CanCast(Thrashkitty) && API.TargetRange < 9 && API.PlayerEnergy >= 40 && GuardianAffintiy && API.TargetDebuffRemainingTime(Thrashkitty) <= 200 && API.PlayerCanAttackTarget)
                    {
                        API.CastSpell(Thrashkitty);
                        return;
                    }
                    if (API.CanCast(Swipekitty) && FeralAffinity && API.PlayerEnergy >= 35 && API.TargetRange < 9 && API.PlayerUnitInMeleeRangeCount >= 2 && API.PlayerCanAttackTarget)
                    {
                        API.CastSpell(Swipekitty);
                        return;
                    }
                    if (API.CanCast(Shred) && API.TargetRange < 6 && API.PlayerEnergy >= 40 && API.PlayerCanAttackTarget)
                    {
                        API.CastSpell(Shred);
                        return;
                    }
                }
            }
        }

        public override void OutOfCombatPulse()
        {
            {
                if (API.PlayerCurrentCastTimeRemaining > 40)
                    return;
                if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !API.PlayerHasBuff(TravelForm))
                {
                    API.CastSpell(TravelForm);
                    return;
                }
            }

        }

    }
}



