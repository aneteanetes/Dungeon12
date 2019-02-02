using System;
using Rogue.Control.Keys;
using Rogue.Drawing.Controls;
using Rogue.Drawing.Impl;
using Rogue.Entites.Alive.Character;
using Rogue.Scenes.Game;
using Rogue.Scenes.Scenes;
using Rogue.Types;

namespace Rogue.Scenes.Menus.Creation
{
    public class PlayerSummaryScene : GameScene<MainScene,PlayerClassScene>
    {
        public PlayerSummaryScene(SceneManager sceneManager) : base(sceneManager)
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
                Direction = Drawing.Controls.Direction.Vertical,
                Left = 16f,
                Top = 4,
                Width = 15,
                Height = 22
            };

            win.Append(new Title
            {
                Left = 3.6f,
                Top = -1f,
                Width = 8,
                Height = 2.4f,
                Label = new DrawText(Player.Name, ConsoleColor.Black) { Size = 30 }
            });

            var strings = new (string,string, ConsoleColor)[] 
            {
                ("Раса: ", Player.Race.ToDisplay(), ConsoleColor.Gray),
                ("Класс: ", Player.ClassName, Player.ResourceColor),
                ("Здоровье: ", string.Format("{0}/{0}", Player.MaxHitPoints), ConsoleColor.Red),
                ($"{Player.ResourceName}: ", Player.Resource, Player.ResourceColor),
                ("Урон: ", string.Format("{0}-{1}", Player.MinDMG, Player.MaxDMG),ConsoleColor.DarkRed),
                (" ", " ",ConsoleColor.DarkRed),
                ("Сила атаки: ", Player.AttackPower.ToString(), ConsoleColor.DarkCyan),
                ("Сила магии: ", Player.AbilityPower.ToString(), ConsoleColor.DarkCyan),
                ("Физ. Защита: ", Player.Defence.ToString(), ConsoleColor.DarkGreen),
                ("Маг. Защита: ", Player.Barrier.ToString(), ConsoleColor.DarkMagenta)
            };

            var top = 2;
            var even = 0;

            foreach (var stringItem in strings)
            {
                if (even==2)
                {
                    top++;
                    even = 0;
                }

                win.Append(new Text
                {
                    Top = top,
                    Left = 3,
                    DrawText = this.Label(stringItem.Item1, stringItem.Item2, stringItem.Item3, 30)
                });

                top += 1;
                even++;
            }
            
            win.Append(new Button
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.DarkRed),
                Left = 4.1f,
                Top = 17,
                Width = 7,
                Height = 2,
                Label = new DrawText("Начать", ConsoleColor.DarkRed) { Size = 28, LetterSpacing = 13 }
                    .Capitalize(20),
                OnClick = () =>
                {
                    this.Switch<MainScene>();
                }
            });

            Drawing.Draw.RunSession(win);
        }

        private DrawText Label(string text, string value, ConsoleColor valueColor, float size)
        {
            var drawtext = new DrawText(text + " " + value, ConsoleColor.Black) { Size = size };
            drawtext.ReplaceAt(text.Length + 1, new DrawText(value, valueColor) { Size = size });

            return drawtext;
        }

        protected void KeyPress(KeyArgs keyEventArgs)
        {
            
            //this.Redraw();
        }

        private class NameOfPlayer : Player
        {

        }
    }
}
