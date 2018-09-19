namespace Rogue.Items.Types
{
    using Rogue.Items.Enums;

    public class Helm : Item
    {
        public override Stats AvailableStats => Stats.MainStats & Stats.Attack;
    }
}