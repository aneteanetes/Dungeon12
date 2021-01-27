namespace Dungeon12.Data.Region
{
    using Dungeon.Physics;
    using Dungeon.Types;
    using Dungeon.Data;
    using System.Collections.Generic;
    using Dungeon12.Database.Respawn;
    using Dungeon.Localization;

    public class Region : Persist
    {
        public override string Image { get; set; }

        public string ImageMin { get; set; }

        public string ImageObjects { get; set; }

        public Point TileBackOffset { get; set; }

        public Point Offset { get; set; }

        public List<RegionPart> Objects { get; set; }

        public List<PhysicalObject> SafeZones { get; set; }

        public List<RespawnData> RandomObjects { get; set; } = new List<RespawnData>();

        public string Name { get; set; }

        public LocalizedString Display { get; set; }

        public bool IsUnderLevel { get; set; }

        public string RegionMusic { get; set; }

        #region Tiled
#pragma warning disable IDE1006

        public List<Layer> layers { get; set; }

        public int tileheight { get; set; }

        public int tilewidth { get; set; }

        public int height { get; set; }

        public int width { get; set; }

        public string orientation { get; set; }

        public Tileset[] tilesets { get; set; }

#pragma warning restore IDE1006
        #endregion
    }

    #region Tiled classes
#pragma warning disable IDE1006

    public class Tileset
    {
        public string image { get; set; }

        public int columns { get; set; }

        public int firstgid { get; set; }

        public int tilecount { get; set; }

        public int tileheight { get; set; }

        public int tilewidth { get; set; }

        public TileInfo[] tiles { get; set; }
    }

    public class TileInfo
    {
        public int id { get; set; }

        public ObjectGroup objectgroup { get; set; }
    }

    public class ObjectGroup
    {
        public TileBoundsPolygonInfo[] objects { get; set; }

        public int id { get; set; } // 2 bounds
    }

    public class TileBoundsPolygonInfo
    {
        public Polygon[] polygon { get; set; }
    }

    public class Polygon
    {
        public float x { get; set; }

        public float y { get; set; }
    }

    public class Layer
    {
        public List<int> data { get; set; }

        public int width { get; set; }

        public int height { get; set; }
    }

#pragma warning restore IDE1006
    #endregion
}