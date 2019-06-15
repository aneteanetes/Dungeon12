namespace Rogue.Map.Objects
{
    using Rogue.Entites.Enemy;
    using Rogue.Map.Infrastructure;

    [Template("r")]
    public class Corpse : MapObject
    {
        public Enemy Enemy { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;
    }
}
