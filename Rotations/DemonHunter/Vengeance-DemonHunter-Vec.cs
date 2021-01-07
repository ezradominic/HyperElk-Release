
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
        private string Fel_Bombardment = "Fel Bombardment";
        private string SigilofSilence = "Sigil of Silence";
        private string SigilofMisery = "Sigil of Misery";
        private string DemonMuzzle = "Demon Muzzle";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool MeleeRange => API.TargetRange < 6;
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");

        private int Current_Soul_Fragments => API.PlayerBuffStacks(Soul_Fragments);

        //Talents
        private bool Talent_agonizing_flames => API.PlayerIsTalentSelected(1, 2);
        private bool Talent_Felblade => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_burning_alive => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_charred_flesh => API.PlayerIsTalentSelected(3, 2);
        private bool Talent_SpiritBomb => API.PlayerIsTalentSelected(3, 3);
        private bool Talent_Fracture => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_Demonic => API.PlayerIsTalentSelected(6, 2);
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
        private bool Playeriscasting => API.PlayerCurrentCastTimeRemaining > 40;
        //( "variable,name=brand_build,value=talent.agonizing_flames.enabled&talent.burning_alive.enabled&talent.charred_flesh.enabled" );
        private bool brand_build => Talent_agonizing_flames&&Talent_burning_alive&&Talent_charred_flesh;


        //CBProperties
        string[] MetamorphosisList = new string[] { "always", "with Cooldowns" };
        string[] Throw_GlaiveList = new string[] { "On", "Off" };
        string[] SigilofFlameList = new string[] { "On", "Off" };
        string[] InfernalStrikeList = new string[] { "On", "Off" };
        string[] FelDevastationList = new string[] { "always", "with Cooldowns" };
        string[] BulkExtractionList = new string[] { "always", "with Cooldowns" };

        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };

        private int MetamorphosisLifePercent => percentListProp[CombatRoutine.GetPropertyInt("MetamorphosisLife")];
        private int DemonSpikes1LifePercent => percentListProp[CombatRoutine.GetPropertyInt(Demon_Spikes)];
        private int DemonSpikes2LifePercent => percentListProp[CombatRoutine.GetPropertyInt(Demon_Spikes+"2")];
        private int SoulBarrierLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Soul_Barrier)];
        private int FieryBrandLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Fiery_Brand)];
        private int SoulFragmentCount => CombatRoutine.GetPropertyInt("SoulFragmentCount");

        private bool razelikhs_defilement_equipped => CombatRoutine.GetPropertyBool("razelikhs_defilement");
        private bool UseSigilofSilence => CombatRoutine.GetPropertyBool(SigilofSilence);
        private bool UseSigilofMisery => CombatRoutine.GetPropertyBool(SigilofMisery);
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseMetamorphosis => MetamorphosisList[CombatRoutine.GetPropertyInt(Metamorphosis)];
        private string UseThrowGlaive => Throw_GlaiveList[CombatRoutine.GetPropertyInt(Throw_Glaive)];
        private string UseSigilofFlame => SigilofFlameList[CombatRoutine.GetPropertyInt(Sigil_of_Flame)];
        private string UseInfernalStrike => InfernalStrikeList[CombatRoutine.GetPropertyInt(Infernal_Strike)];
        private string UseFelDevastation => FelDevastationList[CombatRoutine.GetPropertyInt(Fel_Devastation)];
        private string UseBulkExtraction => BulkExtractionList[CombatRoutine.GetPropertyInt(Bulk_Extraction)];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];

        private float fury_deficit => API.PlayeMaxFury - API.PlayerFury;
        //pooling_for_meta,value=!talent.demonic.enabled&cooldown.metamorphosis.remains<6&fury.deficit>30




        public override void Initialize()
        {
            CombatRoutine.Name = "Vengeance DH by Vec";
            API.WriteLog("Welcome to Vengeance DH Rotation");
            API.WriteLog("Metamorphosis Macro : /cast [@player] Metamorphosis");

            //Spells
            CombatRoutine.AddSpell("Infernal Strike",189110, "NumPad2");
            CombatRoutine.AddSpell("Throw Glaive",204157, "D6");
            CombatRoutine.AddSpell("Shear",203782, "D2");
            CombatRoutine.AddSpell("Demon Spikes",203720, "NumPad3");
            CombatRoutine.AddSpell("Metamorphosis",187827, "D8");
            CombatRoutine.AddSpell("Fiery Brand",204021, "D9");
            CombatRoutine.AddSpell("Soul Cleave",228477, "D3");
            CombatRoutine.AddSpell("Immolation Aura",258920, "D4");
            CombatRoutine.AddSpell("Sigil of Flame",204596, "D7");
            CombatRoutine.AddSpell("Fel Devastation",212084, "D5");
            CombatRoutine.AddSpell("Fracture",263642, "D2");
            CombatRoutine.AddSpell("Spirit Bomb",247454, "Oem6");
            CombatRoutine.AddSpell("Disrupt",183752, "NumPad1");
            CombatRoutine.AddSpell("Sigil of Chains",202138, "F1");
            CombatRoutine.AddSpell("Soul Barrier",263648, "F2");
            CombatRoutine.AddSpell("Bulk Extraction",320341, "F3");
            CombatRoutine.AddSpell("Felblade",232893, "NumPad2");
            CombatRoutine.AddSpell(SinfulBrand, 317009, "NumPad6");
            CombatRoutine.AddSpell(TheHunt, 323639, "NumPad6");
            CombatRoutine.AddSpell(fodder_to_the_flame, 329554, "NumPad6");
            CombatRoutine.AddSpell(elysian_decree, 306830, "NumPad6");
            CombatRoutine.AddSpell(SigilofSilence,202137, "NumPad7");
            CombatRoutine.AddSpell(SigilofMisery,207684, "NumPad8");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            CombatRoutine.AddMacro(PhialofSerenity, "F11");
            CombatRoutine.AddMacro(SpiritualHealingPotion, "F12");
            //Buffs
            CombatRoutine.AddBuff("Demon Spikes",203819);
            CombatRoutine.AddBuff("Soul Fragments",203981);
            CombatRoutine.AddBuff("Metamorphosis",187827);
            CombatRoutine.AddBuff(Fel_Bombardment, 337775);
            CombatRoutine.AddBuff(Immolation_Aura,258920);

            //Debuffs
            CombatRoutine.AddDebuff("Frailty", 247456);
            CombatRoutine.AddDebuff("Fiery Brand", 207771);
            CombatRoutine.AddDebuff(SinfulBrand, 317009);
            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);
            CombatRoutine.AddConduit(DemonMuzzle);
            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            AddProp("razelikhs_defilement", "have razelikhs_defilement", false, "if you have razelikhs_defilement", "Legendary");
            

            //Settings

            CombatRoutine.AddProp(Metamorphosis, "Use " + Metamorphosis, MetamorphosisList, "Use " + Metamorphosis + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Throw_Glaive, "Use " + Throw_Glaive, Throw_GlaiveList, "Use " + Throw_Glaive + "On, Off", "General", 0);
            CombatRoutine.AddProp(Sigil_of_Flame, "Use " + Sigil_of_Flame, SigilofFlameList, "Use " + Sigil_of_Flame + "On, Off", "Cooldowns", 0);
            CombatRoutine.AddProp(Infernal_Strike, "Use " + Infernal_Strike, InfernalStrikeList, "Use " + Infernal_Strike + "On, Off", "Cooldowns", 0);
            CombatRoutine.AddProp(Fel_Devastation, "Use " + Fel_Devastation, FelDevastationList, "Use " + Fel_Devastation + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Bulk_Extraction, "Use " + Bulk_Extraction, BulkExtractionList, "Use " + Bulk_Extraction + "always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(SigilofSilence, SigilofSilence, true, "Enable if you want to let the rotation use" + SigilofSilence, "Generic");
            CombatRoutine.AddProp(SigilofMisery, SigilofMisery, true, "Enable if you want to let the rotation use" + SigilofMisery, "Generic");
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp("MetamorphosisLife", "Use " + Metamorphosis + " below:", percentListProp, "Life percent at which " + Metamorphosis + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Demon_Spikes, "Use " + Demon_Spikes + "1st Charge" + " below:", percentListProp, "Life percent at which " + Demon_Spikes + " is used, set to 0 to disable", "Defense", 8);
            CombatRoutine.AddProp(Demon_Spikes+"2", "Use " + Demon_Spikes + "2nd Charge" + " below:", percentListProp, "Life percent at which " + Demon_Spikes + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(Soul_Barrier, "Use " + Soul_Barrier + " below:", percentListProp, "Life percent at which " + Soul_Barrier + " is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp("SoulFragmentCount", "Use Soul Barrier Soul Fragments Count", 4, "How many Soul Fragments to use Soul Barrier", "Defense");
            CombatRoutine.AddProp(Fiery_Brand, "Use " + Fiery_Brand + " below:", percentListProp, "Life percent at which " + Fiery_Brand + " is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);

        }

        public override void Pulse()
        {

        }
        public override void CombatPulse()
        {
            if (!API.PlayerIsMounted && !Playeriscasting && !API.PlayerSpellonCursor)
            {

                if (API.CanCast(Metamorphosis) && API.PlayerHealthPercent <= MetamorphosisLifePercent)
                {
                    API.CastSpell(Metamorphosis);
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
                // apl_default->add_action(this, "Disrupt");
                if (isInterrupt && API.CanCast(Disrupt) && MeleeRange && PlayerLevel >= 29)
                {
                    API.CastSpell(Disrupt);
                    return;
                }
                if (API.TargetCanInterrupted && UseSigilofSilence && !API.CanCast(Disrupt) && API.TargetCurrentCastTimeRemaining >200 && API.TargetCurrentCastTimeRemaining < 300 && API.CanCast(SigilofSilence) && MeleeRange)
                {
                    API.CastSpell(SigilofSilence);
                    return;
                }
                if (UseSigilofSilence && API.CanCast(SigilofSilence) && API.PlayerIsConduitSelected(DemonMuzzle) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.TargetTimeToDie > 200 && MeleeRange)
                {
                    API.CastSpell(SigilofSilence);
                    return;
                }
                if (API.TargetCanInterrupted && UseSigilofMisery && !API.CanCast(Disrupt) && !API.CanCast(SigilofSilence) && API.TargetCurrentCastTimeRemaining > 200 && API.TargetCurrentCastTimeRemaining < 300 && API.CanCast(SigilofMisery) && MeleeRange)
                {
                    API.CastSpell(SigilofMisery);
                    return;
                }
                // apl_default->add_action(this, "Consume Magic");
                /*   if (API.CanCast("Consume Magic") && DispelWhiteList && API.TargetRange <= 5)
   {
       API.CastSpell("Consume Magic");
       return;
   }*/
                // apl_default->add_action(this, "Throw Glaive", "if=buff.fel_bombardment.stack=5&(buff.immolation_aura.up|!buff.metamorphosis.up)");
                if (API.CanCast("Throw Glaive") && API.PlayerBuffStacks(Fel_Bombardment) == 5 && (PlayerHasBuff(Immolation_Aura) || !PlayerHasBuff(Metamorphosis)) && API.TargetRange <= 30)
                {
                    API.CastSpell("Throw Glaive");
                    return;
                }
                // apl_default->add_action("call_action_list,name=brand,if=variable.brand_build");
                if (brand_build)
                {
                    // apl_brand->add_action(this, "Fiery Brand");
                    if (API.CanCast("Fiery Brand") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
                    {
                        API.CastSpell("Fiery Brand");
                        return;
                    }
                    //apl_brand->add_action(this, "Immolation Aura", "if=dot.fiery_brand.ticking");
                    if (API.CanCast("Immolation Aura") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 8) && API.TargetHasDebuff("Fiery Brand"))
                    {
                        API.CastSpell("Immolation Aura");
                        return;
                    }
                }
                // apl_default->add_action("call_action_list,name=defensives");
                ////apl_defensives->add_action(this, "Demon Spikes");
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
                //apl_defensives->add_action(this, "Metamorphosis", "if=!(talent.demonic.enabled)&(!covenant.venthyr.enabled|!dot.sinful_brand.ticking)|target.time_to_die<15");
                if (API.CanCast("Metamorphosis") && !((Talent_Demonic) && (PlayerCovenantSettings != "Venthyr" || !TargetHasDebuff(SinfulBrand) || API.TargetTimeToDie < 1500)) && (UseMetamorphosis == "always" || UseMetamorphosis == "with Cooldowns" && IsCooldowns))
                {
                    API.CastSpell("Metamorphosis");
                }
                //apl_defensives->add_action(this, "Fiery Brand");
                if (API.CanCast("Fiery Brand") && API.PlayerHealthPercent <= FieryBrandLifePercent && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
                {
                    API.CastSpell("Fiery Brand");
                    return;
                }
                // apl_default->add_action("call_action_list,name=cooldowns");
                // cooldowns->add_action("potion");
                // cooldowns->add_action("use_items", "Default fallback for usable items.");
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && MeleeRange)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && MeleeRange)
                {
                    API.CastSpell("Trinket2");
                }
                // cooldowns->add_action("sinful_brand,if=!dot.sinful_brand.ticking");
                if (!API.SpellISOnCooldown(SinfulBrand) && !TargetHasDebuff(SinfulBrand) && API.TargetRange <= 30 && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
                {
                    API.CastSpell(SinfulBrand);
                    return;
                }
                // cooldowns->add_action("the_hunt");
                if (!API.SpellISOnCooldown(TheHunt) && API.TargetRange <= 50 && PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
                {
                    API.CastSpell(TheHunt);
                    return;
                }
                // cooldowns->add_action("fodder_to_the_flame");
                if (!API.SpellISOnCooldown(fodder_to_the_flame) && API.TargetRange <= 30 && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
                {
                    API.CastSpell(fodder_to_the_flame);
                    return;
                }
                // cooldowns->add_action("elysian_decree");
                if (!API.SpellISOnCooldown(elysian_decree) && API.TargetRange <= 30 && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber))
                {
                    API.CastSpell(elysian_decree);
                    return;
                }
                // apl_default->add_action("call_action_list,name=normal");
                //action_priority_list_t* apl_normal = get_action_priority_list("normal", "Normal Rotation");
                // apl_normal->add_action(this, "Infernal Strike");
                if (API.CanCast("Infernal Strike") && API.SpellCharges(Infernal_Strike) >= 1 && API.SpellChargeCD(Infernal_Strike) < 150 && API.LastSpellCastInGame != Infernal_Strike && API.PlayerCurrentCastSpellID != 189110 && UseInfernalStrike == "On" && API.CanCast("Fiery Brand") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
                {
                    API.CastSpell("Infernal Strike");
                    return;
                }
                //apl_normal->add_talent(this, "Bulk Extraction");
                if (API.CanCast("Bulk Extraction") && (UseBulkExtraction == "always" || UseBulkExtraction == "with Cooldowns" && IsCooldowns) && Talent_Bulk_Extraction && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5))
                {
                    API.CastSpell("Bulk Extraction");
                    return;
                }
                //apl_normal->add_talent(this, "Spirit Bomb", "if=((buff.metamorphosis.up&talent.fracture.enabled&soul_fragments>=3)|soul_fragments>=4)");
                if (API.CanCast("Spirit Bomb") && API.PlayerFury >= 30 && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5) && Talent_SpiritBomb && ((API.PlayerHasBuff("Metamorphosis") && Current_Soul_Fragments == 3) || Current_Soul_Fragments >= 4))
                {
                    API.CastSpell("Spirit Bomb");
                    return;
                }
                // apl_normal->add_action(this, "Fel Devastation");
                if (!API.SpellISOnCooldown("Fel Devastation") && (UseFelDevastation == "with Cooldowns" && IsCooldowns || UseFelDevastation == "always") && API.PlayerFury >= 50 && API.TargetRange <= 5)
                {
                    API.CastSpell("Fel Devastation");
                    return;
                }
                // apl_normal->add_action(this, "Soul Cleave", "if=((talent.spirit_bomb.enabled&soul_fragments=0)|!talent.spirit_bomb.enabled)&((talent.fracture.enabled&fury>=55)|(!talent.fracture.enabled&fury>=70)|cooldown.fel_devastation.remains>target.time_to_die|(buff.metamorphosis.up&((talent.fracture.enabled&fury>=35)|(!talent.fracture.enabled&fury>=50))))");
                if (API.CanCast("Soul Cleave") && API.PlayerFury >= 30 && API.TargetRange <= 5 && (((Talent_SpiritBomb && Current_Soul_Fragments == 0) || !Talent_SpiritBomb) && ((Talent_Fracture && API.PlayerFury >= 55) || (!Talent_Fracture && API.PlayerFury >= 70) || API.SpellCDDuration(Fel_Devastation) > API.TargetTimeToDie || (PlayerHasBuff(Metamorphosis) && ((Talent_Fracture && API.PlayerFury >= 35) || (!Talent_Fracture && API.PlayerFury >= 50))))))
                {
                    API.CastSpell("Soul Cleave");
                    return;
                }
                // apl_normal->add_action(this, "Immolation Aura", "if=((variable.brand_build&cooldown.fiery_brand.remains>10)|!variable.brand_build)&fury<=90");
                if (API.CanCast("Immolation Aura") && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 8) && (((brand_build && API.SpellCDDuration(Fiery_Brand) > 1000) || !brand_build) && API.PlayerFury <= 90))
                {
                    API.CastSpell("Immolation Aura");
                    return;
                }
                // apl_normal->add_talent(this, "Felblade", "if=fury<=60");
                if (API.CanCast("Felblade") && Talent_Felblade && API.PlayerFury <= 60 && API.TargetRange <= 15)
                {
                    API.CastSpell("Felblade");
                    return;
                }
                //apl_normal->add_talent(this, "Fracture", "if=((talent.spirit_bomb.enabled&soul_fragments<=3)|(!talent.spirit_bomb.enabled&((buff.metamorphosis.up&fury<=55)|(buff.metamorphosis.down&fury<=70))))");
                if (API.CanCast("Fracture") && Talent_Fracture && API.TargetRange <= 5 && ((Talent_SpiritBomb && Current_Soul_Fragments <= 3) || (!Talent_SpiritBomb && ((PlayerHasBuff(Metamorphosis) && API.PlayerFury <= 55) || (!PlayerHasBuff(Metamorphosis) && API.PlayerFury <= 70)))))
                {
                    API.CastSpell("Fracture");
                    return;
                }
                // apl_normal->add_action(this, "Sigil of Flame", "if=!(covenant.kyrian.enabled&runeforge.razelikhs_defilement.equipped)");
                if (API.CanCast("Sigil of Flame") && UseSigilofFlame == "On" && (API.PlayerUnitInMeleeRangeCount >= 1 || API.TargetRange <= 5) && !(PlayerCovenantSettings == "Kyrian" && razelikhs_defilement_equipped))
                {
                    API.CastSpell("Sigil of Flame");
                    return;
                }
                // apl_normal->add_action(this, "Shear");
                if (API.CanCast("Shear") && !Talent_Fracture && API.TargetRange <= 5)
                {
                    API.CastSpell("Shear");
                    return;
                }
                // apl_normal->add_action(this, "Throw Glaive");
                if (API.CanCast("Throw Glaive") && UseThrowGlaive == "On" && API.TargetRange <= 30)
                {
                    API.CastSpell("Throw Glaive");
                    return;
                }
            }
        }

        public override void OutOfCombatPulse()
        {
        }





    }
}
