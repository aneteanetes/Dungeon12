using Dungeon.Utils;
using System.Collections.Generic;

namespace Dungeon.Engine.Editable.TileMap
{
    public class DungeonEngineTilemap
    {
        public string Name { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int CellSize { get; set; }

        [Hidden]
        public List<DungeonEngineTilemapLayer> Layers { get; set; }
    }
}
