namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Entites.Alive;
    using Rogue.Entites.Enemy;
    using Rogue.Entites.NPC;
    using Rogue.Map.Infrastructure;
    using Rogue.Physics;
    using Rogue.Settings;
    using Rogue.Types;

    [Template("N")]
    public class NPC : MapObject
    {
        public NPCMoveable NPCEntity { get; set; }

        public string Face { get; set; }

        public override string Icon { get => "N"; set { } }

        protected override MapObject Self => this;

        public override void Interact(GameMap gameMap)
        {
        }

        public override bool Obstruction => true;
        
        public override double MovementSpeed => base.MovementSpeed;

        public bool Moving { get; set; }

        public override PhysicalSize Size
        {
            get => new PhysicalSize
            {
                Height = 24,
                Width = 24
            };
            set { }
        }

        public override PhysicalPosition Position
        {
            get => new PhysicalPosition
            {
                X = base.Position.X + 4,
                Y = base.Position.Y + 4
            };
            set { }
        }
    }
}