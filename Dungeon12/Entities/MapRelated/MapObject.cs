using Dungeon.Physics;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon12.Entities.MapRelated
{
    public class MapObject : PhysicalObject<MapObject>
    {
        protected override MapObject Self => this;

        public virtual double Speed { get; set; } = 1.5;

        public bool Move(Direction dir)
        {
            return MoveByDirection(dir, this.Position, Speed);
        }

        public void MoveThrough(Direction dir)
        {
            MoveByDirection(dir, this.Position, Speed);
        }

        protected Direction lastClosedDirection;

        bool CheckMoveAvailable(Direction dir)
        {
            return true;
        }

        protected bool MoveByDirection(Direction dir, PhysicalPosition p, double step)
        {
            if (!CheckMoveAvailable(dir))
                return false;

            switch (dir)
            {
                case Direction.Idle:
                    break;
                case Direction.Up:
                    p.Y -= step;
                    break;
                case Direction.Down:
                    p.Y += step;
                    break;
                case Direction.Left:
                    p.X -= step;
                    break;
                case Direction.Right:
                    p.X += step;
                    break;
                case Direction.UpLeft:
                    p.Y -= step;
                    p.X -= step;
                    break;
                case Direction.UpRight:
                    p.Y -= step;
                    p.X += step;
                    break;
                case Direction.DownLeft:
                    p.Y += step;
                    p.X -= step;
                    break;
                case Direction.DownRight:
                    p.Y += step;
                    p.X += step;
                    break;
                default:
                    break;
            }

            return true;
        }

        protected void MoveByDirection(Direction dir, Rectangle p, double step)
        {
            switch (dir)
            {
                case Direction.Idle:
                    break;
                case Direction.Up:
                    p.Y -= step;
                    break;
                case Direction.Down:
                    p.Y += step;
                    break;
                case Direction.Left:
                    p.X -= step;
                    break;
                case Direction.Right:
                    p.X += step;
                    break;
                case Direction.UpLeft:
                    p.Y -= step;
                    p.X -= step;
                    break;
                case Direction.UpRight:
                    p.Y -= step;
                    p.X += step;
                    break;
                case Direction.DownLeft:
                    p.Y += step;
                    p.X -= step;
                    break;
                case Direction.DownRight:
                    p.Y += step;
                    p.X += step;
                    break;
                default:
                    break;
            }
        }

        protected void MoveByDirection(Direction dir, ISceneObject p, double step)
        {
            switch (dir)
            {
                case Direction.Idle:
                    break;
                case Direction.Up:
                    p.Top -= step;
                    break;
                case Direction.Down:
                    p.Top += step;
                    break;
                case Direction.Left:
                    p.Left -= step;
                    break;
                case Direction.Right:
                    p.Left += step;
                    break;
                case Direction.UpLeft:
                    p.Top -= step;
                    p.Left -= step;
                    break;
                case Direction.UpRight:
                    p.Top -= step;
                    p.Left += step;
                    break;
                case Direction.DownLeft:
                    p.Top += step;
                    p.Left -= step;
                    break;
                case Direction.DownRight:
                    p.Top += step;
                    p.Left += step;
                    break;
                default:
                    break;
            }
        }
    }
}
