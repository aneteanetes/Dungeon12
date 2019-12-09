namespace Dungeon12.Items.Types
{
    using Dungeon12.Items.Enums;

    public class Helm : Item
    {
        public override Stats AvailableStats => Stats.MainStats & Stats.Attack;
        public override ItemKind Kind => ItemKind.Helm;
    }
}