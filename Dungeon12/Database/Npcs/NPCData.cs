namespace Dungeon12.Data.Npcs
{
    using Dungeon.Conversations;
    using Dungeon.Entites.NPC;
    using Dungeon.Types;
    using System.Collections.Generic;

    public class NPCData : ConversationalDataStore
    {
        public string Name { get; set; }

        public string Face { get; set; }

        public string Tileset { get; set; }

        public Point Size { get; set; }

        public Rectangle TileSetRegion { get; set; }

        public NPCMoveable NPC { get; set; }

        public double MovementSpeed { get; set; }

        public bool Merchant { get; set; }

        public bool Moveable { get; set; } = true;
    }
}
