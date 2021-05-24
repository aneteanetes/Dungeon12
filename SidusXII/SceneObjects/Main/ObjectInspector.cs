using Dungeon;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class ObjectInspector : EmptySceneControl
    {
        public ObjectInspector()
        {
            this.Image = "GUI/Planes/char_info.png".AsmImg();
            this.Width = 368;
            this.Height = 499;
        }
    }
}