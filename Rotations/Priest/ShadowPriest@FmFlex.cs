namespace HyperElk.Core
{
    public class LevelingPriest : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");

        //Speel,Auras
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
        private string Trincket1 = "Trincket 1";
        private string Trincket2 = "Trincket 2";
        private string BoonOfTheAscended = "Boon of the Ascended";

        //Talents
        bool TalentTwistOfFate => API.PlayerIsTalentSelected(3, 1);
        bool TalentMisery => API.PlayerIsTalentSelected(3, 2);
        bool TalentSearingNightmare => API.PlayerIsTalentSelected(3, 3);
        bool TalentPsychicLink => API.PlayerIsTalentSelected(5, 2);
        bool TalentDamnation => API.PlayerIsTalentSelected(6, 1);
        bool TalentMindbender => API.PlayerIsTalentSelected(6, 2);
        bool TalentVoidTorrent => API.PlayerIsTalentSelected(6, 3);
        bool TalentHungeringVoid => API.PlayerIsTalentSelected(7, 2);
        bool TalentSurrenderToMadness => API.PlayerIsTalentSelected(7, 3);
        bool TalentShadowCrash => API.PlayerIsTalentSelected(5, 3);

        //CBProperties
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool IsShieldSpeed => CombatRoutine.GetPropertyBool("SHIELDSPEED");
        private bool IsUseVamp => (bool)CombatRoutine.GetProperty("UseVampiric");
        private bool IsSWDeathMoving => (bool)CombatRoutine.GetProperty(SWDeath);
        private int PWShieldLifePercent => percentListProp[CombatRoutine.GetPropertyInt(PWShield)];
        private int ShadowMendLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ShadowMend)];
        private int ShadowMendOOCLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ShadowMend + "OOC")];
        private int VampiricEmbraceLifePercent => percentListProp[CombatRoutine.GetPropertyInt(VampiricEmbrace)];
        private int DesperatePrayerLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DesperatePrayer)];
        public new string[] CDUsage = new string[] { "Never", "With Cooldowns", "Always" };
        private int Trinket1Usage => CombatRoutine.GetPropertyInt(Trincket1);
        private int Trinket2Usage => CombatRoutine.GetPropertyInt(Trincket2);

        //General
        private int PlayerLevel => API.PlayerLevel;
        bool ChannelingMindFlay => API.CurrentCastSpellID("player") == 15407;
        bool ChannelingMindSear => API.CurrentCastSpellID("player") == 48045;
        bool CastingVT => API.CurrentCastSpellID("player") == 34914;
        //actions+=/variable,name=dots_up,op=set,value=dot.shadow_word_pain.ticking&dot.vampiric_touch.ticking
        bool dots_up => API.TargetHasDebuff(SWPain, true) && API.TargetHasDebuff(VampiricTouch, true);
        //actions+=/variable,name=all_dots_up,op=set,value=dot.shadow_word_pain.ticking&dot.vampiric_touch.ticking&dot.devouring_plague.ticking
        bool all_dots_up => dots_up && API.TargetHasDebuff(DevouringPlague, true);
        //actions+=/variable,name=searing_nightmare_cutoff,op=set,value=spell_targets.mind_sear>3
        bool searing_nightmare_cutoff => API.TargetUnitInRangeCount > 3;
        //actions+=/variable,name=pi_or_vf_sync_condition,op=set,value=(priest.self_power_infusion|runeforge.twins_of_the_sun_priestess.equipped)&level>=58&cooldown.power_infusion.up|(level<58|!priest.self_power_infusion&!runeforge.twins_of_the_sun_priestess.equipped)&cooldown.void_eruption.up
        bool pi_or_vf_sync_condition => (PlayerLevel >= 58 && (API.CanCast(PowerInfusion)|| API.PlayerHasBuff(PowerInfusion)) && API.CanCast(VoidEruption)) || (PlayerLevel < 58 && PlayerLevel >= 23 && API.CanCast(VoidEruption));

        public override void Initialize()
        {
            CombatRoutine.Name = "Shadow Priest Rotation @FmFlex";
            API.WriteLog("Welcome to Shadow Priest rotation @ FmFlex");
            API.WriteLog("Create the following mouseover macro and assigned to the bind:");
            API.WriteLog("Shadow Word: PainMO - /cast [@mouseover] Shadow Word: Pain");
            API.WriteLog("VampiricTouchMO - /cast [@mouseover] Vampiric Touch");

            //Buff
            CombatRoutine.AddBuff(Shadowform);
            CombatRoutine.AddBuff(PWFortitude);
            CombatRoutine.AddBuff(Voidform);
            CombatRoutine.AddBuff(PWShield);
            CombatRoutine.AddBuff(DarkThoughts);
            CombatRoutine.AddBuff(UnfurlingDarkness);
            CombatRoutine.AddBuff(BoonOfTheAscended);
            CombatRoutine.AddBuff(PowerInfusion);
            //Debuff
            CombatRoutine.AddDebuff(DevouringPlague);
            CombatRoutine.AddDebuff(SWPain);
            CombatRoutine.AddDebuff(WeakenedSoul);
            CombatRoutine.AddDebuff(VampiricTouch);
            CombatRoutine.AddDebuff(ShadowCrash);

            //Spell
            CombatRoutine.AddSpell(MindFlay, "D1");
            CombatRoutine.AddSpell(SWPain, "D2");
            CombatRoutine.AddSpell(AscendedBlast, "R");
            CombatRoutine.AddSpell(BoonOfTheAscended, "D2");
            CombatRoutine.AddSpell(MindBlast, "D3");
            CombatRoutine.AddSpell(ShadowMend, "Q");
            CombatRoutine.AddSpell(DevouringPlague, "D4");
            CombatRoutine.AddSpell(Shadowform, "F5");
            CombatRoutine.AddSpell(SWDeath, "D5");
            CombatRoutine.AddSpell(VampiricTouch, "D6");
            CombatRoutine.AddSpell(Shadowfiend, "D7");
            CombatRoutine.AddSpell(VoidEruption, "D8");
            CombatRoutine.AddSpell(VoidBolt, "D8");
            CombatRoutine.AddSpell(MindSear, "D0");
            CombatRoutine.AddSpell(SearingNightmare, "D1");
            CombatRoutine.AddSpell(ShadowCrash, "D1");
            CombatRoutine.AddSpell(VoidTorrent, "D1");
            CombatRoutine.AddSpell(SurrendertoMadness, "D1");
            CombatRoutine.AddSpell(Damnation, "D1");
            CombatRoutine.AddSpell(Silence, "F");
            CombatRoutine.AddSpell(Mindbender, "D7");
            CombatRoutine.AddSpell(PowerInfusion, "0");
            CombatRoutine.AddSpell(VampiricEmbrace, "E");
            CombatRoutine.AddSpell(DesperatePrayer, "S");
            CombatRoutine.AddSpell(PWFortitude, "F6");
            CombatRoutine.AddSpell(PWShield, "F7");
            CombatRoutine.AddSpell(Mindgames, "0");
            CombatRoutine.AddMacro(SWPain + "MO", "D2");
            CombatRoutine.AddMacro(VampiricTouch + "MO", "D6");
            CombatRoutine.AddMacro(Trincket1);
            CombatRoutine.AddMacro(Trincket2);
            CombatRoutine.AddToggle("Mouseover");

            //Prop
            CombatRoutine.AddProp("SHIELDSPEED", "Use PWS for speed", true, "Use powerword shield for speed boost", "Generic");
            CombatRoutine.AddProp("UseVampiric", "Use Vampiric", true, "Should the rotation use Vampiric Aura", "Generic");
            CombatRoutine.AddProp(SWDeath, "Use SW:Death when moving", true, "Should the rotation use " + SWDeath + " when moving even above 20% life", "Generic");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp(PWShield, PWShield + " Life Percent", percentListProp, "Life percent at which " + PWShield + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(ShadowMend, ShadowMend + " Life Percent", percentListProp, "Life percent at which " + ShadowMend + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(ShadowMend + "OOC", ShadowMend + " Life Percent OOC", percentListProp, "Life percent at which " + ShadowMend + " is used out of combat, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(VampiricEmbrace, VampiricEmbrace + " Life Percent", percentListProp, "Life percent at which " + VampiricEmbrace + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(DesperatePrayer, DesperatePrayer + " Life Percent", percentListProp, "Life percent at which " + DesperatePrayer + " is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(Trincket1, "Trinket 1 usage", CDUsage, "When should Trinket 1 be used", "Trinket", 0);
            CombatRoutine.AddProp(Trincket2, "Trinket 2 usage", CDUsage, "When should Trinket 2 be used", "Trinket", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsage, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted)
            {
                if (PlayerLevel >= 12 && API.CanCast(Shadowform) && !API.PlayerHasBuff(Shadowform) && !API.PlayerHasBuff(Voidform))
                {
                    API.CastSpell(Shadowform);
                    return;
                }
                if (PlayerLevel >= 6 && API.CanCast(PWFortitude) && API.PlayerBuffTimeRemaining(PWFortitude) < 30000)
                {
                    API.CastSpell(PWFortitude);
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.PlayerIsCasting(true) && !ChannelingMindFlay && !ChannelingMindSear)
                return;
            if (isInterrupt && API.CanCast(Silence) && PlayerLevel >= 27)
            {
                API.CastSpell(Silence);
                return;
            }
            if (API.PlayerHealthPercent <= PWShieldLifePercent && API.CanCast(PWShield) && !API.PlayerHasBuff(PWShield, false, false) && !API.PlayerHasDebuff(WeakenedSoul, false, false))
            {
                API.CastSpell(PWShield);
                return;
            }
            if (API.PlayerHealthPercent <= DesperatePrayerLifePercent && API.CanCast(DesperatePrayer))
            {
                API.CastSpell(DesperatePrayer);
                return;
            }
            if (API.PlayerHealthPercent <= VampiricEmbraceLifePercent && API.CanCast(VampiricEmbrace))
            {
                API.CastSpell(VampiricEmbrace);
                return;
            }
            if (API.PlayerHealthPercent <= ShadowMendLifePercent && !API.PlayerIsMoving && API.CanCast(ShadowMend) && API.SpellIsCanbeCast(ShadowMend))
            {
                API.CastSpell(ShadowMend);
                return;
            }
            if (((Trinket1Usage == 1 && IsCooldowns) || Trinket1Usage == 2) && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
            {
                API.CastSpell(Trincket1);
                return;
            }
            if (((Trinket2Usage == 1 && IsCooldowns) || Trinket2Usage == 2) && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
            {
                API.CastSpell(Trincket2);
                return;
            }
            rotation();
            return;
        }

        public override void OutOfCombatPulse()
        {
            if (IsShieldSpeed && API.PlayerIsTalentSelected(2, 1) && API.PlayerIsMoving && API.CanCast(PWShield) && !API.PlayerHasDebuff(WeakenedSoul, false, false))
            {
                API.CastSpell(PWShield);
                return;
            }
            if (API.PlayerHealthPercent <= ShadowMendOOCLifePercent && !API.PlayerIsMoving && API.CanCast(ShadowMend) && API.SpellIsCanbeCast(ShadowMend))
            {
                API.CastSpell(ShadowMend);
                return;
            }
        }

        private void rotation()
        {
            //actions+=/call_action_list,name=cwc

            //ascended_blast,if=spell_targets.mind_sear<=3



            if (API.CanCast(AscendedBlast))
            {

                if (API.PlayerHasBuff(BoonOfTheAscended) && API.TargetUnitInRangeCount <= 3)

                {
                    API.CastSpell(AscendedBlast);
                    return;
                }


            }


            //boon_of_the_ascended,if=!buff.voidform.up&!cooldown.void_eruption.up&spell_targets.mind_sear>1&!talent.searing_nightmare.enabled| OR


            //Use on CD but prioritise using Void Eruption first, if used inside of VF on ST use after a voidbolt for cooldown efficiency and for hungering void uptime if talented.


            if (API.CanCast(BoonOfTheAscended) && !API.PlayerHasBuff(BoonOfTheAscended))
            {
                if ((!API.PlayerHasBuff(Voidform) && !API.CanCast(VoidEruption) && API.TargetUnitInRangeCount > 1 && !TalentSearingNightmare) ||

                // or(buff.voidform.up&spell_targets.mind_sear<2&!talent.searing_nightmare.enabled&prev_gcd.1.void_bolt)
                (API.PlayerHasBuff(Voidform) && API.TargetUnitInRangeCount < 2 && !TalentSearingNightmare) ||
                //|(buff.voidform.up&talent.searing_nightmare.enabled)
                (API.PlayerHasBuff(Voidform) && TalentSearingNightmare)

                )

                {

                    API.CastSpell(BoonOfTheAscended);
                    return;
                }


            }


            //actions.cwc=searing_nightmare,use_while_casting=1,target_if=(variable.searing_nightmare_cutoff&!variable.pi_or_vf_sync_condition)|(dot.shadow_word_pain.refreshable&spell_targets.mind_sear>1)
            if (TalentSearingNightmare && API.PlayerInsanity >= 30 && API.CanCast(SearingNightmare) && ChannelingMindSear && !API.PlayerIsMoving)
            {
                if ((searing_nightmare_cutoff && !pi_or_vf_sync_condition) || (API.TargetDebuffRemainingTime(SWPain) <= 360 && API.TargetUnitInRangeCount > 1))
                {
                    API.CastSpell(SearingNightmare);
                    return;
                }
            }

            //actions.cwc+=/searing_nightmare,use_while_casting=1,target_if=talent.searing_nightmare.enabled&dot.shadow_word_pain.refreshable&spell_targets.mind_sear>2
            if (TalentSearingNightmare && API.PlayerInsanity >= 30 && API.CanCast(SearingNightmare) && ChannelingMindSear && !API.PlayerIsMoving)
            {
                if (API.TargetDebuffRemainingTime(SWPain) <= 360 && API.TargetUnitInRangeCount > 2)
                {
                    API.CastSpell(SearingNightmare);
                    return;
                }
            }

            //actions.cwc+=/mind_blast,only_cwc=1
            if (API.CanCast(MindBlast) && (ChannelingMindFlay || ChannelingMindSear) && !API.PlayerIsMoving && PlayerLevel >= 5)
            {
                API.CastSpell(MindBlast);
                return;
            }

            if (IsCooldowns)
            {
                //actions.cds=power_infusion,if=buff.voidform.up|!soulbind.combat_meditation.enabled&cooldown.void_eruption.remains>=10|fight_remains<cooldown.void_eruption.remains
                if (API.CanCast(PowerInfusion) && API.PlayerHasBuff(Voidform) && PlayerLevel >= 58)
                {
                    API.CastSpell(PowerInfusion);
                    return;
                }

                //actions+=/run_action_list,name=main
                //actions.main=void_eruption,if=variable.pi_or_vf_sync_condition&insanity>=40
                if (!API.PlayerIsMoving && pi_or_vf_sync_condition && API.PlayerInsanity >= 40)
                {
                    API.CastSpell(VoidEruption);
                    return;
                }

                if (!API.PlayerIsCasting(true) && PlayerCovenantSettings == "Venthyr" && API.CanCast(Mindgames))
                {
                    API.CastSpell(Mindgames);
                    return;
                }
            }

            if ((!API.PlayerIsCasting(true) || ChannelingMindFlay) && API.CanCast(MindSear) && !API.PlayerIsMoving && PlayerLevel >= 26)
            {
                if (IsAOE && TalentSearingNightmare && API.TargetUnitInRangeCount > 2 && !API.TargetHasDebuff(SWPain, true) &&
                    (TalentMindbender ? API.SpellISOnCooldown(Mindbender) : API.SpellISOnCooldown(Shadowfiend)))
                {
                    API.CastSpell(MindSear);
                    return;
                }
            }

            //actions.main+=/damnation,target_if=!variable.all_dots_up
            if (TalentDamnation && API.CanCast(Damnation))
            {
                if (!all_dots_up)
                {
                    API.CastSpell(Damnation);
                    return;
                }
            }

            //actions.main+=/void_bolt,if=insanity<=85&((talent.hungering_void.enabled&spell_targets.mind_sear<5)|spell_targets.mind_sear=1)
            if (API.CanCast(VoidBolt) && API.PlayerHasBuff(Voidform))
            {
                if (API.PlayerInsanity <= 85 && ((TalentHungeringVoid && API.TargetUnitInRangeCount < 5) || API.TargetUnitInRangeCount == 1))
                {
                    API.CastSpell(VoidBolt);
                    return;
                }
            }

            //actions.main+=/devouring_plague,target_if=(refreshable|insanity>75)&!variable.pi_or_vf_sync_condition&(!talent.searing_nightmare.enabled|(talent.searing_nightmare.enabled&!variable.searing_nightmare_cutoff))
            if (API.CanCast(DevouringPlague) && API.SpellIsCanbeCast(DevouringPlague) && PlayerLevel >= 10)
            {
                if ((API.TargetDebuffRemainingTime(DevouringPlague) <= 180 || API.PlayerInsanity > 75) && (!IsCooldowns || !pi_or_vf_sync_condition || API.PlayerInsanity >= 85) &&
                    (!TalentSearingNightmare || !IsAOE || (IsAOE && TalentSearingNightmare && !searing_nightmare_cutoff)))
                {
                    API.CastSpell(DevouringPlague);
                    return;
                }
            }

            //actions.main+=/void_bolt,if=spell_targets.mind_sear<(4+conduit.dissonant_echoes.enabled)&insanity<=85
            if (API.CanCast(VoidBolt) && API.PlayerHasBuff(Voidform))
            {
                if (API.TargetUnitInRangeCount < (4) && API.PlayerInsanity <= 85)
                {
                    API.CastSpell(VoidBolt);
                    return;
                }
            }

            //actions.main+=/shadow_word_death,target_if=(target.health.pct<20&spell_targets.mind_sear<4)|(pet.fiend.active&runeforge.shadowflame_prism.equipped)
            if (API.CanCast(SWDeath) && PlayerLevel >= 14)
            {
                if ((API.TargetHealthPercent <= 20 && (!IsAOE || API.TargetUnitInRangeCount < 4)))
                {
                    API.CastSpell(SWDeath);
                    return;
                }
            }

            //actions.main+=/surrender_to_madness,target_if=target.time_to_die<25&buff.voidform.down
            if (TalentSurrenderToMadness && API.CanCast("Surrender to Madness") && !API.PlayerIsMoving)
            {
                if (IsCooldowns && API.TargetTimeToDie < 250 && !API.PlayerHasBuff(Voidform))
                {
                    API.CastSpell("Surrender to Madness");
                    return;
                }
            }

            //actions.main+=/mindbender,if=dot.vampiric_touch.ticking&((talent.searing_nightmare.enabled&spell_targets.mind_sear>(variable.mind_sear_cutoff+1))|dot.shadow_word_pain.ticking)
            if (IsCooldowns && (TalentMindbender ? API.CanCast(Mindbender) : API.CanCast(Shadowfiend)) && !API.PlayerIsMoving && PlayerLevel >= 20)
            {
                if ((API.TargetHasDebuff(VampiricTouch, true) || !IsUseVamp) &&
                    ((IsAOE && TalentSearingNightmare && API.TargetUnitInRangeCount > (2)) || API.TargetHasDebuff(SWPain, true)))
                {
                    if (TalentMindbender)
                        API.CastSpell(Mindbender);
                    else
                        API.CastSpell(Shadowfiend);
                    return;
                }
            }

            //actions.main+=/void_torrent,target_if=variable.dots_up&target.time_to_die>4&buff.voidform.down&spell_targets.mind_sear<(5+(6*talent.twist_of_fate.enabled))
            if (TalentVoidTorrent && API.CanCast(VoidTorrent) && !API.PlayerIsMoving)
            {
                if (dots_up && API.TargetTimeToDie > 400 && !API.PlayerHasBuff(Voidform) && (!IsAOE || API.TargetUnitInRangeCount < (5 + (6 * (TalentTwistOfFate ? 1 : 0)))))
                {
                    API.CastSpell(VoidTorrent);
                    return;
                }
            }

            //actions.main+=/shadow_crash,if=spell_targets.shadow_crash=1&(cooldown.shadow_crash.charges=3|debuff.shadow_crash_debuff.up|action.shadow_crash.in_flight|target.time_to_die<cooldown.shadow_crash.full_recharge_time)&raid_event.adds.in>30
            if (TalentShadowCrash && API.CanCast(ShadowCrash))
            {
                if (API.TargetUnitInRangeCount == 1 && (API.SpellCharges(ShadowCrash) == 3 || API.TargetHasDebuff(ShadowCrash, true)))
                {
                    API.CastSpell(ShadowCrash);
                    return;
                }
            }

            //actions.main+=/shadow_crash,if=raid_event.adds.in>30&spell_targets.shadow_crash>1
            if (TalentShadowCrash && API.CanCast(ShadowCrash))
            {
                if (API.TargetUnitInRangeCount > 1)
                {
                    API.CastSpell(ShadowCrash);
                    return;
                }
            }

            //actions.main+=/mind_flay,if=buff.dark_thoughts.up&variable.dots_up,chain=1,interrupt_immediate=1,interrupt_if=ticks>=2&cooldown.void_bolt.up
            if ((!API.PlayerIsCasting(true) || ChannelingMindSear) && API.CanCast(MindFlay) && !API.PlayerIsMoving && PlayerLevel >= 11)
            {
                if (API.PlayerHasBuff(DarkThoughts))
                {
                    API.CastSpell(MindFlay);
                    return;
                }
            }

            //actions.main+=/mind_blast,if=variable.dots_up&raid_event.movement.in>cast_time+0.5&spell_targets.mind_sear<4
            if (API.CanCast(MindBlast) && !API.PlayerIsMoving && PlayerLevel >= 5)
            {
                if ((dots_up || API.TargetTimeToDie <= 600 || !IsUseVamp) && (!IsAOE || API.TargetUnitInRangeCount < 4))
                {
                    API.CastSpell(MindBlast);
                    return;
                }
            }

            if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.TargetUnitInRangeCount <= 9 && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
            {
                if (!API.MacroIsIgnored(VampiricTouch + "MO") && API.CanCast(VampiricTouch) && (API.PlayerHasBuff(UnfurlingDarkness) || !CastingVT && !API.PlayerIsMoving) && PlayerLevel >= 15) //!CastingVT to prevent double casting VT
                {
                    if (API.MouseoverDebuffRemainingTime(VampiricTouch) <= 630 ||
                        (TalentMisery && API.MouseoverDebuffRemainingTime(SWPain) <= 360))
                    {
                        API.CastSpell(VampiricTouch + "MO");
                        return;
                    }
                }

                //actions.main+=/shadow_word_pain,if=refreshable&target.time_to_die>4&!talent.misery.enabled&talent.psychic_link.enabled&spell_targets.mind_sear>2
                if (!API.MacroIsIgnored(SWPain + "MO") && API.CanCast(SWPain) && PlayerLevel >= 2)
                {
                    if (API.MouseoverDebuffRemainingTime(SWPain) <= 360 && !TalentMisery && (!TalentPsychicLink || (TalentPsychicLink && API.TargetUnitInRangeCount <= 2)))
                    {
                        API.CastSpell(SWPain + "MO");
                        return;
                    }
                }
            }

            //actions.main+=/vampiric_touch,target_if=refreshable&target.time_to_die>6|(talent.misery.enabled&dot.shadow_word_pain.refreshable)|buff.unfurling_darkness.up
            if (IsUseVamp && API.LastSpellCastInGame != VampiricTouch && API.CanCast(VampiricTouch) && (API.PlayerHasBuff(UnfurlingDarkness) || (!CastingVT && !API.PlayerIsMoving)) && PlayerLevel >= 15) //!CastingVT to prevent double casting VT
            {
                if (API.TargetDebuffRemainingTime(VampiricTouch) <= 630 && API.TargetTimeToDie > 600 || API.PlayerHasBuff(UnfurlingDarkness) ||
                    (TalentMisery && API.TargetDebuffRemainingTime(SWPain) <= 360))
                {
                    API.CastSpell(VampiricTouch);
                    return;
                }
            }

            //actions.main+=/shadow_word_pain,if=refreshable&target.time_to_die>4&!talent.misery.enabled&talent.psychic_link.enabled&spell_targets.mind_sear>2
            if (IsAOE && API.CanCast(SWPain) && PlayerLevel >= 2)
            {
                if (API.TargetDebuffRemainingTime(SWPain) <= 360 && API.TargetTimeToDie > 400 && !TalentSearingNightmare && !TalentMisery && TalentPsychicLink && API.TargetUnitInRangeCount > 2)
                {
                    API.CastSpell(SWPain);
                    return;
                }
            }

            //actions.main+=/shadow_word_pain,target_if=refreshable&target.time_to_die>4&!talent.misery.enabled&!(talent.searing_nightmare.enabled&spell_targets.mind_sear>(variable.mind_sear_cutoff+1))&(!talent.psychic_link.enabled|(talent.psychic_link.enabled&spell_targets.mind_sear<=2))
            if (API.CanCast(SWPain) && PlayerLevel >= 2)
            {
                if (API.TargetDebuffRemainingTime(SWPain) <= 360 && API.TargetTimeToDie > 400 && !TalentMisery &&
                    (!IsAOE || API.TargetUnitInRangeCount <= 1 || !(TalentSearingNightmare && API.TargetUnitInRangeCount > 2)) &&
                    (!TalentPsychicLink || (TalentPsychicLink && API.TargetUnitInRangeCount <= 2)))
                {
                    API.CastSpell(SWPain);
                    return;
                }
            }

            //actions.main+=/mind_sear,target_if=spell_targets.mind_sear>variable.mind_sear_cutoff,chain=1,interrupt_immediate=1,interrupt_if=ticks>=2
            if ((!API.PlayerIsCasting(true) || ChannelingMindFlay) && IsAOE && API.CanCast(MindSear) && !API.PlayerIsMoving && PlayerLevel >= 26)
            {
                if (API.TargetUnitInRangeCount > 1)
                {
                    API.CastSpell(MindSear);
                    return;
                }
            }

            //actions.main+=/mind_flay,chain=1,interrupt_immediate=1,interrupt_if=ticks>=2&cooldown.void_bolt.up
            if ((!API.PlayerIsCasting(true)) && API.CanCast(MindFlay) && !API.PlayerIsMoving && PlayerLevel >= 11)
            {
                API.CastSpell(MindFlay);
                return;
            }

            //actions.main+=/shadow_word_death
            if (IsSWDeathMoving && API.CanCast(SWDeath) && API.PlayerIsMoving && PlayerLevel >= 14)
            {
                API.CastSpell(SWDeath);
                return;
            }

            if (IsMouseover && !API.MacroIsIgnored(SWPain + "MO") && API.PlayerIsMoving && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0)
            {
                //actions.main+=/shadow_word_pain,if=refreshable&target.time_to_die>4&!talent.misery.enabled&talent.psychic_link.enabled&spell_targets.mind_sear>2
                if (API.CanCast(SWPain) && PlayerLevel >= 2)
                {
                    API.CastSpell(SWPain + "MO");
                    return;
                }
            }

            //actions.main+=/shadow_word_pain
            if (API.CanCast(SWPain) && API.PlayerIsMoving && PlayerLevel >= 2)
            {
                API.CastSpell(SWPain);
                return;
            }
        }
    }
}