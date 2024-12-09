using Dungeon12.SceneObjects.UserInterface.Common;

namespace Dungeon12.SceneObjects.HUD
{
    internal class DialogueBox : EmptySceneControl, IAutoUnfreeze
    {
        public DialogueBox(string left, string right, string text, bool autoresize=true)
        {
            this.Width = 525;

            var drawtext = text.Gabriela().InColor(Global.CommonColorLight).InSize(22).WithWordWrap();

            this.Height = this.MeasureText(drawtext, this).Y + 100;

            this.AddBorderBack(.95);

            this.AddTextCenter(drawtext, vertical: false).Top = 10;

            this.AddChild(new ClassicButton(left)
            {
                OnClick = () => OnLeft?.Invoke(),
                Left = 25 / 2,
                Top = this.Height - 75
            });
            this.AddChild(new ClassicButton(right)
            {
                OnClick = () => OnRight?.Invoke(),
                Left = 250 + 25 / 2,
                Top = this.Height - 75
            });
        }

        public Action OnLeft { get; set; }

        public Action OnRight { get; set; }
    }
}
