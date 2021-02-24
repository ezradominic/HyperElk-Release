using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;



namespace HyperElk.Core
{
    public class DestructionWarlock : CombatRoutine
    {
        //Spells,Buffs,Debuffs
        private string SummonImp = "Summon Imp";
        private string SummonFelhunter = "Summon Felhunter";
        private string SummonSuccubus = "Summon Succubus";
        private string SummonVoidwalker = "Summon Voidwalker";
        private string DrainLife = "Drain Life";
        private string HealthFunnel = "Health Funnel";
        private string Incinerate = "Incinerate";
        private string Immolate = "Immolate";
        private string ChaosBolt = "Chaos Bolt";
        private string Conflagrate = "Conflagrate";
        private string ShadowBurn = "Shadowburn";
        private string Cataclysm = "Cataclysm";
        private string Havoc = "Havoc";
        private string RainOfFire = "Rain of Fire";
        private string DarkPact = "Dark Pact";
        private string MortalCoil = "Mortal Coil";
        private string HowlofTerror = "Howl Of Terror";
        private string GrimoireOfSacrifice = "Grimoire Of Sacrifice";
        private string SummonInfernal = "Summon Infernal";
        private string SoulFire = "Soul Fire";
        private string ChannelDemonFire = "Channel DemonFire";
        private string ScouringTithe = "Scouring Tithe";
        private string Eradication = "Eradication";
        private string ChrashingChaos = "Chrashing Chaos";
        private string DecimatingBolt = "Decimating Bolt";
        private string Misdirection = "Misdirection";
        private string ImpendingCatastrophe = "Impending Catastrophe";
        private string SoulRot = "Soul Rot";
        private string Backdraft = "Backdraft";
        private string CovenantAbility = "Covenant Ability";
        private string darkSoulInstability = "Dark Soul:  Instability";
        private string RoaringBlaze = "Roaring Blaze";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string FelDomination = "Fel Domination";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        private string SpellLock = "Spell Lock";

        //Talents
        private bool TalentFlashover => API.PlayerIsTalentSelected(1, 1);

        private bool TalentSoulFire => API.PlayerIsTalentSelected(1, 3);
        private bool TalentCataclysm => API.PlayerIsTalentSelected(4, 3);
        private bool TalentDarkPact => API.PlayerIsTalentSelected(3, 3);
        private bool TalentGrimoireOfSacrifice => API.PlayerIsTalentSelected(6, 3);
        private bool TalentChannelDemonFire => API.PlayerIsTalentSelected(7, 2);
        private bool TalentDarkSoulInstability => API.PlayerIsTalentSelected(7, 3);
        private bool TalentEradication => API.PlayerIsTalentSelected(1, 2);
        private bool TalentShadowBurn => API.PlayerIsTalentSelected(2, 3);
        private bool TalentInternalCombustion => API.PlayerIsTalentSelected(2, 2);
        private bool TalentRoaringBlaze => API.PlayerIsTalentSelected(6, 1);
        private bool TalentFireandBrimstone => API.PlayerIsTalentSelected(4, 2);
        private bool TalentInferno => API.PlayerIsTalentSelected(4, 1);
        private bool SwitchTarget => (bool)CombatRoutine.GetProperty("SwitchTarget");


        //Misc
        private static readonly Stopwatch InfernalWatch = new Stopwatch();
        private static readonly Stopwatch HavocWatch = new Stopwatch();
        private static readonly Stopwatch HealthFunnelWatch = new Stopwatch();
        float CBTime => 3 / (1f + API.PlayerGetHaste / 1);
        float SFTime => 4 / (1f + API.PlayerGetHaste / 1);
        float DBTime => 2500 / 1000 / (1f + API.PlayerGetHaste / 1);
        float SCTime => 2 / (1f + API.PlayerGetHaste / 1);
        float IncinerateTime => 2 / (1f + API.PlayerGetHaste / 1);

        float HavocTime => 10 - (HavocWatch.ElapsedMilliseconds / 1000);

        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];

        string[] MisdirectionList = new string[] { "Imp", "Voidwalker", "Succubus", "Felhunter", "Infernal" };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private int DarkPactPercentProc => numbList[CombatRoutine.GetPropertyInt(DarkPact)];
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        private bool IsRange => API.TargetRange < 39;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
        private bool NotChanneling => !API.PlayerIsChanneling;
        bool LastCastImmolate => API.LastSpellCastInGame == Immolate || API.LastSpellCastInGame == Immolate || API.PlayerCurrentCastSpellID == 348;
        bool LastCastConflagrate => API.PlayerLastSpell == Conflagrate || API.LastSpellCastInGame == Conflagrate || API.PlayerCurrentCastSpellID == 17962;
        bool LastCastSummonVoidwalker => API.PlayerLastSpell == SummonVoidwalker || API.LastSpellCastInGame == SummonVoidwalker;
        bool LastCastSummonImp => API.PlayerLastSpell == SummonImp || API.LastSpellCastInGame == SummonImp;
        bool LastCastSummonSuccubus => API.PlayerLastSpell == SummonSuccubus || API.LastSpellCastInGame == SummonSuccubus;
        bool LastCastSummonFelhunter => API.PlayerLastSpell == SummonFelhunter || API.LastSpellCastInGame == SummonFelhunter;
        bool LastCastShadowBurn => API.PlayerLastSpell == ShadowBurn || API.LastSpellCastInGame == ShadowBurn;
        private string UseCovenantAbility => CovenantAbilityList[CombatRoutine.GetPropertyInt(CovenantAbility)];
        string[] CovenantAbilityList = new string[] { "always", "Cooldowns", "AOE" };
        private string UseTrinket1 => TrinketList1[CombatRoutine.GetPropertyInt(trinket1)];
        string[] TrinketList1 = new string[] { "always", "Cooldowns", "never" };

        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "never" };

        public override void Initialize()
        {
            CombatRoutine.Name = "Destruction Warlock @Mufflon12";
            API.WriteLog("Welcome to Destruction Warlock rotation @ Mufflon12");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("Use /cast [@cursor] Rain of Fire");
            API.WriteLog("Use /cast [@cursor] Cataclysm");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            //Options
            CombatRoutine.AddProp(Misdirection, "Wich Pet", MisdirectionList, "Chose your Pet", "PETS", 0);
            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 50);
            CombatRoutine.AddProp(HealthFunnel, "Health Funnel", numbList, "Life percent at which " + HealthFunnel + " is used, set to 0 to disable", "PETS", 0);
            CombatRoutine.AddProp(DarkPact, "Dark Pact", numbList, "Life percent at which " + DarkPact + " is used, set to 0 to disable", "Healing", 20);
            CombatRoutine.AddProp("Covenant Ability", "Use " + "Covenant Ability", CovenantAbilityList, "How to use Weapons of Order", "Covenant", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("SwitchTarget", "Switch Target on Havoc", true, "Switch Target if Havoc is Activ", "Generic");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);

            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", TrinketList1, "When should trinket1 be used", "Trinket");
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", TrinketList2, "When should trinket1 be used", "Trinket");


            //Spells
            CombatRoutine.AddSpell("Switch Target", "Tab");

            CombatRoutine.AddSpell(Immolate, 348,"D1");
            CombatRoutine.AddSpell(Incinerate, 29722,"D2");
            CombatRoutine.AddSpell(Conflagrate, 17962,"D3");
            CombatRoutine.AddSpell(ChaosBolt, 116858,"D4");
            CombatRoutine.AddSpell(ShadowBurn, 17877,"D5");
            CombatRoutine.AddSpell(Cataclysm, 152108,"D6");
            CombatRoutine.AddSpell(Havoc, 80240,"D7");
            CombatRoutine.AddSpell(RainOfFire, 5740,"D8");
            CombatRoutine.AddSpell(SoulFire, 6353,"D9");
            CombatRoutine.AddSpell(ChannelDemonFire, 196447,"D0");
            CombatRoutine.AddSpell(darkSoulInstability, 113858,"OemOpenBrackets");

            CombatRoutine.AddSpell(DrainLife, 234153,"NumPad1");
            CombatRoutine.AddSpell(HealthFunnel, 755,"NumPad2");
            CombatRoutine.AddSpell(SummonInfernal, 1122,"NumPad5");
            CombatRoutine.AddSpell(SummonFelhunter, 691,"NumPad6");
            CombatRoutine.AddSpell(SummonSuccubus, 712,"NumPad7");
            CombatRoutine.AddSpell(SummonVoidwalker, 697,"NumPad8");
            CombatRoutine.AddSpell(SummonImp, 688,"NumPad9");
            CombatRoutine.AddSpell(SpellLock, 19647);

            CombatRoutine.AddSpell(ScouringTithe, 312321, "F1");
            CombatRoutine.AddSpell(SoulRot, 325640, "F1");
            CombatRoutine.AddSpell(ImpendingCatastrophe, 321792, "F1");
            CombatRoutine.AddSpell(DecimatingBolt, 325289, "F1");
            CombatRoutine.AddSpell(FelDomination, 333889);

            //Buffs
            CombatRoutine.AddBuff("Grimoire Of Sacrifice", 108503);
            CombatRoutine.AddBuff(ChrashingChaos, 277705);
            CombatRoutine.AddBuff(Backdraft, 196406);
            CombatRoutine.AddBuff(darkSoulInstability, 113858);
            CombatRoutine.AddBuff(FelDomination, 333889);

            //Debuffs
            CombatRoutine.AddDebuff(Immolate, 157736);
            CombatRoutine.AddDebuff(Havoc, 80240);
            CombatRoutine.AddDebuff(Eradication, 196412);
            CombatRoutine.AddDebuff(RoaringBlaze, 205184);

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);


            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);

        }

        public override void Pulse()
        {
            //Summon Imp
            if (API.PlayerHasBuff(FelDomination) && API.PlayerIsInCombat && !TalentGrimoireOfSacrifice && API.CanCast(SummonImp) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (API.PlayerHasBuff(FelDomination) && API.PlayerIsInCombat && !TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (API.PlayerHasBuff(FelDomination) && API.PlayerIsInCombat && !TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (API.PlayerHasBuff(FelDomination) && API.PlayerIsInCombat && !TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && PlayerLevel >= 23)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonFelhunter);
                return;
            }
            if (!API.PlayerHasPet && API.PlayerIsInCombat && API.CanCast(FelDomination))
            {
                API.CastSpell(FelDomination);
                return;
            }
        }

        public override void CombatPulse()
        {
            if (isInterrupt && API.CanCast(SpellLock) && API.PlayerHasPet && isMisdirection == "Felhunter")
            {
                API.CastSpell(SpellLock);
                return;
            }
            if (IsCooldowns && UseTrinket1 == "Cooldowns" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (IsCooldowns && UseTrinket2 == "Cooldowns" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
            if (UseTrinket1 == "always" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (UseTrinket1 == "always" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
            if (API.PlayerItemCanUse("Healthstone") && API.PlayerItemRemainingCD("Healthstone") == 0 && API.PlayerHealthPercent <= HealthStonePercent)
            {
                API.CastSpell("Healthstone");
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
            if (!API.PlayerIsInCombat && InfernalWatch.IsRunning)
            {
                InfernalWatch.Stop();
                InfernalWatch.Reset();
                return;
            }
            if (!API.PlayerIsInCombat && HavocWatch.IsRunning)
            {
                HavocWatch.Stop();
                HavocWatch.Reset();
                return;
            }
            if (InfernalWatch.IsRunning && InfernalWatch.ElapsedMilliseconds >= 30000)
            {
                InfernalWatch.Stop();
                InfernalWatch.Reset();
                return;
            }
            if (HavocWatch.IsRunning && HavocWatch.ElapsedMilliseconds >= 10000)
            {
                HavocWatch.Stop();
                HavocWatch.Reset();
                return;
            }
            //Cooldowns
            if (IsCooldowns)
            {
                //actions.cds=summon_infernal
                if (API.CanCast(SummonInfernal))
                {
                    API.CastSpell(SummonInfernal);
                    InfernalWatch.Start();
                    return;
                }
                //actions.cds+=/dark_soul_instability
                if (API.CanCast(darkSoulInstability) && TalentDarkSoulInstability)
                {
                    API.CastSpell(darkSoulInstability);
                    return;
                }
            }
            // Drain Life
            if (API.PlayerHealthPercent <= DrainLifePercentProc && API.CanCast(DrainLife) && PlayerLevel >= 9)
            {
                API.CastSpell(DrainLife);
                return;
            }
            // Health Funnel
            if (HealthFunnelWatch.IsRunning && API.PetHealthPercent >= 70)
            {
                HealthFunnelWatch.Stop();
                HealthFunnelWatch.Reset();
                return;
            }
            if (HealthFunnelWatch.IsRunning && API.CanCast(HealthFunnel))
            {
                API.CastSpell(HealthFunnel);
                return;
            }
            // Health Funnel
            if (API.PlayerHasPet && API.PetHealthPercent >= 1 && API.PetHealthPercent <= HealthFunnelPercentProc && API.CanCast(HealthFunnel) && PlayerLevel >= 8)
            {
                HealthFunnelWatch.Start();
                return;
            }
            rotation();
            return;
        }

        private void rotation()
        {

            if (API.CanCast(Havoc) && API.PlayerCurrentSoulShards >= 5 && (API.TargetUnitInRangeCount + API.PlayerUnitInMeleeRangeCount == 2))
            {

                API.CastSpell(Havoc);
                HavocWatch.Start();
                if (SwitchTarget)
                {
                    Thread.Sleep(150);
                    API.CastSpell("Switch Target");
                }
                return;
            }
            //AOE
            //actions+=/call_action_list,name=aoe,if=active_enemies>2
            if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber && !HealthFunnelWatch.IsRunning && !API.PlayerIsCasting(true))
            {
                //actions.aoe=rain_of_fire,if=pet.infernal.active&(!cooldown.havoc.ready|active_enemies>3)
                // if (API.CanCast(RainOfFire) && API.PlayerCurrentSoulShards >= 3 && InfernalWatch.IsRunning && API.SpellISOnCooldown(Havoc) || API.TargetUnitInRangeCount > 3)
                // {
                //     API.CastSpell(RainOfFire);
                //     return;
                // }

                //actions.aoe+=/soul_rot
                if (API.CanCast(SoulRot) && PlayerCovenantSettings == "Night Fae" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(SoulRot);
                    return;
                }

                //actions.aoe+=/channel_demonfire,if=dot.immolate.remains>cast_time
                if (API.CanCast(Immolate) && !LastCastImmolate && API.TargetDebuffRemainingTime(Immolate) <= 400)
                {
                    API.CastSpell(Immolate);
                    return;
                }
                //actions.aoe+=/immolate,cycle_targets=1,if=remains<5&(!talent.cataclysm.enabled|cooldown.cataclysm.remains>remains)
                if (API.CanCast(Cataclysm) && TalentCataclysm)
                {
                    API.CastSpell(Cataclysm);
                    return;
                }
                //actions.aoe+=/rain_of_fire
                if (API.CanCast(RainOfFire) && API.PlayerCurrentSoulShards >= 3 && API.TargetUnitInRangeCount >= 3)
                {
                    API.CastSpell(RainOfFire);
                    return;
                }
                //actions.aoe+=/havoc,cycle_targets=1,if=!(self.target=target)

                //actions.aoe+=/decimating_bolt,if=(soulbind.lead_by_example.enabled|!talent.fire_and_brimstone.enabled)
                if (API.CanCast(DecimatingBolt) && !TalentFireandBrimstone && PlayerCovenantSettings == "Necrolord" && (UseCovenantAbility == "always" || UseCovenantAbility == "Cooldowns"))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.aoe+=/incinerate,if=talent.fire_and_brimstone.enabled&buff.backdraft.up&soul_shard<5-0.2*active_enemies
                if (API.CanCast(Incinerate) && TalentFireandBrimstone && API.PlayerHasBuff(Backdraft) && API.PlayerCurrentSoulShards <= 5)
                {
                    API.CastSpell(Incinerate);
                    return;
                }
                //actions.aoe+=/soul_fire
                if (API.CanCast(SoulFire) && TalentSoulFire)
                {
                    API.CastSpell(SoulFire);
                    return;
                }
                //actions.aoe+=/conflagrate,if=buff.backdraft.down
                if (API.CanCast(Conflagrate) && !LastCastConflagrate && !API.PlayerHasBuff(Backdraft))
                {
                    API.CastSpell(Conflagrate);
                    return;
                }
                //actions.aoe+=/shadowburn,if=target.health.pct<20
                if (API.CanCast(ShadowBurn) && !LastCastShadowBurn && TalentShadowBurn && API.TargetHealthPercent <= 20)
                {
                    API.CastSpell(ShadowBurn);
                    return;
                }
                //actions.aoe+=/scouring_tithe,if=!(talent.fire_and_brimstone.enabled|talent.inferno.enabled)
                if (API.CanCast(ScouringTithe) && (TalentFireandBrimstone || TalentInferno) && PlayerCovenantSettings == "Kyrian" && (UseCovenantAbility == "always" || UseCovenantAbility == "Cooldowns"))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
                //actions.aoe+=/impending_catastrophe,if=!(talent.fire_and_brimstone.enabled|talent.inferno.enabled)
                if (API.CanCast(ImpendingCatastrophe) && (TalentFireandBrimstone || TalentInferno) && PlayerCovenantSettings == "Venthyr" && (UseCovenantAbility == "always" || UseCovenantAbility == "Cooldowns"))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions.aoe+=/incinerate
                if (API.CanCast(Incinerate))
                {
                    API.CastSpell(Incinerate);
                    return;
                }
            }
            //SINGLE TARGET
            if ((!IsAOE || IsAOE && API.TargetUnitInRangeCount <= AOEUnitNumber) && IsRange && !HealthFunnelWatch.IsRunning && !API.PlayerIsCasting(true))
            {
                //actions.havoc+=/soul_fire,if=cast_time<havoc_remains
                if (TalentSoulFire && API.CanCast(SoulFire) && HavocWatch.IsRunning && SFTime <= HavocTime)
                {
                    API.CastSpell(SoulFire);
                    return;
                }

                //actions.havoc+=/decimating_bolt,if=cast_time<havoc_remains&soulbind.lead_by_example.enabled
                if (API.CanCast(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && HavocWatch.IsRunning && DBTime <= HavocTime)
                {
                    API.CanCast(DecimatingBolt);
                    return;
                }

                //actions.havoc+=/scouring_tithe,if=cast_time<havoc_remains
                if (API.CanCast(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && HavocWatch.IsRunning && SCTime <= HavocTime)
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }

                //actions.havoc+=/immolate,if=talent.internal_combustion.enabled&remains<duration*0.5|!talent.internal_combustion.enabled&refreshable

                //actions.havoc+=/chaos_bolt,if=cast_time<havoc_remains
                if (API.CanCast(ChaosBolt) && HavocWatch.IsRunning && CBTime <= HavocTime)
                {
                    API.CastSpell(ChaosBolt);
                    return;
                }
                //actions.havoc+=/shadowburn
                if (API.CanCast(ShadowBurn) && HavocWatch.IsRunning && HavocWatch.IsRunning)
                {
                    API.CastSpell(ShadowBurn);
                    return;
                }

                //actions.havoc+=/incinerate,if=cast_time<havoc_remains
                if (API.CanCast(Incinerate) && HavocWatch.IsRunning && IncinerateTime <= HavocTime)
                {
                    API.CastSpell(Incinerate);
                    return;
                }
                //actions.havoc=conflagrate,if=buff.backdraft.down&soul_shard>=1&soul_shard<=4
                if (API.CanCast(Conflagrate) && HavocWatch.IsRunning && !LastCastConflagrate && !API.PlayerHasBuff(Backdraft) && API.PlayerCurrentSoulShards >= 1 && API.PlayerCurrentSoulShards <= 4)
                {
                    API.CastSpell(Conflagrate);
                    return;
                }
                //# Executed every time the actor is available.
                //actions=call_action_list,name=havoc,if=havoc_active&active_enemies>1&active_enemies<5-talent.inferno.enabled+(talent.inferno.enabled&talent.internal_combustion.enabled)
                if (API.CanCast(Havoc) && API.PlayerCurrentSoulShards >= 5 && !API.SpellISOnCooldown(Havoc) && API.TargetUnitInRangeCount == 2)
                {
                    API.CastSpell(Havoc);
                    HavocWatch.Start();
                    if (SwitchTarget)
                    {
                        Thread.Sleep(150);
                        API.CastSpell("Switch Target");
                    }
                    return;
                }
                
                //actions+=/conflagrate,if=talent.roaring_blaze.enabled&debuff.roaring_blaze.remains<1.5
                if (API.CanCast(Conflagrate) && !LastCastConflagrate && TalentRoaringBlaze && API.TargetDebuffRemainingTime(RoaringBlaze) <= 150)
                {
                    API.CastSpell(Conflagrate);
                    return;
                }
                //actions+=/cataclysm,if=!(pet.infernal.active&dot.immolate.remains+1>pet.infernal.remains)|spell_targets.cataclysm>1

                //actions+=/soul_fire,cycle_targets=1,if=refreshable&soul_shard<=4&(!talent.cataclysm.enabled|cooldown.cataclysm.remains>remains)
                if (API.CanCast(SoulFire) && TalentSoulFire && API.PlayerCurrentSoulShards <= 4 && (!TalentCataclysm || API.SpellISOnCooldown(Cataclysm)))
                {
                    API.CastSpell(SoulFire);
                    return;
                }
                //actions+=/immolate,cycle_targets=1,if=refreshable&(!talent.cataclysm.enabled|cooldown.cataclysm.remains>remains)
                if (API.CanCast(Immolate) && !LastCastImmolate && API.TargetDebuffRemainingTime(Immolate) <= 400)
                {
                    API.CastSpell(Immolate);
                    return;
                }
                //actions+=/immolate,if=talent.internal_combustion.enabled&action.chaos_bolt.in_flight&remains<duration*0.5
                if (API.CanCast(ChaosBolt) && API.PlayerCurrentSoulShards >= 2 && TalentInternalCombustion && API.TargetDebuffRemainingTime(Immolate) >= 500 && API.TargetTimeToDie >= 5000)
                {
                    API.CastSpell(ChaosBolt);
                    return;
                }
                //actions+=/channel_demonfire
                if (API.CanCast(ChannelDemonFire) && TalentChannelDemonFire && NotChanneling)
                {
                    API.CastSpell(ChannelDemonFire);
                    return;
                }
                //actions+=/scouring_tithe
                if (API.CanCast(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && (UseCovenantAbility == "always" || UseCovenantAbility == "Cooldowns" && IsCooldowns))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
                //actions+=/decimating_bolt
                if (API.CanCast(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && (UseCovenantAbility == "always" || UseCovenantAbility == "Cooldowns" && IsCooldowns))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions+=/havoc,cycle_targets=1,if=!(target=self.target)&(dot.immolate.remains>dot.immolate.duration*0.5|!talent.internal_combustion.enabled)


                //actions+=/impending_catastrophe
                if (API.CanCast(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && (UseCovenantAbility == "always" || UseCovenantAbility == "Cooldowns" && IsCooldowns))
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
                //actions+=/soul_rot
                if (API.CanCast(SoulRot) && PlayerCovenantSettings == "Night Fae" && (UseCovenantAbility == "always" || UseCovenantAbility == "Cooldowns" && IsCooldowns))
                {
                    API.CastSpell(SoulRot);
                    return;
                }
                //actions+=/havoc,if=runeforge.odr_shawl_of_the_ymirjar.equipped
                //actions+=/variable,name=pool_soul_shards,value=active_enemies>1&cooldown.havoc.remains<=10|cooldown.summon_infernal.remains<=15&talent.dark_soul_instability.enabled&cooldown.dark_soul_instability.remains<=15|talent.dark_soul_instability.enabled&cooldown.dark_soul_instability.remains<=15&(cooldown.summon_infernal.remains>target.time_to_die|cooldown.summon_infernal.remains+cooldown.summon_infernal.duration>target.time_to_die)
                //actions+=/conflagrate,if=buff.backdraft.down&soul_shard>=1.5-0.3*talent.flashover.enabled&!variable.pool_soul_shards
                if (API.CanCast(Conflagrate) && !LastCastConflagrate && !API.PlayerHasBuff(Backdraft) && API.PlayerCurrentSoulShards >= 1 && TalentFlashover)
                {
                    API.CastSpell(Conflagrate);
                    return;
                }
                //actions+=/chaos_bolt,if=buff.dark_soul_instability.up
                if (API.CanCast(ChaosBolt) && API.PlayerHasBuff(darkSoulInstability) && API.PlayerCurrentSoulShards >= 2)
                {
                    API.CastSpell(ChaosBolt);
                    return;
                }
                //actions+=/chaos_bolt,if=buff.backdraft.up&!variable.pool_soul_shards&!talent.eradication.enabled
                if (API.CanCast(ChaosBolt) && API.PlayerHasBuff(Backdraft) && !TalentEradication && API.PlayerCurrentSoulShards >= 2)
                {
                    API.CastSpell(ChaosBolt);
                    return;
                }
                //actions+=/chaos_bolt,if=!variable.pool_soul_shards&talent.eradication.enabled&(debuff.eradication.remains<cast_time|buff.backdraft.up)
                if (API.CanCast(ChaosBolt) && API.PlayerCurrentSoulShards >= 2 && TalentEradication && API.TargetDebuffRemainingTime(Eradication) < API.PlayerCurrentCastTimeRemaining | API.PlayerHasBuff(Backdraft))
                {
                    API.CastSpell(ChaosBolt);
                    return;
                }
                //actions+=/shadowburn,if=!variable.pool_soul_shards|soul_shard>=4.5
                if (API.CanCast(ShadowBurn) && !LastCastShadowBurn && TalentShadowBurn && API.PlayerCurrentSoulShards >= 1)
                {
                    API.CastSpell(ShadowBurn);
                    return;
                }
                //actions+=/chaos_bolt,if=(soul_shard>=4.5-0.2*active_enemies)
                if (API.CanCast(ChaosBolt) && API.PlayerCurrentSoulShards >= 4)
                {
                    API.CastSpell(ChaosBolt);
                    return;
                }
                //actions+=/conflagrate,if=charges>1
                if (API.CanCast(Conflagrate) && !LastCastConflagrate && API.SpellCharges(Conflagrate) >= 1)
                {
                    API.CastSpell(Conflagrate);
                    return;
                }
                //actions+=/incinerate
                if (API.CanCast(Incinerate))
                {
                    API.CastSpell(Incinerate);
                    return;
                }       
            }
        }
        public override void OutOfCombatPulse()
        {
            if (API.PlayerHasPet && TalentGrimoireOfSacrifice && !API.PlayerHasBuff("Grimoire Of Sacrifice"))
            {
                API.CastSpell(GrimoireOfSacrifice);
                return;
            }
            //Summon Imp
            if (API.CanCast(SummonImp) && !LastCastSummonImp && API.PlayerCurrentCastTimeRemaining > 40 && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 3)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (API.CanCast(SummonVoidwalker) && !LastCastSummonVoidwalker && API.PlayerCurrentCastTimeRemaining > 40 && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 10)
            {
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (API.CanCast(SummonSuccubus) && !LastCastSummonSuccubus && API.PlayerCurrentCastTimeRemaining > 40 && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 19)
            {
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (API.CanCast(SummonFelhunter) && !LastCastSummonFelhunter && API.PlayerCurrentCastTimeRemaining > 40 && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 23)
            {
                API.CastSpell(SummonFelhunter);
                return;
            }
        }
    }
}