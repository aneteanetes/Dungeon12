namespace Dungeon.Items.Types
{
    using Dungeon.Items.Enums;

    public class Armor : Item
    {
        public override Stats AvailableStats => Stats.MainStats & Stats.Defence;

        public override ItemKind Kind => ItemKind.Armor;
    }
}