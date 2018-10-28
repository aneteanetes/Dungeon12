namespace Rogue.Drawing.GUI
{
    using Rogue.Drawing.Controls;
    using Rogue.Types;

    public class ButtonsWindow : Window
    {
        public override string Tileset => "Rogue.Resources.Images.GUI.window_cp.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X=0,
            Y=0,
            Width= 294,
            Height=80
        };
    }
}