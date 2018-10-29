namespace Rogue.Drawing.GUI
{
    using Rogue.Drawing.Controls;
    using Rogue.Types;

    public class ButtonsWindow : Window
    {
        public override string Tileset => $"Rogue.Resources.Images.GUI.textbox.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 0,
            Y = 0,
            Height = 91,
            Width = 456
        };
    }
}