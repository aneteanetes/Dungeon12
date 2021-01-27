using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.GUI.Main
{
    public class Minimap : EmptySceneObject
    {
        public Minimap()
        {
            this.AddChild(new DarkRectangle()
            {
                Width = 244,
                Height = 244,
                Left = 6,
                Top = 6,
                Opacity = 1
            });
            this.Image = "GUI/Planes/mini_map.png".AsmImg();
            this.Width = 256;
            this.Height = 256;
        }
    }
}
