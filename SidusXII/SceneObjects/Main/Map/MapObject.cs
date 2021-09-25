using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Gamepad;
using Dungeon.Control.Keys;
using Dungeon.Control.Pointer;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Tiled;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using SidusXII.Objects.Map;
using SidusXII.SceneObjects.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SidusXII.SceneObjects.Main.Map
{
    public class MapObject : SceneControl<LocationMap>
    {
        public const double TileSize = 205;

        public override bool CacheAvailable => false;

        public int YScroll { get; set; } = 5;

        public MapObject(LocationMap component) : base(component, false)
        {
            //Image = "GUI/Planes/maphd.png".AsmImg();

            Width = 1600;
            Height = 710;
            Scale = .5;

            BuildMap();
            SetCameraPosition();

            //this.Left = this.Width / 2 - 1600 / 2 ;
        }

        protected override ControlEventType[] Handles => new ControlEventType[]{
            ControlEventType.MouseWheel,
            ControlEventType.Key,
            ControlEventType.GamePadStickMoves
        };

        public override bool AllKeysHandle => true;

        MapCell CellFocus;
        Point CursorPosition;

        public override void StickMoveOnce(Direction direction, GamePadStick stick)
        {
            if (stick == GamePadStick.RightStick)
            {
                int x = 0;
                int y = 0;

                switch (direction.OppositeX())
                {
                    case Direction.Up:
                        y--;
                        break;
                    case Direction.UpLeft:
                        y--;
                        x--;
                        break;
                    case Direction.Down:
                        y++;
                        break;
                    case Direction.DownLeft:
                        x--;
                        break;
                    case Direction.Left:
                        x--;
                        break;
                    case Direction.Right:
                        x++;
                        break;
                    case Direction.UpRight:
                        y--;
                        break;
                    case Direction.DownRight:
                        y++;
                        x++;
                        break;
                    default:
                        return;
                }

                CellFocus?.Unselect();

                CursorPosition.X += x;
                CursorPosition.Y += y;

                var loc = $"{CursorPosition.X},{CursorPosition.Y}";

                var cell = Cells[loc];

                CellFocus = cell;
                CellFocus.Select();
            }

            base.StickMove(direction, stick);
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.Down)
            {
                this.Top--;
            }

            base.KeyDown(key, modifier, hold);
        }

        public override void MouseWheel(MouseWheelEnum mouseWheelEnum)
        {
            if (mouseWheelEnum == MouseWheelEnum.Down)
            {
                YScroll++;
            }
            else
            {
                YScroll--;
            }

            base.MouseWheel(mouseWheelEnum);
        }

        List<List<MapCell>> Rows = new List<List<MapCell>>();
        List<List<MapCell>> Columns = new List<List<MapCell>>();

        Dictionary<string, MapCell> Cells = new Dictionary<string, MapCell>();

        private void SetCameraPosition()
        {
            //double left = 0;

            //if (Component.Location.Y % 2 != 0)
            //{
            //    left += 167;
            //}

            //this.Left -= Component.Location.X * (TileSize * .5);// -Width/* - Width / 2*/;
            //this.Left /= 3;
            //this.Top -= Component.Location.Y * (TileSize*.5);// -Top/* - Height / 2*/;
            //this.Top /= 3;

            var cell = Cells[Component.Location.X + "," + Component.Location.Y];

            this.Top -= cell.Top;
            this.Left -= cell.Left;

            this.Top += 710*.9;
            this.Left += 1600*.9;

            CursorPosition = Component.Location.Copy();
        }

        private void BuildMap()
        {
            var w = 167;///3;
            var h = 192;///3;

            var tiled = Component.TiledMap;

            var examplelayer = tiled.Layers.FirstOrDefault();

            var tileCount = examplelayer.Tiles.Count;

            var layerWidth = examplelayer.width;
            var layerHeight = examplelayer.height;

            var coefficient = 0;

            var x = 0;
            var y = YScroll;

            bool odd = false;

            //11 rows

            var rows = 11;
            var row = 0;

            List<MapCell> rowTiles = new List<MapCell>();

            Columns.AddRange(Enumerable.Range(0, layerWidth).Select(x => new List<MapCell>()));
            Rows.AddRange(Enumerable.Range(0, layerHeight).Select(x => new List<MapCell>()));

            for (int i = 0; i < tileCount; i++)
            {
                var gameX = x / w;
                var gameY = row;

                var cell = new MapCell()
                {
                    Width = TileSize,
                    Height = TileSize,
                    Left = x /*+ 400*/,
                    Top = y /*+ 50*/,
                    DrawOutOfSight = true,
                    //Scale = .4,
                    MapPosition = new Dungeon.Types.Point(gameX, gameY)
                };

                var tiles = tiled.Layers.Select(x => x.Tiles[i]);

                foreach (var tile in tiles)
                {
                    if (tile.FileName.IsNotEmpty())
                    {
                        if (tile.Layer.name == LocationMap.SpawnLayerName && Component.Location.IsDefault)
                        {
                            Component.Location = new Dungeon.Types.Point(gameX, gameY);
                            continue;
                        }

                        //img.Image = $"Tiles/{tile.FileName}".AsmImg();
                        var img = new ImageObject($"Tiles/{tile.FileName}".AsmImg());

                        if (tile.FlippedHorizontally && tile.FlippedVertically)
                        {
                            img.Flip = Dungeon.View.Enums.FlipStrategy.Both;
                        }
                        else if (tile.FlippedHorizontally)
                        {
                            img.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;
                        }
                        else if (tile.FlippedVertically)
                        {
                            img.Flip = Dungeon.View.Enums.FlipStrategy.Vertically;
                        }

                        cell.AddTile(img);
                    }
                }

                if (!Component.Location.IsDefault && cell.MapPosition.Equals(Component.Location))
                {
                    cell.PlayerCell = true;
                }

                this.AddChild(cell);

                Cells.Add($"{gameX},{gameY}", cell);

                Rows[row].Add(cell);
                Columns[x/w].Add(cell);

                x += w + coefficient;

                if (y / h == layerHeight)
                {
                    y = 0;
                }

                if (x / w == layerWidth)
                {
                    odd = !odd;
                    x = 0;
                    y += h;
                    y -= 46;
                    if (odd)
                    {
                        x = w / 2;
                    }
                    row++;
                }
            }

            var last = Cells.Last();
            this.Height = last.Value.Top + h;
            this.Width = last.Value.Left + w;
        }
    }
}