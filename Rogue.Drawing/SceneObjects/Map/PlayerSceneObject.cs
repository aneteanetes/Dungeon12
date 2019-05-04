namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive.Character;
    using Rogue.Entites.Animations;
    using Rogue.Map;
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
            this.path = path;
        }

        protected override void DrawLoop()
        {
            if (path.Count > 0)
            {
                var step = path.First();
                path.Remove(step);

                this.Left = step.X / 32;
                this.Top = step.Y / 32;
                this.avatar.Location.X = this.Left;
                this.avatar.Location.Y = this.Top;

                this.location.Move(this.avatar, Direction.Idle);
            }

            return;

            var _ = NowMoving.Count == 0
                ? RequestStop()
                : RequestResume();
            
            if (NowMoving.Contains(Direction.Up))
            {
                this.avatar.Location.Y -= Speed;
                SetAnimation(this.Player.MoveUp);
                if (!CheckMoveAvailable(Direction.Up))
                {
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                this.avatar.Location.Y += Speed;
                SetAnimation(this.Player.MoveDown);
                if (!CheckMoveAvailable(Direction.Down))
                {
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                this.avatar.Location.X -= Speed;
                SetAnimation(this.Player.MoveLeft);
                if (!CheckMoveAvailable(Direction.Left))
                {
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                this.avatar.Location.X += Speed;
                SetAnimation(this.Player.MoveRight);
                if (!CheckMoveAvailable(Direction.Right))
                {
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