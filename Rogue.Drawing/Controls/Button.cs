namespace Rogue.Drawing.Controls
{
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class Button : Title
    {
        public Button()
        {
            this.OnFocus = OnFocusEvent;
            this.OnUnfocus = OnUnfocusEvent;
        }

        public bool Large = false;

        public override bool IsControlable => true;

        public override string Tileset => $"Rogue.Resources.Images.ui.button.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 0,
            Y = 0,
            Width = 483,
            Height = 139
        };

        public IDrawColor ActiveColor;
        public IDrawColor InactiveColor;

        private void OnFocusEvent()
        {
            this.Label.Paint(ActiveColor,true);
            this.Run().Publish();
        }

        private void OnUnfocusEvent()
        {
            this.Label.Paint(InactiveColor, true);
            this.Run().Publish();
        }
    }
}