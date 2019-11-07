namespace Dungeon.Entities.NPC
{
    using Dungeon.Conversations;
    using Dungeon.Entities.Alive;

    public class NPCMoveable : Moveable
    {
        public Conversation Conversation { get; set; }
    }
}