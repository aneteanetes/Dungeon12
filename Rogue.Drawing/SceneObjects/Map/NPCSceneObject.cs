namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.SceneObjects.Base;
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
    using Random = Rogue.Random;

    public class NPCSceneObject : AnimatedSceneObject
    {
        private readonly NPC npc;
        private readonly GameMap location;

        public NPCSceneObject(GameMap location, NPC mob, Rectangle defaultFramePosition) : base(defaultFramePosition, null)
        {
            this.location = location;
            this.npc = mob;
            this.Image = mob.Tileset;
            Left = mob.Location.X;
            Top = mob.Location.Y;
            Width = 1;
            Height = 1;

            mob.Die += () =>
            {
                this.Destroy?.Invoke();
            };

            if (mob.NPCEntity.Idle != null)
            {
                this.SetAnimation(mob.NPCEntity.Idle);
            }            
        }

        private Tooltip aliveTooltip = null;

        public override void Focus()
        {
            aliveTooltip = new Tooltip(npc.Name, new Point(this.Position.X, this.Position.Y - 0.8));

            this.ShowEffects(new List<ISceneObject>() { aliveTooltip });
        }

        public override void Unfocus()
        {
            aliveTooltip.Destroy?.Invoke();
            aliveTooltip = null;
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
            moves.Clear();
            var next = Random.Next(0, 10);
            if (next > 5)
            {
                var direction = Random.Next(0, 4);
                moves.Add(direction);

                var diagonally = Random.Next(0, 4);
                if (diagonally != direction && NotPair(direction, diagonally))
                {
                    moves.Add(diagonally);
                }

                moveDistance = Random.Next(100, 300);
            }
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
                            this.npc.Location.Y += this.npc.MovementSpeed;
                            break;
                        case Vector.Minus:
                            this.npc.Location.Y -= this.npc.MovementSpeed;
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
                            this.npc.Location.X += this.npc.MovementSpeed;
                            break;
                        case Vector.Minus:
                            this.npc.Location.X -= this.npc.MovementSpeed;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            SetAnimation(data.anim(this.npc.NPCEntity));
            if (!CheckMoveAvailable(data.dir))
            {
                moveDistance = 0;
            }
            else if (this.aliveTooltip!=null)
            {
                this.aliveTooltip.Left = this.Position.X;
                this.aliveTooltip.Top = this.Position.Y - 0.8;
            }
        }

        private bool CheckMoveAvailable(Direction direction)
        {
            if (this.location.Move(this.npc, direction))
            {
                this.Left = this.npc.Location.X;
                this.Top = this.npc.Location.Y;
                return true;
            }
            else
            {
                this.npc.Location.X = this.Left;
                this.npc.Location.Y = this.Top;
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