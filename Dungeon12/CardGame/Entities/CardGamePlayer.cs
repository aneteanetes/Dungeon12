using Dungeon;
using Dungeon.Entities;
using Dungeon.Network;
using Dungeon12.CardGame.Engine;
using System.Collections.Generic;

namespace Dungeon12.CardGame.Entities
{
    public class CardGamePlayer : Entity
    {
        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public int Influence { get => Get(___Influence, typeof(CardGamePlayer).AssemblyQualifiedName); set => Set(value, typeof(CardGamePlayer).AssemblyQualifiedName); }
        private int ___Influence;

        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public int Hits { get => Get(___Hits, typeof(CardGamePlayer).AssemblyQualifiedName); set => Set(value, typeof(CardGamePlayer).AssemblyQualifiedName); }
        private int ___Hits;

        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public int Resources { get => Get(___Resources, typeof(CardGamePlayer).AssemblyQualifiedName); set => Set(value, typeof(CardGamePlayer).AssemblyQualifiedName); }
        private int ___Resources;

        public List<GuardCard> Guards { get; set; }

        public bool Damage(int amount)
        {
            List<GuardCard> forRemove = new List<GuardCard>();

            foreach (var guard in Guards)
            {
                if (guard.Shield - amount <= 0)
                {
                    amount -= guard.Shield;
                    forRemove.Add(guard);
                }
                else
                {
                    guard.Shield -= amount;
                    amount = 0;
                }
            }

            forRemove.ForEach(g => Guards.Remove(g));
            this.Hits -= amount;
            if (this.Hits <= 0)
            {
                return true;
            }

            return false;
        }

        public int RoundPoints { get; set; }

        public Deck Deck { get; set; }
    }
}