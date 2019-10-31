namespace Dungeon.Data.Npcs
{
    using Dungeon.Entites.NPC;
    using Dungeon.Types;
    using System.Collections.Generic;

    public class NPCData : ConversationalDataStore
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

        public bool Merchant { get; set; }
    }
}
