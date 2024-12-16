using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Combat
{
    internal struct Damage
    {
        public Damage(int value, Element element)
        {
            Value = value;
            Element = element;
        }

        public int Value { get; private set; }

        public Element Element { get; private set; }
    }
}