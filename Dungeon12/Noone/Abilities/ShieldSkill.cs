using Dungeon12.Abilities;
using Dungeon12.Abilities.Enums;
using Dungeon12.Abilities.Scaling;
using Dungeon12.Noone.Talants;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using Dungeon.Transactions;
using System;using Dungeon;using Dungeon.Drawing.SceneObjects;

namespace Dungeon12.Noone.Abilities
{
    public class ShieldSkill : Ability<Noone, AbsorbingTalants>
    {
        public override bool Hold => false;

        public override double Spend => 3;

        public override int Position => 1;
        
        public override long Value => 1;

        public override string Name => "Навык щита";

        public override ScaleRate<Noone> Scale => new ScaleRate<Noone>(x => x.Armor * 2.5d);

        public override AbilityPosition AbilityPosition => AbilityPosition.Q;

        protected override bool CanUse(Noone @class)
        {
            var resources = @class.Actions >= 2;
            var timer = Dungeon12.Global.Time.Timer(nameof(ShieldSkill)).IsAlive;

            return resources && !timer;
        }
        
        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class)
        {
            @class.Actions -= 2;
            var barrierBuff = new BarrierBuff(ScaledValue(@class));
            avatar.AddState(barrierBuff);

            Dungeon12.Global.Time
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
            private readonly long value;

            public BarrierBuff(long value) => this.value = value;

            public override string Image => "Images.Abilities.ShieldSkill.buf.png".NoonePath();

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
