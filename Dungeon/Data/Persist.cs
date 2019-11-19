using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Data
{
    public class Persist
    {
        /// <summary>
        /// Внутреннее свойство для LiteDb
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// числовой Id который можно использовать в коде
        /// </summary>
        public int ObjectId { get; set; }

        public string IdentifyName { get; set; }

        public string Assembly { get; set; }
    }
}
