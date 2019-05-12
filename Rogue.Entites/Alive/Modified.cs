namespace Rogue.Entites.Alive
{
    using System;
    using System.Collections.Generic;
    using Rogue.Transactions;

    /// <summary>
    /// Изменяемый, постоянно или временно
    /// </summary>
    public class Modified : Capable
    {
        private static readonly Dictionary<Type, Applicable> Cache = new Dictionary<Type, Applicable>();

        public List<Applicable> Modifiers = new List<Applicable>();

        public void Add<T>() where T: Applicable
        {
            if (!Cache.TryGetValue(typeof(T), out var perk))
            {
                perk = typeof(T).New<T>();
                Cache.Add(typeof(T), perk);
            }

            perk.Apply(this);
            Modifiers.Add(perk);
        }

        public void Remove<T>() where T : Applicable
        {
            if (!Cache.TryGetValue(typeof(T), out var perk))
            {
                return;
            }

            perk.Discard(this);
            Modifiers.Remove(perk);
        }
    }
}