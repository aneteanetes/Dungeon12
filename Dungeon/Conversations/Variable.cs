namespace Dungeon.Conversations
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Переменная в диалоге
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Наименование переменной
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тэг реплики которая должна сработать если переменная установлена
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Реплика которая должна сработать если переменная установлена
        /// </summary>
        public Replica Replica { get; set; }

        /// <summary>
        /// Флаг говорящий о том что переменная сработала
        /// </summary>
        public bool Triggered { get; private set; }

        /// <summary>
        /// Тэг реплики которая установила флаг
        /// </summary>
        public int TriggeredFrom { get; private set; }

        /// <summary>
        /// Определяет нужно ли сохранять флаг о том что переменная установлена
        /// </summary>
        public bool Global { get; set; }

        public string GlobalName(string conversationId, object replicaTag) => $"{conversationId}{replicaTag}{Name}";

        public Conversation Conversation { get; set; }

        public void Trigger(int from, List<Variable> matched = null)
        {
            if (matched == default)
            {
                matched = new List<Variable>();
            }

            matched.Add(this);
            Triggered = true;
            TriggeredFrom = from;

            if (this.Global)
            {
                var globalName = GlobalName(Conversation.Id, from);
                Dungeon.Global.GameState.Player.Component.Entity[globalName] = true;
            }

            Conversation.Variables.ForEach(v =>
            {
                if (v.Name == this.Name && v != this && matched.IndexOf(v) < 0)
                {
                    v.Trigger(from, matched);
                }
            });
        }
    }
}