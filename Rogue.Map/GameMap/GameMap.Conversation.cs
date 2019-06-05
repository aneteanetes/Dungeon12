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

            new ReplicaBinderVisitor().Visit(mapNpc.NPCEntity.Conversation);
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
                if(!bindedWalk)
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
