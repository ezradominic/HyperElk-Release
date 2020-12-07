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
        private string ConvoketheSpririts = "Convoke the Spirits";
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
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40};

        private static readonly Stopwatch player = new Stopwatch();
        private static readonly Stopwatch party1 = new Stopwatch();
        private static readonly Stopwatch party2 = new Stopwatch();
        private static readonly Stopwatch party3 = new Stopwatch();
        private static readonly Stopwatch party4 = new Stopwatch();
        private static readonly Stopwatch LifeBloomwatch = new Stopwatch();

        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);

        private int FlourishRaidTracking(string buff) => raidunits.Count(p => API.UnitHasBuff(p,buff));
        private int FlourishPartyTracking(string buff) => units.Count(p => API.UnitHasBuff(p,buff));
        private int FlourishTrackingRej(string buff) => API.PlayerIsInRaid ? FlourishRaidTracking(Rejuvenation) : FlourishPartyTracking(Rejuvenation);

        bool ChannelingCov => API.CurrentCastSpellID("player") == 323764;
        bool ChannelingTranq => API.CurrentCastSpellID("player") == 740;

        private bool WGAoE => UnitBelowHealthPercent(WGLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool ToLAoE => UnitBelowHealthPercent(ToLLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool TranqAoE => UnitBelowHealthPercent(TranqLifePercent) >= AoENumber && !API.PlayerCanAttackTarget;
        private bool FloruishTracking => FlourishTrackingRej(Rejuvenation) >= AoENumber;
        private bool FloruishAoE => UnitBelowHealthPercent(FloruishLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool CWCheck => CenarionWardTalent && API.TargetHealthPercent <= CWLifePercent && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool IBCheck => API.TargetHealthPercent <= IronBarkLifePercent && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool NourishCheck => NourishTalent && API.TargetHealthPercent <= NourishLifePercent && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving;
        private bool RegrowthCheck => (API.PlayerHasBuff(Clear) && API.TargetHealthPercent <= 90 ||API.TargetHealthPercent <= RegrowthLifePercent) && !API.PlayerCanAttackTarget && NotChanneling && !ChannelingCov && !ChannelingTranq && !API.PlayerIsMoving;
        private bool RejCheck => API.TargetHealthPercent <= RejLifePercent && !API.PlayerCanAttackTarget && (!API.TargetHasBuff(Rejuvenation) || API.TargetHasBuff(Rejuvenation) && GerminationTalent && !API.TargetHasBuff(GerminationHoT)) && !ChannelingCov && !ChannelingTranq && NotChanneling && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool SwiftCheck => API.TargetHealthPercent <= SwiftmendLifePercent && !API.PlayerCanAttackTarget && API.SpellCharges(Swiftmend) > 0 && (API.TargetHasBuff(Rejuvenation) || API.TargetHasBuff(Regrowth) || API.TargetHasBuff(WildGrowth)) && (!API.PlayerIsMoving || API.PlayerIsMoving);
        // (PhotosynthesisTalent && API.TargetRoleSpec == 998 || API.TargetRoleSpec == 999) &&
        private bool LifeBloomCheck => API.TargetHealthPercent <= LifebloomLifePercent && !API.PlayerCanAttackTarget && !API.TargetHasBuff(Lifebloom) && (!LifeBloomwatch.IsRunning || LifeBloomwatch.ElapsedMilliseconds >= 15000) && NotChanneling && !ChannelingCov && !ChannelingTranq && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool FloruishCheck => FloruishTracking && !API.PlayerCanAttackTarget && FloruishAoE && FlourishTalent;
        private bool KyrianCheck => PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && !API.PlayerIsMoving && !ChannelingTranq;
        private bool NightFaeCheck => PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !ChannelingCov && !ChannelingTranq;
        private bool NecrolordCheck => PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingCov && !ChannelingTranq;
        private bool VenthyrCheck => PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingCov && !ChannelingTranq;
        private bool WGParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= WGLifePercent && units.Count(p => API.UnitHealthPercent(p) <= WGLifePercent) >= 3;
        }

        private bool WGParty1 => WGParty(1);
        private bool WGRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= WGLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= WGLifePercent) >= 3;
        }

        private bool WGRaid1 => WGRaid(1);

        private bool FloruishParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= FloruishLifePercent && units.Count(p => API.UnitHealthPercent(p) <= FloruishLifePercent) >= 3;
        }

        private bool FloruishParty1 => FloruishParty(1);
        private bool FloruishRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= FloruishLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= FloruishLifePercent) >= 3;
        }

        private bool FloruishRaid1 => FloruishRaid(1);

        private bool ToLParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= ToLLifePercent && units.Count(p => API.UnitHealthPercent(p) <= ToLLifePercent) >= 3;
        }

        private bool ToLParty1 => ToLParty(1);
        private bool ToLRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= ToLLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= ToLLifePercent) >= 3;
        }

        private bool ToLRaid1 => ToLRaid(1);

        private bool TranqParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= TranqLifePercent && units.Count(p => API.UnitHealthPercent(p) <= TranqLifePercent) >= 3;
        }

        private bool TranqParty1 => TranqParty(1);
        private bool TranqRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= TranqLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= TranqLifePercent) >= 3;
        }

        private bool TranqRaid1 => TranqRaid(1);


        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool IsAutoSwap => CombatRoutine.GetPropertyBool("Auto Swap");
        private bool IsOOC => CombatRoutine.GetPropertyBool("OOC");
        private int BarkskinLifePercent => numbList[CombatRoutine.GetPropertyInt(Barkskin)];
        private int RenewalLifePercent => numbList[CombatRoutine.GetPropertyInt(Renewal)];
        private int RejLifePercent => numbList[CombatRoutine.GetPropertyInt(Rejuvenation)];
        private int RegrowthLifePercent => numbList[CombatRoutine.GetPropertyInt(Regrowth)];
        private int LifebloomLifePercent => numbList[CombatRoutine.GetPropertyInt(Lifebloom)];
        private int CWLifePercent => numbList[CombatRoutine.GetPropertyInt(CenarionWard)];
        private int IronBarkLifePercent => numbList[CombatRoutine.GetPropertyInt(Ironbark)];
        private int NourishLifePercent => numbList[CombatRoutine.GetPropertyInt(Nourish)];
        private int SwiftmendLifePercent => numbList[CombatRoutine.GetPropertyInt(Swiftmend)];
        private int FloruishLifePercent => numbList[CombatRoutine.GetPropertyInt(Flourish)];
        private int OvergrowthLifePercent => numbList[CombatRoutine.GetPropertyInt(Overgrowth)];
        private int WGLifePercent => numbList[CombatRoutine.GetPropertyInt(WildGrowth)];
        private int TranqLifePercent => numbList[CombatRoutine.GetPropertyInt(Tranquility)];
        private int ToLLifePercent => numbList[CombatRoutine.GetPropertyInt(TreeofLife)];
        private int NSLifePercent => numbList[CombatRoutine.GetPropertyInt(Natureswiftness)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        //private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoER)];

        //General

        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private bool IsMelee => API.TargetRange < 6;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");




        //  public bool isInterrupt => CombatRoutine.GetPropertyBool("KICK") && API.TargetCanInterrupted && API.TargetIsCasting && (API.TargetIsChanneling ? API.TargetElapsedCastTime >= interruptDelay : API.TargetCurrentCastTimeRemaining <= interruptDelay);
        //  public int interruptDelay => random.Next((int)(CombatRoutine.GetPropertyInt("KICKTime") * 0.9), (int)(CombatRoutine.GetPropertyInt("KICKTime") * 1.1));

        public override void Initialize()
        {
            CombatRoutine.Name = "Resto Druid by Ryu";
            API.WriteLog("Welcome to Resto Druid by Ryu");
            API.WriteLog("BETA ROTATION : Some things are still missing. Please post feedback in Druid Channel. Have not fully added defenseive  Innervate will need /break or /break2 macro to use. Cov is supported via Cooldown toggle or break marcos");
           // API.WriteLog("Mouseover Support is added. Please create /cast [@mouseover] xx whereas xx is your spell and assign it the binds with MO on it in keybinds.");
            API.WriteLog("If you want to use Effloresence, you need to use an @Cursor macro with /xxx break or break2 whereas xxx is your addon name(First five only) for it to work");
            API.WriteLog("Lifebloom will cast every 15 seconds on whichever target you are currently targeting.");
            API.WriteLog("Target Spec" + API.TargetRoleSpec);

            //Buff
            CombatRoutine.AddBuff(Rejuvenation);
            CombatRoutine.AddBuff(Regrowth);
            CombatRoutine.AddBuff(Lifebloom);
            CombatRoutine.AddBuff(WildGrowth);
            CombatRoutine.AddBuff(GerminationHoT);
            CombatRoutine.AddBuff(Clear);


            //Debuff
            CombatRoutine.AddDebuff(Sunfire);
            CombatRoutine.AddDebuff(Moonfire);

            //Spell
            CombatRoutine.AddSpell(Rejuvenation);
            CombatRoutine.AddSpell(Regrowth);
            CombatRoutine.AddSpell(Lifebloom);
            CombatRoutine.AddSpell(WildGrowth);
            CombatRoutine.AddSpell(Swiftmend);
            CombatRoutine.AddSpell(Tranquility);
            CombatRoutine.AddSpell(Moonfire);
            CombatRoutine.AddSpell(Sunfire);
            CombatRoutine.AddSpell(Wrath);
            CombatRoutine.AddSpell(Innervate);
            CombatRoutine.AddSpell(Ironbark);
            CombatRoutine.AddSpell(Natureswiftness);
            CombatRoutine.AddSpell(Barkskin);
            CombatRoutine.AddSpell(Bearform);
            CombatRoutine.AddSpell(Catform);
            CombatRoutine.AddSpell(NaturesCure);
            CombatRoutine.AddSpell(EntanglingRoots);
            CombatRoutine.AddSpell(Soothe);
            CombatRoutine.AddSpell(KindredSprirts);
            CombatRoutine.AddSpell(AdaptiveSwarm);
            CombatRoutine.AddSpell(Fleshcraft);
            CombatRoutine.AddSpell(ConvoketheSpririts);
            CombatRoutine.AddSpell(RavenousFrenzy);
            CombatRoutine.AddSpell(Nourish);
            CombatRoutine.AddSpell(CenarionWard);
            CombatRoutine.AddSpell(TreeofLife);
            CombatRoutine.AddSpell(Overgrowth);
            CombatRoutine.AddSpell(Flourish);
            CombatRoutine.AddSpell(Renewal);

            //Macro


            //Prop
           // CombatRoutine.AddProp("Auto Swap", "Auto Swap", false, "Use Auto Swap Logic", "Generic");
            CombatRoutine.AddProp(Barkskin, Barkskin + " Life Percent", numbList, "Life percent at which" + Barkskin + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", numbList, "Life percent at which" + Renewal + "is used, if talented, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 1);

            CombatRoutine.AddProp("OOC", "Healing out of Combat", true, "Heal out of combat", "Healing");
            CombatRoutine.AddProp(Rejuvenation, Rejuvenation + " Life Percent", numbList, "Life percent at which " + Rejuvenation + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(Regrowth, Regrowth + " Life Percent", numbList, "Life percent at which " + Regrowth + " is used, set to 0 to disable", "Healing", 11);
            CombatRoutine.AddProp(Lifebloom, Lifebloom + " Life Percent", numbList, "Life percent at which " + Lifebloom + " is used, set to 0 to disable", "Healing", 12);
            CombatRoutine.AddProp(CenarionWard, CenarionWard + " Life Percent", numbList, "Life percent at which " + CenarionWard + " is used, if talented, set to 0 to disable", "Healing", 13);
            CombatRoutine.AddProp(Ironbark, Ironbark + " Life Percent", numbList, "Life percent at which " + Ironbark + " is used, set to 0 to disable", "Healing", 14);
            CombatRoutine.AddProp(Nourish, Nourish + " Life Percent", numbList, "Life percent at which " + Nourish + " is used, set to 0 to disable", "Healing", 15);
            CombatRoutine.AddProp(Swiftmend, Swiftmend + " Life Percent", numbList, "Life percent at which " + Swiftmend + " is used, set to 0 to disable", "Healing", 16);
            CombatRoutine.AddProp(Natureswiftness, Natureswiftness + " Life Percent", numbList, "Life percent at which " + Natureswiftness + " is used, set to 0 to disable", "Healing", 16);
            CombatRoutine.AddProp(Overgrowth, Overgrowth + " Life Percent", numbList, "Life percent at which " + Overgrowth + " is used, set to 0 to disable", "Healing", 17);
            CombatRoutine.AddProp(Flourish, Flourish + " Life Percent", numbList, "Life percent at which " + Flourish + " is used when AoE Number of members are at and have the HoTs needed, set to 0 to disable", "Healing", 18);
            CombatRoutine.AddProp(WildGrowth, WildGrowth + " Life Percent", numbList, "Life percent at which " + WildGrowth + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 19);
            CombatRoutine.AddProp(Tranquility, Tranquility + " Life Percent", numbList, "Life percent at which " + Tranquility + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 20);
            CombatRoutine.AddProp(TreeofLife, TreeofLife + " Life Percent", numbList, "Life percent at which " + TreeofLife + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 19);
            CombatRoutine.AddProp(AoE, "Number of units for AoE Healing ", numbPartyList, " Units for AoE Healing", "Healing", 3);


        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted && (IsOOC || API.PlayerIsInCombat))
            {
                if (API.CanCast(KindredSprirts) && KyrianCheck)
                {
                    API.CastSpell(KindredSprirts);
                    return;
                }
                if (API.CanCast(ConvoketheSpririts) && NightFaeCheck)
                {
                    API.CastSpell(ConvoketheSpririts);
                    return;
                }
                if (API.CanCast(AdaptiveSwarm) && NecrolordCheck)
                {
                    API.CastSpell(AdaptiveSwarm);
                    return;
                }
                if (API.CanCast(RavenousFrenzy) && VenthyrCheck)
                {
                    API.CastSpell(RavenousFrenzy);
                    return;
                }
                if (API.CanCast(TreeofLife) && TreeofLifeTalent && InRange && ToLAoE)
                {
                    API.CastSpell(TreeofLife);;
                    return;
                }
                if (API.CanCast(Flourish) && InRange && FloruishCheck)
                {
                    API.CastSpell(Flourish);
                    return;
                }
                if (API.CanCast(Lifebloom) && InRange && LifeBloomCheck)
                {
                    API.CastSpell(Lifebloom);
                    API.WriteLog("Target Spec : " + API.TargetRoleSpec);
                    LifeBloomwatch.Reset();
                    LifeBloomwatch.Start();
                    return;
                }
                if (API.CanCast(WildGrowth) && InRange && WGAoE)
                {
                    API.CastSpell(WildGrowth);
                    return;
                }
                if (API.CanCast(Tranquility) && InRange && TranqAoE)
                {
                    API.CastSpell(Tranquility);
                    return;
                }
                if (API.CanCast(Ironbark) && InRange && IBCheck)
                {
                    API.CastSpell(Ironbark);
                    API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    return;
                }
                if (API.CanCast(CenarionWard) && InRange && CWCheck)
                {
                    API.CastSpell(CenarionWard);
                    API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    return;
                }
                if (API.CanCast(Nourish) && InRange && NourishCheck)
                {
                    API.CastSpell(Nourish);
                    API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    return;
                }
                if (API.CanCast(Swiftmend) && InRange && SwiftCheck)
                {
                    API.CastSpell(Swiftmend);
                    API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    return;
                }
                if (API.CanCast(Regrowth) && InRange && RegrowthCheck)
                {
                    API.CastSpell(Regrowth);
                    API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    API.WriteLog("Target Spec :  " + API.TargetRoleSpec);
                    return;
                }
                if (API.CanCast(Rejuvenation) && InRange && RejCheck)
                {
                    API.CastSpell(Rejuvenation);
                    API.WriteLog("Target Health % " + API.TargetHealthPercent);
                    API.WriteLog("Target Spec : " + API.TargetRoleSpec);
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotChanneling && !ChannelingTranq && ChannelingCov && !API.PlayerIsMoving)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.CanCast(Moonfire) && InRange && !API.TargetHasDebuff(Moonfire) && API.PlayerCanAttackTarget && (API.PlayerIsMoving || !API.PlayerIsMoving) && NotChanneling && !ChannelingTranq && ChannelingCov) 
            {
                API.CastSpell(Moonfire);
                return;
            }
            if (API.CanCast(Sunfire) && InRange && !API.TargetHasDebuff(Sunfire) && API.PlayerCanAttackTarget && NotChanneling && (API.PlayerIsMoving || !API.PlayerIsMoving) && !ChannelingTranq && ChannelingCov)
            {
                API.CastSpell(Sunfire);
                return;
            }
        }

        public override void OutOfCombatPulse()
        {

        }

    }
}



