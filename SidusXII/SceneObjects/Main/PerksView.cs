using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class PerksView : EmptySceneControl
    {
        public PerksView()
        {
            this.Image = "GUI/Planes/actions_back.png".AsmImg();
            this.Width = 368;
            this.Height = 208;

            this.AddChild(new ImageObject("GUI/Planes/actions_grid.png".AsmImg()));
        }
    }
}