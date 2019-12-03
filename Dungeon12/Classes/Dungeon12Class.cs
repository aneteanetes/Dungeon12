using Dungeon;
using Dungeon.Classes;
using Dungeon.Entities.Fractions;
using Dungeon12.CardGame.Engine;
using Dungeon12.Entites.Journal;
using Dungeon12.Entities.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12
{
    public class Dungeon12Class : BaseCharacterTileset
    {
        public virtual int InitialHP => 100;

        public override string Avatar => GetType().Name.AsmImgRes();
               
        public Journal Journal { get; set; } = new Journal();

        public Deck CardDeck { get; } = Deck.Load("Guardian");

        public List<IQuest> ActiveQuests { get; set; } = new List<IQuest>();
    }
}
