namespace Rogue.Items.Types
{
    using Rogue.Items.Enums;

    public class Weapon : Item
    {
        public override Stats AvailableStats => Stats.Damage & Stats.Attack & Stats.Defence;

        public override ItemKind Kind => ItemKind.Weapon;
    }
}