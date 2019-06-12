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
        
        public override bool Obstruction => true;

        public bool IsChasing { get; set; }

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

        public Point AttackRangeMultiples { get; set; }

        public MapObject AttackRange => new MapObject
        {
            Position = new PhysicalPosition
            {
                X = this.Position.X - ((this.Size.Width * this.AttackRangeMultiples.X) - this.Size.Width) / 2,
                Y = this.Position.Y - ((this.Size.Height * this.AttackRangeMultiples.Y) - this.Size.Height) / 2
            },
            Size = new PhysicalSize
            {
                Width = this.Size.Width * AttackRangeMultiples.X,
                Height = this.Size.Height * AttackRangeMultiples.Y
            }
        };
    }
}