namespace Rogue.Components
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Rogue.Resources;
    using SkiaSharp;

    public class Game : ContentControl
    {
        public Game()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void Render(DrawingContext context)
        {
            var text = new FormattedText();
            
            var fontFamily = new FontFamily("Triforce(RUS BY LYAJKA) Triforce",new System.Uri("resm:Rogue.Resources.Fonts.Common.otf?assembly=Rogue.Resources#Triforce(RUS BY LYAJKA) Triforce"));

            text.Typeface = new Typeface(fontFamily, 72);

            text.Text = "Hello World!";

            context.DrawText(Brushes.Black, new Avalonia.Point(20, 20), text);

            base.Render(context);
        }
    }
}