namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;

    [Template("#")]
    public class Wall : MapObject
    {
        public override bool Obstruction { get => true; set { } }

        public override void Interact()
        {
            throw new NotImplementedException();
        }
    }
}