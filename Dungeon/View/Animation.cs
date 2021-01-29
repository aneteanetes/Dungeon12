namespace Dungeon.Entities.Animations
{
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon.Types;

    public class Animation
    {
        public bool TilesetAnimation { get; set; } = true;

        public string TileSet { get; set; }

        public Point Size { get; set; }

        public List<Point> Frames { get; set; }

        public double FramesPerSecond { get; set; }

        public string[] FullFrames { get; set; }

        public Rectangle DefaultFramePosition => new Rectangle()
        {
            Height = Size.Y,
            Width = Size.X,
            X = Frames?.FirstOrDefault()?.X ?? 0,
            Y = Frames?.FirstOrDefault()?.Y ?? 0
        };

        public Rectangle LastFramePosition => new Rectangle()
        {
            Height = Size.Y,
            Width = Size.X,
            X = Frames?.LastOrDefault()?.X ?? 0,
            Y = Frames?.LastOrDefault()?.Y ?? 0
        };
    }
}