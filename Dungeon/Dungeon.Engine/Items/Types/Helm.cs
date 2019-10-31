namespace Dungeon.Items.Types
{
    using Dungeon.Items.Enums;

    public class Helm : Item
    {
        public override Stats AvailableStats => Stats.MainStats & Stats.Attack;
        public override ItemKind Kind => ItemKind.Helm;
    }
}