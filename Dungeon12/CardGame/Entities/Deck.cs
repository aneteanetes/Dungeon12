using Dungeon;
using Dungeon.Entities;
using Dungeon.Map.Infrastructure;
using Dungeon12.CardGame.Entities;
using Dungeon12.Database.CardGameDeck;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.CardGame.Engine
{
    public class Deck : DataEntity<Deck,CardGameDeckData>
    {
        public IEnumerable<Card> Cards { get; set; }

        protected override void Init(CardGameDeckData dataClass)
        {
            this.Name = dataClass.Name;
            this.Cards = Card.Load(x => dataClass.Cards.Contains(x.Number), this.Name);
        }
    }
}