using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Classes.Noone.Talants.Defensible;
using Rogue.Entites.Alive;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Transactions;
using System;

namespace Rogue.Classes.Noone.Abilities
{
    public class Defaura : Ability<Noone, DefensibleTalants>
    {
        public override double Spend => 0;

        public override int Position => 0;

        public override string Name => "Аура защитника";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);

        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        protected override bool CanUse(Noone @class) => true;

        private DefauraBuf auraBuf = new DefauraBuf();

        private bool enabled = false;


        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class) => avatar.OnMove += () =>
        {
            var rangeObject = new MapObject
            {
                Position = new Physics.PhysicalPosition
                {
                    X = avatar.Position.X - ((avatar.Size.Width * 4) / 2),
                    Y = avatar.Position.Y - ((avatar.Size.Height * 4) / 2)
                },
                Size = avatar.Size
            };

            rangeObject.Size.Height *= 4;
            rangeObject.Size.Width *= 4;

            var enemyNear = gameMap.Any<Mob>(rangeObject);
            this.PassiveWorking = enemyNear;
            if (enemyNear != enabled)
            {
                if (enemyNear)
                {
                    @class.Add<DefauraBuf>();
                    avatar.AddState(auraBuf);
                }
                else
                {
                    @class.Remove<DefauraBuf>();
                    avatar.RemoveState(auraBuf);
                }
                enabled = enemyNear;
            }
        };

        protected override void Dispose(GameMap gameMap, Avatar avatar, Noone @class) { }

        /// <summary>
        /// так то бафы должны действовать и на аватар тоже
        /// </summary>
        private class DefauraBuf : BasePerk
        {
            public override string Image => "Rogue.Classes.Noone.Images.Abilities.Defaura.buf.png";

            public override bool ClassDependent => true;

            public void Apply(Avatar avatar)
            {
                avatar.Character.Defence += 2;
                avatar.Character.Barrier += 2;
            }

            public void Discard(Avatar avatar)
            {
                avatar.Character.Defence -= 2;
                avatar.Character.Barrier -= 2;
            }
            public void Apply(Noone @class)
            {
                @class.Block += 10;
                @class.Parry += 10;
            }

            public void Discard(Noone @class)
            {
                @class.Block -= 10;
                @class.Parry -= 10;
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

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.Passive;

        public override AbilityCastType CastType => AbilityCastType.Passive;

        public override Location CastLocation => Location.OnlyCombat;

        public override AbilityTargetType TargetType => AbilityTargetType.SelfTarget;

        public override string Description => $"Аура защищающая во время боя.{Environment.NewLine}Активируется если рядом есть враги{Environment.NewLine}увеличивает защиту вам и союзникам.";

    }
}