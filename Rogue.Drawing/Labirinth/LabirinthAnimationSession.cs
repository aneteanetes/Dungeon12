using System;
using System.Linq;
using Rogue.Drawing.Animations;
using Rogue.Entites;
using Rogue.Entites.Animations;
using Rogue.Map;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Labirinth
{
    public class LabirinthAnimationSession : AnimationSession
    {
        public Point BasePosition { get; set; }

        public Point NextPosition { get; set; }

        public Location Location { get; set; }

        public Direction Direction { get; set; }

        public AnimationMap ObjectAnimationMap { get; set; }

        public override IAnimationSession Run()
        {
            var oldPos = this.Location.Map[(int)BasePosition.Y][(int)BasePosition.X].First();
            var newPos = this.Location.Map[(int)NextPosition.Y][(int)NextPosition.X].First();

            var frameStep = 1f / ObjectAnimationMap.Frames.Count;

            float x = BasePosition.X;
            float y = BasePosition.Y;

            void UpdateCoordinates()
            {
                switch (Direction)
                {
                    case Direction.Up:
                        y -= frameStep;
                        break;
                    case Direction.Down:
                        y += frameStep;
                        break;
                    case Direction.Left:
                        x -= frameStep;
                        break;
                    case Direction.Right:
                        x += frameStep;
                        break;
                    default:
                        break;
                }
            }

            var drawableFrames = ObjectAnimationMap.Frames.Select(f =>
            {
                UpdateCoordinates();
                return new Drawable
                {
                    Tileset = ObjectAnimationMap.TileSet,
                    Region = new Rectangle
                    {
                        X = x+1,
                        Y = y+2,
                        Height = 1,
                        Width = 1
                    },
                    TileSetRegion = new Rectangle
                    {
                        X = f.X,
                        Y = f.Y,
                        Height = ObjectAnimationMap.Size.Y,
                        Width = ObjectAnimationMap.Size.X
                    }
                };
            });

            var framesWithFloor = drawableFrames.Select(frame => new IDrawable[] { oldPos, newPos, frame });

            foreach (var frameWithFloor in framesWithFloor)
            {
                this.AddFrame(frameWithFloor);
            }

            return this;
        }
    }
}