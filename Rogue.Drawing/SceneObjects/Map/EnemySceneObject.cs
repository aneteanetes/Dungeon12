namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.Entites.Animations;
    using Rogue.Entites.Enemy;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using Random = Rogue.Random;

    public class EnemySceneObject : AnimatedSceneObject
    {
        private readonly Mob mob;
        private readonly GameMap location;

        public EnemySceneObject(GameMap location, Mob mob, Rectangle defaultFramePosition) : base(defaultFramePosition, null)
        {
            this.location = location;
            this.mob = mob;
            this.Image = mob.Tileset;
            Left = mob.Location.X;
            Top = mob.Location.Y;
            Width = 1;
            Height = 1;

            mob.Die += () =>
            {
                this.Destroy?.Invoke();
            };

            if (mob.Enemy.Idle != null)
            {
                this.SetAnimation(mob.Enemy.Idle);
            }

            this.AddChild(new ObjectHpBar(mob.Enemy));
        }

        protected override void DrawLoop()
        {
            if (moveDistance != 0)
            {
                moveDistance--;
                foreach (var move in moves)
                {
                    Move(DirectionMap[move]);
                }
            }
        }

        protected override void AnimationLoop()
        {
            if (!mob.Enemy.Aggressive)
            {
                return;
            }

            if (moveDistance == 0)
            {
                moves.Clear();
                var next = Random.Next(0, 10);
                if (next > 5)
                {
                    var direction = Random.Next(0, 4);
                    moves.Add(direction);
                    moveDistance = Random.Next(100, 300);
                }
            }
        }

        private int moveDistance = 0;
        private readonly HashSet<int> moves = new HashSet<int>();

        private void Move((Direction dir, Vector vect, Func<Moveable, AnimationMap> anim) data)
        {
            switch (data.dir)
            {
                case Direction.Up:
                case Direction.Down:
                    switch (data.vect)
                    {
                        case Vector.Plus:
                            this.mob.Location.Y += this.mob.MovementSpeed;
                            break;
                        case Vector.Minus:
                            this.mob.Location.Y -= this.mob.MovementSpeed;
                            break;
                        default:
                            break;
                    }
                    break;
                case Direction.Left:
                case Direction.Right:
                    switch (data.vect)
                    {
                        case Vector.Plus:
                            this.mob.Location.X += this.mob.MovementSpeed;
                            break;
                        case Vector.Minus:
                            this.mob.Location.X -= this.mob.MovementSpeed;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            SetAnimation(data.anim(this.mob.Enemy));
            if (!CheckMoveAvailable(data.dir))
            {
                moveDistance = 0;
            }
        }

        private bool CheckMoveAvailable(Direction direction)
        {
            if (this.location.Move(this.mob, direction))
            {
                this.Left = this.mob.Location.X;
                this.Top = this.mob.Location.Y;
                return true;
            }
            else
            {
                this.mob.Location.X = this.Left;
                this.mob.Location.Y = this.Top;
                return false;
            }
        }

        private readonly Dictionary<int, (Direction dir, Vector vect, Func<Moveable, AnimationMap> anim)> DirectionMap = new Dictionary<int, (Direction, Vector, Func<Moveable, AnimationMap>)>
        {
            { 0, (Direction.Up, Vector.Minus, m=>m.MoveUp) },
            { 1,(Direction.Down, Vector.Plus, m=>m.MoveDown) },
            { 2,(Direction.Left, Vector.Minus, m=>m.MoveLeft) },
            { 3,(Direction.Right, Vector.Plus, m=>m.MoveRight) },
        };

        private enum Vector
        {
            Plus,
            Minus
        }
    }
}