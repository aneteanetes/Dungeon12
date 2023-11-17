using Dungeon.Drawing.SceneObjects;
using Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon.Varying;

namespace Dungeon12.SceneObjects.Map
{
    internal class WorldTerrainSceneObject : SceneControl<Entities.Map.World>
    {
        public WorldTerrainSceneObject(Entities.Map.World component) : base(component)
        {
            var tileLayer = component.Map.Layers.FirstOrDefault(x => x.name == "Tiles");

            Variables.Set("GlobalMapTileSize", 210);
            var size = 210;

            List<ImageObject> images= new List<ImageObject>();

            foreach (var tile in tileLayer.Tiles)
            {
                if (tile.FileName.IsNotEmpty())
                {
                    images.Add(this.AddChild(new ImageObject(tile.FileName)
                    {
                        Width = size,
                        Height = size,
                        Left = tile.Position.X * size,
                        Top = tile.Position.Y * size
                    }));
                }
            }

            void resize()
            {
                images.ForEach(i =>
                {
                    i.Height = Variables.Get<int>("GlobalMapTileSize");
                    i.Width = Variables.Get<int>("GlobalMapTileSize");
                });
            }

            Variables.OnChange<int>("GlobalMapTileSize", resize);
        }
    }
}
