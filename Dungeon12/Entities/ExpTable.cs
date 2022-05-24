using Dungeon;
using Dungeon.Resources;
using System.Collections.Generic;

namespace Dungeon12.Entities
{
    internal class ExpTable
    {
        static ExpTable()
        {
            Table = ResourceLoader.LoadJson<Dictionary<int,int>>("Files/exp.json".AsmRes());
        }

        public static Dictionary<int,int> Table { get; set; }


        public int this[int level]
        {
            get => Table[level];
        }
    }
}
