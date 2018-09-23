namespace Rogue.Scenes.Menus
{
    using Rogue.Scenes.Character;
    using Rogue.Scenes.Scenes;
    using Rogue.Drawing.Console;
    using System;
    using Rogue.Drawing;
    using Rogue.Scenes.Controls.Keys;

    public class MainMenuScene : Scene<CreateScene>
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

            w.Header = true;
            w.Height = 24;
            w.Width = 26;
            w.Left = 36;
            w.Top = 5;

            Text t = new Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = TextPosition.Center;
            t.ForegroundColor = ConsoleColor.DarkCyan;
            t.Write("Hellgates");
            t.AppendLine();
            t.TextPosition = TextPosition.Right;
            t.ForegroundColor = ConsoleColor.Red;
            t.WriteLine("Альфа");
            t.TextPosition = TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Cyan;
            t.Write("[London]");
            t.AppendLine();

            w.Text = t;

            //Controls 
            Button bng = new Button(w);
            bng.Top = 6;
            bng.Left = 3;
            bng.Width = 20;
            bng.Height = 6;
            bng.ActiveColor = ConsoleColor.Red;
            bng.InactiveColor = ConsoleColor.DarkRed;
            bng.CloseAfterUse = true;
            bng.Label = "Новая игра";
            bng.OnClick = () =>
            {
                throw new Exception("Для демо хуемо и так много");
                //DrawEngine.ConsoleDraw.WriteTitle("Начало новой игры...\n \n Нажмите любую клавишу для продолжения...");
                //PlayEngine.GamePlay.NewGame.CharacterCreation();
            };
            w.AddControl(bng);

            Button bfg = new Button(w);
            bfg.Top = 9;
            bfg.Left = 3;
            bfg.Width = 20;
            bfg.Height = 6;
            bfg.ActiveColor = ConsoleColor.Red;
            bfg.InactiveColor = ConsoleColor.DarkRed;
            bfg.Label = "Быстрая игра";
            bfg.CloseAfterUse = true;
            bfg.OnClick = () => { /*PlayEngine.GamePlay.NewGame.CharacterCreation(true);*/ };
            w.AddControl(bfg);

            Button ba = new Button(w);
            ba.Top = 12;
            ba.Left = 3;
            ba.Width = 20;
            ba.Height = 6;
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
            bex.Height = 6;
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
                    window.up(); break;
                case Key.Down:
                case Key.Right:
                case Key.Tab:
                    window.tab(); break;
                case Key.Enter:
                    window.ActivateInterface(); break;
                default:
                    break;
            }

            this.Redraw();
        }
    }
}