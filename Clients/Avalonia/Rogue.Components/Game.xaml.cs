namespace Rogue.Components
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Avalonia.Media.Imaging;
    using Avalonia.Styling;
    using MoreLinq;
    using Rogue.Components.Views.Creation;
    using Rogue.Resources;
    using Rogue.Scenes;
    using Rogue.Scenes.Scenes;
    using System;
    using System.Linq;

    public class Game : Canvas
    {
        private SceneManager SceneManager = new SceneManager();

        public Game()
        {
            var style = new Style(x => x.Class("btn"));
            style.Setters.Add(new Setter(Canvas.LeftProperty, 1280 / 2 - 150));
            style.Setters.Add(new Setter(Canvas.TopProperty, 260));
            this.Styles.Add(style);

            SceneManager.CreateVisual = component =>
            {
                var control = (IControl)component;
                this.Draw(control);
            };
            SceneManager.DestroyVisual = component => this.Children.Remove((IControl)component);

            SceneManager.Switch<Views.Start>();

            AvaloniaXamlLoader.Load(this);
        }

        IControl control;
        private void Draw(IControl control)
        {
            this.control = control;
            this.InvalidateVisual();
        }

        bool splashed = false;

        public override void Render(DrawingContext context)
        {
            if (!splashed)
            {
                var splash = ResourceLoader.Load("Rogue.Resources.Images.d12.png");

                this.Children.Add(new Image()
                {
                    Source = new Bitmap(splash),
                });
                splashed = true;
            }

            Initialize(context);

            //if (!Children.Contains(control))
            //{
            //    control.SetValue(Canvas.LeftProperty, this.Bounds.Width / 2);
            //    control.SetValue(Canvas.TopProperty, 0);
            //    this.Children.Add(control);
            //}

            base.Render(context);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            var forDelete = this.Children.Where(x => x.Classes.Contains("btn")).ToArray();

            foreach (var fordel in forDelete)
            {
                this.Children.Remove(fordel);
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            var buttons = new (string label, Action action)[]{
                ("Новая игра",()=> {

                }),
                ("Быстрая игра",()=>{ }),
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

                this.Children.Add(newGameBtn);
            }
            //this.InvalidateVisual();
        }

        private void Initialize(DrawingContext context)
        {

            //var textSprite = ResourceLoader.Load("Rogue.Resources.Images.d12textM.png");
            //var text = new Image()
            //{
            //    Source = new Bitmap(textSprite),
            //};
            //text.SetValue(Canvas.LeftProperty, this.Bounds.Width / 2 - text.Source.PixelSize.Width / 2);
            //text.SetValue(Canvas.TopProperty, 25);

            //this.Children.Add(text);

            var buttons = new (string label, Action action)[]{
                ("Новая игра",()=> {
                    
                }),
                ("Быстрая игра",()=>{ }),
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