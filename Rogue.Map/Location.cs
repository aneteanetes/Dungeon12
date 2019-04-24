using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rogue.Physics;
using Rogue.Types;

namespace Rogue.Map
{
    public class Location
    {
        public string Name;

        public string _Name, _Affics;

        public ConsoleColor Biom;

        public int Level = 1;

        public HashSet<MapObject> Objects = new HashSet<MapObject>();

        public List<List<List<MapObject>>> Map = new List<List<List<MapObject>>>();

        public bool MayMove(MapObject @object)
        {
            var inVision = @object.InVision(Objects);
            return !inVision.Any(x => x.Obstruction);
        }

        public bool MoveObject(Point now, int Level, Point next)
        {
            try
            {
                MapObject moved = this.Map[(int)now.Y][(int)now.X][Level];

                var nextObj = this.Map[(int)next.Y][(int)next.X];

                if (nextObj.First().Obstruction)
                {
                    return false;
                }


                this.Map[(int)now.Y][(int)now.X].Remove(moved);
                this.Map[(int)next.Y][(int)next.X].Add(moved);

                Console.WriteLine($"was: x={now.X}, y={now.Y}");
                Console.WriteLine($"now: x={next.X}, y={next.Y}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION ARGUMENT");
                return false;
            }
        }
    }
}
