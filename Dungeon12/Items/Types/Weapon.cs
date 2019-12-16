namespace Dungeon12.Items.Types
{
    using Dungeon.Types;
    using Dungeon12.Items.Enums;

    [Generation(1, 1, Stats.AbilityPower, Stats.AttackDamage, Stats.Class, Stats.Resource)]
    public class Weapon : Item
    {
        public override ItemKind Kind => ItemKind.Weapon;

        public override Point InventorySize => new Point(1, 3);
    }
}