using Dungeon;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.RegionScreen
{
    internal class AreaTitle : EmptySceneObject
    {
        private readonly TextObject text; 

        public AreaTitle(string title)
        {
            this.Width = 400;
            this.Height = 30;

            this.AddBorderBack();

            text = this.AddTextCenter(title.AsDrawText().Gabriela().InColor(Global.CommonColorLight).InSize(20));
        }

        public void SetText(string title)
        {
            text.Text.SetText(title);
            this.CenterChild(text);
        }
    }
}