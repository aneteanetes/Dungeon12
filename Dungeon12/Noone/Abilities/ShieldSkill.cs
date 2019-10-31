using Dungeon.Abilities;
using Dungeon.Abilities.Enums;
using Dungeon.Abilities.Scaling;
using Dungeon.Classes.Noone.Talants;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.Transactions;
using System;

namespace Dungeon12.Classes.Noone.Abilities
{
    public class ShieldSkill : Ability<Noone, AbsorbingTalants>
    {
        public override bool Hold => false;

        public override double Spend => 3;

        public override int Position => 1;

        public ShieldSkill()
        {
            Level = 1;
        }

        public override double Value => 10;

        public override string Name => "Навык щита";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.None, 0.1);

        public override AbilityPosition AbilityPosition => AbilityPosition.Q;

        protected override bool CanUse(Noone @class)
        {
            var resources = @class.Actions >= 2;
            var timer = Global.Time.Timer(nameof(ShieldSkill)).IsAlive;

            return resources && !timer;
        }
        
        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class)
        {
            @class.Actions -= 2;
            var barrierBuff = new BarrierBuff(this.Level);
            avatar.AddState(barrierBuff);

            Global.Time
                .Timer(nameof(ShieldSkill))
                .After(this.Level * 1500)
                .Do(() => avatar.RemoveState(barrierBuff))
                .Auto();
        }

        /// <summary>
        /// так то бафы должны действовать и на аватар тоже
        /// </summary>
        private class BarrierBuff : Applicable
        {
            private readonly int value;

            public BarrierBuff(int value) => this.value = value;

            public override string Image => "Rogue.Classes.Noone.Images.Abilities.ShieldSkill.buf.png";

            public void Apply(Avatar avatar)
            {
                avatar.Character.Barrier += value;
            }

            public void Discard(Avatar avatar)
            {
                avatar.Character.Barrier -= value;
            }

            protected override void CallApply(dynamic obj)
            {
                this.Apply(obj);
            }

            protected override void CallDiscard(dynamic obj)
            {
                this.Discard(obj);
            }
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Noone @class)
        {
            //TODO elapsed buf
        }

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.EffectOfTime;

        public override AbilityCastType CastType => AbilityCastType.Active;

        public override Location CastLocation => Location.Alltime;

        public override AbilityTargetType TargetType => AbilityTargetType.SelfTarget;

        public override string Description => $"Позволяет активировать навык щита.{Environment.NewLine}Пока действует способность вы {Environment.NewLine}получаете текущий активный эффект.";
        //$"Атакует врага нанося двойной урон {Environment.NewLine} оружием в правой руке. {Environment.NewLine} Может наносить критический урон.";

    }
}
