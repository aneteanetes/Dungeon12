using System.Collections.Generic;

namespace Dungeon.Conversations
{
    /// <summary>
    /// Реплика персонажа
    /// </summary>
    public class Replica
    {
        /// <summary>
        /// Ответ который произносится персонажем
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Текст который будет показан
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Реплики которые могут быть идти после этого
        /// </summary>
        public List<Replica> Replics { get; set; }

        /// <summary>
        /// Теги которые будут означать ссылки
        /// </summary>
        public List<int> ReplicsTags { get; set; }

        /// <summary>
        /// Реплика может быть показана сразу
        /// </summary>
        public bool Shown { get; set; }

        /// <summary>
        /// Тэг по которому строятся ссылки
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Переменные реплики
        /// </summary>
        public List<Variable> Variables { get; set; } = new List<Variable>();

        /// <summary>
        /// Экземпляр разговора которому принадлежит эта реплика
        /// </summary>
        public Conversation Conversation { get; set; }

        /// <summary>
        /// <see cref="IConversationTrigger"/> который нужно инстанциировать и выполнить в реплике
        /// </summary>
        public string TriggerClass { get; set; }

        /// <summary>
        /// Сборка <see cref="IConversationTrigger"/> из которой нужно инстанциировать
        /// </summary>
        public string TriggerClassAsm { get; set; }

        /// <summary>
        /// Аргументы для <see cref="IConversationTrigger.Execute(string[])"/>
        /// </summary>
        public string[] TriggerClassArguments { get; set; }
    }
}