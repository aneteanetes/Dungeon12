namespace Dungeon12.Items.Types
{
    using Dungeon12.Items.Enums;

    public class Armor : Item
    {
        public override Stats AvailableStats => Stats.MainStats & Stats.Defence;

        public override ItemKind Kind => ItemKind.Armor;
    }
}