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
        private Rogue.Map.Objects.Avatar playerMapObject;
        private Player Player => playerMapObject.Character;
        private readonly GameMap location;

        public Action OnStop;
        public Action OnStart;

        public PlayerSceneObject(Rogue.Map.Objects.Avatar player, GameMap location)
            : base(new Rectangle
            {
                X = 32,
                Y = 0,
                Height = 32,
                Width = 32
            })
        {
            this.playerMapObject = player;
            this.location = location;
            this.Image = player.Tileset;
            this.Width = 1;
            this.Height = 1;
            this.AddChild(new ObjectHpBar(Player));
            //this.AddChild(new ObjectHpBar(Player));
        }

        public double Speed => playerMapObject.MovementSpeed;

        protected override void DrawLoop()
        {
            var _ = NowMoving.Count == 0
                ? RequestStop()
                : RequestResume();

            if (NowMoving.Contains(Direction.Up))
            {
                this.playerMapObject.Location.Y -= Speed;
                SetAnimation(this.Player.MoveUp);
                if (!CheckMoveAvailable(Direction.Up))
                {
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                this.playerMapObject.Location.Y += Speed;
                SetAnimation(this.Player.MoveDown);
                if (!CheckMoveAvailable(Direction.Down))
                {
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                this.playerMapObject.Location.X -= Speed;
                SetAnimation(this.Player.MoveLeft);
                if (!CheckMoveAvailable(Direction.Left))
                {
                    OnStop();
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                this.playerMapObject.Location.X += Speed;
                SetAnimation(this.Player.MoveRight);
                if (!CheckMoveAvailable(Direction.Right))
                {
                    OnStop();
                }
            }
        }

        private bool CheckMoveAvailable(Direction direction)
        {
            if (this.location.Move(playerMapObject, direction))
            {
                this.Left = playerMapObject.Location.X;
                this.Top = playerMapObject.Location.Y;
                return true;
            }
            else
            {
                playerMapObject.Location.X = this.Left;
                playerMapObject.Location.Y = this.Top;
                return false;
            }
        }

        private HashSet<Direction> NowMoving = new HashSet<Direction>();

        public override void KeyDown(Key key, KeyModifiers modifier)
        {
            switch (key)
            {
                case Key.D: NowMoving.Add(Direction.Right); break;
                case Key.A: NowMoving.Add(Direction.Left); break;
                case Key.W: NowMoving.Add(Direction.Up); break;
                case Key.S: NowMoving.Add(Direction.Down); break;
                default: break;
            }
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            switch (key)
            {
                case Key.D:
                    NowMoving.Remove(Direction.Right);
                    OnStop(); break;
                case Key.A:
                    NowMoving.Remove(Direction.Left);
                    OnStop(); break;
                case Key.W:
                    NowMoving.Remove(Direction.Up);
                    OnStop(); break;
                case Key.S:
                    NowMoving.Remove(Direction.Down);
                    OnStop(); break;
                default: break;
            }
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