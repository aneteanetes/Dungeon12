namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Entites.Enemy;
    using Rogue.Map.Infrastructure;
    using Rogue.Physics;
    using Rogue.Settings;
    using Rogue.Types;

    [Template("*")]
    public class Mob : MapObject
    {
        public Enemy Enemy { get; set; }

        public override string Icon { get => "*"; set { } }

        protected override MapObject Self => this;

        public override void Interact(GameMap gameMap)
        {
        }

        public override bool Obstruction => true;
    }
}