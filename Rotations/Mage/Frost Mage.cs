using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class FrostMage : CombatRoutine
    {
        //Toggles

        private bool IsPause => API.ToggleIsEnabled("Pause");


        //General
        private int PlayerLevel => API.PlayerLevel;


        //CLASS SPECIFIC
        private bool UseFB => (bool)CombatRoutine.GetProperty("UseFB");
        private bool UseFN => (bool)CombatRoutine.GetProperty("UseFN");



        //Spells,Buffs,Debuffs
        private string Frostbolt = "Frostbolt";
        private string FireBlast = "Fire Blast";
        private string FrostNova = "Frost Nova";
        private string ArcaneIntellect = "Arcane Intellect";
        private string IceLance = "Ice Lance";
        private string Icicles = "Icicles";
        private string FingersofFrost = "Fingers of Frost";
        private string Blizzard = "Blizzard";
        private string ConeOfCold = "Cone of Cold";
        private string Flury = "Flury";
        private string BrainFreeze = "Brain Freeze";
        private string FrozenOrb = "Frozen Orb";


        public override void Initialize()
        {
            CombatRoutine.Name = "Frost Mage @Mufflon12";
            API.WriteLog("Welcome to Frost Mage rotation @ Mufflon12");
            API.WriteLog("Use /cast [@Player] Arcane Intellect to buff Arcane Intellect");
            API.WriteLog("Use /cast [@cursor] Blizzard for Blizzar AEO");

            CombatRoutine.AddProp("UseFB", "Use Fire Blast", true, "Should the rotation use Fire Blast");
            CombatRoutine.AddProp("UseFN", "Use Frost Nova", true, "Should the rotation use Frost Nova if target range is below 10 Yards");


            //Spells
            CombatRoutine.AddSpell("Frostbolt", "D1");
            CombatRoutine.AddSpell("Ice Lance", "D2");
            CombatRoutine.AddSpell("Cone of Cold", "D3");
            CombatRoutine.AddSpell("Flury", "D4");
            CombatRoutine.AddSpell("Frozen Orb", "D5");
            CombatRoutine.AddSpell("Fire Blast", "NumPad1");
            CombatRoutine.AddSpell("Frost Nova", "NumPad2");
            CombatRoutine.AddSpell("Arcane Intellect", "NumPad3");
            CombatRoutine.AddSpell("Blizzard", "NumPad4");


            //Buffs
            CombatRoutine.AddBuff("Arcane Intellect");
            CombatRoutine.AddBuff("Icicles");
            CombatRoutine.AddBuff("Fingers of Frost");
            CombatRoutine.AddBuff("Brain Freeze");


            //Debuffs
            CombatRoutine.AddDebuff("Frost Nova");


            //TOGGLES
            CombatRoutine.AddToggle("Pause");
            CombatRoutine.AddToggle("AOE");
            CombatRoutine.AddToggle("Cooldowns");
        }

        public override void Pulse()
        {
            if (!IsPause && API.PlayerIsInCombat && !API.PlayerIsCasting && !API.PlayerIsMounted)
            {
                //Cooldowns
                if (IsCooldowns)
                {

                }
                //AOE
                if (IsAOE)
                {
                    if (API.CanCast(Blizzard) && API.PlayerLevel >= 14)
                    {
                        API.CastSpell(Blizzard);
                        return;
                    }
                }
                if ((!API.PlayerIsInCombat || API.PlayerIsInCombat) && (!API.TargetIsIncombat || API.TargetIsIncombat) && API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
                {
                    CombatPulse();
                }

                if (!API.PlayerIsMounted)
                {
                    if (!API.PlayerIsInCombat)
                    {
                        OutOfCombatPulse();
                    }

                    rotation();
                    return;

                }
            }
        }

        public override void OutOfCombatPulse()
        {

        }
        public override void CombatPulse()
        {
            if (!IsPause && API.PlayerIsInCombat && !API.PlayerIsCasting && !API.PlayerIsMounted)
            {
                //ARCANE INTELLECT
                if (API.CanCast(Flury) && API.PlayerHasBuff(BrainFreeze) && API.PlayerLevel >= 32)
                {
                    API.CastSpell(Flury);
                    return;
                }

                //FROZEN ORB
                if (API.CanCast(FrozenOrb) && API.PlayerLevel >= 38)
                {
                    API.CastSpell(FrozenOrb);
                    return;
                }

                //ACANE INTELLECT
                if (API.CanCast(ArcaneIntellect) && !API.PlayerHasBuff(ArcaneIntellect) && API.TargetUnitInRangeCount > 2 && API.PlayerLevel >= 3)
                {
                    API.CastSpell(ArcaneIntellect);
                    return;
                }

                //FROST NOVA
                if (UseFN)
                {
                    if (!API.SpellISOnCooldown(FrostNova) && PlayerLevel >= 3 && API.TargetRange < 10)
                    {
                        API.CastSpell(FrostNova);
                        return;
                    }
                }

                //FIREBLAST
                if (UseFB)
                {
                    if (!API.SpellISOnCooldown(FireBlast) && PlayerLevel >= 3 && !API.TargetHasDebuff(FrostNova))
                    {
                        API.CastSpell(FireBlast);
                        return;
                    }
                }

                //ICE LANCE FINGERS OF FROST
                if (API.CanCast(IceLance) && PlayerLevel >= 13 && API.PlayerHasBuff(Icicles) && API.PlayerHasBuff(FingersofFrost) && API.TargetRange < 40)
                {
                    API.CastSpell(IceLance);
                    return;
                }

                //ICE LANCE FROST NOVA
                if (API.CanCast(IceLance) && PlayerLevel >= 10 && API.TargetHasDebuff(FrostNova) && API.PlayerHasBuff(Icicles) && API.TargetRange < 40)
                {
                    API.CastSpell(IceLance);
                    return;
                }

                //Cone of Cold
                if (API.CanCast(ConeOfCold) && API.TargetRange < 10 && PlayerLevel >= 18)
                {
                    API.CastSpell(ConeOfCold);
                    return;
                }

                //FROSTBOLT
                if (API.CanCast(Frostbolt) && !API.PlayerIsMoving && API.TargetRange < 40)
                {
                    API.CastSpell(Frostbolt);
                    return;
                }
            }
        }

        // ROTATION
        private void rotation()
        {

        }
    }
}
