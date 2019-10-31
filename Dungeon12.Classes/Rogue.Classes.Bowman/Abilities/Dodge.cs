using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Types;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Bowman.Abilities
{
    public class Dodge : Ability<Bowman>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        public override Cooldown Cooldown { get; } = Cooldown.Make(10000, nameof(Dodge));

        public override string Name => "Увернуться";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.Special;

        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Bowman @class) => true; //cooldown checking

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            var direction = avatar.VisionDirection.Opposite();
            var move = MoveAvatar(avatar)(direction);

            move(false);

            var plusSpeed = @class.AtackSpeed * 0.5;
            avatar.MovementSpeed += plusSpeed;

            var posX = avatar.Position.X;
            var posY = avatar.Position.Y;

            // Это работает и сделано так потому что остановка персонажа влияет на камеру
            // поэтому после того как движение закончится мы вернём нормальную скорость
            // и только после этого подвинется камера
            void SpeedEffect(Direction dir)
            {
                avatar.MovementSpeed -= plusSpeed;
                avatar.OnMoveStop -= SpeedEffect;

                var camera = Global.DrawClient as ICamera;
                var x = camera.CameraOffsetX;
                var y = camera.CameraOffsetY;

                var posXnew = avatar.Position.X;
                var posYnew = avatar.Position.Y;

                switch (dir)
                {
                    case Direction.Up:
                        y += posY - posYnew;
                        break;
                    case Direction.Down:
                        y -= posYnew - posY;
                        break;
                    case Direction.Left:
                        x += posX - posXnew;
                        break;
                    case Direction.Right:
                        x -= posXnew - posX;
                        break;
                    default: break;
                }

                camera.SetCamera(x, y);
            }

            avatar.OnMoveStop += SpeedEffect;

            Global.Time.Timer(nameof(Dodge) + nameof(Use))
                .After(10 + (plusSpeed * 5))
                .Do(() => move(true))
                .Auto();
        }

        private static Func<Direction, Action<bool>> MoveAvatar(Avatar avatar) => dir => remove => 
            avatar.Flow(a => a.MoveStep(true),
                new {
                    Direction = dir,
                    Remove = remove,
                    BlockMoveInput = true,
                    CameraAffect=false
                });
    }
}