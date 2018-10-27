namespace Rogue.Scenes.Menus
{
    using System;
    using System.Linq;
    using Rogue.Control.Keys;
    using Rogue.Drawing.Controls;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.Utils;
    using Rogue.Races.Perks;
    using Rogue.Scenes.Menus.Creation;
    using Rogue.Scenes.Scenes;
    using Rogue.Types;

    public class MainMenuScene : GameScene<PlayerNameScene, Game.MainScene>
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
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
                Direction= Drawing.Controls.Direction.Vertical,
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

            win.Append(new Image("Rogue.Resources.Images.d12logo.png")
            {
                Left = 6.1f,
                Top = 1.9f,
                Width = 3f,
                Height = 3f,
                ImageTileRegion = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = 300,
                    Width = 300
                }
            });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 6,
                Width = 7,
                Height = 2,
                Label = new DrawText("Новая игра", ConsoleColor.DarkRed) { Size = 30 },
                OnClick = () => { this.Switch<PlayerNameScene>(); }
            });

            var fastgamelabel = new DrawText("Быстрая игра ", ConsoleColor.DarkRed) { Size = 30 };
            fastgamelabel.ReplaceAt(0, new DrawText("Б", ConsoleColor.DarkRed) { Size = 30, LetterSpacing = 20 });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 9,
                Width = 7,
                Height = 2,
                Label = fastgamelabel,
                OnClick = () =>
                {
                    this.Player = Classes.All().Skip(1).First();
                    this.Player.Name = "Adventurer";
                    this.Player.Race = Race.Elf;
                    this.Player.Add<RacePerk>();

                    this.Switch<Game.MainScene>();
                }
            });

            var creators = new DrawText("Создатели ", ConsoleColor.DarkRed) { Size = 30 };
            creators.ReplaceAt(0, new DrawText("С", ConsoleColor.DarkRed) { Size = 30, LetterSpacing = 20 });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 12,
                Width = 7,
                Height = 2,
                Label = creators,
                OnClick = () => { /*MenuEngine.CreditsWindow.Draw(); MenuEngine.MainMenu.Draw();*/ }
            });

            var exit = new DrawText("Выход  ", ConsoleColor.DarkRed) { Size = 30 };
            exit.ReplaceAt(0, new DrawText("В", ConsoleColor.DarkRed) { Size = 30, LetterSpacing = 20 });

            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 15,
                Width = 7,
                Height = 2,
                Label = exit,
                OnClick =()=> { Environment.Exit(0); }
            });

            Drawing.Draw.RunSession(win);
        }
    }
}