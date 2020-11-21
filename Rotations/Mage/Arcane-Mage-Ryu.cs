
namespace HyperElk.Core
{
    public class ArcaneMage : CombatRoutine
    {
        //Spell Strings
        private string RoP = "Rune of Power";
        private string AI = "Arcane Intellect";
        private string Counterspell = "Counterspell";
        private string IB = "Ice Block";
        private string MI = "Mirror Image";
        private string PB = "Prismatic Barrier";

        //Talents
        bool RuleofThrees => API.PlayerIsTalentSelected(1, 2);
        bool ArcaneFamiliar => API.PlayerIsTalentSelected(1, 3);
        bool Slipstream => API.PlayerIsTalentSelected(2, 3);
        bool RuneofPower => API.PlayerIsTalentSelected(3, 3);
        bool NetherTempest => API.PlayerIsTalentSelected(4, 3);
        bool ArcaneOrb => API.PlayerIsTalentSelected(6, 2);
        bool SuperNova => API.PlayerIsTalentSelected(6, 3);
        //CBProperties
        private int PBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(PB)];
        private int IBPercentProc => percentListProp[CombatRoutine.GetPropertyInt(IB)];
        private int MIPercentProc => percentListProp[CombatRoutine.GetPropertyInt(MI)];
        //General
        private int Level => API.PlayerLevel;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool InRange => API.TargetRange <= 40;
        private bool Burn => API.PlayerMana >= 10;
        private bool Conserve => API.PlayerMana <= 59 && API.SpellISOnCooldown("Evocation");
        private int Mana => API.PlayerMana;

        //private static readonly Stopwatch Burnphase = new Stopwatch();
        //private static readonly Stopwatch API.PlayerMana <= 59 && API.SpellISOnCooldown("Evocation")phase = new Stopwatch();

        public override void Initialize()
        {
            CombatRoutine.Name = "Arcane Mage by Ryu";
            API.WriteLog("Welcome to Arcane Mage v1.2 by Ryu989");
            API.WriteLog("Presence of Mind(PoM) will by default cast when Arcane Power as less than 3 seconds left, otherwise, you can check box in settings to cast on CD");
            API.WriteLog("All Talents expect Ice Ward, Ring of Frost, Supernova and Mirror Image are supported");
            API.WriteLog("Current Mana :" + API.PlayerMana);

            // API.WriteLog("Supported Essences are Memory of Lucids, Concerated Flame and Worldvein");
            //Buff

            CombatRoutine.AddBuff("Arcane Power");
            CombatRoutine.AddBuff("Clearcasting");
            CombatRoutine.AddBuff("Rule of Threes");
            CombatRoutine.AddBuff("Presence of Mind");
            CombatRoutine.AddBuff("Rune of Power");
            CombatRoutine.AddBuff("Prismatic Barrier");
            CombatRoutine.AddBuff("Arcane Intellect");
            CombatRoutine.AddBuff("Arcane Familiar");

            //Debuff
            CombatRoutine.AddDebuff("Nether Tempest");
            CombatRoutine.AddDebuff("Touch of the Magi");

            //Spell
            CombatRoutine.AddSpell("Rune of Power", "None");
            CombatRoutine.AddSpell("Ice Block");
            CombatRoutine.AddSpell("Arcane Power");
            CombatRoutine.AddSpell("Arcane Orb");
            CombatRoutine.AddSpell("Nether Tempest");
            CombatRoutine.AddSpell("Arcane Barrage");
            CombatRoutine.AddSpell("Arcane Explosion");
            CombatRoutine.AddSpell("Arcane Missiles");
            CombatRoutine.AddSpell("Arcane Blast", "C");
            CombatRoutine.AddSpell("Evocation", "C");
            CombatRoutine.AddSpell("Prismatic Barrier", "C");
            CombatRoutine.AddSpell("Mirror Image", "C");
            CombatRoutine.AddSpell("Presence of Mind", "None");
            CombatRoutine.AddSpell("Counterspell", "None");
            CombatRoutine.AddSpell("Arcane Familiar", "None");
            CombatRoutine.AddSpell("Mana Gem", "None");
            CombatRoutine.AddSpell("Touch of the Magi", "None");
            CombatRoutine.AddSpell("Arcane Intellect", "None");


            //Prop
            CombatRoutine.AddProp(PB, PB, percentListProp, "Life percent at which " + PB + " is used, set to 0 to disable", "Defense", 5);
            CombatRoutine.AddProp(IB, IB, percentListProp, "Life percent at which " + IB + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(MI, MI, percentListProp, "Life percent at which " + MI + " is used, set to 0 to disable", "Defense", 7);


        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted)
            {
                if (API.CanCast("Arcane Intellect") && Level >= 8 && !API.PlayerHasBuff("Arcane Intellect"))
                {
                    API.CastSpell("Arcane Intellect");
                    return;
                }
                if (API.CanCast("Prismatic Barrier") && Level >= 21 && !API.PlayerHasBuff("Prismatic Barrier") && API.PlayerHealthPercent <= PBPercentProc && API.PlayerHealthPercent != 0)
                {
                    API.CastSpell("Prismatic Barrier");
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (isInterrupt && API.CanCast("Counterspell") && Level >= 7)
            {
                API.CastSpell("Counterspell");
                return;
            }
            if (API.CanCast("Ice Block") && API.PlayerHealthPercent <= IBPercentProc && API.PlayerHealthPercent != 0 && Level >= 22)
            {
                API.CastSpell("Ice Block");
                return;
            }
            if (API.CanCast("Mirror Image") && API.PlayerHealthPercent <= MIPercentProc && API.PlayerHealthPercent != 0 && Level >= 44 && NotCasting && NotChanneling)
            {
                API.CastSpell("Mirror Image");
                return;
            }
            if (Level <= 60)
            {
                rotation();
                return;
            }
        }

        public override void OutOfCombatPulse()
        {

        }

        private void rotation()
        {
            if (API.CanCast("Arcane Power") && Level >= 29 && !API.PlayerIsMoving && API.TargetRange <= 40 && IsCooldowns && NotCasting && NotChanneling)
            {
                API.CastSpell("Arcane Power");
                return;
            }
            if (RuneofPower && API.CanCast("Rune of Power") && Burn && API.TargetRange <= 40 && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && IsCooldowns && NotCasting && NotChanneling)
            {
                API.CastSpell("Rune of Power");
                return;
            }
            if (API.CanCast("Touch of the Magi") && Level >= 33 && Burn && (API.PlayerCurrentArcaneCharges <= 0 || !API.TargetHasDebuff("Touch of the Magi")) && InRange && NotCasting && NotChanneling)
            {
                API.CastSpell("Touch of the Magi");
                API.WriteLog("Current Mana :" + API.PlayerMana);
                return;
            }
            if (API.CanCast("Arcane Orb") && InRange && ArcaneOrb && Burn && API.PlayerCurrentArcaneCharges <= 3 && (API.PlayerIsMoving || !API.PlayerIsMoving) && NotCasting && NotChanneling && API.SpellISOnCooldown("Touch of the Magi"))
            {
                API.CastSpell("Arcane Orb");
                return;
            }
            if (API.CanCast("Nether Tempest") && NotCasting && NotChanneling && InRange && NetherTempest && Burn && API.PlayerCurrentArcaneCharges == 4 && (!API.TargetHasDebuff("Nether Tempest") || API.TargetDebuffRemainingTime("Nether Tempest") <= 300) && !API.PlayerHasBuff("Arcane Power") && !API.PlayerHasBuff("Rune of Power") && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Nether Tempest");
                return;
            }
            if (API.CanCast("Presence of Mind") && NotCasting && NotChanneling && Level >= 42 && API.PlayerHasBuff("Arcane Power") && API.PlayerBuffTimeRemaining("Arcane Power") <= 200 && !API.PlayerHasBuff("Presence of Mind") && Burn)
            {
                API.CastSpell("Presence of Mind");
                return;
            }
            if (API.CanCast("Arcane Barrage") && NotCasting && NotChanneling && Level >= 10 && InRange && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 8 && API.PlayerCurrentArcaneCharges == 4 && Burn && !NetherTempest && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Explosion") && NotCasting && NotChanneling && Level >= 6 && InRange && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 8 && Burn && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Explosion");
                return;
            }
            if (API.CanCast("Arcane Missiles") && NotCasting && NotChanneling && Level >= 13 && InRange && API.PlayerHasBuff("Clearcasting") && !API.PlayerHasBuff("Arcane Power") && Mana < 95 && Burn && (API.PlayerIsMoving && Slipstream || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Missiles");
                return;
            }
            if (API.CanCast("Arcane Blast") && NotCasting && NotChanneling && Level >= 10 && InRange && Burn && !API.PlayerIsMoving)
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Arcane Blast") && NotCasting && NotChanneling && Level >= 10 && InRange && RuleofThrees && API.PlayerHasBuff("Rule of Threes") && Burn && !API.PlayerIsMoving)
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Evocation") && NotCasting && NotChanneling && Level >= 27 && Mana <= 10 && (API.PlayerIsMoving && Slipstream|| !API.PlayerIsMoving))
            {
                API.CastSpell("Evocation");
                return;
            }
            if (API.CanCast("Arcane Barrage") && NotCasting && NotChanneling && Level >= 10 && InRange && API.SpellISOnCooldown("Evocation") && API.PlayerCurrentArcaneCharges <= 4 && Mana <= 10 && !NetherTempest && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Orb") && NotCasting && NotChanneling && InRange && ArcaneOrb && Conserve && API.SpellISOnCooldown("Evocation") && API.PlayerCurrentArcaneCharges <= 3 && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Orb");
                return;
            }
            if (API.CanCast("Nether Tempest") && NotCasting && NotChanneling && InRange && NetherTempest && Conserve && API.SpellISOnCooldown("Evocation") && API.PlayerCurrentArcaneCharges == 4 && (!API.TargetHasDebuff("Nether Tempest") || (API.TargetDebuffRemainingTime("Nether Tempest") <= 300) && !API.PlayerHasBuff("Arcane Power") && !API.PlayerHasBuff("Rune of Power") && (API.PlayerIsMoving || !API.PlayerIsMoving)))
            {
                API.CastSpell("Nether Tempest");
                return;
            }
            if (API.CanCast("Arcane Blast") && NotCasting && NotChanneling && Level >= 10 && InRange && RuleofThrees && API.PlayerHasBuff("Rule of Threes") && Conserve && API.SpellISOnCooldown("Evocation") && !API.PlayerIsMoving)
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Arcane Missiles") && NotCasting && NotChanneling && Level >= 13 && InRange && Level >= 12 && API.PlayerHasBuff("Clearcasting") && !API.PlayerHasBuff("Arcane Power") && Mana < 95 && Conserve && API.SpellISOnCooldown("Evocation") && (API.PlayerIsMoving && Slipstream || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Missiles");
                return;
            }
            if (API.CanCast("Arcane Explosion") && NotCasting && NotChanneling && Level >= 6 && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 8 && API.PlayerHasBuff("Clearcasting") && Conserve && API.SpellISOnCooldown("Evocation") && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Explosion");
                return;
            }
            if (API.CanCast("Arcane Barrage") && NotCasting && NotChanneling && Level >= 10 && InRange && API.TargetUnitInRangeCount >= 3 && API.PlayerCurrentArcaneCharges == 4 && Conserve && API.SpellISOnCooldown("Evocation") && !NetherTempest && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Barrage");
                return;
            }
            if (API.CanCast("Arcane Explosion") && NotCasting && NotChanneling && Level >= 6 && API.TargetUnitInRangeCount >= 3 && API.TargetRange <= 8 && Conserve && API.SpellISOnCooldown("Evocation") && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Arcane Explosion");
                return;
            }
           // if (API.CanCast("Evocation") && NotCasting && NotChanneling && Level >= 27 && Conserve && !API.SpellISOnCooldown("Evocation") && (API.PlayerIsMoving && Slipstream || !API.PlayerIsMoving))
           // {
           //     API.CastSpell("Evocation");
           //     return;
           // }
            if (API.CanCast("Arcane Blast") && NotCasting && NotChanneling && Level >= 10 && InRange && Conserve && API.SpellISOnCooldown("Evocation") && !API.PlayerIsMoving)
            {
                API.CastSpell("Arcane Blast");
                return;
            }
            if (API.CanCast("Arcane Familiar") && NotCasting && NotChanneling && ArcaneFamiliar && !API.PlayerHasBuff("Arcane Familiar") && (Conserve && API.SpellISOnCooldown("Evocation") || Burn))
            {
                API.CastSpell("Arcane Familiar");
                return;
            }

        }

    }
}



