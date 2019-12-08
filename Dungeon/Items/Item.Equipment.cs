namespace Dungeon.Items
{
    using Dungeon.View.Interfaces;
    using System.Collections.Generic;

    public partial class Item : IDrawable
    {
        public Item()
        {
        }

        public string SubType { get; set; }

        public string Class { get; set; }

        public List<Equipment> BaseStats { get; set; } = new List<Equipment>();

        public List<Equipment> Additional { get; set; } = new List<Equipment>();

        public List<Equipment> ClassStats { get; set; } = new List<Equipment>();

        public string ItemSetName { get; set; }

        public List<Equipment> ItemSet { get; set; } = new List<Equipment>();

        public void PutOn(object character)
        {
            void Apply(Equipment equipment)
            {
                equipment.Apply(character);
            }

            this.BaseStats.ForEach(Apply);
            this.Additional.ForEach(Apply);
            this.ClassStats.ForEach(Apply);
            this.ItemSet.ForEach(Apply);
        }

        public void PutOff(object character)
        {
            void Discard(Equipment equipment)
            {
                equipment.Discard(character);
            }

            this.BaseStats.ForEach(Discard);
            this.Additional.ForEach(Discard);
            this.ClassStats.ForEach(Discard);
            this.ItemSet.ForEach(Discard);
        }
    }
}