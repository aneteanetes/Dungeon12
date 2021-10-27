namespace Dungeon12.Conversations
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

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

        [JsonIgnore]
        /// <summary>
        /// Реплика которая должна сработать если переменная установлена
        /// </summary>
        public Replica Replica { get; set; }

        /// <summary>
        /// Флаг говорящий о том что переменная сработала
        /// </summary>
        [JsonIgnore]
        private bool _triggered { get; set; }
        public bool Triggered
        {
            get
            {
#if Core
                if(Conversation!=default && !_triggered)
                {
                    if (Dungeon12.Global.GameState.Player.Component.Entity[this.Name]!=default)
                    {
                        _triggered = true;
                    }
                }
#endif

                return _triggered;
            }
        }
    

        /// <summary>
        /// Тэг реплики которая установила флаг
        /// </summary>
        [JsonIgnore]
        public int TriggeredFrom { get; private set; }

        /// <summary>
        /// Определяет нужно ли сохранять флаг о том что переменная установлена
        /// </summary>
        public bool Global { get; set; }

        [JsonIgnore]
        public Conversation Conversation { get; set; }

        public void Trigger(int from, List<Variable> matched = null)
        {
            if (matched == default)
            {
                matched = new List<Variable>();
            }

            matched.Add(this);
            _triggered = true;
            TriggeredFrom = from;

            if (this.Global)
            {
#if Core
                Dungeon12.Global.GameState.Character[this.Name] = true;
#endif
            }

            if (Conversation.Variables != default)
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