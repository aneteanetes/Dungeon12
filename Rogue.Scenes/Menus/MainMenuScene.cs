namespace Rogue.Scenes.Menus
{
    using System;
    using System.Linq;
    using Rogue.Control.Keys;
    using Rogue.Drawing.Controls;
    using Rogue.Drawing.Impl;
    using Rogue.Races.Perks;
    using Rogue.Scenes.Menus.Creation;
    using Rogue.Scenes.Scenes;

    public class MainMenuScene : GameScene<PlayerNameScene, Game.MainScene>
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        private Window window;

        public override void Draw()
        {
            var win = new Window
            {
                Direction= Direction.Vertical,
                Left = 16f,
                Top = 4,
                Width = 15,
                Height = 20
            };

            win.Append(new Title
            {
                Left = 3.6f,
                Top = -1f,
                Width = 8,
                Height = 2.4f,
                Label = new DrawText("Dungeon 12", ConsoleColor.Black) { Size = 30 }
            });

            win.Append(new Text
            {
                Left = 9f,
                Top = 1,
                DrawText = new DrawText("remastered", ConsoleColor.Red) { Size = 15 }
            });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 5,
                Width = 7,
                Height=2,
                Label = new DrawText("Новая игра", ConsoleColor.DarkRed) { Size = 30 }
            });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 8,
                Width = 7,
                Height = 2,
                Label = new DrawText("Быстрая игра", ConsoleColor.DarkRed) { Size = 30 },
                OnClick = () =>
                {
                    this.Player = Classes.All().Skip(1).First();
                    this.Player.Name = "Adventurer";
                    this.Player.Race = Race.Elf;
                    this.Player.Add<RacePerk>();

                    this.Switch<Game.MainScene>();
                }
            });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 11,
                Width = 7,
                Height = 2,
                Label = new DrawText("Создатели", ConsoleColor.DarkRed) { Size = 30 }
            });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 14,
                Width = 7,
                Height = 2,
                Label = new DrawText("Выход", ConsoleColor.DarkRed) { Size = 30 }
            });

            Drawing.Draw.RunSession(win);

            return;

            //var w = window = new Window();
            //w.AutoClear = false;
            //w.Animation.Frames = 2;
            //w.Speed = 10;
            //w.Border = Additional.BoldBorder;
            //w.BorderColor = ConsoleColor.DarkGray;

            ////w.Header = true;
            //w.Height = 20;
            //w.Width = 15;
            //w.Left = 17;
            //w.Top = 4;

            //var txt = new DrawText("Dungeon 12", ConsoleColor.DarkCyan);
            //txt.ReplaceAt(8, new DrawText("12", ConsoleColor.Red));

            //w.AddControl(new Label(w)
            //{
            //    SourceText = txt,
            //    Top = 1.1f,
            //    Left = 3,
            //    Width = txt.Length,
            //    Height = 1
            //});

            //w.AddControl(new HorizontalLine(w)
            //{
            //    Width = window.Width,
            //    Top = 2
            //});

            //w.AddControl(new Label(w, "Remastered")
            //{
            //    ForegroundColor = ConsoleColor.Red,
            //    Top = 3,
            //    Left = 7.5f,
            //    Width = 10
            //});

            //////Controls 
            //w.AddControl(new Button(w)
            //{
            //    Top = 5,
            //    Left = 3,
            //    Width = 9,
            //    Height = 1.7f,
            //    ActiveColor = ConsoleColor.Red,
            //    InactiveColor = ConsoleColor.DarkRed,
            //    CloseAfterUse = true,
            //    Label = "Новая игра",
            //    OnClick = () =>
            //    {
            //        this.Switch<PlayerNameScene>();
            //    }
            //});

            //w.AddControl(new Button(w)
            //{
            //    Top = 8,
            //    Left = 3,
            //    Width = 9,
            //    Height = 1.7f,
            //    ActiveColor = ConsoleColor.Red,
            //    InactiveColor = ConsoleColor.DarkRed,
            //    Label = "Быстрая игра",
            //    CloseAfterUse = true,
            //    OnClick = () =>
            //    {
            //        this.Player = Classes.All().Skip(1).First();
            //        this.Player.Name = "Adventurer";
            //        this.Player.Race = Race.Elf;
            //        this.Player.Add<RacePerk>();

            //        this.Switch<Game.MainScene>();
            //    }
            //});

            //Button ba = new Button(w);
            //ba.Top = 12;
            //ba.Left = 3;
            //ba.Width = 20;
            //ba.Height = 3;
            //ba.ActiveColor = ConsoleColor.Red;
            //ba.InactiveColor = ConsoleColor.DarkRed;
            //ba.Label = "Создатели";
            //ba.CloseAfterUse = true;
            //ba.OnClick = () => { /*MenuEngine.CreditsWindow.Draw(); MenuEngine.MainMenu.Draw();*/ };
            //w.AddControl(ba);

            //Button bex = new Button(w);
            //bex.Top = 15;
            //bex.Left = 3;
            //bex.Width = 20;
            //bex.Height = 3;
            //bex.ActiveColor = ConsoleColor.Red;
            //bex.InactiveColor = ConsoleColor.DarkRed;
            //bex.Label = "Выход";
            //bex.CloseAfterUse = true;
            //bex.OnClick = () =>
            //{
            //    Environment.Exit(0);
            //};
            //w.AddControl(bex);

            //Drawing.Draw.Session<ClearSession>()
            //    .Then(w)
            //    .Publish();
        }

        protected override void KeyPress(KeyArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                //case Key.Left:
                //case Key.Up:
                //    window.Up(keyEventArgs); break;
                //case Key.Down:
                //case Key.Right:
                //case Key.Tab:
                //    window.Tab(keyEventArgs); break;
                //case Key.Enter:
                //    window.ActivateInterface(keyEventArgs); break;
                default:
                    break;
            }

            this.Redraw();
        }
    }
}