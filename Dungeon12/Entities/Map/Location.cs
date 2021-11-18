using Dungeon.Physics;
using Dungeon.Types;
using System.Collections.Generic;

namespace Dungeon12.Entities.Map
{
    public class Location
    {
        public string Name { get; set; }

        public string Background { get; set; }

        public string Object { get; set; }

        public string ObjectId { get; set; }

        public int Index { get; set; }

        public int[] IndexLinks { get; set; }

        public List<Location> Links { get; set; }

        public Point Size { get; set; }

        public Point Position { get; set; }

        public bool IsOpen { get; set; }
    }
}
