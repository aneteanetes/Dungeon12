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
            MapObject moved = this.Map[now.Y][now.X][Level];
                        
            if (this.Map[next.Y][next.X].Any(x => x.Obstruction))
            {
                return now;
            }

            this.Map[now.Y][now.X].Remove(moved);
            this.Map[next.Y][next.X].Add(moved);
            return next;
        }
    }
}
