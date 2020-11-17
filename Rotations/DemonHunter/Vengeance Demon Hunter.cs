using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class VengeanceDemonHunter : CombatRoutine
    {
        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 8;


        //CBProperties
        private int SoulCleavePercentProc => numbList[CombatRoutine.GetPropertyInt(SoulCleave)];
        private bool UseSIS => (bool)CombatRoutine.GetProperty("UseSIS");
        private bool UseCF => (bool)CombatRoutine.GetProperty("UseCF");
        private bool UseHoR => (bool)CombatRoutine.GetProperty("UseHoR");

        private int SoulFragmentNumner => CombatRoutine.GetPropertyInt("SoulFragmentNumner");




        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };


        //Spells,Buffs,Debuffs
        private string InfernalStrike = "Infernal Strike";
        private string ThrowGlaive = "Throw Glaive";
        private string Shear = "Shear";
        private string DemonSpikes = "Demon Spikes";
        private string Metamorphosis = "Metamorphosis";
        private string FieryBrand = "Fiery Brand";
        private string SoulCleave = "Soul Cleave";
        private string ImmolationAura = "Immolation Aura";
        private string SigilofFlame = "Sigil of Flame";
        private string FelDevastation = "Fel Devastation";
        private string Fracture = "Fracture";
        private string SpiritBomb = "Spirit Bomb";
        private string Disrupt = "Disrupt";
        private string SigilofChains = "Sigil of Chains";
        private string SoulBarrier = "Soul Barrier";
        private string BulkExtraction = "Bulk Extraction";
        private string ConcentratedFlame = "Concentrated Flame";
        private string Frailty = "Frailty";
        private string SoulFragments = "Soul Fragments";

        public override void Initialize()
        {
            CombatRoutine.Name = "Vengeance DemonHunter @Mufflon12";
            API.WriteLog("Welcome to Vengeance DemonHunter rotation @ Mufflon12");

            //Concentrated Flame
            CombatRoutine.AddProp("UseCF", "Use CF", true, "Should the rotation use Concentrated Flame");
            //Self Infernal Strike
            CombatRoutine.AddProp("UseSIS", "Use SIS", true, "Should the rotation use Self Infernal Strike", "Generic");
            //Soul Cleave Heal
            CombatRoutine.AddProp(SoulCleave, "Soul Cleave", numbList, "Life percent at which " + SoulCleave + " is used, set to 0 to disable", "Healing", 9);
            //Soul Fragments to use Spirit Bomb
            CombatRoutine.AddProp("SoulFragmentNumner", "Soul Fragments", 4, "How many Soul Fragments to use Spirit Bomb", "Talents");


            //Spells
            CombatRoutine.AddSpell("Infernal Strike", "NumPad2");
            CombatRoutine.AddSpell("Throw Glaive", "D6");
            CombatRoutine.AddSpell("Shear", "D2");
            CombatRoutine.AddSpell("Demon Spikes", "NumPad3");
            CombatRoutine.AddSpell("Metamorphosis", "D8");
            CombatRoutine.AddSpell("Fiery Brand", "D9");
            CombatRoutine.AddSpell("Soul Cleave", "D3");
            CombatRoutine.AddSpell("Immolation Aura", "D4");
            CombatRoutine.AddSpell("Sigil of Flame", "D7");
            CombatRoutine.AddSpell("Fel Devastation", "D5");
            CombatRoutine.AddSpell("Fracture", "D2");
            CombatRoutine.AddSpell("Spirit Bomb", "Oem6");
            CombatRoutine.AddSpell("Disrupt", "NumPad1");
            CombatRoutine.AddSpell("Sigil of Chains", "F1");
            CombatRoutine.AddSpell("Soul Barrier", "F2");
            CombatRoutine.AddSpell("Bulk Extraction", "F3");
            CombatRoutine.AddSpell("Concentrated Flame", "NumPad4");

            //Buffs
            CombatRoutine.AddBuff("Demon Spikes");
            CombatRoutine.AddBuff("Soul Fragments");
            CombatRoutine.AddBuff("Concentrated Flame");
            CombatRoutine.AddBuff("Hour of Reaping");


            //Debuffs
            CombatRoutine.AddDebuff("Frailty");



            //Debuffs


        }

        public override void Pulse()
        {
            {
                //KICK
                if (isInterrupt && API.TargetCanInterrupted && API.TargetIsCasting && API.TargetCurrentCastTimeRemaining < interruptDelay && !API.SpellISOnCooldown(Disrupt) && IsMelee && PlayerLevel >= 7)
                {
                    API.CastSpell(Disrupt);
                    return;
                }
            }
        }

        public override void CombatPulse()
        {
            {
                //Cooldowns
                if (IsCooldowns)
                {
                    if (API.CanCast(Metamorphosis) && PlayerLevel >= 12)
                    {
                        API.CastSpell(Metamorphosis);
                        return;
                    }
                    if (API.CanCast(FieryBrand) && PlayerLevel >= 14)
                    {
                        API.CastSpell(FieryBrand);
                        return;
                    }
                }
                //Concentrated Flame
                if (UseCF)
                {
                    if (API.CanCast(ConcentratedFlame) && API.TargetRange <= 40)
                    {
                        API.CastSpell(ConcentratedFlame);
                        return;
                    }
                }
                // Soul Cleave
                if (API.PlayerHealthPercent <= SoulCleavePercentProc && API.CanCast(SoulCleave) && API.PlayerBuffStacks(SoulFragments) > 2 && API.PlayerFury > 30 && PlayerLevel >= 1)
                {
                    API.CastSpell(SoulCleave);
                    return;
                }
                //Infernal Strike
                if (UseSIS)
                {
                    if (API.SpellCharges(InfernalStrike) > 1 && IsMelee && !API.PlayerIsChanneling && PlayerLevel >= 10)
                    {
                        API.CastSpell(InfernalStrike);
                        return;
                    }
                }
                //Fracture
                if (API.PlayerIsTalentSelected(4, 3) && !API.SpellISOnCooldown(Fracture) && IsMelee)
                {
                    API.CastSpell(Fracture);
                    return;
                }
                //Fel Devastation
                if (!API.SpellISOnCooldown(FelDevastation) && IsMelee && API.PlayerFury > 50 && PlayerLevel >= 11)
                {
                    API.CastSpell(FelDevastation);
                    return;
                }
                //Spirit Bomb
                if (API.PlayerIsTalentSelected(3, 3) && IsMelee && API.PlayerHealthPercent > 90 && API.PlayerBuffStacks(SoulFragments) >= SoulFragmentNumner)
                {
                    API.CastSpell(SpiritBomb);
                    return;
                }
                //Throw Glaive
                if (API.CanCast(ThrowGlaive) && !IsMelee && API.TargetRange <= 30 && PlayerLevel >= 19)
                {
                    API.CastSpell(ThrowGlaive);
                    return;
                }
                //Shear
                if (!API.SpellISOnCooldown(Shear) && !API.PlayerIsTalentSelected(4, 3) && IsMelee)
                {
                    API.CastSpell(Shear);
                    return;
                }
                //Demon Spikes
                if (API.CanCast(DemonSpikes) && API.PlayerBuffTimeRemaining(DemonSpikes) < 100 && PlayerLevel >= 14)
                {
                    API.CastSpell(DemonSpikes);
                    return;
                }
                //Immolation Aura
                if (!API.SpellISOnCooldown(ImmolationAura) && PlayerLevel >= 14)
                {
                    API.CastSpell(ImmolationAura);
                    return;
                }
                //Sigil of Flames
                if (!API.SpellISOnCooldown(SigilofFlame) && IsMelee && PlayerLevel >= 12)
                {
                    API.CastSpell(SigilofFlame);
                    return;
                }

                //Talents
                if (API.PlayerIsTalentSelected(5, 3) && API.CanCast(SigilofChains) && !IsMelee)
                {
                    API.CastSpell(SigilofChains);
                    return;
                }
                if (API.PlayerIsTalentSelected(6, 3) && API.CanCast(SoulBarrier) && API.PlayerBuffTimeRemaining(SoulBarrier) < 100)
                {
                    API.CastSpell(SoulBarrier);
                    return;
                }
                if (API.PlayerIsTalentSelected(7, 3) && API.CanCast(BulkExtraction))
                {
                    API.CastSpell(BulkExtraction);
                    return;
                }
            }
        }
        public override void OutOfCombatPulse()
        {

        }
    }
}
