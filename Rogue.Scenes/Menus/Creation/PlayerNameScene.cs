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

            win.Append(new Text
            {
                Left = 3f,
                Top = 2f,
                DrawText = new DrawText("Введите имя", ConsoleColor.DarkCyan) { Size = 40, LetterSpacing=20 }
            });

            Input = new TextInput
            {
                Left = 4f,
                Top = 10,
                Height=2f,
                Width=9f,
                Placeholder = "",
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.Gray)
            };
            win.Append(Input);

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 15,
                Width = 7,
                Height = 2,
                Label = new DrawText("Подтвердить", ConsoleColor.DarkRed) { Size = 28, LetterSpacing = 13 },
                OnClick = () =>
                {
                    if (this.Player == null)
                        this.Player = new NameOfPlayer();

                    this.Player.Name = Input.GetValue();

                    if (!string.IsNullOrEmpty(this.Player.Name))
                    {
                        this.Switch<PlayerRaceScene>();
                    }
                }
            });
            
            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 5,
                Width = 7,
                Height = 2,
                Label = new DrawText("Очистить", ConsoleColor.DarkRed) { Size = 28, LetterSpacing = 13 },
                OnClick = () =>
                {
                    Input.BackslashValue(Input.GetValue().Length);
                }
            });

            Drawing.Draw.RunSession(win);
        }

        protected override void KeyPress(KeyArgs keyEventArgs)
        {
            if (Input.Editable)
            {
                if (keyEventArgs.Key == Key.Back)
                {
                    Input.BackslashValue(1);
                }
                else
                {
                    var val = keyEventArgs.Key.ToString();

                    if(keyEventArgs.Modifiers!= KeyModifiers.Shift)
                    {
                        val = val.ToLower();
                    }

                    Input.AppendValue(val);
                }

                Input.Run().Publish();
            }

            this.Redraw();
        }

        private class NameOfPlayer : Player { }
    }
}
