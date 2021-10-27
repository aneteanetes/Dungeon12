namespace Dungeon12.Items.Types
{
    using Dungeon.Types;
    using Dungeon12.Items.Enums;

    [Generation(3, 1, Stats.Barrier, Stats.Class, Stats.Defence)]
    public class Boots : Item
    {
        public override ItemKind Kind => ItemKind.Boots;

        public override Point InventorySize => new Point(2, 2);
    }
}