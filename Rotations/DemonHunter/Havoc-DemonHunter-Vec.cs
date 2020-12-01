
namespace HyperElk.Core
{
    public class MMHunter : CombatRoutine
    {

        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        //Spells,Buffs,Debuffs
        private string Metamorphosis = "Metamorphosis";
        private string Throw_Glaive = "Throw Glaive";
        private string Fel_Barrage = "Fel Barrage";
        private string Eye_Beam = "Eye Beam";
        private string Dark_Slash = "Dark Slash";
        private string Nemesis = "Nemesis";
        private string Blade_Dance = "Blade Dance";
        private string Death_Sweep = "Death Sweep";
        private string Felblade = "Felblade";
        private string Chaos_Strike = "Chaos Strike";
        private string Annihilation = "Annihilation";
        private string Demons_Bite = "Demons Bite";
        private string Immolation_Aura = "Immolation Aura";
        private string Netherwalk = "Netherwalk";
        private string Disrupt = "Disrupt";
        private string Chaos_Nova = "Chaos Nova";
        private string Darkness = "Darkness";
        private string Blur = "Blur";
        private string Consume_Magic = "Consume Magic";
        private string Glaive_Tempest = "Glaive Tempest";  
        private string Fel_Bombardment = "Fel Bombardment";    
        private string Essence_Break = "Essence Break";
        private string SinfulBrand = "Sinful Brand";
        private string TheHunt = "The Hunt";
        private string fodder_to_the_flame = "Fodder to the Flame"; 
        private string elysian_decree = "Elysian Decree"; 
private string furious_gaze = "Furious Gaze";
        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool MeleeRange => API.TargetRange < 6;
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private static void CastSpell(string spell)
        {
            if (API.CanCast(spell))
            {
                API.CastSpell(spell);
                return;
            }
        }

        //Talents
        private bool Talent_BlindFury => API.PlayerIsTalentSelected(1, 1);
        private bool Talent_Felblade => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_DemonBlades => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_TrailofRuin => API.PlayerIsTalentSelected(3, 1);
        private bool Talent_UnboundChaos => API.PlayerIsTalentSelected(3, 2);
        private bool Talent_GlaiveTempest => API.PlayerIsTalentSelected(3, 3);
        private bool Talent_Netherwalk => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_FirstBlood => API.PlayerIsTalentSelected(5, 2);
        private bool Talent_EssenceBreak => API.PlayerIsTalentSelected(5, 3);
        private bool Talent_FelEruption => API.PlayerIsTalentSelected(6, 3);
        private bool Talent_Demonic => API.PlayerIsTalentSelected(7, 1);
        private bool Talent_Momentum => API.PlayerIsTalentSelected(7, 2);
        private bool Talent_FelBarrage => API.PlayerIsTalentSelected(7, 3);



        //CBProperties
        string[] TrueshotList = new string[] { "always", "with Cooldowns" };
        string[] EyeBeamList = new string[] { "always", "with Cooldowns", "On AOE" };
        string[] MetamorphosisList = new string[] { "always", "with Cooldowns", "never" };
        string[] Throw_GlaiveList = new string[] { "On", "Off" };
        string[] FelBarrageList = new string[] { "On", "Off" };

        
        private int NetherwalkLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Netherwalk)];
        private int DarknessLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Darkness)];
        private int BlurLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Blur)];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];

        private string UseEyeBeam => EyeBeamList[CombatRoutine.GetPropertyInt(Eye_Beam)];
        private string UseMetamorphosis => MetamorphosisList[CombatRoutine.GetPropertyInt(Metamorphosis)];
        private string UseThrowGlaive => Throw_GlaiveList[CombatRoutine.GetPropertyInt(Throw_Glaive)];
        private string UseFelBarrage => FelBarrageList[CombatRoutine.GetPropertyInt(Fel_Barrage)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];

        private float fury_deficit => API.PlayeMaxFury - API.PlayerFury;
        //pooling_for_meta,value=!talent.demonic.enabled&cooldown.metamorphosis.remains<6&fury.deficit>30
        private bool PoolingForMeta
        {
            get
            {
                if (!Talent_Demonic && API.SpellCDDuration(Metamorphosis) < 600 && fury_deficit > 30)
                    return true;
                return false;
            }
        }

        //blade_dance,value=talent.first_blood.enabled|spell_targets.blade_dance1>=(3-talent.trail_of_ruin.enabled)
        private bool BladeDance
        {
            get
            {
                if (Talent_FirstBlood || IsAOE && API.PlayerUnitInMeleeRangeCount >= 3 - (Talent_TrailofRuin ? 1 : 0))
                    return true;
                return false;
            }
        }

        //variable,name=pooling_for_blade_dance,value=variable.blade_dance&(fury<75-talent.first_blood.enabled*20)
        private bool PoolingForBladeDance
        {
            get
            {
                if (BladeDance && (API.PlayerFury < 75 - (Talent_FirstBlood ? 20 : 0)))
                    return true;
                return false;
            }
        }
        //waiting_for_essence_break,value=talent.essence_break.enabled&!variable.pooling_for_blade_dance&!variable.pooling_for_meta&cooldown.essence_break.up
        private bool waiting_for_essence_break
        {
            get
            {
                if (Talent_EssenceBreak && !PoolingForBladeDance && !PoolingForMeta && !API.CanCast(Essence_Break))
                    return true;
                return false;
            }
        }
        //waiting_for_momentum,value=talent.momentum.enabled&!buff.momentum.up
        private bool waiting_for_momentum
        {
            get
            {
                if (Talent_Momentum && !API.PlayerHasBuff("Momentum"))
                    return true;
                return false;
            }
        }
        //variable,name_pooling_for_eye_beam,value=talent.demonic.enabled&!talent.blind_fury.enabled&cooldown.eye_beam.remains<(gcd.max*2)&fury.deficit>20
        private bool PoolingForEyeBeam
        {
            get
            {
                if (Talent_Demonic && !Talent_BlindFury && API.SpellCDDuration(Eye_Beam) < 300 && fury_deficit > 20)
                    return true;
                return false;
            }
        }
        private bool BladeDanceCost
        {
            get
            {
                if (API.PlayerFury >= 35 - (Talent_FirstBlood ? 20 : 0))
                    return true;
                return false;
            }
        }
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, false, false);
        }


        public override void Initialize()
        {
            CombatRoutine.Name = "Havoc DH by Vec";
            API.WriteLog("Welcome to Havoc DH Rotation");
            API.WriteLog("Metamorphosis Macro : /cast [@player] Metamorphosis");


            CombatRoutine.AddSpell(Fel_Barrage, "F1");
            CombatRoutine.AddSpell(Eye_Beam, "D1");
            CombatRoutine.AddSpell(Dark_Slash, "F2");
            CombatRoutine.AddSpell(Nemesis, "F3");
            CombatRoutine.AddSpell(Metamorphosis, "F4");
            CombatRoutine.AddSpell(Blade_Dance, "D2");
            CombatRoutine.AddSpell(Death_Sweep, "D2");
            CombatRoutine.AddSpell(Felblade, "D3");
            CombatRoutine.AddSpell(Chaos_Strike, "D4");
            CombatRoutine.AddSpell(Annihilation, "D4");
            CombatRoutine.AddSpell(Demons_Bite, "D5");
            CombatRoutine.AddSpell(Immolation_Aura, "D6");
            CombatRoutine.AddSpell(Throw_Glaive, "D7");
            CombatRoutine.AddSpell(Netherwalk, "F7");
            CombatRoutine.AddSpell(Disrupt, "F8");
            CombatRoutine.AddSpell(Chaos_Nova, "F12");
            CombatRoutine.AddSpell(Darkness, "F5");
            CombatRoutine.AddSpell(Blur, "NumPad1");
            CombatRoutine.AddSpell(Consume_Magic, "NumPad2");
            CombatRoutine.AddSpell(Glaive_Tempest, "NumPad5");
        CombatRoutine.AddSpell(SinfulBrand, "NumPad6");
            CombatRoutine.AddSpell(TheHunt, "NumPad6");
            CombatRoutine.AddSpell(fodder_to_the_flame, "NumPad6");
            CombatRoutine.AddSpell(elysian_decree, "NumPad6");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");

            CombatRoutine.AddBuff(Metamorphosis);
            CombatRoutine.AddBuff(Fel_Bombardment);
            CombatRoutine.AddBuff(furious_gaze);

            CombatRoutine.AddDebuff(Nemesis);
            CombatRoutine.AddDebuff(Dark_Slash);
            CombatRoutine.AddDebuff(Essence_Break);
            CombatRoutine.AddDebuff(SinfulBrand);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");

            //Settings
 
            CombatRoutine.AddProp(Eye_Beam, "Use " + Eye_Beam, EyeBeamList, "Use " + Eye_Beam + "always, with Cooldowns, On AOE", "Cooldowns", 0);
            CombatRoutine.AddProp(Metamorphosis, "Use " + Metamorphosis, MetamorphosisList, "Use " + Metamorphosis + "always, with Cooldowns, never", "Cooldowns");
            CombatRoutine.AddProp(Throw_Glaive, "Use " + Throw_Glaive, Throw_GlaiveList, "Use " + Throw_Glaive + "On, Off", "General", 0);
            CombatRoutine.AddProp(Fel_Barrage, "Use " + Fel_Barrage, FelBarrageList, "Use " + Fel_Barrage + "On, Off", "Cooldowns", 0);

            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Netherwalk, "Use " + Netherwalk + " below:", percentListProp, "Life percent at which " + Netherwalk + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Darkness, "Use " + Darkness + " below:", percentListProp, "Life percent at which " + Darkness + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Blur, "Use " + Blur + " below:", percentListProp, "Life percent at which " + Blur + " is used, set to 0 to disable", "Defense", 2);
        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted && !API.PlayerIsCasting && !API.PlayerIsChanneling)
            {


                if (Talent_Netherwalk && API.PlayerHealthPercent <= NetherwalkLifePercent)
                {
                    CastSpell(Netherwalk);

                }
                if (API.PlayerHealthPercent <= BlurLifePercent && API.PlayerLevel >= 21)
                {
                    CastSpell(Blur);

                }
                if (API.PlayerHealthPercent <= DarknessLifePercent && API.PlayerLevel >= 39)
                {
                    CastSpell(Darkness);

                }
            }
        }
        public override void CombatPulse()
        {
            if (!API.PlayerIsMounted && !API.PlayerIsCasting && !API.PlayerIsChanneling)
            {
                //API.WriteLog("debug:" + "cancast "+ API.CanCast(Metamorphosis)+ "when "+ UseMetamorphosis);
                if (isInterrupt && MeleeRange && PlayerLevel >= 29)
                {
                    CastSpell(Disrupt);

                }
                //API.WriteLog("lastspell " + API.LastSpellCastInGame);
                #region Cooldowns
                //apl_cooldown->add_action(this, "Metamorphosis", "if=!(talent.demonic.enabled|variable.pooling_for_meta)&(!covenant.venthyr.enabled|!dot.sinful_brand.ticking)|target.time_to_die<25");
                if (API.PlayerLevel >= 8 && UseMetamorphosis != "never" && ((UseMetamorphosis == "always" || UseMetamorphosis == "with Cooldowns" && IsCooldowns) && !(Talent_Demonic || PoolingForMeta) || API.TargetTimeToDie < 2500))
                {
                    CastSpell(Metamorphosis);

                }
                //apl_cooldown->add_action(this, "Metamorphosis", "                                                                                                 if=talent.demonic.enabled&(&level<54|(cooldown.eye_beam.remains>20&(!variable.blade_dance|cooldown.blade_dance.remains>gcd.max)))&(!covenant.venthyr.enabled|!dot.sinful_brand.ticking)");
                if (API.PlayerLevel >= 8 && UseMetamorphosis != "never" && ((UseMetamorphosis == "always" || UseMetamorphosis == "with Cooldowns" && IsCooldowns) && Talent_Demonic && (API.PlayerLevel < 54 || (API.SpellCDDuration(Eye_Beam) > 2000 && (!BladeDance || API.SpellCDDuration(Blade_Dance) > 150)))))
                {
                    CastSpell(Metamorphosis);

                }
                //apl_cooldown->add_action("sinful_brand,if=!dot.sinful_brand.ticking");
                if (!API.SpellISOnCooldown(SinfulBrand) && !TargetHasDebuff(SinfulBrand) && API.TargetRange <= 30 && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
                {
                    API.CastSpell(SinfulBrand);
                    return;
                }
                //apl_cooldown->add_action("the_hunt,if=!talent.demonic.enabled&!variable.waiting_for_momentum|buff.furious_gaze.up");
                if (!API.SpellISOnCooldown(TheHunt) && (!Talent_Demonic && !waiting_for_momentum || PlayerHasBuff(furious_gaze)) && API.TargetRange <= 50 && PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
                {
                    API.CastSpell(TheHunt);
                    return;
                }
                //apl_cooldown->add_action("fodder_to_the_flame");
                if (!API.SpellISOnCooldown(fodder_to_the_flame) && API.TargetRange <= 30 && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
                {
                    API.CastSpell(fodder_to_the_flame);
                    return;
                }
                //apl_cooldown->add_action("elysian_decree");
                if (!API.SpellISOnCooldown(elysian_decree) && API.TargetRange <= 30 && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
                {
                    API.CastSpell(elysian_decree);
                    return;
                }
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && MeleeRange)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && MeleeRange)
                {
                    API.CastSpell("Trinket2");
                }
                //apl_cooldown->add_action("potion,if=buff.metamorphosis.remains>25|target.time_to_die<60");
                #endregion
                #region essence_break
                if (Talent_EssenceBreak && (waiting_for_essence_break || API.TargetHasDebuff(Essence_Break)))
                {
                    //essence_break,if= fury >= 80 & (cooldown.blade_dance.ready | !variable.blade_dance)
                    if (API.PlayerFury >= 80 && (API.CanCast(Blade_Dance) && API.PlayerLevel >= 12 || !BladeDance))
                    {
                        CastSpell(Essence_Break);

                    }
                    //death_sweep,if= variable.blade_dance & debuff.essence_break.up
                    if (API.PlayerLevel >= 12 && API.TargetHasDebuff(Essence_Break) && API.PlayerHasBuff(Metamorphosis) && BladeDance && BladeDanceCost && MeleeRange)
                    {
                        CastSpell(Death_Sweep);

                    }
                    //blade_dance,if= variable.blade_dance & debuff.essence_break.up
                    if (API.PlayerLevel >= 12 && API.TargetHasDebuff(Essence_Break) && API.PlayerHasBuff(Metamorphosis) && BladeDance && BladeDanceCost && MeleeRange)
                    {
                        CastSpell(Blade_Dance);

                    }
                    //annihilation,if= debuff.essence_break.up
                    if (API.PlayerLevel >= 14 && API.TargetHasDebuff(Essence_Break) && API.PlayerFury >= 40 && API.PlayerHasBuff(Metamorphosis) && MeleeRange)
                    {
                        CastSpell(Annihilation);

                    }
                    //chaos_strike,if= debuff.essence_break.up
                    if (API.PlayerLevel >= 8 && API.TargetHasDebuff(Essence_Break) && API.PlayerFury >= 40 && !API.PlayerHasBuff(Metamorphosis) && MeleeRange)
                    {
                        CastSpell(Chaos_Strike);

                    }
                }
                #endregion
                #region demonic
                if (Talent_Demonic)
                {
                    //fel_rush,if= (talent.unbound_chaos.enabled & buff.unbound_chaos.up) & (charges = 2 | (raid_event.movement.in> 10 & raid_event.adds.in> 10))
                    //death_sweep,if= variable.blade_dance
                    if (API.PlayerLevel >= 12 && API.PlayerHasBuff(Metamorphosis) && BladeDance && BladeDanceCost && MeleeRange)
                    {
                        CastSpell(Death_Sweep);

                    }
                    //glaive_tempest,if= active_enemies > desired_targets | raid_event.adds.in> 10
                    if (Talent_GlaiveTempest && API.PlayerFury >= 30 && MeleeRange)
                    {
                        CastSpell(Glaive_Tempest);

                    }
                    //throw_glaive,if= conduit.serrated_glaive.enabled & cooldown.eye_beam.remains < 6 & !buff.metamorphosis.up & !debuff.exposed_wound.up

                    //eye_beam,if= raid_event.adds.up | raid_event.adds.in> 25
                    if (API.PlayerLevel >= 11 && !API.PlayerIsMoving && (UseEyeBeam == "always" || (UseEyeBeam == "with Cooldowns" && IsCooldowns)) && API.PlayerFury >= 30 && MeleeRange)
                    {
                        CastSpell(Eye_Beam);

                    }
                    //blade_dance,if= variable.blade_dance & !cooldown.metamorphosis.ready & (cooldown.eye_beam.remains > (5 - azerite.revolving_blades.rank * 3))
                    if (API.PlayerLevel >= 12 && !API.PlayerHasBuff(Metamorphosis) && BladeDanceCost && API.PlayerLevel >= 20 && API.SpellCDDuration(Eye_Beam) > (500) && MeleeRange)
                    {
                        CastSpell(Blade_Dance);

                    }
                    //immolation_aura
                    if (API.PlayerLevel >= 18 && MeleeRange)
                    {
                        CastSpell(Immolation_Aura);

                    }
                    //annihilation,if= !variable.pooling_for_blade_dance
                    if (API.PlayerLevel >= 14 && !PoolingForBladeDance && API.PlayerFury >= 40 && API.PlayerHasBuff(Metamorphosis) && MeleeRange)
                    {
                        CastSpell(Annihilation);

                    }
                    //felblade,if= fury.deficit >= 40
                    if (API.TargetRange <= 15 && Talent_Felblade && fury_deficit >= 40)
                    {
                        CastSpell(Felblade);

                    }
                    //chaos_strike,if= !variable.pooling_for_blade_dance & !variable.pooling_for_eye_beam
                    if (API.PlayerLevel >= 8 && API.PlayerFury >= 40 && !PoolingForBladeDance && !PoolingForEyeBeam && MeleeRange)
                    {
                        CastSpell(Chaos_Strike);

                    }
                    //fel_rush,if= talent.demon_blades.enabled & !cooldown.eye_beam.ready & (charges = 2 | (raid_event.movement.in> 10 & raid_event.adds.in> 10))

                    //demons_bite
                    if (API.PlayerLevel >= 8 && !Talent_DemonBlades && MeleeRange)
                    {
                        CastSpell(Demons_Bite);

                    }
                    //throw_glaive,if= buff.out_of_range.up
                    if (API.PlayerLevel >= 10 && UseThrowGlaive == "On" && API.TargetRange <= 30 && (Talent_DemonBlades || !MeleeRange))
                    {
                        CastSpell(Throw_Glaive);
                    }
                    //fel_rush,if= movement.distance > 15 | buff.out_of_range.up
                    //vengeful_retreat,if= movement.distance > 15
                    //throw_glaive,if= talent.demon_blades.enabled
                }
                #endregion
                #region normal
                //vengeful_retreat,if= talent.momentum.enabled & buff.prepared.down & time > 1
                // fel_rush,if= (variable.waiting_for_momentum | talent.unbound_chaos.enabled & buff.unbound_chaos.up) & (charges = 2 | (raid_event.movement.in> 10 & raid_event.adds.in> 10))
                // fel_barrage,if= active_enemies > desired_targets | raid_event.adds.in> 30
                if (Talent_FelBarrage && UseFelBarrage == "On" && (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber) && MeleeRange)
                {
                    CastSpell(Fel_Barrage);

                }
                // death_sweep,if= variable.blade_dance
                if (API.PlayerLevel >= 12 && API.PlayerHasBuff(Metamorphosis) && BladeDance && BladeDanceCost && MeleeRange)
                {
                    CastSpell(Death_Sweep);

                }
                // immolation_aura
                if (API.PlayerLevel >= 18 && MeleeRange)
                {
                    CastSpell(Immolation_Aura);

                }
                // glaive_tempest,if= !variable.waiting_for_momentum & (active_enemies > desired_targets | raid_event.adds.in> 10)
                if (!waiting_for_momentum && (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber) && Talent_GlaiveTempest && API.PlayerFury >= 30 && MeleeRange)
                {
                    CastSpell(Glaive_Tempest);

                }
                // throw_glaive,if= conduit.serrated_glaive.enabled & cooldown.eye_beam.remains < 6 & !buff.metamorphosis.up & !debuff.exposed_wound.up

                // eye_beam,if= active_enemies > 1 & (!raid_event.adds.exists | raid_event.adds.up) & !variable.waiting_for_momentum
                if (API.PlayerLevel >= 11 && API.PlayerUnitInMeleeRangeCount > 1 && !waiting_for_momentum && !API.PlayerIsMoving && (UseEyeBeam == "always" || (UseEyeBeam == "with Cooldowns" && IsCooldowns)) && API.PlayerFury >= 30 && MeleeRange)
                {
                    CastSpell(Eye_Beam);

                }
                // blade_dance,if= variable.blade_dance
                if (!API.PlayerHasBuff(Metamorphosis) && API.PlayerLevel >= 12 && BladeDance && BladeDanceCost && MeleeRange)
                {
                    CastSpell(Blade_Dance);

                }
                // felblade,if= fury.deficit >= 40
                if (API.TargetRange <= 15 && Talent_Felblade && fury_deficit >= 40)
                {
                    CastSpell(Felblade);

                }
                // eye_beam,if= !talent.blind_fury.enabled & !variable.waiting_for_essence_break & raid_event.adds.in> cooldown
                if (API.PlayerLevel >= 11 && !Talent_BlindFury && !waiting_for_essence_break && !API.PlayerIsMoving && (UseEyeBeam == "always" || (UseEyeBeam == "with Cooldowns" && IsCooldowns)) && API.PlayerFury >= 30 && MeleeRange)
                {
                    CastSpell(Eye_Beam);

                }
                // annihilation,if= (talent.demon_blades.enabled | !variable.waiting_for_momentum | fury.deficit < 30 | buff.metamorphosis.remains < 5) & !variable.pooling_for_blade_dance & !variable.waiting_for_essence_break
                if (API.PlayerLevel >= 14 && (Talent_DemonBlades || !waiting_for_momentum || fury_deficit < 30 || API.PlayerBuffTimeRemaining(Metamorphosis) < 500) && !BladeDance && !waiting_for_essence_break && API.PlayerFury >= 40 && API.PlayerHasBuff(Metamorphosis) && MeleeRange)
                {
                    CastSpell(Annihilation);

                }
                // chaos_strike,if= (talent.demon_blades.enabled | !variable.waiting_for_momentum | fury.deficit < 30) & !variable.pooling_for_meta & !variable.pooling_for_blade_dance & !variable.waiting_for_essence_break
                if (API.PlayerLevel >= 8 && (Talent_DemonBlades || !waiting_for_momentum || fury_deficit < 30) && !PoolingForBladeDance && !waiting_for_essence_break && !waiting_for_essence_break && API.PlayerFury >= 40 && !API.PlayerHasBuff(Metamorphosis) && MeleeRange)
                {
                    CastSpell(Chaos_Strike);

                }
                // eye_beam,if= talent.blind_fury.enabled & raid_event.adds.in> cooldown
                if (API.PlayerLevel >= 11 && Talent_BlindFury && !waiting_for_essence_break && !API.PlayerIsMoving && (UseEyeBeam == "always" || (UseEyeBeam == "with Cooldowns" && IsCooldowns)) && API.PlayerFury >= 30 && MeleeRange)
                {
                    CastSpell(Eye_Beam);

                }
                // demons_bite
                if (API.PlayerLevel >= 8 && !Talent_DemonBlades && MeleeRange)
                {
                    CastSpell(Demons_Bite);

                }
                // fel_rush,if= !talent.momentum.enabled & raid_event.movement.in> charges * 10 & talent.demon_blades.enabled
                // felblade,if= movement.distance > 15 | buff.out_of_range.up
                if (API.TargetRange <= 15 && Talent_Felblade && fury_deficit >= 40)
                {
                    CastSpell(Felblade);

                }
                // fel_rush,if= movement.distance > 15 | (buff.out_of_range.up & !talent.momentum.enabled)
                // vengeful_retreat,if= movement.distance > 15
                // throw_glaive,if= talent.demon_blades.enabled
                if (API.PlayerLevel >= 10 && UseThrowGlaive == "On" && API.TargetRange <= 30 && (Talent_DemonBlades || !MeleeRange))
                {
                    CastSpell(Throw_Glaive);
                }
                #endregion
            }
        }

        public override void OutOfCombatPulse()
        {
        }





    }
}
