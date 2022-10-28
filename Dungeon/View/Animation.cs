namespace Dungeon.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon.Types;

    public class Animation
    {
        public string Name { get; set; }

        public bool TilesetAnimation { get; set; } = true;

        public string TileSet { get; set; }

        public Dot Size { get; set; }

        public List<Dot> Frames { get; set; }

        public double FramesPerSecond { get; set; }

        public string[] FullFrames { get; set; }

        public TimeSpan Time { get; set; }

        public Square DefaultFramePosition => new Square()
        {
            Height = Size.Y,
            Width = Size.X,
            X = Frames?.FirstOrDefault().X ?? 0,
            Y = Frames?.FirstOrDefault().Y ?? 0
        };

        public Square LastFramePosition => new Square()
        {
            Height = Size.Y,
            Width = Size.X,
            X = Frames?.LastOrDefault().X ?? 0,
            Y = Frames?.LastOrDefault().Y ?? 0
        };

        public Action OnEnd { get; set; }
    }
}