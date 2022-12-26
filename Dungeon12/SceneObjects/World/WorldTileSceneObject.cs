using Dungeon.SceneObjects;
using Dungeon.Types;

namespace Dungeon12.SceneObjects.World
{
    public class WorldTileSceneObject : EmptySceneObject
    {
        public override bool Updatable => false;

        public WorldTileSceneObject()
        {
            //Color = DrawColor.DarkBlue;
            Width = WorldSettings.cellSize;
            Height = WorldSettings.cellSize;
            this.Image = "terrain.png";
            //this.Blur = true;
            

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

            this.ImageRegion=ImageRegion.SetCoords(offsetX * 32, offsetY * 32);
        }
    }
}