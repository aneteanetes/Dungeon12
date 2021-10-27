namespace Dungeon12.Items.Types
{
    using Dungeon12.Items.Enums;

    public class Key : Item
    {
        public override ItemKind Kind => ItemKind.Key;

        public override Rarity Rare => Rarity.Watered;
    }
}