using System;
using Rogue.Control.Keys;
using Rogue.Drawing.Controls;
using Rogue.Drawing.Impl;
using Rogue.Entites.Alive.Character;
using Rogue.Scenes.Scenes;
using Rogue.Types;

namespace Rogue.Scenes.Menus.Creation
{
    public class PlayerNameScene : GameScene<PlayerRaceScene,MainMenuScene>
    {
        public PlayerNameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        private TextInput Input;

        public override void Draw()
        {
            new Image("Rogue.Resources.Images.d12back.png")
            {
                Left = 0.4f,
                Top = 1f,
                Width = 48.2f,
                Height = 29f,
                ImageTileRegion = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = 700,
                    Width = 1057
                }
            }.Run().Publish();

            var win = new Window
            {
                Direction = Drawing.Controls.Direction.Vertical,
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
                Label = new DrawText("Введите имя", ConsoleColor.Black) { Size = 30 }
            });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 3,
                Width = 7,
                Height = 2,
                Label = new DrawText("Очистить", ConsoleColor.DarkRed) { Size = 28, LetterSpacing = 13 }
                    .Capitalize(20),
                OnClick = () =>
                {
                    Input.BackslashValue(Input.GetValue().Length);
                    Input.Run().Publish();
                    this.Redraw();
                }
            });

            Input = new TextInput
            {
                Left = 4f,
                Top = 8.5f,
                Height=2f,
                Width=7f,
                Placeholder = "",
                ActiveColor = new DrawColor(ConsoleColor.Black),
                InactiveColor = new DrawColor(ConsoleColor.DarkGray),
                Size=30,
                //LetterSpacing=15,
                Max=13
            };
            win.Append(Input);

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 14,
                Width = 7,
                Height = 2,
                Label = new DrawText("Продолжить", ConsoleColor.DarkRed) { Size = 28, LetterSpacing = 13 }
                    .Capitalize(20),
                OnClick = () =>
                {
                    if (this.Player == null)
                        this.Player = new NameOfPlayer();

                    this.Player.Name = Input.GetValue();

                    if (!string.IsNullOrEmpty(this.Player.Name))
                    {
                        this.Switch<PlayerRaceScene>();
                    }
                    else
                    {
                        Validation(false);
                    }
                }
            });

            Drawing.Draw.RunSession(win);
        }

        private void Validation(bool valid)
        {
            if (!valid)
            {
                Input.Placeholder = "_____________";
                Input.InactiveColor = new DrawColor(ConsoleColor.Red);
            }
            else
            {
                Input.Placeholder = "";
                Input.InactiveColor = new DrawColor(ConsoleColor.DarkGray);                
            }
            Input.Run().Publish();
            this.Redraw();
        }

        protected override void KeyPress(KeyArgs keyEventArgs)
        {
            if (Input.Editable)
            {
                if (!string.IsNullOrWhiteSpace(Input.Placeholder))
                {
                    Validation(true);
                }

                if (keyEventArgs.Key == Key.Back)
                {
                    Input.BackslashValue(1);
                }
                else if (char.TryParse(keyEventArgs.Key.ToString(),out var c))
                {
                    var val = keyEventArgs.Key.ToString().ToLower();

                    Input.AppendValue(val);
                }

                Input.Run().Publish();
            }

            if (keyEventArgs.Key == Key.Escape)
                this.Switch<MainMenuScene>();

            this.Redraw();
        }

        private class NameOfPlayer : Player { }
    }
}
