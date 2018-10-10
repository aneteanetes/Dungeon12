using System;
using Rogue.Control.Keys;
using Rogue.Drawing.Console;
using Rogue.Drawing.Impl;
using Rogue.Races.Perks;
using Rogue.Scenes.Scenes;

namespace Rogue.Scenes.Menus.Creation
{
    public class PlayerClassScene : GameScene<PlayerSummaryScene,MainMenuScene>
    {
        public PlayerClassScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        private Window window;

        public override void Draw()
        {
            Window w = window = new Window();
            w.Border = Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGray;
            w.Border.LowerLeftCorner = '@';
            w.Border.LowerRightCorner = '@';
            w.Border.UpperLeftCorner = '@';
            w.Border.UpperRightCorner = '@';
            w.Border.PerpendicularRightward = '@';
            w.Border.PerpendicularLeftward = '@';

            w.Header = true;
            w.Height = 25;
            w.Width = 47;
            w.Left = 25;
            w.Top = 2;

            w.AddControl(new Label(w, "Выберите класс")
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

            var left = 3;
            var top = 4;
            var column = 0;

            foreach (var @class in Classes.All())
            {
                DrawColor usualColor = @class.ClassColor;
                var activeCOlor = Light(usualColor);

                Button bng = new Button(w)
                {
                    Top = top,
                    Left = left,
                    Width = 20,
                    Height = 3,
                    ActiveColor = activeCOlor,
                    InactiveColor = usualColor,
                    CloseAfterUse = true,
                    Label = @class.ClassName,
                    OnClick = () =>
                    {
                        @class.Name = this.Player.Name;
                        @class.Race = this.Player.Race;

                        this.Player = @class;
                        this.Player.Add<RacePerk>();

                        this.Switch<PlayerSummaryScene>();
                    }
                };
                w.AddControl(bng);

                top += 4;
                column++;

                if (column == 5)
                {
                    top = 4;
                    left = 24;
                };                
            }            

            w.Run();
            w.Publish();
        }

        private DrawColor Light(DrawColor color)
        {
            var r = color.R;
            var g = color.G;
            var b = color.B;

            return new DrawColor(r, g, b, 100);
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
    }
}
