namespace Dungeon.Map.Objects
{
    using Dungeon.Conversations;
    using Dungeon.Data;
    using Dungeon.Data.Conversations;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Сonversational : MapObject
    {
        public List<Conversation> Conversations { get; set; }

        public string ScreenImage { get; set; }

        public int Frames { get; set; }

        public void BuildConversations(ConversationalDataStore data)
        {
            if (data.Conversations == null)
                return;

            var conversations = Database.Entity<ConversationData>(x => data.Conversations.Contains(x.Identify));

            this.Conversations = conversations.Select(c => new Conversation()
            {
                Subjects = c.Subjects,
                Face = c.Face,
                Name = c.Name
            }).ToList();

            foreach (var conv in this.Conversations)
            {
                new ReplicaBinderVisitor().Visit(conv);
            }
        }

        private class ReplicaBinderVisitor : ConversationVisitor
        {
            private List<Variable> variables = new List<Variable>();

            bool bindedWalk = false;
            private readonly List<Replica> replics = new List<Replica>();

            public override void Visit(Conversation conversation)
            {
                base.Visit(conversation);
                bindedWalk = true;
                this.Reset();
                base.Visit(conversation);

                conversation.Variables = this.variables;
            }

            protected override void VisitReplica(Replica replica)
            {
                if (!bindedWalk)
                {
                    replics.Add(replica);

                    if (replica.Variables != null)
                        variables.AddRange(replica.Variables);
                }
                else
                {
                    replica.Conversation = this.conversation;
                    replica.Replics = replics
                        .Where(x => replica.ReplicsTags.Contains(x.Tag))
                        .ToList();

                    foreach (var variable in replica.Variables)
                    {
                        variable.Replica = this.replics.FirstOrDefault(r => r.Tag == variable.Value);
                    }
                }
            }
        }
    }
}
