using Dungeon.Types;
using Dungeon.View;
using System.Collections.Generic;

namespace Dungeon12.Components
{
    internal class FrameAnimated
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

        public virtual Square DefaultFramePosition { get; set; }

        public string Tileset { get; set; }

        public static FrameAnimated FromTileset(string tileset, int xFrame, int yFrame, Square defaultFrame)
        {
            return new FrameAnimated()
            {
                Tileset = tileset,
                DefaultFramePosition = defaultFrame != default 
                    ? defaultFrame
                    : new Square()
                    {
                        X = xFrame,
                        Height = xFrame,
                        Width = yFrame
                    },
                MoveUp = new Animation()
                {
                    TileSet = tileset,
                    Frames = new List<Dot>
                    {
                        new Dot(64,96),
                        new Dot(0,96),
                        new Dot(32,96)
                    }
                },
                MoveDown = new Animation()
                {
                    Frames = new List<Dot>
                    {
                        new Dot(64,0),
                        new Dot(0,0),
                        new Dot(32,0)
                    }
                },
                MoveLeft = new Animation()
                {
                    Frames = new List<Dot>
                    {
                        new Dot(64,32),
                        new Dot(0,32),
                        new Dot(32,32)
                    }
                },
                MoveRight = new Animation()
                {
                    Frames = new List<Dot>
                    {
                        new Dot(64,64),
                        new Dot(0,64),
                        new Dot(32,64)
                    }
                }
            };
        }
    }
}