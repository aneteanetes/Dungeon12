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
        public WorldTerrainSceneObject(Entities.Map.World component) : base(component)
        {
            var tileLayer = component.Map.Layers.FirstOrDefault(x => x.name == "Tiles");

            var size = Variables.Get("GlobalMapTileSize", 210);

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
                top += size-55;
            }

            this.Width = left;
            this.Height = top;
        }
    }
}
