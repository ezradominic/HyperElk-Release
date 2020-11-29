
namespace HyperElk.Core
{
    public class VengeanceDemonHunter : CombatRoutine
    {

        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        //Spells,Buffs,Debuffs
        private string Infernal_Strike = "Infernal Strike";
        private string Throw_Glaive = "Throw Glaive";
        private string Shear = "Shear";
        private string Demon_Spikes = "Demon Spikes";
        private string Metamorphosis = "Metamorphosis";
        private string Fiery_Brand = "Fiery Brand";
        private string Soul_Cleave = "Soul Cleave";
        private string Immolation_Aura = "Immolation Aura";
        private string Sigil_of_Flame = "Sigil of Flame";
        private string Fel_Devastation = "Fel Devastation";
        private string Fracture = "Fracture";
        private string Spirit_Bomb = "Spirit Bomb";
        private string Disrupt = "Disrupt";
        private string Sigil_of_Chains = "Sigil of Chains";
        private string Soul_Barrier = "Soul Barrier";
        private string Bulk_Extraction = "Bulk Extraction";
        private string Frailty = "Frailty";
        private string Soul_Fragments = "Soul Fragments";
        private string SinfulBrand = "Sinful Brand";
        private string TheHunt = "The Hunt";
        private string fodder_to_the_flame = "Fodder to the Flame";
        private string elysian_decree = "Elysian Decree";

        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool MeleeRange => API.TargetRange < 6;
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");

        private int Current_Soul_Fragments => API.PlayerBuffStacks(Soul_Fragments);

        //Talents
        private bool Talent_Felblade => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_SpiritBomb => API.PlayerIsTalentSelected(3, 3);
        private bool Talent_Fracture => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_SoulBarrier => API.PlayerIsTalentSelected(6, 3);
        private bool Talent_Bulk_Extraction => API.PlayerIsTalentSelected(7, 3);
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, false, false);
        }



        //CBProperties
        string[] MetamorphosisList = new string[] { "always", "with Cooldowns" };
        string[] Throw_GlaiveList = new string[] { "On", "Off" };
        string[] SigilofFlameList = new string[] { "On", "Off" };
        string[] InfernalStrikeList = new string[] { "On", "Off" };
        string[] FelDevastationList = new string[] { "always", "with Cooldowns" };
        string[] BulkExtractionList = new string[] { "always", "with Cooldowns" };


        private int MetamorphosisLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Metamorphosis)];
        private int DemonSpikes1LifePercent => percentListProp[CombatRoutine.GetPropertyInt(Demon_Spikes)];
        private int DemonSpikes2LifePercent => percentListProp[CombatRoutine.GetPropertyInt(Demon_Spikes+"2")];
        private int SoulBarrierLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Soul_Barrier)];
        private int FieryBrandLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Fiery_Brand)];
        private int SoulFragmentCount => CombatRoutine.GetPropertyInt("SoulFragmentCount");

        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseMetamorphosis => MetamorphosisList[CombatRoutine.GetPropertyInt(Metamorphosis)];
        private string UseThrowGlaive => Throw_GlaiveList[CombatRoutine.GetPropertyInt(Throw_Glaive)];
        private string UseSigilofFlame => SigilofFlameList[CombatRoutine.GetPropertyInt(Sigil_of_Flame)];
        private string UseInfernalStrike => InfernalStrikeList[CombatRoutine.GetPropertyInt(Infernal_Strike)];
        private string UseFelDevastation => FelDevastationList[CombatRoutine.GetPropertyInt(Fel_Devastation)];
        private string UseBulkExtraction => BulkExtractionList[CombatRoutine.GetPropertyInt(Bulk_Extraction)];

        private float fury_deficit => API.PlayeMaxFury - API.PlayerFury;
        //pooling_for_meta,value=!talent.demonic.enabled&cooldown.metamorphosis.remains<6&fury.deficit>30




        public override void Initialize()
        {
            CombatRoutine.Name = "Vengeance DH by Vec";
            API.WriteLog("Welcome to Vengeance DH Rotation");
            API.WriteLog("Metamorphosis Macro : /cast [@player] Metamorphosis");

            //Spells
            CombatRoutine.AddSpell("Infernal Strike", "NumPad2");
            CombatRoutine.AddSpell("Throw Glaive", "D6");
            CombatRoutine.AddSpell("Shear", "D2");
            CombatRoutine.AddSpell("Demon Spikes", "NumPad3");
            CombatRoutine.AddSpell("Metamorphosis", "D8");
            CombatRoutine.AddSpell("Fiery Brand", "D9");
            CombatRoutine.AddSpell("Soul Cleave", "D3");
            CombatRoutine.AddSpell("Immolation Aura", "D4");
            CombatRoutine.AddSpell("Sigil of Flame", "D7");
            CombatRoutine.AddSpell("Fel Devastation", "D5");
            CombatRoutine.AddSpell("Fracture", "D2");
            CombatRoutine.AddSpell("Spirit Bomb", "Oem6");
            CombatRoutine.AddSpell("Disrupt", "NumPad1");
            CombatRoutine.AddSpell("Sigil of Chains", "F1");
            CombatRoutine.AddSpell("Soul Barrier", "F2");
            CombatRoutine.AddSpell("Bulk Extraction", "F3");
            CombatRoutine.AddSpell("Felblade", "NumPad2");
            CombatRoutine.AddSpell(SinfulBrand, "NumPad6");
            CombatRoutine.AddSpell(TheHunt, "NumPad6");
            CombatRoutine.AddSpell(fodder_to_the_flame, "NumPad6");
            CombatRoutine.AddSpell(elysian_decree, "NumPad6");

            //Buffs
            CombatRoutine.AddBuff("Demon Spikes");
            CombatRoutine.AddBuff("Soul Fragments");
            CombatRoutine.AddBuff("Metamorphosis");
            CombatRoutine.AddBuff("Fiery Brand");

            //Debuffs
            CombatRoutine.AddDebuff("Frailty");
            CombatRoutine.AddDebuff("Fiery Brand");
            CombatRoutine.AddDebuff(SinfulBrand);
            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");

            //Settings
            
            CombatRoutine.AddProp(Metamorphosis, "Use " + Metamorphosis, MetamorphosisList, "Use " + Metamorphosis + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Throw_Glaive, "Use " + Throw_Glaive, Throw_GlaiveList, "Use " + Throw_Glaive + "On, Off", "General", 0);
            CombatRoutine.AddProp(Sigil_of_Flame, "Use " + Sigil_of_Flame, SigilofFlameList, "Use " + Sigil_of_Flame + "On, Off", "Cooldowns", 0);
            CombatRoutine.AddProp(Infernal_Strike, "Use " + Infernal_Strike, InfernalStrikeList, "Use " + Infernal_Strike + "On, Off", "Cooldowns", 0);
            CombatRoutine.AddProp(Fel_Devastation, "Use " + Fel_Devastation, FelDevastationList, "Use " + Fel_Devastation + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Bulk_Extraction, "Use " + Bulk_Extraction, BulkExtractionList, "Use " + Bulk_Extraction + "always, with Cooldowns", "Cooldowns", 0);

            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Metamorphosis, "Use " + Metamorphosis + " below:", percentListProp, "Life percent at which " + Metamorphosis + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Demon_Spikes, "Use " + Demon_Spikes + "1st Charge" + " below:", percentListProp, "Life percent at which " + Demon_Spikes + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(Demon_Spikes+"2", "Use " + Demon_Spikes + "2nd Charge" + " below:", percentListProp, "Life percent at which " + Demon_Spikes + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(Soul_Barrier, "Use " + Soul_Barrier + " below:", percentListProp, "Life percent at which " + Soul_Barrier + " is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp("SoulFragmentCount", "Use Soul Barrier Soul Fragments Count", 4, "How many Soul Fragments to use Soul Barrier", "Defense");
            CombatRoutine.AddProp(Fiery_Brand, "Use " + Fiery_Brand + " below:", percentListProp, "Life percent at which " + Fiery_Brand + " is used, set to 0 to disable", "Defense", 4);

            
        }

        public override void Pulse()
        {
            if (API.PlayerIsMounted || API.PlayerIsCasting || API.PlayerIsChanneling)
            {
                return;
            }
            if (API.CanCast(Metamorphosis) && API.PlayerHealthPercent <= MetamorphosisLifePercent)
            {
                API.CastSpell(Metamorphosis);
                return;
            }

        }
        public override void CombatPulse()
        {

            if (isInterrupt && API.CanCast(Disrupt) && MeleeRange && PlayerLevel >= 29)
            {
                API.CastSpell(Disrupt);
                return;
            }
            //actions +=/ consume_magic
         /*   if (API.CanCast("Consume Magic") && DispelWhiteList && API.TargetRange <= 5)
            {
                API.CastSpell("Consume Magic");
                return;
            }*/
            //actions +=/ call_action_list,name = brand

            //actions.brand = sigil_of_flame,if= cooldown.fiery_brand.remains < 2
            if (API.CanCast("Sigil of Flame") && UseSigilofFlame == "On" && API.SpellCDDuration("Fiery Brand") < 200 && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Sigil of Flame");
                return;
            }
            //actions.brand +=/ infernal_strike,if= cooldown.fiery_brand.remains = 0
            if (API.CanCast("Infernal Strike") && API.SpellCharges(Infernal_Strike) >= 1 && API.SpellChargeCD(Infernal_Strike) < 150 && API.LastSpellCastInGame != Infernal_Strike && API.PlayerCurrentCastSpellID != 189110 && UseInfernalStrike == "On" && API.CanCast("Fiery Brand") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Infernal Strike");
                return;
            }
            //actions.brand +=/ fiery_brand
            if (API.CanCast("Fiery Brand") && API.PlayerHealthPercent <= FieryBrandLifePercent && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Fiery Brand");
                return;
            }
            //actions.brand +=/ immolation_aura,if= dot.fiery_brand.ticking
            if (API.CanCast("Immolation Aura") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 8) && API.TargetHasDebuff("Fiery Brand"))
            {
                API.CastSpell("Immolation Aura");
                return;
            }
            //actions.brand +=/ infernal_strike,if= dot.fiery_brand.ticking
            if (API.CanCast("Infernal Strike") && API.SpellCharges(Infernal_Strike) >= 1 && API.SpellChargeCD(Infernal_Strike) < 150 && API.LastSpellCastInGame != Infernal_Strike && API.PlayerCurrentCastSpellID != 189110 && UseInfernalStrike == "On" && API.TargetHasDebuff("Fiery Brand") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Infernal Strike");
                return;
            }
            //actions.brand +=/ sigil_of_flame,if= dot.fiery_brand.ticking
            if (API.CanCast("Sigil of Flame") && UseSigilofFlame == "On" && API.TargetHasDebuff("Fiery Brand") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Sigil of Flame");
                return;
            }

            //actions +=/ call_action_list,name = defensives
            //actions.defensives +=/ metamorphosis
            if (API.CanCast("Metamorphosis") && (UseMetamorphosis == "always" || UseMetamorphosis == "with Cooldowns" && IsCooldowns) && API.PlayerHealthPercent <= MetamorphosisLifePercent)
            {
                API.CastSpell("Metamorphosis");
            }
            //actions.defensives = demon_spikes
            if (API.CanCast(Demon_Spikes) && API.SpellCharges(Demon_Spikes) >= 2 && API.PlayerHealthPercent <= DemonSpikes1LifePercent && API.PlayerLevel >= 14)
            {
                API.CastSpell(Demon_Spikes);
                return;
            }
            if (API.CanCast(Demon_Spikes) && API.SpellCharges(Demon_Spikes) >= 1 && !API.PlayerHasBuff(Demon_Spikes) && API.PlayerHealthPercent <= DemonSpikes2LifePercent && API.PlayerLevel >= 14)
            {
                API.CastSpell(Demon_Spikes);
                return;
            }
            if (API.CanCast("Soul Barrier") && Current_Soul_Fragments >= SoulFragmentCount && API.PlayerHealthPercent <= SoulBarrierLifePercent && Talent_SoulBarrier)
            {
                API.CastSpell("Soul Barrier");
                return;
            }
            //actions.defensives +=/ fiery_brand
            if (API.CanCast("Fiery Brand") && API.PlayerHealthPercent < FieryBrandLifePercent && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Fiery Brand");
                return;
            }
            // actions +=/ call_action_list,name = cooldowns
            /*
actions.cooldowns = potion
actions.cooldowns +=/ concentrated_flame,if= (!dot.concentrated_flame_burn.ticking & !action.concentrated_flame.in_flight | full_recharge_time < gcd.max)
actions.cooldowns +=/ worldvein_resonance,if= buff.lifeblood.stack < 3
actions.cooldowns +=/ memory_of_lucid_dreams
# Default fallback for usable essences.
actions.cooldowns +=/ heart_essence
actions.cooldowns +=/ use_item,effect_name = cyclotronic_blast,if= buff.memory_of_lucid_dreams.down
actions.cooldowns +=/ use_item,name = ashvanes_razor_coral,if= debuff.razor_coral_debuff.down | debuff.conductive_ink_debuff.up & target.health.pct < 31 | target.time_to_die < 20
# Default fallback for usable items.
actions.cooldowns +=/ use_items*/
            //apl_cooldown->add_action("sinful_brand,if=!dot.sinful_brand.ticking");
            if (!API.SpellISOnCooldown(SinfulBrand) && !TargetHasDebuff(SinfulBrand) && API.TargetRange <= 30 && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
            {
                API.CastSpell(SinfulBrand);
                return;
            }
            //apl_cooldown->add_action("the_hunt");
            if (!API.SpellISOnCooldown(TheHunt) && API.TargetRange <= 50 && PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
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
            //actions +=/ call_action_list,name = normal
            //actions.normal = infernal_strike
            if (API.CanCast("Infernal Strike") && API.SpellCharges(Infernal_Strike) >= 1 && API.SpellChargeCD(Infernal_Strike) < 150 && API.LastSpellCastInGame != Infernal_Strike && API.PlayerCurrentCastSpellID != 189110 && UseInfernalStrike == "On" && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Infernal Strike");
                return;
            }
            //actions.normal +=/ bulk_extraction
            if (API.CanCast("Bulk Extraction") && (UseBulkExtraction == "always" || UseBulkExtraction == "with Cooldowns" && IsCooldowns) && Talent_Bulk_Extraction && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Bulk Extraction");
                return;
            }
            //actions.normal +=/ spirit_bomb,if= ((buff.metamorphosis.up & soul_fragments >= 3) | soul_fragments >= 4)
            if (API.CanCast("Spirit Bomb") && API.PlayerFury >= 30 && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5) && Talent_SpiritBomb && ((API.PlayerHasBuff("Metamorphosis") && Current_Soul_Fragments == 3) || Current_Soul_Fragments >= 4))
            {
                API.CastSpell("Spirit Bomb");
                return;
            }
            //actions.normal +=/ soul_cleave,if= (!talent.spirit_bomb.enabled & ((buff.metamorphosis.up & soul_fragments >= 3) | soul_fragments >= 4))
            if (API.CanCast("Soul Cleave") && API.PlayerFury >= 30 && (!Talent_SpiritBomb && ((API.PlayerHasBuff("Metamorphosis") && Current_Soul_Fragments >= 3) || Current_Soul_Fragments >= 4 || API.PlayerFury >= 100)) && API.TargetRange <= 5)
            {
                API.CastSpell("Soul Cleave");
                return;
            }
            //actions.normal +=/ soul_cleave,if= talent.spirit_bomb.enabled & soul_fragments = 0
            if (API.CanCast("Soul Cleave") && API.PlayerFury >= 30 && Talent_SpiritBomb && Current_Soul_Fragments == 0 && API.TargetRange <= 5)
            {
                API.CastSpell("Soul Cleave");
                return;
            }
            //actions.normal +=/ immolation_aura,if= pain <= 90
            if (API.CanCast("Immolation Aura") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 8) && API.PlayerFury <= 90)
            {
                API.CastSpell("Immolation Aura");
                return;
            }
            //actions.normal +=/ felblade,if= pain <= 70
            if (API.CanCast("Felblade") && Talent_Felblade && API.PlayerFury <= 70 && API.TargetRange <= 15)
            {
                API.CastSpell("Felblade");
                return;
            }
            //actions.normal +=/ fracture,if= soul_fragments <= 3
            if (API.CanCast("Fracture") && Talent_Fracture && (Current_Soul_Fragments <= 3 || API.PlayerFury < 30) && API.TargetRange <= 5)
            {
                API.CastSpell("Fracture");
                return;
            }
            //actions.normal +=/ fel_devastation
            if (API.CanCast("Fel Devastation")  && (UseFelDevastation == "with Cooldowns" && IsCooldowns || UseFelDevastation == "always") && API.PlayerFury >= 50 && API.TargetRange >= 1)
            {
                API.CastSpell("Fel Devastation");
                return;
            }
            //actions.normal +=/ sigil_of_flame
            if (API.CanCast("Sigil of Flame") && UseSigilofFlame == "On" && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
            {
                API.CastSpell("Sigil of Flame");
                return;
            }
            //actions.normal +=/ shear
            if (API.CanCast("Shear") && !Talent_Fracture && API.TargetRange <= 5)
            {
                API.CastSpell("Shear");
                return;
            }
            //actions.normal +=/ throw_glaive
            if (API.CanCast("Throw Glaive") && UseThrowGlaive == "On" && API.TargetRange <= 30)
            {
                API.CastSpell("Throw Glaive");
                return;
            }
        }

        public override void OutOfCombatPulse()
        {
        }





    }
}
