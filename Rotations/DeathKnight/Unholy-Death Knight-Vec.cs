using System;
using System.Diagnostics;
using System.Linq;

namespace HyperElk.Core
{
    public class UHDK : CombatRoutine
    {
        private bool SmallCDs => API.ToggleIsEnabled("Small CDs");
        private string ShackletheUnworthy = "Shackle the Unworthy";
        private string SwarmingMist = "Swarming Mist";
        private string DeathsDue = "Death's Due";
        private string AbominationLimby = "Abomination Limby";

        //stopwatch
        private readonly Stopwatch Dark_Transformation_Ghoul = new Stopwatch();
        private readonly Stopwatch GargoyleActiveTime = new Stopwatch();

        //Spells,Buffs,Debuffs


        //Misc
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool MeleeRange => API.TargetRange <= 5;
        private bool InRange => API.TargetRange <= 40;
        private bool next_wi_bomb(string spellname) => API.SpellIsCanbeCast(spellname);
        
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
        // private float BarbedShotCount => (API.PlayerHasBuff("246152", false, false) ? 1 : 0) + (API.PlayerHasBuff("246851", false, false) ? 1 : 0) + (API.PlayerHasBuff("217200", false, false) ? 1 : 0);
        private float FocusRegen => 10f * (1f + API.PlayerGetHaste / 100f);
        // private float RealFocusRegen => FocusRegen + BarbedShotCount * 2.5f;
        private float FocusTimeToMax => (API.PlayerMaxFocus - API.PlayerFocus) * 100f / FocusRegen;
        private float Barrage_ExecuteTime => 300f / (1f + (API.PlayerGetHaste / 100f));
        private float WildfireBombCooldown => 1800f / (1f + (API.PlayerGetHaste / 100f));
        private float KillCommandCooldown => 600f / (1f + (API.PlayerGetHaste / 100f));
        private float ButcheryCooldown => 900f / (1f + (API.PlayerGetHaste / 100f));
        
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
        //CBProperties


        string[] MisdirectionList = new string[] { "Off", "On AOE", "On" };
        string[] combatList = new string[] { "In Combat", "Out Of Combat", "Everytime" };


        private int IceboundFortitudeLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Icebound")];
        private int DeathPactLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Death Pact")];
        private int AntiMagicShellLifePercent => percentListProp[CombatRoutine.GetPropertyInt("Anti-Magic Shell")];
        private int DeathStrikePercent => percentListProp[CombatRoutine.GetPropertyInt("Death Strike")];
        private int DarkSuccorPercent => percentListProp[CombatRoutine.GetPropertyInt("Dark Succor")];

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
            CombatRoutine.AddSpell("Death Coil", "D5");
            CombatRoutine.AddSpell("Death and Decay", "D6");
            CombatRoutine.AddSpell("Festering Strike", "D7");
            CombatRoutine.AddSpell( "Mind Freeze", "D8");
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
            CombatRoutine.AddSpell(AbominationLimby, "F");

            CombatRoutine.AddBuff("Cold Heart");
            CombatRoutine.AddBuff("Cold Heart from Chest");
            CombatRoutine.AddBuff("Dark Succor");
            CombatRoutine.AddBuff("Unholy Frenzy");
            CombatRoutine.AddBuff("Sudden Doom");
            CombatRoutine.AddBuff("Death and Decay");
            CombatRoutine.AddBuff("Master of Ghouls");
            CombatRoutine.AddBuff("Unholy Strength");
            CombatRoutine.AddBuff("Razor Coral");

            CombatRoutine.AddDebuff("Virulent Plague");
            CombatRoutine.AddDebuff("Festering Wound");
            CombatRoutine.AddDebuff("Necrotic Wound");
            CombatRoutine.AddDebuff("Razor Coral");
            //Toggle
            CombatRoutine.AddToggle("Small CDs");
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Multidot");

           

            //Settings
            CombatRoutine.AddProp("RaiseDead", "Raise Dead", true, "Should the rotation try to Raise Dead", "Pet");
            CombatRoutine.AddProp("DarkTransformation", "Use " + "Dark Transformation", CDUsage, "Use " + "Dark Transformation" + "On Cooldown, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("RaiseAbomination", "Use " + "Raise Abomination", CDUsage, "Use " + "Raise Abomination" + "On Cooldown, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("Apocalypse", "Use " + "Apocalypse", CDUsage, "Use " + "Apocalypse" + "On Cooldown, with Cooldowns", "Cooldowns", 0);

            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " On Cooldown, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " On Cooldown, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " On Cooldown, with Cooldowns", "Trinkets", 0);

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
            if (API.LastSpellCastInGame=="Summon Gargoyle" && !GargoyleActiveTime.IsRunning) { GargoyleActiveTime.Start(); API.WriteLog("Gargoyle is active"); }
            if (GargoyleActiveTime.IsRunning && GargoyleActiveTime.ElapsedMilliseconds > 30000) { GargoyleActiveTime.Reset(); API.WriteLog("Gargoyle ran off"); }
            //API.WriteLog("debug" + MultiDot + API.CanCast(Kill_Command) + API.TargetHasDebuff(Kill_Command, false, false)+ " " + API.PlayerUnitInMeleeRangeCount  +" "+ API.PlayerBuffStacks(Predator));

            if (!API.PlayerIsMounted && !Playeriscasting)
            {

                #region defensives
                if (API.CanCast("Raise Dead") && UseRaiseDead && (!API.PlayerHasPet || API.PetHealthPercent <1))
                {
                    API.CastSpell("Raise Dead");
                    return;
                }
                if (API.CanCast("Icebound Fortitude") && API.PlayerLevel >= 38 && API.PlayerHealthPercent <= IceboundFortitudeLifePercent)
                {
                    API.CastSpell("Icebound Fortitude");
                    return;
                }
                if (API.CanCast("Death Pact") && Talent_DeathPact  && API.PlayerHealthPercent <= DeathPactLifePercent)
                {
                    API.CastSpell("Death Pact");
                    return;
                }
                if (API.CanCast("Anti-Magic Shell") && API.PlayerLevel>=14 && API.PlayerHealthPercent <= AntiMagicShellLifePercent)
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
                if (isRacial && IsCooldowns)
                {
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && MeleeRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Orc" && MeleeRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //  cds->add_action("lights_judgment");
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged Draenei" && MeleeRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                    //  cds->add_action("berserking,if=cooldown.coordinated_assault.remains>60|time_to_die<13");
                    if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Troll" && MeleeRange)
                    {
                        API.CastSpell(RacialSpell1);
                        return;
                    }
                }
                //  cds->add_action(this, "Aspect of the Eagle", "if=target.distance>=6");

                if (API.CanCast(ShackletheUnworthy) && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <=30)
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
                if (API.CanCast(AbominationLimby) && (UseCovenant == "With Cooldowns" && (IsCooldowns) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <= 30)
                {
                    API.CastSpell(AbominationLimby);
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
                if (API.CanCast("Sacrificial Pact") && IsCooldowns && !Dark_Transformation_Ghoul.IsRunning && API.PlayerLevel >= 54 && MeleeRange)
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
                    #region ST
                    if (API.CanCast("Dark Transformation")  && WhenDarkTransformation == "On Cooldown" && MeleeRange)
                    {
                        API.CastSpell("Dark Transformation");
                        return;
                    }
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
                    if (API.CanCast("Death Coil") && (API.PlayerRunicPower >= 40 || API.PlayerHasBuff("Sudden Doom")) && (API.PlayerHasBuff("Sudden Doom") && API.PlayerRuneCD(4) > gcd && !PoolingForGargoyle || GargoyleActiveTime.IsRunning || !IsCooldowns) && API.TargetRange <= 30)
                    {
                        API.CastSpell("Death Coil");
                        return;
                    }
                    //death_coil,if=runic_power.deficit<14&rune.time_to_4>gcd&!variable.pooling_for_gargoyle
                    if (API.CanCast("Death Coil") && API.PlayerRunicPower > 80 && RuneTimeTo(4) > gcd && (!PoolingForGargoyle || !IsCooldowns) && API.TargetRange <= 30)
                    {
                        API.CastSpell("Death Coil");
                        return;
                    }
                    // scourge_strike,if=deCombatRoutine.festering_wound.up
                    if (API.CanCast("Scourge Strike") && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && !Talent_ClawingShadows && MeleeRange)
                    {
                        API.CastSpell("Scourge Strike");
                        return;
                    }
                    if (API.CanCast("Clawing Shadows") && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && Talent_ClawingShadows && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && API.TargetRange <= 30)
                    {
                        API.CastSpell("Clawing Shadows");
                        return;
                    }
                    if (API.CanCast("Festering Strike") && API.PlayerCurrentRunes >= 2 && Festering_Wound_Stacks < 4 && (!PoolingForGargoyle || !IsCooldowns) && MeleeRange)
                    {
                        API.CastSpell("Festering Strike");
                        return;
                    }
                    if (API.CanCast("Death Coil") && (!PoolingForGargoyle || !IsCooldowns) && API.PlayerRunicPower >= 40 && API.TargetRange <= 30)
                    {
                        API.CastSpell("Death Coil");
                        return;
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
                    if (API.CanCast("Epidemic") && (API.PlayerRunicPower >= 30 || API.PlayerHasBuff("Sudden Doom")) && ((API.PlayerHasBuff("Sudden Doom") || API.PlayerRunicPower > 80) && !PoolingForGargoyle || GargoyleActiveTime.IsRunning || !IsCooldowns))
                    {
                        API.CastSpell("Epidemic");
                        return;
                    }
                    if (API.CanCast("Death and Decay") && !API.PlayerIsMoving && Talent_Defile)
                    {
                        API.CastSpell("Death and Decay");
                        return;
                    }

                    if (API.CanCast("Defile") && !API.PlayerIsMoving && Talent_Defile)
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
                    if (API.CanCast("Epidemic") && (API.PlayerRunicPower >= 30 || API.PlayerHasBuff("Sudden Doom")) && (!PoolingForGargoyle || GargoyleActiveTime.IsRunning || !IsCooldowns))
                    {
                        API.CastSpell("Epidemic");
                        return;
                    }
                    // scourge_strike,if=deCombatRoutine.festering_wound.up
                    if (API.CanCast("Scourge Strike") && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && !Talent_ClawingShadows && MeleeRange)
                    {
                        API.CastSpell("Scourge Strike");
                        return;
                    }
                    if (API.CanCast("Clawing Shadows") && API.PlayerCurrentRunes >= 1 && (!PoolingForGargoyle || !IsCooldowns) && Talent_ClawingShadows && (Festering_Wound_Stacks >= 1 && (API.SpellCDDuration("Apocalypse") > 500 || !IsCooldowns) || (Festering_Wound_Stacks >= 4 && !API.CanCast("Apocalypse"))) && API.TargetRange <= 30)
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




