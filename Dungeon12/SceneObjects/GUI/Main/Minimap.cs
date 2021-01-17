using Dungeon;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.GUI.Main
{
    public class Minimap : EmptySceneObject
    {
        public Minimap()
        {
            this.Image = "GUI/Planes/mini_map.png".AsmImg();
            this.Width = 324;
            this.Height = 324;
        }
    }
}
