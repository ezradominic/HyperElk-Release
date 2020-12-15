using System.Linq;
using System.Diagnostics;

namespace HyperElk.Core
{
    public class HolyPriest : CombatRoutine
    {
        //Spell Strings
        private string HolyWordSerenity = "Holy Word: Serenity";
        private string HolyWordSanctify = "Holy Word: Sancity";
        private string CoH = "Circle of Healing";
        private string Halo = "Halo";
        private string FlashHeal = "Flash Heal";
        private string PoH = "Prayer of Healing";
        private string GuardianSpirit = "Guardian Spirit";
        private string HolyWordSalvation = "Holy Word: Salvation";
        private string DivineHymn = "Divine Hymn";
        private string Heal = "Heal";
        private string Renew = "Renew";
        private string HolyWordChastise = "Holy Word: Chastise";
        private string HolyFire = "Holy Fire";
        private string ShadowWordPain = "Shadow Word: Pain";
        private string Smite = "Smite";
        private string HolyNova = "Holy Nova";
        private string PrayerofMending = "Prayer of Mending";
        private string PowerInfusion = "Power Infusion";
        private string ShadowWordDeath = "Shadow Word: Death";
        private string PowerWordShield = "Power Word: Shield";
        private string LeapOfFaith = "Leap of Faith";
        private string MindControl = "Mind Control";
        private string MassDispel = "Mass Dispel";
        private string ShackleUndead = "Shackle Undead";
        private string MindSoothe = "Mind Soothe";
        private string PowerWordFortitude = "Power Word: Fortitude";
        private string SymbolOfHope = "Symbol of Hope";
        private string Fade = "Fade";
        private string DispelMagic = "Dispel Magic";
        private string PsychicScream = "Psychic Scream";
        private string SpiritOfRedemption = "Spirit of Redemption";
        private string BindingHeal = "Binding Heal";
        private string Apotheosis = "Apotheosis";
        private string BoonoftheAscended = "Boon of the Ascended";
        private string AscendedNova = "Ascended Nova";
        private string AscendedBlast = "Ascended Blast";
        private string Mindgames = "Mindgames";
        private string UnholyNova = "Unholy Nova";
        private string FaeGuardians = "Fae Guardians";
        private string Fleshcraft = "Fleshcraft";
        private string WeakenedSoul = "Weakened Soul";
        private string SurgeofLight = "Surge of Light";
        private string PrayerCircle = "Prayer Circle";
        private string AoE = "AOE";



        //Talents
        bool EnlightenmentTalent => API.PlayerIsTalentSelected(1, 1);
        bool TrailofLightTalent => API.PlayerIsTalentSelected(1, 2);
        bool RenewedFaithTalent => API.PlayerIsTalentSelected(1, 3);
        bool AgenlsMercyTalent => API.PlayerIsTalentSelected(2, 1);
        bool BodyandSoulTalent => API.PlayerIsTalentSelected(2, 2);
        bool AngelicFeatherTalent => API.PlayerIsTalentSelected(2, 3);
        bool ComiscRippleTalent => API.PlayerIsTalentSelected(3, 1);
        bool GuardianAngelTalent => API.PlayerIsTalentSelected(3, 2);
        bool AfterlifeTalent => API.PlayerIsTalentSelected(3, 3);
        bool PsychicVoiceTalent => API.PlayerIsTalentSelected(4, 1);
        bool CensureTalent => API.PlayerIsTalentSelected(4, 2);
        bool ShiningForceTalent => API.PlayerIsTalentSelected(4, 3);
        bool SurgeofLightTalent => API.PlayerIsTalentSelected(5, 1);
        bool BindingHealTalent => API.PlayerIsTalentSelected(5, 2);
        bool PrayerCircleTalent => API.PlayerIsTalentSelected(5, 3);
        bool BenedictionTalent => API.PlayerIsTalentSelected(6, 1);
        bool DivineStarTalent => API.PlayerIsTalentSelected(6, 2);
        bool HaloTalent => API.PlayerIsTalentSelected(6, 3);
        bool LightofTheNaaruTalent => API.PlayerIsTalentSelected(7, 1);
        bool ApotheosisTalent => API.PlayerIsTalentSelected(7, 2);
        bool HolyWordSalavationTalent => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40};


        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);


       // bool ChannelingCov => API.CurrentCastSpellID("player") == 323764;
       bool ChannelingDivine => API.CurrentCastSpellID("player") == 64843;

        private bool CoHAoE => UnitBelowHealthPercent(CoHLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool PoHAoE => UnitBelowHealthPercent(PoHLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool HaloAoE => UnitBelowHealthPercent(HaloLifePercent) >= AoENumber && !API.PlayerCanAttackTarget;
        private bool BoonAoE => UnitBelowHealthPercent(BoonLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool UnholyAoE => UnitBelowHealthPercent(UnholyNovaLifePercent) >= AoENumber && !API.PlayerCanAttackTarget && NotChanneling;
        private bool HWSancitfyAoE => UnitBelowHealthPercent(HolyWordSanctifyLifePercent) >= AoENumber;
        private bool DivineHymnAoE => UnitBelowHealthPercent(DivineHymnLifePercent) >= AoENumber;
        private bool HWSalAoE => UnitBelowHealthPercent(HolyWordSalvationLifePercent) >= AoENumber;
        private bool HWSCheck => API.CanCast(HolyWordSalvation) && HolyWordSalavationTalent && HWSalAoE && Mana >= 6 && !ChannelingDivine && !API.PlayerIsMoving;
        private bool CoHCheck => API.CanCast(CoH) && CoHAoE && Mana >= 4 && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingDivine;
        private bool DHCheck => API.CanCast(DivineHymn) && DivineHymnAoE && Mana >= 5 && !API.PlayerCanAttackTarget && !API.PlayerIsMoving;
        private bool PoHCheck => API.CanCast(PoH) && PoHAoE && Mana >= 5 && !API.PlayerIsMoving && !ChannelingDivine;
        private bool GSCheck => API.CanCast(GuardianSpirit) && API.TargetHealthPercent <= GuardianSpirtLifePercent && Mana >= 1 && !API.PlayerCanAttackTarget && NotChanneling && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingDivine;
        private bool HealCheck => API.CanCast(Heal) && API.TargetHealthPercent <= HealLifePercent && Mana >= 3 && !API.PlayerCanAttackTarget && NotChanneling && !API.PlayerIsMoving && !ChannelingDivine;
        private bool FlashHealCheck => API.CanCast(FlashHeal) && API.TargetHealthPercent <= FlashHealLifePercent && Mana >=4 && !API.PlayerCanAttackTarget && NotChanneling && !API.PlayerIsMoving && !ChannelingDivine;
        private bool RenewCheck => API.CanCast(Renew) && API.TargetHealthPercent <= RenewLifePercent && Mana >= 2 && !API.PlayerCanAttackTarget && !API.TargetHasBuff(Renew) && NotChanneling && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingDivine;
        private bool HolyWordSerenityCheck => API.CanCast(HolyWordSerenity) && API.TargetHealthPercent <= HolyWordSerenityLifePercent && Mana >= 4 && !API.PlayerCanAttackTarget && API.SpellCharges(HolyWordSerenity) > 0 && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingDivine;
        private bool HaloCheck => API.CanCast(Halo) && HaloAoE && !ChannelingDivine && Mana >= 3 && HaloTalent && !API.PlayerIsMoving;

       // private bool HWSancitfyCheck => HWSancitfyAoE && !API.PlayerCanAttackTarget && NotChanneling && (!API.PlayerIsMoving || API.PlayerIsMoving);
        private bool PoMCheck => API.TargetHealthPercent <= PoMLifePercent && !API.PlayerCanAttackTarget && Mana >= 2 && !API.PlayerIsMoving && !ChannelingDivine;
        private bool KyrianCheck => API.CanCast(BoonoftheAscended) && PlayerCovenantSettings == "Kyrian" && BoonAoE && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && !API.PlayerIsMoving && !ChannelingDivine;
        private bool NightFaeCheck => API.CanCast(FaeGuardians) && PlayerCovenantSettings == "Night Fae" && Mana >= 2 && API.TargetHealthPercent >= FaeLifePercent && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !ChannelingDivine;
        private bool NecrolordCheck => API.CanCast(UnholyNova) && PlayerCovenantSettings == "Necrolord" && UnholyAoE && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingDivine;
        private bool VenthyrCheck => API.CanCast(Mindgames) && PlayerCovenantSettings == "Venthyr" && Mana >= 2 && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && (!API.PlayerIsMoving || API.PlayerIsMoving) && !ChannelingDivine;
        


        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private bool IsAutoSwap => CombatRoutine.GetPropertyBool("Auto Swap");
        private bool IsOOC => CombatRoutine.GetPropertyBool("OOC");
        private int HealLifePercent => numbList[CombatRoutine.GetPropertyInt(Heal)];
        private int FlashHealLifePercent => numbList[CombatRoutine.GetPropertyInt(FlashHeal)];
        private int RenewLifePercent => numbList[CombatRoutine.GetPropertyInt(Renew)];
        private int HolyWordSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyWordSerenity)];
        private int HolyWordSanctifyLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyWordSanctify)];
        private int PoHLifePercent => numbList[CombatRoutine.GetPropertyInt(PoH)];
        private int PoMLifePercent => numbList[CombatRoutine.GetPropertyInt(PrayerofMending)];
        private int DivineHymnLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineHymn)];
        private int CoHLifePercent => numbList[CombatRoutine.GetPropertyInt(CoH)];
        private int HaloLifePercent => numbList[CombatRoutine.GetPropertyInt(Halo)];
        private int HolyWordSalvationLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyWordSalvation)];
        private int BoonLifePercent => numbList[CombatRoutine.GetPropertyInt(BoonoftheAscended)];
        private int MindgamesLifePercent => numbList[CombatRoutine.GetPropertyInt(Mindgames)];
        private int UnholyNovaLifePercent => numbList[CombatRoutine.GetPropertyInt(UnholyNova)];
        private int FaeLifePercent => numbList[CombatRoutine.GetPropertyInt(FaeGuardians)];
        private int GuardianSpirtLifePercent => numbList[CombatRoutine.GetPropertyInt(GuardianSpirit)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        //private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoER)];

        //General

        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private bool IsMelee => API.TargetRange < 12;
       // private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private int Mana => API.PlayerMana;




        //  public bool isInterrupt => CombatRoutine.GetPropertyBool("KICK") && API.TargetCanInterrupted && API.TargetIsCasting && (API.TargetIsChanneling ? API.TargetElapsedCastTime >= interruptDelay : API.TargetCurrentCastTimeRemaining <= interruptDelay);
        //  public int interruptDelay => random.Next((int)(CombatRoutine.GetPropertyInt("KICKTime") * 0.9), (int)(CombatRoutine.GetPropertyInt("KICKTime") * 1.1));

        public override void Initialize()
        {
            CombatRoutine.Name = "Holy Priest by Ryu";
            API.WriteLog("Welcome to Holy Priest by Ryu");
            API.WriteLog("BETA ROTATION : Things may be missing or not work correctly yet. Please post feedback in Priest channel. Cov expect Unholy Nova is supported via Cooldown toggle or break marcos");
           // API.WriteLog("Mouseover Support is added. Please create /cast [@mouseover] xx whereas xx is your spell and assign it the binds with MO on it in keybinds.");
            API.WriteLog("If you want to use Holy Word: Sanctify, you need to use an @Cursor macro with /xxx break or break2 whereas xxx is your addon name(First five only) for it to work");
            API.WriteLog("Target Spec" + API.TargetRoleSpec);

            //Buff
            CombatRoutine.AddBuff(PowerWordFortitude);
            CombatRoutine.AddBuff(PowerWordShield);
            CombatRoutine.AddBuff(SurgeofLight);
            CombatRoutine.AddBuff(PrayerCircle);
            CombatRoutine.AddBuff(Apotheosis);
            CombatRoutine.AddBuff(SpiritOfRedemption);
            CombatRoutine.AddBuff(Renew);


            //Debuff
            CombatRoutine.AddDebuff(WeakenedSoul);
            CombatRoutine.AddDebuff(ShadowWordPain);

            //Spell
            CombatRoutine.AddSpell(Heal);
            CombatRoutine.AddSpell(FlashHeal);
            CombatRoutine.AddSpell(Renew);
            CombatRoutine.AddSpell(HolyWordSerenity);
            CombatRoutine.AddSpell(HolyWordSanctify);
            CombatRoutine.AddSpell(PoH);
            CombatRoutine.AddSpell(DivineHymn);
            CombatRoutine.AddSpell(CoH);
            CombatRoutine.AddSpell(HolyWordChastise);
            CombatRoutine.AddSpell(Smite);
            CombatRoutine.AddSpell(HolyNova);
            CombatRoutine.AddSpell(HolyFire);
            CombatRoutine.AddSpell(SymbolOfHope);
            CombatRoutine.AddSpell(Fade);
            CombatRoutine.AddSpell(DispelMagic);
            CombatRoutine.AddSpell(GuardianSpirit);
            CombatRoutine.AddSpell(LeapOfFaith);
            CombatRoutine.AddSpell(MindControl);
            CombatRoutine.AddSpell(MassDispel);
            CombatRoutine.AddSpell(ShackleUndead);
            CombatRoutine.AddSpell(Fleshcraft);
            CombatRoutine.AddSpell(MindSoothe);
            CombatRoutine.AddSpell(PowerInfusion);
            CombatRoutine.AddSpell(PsychicScream);
            CombatRoutine.AddSpell(BoonoftheAscended);
            CombatRoutine.AddSpell(BindingHeal);
            CombatRoutine.AddSpell(AscendedBlast);
            CombatRoutine.AddSpell(AscendedNova);
            CombatRoutine.AddSpell(Mindgames);
            CombatRoutine.AddSpell(UnholyNova);
            CombatRoutine.AddSpell(FaeGuardians);

            //Macro


            //Prop
            // CombatRoutine.AddProp("Auto Swap", "Auto Swap", false, "Use Auto Swap Logic", "Generic");
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 8);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used and meets the below healing perecents", "Cooldowns", 1);

            CombatRoutine.AddProp("OOC", "Healing out of Combat", true, "Heal out of combat", "Healing");
            CombatRoutine.AddProp(Heal, Heal + " Life Percent", numbList, "Life percent at which " + Heal + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(FlashHeal, FlashHeal + " Life Percent", numbList, "Life percent at which " + FlashHeal + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(PrayerofMending, PrayerofMending + " Life Percent", numbList, "Life percent at which " + PrayerofMending + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(HolyWordSerenity, HolyWordSerenity + " Life Percent", numbList, "Life percent at which " + HolyWordSerenity + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(Renew, Renew + " Life Percent", numbList, "Life percent at which " + Renew + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(FaeGuardians, FaeGuardians + " Life Percent", numbList, "Life percent at which " + FaeGuardians + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(GuardianSpirit, GuardianSpirit + " Life Percent", numbList, "Life percent at which " + GuardianSpirit + " is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(Halo, Halo + " Life Percent", numbList, "Life percent at which " + Halo + " is used when AoE Number of members are at, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(HolyWordSalvation, HolyWordSalvation + " Life Percent", numbList, "Life percent at which " + HolyWordSalvation + " is used when AoE Number of members are at life percent, if talented, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(CoH, CoH + " Life Percent", numbList, "Life percent at which " + CoH + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(PoH, PoH + " Life Percent", numbList, "Life percent at which " + PoH + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 20);
            CombatRoutine.AddProp(DivineHymn , DivineHymn + " Life Percent", numbList, "Life percent at which " + DivineHymn + " is used when AoE Number of members are at life percent, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(AoE, "Number of units for AoE Healing ", numbPartyList, " Units for AoE Healing", "Healing", 3);


        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted && (IsOOC || API.PlayerIsInCombat))
            {
                if (RenewCheck && InRange)
                {
                    API.CastSpell(Renew);
                    return;
                }
                if (HealCheck && InRange)
                {
                    API.CastSpell(Heal);
                    return;
                }
                if (FlashHealCheck && InRange)
                {
                    API.CastSpell(FlashHeal);
                    return;
                }
                if (PoMCheck && InRange)
                {
                    API.CastSpell(PrayerofMending);
                    return;
                }
                if (HWSCheck && InRange)
                {
                    API.CastSpell(HolyWordSalvation);
                    return;
                }
                if (HolyWordSerenityCheck && InRange)
                {
                    API.CastSpell(HolyWordSerenity);
                    return;
                }
                if (KyrianCheck && InRange)
                {
                    API.CastSpell(BoonoftheAscended);
                    return;
                }
                if (NightFaeCheck && InRange)
                {
                    API.CastSpell(FaeGuardians);
                    return;
                }
                if (GSCheck && InRange)
                {
                    API.CastSpell(GuardianSpirit);
                    return;
                }
                if (CoHCheck & InRange)
                {
                    API.CastSpell(CoH);
                    return;
                }
                if (PoHCheck && InRange)
                {
                    API.CastSpell(PoH);
                    return;
                }
                if (DHCheck && InRange)
                {
                    API.CastSpell(DivineHymn);
                    return;
                }
                if (HaloCheck && InRange)
                {
                    API.CastSpell(Halo);
                    return;
                }

            }
        }
        public override void CombatPulse()
        {
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotChanneling && !ChannelingDivine && !API.PlayerIsMoving)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.CanCast(HolyWordChastise) && InRange && Mana >= 2 && (API.PlayerIsMoving || !API.PlayerIsMoving) && !ChannelingDivine)
            {
                API.CastSpell(HolyWordChastise);
                return;
            }
            if (API.CanCast(HolyFire) && InRange && Mana >= 1 && !ChannelingDivine && !API.PlayerIsMoving)
            {
                API.CastSpell(HolyFire);
                return;
            }
            if (API.CanCast(ShadowWordPain) && InRange && Mana >= 1 && !API.TargetHasDebuff(ShadowWordPain) && (API.PlayerIsMoving || !API.PlayerIsMoving) && !ChannelingDivine)
            {
                API.CanCast(ShadowWordPain);
                return;
            }
            if (API.CanCast(HolyNova) && IsMelee && API.TargetUnitInRangeCount >= 3 && !ChannelingDivine && Mana >= 2 && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CanCast(HolyNova);
                return;
            }
            if (API.CanCast(AscendedNova) && PlayerCovenantSettings == "Kyrian" && API.TargetRange <= 8 && !ChannelingDivine && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CanCast(AscendedNova);
                return;
            }
            if (API.CanCast(AscendedBlast) && PlayerCovenantSettings == "Kyrian" && !ChannelingDivine && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell(AscendedBlast);
                return;
            }
            if (API.CanCast(Smite) && !ChannelingDivine && Mana >= 1 && !ChannelingDivine && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell(Smite);
                return;
            }
            if (VenthyrCheck && InRange)
            {
                API.CastSpell(Mindgames);
                return;
            }
        }

        public override void OutOfCombatPulse()
        {

        }

    }
}



