using System.Collections.Generic;
using System.Diagnostics;

namespace Dungeon.Tiled
{
    [DebuggerDisplay("{name}")]
    public class TiledLayer
    {
        public string name { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public List<TiledPolygon> Tiles { get; set; } = new List<TiledPolygon>();
    }
}