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
using SidusXII.Models.Map;
using SidusXII.SceneObjects.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SidusXII.SceneObjects.Main.Map
{
    public class MapSceneObject : SceneControl<MapComponent>
    {
        public const double TileSize = 205;

        public override bool CacheAvailable => false;

        public int YScroll { get; set; } = 5;

        public MapSceneObject(MapComponent component) : base(component, true)
        {
            Width = Global.Resolution.Width;
            Height = Global.Resolution.Height;
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

        MapCellSceneObject CellFocus;
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
                //Scale -= .1;
            }
            else
            {
                //Scale += .1;
            }

            base.MouseWheel(mouseWheelEnum);
        }

        List<List<MapCellSceneObject>> Rows = new List<List<MapCellSceneObject>>();
        List<List<MapCellSceneObject>> Columns = new List<List<MapCellSceneObject>>();

        Dictionary<string, MapCellSceneObject> Cells = new Dictionary<string, MapCellSceneObject>();

        private void SetCameraPosition()
        {
            var cellcomp = Component.Cells[Component.Location.X + "," + Component.Location.Y];
            var cellsceneobj = cellcomp.SceneObject;

            this.Top -= cellsceneobj.Top;
            this.Left -= cellsceneobj.Left;

            this.Top += 710 * .9;
            this.Left += 1600 * .9;

            CursorPosition = Component.Location.Copy();
        }

        private void BuildMap()
        {
            var w = 167;///3;
            var h = 192;///3;

            var lastTop = 0d;
            var lastLeft = 0d;

            foreach (var cellinfo in Component.Cells)
            {
                var coords = cellinfo.Key.Split(",", StringSplitOptions.RemoveEmptyEntries);
                var gamex = int.Parse(coords[0]);
                var gamey = int.Parse(coords[1]);

                var cell = new MapCellSceneObject(cellinfo.Value)
                {
                    Width = TileSize,
                    Height = TileSize,
                    DrawOutOfSight = true,
                    MapPosition = new Point(gamex, gamey),
                    HighLevelComponent=true,


                    Left = 0 /*+ 400*/,
                    Top = 0 /*+ 50*/,
                };
                Cells.Add($"{gamex},{gamey}", cell);

                //set left
                if (cell.Left == 0)
                {
                    if (gamey % 2 != 0)
                    {
                        cell.Left += w / 2;
                    }
                    cell.Left += gamex * w;
                }

                //set top
                if (cell.Top == 0)
                {
                    cell.Top = gamey * h;
                    if (gamey > 0)
                        cell.Top -= 46*gamey;
                }

                foreach (var tileinfo in cellinfo.Value.Tiles)
                {
                    var img = new ImageObject(tileinfo.AsmImgTile)
                    {
                        Flip = tileinfo.Flip
                    };
                    cell.AddTile(img);
                }

                this.AddChild(cell);
                lastTop = cell.Top;
                lastLeft = cell.Left;
            }

            this.Height = lastTop + h;
            this.Width = lastLeft + w;
        }
    }
}