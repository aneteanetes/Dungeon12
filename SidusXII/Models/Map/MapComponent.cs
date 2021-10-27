using Dungeon;
using Dungeon.GameObjects;
using Dungeon.Tiled;
using Dungeon.Types;
using LiteDB;
using Newtonsoft.Json;
using SidusXII.SceneObjects.Main.Map;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SidusXII.Models.Map
{
    public class MapComponent : GameComponent
    {
        public const string SpawnLayerName = "`[SPAWN]`";
        public const string VisibleLayerName = "`[VISIBLE]`";
        public const string CollisionLayerName = "`[COLLISION]`";

        public MapCellComponent PlayerLocation { get; set; }

        /// <summary>
        /// X,Y
        /// </summary>
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
                    Y = gameY,
                    Cells = Cells
                };

                var tiles = TiledMap.Layers.Select(x => x.Tiles[i]);

                foreach (var tile in tiles)
                {
                    if (tile.FileName.IsNotEmpty())
                    {
                        if (tile.Layer.name == MapComponent.SpawnLayerName && PlayerLocation==null)
                        {
                            PlayerLocation = cell;
                            cell.Player = true;
                            continue;
                        }

                        if(tile.Layer.name== MapComponent.CollisionLayerName)
                        {
                            cell.
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

            Cells.ForEach(c =>
            {
                c.Value.InitAround();
            });

            SetNearFog();
        }

        private void SetNearFog()
        {
            var visibles = Cells.Where(x => x.Value.Visible);
            foreach (var visible in visibles)
            {
                var cell = visible.Value;

                //cell.InitAround();
                cell.Around.ForEach(a =>
                {
                    //a.InitAround();
                    a.ClearFog();
                });
            }
        }

        public void Move(MapCellComponent cell)
        {
            if (cell.Player)
                return;

            ClearPath();

            PlayerLocation.Player = false;
            cell.Player = true;
            PlayerLocation = cell;
        }

        private List<MapCellComponent> MovePath = new List<MapCellComponent>();

        public void FindPath(MapCellComponent cell)
        {
            ClearPath();
            if (cell == PlayerLocation)
                return;

            MovePath.Clear();

            var next = FindNextStep(PlayerLocation, ref cell);
            MovePath.Add(next);

            while (next != cell)
            {
                next = FindNextStep(next, ref cell);
                MovePath.Add(next);
            }
        }

        public void ClearPath()
        {
            PlayerLocation.ClearPath();
            for (int i = 0; i < MovePath.Count; i++)
            {
                MovePath[i].ClearPath();
            }
            MovePath.Clear();
        }

        private MapCellComponent FindNextStep(MapCellComponent start, ref MapCellComponent destination)
        {
            var fromCoords = start.AsPoint();
            var toCoords = destination.AsPoint();
            var cellDir = fromCoords
                .DetectDirection(toCoords)
                .ToMapCell(fromCoords, toCoords);

            var next = start.GetPropertyExpr<MapCellComponent>(cellDir.ToString());
            //if (!next.Visible)
            //{
            //    destination = next;
            //}

            start.SetPathOut(cellDir);

            next.SetPathIn(cellDir.Opposite());

            return next;
        }
    }
}
