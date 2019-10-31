namespace Dungeon.Items.Types
{
    using Dungeon.Items.Enums;

    public class Boots : Item
    {
        public override Stats AvailableStats => Stats.Defence;
        public override ItemKind Kind => ItemKind.Boots;
    }
}