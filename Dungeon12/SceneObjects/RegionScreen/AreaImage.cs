using Dungeon;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.RegionScreen
{
    internal class AreaImage : EmptySceneObject
    {
        public AreaImage()
        {
            this.Image = "Regions/FaithIsland.png".AsmImg();
            this.Width = 1920;
            this.Height = 860;
        }
    }
}
