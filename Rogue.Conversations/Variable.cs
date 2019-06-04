namespace Rogue.Conversations
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
        /// Определяет нужно ли сохранять флаг о том что переменная установлена
        /// </summary>
        public bool Global { get; set; }
    }
}
