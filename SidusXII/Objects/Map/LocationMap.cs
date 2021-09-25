using Dungeon;
using Dungeon.GameObjects;
using Dungeon.Tiled;
using Dungeon.Types;
using LiteDB;
using Newtonsoft.Json;
using SidusXII.SceneObjects.Main.Map;
using System.Collections.Generic;
using System.Linq;

namespace SidusXII.Objects.Map
{
    public class LocationMap : GameComponent
    {
        public const string SpawnLayerName = "`[SPAWN]`";

        public Point Location { get; set; } = new Point();

        public static LocationMap Load(string tmxResPath)
        {
            var map = new LocationMap();
            map.tmxrespath = tmxResPath;

            return map;
        }

        public string tmxrespath;

        [JsonIgnore]
        [BsonIgnore]
        public TiledMap TiledMap { get; set; }

        public override void Initialization()
        {
            TiledMap = TiledMap.Load(tmxrespath.AsmRes());
        }
    }
}
