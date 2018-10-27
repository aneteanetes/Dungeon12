namespace Rogue.Drawing.Controls
{
    using Rogue.Drawing.Impl;
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class TextInput : Title
    {
        public TextInput()
        {
            this.OnFocus = OnFocusEvent;
            this.OnUnfocus = OnUnfocusEvent;
        }

        public override bool IsControlable => true;

        public override string Tileset => $"Rogue.Resources.Images.GUI.title_m.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 0,
            Y = 0,
            Height = 109,
            Width = 498
        };

        private string value = string.Empty;
        public string Placeholder;
        public IDrawColor ActiveColor;
        public IDrawColor InactiveColor;
        public bool Editable = false;

        public override IDrawSession Run()
        {
            IDrawColor currentColor = Editable
                ? ActiveColor
                : InactiveColor;

            if (this.Label == null)
            {
                this.Label = new DrawText(Placeholder, this.InactiveColor);
            }
            else
            {
                var val = value.Length > 0 ? value : Placeholder;

                if (val.Length > this.Width)
                {
                    val = value.Substring(0, (int)this.Width);
                }

                this.Label = new DrawText(val, this.ActiveColor);
            }

            return base.Run();
        }

        public void AppendValue(string data)
        {
            this.value += data;            
        }

        public void BackslashValue(int count)
        {
            this.value = this.value.Substring(0, this.value.Length - count);
        }

        public string GetValue() => this.value;

        private void OnFocusEvent()
        {
            Editable = true;
            this.Label.Paint(ActiveColor, true);
            this.Run().Publish();
        }

        private void OnUnfocusEvent()
        {
            Editable = false;
            this.Label.Paint(InactiveColor, true);
            this.Run().Publish();
        }
    }
}