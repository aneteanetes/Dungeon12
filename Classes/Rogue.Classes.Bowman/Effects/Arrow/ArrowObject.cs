using Rogue.Entites.Animations;
using Rogue.Physics;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.Classes.Bowman.Effects
{
    public class ArrowObject : PhysicalObject
    {
        public Direction Direction { get; }

        public ArrowObject(Direction dir) => Direction = dir;

        public AnimationMap Animation
        {
            get
            {
                return new BaseAnimationMap(@"Effects\Arrow.png".ImgPath())
                {
                    Direction = Direction,
                    Frames = Enumerable.Range(0, 4).Select(x => GetPoint(x)).ToList()
                };
            }
        }

        private Point GetPoint(int frame)
        {
            Point pos = new Point();

            switch (Direction)
            {
                case Direction.Up:
                    pos.Y = 3;
                    break;
                case Direction.Down:
                    pos.Y = 0;
                    break;
                case Direction.Left:
                    pos.Y = 1;
                    break;
                case Direction.Right:
                    pos.Y = 2;
                    break;
                default:
                    break;
            }

            pos.Y *= 32;
            pos.X = frame * 32;

            return pos;
        }
    }
}