namespace Rogue.Drawing.GUI
{
    using Rogue.Drawing.Controls;
    using Rogue.Types;

    public class InfoWindow : Window
    {
        public override string Tileset => "Rogue.Resources.Images.GUI.window_ts_w.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X=0,
            Y=0,
            Width= 1157,
            Height=80
        };
    }
}