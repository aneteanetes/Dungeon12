namespace Dungeon.Items.Types
{
    using Dungeon.Items.Enums;

    public class Resource : Item
    {
        public override Stats AvailableStats => Stats.None;
    }
}