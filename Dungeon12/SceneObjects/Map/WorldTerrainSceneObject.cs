using Dungeon.Drawing.SceneObjects;
using Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon12.SceneObjects.Map
{
    internal class WorldTerrainSceneObject : SceneControl<Entities.Map.World>
    {
        public WorldTerrainSceneObject(Entities.Map.World component) : base(component)
        {
            var tileLayer = component.Map.Layers.FirstOrDefault(x => x.name == "Tiles");

            var size = 210;

            foreach (var tile in tileLayer.Tiles)
            {
                if (tile.FileName.IsNotEmpty())
                {
                    this.AddChild(new ImageObject(tile.FileName)
                    {
                        Width = size,
                        Height = size,
                        Left = tile.Position.X * size,
                        Top = tile.Position.Y * size
                    });
                }
            }
        }
    }
}
