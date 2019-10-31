namespace Dungeon.Conversations
{
    using Dungeon.Data;
    using Dungeon.Data.Conversations;
    using Dungeon.Map.Objects;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Разговор
    /// </summary>
    public class Conversation
    {
        public string Face { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Темы этого разговора
        /// </summary>
        public List<Subject> Subjects { get; set; }

        /// <summary>
        /// Переменные в разговоре
        /// </summary>
        public List<Variable> Variables { get; set; }        
    }
}
