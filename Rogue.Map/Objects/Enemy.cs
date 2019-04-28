namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;
    using Rogue.Physics;
    using Rogue.Settings;
    using Rogue.Types;

    [Template("*")]
    public class Enemy : MapObject
    {
        public override string Icon { get => "*"; set { } }

        public Enemy()
        {
            this.ForegroundColor = new MapObjectColor
            {
                R = 255,
                A = 255
            };
        }

        public override PhysicalSize Size
        {
            get => new PhysicalSize
            {
                Height = 18,
                Width = 18
            };
            set { }
        }

        public override PhysicalPosition Position
        {
            get => new PhysicalPosition
            {
                X = base.Position.X + 8,
                Y = base.Position.Y + 14
            };
            set { }
        }

        protected override MapObject Self => this;
        

        public override string Tileset => "Rogue.Classes.BloodMage.Images.Dolls.Character.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 32,
            Y = 0,
            Height = 32,
            Width = 32
        };

        public override void Interact(GameMap gameMap)
        {
        }
    }
}