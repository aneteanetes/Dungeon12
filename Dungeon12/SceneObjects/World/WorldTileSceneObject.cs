using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Types;

namespace Dungeon12.SceneObjects.World
{
    public class WorldTileSceneObject : ImageSceneObject
    {
        public override bool Updatable => false;

        public WorldTileSceneObject()
        {
            //Color = DrawColor.DarkBlue;
            Width = WorldSettings.cellSize;
            Height = WorldSettings.cellSize;
            Image = "terrain.png";
            
            ImageRegion = new Square()
            {
                Width = 32,
                Height = 32
            };
        }

        public int x { get; set; }

        public int y { get; set; }

        public void Load(int offsetX, int offsetY)
        {
            if (offsetX == 0 && offsetY == 0)
            {
                offsetX = 12;
                offsetY = 10;
            }

            ImageRegion.X = offsetX * 32;
            ImageRegion.Y = offsetY * 32;
        }
    }
}
