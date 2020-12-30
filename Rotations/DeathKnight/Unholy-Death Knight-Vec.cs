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



        private int IceboundFortitudeLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Icebound")];
        private int DeathPactLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Death Pact")];
        private int AntiMagicShellLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Anti-Magic Shell")];
        private int DeathStrikePercent => percentListProp[CombatRoutine.GetPropertyInt("Death Strike")];
        private int DarkSuccorPercent => percentListProp[CombatRoutine.GetPropertyInt("Dark Succor")];
        private int LichborneLifePercent => API.getFromArray(percentListProp, CombatRoutine.GetPropertyInt(Lichborne));

        private string WhenDarkTransformation => CDUsage[CombatRoutine.GetPropertyInt("DarkTransformation")];
        private string UseRaiseAbomination => CDUsage[CombatRoutine.GetPropertyInt("RaiseAbomination")];
        private string UseApocalypse => CDUsage[CombatRoutine.GetPropertyInt("Apocalypse")];

        private bool UseRaiseDead => CombatRoutine.GetPropertyBool("RaiseDead");
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];

        public override void Initialize()
        {
            CombatRoutine.Name = "Unholy DK by Vec";
            API.WriteLog("Welcome to Unholy DK Rotation");

            //Spells

            CombatRoutine.AddSpell("Outbreak", "D1");
            CombatRoutine.AddSpell("Soul Reaper", "D2");
            CombatRoutine.AddSpell("Dark Transformation", "D3");
            CombatRoutine.AddSpell("Apocalypse", "D4");
            CombatRoutine.AddSpell(DeathCoil, "D5");
            CombatRoutine.AddSpell("Death and Decay", "D6");
            CombatRoutine.AddSpell("Festering Strike", "D7");
            CombatRoutine.AddSpell("Mind Freeze", "D8");
            CombatRoutine.AddSpell("Unholy Assault", "D9");
            CombatRoutine.AddSpell("Chains of Ice", "F1");
            CombatRoutine.AddSpell("Scourge Strike", "F2");
            CombatRoutine.AddSpell("Clawing Shadows", "F2");
            CombatRoutine.AddSpell("Icebound Fortitude", "F3");
            CombatRoutine.AddSpell("Anti-Magic Shell", "F4");
            CombatRoutine.AddSpell("Death Strike", "F5");
            CombatRoutine.AddSpell("Epidemic", "F6");
            CombatRoutine.AddSpell("Death Pact", "NumPad4");
            CombatRoutine.AddSpell("Defile", "D6");
            CombatRoutine.AddSpell("Army of the Dead", "F8");
            CombatRoutine.AddSpell("Unholy Blight", "F9");
            CombatRoutine.AddSpell("Summon Gargoyle", "F7");
            CombatRoutine.AddSpell("Raise Dead", "D0");
            CombatRoutine.AddSpell("Raise Abomination", "F8");
            CombatRoutine.AddSpell("Necrotic Strike", "NumPad6");
            CombatRoutine.AddSpell("Sacrificial Pact", "NumPad5");
            CombatRoutine.AddSpell(ShackletheUnworthy, "F");
            CombatRoutine.AddSpell(SwarmingMist, "F");
            CombatRoutine.AddSpell(DeathsDue, "F");
            CombatRoutine.AddSpell(AbominationLimb, "F");
            CombatRoutine.AddSpell(Lichborne, "F12");

            CombatRoutine.AddBuff("Cold Heart");
            CombatRoutine.AddBuff("Cold Heart from Chest");
            CombatRoutine.AddBuff("Dark Succor");
            CombatRoutine.AddBuff("Unholy Frenzy");
            CombatRoutine.AddBuff("Sudden Doom");
            CombatRoutine.AddBuff("Death and Decay");
            CombatRoutine.AddBuff("Master of Ghouls");
            CombatRoutine.AddBuff(UnholyStrength);
            CombatRoutine.AddBuff(UnholyAssault);

            CombatRoutine.AddDebuff("Virulent Plague");
            CombatRoutine.AddDebuff("Festering Wound");
            CombatRoutine.AddDebuff("Necrotic Wound");
            CombatRoutine.AddDebuff("Razor Coral");
            //Toggle
            CombatRoutine.AddToggle("Small CDs");
            CombatRoutine.AddToggle("Use DnD");
            CombatRoutine.AddToggle("Mouseover");

            CombatRoutine.AddMacro("Trinket1");
            CombatRoutine.AddMacro("Trinket2");
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

            if (!API.PlayerIsMounted && !API.PlayerIsCasting(true) && !API.PlayerSpellonCursor)
            {

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
            if (!API.PlayerIsMounted && !API.PlayerIsCasting(true) && !API.PlayerSpellonCursor)
            {
                if (API.CanCast("Mind Freeze") && isInterrupt && MeleeRange)
                {
                    API.CastSpell("Mind Freeze");
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
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell("Trinket2");
                }
                #region cooldowns

                if (API.CanCast("Dark Transformation") && (WhenDarkTransformation == "On Cooldown" || IsCooldowns && WhenDarkTransformation == "With Cooldowns") && MeleeRange)
                {
                    API.CastSpell("Dark Transformation");
                    return;
                }
                if (API.CanCast("Raise Abomination") && (UseRaiseAbomination == "On Cooldown" || IsCooldowns && UseRaiseAbomination == "With Cooldowns") && MeleeRange)
                {
                    API.CastSpell("Raise Abomination");
                    return;
                }
                if (API.CanCast("Apocalypse") && (UseApocalypse == "On Cooldown" || IsCooldowns && UseApocalypse == "With Cooldowns") && Festering_Wound_Stacks >= 4 && MeleeRange)
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
                        if (API.CanCast("Unholy Blight") && (SmallCDs || IsCooldowns) && API.TargetDebuffRemainingTime("Virulent Plague") < 810 && API.PlayerCurrentRunes >= 1 && Talent_UnholyBlight && MeleeRange)
                        {
                            API.CastSpell("Unholy Blight");
                            return;
                        }
                        if (API.CanCast("Outbreak") && API.PlayerLevel >= 17 && API.TargetDebuffRemainingTime("Virulent Plague") < 810 && API.TargetRange <= 30)
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
                        if (API.CanCast("Unholy Blight") && (SmallCDs || IsCooldowns) && API.TargetDebuffRemainingTime("Virulent Plague") < 810 && API.PlayerCurrentRunes >= 1 && Talent_UnholyBlight && MeleeRange)
                        {
                            API.CastSpell("Unholy Blight");
                            return;
                        }

                        if (API.CanCast("Outbreak") && API.PlayerLevel >= 17 && API.TargetDebuffRemainingTime("Virulent Plague") < 810 && API.TargetRange <= 30)
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
                    if (API.CanCast("Unholy Blight") && API.PlayerCurrentRunes >= 1 && Talent_UnholyBlight && MeleeRange)
                    {
                        API.CastSpell("Unholy Blight");
                        return;
                    }
                    if (API.CanCast("Outbreak") && API.PlayerLevel >= 17 && API.TargetDebuffRemainingTime("Virulent Plague") < 200 && API.TargetRange <= 30)
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




