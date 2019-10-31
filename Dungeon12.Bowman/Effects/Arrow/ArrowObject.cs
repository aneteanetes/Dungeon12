using Dungeon;
using Dungeon.Classes;
using Dungeon.Entites.Animations;
using Dungeon.Physics;
using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12.Classes.Bowman.Effects
{
    public class ArrowObject : PhysicalObject
    {
        public Direction Direction { get; }

        public double Range { get;}

        public long Damage { get; }

        public double Speed { get; }

        public ArrowObject(Direction dir, double range, long dmg, double speed)
        {
            Direction = dir;
            Range = range;
            Damage = dmg;
            Speed = speed;
        }

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

        public string Image => @"Effects\Arrow.png".ImgPath();

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