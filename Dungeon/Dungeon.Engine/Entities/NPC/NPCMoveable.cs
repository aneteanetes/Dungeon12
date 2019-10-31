namespace Dungeon.Entites.NPC
{
    using Dungeon.Conversations;
    using Dungeon.Entites.Alive;

    public class NPCMoveable : Moveable
    {
        public Conversation Conversation { get; set; }
    }
}