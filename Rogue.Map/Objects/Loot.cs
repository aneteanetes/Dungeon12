namespace Rogue.Map.Objects
{
    using Rogue.Entites.Enemy;
    using Rogue.Items;
    using Rogue.Map.Infrastructure;

    [Template("l")]
    public class Loot : MapObject
    {
        public Item Item { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;
    }
}
