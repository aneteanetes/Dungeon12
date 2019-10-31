namespace Dungeon12.Data.Conversations
{
    using Dungeon.Conversations;
    using System;using Dungeon;using Dungeon.Drawing.SceneObjects;
    using System.Collections.Generic;
    using System.Text;

    public class ConversationData : Dungeon.Data.Persist
    {
        public string Identify { get; set; }

        public List<Subject> Subjects { get; set; }

        public string Face { get; set; }

        public string Name { get; set; }
    }
}