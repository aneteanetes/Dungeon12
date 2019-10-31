namespace Dungeon12.Classes.Noone.Abilities
{
    using Dungeon.Abilities;
    using Dungeon.Abilities.Enums;
    using Dungeon.Abilities.Scaling;
    using Dungeon.Classes.Noone.Talants.Defensible;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.Transactions;
    using System;

    public class Defstand : Ability<Noone, DefensibleTalants>
    {
        public override bool Hold => true;

        public override double Spend => 2;

        public override int Position => 0;

        public override string Name => "Защитная стойка";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);

        public override AbilityPosition AbilityPosition => AbilityPosition.Right;

        protected override bool CanUse(Noone @class)=> @class.Actions >= 2;

        private ArmorBuf holdedBuf = null;

        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class)
        {
            @class.Actions -= 2;
            holdedBuf = new ArmorBuf();
            avatar.AddState(holdedBuf);
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Noone @class)
        {
            if (holdedBuf != null)
            {
                avatar.RemoveState(holdedBuf);
                holdedBuf = null;
            }
        }

        /// <summary>
        /// так то бафы должны действовать и на аватар тоже
        /// </summary>
        private class ArmorBuf : Applicable
        {
            public override string Image => "Rogue.Classes.Noone.Images.Abilities.Defstand.buf.png";

            public void Apply(Avatar avatar)
            {
                avatar.Character.Defence += 5;
                avatar.MovementSpeed -= 0.03;
            }

            public void Discard(Avatar avatar)
            {
                avatar.Character.Defence -= 5;
                avatar.MovementSpeed += 0.03;
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

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.Hold;

        public override AbilityCastType CastType => AbilityCastType.Active;

        public override Location CastLocation => Location.Combat;

        public override AbilityTargetType TargetType => AbilityTargetType.SelfTarget;

        public override string Description => $"Позволяет укрыться за щитом.{Environment.NewLine}Уменьшает урон на кол-во блока{Environment.NewLine}но уменьшает скорость.";
                                            //$"Атакует врага нанося двойной урон {Environment.NewLine} оружием в правой руке. {Environment.NewLine} Может наносить критический урон.";

    }
}