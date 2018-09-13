using Rogue.Transactions;

namespace Rogue.Entites.Structural
{
    public abstract class Characteristic<T> : Applicable
    {
        public Characteristic(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public T Max { get; set; }

        public T Min { get; set; }

        public T Value { get; set; }
    }
}