namespace Dungeon12.Items.Types
{
    using Dungeon12.Items.Enums;

    public class Boots : Item
    {
        public override Stats AvailableStats => Stats.Defence;
        public override ItemKind Kind => ItemKind.Boots;
    }
}