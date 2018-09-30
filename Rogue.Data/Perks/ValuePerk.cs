using Rogue.Drawing.Impl;

namespace Rogue.Data.Perks
{
    public class ValuePerk : Persist
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DrawColor Color { get; set; }

        public int Value { get; set; }

        public string Property { get; set; }
    }
}