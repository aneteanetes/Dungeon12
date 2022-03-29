using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.RegionScreen
{
    public class TextWindow : SceneControl<GameLog>
    {
        public TextWindow(GameLog component) : base(component)
        {
            Image = "UI/layout/textchat.png".AsmImg();
            this.Width = 412;
            this.Height = 179;
            this.Left = 9;
            this.Top = 882;

            var container = this.AddChild(new TextContainer(Component));
            component.OnPush = container.AddText;
        }

        private class TextContainer : SceneControl<GameLog>
        {
            public TextContainer(GameLog component) : base(component)
            {
                this.Width = 400;
                this.Height = 175;
                this.Left = 7;
                this.Top = -2;
                
                this.Text = "Загрузка...".AsDrawText().FrizQuad().InSize(10).InColor(Global.CommonColorLight).WithWordWrap();
            }

            public void AddText(string text)
            {
                this.Text.AddLine(text);
            }
        }
    }
}