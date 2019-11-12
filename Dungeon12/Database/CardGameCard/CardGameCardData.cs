using Dungeon.Data;
using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;

namespace Dungeon12.Database.CardGameCard
{
    public class CardGameCardData : Persist
    {
        public int Number { get; set; }

        public CardType Type { get; set; }

        public GuardCard Card { get; set; }
    }
}