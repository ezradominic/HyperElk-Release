using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class AfflictionWarlock : CombatRoutine
    {
        //Spells,Buffs,Debuffs
        private string Fireball = "Fireball";
        private string Fireblast = "Fire blast";
        private string Pyroblast = "Pyroblast";
        private string Flamestrike = "Flamestrike";
        private string PhoenixFlames = "Phoenix Flames";
        private string Combustion = "Combustion";
        private string HotSreak = "Hot Streak!";
        private string BlazingBarrier = "Blazing Barrier";
        private string Scorch = "Scorch";
        private string DragonsBreath = "Dragon's Breath";
        private string HeatingUp  = "Heating Up";
        private string ArcaneIntellect = "Arcane Intellect";
        private string ArcaneExplosion = "Arcane Explosion";
        private string MirrorImage = "Mirror Image";


        //Talents

        //Misc
        private bool IsRange => API.TargetRange < 40;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");




        //CBProperties
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");


        public override void Initialize()
        {
            CombatRoutine.Name = "Fire Mage @Mufflon12";
            API.WriteLog("Welcome to Fire Mage rotation @ Mufflon12");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("Use /cast [@cursor] Flamestrike macro");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");

            //Options


            //Spells
            CombatRoutine.AddSpell(Fireball, "D1");
            CombatRoutine.AddSpell(Fireblast, "D2");
            CombatRoutine.AddSpell(Pyroblast, "D3");
            CombatRoutine.AddSpell(DragonsBreath, "D4");

            CombatRoutine.AddSpell(Scorch, "D5");




            CombatRoutine.AddSpell(BlazingBarrier, "D9");



            CombatRoutine.AddSpell(ArcaneExplosion, "NumPad3");
            CombatRoutine.AddSpell(Flamestrike, "NumPad4");
            CombatRoutine.AddSpell(PhoenixFlames, "NumPad5");
            CombatRoutine.AddSpell(Combustion, "NumPad6");
            CombatRoutine.AddSpell(MirrorImage, "NumPad7");


            CombatRoutine.AddSpell(ArcaneIntellect, "NumPad9");



            //Buffs
            CombatRoutine.AddBuff(HotSreak);
            CombatRoutine.AddBuff(BlazingBarrier);
            CombatRoutine.AddBuff(Combustion);
            CombatRoutine.AddBuff(HeatingUp);
            CombatRoutine.AddBuff(ArcaneIntellect);


            //Debuffs


            //Debuffs


        }

        public override void Pulse()
        {

        }

        public override void CombatPulse()
        {
            //Fireblast on HeatingUp
            if (API.CanCast(Fireblast) && API.SpellCharges(Fireblast) >= 1 && !API.PlayerHasBuff(HeatingUp) && NotMoving && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(Fireblast);
                return;
            }
            //PhoenixFlames on Heating up
            if (API.CanCast(PhoenixFlames) && API.SpellCharges(PhoenixFlames) >= 1 && IsRange && API.PlayerHasBuff(HeatingUp) && API.PlayerHasBuff(Combustion))
            {
                API.CastSpell(PhoenixFlames);
                return;
            }
            //Pyroblast on HotSreak
            if (API.CanCast(Pyroblast) && API.PlayerHasBuff(HotSreak) && NotCasting && IsRange && NotChanneling)
            {
                API.CastSpell(Pyroblast);
                return;
            }
            //BlazingBarrier
            if (API.CanCast(BlazingBarrier) && !API.PlayerHasBuff(BlazingBarrier) && NotCasting && NotChanneling)
            {
                API.CastSpell(BlazingBarrier);
                return;
            }
            //Combustion
            if (IsCooldowns && API.CanCast(Combustion))
            {
                API.CastSpell(Combustion);
                return;
            }
            //MirrorImage
            if (IsCooldowns && API.CanCast(MirrorImage))
            {
                API.CastSpell(MirrorImage);
                return;
            }
            //Scorch on Combustion
            if (API.PlayerHasBuff(Combustion) && IsRange && API.CanCast(Scorch))
            {
                API.CastSpell(Scorch);
                return;
            }
            //Flamestrike
            if (IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber && API.CanCast(Flamestrike) && IsRange && API.TargetRange > 10 && API.PlayerHasBuff(HotSreak))
            {
                API.CastSpell(Flamestrike);
                return;
            }
            //DragonsBreath
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.CanCast(DragonsBreath) && API.TargetRange <= 10)
            {
                API.CastSpell(DragonsBreath);
                return;
            }
            //ArcaneExplosion
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.CanCast(ArcaneExplosion) && API.TargetRange <= 10)
            {
                API.CastSpell(ArcaneExplosion);
                return;
            }
            //Fireball
            if (API.CanCast(Fireball) && !API.PlayerHasBuff(HotSreak) && NotMoving && NotCasting && IsRange && NotChanneling && PlayerLevel >= 1)
            {
                API.CastSpell(Fireball);
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            //ArcaneIntellect
            if (API.CanCast(ArcaneIntellect) && !API.PlayerHasBuff(ArcaneIntellect))
            {
                API.CastSpell(ArcaneIntellect);
                return;
            }
        }
    }
}
