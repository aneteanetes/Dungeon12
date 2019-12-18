namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon;
    using Dungeon.Entities.Animations;
    using Dungeon.Types;
    using Dungeon12.Entities.Alive;
    using Dungeon12.Map;
    using System;
    using System.Collections.Generic;

    public abstract class MoveableSceneObject<T> : AnimatedSceneObject<T>
        where T : Dungeon.Physics.PhysicalObject
    {
        protected Moveable moveable;
        protected GameMap location;

        protected MapObject mapObj;

        public MoveableSceneObject(PlayerSceneObject playerSceneObject, T @object, GameMap location, MapObject mapObj, Moveable moveable, Rectangle defaultFramePosition)
            : base(playerSceneObject, @object, mapObj.Name, defaultFramePosition)
        {
            this.mapObj = mapObj;
            this.moveable = moveable;
            this.location = location;

            if (moveable.Static)
            {
                RequestStop();
            }
        }

        protected int moveDistance = 0;
        protected Direction move;

        private bool inQueue = false;

        protected override void DrawLoop()
        {
            if (moveable.Static)
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

            if (moveDistance == 0 && !inQueue)
            {
                inQueue = true;
                #warning ещё обдумать логику передвижения мобов что бы не влияло на fps
                Global.Time.Timer()
                        .After(100)
                        .Do(CalculateMove)
                        .Trigger();
            }
        }

        /// <summary>
        /// Метод выполняющийся когда в фоновом потоке таймер выполняет рассчёт перемещения
        /// </summary>
        /// <returns>Флаг рассчёта движения или нет</returns>
        protected virtual bool OnLogic() => true;
        
        private Direction _dir = Direction.Idle;

        private void CalculateMove()
        {
            if (!OnLogic())
            {
                inQueue = false;
                return;
            }

            //if (moveDistance != 0)
            //    return;

            if (RandomDungeon.Chance(moveable.WalkChance))
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

            switch (dir)
            {
                case Direction.Idle:
                    break;
                case Direction.Up:
                    mapObj.Location.Y -= mapObj.MovementSpeed;
                    break;
                case Direction.Down:
                    mapObj.Location.Y += mapObj.MovementSpeed;
                    break;
                case Direction.Left:
                    mapObj.Location.X -= mapObj.MovementSpeed;
                    break;
                case Direction.Right:
                    mapObj.Location.X += mapObj.MovementSpeed;
                    break;
                case Direction.UpLeft:
                    mapObj.Location.Y -= mapObj.MovementSpeed;
                    mapObj.Location.X -= mapObj.MovementSpeed;
                    break;
                case Direction.UpRight:
                    mapObj.Location.Y -= mapObj.MovementSpeed;
                    mapObj.Location.X += mapObj.MovementSpeed;
                    break;
                case Direction.DownLeft:
                    mapObj.Location.Y += mapObj.MovementSpeed;
                    mapObj.Location.X -= mapObj.MovementSpeed;
                    break;
                case Direction.DownRight:
                    mapObj.Location.Y += mapObj.MovementSpeed;
                    mapObj.Location.X += mapObj.MovementSpeed;
                    break;
                default:
                    break;
            }

            SetAnimation(anim(moveable));

            if (!CheckMoveAvailable(dir))
            {
                moveDistance = -moveable.WaitTime;
                SetAnimation(moveable.Idle);
            }
            else if (this.aliveTooltip != null)
            {
                this.aliveTooltip.Left = this.Position.X;
                this.aliveTooltip.Top = this.Position.Y - 0.8;
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

        private Dictionary<Direction, Func<Moveable, AnimationMap>> MovementMap => new Dictionary<Direction, Func<Moveable, AnimationMap>>()
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
