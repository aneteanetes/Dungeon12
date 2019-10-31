namespace Dungeon.Items.Types
{
    using Dungeon.Items.Enums;

    public class Potion : Item
    {
        public override Stats AvailableStats => Stats.MainStats;
    }
}