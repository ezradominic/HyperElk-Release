
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
namespace HyperElk.Core
{
    public class DemonologyWarlock : CombatRoutine
    {
        //Spells,Buffs,Debuffs
        private string ShadowBolt = "Shadow Bolt";
        private string CovenantAbility = "Covenant Ability";
        private string DrainLife = "Drain Life";
        private string HealthFunnel = "Health Funnel";
        private string SummonImp = "Summon Imp";
        private string SummonVoidwalker = "Summon Voidwalker";
        private string SummonSuccubus = "Summon Succubus";
        private string SummonFelhunter = "Summon Felhunter";
        private string HandofGuldan = "Hand of Gul'dan";
        private string CallDreadstalkers = "Call Dreadstalkers";
        private string Implosion = "Implosion";
        private string NetherPortal = "Nether Portal";
        private string SummonDemonicTyrant = "Summon Demonic Tyrant";
        private string Misdirection = "Misdirection";
        private string SummonVilefiend = "Summon Vilefiend";
        private string Demonbolt = "Demonbolt";
        private string DemonicCore = "Demonic Core";
        private string GrimoireFelguard = "Grimoire:  Felguard";
        private string DemonicPower = "Demonic Power";
        private string PowerSiphon = "Power Siphon";
        private string SoulStrike = "Soul Strike";
        private string Doom = "Doom";
        private string DemonicStrength = "Demonic Strength";
        private string ImpendingCatastrophe = "Impending Catastrophe";
        private string ScouringTithe = "Scouring Tithe";
        private string SacrificedSouls = "Sacrificed Souls";
        private string SoulRot = "Soul Rot";
        private string DecimatingBolt = "Decimating Bolt";
        private string BilescourgeBombers = "Bilescourge Bombers";
        private string SummonFelguard = "Summon Felguard";
        private string FelDomination = "Fel Domination";
        private string SpellLock = "Spell Lock";
        private string Healthstone = "Healthstone";
        private string Quake = "Quake";
        private string Stopcast = "Stopcast Macro";



        //Talents
        private bool TalentPowerSiphon => API.PlayerIsTalentSelected(2, 2);
        private bool TalentSoulStrike => API.PlayerIsTalentSelected(4, 2);
        private bool TalentSummonVilefiend => API.PlayerIsTalentSelected(4, 3);
        private bool TalentSacrificedSouls => API.PlayerIsTalentSelected(7, 1);
        private bool TalentDoom => API.PlayerIsTalentSelected(2, 2);
        private bool TalentDemonicStrength => API.PlayerIsTalentSelected(1, 3);
        private bool TalentBilescourgeBombers => API.PlayerIsTalentSelected(1, 2);
        private bool TalentNetherPortal => API.PlayerIsTalentSelected(7, 3);
        private bool TalentGrimoireFelguard => API.PlayerIsTalentSelected(6, 3);
        private bool TalentDemonicConsumption => API.PlayerIsTalentSelected(7, 2);
        //Misc
        private static readonly Stopwatch ImpWatch = new Stopwatch();

        private bool IsRange => API.TargetRange < 40;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
        //        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private int SoulShards => API.PlayerCurrentSoulShards;


        //CBProperties
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthStoneTempFixProc => numbList[CombatRoutine.GetPropertyInt("HealthStoneTempFix")];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        string[] MisdirectionList = new string[] { "None", "Felguard", "Imp", "Voidwalker", "Succubus", "Felhunter", "Darkglare" };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseCovenantAbility => CovenantAbilityList[CombatRoutine.GetPropertyInt(CovenantAbility)];
        string[] CovenantAbilityList = new string[] { "None", "always", "with Cooldowns", "AOE" };
        private static readonly Stopwatch TyrantWatch = new Stopwatch();
        private static readonly Stopwatch VilefiendWatch = new Stopwatch();
        private static readonly Stopwatch GrimoireFelguardWatch = new Stopwatch();
        private bool Quaking => ((API.PlayerCurrentCastTimeRemaining >= 200 || API.PlayerIsChanneling) && API.PlayerDebuffRemainingTime(Quake) < 200) && API.PlayerHasDebuff(Quake);

        public override void Initialize()
        {
            CombatRoutine.Name = "Demonology Warlock @Mufflon12";
            API.WriteLog("Welcome to Demonology Warlock rotation @ Mufflon12");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(HealthFunnel, "Health Funnel", numbList, "Life percent at which " + HealthFunnel + " is used, set to 0 to disable", "PETS", 0);
            CombatRoutine.AddProp(Misdirection, "Wich Pet", MisdirectionList, "Chose your Pet", "PETS", 0);
            CombatRoutine.AddProp("Covenant Ability", "Use " + "Covenant Ability", CovenantAbilityList, "How to use Covenant Spell", "Covenant", 0);

            //Spells
            CombatRoutine.AddSpell("Shadow Bolt", 686, "D1");
            CombatRoutine.AddSpell("Hand of Gul'dan", 105174, "D2");
            CombatRoutine.AddSpell("Call Dreadstalkers", 104316, "D3");
            CombatRoutine.AddSpell("Implosion", 196277, "D4");
            CombatRoutine.AddSpell(SummonDemonicTyrant, 265187, "D9");
            CombatRoutine.AddSpell(BilescourgeBombers, 267211, "D7");
            CombatRoutine.AddSpell(SummonVilefiend, 264119, "D7");
            CombatRoutine.AddSpell(SoulStrike, 264057, "D8");
            CombatRoutine.AddSpell(Demonbolt, 264178, "D6");
            CombatRoutine.AddSpell(GrimoireFelguard, 111898, "D0");
            CombatRoutine.AddSpell(Doom, 603);
            CombatRoutine.AddSpell(DemonicStrength, 267171);
            CombatRoutine.AddSpell(PowerSiphon, 264130);

            CombatRoutine.AddSpell("Drain Life", 234153, "NumPad1");
            CombatRoutine.AddSpell("Health Funnel", 755, "NumPad2");
            CombatRoutine.AddSpell(ScouringTithe, 312321, "F1");
            CombatRoutine.AddSpell(SoulRot, 325640, "F1");
            CombatRoutine.AddSpell(ImpendingCatastrophe, 321792, "F1");
            CombatRoutine.AddSpell(DecimatingBolt, 325289, "F1");

            CombatRoutine.AddSpell(FelDomination, 333889);
            CombatRoutine.AddSpell(SummonFelguard, 30146, "NumPad5");
            CombatRoutine.AddSpell("Summon Felhunter", 691, "NumPad6");
            CombatRoutine.AddSpell("Summon Succubus", 712, "NumPad7");
            CombatRoutine.AddSpell("Summon Voidwalker", 697, "NumPad8");
            CombatRoutine.AddSpell("Summon Imp", 688, "NumPad9");
            CombatRoutine.AddSpell(SpellLock, 19647);
            CombatRoutine.AddSpell(NetherPortal, 267217);


            //Buffs
            CombatRoutine.AddBuff(NetherPortal);
            CombatRoutine.AddBuff(DemonicCore, 264173);
            CombatRoutine.AddBuff(DemonicPower, 265273);
            CombatRoutine.AddBuff(FelDomination, 333889);
            CombatRoutine.AddBuff(Quake, 240447);

            //Debuffs
            CombatRoutine.AddDebuff(Doom, 603);
            CombatRoutine.AddDebuff(Quake, 240447);
            CombatRoutine.AddMacro(Stopcast, "F10");
        }


        public override void Pulse()
        {
            if (GrimoireFelguardWatch.IsRunning && GrimoireFelguardWatch.ElapsedMilliseconds >= 17000)
            {
                GrimoireFelguardWatch.Reset();
            }
            if (VilefiendWatch.IsRunning && VilefiendWatch.ElapsedMilliseconds >= 15000)
            {
                VilefiendWatch.Reset();
            }
            if (TyrantWatch.IsRunning && TyrantWatch.ElapsedMilliseconds >= 15000)
            {
                TyrantWatch.Reset();
            }
            if (API.PlayerCurrentCastTimeRemaining > 40 && !API.MacroIsIgnored(Stopcast) && Quaking)
            {
                API.CastSpell(Stopcast);
                return;
            }
        }

        public override void CombatPulse()
        {
            //Summon Imp
            if (API.PlayerHasBuff(FelDomination) && API.CanCast(SummonImp) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (API.PlayerHasBuff(FelDomination) && API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (API.PlayerHasBuff(FelDomination) && API.CanCast(SummonSuccubus) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (API.PlayerHasBuff(FelDomination) && API.CanCast(SummonFelhunter) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && PlayerLevel >= 23)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonFelhunter);
                return;
            }
            if (!API.PlayerHasPet && API.CanCast(FelDomination) && (isMisdirection == "Felhunter" || isMisdirection == "Succubus" || isMisdirection == "Voidwalker" || isMisdirection == "Imp"))
            {
                API.CastSpell(FelDomination);
                return;
            }
            if (isInterrupt && API.CanCast(SpellLock) && API.PlayerHasPet && isMisdirection == "Felhunter")
            {
                API.CastSpell(SpellLock);
                return;
            }
            if (API.PlayerHealthPercent <= DrainLifePercentProc && API.CanCast(DrainLife) && PlayerLevel >= 9)
            {
                API.CastSpell(DrainLife);
                return;
            }
            if (API.PlayerHasPet && API.PetHealthPercent > 0 && API.PetHealthPercent <= HealthFunnelPercentProc && API.CanCast(HealthFunnel))
            {
                API.CastSpell(HealthFunnel);
                return;
            }
            rotation();
            return;

        }
        private void rotation()
        {
            if (NotMoving && IsRange && NotChanneling && !API.PlayerIsCasting(true))
            {
                if (!TyrantWatch.IsRunning && ImpWatch.IsRunning && ImpWatch.ElapsedMilliseconds >= 8000 || TyrantWatch.IsRunning && ImpWatch.IsRunning && ImpWatch.ElapsedMilliseconds >= 25000)
                {
                    API.CastSpell(Implosion);
                    ImpWatch.Reset();
                    return;
                }
                //actions=call_action_list,name=off_gcd
                //actions+=/run_action_list,name=tyrant_prep,if=cooldown.summon_demonic_tyrant.remains<4&!variable.tyrant_ready
                if (IsCooldowns && API.SpellCDDuration(SummonDemonicTyrant) < 400)
                {
                    //actions.tyrant_prep=doom,line_cd=30
                    if (API.CanCast(Doom) && TalentDoom)
                    {
                        API.CastSpell(Doom);
                        return;
                    }
                    //actions.tyrant_prep+=/nether_portal
                    if (API.CanCast(NetherPortal) && TalentNetherPortal)
                    {
                        API.CastSpell(NetherPortal);
                        return;
                    }
                    //actions.tyrant_prep+=/grimoire_felguard
                    if (API.CanCast(GrimoireFelguard) && TalentGrimoireFelguard && SoulShards >= 1)
                    {
                        API.CastSpell(GrimoireFelguard);
                        GrimoireFelguardWatch.Start();
                        return;
                    }
                    //actions.tyrant_prep+=/summon_vilefiend
                    if (API.CanCast(SummonVilefiend) && TalentSummonVilefiend && SoulShards >= 1)
                    {
                        API.CastSpell(SummonVilefiend);
                        return;
                    }
                    //actions.tyrant_prep+=/call_dreadstalkers
                    if (API.CanCast(CallDreadstalkers) && SoulShards >= 2)
                    {
                        API.CastSpell(CallDreadstalkers);
                        return;
                    }
                    //actions.tyrant_prep+=/demonbolt,if=buff.demonic_core.up&soul_shard<4&(talent.demonic_consumption.enabled|buff.nether_portal.down)
                    if (API.CanCast(Demonbolt) && API.PlayerHasBuff(DemonicCore) && SoulShards < 4 && (TalentDemonicConsumption || !API.PlayerHasBuff(NetherPortal)))
                    {
                        API.CastSpell(Demonbolt);
                        return;
                    }
                    //actions.tyrant_prep+=/soul_strike,if=soul_shard<5-4*buff.nether_portal.up
                    //actions.tyrant_prep+=/shadow_bolt,if=soul_shard<5-4*buff.nether_portal.up
                    //actions.tyrant_prep+=/variable,name=tyrant_ready,value=1
                    //actions.tyrant_prep+=/hand_of_guldan
                    if (API.CanCast(HandofGuldan) && SoulShards >= 1)
                    {
                        API.CastSpell(HandofGuldan);
                        return;
                    }
                }
                //actions+=/run_action_list,name=summon_tyrant,if=variable.tyrant_ready
                if (IsCooldowns && API.CanCast(SummonDemonicTyrant))
                {
                    //actions.summon_tyrant=hand_of_guldan,if=soul_shard=5,line_cd=20
                    if (API.CanCast(HandofGuldan) && SoulShards == 5)
                    {
                        API.CastSpell(HandofGuldan);
                        return;
                    }
                    //actions.summon_tyrant+=/demonbolt,if=buff.demonic_core.up&(talent.demonic_consumption.enabled|buff.nether_portal.down),line_cd=20
                    if (API.CanCast(Demonbolt) && API.PlayerHasBuff(DemonicCore) && (TalentDemonicConsumption || !API.PlayerHasBuff(NetherPortal)))
                    {
                        API.CastSpell(Demonbolt);
                        return;
                    }
                    //actions.summon_tyrant+=/shadow_bolt,if=buff.wild_imps.stack+incoming_imps<4&(talent.demonic_consumption.enabled|buff.nether_portal.down),line_cd=20
//                    if (API.CanCast(ShadowBolt) && API.PlayerImpCount < 4 && (TalentDemonicConsumption || !API.PlayerHasBuff(NetherPortal)))
//                    {
//                        API.CastSpell(ShadowBolt);
//                        return;
//                    }
                    //actions.summon_tyrant+=/call_dreadstalkers
                    if (API.CanCast(CallDreadstalkers) && SoulShards >= 2)
                    {
                        API.CastSpell(CallDreadstalkers);
                        return;
                    }
                    //actions.summon_tyrant+=/hand_of_guldan
                    if (API.CanCast(HandofGuldan) && SoulShards >= 1)
                    {
                        API.CastSpell(HandofGuldan);
                        return;
                    }
                    //actions.summon_tyrant+=/demonbolt,if=buff.demonic_core.up&buff.nether_portal.up&((buff.vilefiend.remains>5|!talent.summon_vilefiend.enabled)&(buff.grimoire_felguard.remains>5|buff.grimoire_felguard.down))
                    if (API.CanCast(Demonbolt) && API.PlayerHasBuff(NetherPortal) && (VilefiendWatch.ElapsedMilliseconds <= 10000 || !TalentSummonVilefiend) && (GrimoireFelguardWatch.ElapsedMilliseconds <= 12000 || !GrimoireFelguardWatch.IsRunning))
                    {
                        API.CastSpell(Demonbolt);
                        return;
                    }
                    //actions.summon_tyrant+=/soul_strike,if=buff.nether_portal.up&((buff.vilefiend.remains>5|!talent.summon_vilefiend.enabled)&(buff.grimoire_felguard.remains>5|buff.grimoire_felguard.down))
                    if (API.CanCast(SoulStrike) && TalentSoulStrike && API.PlayerHasBuff(NetherPortal) && (VilefiendWatch.ElapsedMilliseconds <= 10000 || !TalentSummonVilefiend) && (GrimoireFelguardWatch.ElapsedMilliseconds <= 12000 || !GrimoireFelguardWatch.IsRunning))
                    {
                        API.CastSpell(SoulStrike);
                        return;
                    }
                    //actions.summon_tyrant+=/shadow_bolt,if=buff.nether_portal.up&((buff.vilefiend.remains>5|!talent.summon_vilefiend.enabled)&(buff.grimoire_felguard.remains>5|buff.grimoire_felguard.down))
                    if (API.CanCast(ShadowBolt) && API.PlayerHasBuff(NetherPortal) && (VilefiendWatch.ElapsedMilliseconds <= 10000 || !TalentSummonVilefiend) && (GrimoireFelguardWatch.ElapsedMilliseconds <= 12000 || !GrimoireFelguardWatch.IsRunning))
                    {
                        API.CastSpell(ShadowBolt);
                        return;
                    }
                    //actions.summon_tyrant+=/variable,name=tyrant_ready,value=!cooldown.summon_demonic_tyrant.ready
                    //actions.summon_tyrant+=/summon_demonic_tyrant
                    if (API.CanCast(SummonDemonicTyrant))
                    {
                        API.CastSpell(SummonDemonicTyrant);
                        TyrantWatch.Start();
                        return;
                    }
                    //actions.summon_tyrant+=/shadow_bolt
                    if (API.CanCast(ShadowBolt))
                    {
                        API.CastSpell(ShadowBolt);
                        return;
                    }
                }
                //actions+=/summon_vilefiend,if=cooldown.summon_demonic_tyrant.remains>40|time_to_die<cooldown.summon_demonic_tyrant.remains+25
                if (API.CanCast(SummonVilefiend) && TalentSummonVilefiend && (API.SpellCDDuration(SummonDemonicTyrant) > 4000 || API.TargetTimeToDie < API.SpellCDDuration(SummonDemonicTyrant) + 2500))
                {
                    API.CastSpell(SummonVilefiend);
                    VilefiendWatch.Start();
                    return;
                }
                //actions+=/call_dreadstalkers
                if (API.CanCast(CallDreadstalkers) && SoulShards >= 2)
                {
                    API.CastSpell(CallDreadstalkers);
                    return;
                }
                //actions+=/doom,if=refreshable
                if (API.CanCast(Doom) && TalentDoom && !API.TargetHasDebuff(Doom))
                {
                    API.CastSpell(Doom);
                    return;
                }
                //actions+=/demonic_strength
                if (API.CanCast(DemonicStrength) && TalentDemonicStrength)
                {
                    API.CastSpell(DemonicStrength);
                    return;
                }
                //actions+=/bilescourge_bombers
                if (API.CanCast(BilescourgeBombers) && TalentBilescourgeBombers && SoulShards >= 2)
                {
                    API.CastSpell(BilescourgeBombers);
                    return;
                }
                //actions+=/implosion,if=active_enemies>1&!talent.sacrificed_souls.enabled&buff.wild_imps.stack>=8&buff.tyrant.down&cooldown.summon_demonic_tyrant.remains>5
                if (API.CanCast(Implosion) && API.TargetUnitInRangeCount > 1 && !TalentSacrificedSouls && API.PlayerImpCount >= 8 && !TyrantWatch.IsRunning && API.SpellCDDuration(SummonDemonicTyrant) > 500)
                {
                    API.CastSpell(Implosion);
                    return;
                }
                //actions+=/implosion,if=active_enemies>2&buff.wild_imps.stack>=8&buff.tyrant.down
 //               if (API.CanCast(Implosion) && API.PlayerImpCount >= 8)
 //               {
 //                   API.CastSpell(Implosion);
 //                   return;
 //               }
                //actions+=/hand_of_guldan,if=soul_shard=5|buff.nether_portal.up
                if (API.CanCast(HandofGuldan) && !API.PlayerIsCasting(false) && (SoulShards == 5 || API.PlayerHasBuff(NetherPortal)))
                {
                    API.CastSpell(HandofGuldan);
                    return;
                }
                //actions+=/hand_of_guldan,if=soul_shard>=3&cooldown.summon_demonic_tyrant.remains>20&(cooldown.summon_vilefiend.remains>5|!talent.summon_vilefiend.enabled)&cooldown.call_dreadstalkers.remains>2
                if (API.CanCast(HandofGuldan) && SoulShards >= 3 && !API.PlayerIsCasting(true) && API.SpellCDDuration(SummonDemonicTyrant) > 2000 && (API.SpellCDDuration(SummonVilefiend) > 500 || !TalentSummonVilefiend) && API.SpellCDDuration(CallDreadstalkers) > 200)
                {
                    API.CastSpell(HandofGuldan);
                    return;
                }
                //actions+=/call_action_list,name=covenant,if=(covenant.necrolord|covenant.night_fae)&!talent.nether_portal.enabled
                if (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Night Fae" && !TalentNetherPortal)
                {
                    //actions.covenant=impending_catastrophe,if=!talent.sacrificed_souls.enabled|active_enemies>1
                    if (PlayerCovenantSettings == "Kyrian" && API.CanCast(ScouringTithe) && !TalentSacrificedSouls && API.TargetUnitInRangeCount > 1)
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe,if=talent.sacrificed_souls.enabled&active_enemies=1
                    if (PlayerCovenantSettings == "Kyrian" && API.CanCast(ScouringTithe) && TalentSacrificedSouls && API.TargetUnitInRangeCount == 1)
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe,if=!talent.sacrificed_souls.enabled&active_enemies<4
                    if (PlayerCovenantSettings == "Kyrian" && API.CanCast(ScouringTithe) && !TalentSacrificedSouls && API.TargetUnitInRangeCount < 4)
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                    //actions.covenant+=/soul_rot
                    if (PlayerCovenantSettings == "Night Fae" && API.CanCast(SoulRot))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt
                    if (PlayerCovenantSettings == "Necrolord" && API.CanCast(DecimatingBolt))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                }
                //actions+=/demonbolt,if=buff.demonic_core.react&soul_shard<4
                if (API.CanCast(Demonbolt) && API.PlayerHasBuff(DemonicCore) && SoulShards < 4)
                {
                    API.CastSpell(Demonbolt);
                    return;
                }
                //actions+=/grimoire_felguard,if=cooldown.summon_demonic_tyrant.remains+cooldown.summon_demonic_tyrant.duration>time_to_die|time_to_die<cooldown.summon_demonic_tyrant.remains+15
                if (API.CanCast(GrimoireFelguard) && TalentGrimoireFelguard && (API.SpellCDDuration(SummonDemonicTyrant) + API.SpellCDDuration(SummonDemonicTyrant) > API.TargetTimeToDie || API.TargetTimeToDie < API.SpellCDDuration(SummonDemonicTyrant) + 1500))
                {
                    API.CastSpell(GrimoireFelguard);
                    return;
                }
                //actions+=/use_items
                //actions+=/power_siphon,if=buff.wild_imps.stack>1&buff.demonic_core.stack<3
                if (API.CanCast(PowerSiphon) && TalentPowerSiphon && API.PlayerHasBuff(DemonicCore)) // API.PlayerImpCount > 1)
                {
                    API.CastSpell(PowerSiphon);
                    return;
                }
                //actions+=/soul_strike
                if (API.CanCast(SoulStrike) && TalentSoulStrike)
                {
                    API.CastSpell(SoulStrike);
                    return;
                }
                //actions+=/call_action_list,name=covenant
                if (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "Venthyr")
                {
                    //actions.covenant=impending_catastrophe,if=!talent.sacrificed_souls.enabled|active_enemies>1
                    if (PlayerCovenantSettings == "Kyrian" && API.CanCast(ScouringTithe) && !TalentSacrificedSouls && API.TargetUnitInRangeCount > 1)
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe,if=talent.sacrificed_souls.enabled&active_enemies=1
                    if (PlayerCovenantSettings == "Kyrian" && API.CanCast(ScouringTithe) && TalentSacrificedSouls && API.TargetUnitInRangeCount == 1)
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                    //actions.covenant+=/scouring_tithe,if=!talent.sacrificed_souls.enabled&active_enemies<4
                    if (PlayerCovenantSettings == "Kyrian" && API.CanCast(ScouringTithe) && !TalentSacrificedSouls && API.TargetUnitInRangeCount < 4)
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                    //actions.covenant+=/soul_rot
                    if (PlayerCovenantSettings == "Night Fae" && API.CanCast(SoulRot))
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions.covenant+=/decimating_bolt
                    if (PlayerCovenantSettings == "Necrolord" && API.CanCast(DecimatingBolt))
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                }
                //actions+=/shadow_bolt
                if (API.CanCast(ShadowBolt) && API.PlayerCurrentSoulShards < 5)
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
            }
        }

        private void covenant()
        {

        }
        public override void OutOfCombatPulse()
        {
            //Summon Imp
            if (API.CanCast(SummonImp) && !API.PlayerHasPet && API.PlayerCurrentCastTimeRemaining > 40 && (isMisdirection == "Imp") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 3)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && API.PlayerCurrentCastTimeRemaining > 40 && (isMisdirection == "Voidwalker") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 10)
            {
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (API.CanCast(SummonSuccubus) && !API.PlayerHasPet && API.PlayerCurrentCastTimeRemaining > 40 && (isMisdirection == "Succubus") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 19)
            {
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (API.CanCast(SummonFelhunter) && !API.PlayerHasPet && API.PlayerCurrentCastTimeRemaining > 40 && (isMisdirection == "Felhunter") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 23)
            {
                API.CastSpell(SummonFelhunter);
                return;
            }
            //Summon Felguard
            if (API.CanCast(SummonFelguard) && !API.PlayerHasPet && API.PlayerCurrentCastTimeRemaining > 40 && (isMisdirection == "Felguard") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 23)
            {
                API.CastSpell(SummonFelguard);
                return;
            }
        }
    }
}