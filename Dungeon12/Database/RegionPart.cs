namespace Dungeon12.Data.Region
{
    using Dungeon.Data;
    using Dungeon.Types;
    
    public class RegionPart : Persist
    {
        public string Icon { get; set; }
        
        public Rectangle Region { get; set; }

        public bool Obstruct { get; set; }

        public Point Position { get; set; }

        public int Layer { get; set; }
    }
}
