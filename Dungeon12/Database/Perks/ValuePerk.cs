namespace Dungeon12.Data.Perks
{
    using Dungeon.Data;
    using System.Collections.Generic;

    public class ValuePerk : Dungeon.Data.Persist
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PersistColor Color { get; set; }

        public string Identity { get; set; }

        public List<Effect> Effects { get; set; }
    }

    public class PersistColor
    {
        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public byte A { get; set; }
    }

    public class Effect
    {
        public string Property { get; set; }

        public int Value { get; set; }

        public bool Positive { get; set; }
    }
}