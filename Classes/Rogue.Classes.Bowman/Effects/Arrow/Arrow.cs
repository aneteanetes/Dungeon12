using Rogue.Control.Pointer;
using Rogue.Drawing.SceneObjects;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Physics;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Effects
{
    public class Arrow : AnimatedSceneObject<ArrowObject>
    {
        private double _range;
        private double _fly;
        private double _speed;

        public Arrow(PlayerSceneObject playerSceneObject,double range,Direction dir, double speed=0.06) : base(playerSceneObject, new ArrowObject(dir), "", new Rectangle()
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
            _speed = speed;
            _range = range;
            SetAnimation(@object.Animation);
        }

        protected override void Action(MouseButton mouseButton)
        {
        }

        protected override void DrawLoop()
        {
            if(_range<=_fly)
            {
                RequestStop();
            }

            switch (@object.Direction)
            {
                case Direction.Up:
                    this.Top -= _speed;
                    break;
                case Direction.Down:
                    this.Top += _speed;
                    break;
                case Direction.Left:
                    this.Left -= _speed;
                    break;
                case Direction.Right:
                    this.Left += _speed;
                    break;
                default:
                    break;
            }

            _fly += _speed;
        }

        protected override void StopAction()
        {
            this.Destroy?.Invoke();
        }
    }
}
