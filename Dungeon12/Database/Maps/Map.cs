namespace Dungeon12.Data.Maps
{
    using Dungeon.Data.Mobs;
    using System.Collections.Generic;

    public class Map : Dungeon.Data.Persist
    {
        public string Identity { get; set; }

        public string Name { get; set; }

        public string Template { get; set; }

        public bool Procedural { get; set; }

        public List<Object> Objects { get; set; }

        public List<MobData> Mobs { get; set; }
    }

    public class Object
    {
        public string Type { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
