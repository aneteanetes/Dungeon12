namespace Rogue.Entites.NPC
{
    using Rogue.Conversations;
    using Rogue.Entites.Alive;

    public class NPCMoveable : Moveable
    {
        public Conversation Conversation { get; set; }
    }
}