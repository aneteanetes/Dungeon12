namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;
    using Rogue.Types;

    [Template("@")]
    public class Player : MapObject
    {
        public Entites.Alive.Character.Player Character { get; set; }

        public override string Tileset => Character.Tileset;

        public override Rectangle Region => Character.Region;

        public override string Icon { get => "@"; set { } }

        public Player()
        {
            this.ForegroundColor = new MapObjectColor
            {
                R = 255,
                A = 255
            };
        }

        public override void Interact()
        {
            throw new NotImplementedException();
        }
    }
}