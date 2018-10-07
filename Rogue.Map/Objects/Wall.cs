namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;
    using Rogue.Types;

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