namespace Rogue.Data.Npcs
{
    using Rogue.Entites.NPC;
    using Rogue.Types;
    using System.Collections.Generic;

    public class NPCData : Persist
    {
        public string IdentifyName { get; set; }

        public string Name { get; set; }

        public string Face { get; set; }

        public string Tileset { get; set; }

        public Point Size { get; set; }

        public Point Position { get; set; }

        public Rectangle TileSetRegion { get; set; }

        public NPCMoveable NPC { get; set; }

        public double MovementSpeed { get; set; }

        public List<string> Conversations { get; set; }
    }
}
