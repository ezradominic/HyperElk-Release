
namespace HyperElk.Core
{
    public class LevelingDK : CombatRoutine
    {

        //CBProperties
        private int IceboundFortitudeLifePercent => percentListProp[CombatRoutine.GetPropertyInt(IceboundFortitude)];
        private int AntiMagicShellLifePercent => percentListProp[CombatRoutine.GetPropertyInt(AntiMagicShell)];
        private int LichborneLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Lichborne)];
        private int DeathStrikeProcLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DeathStrike + "PROC")];
        private int DeathStrikeLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DeathStrike)];


        private string HowlingBlast = "Howling Blast";
        private string DeathCoil = "Death Coil";
        private string FrostStrike = "Frost Strike";
        private string DeathStrike = "Death Strike";
        private string DeathandDecay = "Death and Decay";
        private string Obliterate = "Obliterate";
        private string RemorselessWinter = "Remorseless Winter";
        private string ChainsofIce = "Chains of Ice";
        private string RaiseDead = "Raise Dead";
        private string PillarofFrost = "Pillar of Frost";
        private string EmpowerRuneWeapon = "Empower Rune Weapon";
        private string FrostwyrmsFury = "Frostwyrm's Fury";
        private string Frostscythe = "Frostscythe";
        private string BreathofSindragosa = "Breath of Sindragosa";
        private string GlacialAdvance = "Glacial Advance";
        private string HornofWinter = "Horn of Winter";
        private string ChillStreak = "Chill Streak";
        private string IceboundFortitude = "Icebound Fortitude";
        private string AntiMagicShell = "Anti-Magic Shell";
        private string Lichborne = "Lichborne";
        private string MindFreeze = "Mind Freeze";
        private string SwarmingMist = "Swarming Mist";
        private string ColdHeart = "Cold Heart";
        private string DarkSuccor = "Dark Succor";
        private string Rime = "Rime";
        private string KillingMachine = "Killing Machine";
        private string SacrificialPact = "Sacrificial Pact";

        private string FrostFever = "Frost Fever";

        //Talent

        private bool TalentColdHeart => API.PlayerIsTalentSelected(1, 3);
        private bool TalentRunicAtt => API.PlayerIsTalentSelected(2, 1);
        private bool TalentHornOfWinter => API.PlayerIsTalentSelected(2, 3);
        private bool TalentFrostscythe => API.PlayerIsTalentSelected(4, 3);
        private bool TalentAvalange => API.PlayerIsTalentSelected(4, 1);
        private bool TalentFrozenPulse => API.PlayerIsTalentSelected(4, 2);
        private bool TalentDeathPact => API.PlayerIsTalentSelected(5, 3);
        private bool TalentGlacialAdvance => API.PlayerIsTalentSelected(6, 3);
        private bool TalentGatheringStorm => API.PlayerIsTalentSelected(6, 1);
        private bool TalentHypnothermicPre => API.PlayerIsTalentSelected(6, 2);

        private bool TalentBreathOfSindra => API.PlayerIsTalentSelected(7, 3);
        private bool TalentObliteration => API.PlayerIsTalentSelected(7, 2);
        private bool TalentIcecap => API.PlayerIsTalentSelected(7, 1);
        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;

        //DK specific 

        private int RPDeficit => 100 - CurrentRP;
        private int CurrentRune
        {
            get
            {
                int currentRune = 0;
                for (int i = 1; i <= 6; i++)
                    currentRune = currentRune + (API.PlayerRuneCD(i) <= API.SpellGCDTotalDuration ? 1 : 0);
                return currentRune;
            }
        }
        private int CurrentRP => API.PlayerRunicPower;
        private bool UseDND => (bool)CombatRoutine.GetProperty("UseDND");

        public override void Initialize()
        {
            CombatRoutine.Name = "FrostDK@FmFlex";
            API.WriteLog("Welcome to Frost DK rotation @ FmFlex");
            API.WriteLog("DnD Macro to be used : /cast [@player] Death and Decay");

            CombatRoutine.AddProp("UseDND", "Use DND", true, "Should the rotation use Death and Decay", "Leveling");

            CombatRoutine.AddSpell(HowlingBlast, "D1", "None");
            CombatRoutine.AddSpell(DeathCoil, "D2", "None");
            CombatRoutine.AddSpell(FrostStrike, "D5", "None");
            CombatRoutine.AddSpell(DeathStrike, "D3", "None");
            CombatRoutine.AddSpell(DeathandDecay, "D4", "None");
            CombatRoutine.AddSpell(Obliterate, "D6", "None");
            CombatRoutine.AddSpell(RemorselessWinter, "D7", "None");
            CombatRoutine.AddSpell(ChainsofIce, "D8", "None");
            CombatRoutine.AddSpell(RaiseDead, "F5", "None");
            CombatRoutine.AddSpell(PillarofFrost, "Q", "None");
            CombatRoutine.AddSpell(ChillStreak, "D9", "None");
            CombatRoutine.AddSpell(FrostwyrmsFury, "F6", "None");
            CombatRoutine.AddSpell(Frostscythe, "D1");
            CombatRoutine.AddSpell(BreathofSindragosa, "D1");
            CombatRoutine.AddSpell(GlacialAdvance, "D1");
            CombatRoutine.AddSpell(HornofWinter, "D1");
            CombatRoutine.AddSpell(EmpowerRuneWeapon, "D1");
            CombatRoutine.AddSpell(MindFreeze, "D1");
            CombatRoutine.AddSpell(SwarmingMist, "NumPad2");

            CombatRoutine.AddSpell(IceboundFortitude, "D1");
            CombatRoutine.AddSpell(AntiMagicShell, "D1");
            CombatRoutine.AddSpell(Lichborne, "D1");
            CombatRoutine.AddSpell(SacrificialPact);

            CombatRoutine.AddBuff(ColdHeart);
            CombatRoutine.AddBuff(DarkSuccor);
            CombatRoutine.AddBuff(Rime);
            CombatRoutine.AddBuff(KillingMachine);
            CombatRoutine.AddBuff(PillarofFrost);
            CombatRoutine.AddBuff(BreathofSindragosa);
            CombatRoutine.AddBuff(RemorselessWinter);

            CombatRoutine.AddDebuff(FrostFever);

            CombatRoutine.AddProp(AntiMagicShell, AntiMagicShell + " Life Percent", percentListProp, "Life percent at which" + AntiMagicShell + "is used, set to 0 to disable", "Defense", 4);
            CombatRoutine.AddProp(DeathStrike, DeathStrike + " Life Percent", percentListProp, "Life percent at which" + DeathStrike + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(DeathStrike + "PROC", DeathStrike + " Life % Proc", percentListProp, "Life percent at which" + DeathStrike + "with free proc, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(IceboundFortitude, IceboundFortitude + " Life Percent", percentListProp, "Life percent at which" + IceboundFortitude + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Lichborne, Lichborne + " Life Percent", percentListProp, "Life percent at which" + Lichborne + "is used, set to 0 to disable", "Defense", 5);


        }

        public override void Pulse()
        {
        }

        public override void CombatPulse()
        {
            if (API.PlayerIsCasting)
                return;

            if (API.PlayerHealthPercent <= AntiMagicShellLifePercent && !API.SpellISOnCooldown(AntiMagicShell))
            {
                API.CastSpell(AntiMagicShell);
                return;
            }
            if (API.PlayerHealthPercent <= LichborneLifePercent && !API.SpellISOnCooldown(Lichborne))
            {
                API.CastSpell(Lichborne);
                return;
            }
            if (API.PlayerHealthPercent <= IceboundFortitudeLifePercent && !API.SpellISOnCooldown(IceboundFortitude))
            {
                API.CastSpell(IceboundFortitude);
                return;
            }

            if (isInterrupt && !API.SpellISOnCooldown(MindFreeze) && PlayerLevel >= 27)
            {
                API.CastSpell(MindFreeze);
                return;
            }

            if (((API.PlayerHealthPercent < DeathStrikeLifePercent && CurrentRP >= 45) || (API.PlayerHealthPercent <= DeathStrikeProcLifePercent && API.PlayerHasBuff(DarkSuccor))) && API.CanCast(DeathStrike, true, true) && IsMelee)
            {
                API.CastSpell(DeathStrike);
                return;
            }
            if (IsCooldowns && PlayerCovenantSettings == "Venthyr" && API.CanCast(SwarmingMist) && CurrentRune >= 1 && IsMelee && CurrentRune <= 40)
            {
                API.CastSpell(SwarmingMist);
                return;
            }
            //CD
            /*
             *  cooldowns -> add_action( this, "Empower Rune Weapon", "if=talent.obliteration&(cooldown.pillar_of_frost.ready&rune.time_to_5>gcd&
             *  runic_power.deficit>=10|buff.pillar_of_frost.up&rune.time_to_5>gcd)|fight_remains<20", "Cooldowns" );
  cooldowns -> add_action( this, "Empower Rune Weapon", "if=talent.breath_of_sindragosa&runic_power.deficit>40&rune.time_to_5>gcd&(buff.breath_of_sindragosa.up|fight_remains<20)" );
  cooldowns -> add_action( this, "Empower Rune Weapon", "if=talent.icecap&rune<3" );
             */
            if (IsCooldowns && TalentObliteration && API.CanCast(EmpowerRuneWeapon) && (API.PlayerHasBuff(PillarofFrost)|| (!API.SpellISOnCooldown(PillarofFrost) && RPDeficit >=10)) 
                && CurrentRune < 5 && IsMelee)
            {
                API.CastSpell(EmpowerRuneWeapon);
                return;
            }
            if (IsCooldowns && TalentBreathOfSindra && API.CanCast(EmpowerRuneWeapon) && RPDeficit > 40 && CurrentRune < 5 && API.PlayerHasBuff(BreathofSindragosa) && IsMelee)
            {
                API.CastSpell(EmpowerRuneWeapon);
                return;
            }
            if (IsCooldowns && TalentIcecap && API.CanCast(EmpowerRuneWeapon) && API.CanCast(EmpowerRuneWeapon) && API.PlayerCurrentRunes < 3 && IsMelee)
            {
                API.CastSpell(EmpowerRuneWeapon);
                return;
            }
            /*
             * cooldowns -> add_action( this, "Pillar of Frost", "if=talent.breath_of_sindragosa&(cooldown.breath_of_sindragosa.remains|cooldown.breath_of_sindragosa.ready&runic_power.deficit<60)" );
  cooldowns -> add_action( this, "Pillar of Frost", "if=talent.icecap&!buff.pillar_of_frost.up" );
  cooldowns -> add_action( this, "Pillar of Frost", "if=talent.obliteration&(talent.gathering_storm.enabled&buff.remorseless_winter.up|!talent.gathering_storm.enabled)" );
 
        */
            if (IsCooldowns && PlayerLevel >= 29 && API.CanCast(PillarofFrost) && TalentBreathOfSindra &&
            (((!API.SpellISOnCooldown(BreathofSindragosa) || API.PlayerHasBuff(BreathofSindragosa)) && RPDeficit < 60) || API.SpellCDDuration(BreathofSindragosa) >= 5000)
            && IsMelee)
            {
                API.CastSpell(PillarofFrost);
                return;
            }
            if (IsCooldowns && PlayerLevel >= 29 && API.CanCast(PillarofFrost) && TalentIcecap && API.PlayerHasBuff(PillarofFrost)
            && IsMelee)
            {
                API.CastSpell(PillarofFrost);
                return;
            }
            if (IsCooldowns && PlayerLevel >= 29 && API.CanCast(PillarofFrost) && TalentObliteration && ((TalentGatheringStorm && API.PlayerHasBuff(RemorselessWinter)) ||!TalentGatheringStorm)
            && IsMelee)
                        {
                API.CastSpell(PillarofFrost);
                return;
            }


            if (IsCooldowns && TalentBreathOfSindra && API.CanCast(BreathofSindragosa) && API.PlayerHasBuff(PillarofFrost) && IsMelee)
            {
                API.CastSpell(BreathofSindragosa);
                return;
            }

            /*
             *  cooldowns -> add_action( this, "Frostwyrm's Fury", "if=buff.pillar_of_frost.remains<gcd&buff.pillar_of_frost.up&!talent.obliteration" );
              cooldowns -> add_action( this, "Frostwyrm's Fury", "if=active_enemies>=2&(buff.pillar_of_frost.up&buff.pillar_of_frost.remains<gcd|raid_event.adds.exists&raid_event.adds.remains<gcd|fight_remains<gcd)" );
              ??? wtf cooldowns -> add_action( this, "Frostwyrm's Fury", "if=talent.obliteration&!buff.pillar_of_frost.up&((buff.unholy_strength.up|!death_knight.runeforge.fallen_crusader)&(debuff.razorice.stack=5|!death_knight.runeforge.razorice))" );
  
             */
            if (IsCooldowns && API.CanCast(FrostwyrmsFury) && !IsAOE && !TalentObliteration && API.PlayerBuffTimeRemaining(PillarofFrost) < API.SpellGCDTotalDuration && IsMelee)
            {
                API.CastSpell(FrostwyrmsFury);
                return;
            }
            if (IsCooldowns && API.CanCast(FrostwyrmsFury) && IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerHasBuff(PillarofFrost) && IsMelee)
            {
                API.CastSpell(FrostwyrmsFury);
                return;
            }
            //cooldowns -> add_talent( this, "Hypothermic Presence", "if=talent.breath_of_sindragosa&runic_power.deficit>40&rune>=3&buff.pillar_of_frost.up|!talent.breath_of_sindragosa&runic_power.deficit>=25" );
            //cooldowns->add_action(this, "Raise Dead", "if=buff.pillar_of_frost.up");
            if (IsCooldowns && PlayerLevel >= 12 && API.CanCast(RaiseDead) && IsMelee && API.PlayerHasBuff(PillarofFrost))
            {
                API.CastSpell(RaiseDead);
                return;
            }
            //  cooldowns -> add_action( this, "Sacrificial Pact", "if=active_enemies>=2&(pet.ghoul.remains<gcd|target.time_to_die<gcd)" );

            if (IsCooldowns && PlayerLevel >= 12 && API.CanCast(SacrificialPact,true,true) && IsMelee && API.SpellCDDuration(RaiseDead) < 6500 && API.SpellCDDuration(RaiseDead) > 6000)
            {
                API.CastSpell(SacrificialPact);
                return;
            }
            //  cooldowns -> add_action( this, "Death and Decay", "if=active_enemies>5|runeforge.phearomones" );
            if (UseDND && IsAOE && API.PlayerUnitInMeleeRangeCount >= 5 && CurrentRune >= 1 && API.CanCast(DeathandDecay) && IsMelee)
            {
                API.CastSpell(DeathandDecay);
                return;
            }
            /*
             * cold_heart -> add_action( this, "Chains of Ice", "if=fight_remains<gcd|buff.pillar_of_frost.remains<3&buff.cold_heart.stack=20&!talent.obliteration", "Cold Heart Conditions" );
                cold_heart -> add_action( this, "Chains of Ice", "if=talent.obliteration&!buff.pillar_of_frost.up&(buff.cold_heart.stack>=16&buff.unholy_strength.up|buff.cold_heart.stack>=19|cooldown.pillar_of_frost.remains<3&buff.cold_heart.stack>=14)", "Prevent Cold Heart overcapping during pillar" );

             */
            if (TalentColdHeart && API.PlayerBuffStacks(ColdHeart) >= 20 && !TalentObliteration && CurrentRune >= 1 && API.CanCast(ChainsofIce) && API.PlayerBuffTimeRemaining(PillarofFrost) <300 && API.PlayerHasBuff(PillarofFrost) && API.TargetRange <= 30)
            {
                API.CastSpell(ChainsofIce);
                return;
            }
            if (TalentColdHeart && TalentObliteration && CurrentRune >= 1 && API.CanCast(ChainsofIce) && !API.PlayerHasBuff(PillarofFrost,false,false) && ((API.PlayerBuffStacks(ColdHeart) >= 19)|| (API.PlayerBuffStacks(ColdHeart) >= 14 && API.PlayerBuffTimeRemaining(PillarofFrost) < 300)) && API.TargetRange <= 30)
            {
                API.CastSpell(ChainsofIce);
                return;
            }
            if (PlayerLevel >= 30 && IsAOE && API.CanCast(ChillStreak, true, true) && API.TargetUnitInRangeCount >= AOEUnitNumber && API.TargetRange <= 40)
            {
                API.CastSpell(ChillStreak);
                return;
            }

            if (TalentBreathOfSindra && API.SpellCDDuration(BreathofSindragosa) <= 1000)
            {
                //bos_pooling -> add_action( this, "Howling Blast", "if=buff.rime.up", "Breath of Sindragosa pooling rotation : starts 10s before BoS is available" );
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                // bos_pooling -> add_action( this, "Remorseless Winter", "if=active_enemies>=2|rune.time_to_5<=gcd&(talent.gathering_storm|conduit.everfrost|runeforge.biting_cold)" );

                if ((TalentGatheringStorm && CurrentRune < 5 || API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber) && CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                //bos_pooling -> add_action( this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,if=runic_power.deficit>=25", "'target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice' Repeats a lot, this is intended to target the highest priority enemy with an ability that will apply razorice if runeforged. That being an enemy with 0 stacks, or an enemy that the debuff will soon expire on." );

                if (CurrentRune >= 2 && RPDeficit >= 25 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //bos_pooling -> add_talent( this, "Glacial Advance", "if=runic_power.deficit<20&spell_targets.glacial_advance>=2&cooldown.pillar_of_frost.remains>5" );

                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && TalentGlacialAdvance && RPDeficit < 20
                    && API.SpellCDDuration(PillarofFrost) > 500 && API.CanCast(GlacialAdvance) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }

                // bos_pooling -> add_action( this, "Frost Strike", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)
                //*death_knight.runeforge.razorice,if=runic_power.deficit<20&cooldown.pillar_of_frost.remains>5" );

                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && RPDeficit < 20 && API.SpellCDDuration(PillarofFrost) > 500 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                // bos_pooling->add_talent(this, "Frostscythe", "if=buff.killing_machine.react&runic_power.deficit>(15+talent.runic_attenuation*3)&spell_targets.frostscythe>=2&(buff.deaths_due.stack=8|!death_and_decay.ticking|!covenant.night_fae)");
                //  bos_pooling->add_talent(this, "Frostscythe", "if=runic_power.deficit>=(35+talent.runic_attenuation*3)&spell_targets.frostscythe>=2&(buff.deaths_due.stack=8|!death_and_decay.ticking|!covenant.night_fae)");

                if (TalentFrostscythe && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && RPDeficit >= 15 + (TalentRunicAtt ? 15 : 0) && CurrentRune >= 1 && API.PlayerHasBuff(KillingMachine) && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
                if (TalentFrostscythe && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && RPDeficit >= 35 + (TalentRunicAtt ? 15 : 0) && CurrentRune >= 1 && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
                //bos_pooling -> add_action( this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,if=runic_power.deficit>=(35+talent.runic_attenuation*3)" );
                if ((!TalentFrostscythe || TalentFrostscythe && !IsAOE) && CurrentRune >= 2 && RPDeficit >= 35 + (TalentRunicAtt ? 15 : 0) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //bos_pooling->add_talent(this, "Glacial Advance", "if=cooldown.pillar_of_frost.remains>rune.time_to_4&runic_power.deficit<40&spell_targets.glacial_advance>=2");

                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && TalentGlacialAdvance && RPDeficit < 40
                    && API.PlayerBuffTimeRemaining(PillarofFrost) > 300 && API.CanCast(GlacialAdvance) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }
                //bos_pooling->add_action(this, "Frost Strike", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,if=cooldown.pillar_of_frost.remains>rune.time_to_4&runic_power.deficit<40");

                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && RPDeficit < 40 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }

                if (CurrentRune >= 5 && API.CanCast(HowlingBlast) && !IsMelee && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
            }
            if (TalentBreathOfSindra && API.PlayerHasBuff(BreathofSindragosa, false, false))
            {
                //bos_ticking->add_action(this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,if=runic_power<=40", "Breath of Sindragosa Active Rotation");

                if (CurrentRune >= 2 && CurrentRP <= 40 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //bos_ticking->add_action(this, "Remorseless Winter", "if=talent.gathering_storm|conduit.everfrost|runeforge.biting_cold|active_enemies>=2");

                if ((TalentGatheringStorm || (IsAOE && API.PlayerUnitInMeleeRangeCount > AOEUnitNumber)) && CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                //bos_ticking->add_action(this, "Howling Blast", "if=buff.rime.up");
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                //bos_ticking->add_action(this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,if=rune.time_to_4<gcd|runic_power<=45");

                if (CurrentRune >= 2 && (CurrentRP <= 45 || CurrentRune >= 4) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //bos_ticking->add_talent(this, "Frostscythe", "if=buff.killing_machine.up&spell_targets.frostscythe>=2&(!death_and_decay.ticking&covenant.night_fae|!covenant.night_fae)");
                if (TalentFrostscythe && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && CurrentRune >= 1 && API.PlayerHasBuff(KillingMachine) && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }

                //bos_ticking->add_talent(this, "Horn of Winter", "if=runic_power.deficit>=40&rune.time_to_3>gcd");


                if (TalentHornOfWinter && API.CanCast(HornofWinter) && RPDeficit >= 40 && CurrentRune < 3 && IsMelee)
                {
                    API.CastSpell(HornofWinter);
                    return;
                }
                //bos_ticking->add_talent(this, "Frostscythe", "if=spell_targets.frostscythe>=2&(buff.deaths_due.stack=8|!death_and_decay.ticking|!covenant.night_fae)");
                if (TalentFrostscythe && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && CurrentRune >= 1 && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
                //bos_ticking->add_action(this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,if=runic_power.deficit>25|rune>3");
                if ((CurrentRune > 3 || RPDeficit > 25) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //bos_ticking->add_action("arcane_torrent,if=runic_power.deficit>50");

            }
            if (TalentObliteration && API.PlayerHasBuff(PillarofFrost, false, false)) 
            {

                // obliteration->add_action(this, "Remorseless Winter", "if=active_enemies>=3&(talent.gathering_storm|conduit.everfrost|runeforge.biting_cold)", "Obliteration rotation");
                if ((TalentGatheringStorm || API.PlayerUnitInMeleeRangeCount >= 3) && CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                // obliteration->add_action(this, "Howling Blast", "if=!dot.frost_fever.ticking&!buff.killing_machine.up");
                if (!API.TargetHasDebuff(FrostFever, false, false) && !API.PlayerHasBuff(KillingMachine) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                //obliteration->add_talent(this, "Frostscythe", "if=buff.killing_machine.react&spell_targets.frostscythe>=2&(buff.deaths_due.stack=8|!death_and_decay.ticking|!covenant.night_fae)");
                if (TalentFrostscythe && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && CurrentRune >= 1 && API.PlayerHasBuff(KillingMachine) && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
                //obliteration->add_action(this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,if=buff.killing_machine.react|!buff.rime.up&spell_targets.howling_blast>=3");
                if ((!TalentFrostscythe || TalentFrostscythe && !IsAOE) && CurrentRune >= 2 && (API.PlayerHasBuff(KillingMachine)||(!API.PlayerHasBuff(Rime,false,false)) && API.TargetUnitInRangeCount>=3) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //obliteration->add_talent(this, "Glacial Advance", "if=spell_targets.glacial_advance>=2&(runic_power.deficit<10|rune.time_to_2>gcd)|(debuff.razorice.stack<5|debuff.razorice.remains<15)");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && TalentGlacialAdvance && RPDeficit < 10 && CurrentRP>=2 && API.CanCast(GlacialAdvance) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }
                //obliteration->add_action(this, "Frost Strike", "if=conduit.eradicating_blow&buff.eradicating_blow.stack=2&active_enemies=1");
                //obliteration->add_action(this, "Howling Blast", "if=buff.rime.up&spell_targets.howling_blast>=2");
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast)&& API.TargetUnitInRangeCount >AOEUnitNumber && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                //obliteration->add_talent(this, "Glacial Advance", "if=spell_targets.glacial_advance>=2");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP >= 30 && TalentGlacialAdvance && API.CanCast(GlacialAdvance) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }
                //obliteration->add_action(this, "Frost Strike", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,
                //if=!talent.avalanche&!buff.killing_machine.up|talent.avalanche&!buff.rime.up");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP >= 25 && 
                    ((!TalentAvalange && !API.PlayerHasBuff(KillingMachine)) || (TalentAvalange && !API.PlayerHasBuff(Rime)) )&& API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                //obliteration->add_action(this, "Howling Blast", "if=buff.rime.up");
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                //obliteration->add_action(this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice");
                if ((!TalentFrostscythe || TalentFrostscythe && !IsAOE) && CurrentRune >= 2 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
            }
            if (TalentObliteration && API.SpellCDDuration(PillarofFrost) <= 1000) 
            {
                //  obliteration_pooling->add_action(this, "Remorseless Winter", "if=talent.gathering_storm|conduit.everfrost|runeforge.biting_cold|active_enemies>=2", "Pooling For Obliteration: Starts 10 seconds before Pillar of Frost comes off CD");
                if ((TalentGatheringStorm || API.PlayerUnitInMeleeRangeCount >= 2) && CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                //obliteration_pooling->add_action(this, "Howling Blast", "if=buff.rime.up");
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                //obliteration_pooling->add_action(this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,
                //if=buff.killing_machine.react");
                if ((!TalentFrostscythe || TalentFrostscythe && !IsAOE) && CurrentRune >= 2 && API.PlayerHasBuff(KillingMachine) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //obliteration_pooling->add_talent(this, "Glacial Advance", "if=spell_targets.glacial_advance>=2&runic_power.deficit<60");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && TalentGlacialAdvance && API.CanCast(GlacialAdvance) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }
                //obliteration_pooling->add_action(this, "Frost Strike", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,
                //if=runic_power.deficit<70");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && RPDeficit < 70 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                //obliteration_pooling->add_action(this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,if=rune>4");
                if ((!TalentFrostscythe || TalentFrostscythe && !IsAOE) && CurrentRune >= 4 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //obliteration_pooling->add_talent(this, "Frostscythe", "if=active_enemies>=4&(!death_and_decay.ticking&covenant.night_fae|!covenant.night_fae)");
                if (TalentFrostscythe && API.PlayerUnitInMeleeRangeCount >= 4 && CurrentRune >= 1 && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
            }
            if(IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber)
            {
                // aoe->add_action(this, "Remorseless Winter", "", "AoE Rotation");
                if (CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                // aoe->add_talent(this, "Glacial Advance", "if=talent.frostscythe");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && TalentGlacialAdvance && API.CanCast(GlacialAdvance) && CurrentRP >= 30 && !TalentFrostscythe && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }
                //aoe->add_action(this, "Frost Strike", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,
                //if=cooldown.remorseless_winter.remains<=2*gcd&talent.gathering_storm");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && API.SpellCDDuration(RemorselessWinter) <= 2* API.SpellGCDTotalDuration && CurrentRP >= 25 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                //aoe->add_action(this, "Howling Blast", "if=buff.rime.up");
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                //aoe->add_talent(this, "Frostscythe", "if=buff.killing_machine.react&(!death_and_decay.ticking&covenant.night_fae|!covenant.night_fae)");
                if (TalentFrostscythe && CurrentRune >= 1 && API.PlayerHasBuff(KillingMachine) && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
                //aoe->add_talent(this, "Glacial Advance", "if=runic_power.deficit<(15+talent.runic_attenuation*3)");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && TalentGlacialAdvance && API.CanCast(GlacialAdvance) && RPDeficit < 15 + (TalentRunicAtt ? 15 : 0) && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }
                //aoe->add_action(this, "Frost Strike", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,
                //if=runic_power.deficit<(15+talent.runic_attenuation*3)");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && RPDeficit < 15 + (TalentRunicAtt ? 15 : 0) && API.CanCast(FrostStrike) && API.TargetRange <= 8)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                //aoe->add_action(this, "Remorseless Winter"); DUPLICATE BS
                //aoe->add_talent(this, "Frostscythe", "if=!death_and_decay.ticking&covenant.night_fae|!covenant.night_fae");
                if (TalentFrostscythe && CurrentRune >= 1 && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
                //aoe->add_action(this, "Obliterate", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice,
                //if=runic_power.deficit>(25+talent.runic_attenuation*3)");
                if (CurrentRune >= 2 && RPDeficit >= 25 + (TalentRunicAtt ? 15 : 0) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //aoe->add_talent(this, "Glacial Advance");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP>=30 && TalentGlacialAdvance && API.CanCast(GlacialAdvance) && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }
                //aoe->add_action(this, "Frost Strike", "target_if=max:(debuff.razorice.stack+1)%(debuff.razorice.remains+1)*death_knight.runeforge.razorice");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP >= 25 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                //aoe->add_talent(this, "Horn of Winter");
                if(TalentHornOfWinter && !TalentBreathOfSindra && API.CanCast(HornofWinter) && IsMelee)
                {
                    API.CastSpell(HornofWinter);
                    return;
                }
                //aoe->add_action("arcane_torrent");
            }
            //standard->add_action(this, "Remorseless Winter", "if=talent.gathering_storm|conduit.everfrost|runeforge.biting_cold", "Standard single-target rotation");
            if (TalentGatheringStorm && CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                //standard->add_talent(this, "Glacial Advance", "if=!death_knight.runeforge.razorice&(debuff.razorice.stack<5|debuff.razorice.remains<7)");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP >= 30 && TalentGlacialAdvance && API.CanCast(GlacialAdvance) && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }
                //standard->add_action(this, "Frost Strike", "if=cooldown.remorseless_winter.remains<=2*gcd&talent.gathering_storm");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && API.SpellCDDuration(RemorselessWinter) <= 2 * API.SpellGCDTotalDuration && CurrentRP >= 25 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                //standard->add_action(this, "Frost Strike", "if=conduit.eradicating_blow&buff.eradicating_blow.stack=2|conduit.unleashed_frenzy&buff.unleashed_frenzy.remains<3&buff.unleashed_frenzy.up");
                //TODO
                //standard->add_action(this, "Howling Blast", "if=buff.rime.up");
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                //standard->add_action(this, "Obliterate", "if=!buff.frozen_pulse.up&talent.frozen_pulse");
                if (TalentFrozenPulse && CurrentRune >= 3 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //standard->add_action(this, "Frost Strike", "if=runic_power.deficit<(15+talent.runic_attenuation*3)");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && RPDeficit < 15 + (TalentRunicAtt ? 15 : 0) && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                //standard->add_action(this, "Obliterate", "if=runic_power.deficit>(25+talent.runic_attenuation*3)");
                if (CurrentRune >= 2 && RPDeficit > 25 + (TalentRunicAtt ? 15 : 0) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                //standard->add_action(this, "Frost Strike");
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP >= 25 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                //standard->add_talent(this, "Horn of Winter");
                if (TalentHornOfWinter && !TalentBreathOfSindra && API.CanCast(HornofWinter) && IsMelee)
                {
                    API.CastSpell(HornofWinter);
                    return;
                }
                //standard->add_action("arcane_torrent");
            
        }

        public override void OutOfCombatPulse()
        {
        }
    }
}
