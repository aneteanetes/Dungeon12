﻿namespace Dungeon.Data.Conversations
{
    using Dungeon.Conversations;
    using System.Collections.Generic;

    public class ConversationData : Dungeon.Data.Persist
    {
        public string Identify { get; set; }

        public List<Subject> Subjects { get; set; }

        public string Face { get; set; }

        public string Name { get; set; }
    }
}