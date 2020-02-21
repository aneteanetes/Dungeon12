namespace Dungeon12.Conversations
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Разговор
    /// </summary>
    public class Conversation
    {
        public string Id { get; set; }

        public string Face { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
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
