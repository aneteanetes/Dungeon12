using Dungeon.Data;
using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dungeon12.Database.CardGameCard
{
    public class CardGameCardData : Persist
    {
        public string Name { get; set; }

        public int Number { get; set; }

        public CardType CardType { get; set; }

        public GuardCard Card { get; set; }

        public string Image { get; set; }
    }
}