namespace Rogue.Components.Views
{
    using Avalonia.Controls;
    using Avalonia.Media;
    using Avalonia.Media.Imaging;
    using Avalonia.Styling;
    using Rogue.Resources;

    public class Main : Base<Scenes.Game.MainScene>
    {
        public Main(Scenes.Game.MainScene Scene) : base(Scene)
        {
        }

        protected override void Initialize(DrawingContext context)
        {
            var main = ResourceLoader.Load("Rogue.Resources.Images.ui.horizontal.png");

            var mainWindow = new Image()
            {
                Source = new Bitmap(main),
            };

            mainWindow.SetValue(Canvas.LeftProperty, this.Bounds.Width / 2 - mainWindow.Source.PixelSize.Width / 2);
            mainWindow.SetValue(Canvas.TopProperty, 25);

            this.Children.Add(mainWindow);
        }
    }
}

//<Window.Styles >
//    <Style Selector = "ItemsControl > ContentPresenter" >
//      < Setter Property="Canvas.Left" Value="{Binding Location.X}"/>
//      <Setter Property = "Canvas.Top" Value="{Binding Location.Y}"/>
//      <Setter Property = "ZIndex" Value="{Binding Converter={x:Static infrastructure:ZIndexConverter.Instance }}" />
      
//      <Setter Property = "Transitions" >
//        < Transitions >
//          < DoubleTransition  Property="Opacity" Duration="0:0:0:0.5"/>
//        </Transitions>
//      </Setter>
      
//    </Style>
//    <Style Selector = "ItemsControl > ContentPresenter.test:pointerover" >
//      < Setter Property="Opacity" Value="0.5"/>
//    </Style>
//  </Window.Styles>