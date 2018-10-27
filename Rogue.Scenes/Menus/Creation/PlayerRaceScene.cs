using Rogue.Control.Keys;
using Rogue.Scenes.Scenes;

namespace Rogue.Scenes.Menus.Creation
{
    public class PlayerRaceScene : GameScene<MainMenuScene, PlayerClassScene>
    {
        public PlayerRaceScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        //private Window window;

        public override void Draw()
        {
            //Window w = window = new Window();
            //w.Border = Additional.BoldBorder;
            //w.BorderColor = ConsoleColor.DarkGray;
            //w.Border.LowerLeftCorner = '@';
            //w.Border.LowerRightCorner = '@';
            //w.Border.UpperLeftCorner = '@';
            //w.Border.UpperRightCorner = '@';
            //w.Border.PerpendicularRightward = '@';
            //w.Border.PerpendicularLeftward = '@';

            //w.Header = true;
            //w.Height = 25;
            //w.Width = 47;
            //w.Left = 25;
            //w.Top = 2;

            //w.AddControl(new Label(w, "Выберите расу")
            //{
            //    Align = TextPosition.Center,
            //    ForegroundColor = ConsoleColor.DarkCyan,
            //    Top = 1,
            //    Width = w.Width - 4,
            //    Left = 1
            //});

            //w.AddControl(new HorizontalLine(w)
            //{
            //    Width = window.Width,
            //    Top = 2
            //});

            //var left = 3;
            //var top = 4;
            //var column = 0;

            //foreach (var race in Enum.GetValues(typeof(Race)).Cast<Race>())
            //{
            //    Button bng = new Button(w)
            //    {
            //        Top = top,
            //        Left = left,
            //        Width = 20,
            //        Height = 3,
            //        ActiveColor = ConsoleColor.Red,
            //        InactiveColor = ConsoleColor.DarkRed,
            //        CloseAfterUse = true,
            //        Label = race.ToDisplay(),
            //        OnClick = () =>
            //        {
            //            this.Player.Race = race;
            //            this.Switch<PlayerClassScene>();
            //        }
            //    };
            //    w.AddControl(bng);

            //    top += 4;
            //    column++;

            //    if (column == 5)
            //    {
            //        top = 4;
            //        left = 24;
            //    };                
            //}

            ////Controls 
            

            //w.Run();
            //w.Publish();
        }

        protected override void KeyPress(KeyArgs keyEventArgs)
        {
            this.Redraw();
        }
    }
}
