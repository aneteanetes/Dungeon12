namespace Rogue.Components.Views
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using Avalonia.Controls;
    using Avalonia.Media;
    using Avalonia.Media.Imaging;
    using Avalonia.Styling;
    using Rogue.Components.Views.Creation;
    using Rogue.Races.Perks;
    using Rogue.Resources;

    public class Start : Base<Scenes.Start>
    {
        public Start(Scenes.Start Scene) : base(Scene)
        {
            //var style = new Style(x => x.Class("btn"));
            //style.Setters.Add(new Setter(Canvas.LeftProperty, this.Bounds.Width / 2 - 150));
            //style.Setters.Add(new Setter(Canvas.TopProperty, 260));
            //this.Styles.Add(style);
        }

        private void FastStart()
        {
            Scene.Player = Rogue.Classes.All().Skip(1).First();
            Scene.Player.Name = "Adventurer";
            Scene.Player.Race = Race.Elf;
            Scene.Player.Add<RacePerk>();

            this.Switch<Main>();
        }

        protected override void Initialize(DrawingContext context)
        {

            var textSprite = ResourceLoader.Load("Rogue.Resources.Images.d12textM.png");
            var text = new Image()
            {
                Source = new Bitmap(textSprite),
            };
            //text.SetValue(Canvas.LeftProperty, this.Bounds.Width / 2 - text.Source.PixelSize.Width / 2);
            //text.SetValue(Canvas.TopProperty, 25);

            this.Children.Add(text);

            var buttons = new (string label, Action action)[]{
                ("Новая игра",()=> {
                    Scene.Switch<Name>();
                }),
                ("Быстрая игра",FastStart),
                ("Создатели",()=> { }),
                ("Выход",()=> { Environment.Exit(0); })
            };

            var y = 260;
            foreach (var button in buttons.Reverse())
            {
                var newGameBtn = new UserInterface.Button(button.label);

                var @class = Guid.NewGuid().ToString();
                newGameBtn.Classes.Add("btn");

                //newGameBtn.SetValue(Canvas.LeftProperty, this.Bounds.Width / 2 - newGameBtn.Width / 2);
                //newGameBtn.SetValue(Canvas.TopProperty, y);

                y += 100;

                newGameBtn.PointerPressed += (s, e) =>
                {
                    button.action?.Invoke();
                };
                this.Children.Add(newGameBtn);
            }

        }
    }
}