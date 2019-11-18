using Dungeon.Classes;
using Dungeon;
using Dungeon12.Entites.Journal;
using Dungeon12.CardGame.Engine;

namespace Dungeon12
{
    public class Dungeon12Class : BaseCharacterTileset
    {
        public override string Avatar => GetType().Name.AsmImgRes();
               
        public Journal Journal { get; set; } = new Journal();

        public Deck CardDeck { get; set; } = Deck.Load("Guardian");
    }
}
