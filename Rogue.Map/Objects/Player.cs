namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;

    [Template("@")]
    public class Player : MapObject
    {
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
