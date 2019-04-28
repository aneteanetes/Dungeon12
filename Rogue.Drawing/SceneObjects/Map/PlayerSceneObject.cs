namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Keys;
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

        public PlayerSceneObject(Rogue.Map.Objects.Avatar player, GameMap location)
            :base(new Rectangle
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
        }

        public float Speed = 0.08f;

        protected override void DrawLoop()
        {
            var _ = NowMoving.Count == 0
                ? RequestStop()
                : RequestResume();

            MapObject old = null;// this.playerMapObject.ClonePhysicalObject();

            if (NowMoving.Contains(Direction.Up))
            {
                this.playerMapObject.Location.Y -= Speed;
                SetAnimation(this.Player.MoveUp);
                if (!CheckMoveAvailable(old))
                {
                    //NowMoving.Remove(Direction.Up);
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                this.playerMapObject.Location.Y += Speed;
                SetAnimation(this.Player.MoveDown);
                if (!CheckMoveAvailable(old))
                {
                    //NowMoving.Remove(Direction.Down);
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                this.playerMapObject.Location.X -= Speed;
                SetAnimation(this.Player.MoveLeft);
                if (!CheckMoveAvailable(old))
                {
                    //NowMoving.Remove(Direction.Left);
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                this.playerMapObject.Location.X += Speed;
                SetAnimation(this.Player.MoveRight);
                if (!CheckMoveAvailable(old))
                {
                    //NowMoving.Remove(Direction.Right);
                }
            }
        }
        
        private bool CheckMoveAvailable(MapObject old)
        {
            if (this.location.Move(playerMapObject,old))
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
                case Key.D: NowMoving.Add(Direction.Right);  break;
                case Key.A: NowMoving.Add(Direction.Left);break;
                case Key.W: NowMoving.Add(Direction.Up); break;
                case Key.S: NowMoving.Add(Direction.Down); break;
                default: break;
            }
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            switch (key)
            {
                case Key.D: NowMoving.Remove(Direction.Right); break;
                case Key.A: NowMoving.Remove(Direction.Left); break;
                case Key.W: NowMoving.Remove(Direction.Up); break;
                case Key.S: NowMoving.Remove(Direction.Down); break;
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