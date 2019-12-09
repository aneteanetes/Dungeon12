namespace Dungeon12.Items.Types
{
    using Dungeon12.Items.Enums;

    public class Weapon : Item
    {
        public override Stats AvailableStats => Stats.Damage & Stats.Attack & Stats.Defence;

        public override ItemKind Kind => ItemKind.Weapon;
    }
}