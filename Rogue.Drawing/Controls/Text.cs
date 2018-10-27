namespace Rogue.Drawing.Controls
{
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class Text : BaseControl
    {
        public override string Tileset => string.Empty;

        public override Rectangle TileSetRegion => null;

        public override bool Container => true;

        public IDrawText DrawText { get; set; }
        
        public override IDrawSession Run()
        {
            if (this.Width == default)
            {
                this.Width = this.DrawText.Length;
            }

            if (this.Height == default)
            {
                this.Height = 1f;
            }

            if (this.DrawText.Region == null)
            {
                this.DrawText.Region = new Rectangle
                {
                    X = this.Left,
                    Y = this.Top
                };
            }
            else
            {
                this.DrawText.Region.X += this.Left;
                this.DrawText.Region.Y += this.Top;
            }

            this.WanderingText.Add(DrawText);

            return this;
            //return base.Run();
        }
    }
}
