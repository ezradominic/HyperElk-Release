using System;
using System.Diagnostics;
using System.Linq;

namespace HyperElk.Core
{
    public class UHDK : CombatRoutine
    {
        private bool SmallCDs => API.ToggleIsEnabled("Small CDs");
        private bool UseDnD => API.ToggleIsEnabled("Use DnD");
        private string ShackletheUnworthy = "Shackle the Unworthy";
        private string SwarmingMist = "Swarming Mist";
        private string DeathsDue = "Death's Due";
        private string AbominationLimb = "Abomination Limb";
        private string UnholyAssault = "Unholy Assault";
        private string Apocalypse = "Apocalypse";
        private string ArmyoftheDead = "Army of the Dead";
        private string Lichborne = "Lichborne";
        private string UnholyStrength = "Unholy Strength";
        private string DeathCoil = "Death Coil";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string DeathGrip = "Death Grip";
        private string Fleshcraft = "Fleshcraft";
        private string UnholyBlight = "Unholy Blight";
        private string ConvocationoftheDead = "Convocation of the Dead";
        //stopwatch
        private readonly Stopwatch Dark_Transformation_Ghoul = new Stopwatch();
        private readonly Stopwatch GargoyleActiveTime = new Stopwatch();
        private readonly Stopwatch ApocGhoulActiveTime = new Stopwatch();
        private readonly Stopwatch ArmyGhoulActiveTime = new Stopwatch();
        //Spells,Buffs,Debuffs


        //Misc
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool MeleeRange => API.TargetRange <= 5;
        private bool InRange => API.TargetRange <= 40;

        private bool PoolingForGargoyle
        {
            get
            {
                if (API.SpellCDDuration("Summon Gargoyle") < 500 && Talent_SummonGargoyle)
                    return true;
                return false;
            }
        }

        private float gcd => API.SpellGCDTotalDuration;


        //Talents
        private bool Talent_InfectedClaws => API.PlayerIsTalentSelected(1, 1);
        private bool Talent_ClawingShadows => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_BurstingSores => API.PlayerIsTalentSelected(2, 1);
        private bool Talent_EbonFever => API.PlayerIsTalentSelected(2, 2);
        private bool Talent_UnholyBlight => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_GripoftheDead => API.PlayerIsTalentSelected(4, 1);
        private bool Talent_HarbingerofDoom => API.PlayerIsTalentSelected(4, 2);
        private bool Talent_SoulReaper => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_DeathPact => API.PlayerIsTalentSelected(5, 3);
        private bool Talent_Pestilence => API.PlayerIsTalentSelected(6, 1);
        private bool Talent_UnholyPact => API.PlayerIsTalentSelected(6, 2);
        private bool Talent_Defile => API.PlayerIsTalentSelected(6, 3);
        private bool Talent_ArmyoftheDamned => API.PlayerIsTalentSelected(7, 1);
        private bool Talent_SummonGargoyle => API.PlayerIsTalentSelected(7, 2);
        private bool Talent_UnholyAssault => API.PlayerIsTalentSelected(7, 3);

        private static void CastSpell(string spell)
        {
            if (API.CanCast(spell))
            {
                API.CastSpell(spell);
                return;
            }
        }
        public static int RuneTimeTo(int numberrunes)
        {
            int[] cooldown = new int[7];
            for (int i = 1; i <= 6; ++i)
            {
                cooldown[i] = API.PlayerRuneCD(i);
            }
            Array.Sort(cooldown);
            var time = cooldown.OrderBy(x => x == 0).ToArray();
            switch (numberrunes - API.PlayerCurrentRunes)
            {
                case 1:
                    return time[0];
                case 2:
                    return time[1];
                case 3:
                    return time[2];
                case 4:
                    return time[3];
                case 5:
                    return time[4];
                case 6:
                    return time[5];
                default: return 0;

            }
        }
        private bool Playeriscasting => API.PlayerCurrentCastTimeRemaining > 40;
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool PetHasBuff(string buff)
        {
            return API.PetHasBuff(buff, false, false);
        }

        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        private static int Festering_Wound_Stacks => API.TargetDebuffStacks("Festering Wound");
        private bool GargoyleActive => GargoyleActiveTime.IsRunning && GargoyleActiveTime.ElapsedMilliseconds <= 30000;
        private bool ApocGhoulActive => ApocGhoulActiveTime.IsRunning && ApocGhoulActiveTime.ElapsedMilliseconds <= 15000;
        private bool ArmyGhoulActive => ApocGhoulActiveTime.IsRunning && ApocGhoulActiveTime.ElapsedMilliseconds <= 30000;
        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };



        private int IceboundFortitudeLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Icebound")];
        private int DeathPactLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Death Pact")];
        private int AntiMagicShellLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Anti-Magic Shell")];
        private int DeathStrikePercent => percentListProp[CombatRoutine.GetPropertyInt("Death Strike")];
        private int DarkSuccorPercent => percentListProp[CombatRoutine.GetPropertyInt("Dark Succor")];
        private int LichborneLifePercent => API.getFromArray(percentListProp, CombatRoutine.GetPropertyInt(Lichborne));

        private string WhenDarkTransformation => CDUsage[CombatRoutine.GetPropertyInt("DarkTransformation")];
        private string UseRaiseAbomination => CDUsage[CombatRoutine.GetPropertyInt("RaiseAbomination")];
        private string UseApocalypse => CDUsage[CombatRoutine.GetPropertyInt("Apocalypse")];
        private int FleshcraftPercent => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private bool UseRaiseDead => CombatRoutine.GetPropertyBool("RaiseDead");
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];

        public override void Initialize()
        {
            CombatRoutine.Name = "Unholy DK by Vec";
            API.WriteLog("Welcome to Unholy DK Rotation");

            //Spells


            CombatRoutine.AddSpell("Outbreak", 77575, "D1");
            CombatRoutine.AddSpell("Soul Reaper", 343294, "D2");
            CombatRoutine.AddSpell("Dark Transformation", 63560, "D3");
            CombatRoutine.AddSpell("Apocalypse", 275699, "D4");
            CombatRoutine.AddSpell(DeathCoil, 47541, "D5");
            CombatRoutine.AddSpell("Death and Decay", 43265, "D6");
            CombatRoutine.AddSpell("Festering Strike", 85948, "D7");
            CombatRoutine.AddSpell("Mind Freeze", 47528, "D8");
            CombatRoutine.AddSpell("Unholy Assault", 207289, "D9");
            CombatRoutine.AddSpell("Chains of Ice", 45524, "F1");
            CombatRoutine.AddSpell("Scourge Strike", 55090, "F2");
            CombatRoutine.AddSpell("Clawing Shadows", 207311, "F2");
            CombatRoutine.AddSpell("Icebound Fortitude", 48792, "F3");
            CombatRoutine.AddSpell("Anti-Magic Shell", 48707, "F4");
            CombatRoutine.AddSpell("Death Strike", 49998, "F5");
            CombatRoutine.AddSpell("Epidemic", 207317, "F6");
            CombatRoutine.AddSpell("Death Pact", 48743, "NumPad4");
            CombatRoutine.AddSpell("Defile", 152280, "D6");
            CombatRoutine.AddSpell("Army of the Dead", 42650, "F8");
            CombatRoutine.AddSpell(UnholyBlight, 115989, "NumPad5");
            CombatRoutine.AddSpell("Summon Gargoyle", 49206, "F7");
            CombatRoutine.AddSpell("Raise Dead", 46584, "D0");
            CombatRoutine.AddSpell("Raise Abomination", 288853, "F7");
            CombatRoutine.AddSpell("Necrotic Strike", 223829, "NumPad6");
            CombatRoutine.AddSpell("Sacrificial Pact", 327574, "NumPad5");
            CombatRoutine.AddSpell(ShackletheUnworthy, 312202, "F11");
            CombatRoutine.AddSpell(SwarmingMist, 311648, "F11");
            CombatRoutine.AddSpell(DeathsDue, 324128, "F11");
            CombatRoutine.AddSpell(AbominationLimb, 315443, "F11");
            CombatRoutine.AddSpell(Lichborne, 49039, "F12");
            CombatRoutine.AddSpell(DeathGrip, 49576, "NumPad1");
            CombatRoutine.AddSpell(Fleshcraft, 324631, "NumPad9");

            //Items
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            CombatRoutine.AddBuff("Dark Succor", 101568);
            CombatRoutine.AddBuff("Sudden Doom", 81340);
            CombatRoutine.AddBuff("Death and Decay", 188290);
            CombatRoutine.AddBuff("Master of Ghouls", 246995);
            CombatRoutine.AddBuff(UnholyStrength, 53365);
            CombatRoutine.AddBuff(UnholyAssault, 207289);
            CombatRoutine.AddBuff(UnholyBlight, 115989);

            CombatRoutine.AddDebuff("Virulent Plague", 191587);
            CombatRoutine.AddDebuff("Festering Wound", 194310);
            CombatRoutine.AddDebuff("Necrotic Wound", 209858);

            CombatRoutine.AddConduit("Convocation of the Dead");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            CombatRoutine.AddMacro(DeathGrip + "MO", "NumPad1");

            //Toggle
            CombatRoutine.AddToggle("Small CDs");
            CombatRoutine.AddToggle("Use DnD");
            CombatRoutine.AddToggle("Mouseover");



            //Settings
            CombatRoutine.AddProp("RaiseDead", "Raise Dead", true, "Should the rotation try to Raise Dead", "Pet");
            CombatRoutine.AddProp("DarkTransformation", "Use " + "Dark Transformation", CDUsage, "Use " + "Dark Transformation" + "On Cooldown, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("RaiseAbomination", "Use " + "Raise Abomination", CDUsage, "Use " + "Raise Abomination" + "On Cooldown, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("Apocalypse", "Use " + "Apocalypse", CDUsage, "Use " + "Apocalypse" + "On Cooldown, with Cooldowns", "Cooldowns", 0);

            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " On Cooldown, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " On Cooldown, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " On Cooldown, with Cooldowns", "Trinkets", 0);

            CombatRoutine.AddProp(Lichborne, Lichborne + " Life Percent", percentListProp, "Life percent at which" + Lichborne + "is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp("Icebound", "Use " + "Icebound Fortitude" + " below:", percentListProp, "Life percent at which " + "Icebound Fortitude" + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp("Death Pact", "Use " + "Death Pact" + " below:", percentListProp, "Life percent at which " + "Death Pact" + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp("Anti-Magic Shell", "Use " + "Anti-Magic Shell" + " below:", percentListProp, "Life percent at which " + "Anti-Magic Shell" + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp("Death Strike", "Use " + "Death Strike High Priority" + " below:", percentListProp, "Life percent at which " + "Death Strike" + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp("Dark Succor", "Use " + "Death Strike Dark Succor" + " below:", percentListProp, "Life percent at which " + "Death Strike" + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
        }

        public override void Pulse()
        {
            if (!Dark_Transformation_Ghoul.IsRunning && API.LastSpellCastInGame == "Dark Transformation") { Dark_Transformation_Ghoul.Start(); API.WriteLog("Ghoul Transformed"); }
            if (Dark_Transformation_Ghoul.IsRunning && Dark_Transformation_Ghoul.ElapsedMilliseconds >= 15000) { Dark_Transformation_Ghoul.Reset(); API.WriteLog("Ghoul Transformation ran off"); }
            if (API.LastSpellCastInGame == "Summon Gargoyle" && !GargoyleActiveTime.IsRunning) { GargoyleActiveTime.Start(); API.WriteLog("Gargoyle is active"); }
            if (GargoyleActiveTime.IsRunning && GargoyleActiveTime.ElapsedMilliseconds > 30000) { GargoyleActiveTime.Reset(); API.WriteLog("Gargoyle ran off"); }
            if ((API.LastSpellCastInGame == Apocalypse) && !ApocGhoulActiveTime.IsRunning) { ApocGhoulActiveTime.Start(); API.WriteLog("Apoc Ghoul is active"); }
            if (ApocGhoulActiveTime.IsRunning && ApocGhoulActiveTime.ElapsedMilliseconds > 15000) { ApocGhoulActiveTime.Reset(); API.WriteLog("Apoc Ghoul ran off"); }
            if ((API.LastSpellCastInGame == ArmyoftheDead) && !ApocGhoulActiveTime.IsRunning) { ArmyGhoulActiveTime.Start(); API.WriteLog("Army Ghoul is active"); }
            if (ApocGhoulActiveTime.IsRunning && ArmyGhoulActiveTime.ElapsedMilliseconds > 15000) { ArmyGhoulActiveTime.Reset(); API.WriteLog("Army Ghoul ran off"); }
            //API.WriteLog("debug" + MultiDot + API.CanCast(Kill_Command) + API.TargetHasDebuff(Kill_Command, false, false)+ " " + API.PlayerUnitInMeleeRangeCount  +" "+ API.PlayerBuffStacks(Predator));

            if (!API.PlayerIsMounted && !Playeriscasting)
            {
                if (IsMouseover)
                {
                    if (IsMouseover && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0 && isMOinRange && API.MouseoverRange >= 10)
                    {
                        if (!API.MacroIsIgnored(DeathGrip + "MO"))
                        {
                            API.CastSpell(DeathGrip + "MO");
                            return;
                        }
                    }
                }
                #region defensives
                if (API.CanCast("Raise Dead") && UseRaiseDead && (!API.PlayerHasPet || API.PetHealthPercent < 1))
                {
                    API.CastSpell("Raise Dead");
                    return;
                }
                if (API.CanCast("Icebound Fortitude") && API.PlayerLevel >= 38 && API.PlayerHealthPercent <= IceboundFortitudeLifePercent)
                {
                    API.CastSpell("Icebound Fortitude");
                    return;
                }
                if (API.CanCast(Lichborne) && API.PlayerIsCC(CCList.FEAR_MECHANIC))
                {
                    API.CastSpell(Lichborne);
                    return;
                }
                if (API.CanCast("Death Pact") && Talent_DeathPact && API.PlayerHealthPercent <= DeathPactLifePercent)
                {
                    API.CastSpell("Death Pact");
                    return;
                }
                if (API.CanCast("Anti-Magic Shell") && API.PlayerLevel >= 14 && API.PlayerHealthPercent <= AntiMagicShellLifePercent)
                {
                    API.CastSpell("Anti-Magic Shell");
                    return;
                }
                if (API.CanCast("Death Strike") && API.PlayerRunicPower >= 45 && API.PlayerHealthPercent <= DeathStrikePercent && MeleeRange)
                {
                    API.CastSpell("Death Strike");
                    return;
                }
                if (API.CanCast("Death Strike") && API.PlayerHasBuff("Dark Succor") && API.PlayerHealthPercent <= DarkSuccorPercent && MeleeRange)
                {
                    API.CastSpell("Death Strike");
                    return;
                }
                #endregion
            }
        }

        public override void CombatPulse()
        {
            if (!API.PlayerIsMounted && !Playeriscasting)
            {
                if (API.CanCast("Mind Freeze") && isInterrupt && MeleeRange)
                {
                    API.CastSpell("Mind Freeze");
                    return;
                }
                if (API.PlayerItemCanUse(PhialofSerenity) && API.PlayerItemRemainingCD(PhialofSerenity) == 0 && !API.MacroIsIgnored(PhialofSerenity) && API.PlayerHealthPercent <= PhialofSerenityLifePercent)
                {
                    API.CastSpell(PhialofSerenity);
                    return;
                }
                if (API.PlayerItemCanUse(SpiritualHealingPotion) && API.PlayerItemRemainingCD(SpiritualHealingPotion) == 0 && !API.MacroIsIgnored(SpiritualHealingPotion) && API.PlayerHealthPercent <= SpiritualHealingPotionLifePercent)
                {
                    API.CastSpell(SpiritualHealingPotion);
                    return;
                }
                if (isRacial && IsCooldowns)
                {
                    //actions +=/ arcane_torrent,if= runic_power.deficit > 65 & (pet.gargoyle.active | !talent.summon_gargoyle.enabled) & rune.deficit >= 5
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Blood Elf" && MeleeRange && (API.PlayerRunicPower <= 35 && (GargoyleActive || !Talent_SummonGargoyle) && API.PlayerCurrentRunes < 1))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions +=/ blood_fury,if= pet.gargoyle.active | buff.unholy_assault.up | talent.army_of_the_damned & pet.apoc_ghoul.active & (pet.army_ghoul.active | cooldown.army_of_the_dead.remains > cooldown.blood_fury.duration % 3) | target.time_to_die <= buff.blood_fury.duration
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && MeleeRange && (GargoyleActive || PlayerHasBuff(UnholyAssault) || Talent_ArmyoftheDamned && ApocGhoulActive && (ArmyGhoulActive || API.SpellCDDuration(ArmyoftheDead) > API.SpellCDDuration(RacialSpell1) % 3) || API.TargetTimeToDie <= 1500))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions +=/ berserking,if= pet.gargoyle.active | buff.unholy_assault.up | talent.army_of_the_damned & pet.apoc_ghoul.active & (pet.army_ghoul.active | cooldown.army_of_the_dead.remains > cooldown.berserking.duration % 3) | target.time_to_die <= buff.berserking.duration
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && MeleeRange && (GargoyleActive || PlayerHasBuff(UnholyAssault) || Talent_ArmyoftheDamned && ApocGhoulActive && (ArmyGhoulActive || API.SpellCDDuration(ArmyoftheDead) > API.SpellCDDuration(RacialSpell1) % 3) || API.TargetTimeToDie <= 1500))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions +=/ lights_judgment,if= buff.unholy_strength.up
                    if (API.CanCast(RacialSpell1) && PlayerHasBuff(UnholyStrength) && PlayerRaceSettings == "Lightforged Draenei" && MeleeRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //Ancestral Call can trigger 4 potential buffs, each lasting 15 seconds. Utilized hard coded time as a trigger to keep it readable.
                    //actions +=/ ancestral_call,if= pet.gargoyle.active | buff.unholy_assault.up | talent.army_of_the_damned & pet.apoc_ghoul.active & (pet.army_ghoul.active | cooldown.army_of_the_dead.remains > cooldown.ancestral_call.duration % 3) | target.time_to_die <= 15
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && MeleeRange && (GargoyleActive || PlayerHasBuff(UnholyAssault) || Talent_ArmyoftheDamned && ApocGhoulActive && (ArmyGhoulActive || API.SpellCDDuration(ArmyoftheDead) > API.SpellCDDuration(RacialSpell1) % 3) || API.TargetTimeToDie <= 1500))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions +=/ arcane_pulse,if= active_enemies >= 2 | (rune.deficit >= 5 & runic_power.deficit >= 60)
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Nightborne" && MeleeRange && (API.PlayerUnitInMeleeRangeCount >= 2 || (API.PlayerCurrentRunes < 1 && API.PlayerRunicPower <= 40)))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions +=/ fireblood,if= pet.gargoyle.active | buff.unholy_assault.up | talent.army_of_the_damned & pet.apoc_ghoul.active & (pet.army_ghoul.active | cooldown.army_of_the_dead.remains > cooldown.fireblood.duration % 3) | target.time_to_die <= buff.fireblood.duration
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && MeleeRange && (GargoyleActive || PlayerHasBuff(UnholyAssault) || Talent_ArmyoftheDamned && ApocGhoulActive && (ArmyGhoulActive || API.SpellCDDuration(ArmyoftheDead) > API.SpellCDDuration(RacialSpell1) % 3) || API.TargetTimeToDie <= 800))
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //actions +=/ bag_of_tricks,if= buff.unholy_strength.up & active_enemies = 1

                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Vulpera" && MeleeRange && PlayerHasBuff(UnholyStrength) && API.PlayerUnitInMeleeRangeCount <= 1)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }

                }

                if (API.CanCast(ShackletheUnworthy) && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <= 30)
                {
                    API.CastSpell(ShackletheUnworthy);
                    return;
                }
                if (API.CanCast(DeathsDue) && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <= 30)
                {
                    API.CastSpell(DeathsDue);
                    return;
                }
                if (API.CanCast(ShackletheUnworthy) && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <= 30)
                {
                    API.CastSpell(ShackletheUnworthy);
                    return;
                }
                if (API.CanCast(AbominationLimb) && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <= 30)
                {
                    API.CastSpell(AbominationLimb);
                    return;
                }
                if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercent)
                {
                    API.CastSpell(Fleshcraft);
                    return;
                }
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell("Trinket2");
                }
                #region cooldowns

                if (API.CanCast("Dark Transformation") && (PlayerHasBuff(UnholyBlight) || !Talent_UnholyBlight) && (API.PlayerIsConduitSelected(ConvocationoftheDead) && API.SpellCDDuration(Apocalypse) <= gcd || !API.PlayerIsConduitSelected(ConvocationoftheDead)) && (WhenDarkTransformation == "On Cooldown" || IsCooldowns && WhenDarkTransformation == "With Cooldowns") && MeleeRange)
                {
                    API.CastSpell("Dark Transformation");
                    return;
                }
                if (API.CanCast("Raise Abomination") && (UseRaiseAbomination == "On Cooldown" || IsCooldowns && UseRaiseAbomination == "With Cooldowns") && MeleeRange)
                {
                    API.CastSpell("Raise Abomination");
                    return;
                }
                if (API.CanCast("Apocalypse") && (!API.PlayerIsConduitSelected(ConvocationoftheDead) || API.PlayerIsConduitSelected(ConvocationoftheDead) && Dark_Transformation_Ghoul.IsRunning) && (UseApocalypse == "On Cooldown" || IsCooldowns && UseApocalypse == "With Cooldowns") && Festering_Wound_Stacks >= 4 && MeleeRange)
                {
                    API.CastSpell("Apocalypse");
                    return;
                }
                if (API.CanCast("Sacrificial Pact") && API.PlayerHealthPercent <= 50 && IsCooldowns && !Dark_Transformation_Ghoul.IsRunning && API.PlayerLevel >= 54 && MeleeRange)
                {
                    API.CastSpell("Sacrificial Pact");
                    return;
                }
                if (API.CanCast("Unholy Assault") && IsCooldowns && Talent_UnholyAssault && MeleeRange)
                {
                    API.CastSpell("Unholy Assault");
                    return;
                }
                if (API.CanCast("Summon Gargoyle") && IsCooldowns && Talent_SummonGargoyle && MeleeRange)
                {
                    API.CastSpell("Summon Gargoyle");
                    return;
                }


                #endregion
                if (!IsAOE || API.TargetUnitInRangeCount < AOEUnitNumber)
                {
                    #region Gargoyle active ST
                    if (GargoyleActive)
                    {
                        if (API.CanCast(DeathCoil) && ((API.PlayerRunicPower >= 40 && API.PlayerHealthPercent >= DeathStrikePercent) || API.PlayerHasBuff("Sudden Doom")) && API.TargetRange <= 30)
                        {
                            API.CastSpell(DeathCoil);
                            return;
                        }
                        if (API.CanCast("Death Strike") && API.PlayerHasBuff("Dark Succor") && MeleeRange)
                        {
                            API.CastSpell("Death Strike");
                            return;
                        }
                        if (API.CanCast("Unholy Blight") && (API.PlayerIsConduitSelected(ConvocationoftheDead) && API.SpellCDDuration(Apocalypse) <= 2 * gcd || !API.PlayerIsConduitSelected(ConvocationoftheDead)) && (SmallCDs || IsCooldowns) && API.TargetDebuffRemainingTime("Virulent Plague") < 810 && API.PlayerCurrentRunes >= 1 && Talent_UnholyBlight && MeleeRange)
                        {
                            API.CastSpell("Unholy Blight");
                            return;
                        }
                        if (API.CanCast("Outbreak") & !Talent_UnholyBlight && API.PlayerLevel >= 17 && API.TargetDebuffRemainingTime("Virulent Plague") < 810 && API.TargetRange <= 30)
                        {
                            API.CastSpell("Outbreak");
                            return;
                        }
                        if (API.CanCast("Scourge Strike") && Festering_Wound_Stacks >= 1 && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && !Talent_ClawingShadows && MeleeRange)
                        {
                            API.CastSpell("Scourge Strike");
                            return;
                        }
                        if (API.CanCast("Clawing Shadows") && Festering_Wound_Stacks >= 1 && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && Talent_ClawingShadows && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && API.TargetRange <= 30)
                        {
                            API.CastSpell("Clawing Shadows");
                            return;
                        }
                        if (API.CanCast("Festering Strike") && API.PlayerCurrentRunes >= 2 && Festering_Wound_Stacks < 4 && (!PoolingForGargoyle || !IsCooldowns) && MeleeRange)
                        {
                            API.CastSpell("Festering Strike");
                            return;
                        }
                    }
                    else
                    {
                        #endregion
                        #region ST - outside cd
                        if (API.CanCast("Unholy Blight") && (API.PlayerIsConduitSelected(ConvocationoftheDead) && API.SpellCDDuration(Apocalypse) <= 2 * gcd || !API.PlayerIsConduitSelected(ConvocationoftheDead)) && (SmallCDs || IsCooldowns) && API.TargetDebuffRemainingTime("Virulent Plague") < 810 && API.PlayerCurrentRunes >= 1 && Talent_UnholyBlight && MeleeRange)
                        {
                            API.CastSpell("Unholy Blight");
                            return;
                        }

                        if (API.CanCast("Outbreak") && !Talent_UnholyBlight && API.PlayerLevel >= 17 && API.TargetDebuffRemainingTime("Virulent Plague") < 810 && API.TargetRange <= 30)
                        {
                            API.CastSpell("Outbreak");
                            return;
                        }

                        if (API.CanCast("Soul Reaper") && API.PlayerCurrentRunes >= 1 && Talent_SoulReaper && API.TargetHealthPercent < 35 && MeleeRange)
                        {
                            API.CastSpell("Soul Reaper");
                            return;
                        }
                        //death_coil,if=CombatRoutine.sudden_doom.react&rune.time_to_4>gcd&!variable.pooling_for_gargoyle|pet.gargoyle.active
                        if (API.CanCast(DeathCoil) && ((API.PlayerRunicPower >= 40 && API.PlayerHealthPercent >= DeathStrikePercent) || API.PlayerHasBuff("Sudden Doom")) && (API.PlayerHasBuff("Sudden Doom") && API.PlayerRuneCD(4) > gcd && !PoolingForGargoyle || GargoyleActiveTime.IsRunning || !IsCooldowns) && API.TargetRange <= 30)
                        {
                            API.CastSpell(DeathCoil);
                            return;
                        }
                        //death_coil,if=runic_power.deficit<14&rune.time_to_4>gcd&!variable.pooling_for_gargoyle
                        if (API.CanCast(DeathCoil) && API.PlayerHealthPercent >= DeathStrikePercent && API.PlayerRunicPower > 80 && RuneTimeTo(4) > gcd && (!PoolingForGargoyle || !IsCooldowns) && API.TargetRange <= 30)
                        {
                            API.CastSpell(DeathCoil);
                            return;
                        }
                        // scourge_strike,if=deCombatRoutine.festering_wound.up
                        if (API.CanCast("Scourge Strike") && Festering_Wound_Stacks >= 1 && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && !Talent_ClawingShadows && MeleeRange)
                        {
                            API.CastSpell("Scourge Strike");
                            return;
                        }
                        if (API.CanCast("Clawing Shadows") && Festering_Wound_Stacks >= 1 && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && Talent_ClawingShadows && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && API.TargetRange <= 30)
                        {
                            API.CastSpell("Clawing Shadows");
                            return;
                        }
                        if (API.CanCast("Festering Strike") && API.PlayerCurrentRunes >= 2 && Festering_Wound_Stacks < 4 && (!PoolingForGargoyle || !IsCooldowns) && MeleeRange)
                        {
                            API.CastSpell("Festering Strike");
                            return;
                        }
                        if (API.CanCast(DeathCoil) && API.PlayerHealthPercent >= DeathStrikePercent && (!PoolingForGargoyle || !IsCooldowns) && API.PlayerRunicPower >= 40 && API.TargetRange <= 30)
                        {
                            API.CastSpell(DeathCoil);
                            return;
                        }
                    }
                    #endregion
                }
                if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)
                {
                    #region AoE
                    if (API.CanCast("Unholy Blight") && (API.PlayerIsConduitSelected(ConvocationoftheDead) && API.SpellCDDuration(Apocalypse) <= 2 * gcd || !API.PlayerIsConduitSelected(ConvocationoftheDead)) && API.PlayerCurrentRunes >= 1 && Talent_UnholyBlight && MeleeRange)
                    {
                        API.CastSpell("Unholy Blight");
                        return;
                    }
                    if (API.CanCast("Outbreak") & !Talent_UnholyBlight && API.PlayerLevel >= 17 && API.TargetDebuffRemainingTime("Virulent Plague") < 200 && API.TargetRange <= 30)
                    {
                        API.CastSpell("Outbreak");
                        return;
                    }
                    if (API.CanCast("Soul Reaper") && API.PlayerCurrentRunes >= 1 && Talent_SoulReaper && API.TargetHealthPercent < 35 && MeleeRange)
                    {
                        API.CastSpell("Soul Reaper");
                        return;
                    }
                    //death_coil,if=CombatRoutine.sudden_doom.react&rune.time_to_4>gcd&!variable.pooling_for_gargoyle|pet.gargoyle.active
                    if (API.CanCast("Epidemic") && ((API.PlayerRunicPower >= 30 && API.PlayerHealthPercent >= DeathStrikePercent) || API.PlayerHasBuff("Sudden Doom")) && ((API.PlayerHasBuff("Sudden Doom") || API.PlayerRunicPower > 80) && !PoolingForGargoyle || GargoyleActiveTime.IsRunning || !IsCooldowns))
                    {
                        API.CastSpell("Epidemic");
                        return;
                    }
                    if (API.CanCast("Death and Decay") && UseDnD && !API.PlayerIsMoving && !Talent_Defile)
                    {
                        API.CastSpell("Death and Decay");
                        return;
                    }

                    if (API.CanCast("Defile") && UseDnD && !API.PlayerIsMoving && Talent_Defile)
                    {
                        API.CastSpell("Defile");
                        return;
                    }
                    if (API.CanCast("Scourge Strike") && API.PlayerCurrentRunes >= 1 && API.PlayerHasBuff("Death and Decay") && !Talent_ClawingShadows && MeleeRange)
                    {
                        API.CastSpell("Scourge Strike");
                        return;
                    }
                    if (API.CanCast("Clawing Shadows") && API.PlayerCurrentRunes >= 1 && API.PlayerHasBuff("Death and Decay") && Talent_ClawingShadows && MeleeRange)
                    {
                        API.CastSpell("Clawing Shadows");
                        return;
                    }
                    if (API.CanCast("Epidemic") && ((API.PlayerRunicPower >= 30 && API.PlayerHealthPercent >= DeathStrikePercent) || API.PlayerHasBuff("Sudden Doom")) && (!PoolingForGargoyle || GargoyleActiveTime.IsRunning || !IsCooldowns))
                    {
                        API.CastSpell("Epidemic");
                        return;
                    }
                    // scourge_strike,if=deCombatRoutine.festering_wound.up
                    if (API.CanCast("Scourge Strike") && Festering_Wound_Stacks > 1 && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && !Talent_ClawingShadows && MeleeRange)
                    {
                        API.CastSpell("Scourge Strike");
                        return;
                    }
                    if (API.CanCast("Clawing Shadows") && Festering_Wound_Stacks > 1 && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && Talent_ClawingShadows && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && API.TargetRange <= 30)
                    {
                        API.CastSpell("Clawing Shadows");
                        return;
                    }
                    if (API.CanCast("Festering Strike") && API.PlayerCurrentRunes >= 2 && Festering_Wound_Stacks < 4 && (!PoolingForGargoyle || !IsCooldowns) && MeleeRange)
                    {
                        API.CastSpell("Festering Strike");
                        return;
                    }
                    #endregion
                }

            }

        }

        public override void OutOfCombatPulse()
        {

        }
    }
}




