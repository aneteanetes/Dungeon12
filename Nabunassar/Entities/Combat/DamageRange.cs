using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Combat
{
    internal class DamageRange
    {
        public DamageRange(Damage damage)
        {
            Damages[damage.Element] = damage;
        }

        Dictionary<Element, Damage> Damages { get; set; } = new();
        HashSet<Element> Ignorance = new();

        List<Damage> PureDamages = new();

        public void IgnoreElementDefence(Element element) => Ignorance.Add(element);

        public void Add(Damage dmg, bool isSumming = true)
        {
            if (!isSumming)
            {
                PureDamages.Add(dmg);
            }
            else
            {
                if (Damages.TryGetValue(dmg.Element, out var value))
                {
                    value.Value += dmg.Value;
                    if (dmg.IsIgnoreDefence)
                        Ignorance.Add(dmg.Element);
                }
            }
        }

        public void Decrease(Damage dmg)
        {
            if (Damages.TryGetValue(dmg.Element, out var value))
            {
                value.Value -= dmg.Value;
                if (value.Value < 0)
                    value.Value = 0;
            }
        }
    }
}