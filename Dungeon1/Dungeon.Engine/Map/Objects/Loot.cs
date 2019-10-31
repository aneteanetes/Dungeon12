namespace Dungeon.Map.Objects
{
    using Dungeon.Entites.Enemy;
    using Dungeon.Items;
    using Dungeon.Map.Infrastructure;

    [Template("l")]
    public class Loot : MapObject
    {
        public Item Item { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;
    }
}
