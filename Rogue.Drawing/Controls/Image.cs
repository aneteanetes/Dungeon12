namespace Rogue.Drawing.Controls
{
    using Rogue.Types;

    public class Image : BaseControl
    {
        public Image(string imagePath)
        {
            this.ImagePath = imagePath;
        }

        private string ImagePath { get; set; }
        public override string Tileset => ImagePath;
        
        public Rectangle ImageTileRegion { get; set; }
        public override Rectangle TileSetRegion => ImageTileRegion;
    }
}