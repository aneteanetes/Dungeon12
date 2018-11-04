using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.Types;

namespace Rogue.Map
{
    public class Location
    {
        public string Name;

        public string _Name, _Affics;

        public ConsoleColor Biom;

        public int Level = 1;

        public List<List<List<MapObject>>> Map = new List<List<List<MapObject>>>();

        public Point MoveObject(Point now, int Level, Point next)
        {
            MapObject moved = this.Map[(int)now.Y][(int)now.X][Level];
                        
            if (this.Map[(int)next.Y][(int)next.X].Last().Obstruction)
            {
                return now;
            }

            this.Map[(int)now.Y][(int)now.X].Remove(moved);
            this.Map[(int)next.Y][(int)next.X].Add(moved);
            return next;
        }
    }
}
