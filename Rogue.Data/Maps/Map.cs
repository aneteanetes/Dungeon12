namespace Rogue.Data.Maps
{
    using System.Collections.Generic;

    public class Map : Persist
    {
        public string Identity { get; set; }

        public string Name { get; set; }

        public string Template { get; set; }

        public List<Object> Objects { get; set; }
    }

    public class Object
    {
        public string Type { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
