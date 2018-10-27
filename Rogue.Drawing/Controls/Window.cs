namespace Rogue.Drawing.Controls
{
    using Rogue.Types;

    public class Window : BaseControl
    {
        public Direction Direction { get; set; }

        private string Dir => Direction == Direction.Horizontal
            ? "w"
            : "h";

        public override string Tileset => $"Rogue.Resources.Images.GUI.window_{Dir}.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            Height = Direction== Direction.Horizontal ? 495 : 707,
            Width = Direction == Direction.Horizontal ? 707 : 495,
            X = 0,
            Y = 0
        };
    }
}