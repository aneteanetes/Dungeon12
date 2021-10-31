using Dungeon;
using Dungeon.Entities.Animations;
using Dungeon.Physics;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Base
{
    public abstract class MoveableSceneObject<T> : AnimatedSceneObject<T>
        where T : class, IGameComponent
    {
        protected Moveable moveable;
        protected GameMap location;

        protected MapObject mapObj;

        public MoveableSceneObject(T @object, Moveable moveable, Rectangle defaultFramePosition)
            : base(@object, @object.Name, defaultFramePosition)
        {
            //this.mapObj = mapObj;
            this.moveable = moveable;
            //this.location = location;

            if (moveable.Static)
            {
                RequestStop();
            }
        }

        protected int moveDistance = 0;
        protected Direction move;

        private bool inQueue = false;

        private DateTime lastQueue = DateTime.Now;

        protected override void DrawLoop()
        {
            if (moveable.Static)
                return;
        }

        /// <summary>
        /// Метод выполняющийся когда в фоновом потоке таймер выполняет рассчёт перемещения
        /// </summary>
        /// <returns>Флаг рассчёта движения или нет</returns>
        protected virtual bool OnLogic() => true;
        
        private Direction _dir = Direction.Idle;

        public override bool Updatable => true;

        protected bool AutoMove => true;

        public override void Update()
        {
            if (!Drawed)
                return;

            if (moveDistance > 0)
            {
                moveDistance--;
                Move(move);
            }
            else if (moveDistance < 0)
            {
                moveDistance++;
            }

            if (Math.Abs((lastQueue - DateTime.Now).TotalMilliseconds) >= 300)
            {
                inQueue = false;
            }

            if (moveDistance == 0 && !inQueue)
            {
                if (!OnLogic())
                {
                    inQueue = false;
                    return;
                }

                if (moveDistance != 0)
                    return;

                if (Dungeon.Random.Chance(moveable.WalkChance))
                {
                    move = Direction.Idle;

                    var direction = _dir.Rangom();

                    if (direction == lastClosedDirection)
                        return;

                    move = direction;

                    moveDistance = moveable.WalkDistance.Random();
                }
                inQueue = false;
            }
        }

        protected Direction DetectDirection(PhysicalObject target)
        {
#warning TODO
            return Direction.Idle;

            //var playerPos = target.Position;
            //var thisPos = @object.Position;

            //Direction dirX = Direction.Idle;
            //Direction dirY = Direction.Idle;

            //if (playerPos.X <= thisPos.X)
            //{
            //    dirX = Direction.Left;
            //}
            //if (playerPos.X >= thisPos.X)
            //{
            //    dirX = Direction.Right;
            //}

            //if (playerPos.Y >= thisPos.Y)
            //{
            //    dirY = Direction.Down;
            //}

            //if (playerPos.Y <= thisPos.Y)
            //{
            //    dirY = Direction.Up;
            //}

            //return (Direction)((int)dirX + (int)dirY);
        }

        private void CalculateMove()
        {
            if (!OnLogic())
            {
                inQueue = false;
                return;
            }

            //if (moveDistance != 0)
            //    return;

            if (Dungeon.Random.Chance(moveable.WalkChance))
            {
                move = Direction.Idle;

                var direction = _dir.Rangom();

                if (direction == lastClosedDirection)
                    return;

                move = direction;

                moveDistance = moveable.WalkDistance.Random();
            }
            inQueue = false;
        }

        protected override void AnimationLoop()
        {
            if (moveable.Static)
                return;
        }

        protected void Move(Direction dir)
        {
            var anim = MovementMap[dir];
            MoveByDirection(dir, mapObj.Location,mapObj.MovementSpeed);

            SetAnimation(anim(moveable));

            if (!CheckMoveAvailable(dir))
            {
                WhenMoveNotAvailable(dir);
            }
            else if (this.aliveTooltip != null)
            {
                this.aliveTooltip.Left = this.BoundPosition.X;
                this.aliveTooltip.Top = this.BoundPosition.Y - 0.8;
            }
        }

        protected virtual void WhenMoveNotAvailable(Direction dir)
        {
            moveDistance = -moveable.WaitTime;
            SetAnimation(moveable.Idle);
        }

        protected void MoveByDirection(Direction dir, Point p, double step)
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

        protected Direction lastClosedDirection;

        protected virtual bool CheckMoveAvailable(Direction direction)
        {
            var inMoveRegion = (moveable?.MoveRegion.IntersectsWith(this.mapObj) ?? false);

            if (inMoveRegion && this.location.Move(this.mapObj, direction))
            {
                this.Left = this.mapObj.Location.X;
                this.Top = this.mapObj.Location.Y;
                return true;
            }
            else
            {
                lastClosedDirection = direction;
                this.mapObj.Location.X = this.Left;
                this.mapObj.Location.Y = this.Top;
                return false;
            }
        }

        private Dictionary<Direction, Func<Moveable, Animation>> MovementMap => new Dictionary<Direction, Func<Moveable, Animation>>()
        {
            { Direction.Idle,x=>x.Idle },
            { Direction.Left,x=>x.MoveLeft },
            { Direction.Right,x=>x.MoveRight},
            { Direction.Down,x=>x.MoveDown },
            { Direction.Up,x=>x.MoveUp },
            { Direction.DownLeft,x=>x.MoveDown },
            { Direction.DownRight,x=>x.MoveDown },
            { Direction.UpLeft,x=>x.MoveUp },
            { Direction.UpRight,x=>x.MoveUp },
        };
    }
}
