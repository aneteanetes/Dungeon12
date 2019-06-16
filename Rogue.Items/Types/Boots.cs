namespace Rogue.Items.Types
{
    using Rogue.Items.Enums;

    public class Boots : Item
    {
        public override Stats AvailableStats => Stats.Defence;
        public override ItemKind Kind => ItemKind.Boots;
    }
}