using Dungeon;
using Dungeon.Entities;
using Dungeon.Network;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.CardGame.Engine;
using Dungeon12.CardGame.Interfaces;
using Dungeon12.CardGame.Triggers;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public int MaxResources { get => Get(___MaxResources, typeof(CardGamePlayer).AssemblyQualifiedName); set => Set(value, typeof(CardGamePlayer).AssemblyQualifiedName); }
        private int ___MaxResources;
        
        public List<GuardCard> Guards { get; set; } = new List<GuardCard>();

        public bool Damage(CardGamePlayer enemy, int amount, AreaCard areaCard)
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
                g.OnDie(enemy, this, areaCard);
                Guards.Remove(g);
                MessageBox.Show($"{g.Name} умирает", this.SceneObject.ShowInScene);
            });

            enemy.Influence += amount;

            MessageBox.Show($"{this.Name} получает {amount} урона", this.SceneObject.ShowInScene);
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

        public SafeQueue<Card> Cards { get; set; }

        public List<Card> HandCards { get; set; } = new List<Card>();

        public void Discard(Card handCard)
        {
            HandCards.Remove(handCard);
        }

        public Action OnResourceAdded { get; set; }

        public void AddResource()
        {
            this.MaxResources++;
            OnResourceAdded?.Invoke();
        }

        public Action HandChanged { get; set; }

        public bool AddInHand()
        {
            if (HandCards.Count >= 5)
            {
                return false;
            }

            var res = this.Resources;
            var free = 5 - HandCards.Count;

            var add = res;
            if (res > free)
            {
                add = free;
            }

            if (add == 0)
            {
                return false;
            }

            for (int i = 0; i < add; i++)
            {
                var card = Cards.Dequeue();
                if (card == default)
                {
                    break;
                }

                HandCards.Add(card);
            }
            HandChanged?.Invoke();

            return true;
        }

        /// <summary>
        /// Автоматически разыграть более подходящую карту
        /// </summary>
        /// <returns></returns>
        public Card Auto(Engine.CardGame cardGame)
        {
            var enemy = cardGame.Player1 == this ? cardGame.Player2 : cardGame.Player1;

            var shield = enemy.Guards.Sum(g => g.Shield);

            var enemyHits = enemy.Hits;
            var enemyInf = enemy.Influence;

            var rounds = cardGame.CurrentArea.Rounds;

            var inHand = this.HandCards;

            // если у нас мало хп, надо прикрыться
            // но только если можно разыграть карту защитника
            if (Hits <= 30 && this.Guards.Count< cardGame.CurrentArea.Size)
            {
                var guard = AutoGuard();
                if (guard != default)
                    return guard;
            }

            // Если у врага больше влияния и нормально так хп, надо поднимать влияние,
            // для влияния либо разыгрываем карту которая даёт больше влияния (гварды),
            // либо у которой есть бонус влияния
            if (enemyInf > this.Influence && enemyHits > 20)
            {
                var infCard = AutoInfluence();
                if(infCard!=default)
                {
                    return infCard;
                }
            }

            // если у врага мало хп, надо добивать
            if (enemyHits <= 20)
            {
                Card dmgCard = AutoAttack(inHand);
                if (dmgCard != default)
                {
                    return dmgCard;
                }
                else
                {
                    var infCard = AutoInfluence();
                    if (infCard != default)
                    {
                        return infCard;
                    }
                }
            }

            // в любом другом случае разыгрываем случайную карту если она есть
            var randomCard = HandCards.FirstOrDefault();
            if (randomCard != default)
            {
                // если случайная карта это защитник и мы не можем его разыграть
                if(randomCard.CardType== CardType.Guardian && this.Guards.Count < cardGame.CurrentArea.Size)
                {
                    return randomCard;
                }
                else
                {
                    //пытаемся найти другую карту или вернуть ничего для пропуска хода
                    var randomNotGuard = HandCards.FirstOrDefault(x => x.CardType != CardType.Guardian);
                    return randomNotGuard;
                }

                return randomCard;
            }

            //если карт нет, пропускаем ход
            return default;
        }

        private static Card AutoAttack(List<Card> inHand)
        {
            return inHand.FirstOrDefault(c => c is AbilityCard);
        }

        private Card AutoInfluence()
        {
            var addInf = HandCards.FirstOrDefault(c => c.PublishTriggerName == nameof(AddInfluence));
            if (addInf != default)
            {
                Card maxGuard = AutoGuard();
                if (maxGuard != default)
                {
                    if (Resources > 3)
                    {
                        return addInf;
                    }
                    else
                    {
                        return maxGuard;
                    }
                }
                else
                {
                    return addInf;
                }
            }

            return default;
        }

        private Card AutoGuard()
        {
            return HandCards.MaxBy(c => c is GuardCard guardCard ? guardCard.Shield : 0).FirstOrDefault();
        }
    }
}