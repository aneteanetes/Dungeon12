namespace Dungeon.Map.Objects
{
    using Dungeon.Entites.Enemy;
    using Dungeon.Map.Infrastructure;

    [Template("m")]
    public class Money : MapObject
    {
        public int Amount { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;
    }
}
