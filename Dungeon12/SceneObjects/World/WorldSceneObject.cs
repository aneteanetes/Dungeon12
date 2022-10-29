using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.Tiled;
using Dungeon.Types;
using System;
using System.Linq;

namespace Dungeon12.SceneObjects.World
{
    public class WorldSceneObject : EmptySceneControl
    {
        CoordDictionary<WorldTileSceneObject> views = new CoordDictionary<WorldTileSceneObject>();
        CoordDictionary<TiledPolygon> tiles = new CoordDictionary<TiledPolygon>();
        private TiledMap map;

        public WorldSceneObject(TiledMap map)
        {
            this.map = map;
            Width = WorldSettings.cellSize * WorldSettings._width;
            Height = WorldSettings.cellSize * WorldSettings._height;

            for (int y = 0; y < WorldSettings._height; y++)
            {
                for (int x = 0; x < WorldSettings._width; x++)
                {
                    views.Add(x, y, AddChild(new WorldTileSceneObject()
                    {
                        Left = x * WorldSettings.cellSize,
                        Top = y * WorldSettings.cellSize,
                        x = x,
                        y = y
                    }));
                }
            }

            var start = map.Layers.Last().Tiles.FirstOrDefault(x => x.Gid != 0);

            pointer = AddChildCenter(new WorldPartySceneObject(Global.Game.Party));
            //pointer.Load(start.TileOffsetX, start.TileOffsetY);

            var back = map.Layers.FirstOrDefault(x => x.name == "Background");

            back.Tiles.Where(x => x.Gid != 0)
                .ForEach(t =>
                {
                    var pos = t.Position;
                    tiles.Add(pos.Xi, pos.Yi, t);

                });

            UpdateView(current = start.Position);
            Player = new Dot
            {
                X = current.X,
                Y = current.Y
            };

            this.AddBorder(0);
        }

        WorldPartySceneObject pointer = null;
        Dot current = default;
        Dot Player = default;
        Dot offset = new Dot(0, 0);

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Key };
        protected override Key[] KeyHandles => new Key[] { Key.W, Key.A, Key.S, Key.D, Key.Up, Key.Down };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (hold)
                return;

            if (key == Key.A)
            {
                if (Math.Abs(offset.X)<WorldSettings._widthOffset)
                    offset.X--;
                MoveParty(Player.Move(-1));
            }

            if (key == Key.D)
            {
                if (Math.Abs(offset.X)<WorldSettings._widthOffset)
                    offset.X++;
                MoveParty(Player.Move(1));
            }

            if (key == Key.W)
            {
                if (Math.Abs(offset.Y)<WorldSettings._heightOffset)
                    offset.Y--;
                MoveParty(Player.Move(0,-1));
            }

            if (key == Key.S)
            {
                if (Math.Abs(offset.Y)<WorldSettings._heightOffset)
                    offset.Y++;
                MoveParty(Player.Move(0,1));
            }

            base.KeyDown(key, modifier, hold);
        }

        private void MoveParty(Direction direction)
        {
            if (Math.Abs(offset.X)==WorldSettings._widthOffset || Math.Abs(offset.Y)==WorldSettings._heightOffset)
                return;

            switch (direction)
            {
                case Direction.Up:
                    pointer.Top-=WorldSettings.cellSize;
                    break;
                case Direction.Down:
                    pointer.Top+=WorldSettings.cellSize;
                    break;
                case Direction.Left:
                    pointer.Left-=WorldSettings.cellSize;
                    break;
                case Direction.Right:
                    pointer.Left+=WorldSettings.cellSize;
                    break;
                case Direction.UpLeft:
                    pointer.Top-=WorldSettings.cellSize;
                    pointer.Left-=WorldSettings.cellSize;
                    break;
                case Direction.UpRight:
                    pointer.Top-=WorldSettings.cellSize;
                    pointer.Left+=WorldSettings.cellSize;
                    break;
                case Direction.DownLeft:
                    pointer.Top+=WorldSettings.cellSize;
                    pointer.Left-=WorldSettings.cellSize;
                    break;
                case Direction.DownRight:
                    pointer.Top+=WorldSettings.cellSize;
                    pointer.Left+=WorldSettings.cellSize;
                    break;
                default:
                    break;
            }

            //UpdateLight();
        }

        public override void Update(GameTimeLoop gameTime)
        {
            if (Math.Abs(offset.X)==WorldSettings._widthOffset)
            {
                if (offset.X<0)
                {
                    current.X--;
                    offset.X++;
                }
                if (offset.X>0)
                {
                    current.X++;
                    offset.X--;
                }
                UpdateView(current);
            }

            if(Math.Abs(offset.Y)==WorldSettings._heightOffset)
            {
                if (offset.Y>0)
                {
                    current.Y++;
                    offset.Y--;
                }
                if (offset.Y<0)
                {
                    current.Y--;
                    offset.Y++;
                }
                UpdateView(current);
            }

            //if (!Player.Equals(current))
            //{
            //    current.X = Player.X;
            //    current.Y = Player.Y;
            //    UpdateView(current);
            //}

            //UpdateLight();
        }

        private void UpdateView(Dot pos)
        {
            var back = map.Layers.FirstOrDefault(x => x.name == "Background");

            var xx = pos.Xi - WorldSettings._width / 2;
            var yy = pos.Yi - WorldSettings._height / 2;

            for (int y = 0; y < WorldSettings._height; y++)
            {
                for (int x = 0; x < WorldSettings._width; x++)
                {
                    var ofX = 12;
                    var ofY = 10;

                    var tile = tiles[xx, yy];
                    if (tile != default)
                    {
                        ofX = tile.TileOffsetX;
                        ofY = tile.TileOffsetY;
                    }

                    var view = views[x, y];
                    view.Load(ofX, ofY);

                    xx++;
                }
                xx = pos.Xi - WorldSettings._width / 2;
                yy++;
            }
        }
    }
}