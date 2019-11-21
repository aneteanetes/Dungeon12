using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Data
{
    public interface IPersist
    {
        /// <summary>
        /// Внутреннее свойство для LiteDb
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// числовой Id который можно использовать в коде
        /// </summary>
        int ObjectId { get; set; }

        string IdentifyName { get; set; }

        string Assembly { get; set; }
    }
}
