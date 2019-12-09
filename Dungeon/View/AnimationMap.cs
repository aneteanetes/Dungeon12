namespace Dungeon.Entities.Animations
{
    using System.Collections.Generic;
    using Dungeon.Types;

    public class AnimationMap
    {
        public bool TilesetAnimation { get; set; } = true;

        public string TileSet { get; set; }

        public Point Size { get; set; }

        public List<Point> Frames { get; set; }

        public Direction Direction { get; set; }

        public double FramesPerSecond { get; set; }

        public string[] FullFrames { get; set; }
    }
}