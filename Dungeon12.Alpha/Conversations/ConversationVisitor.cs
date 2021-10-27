using System.Collections.Generic;

namespace Dungeon12.Conversations
{
    public abstract class ConversationVisitor
    {
        private List<Replica> visited = new List<Replica>();
        protected Conversation conversation;

        public virtual void Visit(Conversation conversation)
        {
            this.conversation = conversation;
            foreach (var subject in conversation.Subjects)
            {
                VisitSubject(subject);
            }
        }

        protected virtual void VisitSubject(Subject subject)
        {
            foreach (var replica in subject.Replics)
            {
                if (!visited.Contains(replica))
                    VisitReplica(replica);
            }
        }

        protected virtual void VisitReplica(Replica replica)
        {
            visited.Add(replica);
            //foreach (var nextReplica in replica.ReplicsTags)
            //{
            //    if (!visited.Contains(replica))
            //        VisitReplica(nextReplica);
            //}
        }

        /// <summary>
        /// Сбрасывает проход
        /// </summary>
        protected void Reset() => visited.Clear();
    }
}