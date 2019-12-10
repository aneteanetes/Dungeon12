using Dungeon12.Abilities;
using Dungeon12.Abilities.Enums;
using Dungeon12.Abilities.Scaling;
using Dungeon12.Entities.Alive;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using Dungeon12.Noone.Talants.Defensible;
using System;
namespace Dungeon12.Noone.Abilities
{
    public class Defaura : Ability<Noone, DefensibleTalants>
    {
        public override double Spend => 0;

        public override int Position => 0;

        public override string Name => "Аура защитника";

        public override ScaleRate Scale => ScaleRate.Build(Dungeon12.Entities.Enums.Scale.AbilityPower, 0.1);

        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        protected override bool CanUse(Noone @class) => true;

        private DefauraBuf auraBuf = new DefauraBuf();

        private bool enabled = false;


        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class)
        {
            this.gameMap = gameMap;
            this.avatar = avatar;
            this.@class = @class;
            avatar.OnMove += AuraEffect;
        }

        GameMap gameMap;
        Avatar avatar;
        Noone @class;

        private void AuraEffect()
        {
            var rangeObject = new MapObject
            {
                Position = new Dungeon.Physics.PhysicalPosition
                {
                    X = avatar.Position.X - ((avatar.Size.Width * 4) / 2),
                    Y = avatar.Position.Y - ((avatar.Size.Height * 4) / 2)
                },
                Size = avatar.Size
            };

            rangeObject.Size.Height *= 4;
            rangeObject.Size.Width *= 4;

            var enemyNear = gameMap.Any<NPCMap>(rangeObject,n=>n.IsEnemy);
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
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Noone @class)
        {
            avatar.OnMove -= AuraEffect;
            gameMap = null;
            avatar = null;
            @class = null;
        }

        private class DefauraBuf : BasePerk
        {
            public override string Image => "Images.Abilities.Defaura.buf.png".NoonePath();

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