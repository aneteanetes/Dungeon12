namespace Rogue.Items.Types
{
    using Rogue.Items.Enums;

    public class Potion : Item
    {
        public override Stats AvailableStats => Stats.MainStats;
    }
}