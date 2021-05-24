using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class HpBar : EmptySceneControl
    {
        public HpBar()
        {
            this.Image = "GUI/Parts/hp.png".AsmImg();
            this.Width = 389;
            this.Height = 17;

            this.AddChild(new ImageObject("GUI/Parts/hp_bar.png".AsmImg()));
        }
    }
}