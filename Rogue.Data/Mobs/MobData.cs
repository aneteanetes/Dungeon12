namespace Rogue.Data.Mobs
{
    using Rogue.Entites.Enemy;
    using Rogue.Types;

    public class MobData : Persist
    {
        public int Level { get; set; }

        public string Name { get; set; }

        public string Tileset { get; set; }

        public Point Size { get; set; }

        public Point Position { get; set; }

        public Rectangle TileSetRegion { get; set; }

        public Enemy Enemy { get; set; }

        public double MovementSpeed { get; set; }
    }
}
