using System;
using System.Linq;
using Rogue.Control.Keys;
using Rogue.Drawing.Controls;
using Rogue.Drawing.Impl;
using Rogue.Scenes.Scenes;
using Rogue.Types;

namespace Rogue.Scenes.Menus.Creation
{
    public class PlayerRaceScene : GameScene<MainMenuScene, PlayerClassScene>
    {
        public PlayerRaceScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;


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
                Direction = Drawing.Controls.Direction.Horizontal,
                Left = 11f,
                Top = 4,
                Width = 25,
                Height = 20
            };


            win.Append(new Title
            {
                Left = 8.3f,
                Top = -1f,
                Width = 8,
                Height = 2.4f,
                Label = new DrawText("Выберите расу", ConsoleColor.Black) { Size = 27 }
            });

            var left = 3;
            var top = 2.5f;
            var column = 0;

            foreach (var race in Enum.GetValues(typeof(Race)).Cast<Race>())
            {
                win.Append(new Button
                {
                    ActiveColor = new DrawColor(ConsoleColor.Red),
                    InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                    Left = left,
                    Top = top,
                    Width = 8,
                    Height = 2,
                    Label = new DrawText(race.ToDisplay(), ConsoleColor.DarkRed) { Size = 28, LetterSpacing = 13 }.Capitalize(20),
                    OnClick = () =>
                    {
                        this.Player.Race = race;
                        this.Switch<PlayerClassScene>();
                    }
                });

                top += 3;
                column++;

                if (column == 5)
                {
                    top = 2.5f;
                    left = 13;
                };
            }

            Drawing.Draw.RunSession(win);
        }

        protected override void KeyPress(KeyArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Escape)
                this.Switch<PlayerNameScene>();

            this.Redraw();
        }
    }
}
