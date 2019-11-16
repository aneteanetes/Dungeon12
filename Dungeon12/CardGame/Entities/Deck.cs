using Dungeon.Entities;
using Dungeon12.CardGame.Entities;
using Dungeon12.Database.CardGameDeck;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.CardGame.Engine
{

    public class Deck : DataEntity<Deck,CardGameDeckData>
    {
        /// <summary>
        /// <para>
        /// 3 карты существ 1 уровня (если есть)
        /// </para>
        /// <para>
        /// 2 карты существ 2 уровня (если есть)
        /// </para>
        /// <para>
        /// 2 карты существ 3 уровня (если есть)
        /// </para>
        /// <para>
        /// 4 карты навыка левой кнопки мыши
        /// </para>
        /// <para>
        /// 3 карты навыка правой кнопки мыши
        /// </para>
        /// <para>
        /// 2 карты навыка Q
        /// </para>
        /// <para>
        /// 1 карта навыка E
        /// </para>
        /// <para>
        /// 3 общих карт урона
        /// </para>
        /// <para>
        /// 3 общих карт существ 1, 2 и 3 уровня
        /// </para>
        /// <para>
        /// 3 общих карт приёмов
        /// </para>
        /// </summary>
        public IEnumerable<Card> Cards { get; set; }

        protected override void Init(CardGameDeckData dataClass)
        {
            this.Name = dataClass.Name;
            var deckCards = Card.Load(x => dataClass.Cards.Contains(x.Number), this.Name);
            for (int i = 0; i < 5; i++)
            {
                deckCards.Add(new Card() { CardType = Interfaces.CardType.Resource });
            }
            this.Cards = deckCards;
        }
    }
}