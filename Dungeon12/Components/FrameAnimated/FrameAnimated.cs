using Dungeon.Types;
using Dungeon.View;
using System.Collections.Generic;

namespace Dungeon12.Components
{
    public class FrameAnimated
    {
        public virtual Animation Idle { get; set; }

        public virtual Animation MoveUp { get; set; }

        public virtual Animation MoveDown { get; set; }

        public virtual Animation MoveLeft { get; set; }

        public virtual Animation MoveRight { get; set; }

        public virtual Animation MoveUpLeft { get; set; }

        public virtual Animation MoveUpRight { get; set; }

        public virtual Animation MoveDownLeft { get; set; }

        public virtual Animation MoveDownRight { get; set; }

        public virtual Rectangle DefaultFramePosition { get; set; }

        public string Tileset { get; set; }

        public static FrameAnimated FromTileset(string tileset, int xFrame, int yFrame, Rectangle defaultFrame = null)
        {
            return new FrameAnimated()
            {
                Tileset = tileset,
                DefaultFramePosition = defaultFrame ?? new Rectangle()
                {
                    X = xFrame,
                    Height = xFrame,
                    Width = yFrame
                },
                MoveUp = new Animation()
                {
                    TileSet = tileset,
                    Frames = new List<Point>
                    {
                        new Point(64,96),
                        new Point(0,96),
                        new Point(32,96)
                    }
                },
                MoveDown = new Animation()
                {
                    Frames = new List<Point>
                    {
                        new Point(64,0),
                        new Point(0,0),
                        new Point(32,0)
                    }
                },
                MoveLeft = new Animation()
                {
                    Frames = new List<Point>
                    {
                        new Point(64,32),
                        new Point(0,32),
                        new Point(32,32)
                    }
                },
                MoveRight = new Animation()
                {
                    Frames = new List<Point>
                    {
                        new Point(64,64),
                        new Point(0,64),
                        new Point(32,64)
                    }
                }
            };
        }
    }
}