using System;
using System.Collections.Generic;
using System.Linq;



namespace HyperElk.Core
{
    public class VengeanceDH : CombatRoutine
    {
        //Toggles
        private bool IsAOE => API.ToggleIsEnabled("AOE");
        private bool IsPause => API.ToggleIsEnabled("Pause");
        private bool IsCooldowns => API.ToggleIsEnabled("Cooldowns");

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;

        //DH specific 
        private bool UseSIS => (bool)CombatRoutine.GetProperty("UseSIS");



        public override void Initialize()
        {
            CombatRoutine.Name = "Vengeance DH @Mufflon12";
            API.WriteLog("Welcome to Vengeance DH rotation @ Mufflon12");
            API.WriteLog("Self Infernal Strike : /cast [@player] Infernal Strike");
            API.WriteLog("Sigil of Flame : /cast [@player] Sigil of Flame");
            API.WriteLog("Sigil of Misery : /cast [@player] Sigil of Misery");
            API.WriteLog("Sigil of Silence : /cast [@player] Sigil of Silence");
            API.WriteLog("Sigil of Silence : /cast [@player] Sigil of Chains");

            CombatRoutine.AddProp("UseSIS", "Use SIS", true, "Should the rotation use Self Infernal Strike");


            //Spells
            CombatRoutine.AddSpell("Infernal Strike", "NumPad2");
            CombatRoutine.AddSpell("Throw Glaive", "D6");
            CombatRoutine.AddSpell("Shear", "D2");
            CombatRoutine.AddSpell("Demon Spikes", "NumPad1");
            CombatRoutine.AddSpell("Metamorphosis", "D8");
            CombatRoutine.AddSpell("Fiery Brand", "D9");
            CombatRoutine.AddSpell("Soul Cleave", "D3");
            CombatRoutine.AddSpell("Immolation Aura", "D4");
            CombatRoutine.AddSpell("Sigil of Flame", "D7");
            CombatRoutine.AddSpell("Fel Devastation", "D5");
            CombatRoutine.AddSpell("Fracture", "D2");
            CombatRoutine.AddSpell("Spirit Bomb", "Oem6");
            CombatRoutine.AddSpell("Disrupt", "Oem6");
            CombatRoutine.AddSpell("Sigil of Chains", "NumPad10");
            CombatRoutine.AddSpell("Soul Barrier", "NumPad11");
            CombatRoutine.AddSpell("Bulk Extraction", "NumPad12");
            
            //Buffs
            CombatRoutine.AddBuff("Demon Spikes");
             CombatRoutine.AddBuff("Soul Fragments");

            //Debuffs
            CombatRoutine.AddDebuff("Frailty");



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
                    if (API.CanCast("Metamorphosis", true, true))
                    {
                        API.CastSpell("Metamorphosis");
                        return;
                    }
                    if (API.CanCast("Fiery Brand", true, true))
                    {
                        API.CastSpell("Fiery Brand");
                        return;
                    }

                }
                if (PlayerLevel <= 50)
                {
                    rotation();
                    return;
                }
            }

        }




        // ROTATION
        private void rotation()
        {
            if (UseSIS)
            {
                if (API.SpellCharges("Infernal Strike") > 1 && IsMelee && !API.PlayerIsChanneling)
                {
                    API.CastSpell("Infernal Strike");
                    return;
                }
            }
            if (API.CanCast("Throw Glaive", true, true) && !IsMelee && API.TargetRange <= 30)
            {
               API.CastSpell("Throw Glaive");
                return;
           }
            if (!API.SpellISOnCooldown("Shear") && !API.PlayerIsTalentSelected(4, 3) && IsMelee)
            {
                API.CastSpell("Shear");
                return;
            }
            if (API.CanCast("Demon Spikes", true, true) && API.PlayerBuffTimeRemaining("Demon Spikes") < 100)
            {
                API.CastSpell("Demon Spikes");
                return;
            }
            if (API.PlayerFury > 65 && API.PlayerHealthPercent < 90)
            {
                API.CastSpell("Soul Cleave");
                return;
            }
            if (!API.SpellISOnCooldown("Immolation Aura"))
            {
                API.CastSpell("Immolation Aura");
                return;
            }
            if (!API.SpellISOnCooldown("Sigil of Flame") && IsMelee)
            {
                API.CastSpell("Sigil of Flame");
                return;
            }
            if (!API.SpellISOnCooldown("Fel Devastation") && IsMelee && API.PlayerFury > 50)
            {
                API.CastSpell("Fel Devastation");
                return;
            }

            //Talents
            if (API.PlayerIsTalentSelected(4, 3) && !API.SpellISOnCooldown("Fracture") && IsMelee)
            {
                API.CastSpell("Fracture");
                return;
            }
            if (API.PlayerIsTalentSelected(3, 3) && API.TargetDebuffRemainingTime("Frailty") < 100 && IsMelee && API.PlayerHealthPercent > 90 && API.PlayerBuffStacks("Soul Fragments") > 3)
            {
                API.CastSpell("Spirit Bomb");
                return;
            }
            if (IsMelee && API.TargetIsCasting && API.TargetCanInterrupted)
            {
                API.CastSpell("Disrupt");
                return;
            }
            if (API.PlayerIsTalentSelected(5, 3) && API.CanCast("Sigil of Chains", true, true) && !IsMelee)
            {
                API.CastSpell("Sigil of Chains");
                return;
            }
            if (API.PlayerIsTalentSelected(6, 3) && API.CanCast("Soul Barrier", true, true) && API.PlayerBuffTimeRemaining("Soul Barrier") < 100)
            {
                API.CastSpell("Soul Barrier");
                return;
            }
            if (API.PlayerIsTalentSelected(7, 3) && API.CanCast("Bulk Extraction", true, true))
            {
                API.CastSpell("Bulk Extraction");
                return;
            }
        }
    }
}
