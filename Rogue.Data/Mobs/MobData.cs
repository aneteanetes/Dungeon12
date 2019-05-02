namespace Rogue.Data.Mobs
{
    using Rogue.Entites.Enemy;
    using Rogue.Physics;
    using Rogue.Types;

    public class MobData
    {
        public string Tileset { get; set; }

        public Point Size { get; set; }

        public Point Position { get; set; }

        public Rectangle TileSetRegion { get; set; }

        public Enemy Enemy { get; set; }
    }
}
