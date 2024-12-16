using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Combat
{
    internal class DamageRange
    {
        public DamageRange(Damage damage)
        {
            Damages[damage.Element] = [damage.Value];
        }

        Dictionary<Element, List<int>> Damages { get; set; } = new();

        List<Damage> PureDamages = new();

        public void Add(Damage dmg, bool isSumming = true)
        {
            if (!isSumming)
            {
                PureDamages.Add(dmg);
            }
            else
            {
                if (Damages.TryGetValue(dmg.Element, out var list))
                {
                    list.Add(dmg.Value);
                }
            }
        }

        public void Decrease(Damage dmg)
        {
            if (Damages.TryGetValue(dmg.Element, out var list))
            {
                bool iszero = false;
                var idx = 0;
                while (!iszero)
                {
                    if(idx==list.Count-1)
                        iszero = true;

                    list[idx] -= dmg.Value;
                    if (list[idx] <= 0)
                    {
                        list[idx] = 0;
                        iszero = true;
                    }

                    idx++;
                }

                list.RemoveAll(x => x == 0);
            }
        }
    }
}
