using Dungeon12.Conversations;
using Dungeon.Types;
using Dungeon12.Entities;

namespace Dungeon12.Data.Npcs
{
    public class NPCData : ConversationalDataStore
    {
        public bool IsEnemy { get; set; }

        public string Name { get; set; }

        public string Face { get; set; }

        public string Tileset { get; set; }

        public Point Size { get; set; }

        public Rectangle TileSetRegion { get; set; }

        public NPC NPC { get; set; }

        public double MovementSpeed { get; set; }

        public bool Merchant { get; set; }

        public bool Moveable { get; set; } = true;

        public string NoInteractText { get; set; }

        public bool NoInteract { get; set; }

        public string FractionIdentify { get; set; }

        public int Level { get; set; }

        public Point VisionMultiples { get; set; }

        public Point AttackRangeMultiples { get; set; }
    }
}