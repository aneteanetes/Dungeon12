namespace Rogue.Drawing.Controls
{
    using Rogue.Types;

    public class Window : BaseControl
    {
        public bool Large = false;
        public Direction Direction { get; set; }

        private string Dir => Direction == Direction.Horizontal
            ? "w"
            : "h";

        public override string Tileset => $"Rogue.Resources.Images.GUI.window_{Dir}{(Large ? "_l" : "")}.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            Height = Large
                ? 603
                : (Direction== Direction.Horizontal ? 495 : 707),
            Width = Large
                ? 907
                : (Direction == Direction.Horizontal ? 707 : 495),
            X = 0,
            Y = 0
        };
    }
}