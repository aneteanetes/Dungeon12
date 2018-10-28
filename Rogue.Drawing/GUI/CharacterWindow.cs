namespace Rogue.Drawing.GUI
{
    using Rogue.Drawing.Controls;
    using Rogue.Types;

    public class CharacterWindow : Window
    {
        public override string Tileset => "Rogue.Resources.Images.GUI.window_t_h.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X=0,
            Y=0,
            Width= 256,
            Height=603
        };
    }
}