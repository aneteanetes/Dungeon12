namespace Dungeon.Map.Objects
{
    using Dungeon.Conversations;
    using Dungeon.Data;
    using Dungeon.Data.Conversations;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Сonversational : MapObject
    {
        public string FaceImage { get; set; }

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
                Id=c.Identify,
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

            private string _id;

            public override void Visit(Conversation conversation)
            {
                _id = conversation.Id;
                base.Visit(conversation);
                bindedWalk = true;
                this.Reset();
                base.Visit(conversation);

                foreach (var subject in conversation.Subjects)
                {
                    subject.Conversation = conversation;
                    foreach (var variable in subject.Variables)
                    {
                        variable.Replica = this.replics.FirstOrDefault(r => r.Tag == variable.Value);
                        if (variable.Global)
                        {
                            var globalName = variable.GlobalName(_id, subject.Name);
                            if (Global.GameState.Player.Component.Entity[globalName] != default)
                            {
                                variable.Triggered = true;
                                variable.TriggeredFrom = globalName.GetHashCode();
                            }
                        }
                    }
                }

                conversation.Variables = this.variables;
            }

            protected override void VisitSubject(Subject subject)
            {
                if(!bindedWalk)
                {
                    if (subject.Variables != null)
                        variables.AddRange(subject.Variables);
                }

                base.VisitSubject(subject);
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
                        if(variable.Global)
                        {
                            var globalName = variable.GlobalName(_id, replica.Tag);
                            if (Global.GameState.Player.Component.Entity[globalName] != default)
                            {
                                variable.Triggered = true;
                                variable.TriggeredFrom = replica.Tag;
                            }
                        }
                    }
                }
            }
        }
    }
}
