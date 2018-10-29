namespace Rogue.Drawing.Controls
{
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class ButtonIcon : BaseControl
    {
        public override bool IsControlable => true;

        public ButtonIcon()
        {
            base.OnFocus += OnFocusEvent;
            base.OnUnfocus += OnUnfocusEvent;
        }

        public override string Tileset => "Rogue.Resources.Images.GUI.button-i.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 0,
            Y = 0,
            Height = 97,
            Width = 97
        };

        public new Image Icon { get; set; }

        public override IDrawSession Run()
        {
            this.Icon.Width = 1.8f;
            this.Icon.Height = 1.8f;
            this.Icon.Left = 0.1f;
            this.Icon.Top = -0.15f;

            this.Append(this.Icon);

            return base.Run();
        }

        private void OnFocusEvent()
        {
            this.Icon.Width = 1.6f;
            this.Icon.Height = 1.6f;
            this.Icon.Left = 0.2f;
            this.Icon.Top = -0.1f;

            this.Append(this.Icon);

            base.Run().Publish();
        }

        private void OnUnfocusEvent()
        {
            this.Run().Publish();
        }
    }
}