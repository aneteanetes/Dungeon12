using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Map
{
    public class Location
    {
        public string Name;

        public string _Name, _Affics;

        public ConsoleColor Biom;

        public int Level = 1;

        public Cell[][] Map;

        public class Cell
        {
            public char Vision;

            public object Item;

            public object Enemy;

            public object Wall;

            public object Object;

            public object Player;

            public object Trap;

            public bool Empty;
        }
    }
}
