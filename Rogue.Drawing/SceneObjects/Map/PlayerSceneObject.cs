namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Keys;
    using Rogue.Entites.Alive.Character;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PlayerSceneObject : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public PlayerSceneObject(Player player)
        {
            this.Image = player.Tileset;
            this.Width = 1;
            this.Height = 1;
        }

        public override Rectangle ImageRegion => Apply();
        
        private int frame = 0;
        private readonly Dictionary<int, Point> frames = new Dictionary<int, Point>()
        {
                {0,     new Point(64, 64) },
                {1,     new Point(0, 64)},
                {2,     new Point(32, 64)},
        };

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

        public bool Play
        {
            get
            {
                return playing.LastOrDefault();
            }
            set
            {
                if (value)
                    playing.Add(true);
                else
                    playing.Remove(playing.LastOrDefault());
            }
        }

        private double FrameCounter = 0;

        public Rectangle Apply()
        {
            FrameCounter++;

            if (Play && FrameCounter == Math.Round(FrameTimeIn_60))
            {
                this.Left += distanceOneFrame;

                FrameCounter = 0;
                pos.Pos = frames[frame];

                frame++;
                framesDrawed++;

                if (frame == frames.Count)
                {
                    frame = 0;
                }

                if (framesDrawed > FrameCount)
                {
                    framesDrawed = 0;
                    Play = false;
                }
            }

            if (FrameCounter > 60) FrameCounter = 0;

            return pos;
        }

        //реальные константы

        public int framesDrawed = 0;

        public float distanceOneFrame = 0.1f;
        public double FrameCount => frames.Count * speed;

        public double FrameTimeIn_60 => 60 / FrameCount;

        public double speed = 2;

        public override void KeyDown(Key key, KeyModifiers modifier)
        {
            if (key == Key.D)
            {
                this.Play = true;
            }
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            if (playing.Count > 1)
                StopPlay();
        }
    }
}