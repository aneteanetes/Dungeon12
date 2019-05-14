namespace Rogue.Drawing.SceneObjects.Dialogs
{
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.UI;
    using System;

    public class TypeNameDialogue : VerticalWindow
    {
        private Action<string> yes;
        private TextInput textInput;

        public TypeNameDialogue(Action<string> yes, Action no)
        {
            this.yes= yes;
            var enterName = new DrawText(" Введите имя", new DrawColor(ConsoleColor.White))
            {
                Size = 50
            };

            var text = this.AddTextCenter(enterName, true,false);
            text.Top += 0.5;

            textInput = new TextInput(new DrawText("", new DrawColor(ConsoleColor.White)) { Size = 30 }.Montserrat(), 14);
            textInput.Top = this.Height / 2 - textInput.Height / 2;
            textInput.Left += 0.75;

            var yesBtn = new SmallMetallButtonControl("Y");
            yesBtn.Top = this.Top+this.Height - 3;
            yesBtn.Left = 1;
            yesBtn.OnClick = OnYes;

            var noBtn = new SmallMetallButtonControl("N");
            noBtn.Top = this.Top + this.Height - 3;
            noBtn.Left = this.Left+this.Width-1-noBtn.Width;
            noBtn.OnClick = no;

            this.AddChild(yesBtn);
            this.AddChild(noBtn);
            this.AddChild(textInput);
        }

        private void OnYes()
        {
            var value = textInput.Value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                yes?.Invoke(value);
            }
        }
    }
}
