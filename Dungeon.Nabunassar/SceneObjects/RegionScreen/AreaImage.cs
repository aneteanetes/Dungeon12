using Dungeon;
using Dungeon.SceneObjects;

namespace Nabunassar.SceneObjects.RegionScreen
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
