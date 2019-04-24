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
        private readonly Location location;

        public PlayerSceneObject(Rogue.Map.Objects.Avatar player, Location location)
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

        public float distanceOneFrame = 0.04f;

        public Rectangle Apply()
        {
            FrameCounter++;

            var now = new Point(this.Left, this.Top);
            bool? needRound = null;

            if (NowMoving.Contains(Direction.Up))
            {
                this.playerMapObject.Location.Y -= distanceOneFrame;

                if (this.location.MayMove(playerMapObject))
                {
                    this.Left = playerMapObject.Location.X;
                    this.Top = playerMapObject.Location.Y;
                }
                else
                {
                    playerMapObject.Location.X = this.Left;
                    playerMapObject.Location.Y = this.Top;
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                this.playerMapObject.Location.Y += distanceOneFrame;

                if (this.location.MayMove(playerMapObject))
                {
                    this.Left = playerMapObject.Location.X;
                    this.Top = playerMapObject.Location.Y;
                }
                else
                {
                    playerMapObject.Location.X = this.Left;
                    playerMapObject.Location.Y = this.Top;
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                this.playerMapObject.Location.X -= distanceOneFrame;

                if (this.location.MayMove(playerMapObject))
                {
                    this.Left = playerMapObject.Location.X;
                    this.Top = playerMapObject.Location.Y;
                }
                else
                {
                    playerMapObject.Location.X = this.Left;
                    playerMapObject.Location.Y = this.Top;
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                this.playerMapObject.Location.X += distanceOneFrame;

                if (this.location.MayMove(playerMapObject))
                {
                    this.Left = playerMapObject.Location.X;
                    this.Top = playerMapObject.Location.Y;
                }
                else
                {
                    playerMapObject.Location.X = this.Left;
                    playerMapObject.Location.Y = this.Top;
                }
            }

            if (needRound.HasValue)
            {
                //var then = new Point(Round(needRound,this.Left), Round(needRound, this.Top));
                //var nowCell = new Point(Round(needRound, now.X), Round(needRound, now.Y));


                //if (!this.location.MoveObject(nowCell, 1, then))
                //{
                //    this.Left = now.X;
                //    this.Top = now.Y;
                //}
            }

            //Console.WriteLine("add direction");

            //if (@double)
            //{
            //    Play = false;
            //    @double = false;
            //}
            //else
            //{
            //    @double = true;
            //}        

            //if (Play && FrameCounter == Math.Round(FrameTimeIn_60))
            //{
            //    switch (AnimationMap.Direction)
            //    {
            //        case Direction.Idle:
            //            break;
            //        case Direction.Up:
            //            this.Top -= distanceOneFrame;
            //            break;
            //        case Direction.Down:
            //            this.Top += distanceOneFrame;
            //            break;
            //        case Direction.Left:
            //            this.Left -= distanceOneFrame;
            //            break;
            //        case Direction.Right:
            //            this.Left += distanceOneFrame;
            //            break;
            //        default:
            //            break;
            //    }

            //    Play = false;

            //    //FrameCounter = 0;
            //    //pos.Pos = AnimationMap.Frames[frame];


            //    //frame++;
            //    //framesDrawed++;

            //    //if (frame == AnimationMap.Frames.Count)
            //    //{
            //    //    frame = 0;
            //    //}


            //    //if (framesDrawed > FrameCount)
            //    //{
            //    //    framesDrawed = 0;
            //    //    Play = false;
            //    //}
            //}

            //if (FrameCounter > 60) FrameCounter = 0;

            return pos;
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
                case Key.D: NowMoving.Add(Direction.Right); this.AnimationMap = this.Player.MoveRight; break;
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