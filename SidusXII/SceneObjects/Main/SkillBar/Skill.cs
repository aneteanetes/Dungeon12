using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class Skill : EmptySceneControl
    {
        public Skill()
        {
            this.Image = "GUI/Parts/skillframe.png".AsmImg();
            this.Width = 92;
            this.Height = 92;
        }
    }
}