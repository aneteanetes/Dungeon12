using System;
using Rogue.Control.Keys;
using Rogue.Drawing.Console;
using Rogue.Drawing.Impl;
using Rogue.Entites.Alive.Character;
using Rogue.Scenes.Game;
using Rogue.Scenes.Scenes;

namespace Rogue.Scenes.Menus.Creation
{
    public class PlayerSummaryScene : GameScene<MainScene,MainMenuScene>
    {
        public PlayerSummaryScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        private Window window;

        public override void Draw()
        {
            Window w= window = new Window();         
            w.Border = Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGray;
            w.Border.LowerLeftCorner = '@';
            w.Border.LowerRightCorner = '@';
            w.Border.UpperLeftCorner = '@';
            w.Border.UpperRightCorner = '@';
            w.Border.PerpendicularRightward = '@';
            w.Border.PerpendicularLeftward = '@';

            w.Header = true;
            w.Height = 21;
            w.Width = 24;
            w.Left = 37;
            w.Top = 4;

            w.AddControl(new Label(w, "  Ваш персонаж")
            {
                Align = TextPosition.Center,
                ForegroundColor = ConsoleColor.DarkCyan,
                Top = 1,
                Width = w.Width - 4,
                Left = 1
            });

            w.AddControl(new HorizontalLine(w)
            {
                Width = window.Width,
                Top = 2
            });

            var totalTop = 3;
            void label(string text, (string text, ConsoleColor color) value,bool delimiter=false)
            {
                var drawtext = DrawText.Empty(text.Length + 1 + value.text.Length);
                drawtext.ReplaceAt(0, new DrawText(text, ConsoleColor.DarkRed));
                drawtext.ReplaceAt(text.Length + 1, new DrawText(value.text, value.color));
                w.AddControl(new Label(w)
                {
                    Top = totalTop,
                    Left = 2,
                    SourceText = drawtext,
                    Align= TextPosition.Left,
                    Width=drawtext.Length
                });
                totalTop++;

                if (delimiter)
                {
                    w.AddControl(new HorizontalLine(w)
                    {
                        Width = window.Width,
                        Top = totalTop
                    });
                    totalTop++;
                }
            }

            label("Имя: ",(Player.Name, ConsoleColor.DarkGray));
            label("Раса: ", (Player.Race.ToDisplay(), ConsoleColor.Gray));
            label("Класс: ", (Player.ClassName, Player.ResourceColor),true);
            label("Здоровье: ", (string.Format("{0}/{0}", Player.MaxHitPoints), ConsoleColor.Red));
            label($"{Player.ResourceName}: ", (Player.Resource, Player.ResourceColor),true);
            label("Урон: ", (string.Format("{0}-{1}", Player.MinDMG, Player.MaxDMG), ConsoleColor.DarkYellow));
            label("Сила атаки: ", (Player.AttackPower.ToString(), ConsoleColor.DarkCyan));
            label("Сила магии: ", (Player.AbilityPower.ToString(), ConsoleColor.DarkCyan),true);
            label("Физ. Защита: ", (Player.Defence.ToString(), ConsoleColor.DarkGreen));
            label("Маг. Защита: ", (Player.Barrier.ToString(), ConsoleColor.DarkMagenta),true);
            
            Button bex = new Button(w)
            {
                Border=false,
                Top = totalTop,
                Left = 2,
                Width = 20,
                Height = 3,
                ActiveColor = ConsoleColor.Red,
                InactiveColor = ConsoleColor.DarkRed,                
                Label = "Начать",
                OnClick = () =>
                {
                    this.Switch<MainScene>();
                }
            };
            w.AddControl(bex);

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
