using Dungeon.Data;

namespace Dungeon12.Database.CardGameDeck
{
    public class CardGameDeckData : Persist
    {
        public string Name { get; set; }

        public int[] Cards { get; set; }
    }
}