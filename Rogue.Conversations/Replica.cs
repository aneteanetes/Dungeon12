using System.Collections.Generic;

namespace Rogue.Conversations
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
        /// Теги которые будут означать ссылки (ДЕРЬМО А НЕ КОММЕНТ)
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
    }
}


 //"Shown": false,
 //         "Tag": 2,
 //         "Variables": [
 //           {
 //             "Key": "Already",
 //             "Value": 4,
 //             "Global": false
 //           }
 //         ],
 //         "Answer": "Продолжайте",
 //         "Text": "После подъёма острова на нём начало собираться много рыбаков, но они все начали сходить с ума, поэтому главный архимаг прибыл на остров и совершил обряд жертвоприношения. Самого себя. После этого рыбакам стало на много легче, и был основан целый Орден Служителей Веры. Теперь мы каждый год совершаем ритуальное жертвоприношение для соблюдения мира на острове.",
 //         "Replics": [] //go to start