using Dungeon;
using Dungeon.GameObjects;
using Dungeon.Tiled;
using Dungeon.Types;
using LiteDB;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SidusXII.Models.Map
{
    public class MapComponent : GameComponent
    {
        public const string SpawnLayerName = "`[SPAWN]`";
        public const string VisibleLayerName = "`[VISIBLE]`";

        public Point Location { get; set; } = new Point();

        public Dictionary<string, MapCellComponent> Cells = new Dictionary<string, MapCellComponent>();

        public static MapComponent Load(string tmxResPath)
        {
            var map = new MapComponent
            {
                tmxrespath = tmxResPath
            };

            return map;
        }

        public string tmxrespath;

        [JsonIgnore]
        [BsonIgnore]
        public TiledMap TiledMap { get; set; }

        public override void Initialization()
        {
            this.TiledMap = TiledMap.Load(tmxrespath.AsmRes());

            var examplelayer = TiledMap.Layers.FirstOrDefault();
            var tileCount = examplelayer.Tiles.Count;
            var layerWidth = examplelayer.width;
            var layerHeight = examplelayer.height;

            var gameX = 0;
            var gameY = 0;

            for (int i = 0; i < tileCount; i++)
            {
                var cell = new MapCellComponent
                {
                    X = gameX,
                    Y = gameY
                };

                var tiles = TiledMap.Layers.Select(x => x.Tiles[i]);

                foreach (var tile in tiles)
                {
                    if (tile.FileName.IsNotEmpty())
                    {
                        if (tile.Layer.name == MapComponent.SpawnLayerName && Location.IsDefault)
                        {
                            Location = new Point(gameX, gameY);
                            cell.Spawn = true;
                            continue;
                        }

                        if (tile.Layer.name == MapComponent.VisibleLayerName)
                        {
                            cell.Visible = true;
                        }

                        TileInfo ti = new TileInfo() { AsmImgTile = $"Tiles/{tile.FileName}".AsmImg() };

                        if (tile.FlippedHorizontally && tile.FlippedVertically)
                        {
                            ti.Flip = Dungeon.View.Enums.FlipStrategy.Both;
                        }
                        else if (tile.FlippedHorizontally)
                        {
                            ti.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;
                        }
                        else if (tile.FlippedVertically)
                        {
                            ti.Flip = Dungeon.View.Enums.FlipStrategy.Vertically;
                        }

                        cell.Tiles.Add(ti);
                    }
                }

                Cells.Add($"{gameX},{gameY}", cell);

                gameX++;

                if (gameX == layerWidth)
                {
                    gameX = 0;
                    gameY++;
                }
            }

            SetNearFog();
        }

        private void SetNearFog()
        {
            var visibles = Cells.Where(x => x.Value.Visible);
            foreach (var visible in visibles)
            {


            }
        }
    }
}
