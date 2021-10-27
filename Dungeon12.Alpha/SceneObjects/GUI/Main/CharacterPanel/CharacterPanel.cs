using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.GUI.Main
{
    public class CharacterPanel : EmptySceneControl
    {
        public CharacterPanel()
        {
            this.Image = "GUI/Planes/char_bar.png".AsmImg();
            this.Width = 244;
            this.Height = 91;

            this.AddChild(new ImageObject("GUI/Planes/char_avatar.png".AsmImg())
            {
                Width = 75,
                Height = 74,
                Left = 8,
                Top = 8
            });

            this.AddChild(new ResourceBar(true)
            {
                Left=88,
                Top=38
            });

            this.AddChild(new ResourceBar(false)
            {
                Left = 88,
                Top = 60
            });
        }
    }
}
