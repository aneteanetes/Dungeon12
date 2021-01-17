using Dungeon;
using Dungeon.Entities.Animations;
using Dungeon.Physics;
using Dungeon.Types;
using Dungeon12.Classes;
using System.Linq;

namespace Dungeon12.Bowman.Effects
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
                return new BaseAnimationMap(@"Effects\Arrow.png".AsmImg())
                {
                    Direction = Direction,
                    Frames = Enumerable.Range(0, 4).Select(x => GetPoint(x)).ToList()
                };
            }
        }

        public override string Image => @"Effects\Arrow.png".AsmImg();
        
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