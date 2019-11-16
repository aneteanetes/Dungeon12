using Dungeon;
using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;

namespace Dungeon12.CardGame.Triggers
{
    public class ChanceEscapeGuard : IAbilityCardTrigger
    {
        public string Description => "20% шанс что страж исчезнет";

        public void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            if (RandomDungeon.Chance(20))
            {
                if (card is GuardCard guardCard)
                {
                    player.Guards.Remove(guardCard);
                }
            }
        }
    }
}