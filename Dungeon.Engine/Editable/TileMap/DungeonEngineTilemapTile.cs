namespace Dungeon.Engine.Editable.TileMap
{
    public class DungeonEngineTilemapTile
    {
        public string SourceImage { get; set; }

        public int OffsetX { get; set; }

        public int OffsetY { get; set; }

        public DungeonEngineTilemapTileBounds Bounds { get; set; }

        public DungeonEngineMapObject Object { get; set; }
    }
}
