// Changelog
// v1.0 First release
// v1.1 frenzied regeneration fix
// v1.2 covenants added + cd managment
// v1.3 covenant update :-)
// v1.4 Racials and Trinkets
// v1.5 Mighty Bash added
// v1.6 spell ids and alot of other stuff
// v1.7 new simc apl
// v1.8 mouseover moonfire added
// v1.9 racials and a few other fixes
// v2.0 Growl added for torghast anima power
// v2.1 convoke update
// v2.2 convoke/berserk fix
// v2.3 auto break roots
// v2.4 Thrash spam with leggy
// v2.45 explosive check

namespace HyperElk.Core
{
    public class GuardianDruid : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        //Spell,Auras
        private string Moonfire = "Moonfire";
        private string Swipe = "Swipe";
        private string Thrash = "Thrash";
        private string Mangle = "Mangle";
        private string Maul = "Maul";
        private string Ironfur = "Ironfur";
        private string BearForm = "Bear Form";
        private string TravelForm = "Travel Form";
        private string Barkskin = "Barkskin";
        private string SurvivalInstincts = "Survival Instincts";
        private string FrenziedRegeneration = "Frenzied Regeneration";
        private string BristlingFur = "Bristling Fur";
        private string Pulverize = "Pulverize";
        private string Incarnation = "Incarnation: Guardian of Ursoc";
        private string SkullBash = "Skull Bash";
        private string StampedingRoar = "Stampeding Roar";
        private string Typhoon = "Typhoon";
        private string GalacticGuardian = "Galactic Guardian";
        private string CatForm = "Cat Form";
        private string ToothandClaw = "Tooth and Claw";
        private string Berserk = "Berserk";
        private string Renewal = "Renewal";
        private string RavenousFrenzy = "Ravenous Frenzy";
        private string ConvoketheSpirits = "Convoke the Spirits";
        private string AdaptiveSwarm = "Adaptive Swarm";
        private string LoneProtection = "Lone Protection";
        private string LoneSpirit = "Lone Spirit";
        private string Soulshape = "Soulshape";
        private string MightyBash = "Mighty Bash";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string SavageCombatant = "Savage Combatant";
        private string Growl = "Growl";


        //Talents
        bool TalentBristlingFur => API.PlayerIsTalentSelected(1, 3);
        bool TalentRenewal => API.PlayerIsTalentSelected(2, 2);
        bool TalentBalanceAffinity => API.PlayerIsTalentSelected(3, 1);
        bool TalentMightyBash => API.PlayerIsTalentSelected(4, 1);
        bool TalentSouloftheForest => API.PlayerIsTalentSelected(5, 1);
        bool TalentGalacticGuardian => API.PlayerIsTalentSelected(5, 2);
        bool TalentIncarnation => API.PlayerIsTalentSelected(5, 3);
        bool TalentPulverize => API.PlayerIsTalentSelected(7, 3);
        bool TalentRendandTear => API.PlayerIsTalentSelected(7, 1);

        //General
        private bool isExplosive => API.TargetMaxHealth <= 600 && API.TargetMaxHealth != 0;
        private int PlayerLevel => API.PlayerLevel;
        private bool isMelee => (TalentBalanceAffinity && API.TargetRange < 9 || !TalentBalanceAffinity && API.TargetRange < 6);

        private bool isThrashMelee => (TalentBalanceAffinity && API.TargetRange < 12 || !TalentBalanceAffinity && API.TargetRange < 9);

        private bool isKickRange => (TalentBalanceAffinity && API.TargetRange < 17 || !TalentBalanceAffinity && API.TargetRange < 14);
        private bool isMOinRange => (TalentBalanceAffinity && API.TargetRange < 43 || !TalentBalanceAffinity && API.TargetRange < 40);
        float GCD => API.SpellGCDTotalDuration;
        private bool IncaBerserk => (API.PlayerHasBuff(Incarnation) || API.PlayerHasBuff(Berserk));
        bool IsBerserk => (UseBerserk == "with Cooldowns" && IsCooldowns || UseBerserk == "always");
        bool IsIncarnation => (UseIncarnation == "with Cooldowns" && IsCooldowns || UseIncarnation == "always");
        bool IsCovenant => (UseCovenant == "with Cooldowns" && IsCooldowns || UseCovenant == "always" || UseCovenant == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE);
        bool IsTrinkets1 => (UseTrinket1 == "with Cooldowns" && IsCooldowns || UseTrinket1 == "always" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isMelee;
        bool IsTrinkets2 => (UseTrinket2 == "with Cooldowns" && IsCooldowns || UseTrinket2 == "always" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && isMelee;

        //CBProperties
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "always" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseIncarnation => CDUsage[CombatRoutine.GetPropertyInt(Incarnation)];
        private string UseBerserk => CDUsage[CombatRoutine.GetPropertyInt(Berserk)];
        private bool AutoForm => CombatRoutine.GetPropertyBool("AutoForm");
        private bool ThrashSpam => CombatRoutine.GetPropertyBool("Thrashspam");
        private bool IsGrowl => CombatRoutine.GetPropertyBool("Growl");
        private bool AutoTravelForm => CombatRoutine.GetPropertyBool("AutoTravelForm");
        private int BarkskinLifePercent => numbList[CombatRoutine.GetPropertyInt(Barkskin)];
        private int RenewalLifePercent => numbList[CombatRoutine.GetPropertyInt(Renewal)];
        private int SurvivalInstinctsLifePercent => numbList[CombatRoutine.GetPropertyInt(SurvivalInstincts)];
        private int FrenziedRegenerationLifePercent => numbList[CombatRoutine.GetPropertyInt(FrenziedRegeneration)];
        private int IronfurLifePercent => numbList[CombatRoutine.GetPropertyInt(Ironfur)];
        private int BristlingFurLifePercent => numbList[CombatRoutine.GetPropertyInt(BristlingFur)];
        private int PulverizeLifePercent => numbList[CombatRoutine.GetPropertyInt(Pulverize)];
        private int LoneProtectionLifePercent => numbList[CombatRoutine.GetPropertyInt(LoneProtection)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        public bool rootbreaker => CombatRoutine.GetPropertyBool("Rootbreaker");
        public override void Initialize()
        {
            CombatRoutine.Name = "Guardian Druid by smartie";
            API.WriteLog("Welcome to smartie`s Guardian Druid v2.45");

            //Spells
            CombatRoutine.AddSpell(Moonfire, 8921, "D3");
            CombatRoutine.AddSpell(Swipe, 213771, "D5");
            CombatRoutine.AddSpell(Thrash, 77758, "D4");
            CombatRoutine.AddSpell(Renewal, 108238, "NumPad4");
            CombatRoutine.AddSpell(Mangle, 33917, "D6");
            CombatRoutine.AddSpell(Maul, 6807, "D7");
            CombatRoutine.AddSpell(Ironfur, 192081, "F6");
            CombatRoutine.AddSpell(BearForm, 5487, "NumPad1");
            CombatRoutine.AddSpell(TravelForm, 783, "NumPad2");
            CombatRoutine.AddSpell(Barkskin, 22812, "F4");
            CombatRoutine.AddSpell(SurvivalInstincts, 61336, "F1");
            CombatRoutine.AddSpell(FrenziedRegeneration, 22842, "F5");
            CombatRoutine.AddSpell(BristlingFur, 155835, "D0");
            CombatRoutine.AddSpell(Pulverize, 80313, "D9");
            CombatRoutine.AddSpell(Incarnation, 102558, "D8");
            CombatRoutine.AddSpell(Berserk, 50334, "D8");
            CombatRoutine.AddSpell(SkullBash, 106839, "F12");
            CombatRoutine.AddSpell(StampedingRoar, 106898, "NumPad5");
            CombatRoutine.AddSpell(Typhoon, 132469, "F8");
            CombatRoutine.AddSpell(MightyBash, 5211, "D1");
            CombatRoutine.AddSpell(RavenousFrenzy, 323546, "D1");
            CombatRoutine.AddSpell(ConvoketheSpirits, 323764, "D1");
            CombatRoutine.AddSpell(AdaptiveSwarm, 325727, "D1");
            CombatRoutine.AddSpell(LoneProtection, 338018, "D1");
            CombatRoutine.AddSpell(Growl, 6795, "D1");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

            //Buffs
            CombatRoutine.AddBuff(GalacticGuardian, 213708);
            CombatRoutine.AddBuff(CatForm, 768);
            CombatRoutine.AddBuff(BearForm, 5487);
            CombatRoutine.AddBuff(TravelForm, 783);
            CombatRoutine.AddBuff(SurvivalInstincts, 61336);
            CombatRoutine.AddBuff(Barkskin, 22812);
            CombatRoutine.AddBuff(Ironfur, 192081);
            CombatRoutine.AddBuff(FrenziedRegeneration, 22842);
            CombatRoutine.AddBuff(Incarnation, 102558);
            CombatRoutine.AddBuff(Berserk, 50334);
            CombatRoutine.AddBuff(BristlingFur, 155835);
            CombatRoutine.AddBuff(Pulverize, 158792);
            CombatRoutine.AddBuff(ToothandClaw, 135286);
            CombatRoutine.AddBuff(RavenousFrenzy, 323546);
            CombatRoutine.AddBuff(LoneSpirit, 338041);
            CombatRoutine.AddBuff(Soulshape, 310143);
            CombatRoutine.AddBuff(SavageCombatant, 340613);

            //Debuff
            CombatRoutine.AddDebuff(Thrash, 192090);
            CombatRoutine.AddDebuff(Moonfire, 164812);
            CombatRoutine.AddDebuff(AdaptiveSwarm, 325727);
            CombatRoutine.AddDebuff("Frozen Binds", 320788);

            //Macros
            CombatRoutine.AddMacro(Moonfire + "MO", "NumPad7");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");

            //Prop
            CombatRoutine.AddProp("Rootbreaker", "Break roots", false, "Break roots with shapeshift", "Generic");
            CombatRoutine.AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Incarnation, "Use " + Incarnation, CDUsage, "Use " + Incarnation + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Berserk, "Use " + Berserk, CDUsage, "Use " + Berserk + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("ThrashSpam", "Spam Thrash while cds", false, " Rota will spam Thrash for the Legendary while cds are up", "Generic");
            CombatRoutine.AddProp("AutoForm", "AutoForm", true, "Will auto switch forms", "Generic");
            CombatRoutine.AddProp("Growl", "Use Growl", false, "Torghast Anima Power :-)", "Torghast");
            CombatRoutine.AddProp("AutoTravelForm", "AutoTravelForm", false, "Will auto switch to Travel Form Out of Fight and outside", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(Barkskin, Barkskin + " Life Percent", numbList, "Life percent at which" + Barkskin + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp(Renewal, Renewal + " Life Percent", numbList, "Life percent at which" + Renewal + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SurvivalInstincts, SurvivalInstincts + " Life Percent", numbList, "Life percent at which" + SurvivalInstincts + "is used, set to 0 to disable", "Defense", 30);
            CombatRoutine.AddProp(FrenziedRegeneration, FrenziedRegeneration + " Life Percent", numbList, "Life percent at which" + FrenziedRegeneration + "is used, set to 0 to disable", "Defense", 60);
            CombatRoutine.AddProp(Ironfur, Ironfur + " Life Percent", numbList, "Life percent at which" + Ironfur + "is used, set to 0 to disable", "Defense", 90);
            CombatRoutine.AddProp(BristlingFur, BristlingFur + " Life Percent", numbList, "Life percent at which" + BristlingFur + "is used, set to 0 to disable", "Defense", 50);
            CombatRoutine.AddProp(Pulverize, Pulverize + " Life Percent", numbList, "Life percent at which" + Pulverize + "is used, set to 0 to disable", "Defense", 50);
            CombatRoutine.AddProp(LoneProtection, LoneProtection + " Life Percent", numbList, "Life percent at which" + LoneProtection + "is used, set to 0 to disable", "Defense", 50);
        }
        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            //API.WriteLog("Targets: "+ API.PlayerUnitInMeleeRangeCount);
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            if (!API.PlayerIsMounted && !API.PlayerHasBuff(TravelForm))
            {
                if (API.CanCast(Growl) && IsGrowl && API.PlayerHasBuff(BearForm) && isKickRange)
                {
                    API.CastSpell(Growl);
                    return;
                }
                if (isInterrupt && API.CanCast(SkullBash) && PlayerLevel >= 26 && isKickRange)
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
                if (API.PlayerHealthPercent <= BarkskinLifePercent && PlayerLevel >= 24 && API.CanCast(Barkskin))
                {
                    API.CastSpell(Barkskin);
                    return;
                }
                if (API.CanCast(SurvivalInstincts) && PlayerLevel >= 32 && API.PlayerHealthPercent <= SurvivalInstinctsLifePercent && !API.PlayerHasBuff(SurvivalInstincts))
                {
                    API.CastSpell(SurvivalInstincts);
                    return;
                }
                if (API.CanCast(Renewal) && API.PlayerHealthPercent <= RenewalLifePercent && TalentRenewal)
                {
                    API.CastSpell(Renewal);
                    return;
                }
                if (API.PlayerHealthPercent <= FrenziedRegenerationLifePercent && PlayerLevel >= 21 && API.PlayerRage >= 10 && API.CanCast(FrenziedRegeneration) && API.PlayerHasBuff(BearForm) && API.PlayerBuffTimeRemaining(FrenziedRegeneration) == 0)
                {
                    API.CastSpell(FrenziedRegeneration);
                    return;
                }
                if (API.PlayerHealthPercent <= IronfurLifePercent && PlayerLevel >= 18 && (API.PlayerRage >= 40 || API.PlayerRage >= 20 && IncaBerserk) && API.CanCast(Ironfur) && API.PlayerHasBuff(BearForm))
                {
                    API.CastSpell(Ironfur);
                    return;
                }
                if (API.PlayerHealthPercent <= BristlingFurLifePercent && PlayerLevel >= 8 && API.CanCast(BristlingFur) && API.PlayerHasBuff(BearForm) && TalentBristlingFur)
                {
                    API.CastSpell(BristlingFur);
                    return;
                }
                if (API.PlayerHealthPercent <= PulverizeLifePercent && API.CanCast(Pulverize) && API.PlayerHasBuff(BearForm) && TalentPulverize && API.TargetBuffStacks(Thrash) >= 2)
                {
                    API.CastSpell(Pulverize);
                    return;
                }
                if (API.PlayerHealthPercent <= LoneProtectionLifePercent && API.CanCast(LoneProtection) && isMelee && API.PlayerHasBuff(LoneSpirit) && PlayerCovenantSettings == "Kyrian")
                {
                    API.CastSpell(LoneProtection);
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
                if (API.CanCast(BearForm) && API.PlayerIsCC(CCList.ROOT) && !API.PlayerHasDebuff("Frozen Binds") && AutoForm && rootbreaker)
                {
                    API.CastSpell(BearForm);
                    return;
                }
                rotation();
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            if (API.CanCast(TravelForm) && AutoTravelForm && API.PlayerIsOutdoor && !API.PlayerHasBuff(TravelForm))
            {
                API.CastSpell(TravelForm);
                return;
            }
        }
        private void rotation()
        {
            if ((!API.PlayerHasBuff(BearForm) && PlayerLevel >= 8) && !API.PlayerHasBuff(CatForm) && !API.PlayerHasBuff(Soulshape) && AutoForm)
            {
                API.CastSpell(BearForm);
                return;
            }
            if (PlayerLevel < 8)
            {
                API.WriteLog("Your Current Level is:" + PlayerLevel);
                API.WriteLog("Rota will work once you are Level 8");
            }
            if (API.PlayerHasBuff(BearForm) && PlayerLevel >= 8)
            {
                //actions.cooldown +=/ berserking,if= buff.tigers_fury.up | buff.bs_inc.up
                if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && isMelee && IncaBerserk)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && !isExplosive && IsTrinkets1)
                {
                    API.CastSpell("Trinket1");
                    return;
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && !isExplosive && IsTrinkets2)
                {
                    API.CastSpell("Trinket2");
                    return;
                }
                //actions.bear+=/ravenous_frenzy
                if (API.CanCast(RavenousFrenzy) && isMelee && PlayerCovenantSettings == "Venthyr" && !isExplosive && IsCovenant && IncaBerserk)
                {
                    API.CastSpell(RavenousFrenzy);
                    return;
                }
                //actions.bear+=/convoke_the_spirits,if=!druid.catweave_bear&!druid.owlweave_bear
                if (API.CanCast(ConvoketheSpirits) && isMelee && PlayerCovenantSettings == "Night Fae" && !isExplosive && IsCovenant)
                {
                    API.CastSpell(ConvoketheSpirits);
                    return;
                }
                //actions.bear+=/berserk_bear,if=(buff.ravenous_frenzy.up|!covenant.venthyr)
                if (API.CanCast(Berserk) && !TalentIncarnation && (API.SpellGCDDuration == 0 && PlayerCovenantSettings == "Night Fae" && IsCovenant && API.SpellCDDuration(ConvoketheSpirits) <= GCD || PlayerCovenantSettings != "Night Fae" || !IsCovenant || API.SpellCDDuration(ConvoketheSpirits) > GCD*2) && isMelee && IsBerserk)
                {
                    API.CastSpell(Berserk);
                    return;
                }
                //actions.bear+=/incarnation,if=(buff.ravenous_frenzy.up|!covenant.venthyr)
                if (API.CanCast(Incarnation) && TalentIncarnation && (API.SpellGCDDuration == 0 && PlayerCovenantSettings == "Night Fae" && IsCovenant && API.SpellCDDuration(ConvoketheSpirits) <= GCD || PlayerCovenantSettings != "Night Fae" || !IsCovenant || API.SpellCDDuration(ConvoketheSpirits) > GCD * 2) && isMelee && IsIncarnation)
                {
                    API.CastSpell(Incarnation);
                    return;
                }
                //actions.bear+=/adaptive_swarm,if=(!dot.adaptive_swarm_damage.ticking&!action.adaptive_swarm_damage.in_flight&(!dot.adaptive_swarm_heal.ticking|dot.adaptive_swarm_heal.remains>3)|dot.adaptive_swarm_damage.stack<3&dot.adaptive_swarm_damage.remains<5&dot.adaptive_swarm_damage.ticking)
                if (API.CanCast(AdaptiveSwarm) && isMelee && !isExplosive && PlayerCovenantSettings == "Necrolord" && IsCovenant && !API.TargetHasDebuff(AdaptiveSwarm))
                {
                    API.CastSpell(AdaptiveSwarm);
                    return;
                }
                if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
                {
                    if (API.MouseoverDebuffRemainingTime(Moonfire) <= 300 && !API.MacroIsIgnored(Moonfire + "MO") && API.CanCast(Moonfire) && isMOinRange)
                    {
                        API.CastSpell(Moonfire + "MO");
                        return;
                    }
                }
                if (API.CanCast(Thrash) && PlayerLevel >= 11 && isThrashMelee && ThrashSpam && IncaBerserk)
                {
                    API.CastSpell(Thrash);
                    return;
                }
                //actions.bear+=/thrash_bear,target_if=refreshable|dot.thrash_bear.stack<3|(dot.thrash_bear.stack<4&runeforge.luffainfused_embrace.equipped)|active_enemies>=4
                if (API.CanCast(Thrash) && PlayerLevel >= 11 && isThrashMelee && (API.TargetDebuffRemainingTime(Thrash) < 250 || API.TargetDebuffStacks(Thrash) < 3 || API.PlayerUnitInMeleeRangeCount >= 4))
                {
                    API.CastSpell(Thrash);
                    return;
                }
                //actions.bear+=/moonfire,if=((buff.galactic_guardian.up)&active_enemies<2)|((buff.galactic_guardian.up)&!dot.moonfire.ticking&active_enemies>1&target.time_to_die>12)
                if (API.CanCast(Moonfire) && API.TargetRange < 40 && ((API.PlayerHasBuff(GalacticGuardian) && API.PlayerUnitInMeleeRangeCount < 2) || API.PlayerHasBuff(GalacticGuardian) && !API.TargetHasDebuff(Moonfire) && API.PlayerUnitInMeleeRangeCount > 1))
                {
                    API.CastSpell(Moonfire);
                    return;
                }
                //actions.bear+=/moonfire,if=(dot.moonfire.remains<=3&(buff.galactic_guardian.up)&active_enemies>5&target.time_to_die>12)
                if (API.CanCast(Moonfire) && API.TargetRange < 40 && (API.TargetDebuffRemainingTime(Moonfire) <= 300 && API.PlayerHasBuff(GalacticGuardian) && API.PlayerUnitInMeleeRangeCount > 5))
                {
                    API.CastSpell(Moonfire);
                    return;
                }
                //actions.bear+=/moonfire,if=(refreshable&active_enemies<2&target.time_to_die>12)|(!dot.moonfire.ticking&active_enemies>1&target.time_to_die>12)
                if (API.CanCast(Moonfire) && API.TargetRange < 40 && (API.TargetDebuffRemainingTime(Moonfire) <= 150 && API.PlayerUnitInMeleeRangeCount < 2 || !API.TargetHasDebuff(Moonfire) && API.PlayerUnitInMeleeRangeCount > 1))
                {
                    API.CastSpell(Moonfire);
                    return;
                }
                //actions.bear+=/swipe,if=buff.incarnation_guardian_of_ursoc.down&buff.berserk_bear.down&active_enemies>=4
                if (API.CanCast(Swipe) && PlayerLevel >= 36 && isThrashMelee && !IncaBerserk && API.PlayerUnitInMeleeRangeCount >= 4)
                {
                    API.CastSpell(Swipe);
                    return;
                }
                //actions.bear+=/maul,if=buff.incarnation.up&active_enemies<2
                if (API.CanCast(Maul) && PlayerLevel >= 10 && API.PlayerHasBuff(Incarnation) && API.PlayerUnitInMeleeRangeCount < 2 && API.PlayerRage >= 40 && isMelee)
                {
                    API.CastSpell(Maul);
                    return;
                }
                //actions.bear+=/maul,if=(buff.savage_combatant.stack>=1)&(buff.tooth_and_claw.up)&buff.incarnation.up&active_enemies=2
                if (API.CanCast(Maul) && PlayerLevel >= 10 && API.PlayerBuffStacks(SavageCombatant) >= 1 && API.PlayerHasBuff(ToothandClaw) && API.PlayerHasBuff(Incarnation) && API.PlayerUnitInMeleeRangeCount == 2 && API.PlayerRage >= 40 && isMelee)
                {
                    API.CastSpell(Maul);
                    return;
                }
                //actions.bear+=/mangle,if=buff.incarnation.up&active_enemies<=3
                if (API.CanCast(Mangle) && PlayerLevel >= 8 && API.PlayerHasBuff(Incarnation) && API.PlayerUnitInMeleeRangeCount <= 3 && isMelee)
                {
                    API.CastSpell(Mangle);
                    return;
                }
                //actions.bear+=/maul,if=(((buff.tooth_and_claw.stack>=2)|(buff.tooth_and_claw.up&buff.tooth_and_claw.remains<1.5)|(buff.savage_combatant.stack>=3))&active_enemies<3)
                if (API.CanCast(Maul) && PlayerLevel >= 10 && API.PlayerRage >= 40 && isMelee && (((API.PlayerBuffStacks(ToothandClaw) >= 2) || (API.PlayerHasBuff(ToothandClaw) && API.PlayerBuffTimeRemaining(ToothandClaw) <= 150) || (API.PlayerBuffStacks(SavageCombatant) >= 3)) && API.PlayerUnitInMeleeRangeCount <3))
                {
                    API.CastSpell(Maul);
                    return;
                }
                //actions.bear +=/ thrash_bear,if= active_enemies > 1
                if (API.CanCast(Thrash) && PlayerLevel >= 11 && isThrashMelee && API.PlayerUnitInMeleeRangeCount > 1)
                {
                    API.CastSpell(Thrash);
                    return;
                }
                //actions.bear+=/mangle,if=((rage<90)&active_enemies<3)|((rage<85)&active_enemies<3&talent.soul_of_the_forest.enabled)
                if (API.CanCast(Mangle) && PlayerLevel >= 8 && isMelee && (API.PlayerRage < 90 && API.PlayerUnitInMeleeRangeCount < 3 && !TalentSouloftheForest || API.PlayerRage < 85 && API.PlayerUnitInMeleeRangeCount < 3 && TalentSouloftheForest))
                {
                    API.CastSpell(Mangle);
                    return;
                }
                //actions.bear+=/thrash_bear
                if (API.CanCast(Thrash) && PlayerLevel >= 11 && isThrashMelee)
                {
                    API.CastSpell(Thrash);
                    return;
                }
                //actions.bear+=/maul,if=active_enemies<3
                if (API.CanCast(Maul) && PlayerLevel >= 10 && API.PlayerRage >= 40 && isMelee && API.PlayerUnitInMeleeRangeCount < 3)
                {
                    API.CastSpell(Maul);
                    return;
                }
                //actions.bear+=/swipe_bear
                if (API.CanCast(Swipe) && PlayerLevel >= 36 && isThrashMelee)
                {
                    API.CastSpell(Swipe);
                    return;
                }
            }
        }
    }
}
