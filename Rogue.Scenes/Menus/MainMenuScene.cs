namespace Rogue.Scenes.Menus
{
    using System;
    using System.Linq;
    using Rogue.Control.Keys;
    using Rogue.Drawing.Console;
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
            var w = window = new Window();
            w.Animation.Frames = 2;
            w.Speed = 10;
            w.Border = Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGray;
            //w.Border.HorizontalLine = '░';
            //w.Border.VerticalLine = '⌠';
            w.Border.LowerLeftCorner = '@';
            w.Border.LowerRightCorner = '@';
            w.Border.UpperLeftCorner = '@';
            w.Border.UpperRightCorner = '@';
            w.Border.PerpendicularRightward = '@';
            w.Border.PerpendicularLeftward = '@';

            //w.Header = true;
            w.Height = 20;
            w.Width = 26;
            w.Left = 36;
            w.Top = 5;


            w.AddControl(new Label(w, "  Hellgates")
            {
                ForegroundColor = ConsoleColor.DarkCyan,
                Top = 1,
                Left = 1,
                Width = w.Width - 2,
                Height = 1
            });

            w.AddControl(new HorizontalLine(w)
            {
                Width = window.Width,
                Top = 2
            });

            w.AddControl(new Label(w, "Remastered")
            {
                ForegroundColor = ConsoleColor.Red,
                Top = 3,
                Left = 14,
                Width = 10
            });

            w.AddControl(new Label(w, " [London]")
            {
                ForegroundColor = ConsoleColor.Cyan,
                Top = 4,
                Left = 1,
                Width = w.Width - 2,
                Height = 1
            });

            //Controls 
            Button bng = new Button(w);
            bng.Top = 6;
            bng.Left = 3;
            bng.Width = 20;
            bng.Height = 3;
            bng.ActiveColor = ConsoleColor.Red;
            bng.InactiveColor = ConsoleColor.DarkRed;
            bng.CloseAfterUse = true;
            bng.Label = "Новая игра";
            bng.OnClick = () =>
            {
                this.Switch<PlayerNameScene>();
            };
            w.AddControl(bng);

            Button bfg = new Button(w);
            bfg.Top = 9;
            bfg.Left = 3;
            bfg.Width = 20;
            bfg.Height = 3;
            bfg.ActiveColor = ConsoleColor.Red;
            bfg.InactiveColor = ConsoleColor.DarkRed;
            bfg.Label = "Быстрая игра";
            bfg.CloseAfterUse = true;
            bfg.OnClick = () =>
            {
                this.Player = Classes.All().Skip(1).First();
                this.Player.Name = "Adventurer";
                this.Player.Race = Race.Elf;
                this.Player.Add<RacePerk>();

                this.Switch<Game.MainScene>();
            };
            w.AddControl(bfg);

            Button ba = new Button(w);
            ba.Top = 12;
            ba.Left = 3;
            ba.Width = 20;
            ba.Height = 3;
            ba.ActiveColor = ConsoleColor.Red;
            ba.InactiveColor = ConsoleColor.DarkRed;
            ba.Label = "Создатели";
            ba.CloseAfterUse = true;
            ba.OnClick = () => { /*MenuEngine.CreditsWindow.Draw(); MenuEngine.MainMenu.Draw();*/ };
            w.AddControl(ba);

            Button bex = new Button(w);
            bex.Top = 15;
            bex.Left = 3;
            bex.Width = 20;
            bex.Height = 3;
            bex.ActiveColor = ConsoleColor.Red;
            bex.InactiveColor = ConsoleColor.DarkRed;
            bex.Label = "Выход";
            bex.CloseAfterUse = true;
            bex.OnClick = () =>
            {
                Environment.Exit(0);
            };
            w.AddControl(bex);

            w.Run();
            w.Publish();
        }

        public override void KeyPress(KeyArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.Left:
                case Key.Up:
                    window.Up(keyEventArgs); break;
                case Key.Down:
                case Key.Right:
                case Key.Tab:
                    window.Tab(keyEventArgs); break;
                case Key.Enter:
                    window.ActivateInterface(keyEventArgs); break;
                default:
                    break;
            }

            this.Redraw();
        }
    }
}