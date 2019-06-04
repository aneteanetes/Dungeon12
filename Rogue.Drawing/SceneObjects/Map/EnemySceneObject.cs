namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.Entites.Animations;
    using Rogue.Entites.Enemy;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using RandomRogue = Rogue.RandomRogue;

    public class EnemySceneObject : AnimatedSceneObject
    {
        private readonly Mob mob;
        private readonly GameMap location;

        public EnemySceneObject(GameMap location, Mob mob, Rectangle defaultFramePosition) : base(mob.Name,defaultFramePosition, null)
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

            attackTimer = new Timer(1000);
            attackTimer.AutoReset = false;
            attackTimer.Elapsed += (x, y) => attackAvailable = true;
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
                this.mob.IsChasing = false;
            }

            var players = location.Map.Query(this.mob.Vision, true)
                .SelectMany(nodes => nodes.Nodes)
                .Where(node => this.mob.Vision.IntersectsWith(node))
                .Where(x => typeof(Avatar).IsAssignableFrom(x.GetType()));

            if (players.Count() > 0)
            {
                var player = players.First();
                if (player.IntersectsWith(this.mob.AttackRange))
                {
                    if (attackAvailable)
                    {
                        Attack(player as Avatar);
                        attackAvailable = false;
                        attackTimer.Start();
                    }
                }

                if (!this.mob.IsChasing)
                {
                    moves.Clear();
                    moveDistance = 0;
                    Chasing(player as Avatar);
                }
            }

            if (moveDistance == 0)
            {
                moves.Clear();
                var next = RandomRogue.Next(0, 10);
                if (next > 5)
                {
                    var direction = RandomRogue.Next(0, 4);
                    moves.Add(direction);

                    var diagonally = RandomRogue.Next(0, 4);
                    if (diagonally != direction && NotPair(direction, diagonally))
                    {
                        moves.Add(diagonally);
                    }

                    moveDistance = RandomRogue.Next(100, 300);
                }
            }
        }

        private Timer attackTimer;

        private bool attackAvailable = true;

        private void Attack(Avatar avatar)
        {
            var player = avatar.Character;

            var value = (long)RandomRogue.Next(2, 7);

            var dmg = value - player.Defence;

            if (dmg < 0)
            {
                dmg = 0;
            }

            player.HitPoints -= dmg;

            if (player.HitPoints <= 0)
            {
                avatar.Die?.Invoke();
            }

            var critical = dmg > 6;

            this.ShowEffects(new List<ISceneObject>()
                {
                    new PopupString(dmg.ToString()+(critical ? "!" : ""), critical ? ConsoleColor.Red : ConsoleColor.White,avatar.Location,25,critical ? 19 : 17,0.06)
                });
        }

        private void Chasing(Avatar avatar)
        {
            this.mob.IsChasing = true;
            if (avatar.Position.X <= this.mob.Position.X)
            {
                this.moves.Add(2);
            }
            if (avatar.Position.X >= this.mob.Position.X)
            {
                this.moves.Add(3);
            }
            if (avatar.Position.Y >= this.mob.Position.Y)
            {
                this.moves.Add(1);
            }
            if (avatar.Position.Y <= this.mob.Position.Y)
            {
                this.moves.Add(0);
            }
            moveDistance = 60;
        }

        private bool NotPair(int common, int additional)
        {
            if (common + additional == 1)
                return false;

            if (common + additional == 5)
                return false;

            return true;
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