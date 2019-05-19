namespace Rogue.Scenes.Menus.Creation
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.Controls;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Entites.Alive.Character;
    using Rogue.Scenes.Game;
    using Rogue.Scenes.Manager;
    using Rogue.Types;
    using System;

    public class PlayerSummaryScene : GameScene<Main,PlayerOriginScene>
    {
        public PlayerSummaryScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Init()
        {
            this.AddObject(new Prologue());
        }

        private class Prologue : ColoredRectangle
        {
            public Prologue()
            {
                this.Width = 40;
                this.Height = 22.5;
               var txt = new DrawText("Пролог", ConsoleColor.White);
                txt.Size = 72;

                this.AddTextCenter(txt);
            }
        }

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
                Label = new DrawText(PlayerAvatar.Name, ConsoleColor.Black) { Size = 30 }
            });

            var strings = new (string,string, ConsoleColor)[] 
            {
                ("Раса: ", PlayerAvatar.Character.Race.ToDisplay(), ConsoleColor.Gray),
                ("Класс: ", PlayerAvatar.Character.ClassName, PlayerAvatar.Character.ResourceColor),
                ("Здоровье: ", string.Format("{0}/{0}", PlayerAvatar.Character.MaxHitPoints), ConsoleColor.Red),
                ($"{PlayerAvatar.Character.ResourceName}: ", PlayerAvatar.Character.Resource, PlayerAvatar.Character.ResourceColor),
                ("Урон: ", string.Format("{0}-{1}", PlayerAvatar.Character.MinDMG, PlayerAvatar.Character.MaxDMG),ConsoleColor.DarkRed),
                (" ", " ",ConsoleColor.DarkRed),
                ("Сила атаки: ", PlayerAvatar.Character.AttackPower.ToString(), ConsoleColor.DarkCyan),
                ("Сила магии: ", PlayerAvatar.Character.AbilityPower.ToString(), ConsoleColor.DarkCyan),
                ("Физ. Защита: ", PlayerAvatar.Character.Defence.ToString(), ConsoleColor.DarkGreen),
                ("Маг. Защита: ", PlayerAvatar.Character.Barrier.ToString(), ConsoleColor.DarkMagenta)
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
                    this.Switch<Main>();
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
