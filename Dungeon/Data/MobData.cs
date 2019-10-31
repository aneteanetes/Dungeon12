namespace Dungeon.Data.Mobs
{
    using Dungeon.Entites.Enemy;
    using Dungeon.Types;

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

        public Point VisionMultiples { get; set; }

        public Point AttackRangeMultiples { get; set; }
    }
}
