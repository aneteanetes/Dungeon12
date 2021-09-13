using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class ObjectListBox : EmptySceneControl
    {
        public ObjectListBox()
        {
            this.Image = "GUI/Planes/list_back.png".AsmImg();
            this.Width = 368;
            this.Height = 300;
        }
    }
}