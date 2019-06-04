namespace Rogue.Data.Conversations
{
    using Rogue.Conversations;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConversationData : Persist
    {
        public string Identify { get; set; }

        public List<Subject> Subjects { get; set; }
    }
}