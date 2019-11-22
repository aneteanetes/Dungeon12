namespace Dungeon.Conversations
{
    using System.Collections.Generic;

    /// <summary>
    /// Тема разговора
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// Наименование темы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Текст на который отвечать
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Темы разговора могут реагировать на переменные, но не могут их выставлять
        /// </summary>
        public List<Variable> Variables { get; set; } = new List<Variable>();

        public Variable Visible { get; set; }

        public Conversation Conversation { get; set; }

        /// <summary>
        /// Реплики для ответа
        /// </summary>
        public List<Replica> Replics { get; set; }
    }
}