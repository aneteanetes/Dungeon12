using Dungeon;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class StatusBar : EmptySceneControl
    {
        public StatusBar()
        {
            this.Image = "GUI/Planes/stat_bar.png".AsmImg();
            this.Width = 1600;
            this.Height = 33;
        }
    }
}