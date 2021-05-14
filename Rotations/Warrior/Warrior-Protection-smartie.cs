// Changelog
// v1.0 First release
// v1.1 Victory Rush fix
// v1.2 covenants added - shield block changed
// v1.3 covenant update
// v1.4 heroic throw update
// v1.5 condemn fix
// v1.6 dps toggle and alot more fine tuning for more defensives
// v1.7 rage update
// v1.8 Racials and Trinkets
// v1.9 condemn fix hopefully
// v2.0 typo fix
// v2.1 Spell ids and alot of other stuff
// v2.2 Rallying cry added
// v2.3 Racials and a few other things
// v2.4 Torghast update
// v2.45 Battle shout fix
// v2.5 rewrite
// v2.6 intervene problem solved and rage dump fixed
// v2.7 shockwave options added
// v2.8 explosive protection
// v2.9 hotfix

using System.Linq;

namespace HyperElk.Core
{
    public class ProtWarrior : CombatRoutine
    {
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool IsRavagerOn => API.ToggleIsEnabled("Ravager");
        //Spell,Auras
        private string ShieldSlam = "Shield Slam";
        private string Devastate = "Devastate";
        private string ShieldBlock = "Shield Block";
        private string ThunderClap = "Thunder Clap";
        private string Revenge = "Revenge";
        private string IgnorePain = "Ignore Pain";
        private string Execute = "Execute";
        private string Avatar = "Avatar";
        private string Shockwave = "Shockwave";
        private string Pummel = "Pummel";
        private string RallyingCry = "Rallying Cry";
        private string SpellReflection = "Spell Reflection";
        private string VictoryRush = "Victory Rush";
        private string ImpendingVictory = "Impending Victory";
        private string ShieldWall = "Shield Wall";
        private string LastStand = "Last Stand";
        private string DemoralizingShout = "Demoralizing Shout";
        private string HeroicThrow = "Heroic Throw";
        private string BattleShout = "Battle Shout";
        private string StormBolt = "Storm Bolt";
        private string DragonRoar = "Dragon Roar";
        private string Ravager = "Ravager";
        private string DeepWounds = "Deep Wounds";
        private string FreeRevenge = "Revenge!";
        private string VengeanceRevenge = "Vengeance: Revenge";
        private string VengeanceIgnorePain = "Vengeance: Ignore Pain";
        private string RenewedFury = "Renewed Fury";
        private string Victorious = "Victorious";
        private string Condemn = "Condemn";
        private string SpearofBastion = "Spear of Bastion";
        private string AncientAftershock = "Ancient Aftershock";
        private string ConquerorsBanner = "Conqueror's Banner";
        private string PhialofSerenity = "Phial of Serenity";
        private string BerserkerRage = "Berserker Rage";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string AoE = "AOE";
        private string AoERaid = "AoERaid";
        private string NoLImitCondemn = "NoLImitCondemn";
        private string Intervene = "Intervene";

        //Talents
        bool TalentDevastator => API.PlayerIsTalentSelected(1, 3);
        bool TalentStormBolt => API.PlayerIsTalentSelected(2, 3);
        bool TalentDragonRoar => API.PlayerIsTalentSelected(3, 3);
        bool TalentUnstopableForce => API.PlayerIsTalentSelected(6, 2);
        bool TalentBoomingVoice => API.PlayerIsTalentSelected(3, 2);
        bool TalentImpendingVictory => API.PlayerIsTalentSelected(5, 3);
        bool TalentRavager => API.PlayerIsTalentSelected(6, 3);

        //General
        private bool isExplosive => API.TargetMaxHealth <= 600 && API.TargetMaxHealth != 0 && PlayerLevel == 60;
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;
        private float gcd => API.SpellGCDTotalDuration;
        bool IsCovenant => (UseCovenant == "with Cooldowns" || UseCovenant == "with Cooldowns or AoE" || UseCovenant == "on mobcount or Cooldowns") && IsCooldowns || UseCovenant == "always" || (UseCovenant == "on AOE" || UseCovenant == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseCovenant == "on mobcount or Cooldowns" || UseCovenant == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsAvatar => (UseAvatar == "with Cooldowns" || UseAvatar == "with Cooldowns or AoE" || UseAvatar == "on mobcount or Cooldowns") && IsCooldowns || UseAvatar == "always" || (UseAvatar == "on AOE" || UseAvatar == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseAvatar == "on mobcount or Cooldowns" || UseAvatar == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsRavager => (UseRavager == "with Cooldowns" || UseRavager == "with Cooldowns or AoE" || UseRavager == "on mobcount or Cooldowns") && IsCooldowns || UseRavager == "always" || (UseRavager == "on AOE" || UseRavager == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseRavager == "on mobcount or Cooldowns" || UseRavager == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount;
        bool IsTrinkets1 => ((UseTrinket1 == "with Cooldowns" || UseTrinket1 == "with Cooldowns or AoE" || UseTrinket1 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket1 == "always" || (UseTrinket1 == "on AOE" || UseTrinket1 == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseTrinket1 == "on mobcount or Cooldowns" || UseTrinket1 == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount || UseTrinket1 == "on Health" && API.PlayerHealthPercent <= TrinketLifePercent) && IsMelee;
        bool IsTrinkets2 => ((UseTrinket2 == "with Cooldowns" || UseTrinket2 == "with Cooldowns or AoE" || UseTrinket2 == "on mobcount or Cooldowns") && IsCooldowns || UseTrinket2 == "always" || (UseTrinket2 == "on AOE" || UseTrinket2 == "with Cooldowns or AoE") && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber || (UseTrinket2 == "on mobcount or Cooldowns" || UseTrinket2 == "on mobcount") && API.PlayerUnitInMeleeRangeCount >= MobCount || UseTrinket1 == "on Health" && API.PlayerHealthPercent <= TrinketLifePercent) && IsMelee;
        bool IsShockwave => UseShockwave == "as 2nd kick" && API.SpellISOnCooldown(Pummel) && isInterrupt || UseShockwave == "on AOE" && API.PlayerUnitInMeleeRangeCount >= AoENumber || UseShockwave == "on mobcount" && API.PlayerUnitInMeleeRangeCount >= MobCount;

        //CBProperties
        int[] SpellReflectCast = { 335114, 335304, 326430, 328890, 329742, 331573, 342321, 330968, 347350, 327773, 326851, 320008, 332678, 332707, 332705, 332605, 334076, 332196, 328707, 333250, 332234, 333711, 323544, 323569, 328322, 323538, 338003, 325876, 325872, 325700, 326891, 326829, 323057, 326319, 322767, 322557, 325021, 325223, 325224, 325418, 326092, 322486, 322487, 329110, 328002, 328180, 328094, 334926, 320512, 322554, 328593, 334660, 326712, 321249, 326827, 346537, 326837, 326952, 321038, 323195, 324608, 317661, 323804, 333602, 320171, 320788, 334748, 320462, 333479, 320120, 320300, 319669, 324079, 324589, 330784, 330700, 330703, 330784, 333299, 330875, 345245, 332550, 330810, 340678, 330926, 116858, 203286};
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        int[] RageDump = new int[] { 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        string[] heroiclist = new string[] { "Not Used", "when out of melee", "only Mouseover", "both" };
        public new string[] CDUsage = new string[] { "Not Used", "with Cooldowns", "always" };
        public new string[] CDUsageWithAOE = new string[] { "Not Used", "with Cooldowns", "on AOE", "with Cooldowns or AoE", "on mobcount", "on mobcount or Cooldowns", "always" };
        public string[] trinkets = new string[] { "Not Used", "with Cooldowns", "on AOE", "with Cooldowns or AoE", "on mobcount", "on mobcount or Cooldowns", "always", "on Health" };
        public string[] ShockwaveUsage = new string[] { "Not Used", "as 2nd kick", "on AOE", "on mobcount"};
        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);

        private bool RallyAoE => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(RallyLifePercent) >= AoERaidNumber : UnitBelowHealthPercentParty(RallyLifePercent) >= AoENumber;

        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int AoERaidNumber => numbRaidList[CombatRoutine.GetPropertyInt(AoERaid)];
        private string UseTrinket1 => trinkets[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => trinkets[CombatRoutine.GetPropertyInt("Trinket2")];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        public bool UseIntervene => CombatRoutine.GetPropertyBool("UseIntervene");
        private string UseHeroicThrow => heroiclist[CombatRoutine.GetPropertyInt(HeroicThrow)];
        private int MobCount => numbRaidList[CombatRoutine.GetPropertyInt("MobCount")];
        private string UseShockwave => ShockwaveUsage[CombatRoutine.GetPropertyInt(Shockwave)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseAvatar => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Avatar)];
        private string UseRavager => CDUsageWithAOE[CombatRoutine.GetPropertyInt(Ravager)];
        private int TrinketLifePercent => numbList[CombatRoutine.GetPropertyInt("TrinketLife")];
        private int Ragedumping => RageDump[CombatRoutine.GetPropertyInt("Ragedumping")];
        private int LastStandLifePercent => numbList[CombatRoutine.GetPropertyInt(LastStand)];
        private int ShieldWallLifePercent => numbList[CombatRoutine.GetPropertyInt(ShieldWall)];
        private int RallyLifePercent => numbList[CombatRoutine.GetPropertyInt(RallyingCry)];
        private int VictoryRushLifePercent => numbList[CombatRoutine.GetPropertyInt(VictoryRush)];
        private int ImpendingVictoryLifePercent => numbList[CombatRoutine.GetPropertyInt(ImpendingVictory)];
        private int IgnorePainLifePercent => numbList[CombatRoutine.GetPropertyInt(IgnorePain)];
        private int DemoralizingShoutLifePercent => numbList[CombatRoutine.GetPropertyInt(DemoralizingShout)];
        private int ShieldBlockFirstChargeLifePercent => numbList[CombatRoutine.GetPropertyInt("FirstShieldBlock")];
        private int ShieldBlockSecondChargeLifePercent => numbList[CombatRoutine.GetPropertyInt("SecondShieldBlock")];
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];


        public override void Initialize()
        {
            CombatRoutine.Name = "Protection Warrior by smartie";
            API.WriteLog("Welcome to smartie`s Protection Warrior v2.9");
            API.WriteLog("You need the following macros");
            API.WriteLog("Heroic ThrowMO: /cast [@mouseover] Heroic Throw");
            API.WriteLog("InterveneMO: /cast [@mouseover,exists,help] Intervene");

            //Spells
            CombatRoutine.AddSpell(ShieldSlam, 23922, "D4");
            CombatRoutine.AddSpell(Devastate, 20243, "D5");
            CombatRoutine.AddSpell(ShieldBlock, 2565, "D6");
            CombatRoutine.AddSpell(ThunderClap, 6343, "D7");
            CombatRoutine.AddSpell(Revenge, 6572, "D8");
            CombatRoutine.AddSpell(IgnorePain, 190456, "D9");
            CombatRoutine.AddSpell(Execute, 163201, "D0");
            CombatRoutine.AddSpell(Avatar, 107574, "D2");
            CombatRoutine.AddSpell(Shockwave, 46968, "F12");
            CombatRoutine.AddSpell(Pummel, 6552, "F11");
            CombatRoutine.AddSpell(BerserkerRage, 18499, "F1");
            CombatRoutine.AddSpell(RallyingCry, 97462, "F7");
            CombatRoutine.AddSpell(SpellReflection, 23920, "F6");
            CombatRoutine.AddSpell(VictoryRush, 34428, "F5");
            CombatRoutine.AddSpell(ImpendingVictory, 202168, "F5");
            CombatRoutine.AddSpell(ShieldWall, 871, "F4");
            CombatRoutine.AddSpell(LastStand, 12975, "F2");
            CombatRoutine.AddSpell(DemoralizingShout, 1160, "F1");
            CombatRoutine.AddSpell(HeroicThrow, 57755, "NumPad1");
            CombatRoutine.AddSpell(BattleShout, 6673, "NumPad3");
            CombatRoutine.AddSpell(StormBolt, 107570, "F8");
            CombatRoutine.AddSpell(DragonRoar, 118000, "NumPad5");
            CombatRoutine.AddSpell(Ravager, 228920, "NumPad6");
            CombatRoutine.AddSpell(Condemn, 317349, "D0");
            CombatRoutine.AddSpell(ConquerorsBanner, 324143, "D1");
            CombatRoutine.AddSpell(AncientAftershock, 325886, "D1");
            CombatRoutine.AddSpell(SpearofBastion, 307865, "D1");
            CombatRoutine.AddSpell(Intervene, 3411, "NumPad2");

            //Macros
            CombatRoutine.AddMacro(HeroicThrow + "MO", "D2");
            CombatRoutine.AddMacro(Intervene + "MO", "NumPad2");
            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");


            //Buffs
            CombatRoutine.AddBuff(FreeRevenge, 5302);
            CombatRoutine.AddBuff(Avatar, 107574);
            CombatRoutine.AddBuff(VengeanceRevenge, 202573);
            CombatRoutine.AddBuff(VengeanceIgnorePain, 202574);
            CombatRoutine.AddBuff(IgnorePain, 190456);
            CombatRoutine.AddBuff(RenewedFury, 202288);
            CombatRoutine.AddBuff(ShieldBlock, 132404);
            CombatRoutine.AddBuff(LastStand, 12975);
            CombatRoutine.AddBuff(SpellReflection, 23920);
            CombatRoutine.AddBuff(BattleShout, 6673);
            CombatRoutine.AddBuff(Victorious, 32216);
            CombatRoutine.AddBuff(NoLImitCondemn, 329214);

            //Debuff
            CombatRoutine.AddDebuff(DeepWounds, 115767);

            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Ravager");

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

            //Prop
            CombatRoutine.AddProp("Trinket1", "Use " + "Trinket 1", trinkets, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", trinkets, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("MouseoverInCombat", "Only Mouseover in combat", false, " Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("UseIntervene", "Use "+ Intervene, false, " Use " + Intervene + " mouseover", "Generic");
            CombatRoutine.AddProp(HeroicThrow, "Use Heroic Throw", heroiclist, "Use " + HeroicThrow + " ,when out of melee, only Mousover or both", "Generic");
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp(Shockwave, "Use " + Shockwave, ShockwaveUsage, "Use " + Shockwave + " on AOE, mobcount or as 2nd kick", "Cooldowns", 0);
            CombatRoutine.AddProp(Avatar, "Use " + Avatar, CDUsageWithAOE, "Use " + Avatar + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Ravager, "Use " + Ravager, CDUsageWithAOE, "Use " + Ravager + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("MobCount", "Mobcount to use Cooldowns ", numbRaidList, " Mobcount to use Cooldowns", "Cooldowns", 3);
            CombatRoutine.AddProp("Ragedumping", "Rage dumping", RageDump, "How much Rage before we spend it", "Generic", 5);
            CombatRoutine.AddProp("TrinketLife", "Trinket" + " Life Percent", numbList, "Life percent at which" + "Trinkets" + " will be used, set to 0 to disable", "Defense", 0);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(LastStand, LastStand + " Life Percent", numbList, "Life percent at which" + LastStand + " is used, set to 0 to disable", "Defense", 20);
            CombatRoutine.AddProp(ShieldWall, ShieldWall + " Life Percent", numbList, "Life percent at which" + ShieldWall + " is used, set to 0 to disable", "Defense", 30);
            CombatRoutine.AddProp(VictoryRush, VictoryRush + " Life Percent", numbList, "Life percent at which" + VictoryRush + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(ImpendingVictory, ImpendingVictory + " Life Percent", numbList, "Life percent at which" + ImpendingVictory + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(DemoralizingShout, DemoralizingShout + " Life Percent", numbList, "Life percent at which" + DemoralizingShout + " is used without Booming Voice Talent, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp(IgnorePain, IgnorePain + " Life Percent", numbList, "Life percent at which" + IgnorePain + " is used, set to 0 to disable", "Defense", 90);
            CombatRoutine.AddProp("FirstShieldBlock", "ShieldBlock 1. Charge" + " Life Percent", numbList, "Life percent at which" + ShieldBlock + " is used, set to 0 to disable", "Defense", 80);
            CombatRoutine.AddProp("SecondShieldBlock", "ShieldBlock 2. Charge" + " Life Percent", numbList, "Life percent at which" + ShieldBlock + " is used, set to 0 to disable", "Defense", 50);
            CombatRoutine.AddProp(AoE, "Rallying Cry Party Units ", numbPartyList, " in Party", "Rallying", 3);
            CombatRoutine.AddProp(AoERaid, "Rallying Cry Raid Units ", numbRaidList, "  in Raid", "Rallying", 3);
            CombatRoutine.AddProp(RallyingCry, RallyingCry + "Life Percent", numbList, "Life percent at which" + RallyingCry + " is used, set to 0 to disable", "Rallying", 50);
        }
        public override void Pulse()
        {
            //API.WriteLog("Are we the Target?: "+ API.PlayerIsTargetTarget);
            if (!API.PlayerIsMounted)
            {
                if (API.CanCast(BattleShout) && PlayerLevel >= 39 && API.PlayerBuffTimeRemaining(BattleShout) < 30000)
                {
                    API.CastSpell(BattleShout);
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.PlayerCurrentCastTimeRemaining > 40 || API.PlayerSpellonCursor)
                return;
            for (int i = 0; i < SpellReflectCast.Length; i++)
            {
                if (API.TargetCurrentCastSpellID == SpellReflectCast[i] && API.TargetCurrentCastTimeRemaining < 100 && API.PlayerIsTargetTarget)
                {
                    if (API.CanCast(SpellReflection) && !API.PlayerHasBuff(SpellReflection))
                    {
                        API.CastSpell(SpellReflection);
                        API.WriteLog("Reflect time boys and girls");
                        return;
                    }
                }
            }
            if (isInterrupt && API.CanCast(Pummel) && IsMelee && PlayerLevel >= 7)
            {
                API.CastSpell(Pummel);
                return;
            }
            if (isInterrupt && API.CanCast(StormBolt) && API.TargetRange <= 20 && TalentStormBolt && (API.SpellISOnCooldown(Pummel) || !IsMelee))
            {
                API.CastSpell(StormBolt);
                return;
            }
            if (API.CanCast(Shockwave) && IsMelee && IsShockwave)
            {
                API.CastSpell(Shockwave);
                return;
            }
            if (API.CanCast(RallyingCry) && RallyAoE)
            {
                API.CastSpell(RallyingCry);
                return;
            }
            if (PlayerRaceSettings == "Tauren" && API.CanCast(RacialSpell1) && isInterrupt && !API.PlayerIsMoving && isRacial && IsMelee && API.SpellISOnCooldown(Pummel))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            if (API.CanCast(ShieldBlock) && API.PlayerHealthPercent <= ShieldBlockFirstChargeLifePercent && API.SpellCharges(ShieldBlock) == 2 && PlayerLevel >= 6 && API.PlayerRage >= 30 && !API.PlayerHasBuff(ShieldBlock))
            {
                API.CastSpell(ShieldBlock);
                return;
            }
            if (API.CanCast(ShieldBlock) && API.PlayerHealthPercent <= ShieldBlockSecondChargeLifePercent && API.SpellCharges(ShieldBlock) < 2 && PlayerLevel >= 6 && API.PlayerRage >= 30 && !API.PlayerHasBuff(ShieldBlock))
            {
                API.CastSpell(ShieldBlock);
                return;
            }
            if (API.CanCast(IgnorePain) && (API.PlayerRage >= 40 || API.PlayerRage >= 26 && API.PlayerHasBuff(VengeanceIgnorePain)) && (!API.PlayerHasBuff(IgnorePain) || API.PlayerHasBuff(IgnorePain) && API.PlayerBuffTimeRemaining(IgnorePain) < 200) && API.PlayerHealthPercent <= IgnorePainLifePercent && PlayerLevel >= 17)
            {
                API.CastSpell(IgnorePain);
                return;
            }
            if (API.CanCast(LastStand) && API.PlayerHealthPercent <= LastStandLifePercent && PlayerLevel >= 38)
            {
                API.CastSpell(LastStand);
                return;
            }
            if (API.CanCast(ShieldWall) && API.PlayerHealthPercent <= ShieldWallLifePercent && PlayerLevel >= 23 && !API.PlayerHasBuff(LastStand))
            {
                API.CastSpell(ShieldWall);
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
            if (API.CanCast(DemoralizingShout) && API.PlayerHealthPercent <= DemoralizingShoutLifePercent && IsMelee && PlayerLevel >= 27 && !TalentBoomingVoice)
            {
                API.CastSpell(DemoralizingShout);
                return;
            }
            if (API.CanCast(VictoryRush) && API.PlayerHealthPercent <= VictoryRushLifePercent && API.PlayerHasBuff(Victorious) && PlayerLevel >= 5 && IsMelee)
            {
                API.CastSpell(VictoryRush);
                return;
            }
            if (API.CanCast(ImpendingVictory) && API.PlayerHealthPercent <= ImpendingVictoryLifePercent && TalentImpendingVictory && IsMelee)
            {
                API.CastSpell(ImpendingVictory);
                return;
            }
            if (API.CanCast(BerserkerRage) && API.PlayerIsCC(CCList.FEAR_MECHANIC))
            {
                API.CastSpell(BerserkerRage);
                return;
            }
            rotation();
            return;
        }
        public override void OutOfCombatPulse()
        {
        }
        private void rotation()
        {
            if (API.CanCast(HeroicThrow) && PlayerLevel >= 24 && API.TargetRange >= 8 && API.TargetRange <= 30 && (UseHeroicThrow == "when out of melee" || UseHeroicThrow == "both"))
            {
                API.CastSpell(HeroicThrow);
                return;
            }
            if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && API.MouseoverHealthPercent > 0 && (UseHeroicThrow == "only Mouseover" || UseHeroicThrow == "both"))
            {
                if (API.CanCast(HeroicThrow) && !API.MacroIsIgnored(HeroicThrow + "MO") && PlayerLevel >= 24 && API.MouseoverRange >= 8 && API.MouseoverRange <= 30)
                {
                    API.CastSpell(HeroicThrow + "MO");
                    return;
                }
            }
            if (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && !API.PlayerCanAttackMouseover && API.MouseoverIsUnitIndex != 0 && API.MouseoverIsUnitIndex != 41 && API.MouseoverHealthPercent > 0 && API.MouseoverRange <= 25)
            {
                if (API.CanCast(Intervene) && !API.MacroIsIgnored(Intervene + "MO") && UseIntervene)
                {
                    API.CastSpell(Intervene + "MO");
                    return;
                }
            }
            if (IsMelee)
            {
                //actions+=/blood_fury
                if (PlayerRaceSettings == "Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/berserking
                if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/lights_judgment,if=buff.recklessness.down&debuff.siegebreaker.down
                if (PlayerRaceSettings == "Lightforged" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/fireblood
                if (PlayerRaceSettings == "Dark Iron Dwarf" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/ancestral_call
                if (PlayerRaceSettings == "Mag'har Orc" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions+=/bag_of_tricks,if=buff.recklessness.down&debuff.siegebreaker.down&buff.enrage.up
                if (PlayerRaceSettings == "Vulpera" && API.CanCast(RacialSpell1) && isRacial && IsCooldowns && IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                if (API.PlayerTrinketIsUsable(1) && !API.MacroIsIgnored("Trinket1") && !isExplosive && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
                {
                    API.CastSpell("Trinket1");
                    return;
                }
                if (API.PlayerTrinketIsUsable(2) && !API.MacroIsIgnored("Trinket2") && !isExplosive && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
                {
                    API.CastSpell("Trinket2");
                    return;
                }
                if (API.CanCast(ConquerorsBanner) && PlayerCovenantSettings == "Necrolord" && !isExplosive && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(ConquerorsBanner);
                    return;
                }
                if (API.CanCast(SpearofBastion) && PlayerCovenantSettings == "Kyrian" && !isExplosive && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(SpearofBastion);
                    return;
                }
                if (API.CanCast(AncientAftershock) && PlayerCovenantSettings == "Night Fae" && !isExplosive && !API.PlayerIsMoving && IsCovenant)
                {
                    API.CastSpell(AncientAftershock);
                    return;
                }
                if (API.CanCast(Avatar) && PlayerLevel >= 32 && API.PlayerRage < 70 && IsAvatar)
                {
                    API.CastSpell(Avatar);
                    return;
                }
                if (API.CanCast(DemoralizingShout) && TalentBoomingVoice && !isExplosive && API.PlayerRage < 60)
                {
                    API.CastSpell(DemoralizingShout);
                    return;
                }
                if (API.PlayerUnitInMeleeRangeCount < AOEUnitNumber || !IsAOE)
                {
                    //actions.generic=ravager
                    if (API.CanCast(Ravager) && !isExplosive && TalentRavager && API.PlayerRage < 70 && IsRavagerOn && IsRavager)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    //actions.generic+=/dragon_roar
                    if (API.CanCast(DragonRoar) && !isExplosive && TalentDragonRoar && API.PlayerRage < 80)
                    {
                        API.CastSpell(DragonRoar);
                        return;
                    }
                    //actions.generic+=/shield_slam,if=buff.shield_block.up
                    if (API.CanCast(ShieldSlam) && PlayerLevel >= 3 && API.PlayerHasBuff(ShieldBlock))
                    {
                        API.CastSpell(ShieldSlam);
                        return;
                    }
                    //actions.generic+=/thunder_clap,if=(spell_targets.thunder_clap>1|cooldown.shield_slam.remains)&talent.unstoppable_force.enabled&buff.avatar.up
                    if (API.CanCast(ThunderClap) && PlayerLevel >= 19 && (API.PlayerUnitInMeleeRangeCount > 1 || API.SpellCDDuration(ShieldSlam) < gcd) && TalentUnstopableForce && API.PlayerHasBuff(Avatar))
                    {
                        API.CastSpell(ThunderClap);
                        return;
                    }
                    //actions.generic+=/shield_slam
                    if (API.CanCast(ShieldSlam) && PlayerLevel >= 3)
                    {
                        API.CastSpell(ShieldSlam);
                        return;
                    }
                    //actions.generic+=/execute
                    if (API.CanCast(Execute, true, false) && PlayerCovenantSettings != "Venthyr" && API.PlayerRage >= Ragedumping && (API.TargetHealthPercent < 20 || API.PlayerHasBuff(NoLImitCondemn)) && PlayerLevel >= 10)
                    {
                        API.CastSpell(Execute);
                        return;
                    }
                    if (API.CanCast(Condemn, true, false) && PlayerCovenantSettings == "Venthyr" && API.PlayerRage >= Ragedumping && (API.TargetHealthPercent < 20 || API.PlayerHasBuff(NoLImitCondemn) || API.TargetHealthPercent > 80))
                    {
                        API.CastSpell(Condemn);
                        return;
                    }
                    //actions.generic+=/revenge,if=rage>80&target.health.pct>20|buff.revenge.up
                    if (API.CanCast(Revenge) && API.PlayerRage >= Ragedumping && PlayerLevel >= 12 && (API.TargetHealthPercent > 20 || API.PlayerHasBuff(Revenge)))
                    {
                        API.CastSpell(Revenge);
                        return;
                    }
                    //actions.generic+=/thunder_clap
                    if (API.CanCast(ThunderClap) && PlayerLevel >= 19)
                    {
                        API.CastSpell(ThunderClap);
                        return;
                    }
                    //actions.generic+=/revenge
                    if (API.CanCast(Revenge) && API.PlayerRage >= Ragedumping && PlayerLevel >= 12)
                    {
                        API.CastSpell(Revenge);
                        return;
                    }
                    //actions.generic+=/devastate
                    if (API.CanCast(Devastate) && !TalentDevastator && PlayerLevel >= 14)
                    {
                        API.CastSpell(Devastate);
                        return;
                    }
                }
                if (API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && IsAOE)
                {
                    //actions.aoe=ravager
                    if (API.CanCast(Ravager) && !isExplosive && TalentRavager && API.PlayerRage < 70 && IsRavagerOn && IsRavager)
                    {
                        API.CastSpell(Ravager);
                        return;
                    }
                    //actions.aoe+=/dragon_roar
                    if (API.CanCast(DragonRoar) && !isExplosive && TalentDragonRoar && API.PlayerRage < 80)
                    {
                        API.CastSpell(DragonRoar);
                        return;
                    }
                    //actions.aoe+=/thunder_clap
                    if (API.CanCast(ThunderClap) && PlayerLevel >= 19)
                    {
                        API.CastSpell(ThunderClap);
                        return;
                    }
                    //actions.aoe+=/revenge
                    if (API.CanCast(Revenge) && API.PlayerRage >= Ragedumping && PlayerLevel >= 14)
                    {
                        API.CastSpell(Revenge);
                        return;
                    }
                    //actions.aoe+=/shield_slam
                    if (API.CanCast(ShieldSlam) && PlayerLevel >= 3)
                    {
                        API.CastSpell(ShieldSlam);
                        return;
                    }
                }
            }
        }
    }
}