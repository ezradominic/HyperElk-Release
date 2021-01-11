// Changelog
// v1.0 First release
// v1.1 bearform fix
// v1.2 bloodtalons and covenant stuff
// v1.3 low level fix
// v1.4 improved bloodtalons code and alot of small fixes
// v1.5 covenant update :-)
// v1.6 legendary prep
// v1.7 Racials and Trinkets
// v1.8 Mighty Bash added
// v1.9 Bloodtalons fixed
// v2.0 some small changes
// v2.1 spell ids and alot of other stuff
// v2.2 Bloodtalons fix 
// v2.3 swipe bear added
// v2.4 kitty bear swap adjustment
// v2.5 complete rewrite
// v2.6 racials and a few small fixes
// v2.7 roots for Torghast
// v2.8 regrowth fix

using System.Diagnostics;

namespace HyperElk.Core
{
    public class Feraldruid : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsOwlweave => API.ToggleIsEnabled("Owlweave");
        private bool IsAutoForm => API.ToggleIsEnabled("Auto Form");
        //Spell,Auras
        private string CatForm = "Cat Form";
        private string Rip = "Rip";
        private string Rake = "Rake";
        private string Prowl = "Prowl";
        private string TigersFury = "Tiger's Fury";
        private string Shred = "Shred";
        private string Regrowth = "Regrowth";
        private string FerociousBite = "Ferocious Bite";
        private string SavageRoar = "Savage Roar";
        private string Thrash = "Thrash";
        private string Thrashbear = "Thrash Bear";
        private string Swipe = "Swipe";
        private string Swipebear = "Swipe Bear";
        private string BrutalSlash = "Brutal Slash";
        private string Moonfire = "Moonfire";
        private string FeralFrenzy = "Feral Frenzy";
        private string Berserk = "Berserk";
        private string Incarnation = "Incarnation: King of the Jungle";
        private string SkullBash = "Skull Bash";
        private string SurvivalInstincts = "Survival Instincts";
        private string Barkskin = "Barkskin";
        private string Renewal = "Renewal";
        private string StampedingRoar = "Stampeding Roar";
        private string Maim = "Maim";
        private string Typhoon = "Typhoon";
        private string PrimalWrath = "Primal Wrath";
        private string BearForm = "Bear Form";
        private string Mangle = "Mangle";
        private string FrenziedRegeneration = "Frenzied Regeneration";
        private string Ironfur = "Ironfur";
        private string PredatorySwiftness = "Predatory Swiftness";
        private string Bloodtalons = "Bloodtalons";
        private string Clearcasting = "Clearcasting";
        private string TravelForm = "Travel Form";
        private string ScentofBlood = "Scent of Blood";
        private string Shadowmeld = "Shadowmeld";
        private string RavenousFrenzy = "Ravenous Frenzy";
        private string ConvoketheSpirits = "Convoke the Spirits";
        private string AdaptiveSwarm = "Adaptive Swarm";
        private string LoneEmpowerment = "Lone Empowerment";
        private string LoneSpirit = "Lone Spirit";
        private string Soulshape = "Soulshape";
        private string ApexPredatorsCraving = "Apex Predator's Craving";
        private string MightyBash = "Mighty Bash";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string EmpowerBond = "Empower Bond";
        private string KindredSpirits = "Kindred Spirits";
        private string SuddenAmbush = "Sudden Ambush";
        private string MoonkinForm= "Moonkin Form";
        private string Sunfire = "Sunfire";
        private string Starsurge= "Starsurge";
        private string HeartoftheWild = "Heart of the Wild";
        private string Wrath = "Wrath";
        private string Starfire = "Starfire";
        private string MoonfireOwl = "MoonfireOwl";
        private string EntanglingRoots = "Entangling Roots";
        private string MassEntanglement = "Mass Entanglement";

        //Talents
        bool TalentLunarInspiration => API.PlayerIsTalentSelected(1, 3);
        bool TalentPredator => API.PlayerIsTalentSelected(1, 1);
        bool TalentSabertooth => API.PlayerIsTalentSelected(1, 2);
        bool TalentRenewal => API.PlayerIsTalentSelected(2, 2);
        bool TalentMightyBash => API.PlayerIsTalentSelected(4, 1);
        bool TalentBalanceAffinity => API.PlayerIsTalentSelected(3, 1);
        bool TalentGuardianAffinity => API.PlayerIsTalentSelected(3, 2);
        bool TalentMassEntanglement => API.PlayerIsTalentSelected(4, 2);
        bool TalentHeartoftheWild => API.PlayerIsTalentSelected(4, 3);
        bool TalentIncarnation => API.PlayerIsTalentSelected(5, 3);
        bool TalentSavageRoar => API.PlayerIsTalentSelected(5, 2);
        bool TalentScentofBlood => API.PlayerIsTalentSelected(6, 1);
        bool TalentBrutalSlash => API.PlayerIsTalentSelected(6, 2);
        bool TalentPrimalWrath => API.PlayerIsTalentSelected(6, 3);
        bool TalentBloodtalons => API.PlayerIsTalentSelected(7, 2);
        bool TalentFeralFrenzy => API.PlayerIsTalentSelected(7, 3);

        //General
        private static readonly Stopwatch rakewatch = new Stopwatch();
        private static readonly Stopwatch thrashwatch = new Stopwatch();
        private static readonly Stopwatch brutalwatch = new Stopwatch();
        private static readonly Stopwatch moonwatch = new Stopwatch();
        private static readonly Stopwatch shredwatch = new Stopwatch();
        private static readonly Stopwatch swipewatch = new Stopwatch();
        bool TimersRunning => (rakewatch.IsRunning || thrashwatch.IsRunning || brutalwatch.IsRunning || moonwatch.IsRunning || shredwatch.IsRunning || swipewatch.IsRunning);
        private int PlayerLevel => API.PlayerLevel;
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        bool WeaveConditions => (API.PlayerEnergy < 40 && (PlayerHasBuff(Bloodtalons) || !TalentBloodtalons) && (API.TargetDebuffRemainingTime(Rip) > 450 || API.PlayerComboPoints < 5) && API.SpellCDDuration(TigersFury) >= 650 && API.PlayerBuffStacks(Clearcasting) < 1 && !PlayerHasBuff(ApexPredatorsCraving) && (API.PlayerBuffTimeRemaining(Incarnation) > 500 && TalentIncarnation || !TalentIncarnation && API.PlayerBuffTimeRemaining(Berserk) > 500 || !IncaBerserk) && (API.SpellISOnCooldown(ConvoketheSpirits) || PlayerCovenantSettings != "Night Fae"));
        bool BloodtalonsEnergie => (TalentBloodtalons && !PlayerHasBuff(Bloodtalons) && (API.PlayerEnergy + 3.5 * EnergyRegen + (40 * (PlayerHasBuff(Clearcasting) ? 1 : 0))) < (115 - 23 * (PlayerHasBuff(Incarnation) ? 1 : 0)) && !TimersRunning);
        bool BuffBTRakeUp => rakewatch.IsRunning && rakewatch.ElapsedMilliseconds + GCD < 4000;
        bool BuffBTMoonfireUp => moonwatch.IsRunning && moonwatch.ElapsedMilliseconds + GCD < 4000;
        bool BuffBTThrashUp => thrashwatch.IsRunning && thrashwatch.ElapsedMilliseconds + GCD < 4000;
        bool BuffBTBrutalSlashUp => brutalwatch.IsRunning && brutalwatch.ElapsedMilliseconds + GCD < 4000;
        bool BuffBTSwipeUp => swipewatch.IsRunning && swipewatch.ElapsedMilliseconds + GCD < 4000;
        bool BuffBTShredUp => shredwatch.IsRunning && shredwatch.ElapsedMilliseconds + GCD < 4000;
        int EnergyDefecit => API.PlayeMaxEnergy - API.PlayerEnergy;
        float GCD => API.SpellGCDTotalDuration;
        float EnergyRegen => 10f * (1f + API.PlayerGetHaste);
        private bool isMelee => (TalentBalanceAffinity && API.TargetRange < 8 || !TalentBalanceAffinity && API.TargetRange < 6);
        private bool isInRange => (TalentBalanceAffinity && API.TargetRange < 43 || !TalentBalanceAffinity && API.TargetRange < 40);
        private bool isMOMelee => (TalentBalanceAffinity && API.MouseoverRange < 8 || !TalentBalanceAffinity && API.MouseoverRange < 6);
        private bool isThrashMelee => (TalentBalanceAffinity && API.TargetRange < 11 || !TalentBalanceAffinity && API.TargetRange < 9);
        private bool isMOThrashMelee => (TalentBalanceAffinity && API.MouseoverRange < 11 || !TalentBalanceAffinity && API.MouseoverRange < 9);
        private bool isKickRange => (TalentBalanceAffinity && API.TargetRange < 16 || !TalentBalanceAffinity && API.TargetRange < 14);
        private bool IncaBerserk => (PlayerHasBuff(Incarnation) || PlayerHasBuff(Berserk));
        bool IsFeralFrenzy => (UseFeralFrenzy == "with Cooldowns" && IsCooldowns || UseFeralFrenzy == "always");
        bool IsIncarnation => (UseIncarnation == "with Cooldowns" && IsCooldowns || UseIncarnation == "always");
        bool IsBerserk => (UseBerserk == "with Cooldowns" && IsCooldowns || UseBerserk == "always");
        bool IsCovenant => (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE);
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns && IncaBerserk || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isMelee;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns && IncaBerserk || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isMelee;


        //CBProperties
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseFeralFrenzy => CDUsage[CombatRoutine.GetPropertyInt(FeralFrenzy)];
        private string UseIncarnation => CDUsage[CombatRoutine.GetPropertyInt(Incarnation)];
        private string UseBerserk => CDUsage[CombatRoutine.GetPropertyInt(Berserk)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool ProwlOOC => CombatRoutine.GetPropertyBool("ProwlOOC");
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private bool RootsTorghast => CombatRoutine.GetPropertyBool("RootsTorghast");
        private bool DontAttackRoots => CombatRoutine.GetPropertyBool("DontAttackRoots");
        private bool AutoMoonkin => CombatRoutine.GetPropertyBool(MoonkinForm);
        private int RegrowthLifePercent => numbList[CombatRoutine.GetPropertyInt(Regrowth)];
        private int RenewalLifePercent => numbList[CombatRoutine.GetPropertyInt(Renewal)];
        private int BarkskinLifePercent => numbList[CombatRoutine.GetPropertyInt(Barkskin)];
        private int SurvivalInstinctsLifePercent => numbList[CombatRoutine.GetPropertyInt(SurvivalInstincts)];
        private int FrenziedRegenerationLifePercent => numbList[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => numbList[CombatRoutine.GetPropertyInt(Ironfur)];
        private int BearFormLifePercent => numbList[CombatRoutine.GetPropertyInt(BearForm)];
        private int KittyFormLifePercent => numbList[CombatRoutine.GetPropertyInt(CatForm)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private bool needheal => PlayerHasBuff(PredatorySwiftness) && API.PlayerHealthPercent <= RegrowthLifePercent && !API.SpellIsIgnored(Regrowth);

        public override void Initialize()
        {
            CombatRoutine.Name = "Feral Druid by smartie";
            API.WriteLog("Welcome to smartie`s Feral Druid v2.8");
            API.WriteLog("Create the following mouseover macros and assigned to the bind:");
            API.WriteLog("RakeMO - /cast [@mouseover] Rake");
            API.WriteLog("ThrashMO - /cast [@mouseover] Thrash");
            API.WriteLog("RipMO - /cast [@mouseover] Rip");

            //Spells
            CombatRoutine.AddSpell(CatForm, 768, "NumPad1");
            CombatRoutine.AddSpell(Rip, 1079, "D6");
            CombatRoutine.AddSpell(Rake, 1822, "D2");
            CombatRoutine.AddSpell(Prowl, 5215, "NumPad3");
            CombatRoutine.AddSpell(TigersFury, 5217, "D1");
            CombatRoutine.AddSpell(Shred, 5221, "D3");
            CombatRoutine.AddSpell(Regrowth, 8936, "F2");
            CombatRoutine.AddSpell(FerociousBite, 22568, "D7");
            CombatRoutine.AddSpell(SavageRoar, 52610, "D9");
            CombatRoutine.AddSpell(Thrash, 106830, "D4");
            CombatRoutine.AddSpell(Thrashbear, 77758, "D4");
            CombatRoutine.AddSpell(Swipe, 106785, "D5");
            CombatRoutine.AddSpell(Swipebear, 213771, "D5");
            CombatRoutine.AddSpell(BrutalSlash, 202028, "D5");
            CombatRoutine.AddSpell(Moonfire, 8921, "NumPad2");
            CombatRoutine.AddSpell(FeralFrenzy, 274837, "NumPad4");
            CombatRoutine.AddSpell(Berserk, 106951, "D1");
            CombatRoutine.AddSpell(Incarnation, 102543, "D1");
            CombatRoutine.AddSpell(SkullBash, 106839, "F12");
            CombatRoutine.AddSpell(SurvivalInstincts, 61336, "F1");
            CombatRoutine.AddSpell(Barkskin, 22812, "F1");
            CombatRoutine.AddSpell(Renewal, 108238, "F6");
            CombatRoutine.AddSpell(StampedingRoar, 106898, "F7");
            CombatRoutine.AddSpell(Maim, 22570, "F9");
            CombatRoutine.AddSpell(Typhoon, 132469, "D1");
            CombatRoutine.AddSpell(PrimalWrath, 285381, "D0");
            CombatRoutine.AddSpell(BearForm, 5487, "NumPad4");
            CombatRoutine.AddSpell(Mangle, 33917, "D3");
            CombatRoutine.AddSpell(FrenziedRegeneration, 22842, "D7");
            CombatRoutine.AddSpell(Ironfur, 192081, "D8");
            CombatRoutine.AddSpell(TravelForm, 783, "NumPad6");
            CombatRoutine.AddSpell(MightyBash, 5211, "D1");
            CombatRoutine.AddSpell(RavenousFrenzy, 323546, "D1");
            CombatRoutine.AddSpell(ConvoketheSpirits, 323764, "D1");
            CombatRoutine.AddSpell(AdaptiveSwarm, 325727, "D1");
            CombatRoutine.AddSpell(LoneEmpowerment, 338142, "D1");
            CombatRoutine.AddSpell(EmpowerBond, "D1");
            CombatRoutine.AddSpell(MoonkinForm,197625, "D1");
            CombatRoutine.AddSpell(Sunfire,197630, "D1");
            CombatRoutine.AddSpell(Starsurge,197626, "D1");
            CombatRoutine.AddSpell(HeartoftheWild, 319454, "D1");
            CombatRoutine.AddSpell(Wrath,5176, "D1");
            CombatRoutine.AddSpell(Starfire, 197628, "D1");
            CombatRoutine.AddSpell(EntanglingRoots, 339, "D1");
            CombatRoutine.AddSpell(MassEntanglement, 102359, "D1");


            //Macros
            CombatRoutine.AddMacro(Rake + "MO", "D2");
            CombatRoutine.AddMacro(Thrash + "MO", "D6");
            CombatRoutine.AddMacro(Rip + "MO", "D6");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");


            //Buffs
            CombatRoutine.AddBuff(Prowl, 5215);
            CombatRoutine.AddBuff(CatForm, 768);
            CombatRoutine.AddBuff(BearForm, 5487);
            CombatRoutine.AddBuff(TigersFury, 5217);
            CombatRoutine.AddBuff(SavageRoar, 52610);
            CombatRoutine.AddBuff(PredatorySwiftness, 69369);
            CombatRoutine.AddBuff(Berserk, 106951);
            CombatRoutine.AddBuff(Bloodtalons, 145152);
            CombatRoutine.AddBuff(Clearcasting, 135700);
            CombatRoutine.AddBuff(SurvivalInstincts, 61336);
            CombatRoutine.AddBuff(TravelForm, 783);
            CombatRoutine.AddBuff(Ironfur, 192081);
            CombatRoutine.AddBuff(FrenziedRegeneration, 22842);
            CombatRoutine.AddBuff(Incarnation, 102543);
            CombatRoutine.AddBuff(ScentofBlood, 285646);
            CombatRoutine.AddBuff(Shadowmeld, 58984);
            CombatRoutine.AddBuff(RavenousFrenzy, 323546);
            CombatRoutine.AddBuff(LoneSpirit, 338041);
            CombatRoutine.AddBuff(Soulshape, 310143);
            CombatRoutine.AddBuff(ApexPredatorsCraving, 339140);
            CombatRoutine.AddBuff(KindredSpirits, 326967);
            CombatRoutine.AddBuff(SuddenAmbush, 340698);
            CombatRoutine.AddBuff(HeartoftheWild, 108291);
            CombatRoutine.AddBuff(MoonkinForm, 197625);

            //Debuff
            CombatRoutine.AddDebuff(Rip, 1079);
            CombatRoutine.AddDebuff(Thrash, 106830);
            CombatRoutine.AddDebuff(Thrashbear, 192090);
            CombatRoutine.AddDebuff(Rake, 155722);
            CombatRoutine.AddDebuff(Moonfire, 155625);
            CombatRoutine.AddDebuff(MoonfireOwl, 164812);
            CombatRoutine.AddDebuff(AdaptiveSwarm, 325727);
            CombatRoutine.AddDebuff(Sunfire, 164815);
            CombatRoutine.AddDebuff(EntanglingRoots, 339);
            CombatRoutine.AddDebuff(MassEntanglement, 102359);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Owlweave");
            CombatRoutine.AddToggle("Auto Form");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(FeralFrenzy, "Use " + FeralFrenzy, CDUsage, "Use " + FeralFrenzy + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Incarnation, "Use " + Incarnation, CDUsage, "Use " + Incarnation + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Berserk, "Use " + Berserk, CDUsage, "Use " + Berserk + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("ProwlOOC", "ProwlOOC", true, "Use Prowl out of Combat", "Generic");
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
            CombatRoutine.AddProp("RootsTorghast", "Use Roots in Torghast", false, "Use Roots in Torghast", "Roots");
            CombatRoutine.AddProp("DontAttackRoots", "Dont Attack roots", false, "Rota wont attack Targets that are rooted", "Roots");
            CombatRoutine.AddProp(MoonkinForm, "Auto Moonkin Form", false, "Will auto switch to Moonkin Form when out of melee Range", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Regrowth, Regrowth + " Life Percent", numbList, "Life percent at which" + Regrowth + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", numbList, "Life percent at which" + Renewal + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Barkskin, Barkskin + " Life Percent", numbList, "Life percent at which" + Barkskin + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SurvivalInstincts, SurvivalInstincts + " Life Percent", numbList, "Life percent at which" + SurvivalInstincts + "is used, set to 0 to disable", "Defense", 30);
            CombatRoutine.AddProp(FrenziedRegeneration, FrenziedRegeneration + " Life Percent", numbList, "Life percent at which" + FrenziedRegeneration + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp(Ironfur, Ironfur + " Life Percent", numbList, "Life percent at which" + Ironfur + "is used, set to 0 to disable", "Defense", 90);
            CombatRoutine.AddProp(BearForm, BearForm + " Life Percent", numbList, "Life percent at which rota will go into" + BearForm + "set to 0 to disable", "Defense", 20);
            CombatRoutine.AddProp(CatForm, CatForm + " Life Percent", numbList, "Life percent at which rota will go back into" + CatForm + "set to 0 to disable", "Defense", 40);
        }
        public override void Pulse()
        {
            //API.WriteLog("Energie Regen: " + EnergyRegen);
            //API.WriteLog("Lastspell: " + API.LastSpellCastInGame);
            //API.WriteLog("Targetrange: " + API.TargetRange);
            if (API.LastSpellCastInGame == Rake && !rakewatch.IsRunning)
            {
                rakewatch.Restart();
            }
            if ((API.LastSpellCastInGame == Thrash || API.LastSpellCastInGame == Thrashbear) && !thrashwatch.IsRunning)
            {
                thrashwatch.Restart();
            }
            if (API.LastSpellCastInGame == BrutalSlash && !brutalwatch.IsRunning)
            {
                brutalwatch.Restart();
            }
            if (API.LastSpellCastInGame == Moonfire && !moonwatch.IsRunning)
            {
                moonwatch.Restart();
            }
            if (API.LastSpellCastInGame == Shred && !shredwatch.IsRunning)
            {
                shredwatch.Restart();
            }
            if ((API.LastSpellCastInGame == Swipe || API.LastSpellCastInGame == Swipebear) && !swipewatch.IsRunning)
            {
                swipewatch.Restart();
                //API.WriteLog("Swipewatch started");
            }
            // Stopwatch stop
            if (rakewatch.IsRunning && rakewatch.ElapsedMilliseconds > 4000)
            {
                rakewatch.Reset();
            }
            if (thrashwatch.IsRunning && thrashwatch.ElapsedMilliseconds > 4000)
            {
                thrashwatch.Reset();
            }
            if (brutalwatch.IsRunning && brutalwatch.ElapsedMilliseconds > 4000)
            {
                brutalwatch.Reset();
            }
            if (moonwatch.IsRunning && moonwatch.ElapsedMilliseconds > 4000)
            {
                moonwatch.Reset();
            }
            if (shredwatch.IsRunning && shredwatch.ElapsedMilliseconds > 4000)
            {
                shredwatch.Reset();
            }
            if (swipewatch.IsRunning && swipewatch.ElapsedMilliseconds > 4000)
            {
                swipewatch.Reset();
            }
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            if (ProwlOOC && API.CanCast(Prowl) && PlayerLevel >= 17 && !PlayerHasBuff(Prowl) && !API.PlayerIsMounted && !PlayerHasBuff(TravelForm))
            {
                API.CastSpell(Prowl);
                return;
            }
            if ((!API.PlayerHasBuff(CatForm) && PlayerLevel >= 5) && !API.PlayerHasBuff(BearForm) && !API.PlayerHasBuff(MoonkinForm) && !PlayerHasBuff(TravelForm) && !API.PlayerHasBuff(Soulshape) && IsAutoForm)
            {
                API.CastSpell(CatForm);
                return;
            }
            if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !PlayerHasBuff(TravelForm) && IsAutoForm)
            {
                API.CastSpell(TravelForm);
                return;
            }
        }
        public override void CombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            if (DontAttackRoots && (TargetHasDebuff(EntanglingRoots) || TargetHasDebuff(MassEntanglement)))
                return;
            if (!API.PlayerIsMounted && !PlayerHasBuff(TravelForm))
            {
                //Def cds and kick
                Defensive();
                if (PlayerHasBuff(BearForm))
                {
                    if (API.CanCast(Thrashbear) && isThrashMelee && PlayerLevel >= 11 && !TargetHasDebuff(Thrashbear))
                    {
                        API.CastSpell(Thrashbear);
                        return;
                    }
                    if (API.CanCast(Mangle) && isMelee && PlayerLevel >= 8 && TargetHasDebuff(Thrashbear))
                    {
                        API.CastSpell(Mangle);
                        return;
                    }
                    if (API.CanCast(Swipebear) && isThrashMelee && PlayerLevel >= 42 && TargetHasDebuff(Thrashbear))
                    {
                        API.CastSpell(Swipebear);
                        return;
                    }
                }
                if (!API.PlayerHasBuff(MoonkinForm) && TalentBalanceAffinity && !PlayerHasBuff(Prowl) && !API.PlayerIsMoving && !isMelee && isInRange && IsAutoForm && AutoMoonkin)
                {
                    API.CastSpell(MoonkinForm);
                    return;
                }
                if (PlayerHasBuff(MoonkinForm) && !isMelee && isInRange)
                {
                    if (API.CanCast(Moonfire) && API.TargetDebuffRemainingTime(MoonfireOwl) < GCD * 2)
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (API.CanCast(Sunfire) && API.TargetDebuffRemainingTime(Sunfire) < GCD * 2)
                    {
                        API.CastSpell(Sunfire);
                        return;
                    }
                    if (API.CanCast(Starsurge) && !API.PlayerIsMoving)
                    {
                        API.CastSpell(Starsurge);
                        return;
                    }
                    if (API.CanCast(Wrath) && !API.PlayerIsMoving && API.TargetUnitInRangeCount < 2)
                    {
                        API.CastSpell(Wrath);
                        return;
                    }
                    if (API.CanCast(Starfire) && !API.PlayerIsMoving && API.TargetUnitInRangeCount > 2)
                    {
                        API.CastSpell(Starfire);
                        return;
                    }
                }
                //actions=call_action_list,name=owlweave,if=variable.owlweave=1
                if (IsOwlweave && IsAutoForm && TalentBalanceAffinity)
                {
                    Owlweave();
                }
                if ((!API.PlayerHasBuff(CatForm) && PlayerLevel >= 5) && (API.TargetDebuffRemainingTime(Sunfire) > 200 && IsOwlweave && TalentBalanceAffinity || !IsOwlweave || !TalentBalanceAffinity) && isMelee && !API.PlayerHasBuff(BearForm) && !API.PlayerHasBuff(Soulshape) && IsAutoForm)
                {
                    API.CastSpell(CatForm);
                    return;
                }
                //actions+=/run_action_list,name=stealth,if=buff.shadowmeld.up|buff.prowl.up
                if (PlayerHasBuff(Shadowmeld) || PlayerHasBuff(Prowl))
                {
                    Stealth();
                }
                //actions+=/call_action_list,name=cooldown
                if (PlayerHasBuff(CatForm) && !needheal)
                {
                    Cooldowns();
                    //actions+=/run_action_list,name=finisher,if=combo_points>=(5-variable.4cp_bite)
                    if (API.PlayerComboPoints > 4 && !PlayerHasBuff(Shadowmeld) && !PlayerHasBuff(Prowl))
                    {
                        Finisher();
                    }
                    //actions+=/call_action_list,name=stealth,if=buff.bs_inc.up|buff.sudden_ambush.up
                    if (IncaBerserk || PlayerHasBuff(SuddenAmbush))
                    {
                        Stealth();
                    }
                    //actions+=/pool_resource,if=talent.bloodtalons.enabled&buff.bloodtalons.down&(energy+3.5*energy.regen+(40*buff.clearcasting.up))<(115-23*buff.incarnation_king_of_the_jungle.up)&active_bt_triggers=0
                    if (BloodtalonsEnergie && !(PlayerHasBuff(Shadowmeld) || PlayerHasBuff(Prowl)))
                    {
                        return;
                    }
                    //actions+=/run_action_list,name=bloodtalons,if=talent.bloodtalons.enabled&buff.bloodtalons.down
                    if (TalentBloodtalons && !PlayerHasBuff(Bloodtalons) && !BloodtalonsEnergie && !PlayerHasBuff(Shadowmeld) && !PlayerHasBuff(Prowl))
                    {
                        Bloodtalonsgenerator();
                    }
                    if (API.PlayerComboPoints <= 4)
                    {
                        Generator();
                    }
                    return;
                }
                return;
            }
        }
        private void Owlweave()
        {
            //actions.owlweave = starsurge,if= buff.heart_of_the_wild.up
            if (API.CanCast(Starsurge) && isInRange && PlayerHasBuff(HeartoftheWild) && PlayerHasBuff(MoonkinForm))
            {
                API.CastSpell(Starsurge);
                return;
            }
            //actions.owlweave +=/ sunfire,if= !prev_gcd.1.sunfire & !prev_gcd.2.sunfire
            if (API.CanCast(Sunfire) && isInRange && PlayerHasBuff(MoonkinForm) && API.TargetDebuffRemainingTime(Sunfire) < 200)
            {
                API.CastSpell(Sunfire);
                return;
            }
            //actions.owlweave +=/ heart_of_the_wild,if= energy < 40 & (dot.rip.remains > 4.5 | combo_points < 5) & cooldown.tigers_fury.remains >= 6.5 & buff.clearcasting.stack < 1 & !buff.apex_predators_craving.up & !buff.bloodlust.up & (buff.bs_inc.remains > 5 | !buff.bs_inc.up) & (!cooldown.convoke_the_spirits.up | !covenant.night_fae)
            if (API.CanCast(HeartoftheWild) && TalentHeartoftheWild && isInRange && IsCooldowns && WeaveConditions)
            {
                API.CastSpell(HeartoftheWild);
                return;
            }
            //actions.owlweave +=/ moonkin_form,if= energy < 40 & (dot.rip.remains > 4.5 | combo_points < 5) & cooldown.tigers_fury.remains >= 6.5 & buff.clearcasting.stack < 1 & !buff.apex_predators_craving.up & !buff.bloodlust.up & (buff.bs_inc.remains > 5 | !buff.bs_inc.up) & (!cooldown.convoke_the_spirits.up | !covenant.night_fae)
            if (API.CanCast(MoonkinForm) && isInRange && (!TargetHasDebuff(Sunfire) || PlayerHasBuff(HeartoftheWild) && API.SpellCDDuration(Starsurge) == 0) && WeaveConditions)
            {
                API.CastSpell(MoonkinForm);
                return;
            }
        }
        private void Defensive()
        {
            if (isInterrupt && API.CanCast(SkullBash) && PlayerLevel >= 26 && isKickRange && (PlayerHasBuff(CatForm) || PlayerHasBuff(BearForm)))
            {
                API.CastSpell(SkullBash);
                return;
            }
            if (PlayerRaceSettings == "Tauren" && API.CanCast(RacialSpell1) && isInterrupt && !API.PlayerIsMoving && isRacial && isMelee && API.SpellISOnCooldown(SkullBash))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast(MightyBash) && isInterrupt && TalentMightyBash && isMelee && API.SpellISOnCooldown(SkullBash))
            {
                API.CastSpell(MightyBash);
                return;
            }
            if (API.PlayerHealthPercent <= RegrowthLifePercent && PlayerLevel >= 3 && API.CanCast(Regrowth) && PlayerHasBuff(PredatorySwiftness))
            {
                API.CastSpell(Regrowth);
                return;
            }
            if (API.CanCast(SurvivalInstincts) && PlayerLevel >= 32 && API.PlayerHealthPercent <= SurvivalInstinctsLifePercent && !PlayerHasBuff(SurvivalInstincts))
            {
                API.CastSpell(SurvivalInstincts);
                return;
            }
            if (API.CanCast(Renewal) && API.PlayerHealthPercent <= RenewalLifePercent && TalentRenewal)
            {
                API.CastSpell(Renewal);
                return;
            }
            if (API.CanCast(Barkskin) && PlayerLevel >= 24 && API.PlayerHealthPercent <= BarkskinLifePercent)
            {
                API.CastSpell(Barkskin);
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
            if (API.PlayerHealthPercent <= FrenziedRegenerationLifePercent && API.PlayerRage >= 10 && API.CanCast(FrenziedRegeneration) && PlayerHasBuff(BearForm) && TalentGuardianAffinity)
            {
                API.CastSpell(FrenziedRegeneration);
                return;
            }
            if (API.PlayerHealthPercent <= IronfurLifePercent && API.PlayerRage >= 40 && API.CanCast(Ironfur) && PlayerHasBuff(BearForm))
            {
                API.CastSpell(Ironfur);
                return;
            }
            if (API.PlayerHealthPercent <= BearFormLifePercent && PlayerLevel >= 8 && IsAutoForm && API.CanCast(BearForm) && !PlayerHasBuff(BearForm))
            {
                API.CastSpell(BearForm);
                return;
            }
            if (API.PlayerHealthPercent >= KittyFormLifePercent && KittyFormLifePercent != 0 && API.CanCast(CatForm) && PlayerHasBuff(BearForm) && IsAutoForm)
            {
                API.CastSpell(CatForm);
                return;
            }
            if (API.CanCast(MassEntanglement) && RootsTorghast && TalentMassEntanglement && !TargetHasDebuff(MassEntanglement) && !TargetHasDebuff(EntanglingRoots) && API.PlayerUnitInMeleeRangeCount > 2 && API.TargetRange < 30)
            {
                API.CastSpell(MassEntanglement);
                return;
            }
            if (API.CanCast(EntanglingRoots) && RootsTorghast && !TargetHasDebuff(MassEntanglement) && !TargetHasDebuff(EntanglingRoots) && PlayerHasBuff(PredatorySwiftness) && API.TargetRange < 35)
            {
                API.CastSpell(EntanglingRoots);
                return;
            }
        }
        private void Generator()
        {
            if ((PlayerHasBuff(Bloodtalons) || !TalentBloodtalons) && !PlayerHasBuff(Shadowmeld) && !PlayerHasBuff(Prowl))
            {
                if (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE)
                {
                    if (isMelee && PlayerLevel >= 7 && API.CanCast(FerociousBite) && PlayerHasBuff(ApexPredatorsCraving))
                    {
                        API.CastSpell(FerociousBite);
                        return;
                    }
                    if (TalentBrutalSlash && API.TargetDebuffRemainingTime(Thrash) > 300 && !PlayerHasBuff(Prowl) && isThrashMelee && API.CanCast(BrutalSlash) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 25 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 20 || PlayerHasBuff(Clearcasting)))
                    {
                        API.CastSpell(BrutalSlash);
                        return;
                    }
                    if (API.CanCast(Rake) && PlayerLevel >= 10 && isMelee && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28) && API.TargetDebuffRemainingTime(Rake) <= 360)
                    {
                        API.CastSpell(Rake);
                        return;
                    }
                    if (API.CanCast(Moonfire) && !PlayerHasBuff(Prowl) && TalentLunarInspiration && API.LastSpellCastInGame != (Moonfire) && (!TargetHasDebuff(Moonfire) || API.TargetDebuffRemainingTime(Moonfire) <= 200) && isInRange && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 30 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 24))
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (isThrashMelee && PlayerLevel >= 11 && !PlayerHasBuff(Prowl) && API.CanCast(Thrash) && (!TargetHasDebuff(Thrash) || API.TargetDebuffRemainingTime(Thrash) <= 300) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                    if (isMelee && PlayerLevel >= 5 && (API.TargetDebuffRemainingTime(Thrash) > 300 || PlayerLevel < 11) && !PlayerHasBuff(Prowl) && (API.TargetDebuffRemainingTime(Rake) > 360 || PlayerLevel < 10) && API.CanCast(Shred) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
                    {
                        API.CastSpell(Shred);
                        return;
                    }
                }
                if (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE)
                {
                    if (isMelee && PlayerLevel >= 7 && API.CanCast(FerociousBite) && PlayerHasBuff(ApexPredatorsCraving))
                    {
                        API.CastSpell(FerociousBite);
                        return;
                    }
                    if (isThrashMelee && !PlayerHasBuff(Prowl) && API.TargetDebuffRemainingTime(Rake) > 360 && TalentBrutalSlash && API.CanCast(BrutalSlash) && (TargetHasDebuff(Thrash) || PlayerLevel < 11) && (PlayerHasBuff(Clearcasting) || !PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 25 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 15))
                    {
                        API.CastSpell(BrutalSlash);
                        return;
                    }
                    if (isThrashMelee && PlayerLevel >= 11 && !PlayerHasBuff(Prowl) && API.CanCast(Thrash) && (!TargetHasDebuff(Thrash) || API.TargetDebuffRemainingTime(Thrash) <= 300) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                    if (isThrashMelee && PlayerLevel >= 11 && !PlayerHasBuff(Prowl) && API.CanCast(Thrash) && TalentScentofBlood && !PlayerHasBuff(ScentofBlood) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                    if (isThrashMelee && PlayerLevel >= 42 && !PlayerHasBuff(Prowl) && TalentScentofBlood && API.CanCast(Swipe) && PlayerHasBuff(ScentofBlood) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Clearcasting) || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28))
                    {
                        API.CastSpell(Swipe);
                        return;
                    }
                    if (API.CanCast(Rake) && PlayerLevel >= 10 && isMelee && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28) && API.TargetDebuffRemainingTime(Rake) <= 360)
                    {
                        API.CastSpell(Rake);
                        return;
                    }
                    if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                    {
                        if (API.MouseoverDebuffRemainingTime(Rake) <= 360 && !API.MacroIsIgnored(Rake + "MO") && PlayerLevel >= 10 && API.CanCast(Rake) && isMOMelee && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28))
                        {
                            API.CastSpell(Rake + "MO");
                            return;
                        }
                        if (API.MouseoverDebuffRemainingTime(Thrash) <= 300 && !API.MacroIsIgnored(Thrash + "MO") && PlayerLevel >= 11 && API.CanCast(Thrash) && isMOThrashMelee && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
                        {
                            API.CastSpell(Thrash + "MO");
                            return;
                        }
                    }
                    if (API.CanCast(Moonfire) && !PlayerHasBuff(Prowl) && TalentLunarInspiration && API.LastSpellCastInGame != (Moonfire) && (!TargetHasDebuff(Moonfire) || API.TargetDebuffRemainingTime(Moonfire) <= 200) && isInRange && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 30 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 24))
                    {
                        API.CastSpell(Moonfire);
                        return;
                    }
                    if (isThrashMelee && PlayerLevel >= 42 && !PlayerHasBuff(Prowl) && !TalentBrutalSlash && API.CanCast(Swipe) && TargetHasDebuff(Thrash) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Clearcasting) || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28))
                    {
                        API.CastSpell(Swipe);
                        return;
                    }
                    if (isThrashMelee && PlayerLevel >= 11 && !PlayerHasBuff(Prowl) && API.CanCast(Thrash) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
                    {
                        API.CastSpell(Thrash);
                        return;
                    }
                }
            }
        }
        private void Finisher()
        {
            //actions.finisher=savage_roar,if=buff.savage_roar.down|buff.savage_roar.remains<(combo_points*6+1)*0.3
            if (TalentSavageRoar && isMelee && API.CanCast(SavageRoar) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 30 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 24) && (!PlayerHasBuff(SavageRoar) || API.PlayerBuffTimeRemaining(SavageRoar) < (API.PlayerComboPoints * 600 + 100) *.3))
            {
                API.CastSpell(SavageRoar);
                return;
            }
            //actions.finisher+=/primal_wrath,if=spell_targets.primal_wrath>2
            if (isMelee && TalentPrimalWrath && API.CanCast(PrimalWrath) && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 20 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 16))
            {
                API.CastSpell(PrimalWrath);
                return;
            }
            //actions.finisher+=/rip,target_if=refreshable&druid.rip.ticks_gained_on_refresh>variable.rip_ticks&((buff.tigers_fury.up|cooldown.tigers_fury.remains>5)&(buff.bloodtalons.up|!talent.bloodtalons.enabled)&dot.rip.pmultiplier<=persistent_multiplier|!talent.sabertooth.enabled)
            if (isMelee && (PlayerHasBuff(Bloodtalons) || !TalentBloodtalons) && (!TalentPrimalWrath || API.PlayerUnitInMeleeRangeCount < AOEUnitNumber) && API.CanCast(Rip) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 20 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 16) && (!TargetHasDebuff(Rip) || (API.TargetDebuffRemainingTime(Rip) + API.PlayerComboPoints * (TalentSabertooth ? 100 : 0)) < 600))
            {
                API.CastSpell(Rip);
                return;
            }
            if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
            {
                if (!API.MacroIsIgnored(Rip + "MO") && isMOMelee && (PlayerHasBuff(Bloodtalons) || !TalentBloodtalons) && (!TalentPrimalWrath || API.PlayerUnitInMeleeRangeCount < AOEUnitNumber) && API.CanCast(Rip) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 20 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 16) && API.MouseoverDebuffRemainingTime(Rip) < 600)
                {
                    API.CastSpell(Rip + "MO");
                    return;
                }
            }
            //actions.finisher+=/ferocious_bite,max_energy=1,target_if=max:time_to_die
            if (isMelee && API.CanCast(FerociousBite) && (!TalentPrimalWrath || API.PlayerUnitInMeleeRangeCount < AOEUnitNumber) && API.PlayerEnergy >= 50)
            {
                API.CastSpell(FerociousBite);
                return;
            }
        }
        private void Bloodtalonsgenerator()
        {
            //actions.bloodtalons=rake,target_if=(!ticking|(refreshable&persistent_multiplier>dot.rake.pmultiplier)|(active_bt_triggers=2&persistent_multiplier>dot.rake.pmultiplier)|(active_bt_triggers=2&refreshable))&buff.bt_rake.down&druid.rake.ticks_gained_on_refresh>=2
            if (API.CanCast(Rake) && API.TargetDebuffRemainingTime(Rake) <= GCD * 2 && !BuffBTRakeUp && isMelee && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28))
            {
                API.CastSpell(Rake);
                return;
            }
            //actions.bloodtalons+=/lunar_inspiration,target_if=refreshable&buff.bt_moonfire.down
            if (API.CanCast(Moonfire) && TalentLunarInspiration && API.TargetDebuffRemainingTime(Moonfire) <= GCD * 2 && !BuffBTMoonfireUp && isInRange && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 30 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 24))
            {
                API.CastSpell(Moonfire);
                return;
            }
            //actions.bloodtalons+=/thrash_cat,target_if=refreshable&buff.bt_thrash.down&druid.thrash_cat.ticks_gained_on_refresh>variable.thrash_ticks
            if (isThrashMelee && !BuffBTThrashUp && !PlayerHasBuff(Shadowmeld) && !PlayerHasBuff(Prowl) && API.CanCast(Thrash) && API.TargetDebuffRemainingTime(Thrash) <= GCD * 2 && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
            {
                API.CastSpell(Thrash);
                return;
            }
            //actions.bloodtalons+=/brutal_slash,if=buff.bt_brutal_slash.down
            if (TalentBrutalSlash && !BuffBTBrutalSlashUp && API.TargetDebuffRemainingTime(Thrash) > GCD * 2 && isThrashMelee && API.CanCast(BrutalSlash) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 25 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 20 || PlayerHasBuff(Clearcasting)))
            {
                API.CastSpell(BrutalSlash);
                return;
            }
            //actions.bloodtalons+=/swipe_cat,if=buff.bt_swipe.down&spell_targets.swipe_cat>1
            if (isThrashMelee && !BuffBTSwipeUp && !TalentBrutalSlash && API.CanCast(Swipe) && API.PlayerUnitInMeleeRangeCount > 1 && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Clearcasting) || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28))
            {
                API.CastSpell(Swipe);
                return;
            }
            //actions.bloodtalons+=/shred,if=buff.bt_shred.down
            if (isMelee && PlayerLevel >= 5 && !BuffBTShredUp && API.CanCast(Shred) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
            {
                API.CastSpell(Shred);
                return;
            }
            //actions.bloodtalons+=/swipe_cat,if=buff.bt_swipe.down
            if (isThrashMelee && !BuffBTSwipeUp && !TalentBrutalSlash && API.CanCast(Swipe) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Clearcasting) || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28))
            {
                API.CastSpell(Swipe);
                return;
            }
            //actions.bloodtalons+=/thrash_cat,if=buff.bt_thrash.down
            if (isThrashMelee && !BuffBTThrashUp && !PlayerHasBuff(Shadowmeld) && !PlayerHasBuff(Prowl) && API.CanCast(Thrash) && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
            {
                API.CastSpell(Thrash);
                return;
            }
        }
        private void Stealth()
        {
            //actions.stealth+=/rake,target_if=(dot.rake.pmultiplier<1.5|refreshable)&druid.rake.ticks_gained_on_refresh>2
            if (API.CanCast(Rake) && PlayerLevel >= 10 && isMelee && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 35 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 28) && API.TargetDebuffRemainingTime(Rake) <= 360)
            {
                API.CastSpell(Rake);
                return;
            }
            //actions.stealth+=/thrash_cat,target_if=refreshable&druid.thrash_cat.ticks_gained_on_refresh>variable.thrash_ticks,if=spell_targets.thrash_cat>3
            if (isThrashMelee && API.CanCast(Thrash) && API.PlayerUnitInMeleeRangeCount > 3 && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
            {
                API.CastSpell(Thrash);
                return;
            }
            //actions.stealth+=/brutal_slash,if=spell_targets.brutal_slash>2
            if (TalentBrutalSlash && isThrashMelee && API.CanCast(BrutalSlash) && API.PlayerUnitInMeleeRangeCount > 2 && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 25 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 20 || PlayerHasBuff(Clearcasting)))
            {
                API.CastSpell(BrutalSlash);
                return;
            }
            //actions.stealth+=/shred,if=combo_points<4
            if (isMelee && API.CanCast(Shred) && API.PlayerComboPoints < 4 && (!PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 40 || PlayerHasBuff(Incarnation) && API.PlayerEnergy >= 32 || PlayerHasBuff(Clearcasting)))
            {
                API.CastSpell(Shred);
                return;
            }
        }
        private void Cooldowns()
        {
            //Cooldowns
            //actions.cooldown=feral_frenzy,if=combo_points<3
            if (isMelee && TalentFeralFrenzy && API.CanCast(FeralFrenzy) && API.PlayerComboPoints < 3 && IsFeralFrenzy)
            {
                API.CastSpell(FeralFrenzy);
                return;
            }
            //actions.cooldown+=/berserk,if=combo_points>=3
            if (!TalentIncarnation && API.PlayerComboPoints >= 3 && API.CanCast(Berserk) && IsBerserk)
            {
                API.CastSpell(Berserk);
                return;
            }
            //actions.cooldown+=/incarnation,if=combo_points>=3
            if (TalentIncarnation && API.PlayerComboPoints >= 3 && API.CanCast(Incarnation) && IsIncarnation)
            {
                API.CastSpell(Incarnation);
                return;
            }
            //actions.cooldown+=/tigers_fury,if=energy.deficit>40|buff.bs_inc.up|(talent.predator.enabled&variable.shortest_ttd<3)
            if (API.CanCast(TigersFury) && isMelee && (EnergyDefecit > 40 || IncaBerserk || TalentPredator && !PlayerHasBuff(TigersFury)))
            {
                API.CastSpell(TigersFury);
                return;
            }
            //actions.cooldown+=/shadowmeld,if=buff.tigers_fury.up&buff.bs_inc.down&combo_points<4&dot.rake.pmultiplier<1.6&energy>40
            if (PlayerRaceSettings == "Night Elf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee && (PlayerHasBuff(TigersFury) && !IncaBerserk && API.PlayerComboPoints < 4 && API.PlayerEnergy > 40))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cooldown+=/berserking,if=buff.tigers_fury.up|buff.bs_inc.up
            if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee && (PlayerHasBuff(TigersFury) || IncaBerserk))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //actions.cooldown+=/ravenous_frenzy,if=buff.bs_inc.up|fight_remains<21
            if (API.CanCast(RavenousFrenzy) && isMelee && IncaBerserk && PlayerCovenantSettings == "Venthyr" && IsCovenant)
            {
                API.CastSpell(RavenousFrenzy);
                return;
            }
            //actions.cooldown+=/convoke_the_spirits,if=(dot.rip.remains>4&combo_points<3&dot.rake.ticking&energy.deficit>=20)|fight_remains<5
            if (API.CanCast(ConvoketheSpirits) && isMelee && !API.PlayerIsMoving && PlayerCovenantSettings == "Night Fae" && IsCovenant && (API.TargetDebuffRemainingTime(Rip) > 400 && API.PlayerComboPoints < 3 && TargetHasDebuff(Rake) && EnergyDefecit >= 20))
            {
                API.CastSpell(ConvoketheSpirits);
                return;
            }
            //actions.cooldown+=/kindred_spirits,if=buff.tigers_fury.up|(conduit.deep_allegiance.enabled)
            if (API.CanCast(LoneEmpowerment) && isMelee && PlayerCovenantSettings == "Kyrian" && IsCovenant && API.PlayerBuffTimeRemaining(TigersFury) >= 900 && PlayerHasBuff(LoneSpirit))
            {
                API.CastSpell(LoneEmpowerment);
                return;
            }
            //actions.cooldown+=/kindred_spirits,if=buff.tigers_fury.up|(conduit.deep_allegiance.enabled)
            if (API.CanCast(EmpowerBond) && isMelee && PlayerCovenantSettings == "Kyrian" && IsCovenant && API.PlayerBuffTimeRemaining(TigersFury) >= 900 && PlayerHasBuff(KindredSpirits))
            {
                API.CastSpell(EmpowerBond);
                return;
            }
            //actions.cooldown+=/adaptive_swarm,target_if=max:time_to_die*(combo_points=5&!dot.adaptive_swarm_damage.ticking)
            if (API.CanCast(AdaptiveSwarm) && isMelee && PlayerCovenantSettings == "Necrolord" && IsCovenant && (API.PlayerComboPoints == 5 && !TargetHasDebuff(AdaptiveSwarm)))
            {
                API.CastSpell(AdaptiveSwarm);
                return;
            }
            //actions.cooldown+=/use_items
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
            {
                API.CastSpell("Trinket1");
                return;
            }
            //actions.cooldown+=/use_items
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
            {
                API.CastSpell("Trinket2");
                return;
            }
        }
    }
}