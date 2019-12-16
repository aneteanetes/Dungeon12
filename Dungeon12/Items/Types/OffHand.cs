namespace Dungeon12.Items.Types
{
    using Dungeon.Types;
    using Dungeon12.Items.Enums;

    [Generation(2, 1, Stats.Barrier, Stats.Class, Stats.Defence, Stats.Health, Stats.Resource, Stats.AbilityPower)]
    public class OffHand : Item
    {
        public override ItemKind Kind => ItemKind.OffHand;

        public override Point InventorySize => new Point(2, 3);
    }
}