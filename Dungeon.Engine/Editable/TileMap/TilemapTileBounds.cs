namespace Dungeon.Engine.Editable.TileMap
{
    public class TilemapTileBounds
    {
        public bool Left { get; set; }

        public bool Top { get; set; }

        public bool Right { get; set; }

        public bool Bottom { get; set; }

        public bool Full { get; set; }

        public bool PerPixel { get; set; }
    }

    public enum DungeonEgineTilemapTileBoundsType
    { 
        Left,
        Top,
        Right,
        Bottom,
        Full
    }
}
