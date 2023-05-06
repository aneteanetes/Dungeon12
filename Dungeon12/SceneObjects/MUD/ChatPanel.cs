using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Journal;

namespace Dungeon12.SceneObjects.MUD
{
    internal class ChatPanel : SceneObject<GameLog>
    {
        private TextObject textBox;

        public ChatPanel(GameLog component) : base(component)
        {
            this.Width=1120;
            this.Height=200;

            this.AddBorderBack();

            var text = "".AsDrawText()
                        .InBold()
                        .Calibri()
                        .InSize(20)
                        .IsNew(true)
                        .WithWordWrap()
                        .InColor(Global.CommonColorLight);

            textBox = this.AddChild(new TextObject(text) { Left=7, Width=Width-7, Top=5, Height=Height-5 });

            component.Records.ForEach(OnPush);
            component.OnPush+=OnPush;
        }

        private void OnPush(GameLogMessage message)
        {
            var height = this.MeasureText(textBox.Text,this).Y;
            if (height>=this.Height-10)
            {
                textBox.Text.RemoveLine(0);
            }

            textBox.Text.AddLine(message.ToString());
        }
    }
}