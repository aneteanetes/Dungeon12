using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon12.Entities
{
    internal class Prayer
    {
        public MonthYear God { get; set; }

        public int Value { get; set; }

        public int Level { get; set; }
    }
}
