using System;
using Rogue.Control.Keys;
using Rogue.Drawing.Console;
using Rogue.Entites.Alive.Character;
using Rogue.Scenes.Scenes;

namespace Rogue.Scenes.Menus.Creation
{
    public class PlayerNameScene : GameScene<PlayerRaceScene,MainMenuScene>
    {
        public PlayerNameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        private Window window;

        public override void Draw()
        {

            var w = window = new Window();
            w.Border = Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGray;
            w.Border.LowerLeftCorner = '@';
            w.Border.LowerRightCorner = '@';
            w.Border.UpperLeftCorner = '@';
            w.Border.UpperRightCorner = '@';
            w.Border.PerpendicularRightward = '@';
            w.Border.PerpendicularLeftward = '@';

            w.Height = 20;
            w.Width = 26;
            w.Left = 36;
            w.Top = 5;

            w.AddControl(new Label(w, "Введите имя")
            {
                Align = TextPosition.Center,
                ForegroundColor = ConsoleColor.DarkCyan,
                Top = 1,
                Width= w.Width - 4,
                Left=1
            });

            w.AddControl(new HorizontalLine(w)
            {
                Width = window.Width,
                Top = 2
            });

            TextBox bfg = new TextBox(w)
            {
                Top = 9,
                Left = 3,
                Width = 20,
                Height = 3,
                ActiveColor = ConsoleColor.Red,
                InactiveColor = ConsoleColor.DarkRed
            };
            bfg.OnEndTyping = () => { bfg.Return = bfg.Text; };
            bfg.Label = " ";
            w.AddControl(bfg);

            //Controls 
            Button bng = new Button(w);
            bng.Top = 15;
            bng.Left = 3;
            bng.Width = 20;
            bng.Height = 3;
            bng.ActiveColor = ConsoleColor.Red;
            bng.InactiveColor = ConsoleColor.DarkRed;
            bng.CloseAfterUse = true;
            bng.Label = "Подтвердить";
            bng.OnClick = () =>
            {
                if (this.Player == null)
                    this.Player = new NameOfPlayer();
                
                this.Player.Name = bfg.Text;

                if (!string.IsNullOrEmpty(this.Player.Name))
                {
                    this.Switch<PlayerRaceScene>();
                }
            };
            w.AddControl(bng);

            Button bgcl = new Button(w);
            bgcl.Top = 4;
            bgcl.Left = 3;
            bgcl.Width = 20;
            bgcl.Height = 3;
            bgcl.ActiveColor = ConsoleColor.Red;
            bgcl.InactiveColor = ConsoleColor.DarkRed;
            bgcl.CloseAfterUse = true;
            bgcl.Label = "Очистить";
            bgcl.OnClick = () =>
            {
                bfg.String = string.Empty;
                bfg.Run();
                bfg.Publish();

            };
            w.AddControl(bgcl);

            w.Run();
            w.Publish();
        }

        protected override void KeyPress(KeyArgs keyEventArgs)
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
                case Key.Escape:
                    this.Switch<MainMenuScene>(); break;
                default:
                    window.PropagateInput(keyEventArgs);
                    break;
            }

            this.Redraw();
        }

        private class NameOfPlayer : Player
        {

        }
    }
}
