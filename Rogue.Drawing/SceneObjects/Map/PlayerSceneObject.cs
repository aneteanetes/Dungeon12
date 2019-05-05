namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive.Character;
    using Rogue.Entites.Animations;
    using Rogue.Map;
    using Rogue.Physics;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    public class PlayerSceneObject : AnimatedSceneObject
    {
        public Rogue.Map.Objects.Avatar avatar;
        private Player Player => avatar.Character;
        private readonly GameMap location;

        public Action OnStop;
        public Action OnStart;

        public PlayerSceneObject(Rogue.Map.Objects.Avatar player, GameMap location)
            :base(new Rectangle
            {
                X = 32,
                Y = 0,
                Height = 32,
                Width = 32
            })
        {
            this.avatar = player;            
            this.location = location;
            this.Image = player.Tileset;
            this.Width = 1;
            this.Height = 1;
            this.AddChild(new ObjectHpBar(Player));
            //this.AddChild(new ObjectHpBar(Player));
        }

        public double Speed => avatar.MovementSpeed;

        private List<Point> path = new List<Point>();
        public void SetPath(List<Point> path)
        {
            NowMoving.Clear();
            nextPos = null;
            this.path = path;
        }

        private PhysicalObject nextPos = null;

        protected override void DrawLoop()
        {
            if (path.Count > 0)
            {
                if (nextPos == null || nextPos.IntersectsWith(avatar))
                {
                    NowMoving.Clear();

                    var step = path.First();
                    path.Remove(step);
                    nextPos = new PhysicalObject
                    {
                        Position = new PhysicalPosition { X = step.X, Y = step.Y },
                        Size = this.avatar.Size
                    };

                    //if (step.X > this.Left * 32)
                    //{
                    //    NowMoving.Add(Direction.Right);
                    //}

                    //if (step.X < this.Left * 32)
                    //{
                    //    NowMoving.Add(Direction.Left);
                    //}

                    //if (step.Y < this.Top * 32)
                    //{
                    //    NowMoving.Add(Direction.Up);
                    //}

                    //if (step.Y > this.Top * 32)
                    //{
                    //    NowMoving.Add(Direction.Down);
                    //}
                }
            }

            if (nextPos != null)
            {
                if (!nextPos.IntersectsWith(avatar))
                {
                    NowMoving.Clear();

                    if (nextPos.Position.X > this.Left * 32)
                    {
                        NowMoving.Add(Direction.Right);
                    }

                    if (nextPos.Position.X < this.Left * 32)
                    {
                        NowMoving.Add(Direction.Left);
                    }

                    if (nextPos.Position.Y < this.Top * 32)
                    {
                        NowMoving.Add(Direction.Up);
                    }

                    if (nextPos.Position.Y > this.Top * 32)
                    {
                        NowMoving.Add(Direction.Down);
                    }
                }
                else
                {
                    NowMoving.Clear();
                    nextPos = null;
                }
            }

            var _ = NowMoving.Count == 0
                ? RequestStop()
                : RequestResume();
            
            if (NowMoving.Contains(Direction.Up))
            {
                this.avatar.Location.Y -= Speed;
                SetAnimation(this.Player.MoveUp);
                if (!CheckMoveAvailable(Direction.Up))
                {
                    this.NowMoving.Remove(Direction.Up);
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                this.avatar.Location.Y += Speed;
                SetAnimation(this.Player.MoveDown);
                if (!CheckMoveAvailable(Direction.Down))
                {
                    this.NowMoving.Remove(Direction.Down);
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                this.avatar.Location.X -= Speed;
                SetAnimation(this.Player.MoveLeft);
                if (!CheckMoveAvailable(Direction.Left))
                {
                    this.NowMoving.Remove(Direction.Left);
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                this.avatar.Location.X += Speed;
                SetAnimation(this.Player.MoveRight);
                if (!CheckMoveAvailable(Direction.Right))
                {
                    this.NowMoving.Remove(Direction.Right);
                    OnStop();
                }
            }
        }
        
        private bool CheckMoveAvailable(Direction direction)
        {
            if (this.location.Move(avatar, direction))
            {
                this.Left = avatar.Location.X;
                this.Top = avatar.Location.Y;
                return true;
            }
            else
            {
                avatar.Location.X = this.Left;
                avatar.Location.Y = this.Top;
                return false;
            }
        }
        
        private HashSet<Direction> NowMoving = new HashSet<Direction>();

        public override void KeyDown(Key key, KeyModifiers modifier)
        {
            //switch (key)
            //{
            //    case Key.D: NowMoving.Add(Direction.Right);  break;
            //    case Key.A: NowMoving.Add(Direction.Left);break;
            //    case Key.W: NowMoving.Add(Direction.Up); break;
            //    case Key.S: NowMoving.Add(Direction.Down); break;
            //    default: break;
            //}
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {            
            //switch (key)
            //{
            //    case Key.D: NowMoving.Remove(Direction.Right);
            //        OnStop(); break;
            //    case Key.A: NowMoving.Remove(Direction.Left);
            //        OnStop(); break;
            //    case Key.W: NowMoving.Remove(Direction.Up);
            //        OnStop(); break;
            //    case Key.S: NowMoving.Remove(Direction.Down);
            //        OnStop(); break;
            //    default: break;
            //}
        }

        protected override Key[] KeyHandles => new Key[]
        {
            Key.D,
            Key.A,
            Key.W,
            Key.S,
        };
    }
}