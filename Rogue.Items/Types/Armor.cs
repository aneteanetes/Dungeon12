namespace Rogue.Items.Types
{
    using Rogue.Items.Enums;

    public class Armor : Item
    {
        public override Stats AvailableStats => Stats.MainStats & Stats.Defence;
    }
}