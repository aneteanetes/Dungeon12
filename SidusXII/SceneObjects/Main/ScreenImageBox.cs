using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class ScreenImageBox : EmptySceneControl
    {
        public ScreenImageBox()
        {
            this.Image = "GUI/Planes/perks_back.png".AsmImg();
            this.Width = 368;
            this.Height = 368;

            this.AddChild(new ImageObject("Screens/autumn_tree.jpg".AsmImg())
            {
                Width=350,
                Height=350,
                Left=9,
                Top=9
            });
        }
    }
}