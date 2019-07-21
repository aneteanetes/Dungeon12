namespace Rogue.Items
{
    using Rogue.View.Interfaces;
    using System.Collections.Generic;

    public abstract partial class Item : IDrawable
    {
        public string SubType { get; set; }

        public string Class { get; set; }

        public List<Equipment> BaseStats { get; set; }

        public List<Equipment> Additional { get; set; }

        public List<Equipment> ClassStats { get; set; }

        public string SetName { get; set; }

        public List<Equipment> Set { get; set; }
    }
}