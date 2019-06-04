namespace Rogue.Map
{
    using Force.DeepCloner;
    using Rogue.Conversations;
    using Rogue.Data.Conversations;
    using Rogue.Data.Mobs;
    using Rogue.Data.Npcs;
    using Rogue.Data.Region;
    using Rogue.DataAccess;
    using Rogue.Map.Objects;
    using Rogue.Physics;
    using Rogue.Settings;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    public partial class GameMap
    {
        private static void BindConversations(NPCData data, NPC mapNpc)
        {
            var conversations = Database.Entity<ConversationData>(x => data.Conversations.Contains(x.Identify));
            mapNpc.NPCEntity.Conversation = new Conversations.Conversation()
            {
                Subjects = conversations.SelectMany(x => x.Subjects).ToList()
            };

            mapNpc.NPCEntity.Conversation.Variables = new ReplicaVariableVisitor(mapNpc.NPCEntity.Conversation)
                .Visit();

            new ReplicaBinderVisitor()
                .Visit(mapNpc.NPCEntity.Conversation);
        }

        private class ReplicaVariableVisitor : ConversationVisitor
        {
            private List<Variable> variables = new List<Variable>();

            private Conversation conversation;

            public ReplicaVariableVisitor(Conversation conversation) => this.conversation = conversation;

            public List<Variable> Visit()
            {
                base.Visit(conversation);
                return variables;
            }

            protected override void VisitReplica(Replica replica)
            {
                if (replica.Variables != null)
                {
                    variables.AddRange(replica.Variables);
                }
                base.VisitReplica(replica);
            }
        }

        private class ReplicaBinderVisitor : ConversationVisitor
        {
            bool bindedWalk = false;
            private readonly List<Replica> replics = new List<Replica>();

            public override void Visit(Conversation conversation)
            {
                base.Visit(conversation);
                bindedWalk = true;
                this.Reset();
                base.Visit(conversation);
            }

            protected override void VisitReplica(Replica replica)
            {
                if(!bindedWalk)
                { 
                    replics.Add(replica);
                }
                else
                {
                    replica.Conversation = this.conversation;
                    replica.Replics = replics
                        .Where(x => replica.ReplicsTags.Contains(x.Tag))
                        .ToList();
                }
            }
        }
    }
}
