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

        public List<List<MapObject>> Map = new List<List<MapObject>>();
    }
}
