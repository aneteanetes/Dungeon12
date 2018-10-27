namespace Rogue.Drawing.Controls
{
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class Button : Title
    {
        public bool Large = false;

        public override bool IsControlable => true;

        public override string Tileset => $"Rogue.Resources.Images.GUI.button_{(Large ? "l" : "m")}.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 0,
            Y = 0,
            Width = Large ? 340 : 283,
            Height = Large ? 100 : 116
        };

        public IDrawColor ActiveColor;
        public IDrawColor InactiveColor;

        public override void OnFocus()
        {
            this.Label.Paint(ActiveColor);
        }

        public override void OnUnfocus()
        {
            this.Label.Paint(InactiveColor);
        }
    }
}