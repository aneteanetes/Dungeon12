using Dungeon12.Abilities;
using Dungeon12.Abilities.Enums;
using Dungeon12.Abilities.Scaling;
using Dungeon12.Entities.Alive;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using Dungeon12.Noone.Talants;
using System;
namespace Dungeon12.Noone.Abilities
{
    public class Defaura : Ability<Noone, AbsorbingTalants>
    {
        public override double Spend => 0;

        public override int Position => 0;

        public override string Name => "Аура защитника";

        public override ScaleRate<Noone> Scale => new ScaleRate<Noone>(x => x.Armor * 1.1, x => x.Defence * 0.5, x => x.Barrier * 0.25);

        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        protected override bool CanUse(Noone @class) => true;

        private DefauraBuf auraBuf;

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

        public override long Value => 1 * Global.GameState.Character.Level;

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
            var value = ScaledValue(@class, Value);
            auraBuf = new DefauraBuf();
            if (enemyNear != enabled)
            {
                if (enemyNear)
                {
                    DefauraBuf.value = value;
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

            public static long value;

            public void Apply(Avatar avatar)
            {
                avatar.Character.Defence += value;
                avatar.Character.Barrier += value;
            }

            public void Discard(Avatar avatar)
            {
                avatar.Character.Defence -= value;
                avatar.Character.Barrier -= value;
            }
            public void Apply(Noone @class)
            {
                @class.Block += value;
                @class.Parry += value;
            }

            public void Discard(Noone @class)
            {
                @class.Block -= value;
                @class.Parry -= value;
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