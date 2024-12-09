using Dungeon.SceneObjects;
using Dungeon12.SceneObjects.Base;
using Dungeon12.SceneObjects.UserInterface.Common;

namespace Dungeon12.SceneObjects.HUD
{
    internal class TextPanel : EmptySceneObject
    {
        public TextPanel(string text, int textSize = 20, bool autoresize = true)
        {
            var drawtext = text.Calibri().InColor(Global.CommonColorLight).InSize(textSize).WithWordWrap();
            var m = this.MeasureText(drawtext, this);

            this.Height = m.Y + 50;
            this.Width = m.X + 50;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/bord5.png",
                Size = 16,
                Padding = 2
            });

            this.AddTextCenter(drawtext);
        }
    }
}