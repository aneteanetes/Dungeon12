using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Tilemaps;
using Dungeon.Tiled;
using Dungeon.Types;
using ProjectMercury;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects
{
    internal class WorldSceneObject : EmptySceneControl
    {
        List<WorldTileSceneObject> views = new List<WorldTileSceneObject>();
        private TiledMap map;

        public WorldSceneObject(TiledMap map)
        {
            this.map=map;
            this.Width = 64*9;
            this.Height = 64*9;

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    views.Add(this.AddChild(new WorldTileSceneObject()
                    {
                        Left=x*64,
                        Top=y*64,
                        x=x,
                        y=y
                    }));
                }
            }

            pointer = this.AddChildCenter(new WorldTileSceneObject());

            var start = map.Layers.Last().Tiles.FirstOrDefault(x => x.Gid!=0);
            pointer.Load("terrain", start.TileOffsetX, start.TileOffsetY);


            UpdateView(current = start.Position);
            Player=current.Clone();
        }

        private void UpdateView(Point pos)
        {
            var back = map.Layers.FirstOrDefault(x => x.name=="Background");

            var xx = pos.X-4;
            var yy = pos.Y-4;

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    var tile = back.Tiles.FirstOrDefault(t => t.Position.X==xx && t.Position.Y==yy);

                    var view = views.FirstOrDefault(t => t.x==x && t.y==y);
                    view.Load("terrain", tile.TileOffsetX, tile.TileOffsetY);

                    xx++;
                }
                xx = pos.X-4;
                yy++;
            }
        }

        WorldTileSceneObject pointer = null;
        Point current = null;
        Point Player = null;

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Key };
        protected override Key[] KeyHandles => new Key[] { Key.W, Key.A, Key.S, Key.D };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (hold)
                return;

            if (key== Key.A)
                Player.X--;
            if (key== Key.D)
                Player.X++;
            if (key== Key.W)
                Player.Y--;
            if (key== Key.S)
                Player.Y++;

            base.KeyDown(key, modifier, hold);
        }

        public override void Update(GameTimeLoop gameTime)
        {
            if (!Player.Equals(current))
            {
                current = Player.Clone();
                UpdateView(current);
            }
        }

        private class WorldTileSceneObject : ImageSceneObject
        {
            public WorldTileSceneObject()
            {
                this.Width=64;
                this.Height=64;
            }

            public int x { get; set; }

            public int y { get; set; }

            private string image;

            public void Load(string imagePath, int offsetX, int offsetY)
            {
                if(offsetX==0 && offsetY==0)
                {
                    offsetX=12;
                    offsetY=10;
                }

                image=imagePath+".png";
                this.ImageRegion=new Dungeon.Types.Rectangle(offsetX*32, offsetY*32, 32, 32);
            }

            public override void Update(GameTimeLoop gameTime)
            {
                if (Image!=image)
                    this.Image=image;
            }
        }
    }
}