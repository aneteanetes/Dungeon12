namespace Rogue.Data.Npcs
{
    using Rogue.Entites.Alive;
    using Rogue.Types;

    public class NPCData : Persist
    {
        public string IdentifyName { get; set; }

        public string Name { get; set; }

        public string Tileset { get; set; }

        public Point Size { get; set; }

        public Point Position { get; set; }

        public Rectangle TileSetRegion { get; set; }

        public Moveable NPC { get; set; }

        public double MovementSpeed { get; set; }
    }
}
