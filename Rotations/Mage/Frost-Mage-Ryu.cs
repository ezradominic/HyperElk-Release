
namespace HyperElk.Core
{
    public class FrostMage : CombatRoutine
    {
        //Spell Strings
        private string Flurry = "Flurry";

        //Talents
        bool LonelyWinter => API.PlayerIsTalentSelected(1, 2);
        bool IceNova => API.PlayerIsTalentSelected(1, 3);
        bool IceFloes => API.PlayerIsTalentSelected(2, 3);
        bool RuneOfPower => API.PlayerIsTalentSelected(3, 3);
        bool Ebonbolt => API.PlayerIsTalentSelected(4, 3);
        bool Cometstorm => API.PlayerIsTalentSelected(6, 3);
        bool RayofFrost => API.PlayerIsTalentSelected(7, 2);
        bool GlacialSpike => API.PlayerIsTalentSelected(7, 3);

        //CBProperties

        //General
        bool CastFlurry => API.PlayerLastSpell == Flurry;
        private int Level => API.PlayerLevel;


        public override void Initialize()
        {
            CombatRoutine.Name = "Frost Mage by Ryu";
            API.WriteLog("Welcome to Frost Mage by Ryu");
            API.WriteLog("Create the following cursor macro for Blizzard");
            API.WriteLog("Blizzard -- /cast [@cursor] Blizzard");
            API.WriteLog("Create Macro /cast [@Player] Arcane Intellect to buff Arcane Intellect so you don't require a target");
            API.WriteLog("All Talents expect Ring of Frost supported. All Cooldowns are associated with Cooldown toggle.");
            //Buff

            CombatRoutine.AddBuff("Icicles");
            CombatRoutine.AddBuff("Brain Freeze");
            CombatRoutine.AddBuff("Fingers of Frost");
            CombatRoutine.AddBuff("Ice Floes");
            CombatRoutine.AddBuff("Rune of Power");
            CombatRoutine.AddBuff("Ice Barrier");
            CombatRoutine.AddBuff("Arcane Intellect");

            //Debuff
            CombatRoutine.AddDebuff("Winter's Chill");

            //Spell
            CombatRoutine.AddSpell("Rune of Power", "None");
            CombatRoutine.AddSpell("Ice Block");
            CombatRoutine.AddSpell("Frostbolt");
            CombatRoutine.AddSpell("Ice Lance");
            CombatRoutine.AddSpell("Flurry");
            CombatRoutine.AddSpell("Cone of Cold");
            CombatRoutine.AddSpell("Ebonbolt");
            CombatRoutine.AddSpell("Blizzard");
            CombatRoutine.AddSpell("Glacial Spike", "C");
            CombatRoutine.AddSpell("Icy Veins", "C");
            CombatRoutine.AddSpell("Ice Barrier", "C");
            CombatRoutine.AddSpell("Mirror Image", "C");
            CombatRoutine.AddSpell("Ice Nova", "None");
            CombatRoutine.AddSpell("Frozen Orb", "None");
            CombatRoutine.AddSpell("Comet Storm", "None");
            CombatRoutine.AddSpell("Ray of Frost", "None");
            CombatRoutine.AddSpell("Freeze", "None");
            CombatRoutine.AddSpell("Water Elemental", "None");
            CombatRoutine.AddSpell("Ice Floes", "None");
            CombatRoutine.AddSpell("Arcane Intellect", "None");


            //Prop



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
                if (API.CanCast("Ice Barrier") && Level >= 21 && !API.PlayerHasBuff("Ice Barrier") && API.PlayerHealthPercent != 0)
                {
                    API.CastSpell("Ice Barrier");
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.CanCast("Ice Block") && API.PlayerHealthPercent <= 10 && API.PlayerHealthPercent != 0 && Level >= 22)
            {
                API.CastSpell("Ice Block");
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
            if (IsCooldowns && API.CanCast("Mirror Image"))
            {
                API.CastSpell("Mirror Image");
                return;
            }
            if (API.CanCast("Icy Veins") && Level >= 29 && !API.PlayerIsMoving && API.TargetRange <= 40 && IsCooldowns)
            {
                API.CastSpell("Icy Veins");
                return;
            }
            if (RuneOfPower && API.CanCast("Rune of Power") && API.TargetRange <= 40 && !API.PlayerHasBuff("Rune of Power") && !API.PlayerIsMoving && IsCooldowns)
            {
                API.CastSpell("Rune of Power");
                return;
            }
            if (API.CanCast("Ice Floes") && IceFloes && API.PlayerIsMoving && !API.PlayerHasBuff("Ice Floes"))
            {
                API.CastSpell("Ice Floes");
                return;
            }
            if (!API.PlayerHasPet && !LonelyWinter && API.CanCast("Water Elemental") && !API.PlayerIsMoving && Level >= 12)
            {
                API.CastSpell("Water Elemental");
                return;
            }
            if (API.CanCast("Flurry") && Level >= 19 && API.PlayerHasBuff("Brain Freeze") && API.TargetRange <= 40)
            {
                API.CastSpell("Flurry");
                return;
            }
            if (API.CanCast("Frozen Orb") && Level >= 38 && API.TargetRange <= 40)
            {
                API.CastSpell("Frozen Orb");
                return;
            }
            if (API.CanCast("Blizzard") && Level >= 14 && API.TargetRange <= 40 && !API.PlayerIsMoving && API.TargetUnitInRangeCount >= 3)
            {
                API.CastSpell("Blizzard");
                return;
            }
            if (API.CanCast("Blizzard") && Level >= 14 && API.TargetRange <= 40 && API.PlayerHasBuff("Ice Floes") && API.PlayerIsMoving && API.TargetUnitInRangeCount >= 3)
            {
                API.CastSpell("Blizzard");
                return;
            }
            if (Cometstorm && API.CanCast("Comet Storm") && API.TargetRange <= 40 && !API.PlayerIsMoving)
            {
                API.CastSpell("Comet Storm");
                return;
            }
            if (IceNova && API.CanCast("Ice Nova") && API.TargetRange <= 40 && (API.PlayerIsMoving || !API.PlayerIsMoving))
            {
                API.CastSpell("Ice Nova");
                return;
            }
            if (API.CanCast("Cone of Cold") && Level >= 18 && API.TargetRange <= 10 && API.TargetUnitInRangeCount >= 5)
            {
                API.CastSpell("Cone of Cold");
                return;
            }
            if (Ebonbolt && API.CanCast("Ebonbolt") && API.TargetRange <= 40 && !API.PlayerHasBuff("Brain Freeze") && API.PlayerBuffStacks("Icicles") > 4 && !API.PlayerIsMoving)
            {
                API.CastSpell("Ebonbolt");
                return;
            }
            if (Ebonbolt && API.CanCast("Ebonbolt") && API.TargetRange <= 40 && !API.PlayerHasBuff("Brain Freeze") && API.PlayerBuffStacks("Icicles") > 4 && API.PlayerHasBuff("Ice Floes") && API.PlayerIsMoving)
            {
                API.CastSpell("Ebonbolt");
                return;
            }
            if (RayofFrost && API.CanCast("Ray of Frost") && API.TargetHasDebuff("Winter's Chill") && API.PlayerHasBuff("Ice Floes"))
            {
                API.CastSpell("Ray of Frost");
                return;
            }
            if (GlacialSpike && API.CanCast("Glacial Spike") && API.TargetHasDebuff("Winter's Chill") && API.TargetRange <= 40 && API.PlayerBuffStacks("Icicles") > 4 && !API.PlayerIsMoving)
            {
                API.CastSpell("Glacial Spike");
                return;
            }
            if (GlacialSpike && API.CanCast("Glacial Spike") && API.TargetHasDebuff("Winter's Chill") && API.TargetRange <= 40 && API.PlayerBuffStacks("Icicles") > 4 && API.PlayerHasBuff("Ice Floes") && API.PlayerIsMoving)
            {
                API.CastSpell("Glacial Spike");
                return;
            }
            if (API.CanCast("Ice Lance") && Level >= 10 && API.TargetRange <= 40 && (API.PlayerHasBuff("Fingers of Frost") || API.TargetHasDebuff("Winters Chill")))
            {
                API.CastSpell("Ice Lance");
                return;
            }
            if (API.CanCast("Frostbolt") && Level >= 1 && API.TargetRange <= 40 && !API.PlayerIsMoving)
            {
                API.CastSpell("Frostbolt");
                return;
            }
            if (API.CanCast("Frostbolt") && Level >= 1 && API.TargetRange <= 40 && API.PlayerIsMoving && API.PlayerHasBuff("Ice Floes"))
            {
                API.CastSpell("Frostbolt");
                return;
            }
            if (API.PlayerIsMoving && API.CanCast("Ice Lance") && Level >= 10 && !API.PlayerHasBuff("Ice Floes") && API.TargetRange <= 40)
            {
                API.CastSpell("Ice Lance");
                return;
            }

        }

    }
}



