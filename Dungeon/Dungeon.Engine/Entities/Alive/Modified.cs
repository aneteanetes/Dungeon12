namespace Dungeon.Entites.Alive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon.Transactions;

    /// <summary>
    /// Изменяемый, постоянно или временно
    /// </summary>
    public class Modified : Capable
    {
        private static readonly Dictionary<Type, Perk> Cache = new Dictionary<Type, Perk>();

        public List<Perk> Modifiers = new List<Perk>();

        public void Add<T>() where T: Perk
        {
            if (!Cache.TryGetValue(typeof(T), out var perk))
            {
                perk = typeof(T).New<T>();
                Cache.Add(typeof(T), perk);
            }

            perk.Apply(this);
            Modifiers.Add(perk);
        }

        public void Remove<T>() where T : Perk
        {
            if (!Cache.TryGetValue(typeof(T), out var perk))
            {
                return;
            }

            perk.Discard(this);
            Modifiers.Remove(perk);
        }

        public void RemoveAll(Func<Perk,bool> filter=default)
        {
            if (filter == default)
            {
                filter = p => true;
            }

            var classPerks = Modifiers.Where(filter).ToArray();
            for (int i = 0; i < classPerks.Count(); i++)
            {
                var classPerk = classPerks.ElementAtOrDefault(i);
                classPerk.Discard(this);
                Modifiers.Remove(classPerk);
            }
        }
    }
}