namespace Dungeon12.Items.Types
{
    using Dungeon12.Items.Enums;

    public class Potion : Item
    {
        public override Stats AvailableStats => Stats.MainStats;
    }
}