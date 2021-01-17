namespace Dungeon.Tiled
{
    public class TiledTileset
    {
        public int tilewidth { get; set; }

        public int tiledheight { get; set; }

        public int firstgid { get; set; }

        public int tilecount { get; set; }

        public string image { get; set; }

        public int TileIndexFrom => firstgid;

        public int TiledIndexTo => tilecount;
    }
}