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

    public class PlayerSceneObject : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        private Rogue.Map.Objects.Avatar playerMapObject;
        private Player Player => playerMapObject.Character;
        private readonly GameMap location;

        public PlayerSceneObject(Rogue.Map.Objects.Avatar player, GameMap location)
        {
            this.playerMapObject = player;            
            this.location = location;
            this.Image = player.Tileset;
            this.Width = 1;
            this.Height = 1;
        }

        public override Rectangle ImageRegion => Apply();
        
        private int frame = 0;

        private AnimationMap AnimationMap;

        private Rectangle pos = new Rectangle
        {
            X = 32,
            Y = 0,
            Height = 32,
            Width = 32
        };

        private List<bool> playing = new List<bool>();

        public void StopPlay()
        {
            playing.Clear();
        }

        public bool Play { get; set; }
        //{
        //    get
        //    {
        //        return playing.LastOrDefault();
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            Console.WriteLine("playing");
        //            playing.Add(true);
        //        }
        //        else
        //            playing.Remove(playing.LastOrDefault());
        //    }
        //}

        private double FrameCounter = 0;

        public float distanceOneFrame = 0.03f;

        public Rectangle Apply()
        {
            FrameCounter++;

            animationStop = NowMoving.Count == 0;

            if (NowMoving.Contains(Direction.Up))
            {
                this.playerMapObject.Location.Y -= distanceOneFrame;
                if (TryMove())
                {
                    this.AnimationMap = this.Player.MoveUp;
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                this.playerMapObject.Location.Y += distanceOneFrame;
                if (TryMove())
                {
                    this.AnimationMap = this.Player.MoveDown;
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                this.playerMapObject.Location.X -= distanceOneFrame;
                if (TryMove())
                {
                    this.AnimationMap = this.Player.MoveLeft;
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                this.playerMapObject.Location.X += distanceOneFrame;
                if (TryMove())
                {
                    this.AnimationMap = this.Player.MoveRight;
                }
            }

            if (AnimationMap != null && !animationStop)
            {
                pos.Pos = AnimationMap.Frames[frame];

                if (FrameCounter % (60/AnimationMap.Frames.Count)==0)
                {
                    frame++;
                }

                if (frame == AnimationMap.Frames.Count)
                {
                    frame = 0;
                }
            }

            return pos;
        }

        private bool animationStop = false;

        private bool TryMove()
        {
            if (this.location.MayMove(playerMapObject))
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

        private int Round(bool? r,double v) => r == true
                    ? (int)Math.Ceiling(v)
                    : (int)v;

        private int Round(bool? r, float v) => r == true
                    ? (int)Math.Ceiling(v)
                    : (int)v;

        //реальные константы

        public int framesDrawed = 0;

        public double FrameCount => AnimationMap.Frames.Count * speed;

        public double FrameTimeIn_60 => 24 / FrameCount;

        public double speed = 1;


        /////////////////////////////////// ВОТ ТУТ НОВАЯ НЕ ДЁРГАННАЯ РЕАЛИЗАЦИЯ
        private HashSet<Direction> NowMoving = new HashSet<Direction>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifier"></param>
        public override void KeyDown(Key key, KeyModifiers modifier)
        {
            //bool actions = true;

            switch (key)
            {
                case Key.D: NowMoving.Add(Direction.Right);  break;
                case Key.A: NowMoving.Add(Direction.Left); this.AnimationMap = this.Player.MoveLeft; break;
                case Key.W: NowMoving.Add(Direction.Up); this.AnimationMap = this.Player.MoveUp; break;
                case Key.S: NowMoving.Add(Direction.Down); this.AnimationMap = this.Player.MoveDown; break;
                default: break;
            }

            //if (actions)
            //{
            //    this.Image = this.AnimationMap.TileSet;
            //this.Play = true;
            //}
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            switch (key)
            {
                case Key.D: NowMoving.Remove(Direction.Right); this.AnimationMap = this.Player.MoveRight; break;
                case Key.A: NowMoving.Remove(Direction.Left); this.AnimationMap = this.Player.MoveLeft; break;
                case Key.W: NowMoving.Remove(Direction.Up); this.AnimationMap = this.Player.MoveUp; break;
                case Key.S: NowMoving.Remove(Direction.Down); this.AnimationMap = this.Player.MoveDown; break;
                default: break;
            }

            //this.Play = false;
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