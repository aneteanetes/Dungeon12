namespace Rogue.Drawing.Controls
{
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class Title : BaseControl
    {
        public override string Tileset => $"Rogue.Resources.Images.GUI.title_m.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 0,
            Y = 0,
            Height = 109,
            Width = 394
        };

        public IDrawText Label { get; set; }

        public override IDrawSession Run()
        {
            if (Label != null)
            {
                Label.Region = new Rectangle
                {
                    X = this.Width / 2 - Label.Length / 2 / 1.75f,
                    Y = this.Height / 2 - 1f / 1.9f
                };

                this.Append(new Text
                {
                    DrawText = Label
                });
            }

            return this;
        }
    }
}