// Changelog
// v0.5 Beta
// v1.0 first release


using System.Diagnostics;
using System.Linq;


namespace HyperElk.Core
{
    public class ShadowPriest : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsForceAOE => API.ToggleIsEnabled("Force AoE");
        //Spell,Auras
        private string Shadowform = "Shadowform";
        private string PWFortitude = "Power Word: Fortitude";
        private string Voidform = "Voidform";
        private string AscendedBlast = "Ascended Blast";
        private string PWShield = "Power Word: Shield";
        private string DevouringPlague = "Devouring Plague";
        private string SWPain = "Shadow Word: Pain";
        private string WeakenedSoul = "Weakened Soul";
        private string VampiricTouch = "Vampiric Touch";
        private string DarkThoughts = "Dark Thoughts";
        private string UnfurlingDarkness = "Unfurling Darkness";
        private string SurrendertoMadness = "Surrender to Madness";
        private string Damnation = "Damnation";
        private string Silence = "Silence";
        private string Mindbender = "Mindbender";
        private string PowerInfusion = "Power Infusion";
        private string MindFlay = "Mind Flay";
        private string MindBlast = "Mind Blast";
        private string ShadowMend = "Shadow Mend";
        private string SWDeath = "Shadow Word: Death";
        private string Shadowfiend = "Shadowfiend";
        private string VoidEruption = "Void Eruption";
        private string VoidBolt = "Void Bolt";
        private string MindSear = "Mind Sear";
        private string SearingNightmare = "Searing Nightmare";
        private string ShadowCrash = "Shadow Crash";
        private string VoidTorrent = "Void Torrent";
        private string VampiricEmbrace = "Vampiric Embrace";
        private string DesperatePrayer = "Desperate Prayer";
        private string Mindgames = "Mindgames";
        private string BoonOfTheAscended = "Boon of the Ascended";
        private string DissonantEchoes = "Dissonant Echoes";
        private string UnholyNova = "Unholy Nova";
        private string FaeGuardians = "Fae Guardians";
        private string Stopcast = "Stopcast";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string Dispersion = "Dispersion";
        private string Quake = "Quake";
        private string HungeringVoid = "Hungering Void";
        private string WrathfulFaerie = "Wrathful Faerie";
        private string AoE = "AOE";
        private string AoERaid = "AoERaid";

        //Talents
        bool TalentBodyandSoul => API.PlayerIsTalentSelected(2, 1);
        bool TalentTwistofFate => API.PlayerIsTalentSelected(3, 1);
        bool TalentMisery => API.PlayerIsTalentSelected(3, 2);
        bool TalentSearingNightmare => API.PlayerIsTalentSelected(3, 3);
        bool TalentAuspciousSpirits => API.PlayerIsTalentSelected(5, 1);
        bool TalentPsychicLink => API.PlayerIsTalentSelected(5, 2);
        bool TalentShadowCrash => API.PlayerIsTalentSelected(5, 3);
        bool TalentDamnation => API.PlayerIsTalentSelected(6, 1);
        bool TalentMindbender => API.PlayerIsTalentSelected(6, 2);
        bool TalentVoidTorrent => API.PlayerIsTalentSelected(6, 3);
        bool TalentHungeringVoid => API.PlayerIsTalentSelected(7, 2);
        bool TalentSurrendertoMadness => API.PlayerIsTalentSelected(7, 3);


        //General
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool PlayerHasDebuff(string buff)
        {
            return API.PlayerHasDebuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        bool ChannelingMindFlay => API.CurrentCastSpellID("player") == 15407;
        bool ChannelingMindSear => API.CurrentCastSpellID("player") == 48045;
        bool CastingVT => API.CurrentCastSpellID("player") == 34914;
        //actions+=/variable,name=dots_up,op=set,value=dot.shadow_word_pain.ticking&dot.vampiric_touch.ticking
        bool dots_up => TargetHasDebuff(SWPain) && TargetHasDebuff(VampiricTouch);
        //actions+=/variable,name=all_dots_up,op=set,value=dot.shadow_word_pain.ticking&dot.vampiric_touch.ticking&dot.devouring_plague.ticking
        bool all_dots_up => TargetHasDebuff(SWPain) && TargetHasDebuff(VampiricTouch) && TargetHasDebuff(DevouringPlague);
        //actions+=/variable,name=searing_nightmare_cutoff,op=set,value=spell_targets.mind_sear>2+buff.voidform.up
        bool searing_nightmare_cutoff => API.TargetUnitInRangeCount > 2 + (API.PlayerHasBuff(Voidform) ? 1 : 0);
        private float gcd => API.SpellGCDTotalDuration;
        private int PlayerLevel => API.PlayerLevel;
        private bool IsInRange => API.TargetRange < 41;
        private bool isMOinRange => API.MouseoverRange < 41;
        private bool IsInKickRange => API.TargetRange < 31;
        private bool isExplosive => API.TargetMaxHealth <= 600 && API.TargetMaxHealth != 0 && PlayerLevel == 60;
        bool IsVoidEruption => (UseVoidEruption == "with Cooldowns" || UseVoidEruption == "with Cooldowns or AoE" || UseVoidEruption == "on mobcount or Cooldowns") && IsCooldowns || UseVoidEruption == "always" || (UseVoidEruption == "on AOE" || UseVoidEruption == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseVoidEruption == "on mobcount or Cooldowns" || UseVoidEruption == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsMindbender => (UseMindbender == "with Cooldowns" || UseMindbender == "with Cooldowns or AoE" || UseMindbender == "on mobcount or Cooldowns") && IsCooldowns || UseMindbender == "always" || (UseMindbender == "on AOE" || UseMindbender == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseMindbender == "on mobcount or Cooldowns" || UseMindbender == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsShadowfiend => (UseShadowfiend == "with Cooldowns" || UseShadowfiend == "with Cooldowns or AoE" || UseShadowfiend == "on mobcount or Cooldowns") && IsCooldowns || UseShadowfiend == "always" || (UseShadowfiend == "on AOE" || UseShadowfiend == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseShadowfiend == "on mobcount or Cooldowns" || UseShadowfiend == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsDamnation => (UseDamnation == "with Cooldowns" || UseDamnation == "with Cooldowns or AoE" || UseDamnation == "on mobcount or Cooldowns") && IsCooldowns || UseDamnation == "always" || (UseDamnation == "on AOE" || UseDamnation == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseDamnation == "on mobcount or Cooldowns" || UseDamnation == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsPowerInfusion => (UsePowerInfusion == "with Cooldowns" || UsePowerInfusion == "with Cooldowns or AoE" || UsePowerInfusion == "on mobcount or Cooldowns") && IsCooldowns || UsePowerInfusion == "always" || (UsePowerInfusion == "on AOE" || UsePowerInfusion == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UsePowerInfusion == "on mobcount or Cooldowns" || UsePowerInfusion == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;

        bool IsCovenant => (UseCovenant == "with Cooldowns" || UseCovenant == "with Cooldowns or AoE" || UseCovenant == "on mobcount or Cooldowns") && IsCooldowns || UseCovenant == "always" || (UseCovenant == "on AOE" || UseCovenant == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseCovenant == "on mobcount or Cooldowns" || UseCovenant == "on mobcount") && API.TargetUnitInRangeCount >= MobCount;
        bool IsTrinkets1 => ((UseTrinket1 == "with Cooldowns" || UseTrinket1 == "with Cooldowns or AoE" || UseTrinket1 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket1 == "always" || (UseTrinket1 == "on AOE" || UseTrinket1 == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseTrinket1 == "on mobcount or Cooldowns" || UseTrinket1 == "on mobcount") && API.TargetUnitInRangeCount >= MobCount) && IsInRange;
        bool IsTrinkets2 => ((UseTrinket2 == "with Cooldowns" || UseTrinket2 == "with Cooldowns or AoE" || UseTrinket2 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket2 == "always" || (UseTrinket2 == "on AOE" || UseTrinket2 == "with Cooldowns or AoE") && API.TargetUnitInRangeCount >= AOEUnitNumber || (UseTrinket2 == "on mobcount or Cooldowns" || UseTrinket2 == "on mobcount") && API.TargetUnitInRangeCount >= MobCount) && IsInRange;

        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "with Cooldowns or AoE", "on mobcount", "on mobcount or Cooldowns", "always" };
        public string[] Legendary = new string[] { "No Legendary", "Shadowflame Prism" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);

        private bool VampiricEmbraceAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(VampiricEmbraceLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(VampiricEmbraceLifePercent) >= AoENumber;
        private int VampiricEmbraceLifePercent => numbList[CombatRoutine.GetPropertyInt(VampiricEmbrace)];
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoERaid)];
        private int MobCount => numbRaidList[CombatRoutine.GetPropertyInt("MobCount")];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseVoidEruption => CDUsageWithAOE[CombatRoutine.GetPropertyInt(VoidEruption)];
        private string UseShadowfiend => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Shadowfiend)];
        private string UseMindbender => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Mindbender)];
        private string UseDamnation => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Damnation)];
        private string UsePowerInfusion => CDUsageWithAOE[CombatRoutine.GetPropertyInt(PowerInfusion)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool PwShieldMove => CombatRoutine.GetPropertyBool("PwShieldMove");
        private bool QuakingHelper => CombatRoutine.GetPropertyBool("QuakingHelper");
        private int ShadowMendLifePercent => numbList[CombatRoutine.GetPropertyInt(ShadowMend)];
        private int ShadowMendLifePercentOOC => numbList[CombatRoutine.GetPropertyInt("ShadowMendOOC")];
        private int DesperatePrayerLifePercent => numbList[CombatRoutine.GetPropertyInt(DesperatePrayer)];
        private int DispersionLifePercent => numbList[CombatRoutine.GetPropertyInt(Dispersion)];
        private int PWShieldLifePercent => numbList[CombatRoutine.GetPropertyInt(PWShield)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private string IsLegendary => Legendary[CombatRoutine.GetPropertyInt("IsLegendary")];
        private bool Quaking => ((API.PlayerCurrentCastTimeRemaining >= 200 || API.PlayerIsChanneling) && API.PlayerDebuffRemainingTime(Quake) < 200) && PlayerHasDebuff(Quake);
        private bool SaveQuake => (PlayerHasDebuff(Quake) && API.PlayerDebuffRemainingTime(Quake) > 200 && QuakingHelper || !PlayerHasDebuff(Quake) || !QuakingHelper);
        public override void Initialize()
        {
            CombatRoutine.Name = "Shadow Priest by smartie";
            API.WriteLog("Welcome to smartie`s Shadow Priest v1.0");
            API.WriteLog("For this rota you need to following macros");
            API.WriteLog("For stopcasting (which is important): /stopcasting");
            API.WriteLog("Shadow Word: PainMO - /cast [@mouseover] Shadow Word: Pain");
            API.WriteLog("VampiricTouchMO - /cast [@mouseover] Vampiric Touch");

            //Spells
            CombatRoutine.AddSpell(MindFlay, 15407);
            CombatRoutine.AddSpell(SWPain, 589);
            CombatRoutine.AddSpell(AscendedBlast, 325315);
            CombatRoutine.AddSpell(MindBlast, 8092);
            CombatRoutine.AddSpell(ShadowMend, 186263);
            CombatRoutine.AddSpell(DevouringPlague, 335467);
            CombatRoutine.AddSpell(Shadowform, 232698);
            CombatRoutine.AddSpell(SWDeath, 32379);
            CombatRoutine.AddSpell(VampiricTouch, 34914);
            CombatRoutine.AddSpell(Shadowfiend, 34433);
            CombatRoutine.AddSpell(Mindbender, 123040);
            CombatRoutine.AddSpell(VoidEruption, 228260);
            CombatRoutine.AddSpell(VoidBolt, 205448);
            CombatRoutine.AddSpell(MindSear, 48045);
            CombatRoutine.AddSpell(SearingNightmare, 341385);
            CombatRoutine.AddSpell(ShadowCrash, 205385);
            CombatRoutine.AddSpell(VoidTorrent, 263165);
            CombatRoutine.AddSpell(SurrendertoMadness, 193223);
            CombatRoutine.AddSpell(Damnation, 341374);
            CombatRoutine.AddSpell(Silence, 15487);
            CombatRoutine.AddSpell(PowerInfusion, 10060);
            CombatRoutine.AddSpell(Dispersion, 47585);
            CombatRoutine.AddSpell(VampiricEmbrace, 15286);
            CombatRoutine.AddSpell(DesperatePrayer, 19236);
            CombatRoutine.AddSpell(PWFortitude, 21562);
            CombatRoutine.AddSpell(PWShield, 17);
            CombatRoutine.AddSpell(BoonOfTheAscended, 325013);
            CombatRoutine.AddSpell(Mindgames, 323673);
            CombatRoutine.AddSpell(UnholyNova, 324724);
            CombatRoutine.AddSpell(FaeGuardians, 327661);


            //Macros
            CombatRoutine.AddMacro(SWPain + "MO");
            CombatRoutine.AddMacro(VampiricTouch + "MO");
            CombatRoutine.AddMacro("Trinket1");
            CombatRoutine.AddMacro("Trinket2");
            CombatRoutine.AddMacro(Stopcast);

            //Buffs
            CombatRoutine.AddBuff(Shadowform, 232698);
            CombatRoutine.AddBuff(PWFortitude, 21562);
            CombatRoutine.AddBuff(Voidform, 194249);
            CombatRoutine.AddBuff(PWShield, 17);
            CombatRoutine.AddBuff(DarkThoughts, 341205);
            CombatRoutine.AddBuff(UnfurlingDarkness, 341273);
            CombatRoutine.AddBuff(BoonOfTheAscended, 325013);
            CombatRoutine.AddBuff(PowerInfusion, 10060);
            CombatRoutine.AddBuff(DissonantEchoes, 343144);
            CombatRoutine.AddBuff(FaeGuardians, 327661);

            //Debuff
            CombatRoutine.AddDebuff(DevouringPlague, 335467);
            CombatRoutine.AddDebuff(SWPain, 589);
            CombatRoutine.AddDebuff(WeakenedSoul, 6788);
            CombatRoutine.AddDebuff(VampiricTouch, 34914);
            CombatRoutine.AddDebuff(ShadowCrash, 205385);
            CombatRoutine.AddDebuff(Quake, 240447);
            CombatRoutine.AddDebuff(HungeringVoid, 345219);
            CombatRoutine.AddDebuff(WrathfulFaerie, 342132);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Force AoE");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);


            //Prop
            CombatRoutine.AddProp("MobCount", "Mobcount to use Cooldowns ", numbRaidList, " Mobcount to use Cooldowns", "Cooldowns", 3);
            CombatRoutine.AddProp("MouseoverInCombat", "Only Mouseover in combat", false, " Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(VoidEruption, "Use " + VoidEruption, CDUsageWithAOE, "Use " + VoidEruption + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Shadowfiend, "Use " + Shadowfiend, CDUsageWithAOE, "Use " + Shadowfiend + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Mindbender, "Use " + Mindbender, CDUsageWithAOE, "Use " + Mindbender + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Damnation, "Use " + Damnation, CDUsageWithAOE, "Use " + Damnation + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(PowerInfusion, "Use " + PowerInfusion, CDUsageWithAOE, "Use " + PowerInfusion + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("PwShieldMove", "PW Shield while moving", true, "Will use" + PWShield + " while moving when Talented into Body and Sould", "Generic");
            CombatRoutine.AddProp("QuakingHelper", "Quaking Helper", false, "Will cancle casts on Quaking", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(ShadowMend, ShadowMend + " Life Percent", numbList, "Life percent at which" + ShadowMend + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(DesperatePrayer, DesperatePrayer + " Life Percent", numbList, "Life percent at which" + DesperatePrayer + "is used, set to 0 to disable", "Defense", 20);
            CombatRoutine.AddProp(Dispersion, Dispersion + " Life Percent", numbList, "Life percent at which" + Dispersion + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(PWShield, PWShield + " Life Percent", numbList, "Life percent at which" + PWShield + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp("ShadowMendOOC", "Shadow Mend out of Combat" + " Life Percent", numbList, "Life percent at which" + ShadowMend + "is used out of combat - set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp("IsLegendary", "Choose your Legendary", Legendary, "Please choose your Legendary", "Generic");
            CombatRoutine.AddProp(AoE, "Vampiric Embrace Party Units ", numbPartyList, " in Party", "VampiricEmbrace", 3);
            CombatRoutine.AddProp(AoERaid, "Vampiric Embrace Raid Units ", numbRaidList, "  in Raid", "VampiricEmbrace", 5);
            CombatRoutine.AddProp(VampiricEmbrace, VampiricEmbrace + " Life Percent", numbList, "Life percent at which" + VampiricEmbrace + " is used, set to 0 to disable", "VampiricEmbrace", 50);
        }
        public override void Pulse()
        {
            //API.WriteLog("Pet: " + API.PlayerTotemPetDuration);
        }
        public override void CombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 && !API.MacroIsIgnored(Stopcast) && QuakingHelper && Quaking)
            {
                API.CastSpell(Stopcast);
                return;
            }
            if ((API.PlayerCurrentCastTimeRemaining > 40 && !ChannelingMindFlay && !ChannelingMindSear && !CastingVT || CastingVT && API.PlayerCurrentCastTimeRemaining > 0) || API.PlayerSpellonCursor)
                return;
            if (!API.PlayerIsMounted)
            {
                //Kick + Def cds
                if (isInterrupt && API.CanCast(Silence) && PlayerLevel >= 12 && IsInKickRange)
                {
                    API.CastSpell(Silence);
                    return;
                }
                if (PlayerRaceSettings == "Tauren"  && API.CanCast(RacialSpell1) && isInterrupt && !API.PlayerIsMoving && isRacial && API.TargetRange < 8 && API.SpellISOnCooldown(Silence))
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.CanCast(ShadowMend) && SaveQuake && !API.PlayerIsMoving && API.PlayerHealthPercent <= ShadowMendLifePercent)
                {
                    API.CastSpell(ShadowMend);
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
                if (API.CanCast(DesperatePrayer) && API.PlayerHealthPercent <= DesperatePrayerLifePercent)
                {
                    API.CastSpell(DesperatePrayer);
                    return;
                }
                if (API.CanCast(Dispersion) && API.PlayerHealthPercent <= DispersionLifePercent)
                {
                    API.CastSpell(Dispersion);
                    return;
                }
                if (API.CanCast(PWShield) && API.PlayerHealthPercent <= PWShieldLifePercent && !PlayerHasDebuff(WeakenedSoul))
                {
                    API.CastSpell(PWShield);
                    return;
                }
                if (API.CanCast(VampiricEmbrace) && VampiricEmbraceAoE)
                {
                    API.CastSpell(VampiricEmbrace);
                    return;
                }
                //Rotation
                rotation();
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerIsCasting(true) && !ChannelingMindFlay && !ChannelingMindSear)
                return;
            //OOC Stuff
            if (!API.PlayerIsMounted)
            {
                if (API.CanCast(Shadowform) && !API.PlayerHasBuff(Shadowform) && !API.PlayerHasBuff(Voidform))
                {
                    API.CastSpell(Shadowform);
                    return;
                }
                if (API.CanCast(PWFortitude)&& API.PlayerBuffTimeRemaining(PWFortitude) < 30000)
                {
                    API.CastSpell(PWFortitude);
                    return;
                }
                if (API.CanCast(PWShield) && PwShieldMove && TalentBodyandSoul && API.PlayerIsMoving && !PlayerHasDebuff(WeakenedSoul))
                {
                    API.CastSpell(PWShield);
                    return;
                }
                if (API.CanCast(ShadowMend) && SaveQuake && API.PlayerHealthPercent <= ShadowMendLifePercentOOC && !API.PlayerIsMoving)
                {
                    API.CastSpell(ShadowMend);
                    return;
                }
            }

        }
        private void rotation()
        {
            if (isExplosive)
            {
                if (API.CanCast(SWPain) && IsInRange)
                {
                    API.CastSpell(SWPain);
                    API.WriteLog("Explosive killer");
                    return;
                }
            }
            if (!isExplosive)
            {
                if (API.CanCast(Shadowform) && !PlayerHasBuff(Shadowform) && !PlayerHasBuff(Voidform))
                {
                    API.CastSpell(Shadowform);
                    return;
                }
                if (API.CanCast(PWFortitude) && API.PlayerBuffTimeRemaining(PWFortitude) < 30000)
                {
                    API.CastSpell(PWFortitude);
                    return;
                }
                if (API.CanCast(PWShield) && PwShieldMove && TalentBodyandSoul && API.PlayerIsMoving && !PlayerHasDebuff(WeakenedSoul))
                {
                    API.CastSpell(PWShield);
                    return;
                }
                if (IsInRange)
                {
                    //actions+=/fireblood,if=buff.voidform.up
                    if (PlayerRaceSettings == "Dark Iron Dwarf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && PlayerHasBuff(Voidform))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions+=/berserking,if=buff.voidform.up
                    if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && PlayerHasBuff(Voidform))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions+=/lights_judgment,if=spell_targets.lights_judgment>=2|(!raid_event.adds.exists|raid_event.adds.in>75)
                    if (PlayerRaceSettings == "Lightforged" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && API.TargetUnitInRangeCount >= 2)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions+=/ancestral_call,if=buff.voidform.up
                    if (PlayerRaceSettings == "Mag'har Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && PlayerHasBuff(Voidform))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions.cds=power_infusion,if=priest.self_power_infusion&(buff.voidform.up|!soulbind.combat_meditation.enabled&cooldown.void_eruption.remains>=10|fight_remains<cooldown.void_eruption.remains)&(fight_remains>=cooldown.void_eruption.remains+15&cooldown.void_eruption.remains<=gcd*4|fight_remains>cooldown.power_infusion.duration|fight_remains<cooldown.void_eruption.remains+15|covenant.kyrian|buff.bloodlust.up)
                    if (API.CanCast(PowerInfusion) && IsPowerInfusion && API.PlayerHasBuff(Voidform))
                    {
                        API.CastSpell(PowerInfusion);
                        return;
                    }
                    //actions.cds+=/fae_guardians,if=!buff.voidform.up&(!cooldown.void_torrent.up|!talent.void_torrent.enabled)&(variable.dots_up&spell_targets.vampiric_touch==1|active_dot.vampiric_touch==spell_targets.vampiric_touch&spell_targets.vampiric_touch>1)|buff.voidform.up&(soulbind.grove_invigoration.enabled|soulbind.field_of_blossoms.enabled)
                    if (API.CanCast(FaeGuardians) && IsCovenant && !isExplosive && SaveQuake && PlayerCovenantSettings == "Night Fae" && (!API.PlayerHasBuff(Voidform) && (API.SpellCDDuration(VoidTorrent) > gcd || !TalentVoidTorrent) && dots_up))
                    {
                        API.CastSpell(FaeGuardians);
                        return;
                    }
                    //actions.cds+=/mindgames,target_if=insanity<90&((variable.all_dots_up&(!cooldown.void_eruption.up|!talent.hungering_void.enabled))|buff.voidform.up)&(!talent.hungering_void.enabled|debuff.hungering_void.up|!buff.voidform.up)&(!talent.searing_nightmare.enabled|spell_targets.mind_sear<5)
                    if (API.CanCast(Mindgames) && PlayerCovenantSettings == "Venthyr" && !isExplosive && SaveQuake && IsCovenant && (API.PlayerInsanity < 90 && ((all_dots_up && (API.SpellCDDuration(VoidEruption) > gcd || !TalentHungeringVoid)) || PlayerHasBuff(Voidform)) && (!TalentHungeringVoid || TargetHasDebuff(HungeringVoid) || !PlayerHasBuff(Voidform)) && (!TalentSearingNightmare || API.TargetUnitInRangeCount <5)))
                    {
                        API.CastSpell(Mindgames);
                        return;
                    }
                    //actions.cds+=/boon_of_the_ascended,if=!buff.voidform.up&!cooldown.void_eruption.up&spell_targets.mind_sear>1&!talent.searing_nightmare.enabled|(buff.voidform.up&spell_targets.mind_sear<2&!talent.searing_nightmare.enabled&(prev_gcd.1.void_bolt&(!equipped.empyreal_ordnance|!talent.hungering_void.enabled)|equipped.empyreal_ordnance&trinket.empyreal_ordnance.cooldown.remains<=162&debuff.hungering_void.up))|(buff.voidform.up&talent.searing_nightmare.enabled)
                    if (API.CanCast(BoonOfTheAscended) && IsCovenant && PlayerCovenantSettings == "Kyrian" && (!PlayerHasBuff(Voidform) && API.SpellCDDuration(VoidEruption) > gcd && API.TargetUnitInRangeCount > 1 && (IsAOE || IsForceAOE) && !TalentSearingNightmare || (PlayerHasBuff(Voidform) && API.TargetUnitInRangeCount < 2 && !TalentSearingNightmare) || (PlayerHasBuff(Voidform) && TalentSearingNightmare)))
                    {
                        API.CastSpell(BoonOfTheAscended);
                        return;
                    }
                    //actions.cds+=/unholy_nova,if=((!raid_event.adds.up&raid_event.adds.in>20)|raid_event.adds.remains>=15|raid_event.adds.duration<15)&(buff.power_infusion.up|cooldown.power_infusion.remains>=10|!priest.self_power_infusion)&(!talent.hungering_void.enabled|debuff.hungering_void.up|!buff.voidform.up)
                    if (API.CanCast(UnholyNova) && IsCovenant && PlayerCovenantSettings == "Necrolord" && (PlayerHasBuff(PowerInfusion) || API.SpellCDDuration(PowerInfusion) >= 1000 && !IsPowerInfusion || !IsPowerInfusion) && (!TalentHungeringVoid || TargetHasDebuff(HungeringVoid) || !PlayerHasBuff(Voidform)))
                    {
                        API.CastSpell(UnholyNova);
                        return;
                    }
                    //actions.trinkets+=/use_items,if=buff.voidform.up|buff.power_infusion.up|cooldown.void_eruption.remains>10
                    if (API.PlayerTrinketIsUsable(1) && !isExplosive && !API.MacroIsIgnored("Trinket1") && (PlayerHasBuff(Voidform) || PlayerHasBuff(PowerInfusion) || API.SpellCDDuration(VoidEruption) > 1000) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
                    {
                        API.CastSpell("Trinket1");
                        return;
                    }
                    //actions.cds+=/use_items,slots=trinket2,if=debuff.between_the_eyes.up|trinket.2.has_stat.any_dps|fight_remains<=20
                    if (API.PlayerTrinketIsUsable(2) && !isExplosive && !API.MacroIsIgnored("Trinket2") && (PlayerHasBuff(Voidform) || PlayerHasBuff(PowerInfusion) || API.SpellCDDuration(VoidEruption) > 1000) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
                    {
                        API.CastSpell("Trinket2");
                        return;
                    }
                    //actions+=/call_action_list,name=cwc
                    //actions.cwc=searing_nightmare,use_while_casting=1,target_if=(variable.searing_nightmare_cutoff&!variable.pool_for_cds)|(dot.shadow_word_pain.refreshable&spell_targets.mind_sear>1)
                    if (API.CanCast(SearingNightmare) && ChannelingMindSear && TalentSearingNightmare && (searing_nightmare_cutoff || API.TargetDebuffRemainingTime(SWPain) <= 360 && (API.TargetUnitInRangeCount > 1 || IsForceAOE) && (IsAOE || IsForceAOE)))
                    {
                        API.CastSpell(SearingNightmare);
                        return;
                    }
                    //actions.cwc+=/searing_nightmare,use_while_casting=1,target_if=talent.searing_nightmare.enabled&dot.shadow_word_pain.refreshable&spell_targets.mind_sear>2
                    if (API.CanCast(SearingNightmare) && ChannelingMindSear && TalentSearingNightmare && API.TargetDebuffRemainingTime(SWPain) <= 360 && (API.TargetUnitInRangeCount > 2 || IsForceAOE) && (IsAOE || IsForceAOE))
                    {
                        API.CastSpell(SearingNightmare);
                        return;
                    }
                    //actions.cwc+=/mind_blast,only_cwc=1
                    if (API.CanCast(MindBlast) && PlayerHasBuff(DarkThoughts) && (ChannelingMindSear || ChannelingMindFlay))
                    {
                        API.CastSpell(MindBlast);
                        return;
                    }
                    //actions+=/run_action_list,name=main
                    //actions.main=void_eruption,if=variable.pool_for_cds&(insanity>=40|pet.fiend.active&runeforge.shadowflame_prism.equipped&!cooldown.mind_blast.up&!cooldown.shadow_word_death.up)&(insanity<=85|talent.searing_nightmare.enabled&variable.searing_nightmare_cutoff)&!cooldown.fiend.up&(!cooldown.mind_blast.up|spell_targets.mind_sear>2)
                    if (API.CanCast(VoidEruption) && IsVoidEruption && !API.PlayerIsMoving && (API.PlayerInsanity >= 40 || API.PlayerTotemPetDuration > gcd*2 && IsLegendary == "Shadowflame Prism" && API.SpellCDDuration(MindBlast) > gcd && API.SpellCDDuration(SWDeath) > gcd) && (API.PlayerInsanity <= 85 || TalentSearingNightmare && searing_nightmare_cutoff) && (API.SpellCDDuration(Shadowfiend) > gcd && !TalentMindbender && IsShadowfiend || !IsShadowfiend && !TalentMindbender || TalentMindbender && IsMindbender && API.SpellCDDuration(Mindbender) > gcd || TalentMindbender && !IsMindbender))
                    {
                        API.CastSpell(VoidEruption);
                        return;
                    }
                    //actions.main+=/shadow_word_pain,if=buff.fae_guardians.up&!debuff.wrathful_faerie.up&spell_targets.mind_sear<4
                    if (API.CanCast(SWPain) && PlayerHasBuff(FaeGuardians) && !TargetHasDebuff(WrathfulFaerie) && (API.TargetUnitInRangeCount < 4 || !IsAOE))
                    {
                        API.CastSpell(SWPain);
                        return;
                    }
                    //actions.main+=/damnation,target_if=!variable.all_dots_up
                    if (API.CanCast(Damnation) && IsDamnation && TalentDamnation && !all_dots_up)
                    {
                        API.CastSpell(Damnation);
                        return;
                    }
                    //actions.main+=/shadow_word_death,if=pet.fiend.active&runeforge.shadowflame_prism.equipped&pet.fiend.remains<=gcd
                    if (API.CanCast(SWDeath) && IsLegendary == "Shadowflame Prism" && API.PlayerTotemPetDuration <= gcd)
                    {
                        API.CastSpell(SWDeath);
                        return;
                    }
                    //actions.main+=/mind_blast,if=(cooldown.mind_blast.charges>1&(debuff.hungering_void.up|!talent.hungering_void.enabled)|pet.fiend.remains<=cast_time+gcd)&pet.fiend.active&runeforge.shadowflame_prism.equipped&pet.fiend.remains>=cast_time
                    if (API.CanCast(MindBlast) && (!API.PlayerIsMoving || PlayerHasBuff(DarkThoughts)) && ((API.SpellCharges(MindBlast) > 1 && (TargetHasDebuff(HungeringVoid) || !TalentHungeringVoid) || API.PlayerTotemPetDuration <= gcd*2) && IsLegendary == "Shadowflame Prism" && API.PlayerTotemPetDuration >= gcd*2))
                    {
                        API.CastSpell(MindBlast);
                        return;
                    }
                    //actions.main+=/mind_blast,if=cooldown.mind_blast.charges>1&pet.fiend.active&runeforge.shadowflame_prism.equipped&!cooldown.void_bolt.up
                    if (API.CanCast(MindBlast) && (!API.PlayerIsMoving || PlayerHasBuff(DarkThoughts)) && (API.SpellCharges(MindBlast) > 1 && API.PlayerTotemPetDuration > gcd*2 && IsLegendary == "Shadowflame Prism" && API.SpellCDDuration(VoidBolt) > gcd))
                    {
                        API.CastSpell(MindBlast);
                        return;
                    }
                    //actions.main+=/void_bolt,if=insanity<=85&talent.hungering_void.enabled&talent.searing_nightmare.enabled&spell_targets.mind_sear<=6|((talent.hungering_void.enabled&!talent.searing_nightmare.enabled)|spell_targets.mind_sear=1)
                    if (API.CanCast(VoidBolt) && PlayerHasBuff(Voidform) && (API.PlayerInsanity <= 85 && TalentHungeringVoid && TalentSearingNightmare && (API.TargetUnitInRangeCount <= 6 || !IsAOE) || ((TalentHungeringVoid && !TalentSearingNightmare) || API.TargetUnitInRangeCount == 1)))
                    {
                        API.CastSpell(VoidBolt);
                        return;
                    }
                    //actions.main+=/devouring_plague,if=(refreshable|insanity>75)&(!variable.pool_for_cds|insanity>=85)&(!talent.searing_nightmare.enabled|(talent.searing_nightmare.enabled&!variable.searing_nightmare_cutoff))
                    if (API.CanCast(DevouringPlague) && (API.TargetDebuffRemainingTime(DevouringPlague) <= 180 || API.PlayerInsanity > 75) && (!TalentSearingNightmare || (TalentSearingNightmare && !searing_nightmare_cutoff && !IsForceAOE || API.PlayerInsanity >= 85)))
                    {
                        API.CastSpell(DevouringPlague);
                        return;
                    }
                    //actions.main+=/void_bolt,if=spell_targets.mind_sear<(4+conduit.dissonant_echoes.enabled)&insanity<=85&talent.searing_nightmare.enabled|!talent.searing_nightmare.enabled
                    if (API.CanCast(VoidBolt) && PlayerHasBuff(Voidform) && (API.TargetUnitInRangeCount < 4 || !IsAOE) && (API.PlayerInsanity <= 85 && TalentSearingNightmare || !TalentSearingNightmare))
                    {
                        API.CastSpell(VoidBolt);
                        return;
                    }
                    //actions.main+=/shadow_word_death,target_if=(target.health.pct<20&spell_targets.mind_sear<4)|(pet.fiend.active&runeforge.shadowflame_prism.equipped)
                    if (API.CanCast(SWDeath) && (API.TargetHealthPercent < 20 && (API.TargetUnitInRangeCount < 4 || !IsAOE) || (IsLegendary == "Shadowflame Prism" && API.PlayerTotemPetDuration > gcd*2)))
                    {
                        API.CastSpell(SWDeath);
                        return;
                    }
                    //actions.main+=/surrender_to_madness,target_if=target.time_to_die<25&buff.voidform.down
                    if (API.CanCast(SurrendertoMadness) && TalentSurrendertoMadness && IsCooldowns && API.TargetTimeToDie < 2500 && !PlayerHasBuff(Voidform))
                    {
                        API.CastSpell(SurrendertoMadness);
                        return;
                    }
                    //actions.main+=/void_torrent,target_if=variable.dots_up&target.time_to_die>3&(buff.voidform.down|buff.voidform.remains<cooldown.void_bolt.remains)&active_dot.vampiric_touch==spell_targets.vampiric_touch&spell_targets.mind_sear<(5+(6*talent.twist_of_fate.enabled))
                    if (API.CanCast(VoidTorrent) && !API.PlayerIsMoving && TalentVoidTorrent && dots_up && (!PlayerHasBuff(Voidform) || API.PlayerBuffTimeRemaining(Voidform) < API.SpellCDDuration(VoidBolt)) && API.TargetUnitInRangeCount < 5+(6*(TalentTwistofFate ? 1 : 0)))
                    {
                        API.CastSpell(VoidTorrent);
                        return;
                    }
                    //actions.main+=/mindbender,if=dot.vampiric_touch.ticking&(talent.searing_nightmare.enabled&spell_targets.mind_sear>variable.mind_sear_cutoff|dot.shadow_word_pain.ticking)&(!runeforge.shadowflame_prism.equipped|active_dot.vampiric_touch==spell_targets.vampiric_touch)
                    if (API.CanCast(Mindbender) && TalentMindbender && IsMindbender && TargetHasDebuff(VampiricTouch) && (TalentSearingNightmare && API.TargetUnitInRangeCount > 2 || TargetHasDebuff(SWPain)))
                    {
                        API.CastSpell(Mindbender);
                        return;
                    }
                    //actions.main+=/shadow_crash,if=raid_event.adds.in>10
                    if (API.CanCast(ShadowCrash) && TalentShadowCrash && (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE || IsForceAOE))
                    {
                        API.CastSpell(ShadowCrash);
                        return;
                    }
                    //actions.main+=/mind_sear,target_if=spell_targets.mind_sear>variable.mind_sear_cutoff&buff.dark_thought.up,chain=1,interrupt_immediate=1,interrupt_if=ticks>=2
                    if (API.CanCast(MindSear) && !API.PlayerIsCasting(true) && !API.PlayerIsMoving && PlayerHasBuff(DarkThoughts) && (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE || IsForceAOE))
                    {
                        API.CastSpell(MindSear);
                        return;
                    }
                    //actions.main+=/mind_flay,if=buff.dark_thought.up&variable.dots_up,chain=1,interrupt_immediate=1,interrupt_if=ticks>=2&cooldown.void_bolt.up
                    if (API.CanCast(MindFlay) && !API.PlayerIsCasting(true) && !API.PlayerIsMoving && PlayerHasBuff(DarkThoughts) && dots_up && !IsForceAOE)
                    {
                        API.CastSpell(MindFlay);
                        return;
                    }
                    //actions.main+=/mind_blast,if=variable.dots_up&raid_event.movement.in>cast_time+0.5&spell_targets.mind_sear<(4+2*talent.misery.enabled+active_dot.vampiric_touch*talent.psychic_link.enabled+(spell_targets.mind_sear>?5)*(pet.fiend.active&runeforge.shadowflame_prism.equipped))&(!runeforge.shadowflame_prism.equipped|!cooldown.fiend.up&runeforge.shadowflame_prism.equipped|active_dot.vampiric_touch==spell_targets.vampiric_touch)
                    if (API.CanCast(MindBlast) && (!API.PlayerIsMoving || PlayerHasBuff(DarkThoughts)) && dots_up && API.TargetUnitInRangeCount < 6)
                    {
                        API.CastSpell(MindBlast);
                        return;
                    }
                    //actions.main+=/vampiric_touch,target_if=refreshable&target.time_to_die>6|(talent.misery.enabled&dot.shadow_word_pain.refreshable)|buff.unfurling_darkness.up
                    if (API.CanCast(VampiricTouch) && !API.PlayerIsMoving && !(API.LastSpellCastInGame == VampiricTouch || API.PlayerCurrentCastSpellID == 34914) && (API.TargetDebuffRemainingTime(VampiricTouch) < 630 || (TalentMisery && API.TargetDebuffRemainingTime(SWPain) < 360) || PlayerHasBuff(UnfurlingDarkness)))
                    {
                        API.CastSpell(VampiricTouch);
                        return;
                    }
                    //actions.main+=/shadow_word_pain,if=refreshable&target.time_to_die>4&!talent.misery.enabled&talent.psychic_link.enabled&spell_targets.mind_sear>2
                    if (API.CanCast(SWPain) && API.TargetDebuffRemainingTime(SWPain) < 360 && !TalentMisery && TalentPsychicLink && (API.TargetUnitInRangeCount > 2 && IsAOE || IsForceAOE))
                    {
                        API.CastSpell(SWPain);
                        return;
                    }
                    //actions.main+=/shadow_word_pain,target_if=refreshable&target.time_to_die>4&!talent.misery.enabled&!(talent.searing_nightmare.enabled&spell_targets.mind_sear>variable.mind_sear_cutoff)&(!talent.psychic_link.enabled|(talent.psychic_link.enabled&spell_targets.mind_sear<=2))
                    if (API.CanCast(SWPain) && API.TargetDebuffRemainingTime(SWPain) < 360 && !TalentMisery && (!TalentSearingNightmare || TalentSearingNightmare && (API.TargetUnitInRangeCount < 2 && !IsForceAOE)) && (!TalentPsychicLink || TalentPsychicLink && API.TargetUnitInRangeCount <= 2 && !IsForceAOE))
                    {
                        API.CastSpell(SWPain);
                        return;
                    }
                    if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && isMOinRange && API.MouseoverHealthPercent > 0)
                    {
                        if (API.CanCast(VampiricTouch) && !API.PlayerIsMoving && !API.MacroIsIgnored(VampiricTouch + "MO") && !(API.LastSpellCastInGame == VampiricTouch || API.PlayerCurrentCastSpellID == 34914) && (API.MouseoverDebuffRemainingTime(VampiricTouch) < 630 || (TalentMisery && API.MouseoverDebuffRemainingTime(SWPain) < 360) || PlayerHasBuff(UnfurlingDarkness)))
                        {
                            API.CastSpell(VampiricTouch + "MO");
                            return;
                        }
                        if (API.CanCast(SWPain) && !API.MacroIsIgnored(SWPain + "MO") && API.MouseoverDebuffRemainingTime(SWPain) < 360 && !TalentMisery && TalentPsychicLink && (API.TargetUnitInRangeCount > 2 && IsAOE || IsForceAOE))
                        {
                            API.CastSpell(SWPain + "MO");
                            return;
                        }
                        if (API.CanCast(SWPain) && !API.MacroIsIgnored(SWPain + "MO") && API.MouseoverDebuffRemainingTime(SWPain) < 360 && !TalentMisery && (!TalentSearingNightmare || TalentSearingNightmare && API.TargetUnitInRangeCount < 2 && !IsForceAOE) && (!TalentPsychicLink || TalentPsychicLink && API.TargetUnitInRangeCount <= 2 && !IsForceAOE))
                        {
                            API.CastSpell(SWPain + "MO");
                            return;
                        }
                    }
                    //actions.main+=/mind_sear,target_if=spell_targets.mind_sear>variable.mind_sear_cutoff,chain=1,interrupt_immediate=1,interrupt_if=ticks>=2
                    if (API.CanCast(MindSear) && !API.PlayerIsCasting(true) && !API.PlayerIsMoving && (API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE || IsForceAOE))
                    {
                        API.CastSpell(MindSear);
                        return;
                    }
                    //actions.main+=/mind_flay,chain=1,interrupt_immediate=1,interrupt_if=ticks>=2&cooldown.void_bolt.up
                    if (API.CanCast(MindFlay) && !API.PlayerIsCasting(true) && !IsForceAOE && !API.PlayerIsMoving)
                    {
                        API.CastSpell(MindFlay);
                        return;
                    }
                    //actions.main+=/shadow_word_death
                    if (API.CanCast(SWDeath) && API.PlayerIsMoving)
                    {
                        API.CastSpell(SWDeath);
                        return;
                    }
                    //actions.main+=/shadow_word_pain
                    if (API.CanCast(SWPain) && API.PlayerIsMoving)
                    {
                        API.CastSpell(SWPain);
                        return;
                    }
                }
            }
        }
    }
}

