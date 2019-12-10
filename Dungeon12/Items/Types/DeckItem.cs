using Dungeon12.CardGame.Engine;
using Dungeon12.Items.Enums;

namespace Dungeon12.Items.Types
{
    public class DeckItem : Item
    {
        public override ItemKind Kind => ItemKind.Deck;

        public override Rarity Rare => Rarity.Deck;

        public string DeckIdentity { get; set; }

        private Deck _deck;
        public Deck Deck
        {
            get
            {
                if (_deck == default)
                {
                    _deck = Deck.Load("Guardian");
                }
                return _deck;
            }
        }

        public override void PutOn(object character)
        {
            base.PutOn(character);
            Global.GameState.Character["CardsExisted"] = true;
        }

        public override void PutOff(object character)
        {
            base.PutOff(character);
            Global.GameState.Character["CardsExisted"] = default;
        }
    }
}