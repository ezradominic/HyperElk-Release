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
        public string[] LegendaryList = new string[] { "None", "Verdant Infustion", "The Dark Titan's Lesson" };
        int PlayerHealth => API.TargetHealthPercent;
        string[] PlayerTargetArray = { "player", "party1", "party2", "party3", "party4" };
        string[] RaidTargetArray = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };

        private static readonly Stopwatch player = new Stopwatch();
        private static readonly Stopwatch party1 = new Stopwatch();
        private static readonly Stopwatch party2 = new Stopwatch();
        private static readonly Stopwatch party3 = new Stopwatch();
        private static readonly Stopwatch party4 = new Stopwatch();
        private static readonly Stopwatch LifeBloomwatch = new Stopwatch();
        private static readonly Stopwatch EfflorWatch = new Stopwatch();
        private static readonly Stopwatch GroundWatch = new Stopwatch();

        private string UseLeg => LegendaryList[CombatRoutine.GetPropertyInt("Legendary")];
        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);

        private int FlourishRaidTracking(string buff) => raidunits.Count(p => API.UnitHasBuff(buff, p));
        private int FlourishPartyTracking(string buff) => units.Count(p => API.UnitHasBuff(buff, p));
       // private int FlourishTracking(string buff) => API.PlayerIsInRaid ? FlourishRaidTracking(Rejuvenation) : FlourishPartyTracking(Rejuvenation);

        bool ChannelingCov => API.CurrentCastSpellID("player") == 323764;
        bool ChannelingTranq => API.CurrentCastSpellID("player") == 740;

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
        private bool LifeBloomCheck => API.TargetHealthPercent <= LifebloomLifePercent && !API.PlayerCanAttackTarget && (!PhotosynthesisTalent && !API.TargetHasBuff(Lifebloom) && API.TargetRoleSpec == API.TankRole || PhotosynthesisTalent && (!LifeBloomwatch.IsRunning || LifeBloomwatch.ElapsedMilliseconds >= 15000) && API.TargetRoleSpec == API.HealerRole && !API.TargetHasBuff(Lifebloom) || API.TargetRoleSpec == API.TankRole && (!LifeBloomwatch.IsRunning || LifeBloomwatch.ElapsedMilliseconds >= 15000) && !API.TargetHasBuff(Lifebloom)) && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool LifeBloomLegCheck => API.TargetHealthPercent <= LifebloomLifePercent && !API.PlayerCanAttackTarget && !API.TargetHasBuff(LifebloomL) && UseLeg == "The Dark Titan's Lesson" && (API.TargetRoleSpec == API.HealerRole || API.TargetRoleSpec == API.TankRole);
        private bool FloruishCheck => (FloruishRejTracking && FlourishLifeTracking && FlourishRegTracking || FlourishWGTracking && FlourishTranqTracking) && !API.PlayerCanAttackTarget && FloruishAoE && FlourishTalent;
        private bool KyrianCheck => PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && !API.PlayerIsMoving && !ChannelingTranq;
        private bool NightFaeCheck => PlayerCovenantSettings == "Night Fae" && ConvokeAoE;
        private bool NecrolordCheck => PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingCov && !ChannelingTranq;
        private bool VenthyrCheck => PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingCov && !ChannelingTranq;
        private bool Forms => !API.PlayerHasBuff(BearForm) || !API.PlayerHasBuff(CatForm) || !API.PlayerHasBuff(MoonkinForm) || !API.PlayerHasBuff(Soulshape);
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool IsAutoSwap => API.ToggleIsEnabled("Auto Target");
        private bool IsOOC => CombatRoutine.GetPropertyBool("OOC");
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
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private int PartySwapPercent => numbList[CombatRoutine.GetPropertyInt(PartySwap)];
        private int TargetChangePercent => numbList[CombatRoutine.GetPropertyInt(TargetChange)];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        private string UseHeart => CDUsage[CombatRoutine.GetPropertyInt(HeartoftheWild)];
        //private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoER)];

        //General
        bool IsTrinkets1 => (UseTrinket1 == "With Cooldowns" && IsCooldowns && API.TargetHealthPercent <= TrinketLifePercent || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && TrinketAoE);
        bool IsTrinkets2 => (UseTrinket2 == "With Cooldowns" && IsCooldowns && API.TargetHealthPercent <= TrinketLifePercent || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && TrinketAoE);
        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private bool IsMelee => API.TargetRange < 6;
       // private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");




        //  public bool isInterrupt => CombatRoutine.GetPropertyBool("KICK") && API.TargetCanInterrupted && API.TargetIsCasting && (API.TargetIsChanneling ? API.TargetElapsedCastTime >= interruptDelay : API.TargetCurrentCastTimeRemaining <= interruptDelay);
        //  public int interruptDelay => random.Next((int)(CombatRoutine.GetPropertyInt("KICKTime") * 0.9), (int)(CombatRoutine.GetPropertyInt("KICKTime") * 1.1));
        public override void Initialize()
        {
            CombatRoutine.Name = "Resto Druid by Ryu";
            API.WriteLog("Welcome to Resto Druid v1.2 by Ryu");
            API.WriteLog("BETA ROTATION : Some things are still missing. Please post feedback in Druid Channel. Have not fully added defenseive  Innervate will need /break or /break2 macro to use. Cov is supported via Cooldown toggle or break marcos -- Flourish will only work for Party, not for raid currently");
           // API.WriteLog("Mouseover Support is added. Please create /cast [@mouseover] xx whereas xx is your spell and assign it the binds with MO on it in keybinds.");
            API.WriteLog("If you want to use Effloresence, you need to use an @Cursor macro, a /xxx break2 whereas xxx is your addon name(First five only) for it to work correctly, or ignore it in the spellbook. It will use it every 30 seconds.");
            API.WriteLog("Please us a /cast [target=player] macro for Innervate to work properly or it will cast on your current target");
            API.WriteLog("If you wish to use Auto Target, please set your WoW keybinds in the keybinds => Targeting for Self and party, and then match them to the Macro's's in the spell book. Enable it Toggles. You must at least have a target for it swap, friendly or enemy. It will not swap BACK to a enemy. This works ONLY for party at this time.");
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

            //Debuff
            CombatRoutine.AddDebuff(Sunfire, 164815);
            CombatRoutine.AddDebuff(Moonfire, 164812);

            //Spell
            CombatRoutine.AddSpell(Rejuvenation, 774);
            CombatRoutine.AddSpell(Regrowth, 8936);
            CombatRoutine.AddSpell(Lifebloom, 33763);
            CombatRoutine.AddSpell(WildGrowth, 48438);
            CombatRoutine.AddSpell(Swiftmend, 18562);
            CombatRoutine.AddSpell(Tranquility, 740);
            CombatRoutine.AddSpell(Moonfire, 8921);
            CombatRoutine.AddSpell(Sunfire, 93402);
            CombatRoutine.AddSpell(Wrath, 5176);
            CombatRoutine.AddSpell(Innervate, 29166);
            CombatRoutine.AddSpell(Ironbark, 102342);
            CombatRoutine.AddSpell(Natureswiftness, 132158);
            CombatRoutine.AddSpell(Barkskin, 22812);
            CombatRoutine.AddSpell(Bearform, 5487);
            CombatRoutine.AddSpell(Catform, 768);
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
            CombatRoutine.AddSpell(FrenziedRegeneration, 22842);
            CombatRoutine.AddSpell(Ironfur, 192081);
            CombatRoutine.AddSpell(HeartoftheWild, 319454);
            CombatRoutine.AddSpell(LifebloomL, 188550);
            CombatRoutine.AddSpell(Starfire, 197628);
            CombatRoutine.AddSpell(Starsurge, 197626);
            CombatRoutine.AddSpell(Efflor, 145205);

            //Toggle
            CombatRoutine.AddToggle("Auto Target");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Macro
            CombatRoutine.AddMacro(Trinket1);
            CombatRoutine.AddMacro(Trinket2);
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
            CombatRoutine.AddProp("Legendary", "Select your Legendary", LegendaryList, "Select Your Legendary", "Legendary");
            CombatRoutine.AddProp(Trinket, Trinket + " Life Percent", numbList, "Life percent at which " + "Trinkets" + " should be used, set to 0 to disable", "Healing", 55);
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", CDUsageWithAOE, "When should trinket1 be used", "Trinket", 0);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", CDUsageWithAOE, "When should trinket1 be used", "Trinket", 0);

        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted && !API.PlayerSpellonCursor && (!API.PlayerHasBuff(TravelForm) || !API.PlayerHasBuff(BearForm) || !API.PlayerHasBuff(CatForm) || !API.PlayerHasBuff(Soulshape)) && (IsOOC || API.PlayerIsInCombat))
            {
                if (API.CanCast(Efflor) && (!EfflorWatch.IsRunning || EfflorWatch.ElapsedMilliseconds >= 30000))
                {
                    API.CastSpell(Efflor);
                    EfflorWatch.Reset();
                    EfflorWatch.Start();
                    return;
                }
                if (API.CanCast(Convoke) && NightFaeCheck && InRange)
                {
                    API.CastSpell(Convoke);
                    //      API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
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
                    //       API.WriteLog("Target Spec : " + API.TargetRoleSpec);
                    //      API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    LifeBloomwatch.Reset();
                    LifeBloomwatch.Start();
                    return;
                }
                if (API.CanCast(LifebloomL) && InRange && LifeBloomLegCheck)
                {
                    API.CastSpell(LifebloomL);
                    //       API.WriteLog("Target Spec : " + API.TargetRoleSpec);
                    //      API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    LifeBloomwatch.Reset();
                    LifeBloomwatch.Start();
                    return;
                }
                if (API.CanCast(WildGrowth) && InRange && WGAoE || API.CanCast(WildGrowth) && API.PlayerHasBuff(SouloftheForest) && UnitBelowHealthPercent(65) >= 3 && InRange)
                {
                    API.CastSpell(WildGrowth);
                    //       API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    return;
                }
                if (API.CanCast(Tranquility) && InRange && TranqAoE)
                {
                    API.CastSpell(Tranquility);
                    //      API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
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
                    //      API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    //      API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    return;
                }
                if (API.CanCast(CenarionWard) && InRange && CWCheck)
                {
                    API.CastSpell(CenarionWard);
                    //     API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    //     API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    return;
                }
                if (API.CanCast(Swiftmend) && InRange && SwiftCheck)
                {
                    API.CastSpell(Swiftmend);
                    //     API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    //     API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    return;
                }
                if (API.CanCast(Rejuvenation) && InRange && RejCheck)
                {
                    API.CastSpell(Rejuvenation);
                    return;
                }
                if (API.CanCast(Regrowth) && InRange && RegrowthCheck)
                {
                    API.CastSpell(Regrowth);
                    //       API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    //       API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    return;
                }
                if (API.CanCast(Nourish) && InRange && NourishCheck)
                {
                    API.CastSpell(Nourish);
                    //  API.WriteLog("Player Health :" + API.UnitHealthPercent("player") + " Party1 Health :" + API.UnitHealthPercent("party1") + " Party2 Health :" + API.UnitHealthPercent("party2") + " Party3 Health :" + API.UnitHealthPercent("party3") + " Party4 Health :" + API.UnitHealthPercent("party4"));
                    return;
                }
                //DPS
                if (API.CanCast(HeartoftheWild) && HeartoftheWildTalent && !ChannelingCov && !ChannelingTranq && (UseHeart == "With Cooldowns" && IsCooldowns || UseHeart == "On Cooldown"))
                {
                    API.CastSpell(HeartoftheWild);
                    return;
                }
                if (API.CanCast(Starsurge) && API.PlayerHasBuff(MoonkinForm) && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && (API.PlayerHasBuff(EclispeLunar) || API.PlayerHasBuff(EclispleSolar)) && InRange && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Starsurge);
                    return;
                }
                if (API.CanCast(Starfire) && API.PlayerHasBuff(MoonkinForm) && InRange && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && API.TargetHasDebuff(Sunfire) && API.TargetHasDebuff(Moonfire) && API.TargetHealthPercent > 0 && API.PlayerHasBuff(EclispeLunar))
                {
                    API.CastSpell(Starfire);
                    return;
                }
                if (API.CanCast(Wrath) && InRange && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && API.PlayerHasBuff(EclispleSolar) && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Wrath);
                    return;
                }
                if (API.CanCast(Wrath) && InRange && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && API.TargetHasDebuff(Sunfire) && API.TargetHasDebuff(Moonfire) && API.TargetHealthPercent > 0)
                {
                    API.CastSpell(Wrath);
                    return;
                }
                if (API.CanCast(Starfire) && API.PlayerHasBuff(MoonkinForm) && InRange && API.PlayerCanAttackTarget && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving && API.TargetHasDebuff(Sunfire) && API.TargetHasDebuff(Moonfire) && API.TargetHealthPercent > 0 && API.TargetUnitInRangeCount >= 3)
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
                    if (API.PlayerIsInGroup)
                    {
                        for (int i = 0; i < units.Length; i++)
                        {
                            if (API.UnitHealthPercent(units[i]) <= RegrowthLifePercent && (PlayerHealth >= RegrowthLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= IronBarkLifePercent && (PlayerHealth >= IronBarkLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= SwiftmendLifePercent && (PlayerHealth >= SwiftmendLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(units[i]) <= RejLifePercent && (PlayerHealth >= RejLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                            {
                                API.CastSpell(PlayerTargetArray[i]);
                                return;
                            }
                        }
                    }
                    if (API.PlayerIsInRaid)
                    {
                        for (int i = 0; i < raidunits.Length; i++)
                        {
                            if (API.UnitHealthPercent(raidunits[i]) <= RegrowthLifePercent && (PlayerHealth >= RegrowthLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= IronBarkLifePercent && (PlayerHealth >= IronBarkLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= SwiftmendLifePercent && (PlayerHealth >= SwiftmendLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= RejLifePercent && (PlayerHealth >= RejLifePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
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



