using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class ExpBar : EmptySceneControl
    {
        public ExpBar()
        {
            this.Image = "GUI/Parts/exp.png".AsmImg();
            this.Width = 402;
            this.Height = 16;

            this.AddChild(new ImageObject("GUI/Parts/exp_bar.png".AsmImg()));
        }
    }
}