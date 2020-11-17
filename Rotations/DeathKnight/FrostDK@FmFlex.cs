
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
        private string ColdHeart = "Cold Heart";
        private string DarkSuccor = "Dark Succor";
        private string Rime = "Rime";
        private string KillingMachine = "Killing Machine";

        private string FrostFever = "Frost Fever";

        //Talent

        private bool TalentColdHeart => API.PlayerIsTalentSelected(1, 3);
        private bool TalentHornOfWinter => API.PlayerIsTalentSelected(2, 3);
        private bool TalentFrostscythe => API.PlayerIsTalentSelected(4, 3);
        private bool TalentDeathPact => API.PlayerIsTalentSelected(5, 3);
        private bool TalentGlacialAdvance => API.PlayerIsTalentSelected(6, 3);
        private bool TalentGatheringStorm => API.PlayerIsTalentSelected(6, 1);
        private bool TalentHypnothermicPre => API.PlayerIsTalentSelected(6, 2);

        private bool TalentBreathOfSindra => API.PlayerIsTalentSelected(7, 3);
        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;

        //DK specific 

        private int CurrentRune
        {
            get
            {
                int currentRune = 0;
                for (int i = 1; i <= 6; i++)
                    currentRune = currentRune + (API.PlayerRuneCD(i) <= 40 ? 1 : 0);
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

            CombatRoutine.AddSpell(IceboundFortitude, "D1");
            CombatRoutine.AddSpell(AntiMagicShell, "D1");
            CombatRoutine.AddSpell(Lichborne, "D1");

            CombatRoutine.AddBuff(ColdHeart);
            CombatRoutine.AddBuff(DarkSuccor);
            CombatRoutine.AddBuff(Rime);
            CombatRoutine.AddBuff(KillingMachine);
            CombatRoutine.AddBuff(PillarofFrost);
            CombatRoutine.AddBuff(BreathofSindragosa);

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

            if (isInterrupt && API.TargetRange <= 30 && API.TargetCanInterrupted && API.TargetIsCasting &&
                API.TargetCurrentCastTimeRemaining < interruptDelay && !API.SpellISOnCooldown(MindFreeze) && PlayerLevel >= 27)
            {
                API.CastSpell(MindFreeze);
                return;
            }
            if (UseDND && IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && CurrentRune >= 1 && API.CanCast(DeathandDecay) && IsMelee)
            {
                API.CastSpell(DeathandDecay);
                return;
            }

            if (((API.PlayerHealthPercent < DeathStrikeLifePercent && CurrentRP >= 45) || (API.PlayerHealthPercent <= DeathStrikeProcLifePercent && API.PlayerHasBuff(DarkSuccor))) && API.CanCast(DeathStrike, true, true) && IsMelee)
            {
                API.CastSpell(DeathStrike);
                return;
            }
            if (IsCooldowns)
            {
                if (IsCooldowns && PlayerLevel >= 12 && API.CanCast(RaiseDead) && API.TargetRange <= 40)
                {
                    API.CastSpell(RaiseDead);
                    return;
                }
                if (IsCooldowns && PlayerLevel >= 29 && API.CanCast(PillarofFrost) &&
                    (!TalentBreathOfSindra || API.PlayerHasBuff(BreathofSindragosa) || API.SpellCDDuration(BreathofSindragosa) >= 5000)
                    && IsMelee)
                {
                    API.CastSpell(PillarofFrost);
                    return;
                }

                if (IsCooldowns && TalentBreathOfSindra && API.CanCast(BreathofSindragosa) && CurrentRP >= 80 && IsMelee)
                {
                    API.CastSpell(BreathofSindragosa);
                    return;
                }

            }
            if (!API.PlayerHasBuff(BreathofSindragosa))
            {
                if (PlayerLevel >= 30 && IsAOE && API.CanCast(ChillStreak, true, true) && API.TargetUnitInRangeCount >= AOEUnitNumber && API.TargetRange <= 40)
                {
                    API.CastSpell(ChillStreak);
                    return;
                }
                if (TalentColdHeart && CurrentRune >= 1 && API.PlayerBuffStacks(ColdHeart) == 20 && API.CanCast(ChainsofIce) &&
                    ((API.PlayerHasBuff(PillarofFrost) && API.PlayerBuffTimeRemaining(PillarofFrost) <= 3 * API.SpellGCDTotalDuration) || !API.PlayerHasBuff(PillarofFrost))
                    && API.TargetRange <= 30)
                {
                    API.CastSpell(ChainsofIce);
                    return;
                }
                if (IsCooldowns && PlayerLevel >= 44 && API.PlayerHasBuff(PillarofFrost,false,true) && API.PlayerBuffTimeRemaining(PillarofFrost) <= 2 * API.SpellGCDTotalDuration
                    && (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || !IsAOE) && API.CanCast(FrostwyrmsFury))
                {
                    API.CastSpell(FrostwyrmsFury);
                    return;
                }


                if (TalentGatheringStorm && CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                if (CurrentRune >= 4 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP >= 90 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }

                if ((!TalentFrostscythe || TalentFrostscythe && !IsAOE) && CurrentRune >= 2 && API.PlayerHasBuff(KillingMachine) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                if (TalentFrostscythe && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && CurrentRune >= 1 && API.PlayerHasBuff(KillingMachine) && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
                if (IsAOE && CurrentRune >= 1 && IsAOE && API.CanCast(RemorselessWinter) && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && (!TalentBreathOfSindra || !IsCooldowns || API.SpellISOnCooldown(BreathofSindragosa)) && CurrentRP >= 70 && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                if ((!TalentFrostscythe || TalentFrostscythe && !IsAOE) && CurrentRune >= 2 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                if (TalentFrostscythe && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && CurrentRune >= 1 && API.CanCast(Frostscythe) && API.TargetRange <= 8)
                {
                    API.CastSpell(Frostscythe);
                    return;
                }
                if (IsAOE && API.PlayerHealthPercent >= DeathStrikeLifePercent && (!TalentBreathOfSindra || !IsCooldowns || API.SpellISOnCooldown(BreathofSindragosa)) && TalentGlacialAdvance && CurrentRP >= 35 && API.CanCast(GlacialAdvance) && IsMelee)
                {
                    API.CastSpell(GlacialAdvance);
                    return;
                }

                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP >= 25 && (!TalentBreathOfSindra || !IsCooldowns || API.SpellISOnCooldown(BreathofSindragosa)) && API.CanCast(FrostStrike) && IsMelee)
                {
                    API.CastSpell(FrostStrike);
                    return;
                }
                if (IsCooldowns&& API.CanCast(EmpowerRuneWeapon)&& API.PlayerHasBuff(PillarofFrost)&& CurrentRune <5 && CurrentRP<=90 && IsMelee && !TalentBreathOfSindra)
                {
                    API.CastSpell(EmpowerRuneWeapon);
                    return;
                }
                if (TalentHornOfWinter && API.CanCast(HornofWinter) && IsMelee && (!TalentBreathOfSindra || (!API.SpellISOnCooldown(BreathofSindragosa) && API.SpellCDDuration(BreathofSindragosa) >= 300)))
                {
                    API.CastSpell(HornofWinter);
                    return;
                }
                if (API.PlayerHealthPercent >= DeathStrikeLifePercent && CurrentRP >= 90 && API.CanCast(DeathCoil) && !IsMelee && API.TargetRange <= 30)
                {
                    API.CastSpell(DeathCoil);
                    return;
                }
                if (CurrentRune >= 5 && API.CanCast(HowlingBlast) && !IsMelee && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
            }
            else
            {
                if (CurrentRune >= 2 && CurrentRP <= 30 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                if (TalentHornOfWinter && CurrentRP <= 30 && API.CanCast(HornofWinter) && IsMelee)
                {
                    API.CastSpell(HornofWinter);
                    return;
                }
                if (TalentGatheringStorm && CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                if (API.PlayerHasBuff(Rime) && API.CanCast(HowlingBlast) && API.TargetRange <= 30)
                {
                    API.CastSpell(HowlingBlast);
                    return;
                }
                if (CurrentRune >= 2 && (CurrentRP <= 45 || CurrentRune >= 4) && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                if (CurrentRune >= 1 && API.CanCast(RemorselessWinter) && IsMelee)
                {
                    API.CastSpell(RemorselessWinter);
                    return;
                }
                if (CurrentRune >= 2 && API.CanCast(Obliterate) && IsMelee)
                {
                    API.CastSpell(Obliterate);
                    return;
                }
                if (API.CanCast(EmpowerRuneWeapon) && IsMelee)
                {
                    API.CastSpell(EmpowerRuneWeapon);
                    return;
                }
                if (TalentHornOfWinter && API.CanCast(HornofWinter) && IsMelee)
                {
                    API.CastSpell(HornofWinter);
                    return;
                }

            }
        }

        public override void OutOfCombatPulse()
        {
        }
    }
}
