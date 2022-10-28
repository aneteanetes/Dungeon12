using Dungeon.SceneObjects;
using Dungeon.Types;

namespace Dungeon12.SceneObjects.World
{
    internal class WorldCharacterSceneObject : EmptySceneObject
    {
        public WorldCharacterSceneObject(int xTilesetOffset, int yTilestOffset)
        {
            this.Width=16;
            this.Height=16;
            this.Image = "units1.png";
            this.ImageRegion = new Square() { X=xTilesetOffset, Y=yTilestOffset, Height=32, Width=32 };
        }
    }
}
