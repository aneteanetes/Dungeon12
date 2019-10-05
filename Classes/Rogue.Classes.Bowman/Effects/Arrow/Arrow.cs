using Rogue.Control.Pointer;
using Rogue.Drawing.SceneObjects;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Physics;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Effects
{
    public class Arrow : AnimatedSceneObject<ArrowObject>
    {
        private double _fly;
        private GameMap _gameMap;

        public Arrow(GameMap gameMap, ArrowObject arrow, Direction dir) : base(null,arrow, "", new Rectangle()
        {
            Height = 32,
            Width = 32,
            X = 0,
            Y = dir == Direction.Down ? 0
                : (dir == Direction.Left ? 32
                    : (dir == Direction.Right ? 64
                        : dir == Direction.Up ? 96 : 0))
        })
        {
            _gameMap = gameMap;

            this.Width = 1;
            this.Height = 1;

            this.Image = @object.Image;
            SetAnimation(@object.Animation);
        }

        protected override void Action(MouseButton mouseButton)
        {
        }

        public override Rectangle ImageRegion => base.ImageRegion;

        protected override void DrawLoop()
        {
            if (@object.Range <= _fly)
            {
                RequestStop();
                return;
            }

            switch (@object.Direction)
            {
                case Direction.Up:
                    this.Top -= @object.Speed;
                    break;
                case Direction.Down:
                    this.Top += @object.Speed;
                    break;
                case Direction.Left:
                    this.Left -= @object.Speed;
                    break;
                case Direction.Right:
                    this.Left += @object.Speed;
                    break;
                default:
                    break;
            }

            _fly += @object.Speed;
        }

        protected override void AnimationLoop()
        {
            var rangeObject = new MapObject
            {
                Position = new Physics.PhysicalPosition
                {
                    X = (this.Left * 32) + 10,
                    Y = (this.Top * 32) + 10
                },
                Size = new PhysicalSize()
                {
                    Height = 16,
                    Width = 16
                }
            };

            var target = _gameMap.One<Mob>(rangeObject);
            if (target != default)
            {
                target.Flow(t => t.Damage(true), new { @object.Damage });
                RequestStop();
            }
        }

        protected override void OnAnimationStop()
        {
            this.Destroy?.Invoke();
        }
    }
}
