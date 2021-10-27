namespace Dungeon12.Data.Conversations
{
    using Dungeon12.Conversations;
    using System.Collections.Generic;

    public class ConversationData : Dungeon.Data.Persist
    {
        public string Identify { get; set; }

        public List<Subject> Subjects { get; set; }

        public string Face { get; set; }

        public string Name { get; set; }
    }
}