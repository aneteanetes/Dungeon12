namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;

    [Template(".")]
    public class Empty : MapObject
    {
        public override string Icon { get => "°"; set { } }

        public override void Interact()
        {
            throw new NotImplementedException();
        }
    }
}