using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.GUI.Main
{
    public class ResourceBar : EmptySceneControl
    {
        public ResourceBar(bool hp)
        {
            this.Image = "GUI/Planes/hp_bar.png".AsmImg();
            this.Width = 146;
            this.Height = 19;

            this.AddChild(new ImageControl($"GUI/Planes/hp_{(hp ? "hp" : "mana")}bar.png".AsmImg())
            {
                Width = 146,
                Height = 19
            });
        }
    }
}
