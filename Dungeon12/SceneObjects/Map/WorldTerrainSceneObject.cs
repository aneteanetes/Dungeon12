using Dungeon.Drawing.SceneObjects;
using Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon.Varying;
using Dungeon.SceneObjects.Tilemaps;

namespace Dungeon12.SceneObjects.Map
{
    internal class WorldTerrainSceneObject : SceneControl<Entities.Map.World>
    {
        int tilePlusTop;

        public WorldTerrainSceneObject(Entities.Map.World component) : base(component)
        {
            Scale = 1;
            var tileLayer = component.Map.Layers.FirstOrDefault(x => x.name == "Tiles");

            var size = Variables.Get("GlobalMapTileSize", 210);
            tilePlusTop = size - Variables.Get("GlobalMapTileTopCut", 55);

            double left = 0;
            double top = 0;

            for (int y = 0; y < tileLayer.TilesArray.Count; y++)
            {
                var row = tileLayer.TilesArray[y];
                left = 0;

                if (y % 2 == 0)
                {
                    left -= Variables.Get("GlobalMapLeftOddOffset", 85d);
                }

                for (int x = 0; x < row.Count; x++)
                {
                    var tile = row[x];

                    if (tile.FileName.IsNotEmpty())
                    {
                        this.AddChild(new ImageControl(tile.FileName)
                        {
                            Width = size,
                            Height = size,
                            Left = left,
                            Top = top
                        });
                    }
                    left += Variables.Get("GlobalMapTileLeftPlus", 172);
                }
                top += tilePlusTop;
            }

            this.Width = left;
            this.Height = top;
        }

        public void Move(int x, int y)
        {
            x -= 5;
            y -= 3;
            if (y % 2 == 0)
            {
                this.Left -= Variables.Get<double>("GlobalMapLeftOddOffset");
            }

            this.Left -= x * Variables.Get<int>("GlobalMapTileLeftPlus");
            this.Top -= y * tilePlusTop;
        }
    }
}
