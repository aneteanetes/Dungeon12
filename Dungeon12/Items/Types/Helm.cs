namespace Dungeon12.Items.Types
{
    using Dungeon.Types;
    using Dungeon12.Items.Enums;

    [Generation(4, 1, Stats.Health, Stats.Class, Stats.Resource)]
    public class Helm : Item
    {
        public override ItemKind Kind => ItemKind.Helm;

        public override Point InventorySize => new Point(2, 2);
    }
}