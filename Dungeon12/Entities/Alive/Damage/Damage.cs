using Dungeon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12.Entities.Alive
{
    public class Damage
    {
        public DamageType Type { get; set; }

        public long Amount { get; set; }

        /// <summary>
        /// Проценты
        /// </summary>
        public long ArmorPenetration { get; set; }

        /// <summary>
        /// Проценты
        /// </summary>
        public long MagicPenetration { get; set; }
    }
}