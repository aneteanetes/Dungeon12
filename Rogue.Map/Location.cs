using System;
using System.Collections.Generic;
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

            //+1 и +2 это offset рисования карты, т.к. регион это АБСОЛЮТНЫЕ цифры относительно экрана
            //moved.Region.X = next.X;
            //moved.Region.Y = next.Y;

            this.Map[now.Y][now.X].Remove(moved);
            this.Map[next.Y][next.X].Add(moved);
        }        
    }
}
