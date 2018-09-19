namespace Rogue.Items.Types
{
    using Rogue.Items.Enums;

    public class OffHand : Item
    {
        public override Stats AvailableStats => Stats.Attack & Stats.Damage & Stats.Defence;
    }
}