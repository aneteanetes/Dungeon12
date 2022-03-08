using Dungeon;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.RegionScreen
{
    public class AreaImage : EmptySceneObject
    {
        public AreaImage()
        {
            this.Image = "Regions/FaithIsland.png".AsmImg();
            this.Width = 1380;
            this.Height = 850;
            this.Left = 7;
            this.Top = 12;
        }
    }
}
