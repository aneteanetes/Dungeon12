using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Map.Objects;
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

        public void MoveObject(Point now, int Level, Point next)
        {
            MapObject moved = this.Map[now.Y][now.X][Level];
            this.Map[now.Y][now.X].RemoveAt(Level);
            this.Map[next.Y][next.X].Add(moved);
        }        
    }
}
