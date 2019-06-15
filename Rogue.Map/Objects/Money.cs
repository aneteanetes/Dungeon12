namespace Rogue.Map.Objects
{
    using Rogue.Entites.Enemy;
    using Rogue.Map.Infrastructure;

    [Template("m")]
    public class Money : MapObject
    {
        public int Amount { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;
    }
}
