using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



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
        private string DemonFire = "Demon Fire";
        private string DarkSoulMisery = "Dark Soul Misery";
        private string SummonInfernal = "Summon Infernal";
        private string ChangeTarget = "Change Target";
        private string SoulFire = "Soul Fire";
        private string ChannelDemonFire = "Channel DemonFire";
        private string ScouringTithe = "Scouring Tithe";
        private string Eradication = "Eradication";
        private string ChrashingChaos = "Chrashing Chaos";
        private string DecimatingBolt = "Decimating Bolt";
        private string InternalCombustion = "Internal Combustion";
        private string Misdirection = "Misdirection";
        private string ImpendingCatastrophe = "Impending Catastrophe";
        private string SoulRot = "Soul Rot";
        private string Backdraft = "Backdraft";
        private string CovenantAbility = "Covenant Ability";
        private string darkSoulInstability = "Dark Soul:  Instabilityy";

        //Talents
        private bool TalentFlashover => API.PlayerIsTalentSelected(1, 1);

        private bool TalentSoulFire => API.PlayerIsTalentSelected(1, 3);
        private bool TalentCataclysm => API.PlayerIsTalentSelected(4, 3);
        private bool TalentDarkPact => API.PlayerIsTalentSelected(3, 3);
        private bool TalentGrimoireOfSacrifice => API.PlayerIsTalentSelected(6, 3);
        private bool TalentChannelDemonFire => API.PlayerIsTalentSelected(7, 2);
        private bool TalentDarkSoulMisery => API.PlayerIsTalentSelected(7, 3);
        private bool TalentEradication => API.PlayerIsTalentSelected(1, 2);
        private bool TalentShadowBurn => API.PlayerIsTalentSelected(2, 3);
        private bool TalentInternalCombustion => API.PlayerIsTalentSelected(2, 2);

        //Misc


        //CBProperties
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");


        string[] MisdirectionList = new string[] { "Imp", "Voidwalker", "Succubus", "Felhunter", "Infernal" };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private int DarkPactPercentProc => numbList[CombatRoutine.GetPropertyInt(DarkPact)];
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];
        private bool IsRange => API.TargetRange < 40;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        bool LastCastImmolate => API.PlayerLastSpell == Immolate;
        bool LastCastConflagrate => API.PlayerLastSpell == Conflagrate;
        bool LastCastShadowBurn => API.PlayerLastSpell == ShadowBurn;
        private string UseCovenantAbility => CovenantAbilityList[CombatRoutine.GetPropertyInt(CovenantAbility)];
        string[] CovenantAbilityList = new string[] { "always", "Cooldowns", "AOE" };

        public override void Initialize()
        {
            CombatRoutine.Name = "Destruction Warlock @Mufflon12";
            API.WriteLog("Welcome to Destruction Warlock rotation @ Mufflon12");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            //Options
            CombatRoutine.AddProp(Misdirection, "Wich Pet", MisdirectionList, "Chose your Pet", "PETS", 0);
            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(HealthFunnel, "Health Funnel", numbList, "Life percent at which " + HealthFunnel + " is used, set to 0 to disable", "PETS", 0);
            CombatRoutine.AddProp(DarkPact, "Dark Pact", numbList, "Life percent at which " + DarkPact + " is used, set to 0 to disable", "Healing", 2);
            CombatRoutine.AddProp("Covenant Ability", "Use " + "Covenant Ability", CovenantAbilityList, "How to use Weapons of Order", "Covenant", 0);
            AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");


            //Spells
            CombatRoutine.AddSpell(Immolate, "D1");
            CombatRoutine.AddSpell(Incinerate, "D2");
            CombatRoutine.AddSpell(Conflagrate, "D3");
            CombatRoutine.AddSpell(ChaosBolt, "D4");
            CombatRoutine.AddSpell(ShadowBurn, "D5");
            CombatRoutine.AddSpell(Cataclysm, "D6");
            CombatRoutine.AddSpell(Havoc, "D7");
            CombatRoutine.AddMacro(RainOfFire, "D8");
            CombatRoutine.AddSpell(SoulFire, "D9");
            CombatRoutine.AddSpell(ChannelDemonFire, "D0");
            CombatRoutine.AddSpell(DarkSoulMisery, "OemOpenBrackets");

            CombatRoutine.AddSpell(DrainLife, "NumPad1");
            CombatRoutine.AddSpell(HealthFunnel, "NumPad2");
            CombatRoutine.AddSpell(SummonInfernal, "NumPad5");
            CombatRoutine.AddSpell(SummonFelhunter, "NumPad6");
            CombatRoutine.AddSpell(SummonSuccubus, "NumPad7");
            CombatRoutine.AddSpell(SummonVoidwalker, "NumPad8");
            CombatRoutine.AddSpell(SummonImp, "NumPad9");

            CombatRoutine.AddSpell(ScouringTithe, "F1");
            CombatRoutine.AddSpell(DecimatingBolt, "F1");
            CombatRoutine.AddSpell(ImpendingCatastrophe, "F1");
            CombatRoutine.AddSpell(SoulRot, "F1");

            //Buffs
            CombatRoutine.AddBuff("Grimoire Of Sacrifice");
            CombatRoutine.AddBuff(ChrashingChaos);
            CombatRoutine.AddBuff(Backdraft);
            CombatRoutine.AddBuff(darkSoulInstability);

            //Debuffs
            CombatRoutine.AddDebuff(Immolate);
            CombatRoutine.AddDebuff(Havoc);
            CombatRoutine.AddDebuff(Eradication);
        }

        public override void Pulse()
        {

        }

        public override void CombatPulse()
        {
            //AOE
            if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber && NotCasting)
            {
                if (API.CanCast(Cataclysm) && !API.MouseoverHasDebuff(Immolate) && API.PlayerCanAttackMouseover && API.CanCast(Cataclysm) && API.TargetDebuffRemainingTime(Immolate) <= 500 && IsRange && TalentCataclysm && NotMoving && NotCasting && IsRange && NotChanneling)
                {
                    API.CastSpell(Cataclysm);
                    return;
                }
                //actions.cds=summon_infernal
                if (IsCooldowns && API.CanCast(SummonInfernal) && PlayerLevel >= 42)
                {
                    API.CastSpell(SummonInfernal);
                    return;
                }
                //actions.havoc=conflagrate,if=buff.backdraft.down&soul_shard>=1&soul_shard<=4
                if (API.CanCast(Havoc) && API.PlayerHasBuff(Backdraft) && API.PlayerCurrentSoulShards <= 4)
                {
                    API.CastSpell(Havoc);
                    return;
                }
                //actions.havoc+=/soul_fire,if=cast_time<havoc_remains
                if (!API.SpellISOnCooldown(SoulFire) && TalentSoulFire && API.TargetDebuffRemainingTime(Havoc) >= 500)
                {
                    API.CastSpell(SoulFire);
                    return;
                }
                //actions.havoc+=/decimating_bolt,if=cast_time<havoc_remains&soulbind.lead_by_example.enabled
                //decimating_bolt Necrolord Covenant Spell
                if (API.CanCast(DecimatingBolt) && PlayerLevel >= 52 && API.TargetDebuffRemainingTime(Havoc) >= 500 && !API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.havoc+=/scouring_tithe,if=cast_time<havoc_remains
                //actions +=/ scouring_tithe
                //scouring_tithe Kyrian	Covenant Spell
                if (API.CanCast(ScouringTithe) && PlayerLevel >= 52 && API.TargetDebuffRemainingTime(Havoc) >= 500 && !API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
                //actions.havoc+=/immolate,if=talent.internal_combustion.enabled&remains<duration*0.5|!talent.internal_combustion.enabled&refreshable
                if (API.CanCast(Havoc) && API.TargetDebuffRemainingTime(Immolate) > 500 && TalentInternalCombustion)
                {
                    API.CastSpell(Havoc);
                    return;
                }
                //actions.havoc+=/chaos_bolt,if=cast_time<havoc_remains
                if (API.CanCast(ChaosBolt) && API.PlayerCurrentSoulShards > 2 && PlayerLevel >= 10)
                {
                    API.CastSpell(ChaosBolt);
                    return;
                }
                //actions.havoc+=/shadowburn
                if (API.CanCast(ShadowBurn) && !LastCastShadowBurn && TalentShadowBurn && API.SpellCharges(ShadowBurn) >= 1 && API.PlayerCurrentSoulShards >= 4)
                {
                    API.CastSpell(ShadowBurn);
                    return;
                }
                //actions.havoc+=/incinerate,if=cast_time<havoc_remains
                if (API.CanCast(Incinerate) && PlayerLevel >= 10)
                {
                    API.CastSpell(Incinerate);
                    return;
                }
                //SingleTarget
                if (NotMoving && NotCasting && IsRange && NotChanneling)
                {
                    // Immolate
                    if (API.CanCast(Immolate) && !LastCastImmolate && API.TargetDebuffRemainingTime(Immolate) <= 320 && PlayerLevel >= 11)
                    {
                        API.CastSpell(Immolate);
                        return;
                    }
                    // Channel DemonFire
                    //actions+=/channel_demonfire
                    if (API.CanCast(ChannelDemonFire) && TalentChannelDemonFire)
                    {
                        API.CastSpell(ChannelDemonFire);
                        return;
                    }
                    //actions +=/ scouring_tithe
                    //scouring_tithe Kyrian	Covenant Spell
                    if (API.CanCast(ScouringTithe) && PlayerLevel >= 52 && !API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && UseCovenantAbility == "always")
                    {
                        API.CastSpell(ScouringTithe);
                        return;
                    }
                    //actions+=/decimating_bolt
                    //decimating_bolt Necrolord Covenant Spell
                    if (API.CanCast(DecimatingBolt) && PlayerLevel >= 52 && !API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && UseCovenantAbility == "always")
                    {
                        API.CastSpell(DecimatingBolt);
                        return;
                    }
                    //actions +=/ havoc,cycle_targets = 1,if= !(target = self.target) & (dot.immolate.remains > dot.immolate.duration * 0.5 | !talent.internal_combustion.enabled)
                    if (API.CanCast(Havoc) && API.TargetDebuffRemainingTime(Immolate) > 500 && TalentInternalCombustion)
                    {
                        API.CastSpell(Havoc);
                        return;
                    }
                    //actions +=/ impending_catastrophe
                    //impending_catastrophe Venthyr Covenant Spell 
                    if (API.CanCast(ImpendingCatastrophe) && PlayerLevel >= 52 && !API.SpellISOnCooldown(ImpendingCatastrophe) && PlayerCovenantSettings == "Venthyr" && UseCovenantAbility == "always")
                    {
                        API.CastSpell(ImpendingCatastrophe);
                        return;
                    }
                    //actions +=/ soul_rot
                    // Soul Rot Night Fae Covenant Spell
                    if (API.CanCast(SoulRot) && PlayerLevel >= 52 && !API.SpellISOnCooldown(SoulRot) && PlayerCovenantSettings == "Night Fae" && UseCovenantAbility == "always")
                    {
                        API.CastSpell(SoulRot);
                        return;
                    }
                    //actions +=/ conflagrate,if= buff.backdraft.down & soul_shard >= 1.5 - 0.3 * talent.flashover.enabled & !variable.pool_soul_shards
                    if (API.CanCast(Conflagrate) && !LastCastConflagrate && PlayerLevel >= 13 && API.PlayerCurrentSoulShards >= 1.5 && TalentFlashover)
                    {
                        API.CastSpell(Conflagrate);
                        return;
                    }
                    //actions+=/chaos_bolt,if=buff.dark_soul_instability.up
                    // Chaos Bolt
                    if (API.CanCast(ChaosBolt) && API.PlayerCurrentSoulShards >= 2 && PlayerLevel >= 10 && API.PlayerHasBuff(darkSoulInstability))
                    {
                        API.CastSpell(ChaosBolt);
                        return;
                    }
                    //actions+=/chaos_bolt,if=buff.backdraft.up&!variable.pool_soul_shards&!talent.eradication.enabled
                    if (API.CanCast(ChaosBolt) && PlayerLevel >= 10 && API.PlayerCurrentSoulShards >= 2 && API.PlayerHasBuff(Backdraft))
                    {
                        API.CastSpell(ChaosBolt);
                        return;
                    }
                    //actions+=/chaos_bolt,if=!variable.pool_soul_shards&talent.eradication.enabled&(debuff.eradication.remains<cast_time|buff.backdraft.up)
                    if (API.CanCast(ChaosBolt) && PlayerLevel >= 10 && API.PlayerCurrentSoulShards >= 2 && TalentEradication && (!API.TargetHasDebuff(Eradication) || (API.TargetHasDebuff(Eradication) && API.TargetDebuffRemainingTime(Eradication) <= 150)))
                    {
                        API.CastSpell(ChaosBolt);
                        return;
                    }
                    //actions+=/shadowburn,if=!variable.pool_soul_shards|soul_shard>=4.5
                    if (API.CanCast(ShadowBurn) && !LastCastShadowBurn && TalentShadowBurn && API.PlayerCurrentSoulShards >= 4 && API.SpellCharges(ShadowBurn) >= 1)
                    {
                        API.CastSpell(ShadowBurn);
                        return;
                    }
                    //actions +=/ chaos_bolt,if= (soul_shard >= 4.5 - 0.2 * active_enemies)

                    // Chaos Bolt
                    if (API.CanCast(ChaosBolt) && API.PlayerCurrentSoulShards == 5 && PlayerLevel >= 10)
                    {
                        API.CastSpell(ChaosBolt);
                        return;
                    }
                    //Soul Fire
                    if (!API.SpellISOnCooldown(SoulFire) && TalentSoulFire)
                    {
                        API.CastSpell(SoulFire);
                        return;
                    }
                    //Conflagrate
                    if (API.CanCast(Conflagrate) && !LastCastConflagrate && API.SpellCharges(Conflagrate) >= 2 && API.PlayerCurrentSoulShards < 5)
                    {
                        API.CastSpell(Conflagrate);
                        return;
                    }

                    if (API.CanCast(ChaosBolt) && PlayerLevel >= 10 && API.SpellCDDuration(Havoc) >= 2000 && API.PlayerCurrentSoulShards >= 2 && TalentEradication && (!API.TargetHasDebuff(Eradication) || (API.TargetHasDebuff(Eradication) && API.TargetDebuffRemainingTime(Eradication) <= 150)))
                    {
                        API.CastSpell(ChaosBolt);
                        return;
                    }

                    if (API.CanCast(Conflagrate) && !LastCastConflagrate && PlayerLevel >= 13 && API.PlayerCurrentSoulShards < 5 && API.SpellCharges(Conflagrate) >= 1)
                    {
                        API.CastSpell(Conflagrate);
                        return;
                    }

                    if (API.CanCast(Incinerate) && PlayerLevel >= 10 && API.PlayerCurrentSoulShards < 5)
                    {
                        API.CastSpell(Incinerate);
                        return;
                    }
                }
            }
            //SINGLE TARGET
            if (API.TargetUnitInRangeCount <= AOEUnitNumber && NotCasting)
            {
                if (API.CanCast(Cataclysm) && !API.MouseoverHasDebuff(Immolate) && API.PlayerCanAttackMouseover && API.CanCast(Cataclysm) && API.TargetDebuffRemainingTime(Immolate) <= 500 && IsRange && TalentCataclysm && NotMoving && NotCasting && IsRange && NotChanneling)
                {
                    API.CastSpell(Cataclysm);
                    return;
                }
                //actions.cds=summon_infernal
                if (IsCooldowns && API.CanCast(SummonInfernal) && PlayerLevel >= 42)
                {
                    API.CastSpell(SummonInfernal);
                    return;
                }
                //actions.havoc=conflagrate,if=buff.backdraft.down&soul_shard>=1&soul_shard<=4
                if (API.CanCast(Havoc) && API.PlayerHasBuff(Backdraft) && API.PlayerCurrentSoulShards <= 4)
                {
                    API.CastSpell(Havoc);
                    return;
                }
                //actions.havoc+=/soul_fire,if=cast_time<havoc_remains
                if (!API.SpellISOnCooldown(SoulFire) && TalentSoulFire && API.TargetDebuffRemainingTime(Havoc) >= 500)
                {
                    API.CastSpell(SoulFire);
                    return;
                }
                //actions.havoc+=/decimating_bolt,if=cast_time<havoc_remains&soulbind.lead_by_example.enabled
                //decimating_bolt Necrolord Covenant Spell
                if (API.CanCast(DecimatingBolt) && PlayerLevel >= 52 && API.TargetDebuffRemainingTime(Havoc) >= 500 && !API.SpellISOnCooldown(DecimatingBolt) && PlayerCovenantSettings == "Necrolord" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
                //actions.havoc+=/scouring_tithe,if=cast_time<havoc_remains
                //actions +=/ scouring_tithe
                //scouring_tithe Kyrian	Covenant Spell
                if (API.CanCast(ScouringTithe) && PlayerLevel >= 52 && API.TargetDebuffRemainingTime(Havoc) >= 500 && !API.SpellISOnCooldown(ScouringTithe) && PlayerCovenantSettings == "Kyrian" && (UseCovenantAbility == "always" || UseCovenantAbility == "AOE"))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
                //actions.havoc+=/immolate,if=talent.internal_combustion.enabled&remains<duration*0.5|!talent.internal_combustion.enabled&refreshable
                if (API.CanCast(Havoc) && API.TargetDebuffRemainingTime(Immolate) > 500 && TalentInternalCombustion)
                {
                    API.CastSpell(Havoc);
                    return;
                }
                //actions.havoc+=/chaos_bolt,if=cast_time<havoc_remains
                if (API.CanCast(ChaosBolt) && API.PlayerCurrentSoulShards > 2 && PlayerLevel >= 10)
                {
                    API.CastSpell(ChaosBolt);
                    return;
                }
                //actions.havoc+=/shadowburn
                if (API.CanCast(ShadowBurn) && !LastCastShadowBurn && TalentShadowBurn && API.SpellCharges(ShadowBurn) >= 1 && API.PlayerCurrentSoulShards >= 4)
                {
                    API.CastSpell(ShadowBurn);
                    return;
                }
                //actions.havoc+=/incinerate,if=cast_time<havoc_remains
                if (API.CanCast(Incinerate) && PlayerLevel >= 10)
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
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonImp) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 3)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 10)
            {
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 19)
            {
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 23)
            {
                API.CastSpell(SummonFelhunter);
                return;
            }
        }
    }
}
