namespace Dungeon.Data.Conversations
{
    using Dungeon.Conversations;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConversationData : Persist
    {
        public string Identify { get; set; }

        public List<Subject> Subjects { get; set; }

        public string Face { get; set; }

        public string Name { get; set; }
    }
}