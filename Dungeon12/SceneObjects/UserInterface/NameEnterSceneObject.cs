namespace Dungeon12.SceneObjects.UserInterface
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon12;
    using Dungeon12.Entities;
    using Dungeon12.Functions.ObjectFunctions;
    using Dungeon12.SceneObjects;
    using Dungeon12.SceneObjects.UserInterface.Common;
    using System;

    public class NameEnterSceneObject : SceneControl<Hero>
    {
        private Action<string> yes;
        private TextInputControl textInput;

        private TextControl validationDisplay;

        public NameEnterSceneObject(Hero component, Action<string> yes = null, Action no=null):base(component)
        {
            Global.Freezer.Freeze(this);
            this.Width = 1000;
            this.Height = 800;
            this.AddChild(new ImageObject("Other/namescroll.png".AsmImg()));

            var title = this.AddTextCenter("Свидетельство".AsDrawText().Gabriela().InSize(46).InColor(ConsoleColor.Black), vertical: false);
            title.Top = 143;


            var txt = @"Я,                      , уведомляю службу порядка
Острова Веры и мэрию города Туманный, в том, 
что являюсь официальным представителем, 
явившимся для расследования дела о массовых 
убийствах в городе Туманный и обязуюсь 
выполнять все возможное для того что бы найти 
убийцу.";

            var text = this.AddTextCenter(txt.AsDrawText().Gabriela().InSize(23).InColor(ConsoleColor.Black));

            var heroName = Global.Game.Party.Hero1.Name;

            textInput = this.AddChild(new TextInputControl(heroName.AsDrawText().InSize(23).Gabriela().InColor(ConsoleColor.Black), 9, true,autofocus:false));
            textInput.OnTyping += ValidateShow;
            textInput.Top = text.Top;
            textInput.Left = text.Left + 25;

            if (heroName.IsNotEmpty())
            {
                textInput.Value = heroName;
            }


            var btn = this.AddChild(new BrownButton("Подписать"));
            btn.Top = 560;
            btn.Left = 500;
            btn.OnClick = SignIn;

            var close = this.AddChild(new BrownButton75("X"));
            close.Top = 560;
            close.Left = btn.Left+btn.Width+10;
            close.OnClick = Close;
            //text.Top = 143;

            //this.yes = yes;
            //var enterName = new DrawText("В данном документе вам требуется", new DrawColor(ConsoleColor.White))
            //{
            //    Size = 50
            //}.Triforce();

            //var text = AddTextCenter(enterName, true, false);
            //text.Top += 0.5;

            //validationDisplay = AddTextCenter(new DrawText("Имя не может быть меньше 5 символов!", new DrawColor(ConsoleColor.Red)).Montserrat());
            //validationDisplay.Visible = false;
            //validationDisplay.Top -= 2.5;

            //textInput = new TextInputControl(new DrawText("", new DrawColor(ConsoleColor.White)) { Size = 30 }.Triforce(), 14, true, width: 11, height: 1.5);
            //textInput.OnTyping += ValidateShow;
            //textInput.Top = Height / 2 - textInput.Height / 2;
            //textInput.Left += 0.75;

            //var yesBtn = new MetallButton("Назад");
            //yesBtn.Top = Top + Height - 1.5;
            //yesBtn.Left -= yesBtn.Width / 4;
            //yesBtn.OnClick = no;

            //var noBtn = new MetallButton("Далее");
            //noBtn.Top = Top + Height - 1.5;
            //noBtn.Left = Width / 2;
            //noBtn.OnClick = OnYes;

            //this.AddChild(yesBtn);
            //this.AddChild(noBtn);
            //AddChild(textInput);
        }

        private void SignIn()
        {
            var value = textInput.Value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                Global.Game.Party.Hero1.Name = value;
                Global.Game.Location.Polygon.P1.Load(new Entities.Map.Polygon
                {
                    Name = "Происхождение",
                    Icon = "mountains.png",
                    Function = nameof(SelectOriginFunction)
                });
                this.Close();
            }
        }

        private void Close()
        {
            this.Destroy?.Invoke();
            Global.Freezer.Unfreeze();
        }

        private void ValidateShow(string data)
        {
            if (validationDisplay != default)
                validationDisplay.Visible = string.IsNullOrEmpty(data) || data.Length < 5;
        }

        private void OnYes()
        {
            var value = textInput.Value;
            if (!string.IsNullOrWhiteSpace(value) && value.Length >= 5)
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
