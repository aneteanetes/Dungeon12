namespace Rogue.Data.Region
{
    using Rogue.Types;

    public class RegionPart
    {
        public string Image { get; set; }

        public Rectangle Region { get; set; }

        public bool Obstruct { get; set; }

        public Point Position { get; set; }

        public int Layer { get; set; }
    }
}
