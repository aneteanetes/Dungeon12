using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class PerksView : EmptySceneControl
    {
        public PerksView()
        {
            this.Image = "GUI/Planes/perks_back.png".AsmImg();
            this.Width = 368;
            this.Height = 368;

            this.AddChild(new ImageObject("GUI/Planes/perks_grid.png".AsmImg()));
        }
    }
}