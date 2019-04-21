namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Keys;
    using Rogue.Entites.Alive.Character;
    using Rogue.Entites.Animations;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PlayerSceneObject : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        private Player player;

        public PlayerSceneObject(Player player)
        {
            this.player = player;
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


        bool @double = false;
        public Rectangle Apply()
        {
            FrameCounter++;

            if (NowMoving.IsNotEmpty())
            {
                foreach (var direction in NowMoving)
                {
                    switch (direction)
                    {
                        case Direction.Idle:
                            break;
                        case Direction.Up:
                            this.Top -= distanceOneFrame;
                            break;
                        case Direction.Down:
                            this.Top += distanceOneFrame;
                            break;
                        case Direction.Left:
                            this.Left -= distanceOneFrame;
                            break;
                        case Direction.Right:
                            this.Left += distanceOneFrame;
                            break;
                        default:
                            break;
                    }
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
            }
            
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

        //реальные константы

        public int framesDrawed = 0;

        public float distanceOneFrame = 0.1f;
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
                case Key.D: NowMoving.Add(Direction.Right); this.AnimationMap = this.player.MoveRight; break;
                case Key.A: NowMoving.Add(Direction.Left); this.AnimationMap = this.player.MoveLeft; break;
                case Key.W: NowMoving.Add(Direction.Up); this.AnimationMap = this.player.MoveUp; break;
                case Key.S: NowMoving.Add(Direction.Down); this.AnimationMap = this.player.MoveDown; break;
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
                case Key.D: NowMoving.Remove(Direction.Right); this.AnimationMap = this.player.MoveRight; break;
                case Key.A: NowMoving.Remove(Direction.Left); this.AnimationMap = this.player.MoveLeft; break;
                case Key.W: NowMoving.Remove(Direction.Up); this.AnimationMap = this.player.MoveUp; break;
                case Key.S: NowMoving.Remove(Direction.Down); this.AnimationMap = this.player.MoveDown; break;
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