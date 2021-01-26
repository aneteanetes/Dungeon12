using Dungeon.Control.Keys;
using Dungeon.SceneObjects.Tilemaps;
using Dungeon12.World.Map;

namespace Dungeon12.SceneObjects.World
{
    public class WorldMapSceneObject : SceneControl<WorldMap>
    {
        public WorldMapSceneObject(WorldMap component) : base(component, true)
        {
            BuildTileMap();            
        }

        protected override Key[] KeyHandles => new Key[]
        {
             Key.B
        };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            AlphaBlend = !AlphaBlend;
            base.KeyDown(key, modifier, hold);
        }

        private void BuildTileMap()
        {
            this.TileMap = new TileMap();

            foreach (var layer in Component.Data.layers)
            {

                int x = 0;
                int y = 0;

                int yoffset = 0;
                int xoffset = 0;

                for (int i = 0; i < layer.data.Count; i++)
                {
                    var tileIndex = layer.data[i];

                    if (x > Component.Data.width - 1)
                    {
                        x = 0;
                        yoffset += Component.Data.tileheight / 2;
                        xoffset -= Component.Data.tilewidth / 2;
                        y++;
                    }

                    if (tileIndex == 0)
                    {
                        x++;
                        continue;
                    }

                    var fullWidth = Component.Data.width * Component.Data.tilewidth;

                    var tile = new TileWithTileset
                    {
                        Index = tileIndex,
                        Tileset = GetTileSource(tileIndex),
                        Left = (fullWidth / 2) + ((Component.Data.tilewidth/2) * x),
                        Top = (Component.Data.tileheight/2) * x
                    };
                    tile.Left += xoffset;
                    tile.Top += yoffset;
                    SetTilePosInTileSource(tile);

                    this.TileMap.Tiles.Add(tile);

                    x++;

                    tile.Build();
                }
            }
        }

        private void SetTilePosInTileSource(TileWithTileset tile)
        {
            var idx = tile.Index - 1;

            var row = idx / tile.Tileset.columns;
            var col = idx % tile.Tileset.columns;

            tile.X = col * tile.Tileset.tilewidth + col;
            tile.Y = row * tile.Tileset.tileheight + row;
        }

        private class TileWithTileset : Tile
        {
            public int Index { get; set; }

            Data.Region.Tileset _tileset;
            public Data.Region.Tileset Tileset
            {
                get => _tileset;
                set
                {
                    Source = value.image;
                    Width = value.tilewidth;
                    Height = value.tileheight;
                    _tileset = value;
                }
            }

            public void Build()
            {
                _tileset = default;
            }
        }

        private Data.Region.Tileset GetTileSource(int index)
        {
            foreach (var tileset in Component.Data.tilesets)
            {
                if (index >= tileset.firstgid && index <= tileset.tilecount)
                    return tileset;
            }

            return default;
        }

        public override bool Updatable => true;

        public override void Update()
        {
            base.Update();
        }
    }
}
