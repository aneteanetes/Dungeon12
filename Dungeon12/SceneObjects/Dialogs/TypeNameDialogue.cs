namespace Dungeon12.Drawing.SceneObjects.Dialogs
{
    using Dungeon.Drawing;
    using Dungeon12.Drawing.SceneObjects.UI;
    using Dungeon12.SceneObjects;
    using Dungeon.SceneObjects;
    using System;
    using Dungeon12;

    public class TypeNameDialogue : VerticalWindow
    {
        private Action<string> yes;
        private TextInputControl textInput;

        private TextControl validationDisplay;

        public TypeNameDialogue(Action<string> yes, Action no)
        {
            this.yes= yes;
            var enterName = new DrawText(" Введите имя", new DrawColor(ConsoleColor.White))
            {
                Size = 50
            }.Triforce();

            var text = this.AddTextCenter(enterName, true,false);
            text.Top += 0.5;
            
            validationDisplay = this.AddTextCenter(new DrawText("Имя не может быть меньше 5 символов!",new DrawColor(ConsoleColor.Red)).Montserrat());
            validationDisplay.Visible = false;
            validationDisplay.Top -= 2.5;

            textInput = new TextInputControl(new DrawText("", new DrawColor(ConsoleColor.White)) { Size = 30 }.Triforce(), 14,true,width:11,height:1.5);
            textInput.OnTyping += ValidateShow;
            textInput.Top = this.Height / 2 - textInput.Height / 2;
            textInput.Left += 0.75;
           
            var yesBtn = new MetallButtonControl("Далее");
            yesBtn.Top = this.Top+this.Height - 1.5;
            yesBtn.Left -= yesBtn.Width/4;
            yesBtn.OnClick = OnYes;

            var noBtn = new MetallButtonControl("Назад");
            noBtn.Top = this.Top + this.Height - 1.5;
            noBtn.Left = this.Width / 2;
            noBtn.OnClick = no;

            this.AddChild(yesBtn);
            this.AddChild(noBtn);
            this.AddChild(textInput);
        }

        private void ValidateShow(string data)
        {
            validationDisplay.Visible = string.IsNullOrEmpty(data) || data.Length < 5;
        }

        private void OnYes()
        {
            var value = textInput.Value;
            if (!string.IsNullOrWhiteSpace(value) && value.Length>=5)
            {
                yes?.Invoke(value);
            }
            else
            {
                validationDisplay.Visible = true;
            }
        }
    }
}
