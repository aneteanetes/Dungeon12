using Dungeon.Drawing;
using Dungeon.Types;

namespace Dungeon12.SceneObjects.World
{
    public class WorldPartySceneObject : WorldTileSceneObject
    {
        public WorldPartySceneObject()
        {
            Color = DrawColor.White;
            Width = WorldSettings.cellSize;
            Height = WorldSettings.cellSize;
            Image = "units1.png";

            ImageRegion = new Square()
            {
                Width = 32,
                Height = 32,
                X=1*32,
                Y=3*32
            };

            this.Light=new Light()
            {
                Color = new DrawColor(245, 132, 66),
                Range=1
            };
        }
        public Dot Coords { get; set; } = new Dot();
    }
}
