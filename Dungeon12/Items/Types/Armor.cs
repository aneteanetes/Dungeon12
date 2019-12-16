namespace Dungeon12.Items.Types
{
    using Dungeon.Types;
    using Dungeon12.Items.Enums;

    [Generation(4, 2, Stats.AttackDamage, Stats.Class, Stats.AbilityPower, Stats.Health)]
    
    public class Armor : Item
    {
        public override ItemKind Kind => ItemKind.Armor;

        public override Point InventorySize => new Point() { X = 2, Y = 3 };
    }
}