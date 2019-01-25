namespace Rogue.Components.UserInterface
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Media;
    using Avalonia.Media.Imaging;
    using Rogue.Resources;

    public class Button : Control
    {
        private string text = string.Empty;
        public Button(string text)
        {
            this.text = text;
            this.Width = 300;
            this.Height = 70;
        }

        public override void Render(DrawingContext context)
        {
            var texture = "Rogue.Resources.Images.ui.button{0}.png";

            texture = string.Format(texture, focused
                ? "_f"
                : string.Empty);

            var button = ResourceLoader.Load(texture);
            context.DrawImage(new Bitmap(button), 1, new Avalonia.Rect(new Size(483, 139)), new Avalonia.Rect(new Size(300, 70)), Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality);


            var formattedText = new FormattedText();

            var fontFamily = new FontFamily("Triforce(RUS BY LYAJKA) Triforce", new System.Uri("resm:Rogue.Resources.Fonts.Common.otf?assembly=Rogue.Resources#Triforce(RUS BY LYAJKA) Triforce"));

            formattedText.Typeface = new Typeface(fontFamily, 30);

            formattedText.Text = text;

            var measure = formattedText.Measure();

            var top = this.Height / 2 - measure.Height / 2;
            var left = this.Width / 2 - measure.Width / 2;

            var color = focused
                ? Brushes.LightCyan
                : Brushes.LightCyan;

            context.DrawText(color, new Avalonia.Point(left, top), formattedText);

            base.Render(context);
        }

        protected override void OnPointerLeave(PointerEventArgs e)
        {
            focused = false;
            this.InvalidateVisual();
            base.OnPointerLeave(e);
        }

        protected override void OnPointerEnter(PointerEventArgs e)
        {
            focused = true;
            this.InvalidateVisual();
            base.OnPointerEnter(e);
        }

        private bool focused = false;
    }
}
