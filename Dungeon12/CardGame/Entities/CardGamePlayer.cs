using Dungeon;
using Dungeon.Entities;
using Dungeon.Network;
using Dungeon12.CardGame.Engine;
using Dungeon12.CardGame.Interfaces;
using System;
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

        public List<GuardCard> Guards { get; set; } = new List<GuardCard>();

        public bool Damage(CardGamePlayer enemy, int amount,AreaCard areaCard)
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

            forRemove.ForEach(g =>
            {
                g.OnDie(enemy, this,areaCard);
                Guards.Remove(g);
            });

            enemy.Influence += amount;

            this.Hits -= amount;
            if (this.Hits <= 0)
            {
                return true;
            }

            return false;
        }

        public bool DamageAll(CardGamePlayer enemy, int amount, AreaCard areaCard)
        {
            List<GuardCard> forRemove = new List<GuardCard>();

            foreach (var guard in Guards)
            {
                if (guard.Shield - amount <= 0)
                {
                    forRemove.Add(guard);
                }
                else
                {
                    guard.Shield -= amount;
                }
            }

            forRemove.ForEach(g =>
            {
                g.OnDie(enemy, this,areaCard);
                Guards.Remove(g);
            });

            enemy.Influence += amount;

            this.Hits -= amount;
            if (this.Hits <= 0)
            {
                return true;
            }

            return false;
        }

        public int RoundPoints { get; set; }

        public Deck Deck { get; set; }

        public Queue<Card> Cards { get; set; }

        public List<Card> HandCards { get; set; } = new List<Card>();

        public void Discard(Card handCard)
        {
            HandCards.Remove(handCard);
        }

        public Action OnResourceAdded { get; set; }

        public void AddResource()
        {
            this.Resources++;
            OnResourceAdded?.Invoke();
        }

        public Action HandChanged { get; set; }

        public bool AddInHand()
        {
            if (HandCards.Count >= 5)
            {
                return false;
            }

            var card = Cards.Dequeue();
            if (card == default)
            {
                return false;
            }

            HandCards.Add(card);
            HandChanged?.Invoke();

            return true;
        }
    }
}