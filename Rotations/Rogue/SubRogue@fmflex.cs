
namespace HyperElk.Core
{
    public class SubRogueFmFlex : CombatRoutine
    {

        //Spells, Buff, Debuff
        private string Backstab = "Backstab";
        private string ShurikenStorm = "Shuriken Storm";
        private string Shadowstrike = "Shadowstrike";
        private string Rupture = "Rupture";
        private string Eviscerate = "Eviscerate";
        private string BlackPowder = "Black Powder";
        private string ShadowDance = "Shadow Dance";
        private string SymbolsofDeath = "Symbols of Death";
        private string ShadowBlades = "Shadow Blades";
        private string SliceandDice = "Slice and Dice";
        private string Stealth = "Stealth";
        private string Vanish = "Vanish";
        private string Ambush = "Ambush";
        private string GhostlyStrike = "Ghostly Strike";
        private string ShurikenTornado = "Shuriken Tornado";
        private string Subterfuge = "Subterfuge";

        private string Kick = "Kick";

        private string MarkedforDeath = "Marked for Death";
        private string Gloomblade = "Gloomblade";
        private string SecretTechnique = "Secret Technique";
        private string MasterofShadows = "Master of Shadows";
        private string CloakofShadows = "Cloak of Shadows";
        private string CrimsonVial = "Crimson Vial";
        private string Feint = "Feint";
        private string Evasion = "Evasion";
        private string PhialofSerenity = "Phial of Serenity";

        private string FindWeakness = "Find Weakness";
        private string Premeditation = "Premeditation";
        private string TheRotten = "The Rotten";

        private string EchoingReprimand = "Echoing Reprimand";
        private string SerratedBoneSpike = "Serrated Bone Spike";
        private string Flagellation = "Flagellation";
        private string Sepsis = "Sepsis";

        private string nextCrit = "227151";
        private string LeadbyExample = "Lead by Example";
        private string Soulshape = "Soulshape";

        private bool IsSmallCD => API.ToggleIsEnabled("SmallCD");

        //Properties
        private int EvasionLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Evasion)];
        private int CrimsonVialLifePercent => percentListProp[CombatRoutine.GetPropertyInt(CrimsonVial)];
        private int FeintLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feint)];
        private bool AutoStealth => CombatRoutine.GetPropertyBool("AUTOSTEALTH");

        private int PhialofSerenityLifePercent => percentListProp[CombatRoutine.GetPropertyInt(PhialofSerenity)];

        //Talents

        bool TalentWeaponmast => API.PlayerIsTalentSelected(1, 1);
        bool TalentPremeditation => API.PlayerIsTalentSelected(1, 2);
        bool TalentGloomblade => API.PlayerIsTalentSelected(1, 3);
        bool TalentNightstalker => API.PlayerIsTalentSelected(2, 1);
        bool TalentSubterfuge => API.PlayerIsTalentSelected(2, 2);
        bool TalentShadowFocus => API.PlayerIsTalentSelected(2, 3);
        bool TalentVigor => API.PlayerIsTalentSelected(3, 1);
        bool TalentMarkedForDeath => API.PlayerIsTalentSelected(3, 3);
        bool TalentDeeperStratagem => API.PlayerIsTalentSelected(3, 2);
        bool TalentDarkShadow => API.PlayerIsTalentSelected(6, 1);
        bool TalentAlacrity => API.PlayerIsTalentSelected(6, 2);
        bool TalentEnvelopingShadows => API.PlayerIsTalentSelected(6, 3);
        bool TalentMasterOfShadows => API.PlayerIsTalentSelected(7, 1);
        bool TalentSecretTechnique => API.PlayerIsTalentSelected(7, 2);
        bool TalentShurikenTornado => API.PlayerIsTalentSelected(7, 3);


        //Rotation Utilities

        private bool isStealth => API.PlayerHasBuff(Stealth) || API.PlayerHasBuff(Vanish);

        int MaxEnergy => API.PlayeMaxEnergy;
        int MaxComboPoints => TalentDeeperStratagem ? 6 : 5;
        int ComboPointDeficit => MaxComboPoints - API.PlayerComboPoints;

        double RuptureMaxTime;
        bool IsMelee => API.TargetRange <= 6;
        float EnergyRegen => 10f * (1f + API.PlayerGetHaste) * (TalentVigor ? 1.1f : 1f);

        bool SnDCondition => API.PlayerHasBuff(SliceandDice) || API.PlayerUnitInMeleeRangeCount >= 6;
        int StealthThreshold => (25 + ((TalentVigor ? 1 : 0) * 20) + ((TalentMasterOfShadows ? 1 : 0) * 20) + ((TalentShadowFocus ? 1 : 0) * 25) + ((TalentAlacrity ? 1 : 0) * 20) + (25 * (API.PlayerUnitInMeleeRangeCount >= 4 ? 1 : 0)));

        // finish->add_action("variable,name=skip_rupture,value=master_assassin_remains>0|!talent.nightstalker.enabled&talent.dark_shadow.enabled&buff.shadow_dance.up|spell_targets.shuriken_storm>=5", "Helper Variable for Rupture. Skip during Master Assassin or during Dance with Dark and no Nightstalker.");
        bool SkipRupture => (API.PlayerHasBuff(MasterofShadows, false, false) || !TalentNightstalker && TalentDarkShadow && API.PlayerHasBuff(ShadowDance) || (API.PlayerUnitInMeleeRangeCount >= 5 && IsAOE));


        // finish->add_action("variable,name=premed_snd_condition,value=talent.premeditation.enabled&spell_targets<(5-covenant.necrolord)&!covenant.kyrian", "While using Premeditation, avoid casting Slice and Dice when Shadow Dance is soon to be used, except for Kyrian");
        bool premed_snd_condition => TalentPremeditation && API.PlayerUnitInMeleeRangeCount < (5 + PlayerCovenantSettings == "Necrolord" ? 1 : 0) && PlayerCovenantSettings != "Kyrian";
        bool RuptureRefreshable => API.TargetDebuffRemainingTime(Rupture) < (RuptureMaxTime * 0.30) || !API.TargetHasDebuff(Rupture);
        bool shdThreshold => (API.SpellCharges(ShadowDance) + ((6000 - API.SpellChargeCD(ShadowDance)) / 6000)) >= 1.75;
        bool shd_combo_points => PlayerCovenantSettings == "Kyrian" ? ComboPointDeficit >= 3 : ComboPointDeficit >= 2 + (API.PlayerHasBuff(ShadowBlades) ? 1 : 0);

        int current_effective_cp => PlayerCovenantSettings == "Kyrian" && API.PlayerBuffStacks(EchoingReprimand) == API.PlayerComboPoints ? 7 : API.PlayerComboPoints;

        bool shadow_dance_ready => !API.SpellISOnCooldown(ShadowDance) && !API.PlayerHasBuff(Stealth, false, false);

        bool SerratedBoneSpikeThreshold => (API.SpellCharges(SerratedBoneSpike) + ((3000 - API.SpellChargeCD(SerratedBoneSpike)) / 3000)) >= 2.75;
        public override void Initialize()
        {
            CombatRoutine.Name = "Sub Rotation @FmFlex";
            API.WriteLog("Welcome to FmFlex's Sub rotation");


            //Add Spell, Buff, Debuffs

            //Add Spell, Buff, Debuffs

            //ComboPoint-Generator
            CombatRoutine.AddSpell(Backstab, 53, "D1");
            CombatRoutine.AddSpell(ShurikenStorm, 197835, "D1");
            CombatRoutine.AddSpell(Shadowstrike, 185438, "D1");
            CombatRoutine.AddSpell(Ambush, 8676, "D1");
            //ComboPoint-Spender
            CombatRoutine.AddSpell(Rupture, 1943, "D1");
            CombatRoutine.AddSpell(Eviscerate, 196819, "D1");
            CombatRoutine.AddSpell(SliceandDice, 315496, "D1");

            CombatRoutine.AddSpell(ShadowDance, 185313, "D1");
            CombatRoutine.AddSpell(SymbolsofDeath, 212283, "D1");
            CombatRoutine.AddSpell(ShadowBlades, 121471, "D1");
            CombatRoutine.AddSpell(BlackPowder, 319175);

            CombatRoutine.AddSpell(Kick, 1766, "D1");
            CombatRoutine.AddSpell(Stealth, 1784, "D1");
            CombatRoutine.AddSpell(Vanish, 1856, "D1");

            CombatRoutine.AddSpell(CloakofShadows, 31224, "D1");
            CombatRoutine.AddSpell(CrimsonVial, 185311, "D1");
            CombatRoutine.AddSpell(Feint, 1966, "D1");
            CombatRoutine.AddSpell(Evasion, 5277, "D1");

            CombatRoutine.AddSpell(MarkedforDeath, 137619, "D1");
            CombatRoutine.AddSpell(GhostlyStrike, 196937, "D1");
            CombatRoutine.AddSpell(Gloomblade, 200758, "D1");
            CombatRoutine.AddSpell(SecretTechnique, 280719, "D1");
            CombatRoutine.AddSpell(ShurikenTornado, 277925);

            CombatRoutine.AddSpell(EchoingReprimand, 323547);
            CombatRoutine.AddSpell(SerratedBoneSpike, 328547);
            CombatRoutine.AddSpell(Flagellation, 323654);
            CombatRoutine.AddSpell(Sepsis, 328305);
            CombatRoutine.AddBuff(SliceandDice, 315496);
            CombatRoutine.AddBuff(ShadowDance, 185422);
            CombatRoutine.AddBuff(ShadowBlades, 121471);
            CombatRoutine.AddBuff(SymbolsofDeath, 212283);
            CombatRoutine.AddBuff(ShurikenTornado, 277925);
            CombatRoutine.AddBuff(Stealth, 115191);
            CombatRoutine.AddBuff(Vanish, 1856);
            CombatRoutine.AddBuff(EchoingReprimand, 323547);
            CombatRoutine.AddBuff(LeadbyExample, 342156);
            CombatRoutine.AddBuff(Premeditation, 343160);
            CombatRoutine.AddBuff(TheRotten, 340091);
            CombatRoutine.AddBuff(nextCrit);
            CombatRoutine.AddBuff(Flagellation, 323654);
            CombatRoutine.AddBuff(Sepsis, 328305);
            CombatRoutine.AddBuff(Subterfuge, 108208);
            CombatRoutine.AddBuff(MasterofShadows, 196976);
            CombatRoutine.AddBuff(Soulshape, 310143);

            CombatRoutine.AddDebuff(Rupture, 1943);
            CombatRoutine.AddDebuff(FindWeakness, 316219);
            CombatRoutine.AddDebuff(Flagellation, 323654);

            CombatRoutine.AddConduit(LeadbyExample);


            CombatRoutine.AddItem(PhialofSerenity, 177278);
            //CB Properties

            CombatRoutine.AddProp("AUTOSTEALTH", "Auto Stealth", true, "Auto Stealth when not in combat", "Generic");

            CombatRoutine.AddProp(Evasion, Evasion + " Life Percent", percentListProp, "Life percent at which" + Evasion + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(CrimsonVial, CrimsonVial + " Life Percent", percentListProp, "Life percent at which" + CrimsonVial + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Feint, Feint + " Life Percent", percentListProp, "Life percent at which" + Feint + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", percentListProp, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);

            CombatRoutine.AddToggle("SmallCD");
        }


        public override void Pulse()
        {
            if (!API.PlayerIsMounted && !API.PlayerIsCasting())
            {
                if (API.PlayerHealthPercent <= CrimsonVialLifePercent && API.CanCast(CrimsonVial) && API.PlayerEnergy >= 20)
                {
                    API.CastSpell(CrimsonVial);
                    return;
                }
                if (API.PlayerItemCanUse(PhialofSerenity) && API.PlayerItemRemainingCD(PhialofSerenity) == 0 && API.PlayerHealthPercent <= PhialofSerenityLifePercent)
                {
                    API.CastSpell(PhialofSerenity);
                    return;
                }
            }
        }

        public override void CombatPulse()
        {
            if (!API.PlayerIsCasting())
            {
                //DEF
                if (isInterrupt && API.CanCast(Kick) && IsMelee)
                {
                    API.CastSpell(Kick);
                    return;
                }

                if (IsCooldowns && API.PlayerHealthPercent <= EvasionLifePercent && API.CanCast(Evasion))
                {
                    API.CastSpell(Evasion);
                    return;
                }
                if (API.PlayerHealthPercent <= FeintLifePercent && API.CanCast(Feint) && API.PlayerEnergy >= 35)
                {
                    API.CastSpell(Feint);
                    return;
                }

                //CDS

                //cds->add_action(this, "Shadow Dance", "use_off_gcd=1,if=!buff.shadow_dance.up&buff.shuriken_tornado.up&buff.shuriken_tornado.remains<=3.5", "Use Dance off-gcd before the first Shuriken Storm from Tornado comes in.");
                if (IsSmallCD && !(isStealth || API.PlayerHasBuff(Sepsis) || API.PlayerHasBuff(Subterfuge)) && IsMelee && API.CanCast(ShadowDance) && !API.PlayerHasBuff(ShadowDance) && API.PlayerHasBuff(ShurikenTornado) && API.PlayerBuffTimeRemaining(ShurikenTornado) <= 350)
                {
                    API.CastSpell(ShadowDance);
                }
                //cds->add_action( this, "Symbols of Death", "use_off_gcd=1,if=buff.shuriken_tornado.up&buff.shuriken_tornado.remains<=3.5", "(Unless already up because we took Shadow Focus) use Symbols off-gcd before the first Shuriken Storm from Tornado comes in." );
                if (IsSmallCD && API.CanCast(SymbolsofDeath) && IsMelee && API.PlayerHasBuff(ShurikenTornado) && API.PlayerBuffTimeRemaining(ShurikenTornado) <= 350)
                {
                    API.CastSpell(SymbolsofDeath);
                }
                //cds->add_action("flagellation,if=variable.snd_condition&!stealthed.mantle");
                if (IsSmallCD && PlayerCovenantSettings == "Venthyr" && IsMelee && !API.TargetHasDebuff(Flagellation) && SnDCondition && !isStealth && API.CanCast(Flagellation) && IsMelee)
                {
                    API.CastSpell(Flagellation);
                    return;
                }
                //cds->add_action("flagellation_cleanse,if=debuff.flagellation.remains<2");
                if (IsSmallCD && PlayerCovenantSettings == "Venthyr" && IsMelee && API.TargetHasDebuff(Flagellation) && API.TargetDebuffRemainingTime(Flagellation) < 200 && API.CanCast(Flagellation) && IsMelee)
                {
                    API.CastSpell(Flagellation);
                    return;
                }
                //cds->add_action(this, "Vanish", "if=(runeforge.mark_of_the_master_assassin&combo_points.deficit<=3|runeforge.deathly_shadows&combo_points<1)&buff.symbols_of_death.up&buff.shadow_dance.up&master_assassin_remains=0&buff.deathly_shadows.down");
                //TODO


                //cds->add_talent(this, "Shuriken Tornado", "if=energy>=60&variable.snd_condition&cooldown.symbols_of_death.up&cooldown.shadow_dance.charges>=1", "Use Tornado pre SoD when we have the energy whether from pooling without SF or just generally.");
                if (IsSmallCD && IsAOE && TalentShurikenTornado && API.CanCast(ShurikenTornado) && IsMelee && API.PlayerEnergy >= 60 && SnDCondition && !API.SpellISOnCooldown(SymbolsofDeath) && API.SpellCharges(ShadowDance) >= 1)
                {
                    API.CastSpell(ShurikenTornado);
                    return;
                }
                //cds->add_action("serrated_bone_spike,cycle_targets=1,if=variable.snd_condition&!dot.serrated_bone_spike_dot.ticking&target.time_to_die>=21|fight_remains<=5&spell_targets.shuriken_storm<3");
                if (PlayerCovenantSettings == "Necroload" && IsMelee && API.CanCast(SerratedBoneSpike) && SnDCondition && !API.TargetHasDebuff(SerratedBoneSpike) && API.TargetTimeToDie > 2100)
                {
                    API.CastSpell(SerratedBoneSpike);
                    return;
                }
                //cds->add_action("sepsis,if=variable.snd_condition&combo_points.deficit>=1");
                if (IsAOE && PlayerCovenantSettings == "Night Fae" && IsMelee && API.CanCast(Sepsis) && SnDCondition && ComboPointDeficit >= 1 && IsMelee)
                {
                    API.CastSpell(Sepsis);
                    return;
                }
                //cds->add_action(this, "Symbols of Death", "if=variable.snd_condition&(talent.enveloping_shadows.enabled|cooldown.shadow_dance.charges>=1)&(!talent.shuriken_tornado.enabled|talent.shadow_focus.enabled|cooldown.shuriken_tornado.remains>2)", "Use Symbols on cooldown (after first SnD) unless we are going to pop Tornado and do not have Shadow Focus.");

                if (API.CanCast(SymbolsofDeath) && SnDCondition && IsMelee &&
                (TalentEnvelopingShadows || API.SpellCharges(ShadowDance) >= 1) &&
                (!TalentShurikenTornado || TalentShadowFocus || API.SpellCDDuration(ShurikenStorm) > 200))
                {
                    API.CastSpell(SymbolsofDeath);
                    return;
                }
                //cds->add_talent(this, "Marked for Death", "target_if=min:target.time_to_die,if=raid_event.adds.up&(target.time_to_die<combo_points.deficit|!stealthed.all&combo_points.deficit>=cp_max_spend)", "If adds are up, snipe the one with lowest TTD. Use when dying faster than CP deficit or not stealthed without any CP.");
                //cds->add_talent(this, "Marked for Death", "if=raid_event.adds.in>30-raid_event.adds.duration&combo_points.deficit>=cp_max_spend", "If no adds will die within the next 30s, use MfD on boss without any CP.");

                if (IsSmallCD && TalentMarkedForDeath && ComboPointDeficit >= MaxComboPoints - 1 && API.CanCast(MarkedforDeath) && API.TargetRange <= 30)
                {
                    API.CastSpell(MarkedforDeath);
                    return;
                }

                //cds->add_action(this, "Shadow Blades", "if=variable.snd_condition&combo_points.deficit>=2");
                if (IsSmallCD && IsCooldowns && IsMelee && API.CanCast(ShadowBlades) && SnDCondition && ComboPointDeficit >= 2 && IsMelee)
                {
                    API.CastSpell(ShadowBlades);
                    return;
                }
                //cds->add_action("echoing_reprimand,if=variable.snd_condition&combo_points.deficit>=2&(variable.use_priority_rotation|spell_targets.shuriken_storm<=4)");
                if (IsSmallCD && PlayerCovenantSettings == "Kyrian" && IsMelee && API.CanCast(EchoingReprimand) && SnDCondition && ComboPointDeficit >= 2 && (API.PlayerUnitInMeleeRangeCount <= 4 || !IsAOE) && IsMelee)
                {
                    API.CastSpell(EchoingReprimand);
                    return;
                }
                //cds->add_talent(this, "Shuriken Tornado", "if=talent.shadow_focus.enabled&variable.snd_condition&buff.symbols_of_death.up", "With SF, if not already done, use Tornado with SoD up.");

                if (IsSmallCD && IsAOE && TalentShurikenTornado && IsMelee && TalentShadowFocus && API.CanCast(ShurikenTornado) && SnDCondition
                    && API.PlayerHasBuff(SymbolsofDeath))
                {
                    API.CastSpell(ShurikenTornado);
                    return;
                }
                //    cds->add_action( this, "Shadow Dance", "if=!buff.shadow_dance.up&fight_remains<=8+talent.subterfuge.enabled" );
                if (IsSmallCD && IsCooldowns && API.TargetIsBoss && !(isStealth || API.PlayerHasBuff(Sepsis) || API.PlayerHasBuff(Subterfuge)) && API.CanCast(ShadowDance) && !API.PlayerHasBuff(ShadowDance) && API.TargetTimeToDie < 800 + (TalentSubterfuge ? 1 : 0))
                {
                    API.CastSpell(ShadowDance);
                }

                if ((isStealth || API.PlayerHasBuff(ShadowDance) || API.PlayerHasBuff(Sepsis) || API.PlayerHasBuff(Subterfuge)))
                {
                    //stealthed->add_action(this, "Shadowstrike", "if=(buff.stealth.up|buff.vanish.up)", "If Stealth/vanish are up, use Shadowstrike to benefit from the passive bonus and Find Weakness, even if we are at max CP (from the precombat MfD).");


                    if (isStealth && API.TargetRange <= 25 && API.CanCast(Shadowstrike) && API.PlayerEnergy >= 40)
                    {
                        API.CastSpell(Shadowstrike);
                        return;
                    }
                    //stealthed->add_action("call_action_list,name=finish,if=buff.shuriken_tornado.up&combo_points.deficit<=2", "Finish at 3+ CP without DS / 4+ with DS with Shuriken Tornado buff up to avoid some CP waste situations.");

                    if (IsAOE && API.PlayerHasBuff(ShurikenTornado) && ComboPointDeficit <= 2)
                    {
                        Finisher();
                        return;
                    }
                    // stealthed->add_action("call_action_list,name=finish,if=spell_targets.shuriken_storm>=4&combo_points>=4", "Also safe to finish at 4+ CP with exactly 4 targets. (Same as outside stealth.)");

                    if (IsAOE && API.PlayerUnitInMeleeRangeCount >= 4 && API.PlayerComboPoints >= 4)
                    {
                        Finisher();
                        return;
                    }
                    // stealthed->add_action("call_action_list,name=finish,if=combo_points.deficit<=1-(talent.deeper_stratagem.enabled&buff.vanish.up)", "Finish at 4+ CP without DS, 5+ with DS, and 6 with DS after Vanish");

                    if (ComboPointDeficit <= 1 - ((TalentDeeperStratagem && API.PlayerHasBuff(Vanish)) ? 1 : 0))
                    {
                        Finisher();
                        return;
                    }
                    // stealthed->add_action(this, "Shiv", "if=talent.nightstalker.enabled&runeforge.tiny_toxic_blade&spell_targets.shuriken_storm<5");
                    //TODO

                    //stealthed->add_action(this, "Shadowstrike", "cycle_targets=1,if=debuff.find_weakness.remains<1&spell_targets.shuriken_storm<=3&target.time_to_die-remains>6", "Up to 3 targets keep up Find Weakness by cycling Shadowstrike.");
                    if (IsAOE && API.CanCast(Shadowstrike) && API.TargetRange <= 25 &&  API.TargetDebuffRemainingTime(FindWeakness) < 100 && API.PlayerUnitInMeleeRangeCount <= 3)
                    {
                        API.CastSpell(Shadowstrike);
                        return;
                    }

                    //stealthed->add_action(this, "Shadowstrike", "if=variable.use_priority_rotation&(debuff.find_weakness.remains<1|talent.weaponmaster.enabled&spell_targets.shuriken_storm<=4)", "For priority rotation, use Shadowstrike over Storm with WM against up to 4 targets or if FW is running off (on any amount of targets)");

                    //TODO

                    //stealthed->add_action( this, "Shuriken Storm", "if=spell_targets>=3+(buff.the_rotten.up|runeforge.akaaris_soul_fragment&conduit.deeper_daggers.rank>=7)&(buff.symbols_of_death_autocrit.up|!buff.premeditation.up|spell_targets>=5)" );
                    //TOPTI
                    if (IsAOE && IsMelee && API.CanCast(ShurikenStorm) && API.PlayerUnitInMeleeRangeCount >= 3 + (API.PlayerHasBuff(TheRotten) ? 1 : 0) && (API.PlayerHasBuff(nextCrit) || !API.PlayerHasBuff(Premeditation) || API.PlayerUnitInMeleeRangeCount >= 5))
                    {
                        API.CastSpell(ShurikenStorm);
                        return;
                    }
                    //stealthed->add_action(this, "Shadowstrike", "if=debuff.find_weakness.remains<=1|cooldown.symbols_of_death.remains<18&debuff.find_weakness.remains<cooldown.symbols_of_death.remains", "Shadowstrike to refresh Find Weakness and to ensure we can carry over a full FW into the next SoD if possible.");
                    if (API.CanCast(Shadowstrike) && API.TargetRange <= 25 && (API.TargetDebuffRemainingTime(FindWeakness) <= 100) ||( API.SpellCDDuration(SymbolsofDeath) < 1800 && API.TargetDebuffRemainingTime(FindWeakness) < API.SpellCDDuration(SymbolsofDeath)))
                    {
                        API.CastSpell(Shadowstrike);
                        return;
                    }
                    //stealthed->add_talent(this, "Gloomblade", "if=buff.perforated_veins.stack>=5&conduit.perforated_veins.rank>=13");

                    // stealthed->add_action(this, "Shadowstrike");
                    if (API.CanCast(Shadowstrike) && API.TargetRange <= 25)
                    {
                        API.CastSpell(Shadowstrike);
                        return;
                    }
                }

                if (API.PlayeMaxEnergy - API.PlayerEnergy <= StealthThreshold)
                {
                    //stealth_cds->add_action(this, "Vanish", "if=(!variable.shd_threshold|!talent.nightstalker.enabled&talent.dark_shadow.enabled)&combo_points.deficit>1&!runeforge.mark_of_the_master_assassin", "Vanish if we are capping on Dance charges. Early before first dance if we have no Nightstalker but Dark Shadow in order to get Rupture up (no Master Assassin).");
                    //TODO

                    //stealth_cds->add_action("pool_resource,for_next=1,extra_amount=40,if=race.night_elf", "Pool for Shadowmeld + Shadowstrike unless we are about to cap on Dance charges. Only when Find Weakness is about to run out.");
                    //stealth_cds->add_action("shadowmeld,if=energy>=40&energy.deficit>=10&!variable.shd_threshold&combo_points.deficit>1&debuff.find_weakness.remains<1");
                    //TODO

                    //stealth_cds->add_action("variable,name=shd_combo_points,value=combo_points.deficit>=2+buff.shadow_blades.up", "CP thresholds for entering Shadow Dance");
                    //stealth_cds->add_action("variable,name=shd_combo_points,value=combo_points.deficit>=3,if=covenant.kyrian");
                    //stealth_cds->add_action("variable,name=shd_combo_points,value=combo_points.deficit<=1,if=variable.use_priority_rotation&spell_targets.shuriken_storm>=4");
                    //stealth_cds->add_action(this, "Shadow Dance", "if=variable.shd_combo_points&
                    //(variable.shd_threshold|buff.symbols_of_death.remains>=1.2|spell_targets.shuriken_storm>=4&cooldown.symbols_of_death.remains>10)", "Dance during Symbols or above threshold.");
                    if (IsSmallCD && !(isStealth || API.PlayerHasBuff(Sepsis) || API.PlayerHasBuff(Subterfuge)) && IsMelee && API.CanCast(ShadowDance) && !API.PlayerHasBuff(ShadowDance) && shd_combo_points
                        && (shdThreshold || API.PlayerBuffTimeRemaining(SymbolsofDeath) >= 120 || (IsAOE && API.PlayerUnitInMeleeRangeCount >= 4 && API.SpellCDDuration(SymbolsofDeath) >= 1000)))
                    {
                        API.CastSpell(ShadowDance);
                    }
                    //stealth_cds->add_action(this, "Shadow Dance", "if=variable.shd_combo_points&fight_remains<cooldown.symbols_of_death.remains", "Burn remaining Dances before the fight ends if SoD won't be ready in time.");
                    if (IsSmallCD && IsCooldowns && !(isStealth || API.PlayerHasBuff(Sepsis) || API.PlayerHasBuff(Subterfuge)) && IsMelee && API.TargetIsBoss && API.CanCast(ShadowDance) && !API.PlayerHasBuff(ShadowDance) && shd_combo_points
                         && API.TargetTimeToDie < API.SpellCDDuration(SymbolsofDeath))
                    {
                        API.CastSpell(ShadowDance);
                    }
                }


                if ((API.PlayerUnitInMeleeRangeCount < 6 || !IsAOE) && API.PlayerEnergy >= 25 && API.CanCast(SliceandDice) && API.PlayerBuffTimeRemaining(SliceandDice) < API.SpellGCDTotalDuration && API.PlayerComboPoints >= 4 - (API.PlayerTimeInCombat < 1000 ? 1 : 0) * 2)
                {
                    API.CastSpell(SliceandDice);
                    return;
                }
                //def->add_action( "call_action_list,name=finish,if=combo_points=animacharged_cp" );
                if (PlayerCovenantSettings == "Kyrian" && API.PlayerBuffStacks(EchoingReprimand) == API.PlayerComboPoints)
                {
                    Finisher();
                    return;
                }
                //def->add_action("call_action_list,name=finish,if=combo_points.deficit<=1|fight_remains<=1&combo_points>=3|buff.symbols_of_death_autocrit.up&combo_points>=4", "Finish at 4+ without DS or with SoD crit buff, 5+ with DS (outside stealth)");

                if (IsAOE && ComboPointDeficit <= 1 || (API.PlayerHasBuff(nextCrit) && API.PlayerComboPoints >= 4))
                {
                    Finisher();
                    return;
                }
                //def->add_action( "call_action_list,name=finish,if=spell_targets.shuriken_storm>=4&combo_points>=4", "With DS also finish at 4+ against 4 targets (outside stealth)" );
                if (ComboPointDeficit <= 1 - ((TalentDeeperStratagem && API.PlayerHasBuff(Vanish)) ? 1 : 0))
                {
                    Finisher();
                    return;
                }

                if (API.PlayeMaxEnergy - API.PlayerEnergy <= StealthThreshold)
                {
                    Builder();
                    return;
                }
            }
        }

        public override void OutOfCombatPulse()
        {
            if (AutoStealth && !isStealth && !API.PlayerHasBuff(ShadowDance) && !API.PlayerHasBuff(Soulshape) && API.CanCast(Stealth) && !API.PlayerIsCasting())
            {
                API.CastSpell(Stealth);
                return;
            }
        }

        public void Builder()
        {

            //// Builders
            //build->add_action(this, "Shiv", "if=!talent.nightstalker.enabled&runeforge.tiny_toxic_blade&spell_targets.shuriken_storm<5");
            //TODO



            //build->add_action(this, "Shuriken Storm", "if=spell_targets>=2");
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= 2 && API.CanCast(ShurikenStorm) && IsMelee && API.PlayerEnergy >= 35)
            {
                API.CastSpell(ShurikenStorm);
                return;
            }
            //build->add_action("serrated_bone_spike,if=cooldown.serrated_bone_spike.charges_fractional>=2.75|soulbind.lead_by_example.enabled&!buff.lead_by_example.up");
            if (PlayerCovenantSettings == "Necrolord" && IsMelee && API.CanCast(SerratedBoneSpike) && (SerratedBoneSpikeThreshold || (API.PlayerIsConduitSelected(LeadbyExample) && API.PlayerHasBuff(LeadbyExample))))
            {
                API.CastSpell(SerratedBoneSpike);
                return;
            }
            //build->add_talent(this, "Gloomblade");
            if (TalentGloomblade && API.CanCast(Gloomblade) && IsMelee)
            {
                API.CastSpell(Gloomblade);
                return;
            }
            //build->add_action(this, "Backstab");
            else if (API.CanCast(Backstab) && IsMelee && API.PlayerEnergy >= 35)
            {
                API.CastSpell(Backstab);
                return;
            }
        }
        public void Finisher()
        {
            // Finishers


            // finish->add_action(this, "Slice and Dice", "if=!variable.premed_snd_condition&spell_targets.shuriken_storm<6&!buff.shadow_dance.up&buff.slice_and_dice.remains<fight_remains&refreshable");

            if (API.CanCast(SliceandDice) && API.PlayerEnergy >= 25 && premed_snd_condition && (API.PlayerUnitInMeleeRangeCount < 6 || !IsAOE) && !API.PlayerHasBuff(ShadowDance) && API.PlayerBuffTimeRemaining(SliceandDice) < current_effective_cp * 30)
            {
                API.CastSpell(SliceandDice);
                return;
            }
            // finish->add_action(this, "Slice and Dice", "if=variable.premed_snd_condition&cooldown.shadow_dance.charges_fractional<1.75&buff.slice_and_dice.remains<cooldown.symbols_of_death.remains&(cooldown.shadow_dance.ready&buff.symbols_of_death.remains-buff.shadow_dance.remains<1.2)");

            if (API.CanCast(SliceandDice) && API.PlayerEnergy >= 25 && premed_snd_condition && !shdThreshold && API.PlayerBuffTimeRemaining(SliceandDice) < API.SpellCDDuration(SymbolsofDeath) && (shadow_dance_ready && (API.PlayerBuffTimeRemaining(SymbolsofDeath) - API.PlayerBuffTimeRemaining(ShadowDance)) < 120))
            {
                API.CastSpell(SliceandDice);
                return;
            }
            // finish->add_action(this, "Rupture", "if=!variable.skip_rupture&target.time_to_die-remains>6&refreshable", "Keep up Rupture if it is about to run out.");
            if (!SkipRupture && API.PlayerEnergy >= 25 && IsMelee && RuptureRefreshable && API.CanCast(Rupture) && API.TargetTimeToDie > 600)
            {
                RuptureMaxTime = 400 + (API.PlayerComboPoints * 400);
                API.CastSpell(Rupture);
                return;
            }
            // finish->add_talent(this, "Secret Technique");
            if (IsSmallCD && TalentSecretTechnique && IsMelee && API.PlayerEnergy >= 30 && API.CanCast(SecretTechnique))
            {
                API.CastSpell(SecretTechnique);
                return;
            }
            // finish->add_action(this, "Rupture", "cycle_targets=1,if=!variable.skip_rupture&!variable.use_priority_rotation&spell_targets.shuriken_storm>=2&target.time_to_die>=(5+(2*combo_points))&refreshable", "Multidotting targets that will live for the duration of Rupture, refresh during pandemic.");

            //TODO MOUSE OVER RUPTURE
            // finish->add_action(this, "Rupture", "if=!variable.skip_rupture&remains<cooldown.symbols_of_death.remains+10&cooldown.symbols_of_death.remains<=5&target.time_to_die-remains>cooldown.symbols_of_death.remains+5", "Refresh Rupture early if it will expire during Symbols. Do that refresh if SoD gets ready in the next 5s.");

            if (API.CanCast(Rupture) && API.PlayerEnergy >= 25 && IsMelee && !SkipRupture && API.TargetDebuffRemainingTime(Rupture) < API.SpellCDDuration(SymbolsofDeath) + 1000 && API.SpellCDDuration(SymbolsofDeath) <= 500)
            {
                RuptureMaxTime = 400 + (API.PlayerComboPoints * 400);
                API.CastSpell(Rupture);
                return;
            }
            // finish->add_action(this, "Black Powder", "if=!variable.use_priority_rotation&spell_targets>=4-debuff.find_weakness.down");
            if (API.CanCast(BlackPowder) && IsMelee && API.PlayerUnitInMeleeRangeCount > 4 && IsAOE && API.PlayerLevel >= 52)
            {
                API.CastSpell(BlackPowder);
                return;
            }
            // finish->add_action(this, "Eviscerate");


            if (API.CanCast(Eviscerate) && API.PlayerEnergy >= 35 && IsMelee)
            {
                API.CastSpell(Eviscerate);
                return;
            }
        }
    }

}

